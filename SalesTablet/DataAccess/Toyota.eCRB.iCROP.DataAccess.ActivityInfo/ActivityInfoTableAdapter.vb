'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ActivityInfoTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/02/27 TCS 安田 【SALES_2】
'更新： 2012/04/17 TCS 河原 【SALES_2】号口課題No.118
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/03/06 TCS 河原 GL0874
'更新： 2013/06/30 TCS 松月 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 高橋、山口 受注後フォロー機能開発
'更新： 2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15） 
'更新： 2014/02/02 TCS 松月 【A STEP2】希望車表示不具合対応（号口切替BTS-39）
'更新： 2014/02/26 TCS 松月 【A STEP2】担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）
'更新： 2014/03/07 TCS 各務 再構築不具合対応マージ版
'更新： 2014/03/18 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/21 TCS 松月 【A STEP2】業務種別設定不正対応（問連TR-V4-GTMC140416001）
'更新： 2014/05/31 TCS 外崎 TMT不具合対応
'更新： 2014/07/09 TCS 高橋 受注後活動完了条件変更対応
'更新： 2014/08/20 TCS 森   受注後活動A⇒H移行対応
'更新： 2014/09/01 TCS 松月 【A STEP2】ToDo連携店舗コード変更対応(初期活動店舗)（問連TR-V4-GTMC140807001）
'更新： 2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）
'更新： 2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001)
'更新： 2015/07/21 TCS 藤井 TR-V4-FTMS150309002(FTMS→TMTマージ)
'更新： 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応
'更新： 2016/03/25 TCS 鈴木 性能改善（ActivityInfo_302）
'更新： 2016/10/19 TCS 河原 TR-SVT-TMT-20160727-002
'更新： 2017/05/11 TCS 河原 TR-SLT-TMT-20161020-001
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件
'更新： 2019/04/05 TS  舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス）【UAT-0614】
'更新： 2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001)
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061)
'更新： 2020/04/08 TS  髙橋(龍) 見積承認された希望車を特定するSQLを修正(TR-V4-TKM-20191227-001)
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 松月 2013/10対応版 既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 松月 2013/10対応版 既存流用 END
' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization

' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

Public NotInheritable Class ActivityInfoTableAdapter

#Region "定数"

    ''' <summary>
    ''' Follow-upBoxのCR活動スタータス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONSTFLLWUPHOT As String = "1"
    Public Const CONSTFLLWUPPROSPECT As String = "2"
    Public Const CONSTFLLWUPREPUCHASE As String = "3"
    Public Const CONSTFLLWUPPERIODICAL As String = "4"
    Public Const CONSTFLLWUPPROMOTION As String = "5"
    Public Const CONSTFLLWUPREQUEST As String = "6"
    Public Const CONSTFLLWUPWALKIN As String = "7"

    ''' <summary>
    ''' Follow-upBoxの活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONSTCRACTRSLTHOT As String = "1"
    Public Const CONSTCRACTRSLTPROSPECT As String = "2"
    Public Const CONSTCRACTRSLTSUCCESS As String = "3"
    Public Const CONSTCRACTRSLTCONTINUE As String = "4"
    Public Const CONSTCRACTRSLTGIVEUP As String = "5"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONSTSELECTTOHOT = "3"
    Public Const CONSTSELECTTOPROC = "2"
    Public Const CONSTSELECTTOWALK = "1"

    'Public Const FLLWUPTYP = ""

    Public Const CONSTCUSTOMERCLASSOWNER = "1"

    Public Const CONSTCUSTSEGMENTCUSTOMER = "1"
    Public Const CONSTCUSTSEGMENTNEWCSTOMER = "2"

    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' モデルコード　AHV41L-JEXGBC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODEL_CD_HV As String = "AHV41L-JEXGBC%"
    ''' <summary>
    ''' シリーズコード　CAMRY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CAMRY As String = "CAMRY"
    ''' <summary>
    ''' シリーズコード　CMYHV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CMYHV As String = "CMYHV"
    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    ' 2019/05/24 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス）【UAT-0614】DEL

    ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
    ' 外版色コードを前3桁だけで比較するか否かフラグ(システム環境設定)
    Private Const EXTERIOR_COLOR_3_FLG As String = "EXTERIOR_COLOR_3_FLG"
    ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）START

    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORGCUSTFLG As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCUSTFLG As String = "2"

    ''' <summary>
    ''' コンタクト履歴タブ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTACTHISTORY_TAB_ALL As String = "0"
    Public Const CONTACTHISTORY_TAB_SALES As String = "1"
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    Public Const CONTACTHISTORY_TAB_SERVICE As String = "2"
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
    Public Const CONTACTHISTORY_TAB_CR As String = "3"

    ''' <summary>
    ''' 固定STRCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRCD000 As String = "000"

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）END

    ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    ''' <summary>
    ''' サフィックス使用可否フラグ (1：使用する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_SUFFIX_TURE As String = "1"
    ''' <summary>
    ''' 内装色使用可否フラグ (1：使用する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_INTERIOR_CLR_TURE As String = "1"
    ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' サフィックス使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_SUFFIX As String = "USE_FLG_SUFFIX"
    ''' <summary>
    ''' 内装色使用可否フラグ名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_FLG_INTERIORCLR As String = "USE_FLG_INTERIORCLR"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

#End Region

#Region "メソット"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 029.担当スタッフ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsers(ByVal dlrcd As String, ByVal strcd As String) As ActivityInfoDataSet.ActivityInfoUsersDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoUsersDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    A.USERNAME, ")
                .Append("    REPLACE(A.ACCOUNT ,'@' || :DLRCD ,'') AS ACCOUNT ")
                .Append("FROM ")
                .Append("    TBL_USERS A, ")
                .Append("    TBL_USERDISPLAY B ")
                .Append("WHERE ")
                .Append("    A.DLRCD = :DLRCD AND ")
                .Append("    A.STRCD = :STRCD AND ")
                .Append("    A.OPERATIONCODE = '8' AND ")
                .Append("    A.DELFLG = '0' AND ")
                .Append("    B.ACCOUNT(+) = A.ACCOUNT ")
                .Append("ORDER BY ")
                .Append("    B.SORTNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 054.Follow-up Box選択車種取得(シリーズ)
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFllwSeries(ByVal dlrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwSeriesDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoFllwSeriesDataTable)("ActivityInfo_102")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwSeries_Start")
            'ログ出力 End *****************************************************************************
            '2015/07/21 TCS 藤井 TR-V4-FTMS150309002(FTMS→TMTマージ) START
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_102 */ ")
                .Append("  T7.SEQNO , ")
                .Append("  T7.MODEL_CD AS SERIESCD, ")
                .Append("  T6.MODEL_NAME AS SERIESNM ")
                .Append("FROM ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    MIN(PREF_VCL_SEQ) AS SEQNO , ")
                .Append("    MODEL_CD ")
                .Append("  FROM ")
                .Append("    TB_T_PREFER_VCL ")
                .Append("  WHERE ")
                .Append("    SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("  GROUP BY ")
                .Append("    MODEL_CD ")
                .Append("  ORDER BY ")
                .Append("    PREF_VCL_SEQ ")
                .Append("  ) T7 , ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    T1.DLR_CD , ")
                .Append("    T2.MODEL_CD , ")
                .Append("    T2.MODEL_NAME ")
                .Append("  FROM ")
                .Append("    TB_M_DEALER T1 , ")
                .Append("    TB_M_MODEL T2 , ")
                .Append("    TB_M_MODEL_DLR T3 ")
                .Append("  WHERE ")
                .Append("        T1.DLR_CD = T3.DLR_CD ")
                .Append("    AND T2.MODEL_CD = T3.MODEL_CD ")
                .Append("    AND T1.DLR_CD = :DLRCD ")
                .Append("    AND T2.INUSE_FLG = '1' ")
                .Append("    OR (T3.DLR_CD = 'XXXXX' ")
                .Append("    AND T2.MODEL_CD = T3.MODEL_CD ")
                .Append("    AND T1.DLR_CD = :DLRCD ")
                .Append("    AND T1.INUSE_FLG = '1' ")
                .Append("    AND NOT EXISTS ")
                .Append("      ( ")
                .Append("      SELECT ")
                .Append("         1 ")
                .Append("      FROM ")
                .Append("        TB_M_MODEL T4 , ")
                .Append("        TB_M_MODEL_DLR T5 ")
                .Append("      WHERE ")
                .Append("            T5.DLR_CD = T1.DLR_CD ")
                .Append("        AND T5.MODEL_CD = T4.MODEL_CD ")
                .Append("        AND T5.MODEL_CD = T3.MODEL_CD ")
                .Append("      )) ")
                .Append("  ) T6 ")
                .Append("WHERE ")
                .Append("      T6.DLR_CD = :DLRCD ")
                .Append("  AND T7.MODEL_CD = T6.MODEL_CD ")
                .Append("ORDER BY ")
                .Append("  SEQNO ")
            End With
            '2015/07/21 TCS 藤井 TR-V4-FTMS150309002(FTMS→TMTマージ) END
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwSeries_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 055.Follow-up Box選択車種取得(モデル)　(移行済み)
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFllwModel(ByVal dlrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwModelDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwModel_Start")
        'ログ出力 End *****************************************************************************
        '2015/07/21 TCS藤井 TR-V4-FTMS150309002(FTMS→TMTマージ) START
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_103*/ ")
            .Append("  T8.PREF_VCL_SEQ AS SEQNO , ")
            .Append("  T8.MODEL_CD AS SERIESCD , ")
            .Append("  T8.MODEL_NAME AS SERIESNM , ")
            .Append("  T8.GRADE_CD AS MODELCD , ")
            .Append("  T8.GRADE_NAME AS VCLMODEL_NAME ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    T1.PREF_VCL_SEQ , ")
            .Append("    T1.MODEL_CD , ")
            .Append("    T1.GRADE_CD , ")
            .Append("    T7.COMMON_MODEL_CD , ")
            .Append("    T7.MODEL_NAME , ")
            .Append("    T9.GRADE_NAME  ")
            .Append("  FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      MIN(PREF_VCL_SEQ) AS PREF_VCL_SEQ , ")
            .Append("      MODEL_CD , ")
            .Append("      GRADE_CD ")
            .Append("    FROM ")
            .Append("      TB_T_PREFER_VCL ")
            .Append("    WHERE ")
            .Append("      SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    GROUP BY ")
            .Append("      MODEL_CD , ")
            .Append("      GRADE_CD ")
            .Append("    ) T1 , ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      T2.DLR_CD , ")
            .Append("      T3.MODEL_CD , ")
            .Append("      T3.MODEL_NAME , ")
            .Append("      T5.MAKER_TYPE , ")
            .Append("      T3.MODEL_PICTURE , ")
            .Append("      T3.COMMON_MODEL_CD , ")
            .Append("      T3.INUSE_FLG , ")
            .Append("      T3.MAKER_CD ")
            .Append("    FROM ")
            .Append("      TB_M_DEALER T2 , ")
            .Append("      TB_M_MODEL T3 , ")
            .Append("      TB_M_MODEL_DLR T4 , ")
            .Append("      TB_M_MAKER T5 ")
            .Append("    WHERE ")
            .Append("          T2.DLR_CD = T4.DLR_CD ")
            .Append("      AND T3.MODEL_CD = T4.MODEL_CD ")
            .Append("      AND T3.MAKER_CD = T5.MAKER_CD ")
            .Append("      AND T2.DLR_CD = :DLRCD ")
            .Append("      AND T2.INUSE_FLG = '1' ")
            .Append("      OR (T3.MODEL_CD = T4.MODEL_CD ")
            .Append("      AND T3.MAKER_CD = T5.MAKER_CD ")
            .Append("      AND T2.DLR_CD = :DLRCD ")
            .Append("      AND T4.DLR_CD = 'XXXXX' ")
            .Append("      AND T2.DLR_CD = :DLRCD ")
            .Append("      AND T2.INUSE_FLG = '1' ")
            .Append("      AND NOT EXISTS ")
            .Append("        ( ")
            .Append("        SELECT ")
            .Append("           1 ")
            .Append("        FROM ")
            .Append("          TB_M_MODEL_DLR T6 ")
            .Append("        WHERE ")
            .Append("              T6.DLR_CD = T2.DLR_CD ")
            .Append("          AND T6.MODEL_CD = T4.MODEL_CD ")
            .Append("        )) ")
            .Append("    ) T7  , ")
            .Append("    TB_M_GRADE T9 ")
            .Append("WHERE ")
            .Append("      T7.DLR_CD = :DLRCD ")
            .Append("  AND T1.MODEL_CD = T7.MODEL_CD ")
            .Append("  AND T1.GRADE_CD = T9.GRADE_CD(+) ")
            .Append("  AND T1.MODEL_CD = T9.MODEL_CD(+) ")
            .Append("  ) T8 ")
            .Append("ORDER BY ")
            .Append("  T8.PREF_VCL_SEQ ")
        End With
        '2015/07/21 TCS 藤井 TR-V4-FTMS150309002(FTMS→TMTマージ) END
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoFllwModelDataTable)("ActivityInfo_103")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwModel_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 056.Follow-up Box選択車種取得(カラー)
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFllwColor(ByVal dlrcd As String, ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoFllwColorDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwColor_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_104 */ ")
            .Append("  T8.PREF_VCL_SEQ , ")
            .Append("  T8.SERIESCD , ")
            .Append("  T8.SERIESNM , ")
            .Append("  T8.MODELCD , ")
            .Append("  T8.VCLMODEL_NAME , ")
            .Append("  T8.COLORCD , ")
            .Append("  T8.DISP_BDY_COLOR AS DISP_BDY_COLOR ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    T7.PREF_VCL_SEQ , ")
            .Append("    T7.MODEL_CD AS SERIESCD , ")
            .Append("    T7.MODEL_NAME AS SERIESNM , ")
            .Append("    T7.GRADE_CD AS MODELCD , ")
            .Append("    T7.GRADE_NAME AS VCLMODEL_NAME , ")
            .Append("    T7.BODYCLR_NAME AS DISP_BDY_COLOR , ")
            .Append("    T7.BODYCLR_CD AS COLORCD ")
            .Append("  FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      T1.PREF_VCL_SEQ , ")
            .Append("      T1.MODEL_CD , ")
            .Append("      T1.GRADE_CD , ")
            .Append("      T6.COMMON_MODEL_CD , ")
            .Append("      T6.MODEL_NAME , ")
            .Append("      T1.BODYCLR_CD ,")
            .Append("      T1.BODYCLR_NAME ,")
            .Append("      T1.GRADE_NAME ")
            .Append("    FROM ")
            .Append("      ( ")
            .Append("      SELECT ")
            .Append("        T9.PREF_VCL_SEQ , ")
            .Append("        T9.MODEL_CD , ")
            .Append("        T9.GRADE_CD , ")
            .Append("        T9.BODYCLR_CD ,")
            .Append("        T10.BODYCLR_NAME ,")
            .Append("        T11.GRADE_NAME ")
            .Append("      FROM ")
            .Append("        TB_T_PREFER_VCL T9 , ")
            .Append("        TB_M_BODYCOLOR T10 , ")
            .Append("        TB_M_GRADE T11   ")
            .Append("      WHERE ")
            .Append("        T9.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("      AND T9.MODEL_CD = T10.MODEL_CD ")
            .Append("      AND T9.GRADE_CD = T10.GRADE_CD ")
            .Append("      AND T9.SUFFIX_CD = T10.SUFFIX_CD ")
            .Append("      AND T9.BODYCLR_CD = T10.BODYCLR_CD ")
            .Append("      AND T9.GRADE_CD = T11.GRADE_CD ")
            .Append("      AND T9.MODEL_CD = T11.MODEL_CD ")
            .Append("      ORDER BY ")
            .Append("        PREF_VCL_SEQ ")
            .Append("      ) T1 , ")
            .Append("      ( ")
            .Append("      SELECT ")
            .Append("        T2.DLR_CD , ")
            .Append("        T3.MODEL_CD , ")
            .Append("        T3.MODEL_NAME , ")
            .Append("        T5.MAKER_TYPE , ")
            .Append("        T3.MODEL_PICTURE , ")
            .Append("        T3.COMMON_MODEL_CD , ")
            .Append("        T3.INUSE_FLG , ")
            .Append("        T3.MAKER_CD ")
            .Append("      FROM ")
            .Append("        TB_M_DEALER T2 , ")
            .Append("        TB_M_MODEL T3 , ")
            .Append("        TB_M_MODEL_DLR T4 , ")
            .Append("        TB_M_MAKER T5 ")
            .Append("      WHERE ")
            .Append("            T2.DLR_CD = T4.DLR_CD ")
            .Append("        AND T3.MODEL_CD = T4.MODEL_CD ")
            .Append("        AND T3.MAKER_CD = T5.MAKER_CD ")
            .Append("        AND T2.DLR_CD = :DLRCD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        OR (T3.MODEL_CD = T4.MODEL_CD ")
            .Append("        AND T3.MAKER_CD = T5.MAKER_CD ")
            .Append("        AND T2.DLR_CD = :DLRCD ")
            .Append("        AND T4.DLR_CD = 'XXXXX' ")
            .Append("        AND T2.DLR_CD = :DLRCD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        AND NOT EXISTS ")
            .Append("          ( ")
            .Append("          SELECT ")
            .Append("            1 ")
            .Append("          FROM ")
            .Append("            TB_M_MODEL_DLR T5 ")
            .Append("          WHERE ")
            .Append("            T5.DLR_CD = T2.DLR_CD ")
            .Append("          )) ")
            .Append("      ) T6 ")
            .Append("  WHERE ")
            .Append("        T6.DLR_CD = :DLRCD ")
            .Append("    AND T1.MODEL_CD = T6.MODEL_CD ")
            .Append("    ) T7 ")
            .Append("  ) T8 ")
            .Append("ORDER BY ")
            .Append("    T8.PREF_VCL_SEQ ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoFllwColorDataTable)("ActivityInfo_104")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwColor_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 061.文言取得
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContentWord(ByVal seqno As Long) As ActivityInfoDataSet.ActivityInfoContentWordDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContentWordDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    DECODE(TRIM(ACTION_LOCAL), '', ACTION, ACTION_LOCAL) AS ACTION ")
                .Append("FROM ")
                .Append("    TBL_FLLWUPBOXCONTENT ")
                .Append("WHERE ")
                .Append("    SEQNO = :SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 062.日付フォーマット取得 (移行済み)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDateFormat() As ActivityInfoDataSet.ActivityInfoDateFormatDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoDateFormatDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    FORMAT ")
                .Append("FROM ")
                .Append("    TBL_DATETIMEFORM ")
                .Append("WHERE ")
                .Append("    CONVID = 11 ")
            End With
            query.CommandText = sql.ToString()
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 069.アイコンのパス取得
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContentIconPath(ByVal seqno As Integer) As ActivityInfoDataSet.ActivityInfoContentIconPathDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContentIconPathDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    ICONPATH_RESULT_NOTSELECTED, ")
                .Append("    ICONPATH_RESULT_SELECTED ")
                .Append("FROM ")
                .Append("    TBL_FLLWUPBOXCONTENT ")
                .Append("WHERE ")
                .Append("    SEQNO = :SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Char, seqno)
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 073.活動実績登録用の希望車種情報を取得　
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActHisCarSeq(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoSeqDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSeqDataTable)("ActivityInfo_108")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_108 */ ")
                .Append("  T1.PREF_VCL_SEQ AS SEQNO ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .Append("   , T1.ROW_LOCK_VERSION AS LOCKVERSION ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                .Append("FROM ")
                .Append("  TB_T_PREFER_VCL T1 ")
                .Append("WHERE ")
                .Append("  T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("ORDER BY ")
                .Append("  T1.PREF_VCL_SEQ ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function



    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 074.活動実績登録用の希望車種情報を取得 
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <param name="seqno"></param>
    ''' <param name="div"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActHisSelCarSeq(ByVal fllwupboxseqno As Decimal, ByVal seqno As String,
                                              ByVal div As String) As ActivityInfoDataSet.ActivityInfoSeqDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSeqDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisSelCarSeq_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append(" SELECT ")
                .Append("   /* SC3080203_174 */ ")
                .Append("   T1.PREF_VCL_SEQ AS SEQNO ,")
                .Append("   T1.ROW_LOCK_VERSION AS LOCKVERSION ")
                .Append(" FROM ")
                .Append("   TB_T_PREFER_VCL T1 , ")
                .Append("   ( ")
                .Append("   SELECT ")
                .Append("     T2.MODEL_CD AS SERIESCD , ")
                .Append("     T2.GRADE_CD AS MODELCD , ")
                .Append("     T2.BODYCLR_CD AS COLORCD ")
                .Append("   FROM ")
                .Append("     TB_T_PREFER_VCL T2 ")
                .Append("   WHERE ")
                .Append("         T2.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("     AND T2.PREF_VCL_SEQ = :SEQNO ")
                .Append("   ) T3 ")
                .Append(" WHERE ")
                .Append("       T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("   AND T1.MODEL_CD = T3.SERIESCD ")
                .Append("   AND T1.GRADE_CD = T3.MODELCD ")
                .Append("   AND T1.BODYCLR_CD = T3.COLORCD ")
                .Append(" ORDER BY ")
                .Append("   SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisSelCarSeq_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
    End Function
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 052.シリーズ取得　
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <param name="vin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetVclinfo(ByVal originalid As String, ByVal vin As String) As ActivityInfoDataSet.ActivityInfoSeriesDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSeriesDataTable)("ActivityInfo_110")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclinfo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_110 */ ")
                .Append("  T2.MODEL_CD AS SERIESCD , ")
                .Append("  T3.MODEL_NAME AS SERIESNM ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL T1 , ")
                .Append("  TB_M_VEHICLE T2 , ")
                .Append("  TB_M_MODEL T3 ")
                .Append("WHERE ")
                .Append("      T1.VCL_ID = T2.VCL_ID ")
                .Append("  AND T2.GRADE_CD = T3.MODEL_CD ")
                .Append("  AND T1.CST_ID = :ORIGINALID ")
                .Append("  AND T1.CST_VCL_TYPE = '1' ")
                .Append("  AND T2.VCL_VIN = :VIN ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclinfo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 067.未取引客存在確認
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustID(ByVal originalid As String) As ActivityInfoDataSet.ActivityInfoNewCustIDDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoNewCustIDDataTable)("ActivityInfo_112")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustID_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_112 */ ")
                .Append("  TO_CHAR(CST_ID) ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :ORIGINALID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustID_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function



    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 036.未取引客個人情報追加SeqNo取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSeqNewcustomerCstId() As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSequenceDataTable)("ActivityInfo_114")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSeqNewcustomerCstId_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_114 */ ")
                .Append("  SQ_CUSTOMER.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSeqNewcustomerCstId_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 005.関連情報取得
    ''' </summary>
    ''' <param name="vin">VIN</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRelatedInfo(ByVal vin As String) As ActivityInfoDataSet.ActivityInfoSequenceDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSequenceDataTable)("ActivityInfo_124")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRelatedInfo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_124 */ ")
                .Append("  SUM(CNT) AS SEQ ")
                .Append("FROM ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    COUNT(1) AS CNT ")
                .Append("  FROM ")
                .Append("      TB_M_INSURANCE ")
                .Append("  WHERE ")
                .Append("        VCL_VIN = :VIN ")
                .Append("    AND ROWNUM = 1 ")
                .Append("  UNION ALL ")
                .Append("    SELECT ")
                .Append("      COUNT(1) AS CNT ")
                .Append("    FROM ")
                .Append("      TB_T_LOAN ")
                .Append("    WHERE ")
                .Append("      VCL_ID = ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("          VCL_ID ")
                .Append("        FROM ")
                .Append("          TB_M_VEHICLE ")
                .Append("        WHERE ")
                .Append("          VCL_VIN = :VIN ")
                .Append("        ) ")
                .Append("          AND ROWNUM = 1 ")
                .Append("  ) ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRelatedInfo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 071.入庫履歴よりサービススタッフ情報を取得　
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <param name="vin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetServiceStaff(ByVal originalid As String, ByVal vin As String) As ActivityInfoDataSet.ActivityInfoServiceStaffDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoServiceStaffDataTable)("ActivityInfo_125")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceStaff_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_125 */ ")
                .Append("  T1.PIC_STF_CD AS SERVICESTAFFCD , ")
                .Append("  T2.USERNAME AS SERVICESTAFFNM ")
                .Append("FROM ")
                .Append("  TB_T_VEHICLE_SVCIN_HIS T1 , ")
                .Append("  TBL_USERS T2 , ")
                .Append("  TB_M_VEHICLE T3 ")
                .Append("WHERE ")
                .Append("      T1.PIC_STF_CD = RTRIM(T2.ACCOUNT) ")
                .Append("  AND T1.VCL_ID = T3.VCL_ID ")
                .Append("  AND T1.CST_ID = :ORIGINALID ")
                .Append("  AND T3.VCL_VIN = :VIN ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceStaff_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 041.来店区分取得
    ''' </summary>
    ''' <param name="wicid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetWinclass(ByVal wicid As String) As ActivityInfoDataSet.ActivityInfoWinclassDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoWinclassDataTable)("ActivityInfo_128")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWinclass_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_128 */ ")
                .Append("  SOURCE_1_NAME AS WICNAME , ")
                .Append("  ' ' AS ACTIONCD ")
                .Append("FROM ")
                .Append("  TB_M_SOURCE_1 ")
                .Append("WHERE ")
                .Append("  SOURCE_1_CD = :WICID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("WICID", OracleDbType.Long, wicid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWinclass_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 072.自社客連番に紐付くVINを取得
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetVin(ByVal originalid As String) As ActivityInfoDataSet.ActivityInfoVinDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoVinDataTable)("ActivityInfo_131")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVin_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_131 */ ")
                .Append("  T1.VCL_VIN ")
                .Append("FROM ")
                .Append("  TB_M_VEHICLE T1 , ")
                .Append("  TB_M_CUSTOMER_VCL T2 ")
                .Append("WHERE ")
                .Append("      T1.VCL_ID = T2.VCL_ID ")
                .Append("  AND T2.CST_ID = :ORIGINALID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVin_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END



    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 020.Follow-up Box商談メモ追加
    ''' </summary>
    ''' <param name="folloupseqno"></param>
    ''' <param name="crcstid"></param>
    ''' <param name="vclid"></param>
    ''' <param name="actid"></param>
    ''' <param name="acount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertFllwupboxSalesmemo(ByVal folloupseqno As Decimal,
                                                        ByVal crcstid As Decimal, ByVal vclid As Decimal,
                                                        ByVal actid As Decimal, ByVal acount As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSalesmemo_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("INSERT /* ActivityInfo_139 */ ")
            .Append("     INTO TB_T_ACTIVITY_MEMO ( ")
            .Append("     ACT_MEMO_ID , ")
            .Append("     DLR_CD , ")
            .Append("     CST_ID , ")
            .Append("     VCL_ID , ")
            .Append("     RELATION_ACT_TYPE , ")
            .Append("     RELATION_ACT_ID , ")
            .Append("     CST_MEMO_SUBJECT , ")
            .Append("     CST_MEMO , ")
            .Append("     CREATE_STF_CD , ")
            .Append("     CREATE_DATETIME , ")
            .Append("     ROW_CREATE_DATETIME , ")
            .Append("     ROW_CREATE_ACCOUNT , ")
            .Append("     ROW_CREATE_FUNCTION , ")
            .Append("     ROW_UPDATE_DATETIME , ")
            .Append("     ROW_UPDATE_ACCOUNT , ")
            .Append("     ROW_UPDATE_FUNCTION , ")
            .Append("     ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("     SELECT ")
            .Append("     SQ_ACTIVITY_MEMO.NEXTVAL, ")
            .Append("     DLRCD, ")
            .Append("     :CRCUSTID, ")
            .Append("     :VCLID, ")
            .Append("     '2', ")
            .Append("     :ACT_ID, ")
            .Append("     ' ', ")
            .Append("     MEMO, ")
            .Append("     :INPUTACCOUNT, ")
            .Append("     SYSDATE, ")
            .Append("     SYSDATE, ")
            .Append("     :INPUTACCOUNT, ")
            .Append("     'SC3080203', ")
            .Append("     SYSDATE, ")
            .Append("     :INPUTACCOUNT, ")
            .Append("     'SC3080203', ")
            .Append(" 0 ")
            .Append(" FROM ")
            .Append("     TBL_FLLWUPBOX_SALESMEMO_WK ")
            .Append(" WHERE ")
            .Append("     FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("     AND TRIM(MEMO) IS NOT NULL ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_139")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, folloupseqno)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("ACT_ID", OracleDbType.Decimal, actid)
            query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.NVarchar2, acount)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSalesmemo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 038.Follow-up Box商談メモWK削除
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteFllwupboxSalesmemowk(ByVal fllwupboxseqno As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_000")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteFllwupboxSalesmemowk_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("DELETE /* ActivityInfo_000 */ ")
                .Append("FROM ")
                .Append("    TBL_FLLWUPBOX_SALESMEMO_WK ")
                .Append("WHERE ")
                .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteFllwupboxSalesmemowk_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 075.活動実績登録用のフォローアップボックス情報を取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActHisFllw(ByVal fllwupboxseqno As Decimal, ByVal dlrcd As String
                                         ) As ActivityInfoDataSet.ActivityInfoActHisFllwDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoActHisFllwDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisFllw_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT  ")
                .Append("  /* ActivityInfo_142 */  ")
                .Append("   0 AS CRPLAN_ID , ")
                .Append("   ' ' AS BFAFDVS , ")
                .Append("   0 AS CRDVSID , ")
                .Append("   T2.CST_ID AS INSDID , ")
                .Append("   TO_CHAR(T3.MODEL_CD) AS SERIESCODE , ")
                .Append("   CASE WHEN T3.MODEL_CD = ' ' THEN TO_CHAR(T3.NEWCST_MODEL_NAME) ")
                .Append("        ELSE TO_CHAR(T4.MODEL_NAME) ")
                .Append("   END  AS SERIESNAME , ")
                .Append("   TO_CHAR(T5.REG_NUM) AS VCLREGNO , ")
                .Append("   0 AS SUBCTGCODE , ")
                .Append("   ' ' AS SERVICECD , ")
                .Append("   ' ' AS SUBCTGORGNAME , ")
                .Append("   ' ' AS SUBCTGORGNAME_EX , ")
                .Append("   0 AS PROMOTION_ID , ")
                .Append("   CASE WHEN T2.REQ_STATUS = '31' THEN '3' ")
                .Append("        WHEN T2.REQ_STATUS = '32' THEN '5' ")
                .Append("        ELSE CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN ")
                .Append("                 '1' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '20' THEN ")
                .Append("                 '2' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '10' THEN ")
                .Append("                 '4' ")
                .Append("             ELSE ")
                .Append("                '4' ")
                .Append("            END ")
                .Append("        END AS CRACTRESULT , ")
                .Append("   ' ' AS PLANDVS , ")
                .Append("   TO_CHAR(T3.VCL_VIN) AS VIN , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）START
                .Append("   NVL(T8.USERNAME,' ') AS CUSTCHRGSTAFFNM , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）END
                .Append("   T6.SCHE_STF_CD AS ACCOUNT_PLAN , ")
                .Append("   T2.CST_ID AS CRCUSTID , ")
                .Append("   T7.CST_VCL_TYPE AS CUSTOMERCLASS ")
                .Append(" FROM ")
                .Append("   TB_T_SALES T1 , ")
                .Append("   TB_T_REQUEST T2 , ")
                .Append("   TB_M_VEHICLE T3 , ")
                .Append("   TB_M_MODEL T4 , ")
                .Append("   TB_M_VEHICLE_DLR T5 , ")
                .Append("   TB_T_ACTIVITY T6 , ")
                .Append("   TB_M_CUSTOMER_VCL T7 , ")
                .Append("   TBL_USERS T8 ")
                .Append(" WHERE ")
                .Append("       T1.REQ_ID = T2.REQ_ID ")
                .Append("   AND T2.VCL_ID = T3.VCL_ID ")
                .Append("   AND T3.MODEL_CD = T4.MODEL_CD(+) ")
                .Append("   AND T2.VCL_ID = T5.VCL_ID ")
                .Append("   AND T2.LAST_ACT_ID = T6.ACT_ID ")
                .Append("   AND T2.CST_ID = T7.CST_ID ")
                .Append("   AND T2.VCL_ID = T7.VCL_ID ")
                .Append("   AND T5.DLR_CD = T7.DLR_CD ")
                .Append("   AND T7.SLS_PIC_STF_CD = T8.ACCOUNT(+) ")
                .Append("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("   AND T5.DLR_CD = :DLRCD ")
                .Append(" UNION ALL ")
                .Append(" SELECT ")
                .Append("   0 AS CRPLAN_ID , ")
                .Append("   ' ' AS BFAFDVS , ")
                .Append("   0 AS CRDVSID , ")
                .Append("   T2.CST_ID AS INSDID , ")
                .Append("   TO_CHAR(T3.MODEL_CD) AS SERIESCODE , ")
                .Append("   CASE WHEN T3.MODEL_CD = ' ' THEN TO_CHAR(T3.NEWCST_MODEL_NAME) ")
                .Append("        ELSE TO_CHAR(T4.MODEL_NAME) ")
                .Append("   END  AS SERIESNAME , ")
                .Append("   TO_CHAR(T5.REG_NUM) AS VCLREGNO , ")
                .Append("   T2.ATTPLAN_ID AS SUBCTGCODE , ")
                .Append("    TO_CHAR(T2.SVC_CD) AS SERVICECD , ")
                .Append("   ' ' AS SUBCTGORGNAME , ")
                .Append("   ' ' AS SUBCTGORGNAME_EX , ")
                .Append("   T2.ATTPLAN_ID AS PROMOTION_ID , ")
                .Append("   CASE WHEN T2.CONTINUE_ACT_STATUS = '31' THEN '3' ")
                .Append("        WHEN T2.CONTINUE_ACT_STATUS = '32' THEN '5' ")
                .Append("        ELSE CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN ")
                .Append("                 '1' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '20' THEN ")
                .Append("                 '2' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '10' THEN ")
                .Append("                 '4' ")
                .Append("             ELSE ")
                .Append("                '4' ")
                .Append("            END ")
                .Append("        END AS CRACTRESULT , ")
                .Append("   ' ' AS PLANDVS , ")
                .Append("   TO_CHAR(T3.VCL_VIN) AS VIN , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）START
                .Append("   NVL(T8.USERNAME,' ') AS CUSTCHRGSTAFFNM , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）END
                .Append("   T6.SCHE_STF_CD AS ACCOUNT_PLAN , ")
                .Append("   T2.CST_ID AS CRCUSTID , ")
                .Append("   T7.CST_VCL_TYPE AS CUSTOMERCLASS ")
                .Append(" FROM ")
                .Append("   TB_T_SALES T1 , ")
                .Append("   TB_T_ATTRACT T2 , ")
                .Append("   TB_M_VEHICLE T3 , ")
                .Append("   TB_M_MODEL T4 , ")
                .Append("   TB_M_VEHICLE_DLR T5 , ")
                .Append("   TB_T_ACTIVITY T6 , ")
                .Append("   TB_M_CUSTOMER_VCL T7 , ")
                .Append("   TBL_USERS T8 ")
                .Append(" WHERE ")
                .Append("       T1.ATT_ID = T2.ATT_ID ")
                .Append("   AND T2.VCL_ID = T3.VCL_ID ")
                .Append("   AND T3.MODEL_CD = T4.MODEL_CD(+) ")
                .Append("   AND T2.VCL_ID = T5.VCL_ID ")
                .Append("   AND T2.LAST_ACT_ID = T6.ACT_ID ")
                .Append("   AND T2.CST_ID = T7.CST_ID ")
                .Append("   AND T2.VCL_ID = T7.VCL_ID ")
                .Append("   AND T5.DLR_CD = T7.DLR_CD ")
                .Append("   AND T7.SLS_PIC_STF_CD = T8.ACCOUNT(+) ")
                .Append("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("   AND T5.DLR_CD = :DLRCD ")
                .Append(" UNION ALL ")
                .Append(" SELECT ")
                .Append("   0 AS CRPLAN_ID , ")
                .Append("   ' ' AS BFAFDVS , ")
                .Append("   0 AS CRDVSID , ")
                .Append("   T2.CST_ID AS INSDID , ")
                .Append("   ' ' AS SERIESCODE , ")
                .Append("   ' ' AS SERIESNAME , ")
                .Append("   ' ' AS VCLREGNO , ")
                .Append("   0 AS SUBCTGCODE , ")
                .Append("   ' ' AS SERVICECD , ")
                .Append("   ' ' AS SUBCTGORGNAME , ")
                .Append("   ' ' AS SUBCTGORGNAME_EX , ")
                .Append("   0 AS PROMOTION_ID , ")
                .Append("   CASE WHEN T2.REQ_STATUS = '31' THEN '3' ")
                .Append("        WHEN T2.REQ_STATUS = '32' THEN '5' ")
                .Append("        ELSE CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN ")
                .Append("                 '1' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '20' THEN ")
                .Append("                 '2' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '10' THEN ")
                .Append("                 '4' ")
                .Append("             ELSE ")
                .Append("                '4' ")
                .Append("            END ")
                .Append("        END AS CRACTRESULT , ")
                .Append("   ' ' AS PLANDVS , ")
                .Append("   ' ' AS VIN , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）START
                .Append("   NVL(T8.USERNAME,' ') AS CUSTCHRGSTAFFNM , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）END
                .Append("   T6.SCHE_STF_CD AS ACCOUNT_PLAN , ")
                .Append("   T2.CST_ID AS CRCUSTID , ")
                .Append("   T7.CST_VCL_TYPE AS CUSTOMERCLASS ")
                .Append(" FROM ")
                .Append("   TB_T_SALES T1 , ")
                .Append("   TB_T_REQUEST T2 , ")
                .Append("   TB_T_ACTIVITY T6 , ")
                .Append("   TB_M_CUSTOMER_VCL T7 , ")
                .Append("   TBL_USERS T8 ")
                .Append(" WHERE ")
                .Append("       T1.REQ_ID = T2.REQ_ID ")
                .Append("   AND T2.LAST_ACT_ID = T6.ACT_ID ")
                .Append("   AND T2.CST_ID = T7.CST_ID ")
                .Append("   AND T2.VCL_ID = T7.VCL_ID ")
                .Append("   AND T7.SLS_PIC_STF_CD = T8.ACCOUNT(+) ")
                .Append("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("   AND T2.VCL_ID = '0' ")
                .Append("   AND T7.DLR_CD = :DLRCD ")
                .Append(" UNION ALL ")
                .Append(" SELECT ")
                .Append("   /* SC3080203_175 */ ")
                .Append("   0 AS CRPLAN_ID , ")
                .Append("   ' ' AS BFAFDVS , ")
                .Append("   0 AS CRDVSID , ")
                .Append("   T2.CST_ID AS INSDID , ")
                .Append("   ' ' AS SERIESCODE , ")
                .Append("   ' ' AS SERIESNAME , ")
                .Append("   ' ' AS VCLREGNO , ")
                .Append("   T2.ATTPLAN_ID AS SUBCTGCODE , ")
                .Append("    TO_CHAR(T2.SVC_CD) AS SERVICECD , ")
                .Append("   ' ' AS SUBCTGORGNAME , ")
                .Append("   ' ' AS SUBCTGORGNAME_EX , ")
                .Append("   T2.ATTPLAN_ID AS PROMOTION_ID , ")
                .Append("   CASE WHEN T2.CONTINUE_ACT_STATUS = '31' THEN '3' ")
                .Append("        WHEN T2.CONTINUE_ACT_STATUS = '32' THEN '5' ")
                .Append("        ELSE CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN ")
                .Append("                 '1' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '20' THEN ")
                .Append("                 '2' ")
                .Append("             WHEN T1.SALES_PROSPECT_CD = '10' THEN ")
                .Append("                 '4' ")
                .Append("             ELSE ")
                .Append("                '4' ")
                .Append("            END ")
                .Append("        END AS CRACTRESULT , ")
                .Append("   ' ' AS PLANDVS , ")
                .Append("   ' ' AS VIN , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）START
                .Append("   NVL(T8.USERNAME,' ') AS CUSTCHRGSTAFFNM , ")
                '2014/02/26 TCS 松月 担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）END
                .Append("   T6.SCHE_STF_CD AS ACCOUNT_PLAN , ")
                .Append("   T2.CST_ID AS CRCUSTID , ")
                .Append("   T7.CST_VCL_TYPE AS CUSTOMERCLASS ")
                .Append(" FROM ")
                .Append("   TB_T_SALES T1 , ")
                .Append("   TB_T_ATTRACT T2 , ")
                .Append("   TB_T_ACTIVITY T6 , ")
                .Append("   TB_M_CUSTOMER_VCL T7 , ")
                .Append("   TBL_USERS T8 ")
                .Append(" WHERE ")
                .Append("       T1.ATT_ID = T2.ATT_ID ")
                .Append("   AND T2.LAST_ACT_ID = T6.ACT_ID ")
                .Append("   AND T2.CST_ID = T7.CST_ID ")
                .Append("   AND T2.VCL_ID = T7.VCL_ID ")
                .Append("   AND T7.SLS_PIC_STF_CD = T8.ACCOUNT(+) ")
                .Append("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("   AND T2.VCL_ID = '0' ")
                .Append("   AND T7.DLR_CD = :DLRCD ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisFllw_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 076.Follow-up BOX活動内容取得(活動結果登録用)
    ''' </summary>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActHisContent(ByVal seqno As Long) As ActivityInfoDataSet.ActivityInfoActHisContentDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoActHisContentDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    ACTIONTYPE, ")
                .Append("    DECODE(ACTION_LOCAL,' ', ACTION, ACTION_LOCAL) AS ACTION, ")
                .Append("    METHOD, ")
                .Append("    ACTIONCD, ")
                .Append("    CATEGORYID, ")
                .Append("    CATEGORYDVSID ")
                .Append("FROM ")
                .Append("    TBL_FLLWUPBOXCONTENT ")
                .Append("WHERE ")
                .Append("    SEQNO = :SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 077.Follow-up Box選択車種取得(活動結果登録用)
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <param name="seqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActHisCarSeq(ByVal dlrcd As String, ByVal fllwupboxseqno As Decimal, ByVal seqno As Long) As ActivityInfoDataSet.ActivityInfoActHisSelCarDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_144 */ ")
            .Append("  T9.SERIESCD AS SERIESNM , ")
            .Append("  T9.VCLMODEL_NAME , ")
            .Append("  T9.COLORCD AS DISP_BDY_COLOR , ")
            .Append("  T9.QUANTITY ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    T8.SERIESCD , ")
            .Append("    T8.MODELCD AS VCLMODEL_NAME , ")
            .Append("    T8.SERIESNM , ")
            .Append("    T8.COLORCD , ")
            .Append("    T8.SEQNO , ")
            .Append("    T8.QUANTITY ")
            .Append("  FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      T1.MODEL_CD AS SERIESCD , ")
            .Append("      T1.GRADE_CD AS MODELCD , ")
            .Append("      T1.BODYCLR_CD AS COLORCD , ")
            .Append("      T1.PREF_VCL_SEQ AS SEQNO , ")
            .Append("      T7.COMSERIESCD , ")
            .Append("      T7.SERIESNM , ")
            .Append("      T1.PREF_AMOUNT AS QUANTITY ")
            .Append("    FROM ")
            .Append("      TB_T_PREFER_VCL T1 , ")
            .Append("      ( ")
            .Append("      SELECT ")
            .Append("        T2.DLR_CD AS DLRCD , ")
            .Append("        T3.MODEL_CD AS SERIESCD , ")
            .Append("        T3.MODEL_NAME AS SERIESNM , ")
            .Append("        T4.MAKER_TYPE AS TOYOTABRAND , ")
            .Append("        T3.MODEL_PICTURE AS IMAGEPATH , ")
            .Append("        T3.COMMON_MODEL_CD AS COMSERIESCD , ")
            .Append("        T3.INUSE_FLG , ")
            .Append("        NULL AS DELDATE , ")
            .Append("        T3.MAKER_CD AS MAKERCD ")
            .Append("      FROM ")
            .Append("        TB_M_DEALER T2 , ")
            .Append("        TB_M_MODEL T3 , ")
            .Append("        TB_M_MAKER T4 , ")
            .Append("        TB_M_MODEL_DLR T5 ")
            .Append("      WHERE ")
            .Append("            T2.DLR_CD = T5.DLR_CD ")
            .Append("        AND T3.MODEL_CD = T5.MODEL_CD ")
            .Append("        AND T3.MAKER_CD = T4.MAKER_CD ")
            .Append("        AND T2.DLR_CD = :DLRCD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        OR (T5.DLR_CD = 'XXXXX' ")
            .Append("        AND T2.DLR_CD = :DLRCD ")
            .Append("        AND T2.INUSE_FLG = '1' ")
            .Append("        AND NOT EXISTS ")
            .Append("          ( ")
            .Append("          SELECT ")
            .Append("1 ")
            .Append("          FROM ")
            .Append("            TB_M_MODEL_DLR T6 ")
            .Append("          WHERE ")
            .Append("                T6.DLR_CD = T2.DLR_CD ")
            .Append("            AND T6.MODEL_CD = T3.MODEL_CD ")
            .Append("          )) ")
            .Append("      ) T7 ")
            .Append("  WHERE ")
            .Append("          T1.MODEL_CD = T7.SERIESCD ")
            .Append("      AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("      AND T1.PREF_VCL_SEQ = :SEQNO ")
            .Append("    ) T8 ")
            .Append("  ) T9 ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoActHisSelCarDataTable)("ActivityInfo_144")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 081. Follow-up Box商談 を更新　(商談中⇒商談終了)
    ''' </summary>
    ''' <param name="fllwupbox_seqno"></param>
    ''' <param name="actualaccount"></param>
    ''' <param name="salesstarttime"></param>
    ''' <param name="salesendtime"></param>
    ''' <param name="account"></param>
    ''' <param name="updateid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal fllwupbox_seqno As Decimal, _
                        ByVal actualaccount As String, _
                        ByVal salesstarttime As Date, _
                        ByVal salesendtime As Date, _
                        ByVal account As String, _
                        ByVal updateid As String) As Integer

        Using query As New DBUpdateQuery("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* ActivityInfo_000 */ ")
                .Append("    TBL_FLLWUPBOX_SALES ")
                .Append("SET ")
                .Append("    NEWFLLWUPBOXFLG = '0', ")
                .Append("    REGISTFLG = '1', ")
                .Append("    ACTUALACCOUNT = :ACTUALACCOUNT, ")
                .Append("    STARTTIME = :STARTTIME, ")
                .Append("    ENDTIME = :ENDTIME, ")
                .Append("    UPDATEID = :UPDATEID, ")
                .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ")
                .Append("    UPDATEDATE = SYSDATE ")
                .Append("WHERE ")
                .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                .Append("    REGISTFLG = '0' ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Varchar2, actualaccount)
            query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, salesstarttime)
            query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, salesendtime)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)
            Return query.Execute()
        End Using
    End Function
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 063.自社客名前・敬称取得
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgNameTitle(ByVal originalid As String) As ActivityInfoDataSet.ActivityInfoNameTitleDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoNameTitleDataTable)("ActivityInfo_148")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgNameTitle_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_148 */ ")
                .Append("  CST_NAME AS NAME , ")
                .Append("  NAMETITLE_NAME AS NAMETITLE ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :ORIGINALID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgNameTitle_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 064.未取引客名前・敬称取得
    ''' </summary>
    ''' <param name="cstid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewNameTitle(ByVal cstid As String) As ActivityInfoDataSet.ActivityInfoNameTitleDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoNameTitleDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    NAME, ")
                .Append("    NAMETITLE ")
                .Append("FROM ")
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                .Append("    TBL_NEWCUSTOMER ")
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                .Append("WHERE ")
                .Append("    CSTID = :CSTID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 065.CalDAV用接触方法名取得
    ''' </summary>
    ''' <param name="contactno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactNM(ByVal contactno As Long) As ActivityInfoDataSet.ActivityInfoGetContactNmDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetContactNmDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactNM_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT /* ActivityInfo_000 */  ")
                .Append("    CONTACT_NAME  AS CONTACT ")
                .Append("FROM  ")
                .Append("    TB_M_CONTACT_MTD  ")
                .Append("WHERE  ")
                .Append("CONTACT_MTD = :CONTACTNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactNM_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 066.CalDAV用ToDo背景色取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="createdatadiv"></param>
    ''' <param name="scheduledvs"></param>
    ''' <param name="nextactiondvs"></param>
    ''' <param name="contactno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetToDoColor(ByVal dlrcd As String, ByVal createdatadiv As String, ByVal scheduledvs As String,
                                 ByVal nextactiondvs As String, ByVal contactno As Long) As ActivityInfoDataSet.ActivityInfoTodoColorDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoTodoColorDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    BACKGROUNDCOLOR ")
                .Append("FROM ")
                .Append("    TBL_TODO_TIP_COLOR ")
                .Append("WHERE ")
                .Append("    DLRCD = :DLRCD AND ")
                .Append("    CREATEDATADIV= :CREATEDATADIV AND ")
                .Append("    SCHEDULEDVS= :SCHEDULEDVS AND ")
                .Append("    NEXTACTIONDVS= :NEXTACTIONDVS AND ")
                .Append("    CONTACTNO= :CONTACTNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("CREATEDATADIV", OracleDbType.Char, createdatadiv)
            query.AddParameterWithTypeValue("SCHEDULEDVS", OracleDbType.Char, scheduledvs)
            query.AddParameterWithTypeValue("NEXTACTIONDVS", OracleDbType.Char, nextactiondvs)
            query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ''' <summary>
    ''' 080.競合車種取得(ALL)
    ''' </summary>
    ''' <param name="competitionmakerno"></param>
    ''' <param name="competitorcd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCompetition(ByVal competitionmakerno As String, ByVal competitorcd As String) As ActivityInfoDataSet.ActivityInfoCompetitionDataTable
        Dim sql As New StringBuilder
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetition_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_155 */ ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      T1.MAKER_NAME ")
            .Append("    FROM ")
            .Append("      TB_M_MAKER T1 ")
            .Append("    WHERE ")
            .Append("      T1.MAKER_CD = :COMPETITIONMAKERNO ")
            .Append("    ) AS COMPETITIONMAKER , ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("      T2.MODEL_NAME ")
            .Append("    FROM ")
            .Append("      TB_M_MODEL T2 ")
            .Append("    WHERE ")
            .Append("      T2.MODEL_CD = :COMPETITORCD ")
            .Append("    ) AS COMPETITORNM ")
            .Append("FROM ")
            .Append("  DUAL ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCompetitionDataTable)("ActivityInfo_155")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("COMPETITIONMAKERNO", OracleDbType.NVarchar2, competitionmakerno)
            query.AddParameterWithTypeValue("COMPETITORCD", OracleDbType.NVarchar2, competitorcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetition_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 049.顧客メモ履歴登録
    ''' </summary>
    ''' <param name="crcustid"></param>
    ''' <param name="dlrcd"></param>
    ''' <param name="account"></param>
    ''' <param name="memo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertCustMemohis(ByVal crcustid As Decimal, ByVal dlrcd As String,
                                           ByVal account As String, ByVal memo As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustMemohis_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("INSERT ")
            .Append("    /*ActivityInfo_161*/ ")
            .Append("INTO TB_T_CUSTOMER_MEMO ( ")
            .Append("     DLR_CD , ")
            .Append("     CST_ID , ")
            .Append("     CST_MEMO_SEQ , ")
            .Append("     CST_MEMO , ")
            .Append("     CREATE_STF_CD , ")
            .Append("     CREATE_DATETIME , ")
            .Append("     ROW_CREATE_DATETIME , ")
            .Append("     ROW_CREATE_ACCOUNT , ")
            .Append("     ROW_CREATE_FUNCTION , ")
            .Append("     ROW_UPDATE_DATETIME , ")
            .Append("     ROW_UPDATE_ACCOUNT , ")
            .Append("     ROW_UPDATE_FUNCTION , ")
            .Append("     ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append(" VALUES ( ")
            .Append("     :DLRCD , ")
            .Append("     :CRCUSTID , ")
            .Append("     TO_NUMBER(SEQ_CUSTMEMOHIS_SEQNO.NEXTVAL) , ")
            .Append("     :MEMO , ")
            .Append("     :ACCOUNT , ")
            .Append("     SYSDATE , ")
            .Append("     SYSDATE , ")
            .Append("     :ACCOUNT , ")
            .Append("     'SC3080203' , ")
            .Append("     SYSDATE , ")
            .Append("     :ACCOUNT , ")
            .Append("     'SC3080203' , ")
            .Append("     0 ")
            .Append(" ) ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_161")
            query.CommandText = sql.ToString()
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustMemohis_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 044.希望車種の台数を取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSelectedCarNum(ByVal fllwupboxseqno As Decimal, ByVal seqno As String) As ActivityInfoDataSet.ActivityInfoSelectedCarNumDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSelectedCarNumDataTable)("ActivityInfo_169")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCarNum_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_169 */ ")
                .Append("  PREF_AMOUNT AS QUANTITY ")
                .Append("FROM ")
                .Append("  TB_T_PREFER_VCL ")
                .Append("WHERE ")
                .Append("      SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("  AND PREF_VCL_SEQ = TO_NUMBER(:SEQNO) ")
            End With
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCarNum_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 008.Follow-up Box成約車種追加
    ''' </summary>
    ''' <param name="salesid"></param>
    ''' <param name="prefvclseq"></param>
    ''' <param name="modelcd"></param>
    ''' <param name="gradecd"></param>
    ''' <param name="bodyclrcd"></param>
    ''' <param name="acount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertFllwupboxSuccessSeries(ByVal salesid As Decimal, ByVal prefvclseq As Long, ByVal modelcd As String,
                                        ByVal gradecd As String, ByVal bodyclrcd As String, ByVal acount As String) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_170")
            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* ActivityInfo_170 */ ")
                .Append("   INTO TB_T_PREFER_VCL ")
                .Append("      ( ")
                .Append("        SALES_ID ")
                .Append("      , PREF_VCL_SEQ ")
                .Append("      , MODEL_CD ")
                .Append("      , GRADE_CD ")
                .Append("      , SUFFIX_CD ")
                .Append("      , BODYCLR_CD ")
                .Append("      , INTERIORCLR_CD ")
                .Append("      , PREF_AMOUNT ")
                .Append("      , EST_RSLT_FLG ")
                .Append("      , SALESBKG_NUM ")
                .Append("      , ROW_CREATE_DATETIME ")
                .Append("      , ROW_CREATE_ACCOUNT ")
                .Append("      , ROW_CREATE_FUNCTION ")
                .Append("      , ROW_UPDATE_DATETIME ")
                .Append("      , ROW_UPDATE_ACCOUNT ")
                .Append("      , ROW_UPDATE_FUNCTION ")
                .Append("      , ROW_LOCK_VERSION ")
                .Append("      ) ")
                .Append(" VALUES ")
                .Append("      ( ")
                .Append("        :SALES_ID ")
                .Append("      , :PREF_VCL_SEQ ")
                .Append("      , :MODEL_CD ")
                .Append("      , :GRADE_CD ")
                .Append("      , ' ' ")
                .Append("      , :BODYCLR_CD ")
                .Append("      , ' ' ")
                .Append("      , 1 ")
                .Append("      , '0' ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .Append("      , ' ' ")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .Append("      , SYSDATE ")
                .Append("      , :INSERTACCOUNT ")
                .Append("      , 'SC3080203' ")
                .Append("      , SYSDATE ")
                .Append("      , :UPDATEACCOUNT ")
                .Append("      , 'SC3080203' ")
                .Append("      , 0 ")
                .Append("      ) ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Char, salesid)
            query.AddParameterWithTypeValue("PREF_VCL_SEQ", OracleDbType.Char, prefvclseq)
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.Int64, modelcd)
            query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.Date, gradecd)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Char, bodyclrcd)
            query.AddParameterWithTypeValue("INSERTACCOUNT", OracleDbType.Date, acount)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, acount)
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 松月 2013/10対応版　既存流用 END





    ''' <summary>
    ''' 接触方法マスタ取得
    ''' </summary>
    ''' <param name="bookedafterflg">受注後フラグ (指定がなければ前件検索)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActContact(ByVal bookedafterflg As String) As ActivityInfoDataSet.ActivityInfoActContactDataTable

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoActContactDataTable)("ActivityInfo_001")
            Dim sql As New StringBuilder
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContact_Start")
            'ログ出力 End *****************************************************************************
            With sql
                If (String.IsNullOrEmpty(bookedafterflg) = False) Then
                    .Append("SELECT /* ActivityInfo_001 */  ")
                    .Append("    T1.CONTACT_MTD AS CONTACTNO,  ")
                    .Append("    T1.CONTACT_NAME AS CONTACT,  ")
                    .Append("    T1.SALES_INPUT_FLG AS PROCESS,  ")
                    .Append("    CASE WHEN NVL(T2.INIT_SEL_KEY,' ') = ' ' THEN 0 ")
                    .Append("         ELSE 1 END AS FIRSTSELECT_WALKIN,  ")
                    .Append("    CASE WHEN NVL(T3.INIT_SEL_KEY,' ') = ' ' THEN 0 ")
                    .Append("         ELSE 1 END AS FIRSTSELECT_NOTWALKIN ")
                    .Append("FROM  ")
                    .Append("    TB_M_CONTACT_MTD T1, ")
                    .Append("    TB_M_INIT_SEL_CONTROL T2, ")
                    .Append("    TB_M_INIT_SEL_CONTROL T3 ")
                    .Append("WHERE  ")
                    .Append("    T1.CONTACT_MTD = T2.INIT_SEL_KEY(+) ")
                    .Append("    AND T1.CONTACT_MTD = T3.INIT_SEL_KEY(+) ")
                    .Append("    AND T1.INUSE_FLG = '1' ")
                    .Append("    AND T2.TYPE_CD(+)  = 'CONTACT_MTD' ")
                    .Append("    AND T3.TYPE_CD(+)  = 'CONTACT_MTD'  ")
                    .Append("    AND T2.USE_TYPE(+)  = '01'  ")
                    .Append("    AND T3.USE_TYPE(+)  = '02'  ")
                    .Append("ORDER BY  ")
                    .Append("    T1.SORT_ORDER  ")
                Else
                    .Append("SELECT * ")
                    .Append("FROM ")
                    .Append("(SELECT /* ActivityInfo_001 */   ")
                    .Append("     TO_CHAR(T1.CONTACT_MTD) AS CONTACTNO,   ")
                    .Append("     TO_CHAR(T1.CONTACT_NAME) AS CONTACT,   ")
                    .Append("     TO_CHAR(T1.SALES_INPUT_FLG) AS PROCESS,   ")
                    .Append("     CASE WHEN NVL(T2.INIT_SEL_KEY,' ') = ' ' THEN 0  ")
                    .Append("          ELSE 1 END AS FIRSTSELECT_WALKIN,   ")
                    .Append("     CASE WHEN NVL(T3.INIT_SEL_KEY,' ') = ' ' THEN 0  ")
                    .Append("          ELSE 1 END AS FIRSTSELECT_NOTWALKIN ")
                    .Append(" FROM   ")
                    .Append("     TB_M_CONTACT_MTD T1,  ")
                    .Append("     TB_M_INIT_SEL_CONTROL T2,  ")
                    .Append("     TB_M_INIT_SEL_CONTROL T3  ")
                    .Append(" WHERE   ")
                    .Append("     T1.CONTACT_MTD = T2.INIT_SEL_KEY(+)  ")
                    .Append("     AND T1.CONTACT_MTD = T3.INIT_SEL_KEY(+)  ")
                    .Append("     AND T1.INUSE_FLG = '1'  ")
                    .Append("     AND T2.TYPE_CD(+)  = 'CONTACT_MTD'  ")
                    .Append("     AND T3.TYPE_CD(+)  = 'CONTACT_MTD'   ")
                    .Append("     AND T2.USE_TYPE(+)  = '01'   ")
                    .Append("     AND T3.USE_TYPE(+)  = '02'   ")
                    .Append(" ORDER BY   ")
                    .Append("     T1.SORT_ORDER  ) ")
                    ' 2019/05/24 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス）【UAT-0614】DEL
                End If

            End With

            query.CommandText = sql.ToString()
            ' 2019/05/24 TS 舩橋 (FS)納車時オペレーションCS向上にむけた評価（サービス）【UAT-0614】DEL

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContact_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()

        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' CR活動成功のデータ存在確認
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <param name="cractresult">CR活動結果（Success）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CountFllwupboxRslt(ByVal fllwupboxseqno As Decimal, _
                                              ByVal cractresult As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupboxRslt_Start")
        'ログ出力 End *****************************************************************************
        With sql
            ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
            .Append("SELECT ")
            .Append("  /* ActivityInfo_301 */ ")
            .Append("  SUM(CNT) FROM( ")
            .Append("  (SELECT COUNT(1) CNT ")
            .Append("    FROM TB_T_SALES T1 ")
            .Append("    WHERE EXISTS (SELECT 1 ")
            .Append("       FROM TB_T_ACTIVITY T2 ")
            .Append("       WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("       AND T1.REQ_ID = T2.REQ_ID ")
            .Append("       AND T2.ACT_STATUS = :CRACTRESULT) ")
            .Append("    AND T1.SALES_ID = :FLLWUPBOX_SEQNO) ")
            .Append("  UNION (SELECT COUNT(1) CNT ")
            .Append("    FROM TB_H_SALES T1 ")
            .Append("    WHERE EXISTS (SELECT 1 ")
            .Append("      FROM TB_H_ACTIVITY T2 ")
            .Append("      WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("      AND T1.REQ_ID = T2.REQ_ID ")
            .Append("      AND T2.ACT_STATUS = :CRACTRESULT) ")
            .Append("      AND T1.SALES_ID = :FLLWUPBOX_SEQNO)) ")
            ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 END
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCountDataTable)("ActivityInfo_301")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.NVarchar2, cractresult)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupboxRslt_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetCount()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 契約書No取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>データセット</returns>
    ''' <remarks>契約書No.を取得する</remarks>
    Public Shared Function GetContractNo(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoContractNoDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractNo_Start")
        'ログ出力 End *****************************************************************************
        With sql
            ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .AppendLine("SELECT /* ActivityInfo_003 */ ")
            .AppendLine("       DECODE(B.CANCEL_FLG, '1', '', A.CONTRACTNO) AS CONTRACTNO")
            .AppendLine("  FROM TBL_ESTIMATEINFO A ")
            .AppendLine("     , TB_T_SALESBOOKING B")
            .AppendLine(" WHERE A.DLRCD = B.DLR_CD(+)")
            .AppendLine("   AND RTRIM(A.CONTRACTNO) = B.SALESBKG_NUM(+)")
            .AppendLine("   AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .AppendLine("   AND A.DELFLG = '0'")
            .AppendLine("   AND A.CONTRACTFLG = '1' ")
            ' 2014/02/12 TCS 高橋 受注後フォロー機能開発 END
        End With

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContractNoDataTable)("ActivityInfo_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractNo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using

    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' プロセス取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProcess(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoGetProcessDataTable

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetProcess_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_302 */ ")
            .Append("  PREF_VCL_SEQ AS SEQNO , ")
            .Append("  SALES_ACT_ID AS CTNTSEQNO , ")
            .Append("  ACTIONCD , ")
            .Append("  MAX(RSLT_DATETIME) AS LASTACTDATE ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT  ")
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.TESTDRIVE_RSLT_DATE AS RSLT_DATETIME , ")
            .Append("      'A26' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_T_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_T_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("          AND RSLT_SALES_CAT = '4' ")
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_T_PREFER_VCL T3 , ")
            .Append("      TB_H_TESTDRIVE T4  ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.SALES_ID ")
            .Append("      AND T3.MODEL_CD = T4.PREF_MODEL_CD ")
            .Append("      AND T3.GRADE_CD = T4.PREF_GRADE_CD ")
            .Append("    UNION ALL ")
            .Append("  SELECT  ")
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.UPDATEDATE AS RSLT_DATETIME , ")
            .Append("      'A23' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_T_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_T_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("          AND RSLT_SALES_CAT = '6' ")
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_T_PREFER_VCL T3 , ")
            .Append("      TBL_ESTIMATEINFO T4 , ")
            .Append("      TBL_EST_VCLINFO T5 , ")
            .Append("      TBL_MSTEXTERIOR T6 ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.FLLWUPBOX_SEQNO ")
            .Append("      AND T4.ESTIMATEID = T5.ESTIMATEID ")
            .Append("      AND T3.GRADE_CD = T6.VCLMODEL_CODE ")
            .Append("      AND T3.BODYCLR_CD = T6.COLOR_CD ")
            .Append("      AND T3.MODEL_CD = T5.SERIESCD ")
            .Append("      AND T3.GRADE_CD = T5.MODELCD ")
            .Append("      AND T6.BODYCLR_CD = T5.EXTCOLORCD ")
            .Append("    UNION ALL ")
            .Append("  SELECT  ")
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.UPDATEDATE AS RSLT_DATETIME , ")
            .Append("      'A23' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_T_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_T_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("          AND RSLT_SALES_CAT = '6' ")
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_T_PREFER_VCL T3 , ")
            .Append("      TBL_ESTIMATEINFO T4 , ")
            .Append("      TBL_EST_VCLINFO T5  ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.FLLWUPBOX_SEQNO ")
            .Append("      AND T4.ESTIMATEID = T5.ESTIMATEID ")
            .Append("      AND T3.MODEL_CD = T5.SERIESCD ")
            .Append("      AND T3.GRADE_CD = T5.MODELCD ")
            .Append("      AND T3.BODYCLR_CD = T5.EXTCOLORCD ")
            .Append("      AND T3.BODYCLR_CD = ' ' ")
            .Append("    UNION ALL ")
            .Append("  SELECT ")
            .Append("    T5.PREF_VCL_SEQ, ")
            .Append("    T4.SALES_ACT_ID , ")
            .Append("    T6.RSLT_DATETIME , ")
            .Append("    'A22' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("    TB_T_SALES_ACT T4, ")
            .Append("    TB_T_PREFER_VCL T5, ")
            .Append("    TB_T_ACTIVITY T6 ")
            .Append("  WHERE ")
            .Append("        T4.SALES_ID = T5.SALES_ID ")
            .Append("    AND T4.MODEL_CD = T5.MODEL_CD ")
            .Append("    AND T4.ACT_ID = T6.ACT_ID ")
            .Append("    AND T4.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND T4.RSLT_SALES_CAT = '2' ")
            .Append("    UNION ALL ")
            .Append("  SELECT ")
            .Append("    T8.PREF_VCL_SEQ, ")
            .Append("    T7.SALES_ACT_ID , ")
            .Append("    T9.RSLT_DATETIME , ")
            .Append("    'A30' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("    TB_T_SALES_ACT T7, ")
            .Append("    TB_T_PREFER_VCL T8, ")
            .Append("    TB_T_ACTIVITY T9 ")
            .Append("  WHERE ")
            .Append("        T7.SALES_ID = T8.SALES_ID ")
            .Append("    AND T7.ACT_ID = T9.ACT_ID ")
            .Append("    AND T7.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND T7.RSLT_SALES_CAT = '7' ")
            .Append("    UNION ALL ")
            .Append("  SELECT  ")
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.TESTDRIVE_RSLT_DATE AS RSLT_DATETIME , ")
            .Append("      'A26' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_H_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_H_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("          AND RSLT_SALES_CAT = '4' ")
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_H_PREFER_VCL T3 , ")
            .Append("      TB_H_TESTDRIVE T4  ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.SALES_ID ")
            .Append("      AND T3.MODEL_CD = T4.PREF_MODEL_CD ")
            .Append("      AND T3.GRADE_CD = T4.PREF_GRADE_CD ")
            .Append("    UNION ALL ")
            .Append("  SELECT  ")
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.UPDATEDATE AS RSLT_DATETIME , ")
            .Append("      'A23' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_H_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_H_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .Append("          AND RSLT_SALES_CAT = '6' ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_H_PREFER_VCL T3 , ")
            .Append("      TBL_ESTIMATEINFO T4 , ")
            .Append("      TBL_EST_VCLINFO T5 , ")
            .Append("      TBL_MSTEXTERIOR T6 ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.FLLWUPBOX_SEQNO ")
            .Append("      AND T4.ESTIMATEID = T5.ESTIMATEID ")
            .Append("      AND T3.GRADE_CD = T6.VCLMODEL_CODE ")
            .Append("      AND T3.BODYCLR_CD = T6.COLOR_CD ")
            .Append("      AND T3.MODEL_CD = T5.SERIESCD ")
            .Append("      AND T3.GRADE_CD = T5.MODELCD ")
            .Append("      AND T6.BODYCLR_CD = T5.EXTCOLORCD ")
            .Append("    UNION ALL ")
            '2016/03/25 TCS 鈴木 性能改善 START
            .Append("  SELECT  /*+ INDEX(T5 PK_TBL_EST_VCLINFO) */  ")
            '2016/03/25 TCS 鈴木 性能改善 END
            .Append("      T3.PREF_VCL_SEQ, ")
            .Append("      T1.SALES_ACT_ID, ")
            .Append("      T4.UPDATEDATE AS RSLT_DATETIME , ")
            .Append("      'A23' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("      TB_H_SALES_ACT T1 , ")
            .Append("      (SELECT  ")
            .Append("          MAX(ACT_ID) AS ACT_ID ")
            .Append("      FROM ")
            .Append("          TB_H_SALES_ACT  ")
            .Append("      WHERE ")
            .Append("          SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("          AND RSLT_SALES_CAT = '6' ")
            .Append("      GROUP BY MODEL_CD ")
            .Append("      ) T2, ")
            .Append("      TB_H_PREFER_VCL T3 , ")
            .Append("      TBL_ESTIMATEINFO T4 , ")
            .Append("      TBL_EST_VCLINFO T5 ")
            .Append("  WHERE ")
            .Append("      T1.ACT_ID = T2.ACT_ID ")
            .Append("      AND T1.SALES_ID = T3.SALES_ID ")
            .Append("      AND T1.MODEL_CD = T3.MODEL_CD ")
            .Append("      AND T1.SALES_ID = T4.FLLWUPBOX_SEQNO ")
            .Append("      AND T4.ESTIMATEID = T5.ESTIMATEID ")
            .Append("      AND T3.MODEL_CD = T5.SERIESCD ")
            .Append("      AND T3.GRADE_CD = T5.MODELCD ")
            .Append("      AND T3.BODYCLR_CD = T5.EXTCOLORCD ")
            .Append("      AND T3.BODYCLR_CD = ' ' ")
            .Append("    UNION ALL ")
            .Append("  SELECT ")
            .Append("    T15.PREF_VCL_SEQ, ")
            .Append("    T14.SALES_ACT_ID , ")
            .Append("    T16.RSLT_DATETIME , ")
            .Append("    'A22' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("    TB_H_SALES_ACT T14, ")
            .Append("    TB_H_PREFER_VCL T15, ")
            .Append("    TB_H_ACTIVITY T16 ")
            .Append("  WHERE ")
            .Append("        T14.SALES_ID = T15.SALES_ID ")
            .Append("    AND T14.MODEL_CD = T15.MODEL_CD ")
            .Append("    AND T14.ACT_ID = T16.ACT_ID ")
            .Append("    AND T14.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND T14.RSLT_SALES_CAT = '2' ")
            .Append("    UNION ALL ")
            .Append("  SELECT ")
            .Append("    T18.PREF_VCL_SEQ, ")
            .Append("    T17.SALES_ACT_ID , ")
            .Append("    T19.RSLT_DATETIME , ")
            .Append("    'A30' AS ACTIONCD ")
            .Append("  FROM ")
            .Append("    TB_H_SALES_ACT T17, ")
            .Append("    TB_H_PREFER_VCL T18, ")
            .Append("    TB_H_ACTIVITY T19 ")
            .Append("  WHERE ")
            .Append("        T17.SALES_ID = T18.SALES_ID ")
            .Append("    AND T17.ACT_ID = T19.ACT_ID ")
            .Append("    AND T17.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND T17.RSLT_SALES_CAT = '7' ")
            .Append("  ) ")
            .Append("GROUP BY ")
            .Append("  PREF_VCL_SEQ , ")
            .Append("  SALES_ACT_ID , ")
            .Append("  ACTIONCD ")
            .Append("ORDER BY ")
            .Append("  PREF_VCL_SEQ , ")
            .Append("  SALES_ACT_ID , ")
            .Append("  ACTIONCD ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetProcessDataTable)("ActivityInfo_302")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetProcess_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' ステータス取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetStatus(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoGetStatusDataTable

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStatus_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_303 */ ")
            .Append(" CASE WHEN T2.REQ_STATUS = '31' THEN '3' ")
            .Append("      WHEN T2.REQ_STATUS = '32' THEN '5' ")
            .Append("      WHEN T3.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("      WHEN T3.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("      ELSE ")
            .Append("        CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("             WHEN T1.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("             WHEN T1.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("             ELSE '4' END ")
            .Append("      END AS CRACTRESULT ")
            .Append("FROM ")
            .Append("  TB_T_SALES T1 , ")
            .Append("  TB_T_REQUEST T2 , ")
            .Append("  TB_T_ATTRACT T3 ")
            .Append("WHERE ")
            .Append("      T1.REQ_ID = T2.REQ_ID(+) ")
            .Append("  AND T1.ATT_ID = T3.ATT_ID(+) ")
            .Append("  AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append(" CASE WHEN T5.REQ_STATUS = '31' THEN '3' ")
            .Append("      WHEN T5.REQ_STATUS = '32' THEN '5' ")
            .Append("      WHEN T6.CONTINUE_ACT_STATUS = '31' THEN '3' ")
            .Append("      WHEN T6.CONTINUE_ACT_STATUS = '32' THEN '5' ")
            .Append("      ELSE ")
            .Append("        CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
            .Append("             WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
            .Append("             WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
            .Append("             ELSE '4' END ")
            .Append("      END AS CRACTRESULT ")
            .Append("FROM ")
            .Append("  TB_H_SALES T4 , ")
            .Append("  TB_H_REQUEST T5 , ")
            .Append("  TB_H_ATTRACT T6 ")
            .Append("WHERE ")
            .Append("      T4.REQ_ID = T5.REQ_ID(+) ")
            .Append("  AND T4.ATT_ID = T6.ATT_ID(+) ")
            .Append("  AND T4.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetStatusDataTable)("ActivityInfo_303")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStatus_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 選択希望車種リスト取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedSeries(ByVal dlrcd As String,
                                      ByVal strcd As String,
                                      ByVal cntcd As String,
                                      ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSeries_Start")
        'ログ出力 End *****************************************************************************

        '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim useFlgSuffix As String
        Dim useFlgInteriorClr As String

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            useFlgSuffix = "0"
        Else
            useFlgSuffix = dataRow.SETTING_VAL
        End If

        '内装色使用可否フラグ(設定値が無ければ0)
        Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRowclr = systemBiz.GetSystemSetting(USE_FLG_INTERIORCLR)

        If IsNothing(dataRowclr) Then
            useFlgInteriorClr = "0"
        Else
            useFlgInteriorClr = dataRowclr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
        '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        With sql

            .Append(" SELECT /* ActivityInfo_307 */ ")
            .Append("     T21.SERIESCD , ")
            .Append("     T21.SERIESNM , ")
            .Append("     T21.MODELCD , ")
            .Append("     T21.VCLMODEL_NAME , ")
            .Append("     T21.COLORCD , ")
            .Append("     T21.DISP_BDY_COLOR , ")
            .Append("     T21.PICIMAGE , ")
            .Append("     T21.LOGOIMAGE , ")
            .Append("     T21.QUANTITY , ")
            .Append("     T21.SEQNO, ")
            .Append("     T21.SALES_PROSPECT_CD, ")
            .Append("     T21.ROWLOCKVERSION, ")
            .Append("     T21.SUFFIX_CD, ")
            .Append("     T21.INTERIORCLR_CD, ")
            .Append("     T21.SUFFIX_NAME, ")
            .Append("     T21.INTERIORCLR_NAME ")
            .Append(" FROM ")
            .Append("     ( ")
            .Append("     SELECT ")
            .Append("         T9.SERIESCD , ")
            .Append("         T9.SERIESNM , ")
            .Append("         T9.MODELCD , ")
            .Append("         T9.GRADE_NAME AS VCLMODEL_NAME , ")
            .Append("         T9.COLORCD , ")
            .Append("         T9.BODYCLR_NAME AS DISP_BDY_COLOR , ")
            .Append("         TRIM(NVL(T21.VCL_PICTURE,T7.MODEL_PICTURE))AS PICIMAGE , ")
            .Append("         TRIM(T7.LOGO_PICTURE) AS LOGOIMAGE , ")
            .Append("         T9.QUANTITY , ")
            .Append("         T9.SEQNO, ")
            .Append("         T9.SALES_PROSPECT_CD, ")
            .Append("         T9.ROWLOCKVERSION, ")
            .Append("         T9.SUFFIX_CD, ")
            .Append("         T9.INTERIORCLR_CD, ")
            .Append("         T9.SUFFIX_NAME, ")
            .Append("         T9.INTERIORCLR_NAME ")
            .Append("     FROM ")
            .Append("         ( ")
            .Append("         SELECT DISTINCT ")
            .Append("             T1.MODEL_CD AS SERIESCD , ")
            .Append("             T1.GRADE_CD AS MODELCD , ")
            .Append("             T1.BODYCLR_CD AS COLORCD , ")
            .Append("             T10.BODYCLR_NAME AS BODYCLR_NAME , ")
            .Append("             T8.COMSERIESCD , ")
            .Append("             T8.SERIESNM , ")
            .Append("             T6.GRADE_NAME , ")
            .Append("             T1.PREF_AMOUNT AS QUANTITY , ")
            .Append("             T1.PREF_VCL_SEQ AS SEQNO, ")
            .Append("             T1.SALES_PROSPECT_CD, ")
            .Append("             T1.ROW_LOCK_VERSION AS ROWLOCKVERSION, ")
            .Append("             T1.SUFFIX_CD, ")
            .Append("             T1.INTERIORCLR_CD, ")
            .Append("             T23.SUFFIX_NAME, ")
            .Append("             T24.INTERIORCLR_NAME ")
            .Append("         FROM ")
            .Append("             TB_T_PREFER_VCL T1 , ")
            .Append("             ( ")
            .Append("                 SELECT ")
            .Append("                     T2.DLR_CD AS DLRCD , ")
            .Append("                     T3.MODEL_CD AS SERIESCD , ")
            .Append("                     T3.MODEL_NAME AS SERIESNM , ")
            .Append("                     T3.COMMON_MODEL_CD AS COMSERIESCD ")
            .Append("                 FROM ")
            .Append("                     TB_M_DEALER T2 , ")
            .Append("                     TB_M_MODEL T3 , ")
            .Append("                     TB_M_MODEL_DLR T4 ")
            .Append("                 WHERE ")
            .Append("                     T2.DLR_CD = T4.DLR_CD ")
            .Append("                     AND T3.MODEL_CD = T4.MODEL_CD ")
            .Append("                     AND T2.DLR_CD = :DLRCD ")
            .Append("                     AND T2.INUSE_FLG = '1' ")
            .Append("                     OR (T4.DLR_CD = 'XXXXX' ")
            .Append("                     AND T2.DLR_CD = :DLRCD ")
            .Append("                     AND T2.INUSE_FLG = '1' ")
            .Append("                     AND NOT EXISTS (SELECT 1 FROM TB_M_MODEL_DLR T5 WHERE T5.DLR_CD = T2.DLR_CD AND T5.MODEL_CD = T3.MODEL_CD)) ")
            .Append("             ) T8 , ")
            .Append("             TB_M_GRADE T6, ")
            '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("             (SELECT ")
                .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T2.SUFFIX_NAME ")
                .Append("              FROM ")
                .Append("                  TB_T_PREFER_VCL T1, TB_M_SUFFIX T2 ")
                .Append("              WHERE ")
                .Append("                  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
                .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
                .Append("                  AND T2.SUFFIX_CD = T1.SUFFIX_CD ")
            Else
                'サフィックス使用不可の場合
                .Append("             (SELECT ")
                .Append("                  '' AS SUFFIX_NAME ")
                .Append("              FROM ")
                .Append("                  DUAL ")
            End If
            .Append("             ) T23, ")
            .Append("             (SELECT ")
            .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T1.BODYCLR_CD, T2.BODYCLR_NAME ")
            .Append("              FROM ")
            .Append("                  TB_T_PREFER_VCL T1, TB_M_BODYCOLOR T2 ")
            .Append("              WHERE SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
            .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("                  AND (T2.SUFFIX_CD = T1.SUFFIX_CD OR T2.SUFFIX_CD = 'X') ")
            End If
            .Append("                  AND T2.BODYCLR_CD = T1.BODYCLR_CD ")
            .Append("             ) T10, ")
            '内装色使用可の場合
            If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                .Append("             (SELECT ")
                .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T1.BODYCLR_CD, T1.INTERIORCLR_CD, T2.INTERIORCLR_NAME ")
                .Append("              FROM ")
                .Append("                  TB_T_PREFER_VCL T1, TB_M_INTERIORCOLOR T2 ")
                .Append("              WHERE ")
                .Append("                  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
                .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
                'サフィックス使用可の場合
                If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                    .Append("                  AND (T2.SUFFIX_CD = T1.SUFFIX_CD OR T2.SUFFIX_CD = 'X') ")
                End If
                .Append("                  AND (T2.BODYCLR_CD = T1.BODYCLR_CD OR T2.BODYCLR_CD = 'X') ")
                .Append("                  AND T2.INTERIORCLR_CD = T1.INTERIORCLR_CD ")
            Else
                '内装色使用不可の場合
                .Append("             (SELECT ")
                .Append("                  '' AS INTERIORCLR_NAME ")
                .Append("              FROM ")
                .Append("                  DUAL ")
            End If
            .Append("             ) T24 ")
            .Append("         WHERE ")
            .Append("                 T1.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("             AND T1.MODEL_CD = T8.SERIESCD ")
            .Append("             AND T1.MODEL_CD = T6.MODEL_CD(+) ")
            .Append("             AND T1.GRADE_CD = T6.GRADE_CD(+) ")
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("             AND T1.MODEL_CD = T23.MODEL_CD(+) ")
                .Append("             AND T1.GRADE_CD = T23.GRADE_CD(+) ")
                .Append("             AND T1.SUFFIX_CD = T23.SUFFIX_CD(+) ")
            End If
            .Append("             AND T1.MODEL_CD = T10.MODEL_CD(+) ")
            .Append("             AND T1.GRADE_CD = T10.GRADE_CD(+) ")
            .Append("             AND T1.SUFFIX_CD = T10.SUFFIX_CD(+) ")
            .Append("             AND T1.BODYCLR_CD = T10.BODYCLR_CD(+) ")
            '内装色使用可の場合
            If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                .Append("             AND T1.MODEL_CD = T24.MODEL_CD(+) ")
                .Append("             AND T1.GRADE_CD = T24.GRADE_CD(+) ")
                'サフィックス使用可の場合
                If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                    .Append("             AND T1.SUFFIX_CD = T24.SUFFIX_CD(+) ")
                End If
                .Append("             AND T1.BODYCLR_CD = T24.BODYCLR_CD(+) ")
                .Append("             AND T1.INTERIORCLR_CD = T24.INTERIORCLR_CD(+) ")
            End If
            '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            .Append("         ) T9 , ")
            .Append("         TB_M_MODEL T7 , ")
            .Append("         TB_M_KATASHIKI_PICTURE T21 ")
            .Append("     WHERE ")
            .Append("             T9.SERIESCD = T7.MODEL_CD ")
            .Append("         AND T9.MODELCD = T21.VCL_KATASHIKI(+) ")
            .Append("         AND T9.COLORCD = T21.BODYCLR_CD(+) ")
            .Append("     UNION ALL ")
            .Append("     SELECT ")
            .Append("         T19.SERIESCD , ")
            .Append("         T19.SERIESNM , ")
            .Append("         T19.MODELCD , ")
            .Append("         T19.GRADE_NAME AS VCLMODEL_NAME , ")
            .Append("         T19.COLORCD , ")
            .Append("         T19.BODYCLR_NAME AS DISP_BDY_COLOR, ")
            .Append("         TRIM(NVL(T22.VCL_PICTURE,T17.MODEL_PICTURE)) AS PICIMAGE , ")
            .Append("         TRIM(T17.LOGO_PICTURE) AS LOGOIMAGE , ")
            .Append("         T19.QUANTITY , ")
            .Append("         T19.SEQNO, ")
            .Append("         T19.SALES_PROSPECT_CD, ")
            .Append("         T19.ROWLOCKVERSION, ")
            .Append("         T19.SUFFIX_CD, ")
            .Append("         T19.INTERIORCLR_CD, ")
            .Append("         T19.SUFFIX_NAME, ")
            .Append("         T19.INTERIORCLR_NAME ")
            .Append("     FROM ")
            .Append("         ( ")
            .Append("             SELECT DISTINCT ")
            .Append("             T11.MODEL_CD AS SERIESCD , ")
            .Append("             T11.GRADE_CD AS MODELCD , ")
            .Append("             T11.BODYCLR_CD AS COLORCD , ")
            .Append("             T10.BODYCLR_NAME AS BODYCLR_NAME , ")
            .Append("             T18.COMSERIESCD , ")
            .Append("             T18.SERIESNM , ")
            .Append("             T16.GRADE_NAME , ")
            .Append("             T11.PREF_AMOUNT AS QUANTITY , ")
            .Append("             T11.PREF_VCL_SEQ AS SEQNO, ")
            .Append("             T11.SALES_PROSPECT_CD, ")
            .Append("             T11.ROW_LOCK_VERSION AS ROWLOCKVERSION, ")
            .Append("             T11.SUFFIX_CD, ")
            .Append("             T11.INTERIORCLR_CD, ")
            .Append("             T23.SUFFIX_NAME, ")
            .Append("             T24.INTERIORCLR_NAME ")
            .Append("         FROM ")
            .Append("             TB_H_PREFER_VCL T11 , ")
            .Append("             ( ")
            .Append("                 SELECT ")
            .Append("                     T12.DLR_CD AS DLRCD , ")
            .Append("                     T13.MODEL_CD AS SERIESCD , ")
            .Append("                     T13.MODEL_NAME AS SERIESNM , ")
            .Append("                     T13.COMMON_MODEL_CD AS COMSERIESCD ")
            .Append("                 FROM ")
            .Append("                     TB_M_DEALER T12 , ")
            .Append("                     TB_M_MODEL T13 , ")
            .Append("                     TB_M_MODEL_DLR T14 ")
            .Append("                 WHERE ")
            .Append("                     T12.DLR_CD = T14.DLR_CD ")
            .Append("                     AND T13.MODEL_CD = T14.MODEL_CD ")
            .Append("                     AND T12.DLR_CD = :DLRCD ")
            .Append("                     AND T12.INUSE_FLG = '1' ")
            .Append("                     OR (T14.DLR_CD = 'XXXXX' ")
            .Append("                     AND T12.DLR_CD = :DLRCD ")
            .Append("                     AND T12.INUSE_FLG = '1' ")
            .Append("                     AND NOT EXISTS (SELECT 1 FROM TB_M_MODEL_DLR T15 WHERE T15.DLR_CD = T12.DLR_CD AND T15.MODEL_CD = T13.MODEL_CD)) ")
            .Append("             ) T18 , ")
            .Append("             TB_M_GRADE T16, ")
            '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("             (SELECT ")
                .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T2.SUFFIX_NAME ")
                .Append("              FROM ")
                .Append("                  TB_H_PREFER_VCL T1, TB_M_SUFFIX T2 ")
                .Append("              WHERE ")
                .Append("                  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
                .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
                .Append("                  AND T2.SUFFIX_CD = T1.SUFFIX_CD ")
            Else
                'サフィックス使用不可の場合
                .Append("             (SELECT ")
                .Append("                  '' AS SUFFIX_NAME ")
                .Append("              FROM ")
                .Append("                  DUAL ")
            End If
            .Append("             ) T23, ")
            .Append("             (SELECT ")
            .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T1.BODYCLR_CD, T2.BODYCLR_NAME ")
            .Append("              FROM ")
            .Append("                  TB_H_PREFER_VCL T1, TB_M_BODYCOLOR T2 ")
            .Append("              WHERE ")
            .Append("                  SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
            .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("                  AND (T2.SUFFIX_CD = T1.SUFFIX_CD OR T2.SUFFIX_CD = 'X') ")
            End If
            .Append("                  AND T2.BODYCLR_CD = T1.BODYCLR_CD ")
            .Append("             ) T10, ")
            '内装色使用可の場合
            If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                .Append("             (SELECT ")
                .Append("                  T1.MODEL_CD, T1.GRADE_CD, T1.SUFFIX_CD, T1.BODYCLR_CD, T1.INTERIORCLR_CD, T2.INTERIORCLR_NAME ")
                .Append("              FROM ")
                .Append("                  TB_H_PREFER_VCL T1, TB_M_INTERIORCOLOR T2 ")
                .Append("              WHERE ")
                .Append("                  SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("                  AND T2.MODEL_CD = T1.MODEL_CD ")
                .Append("                  AND (T2.GRADE_CD = T1.GRADE_CD OR T2.GRADE_CD = 'X') ")
                'サフィックス使用可の場合
                If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                    .Append("                  AND (T2.SUFFIX_CD = T1.SUFFIX_CD OR T2.SUFFIX_CD = 'X') ")
                End If
                .Append("                  AND (T2.BODYCLR_CD = T1.BODYCLR_CD OR T2.BODYCLR_CD = 'X') ")
                .Append("                  AND T2.INTERIORCLR_CD = T1.INTERIORCLR_CD ")
            Else
                '内装色使不可の場合
                .Append("             (SELECT ")
                .Append("                  '' AS INTERIORCLR_NAME ")
                .Append("              FROM ")
                .Append("                  DUAL ")
            End If
            .Append("             ) T24 ")
            .Append("         WHERE ")
            .Append("                 T11.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("             AND T11.MODEL_CD = T18.SERIESCD ")
            .Append("             AND T11.MODEL_CD = T16.MODEL_CD(+) ")
            .Append("             AND T11.GRADE_CD = T16.GRADE_CD(+) ")
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("             AND T11.MODEL_CD = T23.MODEL_CD(+) ")
                .Append("             AND T11.GRADE_CD = T23.GRADE_CD(+) ")
                .Append("             AND T11.SUFFIX_CD = T23.SUFFIX_CD(+) ")
            End If
            .Append("             AND T11.MODEL_CD = T10.MODEL_CD(+) ")
            .Append("             AND T11.GRADE_CD = T10.GRADE_CD(+) ")
            .Append("             AND T11.SUFFIX_CD = T10.SUFFIX_CD(+) ")
            .Append("             AND T11.BODYCLR_CD = T10.BODYCLR_CD(+) ")
            '内装色使用可の場合
            If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                .Append("             AND T11.MODEL_CD = T24.MODEL_CD(+) ")
                .Append("             AND T11.GRADE_CD = T24.GRADE_CD(+) ")
                'サフィックス使用可の場合
                If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                    .Append("             AND T11.SUFFIX_CD = T24.SUFFIX_CD(+) ")
                End If
                .Append("             AND T11.BODYCLR_CD = T24.BODYCLR_CD(+) ")
                .Append("             AND T11.INTERIORCLR_CD = T24.INTERIORCLR_CD(+) ")
            End If
            '2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            .Append("         ) T19 , ")
            .Append("         TB_M_MODEL T17 , ")
            .Append("         TB_M_KATASHIKI_PICTURE T22 ")
            .Append("     WHERE ")
            .Append("             T19.SERIESCD = T17.MODEL_CD ")
            .Append("         AND T19.MODELCD = T22.VCL_KATASHIKI(+) ")
            .Append("         AND T19.COLORCD = T22.BODYCLR_CD(+) ")
            .Append("     ) T21 ")
            .Append(" ORDER BY ")
            .Append("     T21.SEQNO ")

        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable)("ActivityInfo_307")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedSeries_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 成約車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSuccessSeries(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSuccessSeries_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_308 */ ")
            .Append("  T2.SERIESCD , ")
            .Append("  T2.SERIESNM , ")
            .Append("  T2.MODELCD , ")
            .Append("  T2.MODELNM VCLMODEL_NAME , ")
            .Append("  T2.EXTCOLORCD COLORCD , ")
            .Append("  T2.EXTCOLOR DISP_BDY_COLOR , ")
            .Append("  T2.SUFFIXCD SUFFIX_CD , ")
            .Append("  NVL(T7.SUFFIX_NAME,T8.SUFFIX_NAME) SUFFIX_NAME , ")
            .Append("  T2.INTCOLORCD INTERIORCLR_CD , ")
            .Append("  T2.INTCOLOR INTERIORCLR_NAME , ")
            .Append("  NVL(T5.VCL_PICTURE,T3.MODEL_PICTURE) AS PICIMAGE , ")
            .Append("  T3.LOGO_PICTURE AS LOGOIMAGE , ")
            .Append("  1 QUANTITY , ")
            .Append("  T4.PREF_VCL_SEQ AS SEQNO ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO T1 , ")
            .Append("  TBL_EST_VCLINFO T2 , ")
            .Append("  TB_M_MODEL T3 , ")
            .Append("  TB_T_PREFER_VCL T4 , ")
            .Append("  TB_M_KATASHIKI_PICTURE T5 , ")
            .Append("  TBL_MSTEXTERIOR T6, ")
            .Append("  TB_M_SUFFIX T7, ")
            .Append("  TB_M_SUFFIX T8 ")
            .Append("WHERE ")
            .Append("      T1.ESTIMATEID = T2.ESTIMATEID ")
            .Append("  AND T2.SERIESCD = T3.MODEL_CD ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = T4.SALES_ID ")
            .Append("  AND T2.MODELCD = T6.VCLMODEL_CODE ")
            .Append("  AND T2.EXTCOLORCD = T6.BODYCLR_CD ")
            .Append("  AND T2.SERIESCD = T4.MODEL_CD ")
            .Append("  AND T2.MODELCD = T4.GRADE_CD ")
            .Append("  AND T6.COLOR_CD = T4.BODYCLR_CD ")
            .Append("  AND T6.VCLMODEL_CODE = T5.VCL_KATASHIKI(+) ")
            .Append("  AND T6.COLOR_CD = T5.BODYCLR_CD(+) ")
            .Append("  AND T2.SERIESCD = T7.MODEL_CD(+) ")
            .Append("  AND T2.MODELCD = T7.GRADE_CD(+) ")
            .Append("  AND T2.SUFFIXCD = T7.SUFFIX_CD(+) ")
            .Append("  AND T2.SERIESCD = T8.MODEL_CD(+) ")
            .Append("  AND T8.GRADE_CD(+) = 'X' ")
            .Append("  AND T2.SUFFIXCD = T8.SUFFIX_CD(+) ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("  AND T1.CONTRACTFLG IN ('1','2') ")
            .Append("  AND T1.DELFLG = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("  T2.SERIESCD , ")
            .Append("  T2.SERIESNM , ")
            .Append("  T2.MODELCD , ")
            .Append("  T2.MODELNM VCLMODEL_NAME , ")
            .Append("  T2.EXTCOLORCD COLORCD , ")
            .Append("  T2.EXTCOLOR DISP_BDY_COLOR , ")
            .Append("  T2.SUFFIXCD SUFFIX_CD , ")
            .Append("  NVL(T7.SUFFIX_NAME,T8.SUFFIX_NAME) SUFFIX_NAME , ")
            .Append("  T2.INTCOLORCD INTERIORCLR_CD , ")
            .Append("  T2.INTCOLOR INTERIORCLR_NAME , ")
            .Append("  NVL(T5.VCL_PICTURE,T3.MODEL_PICTURE) AS PICIMAGE , ")
            .Append("  T3.LOGO_PICTURE AS LOGOIMAGE , ")
            .Append("  1 QUANTITY , ")
            .Append("  T4.PREF_VCL_SEQ AS SEQNO ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO T1 , ")
            .Append("  TBL_EST_VCLINFO T2 , ")
            .Append("  TB_M_MODEL T3 , ")
            .Append("  TB_T_PREFER_VCL T4 , ")
            .Append("  TB_M_KATASHIKI_PICTURE T5, ")
            .Append("  TB_M_SUFFIX T7, ")
            .Append("  TB_M_SUFFIX T8 ")
            .Append("WHERE ")
            .Append("      T1.ESTIMATEID = T2.ESTIMATEID ")
            .Append("  AND T2.SERIESCD = T3.MODEL_CD ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = T4.SALES_ID ")
            .Append("  AND T2.SERIESCD = T4.MODEL_CD ")
            .Append("  AND T2.MODELCD = T4.GRADE_CD ")
            .Append("  AND T2.EXTCOLORCD = T4.BODYCLR_CD ")
            .Append("  AND T2.MODELCD = T5.VCL_KATASHIKI(+) ")
            .Append("  AND T2.EXTCOLORCD = T5.BODYCLR_CD(+) ")
            .Append("  AND T2.SERIESCD = T7.MODEL_CD(+) ")
            .Append("  AND T2.MODELCD = T7.GRADE_CD(+) ")
            .Append("  AND T2.SUFFIXCD = T7.SUFFIX_CD(+) ")
            .Append("  AND T2.SERIESCD = T8.MODEL_CD(+) ")
            .Append("  AND T8.GRADE_CD(+) = 'X' ")
            .Append("  AND T2.SUFFIXCD = T8.SUFFIX_CD(+) ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("  AND T1.CONTRACTFLG IN ('1','2') ")
            .Append("  AND T1.DELFLG = '0' ")
            .Append("  AND T2.EXTCOLORCD = ' ' ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("  T2.SERIESCD , ")
            .Append("  T2.SERIESNM , ")
            .Append("  T2.MODELCD , ")
            .Append("  T2.MODELNM VCLMODEL_NAME , ")
            .Append("  T2.EXTCOLORCD COLORCD , ")
            .Append("  T2.EXTCOLOR DISP_BDY_COLOR , ")
            .Append("  T2.SUFFIXCD SUFFIX_CD , ")
            .Append("  NVL(T7.SUFFIX_NAME,T8.SUFFIX_NAME) SUFFIX_NAME , ")
            .Append("  T2.INTCOLORCD INTERIORCLR_CD , ")
            .Append("  T2.INTCOLOR INTERIORCLR_NAME , ")
            .Append("  NVL(T5.VCL_PICTURE,T3.MODEL_PICTURE) AS PICIMAGE , ")
            .Append("  T3.LOGO_PICTURE AS LOGOIMAGE , ")
            .Append("  1 QUANTITY , ")
            .Append("  T4.PREF_VCL_SEQ AS SEQNO ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO T1 , ")
            .Append("  TBL_EST_VCLINFO T2 , ")
            .Append("  TB_M_MODEL T3 , ")
            .Append("  TB_H_PREFER_VCL T4 , ")
            .Append("  TB_M_KATASHIKI_PICTURE T5 , ")
            .Append("  TBL_MSTEXTERIOR T6, ")
            .Append("  TB_M_SUFFIX T7, ")
            .Append("  TB_M_SUFFIX T8 ")
            .Append("WHERE ")
            .Append("      T1.ESTIMATEID = T2.ESTIMATEID ")
            .Append("  AND T2.SERIESCD = T3.MODEL_CD ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = T4.SALES_ID ")
            .Append("  AND T2.MODELCD = T6.VCLMODEL_CODE ")
            .Append("  AND T2.EXTCOLORCD = T6.BODYCLR_CD ")
            .Append("  AND T2.SERIESCD = T4.MODEL_CD ")
            .Append("  AND T2.MODELCD = T4.GRADE_CD ")
            .Append("  AND T6.COLOR_CD = T4.BODYCLR_CD ")
            .Append("  AND T6.VCLMODEL_CODE = T5.VCL_KATASHIKI(+) ")
            .Append("  AND T6.COLOR_CD = T5.BODYCLR_CD(+) ")
            .Append("  AND T2.SERIESCD = T7.MODEL_CD(+) ")
            .Append("  AND T2.MODELCD = T7.GRADE_CD(+) ")
            .Append("  AND T2.SUFFIXCD = T7.SUFFIX_CD(+) ")
            .Append("  AND T2.SERIESCD = T8.MODEL_CD(+) ")
            .Append("  AND T8.GRADE_CD(+) = 'X' ")
            .Append("  AND T2.SUFFIXCD = T8.SUFFIX_CD(+) ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("  AND T1.CONTRACTFLG IN ('1','2') ")
            .Append("  AND T1.DELFLG = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("  T2.SERIESCD , ")
            .Append("  T2.SERIESNM , ")
            .Append("  T2.MODELCD , ")
            .Append("  T2.MODELNM VCLMODEL_NAME , ")
            .Append("  T2.EXTCOLORCD COLORCD , ")
            .Append("  T2.EXTCOLOR DISP_BDY_COLOR , ")
            .Append("  T2.SUFFIXCD SUFFIX_CD , ")
            .Append("  NVL(T7.SUFFIX_NAME,T8.SUFFIX_NAME) SUFFIX_NAME , ")
            .Append("  T2.INTCOLORCD INTERIORCLR_CD , ")
            .Append("  T2.INTCOLOR INTERIORCLR_NAME , ")
            .Append("  NVL(T5.VCL_PICTURE,T3.MODEL_PICTURE) AS PICIMAGE , ")
            .Append("  T3.LOGO_PICTURE AS LOGOIMAGE , ")
            .Append("  1 QUANTITY , ")
            .Append("  T4.PREF_VCL_SEQ AS SEQNO ")
            .Append("FROM ")
            .Append("  TBL_ESTIMATEINFO T1 , ")
            .Append("  TBL_EST_VCLINFO T2 , ")
            .Append("  TB_M_MODEL T3 , ")
            .Append("  TB_H_PREFER_VCL T4 , ")
            .Append("  TB_M_KATASHIKI_PICTURE T5, ")
            .Append("  TB_M_SUFFIX T7, ")
            .Append("  TB_M_SUFFIX T8 ")
            .Append("WHERE ")
            .Append("      T1.ESTIMATEID = T2.ESTIMATEID ")
            .Append("  AND T2.SERIESCD = T3.MODEL_CD ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = T4.SALES_ID ")
            .Append("  AND T2.SERIESCD = T4.MODEL_CD ")
            .Append("  AND T2.MODELCD = T4.GRADE_CD ")
            .Append("  AND T2.EXTCOLORCD = T4.BODYCLR_CD ")
            .Append("  AND T2.MODELCD = T5.VCL_KATASHIKI(+) ")
            .Append("  AND T2.EXTCOLORCD = T5.BODYCLR_CD(+) ")
            .Append("  AND T2.SERIESCD = T7.MODEL_CD(+) ")
            .Append("  AND T2.MODELCD = T7.GRADE_CD(+) ")
            .Append("  AND T2.SUFFIXCD = T7.SUFFIX_CD(+) ")
            .Append("  AND T2.SERIESCD = T8.MODEL_CD(+) ")
            .Append("  AND T8.GRADE_CD(+) = 'X' ")
            .Append("  AND T2.SUFFIXCD = T8.SUFFIX_CD(+) ")
            .Append("  AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("  AND T1.CONTRACTFLG IN ('1','2') ")
            .Append("  AND T1.DELFLG = '0' ")
            .Append("  AND T2.EXTCOLORCD = ' ' ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable)("ActivityInfo_308")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSuccessSeries_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注後活動未実施件数取得
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>受注後活動未実施件数</returns>
    ''' <remarks></remarks>
    Public Shared Function CountUnexecutedAfterOdrAct(ByVal salesid As Decimal) As Integer

        'ログ出力 Start ***************************************************************************
        Logger.Info("CountUnexecutedAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT /* ActivityInfo_009 */ ")
            .AppendLine("     1 ")
            .AppendLine("FROM TB_T_AFTER_ODR T1 ")
            .AppendLine("   , TB_T_AFTER_ODR_ACT T2 ")
            .AppendLine("   , TB_M_AFTER_ODR_ACT T3 ")
            .AppendLine("WHERE T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
            .AppendLine("  AND T2.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD ")
            .AppendLine("  AND T1.SALES_ID = :SALES_ID ")
            .AppendLine("  AND T2.AFTER_ODR_ACT_STATUS <> 1 ")
            .AppendLine("  AND T3.MANDATORY_ACT_FLG = '1' ")
        End With

        Dim cnt As Integer = 0

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_009")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            cnt = query.GetCount()
        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Info("CountUnexecutedAfterOdrAct_End")
        'ログ出力 End *****************************************************************************

        Return cnt

    End Function

    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
    ''' <summary>
    ''' 受注後活動存在チェック
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>True(する)/False(しない)</returns>
    ''' <remarks></remarks>
    Public Shared Function IsExistsAfterOdr(ByVal salesid As Decimal) As Boolean
        'ログ出力 Start ***************************************************************************
        Logger.Info("IsExistsAfterOdr_Start")
        'ログ出力 End *****************************************************************************
        Dim result As Boolean = False
        Dim sql As New StringBuilder
        sql.Append("SELECT /* ActivityInfo_012 */ 1 FROM TB_T_AFTER_ODR T1 ")
        sql.Append("WHERE T1.SALES_ID = :SALES_ID ")
        sql.Append("AND EXISTS(SELECT 1 FROM TB_T_AFTER_ODR_ACT T2 WHERE T1.AFTER_ODR_ID = T2.AFTER_ODR_ID) ")
        sql.Append("UNION ALL ")
        sql.Append("SELECT 1 FROM TB_H_AFTER_ODR T3 ")
        sql.Append("WHERE T3.SALES_ID = :SALES_ID ")
        sql.Append("AND EXISTS(SELECT 1 FROM TB_H_AFTER_ODR_ACT T4 WHERE T3.AFTER_ODR_ID = T4.AFTER_ODR_ID) ")

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            result = (query.GetCount() > 0)
        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Info("IsExistsAfterOdr_End")
        'ログ出力 End *****************************************************************************

        Return result
    End Function
    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END

    '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 契約日取得
    ''' </summary>
    ''' <param name="seqno">Follow-up Box内連番</param>
    ''' <returns>契約日</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractDate(ByVal seqno As Decimal) As ActivityInfoDataSet.ActivityInfoGetContractDateDataTable

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetContractDateDataTable)("ActivityInfo_010")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractDate_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT /* ActivityInfo_010 */ ")
                .Append("     CONTRACTDATE")
                .Append("  FROM TBL_ESTIMATEINFO")
                .Append(" WHERE ")
                .Append("   FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                .Append("   AND DELFLG = '0' ")
                .Append("   AND CONTRACTFLG = '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractDate_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' キャンセル区分取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesbkgno">受注No</param>
    ''' <returns>データセット</returns>
    ''' <remarks>キャンセル区分を取得する</remarks>
    Public Shared Function GetSalesCancel(ByVal dlrcd As String,
                                            ByVal salesbkgno As String) As ActivityInfoDataSet.ActivityInfoGetCancelStatusDataTable
        Dim sql As New StringBuilder
        With sql
            '2014/02/12 TCS 山口 受注後フォロー機能開発 START
            .Append("SELECT /* ActivityInfo_011 */ ")
            .Append("       T1.CANCEL_FLG    ")
            .Append("  FROM TB_T_SALESBOOKING T1 ")
            .Append(" WHERE T1.DLR_CD = :DLRCD ")
            .Append("   AND T1.SALESBKG_NUM = :SALESBKGNO ")
            '2014/02/12 TCS 山口 受注後フォロー機能開発 END
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetCancelStatusDataTable)("ActivityInfo_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                       '販売店コード
            query.AddParameterWithTypeValue("SALESBKGNO", OracleDbType.Char, salesbkgno)             '受注No
            Return query.GetData()
        End Using

    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 083. 最大の活動終了時間を取得 
    ''' </summary>
    ''' <param name="fllwupboxSeqNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetLatestActTimeEnd(ByVal fllwupboxSeqNo As Decimal) As ActivityInfoDataSet.ActivityInfoLatestActTimeDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLatestActTimeEnd_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_312 */ ")
            .Append("  MAX(LATEST_TIME_END) LATEST_TIME_END ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    T1.RSLT_DATETIME AS LATEST_TIME_END ")
            .Append("  FROM ")
            .Append("    TB_T_ACTIVITY T1 , ")
            .Append("    TB_T_SALES T2 , ")
            .Append("    TB_T_SALES_ACT T3 ")
            .Append("  WHERE ")
            .Append("        T1.ACT_ID = T3.ACT_ID ")
            .Append("    AND T2.SALES_ID = T3.SALES_ID ")
            .Append("    AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("    AND T1.ACT_ID = T3.ACT_ID ")
            .Append("    AND T2.SALES_ID = T3.SALES_ID ")
            .Append("  UNION ALL ")
            .Append("    SELECT ")
            .Append("      SALESENDTIME AS LATEST_TIME_END ")
            .Append("    FROM ")
            .Append("      TBL_BOOKEDAFTERFOLLOWRSLT ")
            .Append("    WHERE ")
            .Append("      FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("  ) ")

        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoLatestActTimeDataTable)("ActivityInfo_312")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxSeqNo)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLatestActTimeEnd_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 担当スタッフ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="account">アカウントコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStaff(ByVal dlrcd As String, ByVal strcd As String, ByVal account As String) As ActivityInfoDataSet.ActivityInfoUsersDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoUsersDataTable)("ActivityInfo_000")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* ActivityInfo_000 */ ")
                .Append("    A.USERNAME, ")
                .Append("    REPLACE(A.ACCOUNT ,'@' || :DLRCD ,'') AS ACCOUNT ")
                .Append("FROM ")
                .Append("    TBL_USERS A, ")
                .Append("    TBL_USERDISPLAY B ")
                .Append("WHERE ")
                .Append("    A.DLRCD = :DLRCD AND ")
                .Append("    A.STRCD = :STRCD AND ")
                .Append("    A.ACCOUNT = :ACCOUNT AND ")
                .Append("    A.OPERATIONCODE = '8' AND ")
                .Append("    A.DELFLG = '0' AND ")
                .Append("    B.ACCOUNT(+) = A.ACCOUNT ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
            Return query.GetData()
        End Using
    End Function

    ' 2012/02/29 TCS 安田 【SALES_2】 START
    ''' <summary>
    ''' 活動中リスト取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="insdid">未取引客ID／自社客連番</param>
    ''' <param name="cstkind">未取引客:2／自社客種別:1</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesActiveList(ByVal dlrcd As String,
                                       ByVal strcd As String,
                                       ByVal insdid As String,
                                       ByVal cstkind As String,
                                       ByVal newcustid As String) As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT FLLWUPBOX_SEQNO, NEWFLLWUPBOXFLG, REGISTFLG ")
            .Append("FROM TBL_FLLWUPBOX_SALES ")
            .Append("WHERE DLRCD = :DLRCD ")
            .Append("  AND TRIM(STRCD) = :STRCD ")
            .Append("  AND NEWFLLWUPBOXFLG = '1' ")
            If "1".Equals(cstkind) Then
                If String.IsNullOrEmpty(newcustid) Then
                    .Append("    AND CRCUSTID = :INSDID ")
                Else
                    .Append("    AND ( CRCUSTID = :INSDID OR CRCUSTID = :NEWCUSTID ) ")
                End If
            Else
                .Append("    AND CRCUSTID = :INSDID ")
            End If
            .Append("    ORDER BY ")
            .Append("        CREATEDATE DESC ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable)("SC3080202_001")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)              '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)              '店舗コード
            query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)            '内部管理ID

            If Not (String.IsNullOrEmpty(newcustid)) And "1".Equals(cstkind) Then
                query.AddParameterWithTypeValue("NEWCUSTID", OracleDbType.Char, newcustid)            '未取引客ID
            Else
            End If
            Return query.GetData()
        End Using
    End Function
    ' 2012/02/29 TCS 安田 【SALES_2】 END

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客個人情報取得
    ''' </summary>
    ''' <param name="custId">未取引客ユーザーID</param>
    ''' <returns>GetNewCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomer(ByVal custId As String) As ActivityInfoDataSet.GetNewCustomerDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.GetNewCustomerDataTable)("ActivityInfo_188")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_188 */ ")
                .Append("  CST_NAME AS NAME , ")
                .Append("  CST_PHONE AS TELNO , ")
                .Append("  CST_MOBILE AS MOBILE , ")
                .Append("  CST_REG_STATUS AS DUMMYNAMEFLG ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :CSTID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, custId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2013/03/06 TCS 河原 GL0874 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 契約状況フラグの取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-upBox内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal dlrcd As String,
                                          ByVal strcd As String,
                                          ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoContractFlgDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractFlg_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* ActivityInfo_013 */ ")
            .Append("    A.CONTRACTFLG ")
            .Append("FROM ")
            .Append("    TBL_ESTIMATEINFO A ")
            .Append("WHERE ")
            .Append("       A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append(" AND   A.DELFLG = '0' ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContractFlgDataTable)("ActivityInfo_013")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractFlg_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using

    End Function
    '2013/03/06 TCS 河原 GL0874 END

    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 DEL START
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 DEL END

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定実績情報取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="isForUpdateLock">レコードロックの有無</param>
    ''' <returns>下取り査定情報のデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetActAsmInfo(ByVal fllwupboxseqno As Decimal, ByVal isForUpdateLock As Boolean
                                         ) As ActivityInfoDataSet.ActivityInfoActAsmInfoDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoActAsmInfoDataTable)("ActivityInfo_013")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActAsmInfo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT /* ActivityInfo_013 */ ")
                .Append("       ASM.ASM_ANSWERED_FLG ")
                .Append("     , ASM.ASSESSMENTNO ")
                .Append("     , NOTICE_REQ.STATUS ")
                .Append("  FROM TBL_UCARASSESSMENT ASM ")
                .Append("     , TBL_NOTICEREQUEST NOTICE_REQ ")
                .Append(" WHERE ASM.NOTICEREQID = NOTICE_REQ.NOTICEREQID ")
                .Append("   AND ASM.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("   AND ASM.ASM_ACT_FLG = '1' ")
                If isForUpdateLock Then
                    .Append("   FOR UPDATE ")
                End If
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActAsmInfo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積車種取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns>見積車種のデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateCar(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActivityInfoEstimateCarDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoEstimateCarDataTable)("ActivityInfo_314")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateCar_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_314 */ ")
                .Append("  T5.PREF_VCL_SEQ AS SEQNO , ")
                .Append("  T4.SERIESCD , ")
                .Append("  T4.SERIESNM , ")
                .Append("  T4.MODELCD , ")
                .Append("  T4.MODELNM AS VCLMODEL_NAME , ")
                .Append("  T4.EXTCOLORCD AS EXTCOLORCD , ")
                .Append("  T4.EXTCOLOR AS DISP_BDY_COLOR , ")
                .Append("  T4.ESTIMATEID , ")
                .Append("  T4.EST_ACT_FLG , ")
                .Append("  ' ' AS DISPLAY_PRICE, ")
                .Append("  TO_NUMBER(NVL2(T5.PREF_VCL_SEQ,T5.PREF_VCL_SEQ,T4.ESTIMATEID)) AS KEYVALUE , ")
                .Append("  NVL2(T5.PREF_VCL_SEQ,'1','0') AS IS_EXISTS_SELECTED_SERIES , ")
                .Append("  '1' AS IS_EXISTS_ESTIMATE ")
                .Append("FROM ")
                .Append("  ( ")
                '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
                .Append("  SELECT /*+ USE_NL(T6) INDEX(T6 PK_MSTEXTERIOR)*/ ")
                '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
                .Append("    T1.ESTIMATEID , ")
                .Append("    T1.DLRCD , ")
                .Append("    T1.STRCD , ")
                .Append("    T1.FLLWUPBOX_SEQNO , ")
                .Append("    T2.SERIESCD AS CAR_NAME_CD_AI21 , ")
                .Append("    T2.SERIESCD , ")
                .Append("    T2.MODELNM AS SERIESNM , ")
                .Append("    T2.MODELCD , ")
                .Append("    T2.MODELNM , ")
                .Append("    T2.EXTCOLORCD , ")
                .Append("    T2.EXTCOLOR , ")
                .Append("    T1.EST_ACT_FLG, ")
                .Append("    NVL(T6.COLOR_CD,' ') AS COLOR_CD")
                .Append("  FROM ")
                .Append("    TBL_ESTIMATEINFO T1 , ")
                .Append("    TBL_EST_VCLINFO T2 ,  ")
                .Append("    TBL_MSTEXTERIOR T6  ")
                .Append("  WHERE ")
                .Append("        T2.ESTIMATEID = T1.ESTIMATEID ")
                .Append("    AND T2.MODELCD = T6.VCLMODEL_CODE(+) ")
                .Append("    AND T2.EXTCOLORCD = T6.BODYCLR_CD(+) ")
                .Append("    AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("    AND T1.DELFLG = '0' ")
                .Append("  ) T4 , ")
                .Append("  TB_T_PREFER_VCL T5 ")
                .Append("WHERE ")
                .Append("      T5.SALES_ID(+) = T4.FLLWUPBOX_SEQNO ")
                .Append("  AND T5.MODEL_CD(+) = T4.SERIESCD ")
                .Append("  AND T5.GRADE_CD(+) = T4.MODELCD ")
                .Append("  AND T5.BODYCLR_CD(+) = T4.COLOR_CD ")
                .Append("ORDER BY ")
                .Append("  T4.ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateCar_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種の既存シーケンス取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="salesHisFlg">商談Histroyフラグ</param> 
    ''' <returns>Follow-up Box選択車種情報シーケンスNoのデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetExistSeqSelectedSeries(ByVal estimateId As Long, _
                                                     ByVal salesHisFlg As Boolean, _
                                                     ByVal useFlgSuffix As String, _
                                                     ByVal useFlgInteriorClr As String) As ActivityInfoDataSet.ActivityInfoExistSeqSelectedSeriesDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoExistSeqSelectedSeriesDataTable)("ActivityInfo_315")
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            Dim env As New SystemEnvSetting
            ' 外版色コードを前3桁だけで比較するか否かフラグ
            Dim extColor3Flg As String = String.Empty
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
            If IsNothing(sysEnvRow) Then
                '取得できなかった場合、"0"を設定
                extColor3Flg = "0"
            Else
                extColor3Flg = sysEnvRow.PARAMVALUE
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetExistSeqSelectedSeries_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_315 */ ")
                .Append("  T3.PREF_VCL_SEQ AS SEQNO ")
                .Append("FROM ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    T1.DLRCD , ")
                .Append("    T1.STRCD , ")
                .Append("    T1.FLLWUPBOX_SEQNO , ")
                .Append("    T2.MODELCD , ")
                .Append("    T2.SUFFIXCD, ")
                .Append("    T2.EXTCOLORCD, ")
                .Append("    T2.INTCOLORCD ")
                .Append("  FROM ")
                .Append("    TBL_ESTIMATEINFO T1 , ")
                .Append("    TBL_EST_VCLINFO T2 ")
                .Append("  WHERE ")
                .Append("        T1.ESTIMATEID = :ESTIMATEID ")
                .Append("    AND T1.DELFLG = '0' ")
                .Append("    AND T2.ESTIMATEID = T1.ESTIMATEID ")
                .Append("  ) T4 , ")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                If (salesHisFlg = True) Then
                    .Append("  TB_H_PREFER_VCL T3 ")
                Else
                    .Append("  TB_T_PREFER_VCL T3 ")
                End If
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                .Append("WHERE ")
                .Append("      T3.SALES_ID = T4.FLLWUPBOX_SEQNO ")
                .Append("  AND T3.GRADE_CD = T4.MODELCD ")
                ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                    .Append("  AND T3.SUFFIX_CD = T4.SUFFIXCD ")
                End If
                ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
                If (extColor3Flg = "1") Then
                    .Append("  AND T3.BODYCLR_CD = SUBSTR(T4.EXTCOLORCD,1,3) ")
                Else
                    .Append("  AND T3.BODYCLR_CD = T4.EXTCOLORCD ")
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                    .Append("  AND T3.INTERIORCLR_CD = T4.INTCOLORCD ")
                End If
                ' 2018/04/23 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetExistSeqSelectedSeries_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 商談がHistoryテーブルに移行されているかチェックする
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns>Follow-up Box選択車種情報シーケンスNoのデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckSalesHistory(ByVal salesid As Long) As Boolean

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckSalesHistory_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* ActivityInfo_318 */ ")
            .Append("  COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("  TB_H_SALES ")
            .Append("WHERE ")
            .Append("  SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCountDataTable)("ActivityInfo_318")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Int64, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CheckSalesHistory_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

            Return (query.GetCount() > 0)
        End Using

    End Function
    ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車種の新規シーケンス取得
    ''' </summary>
    ''' <param name="salesHisFlg">商談Histroyフラグ</param> 
    ''' <returns>Follow-up Box選択車種情報シーケンスNoのデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewSeqSelectedSeries(ByVal salesid As Decimal, _
                                                   ByVal salesHisFlg As Boolean) As ActivityInfoDataSet.ActivityInfoNewSeqSelectedSeriesDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoNewSeqSelectedSeriesDataTable)("ActivityInfo_316")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewSeqSelectedSeries_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_316 */ ")
                .Append("  MAX(PREF_VCL_SEQ)+1 AS SEQNO ")
                .Append("FROM ")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                If (salesHisFlg = True) Then
                    .Append("  TB_H_PREFER_VCL ")
                Else
                    .Append("  TB_T_PREFER_VCL ")
                End If
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                .Append("WHERE ")
                .Append("  SALES_ID = :SALESID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewSeqSelectedSeries_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積車両情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積車両情報のデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstVclInfo(ByVal estimateId As Long) As ActivityInfoDataSet.ActivityInfoEstVclInfoDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoEstVclInfoDataTable)("ActivityInfo_317")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstVclInfo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* ActivityInfo_317 */ ")
                .Append("  T1.DLRCD AS DLRCD , ")
                .Append("  T1.STRCD AS STRCD , ")
                .Append("  T1.FLLWUPBOX_SEQNO AS FLLWUPBOX_SEQNO , ")
                .Append("  T4.MODEL_CD AS SERIESCD , ")
                .Append("  T2.MODELCD AS MODELCD , ")
                .Append("  T2.SUFFIXCD AS SUFFIXCD , ")
                .Append("  T2.EXTCOLORCD AS EXTCOLORCD, ")
                .Append("  T2.INTCOLORCD AS INTCOLORCD ")
                .Append("FROM ")
                .Append("  TBL_ESTIMATEINFO T1 , ")
                .Append("  TBL_EST_VCLINFO T2 , ")
                .Append("  TB_M_DEALER T3 , ")
                .Append("  TB_M_MODEL T4 , ")
                .Append("  TB_M_MODEL_DLR T5 ")
                .Append("WHERE ")
                .Append("      T4.MODEL_CD = T5.MODEL_CD ")
                .Append("  AND T2.SERIESCD = T5.MODEL_CD ")
                .Append("  AND T1.ESTIMATEID = :ESTIMATEID ")
                .Append("  AND T1.DELFLG = '0' ")
                .Append("  AND T2.ESTIMATEID = T1.ESTIMATEID ")
                .Append("  AND T3.DLR_CD = T1.DLRCD ")
                .Append("  AND T3.INUSE_FLG = '1' ")
                .Append("  AND (T3.DLR_CD = T5.DLR_CD ")
                .Append("  OR  (T5.DLR_CD = 'XXXXX' ")
                .Append("  AND NOT EXISTS ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("      1 ")
                .Append("    FROM ")
                .Append("      TB_M_MODEL T6 , ")
                .Append("      TB_M_MODEL_DLR T7 ")
                .Append("    WHERE ")
                .Append("          T6.MODEL_CD = T7.MODEL_CD ")
                .Append("      AND T7.DLR_CD = T3.DLR_CD ")
                .Append("      AND T6.MODEL_CD = T5.MODEL_CD ")
                .Append("    ))) ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstVclInfo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '更新： 2017/05/11 TCS 河原  TR-SLT-TMT-20161020-001 START
    ''' <summary>
    ''' 未存在希望車種の登録
    ''' </summary>
    ''' <param name="rwEstVclInfo">見積車両情報のデータセットのレコード</param>
    ''' <param name="seqno">Follow-up Box選択車種情報シーケンスNo</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="salesHisFlg">History済みフラグ</param>
    ''' <returns>登録件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertNotRegSelectedSeries(ByVal rwEstVclInfo As ActivityInfoDataSet.ActivityInfoEstVclInfoRow, _
                                                      ByVal seqno As Long, _
                                                      ByVal updateAccount As String, _
                                                      ByVal salesHisFlg As Boolean, _
                                                      ByVal mostPerfCd As String) As Integer
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
        Dim env As New SystemEnvSetting
        ' 外版色コードを前3桁だけで比較するか否かフラグ
        Dim extColor3Flg As String = String.Empty
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            extColor3Flg = "0"
        Else
            extColor3Flg = sysEnvRow.PARAMVALUE
        End If
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

        Dim sql As New StringBuilder
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNotRegSelectedSeries_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append(" INSERT /* ActivityInfo_170 */ ")
            ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            If (salesHisFlg = True) Then
                .Append("   INTO TB_H_PREFER_VCL ")
            Else
                .Append("   INTO TB_T_PREFER_VCL ")
            End If
            ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            .Append("      ( ")
            .Append("        SALES_ID ")
            .Append("      , PREF_VCL_SEQ ")
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .Append("      , SALES_STATUS ")
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .Append("      , MODEL_CD ")
            .Append("      , GRADE_CD ")
            .Append("      , SUFFIX_CD ")
            .Append("      , BODYCLR_CD ")
            .Append("      , INTERIORCLR_CD ")
            .Append("      , PREF_AMOUNT ")
            .Append("      , EST_RSLT_FLG ")
            .Append("      , SALESBKG_NUM ")
            .Append("      , ROW_CREATE_DATETIME ")
            .Append("      , ROW_CREATE_ACCOUNT ")
            .Append("      , ROW_CREATE_FUNCTION ")
            .Append("      , ROW_UPDATE_DATETIME ")
            .Append("      , ROW_UPDATE_ACCOUNT ")
            .Append("      , ROW_UPDATE_FUNCTION ")
            .Append("      , ROW_LOCK_VERSION ")
            .Append("      , SALES_PROSPECT_CD ")
            .Append("      ) ")
            .Append(" VALUES ")
            .Append("      ( ")
            .Append("        :SALES_ID ")
            .Append("      , :PREF_VCL_SEQ ")
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .Append("      , '21'")
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .Append("      , :MODEL_CD ")
            .Append("      , :GRADE_CD ")
            .Append("      , :SUFFIX_CD ")
            .Append("      , :BODYCLR_CD ")
            .Append("      , :INTERIORCLR_CD ")
            .Append("      , 1 ")
            .Append("      , '0' ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .Append("      , ' ' ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .Append("      , SYSDATE ")
            .Append("      , :INSERTACCOUNT ")
            .Append("      , 'SC3080203' ")
            .Append("      , SYSDATE ")
            .Append("      , :UPDATEACCOUNT ")
            .Append("      , 'SC3080203' ")
            .Append("      , 0 ")
            .Append("      , :SALES_PROSPECT_CD ")
            .Append("      ) ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_170")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, rwEstVclInfo.FLLWUPBOX_SEQNO)
            query.AddParameterWithTypeValue("PREF_VCL_SEQ", OracleDbType.Long, seqno)
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, rwEstVclInfo.SERIESCD)
            query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.Varchar2, rwEstVclInfo.MODELCD)
            query.AddParameterWithTypeValue("SUFFIX_CD", OracleDbType.Varchar2, rwEstVclInfo.SUFFIXCD)
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            If (extColor3Flg = "1") Then
                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, Left(rwEstVclInfo.EXTCOLORCD, 3))
            Else
                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, rwEstVclInfo.EXTCOLORCD)
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
            query.AddParameterWithTypeValue("INTERIORCLR_CD", OracleDbType.Varchar2, rwEstVclInfo.INTCOLORCD)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, rwEstVclInfo.EXTCOLORCD)
            query.AddParameterWithTypeValue("INSERTACCOUNT", OracleDbType.Varchar2, updateAccount)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)

            query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.Varchar2, mostPerfCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNotRegSelectedSeries_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 商談見込み度コードクリア
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">アカウント</param>
    ''' <returns>処理結果（True:クリア成功/False:クリア失敗）</returns>
    ''' <remarks>対象商談の全希望車種の商談見込み度コードを倒します。</remarks>
    Public Shared Function ClearSalesProspectCd(ByVal salesId As Decimal, ByVal updateAccount As String, ByVal salesHisFlg As Boolean, ByVal mostPerfCd As String) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ClearSalesProspectCd_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* SC3080202_206 */ ")
                If (salesHisFlg = True) Then
                    .AppendLine("    TB_H_PREFER_VCL ")
                Else
                    .AppendLine("    TB_T_PREFER_VCL ")
                End If
                .AppendLine("SET  ")
                .AppendLine("    SALES_PROSPECT_CD = ' ', ")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE,  ")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT,  ")
                .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3070207',  ")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1  ")
                .AppendLine("WHERE  ")
                .AppendLine("        SALES_ID = :SALES_ID ")
                .AppendLine("    AND SALES_PROSPECT_CD = :SALES_PROSPECT_CD ")
            End With

            Using query As New DBUpdateQuery("SC3080202_206")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("SALES_PROSPECT_CD", OracleDbType.NVarchar2, mostPerfCd)

                ret = query.Execute()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ClearSalesProspectCd_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function
    '更新： 2017/05/11 TCS 河原  TR-SLT-TMT-20161020-001 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定実績フラグの更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateActAsmFlg(ByVal fllwupboxseqno As Decimal, ByVal updateAccount As String, _
                                                    ByVal updateId As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActAsmFlg_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("UPDATE /* ActivityInfo_019 */ ")
            .Append("       TBL_UCARASSESSMENT ")
            .Append("   SET ASM_ACT_FLG = '0' ")
            '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
            .Append("     , ASM_ANSWERED_FLG = '0' ")
            '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END
            .Append("     , UPDATEID = :UPDATEID ")
            .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .Append("     , UPDATEDATE = SYSDATE ")
            .Append(" WHERE  ")
            .Append("   FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("   AND ASM_ACT_FLG = '1' ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_019")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActAsmFlg_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 見積実績フラグの更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateActEstFlg(ByVal fllwupboxseqno As Decimal, ByVal updateAccount As String, _
                                                ByVal updateId As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActEstFlg_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("UPDATE /* ActivityInfo_020 */ ")
            .Append("       TBL_ESTIMATEINFO ")
            .Append("   SET EST_ACT_FLG = '0' ")
            .Append("     , UPDATEID = :UPDATEID ")
            .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .Append("     , UPDATEDATE = SYSDATE ")
            .Append(" WHERE  ")
            .Append("   FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("   AND EST_ACT_FLG = '1' ")
            .Append("   AND DELFLG = '0' ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_020")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActEstFlg_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function
    '2013/01/16 TCS 坪根 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定依頼中活動履歴の登録
    ''' </summary>
    ''' <param name="assessmentNo"></param>
    ''' <param name="dlrcd"></param>
    ''' <param name="strcd"></param>
    ''' <param name="fllwupboxseqno"></param>
    ''' <param name="actid"></param>
    ''' <param name="crplanid"></param>
    ''' <param name="bfafdvs"></param>
    ''' <param name="crdvsid"></param>
    ''' <param name="insdid"></param>
    ''' <param name="seriescode"></param>
    ''' <param name="seriesname"></param>
    ''' <param name="account"></param>
    ''' <param name="regno"></param>
    ''' <param name="subctgcode"></param>
    ''' <param name="servicecd"></param>
    ''' <param name="subctgorgname"></param>
    ''' <param name="subctgorgnameex"></param>
    ''' <param name="promotionid"></param>
    ''' <param name="activityresult"></param>
    ''' <param name="plandvs"></param>
    ''' <param name="actdate"></param>
    ''' <param name="method"></param>
    ''' <param name="action"></param>
    ''' <param name="actiontype"></param>
    ''' <param name="brnchaccount"></param>
    ''' <param name="actioncd"></param>
    ''' <param name="ctntseqno"></param>
    ''' <param name="selectseriesseqno"></param>
    ''' <param name="seriesnm"></param>
    ''' <param name="vclmodelname"></param>
    ''' <param name="dispbdycolor"></param>
    ''' <param name="quantity"></param>
    ''' <param name="fllwupboxrsltseqno"></param>
    ''' <param name="categoryid"></param>
    ''' <param name="categorydvsid"></param>
    ''' <param name="vin"></param>
    ''' <param name="accountnm"></param>
    ''' <param name="crcustid"></param>
    ''' <param name="customerclass"></param>
    ''' <returns>登録件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertFllwupBoxCrHisAsm(ByVal assessmentNo As Long, ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal, ByVal actid As Decimal,
                                                   ByVal crplanid As Nullable(Of Long), ByVal bfafdvs As String, ByVal crdvsid As Long, ByVal insdid As String,
                                                   ByVal seriescode As String, ByVal seriesname As String, ByVal account As String, ByVal regno As String,
                                                   ByVal subctgcode As String, ByVal servicecd As String, ByVal subctgorgname As String,
                                                   ByVal subctgorgnameex As String, ByVal promotionid As Nullable(Of Long), ByVal activityresult As String,
                                                   ByVal plandvs As String, ByVal actdate As Date, ByVal method As String, ByVal action As String,
                                                   ByVal actiontype As String, ByVal brnchaccount As String, ByVal actioncd As String, ByVal ctntseqno As Long,
                                                   ByVal selectseriesseqno As Long, ByVal seriesnm As String, ByVal vclmodelname As String,
                                                   ByVal dispbdycolor As String, ByVal quantity As Long, ByVal fllwupboxrsltseqno As Decimal,
                                                   ByVal categoryid As String, ByVal categorydvsid As Nullable(Of Long), ByVal vin As String,
                                                   ByVal accountnm As String, ByVal crcustid As String, ByVal customerclass As String
                                                   ) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupBoxCrHisAsm_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("INSERT /* ActivityInfo_021 */ ")
            .Append("INTO TBL_FLLWUPBOXCRHIS_ASM ")
            .Append("( ")
            .Append("     ASSESSMENTNO ")
            .Append("    ,ASMREQCRHIS_SEQNO ")
            .Append("    ,DLRCD ")
            .Append("    ,STRCD ")
            .Append("    ,FLLWUPBOX_SEQNO ")
            .Append("    ,ACT_ID ")
            .Append("    ,CRPLAN_ID ")
            .Append("    ,BFAFDVS ")
            .Append("    ,CRDVSID ")
            .Append("    ,INSDID ")
            .Append("    ,SERIESCODE ")
            .Append("    ,SERIESNAME ")
            .Append("    ,ACCOUNT ")
            .Append("    ,REGNO ")
            .Append("    ,SUBCTGCODE ")
            .Append("    ,SERVICECD ")
            .Append("    ,SUBCTGORGNAME ")
            .Append("    ,SUBCTGORGNAME_EX ")
            .Append("    ,PROMOTION_ID ")
            .Append("    ,ACTIVITYRESULT ")
            .Append("    ,PLANDVS ")
            .Append("    ,ACTDATE ")
            .Append("    ,METHOD ")
            .Append("    ,ACTION ")
            .Append("    ,ACTIONTYPE ")
            .Append("    ,BRNCHACCOUNT ")
            .Append("    ,ACTIONCD ")
            .Append("    ,CTNTSEQNO ")
            .Append("    ,SELECT_SERIES_SEQNO ")
            .Append("    ,SERIESNM ")
            .Append("    ,VCLMODEL_NAME ")
            .Append("    ,DISP_BDY_COLOR ")
            .Append("    ,QUANTITY ")
            .Append("    ,FLLWUPBOXRSLT_SEQNO ")
            .Append("    ,CATEGORYID ")
            .Append("    ,CATEGORYDVSID ")
            .Append("    ,VIN ")
            .Append("    ,ACCOUNT_NM ")
            .Append("    ,CRCUSTID ")
            .Append("    ,CUSTOMERCLASS ")
            .Append("    ,CREATEDATE ")
            .Append("    ,UPDATEDATE ")
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("     :ASSESSMENTNO ")
            .Append("    ,(SELECT NVL(MAX(ASMREQCRHIS_SEQNO),0) + 1 ")
            .Append("        FROM TBL_FLLWUPBOXCRHIS_ASM ")
            .Append("       WHERE ASSESSMENTNO = :ASSESSMENTNO ")
            .Append("     ) ")
            .Append("    ,:DLRCD ")
            .Append("    ,:STRCD ")
            .Append("    ,:FLLWUPBOX_SEQNO ")
            .Append("    ,:ACTID ")
            .Append("    ,:CRPLAN_ID ")
            .Append("    ,:BFAFDVS ")
            .Append("    ,:CRDVSID ")
            .Append("    ,:INSDID ")
            .Append("    ,:SERIESCODE ")
            .Append("    ,:SERIESNAME ")
            .Append("    ,:ACCOUNT ")
            .Append("    ,:REGNO ")
            .Append("    ,:SUBCTGCODE ")
            .Append("    ,:SERVICECD ")
            .Append("    ,:SUBCTGORGNAME ")
            .Append("    ,:SUBCTGORGNAME_EX ")
            .Append("    ,:PROMOTION_ID ")
            .Append("    ,:ACTIVITYRESULT ")
            .Append("    ,:PLANDVS ")
            .Append("    ,:ACTDATE ")
            .Append("    ,:METHOD ")
            .Append("    ,:ACTION ")
            .Append("    ,:ACTIONTYPE ")
            .Append("    ,:BRNCHACCOUNT ")
            .Append("    ,:ACTIONCD ")
            .Append("    ,:CTNTSEQNO ")
            .Append("    ,:SELECT_SERIES_SEQNO ")
            .Append("    ,:SERIESNM ")
            .Append("    ,:VCLMODEL_NAME ")
            .Append("    ,:DISP_BDY_COLOR ")
            .Append("    ,:QUANTITY ")
            .Append("    ,:FLLWUPBOXRSLT_SEQNO ")
            .Append("    ,:CATEGORYID ")
            .Append("    ,:CATEGORYDVSID ")
            .Append("    ,:VIN ")
            .Append("    ,:ACCOUNT_NM ")
            .Append("    ,:CRCUSTID ")
            .Append("    ,:CUSTOMERCLASS ")
            .Append("    ,SYSDATE ")
            .Append("    ,SYSDATE ")
            .Append(") ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_021")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Char, assessmentNo)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            query.AddParameterWithTypeValue("CRPLAN_ID", OracleDbType.Int64, crplanid)
            query.AddParameterWithTypeValue("BFAFDVS", OracleDbType.Char, bfafdvs)
            query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Int64, crdvsid)
            query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
            query.AddParameterWithTypeValue("SERIESCODE", OracleDbType.Char, seriescode)
            query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
            query.AddParameterWithTypeValue("REGNO", OracleDbType.Char, regno)
            query.AddParameterWithTypeValue("SUBCTGCODE", OracleDbType.Char, subctgcode)
            query.AddParameterWithTypeValue("SERVICECD", OracleDbType.Char, servicecd)
            query.AddParameterWithTypeValue("SUBCTGORGNAME", OracleDbType.Char, subctgorgname)
            query.AddParameterWithTypeValue("SUBCTGORGNAME_EX", OracleDbType.Char, subctgorgnameex)
            query.AddParameterWithTypeValue("PROMOTION_ID", OracleDbType.Int64, promotionid)
            query.AddParameterWithTypeValue("ACTIVITYRESULT", OracleDbType.Char, activityresult)
            query.AddParameterWithTypeValue("PLANDVS", OracleDbType.Char, plandvs)
            query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Date, actdate)
            query.AddParameterWithTypeValue("METHOD", OracleDbType.Char, method)
            query.AddParameterWithTypeValue("ACTION", OracleDbType.Char, action)
            query.AddParameterWithTypeValue("ACTIONTYPE", OracleDbType.Char, actiontype)
            query.AddParameterWithTypeValue("BRNCHACCOUNT", OracleDbType.Char, brnchaccount)
            query.AddParameterWithTypeValue("ACTIONCD", OracleDbType.Char, actioncd)
            query.AddParameterWithTypeValue("CTNTSEQNO", OracleDbType.Int64, ctntseqno)
            query.AddParameterWithTypeValue("SELECT_SERIES_SEQNO", OracleDbType.Int64, selectseriesseqno)
            query.AddParameterWithTypeValue("SERIESNM", OracleDbType.Char, seriesnm)
            query.AddParameterWithTypeValue("VCLMODEL_NAME", OracleDbType.Char, vclmodelname)
            query.AddParameterWithTypeValue("DISP_BDY_COLOR", OracleDbType.Char, dispbdycolor)
            query.AddParameterWithTypeValue("QUANTITY", OracleDbType.Long, quantity)
            query.AddParameterWithTypeValue("FLLWUPBOXRSLT_SEQNO", OracleDbType.Decimal, fllwupboxrsltseqno)
            query.AddParameterWithTypeValue("CATEGORYID", OracleDbType.Char, categoryid)
            query.AddParameterWithTypeValue("CATEGORYDVSID", OracleDbType.Int64, categorydvsid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
            query.AddParameterWithTypeValue("ACCOUNT_NM", OracleDbType.Char, accountnm)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupBoxCrHisAsm_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function
    '2013/03/05 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.93.商談ロック
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <remarks></remarks>
    Public Shared Sub GetSalesLock(ByVal salesid As Decimal, ByVal rowlockversion As Long)

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_401")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT")
                .AppendLine("  /* ActivityInfo_401 */")
                .AppendLine("1")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_SALES")
                .AppendLine(" WHERE")
                .AppendLine("  SALES_ID = :SALES_ID")
                .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            query.GetData()

        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesLock_End")
        'ログ出力 End *****************************************************************************
    End Sub
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.94.商談活動追加
    ''' </summary>
    ''' <param name="salesactid">商談活動ID</param>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="actid">活動ID</param>
    ''' <param name="rsltsalescat">実施商談分類</param>
    ''' <param name="createctactresult">実施後商談見込み度コード</param>
    ''' <param name="modelcode">モデルコード</param>
    ''' <param name="modelname">査定モデル名</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertSalesActivity(ByVal salesactid As Decimal, ByVal fllwupboxseqno As Decimal, ByVal actid As Decimal,
                                                   ByVal rsltsalescat As String, ByVal createctactresult As String, ByVal modelcode As String,
                                                   ByVal modelname As String,
                                                   ByVal acount As String, ByVal rowfunction As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSalesActivity_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_402 */")
            .AppendLine(" INTO TB_T_SALES_ACT (")
            .AppendLine("    SALES_ACT_ID ,")
            .AppendLine("    SALES_ID ,")
            .AppendLine("    ACT_ID ,")
            .AppendLine("    RSLT_SALES_CAT ,")
            .AppendLine("    MODEL_CD ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    ASSMNT_VCL_NAME ,")
            .AppendLine("    CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            .AppendLine("    ROW_LOCK_VERSION")
            .AppendLine(")")
            .AppendLine("VALUES (")
            .AppendLine("    :SALESACTID ,")
            .AppendLine("    :FLLWUPBOX_SEQNO ,")
            .AppendLine("    :ACTID ,")
            .AppendLine("    :RSLT_SALES_CAT ,")
            .AppendLine("    :MODELCD ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    :MODELNAME ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    0")
            .AppendLine(")")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_402")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESACTID", OracleDbType.Decimal, salesactid)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            query.AddParameterWithTypeValue("RSLT_SALES_CAT", OracleDbType.NVarchar2, rsltsalescat)
            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcode)
            query.AddParameterWithTypeValue("MODELNAME", OracleDbType.NVarchar2, modelname)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSalesActivity_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.95.用件データロック
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <remarks></remarks>
    Public Shared Sub GetRequestLock(ByVal reqid As Decimal, ByVal rowlockversion As Long)

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_403")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRequestLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT")
                .AppendLine("  /* ActivityInfo_403 */")
                .AppendLine("1")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_REQUEST")
                .AppendLine(" WHERE")
                .AppendLine("      REQ_ID = :REQ_ID")
                .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
                .Append(sqlForUpdate)
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            query.GetData()
        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRequestLock_End")
        'ログ出力 End *****************************************************************************    
    End Sub
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.96.商談追加
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="prospectcd">商談見込み度コード</param>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="compflg">商談完了フラグ</param>
    ''' <param name="giveupvclseq">断念競合車種連番</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="actid">初回活動ID</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertSales(ByVal fllwupboxseqno As Decimal, ByVal dlrcd As String, ByVal brncd As String,
                                                   ByVal cstid As Decimal, ByVal prospectcd As String, ByVal reqid As Decimal,
                                                   ByVal compflg As String, ByVal giveupvclseq As String, ByVal giveupresion As String,
                                                   ByVal acount As String, ByVal rowfunction As String, ByVal actid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSales_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_404 */")
            .AppendLine("INTO TB_T_SALES (")
            .AppendLine("    SALES_ID ,")
            .AppendLine("    DLR_CD ,")
            .AppendLine("    BRN_CD ,")
            .AppendLine("    CST_ID ,")
            .AppendLine("    SALES_PROSPECT_CD ,")
            .AppendLine("    REQ_ID ,")
            .AppendLine("    ATT_ID ,")
            .AppendLine("    ORIGIN_SALES_ID ,")
            .AppendLine("    ASK_PURCHASE_MTD ,")
            .AppendLine("    ASK_PURCHASE_TIMING ,")
            .AppendLine("    ASK_USE_TYPE ,")
            .AppendLine("    ASK_IMPORTANCE_POINT ,")
            .AppendLine("    ASK_BUDGET_TYPE ,")
            .AppendLine("    ASK_PAYMENT_MTD ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    SALES_COMPLETE_FLG ,")
            .AppendLine("    DIRECT_SALES_FLG ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    GIVEUP_COMP_VCL_SEQ ,")
            .AppendLine("    GIVEUP_REASON ,")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("    BRAND_RECOGNITION_ID , ")
            .AppendLine("    ACARD_NUM , ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            .AppendLine("    ROW_LOCK_VERSION, ")
            .AppendLine("    FIRST_SALES_ACT_ID, ")
            .AppendLine("    DIRECT_SALES_FLG_UPDATE_FLG ")
            .AppendLine(")")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine(" SELECT ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            .AppendLine("    :FLLWUPBOX_SEQNO ,")
            .AppendLine("    :DLR_CD ,")
            .AppendLine("    :BRN_CD ,")
            .AppendLine("    :CST_ID ,")
            .AppendLine("    :PROSPECTCD ,")
            .AppendLine("    :REQ_ID ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            .AppendLine("    0 ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    :COMPFLG ,")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            '直販フラグ(既存流用時の変更を再構築前の状態に復元)
            .AppendLine("    DIRECT_SALES_FLG ,")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
            .AppendLine("    :GIVEUPVCLSEQ ,")
            .AppendLine("    :GIVEUPREASON ,")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("    SLST.BRAND_RECOGNITION_ID , ")
            .AppendLine("    SLST.ACARD_NUM , ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNCTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNCTION ,")
            .AppendLine("    0, ")
            .AppendLine("    :FIRST_SALES_ACT_ID, ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("    DECODE(DIRECT_SALES_FLG,'1','1','0') ")
            .AppendLine(" FROM ")
            .AppendLine("    TB_T_SALES_TEMP SLST ")
            .AppendLine(" WHERE ")
            .AppendLine("    SLST.SALES_ID = :SALES_ID ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        End With
        Using query As New DBUpdateQuery("ActivityInfo_404")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brncd)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("PROSPECTCD", OracleDbType.NVarchar2, prospectcd)
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("COMPFLG", OracleDbType.NVarchar2, compflg)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("GIVEUPVCLSEQ", OracleDbType.Long, CType(giveupvclseq, Long))
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            query.AddParameterWithTypeValue("GIVEUPREASON", OracleDbType.NVarchar2, giveupresion)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, rowfunction)
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, fllwupboxseqno)
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("FIRST_SALES_ACT_ID", OracleDbType.Decimal, actid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSales_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 4.97.用件追加
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="crsuctid">顧客ID</param>
    ''' <param name="vclid">車両ID</param>
    ''' <param name="customerclass">受付顧客車両区分</param>
    ''' <param name="source1cd">用件ソース(1st）コード</param>
    ''' <param name="cractrslt">用件ステータス</param>
    ''' <param name="lastactdatetime">最終活動日時</param>
    ''' <param name="lastcallrsltid">最終活動結果ID</param>
    ''' <param name="lastactid">最終活動ID</param>
    ''' <param name="recldatetime">受付日時（現地）</param>
    ''' <param name="recdatetime">受付日時</param>
    ''' <param name="dlrcd">受付販売店コード</param>
    ''' <param name="brncd">受付店舗コード</param>
    ''' <param name="staffcd">受付スタッフコード</param>
    ''' <param name="reccontactmtd">受付方法</param>
    ''' <param name="reqactid">受付活動ID</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="orgnzid">組織ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertRequest(ByVal reqid As Decimal, ByVal crsuctid As Decimal, ByVal vclid As Decimal,
                                                   ByVal customerclass As String, ByVal source1cd As String, ByVal cractrslt As String,
                                                   ByVal lastactdatetime As Date, ByVal lastcallrsltid As String, ByVal lastactid As String,
                                                   ByVal recldatetime As Date, ByVal recdatetime As Date, ByVal dlrcd As String,
                                                   ByVal brncd As String, ByVal staffcd As String, ByVal reccontactmtd As String, ByVal reqactid As Decimal,
                                                   ByVal acount As String, ByVal rowfunction As String, ByVal salesId As Decimal, ByVal orgnzid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertRequest_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_405 */")
            .AppendLine("INTO TB_T_REQUEST (")
            .AppendLine("    REQ_ID ,")
            .AppendLine("    CST_ID ,")
            .AppendLine("    VCL_ID ,")
            .AppendLine("    REC_CST_VCL_TYPE ,")
            .AppendLine("    BIZ_TYPE ,")
            .AppendLine("    SOURCE_1_CD ,")
            .AppendLine("    SOURCE_2_CD ,")
            .AppendLine("    BIZ_CAT_ID ,")
            .AppendLine("    REQ_STATUS ,")
            .AppendLine("    LAST_ACT_DATETIME ,")
            .AppendLine("    ACT_RSLT_REG_COUNT ,")
            .AppendLine("    LAST_CALL_RSLT_ID ,")
            .AppendLine("    LAST_ACT_ID ,")
            .AppendLine("    REC_LDATETIME ,")
            .AppendLine("    REC_DATETIME ,")
            .AppendLine("    REC_DLR_CD ,")
            .AppendLine("    REC_BRN_CD ,")
            .AppendLine("    REC_STF_CD ,")
            .AppendLine("    REC_CONTACT_MTD ,")
            .AppendLine("    REC_ORGNZ_ID ,")
            .AppendLine("    REC_ACT_ID ,")
            .AppendLine("    REQ_CONTENT ,")
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            .AppendLine("    ROW_LOCK_VERSION")
            .AppendLine(")")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine(" SELECT ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            .AppendLine("    :REQ_ID ,")
            .AppendLine("    :CRCUSTID ,")
            .AppendLine("    :VCL_ID ,")
            .AppendLine("    :CUSTOMERCLASS ,")
            .AppendLine("    '2' ,")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("    SLST.SOURCE_1_CD , ")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine("    NVL(LSLS.SOURCE_2_CD, 0) , ")
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END
            '2014/04/21 TCS 松月 【A STEP2】業務種別設定不正対応（問連TR-V4-GTMC140416001）START
            .AppendLine("    (SELECT MAX(BIZ_CAT_ID) FROM TB_M_BUSSINES_CATEGORY WHERE BIZ_TYPE = '2' AND DLR_CD = 'XXXXX' AND BRN_CD = 'XXX' ) ,")
            '2014/04/21 TCS 松月 【A STEP2】業務種別設定不正対応（問連TR-V4-GTMC140416001）END
            .AppendLine("    :CRACTRESULT ,")
            .AppendLine("    :LAST_ACT_DATETIME ,")
            .AppendLine("    1 ,")
            .AppendLine("    :LASTCALLRSLTID ,")
            .AppendLine("    :LASTACTID ,")
            .AppendLine("    :REC_LDATETIME ,")
            .AppendLine("    :REC_DATETIME ,")
            .AppendLine("    :DLRCD ,")
            .AppendLine("    :STRCD ,")
            .AppendLine("    :STAFFCD ,")
            .AppendLine("    :RECCONTACTCMD ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .AppendLine("    :ORG_ID ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .AppendLine("    :LASTACTID ,")
            .AppendLine("    ' ' ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    0")
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine(" FROM ")
            .AppendLine("    TB_T_SALES_TEMP SLST ")
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine("    ,TB_LT_SALES LSLS ")
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END
            .AppendLine(" WHERE ")
            .AppendLine("     SLST.SALES_ID = :SALES_ID ")
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine(" AND SLST.SALES_ID = LSLS.SALES_ID(+) ")
            '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        End With
        Using query As New DBUpdateQuery("ActivityInfo_405")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crsuctid)
            query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.NVarchar2, customerclass)
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 DELETE
            query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.NVarchar2, cractrslt)
            query.AddParameterWithTypeValue("LAST_ACT_DATETIME", OracleDbType.Date, lastactdatetime)
            query.AddParameterWithTypeValue("LASTCALLRSLTID", OracleDbType.NVarchar2, lastcallrsltid)
            query.AddParameterWithTypeValue("LASTACTID", OracleDbType.Decimal, lastactid)
            query.AddParameterWithTypeValue("REC_LDATETIME", OracleDbType.Date, recldatetime)
            query.AddParameterWithTypeValue("REC_DATETIME", OracleDbType.Date, recdatetime)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, brncd)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("RECCONTACTCMD", OracleDbType.NVarchar2, reccontactmtd)
            query.AddParameterWithTypeValue("LASTACTID", OracleDbType.NVarchar2, reqactid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)

            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORG_ID", OracleDbType.Decimal, orgnzid)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End

            '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertRequest_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.98.活動追加
    ''' </summary>
    ''' <param name="actid">活動ID</param>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="attid">誘致ID</param>
    ''' <param name="count">活動回数</param>
    ''' <param name="schedatetime">予定日時</param>
    ''' <param name="walkinschestart">来店予定開始日時</param>
    ''' <param name="walkinscheend">来店予定終了日時</param>
    ''' <param name="dlrcdplan">予定販売店コード</param>
    ''' <param name="brncdplan">予定店舗コード</param>
    ''' <param name="staffcdplan">予定スタッフコード</param>
    ''' <param name="schecontactmtd">予定コンタクト方法</param>
    ''' <param name="rsltflg">実施フラグ</param>
    ''' <param name="rsltdate">実施日</param>
    ''' <param name="dlrcd">実施販売店コード</param>
    ''' <param name="brncd">実施店舗コード</param>
    ''' <param name="rsltstaffcd">実施スタッフコード</param>
    ''' <param name="rsltcontactmtd">実施コンタクト方法</param>
    ''' <param name="cractstatus">活動ステータス</param>
    ''' <param name="rsltid">活動結果ID</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <param name="createctactresult">実施後商談見込み度コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertActivity(ByVal actid As Decimal, ByVal reqid As Decimal, ByVal attid As Decimal,
                                                   ByVal count As Long, ByVal schedatetime As Date, ByVal walkinschestart As Date,
                                                   ByVal walkinscheend As Date, ByVal dlrcdplan As String, ByVal brncdplan As String,
                                                   ByVal staffcdplan As String, ByVal schecontactmtd As String, ByVal rsltflg As String,
                                                   ByVal rsltdate As Date, ByVal dlrcd As String, ByVal brncd As String,
                                                   ByVal rsltstaffcd As String, ByVal rsltcontactmtd As String, ByVal cractstatus As String,
                                                   ByVal rsltid As String,
                                                   ByVal acount As String, ByVal rowfunction As String,
                                                   ByVal createctactresult As String,
                                                   ByVal orgnzid As Decimal, ByVal orgnzidplan As Decimal) As Integer
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertActivity_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_406 */")
            .AppendLine("INTO TB_T_ACTIVITY (")
            .AppendLine("    ACT_ID ,")
            .AppendLine("    REQ_ID ,")
            .AppendLine("    ATT_ID ,")
            .AppendLine("    ACT_COUNT ,")
            .AppendLine("    SCHE_DATEORTIME ,")
            .AppendLine("    SCHE_DATEORTIME_FLG ,")
            .AppendLine("    WALKIN_SCHE_START_DATEORTIME ,")
            .AppendLine("    WALKIN_SCHE_END_DATEORTIME ,")
            .AppendLine("    WALKIN_SCHE_DATEORTIME_FLG ,")
            .AppendLine("    SCHE_DLR_CD ,")
            .AppendLine("    SCHE_BRN_CD ,")
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
            .AppendLine("    SCHE_ORGNZ_ID ,")
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End
            .AppendLine("    SCHE_STF_CD ,")
            .AppendLine("    SCHE_CONTACT_MTD ,")
            .AppendLine("    RSLT_FLG ,")
            .AppendLine("    RSLT_DATE ,")
            .AppendLine("    RSLT_DATETIME ,")
            .AppendLine("    RSLT_DLR_CD ,")
            .AppendLine("    RSLT_BRN_CD ,")
            .AppendLine("    RSLT_ORGNZ_ID ,")
            .AppendLine("    RSLT_STF_CD ,")
            .AppendLine("    RSLT_CONTACT_MTD ,")
            .AppendLine("    ACT_STATUS ,")
            .AppendLine("    RSLT_ID ,")
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .AppendLine("    ROW_LOCK_VERSION ,")
            .AppendLine("    RSLT_SALES_PROSPECT_CD, ")
            .AppendLine("    RSLT_INPUT_DATETIME ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .AppendLine(")")
            .AppendLine("VALUES (")
            .AppendLine("    :ACTID ,")
            .AppendLine("    :REQ_ID ,")
            .AppendLine("    :ATT_ID ,")
            .AppendLine("    :COUNT ,")
            .AppendLine("    :SCHE_DATEORTIME ,")
            .AppendLine("    '1' ,")
            .AppendLine("    :WALKIN_SCHE_START_DATEORTIME ,")
            .AppendLine("    :WALKIN_SCHE_END_DATEORTIME ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .AppendLine("    '0' ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .AppendLine("    :DLRCD_PLAN ,")
            .AppendLine("    :BRANCH_PLAN ,")
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
            .AppendLine("    :ORG_PLAN ,")
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End
            .AppendLine("    :ACCOUNT_PLAN ,")
            .AppendLine("    :SCHE_CONTACT_MTD ,")
            .AppendLine("    :RSLT_FLG ,")
            .AppendLine("    :RSLT_DATE ,")
            .AppendLine("    :RSLT_DATETIME ,")
            .AppendLine("    :DLRCD ,")
            .AppendLine("    :STRCD ,")
            .AppendLine("    :RSLT_ORGNZ_ID ,")
            .AppendLine("    :RSLT_STF_CD ,")
            .AppendLine("    :RSLT_CONTACT_MTD ,")
            .AppendLine("    :CRACTSTATUS ,")
            .AppendLine("    :RSLTID ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .AppendLine("    0 ,")
            .AppendLine("    :CREATE_CRACTRESULT, ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .AppendLine("    SYSDATE ")
            .AppendLine(")")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_406")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
            query.AddParameterWithTypeValue("COUNT", OracleDbType.Long, count)
            query.AddParameterWithTypeValue("SCHE_DATEORTIME", OracleDbType.Date, schedatetime)
            query.AddParameterWithTypeValue("WALKIN_SCHE_START_DATEORTIME", OracleDbType.Date, walkinschestart)
            query.AddParameterWithTypeValue("WALKIN_SCHE_END_DATEORTIME", OracleDbType.Date, walkinscheend)
            query.AddParameterWithTypeValue("DLRCD_PLAN", OracleDbType.NVarchar2, dlrcdplan)
            query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.NVarchar2, brncdplan)
            query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.NVarchar2, staffcdplan)
            query.AddParameterWithTypeValue("SCHE_CONTACT_MTD", OracleDbType.NVarchar2, schecontactmtd)
            query.AddParameterWithTypeValue("RSLT_FLG", OracleDbType.NVarchar2, rsltflg)
            query.AddParameterWithTypeValue("RSLT_DATE", OracleDbType.NVarchar2, Format(rsltdate, "yyyyMMdd"))
            query.AddParameterWithTypeValue("RSLT_DATETIME", OracleDbType.Date, rsltdate)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, brncd)
            query.AddParameterWithTypeValue("RSLT_STF_CD", OracleDbType.NVarchar2, rsltstaffcd)
            query.AddParameterWithTypeValue("RSLT_CONTACT_MTD", OracleDbType.NVarchar2, rsltcontactmtd)
            query.AddParameterWithTypeValue("CRACTSTATUS", OracleDbType.NVarchar2, cractstatus)
            query.AddParameterWithTypeValue("RSLTID", OracleDbType.NVarchar2, rsltid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CREATE_CRACTRESULT", OracleDbType.NVarchar2, createctactresult)
            query.AddParameterWithTypeValue("RSLT_ORGNZ_ID", OracleDbType.Decimal, orgnzid)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORG_PLAN", OracleDbType.Decimal, orgnzidplan)
            ' 2014/05/31 TCS 外崎 TMT不具合対応 Modify End

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertActivity_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.99.Follow-up Box商談データロック
    ''' </summary>
    ''' <param name="followupseq">商談ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetFollowupSalesLock(ByVal followupseq As Decimal)

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_407")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupSalesLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT")
                .AppendLine("  /* ActivityInfo_407 */")
                .AppendLine("1")
                .AppendLine(" FROM")
                .AppendLine("  TBL_FLLWUPBOX_SALES")
                .AppendLine(" WHERE")
                .AppendLine("      FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                .AppendLine("  AND REGISTFLG = '0'")
                .Append(sqlForUpdate)
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, followupseq)
            query.GetData()
        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupSalesLock_End")
        'ログ出力 End *****************************************************************************
    End Sub
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.100.見積データロック
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <remarks></remarks>

    Public Shared Sub GetEstimateLock(ByVal fllwupboxseqno As Decimal, ByVal seq As String)

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_408")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            ' 外版色コードを前3桁だけで比較するか否かフラグ
            Dim extColor3Flg As String = String.Empty
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
            If IsNothing(sysEnvRow) Then
                '取得できなかった場合、"0"を設定
                extColor3Flg = "0"
            Else
                extColor3Flg = sysEnvRow.PARAMVALUE
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine(" SELECT")
                .AppendLine("  /*ActivityInfo_408 */")
                .AppendLine(" 1")
                .AppendLine(" FROM")
                .AppendLine("     TBL_ESTIMATEINFO T1 ")
                .AppendLine(" WHERE EXISTS ( ")
                .AppendLine("     SELECT ")
                .AppendLine("          * ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_T_PREFER_VCL T2, ")
                .AppendLine("          TBL_EST_VCLINFO T3 ")
                .AppendLine("      WHERE ")
                .AppendLine("          T2.SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
                .AppendLine("      AND T2.PREF_VCL_SEQ = TO_NUMBER(:SEQ) ")
                .AppendLine("      AND T2.MODEL_CD = T3.SERIESCD ")
                .AppendLine("      AND T2.GRADE_CD = T3.MODELCD ")
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
                If (extColor3Flg = "1") Then
                    .AppendLine("      AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
                Else
                    .AppendLine("      AND T2.BODYCLR_CD = T3.EXTCOLORCD ")
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                .AppendLine("      AND T1.ESTIMATEID = T3.ESTIMATEID ")
                .AppendLine("      AND T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .AppendLine(" ) ")
                .AppendLine(sqlForUpdate)
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
            query.GetData()
        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateLock_End")
        'ログ出力 End *****************************************************************************
    End Sub
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致データロック
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <remarks></remarks>
    Public Shared Sub GetAttractLock(ByVal attid As Decimal, ByVal rowlockversion As Long)

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_444")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAttractLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT")
                .AppendLine("  /* ActivityInfo_444 */")
                .AppendLine("1")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_ATTRACT")
                .AppendLine(" WHERE")
                .AppendLine("      ATT_ID = :ATT_ID")
                .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            query.GetData()

        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAttractLock_End")
        'ログ出力 End *****************************************************************************

    End Sub
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.102.用件IDシーケンス取得
    ''' </summary>
    ''' <returns>用件ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqReqId() As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqReqId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT")
            .AppendLine("  /* ActivityInfo_409 */")
            .AppendLine("  SQ_REQUEST.NEXTVAL AS SEQ ")
            .AppendLine(" FROM ")
            .AppendLine("  DUAL")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_409")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqReqId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.103.活動IDシーケンス取得
    ''' </summary>
    ''' <returns>活動ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqActId() As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqActId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT")
            .AppendLine("  /* ActivityInfo_410 */")
            .AppendLine("  SQ_ACTIVITY.NEXTVAL AS SEQ ")
            .AppendLine(" FROM ")
            .AppendLine("  DUAL")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_410")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqActId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    '''         ''' <summary>
    ''' 4.104.商談IDシーケンス取得
    ''' </summary>
    ''' <returns>商談ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqSalesId() As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSalesId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT")
            .AppendLine("  /* ActivityInfo_411 */")
            .AppendLine("  SQ_SALES.NEXTVAL AS SEQ")
            .AppendLine(" FROM")
            .AppendLine("  DUAL")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_411")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSalesId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.105.商談活動IDシーケンス取得
    ''' </summary>
    ''' <returns>商談活動ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqSalesActId() As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSalesActId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT")
            .AppendLine("  /* ActivityInfo_412 */")
            .AppendLine("  SQ_SALES_ACT.NEXTVAL AS SEQ")
            .AppendLine(" FROM")
            .AppendLine("  DUAL")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_412")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSalesActId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 4.107.カタログ請求追加
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="modelcd">カタログモデルコード</param>
    ''' <param name="rsltdate">受取希望日</param>
    ''' <param name="staffcd">実施スタッフコード</param>
    ''' <param name="rsltcontactmtd">実施コンタクト方法</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertBrochure(ByVal salesid As Decimal, ByVal modelcd As String, ByVal rsltdate As Date,
                                                   ByVal staffcd As String, ByVal rsltcontactmtd As String,
                                                   ByVal acount As String, ByVal rowfunction As String, ByVal orgnzid As Decimal) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertBrochure_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_413 */")
            .AppendLine("INTO TB_T_BROCHURE (")
            .AppendLine("    SALES_ID ,")
            .AppendLine("    BROCHURE_MODEL_CD ,")
            .AppendLine("    RECV_PREF_DATE ,")
            .AppendLine("    RSLT_FLG ,")
            .AppendLine("    SEND_RSLT_DATE ,")
            .AppendLine("    RSLT_STF_CD ,")
            .AppendLine("    RSLT_DEPT_ID ,")
            .AppendLine("    RSLT_CONTACT_MTD ,")
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            .AppendLine("    ROW_LOCK_VERSION")
            .AppendLine(")")
            .AppendLine("VALUES (")
            .AppendLine("    :SALESID ,")
            .AppendLine("    :MODELCD ,")
            .AppendLine("    :RSLTDATE ,")
            .AppendLine("    '1' ,")
            .AppendLine("    :RSLTDATE ,")
            .AppendLine("    :STAFFCD ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .AppendLine("    :ORGID ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .AppendLine("    :RSLTCONTACTMTD ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    0")
            .AppendLine(")")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_413")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcd)
            query.AddParameterWithTypeValue("RSLTDATE", OracleDbType.Date, rsltdate)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("RSLTCONTACTMTD", OracleDbType.NVarchar2, rsltcontactmtd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORGID", OracleDbType.Decimal, orgnzid)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertBrochure_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.108.用件テーブル移動
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveRequest(ByVal reqid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveRequest_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_414 */")
            .AppendLine("INTO TB_H_REQUEST")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_REQUEST")
            .AppendLine("    WHERE")
            .AppendLine("      REQ_ID = :REQID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_414")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveRequest_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.109.誘致テーブル移動
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveAttract(ByVal attid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttract_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_415 */")
            .AppendLine("INTO TB_H_ATTRACT")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ATTRACT")
            .AppendLine("    WHERE")
            .AppendLine("      ATT_ID = :ATTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_415")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttract_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.110.コール誘致活動テーブル移動
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveAttractCall(ByVal attid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractCall_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_416 */")
            .AppendLine("INTO TB_H_ATTRACT_CALL")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ATTRACT_CALL")
            .AppendLine("    WHERE")
            .AppendLine("      ATT_ID = :ATTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_416")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractCall_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.111.活動テーブル移動
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveActivity(ByVal reqid As Decimal, ByVal attid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveActivity_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_417 */")
            .AppendLine("INTO TB_H_ACTIVITY")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ACTIVITY")
            .AppendLine("    WHERE")
            .AppendLine("          REQ_ID = :REQID")
            .AppendLine("      AND ATT_ID = :ATTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_417")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveActivity_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.112.活動一覧取得
    ''' </summary>
    ''' <param name="reqid"></param>
    ''' <param name="attid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SelectActionID(ByVal reqid As Decimal, ByVal attid As Decimal) As ActivityInfoDataSet.ActionidDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActionidDataTable)("ActivityInfo_418")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectActionID_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT")
                .AppendLine("  /* ActivityInfo_418 */")
                .AppendLine("  ACT_ID")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_ACTIVITY")
                .AppendLine(" WHERE")
                .AppendLine("      REQ_ID = :REQID")
                .AppendLine("  AND ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectActionID_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.113.活動メモテーブル移動
    ''' </summary>
    ''' <param name="actid">活動ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveActivityMemo(ByVal actid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveActivityMemo_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_419 */")
            .AppendLine("INTO TB_H_ACTIVITY_MEMO")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ACTIVITY_MEMO")
            .AppendLine("    WHERE")
            .AppendLine("      RELATION_ACT_ID = :ACTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_419")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveActivityMemo_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.114.商談テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveSales(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSales_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_420 */")
            .AppendLine("INTO TB_H_SALES")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_SALES")
            .AppendLine("    WHERE")
            .AppendLine("      SALES_ID = :SALESID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_420")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSales_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.115.商談活動テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveSalesAct(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSalesAct_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_421 */")
            .AppendLine("INTO TB_H_SALES_ACT")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_SALES_ACT")
            .AppendLine("    WHERE")
            .AppendLine("      SALES_ID = :SALESID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_421")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSalesAct_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.116.希望車テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MovePreferVcl(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MovePreferVcl_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_422 */")
            .AppendLine("INTO TB_H_PREFER_VCL")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_PREFER_VCL")
            .AppendLine("    WHERE")
            .AppendLine("      SALES_ID = :SALESID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_422")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MovePreferVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.117.競合車種テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveCompetitorVcl(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveCompetitorVcl_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_423 */")
            .AppendLine("INTO TB_H_COMPETITOR_VCL")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_COMPETITOR_VCL")
            .AppendLine("    WHERE")
            .AppendLine("      SALES_ID = :SALESID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_423")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveCompetitorVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.118.カタログ請求テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveBrochure(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveBrochure_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_424 */")
            .AppendLine("INTO TB_H_BROCHURE")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_BROCHURE")
            .AppendLine("    WHERE")
            .AppendLine("      SALES_ID = :SALESID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_424")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveBrochure_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveAssessmentAct(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAssessmentAct_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT ")
            .AppendLine("    /* ActivityInfo_441 */ ")
            .AppendLine(" INTO TB_H_ASSESSMENT_ACT ")
            .AppendLine("    SELECT ")
            .AppendLine("      * ")
            .AppendLine("    FROM ")
            .AppendLine("      TB_T_ASSESSMENT_ACT ")
            .AppendLine("    WHERE ")
            .AppendLine("      SALES_ID = :SALESID ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_441")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAssessmentAct_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.119.用件テーブル削除
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteRequest(ByVal reqid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_425")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteRequest_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_425 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_REQUEST")
                .AppendLine("WHERE")
                .AppendLine("    REQ_ID = :REQID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteRequest_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.120.誘致テーブル削除
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteAttract(ByVal attid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_426")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttract_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_426 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ATTRACT")
                .AppendLine("WHERE")
                .AppendLine("    ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttract_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.121.コール誘致活動テーブル削除
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteAttractCall(ByVal attid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_427")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractCall_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_427 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ATTRACT_CALL")
                .AppendLine("WHERE")
                .AppendLine("    ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractCall_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.122.活動テーブル削除
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteActivity(ByVal reqid As Decimal, ByVal attid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_428")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivity_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_428 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ACTIVITY")
                .AppendLine("WHERE")
                .AppendLine("        REQ_ID = :REQID")
                .AppendLine("    AND ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivity_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.123.活動メモテーブル削除
    ''' </summary>
    ''' <param name="actid">活動ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteActivityMemo(ByVal actid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_429")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivityMemo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_429 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ACTIVITY_MEMO")
                .AppendLine("WHERE")
                .AppendLine("      RELATION_ACT_ID = :ACTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivityMemo_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.124.商談テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteSales(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_430")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSales_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_430 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_SALES")
                .AppendLine("WHERE")
                .AppendLine("    SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSales_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.125.商談活動テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteSalesAct(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_431")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSalesAct_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_431 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_SALES_ACT")
                .AppendLine("WHERE")
                .AppendLine("    SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSalesAct_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.126.希望車テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeletePreferVcl(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_432")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeletePreferVcl_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_432 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_PREFER_VCL")
                .AppendLine("WHERE")
                .AppendLine("   SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeletePreferVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.127.競合車種テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteCompetitorVcl(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_433")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCompetitorVcl_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_433 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_COMPETITOR_VCL")
                .AppendLine("WHERE")
                .AppendLine("    SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCompetitorVcl_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.128.カタログ請求テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteBrochure(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_434")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteBrochure_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_434 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_BROCHURE")
                .AppendLine("WHERE")
                .AppendLine("    SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteBrochure_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 未 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 試乗予約テーブル移動
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveTestDrive(ByVal salesid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveTestDrive_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT ")
            .AppendLine("    /* ActivityInfo_435 */ ")
            .AppendLine(" INTO TB_H_TESTDRIVE ")
            .AppendLine("    SELECT ")
            .AppendLine("      * ")
            .AppendLine("    FROM ")
            .AppendLine("      TB_T_TESTDRIVE ")
            .AppendLine("    WHERE ")
            .AppendLine("      SALES_ID = :SALESID ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_435")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveTestDrive_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 未 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 未 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 試乗予約テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteTestDrive(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_436")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteTestDrive_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE ")
                .AppendLine("    /* ActivityInfo_436 */ ")
                .AppendLine(" FROM ")
                .AppendLine("    TB_T_TESTDRIVE ")
                .AppendLine(" WHERE ")
                .AppendLine("    SALES_ID = :SALESID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteTestDrive_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 未 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定テーブル削除
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteAssessmentAct(ByVal salesid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_442")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAssessmentAct_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_442 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ASSESSMENT_ACT")
                .AppendLine("WHERE")
                .AppendLine("    SALES_ID = :SALESID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAssessmentAct_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.91.用件更新
    ''' </summary>
    ''' <param name="reqid">用件ID</param>
    ''' <param name="thistimecractrslt">用件ステータス</param>
    ''' <param name="lastactdatetime">最終活動日時</param>
    ''' <param name="count">活動結果登録回数</param>
    ''' <param name="lastcallrsltid">最終活動結果ID</param>
    ''' <param name="lastactid">最終活動ID</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateRequest(ByVal reqid As Decimal, ByVal thistimecractrslt As String, ByVal lastactdatetime As Date, ByVal count As Long,
                                                   ByVal lastcallrsltid As Long, ByVal lastactid As Decimal,
                                                   ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long) As Integer

        Using query As New DBUpdateQuery("ActivityInfo_217")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateRequest_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_217 */")
                .AppendLine("    TB_T_REQUEST")
                .AppendLine(" SET")
                .AppendLine("    REQ_STATUS = :THISTIME_CRACTRESULT ,")
                .AppendLine("    LAST_ACT_DATETIME = :LASTACTDATETIME ,")
                .AppendLine("    ACT_RSLT_REG_COUNT = :COUNT ,")
                .AppendLine("    LAST_CALL_RSLT_ID = :LASTCALLRSLTID ,")
                .AppendLine("    LAST_ACT_ID = :LASTACTID ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        REQ_ID = :REQ_ID")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.NVarchar2, thistimecractrslt)
            query.AddParameterWithTypeValue("LASTACTDATETIME", OracleDbType.Date, lastactdatetime)
            query.AddParameterWithTypeValue("COUNT", OracleDbType.Int64, count)
            query.AddParameterWithTypeValue("LASTCALLRSLTID", OracleDbType.Long, lastcallrsltid)
            query.AddParameterWithTypeValue("LASTACTID", OracleDbType.Decimal, lastactid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateRequest_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.106.誘致更新
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <param name="attstatus">誘致ステータス</param>
    ''' <param name="lastactdate">最終活動日時</param>
    ''' <param name="count">活動結果登録回数</param>
    ''' <param name="lastrsltid">最終活動結果ID</param>
    ''' <param name="lastactid">最終活動ID</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateAttract(ByVal attid As Decimal, ByVal attstatus As String, ByVal lastactdate As Date, ByVal count As Long,
                                                 ByVal lastrsltid As String, ByVal lastactid As Decimal,
                                                 ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long) As Integer

        Using query As New DBUpdateQuery("ActivityInfo_443")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateAttract_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_443 */")
                .AppendLine("    TB_T_ATTRACT")
                .AppendLine(" SET")
                .AppendLine("    CONTINUE_ACT_STATUS = :ATTSTATUS ,")
                .AppendLine("    LAST_ACT_DATETIME = :LASTACTDATETIME ,")
                .AppendLine("    ACT_RSLT_REG_COUNT = :COUNT ,")
                .AppendLine("    LAST_RSLT_ID = :LASTRSLTID ,")
                .AppendLine("    LAST_ACT_ID = :LASTACTID ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        ATT_ID = TO_NUMBER(:ATTID)")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            query.AddParameterWithTypeValue("ATTSTATUS", OracleDbType.NVarchar2, attstatus)
            query.AddParameterWithTypeValue("LASTACTDATETIME", OracleDbType.Date, lastactdate)
            query.AddParameterWithTypeValue("COUNT", OracleDbType.Long, count)
            query.AddParameterWithTypeValue("LASTRSLTID", OracleDbType.NVarchar2, lastrsltid)
            query.AddParameterWithTypeValue("LASTACTID", OracleDbType.NVarchar2, lastactid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateAttract_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.92.活動更新
    ''' </summary>
    ''' <param name="actid">活動ID</param>
    ''' <param name="rsltdate">実施日</param>
    ''' <param name="rsltdatetime">実施日時</param>
    ''' <param name="dlrcd">実施販売店コード</param>
    ''' <param name="brncd">実施店舗コード</param>
    ''' <param name="staffcd">実施スタッフコード</param>
    ''' <param name="rsltcontactmthd">実施コンタクト方法</param>
    ''' <param name="thistimecractstatus">活動ステータス</param>
    ''' <param name="rsltid">活動結果ID</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <param name="createctactresult">実施後商談見込み度コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateActivity(ByVal actid As Decimal, ByVal rsltdate As Date, ByVal rsltdatetime As Date, ByVal dlrcd As String,
                                                   ByVal brncd As String, ByVal staffcd As String, ByVal rsltcontactmthd As String,
                                                   ByVal thistimecractstatus As String, ByVal rsltid As String,
                                                   ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long,
                                                   ByVal createctactresult As String,
                                                   ByVal orgnzid As Decimal) As Integer
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Using query As New DBUpdateQuery("ActivityInfo_219")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActivity_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_219 */")
                .AppendLine("    TB_T_ACTIVITY")
                .AppendLine(" SET")
                .AppendLine("    RSLT_FLG = '1' ,")
                .AppendLine("    RSLT_DATE = :RSLTDATE ,")
                .AppendLine("    RSLT_DATETIME = :RSLTDATETIME ,")
                .AppendLine("    RSLT_DLR_CD = :DLRCD ,")
                .AppendLine("    RSLT_BRN_CD = :BRNCD ,")
                .AppendLine("    RSLT_ORGNZ_ID = :RSLTORGNZID ,")
                .AppendLine("    RSLT_STF_CD = :STAFFCD ,")
                .AppendLine("    RSLT_CONTACT_MTD = :RSLTCONTACTMTD ,")
                .AppendLine("    ACT_STATUS = :THISTIME_CRACTSTATUS ,")
                .AppendLine("    RSLT_ID = :RSLTID ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1,")
                .AppendLine("    RSLT_SALES_PROSPECT_CD = :CREATE_CRACTRESULT, ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                .AppendLine("    RSLT_INPUT_DATETIME = SYSDATE")
                .AppendLine(" WHERE")
                .AppendLine("    ACT_ID = :ACT_ID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACT_ID", OracleDbType.Decimal, actid)
            query.AddParameterWithTypeValue("RSLTDATE", OracleDbType.NVarchar2, Format(rsltdate, "yyyyMMdd"))
            query.AddParameterWithTypeValue("RSLTDATETIME", OracleDbType.Date, rsltdatetime)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brncd)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("RSLTCONTACTMTD", OracleDbType.NVarchar2, rsltcontactmthd)
            query.AddParameterWithTypeValue("THISTIME_CRACTSTATUS", OracleDbType.NVarchar2, thistimecractstatus)
            query.AddParameterWithTypeValue("RSLTID", OracleDbType.NVarchar2, rsltid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CREATE_CRACTRESULT", OracleDbType.NVarchar2, createctactresult)
            query.AddParameterWithTypeValue("RSLTORGNZID", OracleDbType.Decimal, orgnzid)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActivity_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.90.商談更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="prospectcd">商談見込み度コード</param>
    ''' <param name="completeflg">商談完了フラグ</param>
    ''' <param name="giveupvclseq">断念競合車種連番</param>
    ''' <param name="giveupresion">断念原因</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateSales(ByVal fllwupboxseqno As Decimal, ByVal prospectcd As String, ByVal completeflg As String, ByVal giveupvclseq As Long,
                                       ByVal giveupresion As String, ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_213")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSales_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_213 */")
                .AppendLine("    TB_T_SALES")
                .AppendLine(" SET")
                .AppendLine("    SALES_PROSPECT_CD = :PROSPECTCD ,")
                .AppendLine("    SALES_COMPLETE_FLG = :COMPLETEFLG ,")
                .AppendLine("    GIVEUP_COMP_VCL_SEQ = :GIVEUPVCLSEQ ,")
                .AppendLine("    GIVEUP_REASON = :GIVEUPREASON ,")
                '2017/11/20 TCS 河原 TKM独自機能開発 START
                .AppendLine("    DIRECT_SALES_FLG_UPDATE_FLG = DECODE(DIRECT_SALES_FLG || ',' || DIRECT_SALES_FLG_UPDATE_FLG,'1,0','1',DIRECT_SALES_FLG_UPDATE_FLG), ")
                '2017/11/20 TCS 河原 TKM独自機能開発 END
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO)")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("PROSPECTCD", OracleDbType.NVarchar2, prospectcd)
            query.AddParameterWithTypeValue("COMPLETEFLG", OracleDbType.NVarchar2, completeflg)
            query.AddParameterWithTypeValue("GIVEUPVCLSEQ", OracleDbType.Long, giveupvclseq)
            query.AddParameterWithTypeValue("GIVEUPREASON", OracleDbType.Long, giveupresion)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSales_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 予定活動ID取得
    ''' </summary>
    ''' <returns>予定活動情報関連データセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetScheSqActId(ByVal reqid As Decimal, ByVal attid As Decimal) As ActivityInfoDataSet.ActivityInfoGetScheDataDataTable

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetScheSqActId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT ")
            .AppendLine("  /* ActivityInfo_435 */")
            .AppendLine("  ACT_ID, ")
            .AppendLine("  ROW_LOCK_VERSION ")
            .AppendLine(" FROM")
            .AppendLine("  TB_T_ACTIVITY ")
            .AppendLine(" WHERE")
            .AppendLine("      REQ_ID = :REQ_ID ")
            .AppendLine("  AND ATT_ID = :ATT_ID ")
            .AppendLine("  AND RSLT_FLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetScheDataDataTable)("ActivityInfo_435")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
            query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetScheSqActId_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using

    End Function
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 試乗予約ID取得
    ''' </summary>
    ''' <returns>試乗予約ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetReqTestDriveId() As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetReqTestDriveId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT ")
            .AppendLine("  /* ActivityInfo_436 */ ")
            .AppendLine("  SQ_TESTDRIVE_ID.NEXTVAL AS SEQ ")
            .AppendLine(" FROM ")
            .AppendLine("  DUAL ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_436")
            query.CommandText = sql.ToString()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetReqTestDriveId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using

    End Function
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 試乗予約追加
    ''' </summary>
    ''' <param name="testdriveid">試乗予約ID</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <param name="modelcd">モデルコード</param>
    ''' <param name="gradecd">グレードコード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="rsltdate">実施日</param>
    ''' <param name="rsltfrom">開始時間</param>
    ''' <param name="rsltto">終了時間</param>
    ''' <param name="stfcd">スタッフコード</param>
    ''' <param name="account">作成アカウント</param>
    ''' <param name="rowfunction">作成機能</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertTestDrive(ByVal testdriveid As Decimal, ByVal dlrcd As String, ByVal brncd As String, _
                                           ByVal modelcd As String, ByVal gradecd As String, ByVal cstid As Decimal, ByVal salesid As Decimal, _
                                           ByVal rsltdate As Date, ByVal rsltfrom As Date, ByVal rsltto As Date, _
                                           ByVal stfcd As String, ByVal account As String, ByVal rowfunction As String, ByVal orgnzid As Decimal) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertTestDrive_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT ")
            .AppendLine("    /* ActivityInfo_437 */ ")
            .AppendLine(" INTO TB_H_TESTDRIVE ( ")
            .AppendLine("    REQ_TESTDRIVE_ID, ")
            .AppendLine("    DLR_CD, ")
            .AppendLine("    VCL_TESTDRIVE_ID, ")
            .AppendLine("    VCL_TESTDRIVE_BRN_CD, ")
            .AppendLine("    PREF_MODEL_CD, ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .AppendLine("    PREF_GRADE_CD, ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .AppendLine("    CST_ID, ")
            .AppendLine("    SALES_ID, ")
            .AppendLine("    TESTDRIVE_SCHE_FROM_DATE, ")
            .AppendLine("    TESTDRIVE_SCHE_FROM_DATETIME, ")
            .AppendLine("    TESTDRIVE_SCHE_TO_DATETIME, ")
            .AppendLine("    TESTDRIVE_SCHE_BRN_CD, ")
            .AppendLine("    TESTDRIVE_SCHE_STF_CD, ")
            .AppendLine("    TESTDRIVE_RSLT_FLG, ")
            .AppendLine("    TESTDRIVE_RSLT_DATE, ")
            .AppendLine("    TESTDRIVE_RSLT_BRN_CD, ")
            .AppendLine("    TESTDRIVE_RSLT_STF_CD, ")
            .AppendLine("    TESTDRIVE_RSLT_DEPT_ID, ")
            .AppendLine("    REQ_TESTDRIVE_STATUS, ")
            .AppendLine("    ROW_CREATE_DATETIME, ")
            .AppendLine("    ROW_CREATE_ACCOUNT, ")
            .AppendLine("    ROW_CREATE_FUNCTION, ")
            .AppendLine("    ROW_UPDATE_DATETIME, ")
            .AppendLine("    ROW_UPDATE_ACCOUNT, ")
            .AppendLine("    ROW_UPDATE_FUNCTION, ")
            .AppendLine("    ROW_LOCK_VERSION ")
            .AppendLine("    ) ")
            .AppendLine(" VALUES ( ")
            .AppendLine("    :TESTDRIVEID, ")
            .AppendLine("    :DLRCD, ")
            .AppendLine("    0, ")
            .AppendLine("    :BRNCD, ")
            .AppendLine("    :MODELCD, ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            .AppendLine("    :GRADECD, ")
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            .AppendLine("    :CSTID, ")
            .AppendLine("    :SALESID, ")
            .AppendLine("    :SCHEFROMDATE, ")
            .AppendLine("    :SCHEFROMDATETIME, ")
            .AppendLine("    :SCHETODATETIME, ")
            .AppendLine("    :BRNCD, ")
            .AppendLine("    :STFCD, ")
            .AppendLine("    '1', ")
            .AppendLine("    :RSLTDATE, ")
            .AppendLine("    :BRNCD, ")
            .AppendLine("    :STFCD, ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .AppendLine("    :ORGID, ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .AppendLine("    '1', ")
            .AppendLine("    SYSDATE, ")
            .AppendLine("    :ACCOUNT, ")
            .AppendLine("    :FUNTION, ")
            .AppendLine("    SYSDATE, ")
            .AppendLine("    :ACCOUNT, ")
            .AppendLine("    :FUNTION, ")
            .AppendLine("    0 ")
            .AppendLine("    ) ")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_437")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("TESTDRIVEID", OracleDbType.Decimal, testdriveid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brncd)
            query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcd)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("GRADECD", OracleDbType.NVarchar2, gradecd)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            query.AddParameterWithTypeValue("SCHEFROMDATE", OracleDbType.NVarchar2, Format(rsltdate, "yyyyMMdd"))
            query.AddParameterWithTypeValue("SCHEFROMDATETIME", OracleDbType.Date, rsltfrom)
            query.AddParameterWithTypeValue("SCHETODATETIME", OracleDbType.Date, rsltto)
            query.AddParameterWithTypeValue("RSLTDATE", OracleDbType.Date, rsltdate)
            query.AddParameterWithTypeValue("STFCD", OracleDbType.NVarchar2, stfcd)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORGID", OracleDbType.Decimal, orgnzid)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertTestDrive_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定情報取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns>下取り査定情報のデータセット</returns>
    ''' <remarks></remarks>
    Public Shared Function GetActAsmInfo(ByVal fllwupboxseqno As Decimal) As ActivityInfoDataSet.ActAsmInfoDataTable
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActAsmInfoDataTable)("ActivityInfo_438")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActAsmInfo_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT /* ActivityInfo_438 */ ")
                .Append("       T1.ASSESSMENTNO , ")
                .Append("       NVL(T1.VEHICLENAME , ' ') AS VEHICLENAME , ")
                .Append("       NVL(T1.APPRISAL_PRICE, 0) AS APPRISAL_PRICE , ")
                .Append("       NVL(T2.ASSMNT_SEQ, 0) + 1 AS ASSMNTSEQ ")
                .Append("  FROM TBL_UCARASSESSMENT T1 , ")
                .Append("       TB_T_ASSESSMENT_ACT T2 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID(+) ")
                .Append("   AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("   AND T1.ASM_ACT_FLG = '1' ")
                .Append(" ORDER BY ")
                .Append("       T2.ASSMNT_SEQ  DESC ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActAsmInfo_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
    End Function
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 査定実績登録
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <param name="assmntseq">査定連番</param>
    ''' <param name="assmntvclname">査定モデル名</param>
    ''' <param name="staffcd">スタッフコード</param>
    ''' <param name="assmntprice">査定価格</param>
    ''' <param name="acount">行作成アカウント</param>
    ''' <param name="rowfunction">行作成機能</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SetAssessmentAct(ByVal salesid As Decimal, ByVal assmntseq As Long, ByVal assmntvclname As String,
                                                   ByVal staffcd As String, ByVal assmntprice As Long,
                                                   ByVal acount As String, ByVal rowfunction As String, ByVal orgnzid As Decimal) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetAssessmentAct_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT ")
            .AppendLine("    /* ActivityInfo_439 */ ")
            .AppendLine("INTO TB_T_ASSESSMENT_ACT (")
            .AppendLine("    SALES_ID ,")
            .AppendLine("    ASSMNT_SEQ ,")
            .AppendLine("    ASSMNT_VCL_NAME ,")
            .AppendLine("    ASSMNT_RSLT_FLG ,")
            .AppendLine("    ASSMNT_RSLT_DATE ,")
            .AppendLine("    RSLT_STF_CD ,")
            .AppendLine("    RSLT_DEPT_ID ,")
            .AppendLine("    ASSMNT_PRICE ,")
            .AppendLine("    ROW_CREATE_DATETIME ,")
            .AppendLine("    ROW_CREATE_ACCOUNT ,")
            .AppendLine("    ROW_CREATE_FUNCTION ,")
            .AppendLine("    ROW_UPDATE_DATETIME ,")
            .AppendLine("    ROW_UPDATE_ACCOUNT ,")
            .AppendLine("    ROW_UPDATE_FUNCTION ,")
            .AppendLine("    ROW_LOCK_VERSION")
            .AppendLine(")")
            .AppendLine("VALUES (")
            .AppendLine("    :SALESID ,")
            .AppendLine("    :ASSMNTSEQ ,")
            .AppendLine("    :ASSMNTVCLNAME ,")
            .AppendLine("    '1' ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :STAFFCD ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .AppendLine("    :ORGID ,")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            .AppendLine("    :ASSMNTPRICE ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    SYSDATE ,")
            .AppendLine("    :ACCOUNT ,")
            .AppendLine("    :FUNTION ,")
            .AppendLine("    0")
            .AppendLine(")")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_439")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
            query.AddParameterWithTypeValue("ASSMNTSEQ", OracleDbType.Long, assmntseq)
            query.AddParameterWithTypeValue("ASSMNTVCLNAME", OracleDbType.NVarchar2, assmntvclname)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("ASSMNTPRICE", OracleDbType.Long, assmntprice)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
            query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORGID", OracleDbType.Decimal, orgnzid)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetAssessmentAct_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 4.90.希望車更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <param name="estamount">金額</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdatePrefer(ByVal fllwupboxseqno As Decimal, ByVal seq As String, ByVal rsltcontactmtd As String, ByVal estamount As Long, ByVal rsltstaffcd As String,
                                    ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long, ByVal orgnzid As Decimal) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        Using query As New DBUpdateQuery("ActivityInfo_440")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_440 */")
                .AppendLine("    TB_T_PREFER_VCL")
                .AppendLine(" SET")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .AppendLine("    EST_RSLT_DATE = SYSDATE ,")
                .AppendLine("    EST_RSLT_CONTACT_MTD = :RSLT_CONTACT_MTD ,")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                .AppendLine("    EST_AMOUNT = :ESTAMOUNT ,")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .AppendLine("    EST_RSLT_FLG = '1' ,")
                .AppendLine("    EST_RSLT_STF_CD = :RSLT_STF_CD ,")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .AppendLine("    EST_RSLT_DEPT_ID = :ORGID ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO)")
                .AppendLine("    AND PREF_VCL_SEQ = TO_NUMBER(:SEQ)")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            If String.IsNullOrEmpty(rsltcontactmtd) Then
                rsltcontactmtd = " "
            End If
            query.AddParameterWithTypeValue("RSLT_CONTACT_MTD", OracleDbType.NVarchar2, rsltcontactmtd)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            query.AddParameterWithTypeValue("ESTAMOUNT", OracleDbType.NVarchar2, estamount)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("RSLT_STF_CD", OracleDbType.NVarchar2, rsltstaffcd)
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("ORGID", OracleDbType.Decimal, orgnzid)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function

    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END
    ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望車テーブルの商談ステータスを更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <param name="cractrslt">商談ステータス</param>
    ''' <param name="account">行更新アカウント</param>
    ''' <param name="rowuodatefunction">行更新機能</param>
    ''' <param name="rowlockversion">行ロックバージョン</param>
    ''' <param name="actid">活動ID</param>
    ''' <param name="salesbkgnum">成約No</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateSalesstatus(ByVal fllwupboxseqno As Decimal, ByVal seq As String, ByVal cractrslt As String, ByVal account As String,
                                             ByVal rowuodatefunction As String, ByVal rowlockversion As Long, ByVal actid As Decimal,
                                             ByVal salesbkgnum As String) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        Using query As New DBUpdateQuery("ActivityInfo_445")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_445 */")
                .AppendLine("    TB_T_PREFER_VCL")
                .AppendLine(" SET")
                .AppendLine("    SALES_STATUS = :SALESTATUS ,")
                '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
                .AppendLine("    SALESBKG_ACT_ID = :ACTID ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1,")
                .AppendLine("    SALESBKG_NUM = :CONTRACTNO ")
                '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
                .AppendLine(" WHERE")
                .AppendLine("        SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO)")
                .AppendLine("    AND PREF_VCL_SEQ = TO_NUMBER(:SEQ)")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
            query.AddParameterWithTypeValue("SALESTATUS", OracleDbType.NVarchar2, cractrslt)
            '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
            If cractrslt = "31" Then
                query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
            Else
                query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, 0)
            End If
            query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.NVarchar2, salesbkgnum)
            '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END
    ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 START
    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END

    ''' <summary>
    ''' 契約状況フラグの取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateContractFlg(ByVal fllwupboxseqno As Decimal, ByVal seq As String) As Integer
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
        Dim env As New SystemEnvSetting
        ' 外版色コードを前3桁だけで比較するか否かフラグ
        Dim extColor3Flg As String = String.Empty
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            extColor3Flg = "0"
        Else
            extColor3Flg = sysEnvRow.PARAMVALUE
        End If
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

        ' 2020/04/08 TS 髙橋(龍) TR-V4-TKM-20191227-001対応 START
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim useFlgSuffix As String
        Dim useFlgInteriorClr As String

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            useFlgSuffix = "0"
        Else
            useFlgSuffix = dataRow.SETTING_VAL
        End If

        '内装色使用可否フラグ(設定値が無ければ0)
        Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRowclr = systemBiz.GetSystemSetting(USE_FLG_INTERIORCLR)

        If IsNothing(dataRowclr) Then
            useFlgInteriorClr = "0"
        Else
            useFlgInteriorClr = dataRowclr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
        ' 2020/04/08 TS 髙橋(龍) TR-V4-TKM-20191227-001対応 END
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateContractFlg_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* ActivityInfo_446 */ ")
            .Append("    NVL(MAX(T1.CONTRACTFLG),0) ")
            .Append("FROM ")
            .Append("    TBL_ESTIMATEINFO T1, ")
            .Append("    TB_T_PREFER_VCL T2, ")
            .Append("    TBL_EST_VCLINFO T3 ")
            .Append("WHERE ")
            .Append("    T2.SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
            .Append("    AND T2.PREF_VCL_SEQ = TO_NUMBER(:SEQ) ")
            .Append("    AND T2.MODEL_CD = T3.SERIESCD ")
            .Append("    AND T2.GRADE_CD = T3.MODELCD ")
            '.Append("    AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
            If (extColor3Flg = "1") Then
                .Append("    AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
            Else
                .Append("    AND T2.BODYCLR_CD = T3.EXTCOLORCD ")
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
            ' 2020/04/08 TS 髙橋(龍) TR-V4-TKM-20191227-001対応 START
            'サフィックス使用可の場合
            If (USE_FLG_SUFFIX_TURE.Equals(useFlgSuffix)) Then
                .Append("    AND T2.SUFFIX_CD = T3.SUFFIXCD ")
            End If
            '内装色使用可の場合
            If (USE_INTERIOR_CLR_TURE.Equals(useFlgInteriorClr)) Then
                .Append("    AND T2.INTERIORCLR_CD = T3.INTCOLORCD ")
            End If
            ' 2020/04/08 TS 髙橋(龍) TR-V4-TKM-20191227-001対応 END
            .Append("    AND T1.ESTIMATEID = T3.ESTIMATEID ")
            .Append("    AND T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
            .Append("    AND T1.DELFLG = '0' ")

        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCountDataTable)("ActivityInfo_446")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateContractFlg_End")
            'ログ出力 End *****************************************************************************
            Return query.GetCount()

        End Using

    End Function
    '2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END
    ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 成約フラグ更新
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <param name="updateAccount"></param>
    ''' <param name="updateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Shared Function UpdateSuccessFlag(ByVal fllwupboxseqno As Decimal, ByVal seq As String,
                                          ByVal updateAccount As String, ByVal updateId As String) As Integer

        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
        Dim env As New SystemEnvSetting
        ' 外版色コードを前3桁だけで比較するか否かフラグ
        Dim extColor3Flg As String = String.Empty
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            extColor3Flg = "0"
        Else
            extColor3Flg = sysEnvRow.PARAMVALUE
        End If
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

        Dim sql As New StringBuilder
        With sql
            .AppendLine("UPDATE /* ActivityInfo_447 */")
            .AppendLine("       TBL_ESTIMATEINFO T1 ")
            .AppendLine("   SET T1.SUCCESSFLG = '1' ")
            .AppendLine("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT ")
            .AppendLine("     , T1.UPDATEDATE = SYSDATE ")
            .AppendLine("     , T1.UPDATEID  = :UPDATEID ")
            .AppendLine(" WHERE EXISTS ( ")
            .AppendLine("   SELECT ")
            .AppendLine("       * ")
            .AppendLine("   FROM ")
            .AppendLine("       TB_T_PREFER_VCL T2, ")
            .AppendLine("       TBL_EST_VCLINFO T3 ")
            .AppendLine("   WHERE ")
            .AppendLine("       T2.SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
            .AppendLine("        AND T2.PREF_VCL_SEQ = TO_NUMBER(:SEQ) ")
            .AppendLine("        AND T2.MODEL_CD = T3.SERIESCD ")
            .AppendLine("        AND T2.GRADE_CD = T3.MODELCD ")
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            If (extColor3Flg = "1") Then
                .AppendLine("        AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
            Else
                .AppendLine("        AND T2.BODYCLR_CD = T3.EXTCOLORCD ")
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
            .AppendLine("        AND T1.ESTIMATEID = T3.ESTIMATEID ")
            .AppendLine("        AND T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
            .AppendLine(" ) ")
            .AppendLine("  AND T1.DELFLG = '0' ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_447")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
            Return query.Execute()
        End Using

    End Function
    '2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致ステータスチェック
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AttractStatusCheck(ByVal attid As Decimal) As Integer

        Dim sql As New StringBuilder

        With sql
            .Append("SELECT /* ActivityInfo_501 */ ")
            .Append("    SUM(CNT) ")
            .Append("FROM( ")
            .Append("    SELECT COUNT(*) AS CNT ")
            .Append("    FROM TB_T_ATTRACT_DM ")
            .Append("    WHERE ATT_ID = :ATT_ID ")
            .Append("      AND CONSTRAINT_STATUS = '1' ")
            .Append("      AND RSLT_FLG <> '1'  ")
            .Append("    UNION ALL ")
            .Append("    SELECT COUNT(*) AS CNT ")
            .Append("    FROM TB_T_ATTRACT_RMM ")
            .Append("    WHERE ATT_ID = :ATT_ID ")
            .Append("      AND CONSTRAINT_STATUS = '1' ")
            .Append("      AND RSLT_FLG <> '1'  ")
            .Append(") ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_501")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)

            query.CommandText = sql.ToString()
            Return query.GetCount()

        End Using

    End Function
    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.110.DM誘致活動テーブル移動
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveAttractDM(ByVal attid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractDM_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_502 */")
            .AppendLine("INTO TB_H_ATTRACT_DM")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ATTRACT_DM")
            .AppendLine("    WHERE")
            .AppendLine("      ATT_ID = :ATTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_502")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractDM_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.121.DM誘致活動テーブル削除
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteAttractDM(ByVal attid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_503")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractDM_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_503 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ATTRACT_DM")
                .AppendLine("WHERE")
                .AppendLine("    ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractDM_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END




    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.110.RMM誘致活動テーブル移動
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MoveAttractRMM(ByVal attid As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractRMM_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("INSERT")
            .AppendLine("    /* ActivityInfo_504 */")
            .AppendLine("INTO TB_H_ATTRACT_RMM")
            .AppendLine("    SELECT")
            .AppendLine("      *")
            .AppendLine("    FROM")
            .AppendLine("      TB_T_ATTRACT_RMM")
            .AppendLine("    WHERE")
            .AppendLine("      ATT_ID = :ATTID")
        End With
        Using query As New DBUpdateQuery("ActivityInfo_504")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveAttractRMM_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.121.DM誘致活動テーブル削除
    ''' </summary>
    ''' <param name="attid">誘致ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteAttractRMM(ByVal attid As Decimal) As Integer
        Using query As New DBUpdateQuery("ActivityInfo_505")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractRMM_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("DELETE")
                .AppendLine("    /* ActivityInfo_505 */")
                .AppendLine("FROM")
                .AppendLine("    TB_T_ATTRACT_RMM")
                .AppendLine("WHERE")
                .AppendLine("    ATT_ID = :ATTID")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attid)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractRMM_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()
        End Using
    End Function
    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

    ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 4.506.組織ID取得
    ''' </summary>
    ''' <returns>組織ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetorgnzId(ByVal stfcd As String) As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetorgnzId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine("SELECT")
            .AppendLine("  /* ActivityInfo_506 */")
            .AppendLine("    ORGNZ_ID ")
            .AppendLine("FROM ")
            .AppendLine("    TB_M_STAFF ")
            .AppendLine("WHERE ")
            .AppendLine("    STF_CD = :STFCD ")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_506")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("STFCD", OracleDbType.NVarchar2, stfcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetorgnzId_End")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 END

    '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
    ''' <summary>
    ''' 4.100.注文番号取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">商談ID</param>
    ''' <param name="seq">希望車シーケンス</param>
    ''' <remarks></remarks>

    Public Shared Function GetSalesbkgNum(ByVal fllwupboxseqno As Decimal, ByVal seq As String) As String

        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
        Dim env As New SystemEnvSetting
        ' 外版色コードを前3桁だけで比較するか否かフラグ
        Dim extColor3Flg As String = String.Empty
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        sysEnvRow = env.GetSystemEnvSetting(EXTERIOR_COLOR_3_FLG)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            extColor3Flg = "0"
        Else
            extColor3Flg = sysEnvRow.PARAMVALUE
        End If
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesbkgNum")
        'ログ出力 End *****************************************************************************
        With sql
            .AppendLine(" SELECT")
            .AppendLine("  /*ActivityInfo_507 */")
            .AppendLine(" CONTRACTNO ")
            .AppendLine(" FROM")
            .AppendLine("     TBL_ESTIMATEINFO T1 ")
            .AppendLine(" WHERE EXISTS ( ")
            .AppendLine("     SELECT ")
            .AppendLine("          * ")
            .AppendLine("     FROM ")
            .AppendLine("          TB_T_PREFER_VCL T2, ")
            .AppendLine("          TBL_EST_VCLINFO T3 ")
            .AppendLine("      WHERE ")
            .AppendLine("          T2.SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
            .AppendLine("      AND T2.PREF_VCL_SEQ = TO_NUMBER(:SEQ) ")
            .AppendLine("      AND T2.MODEL_CD = T3.SERIESCD ")
            .AppendLine("      AND T2.GRADE_CD = T3.MODELCD ")
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
            If (extColor3Flg = "1") Then
                .AppendLine("      AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
            Else
                .AppendLine("      AND T2.BODYCLR_CD = T3.EXTCOLORCD ")
            End If
            ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
            .AppendLine("      AND T1.ESTIMATEID = T3.ESTIMATEID ")
            .AppendLine("      AND T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
            .AppendLine(" ) ")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_507")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesbkgNum")
            'ログ出力 End *****************************************************************************
            Return Trim(query.GetData()(0)(0).ToString)
        End Using


    End Function
    ''2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END

#Region "Aカード情報相互連携開発"
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START

    ''' <summary>
    ''' 入力チェック設定取得
    ''' </summary>
    ''' <param name="checkTimingType">チェックタイミング区分</param>
    ''' <returns></returns>
    ''' <remarks>指定されたチェックタイミング区分にて使用する必須チェック有無を取得する。</remarks>
    Public Shared Function GetSettingsInputCheck(ByVal checkTimingType As String) As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable

        Dim ret As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable = Nothing
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSettingsInputCheck_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_601 */  ")
                .AppendLine("    TGT_ITEM_ID ")
                .AppendLine("    ,TGT_ITEM ")
                .AppendLine("    ,CASE DISP_SETTING_STATUS WHEN N'2' THEN 'True' ELSE 'False' END IS_CHECKTARGET ")
                .AppendLine("FROM TBL_INPUT_ITEM_SETTING ")
                .AppendLine("WHERE CHECK_TIMING_TYPE = :CHECK_TIMING_TYPE ")
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable)("ActivityInfo_601")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("CHECK_TIMING_TYPE", OracleDbType.NVarchar2, checkTimingType)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSettingsInputCheck_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function


    ''' <summary>
    ''' 商談一時情報データロック
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>処理結果（ロック成功行数）</returns>
    ''' <remarks>商談一時情報を行ロックする。</remarks>
    Public Shared Function LockSalesTemp(ByVal salesId As Decimal) As Integer

        Dim ret As Integer = 0
        Dim sql As New StringBuilder(10000)
        Dim env As New SystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockSalesTemp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_602 */ ")
                .AppendLine("   1 ")
                .AppendLine("FROM TB_T_SALES_TEMP ")
                .AppendLine("WHERE SALES_ID = :SALES_ID ")
                .AppendFormat("FOR UPDATE WAIT {0} ", env.GetLockWaitTime())
            End With

            Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_602")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = query.GetCount()
            End Using
        Finally
            sql.Clear()
            env = Nothing
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("LockSalesTemp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function


    ''' <summary>
    ''' 商談一時情報移動
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>処理結果（True：移動成功／False：移動失敗）</returns>
    ''' <remarks>商談一時情報をDELテーブルへ移動（商談・用件データ追加後に利用する。）</remarks>
    Public Shared Function MoveSalesTemp(ByVal salesId As Decimal) As Boolean

        Dim ret As Boolean = False
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSalesTemp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("INSERT /* ActivityInfo_603 */ ")
                .AppendLine("INTO TB_T_SALES_TEMP_DEL ")
                .AppendLine("SELECT * ")
                .AppendLine("FROM TB_T_SALES_TEMP ")
                .AppendLine("WHERE SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("ActivityInfo_603")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = (query.Execute() = 1)
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveSalesTemp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function


    ''' <summary>
    ''' 商談一時情報削除
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>処理結果（True：削除成功／False：削除失敗）</returns>
    ''' <remarks>商談一時情報を削除（商談・用件データ追加後に利用する。）</remarks>
    Public Shared Function DeleteSalesTemp(ByVal salesId As Decimal) As Boolean

        Dim ret As Boolean = False
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSalesTemp_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("DELETE /* ActivityInfo_604 */  ")
                .AppendLine("FROM TB_T_SALES_TEMP ")
                .AppendLine("WHERE SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("ActivityInfo_604")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = (query.Execute() = 1)
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSalesTemp_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' Aカード番号更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="aCardNum">Aカード番号</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>処理結果（True：更新成功／False：更新失敗）</returns>
    ''' <remarks>商談テーブルに（活動登録完了後、基幹連携により採番された）Aカード番号登録</remarks>
    Public Shared Function UpdateSalesACardNum(ByVal salesId As Decimal, ByVal aCardNum As String _
                                               , ByVal updateAccount As String) As Boolean

        Dim ret As Boolean = False
        Dim sql As New StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSalesACardNum_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("UPDATE /* ActivityInfo_605 */  ")
                .AppendLine("    TB_T_SALES ")
                .AppendLine("SET ")
                .AppendLine("    ACARD_NUM = :ACARD_NUM ")
                .AppendLine("   ,ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("   ,ROW_UPDATE_ACCOUNT = :STF_CD ")
                .AppendLine("   ,ROW_UPDATE_FUNCTION = 'SC3080203' ")
                .AppendLine("   ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                .AppendLine("WHERE SALES_ID = :SALES_ID ")
            End With

            Using query As New DBUpdateQuery("ActivityInfo_605")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("ACARD_NUM", OracleDbType.NVarchar2, aCardNum)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, updateAccount)

                ret = (query.Execute() = 1)
            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSalesACardNum_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    ''' <summary>
    ''' 入力チェック用情報取得（顧客）
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks>（入力チェック用の）顧客情報（顧客・販売店顧客車両 他）を取得する。</remarks>
    Public Shared Function GetCustomerInfoForCheck(ByVal dlrCD As String, ByVal cstId As Decimal) As ActivityInfoDataSet.CustomerInfoForCheckDataTable

        Dim ret As ActivityInfoDataSet.CustomerInfoForCheckDataTable = Nothing
        Dim sql As New Text.StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoForCheck_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_606 */ ")
                .AppendLine("     CST.CST_ID ")
                .AppendLine("    ,CST.FIRST_NAME ")
                .AppendLine("    ,CST.MIDDLE_NAME ")
                .AppendLine("    ,CST.LAST_NAME ")
                .AppendLine("    ,CST.NAMETITLE_CD ")
                .AppendLine("    ,CST.NAMETITLE_NAME ")
                .AppendLine("    ,CST.CST_GENDER ")
                .AppendLine("    ,CST.FLEET_FLG  ")
                .AppendLine("    ,PFI.PRIVATE_FLEET_ITEM_CD  ")
                .AppendLine("    ,CST.FLEET_PIC_NAME  ")
                .AppendLine("    ,CST.FLEET_PIC_DEPT  ")
                .AppendLine("    ,CST.FLEET_PIC_POSITION  ")
                .AppendLine("    ,CST.CST_PHONE ")
                .AppendLine("    ,CST.CST_MOBILE ")
                .AppendLine("    ,CST.CST_BIZ_PHONE ")
                .AppendLine("    ,CST.CST_FAX ")
                .AppendLine("    ,CST.CST_ZIPCD ")
                .AppendLine("    ,CST.CST_ADDRESS_1 ")
                .AppendLine("    ,CST.CST_ADDRESS_2 ")
                .AppendLine("    ,CST.CST_ADDRESS_3 ")
                .AppendLine("    ,CST.CST_ADDRESS_STATE ")
                .AppendLine("    ,CST.CST_ADDRESS_DISTRICT ")
                .AppendLine("    ,CST.CST_ADDRESS_CITY ")
                .AppendLine("    ,CST.CST_ADDRESS_LOCATION ")
                .AppendLine("    ,CST.CST_DOMICILE ")
                .AppendLine("    ,CST.CST_EMAIL_1 ")
                .AppendLine("    ,CST.CST_EMAIL_2 ")
                .AppendLine("    ,CST.CST_COUNTRY ")
                .AppendLine("    ,CST.CST_SOCIALNUM ")
                .AppendLine("    ,CST.CST_BIRTH_DATE ")
                .AppendLine("    ,RCV.ACT_CAT_TYPE ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                .AppendLine("    ,RCD.CST_TYPE ")
                .AppendLine("    ,CASE WHEN LCST.CST_ORGNZ_INPUT_TYPE = '1' THEN NVL(CSTORZ.CST_ORGNZ_CD, ' ') ")
                .AppendLine("          ELSE N' ' END AS CST_ORGNZ_CD ")
                .AppendLine("    ,LCST.CST_ORGNZ_INPUT_TYPE AS CST_ORGNZ_INPUT_TYPE ")
                .AppendLine("    ,CASE WHEN LCST.CST_ORGNZ_INPUT_TYPE = '2' AND (PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '0' OR PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '2') THEN NVL(LCST.CST_ORGNZ_NAME, ' ') ")
                .AppendLine("          WHEN LCST.CST_ORGNZ_INPUT_TYPE = '1' AND (PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '1' OR PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '2') THEN NVL(CSTORZ.CST_ORGNZ_NAME, ' ') ")
                .AppendLine("          ELSE N' ' END AS CST_ORGNZ_NAME ")
                .AppendLine("    ,CASE WHEN LCST.CST_ORGNZ_INPUT_TYPE = '2' AND (PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '0' OR PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '2') THEN NVL(CSTSUB2.CST_SUBCAT2_CD, ' ') ")
                .AppendLine("          WHEN LCST.CST_ORGNZ_INPUT_TYPE = '1' AND (PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '1' OR PFIL.CST_ORGNZ_NAME_INPUT_TYPE = '2') ")
                .AppendLine("               AND CSTSUB2.CST_ORGNZ_CD = LCST.CST_ORGNZ_CD AND CSTSUB2.CST_ORGNZ_CD = CSTORZ.CST_ORGNZ_CD THEN NVL(CSTSUB2.CST_SUBCAT2_CD, ' ') ")
                .AppendLine("          ELSE N' ' END AS CST_SUBCAT2_CD ")
                .AppendLine(" FROM ")
                .AppendLine("               TB_M_CUSTOMER CST ")
                .AppendLine("    INNER JOIN TB_M_CUSTOMER_VCL RCV ON RCV.CST_ID = CST.CST_ID ")
                .AppendLine("                                    AND RCV.DLR_CD = :DLR_CD ")
                .AppendLine("    INNER JOIN TB_M_CUSTOMER_DLR RCD ON RCD.CST_ID = CST.CST_ID ")
                .AppendLine("                                    AND RCD.DLR_CD = RCV.DLR_CD ")
                .AppendLine("     LEFT JOIN TB_LM_CUSTOMER LCST ON LCST.CST_ID = CST.CST_ID ")
                .AppendLine("     LEFT JOIN TB_M_PRIVATE_FLEET_ITEM PFI ON PFI.PRIVATE_FLEET_ITEM_CD = CST.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("                                          AND PFI.FLEET_FLG = CST.FLEET_FLG ")
                .AppendLine("                                          AND PFI.INUSE_FLG = '1' ")
                .AppendLine("     LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM PFIL ON PFIL.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("     LEFT JOIN TB_LM_CUSTOMER_ORGANIZATION CSTORZ ON CSTORZ.CST_ORGNZ_CD = LCST.CST_ORGNZ_CD ")
                .AppendLine("                                                 AND CSTORZ.PRIVATE_FLEET_ITEM_CD = CST.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("                                                 AND CSTORZ.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("                                                 AND CSTORZ.INUSE_FLG = '1' ")
                .AppendLine("     LEFT JOIN TB_LM_CUSTOMER_SUBCATEGORY2 CSTSUB2 ON CSTSUB2.CST_SUBCAT2_CD = LCST.CST_SUBCAT2_CD ")
                .AppendLine("                                                  AND CSTSUB2.PRIVATE_FLEET_ITEM_CD = CST.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("                                                  AND CSTSUB2.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD ")
                .AppendLine("                                                  AND CSTSUB2.INUSE_FLG = '1' ")
                .AppendLine(" WHERE ")
                .AppendLine("    CST.CST_ID = :CST_ID ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.CustomerInfoForCheckDataTable)("ActivityInfo_606")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoForCheck_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
    ''' <summary>
    ''' 入力チェック用情報取得（商談）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks>(入力チェック用の)商談情報（商談・用件・誘致・商談一時情報）を取得する</remarks>
    Public Shared Function GetSalesInfoForCheck(ByVal salesId As Decimal) As ActivityInfoDataSet.SalesInfoForCheckDataTable

        Dim ret As ActivityInfoDataSet.SalesInfoForCheckDataTable = Nothing
        Dim sql As New Text.StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesInfoForCheck_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_607 */ ")
                .AppendLine("     SLS.SALES_ID ")
                .AppendLine("    ,NVL(REQ.SOURCE_1_CD,ATT.SOURCE_1_CD) AS SOURCE_1_CD ")
                .AppendLine("    ,NVL(REQ.SOURCE_2_CD,ATT.SOURCE_2_CD) AS SOURCE_2_CD ")
                .AppendLine("FROM ")
                .AppendLine("     TB_T_SALES SLS ")
                .AppendLine("    ,TB_T_REQUEST REQ ")
                .AppendLine("    ,TB_T_ATTRACT ATT ")
                .AppendLine("WHERE ")
                .AppendLine("           SLS.REQ_ID = REQ.REQ_ID(+) ")
                .AppendLine("    AND SLS.ATT_ID = ATT.ATT_ID(+) ")
                .AppendLine("    AND SLS.SALES_ID = :SALES_ID ")
                .AppendLine("UNION ALL ")
                .AppendLine("SELECT ")
                .AppendLine("     SLST.SALES_ID ")
                .AppendLine("    ,SLST.SOURCE_1_CD ")
                .AppendLine("    ,NVL(LSLS.SOURCE_2_CD, 0) AS SOURCE_2_CD ")
                .AppendLine("FROM ")
                .AppendLine("    TB_T_SALES_TEMP SLST ")
                .AppendLine("   ,TB_LT_SALES LSLS ")
                .AppendLine("WHERE ")
                .AppendLine("        SLST.SALES_ID = :SALES_ID ")
                .AppendLine(" AND SLST.SALES_ID = LSLS.SALES_ID(+) ")
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.SalesInfoForCheckDataTable)("ActivityInfo_607")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesInfoForCheck_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function
    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END

    ''' <summary>
    ''' 指定店舗の全組織を取得する。(チームメンバー判定用)
    ''' </summary>
    ''' <param name="dlrCd">販売店</param>
    ''' <param name="brnCd">店舗</param>
    ''' <param name="salesStaffCd">セールス担当スタッフアカウント</param>
    ''' <param name="teamLeaderOrgnzId">チームリーダー組織ID</param>
    ''' <returns>指定店舗の全組織</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMyBranchOrganizations(ByVal dlrCd As String, ByVal brnCd As String, _
                                                    ByVal salesStaffCd As String, ByVal teamLeaderOrgnzId As Decimal) As ActivityInfoDataSet.MyBranchOrganizationsDataTable

        Dim ret As ActivityInfoDataSet.MyBranchOrganizationsDataTable = Nothing
        Dim sql As New Text.StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMyBranchOrganizations_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_609 */  ")
                .AppendLine("	 ORG.ORGNZ_ID ")
                .AppendLine("	,ORG.ORGNZ_SC_FLG  ")
                .AppendLine("	,ORG.PARENT_ORGNZ_ID ")
                .AppendLine("	,CASE WHEN STF.STF_CD IS NOT NULL THEN 'TRUE' ELSE 'FALSE' END AS IsSalesStaffOrgnz ")
                .AppendLine("	,CASE WHEN ORG.ORGNZ_ID=:TL_ORG_ID THEN 'TRUE' ELSE 'FALSE' END AS IsTeamLeaderOrgnz ")
                .AppendLine("FROM TB_M_ORGANIZATION ORG  ")
                .AppendLine("	,TB_M_STAFF STF  ")
                .AppendLine("WHERE  ")
                .AppendLine("	ORG.DLR_CD = :DLR_CD  ")
                .AppendLine("	AND ORG.BRN_CD = :BRN_CD  ")
                .AppendLine("	AND ORG.INUSE_FLG = '1'  ")
                .AppendLine("	AND ORG.ORGNZ_ID = STF.ORGNZ_ID(+)  ")
                .AppendLine("	AND STF.STF_CD(+) = :SALES_STF_CD  ")
                .AppendLine("	AND STF.INUSE_FLG(+) = '1'  ")
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.MyBranchOrganizationsDataTable)("ActivityInfo_609")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                query.AddParameterWithTypeValue("TL_ORG_ID", OracleDbType.Decimal, teamLeaderOrgnzId)
                query.AddParameterWithTypeValue("SALES_STF_CD", OracleDbType.NVarchar2, salesStaffCd)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMyBranchOrganizations_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    ''' <summary>
    ''' 入力チェック用情報取得（商談条件） 
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="checkTimingType">チェックタイミング区分</param>
    ''' <param name="targetItemID">チェック項目ID（商談条件）</param>
    ''' <returns></returns>
    ''' <remarks>（入力チェック用の）商談条件を取得する。</remarks>
    Public Shared Function GetSalesConditionsForCheck(ByVal salesId As Decimal, ByVal checkTimingType As String, ByVal targetItemID As String) As ActivityInfoDataSet.SalesConditionsForCheckDataTable
        Dim ret As ActivityInfoDataSet.SalesConditionsForCheckDataTable = Nothing
        Dim sql As New Text.StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesConditionsForCheck_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_608 */ ")
                .AppendLine("    T2.SALESCONDITIONNO ")
                .AppendLine("    , T2.TITLE ")
                .AppendLine("    , CASE T1.DISP_SETTING_STATUS WHEN N'2' THEN 'True' ELSE 'False' END IsMandatory ")
                .AppendLine("    , COUNT(T3.ITEMNO) AS SelectedValues ")
                .AppendLine("FROM  ")
                .AppendLine("    TBL_INPUT_ITEM_SETTING T1 ")
                .AppendLine("    ,TBL_SALESCONDITION T2 ")
                .AppendLine("    ,TBL_FLLWUPBOX_SALESCONDITION T3 ")
                .AppendLine("WHERE  ")
                .AppendLine("    T1.TGT_ITEM_DETAIL_ID = CAST(T2.SALESCONDITIONNO AS NVARCHAR2(10)) ")
                .AppendLine("    AND T2.SALESCONDITIONNO = T3.SALESCONDITIONNO(+) ")
                .AppendLine("    AND T1.CHECK_TIMING_TYPE = :CHECK_TIMING_TYPE ")
                .AppendLine("    AND T1.TGT_ITEM_ID = :TGT_ITEM_ID ")
                .AppendLine("    AND T2.DELFLG = '0' ")
                .AppendLine("    AND T3.FLLWUPBOX_SEQNO(+) = :SALES_ID ")
                .AppendLine("GROUP BY ")
                .AppendLine("   T2.SALESCONDITIONNO ")
                .AppendLine("   ,T2.TITLE ")
                .AppendLine("   ,T1.DISP_SETTING_STATUS ")
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.SalesConditionsForCheckDataTable)("ActivityInfo_608")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                query.AddParameterWithTypeValue("CHECK_TIMING_TYPE", OracleDbType.NVarchar2, checkTimingType)
                query.AddParameterWithTypeValue("TGT_ITEM_ID", OracleDbType.NVarchar2, targetItemID)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesConditionsForCheck_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
#End Region

#Region "受注後フォロー機能開発"
    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注後プロセスマスタ取得
    ''' </summary>
    ''' <param name="afterOdrPrcsCd">受注後工程コード</param>
    ''' <returns>受注後プロセスマスタ情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterProcessMaster(ByVal afterOdrPrcsCd As String) As ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterProcessMaster_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoBookedAfterProcessMasterDataTable)("ActivityInfo_609")
            With sql
                .Append(" SELECT /* ActivityInfo_609 */ ")
                .Append("     T1.AFTER_ODR_PRCS_CD, ")
                .Append("     CASE WHEN T4.WORD_VAL IS NULL THEN NULL ")
                .Append("          WHEN T4.WORD_VAL = ' ' THEN TRIM(T4.WORD_VAL_ENG) ")
                .Append("          ELSE TRIM(T4.WORD_VAL) ")
                .Append("     END AS AFTER_ODR_PRCS_NAME, ")
                .Append("     T2.ICON_PATH AS ICON_PATH_ON, ")
                .Append("     T3.ICON_PATH AS ICON_PATH_OFF ")
                .Append(" FROM ")
                .Append("     TB_M_AFTER_ODR_PROC T1, ")
                .Append("     TB_M_IMG_PATH_CONTROL T2, ")
                .Append("     TB_M_IMG_PATH_CONTROL T3, ")
                .Append("     TB_M_WORD T4 ")
                .Append(" WHERE ")
                .Append("         T1.AFTER_ODR_PRCS_CD IN ")
                .Append("     (SELECT AFTER_ODR_PRCS_CD ")
                .Append("      FROM TB_M_AFTER_ODR_ACT ")
                .Append("      WHERE AFTER_ODR_PRCS_CD != :AFTER_ODR_PRCS_CD ")
                .Append("      AND MANDATORY_ACT_FLG = '1' ")
                .Append("      GROUP BY AFTER_ODR_PRCS_CD) ")
                .Append("     AND T2.DLR_CD = 'XXXXX' ")
                .Append("     AND T2.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("     AND T2.DEVICE_TYPE = '01' ")
                .Append("     AND T2.FIRST_KEY = T1.AFTER_ODR_PRCS_CD ")
                .Append("     AND T2.SECOND_KEY = '01' ")
                .Append("     AND T3.DLR_CD = 'XXXXX' ")
                .Append("     AND T3.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
                .Append("     AND T3.DEVICE_TYPE = '01' ")
                .Append("     AND T3.FIRST_KEY = T1.AFTER_ODR_PRCS_CD ")
                .Append("     AND T3.SECOND_KEY = '00' ")
                .Append("     AND T1.AFTER_ODR_PRCS_NAME = T4.WORD_CD(+) ")
                .Append(" ORDER BY ")
                .Append("     T1.SORT_ORDER ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_PRCS_CD", OracleDbType.NVarchar2, afterOdrPrcsCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterProcessMaster_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 受注後プロセス実績取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="afterOdrPrcsCd">受注後工程コード</param>
    ''' <returns>受注後プロセス実績情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterProcessResult(ByVal salesId As Decimal, _
                                                       ByVal afterOdrPrcsCd As String) As ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterProcessResult_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder()

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoBookedAfterProcessResultDataTable)("ActivityInfo_610")
            With sql
                .Append(" SELECT /* ActivityInfo_610 */ ")
                .Append("     DISTINCT(T7.AFTER_ODR_PRCS_CD), ")
                .Append("     T7.RSLT_DATE, ")
                .Append("     T7.CHECKFLG, ")
                .Append("     T8.RSLT_DATEORTIME_FLG, ")
                .Append("     T7.SORT_ORDER ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("     SELECT ")
                .Append("         T6.AFTER_ODR_PRCS_CD, ")
                .Append("         CASE ")
                .Append("         WHEN SUM(T6.NULLCNT) = SUM(T6.MANDATORY_ACT_FLG) THEN ")
                .Append("             MAX(T6.RSLT_DATE) ")
                .Append("         ELSE ")
                .Append("             NULL ")
                .Append("         END AS RSLT_DATE, ")
                .Append("         MAX(T6.FLG) AS CHECKFLG, ")
                .Append("         T6.SORT_ORDER ")
                .Append("     FROM ")
                .Append("         ( ")
                .Append("         SELECT ")
                .Append("             T3.AFTER_ODR_PRCS_CD, ")
                .Append("             T3.MANDATORY_ACT_FLG, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,'1', DECODE(T3.MANDATORY_ACT_FLG,'1',RSLT_END_DATEORTIME,NULL) ,NULL) AS RSLT_DATE, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,'1', DECODE(T3.MANDATORY_ACT_FLG,'1',1,0) ,0) AS NULLCNT, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,NULL,0,1) AS FLG, ")
                .Append("             T5.SORT_ORDER ")
                .Append("         FROM ")
                .Append("             TB_M_AFTER_ODR_ACT T3, ")
                .Append("             ( ")
                .Append("             SELECT ")
                .Append("                 T2.RSLT_END_DATEORTIME, ")
                .Append("                 T2.AFTER_ODR_ACT_CD, ")
                .Append("                 T2.AFTER_ODR_ACT_STATUS ")
                .Append("             FROM ")
                .Append("                 TB_T_AFTER_ODR T1, ")
                .Append("                 TB_T_AFTER_ODR_ACT T2 ")
                .Append("             WHERE ")
                .Append("                     T1.SALES_ID = :SALES_ID ")
                .Append("                 AND T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .Append("             ) T4, ")
                .Append("             TB_M_AFTER_ODR_PROC T5 ")
                .Append("         WHERE ")
                .Append("                 T3.AFTER_ODR_ACT_CD = T4.AFTER_ODR_ACT_CD ")
                .Append("             AND T3.AFTER_ODR_PRCS_CD = T5.AFTER_ODR_PRCS_CD ")
                .Append("             AND T5.AFTER_ODR_PRCS_CD != :AFTER_ODR_PRCS_CD ")
                .Append("         ) T6 ")
                .Append("     GROUP BY ")
                .Append("         T6.AFTER_ODR_PRCS_CD, ")
                .Append("         T6.SORT_ORDER ")
                .Append("     ) T7 ")
                .Append("     INNER JOIN TB_T_AFTER_ODR T9 ")
                .Append("         ON T9.SALES_ID = :SALES_ID ")
                .Append("     LEFT JOIN TB_T_AFTER_ODR_ACT T8 ")
                .Append("         ON T8.AFTER_ODR_ID = T9.AFTER_ODR_ID ")
                .Append("        AND T8.AFTER_ODR_PRCS_CD = T7.AFTER_ODR_PRCS_CD ")
                .Append("        AND T8.RSLT_END_DATEORTIME = T7.RSLT_DATE ")
                .Append(" UNION ALL ")
                .Append(" SELECT /* ActivityInfo_610 */ ")
                .Append("     DISTINCT(T7.AFTER_ODR_PRCS_CD), ")
                .Append("     T7.RSLT_DATE, ")
                .Append("     T7.CHECKFLG, ")
                .Append("     T8.RSLT_DATEORTIME_FLG, ")
                .Append("     T7.SORT_ORDER ")
                .Append(" FROM ")
                .Append("     ( ")
                .Append("     SELECT ")
                .Append("         T6.AFTER_ODR_PRCS_CD, ")
                .Append("         CASE ")
                .Append("         WHEN SUM(T6.NULLCNT) = SUM(T6.MANDATORY_ACT_FLG) THEN ")
                .Append("             MAX(T6.RSLT_DATE) ")
                .Append("         ELSE ")
                .Append("             NULL ")
                .Append("         END AS RSLT_DATE, ")
                .Append("         MAX(T6.FLG) AS CHECKFLG, ")
                .Append("         T6.SORT_ORDER ")
                .Append("     FROM ")
                .Append("         ( ")
                .Append("         SELECT ")
                .Append("             T3.AFTER_ODR_PRCS_CD, ")
                .Append("             T3.MANDATORY_ACT_FLG, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,'1', DECODE(T3.MANDATORY_ACT_FLG,'1',RSLT_END_DATEORTIME,NULL) ,NULL) AS RSLT_DATE, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,'1', DECODE(T3.MANDATORY_ACT_FLG,'1',1,0) ,0) AS NULLCNT, ")
                .Append("             DECODE(T4.AFTER_ODR_ACT_STATUS,NULL,0,1) AS FLG, ")
                .Append("             T5.SORT_ORDER ")
                .Append("         FROM ")
                .Append("             TB_M_AFTER_ODR_ACT T3, ")
                .Append("             ( ")
                .Append("             SELECT ")
                .Append("                 T2.RSLT_END_DATEORTIME, ")
                .Append("                 T2.AFTER_ODR_ACT_CD, ")
                .Append("                 T2.AFTER_ODR_ACT_STATUS ")
                .Append("             FROM ")
                .Append("                 TB_H_AFTER_ODR T1, ")
                .Append("                 TB_H_AFTER_ODR_ACT T2 ")
                .Append("             WHERE ")
                .Append("                     T1.SALES_ID = :SALES_ID ")
                .Append("                 AND T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
                .Append("             ) T4, ")
                .Append("             TB_M_AFTER_ODR_PROC T5 ")
                .Append("         WHERE ")
                .Append("                 T3.AFTER_ODR_ACT_CD = T4.AFTER_ODR_ACT_CD ")
                .Append("             AND T3.AFTER_ODR_PRCS_CD = T5.AFTER_ODR_PRCS_CD ")
                .Append("             AND T5.AFTER_ODR_PRCS_CD != :AFTER_ODR_PRCS_CD ")
                .Append("         ) T6 ")
                .Append("     GROUP BY ")
                .Append("         T6.AFTER_ODR_PRCS_CD, ")
                .Append("         T6.SORT_ORDER ")
                .Append("     ) T7 ")
                .Append("     INNER JOIN TB_H_AFTER_ODR T9 ")
                .Append("         ON T9.SALES_ID = :SALES_ID ")
                .Append("     LEFT JOIN TB_H_AFTER_ODR_ACT T8 ")
                .Append("         ON T8.AFTER_ODR_ID = T9.AFTER_ODR_ID ")
                .Append("        AND T8.AFTER_ODR_PRCS_CD = T7.AFTER_ODR_PRCS_CD ")
                .Append("        AND T8.RSLT_END_DATEORTIME = T7.RSLT_DATE ")
                .Append(" ORDER BY SORT_ORDER ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_PRCS_CD", OracleDbType.NVarchar2, afterOdrPrcsCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterProcessResult_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()
        End Using
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2014/04/18 TCS 森 受注後フォロー機能開発 START
    ''' <summary>
    ''' 入力チェック用情報取得（商談）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks>(入力チェック用の)商談情報（商談・用件・誘致・商談一時情報）を取得する</remarks>
    Public Shared Function GetSalesHistInfoForCheck(ByVal salesId As Decimal) As ActivityInfoDataSet.SalesInfoForCheckDataTable

        Dim ret As ActivityInfoDataSet.SalesInfoForCheckDataTable = Nothing
        Dim sql As New Text.StringBuilder(10000)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesHistInfoForCheck_Start")
        'ログ出力 End *****************************************************************************

        Try
            With sql
                .AppendLine("SELECT /* ActivityInfo_611 */  ")
                .AppendLine("    SLS.SALES_ID ")
                .AppendLine("    ,NVL(REQ.SOURCE_1_CD,ATT.SOURCE_1_CD) AS SOURCE_1_CD ")
                .AppendLine("    ,SLS.BRAND_RECOGNITION_ID ")
                .AppendLine("FROM  ")
                .AppendLine("    TB_H_SALES SLS ")
                .AppendLine("    ,TB_H_REQUEST REQ ")
                .AppendLine("    ,TB_H_ATTRACT ATT ")
                .AppendLine("WHERE  ")
                .AppendLine("    SLS.REQ_ID = REQ.REQ_ID(+)  ")
                .AppendLine("    AND SLS.ATT_ID = ATT.ATT_ID(+) ")
                .AppendLine("    AND SLS.SALES_ID = :SALES_ID ")
            End With

            Using query As New DBSelectQuery(Of ActivityInfoDataSet.SalesInfoForCheckDataTable)("ActivityInfo_611")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                ret = query.GetData()

            End Using
        Finally
            sql.Clear()
        End Try

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesHistInfoForCheck_End")
        'ログ出力 End *****************************************************************************

        Return ret

    End Function

    '2014/04/18 TCS 森 受注後フォロー機能開発 END

    '2014/08/20 TCS 森 受注後活動A⇒H移行対応 START
    ''' <summary>
    ''' 受注後ロック取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetLockAfterOdr(ByVal salesId As Decimal) As ActivityInfoDataSet.ActivityInfoGetAfterActDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()
        With sql
            .Append("SELECT ")
            .Append(" /* ActivityInfo_612 */ ")
            .Append("    T1.AFTER_ODR_ID ")
            .Append(" FROM ")
            .Append("    TB_T_AFTER_ODR T1 ")
            .Append(" WHERE ")
            .Append("    T1.SALES_ID = :SALES_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoGetAfterActDataTable)("ActivityInfo_612")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId) '商談ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdr_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 受注後活動ロック取得
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetLockAfterOdrAct(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        With sql
            .Append("SELECT ")
            .Append(" /* ActivityInfo_613 */ ")
            .Append(" 1 ")
            .Append("FROM ")
            .Append("    TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBUpdateQuery("ActivityInfo_613")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdrAct_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後History移行
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="actAccount">更新アカウント</param>
    ''' <param name="actFunction">更新機能ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOdr(ByVal salesId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT /* ActivityInfo_614 */")
            .Append("INTO ")
            .Append("    TB_H_AFTER_ODR T2 ")
            .Append(" ( ")
            .Append("    AFTER_ODR_ID, ")
            .Append("    SALES_ID, ")
            .Append("    DLR_CD, ")
            .Append("    SALESBKG_NUM, ")
            .Append("    DELI_SCHE_TERM_YEARMONTH, ")
            .Append("    DELI_SCHE_TERM_WEEKLY, ")
            .Append("    TENTATIVE_DELI_SCHE_DATE_FLG, ")
            .Append("    MODEL_CD, ")
            .Append("    REMAINDER_AMOUNT, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION, ")
            .Append("    AFTER_ODR_PIC_DLR_CD, ")
            .Append("    AFTER_ODR_PIC_BRN_CD, ")
            .Append("    AFTER_ODR_PIC_ORGNZ_ID, ")
            .Append("    AFTER_ODR_PIC_STF_CD ")
            .Append(" ) ")
            .Append("   SELECT ")
            .Append("     T1.AFTER_ODR_ID, ")
            .Append("     T1.SALES_ID, ")
            .Append("     T1.DLR_CD, ")
            .Append("     T1.SALESBKG_NUM, ")
            .Append("     T1.DELI_SCHE_TERM_YEARMONTH, ")
            .Append("     T1.DELI_SCHE_TERM_WEEKLY, ")
            .Append("     T1.TENTATIVE_DELI_SCHE_DATE_FLG, ")
            .Append("     T1.MODEL_CD, ")
            .Append("     T1.REMAINDER_AMOUNT, ")
            .Append("     SYSDATE, ")
            .Append("     :ACCOUNT, ")
            .Append("     :FUNCTION, ")
            .Append("     SYSDATE, ")
            .Append("     :ACCOUNT, ")
            .Append("     :FUNCTION, ")
            .Append("     T1.ROW_LOCK_VERSION, ")
            .Append("     T1.AFTER_ODR_PIC_DLR_CD, ")
            .Append("     T1.AFTER_ODR_PIC_BRN_CD, ")
            .Append("     T1.AFTER_ODR_PIC_ORGNZ_ID, ")
            .Append("     T1.AFTER_ODR_PIC_STF_CD ")
            .Append("    FROM ")
            .Append("       TB_T_AFTER_ODR T1 ")
            .Append("    WHERE ")
            .Append("       T1.SALES_ID = :SALES_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_614")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)       '商談ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)   '更新アカウント
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction) '更新機能ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdr_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後活動History移行
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <param name="actAccount">更新アカウント</param>
    ''' <param name="actFunction">更新機能ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOdrAct(ByVal afterOdrId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT /* ActivityInfo_615 */ ")
            .Append("INTO ")
            .Append("   TB_H_AFTER_ODR_ACT T2 ")
            .Append(" ( ")
            .Append("    AFTER_ODR_ACT_ID, ")
            .Append("    AFTER_ODR_ID, ")
            .Append("    AFTER_ODR_ACT_STATUS, ")
            .Append("    AFTER_ODR_PRCS_CD, ")
            .Append("    AFTER_ODR_ACT_CD, ")
            .Append("    ACT_ID, ")
            .Append("    AFTER_ODR_FLLW_SEQ, ")
            .Append("    STD_VOLUNTARYINS_ACT_TYPE, ")
            .Append("    VOLUNTARYINS_ACT_NAME, ")
            .Append("    CST_STF_DISP_TYPE, ")
            .Append("    STD_DATEORTIME_FLG, ")
            .Append("    STD_START_DATEORTIME, ")
            .Append("    STD_END_DATEORTIME, ")
            .Append("    SCHE_DATEORTIME_FLG, ")
            .Append("    SCHE_START_DATEORTIME, ")
            .Append("    SCHE_END_DATEORTIME, ")
            .Append("    SCHE_CONTACT_MTD, ")
            .Append("    SCHE_DLR_CD, ")
            .Append("    SCHE_BRN_CD, ")
            .Append("    SCHE_ORGNZ_ID, ")
            .Append("    SCHE_STF_CD, ")
            .Append("    RSLT_DATEORTIME_FLG, ")
            .Append("    RSLT_START_DATEORTIME, ")
            .Append("    RSLT_END_DATEORTIME, ")
            .Append("    RSLT_CONTACT_MTD, ")
            .Append("    RSLT_DLR_CD, ")
            .Append("    RSLT_BRN_CD, ")
            .Append("    RSLT_ORGNZ_ID, ")
            .Append("    RSLT_STF_CD, ")
            .Append("    FLLW_TGT_ID, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("SELECT ")
            .Append("    T1.AFTER_ODR_ACT_ID, ")
            .Append("    T1.AFTER_ODR_ID, ")
            .Append("    T1.AFTER_ODR_ACT_STATUS, ")
            .Append("    T1.AFTER_ODR_PRCS_CD, ")
            .Append("    T1.AFTER_ODR_ACT_CD, ")
            .Append("    T1.ACT_ID, ")
            .Append("    T1.AFTER_ODR_FLLW_SEQ, ")
            .Append("    T1.STD_VOLUNTARYINS_ACT_TYPE, ")
            .Append("    T1.VOLUNTARYINS_ACT_NAME, ")
            .Append("    T1.CST_STF_DISP_TYPE, ")
            .Append("    T1.STD_DATEORTIME_FLG, ")
            .Append("    T1.STD_START_DATEORTIME, ")
            .Append("    T1.STD_END_DATEORTIME, ")
            .Append("    T1.SCHE_DATEORTIME_FLG, ")
            .Append("    T1.SCHE_START_DATEORTIME, ")
            .Append("    T1.SCHE_END_DATEORTIME, ")
            .Append("    T1.SCHE_CONTACT_MTD, ")
            .Append("    T1.SCHE_DLR_CD, ")
            .Append("    T1.SCHE_BRN_CD, ")
            .Append("    T1.SCHE_ORGNZ_ID, ")
            .Append("    T1.SCHE_STF_CD, ")
            .Append("    T1.RSLT_DATEORTIME_FLG, ")
            .Append("    T1.RSLT_START_DATEORTIME, ")
            .Append("    T1.RSLT_END_DATEORTIME, ")
            .Append("    T1.RSLT_CONTACT_MTD, ")
            .Append("    T1.RSLT_DLR_CD, ")
            .Append("    T1.RSLT_BRN_CD, ")
            .Append("    T1.RSLT_ORGNZ_ID, ")
            .Append("    T1.RSLT_STF_CD, ")
            .Append("    T1.FLLW_TGT_ID, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    T1.ROW_LOCK_VERSION ")
            .Append("FROM ")
            .Append("   TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_615")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)    '更新アカウント
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)  '更新機能ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdrAct_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後削除
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOdr(ByVal salesId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("DELETE /* ActivityInfo_616 */ ")
            .Append("FROM ")
            .Append("   TB_T_AFTER_ODR T1 ")
            .Append("WHERE ")
            .Append("    T1.SALES_ID = :SALES_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_616")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId) '商談ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdr_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後活動削除
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOdrAct(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("DELETE /* ActivityInfo_617 */ ")
            .Append("FROM ")
            .Append("   TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_617")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdrAct_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 契約条件移行対象ロック取得
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetLockAfterOrdContract(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdContract_Start")
        'ログ出力 End *****************************************************************************

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        With sql
            .Append("SELECT ")
            .Append(" /* ActivityInfo_618 */ ")
            .Append("    T1.AFTER_ODR_ID ")
            .Append(" FROM ")
            .Append("    TB_T_NEWVCL_CONTRACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBUpdateQuery("ActivityInfo_618")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdContract_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 契約条件History移行
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <param name="actAccount">更新アカウント</param>
    ''' <param name="actFunction">更新機能ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOrdContract(ByVal afterOdrId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdContract_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT /* ActivityInfo_619 */ ")
            .Append(" INTO ")
            .Append("  TB_H_NEWVCL_CONTRACT ")
            .Append(" ( ")
            .Append("   CONTRACT_ID, ")
            .Append("   AFTER_ODR_ID, ")
            .Append("   SEL_VAL, ")
            .Append("   ROW_CREATE_DATETIME, ")
            .Append("   ROW_CREATE_ACCOUNT, ")
            .Append("   ROW_CREATE_FUNCTION, ")
            .Append("   ROW_UPDATE_DATETIME, ")
            .Append("   ROW_UPDATE_ACCOUNT, ")
            .Append("   ROW_UPDATE_FUNCTION, ")
            .Append("   ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("   SELECT ")
            .Append("    T1.CONTRACT_ID, ")
            .Append("    T1.AFTER_ODR_ID, ")
            .Append("    T1.SEL_VAL, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    T1.ROW_LOCK_VERSION ")
            .Append("  FROM ")
            .Append("    TB_T_NEWVCL_CONTRACT T1 ")
            .Append("  WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_619")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)    '更新アカウント
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)  '更新機能ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdContract_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 契約条件移行元削除
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOrdContract(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdContract_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("DELETE /* ActivityInfo_620 */ ")
            .Append("FROM ")
            .Append(" TB_T_NEWVCL_CONTRACT T1 ")
            .Append("WHERE  ")
            .Append(" T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_620")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdContract_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 予定変更履歴移行対象ロック取得
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetLockAfterOrdHis(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdHis_Start")
        'ログ出力 End *****************************************************************************

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpData As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        With sql
            .Append("SELECT ")
            .Append(" /* ActivityInfo_621 */ ")
            .Append("　　T1.AFTER_ODR_ID ")
            .Append("FROM ")
            .Append("    TB_T_AFTER_ODR_ACT_HIS T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
            .Append(sqlForUpData)
        End With

        Using query As New DBUpdateQuery("ActivityInfo_621")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdHis_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 予定変更履歴History移行
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <param name="actAccount">更新アカウント</param>
    ''' <param name="actFunction">更新機能ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOrdHis(ByVal afterOdrId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdHis_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT /* ActivityInfo_622 */ ")
            .Append(" INTO ")
            .Append(" TB_H_AFTER_ODR_ACT_HIS ")
            .Append(" ( ")
            .Append("   AFTER_ODR_ACT_CHG_HIS_ID, ")
            .Append("   AFTER_ODR_ID, ")
            .Append("   AFTER_ODR_ACT_CD, ")
            .Append("   CHG_DATETIME, ")
            .Append("   DATE_RANGE_TYPE, ")
            .Append("   OLD_SCHE_DATE, ")
            .Append("   OLD_SCHE_TERM_WEEKLY, ")
            .Append("   CHG_REASON, ")
            .Append("   ROW_CREATE_DATETIME, ")
            .Append("   ROW_CREATE_ACCOUNT, ")
            .Append("   ROW_CREATE_FUNCTION, ")
            .Append("   ROW_UPDATE_DATETIME, ")
            .Append("   ROW_UPDATE_ACCOUNT, ")
            .Append("   ROW_UPDATE_FUNCTION, ")
            .Append("   ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("   SELECT ")
            .Append("    T1.AFTER_ODR_ACT_CHG_HIS_ID, ")
            .Append("    T1.AFTER_ODR_ID, ")
            .Append("    T1.AFTER_ODR_ACT_CD, ")
            .Append("    T1.CHG_DATETIME, ")
            .Append("    T1.DATE_RANGE_TYPE, ")
            .Append("    T1.OLD_SCHE_DATE, ")
            .Append("    T1.OLD_SCHE_TERM_WEEKLY, ")
            .Append("    T1.CHG_REASON, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :FUNCTION, ")
            .Append("    T1.ROW_LOCK_VERSION ")
            .Append(" FROM ")
            .Append(" TB_T_AFTER_ODR_ACT_HIS T1 ")
            .Append(" WHERE ")
            .Append(" T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_622")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)    '更新アカウント
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)  '更新機能

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdHis_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 予定変更履歴移行元削除
    ''' </summary>
    ''' <param name="afterOdrId"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOrdHis(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdHis_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("DELETE /* ActivityInfo_623 */ ")
            .Append(" FROM ")
            .Append("  TB_T_AFTER_ODR_ACT_HIS T1 ")
            .Append(" WHERE ")
            .Append("  T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_623")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdHis_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後必要書類移行対象ロック取得
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetLockAfterOrdDoc(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdDoc_Start")
        'ログ出力 End *****************************************************************************

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        With sql
            .Append("SELECT ")
            .Append(" /* ActivityInfo_624 */ ")
            .Append("   T1.AFTER_ODR_ID ")
            .Append(" FROM ")
            .Append("    TB_T_AFTER_ODR_NEED_DOC T1 ")
            .Append(" WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBUpdateQuery("ActivityInfo_624")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOrdDoc_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後必要書類History移行
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <param name="actAccount">更新アカウント</param>
    ''' <param name="actFunction">更新機能ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOrdDoc(ByVal afterOdrId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdDoc_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("INSERT /* ActivityInfo_625 */ ")
            .Append(" INTO ")
            .Append(" TB_H_AFTER_ODR_NEED_DOC ")
            .Append(" ( ")
            .Append("   AFTER_ODR_NEED_DOC_ID, ")
            .Append("   AFTER_ODR_ID, ")
            .Append("   AFTER_ODR_PRCS_CD, DOC_ID, ")
            .Append("   VOLUNTARYINS_DOC_NAME, ")
            .Append("   NEED_AMOUNT, ")
            .Append("   ARRIVAL_AMOUNT, ")
            .Append("   SCHE_ARRIVAL_DATE, ")
            .Append("   RSLT_ARRIVAL_DATE, ")
            .Append("   ROW_CREATE_DATETIME, ")
            .Append("   ROW_CREATE_ACCOUNT, ")
            .Append("   ROW_CREATE_FUNCTION, ")
            .Append("   ROW_UPDATE_DATETIME, ")
            .Append("   ROW_UPDATE_ACCOUNT, ")
            .Append("   ROW_UPDATE_FUNCTION, ")
            .Append("   ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("SELECT ")
            .Append("  T1.AFTER_ODR_NEED_DOC_ID, ")
            .Append("  T1.AFTER_ODR_ID, ")
            .Append("  T1.AFTER_ODR_PRCS_CD, ")
            .Append("  T1.DOC_ID, ")
            .Append("  T1.VOLUNTARYINS_DOC_NAME, ")
            .Append("  T1.NEED_AMOUNT, ")
            .Append("  T1.ARRIVAL_AMOUNT, ")
            .Append("  T1.SCHE_ARRIVAL_DATE, ")
            .Append("  T1.RSLT_ARRIVAL_DATE, ")
            .Append("  SYSDATE, ")
            .Append("  :ACCOUNT, ")
            .Append("  :FUNCTION, ")
            .Append("  SYSDATE, ")
            .Append("  :ACCOUNT, ")
            .Append("  :FUNCTION, ")
            .Append("  T1.ROW_LOCK_VERSION ")
            .Append(" FROM ")
            .Append("  TB_T_AFTER_ODR_NEED_DOC T1 ")
            .Append(" WHERE ")
            .Append("  T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_625")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)    '更新アカウント
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)  '更新機能ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOrdDoc_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    ''' <summary>
    ''' 受注後必要書類移行元削除
    ''' </summary>
    ''' <param name="afterOdrId">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOrdDoc(ByVal afterOdrId As Decimal)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdDoc_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        With sql
            .Append("DELETE /* ActivityInfo_626 */ ")
            .Append(" FROM ")
            .Append("   TB_T_AFTER_ODR_NEED_DOC T1 ")
            .Append(" WHERE ")
            .Append("   T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
        End With

        Using query As New DBUpdateQuery("ActivityInfo_626")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrId) '受注後ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOrdDoc_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub

    '2014/08/20 TCS 森 受注後活動A⇒H移行対応 END
#End Region


    '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 START
    ''' <summary>
    ''' 初期活動店舗コード取得
    ''' </summary>
    ''' <returns>商談ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPreBrnCd(ByVal followseq As Decimal) As String

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPreBrnCd_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT")
            .Append("  /* ActivityInfo_508 */")
            .Append("  BRNCD ")
            .Append("FROM( ")
            .Append("SELECT T2.REC_BRN_CD AS BRNCD ")
            .Append(" FROM TB_T_SALES T1, ")
            .Append("      TB_T_REQUEST T2 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T1.REQ_ID <> 0 ")
            .Append("   AND T1.SALES_ID = :SALESID ")
            .Append("UNION ")
            .Append("SELECT T2.REC_BRN_CD AS BRNCD ")
            .Append(" FROM TB_H_SALES T1, ")
            .Append("      TB_H_REQUEST T2 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T1.REQ_ID <> 0 ")
            .Append("   AND T1.SALES_ID = :SALESID ")
            .Append("UNION ")
            .Append("SELECT T2.BRN_CD AS BRNCD ")
            .Append(" FROM TB_T_SALES T1, ")
            .Append("      TB_T_ATTRACT T2 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T1.ATT_ID <> 0 ")
            .Append("   AND T1.SALES_ID = :SALESID ")
            .Append("UNION ")
            .Append("SELECT T2.BRN_CD AS BRNCD ")
            .Append(" FROM TB_H_SALES T1, ")
            .Append("      TB_H_ATTRACT T2 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T1.ATT_ID <> 0 ")
            .Append("   AND T1.SALES_ID = :SALESID) ")
        End With
        Using query As New DBSelectQuery(Of DataTable)("ActivityInfo_508")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, followseq)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPreBrnCd_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()(0)(0).ToString
        End Using
    End Function
    '2014/09/01 TCS 松月 問連TR-V4-GTMC140807001対応 END

#End Region

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）START

    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' コンタクト履歴取得
    ''' </summary>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="tabIndex">検索対象のタブ</param>
    ''' <returns>ActivityInfoContactHistoryDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactHistory(ByVal customerClass As String, _
                                             ByVal crcustId As String, _
                                             ByVal dlrCD As String, _
                                             ByVal cstKind As String, _
                                             ByVal newCustId As String, _
                                             ByVal tabIndex As String, _
                                             ByVal vin As String) As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable

        Dim sql As New StringBuilder
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactHistory_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append(" SELECT /* SC3080201_132 */ ")
            .Append("        ACTUALKIND ") '活動種類
            .Append("      , ACTUALDATE ") '活動日
            .Append("      , CONTACTNO ") '接触方法No
            .Append("      , COUNTVIEW ") 'カウント表示
            .Append("      , CONTACT ") '接触方法
            .Append("      , CRACTSTATUS ") 'ステータス
            .Append("      , USERNAME ") '実施者名
            .Append("      , ICON_IMGFILE ") '権限アイコンパス
            .Append("      , ROW_NUMBER() OVER(PARTITION BY CONTACTNO, FLLWUPBOX_SEQNO ORDER BY ACTUALDATE,UPDATEDATE) AS CONTACTCOUNT ") 'カウント
            .Append("      , COMPLAINT_OVERVIEW ") '苦情概要
            .Append("      , ACTUAL_DETAIL ") '苦情対応内容
            .Append("      , MEMO ") '苦情メモ
            .Append("      , MILEAGE ") '走行距離
            .Append("      , DLRNICNM_LOCAL ") '販売店名
            .Append("      , MAINTEAMOUNT ") '整備費用
            .Append("      , JOBNO ") '整備番号
            .Append("      , MILEAGESEQ ") '入庫番号
            .Append("      , DLRCD ") '販売店コード
            .Append("      , ORIGINALID ") '自社客連番
            .Append("      , VIN ") 'VIN
            .Append("      , VCLREGNO ") '車両登録番号
            .Append("      , OPERATIONCODE ") '実施者権限
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("      , ACT_ID ") '活動ID
            .Append("      , AFTER_ODR_FLLW_SEQ ") '受注後工程フォロー結果連番
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("   FROM ( ")
            'tabIndexで分岐
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Or _
               String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '全てタブ、セールスタブSQL
                'FOLLOW-UP BOX
                .Append(ContactHistoryFollowSqlCreate(False))
                .Append("     UNION ALL ")
                'FOLLOW-UP BOX PAST
                .Append(ContactHistoryFollowSqlCreate(True))
                'Follow-upBoxベース
                .Append("     UNION ALL ")
                .Append(ContactHistoryFollowupBoxSqlCreate())
                '受注後(計画)
                .Append("     UNION ALL ")
                .Append(ContactHistoryPlanSqlCreate(cstKind))
                '受注後(キャンセル)
                .Append("     UNION ALL ")
                .Append(ContactHistoryCancelSqlCreate(False))
                '受注後(キャンセル) PAST
                .Append("     UNION ALL ")
                .Append(ContactHistoryCancelSqlCreate(True))
                If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                    '全てタブの場合、サービス追加
                    .Append("     UNION ALL ")
                    .Append(ContactHistoryServiceSqlCreate(tabIndex))
                End If

                If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                    '全てタブの場合、CR追加
                    .Append("     UNION ALL ")
                    .Append(ContactHistoryCRSqlCreate())
                End If
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SERVICE) Then
                'サービスタブSQL
                .Append(ContactHistoryServiceSqlCreate(tabIndex))
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                'CRタブSQL
                .Append(ContactHistoryCRSqlCreate())
            End If
            .Append(" ) ")
        End With
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContactHistoryDataTable)("SC3080201_032")
            query.CommandText = sql.ToString()

            '共通パラメータ
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD) '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, STRCD000) '店舗コード
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                '全てタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If Not String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類

                '2016/10/19 TCS 河原 TR-SVT-TMT-20160727-002 UPDATE
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                'セールスタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If Not String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SERVICE) Then
                'サービスタブパラメータ
                '2016/10/19 TCS 河原 TR-SVT-TMT-20160727-002 DEL
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            End If
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactHistory_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 FOLLOW-UP BOX
    ''' </summary>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryFollowSqlCreate(ByVal pastFlg As Boolean) As String
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryFollowSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT DISTINCT /* SC3080201_170 */ /* FOLLOW-UP BOX */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T1.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T1.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T1.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , T3.SALES_ID AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , CASE ")
            .Append("            WHEN T1.ACT_STATUS = '31' THEN ")
            .Append("                 '4' ")
            .Append("            WHEN T1.ACT_STATUS = '32' THEN ")
            .Append("                 '5' ")
            .Append("       ELSE ")
            .Append("            CASE ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '30' THEN ")
            .Append("                      '3' ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '20' THEN ")
            .Append("                      '2' ")
            .Append("                 ELSE '1' ")
            .Append("            END ")
            .Append("       END AS CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T1.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , ' ' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , TO_CHAR(T5.OPERATIONCODE) AS OPERATIONCODE ")
            If pastFlg = True Then
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                .Append("     , CASE WHEN T1.ACT_STATUS = '31' THEN T1.ACT_ID ")
                .Append("            ELSE 0 ")
                .Append("       END AS ACT_ID ")
                .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                .Append("  FROM TB_H_ACTIVITY T1 ")
                .Append("     , TB_H_REQUEST T2 ")
                .Append("     , TB_H_SALES T3 ")
            Else
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                .Append("     , 0 AS ACT_ID ")
                .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                .Append("  FROM TB_T_ACTIVITY T1 ")
                .Append("     , TB_T_REQUEST T2 ")
                .Append("     , TB_T_SALES T3 ")
            End If
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T7 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T1.REQ_ID = T3.REQ_ID ")
            .Append("   AND T1.RSLT_CONTACT_MTD = T4.CONTACT_MTD(+) ")
            .Append("   AND T1.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T1.RSLT_FLG = '1' ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T2.CST_ID = :CRCUSTID ")
            .Append("   AND T2.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T2.BIZ_CAT_ID = T7.BIZ_CAT_ID ")
            .Append("   AND T7.BIZ_TYPE = '2' ")
            .Append("   AND T3.SALES_PROSPECT_CD <> ' ' ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) START
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) END
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT DISTINCT ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T1.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T1.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T1.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , T3.SALES_ID AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , CASE ")
            .Append("            WHEN T1.ACT_STATUS = '31' THEN ")
            .Append("                 '4' ")
            .Append("            WHEN T1.ACT_STATUS = '32' THEN ")
            .Append("                 '5' ")
            .Append("       ELSE ")
            .Append("            CASE ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '30' THEN ")
            .Append("                      '3' ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '20' THEN ")
            .Append("                      '2' ")
            .Append("                 ELSE '1' ")
            .Append("            END ")
            .Append("       END AS CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T1.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , ' ' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , TO_CHAR(T5.OPERATIONCODE) AS OPERATIONCODE ")
            If pastFlg = True Then
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                .Append("     , CASE WHEN T1.ACT_STATUS = '31' THEN T1.ACT_ID ")
                .Append("            ELSE 0 ")
                .Append("       END AS ACT_ID ")
                .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                .Append("  FROM TB_H_ACTIVITY T1 ")
                .Append("     , TB_H_ATTRACT T2 ")
                .Append("     , TB_H_SALES T3 ")
            Else
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                .Append("     , 0 AS ACT_ID ")
                .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                .Append("  FROM TB_T_ACTIVITY T1 ")
                .Append("     , TB_T_ATTRACT T2 ")
                .Append("     , TB_T_SALES T3 ")
            End If
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T7 ")
            .Append("     , TB_M_ATTPLAN T8 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T1.ATT_ID = T3.ATT_ID ")
            .Append("   AND T1.RSLT_CONTACT_MTD = T4.CONTACT_MTD(+) ")
            .Append("   AND T1.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T1.RSLT_FLG = '1' ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T2.CST_ID = :CRCUSTID ")
            .Append("   AND T2.ATTPLAN_CREATE_DLR_CD = T8.DLR_CD ")
            .Append("   AND T2.ATTPLAN_CREATE_BRN_CD = T8.BRN_CD ")
            .Append("   AND T2.ATTPLAN_ID = T8.ATTPLAN_ID ")
            .Append("   AND T2.ATTPLAN_VERSION = T8.ATTPLAN_VERSION ")
            .Append("   AND T8.BIZ_CAT_ID = T7.BIZ_CAT_ID ")
            .Append("   AND T7.BIZ_TYPE = '2' ")
            .Append("   AND T2.ATT_STATUS = '31' ")
            .Append("   AND T3.SALES_PROSPECT_CD <> ' ' ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) START
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) END
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryFollowSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()
    End Function


    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 受注後(計画)
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryPlanSqlCreate(ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 受注後(計画) */ DISTINCT")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("            '4' AS ACTUALKIND ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("          , A.ACTUALTIME_END AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , TO_CHAR(B.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("          , TO_CHAR(B.CONTACT_NAME) AS CONTACT ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("          , '' AS CRACTSTATUS ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            .Append("          , TO_CHAR(D.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            .Append("          ,'' AS MILEAGE, ")
            .Append("           '' AS DLRNICNM_LOCAL, ")
            .Append("           '' AS MAINTEAMOUNT, ")
            .Append("           '' AS JOBNO, ")
            .Append("           '' AS MILEAGESEQ, ")
            .Append("           '' AS DLRCD, ")
            .Append("           '' AS ORIGINALID, ")
            .Append("           '' AS VIN, ")
            .Append("           '' AS VCLREGNO, ")
            .Append("           TO_CHAR(C.OPERATIONCODE) AS OPERATIONCODE ")
            '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("          , 0 AS ACT_ID ")
            .Append("          , A.SEQNO AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("       FROM TBL_BOOKEDAFTERFOLLOWRSLT A ")
            .Append("          , TB_M_CONTACT_MTD B ")
            .Append("          , tbl_USERS C ")
            .Append("          , TBL_OPERATIONTYPE D ")
            .Append("      WHERE A.DLRCD = :DLRCD ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND A.CRCUSTID = :CRCUSTID ")
            .Append("        AND B.CONTACT_MTD(+) = TO_CHAR(A.CONTACTNO) ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) START
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) END
            .Append("        AND C.ACCOUNT(+) = A.ACTUALACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND D.DLRCD(+) = :DLRCD ")
            .Append("        AND D.STRCD(+) = :STRCD ")
            .Append("        AND D.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 受注後(キャンセル)
    ''' </summary>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryCancelSqlCreate(ByVal pastFlg As Boolean) As String
        Dim sql As New StringBuilder

        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryCancelSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
            If pastFlg = True Then
                'ヒストリーに対してヒント句を使用
                .Append("SELECT /* SC3080201_182 */ /* 受注後(キャンセル) */ /*+ LEADING(T1 T2 T3) USE_NL(T1 T2 T3) INDEX(T1 TB_H_REQUEST_IX2) INDEX(T2 TB_H_SALES_IX2) INDEX(T3 IDX_ESTIMATEINFO_05)*/ ")
            Else
                'トランに対してヒント句を使用
                .Append("SELECT /* SC3080201_182 */ /* 受注後(キャンセル) */ /*+ LEADING(T1 T2 T3) USE_NL(T1 T2 T3) INDEX(T1 TB_T_REQUEST_IX2) INDEX(T2 TB_T_SALES_IX2) INDEX(T3 IDX_ESTIMATEINFO_05)*/ ")
            End If
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T5.CANCEL_DATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '12' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , T5.CANCEL_DATE AS UPDATEDATE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , '' AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_REQUEST T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_REQUEST T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , TB_T_SALESBOOKING T5 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T6 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND T3.DLRCD = T5.DLR_CD")
            .Append("   AND RTRIM(T3.CONTRACTNO) = T5.SALESBKG_NUM")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_CAT_ID = T6.BIZ_CAT_ID ")
            .Append("   AND T6.BIZ_TYPE = '2' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T5.CANCEL_FLG = '1' ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("UNION ALL ")
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
            If pastFlg = True Then
                'ヒストリーに対してヒント句を使用
                .Append("SELECT /*+ LEADING(T1 T2 T3) USE_NL(T1 T2 T3) INDEX(T1 TB_H_ATTRACT_IX4) INDEX(T2 TB_H_SALES_IX3) INDEX(T3 IDX_ESTIMATEINFO_05)*/ ")
            Else
                'トランに対してヒント句を使用
                .Append("SELECT /*+ LEADING(T1 T2 T3) USE_NL(T1 T2 T3) INDEX(T1 TB_T_ATTRACT_IX4) INDEX(T2 TB_T_SALES_IX3) INDEX(T3 IDX_ESTIMATEINFO_05)*/ ")
            End If
            '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T5.CANCEL_DATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '12' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , T5.CANCEL_DATE AS UPDATEDATE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , '' AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_ATTRACT T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_ATTRACT T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , TB_T_SALESBOOKING T5 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T6 ")
            .Append("     , TB_M_ATTPLAN T7 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND T3.DLRCD = T5.DLR_CD")
            .Append("   AND RTRIM(T3.CONTRACTNO) = T5.SALESBKG_NUM")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.ATTPLAN_CREATE_DLR_CD = T7.DLR_CD ")
            .Append("   AND T1.ATTPLAN_CREATE_BRN_CD = T7.BRN_CD ")
            .Append("   AND T1.ATTPLAN_ID = T7.ATTPLAN_ID ")
            .Append("   AND T1.ATTPLAN_VERSION = T7.ATTPLAN_VERSION ")
            .Append("   AND T7.BIZ_CAT_ID = T6.BIZ_CAT_ID ")
            .Append("   AND T6.BIZ_TYPE = '2' ")
            .Append("   AND T1.ATT_STATUS = '31' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T5.CANCEL_FLG = '1' ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryCancelSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴　CR用SQL作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ContactHistoryCRSqlCreate() As String
        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryCRSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("    /* SC3080201_183 */ /* 苦情 */ ")
            .Append("    '3' AS ACTUALKIND      , ")
            .Append("    T4.ACT_DATETIME AS ACTUALDATE      , ")
            .Append("    0 AS CONTACTNO      , ")
            .Append("    0 AS FLLWUPBOX_SEQNO      , ")
            .Append("    '0' AS COUNTVIEW      , ")
            .Append("    TO_CHAR('%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-')) AS CONTACT      , ")
            .Append("    TO_CHAR(T3.CMPL_STATUS) AS CRACTSTATUS      , ")
            .Append("    TO_CHAR(T7.USERNAME) AS USERNAME      , ")
            .Append("    TO_CHAR(T8.ICON_IMGFILE) AS ICON_IMGFILE      , ")
            .Append("    T3.UPDATE_DATETIME AS UPDATEDATE     , ")
            .Append("    TO_CHAR(T3.CMPL_OVERVIEW) AS COMPLAINT_OVERVIEW      , ")
            .Append("    TO_CHAR(T4.ACT_CONTENT) AS ACTUAL_DETAIL      , ")
            .Append("    TO_CHAR(T9.CST_MEMO) AS MEMO      , ")
            .Append("    0 AS ORDER_NO      , ")
            .Append("    '' AS MILEAGE      , ")
            .Append("    '' AS DLRNICNM_LOCAL      , ")
            .Append("    '' AS MAINTEAMOUNT      , ")
            .Append("    '' AS JOBNO      , ")
            .Append("    '' AS MILEAGESEQ      , ")
            .Append("    '' AS DLRCD      , ")
            .Append("    '' AS ORIGINALID      , ")
            .Append("    '' AS VIN      , ")
            .Append("    '' AS VCLREGNO , ")
            .Append("    TO_CHAR(T7.OPERATIONCODE) AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("FROM ")
            .Append("    TB_T_ACTIVITY T1      , ")
            .Append("    TB_T_REQUEST T2      , ")
            .Append("    TB_T_COMPLAINT T3      , ")
            .Append("    TB_T_COMPLAINT_DETAIL T4      , ")
            .Append("    TB_M_COMPLAINT_IMPORTANCE T5      , ")
            .Append("    TB_M_COMPLAINT_CAT T6      , ")
            .Append("    TBL_USERS T7      , ")
            .Append("    TBL_OPERATIONTYPE T8      , ")
            .Append("    TB_T_ACTIVITY_MEMO T9 , ")
            .Append("    TB_M_BUSSINES_CATEGORY T10 ")
            .Append("WHERE ")
            .Append("    T1.REQ_ID = T2.REQ_ID AND ")
            .Append("    T1.REQ_ID = T3.REQ_ID AND ")
            .Append("    T1.ACT_ID = T4.ACT_ID    AND ")
            .Append("    T3.CMPL_IMPORTANCE_ID = T5.CMPL_IMPORTANCE_ID(+)    AND ")
            .Append("    T3.CMPL_CAT_ID = T6.CMPL_CAT_ID(+)    AND ")
            .Append("    T4.ACT_STF_CD = T7.ACCOUNT(+)     AND ")
            .Append("    T7.OPERATIONCODE = T8.OPERATIONCODE(+) AND ")
            .Append("    T1.ACT_ID = T9.RELATION_ACT_ID(+) AND ")
            .Append("    T2.CST_ID = :CRCUSTID    AND ")
            .Append("    T2.REC_CST_VCL_TYPE = '1'    AND ")
            .Append("    T2.BIZ_CAT_ID = T10.BIZ_CAT_ID    AND ")
            .Append("    T10.BIZ_TYPE = '3'    AND ")
            .Append("    T3.RELATION_TYPE <> 2    AND ")
            .Append("    T4.DIST_FLG(+) = '0'    AND ")
            .Append("    T5.INUSE_FLG(+) = '1'    AND ")
            .Append("    T6.INUSE_FLG(+) = '1'    AND ")
            .Append("    T7.DELFLG(+) = '0'    AND ")
            .Append("    T8.DLRCD(+) = :DLRCD    AND ")
            .Append("    T8.STRCD(+) = :STRCD ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("    /* SC3080201_183 */ /* 苦情 */ ")
            .Append("    '3' AS ACTUALKIND      , ")
            .Append("    T4.ACT_DATETIME AS ACTUALDATE      , ")
            .Append("    0 AS CONTACTNO      , ")
            .Append("    0 AS FLLWUPBOX_SEQNO      , ")
            .Append("    '0' AS COUNTVIEW      , ")
            .Append("    TO_CHAR('%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-')) AS CONTACT      , ")
            .Append("    TO_CHAR(T3.CMPL_STATUS) AS CRACTSTATUS      , ")
            .Append("    TO_CHAR(T7.USERNAME) AS USERNAME      , ")
            .Append("    TO_CHAR(T8.ICON_IMGFILE) AS ICON_IMGFILE      , ")
            .Append("    T3.UPDATE_DATETIME AS UPDATEDATE     , ")
            .Append("    TO_CHAR(T3.CMPL_OVERVIEW) AS COMPLAINT_OVERVIEW      , ")
            .Append("    TO_CHAR(T4.ACT_CONTENT) AS ACTUAL_DETAIL      , ")
            .Append("    TO_CHAR(T9.CST_MEMO) AS MEMO      , ")
            .Append("    0 AS ORDER_NO      , ")
            .Append("    '' AS MILEAGE      , ")
            .Append("    '' AS DLRNICNM_LOCAL      , ")
            .Append("    '' AS MAINTEAMOUNT      , ")
            .Append("    '' AS JOBNO      , ")
            .Append("    '' AS MILEAGESEQ      , ")
            .Append("    '' AS DLRCD      , ")
            .Append("    '' AS ORIGINALID      , ")
            .Append("    '' AS VIN      , ")
            .Append("    '' AS VCLREGNO , ")
            .Append("    TO_CHAR(T7.OPERATIONCODE) AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("FROM ")
            .Append("    TB_H_ACTIVITY T1      , ")
            .Append("    TB_H_REQUEST T2      , ")
            .Append("    TB_H_COMPLAINT T3      , ")
            .Append("    TB_H_COMPLAINT_DETAIL T4      , ")
            .Append("    TB_M_COMPLAINT_IMPORTANCE T5      , ")
            .Append("    TB_M_COMPLAINT_CAT T6      , ")
            .Append("    TBL_USERS T7      , ")
            .Append("    TBL_OPERATIONTYPE T8      , ")
            .Append("    TB_H_ACTIVITY_MEMO T9 , ")
            .Append("    TB_M_BUSSINES_CATEGORY T10 ")
            .Append("WHERE ")
            .Append("    T1.REQ_ID = T2.REQ_ID AND ")
            .Append("    T1.REQ_ID = T3.REQ_ID AND ")
            .Append("    T1.ACT_ID = T4.ACT_ID    AND ")
            .Append("    T3.CMPL_IMPORTANCE_ID = T5.CMPL_IMPORTANCE_ID(+)    AND ")
            .Append("    T3.CMPL_CAT_ID = T6.CMPL_CAT_ID(+)    AND ")
            .Append("    T4.ACT_STF_CD = T7.ACCOUNT(+)     AND ")
            .Append("    T7.OPERATIONCODE = T8.OPERATIONCODE(+) AND ")
            .Append("    T1.ACT_ID = T9.RELATION_ACT_ID(+) AND ")
            .Append("    T2.CST_ID = :CRCUSTID    AND ")
            .Append("    T2.REC_CST_VCL_TYPE = '1'    AND ")
            .Append("    T2.BIZ_CAT_ID = T10.BIZ_CAT_ID    AND ")
            .Append("    T10.BIZ_TYPE = '3'    AND ")
            .Append("    T3.RELATION_TYPE <> 2    AND ")
            .Append("    T4.DIST_FLG(+) = '0'    AND ")
            .Append("    T5.INUSE_FLG(+) = '1'    AND ")
            .Append("    T6.INUSE_FLG(+) = '1'    AND ")
            .Append("    T7.DELFLG(+) = '0'    AND ")
            .Append("    T8.DLRCD(+) = :DLRCD    AND ")
            .Append("    T8.STRCD(+) = :STRCD ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryCRSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 Follow-upBox
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Follow-upBoxベースで履歴取得</remarks>
    Public Shared Function ContactHistoryFollowupBoxSqlCreate() As String

        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryFollowupBoxSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_184 */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T2.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T2.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T2.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , '1' CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T2.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , TO_CHAR(T5.OPERATIONCODE) AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append(" FROM TB_T_REQUEST T1 ")
            .Append("     , TB_T_ACTIVITY T2 ")
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T7 ")
            .Append(" WHERE T1.LAST_ACT_ID = T2.ACT_ID ")
            .Append("   AND T2.RSLT_CONTACT_MTD = T4.CONTACT_MTD(+) ")
            .Append("   AND T2.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_CAT_ID = T7.BIZ_CAT_ID ")
            .Append("   AND T7.BIZ_TYPE = '4' ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) START
            '.Append("   AND T4.INUSE_FLG(+) = '1' ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) END
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T2.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T2.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T2.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , '7' CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T2.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("     , TO_CHAR(T5.OPERATIONCODE) AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("  FROM TB_H_REQUEST T1 ")
            .Append("     , TB_H_ACTIVITY T2 ")
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_BUSSINES_CATEGORY T7 ")
            .Append(" WHERE T1.LAST_ACT_ID = T2.ACT_ID ")
            .Append("   AND T2.RSLT_CONTACT_MTD = T4.CONTACT_MTD(+) ")
            .Append("   AND T2.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_CAT_ID = T7.BIZ_CAT_ID ")
            .Append("   AND T7.BIZ_TYPE = '4' ")
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) START
            '2015/07/13 TCS 中村 問連対応(TR-V4-TMT-20150511-001) END
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryFollowupBoxSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Return sql.ToString()
    End Function

    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' コンタクト履歴 サービス用SQL作成
    ''' </summary>
    ''' <param name="tabIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryServiceSqlCreate(ByVal tabIndex As String) As String
        Dim sql As New StringBuilder

        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryServiceSqlCreate_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_185 */  ")
            .Append("    '2' AS ACTUALKIND,  ")
            .Append("    T4.REG_DATE AS ACTUALDATE,  ")
            .Append("    0 AS CONTACTNO,  ")
            .Append("    0 AS FLLWUPBOX_SEQNO,  ")
            .Append("    '0' AS COUNTVIEW,  ")
            .Append("    '' AS CONTACT,  ")
            .Append("    '' AS CRACTSTATUS,  ")
            .Append("    TO_CHAR(T5.USERNAME) AS USERNAME,  ")
            .Append("    TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE,  ")
            .Append("    T4.REG_DATE AS UPDATEDATE,  ")
            .Append("    '' AS COMPLAINT_OVERVIEW,  ")
            .Append("    '' AS ACTUAL_DETAIL,  ")
            .Append("    '' AS MEMO,  ")
            .Append("    0 AS ORDER_NO,  ")
            .Append("    TO_CHAR(T4.REG_MILE,'9G999G999G999G999G999') AS MILEAGE,  ")
            .Append("    TO_CHAR(T7.DLR_NAME) AS DLRNICNM_LOCAL,  ")
            .Append("    TO_CHAR(T3.MAINTE_AMOUNT) AS MAINTEAMOUNT,  ")
            .Append("    TO_CHAR(T3.SVCIN_NUM) AS JOBNO,  ")
            .Append("    TO_CHAR(T3.VCL_MILE_ID) AS MILEAGESEQ,  ")
            .Append("    TO_CHAR(T3.DLR_CD) AS DLRCD,  ")
            .Append("    TO_CHAR(T1.CST_ID) AS ORIGINALID,  ")
            .Append("    TO_CHAR(T2.VCL_VIN) AS VIN,  ")
            .Append("    TO_CHAR(T8.REG_NUM) AS VCLREGNO,  ")
            .Append("    TO_CHAR(T5.OPERATIONCODE) AS OPERATIONCODE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("     , 0 AS ACT_ID ")
            .Append("     , 0 AS AFTER_ODR_FLLW_SEQ ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append("  FROM TB_M_CUSTOMER_VCL T1 ")
            .Append("     , TB_M_VEHICLE T2 ")
            .Append("     , TB_T_VEHICLE_SVCIN_HIS T3 ")
            .Append("     , TB_T_VEHICLE_MILEAGE T4 ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_DEALER T7 ")
            .Append("     , TB_M_VEHICLE_DLR T8 ")
            .Append(" WHERE T1.VCL_ID = T2.VCL_ID ")
            .Append("   AND T1.DLR_CD = T3.DLR_CD ")
            .Append("   AND T1.VCL_ID = T3.VCL_ID ")
            .Append("   AND T1.CST_ID = T3.CST_ID ")
            .Append("   AND T3.VCL_MILE_ID = T4.VCL_MILE_ID ")
            .Append("   AND T3.PIC_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T3.DLR_CD = T7.DLR_CD ")
            .Append("   AND T1.DLR_CD = T8.DLR_CD ")
            .Append("   AND T1.VCL_ID = T8.VCL_ID ")
            '2016/10/19 TCS 河原 TR-SVT-TMT-20160727-002 DEL
            .Append("   AND T1.CST_VCL_TYPE = '1' ")
            .Append("   AND T2.VCL_VIN = :VIN ")
            .Append("   AND T4.REG_MTD = '1'  ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("ContactHistoryServiceSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()

    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END

#Region "V3コンタクト履歴"

    ''' <summary>
    ''' コンタクト履歴取得
    ''' </summary>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="tabIndex">検索対象のタブ</param>
    ''' <returns>SC3080201ContactHistoryDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetV3ContactHistory(ByVal customerClass As String, _
                                             ByVal crcustId As String, _
                                             ByVal dlrCD As String, _
                                             ByVal cstKind As String, _
                                             ByVal newCustId As String, _
                                             ByVal tabIndex As String, _
                                             ByVal vin As String) As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3080201_301 */ ")
            .Append("        ACTUALKIND ") '活動種類
            .Append("      , ACTUALDATE ") '活動日
            .Append("      , CONTACTNO ") '接触方法No
            .Append("      , COUNTVIEW ") 'カウント表示
            .Append("      , CONTACT ") '接触方法
            .Append("      , CRACTSTATUS ") 'ステータス
            .Append("      , USERNAME ") '実施者名
            .Append("      , ICON_IMGFILE ") '権限アイコンパス
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 START
            .Append("      , ROW_NUMBER() OVER(PARTITION BY FLLWUPBOX_SEQNO ORDER BY ACTUALDATE,UPDATEDATE) AS CONTACTCOUNT ") 'カウント
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 END
            .Append("      , COMPLAINT_OVERVIEW ") '苦情概要
            .Append("      , ACTUAL_DETAIL ") '苦情対応内容
            .Append("      , MEMO ") '苦情メモ
            .Append("      , MILEAGE ") '走行距離
            .Append("      , DLRNICNM_LOCAL ") '販売店名
            .Append("      , MAINTEAMOUNT ") '整備費用
            .Append("      , JOBNO ") '整備番号
            .Append("      , MILEAGESEQ ") '入庫番号
            .Append("      , DLRCD ") '販売店コード
            .Append("      , ORIGINALID ") '自社客連番
            .Append("      , VIN ") 'VIN
            .Append("      , VCLREGNO ") '車両登録番号
            .Append("      , OPERATIONCODE ") '実施者権限
            .Append("   FROM ( ")
            'tabIndexで分岐
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Or _
               String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '全てタブ、セールスタブSQL
                'FOLLOW-UP BOX
                .Append(V3ContactHistoryFollowSqlCreate(newCustId, False, cstKind))
                'FOLLOW-UP BOX PAST
                .Append("     UNION ALL ")
                .Append(V3ContactHistoryFollowSqlCreate(newCustId, True, cstKind))
                'Follow-upBoxベース
                .Append("     UNION ALL ")
                .Append(V3ContactHistoryFollowupBoxSqlCreate(newCustId, False, cstKind))
                .Append("     UNION ALL ")
                .Append(V3ContactHistoryFollowupBoxSqlCreate(newCustId, True, cstKind))
                '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 :サービスのV3履歴は参照しない DELETE
                If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                    '全てタブの場合、CR追加
                    .Append("     UNION ALL ")
                    .Append(V3ContactHistoryCRSqlCreate(newCustId, cstKind))
                End If
                '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 :サービスのV3履歴は参照しない DELETE
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                'CRタブSQL
                .Append(V3ContactHistoryCRSqlCreate(newCustId, cstKind))
            End If
            .Append(" ) ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContactHistoryDataTable)("SC3080201_301", DBQueryTarget.DMS)
            query.CommandText = sql.ToString()

            '共通パラメータ
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD) '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, STRCD000) '店舗コード
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                '全てタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在しない
                        query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                    Else
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                Else
                    '未取引客
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類

                '2016/10/19 TCS 河原 TR-SVT-TMT-20160727-002 UPDATE
                '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 :サービスのV3履歴は参照しない DELETE
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                'セールスタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在しない
                        'query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                    Else
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                Else
                    '未取引客
                    'query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
                '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 :サービスのV3履歴は参照しない DELETE
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                'CRタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在しない
                        query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                    Else
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                Else
                    '未取引客
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                End If
            End If

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 FOLLOW-UP BOX
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function V3ContactHistoryFollowSqlCreate(ByVal newCustId As String, _
                                                         ByVal pastFlg As Boolean, _
                                                         ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* FOLLOW-UP BOX */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.ACTUALTIME_END AS ACTUALDATE ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 START
            .Append("          , 0 AS CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , '0' AS COUNTVIEW ")
            .Append("          , '' AS CONTACT ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 END
            .Append("          , CASE WHEN A.CRACTRESULT = '3' THEN '4' ")
            .Append("                 WHEN A.CRACTRESULT = '5' THEN '5' ")
            .Append("            ELSE ")
            .Append("                 CASE WHEN A.CRACTSTATUS = '1' THEN '3' ")
            .Append("                      WHEN A.CRACTSTATUS = '2' THEN '2' ")
            .Append("                      WHEN A.CRACTSTATUS = '7' THEN '1' ")
            .Append("                 END ")
            .Append("            END AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 START
            .Append("          , '' AS ICON_IMGFILE ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 END
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("          , '' AS MILEAGE ")
            .Append("          , '' AS DLRNICNM_LOCAL ")
            .Append("          , '' AS MAINTEAMOUNT ")
            .Append("          , '' AS JOBNO ")
            .Append("          , '' AS MILEAGESEQ ")
            .Append("          , '' AS DLRCD ")
            .Append("          , '' AS ORIGINALID ")
            .Append("          , '' AS VIN ")
            .Append("          , '' AS VCLREGNO ")
            .Append("          , TO_CHAR(C.OPERATIONCODE) AS OPERATIONCODE ")
            If pastFlg = True Then
                'PAST
                .Append("       FROM TBL_FLLWUPBOXRSLT_PAST A ")
                .Append("          , TBL_FLLWUPBOX_PAST B ")
            Else
                .Append("       FROM TBL_FLLWUPBOXRSLT A ")
                .Append("          , TBL_FLLWUPBOX B ")
            End If
            .Append("          , TBL_USERS C ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 DELETE
            .Append("          , TBL_OPERATIONTYPE E ")
            .Append("      WHERE A.CRACTSTATUS IN ('1', '2', '7') ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("        AND A.insdid = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("        AND (A.insdid = :CRCUSTID OR A.insdid = :NEW_CUST_ID) ")
                End If
            Else
                '未取引客
                .Append("        AND A.insdid = :CRCUSTID ")
            End If
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND B.DLRCD = A.DLRCD ")
            .Append("        AND B.STRCD = A.STRCD ")
            .Append("        AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("        AND C.ACCOUNT(+) = A.ACCOUNT_ACTUAL ")
            .Append("        AND C.DELFLG(+) = '0' ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 DELETE
            .Append("        AND E.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND E.DLRCD(+) = :DLRCD ")
            .Append("        AND E.STRCD(+) = :STRCD ")
            .Append("        AND E.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 来店受付
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function V3ContactHistoryWalkInSqlCreate(ByVal newCustId As String, _
                                                           ByVal pastFlg As Boolean, _
                                                           ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 来店受付 */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.WALKINDATE AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , D.COUNTVIEW ")
            .Append("          , TO_CHAR(D.CONTACT) AS CONTACT ")
            .Append("          , CASE WHEN B.CREATE_CRACTRESULT = '0' THEN '1' ")
            .Append("                 WHEN B.CREATE_CRACTRESULT = '1' THEN '3' ")
            .Append("                 WHEN B.CREATE_CRACTRESULT = '2' THEN '2' ")
            .Append("            END AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 START
            .Append("          , '' AS ICON_IMGFILE ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 END
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("          , TO_CHAR(C.OPERATIONCODE) AS OPERATIONCODE ")
            .Append("       FROM TBL_WALKINPERSON A ")
            If pastFlg = True Then
                'PAST
                .Append("          , TBL_FLLWUPBOX_PAST B ")
            Else
                .Append("          , TBL_FLLWUPBOX B ")
            End If
            .Append("          , TBL_USERS C ")
            .Append("          , TBL_CONTACTMETHOD D ")
            .Append("          , TBL_NEWCUSTOMER E ")
            .Append("          , TBL_OPERATIONTYPE F ")
            .Append("      WHERE A.REGISTRATIONTYPE <> '3' ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND B.DLRCD = A.DLRCD ")
            .Append("        AND B.STRCD = A.STRCD ")
            .Append("        AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("        AND C.ACCOUNT(+) = A.ACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.CONTACTNO(+) = A.CONTACTNO ")
            .Append("        AND D.DELFLG(+) = '0' ")
            .Append("        AND E.DLRCD = :DLRCD ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("        AND E.ORIGINALID = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("        AND (E.ORIGINALID = :CRCUSTID OR E.CSTID = :NEW_CUST_ID) ")
                End If
            Else
                '未取引客
                .Append("        AND E.CSTID = :CRCUSTID ")
            End If
            .Append("        AND A.CSTID = E.CSTID ")
            .Append("        AND F.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND F.DLRCD(+) = :DLRCD ")
            .Append("        AND F.STRCD(+) = :STRCD ")
            .Append("        AND F.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 来店受付 Follow Null
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function V3ContactHistoryWalkInNullSqlCreate(ByVal newCustId As String, _
                                                               ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 来店受付 */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.WALKINDATE AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , D.COUNTVIEW ")
            .Append("          , TO_CHAR(D.CONTACT) AS CONTACT ")
            .Append("          , null AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 START
            .Append("          , '' AS ICON_IMGFILE ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 END
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("          , TO_CHAR(C.OPERATIONCODE) AS OPERATIONCODE ")
            .Append("       FROM TBL_WALKINPERSON A ")
            .Append("          , TBL_USERS C ")
            .Append("          , TBL_CONTACTMETHOD D ")
            .Append("          , TBL_NEWCUSTOMER E ")
            .Append("          , TBL_OPERATIONTYPE F ")
            .Append("      WHERE A.REGISTRATIONTYPE <> '3' ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND NOT EXISTS(SELECT 1 FROM TBL_FLLWUPBOX B ")
            .Append("                        WHERE B.DLRCD = A.DLRCD ")
            .Append("                          AND B.STRCD = A.STRCD ")
            .Append("                          AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("                       ) ")
            .Append("        AND NOT EXISTS(SELECT 1 FROM TBL_FLLWUPBOX_PAST B ")
            .Append("                        WHERE B.DLRCD = A.DLRCD ")
            .Append("                          AND B.STRCD = A.STRCD ")
            .Append("                          AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("                       )")
            .Append("        AND C.ACCOUNT(+) = A.ACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.CONTACTNO(+) = A.CONTACTNO ")
            .Append("        AND D.DELFLG(+) = '0' ")
            .Append("        AND E.DLRCD = :DLRCD ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("        AND E.ORIGINALID = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("        AND (E.ORIGINALID = :CRCUSTID OR E.CSTID = :NEW_CUST_ID) ")
                End If
            Else
                '未取引客
                .Append("        AND E.CSTID = :CRCUSTID ")
            End If
            .Append("        AND A.CSTID = E.CSTID ")
            .Append("        AND F.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND F.DLRCD(+) = :DLRCD ")
            .Append("        AND F.STRCD(+) = :STRCD ")
            .Append("        AND F.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴　CR用SQL作成
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function V3ContactHistoryCRSqlCreate(ByVal newCustId As String, _
                                                        ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 苦情 */ ")
            .Append("            '3' AS ACTUALKIND ")
            .Append("          , B.ACTUAL_DATE AS ACTUALDATE ")
            .Append("          , 0 AS CONTACTNO ")
            .Append("          , 0 AS FLLWUPBOX_SEQNO ")
            .Append("          , '0' AS COUNTVIEW ")
            .Append("          , TO_CHAR('%1' || '%2' || NVL(C.CLM_IMPORTANCE,'-') || '%2' || NVL(D.CLM_CATEGORYTITLE,'-')) AS CONTACT ")
            .Append("          , A.STATUS AS CRACTSTATUS ")
            .Append("          , TO_CHAR(E.USERNAME) AS USERNAME ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 START
            .Append("          , '' AS ICON_IMGFILE ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 END
            .Append("          , A.UPDATEDATE ")
            .Append("          , TO_CHAR(A.COMPLAINT_OVERVIEW) AS COMPLAINT_OVERVIEW ")
            .Append("          , TO_CHAR(B.ACTUAL_DETAIL) AS ACTUAL_DETAIL ")
            .Append("          , TO_CHAR(B.MEMO) AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("          , '' AS MILEAGE ")
            .Append("          , '' AS DLRNICNM_LOCAL ")
            .Append("          , '' AS MAINTEAMOUNT ")
            .Append("          , '' AS JOBNO ")
            .Append("          , '' AS MILEAGESEQ ")
            .Append("          , '' AS DLRCD ")
            .Append("          , '' AS ORIGINALID ")
            .Append("          , '' AS VIN ")
            .Append("          , '' AS VCLREGNO ")
            .Append("          , TO_CHAR(E.OPERATIONCODE) AS OPERATIONCODE ")
            .Append("       FROM TBL_CLM_COMPLAINT A ")
            .Append("          , TBL_CLM_COMPLAINTDETAIL B ")
            .Append("          , TBL_CLM_IMPORTANCE C ")
            .Append("          , TBL_CLM_CATEGORY D ")
            .Append("          , TBL_USERS E ")
            .Append("          , TBL_OPERATIONTYPE F ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 START
            .Append("      WHERE ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("            A.CSTKIND = :CSTKIND ")
                    .Append("        AND A.INSDID = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("            ((A.CSTKIND = '1' AND A.INSDID = :CRCUSTID) ")
                    .Append("         OR (A.CSTKIND = '2' AND A.INSDID = :NEW_CUST_ID)) ")
                End If
            Else
                '未取引客
                .Append("            A.CSTKIND = :CSTKIND ")
                .Append("        AND A.INSDID = :CRCUSTID ")
            End If
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 END
            .Append("        AND B.COMPLAINTNO(+) = A.COMPLAINTNO ")
            .Append("        AND B.DIST_FLG(+) = '0' ")
            .Append("        AND C.CLM_IMPORTANCENO(+) = A.CLM_IMPORTANCENO ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.CLM_CATEGORYNO(+) = A.CLM_CATEGORYNO ")
            .Append("        AND D.DELFLG(+) = '0' ")
            .Append("        AND E.ACCOUNT(+) = B.ACTUAL_ACCOUNT ")
            .Append("        AND E.DELFLG(+) = '0' ")
            .Append("        AND F.OPERATIONCODE(+) = E.OPERATIONCODE ")
            .Append("        AND F.DLRCD(+) = :DLRCD ")
            .Append("        AND F.STRCD(+) = :STRCD ")
            .Append("        AND F.DELFLG(+) = '0' ")
        End With
        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 Follow-upBox
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks>Follow-upBoxベースで履歴取得</remarks>
    Public Shared Function V3ContactHistoryFollowupBoxSqlCreate(ByVal newCustId As String,
                                                                ByVal pastFlg As Boolean,
                                                                ByVal cstKind As String) As String

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT ")
            .Append("    '1' AS ACTUALKIND, ")
            .Append("    C.ACTUALDATE AS ACTUALDATE, ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 START
            .Append("    0 AS CONTACTNO, ")
            .Append("    C.FLLWUPBOX_SEQNO AS FLLWUPBOX_SEQNO, ")
            .Append("    '0' AS COUNTVIEW, ")
            .Append("    '' AS CONTACT, ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 END
            .Append("    CRACTSTATUS AS CRACTSTATUS, ")
            .Append("    TO_CHAR(E.USERNAME) AS USERNAME, ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 START
            .Append("    '' AS ICON_IMGFILE, ")
            '2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36 END
            .Append("    C.UPDATEDATE AS UPDATEDATE, ")
            .Append("    '' AS COMPLAINT_OVERVIEW, ")
            .Append("    '' AS ACTUAL_DETAIL, ")
            .Append("    '' AS MEMO, ")
            .Append("    0 AS ORDER_NO, ")
            .Append("    '' AS MILEAGE, ")
            .Append("    '' AS DLRNICNM_LOCAL, ")
            .Append("    '' AS MAINTEAMOUNT, ")
            .Append("    '' AS JOBNO, ")
            .Append("    '' AS MILEAGESEQ, ")
            .Append("    '' AS DLRCD, ")
            .Append("    '' AS ORIGINALID, ")
            .Append("    '' AS VIN, ")
            .Append("    '' AS VCLREGNO, ")
            .Append("    TO_CHAR(E.OPERATIONCODE) AS OPERATIONCODE ")
            .Append("FROM ")
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("        NVL(B.WALKINDATE,A.CREATEDATE) AS ACTUALDATE, ")
            .Append("        A.FLLWUPBOX_SEQNO AS FLLWUPBOX_SEQNO, ")
            .Append("        CASE ")
            .Append("        WHEN A.CRACTSTATUS_1ST = '1' THEN ")
            .Append("            '3' ")
            .Append("        WHEN A.CRACTSTATUS_1ST = '2' THEN ")
            .Append("            '2' ")
            .Append("        WHEN A.CRACTSTATUS_1ST = '7' THEN ")
            .Append("            '1' ")
            .Append("        END AS CRACTSTATUS, ")
            .Append("        NVL(B.UPDATEDATE,A.UPDATEDATE) AS UPDATEDATE, ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 DELETE
            .Append("        NVL(B.ACCOUNT,A.CREATEDBY) AS ACCOUNT ")
            .Append("    FROM ")
            If pastFlg = True Then
                .Append("        TBL_FLLWUPBOX_PAST A, ")
            Else
                .Append("        TBL_FLLWUPBOX A, ")
            End If
            .Append("        TBL_WALKINPERSON B ")
            .Append("    WHERE ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                If String.IsNullOrEmpty(newCustId) Then
                    .Append("        A.INSDID = :CRCUSTID AND ")
                Else
                    .Append("        (A.INSDID = :CRCUSTID OR A.UNTRADEDCSTID = :NEW_CUST_ID) AND ")
                End If
            Else
                .Append("        A.UNTRADEDCSTID = :CRCUSTID AND ")
            End If
            .Append("        A.CUSTOMERCLASS = :CUSTOMERCLASS AND ")
            .Append("        A.CRACTSTATUS_1ST IN ('1','2','7') AND ")
            .Append("        B.DLRCD(+) = A.DLRCD AND ")
            .Append("        B.STRCD(+) = A.STRCD AND ")
            .Append("        B.FLLWUPBOX_SEQNO(+) = A.FLLWUPBOX_SEQNO AND ")
            .Append("        NOT EXISTS ")
            .Append("            ( ")
            .Append("            SELECT ")
            .Append("                1 ")
            .Append("            FROM ")
            .Append("                TBL_WALKINPERSON ")
            .Append("            WHERE ")
            .Append("                REGISTRATIONTYPE = '3' AND ")
            .Append("                A.PARENT_FLLWUPBOX_SEQNO IS NULL AND ")
            .Append("                DLRCD(+) = A.DLRCD AND ")
            .Append("                STRCD(+) = A.STRCD AND ")
            .Append("                FLLWUPBOX_SEQNO(+) = A.FLLWUPBOX_SEQNO ")
            .Append("            ) ")
            .Append("    ) C, ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 DELETE
            .Append("    TBL_USERS E, ")
            .Append("    TBL_OPERATIONTYPE F ")
            .Append("WHERE ")
            '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 DELETE
            .Append("    E.ACCOUNT(+) = C.ACCOUNT AND ")
            .Append("    E.DELFLG(+) = '0' AND ")
            .Append("    F.OPERATIONCODE(+) = E.OPERATIONCODE AND ")
            .Append("    F.DLRCD(+) = :DLRCD AND ")
            .Append("    F.STRCD(+) = :STRCD AND ")
            .Append("    F.DELFLG(+) = '0' ")
        End With
        Return sql.ToString()
    End Function

    '2018/11/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-3 :サービスのV3履歴は参照しない DELETE

    ''' <summary>
    ''' V3顧客ID取得
    ''' </summary>
    ''' <param name="crcustid">顧客ID</param>
    ''' <returns>SC3080204Cst_CDDataTable</returns>
    ''' <remarks>V3用の顧客IDを取得</remarks>
    Public Shared Function GetV3CustomerCD(ByVal crcustid As Decimal, ByVal dlr_cd As String) As ActivityInfoDataSet.ActivityInfoCst_CDDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_302 */ ")
            .Append("    NVL(TRIM(NEWCST_CD),ORGCST_CD) AS CST_CD, ")
            .Append("    CST_TYPE ")
            .Append("FROM ")
            .Append("    TB_M_CUSTOMER A, ")
            .Append("    TB_M_CUSTOMER_DLR B ")
            .Append("WHERE ")
            .Append("    A.CST_ID = :CST_ID ")
            .Append("AND B.CST_ID = A.CST_ID ")
            .Append("AND B.DLR_CD = :DLR_CD ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCst_CDDataTable)("SC3080201_302")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, crcustid)   '内部管理ID
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Varchar2, dlr_cd)   '内部管理ID
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' V3顧客ID取得
    ''' </summary>
    ''' <param name="originalid">顧客ID</param>
    ''' <returns>SC3080204Cst_CDDataTable</returns>
    ''' <remarks>V3用の顧客IDを取得</remarks>
    Public Shared Function GetV3NewCustomerCD(ByVal originalid As String) As ActivityInfoDataSet.ActivityInfoCst_CDDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_303 */ ")
            .Append("    CSTID AS CST_CD ")
            .Append("FROM ")
            .Append("    TBL_NEWCUSTOMER ")
            .Append("WHERE ")
            .Append("    ORIGINALID = :ORIGINALID ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoCst_CDDataTable)("SC3080201_303", DBQueryTarget.DMS)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Varchar2, originalid)   '内部管理ID
            Return query.GetData()
        End Using
    End Function

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

#End Region

    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
    ''' <summary>
    ''' 受注後活動内容取得
    ''' </summary>
    ''' <param name="actidList">活動IDのリスト</param>
    ''' <param name="afterOdrFllwSeqList">受注後工程フォロー結果連番のリスト</param>
    ''' <returns>受注後活動内容</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactAfterOdrAct(actidList As List(Of Decimal), _
                                                 afterOdrFllwSeqList As List(Of Decimal) _
                                                 ) As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable

        Logger.Info("GetContactAfterOdrAct_Start")

        Dim bookedAfterFllwSeqFlg As Boolean = (afterOdrFllwSeqList IsNot Nothing AndAlso afterOdrFllwSeqList.Count > 0)
        Dim actidFlg As Boolean = (actidList IsNot Nothing AndAlso actidList.Count > 0)

        If (actidFlg Or bookedAfterFllwSeqFlg) = False Then
            Logger.Info("GetContactAfterOdrAct_End2")
            Return New ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable
        End If
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable)("SC3080201_203")
            Dim sql As New StringBuilder
            Dim sqlSub As New StringBuilder
            Dim sqlInCondition As New StringBuilder

            '受注後工程フォロー結果連番、活動IDの検索条件作成
            '受注後工程フォロー結果連番
            If bookedAfterFllwSeqFlg Then
                Dim cnt As Integer = 0
                sqlInCondition.Append(" T1.AFTER_ODR_FLLW_SEQ IN (")
                '要素の数だけ条件を設定
                For Each seq In afterOdrFllwSeqList
                    If cnt > 0 Then
                        sqlInCondition.Append(",")
                    End If
                    query.AddParameterWithTypeValue("SEQ" & cnt.ToString(CultureInfo.CurrentCulture), OracleDbType.Decimal, seq)
                    sqlInCondition.Append(":SEQ" & cnt.ToString(CultureInfo.CurrentCulture))
                    cnt += 1
                Next
                sqlInCondition.AppendLine(")")
            End If

            '活動ID
            If actidFlg Then
                If bookedAfterFllwSeqFlg Then
                    sqlInCondition.Append("            OR")
                End If

                Dim cnt As Integer = 0
                sqlInCondition.Append(" T1.ACT_ID IN (")
                '要素の数だけ条件を設定
                For Each id In actidList
                    If cnt > 0 Then
                        sqlInCondition.Append(",")
                    End If
                    query.AddParameterWithTypeValue("ID" & cnt.ToString(CultureInfo.CurrentCulture), OracleDbType.Decimal, id)
                    sqlInCondition.Append(":ID" & cnt.ToString(CultureInfo.CurrentCulture))
                    cnt += 1
                Next
                sqlInCondition.AppendLine(")")
            End If

            sqlSub.AppendLine("SELECT /* SC3080201_203 */")
            sqlSub.AppendLine("       ACT_ID")
            sqlSub.AppendLine("     , AFTER_ODR_FLLW_SEQ")
            sqlSub.AppendLine("     , AFTER_ODR_ACT_ID")
            sqlSub.AppendLine("     , AFTER_ODR_ACT_NAME")
            sqlSub.AppendLine("FROM (")
            sqlSub.AppendLine("    SELECT")
            sqlSub.AppendLine("           T1.ACT_ID")
            sqlSub.AppendLine("         , T1.AFTER_ODR_FLLW_SEQ")
            sqlSub.AppendLine("         , T1.AFTER_ODR_ACT_ID")
            sqlSub.AppendLine("         , CASE WHEN T1.STD_VOLUNTARYINS_ACT_TYPE = '1' ")
            sqlSub.AppendLine("                  THEN CASE WHEN T4.WORD_VAL = ' ' THEN T4.WORD_VAL_ENG")
            sqlSub.AppendLine("                            ELSE T4.WORD_VAL")
            sqlSub.AppendLine("                       END")
            sqlSub.AppendLine("                ELSE T1.VOLUNTARYINS_ACT_NAME")
            sqlSub.AppendLine("           END AS AFTER_ODR_ACT_NAME")
            sqlSub.AppendLine("         , T1.SCHE_DATEORTIME_FLG")
            sqlSub.AppendLine("         , T1.SCHE_END_DATEORTIME")
            sqlSub.AppendLine("         , T2.SORT_ORDER AS SORT_ORDER_PROC")
            sqlSub.AppendLine("         , T1.STD_VOLUNTARYINS_ACT_TYPE")
            sqlSub.AppendLine("         , T3.SORT_ORDER AS SORT_ORDER_ACT")
            sqlSub.AppendLine("    FROM TB_T_AFTER_ODR_ACT T1")
            sqlSub.AppendLine("       , TB_M_AFTER_ODR_PROC T2")
            sqlSub.AppendLine("       , TB_M_AFTER_ODR_ACT T3")
            sqlSub.AppendLine("       , TB_M_WORD T4")
            sqlSub.AppendLine("    WHERE T1.AFTER_ODR_PRCS_CD = T2.AFTER_ODR_PRCS_CD(+)")
            sqlSub.AppendLine("      AND T1.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD(+)")
            sqlSub.AppendLine("      AND T3.AFTER_ODR_ACT_NAME = T4.WORD_CD(+)")
            sqlSub.Append("      AND (")
            sqlSub.Append(sqlInCondition.ToString)
            sqlSub.AppendLine("          )")
            sqlSub.AppendLine("    UNION ALL")
            sqlSub.AppendLine("    SELECT")
            sqlSub.AppendLine("           T1.ACT_ID")
            sqlSub.AppendLine("         , T1.AFTER_ODR_FLLW_SEQ")
            sqlSub.AppendLine("         , T1.AFTER_ODR_ACT_ID")
            sqlSub.AppendLine("         , CASE WHEN T1.STD_VOLUNTARYINS_ACT_TYPE = '1' ")
            sqlSub.AppendLine("                  THEN CASE WHEN T4.WORD_VAL = ' ' THEN T4.WORD_VAL_ENG")
            sqlSub.AppendLine("                            ELSE T4.WORD_VAL")
            sqlSub.AppendLine("                       END")
            sqlSub.AppendLine("                ELSE T1.VOLUNTARYINS_ACT_NAME")
            sqlSub.AppendLine("           END AS AFTER_ODR_ACT_NAME")
            sqlSub.AppendLine("         , T1.SCHE_DATEORTIME_FLG")
            sqlSub.AppendLine("         , T1.SCHE_END_DATEORTIME")
            sqlSub.AppendLine("         , T2.SORT_ORDER AS SORT_ORDER_PROC")
            sqlSub.AppendLine("         , T1.STD_VOLUNTARYINS_ACT_TYPE")
            sqlSub.AppendLine("         , T3.SORT_ORDER AS SORT_ORDER_ACT")
            sqlSub.AppendLine("    FROM TB_H_AFTER_ODR_ACT T1")
            sqlSub.AppendLine("       , TB_M_AFTER_ODR_PROC T2")
            sqlSub.AppendLine("       , TB_M_AFTER_ODR_ACT T3")
            sqlSub.AppendLine("       , TB_M_WORD T4")
            sqlSub.AppendLine("    WHERE T1.AFTER_ODR_PRCS_CD = T2.AFTER_ODR_PRCS_CD(+)")
            sqlSub.AppendLine("      AND T1.AFTER_ODR_ACT_CD = T3.AFTER_ODR_ACT_CD(+)")
            sqlSub.AppendLine("      AND T3.AFTER_ODR_ACT_NAME = T4.WORD_CD(+)")
            sqlSub.Append("      AND (")
            sqlSub.Append(sqlInCondition.ToString)
            sqlSub.AppendLine("          )")
            sqlSub.AppendLine(")")
            sqlSub.AppendLine("ORDER BY AFTER_ODR_FLLW_SEQ")
            sqlSub.AppendLine("       , ACT_ID")
            sqlSub.AppendLine("       , SCHE_DATEORTIME_FLG DESC")
            sqlSub.AppendLine("       , SCHE_END_DATEORTIME")
            sqlSub.AppendLine("       , SORT_ORDER_PROC")
            sqlSub.AppendLine("       , STD_VOLUNTARYINS_ACT_TYPE")
            sqlSub.AppendLine("       , SORT_ORDER_ACT")
            sqlSub.AppendLine("       , AFTER_ODR_ACT_ID")

            query.CommandText = sqlSub.ToString()

            Logger.Info("GetContactAfterOdrAct_End1")
            Return query.GetData()
        End Using

    End Function
    '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetServiceInInfo(ByVal originalid As String, _
                                            ByVal vin As String, _
                                            ByVal dlrcd As String) As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceInInfo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080201_160 */ ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("  CASE T6.MAINTE_NAME WHEN N' ' THEN T4.MAINTE_NAME ELSE T6.MAINTE_NAME END AS INSPECTNM , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("  T6.SERVICECD , ")
            .Append("  T6.SV_PR , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("  NVL(T6.SERVICENAME,CASE T6.MAINTE_NAME WHEN N' ' THEN T4.MAINTE_NAME ELSE T6.MAINTE_NAME END) AS SERVICENAME , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("  T6.INSPECSEQ ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("    T3.MAINTE_NAME , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("    T3.SVC_CD AS SERVICECD , ")
            .Append("    ROW_NUMBER() OVER (ORDER BY NVL('',9999)) AS SV_PR , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("    T5.SVC_NAME_MILE AS SERVICENAME , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("    T3.INSPEC_SEQ AS INSPECSEQ ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("    , T3.DLR_CD ")
            .Append("    , T3.MAINTE_CD ")
            .Append("    , SUBSTR(T1.VCL_KATASHIKI,1,INSTR(T1.VCL_KATASHIKI,'-')-1) AS MAINTE_KATASHIKI ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("  FROM ")
            .Append("    TB_M_VEHICLE T1 , ")
            .Append("    TB_T_VEHICLE_SVCIN_HIS T2 , ")
            .Append("    TB_T_VEHICLE_MAINTE_HIS T3 , ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 DELETE
            .Append("    TB_M_SERVICE T5 ")
            .Append("  WHERE ")
            .Append("        T1.VCL_VIN = :VIN ")
            .Append("    AND T1.VCL_ID = T2.VCL_ID ")
            .Append("    AND T2.CST_ID = :ORIGINALID ")
            .Append("    AND T2.DLR_CD = :DLRCD ")
            .Append("    AND T2.DLR_CD = T3.DLR_CD ")
            .Append("    AND T2.SVCIN_NUM = T3.SVCIN_NUM ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 DELETE
            .Append("    AND T2.DLR_CD = T5.DLR_CD(+) ")
            .Append("    AND T2.SVC_CD = T5.SVC_CD(+) ")
            .Append("  ) T6 ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 START
            .Append("LEFT JOIN TB_M_MAINTE T4 ")
            .Append("ON (T6.DLR_CD = T4.DLR_CD ")
            .Append("    AND T6.MAINTE_CD = T4.MAINTE_CD ")
            .Append("    AND T6.MAINTE_KATASHIKI = T4.MAINTE_KATASHIKI ")
            .Append(") ")
            .Append("WHERE (T6.MAINTE_NAME <> ' ' OR T4.MAINTE_NAME IS NOT NULL) ")
            '2015/12/14 TCS 市川 （トライ店システム評価）サービス入庫管理機能の冗長化対応 END
            .Append("ORDER BY ")
            .Append("  T6.INSPECSEQ ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable)("SC3080201_160")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceInInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Dim rtnDt As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function

    Public Shared Function GetBasesystemNM() As ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable
        Dim sql As New StringBuilder
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBasesystemNM_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_061 */ ")
            .Append("    WORD_VAL AS BASESYSTEMNM ")
            .Append("FROM ")
            .Append("    TB_M_WORD ")
            .Append("WHERE ")
            .Append("    WORD_CD = '50070' ")
        End With
        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable)("SC3080201_061")
            query.CommandText = sql.ToString()
            Dim rtnDt As ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable = query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBasesystemNM_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return rtnDt
        End Using
    End Function
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発

    '2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 START
    ''' <summary>
    ''' 見積印刷実績確認
    ''' </summary>
    ''' <param name="sales_id"></param>
    ''' <param name="pref_vcl_seq"></param>
    ''' <param name="useFlgSuffix"></param>
    ''' <param name="useFlgInteriorClr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsPrintedEstimate(ByVal sales_id As Decimal, ByVal pref_vcl_seq As Integer, ByVal useFlgSuffix As String, ByVal useFlgInteriorClr As String) As Decimal

        Using query As New DBSelectQuery(Of ActivityInfoDataSet.ActivityInfoEstimateIDDataTable)("ActivityInfo_628")
            Dim sql As New StringBuilder

            With sql
                .AppendLine("SELECT ")
                .AppendLine("    /*ActivityInfo_628*/ ")
                .AppendLine("    NVL(ESTIMATEID, 0) AS ESTIMATEID ")
                .AppendLine("FROM ")
                .AppendLine("( ")
                .AppendLine("    SELECT ")
                .AppendLine("        B.ESTIMATEID ")
                .AppendLine("    FROM ")
                .AppendLine("        TB_T_PREFER_VCL A, ")
                .AppendLine("        TBL_ESTIMATEINFO B, ")
                .AppendLine("        TBL_EST_VCLINFO C ")
                .AppendLine("    WHERE ")
                .AppendLine("        A.SALES_ID = :SALES_ID ")
                .AppendLine("    AND A.PREF_VCL_SEQ = :PREF_VCL_SEQ ")
                .AppendLine("    AND B.FLLWUPBOX_SEQNO = A.SALES_ID ")
                .AppendLine("    AND B.EST_ACT_FLG = '1' ")
                .AppendLine("    AND B.DELFLG = '0' ")
                .AppendLine("    AND C.ESTIMATEID = B.ESTIMATEID ")
                .AppendLine("    AND C.SERIESCD = A.MODEL_CD ")
                .AppendLine("    AND C.MODELCD = A.GRADE_CD ")
                If String.Equals(useFlgSuffix, USE_FLG_SUFFIX_TURE) Then
                    .AppendLine("    AND C.SUFFIXCD = A.SUFFIX_CD ")
                End If
                .AppendLine("    AND C.EXTCOLORCD = A.BODYCLR_CD ")
                If String.Equals(useFlgInteriorClr, USE_INTERIOR_CLR_TURE) Then
                    .AppendLine("    AND C.INTCOLORCD = A.INTERIORCLR_CD ")
                End If
                .AppendLine("    ORDER BY ESTIMATEID DESC")
                .AppendLine(") ")
                .AppendLine("WHERE ")
                .AppendLine("    ROWNUM = 1 ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, sales_id)
            query.AddParameterWithTypeValue("PREF_VCL_SEQ", OracleDbType.Int16, pref_vcl_seq)

            Dim ActivityInfoEstimateIDDataTable As New ActivityInfoDataSet.ActivityInfoEstimateIDDataTable
            ActivityInfoEstimateIDDataTable = query.GetData()

            Dim estimateID As Decimal
            If ActivityInfoEstimateIDDataTable.Count > 0 Then
                Dim SC3080201SystemSettingDlrRW As ActivityInfoDataSet.ActivityInfoEstimateIDRow
                SC3080201SystemSettingDlrRW = CType(ActivityInfoEstimateIDDataTable.Rows(0), ActivityInfoDataSet.ActivityInfoEstimateIDRow)
                estimateID = SC3080201SystemSettingDlrRW.ESTIMATEID
            End If

            Return estimateID

        End Using

        Return 0
    End Function

    ''' <summary>
    ''' 見積結果更新
    ''' </summary>
    ''' <param name="sales_id"></param>
    ''' <param name="pref_vcl_seq"></param>
    ''' <param name="contact_mtd"></param>
    ''' <param name="est_amount"></param>
    ''' <param name="stf_cd"></param>
    ''' <param name="account"></param>
    ''' <param name="row_function"></param>
    ''' <param name="lock_version"></param>
    ''' <param name="orgnz_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateEstimateAmount(ByVal sales_id As Decimal, ByVal pref_vcl_seq As Integer, ByVal contact_mtd As String, ByVal est_amount As Double, ByVal stf_cd As String,
                                                ByVal account As String, ByVal row_function As String, ByVal lock_version As Long, ByVal orgnz_id As String) As Integer

        Using query As New DBUpdateQuery("ActivityInfo_629")
            Dim sql As New StringBuilder

            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_629 */")
                .AppendLine("    TB_T_PREFER_VCL")
                .AppendLine(" SET")
                .AppendLine("    EST_RSLT_DATE = SYSDATE ,")
                .AppendLine("    EST_RSLT_CONTACT_MTD = :EST_RSLT_CONTACT_MTD ,")
                .AppendLine("    EST_AMOUNT = :EST_AMOUNT ,")
                .AppendLine("    EST_RSLT_FLG = '1' ,")
                .AppendLine("    EST_RSLT_STF_CD = :EST_RSLT_STF_CD ,")
                .AppendLine("    EST_RSLT_DEPT_ID = :EST_RSLT_DEPT_ID ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        SALES_ID = TO_NUMBER(:SALES_ID)")
                .AppendLine("    AND PREF_VCL_SEQ = TO_NUMBER(:PREF_VCL_SEQ)")
                .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, sales_id)
            query.AddParameterWithTypeValue("PREF_VCL_SEQ", OracleDbType.Int16, pref_vcl_seq)
            If String.IsNullOrEmpty(contact_mtd) Then
                contact_mtd = " "
            End If
            query.AddParameterWithTypeValue("EST_RSLT_CONTACT_MTD", OracleDbType.NVarchar2, contact_mtd)
            query.AddParameterWithTypeValue("EST_AMOUNT", OracleDbType.Double, est_amount)
            query.AddParameterWithTypeValue("EST_RSLT_STF_CD", OracleDbType.NVarchar2, stf_cd)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, row_function)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lock_version)
            query.AddParameterWithTypeValue("EST_RSLT_DEPT_ID", OracleDbType.Decimal, orgnz_id)

            Return query.Execute()

        End Using
    End Function
    '2019/04/05 TS  河原 見積印刷実績がある状態で活動結果登録を行うとエラーになる件 END

    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) START
    ''' <summary>
    ''' 用件ソース変更可能フラグ更新
    ''' </summary>
    ''' <param name="sales_id">商談ID</param>
    ''' <param name="row_update_account">行更新アカウント</param>
    ''' <param name="row_function">行更新機能</param>
    ''' <returns>処理件数</returns>
    ''' <remarks>活動結果登録時に【用件ソース変更可能フラグ】を固定で0に更新</remarks>
    Public Shared Function UpdateSourceChgPossibleFlg(ByVal sales_id As Decimal, ByVal row_update_account As String, ByVal row_function As String) As Integer

        Using query As New DBUpdateQuery("ActivityInfo_630")
            Dim sql As New StringBuilder

            With sql
                .AppendLine("UPDATE")
                .AppendLine("    /* ActivityInfo_630 */")
                .AppendLine("    TB_LT_SALES")
                .AppendLine(" SET")
                .AppendLine("    SOURCE_1_CHG_POSSIBLE_FLG = '0' ,")
                .AppendLine("    SOURCE_2_CHG_POSSIBLE_FLG = '0' ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
                .AppendLine(" WHERE")
                .AppendLine("        SALES_ID = :SALES_ID")
                .AppendLine("    AND (SOURCE_1_CHG_POSSIBLE_FLG = '1' OR SOURCE_2_CHG_POSSIBLE_FLG = '1')")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Int64, sales_id)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, row_update_account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, row_function)

            Return query.Execute()

        End Using
    End Function
    '2020/01/23 TS  河原 TKM Change request development for Next Gen e-CRB (CR058,CR061) END

End Class

