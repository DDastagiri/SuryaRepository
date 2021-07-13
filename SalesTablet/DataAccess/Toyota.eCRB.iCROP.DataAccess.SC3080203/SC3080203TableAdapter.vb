'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動結果)
'補足： 
'作成： 2011/12/01 TCS 河原
'更新： 2012/02/15 TCS 河原 【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正
'更新： 2012/03/02 TCS 安田 【STEP2】接触方法マスタ・受注後フラグ条件追加 
'更新： 2012/09/13 TCS 山口 業活動開始時の活動開始時間の表示不良
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 松月 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15） 
'更新： 2014/02/02 TCS 松月 【A STEP2】希望車表示不具合対応（号口切替BTS-39）
'更新： 2014/02/26 TCS 松月 【A STEP2】担当未割当顧客不具合対応（問連TR-V4-GTMC140220003）
'更新： 2014/03/07 TCS 各務 再構築不具合対応マージ版
'更新： 2014/03/18 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）
'更新： 2014/04/21 TCS 松月 【A STEP2】業務種別設定不正対応（問連TR-V4-GTMC140416001）
'更新： 2014/05/15 TCS 武田 受注後フォロー機能開発
'更新： 2014/05/23 TCS 安田  TMT不具合対応
'更新： 2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ)
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2020/01/28 TS  舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) 
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 松月 2013/10対応版 既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 松月 2013/10対応版 既存流用 END
' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

Namespace SC3080203DataSetTableAdapters
    Public NotInheritable Class SC3080203TableAdapter

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

        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
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
        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

        '2013/06/30 TCS 内藤 2013/10対応版 既存流用 START
        Private Const BIZCATID As String = "48"
        '2013/06/30 TCS 内藤 2013/10対応版 既存流用 END

        '2013/12/11 TCS 市川 Aカード情報相互連携開発 START
        Private Const ACT_STATUS_GIVEUP As String = "32"
        '2013/12/11 TCS 市川 Aカード情報相互連携開発 END

        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
        ' 外版色コードを前3桁だけで比較するか否かフラグ(システム環境設定)
        Private Const EXTERIOR_COLOR_3_FLG As String = "EXTERIOR_COLOR_3_FLG"
        ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END

#End Region

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
            '処理なし
        End Sub

        ''' <summary>
        ''' 001.アラームマスタ取得
        ''' </summary>
        ''' <param name="selection">0: どちらでも選択可、1: 時間指定がある場合のみ選択可、2: 時間指定が無い場合のみ選択可</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetAlarmMaster(ByVal selection As String) As SC3080203DataSet.SC3080203AlarmMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203AlarmMasterDataTable)("SC3080203_001")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_001 */ ")
                    .Append("    ALARMNO, ")
                    .Append("    UNIT, ")
                    .Append("    TIME ")
                    .Append("FROM ")
                    .Append("    TBL_ALARM ")
                    .Append("WHERE ")
                    .Append("    DELFLG = '0' AND ")
                    .Append("    SELECTION IN ('0',:SELECTION) ")
                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SELECTION", OracleDbType.Char, selection)
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 002.競合メーカーマスタ取得(ALL)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionMakermasterDataTable)("SC3080203_102")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetitionMakermaster_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_102 */ ")
                    .Append("   MAKER_CD AS COMPETITIONMAKERNO, ")
                    .Append("   MAKER_NAME AS COMPETITIONMAKER ")
                    .Append(" FROM ")
                    .Append("   TB_M_MAKER ")
                    .Append(" WHERE ")
                    .Append("   MAKER_TYPE <> '1' ")
                    .Append(" ORDER BY ")
                    .Append("   SORT_ORDER ")
                End With
                query.CommandText = sql.ToString()
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetitionMakermaster_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 003.競合メーカーマスタ取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNoCompetitionMakermaster(ByVal dlrcd As String) As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionMakermasterDataTable)("SC3080203_103")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNoCompetitionMakermaster_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT DISTINCT ")
                    .Append("   /* SC3080203_103 */ ")
                    .Append("   T4.COMPETITIONMAKERNO , ")
                    .Append("   T4.COMPETITIONMAKER ")
                    .Append("   FROM ( ")
                    .Append("   SELECT ")
                    .Append("   T1.MAKER_CD AS COMPETITIONMAKERNO , ")
                    .Append("   T1.MAKER_NAME AS COMPETITIONMAKER ")
                    .Append(" FROM ")
                    .Append("   TB_M_MAKER T1, ")
                    .Append("   TB_M_MODEL T2, ")
                    .Append("   TB_M_MODEL_COMPETITOR_DLR T3 ")
                    .Append(" WHERE ")
                    .Append("   T1.MAKER_CD = T2.MAKER_CD ")
                    .Append("   AND T2.MODEL_CD = T3.COMP_MODEL_CD ")
                    .Append("   AND T2.INUSE_FLG = '1' ")
                    .Append("   AND (T3.DLR_CD = :DLRCD OR T3.DLR_CD = 'XXXXX') ")
                    .Append("   AND T3.DISP_TERM_START < SYSDATE ")
                    .Append("   AND T3.DISP_TERM_END > SYSDATE ")
                    .Append(" ORDER BY ")
                    .Append("   T1.SORT_ORDER ) T4")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNoCompetitionMakermaster_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 004.競合車種マスタ取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetitorMaster(ByVal dlrcd As String) As SC3080203DataSet.SC3080203CompetitorMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitorMasterDataTable)("SC3080203_104")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetitorMaster_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_104 */ ")
                    .Append("   T1.MAKER_CD AS COMPETITIONMAKERNO , ")
                    .Append("   T3.MAKER_NAME AS COMPETITIONMAKER , ")
                    .Append("   T1.MODEL_CD AS COMPETITORCD , ")
                    .Append("   T1.MODEL_NAME AS COMPETITORNM ")
                    .Append(" FROM ")
                    .Append("   TB_M_MODEL T1 , ")
                    .Append("   TB_M_MODEL_COMPETITOR_DLR T2 , ")
                    .Append("   TB_M_MAKER T3 ")

                    .Append(" WHERE ")
                    .Append("          T1.MODEL_CD = T2.COMP_MODEL_CD ")
                    .Append("   AND    T1.MAKER_CD = T3.MAKER_CD ")
                    .Append("   AND    T1.INUSE_FLG = '1' ")
                    .Append("   AND    (T2.DLR_CD =:DLRCD OR T2.DLR_CD = 'XXXXX') ")
                    .Append("   AND    T2.DISP_TERM_START < SYSDATE ")
                    .Append("   AND    T2.DISP_TERM_END > SYSDATE ")
                    .Append(" ORDER BY ")
                    .Append("   T3.SORT_ORDER, ")
                    .Append("   T1.MODEL_CD ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCompetitorMaster_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 005.関連情報取得
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRelatedInfo(ByVal vin As String) As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_105")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRelatedInfo_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_105 */ ")
                    .Append("   SUM(CNT) AS SEQ ")
                    .Append(" FROM ")
                    .Append("   ( ")
                    .Append("   SELECT ")
                    .Append("     COUNT(1) AS CNT ")
                    .Append("   FROM ")
                    .Append("     TB_M_INSURANCE ")
                    .Append("   WHERE ")
                    .Append("         VCL_VIN = :VIN ")
                    .Append("     AND ROWNUM = 1 ")
                    .Append("   UNION ALL ")
                    .Append("     SELECT ")
                    .Append("         COUNT(1) AS CNT ")
                    .Append("     FROM ")
                    .Append("         TB_T_LOAN ")
                    .Append("     WHERE ")
                    .Append("       VCL_ID = ")
                    .Append("         ( ")
                    .Append("         SELECT ")
                    .Append("           VCL_ID ")
                    .Append("         FROM ")
                    .Append("           TB_M_VEHICLE ")
                    .Append("         WHERE ")
                    .Append("           VCL_VIN = :VIN ")
                    .Append("         ) ")
                    .Append("       AND ROWNUM = 1 ")
                    .Append("   ) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRelatedInfo_Start")
                'ログ出力 End *****************************************************************************
                Return query.GetData()
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
            End Using
        End Function

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END



        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 008.Follow-up Box成約車種追加 (移行済み)
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
            Using query As New DBUpdateQuery("SC3080203_108")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSuccessSeries_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" INSERT /* SC3080203_108 */ ")
                    .Append("   INTO TB_T_PREFER_VCL ")
                    .Append("      ( ")
                    .Append("        SALES_ID ")
                    .Append("      , PREF_VCL_SEQ ")
                    .Append("      , MODEL_CD ")
                    .Append("      , GRADE_CD ")
                    .Append("      , SUFFIX_CD ")
                    .Append("      , BODYCLR_CD ")
                    .Append("      , INTERIORCLR_CD ")
                    .Append("      , SALESBKG_NUM ")
                    .Append("      , PREF_AMOUNT ")
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
                    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                    .Append("      , ' ' ")
                    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                    .Append("      , 1 ")
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
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
                query.AddParameterWithTypeValue("PREF_VCL_SEQ", OracleDbType.Int64, prefvclseq)
                query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, modelcd)
                query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, gradecd)
                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, bodyclrcd)
                query.AddParameterWithTypeValue("INSERTACCOUNT", OracleDbType.NVarchar2, acount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, acount)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSuccessSeries_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


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

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 020.Follow-up Box商談メモ追加　(移行済み)
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
                .Append(" INSERT ")
                .Append("     /* SC3080203_120 */ ")
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
            Using query As New DBUpdateQuery("SC3080203_020")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, folloupseqno)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcstid)
                query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
                query.AddParameterWithTypeValue("ACT_ID", OracleDbType.Decimal, actid)
                query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.NVarchar2, acount)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSalesmemo_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

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
        ''' 026.サービスマスタ取得 (移行済み)
        ''' </summary>
        ''' <param name="mntncd"></param>
        ''' <param name="dlrcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceMaster(ByVal mntncd As String, ByVal dlrcd As String) As SC3080203DataSet.SC3080203ServiceMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ServiceMasterDataTable)("SC3080203_126")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceMaster_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_126 */ ")
                    .Append("   SVC_CD AS SERVICECD , ")
                    .Append("   SVC_NAME_MILE AS SERVICENAME ")
                    .Append(" FROM ")
                    .Append("   TB_M_SERVICE ")
                    .Append(" WHERE ")
                    .Append("       MNTNCD = :MNTNCD ")
                    .Append("   AND DLR_CD = :DLRCD ")
                    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MNTNCD", OracleDbType.NVarchar2, mntncd)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceMaster_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 027.中項目マスタ取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="brncd"></param>
        ''' <param name="attplancd"></param>
        ''' <param name="attplanversion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSubCategory(ByVal dlrcd As String, ByVal brncd As String,
                                              ByVal attplancd As Decimal, ByVal attplanversion As Decimal) As SC3080203DataSet.SC3080203SubCategoryDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SubCategoryDataTable)("SC3080203_127")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSubCategory_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_127 */ ")
                    .Append("   ATTPLAN_NAME AS SUBCTGCODE  ")
                    .Append(" FROM ")
                    .Append("   TB_M_ATTPLAN ")
                    .Append(" WHERE ")
                    .Append("       DLR_CD = :DLR_CD ")
                    .Append("   AND BRN_CD = :BRN_CD ")
                    .Append("   AND ATTPLAN_ID = :ATTPLAN_ID ")
                    .Append("   AND ATTPLAN_VERSION = :ATTPLAN_VERSION ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brncd)
                query.AddParameterWithTypeValue("ATTPLAN_ID", OracleDbType.Decimal, attplancd)
                query.AddParameterWithTypeValue("ATTPLAN_VERSION", OracleDbType.Long, attplanversion)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSubCategory_End")
                'ログ出力 End *****************************************************************************
                Return query.GetData()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ''' <summary>
        ''' 029.担当スタッフ取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetUsers(ByVal dlrcd As String, ByVal strcd As String) As SC3080203DataSet.SC3080203UsersDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203UsersDataTable)("SC3080203_029")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_029 */ ")
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


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 033.自社客点検履歴取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="jobno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceHis(ByVal dlrcd As String, ByVal jobno As String) As SC3080203DataSet.SC3080201ServiceHisDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201ServiceHisDataTable)("SC3080203_133")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceHis_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_133 */ ")
                    .Append("   DLR_CD AS DLRCD , ")
                    .Append("   SVCIN_NUM AS JOBNO , ")
                    .Append("   INSPEC_SEQ AS INSPECSEQ , ")
                    .Append("   SVC_CD AS SERVICECD ")
                    .Append(" FROM ")
                    .Append("   TB_T_VEHICLE_MAINTE_HIS ")
                    .Append(" WHERE ")
                    .Append("       DLR_CD = :DLRCD ")
                    .Append("   AND SVCIN_NUM = :JOBNO ")
                    .Append("   AND INSPEC_SEQ IN ")
                    .Append("     ( ")
                    .Append("     SELECT ")
                    .Append("       MAX(INSPEC_SEQ) ")
                    .Append("     FROM ")
                    .Append("       TB_T_VEHICLE_MAINTE_HIS ")
                    .Append("     WHERE ")
                    .Append("           DLR_CD = :DLRCD ")
                    .Append("       AND SVCIN_NUM = :JOBNO ")
                    .Append("     GROUP BY ")
                    .Append("       DLR_CD , ")
                    .Append("       SVCIN_NUM ")
                    .Append("     ) ")
                    ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("JOBNO", OracleDbType.NVarchar2, jobno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceHis_End")
                'ログ出力 End *****************************************************************************
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


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 038.Follow-up Box商談メモWK削除 (移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DeleteFllwupboxSalesmemowk(ByVal fllwupboxseqno As Decimal) As Integer
            Using query As New DBUpdateQuery("SC3080203_038")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteFllwupboxSalesmemowk_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("DELETE /* SC3080203_038 */ ")
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
                Return query.Execute()
            End Using
        End Function
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 041.来店区分取得　(移行済み)
        ''' </summary>
        ''' <param name="wicid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetWinclass(ByVal wicid As String) As SC3080203DataSet.SC3080203WinclassDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203WinclassDataTable)("SC3080203_141")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWinclass_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_141 */ ")
                    .Append("   SOURCE_1_NAME AS WICNAME , ")
                    .Append("   ' ' AS ACTIONCD ")
                    .Append(" FROM ")
                    .Append("   TB_M_SOURCE_1 ")
                    .Append(" WHERE ")
                    .Append("   SOURCE_1_CD = :WICID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("WICID", OracleDbType.Char, wicid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetWinclass_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 043.Follow-upBox取得 (移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupBox(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203FllwupBoxDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupBoxDataTable)("SC3080203_143")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupBox_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT  ")
                    .Append("   /* SC3080203_143 */  ")
                    .Append("   0 AS CRPLAN_ID ,  ")
                    .Append("   ' ' AS BFAFDVS ,  ")
                    .Append("   0 AS CRDVSID ,  ")
                    .Append("   ' ' AS PLANDVS ,  ")
                    .Append("   0 AS SUBCTGCODE ,  ")
                    .Append("   0 AS PROMOTION_ID ,  ")
                    .Append("   TO_CHAR(T4.BIZ_TYPE) AS REQUESTID ,  ")
                    .Append("   T4.CST_ID AS UNTRADEDCSTID ,  ")
                    .Append("   T6.ACT_ID AS VCLSEQNO ,  ")
                    .Append("   T6.SCHE_STF_CD AS ACCOUNT_PLAN ,  ")
                    .Append("   T4.CST_ID AS INSDID ,  ")
                    .Append("   T1.VCL_VIN AS VIN ,  ")
                    .Append("   T9.CST_TYPE AS MEMKIND ,  ")
                    .Append("   T7.SLS_PIC_BRN_CD AS CUSTCHRGSTRCD ,  ")
                    .Append("   T7.SLS_PIC_STF_CD AS CUSTCHRGSTAFFCD ,  ")
                    .Append("   T9.CST_TYPE AS CUSTSEGMENT ,  ")
                    .Append("   T3.REG_NUM AS VCLREGNO ,  ")
                    .Append("   T2.MODEL_NAME AS SERIESNAME ,  ")
                    .Append("   T6.SCHE_BRN_CD AS BRANCH_PLAN ,  ")
                    .Append("   ' ' AS SERVICECD ,  ")
                    .Append("   ' ' AS SUBCTGORGNAME ,  ")
                    .Append("   ' ' AS SUBCTGORGNAME_EX ,  ")
                    .Append("   ' ' AS INSURANCEFLG ,  ")
                    .Append("   TO_CHAR(T8.SALES_TARGET_DATE,'YYYY/MM/DD HH24:MI:SS') AS CRACTLIMITDATE ,  ")
                    .Append("   ' ' AS CRACTCATEGORY ,  ")
                    .Append("   T6.RSLT_CONTACT_MTD AS REQCATEGORY ,  ")
                    .Append("   CASE WHEN T4.REQ_STATUS = '31' THEN '3'  ")
                    .Append("        WHEN T4.REQ_STATUS = '32' THEN '5'  ")
                    .Append("        ELSE  ")
                    .Append("          CASE WHEN T8.SALES_PROSPECT_CD = '30' THEN '1'  ")
                    .Append("               WHEN T8.SALES_PROSPECT_CD = '20' THEN '2'  ")
                    .Append("               WHEN T8.SALES_PROSPECT_CD = '10' THEN '4'  ")
                    .Append("               ELSE '4' END  ")
                    .Append("   END AS CRACTRESULT ,  ")
                    .Append("   T1.MODEL_CD AS SERIESCODE ,  ")
                    .Append("   ' ' AS PROMOTIONNAME ,  ")
                    .Append("   ' ' AS CONDITION ,  ")
                    .Append("   ' ' AS REQUESTNM ,  ")
                    .Append("   T6.ACT_STATUS AS CRACTSTATUS  ")
                    .Append("FROM  ")
                    .Append("  TB_M_VEHICLE T1 ,  ")
                    .Append("  TB_M_MODEL T2 ,  ")
                    .Append("  TB_M_VEHICLE_DLR T3 ,  ")
                    .Append("  TB_T_REQUEST T4 ,  ")
                    .Append("  TB_T_ACTIVITY T6 ,  ")
                    .Append("  TB_M_CUSTOMER_VCL T7 ,  ")
                    .Append("  TB_T_SALES T8 ,  ")
                    .Append("  TB_M_CUSTOMER_DLR T9  ")
                    .Append("WHERE  ")
                    .Append("      T1.MODEL_CD = T2.MODEL_CD  ")
                    .Append("  AND T1.VCL_ID = T3.VCL_ID  ")
                    .Append("  AND T3.VCL_ID = T7.VCL_ID  ")
                    .Append("  AND T3.DLR_CD = T7.DLR_CD  ")
                    .Append("  AND T7.VCL_ID = T4.VCL_ID  ")
                    .Append("  AND T7.CST_ID = T4.CST_ID  ")
                    .Append("  AND T4.REQ_ID = T6.REQ_ID  ")
                    .Append("  AND T8.REQ_ID = T4.REQ_ID  ")
                    .Append("  AND T3.DLR_CD = T9.DLR_CD  ")
                    .Append("  AND T4.CST_ID = T9.CST_ID ")
                    .Append("  AND T8.SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append(" UNION ALL  ")
                    .Append("   SELECT  ")
                    .Append("     0 AS CRPLAN_ID ,  ")
                    .Append("     ' ' AS BFAFDVS ,  ")
                    .Append("     0 AS CRDVSID ,  ")
                    .Append("     ' ' AS PLANDVS ,  ")
                    .Append("     T14.ATTPLAN_ID AS SUBCTGCODE ,  ")
                    .Append("     T14.ATTPLAN_ID AS PROMOTION_ID ,  ")
                    .Append("     ' ' AS REQUESTID ,  ")
                    .Append("     0 AS UNTRADEDCSTID ,  ")
                    .Append("     T16.ACT_ID AS VCLSEQNO ,  ")
                    .Append("     T16.SCHE_STF_CD AS ACCOUNT_PLAN ,  ")
                    .Append("     0 AS INSDID ,  ")
                    .Append("     T11.VCL_VIN AS VIN ,  ")
                    .Append("     T19.CST_TYPE AS MEMKIND ,  ")
                    .Append("     T17.SLS_PIC_BRN_CD AS CUSTCHRGSTRCD ,  ")
                    .Append("     T17.SLS_PIC_STF_CD AS CUSTCHRGSTAFFCD ,  ")
                    .Append("     T19.CST_TYPE AS CUSTSEGMENT ,  ")
                    .Append("     T13.REG_NUM AS VCLREGNO ,  ")
                    .Append("     T12.MODEL_NAME AS SERIESNAME ,  ")
                    .Append("     T16.SCHE_BRN_CD AS BRANCH_PLAN ,  ")
                    .Append("     TO_CHAR(T14.SVC_CD) AS SERVICECD ,  ")
                    .Append("     ' ' AS SUBCTGORGNAME ,  ")
                    .Append("     ' ' AS SUBCTGORGNAME_EX ,  ")
                    .Append("     ' ' AS INSURANCEFLG ,  ")
                    .Append("     TO_CHAR(T18.SALES_TARGET_DATE,'YYYY/MM/DD HH24:MI:SS') AS CRACTLIMITDA ,  ")
                    .Append("     ' ' AS CRACTCATEGORY ,  ")
                    .Append("     T16.RSLT_CONTACT_MTD AS REQCATEGORY ,  ")
                    .Append("     CASE WHEN T14.CONTINUE_ACT_STATUS = '31' THEN '3'  ")
                    .Append("          WHEN T14.CONTINUE_ACT_STATUS = '32' THEN '5'  ")
                    .Append("          ELSE  ")
                    .Append("            CASE WHEN T18.SALES_PROSPECT_CD = '30' THEN '1'  ")
                    .Append("                 WHEN T18.SALES_PROSPECT_CD = '20' THEN '2'  ")
                    .Append("                 WHEN T18.SALES_PROSPECT_CD = '10' THEN '4'  ")
                    .Append("                 ELSE '4' END  ")
                    .Append("     END AS CRACTRESULT ,  ")
                    .Append("     T11.MODEL_CD AS SERIESCODE ,  ")
                    .Append("     TO_CHAR(T20.ATTPLAN_NAME) AS PROMOTIONNAME ,  ")
                    .Append("     TO_CHAR(T20.ATTPLAN_TYPE) AS CONDITION ,  ")
                    .Append("     ' ' AS REQUESTNM ,  ")
                    .Append("     T16.ACT_STATUS AS CRACTSTATUS  ")
                    .Append(" FROM  ")
                    .Append("  TB_M_VEHICLE T11 ,  ")
                    .Append("  TB_M_MODEL T12 ,  ")
                    .Append("  TB_M_VEHICLE_DLR T13 ,  ")
                    .Append("  TB_T_ATTRACT T14 ,  ")
                    .Append("  TB_T_ACTIVITY T16 ,  ")
                    .Append("  TB_M_CUSTOMER_VCL T17 ,  ")
                    .Append("  TB_T_SALES T18 ,  ")
                    .Append("  TB_M_CUSTOMER_DLR T19 , ")
                    .Append("  TB_M_ATTPLAN T20 ")
                    .Append("WHERE  ")
                    .Append("      T11.MODEL_CD = T12.MODEL_CD  ")
                    .Append("  AND T11.VCL_ID = T13.VCL_ID  ")
                    .Append("  AND T13.VCL_ID = T17.VCL_ID  ")
                    .Append("  AND T13.DLR_CD = T17.DLR_CD  ")
                    .Append("  AND T17.VCL_ID = T14.VCL_ID  ")
                    .Append("  AND T17.CST_ID = T14.CST_ID  ")
                    .Append("  AND T14.ATT_ID = T16.ATT_ID  ")
                    .Append("  AND T18.ATT_ID = T14.ATT_ID  ")
                    .Append("  AND T13.DLR_CD = T19.DLR_CD  ")
                    .Append("  AND T14.CST_ID = T19.CST_ID ")
                    .Append("  AND T14.DLR_CD = T20.DLR_CD  ")
                    .Append("  AND T14.BRN_CD = T20.BRN_CD  ")
                    .Append("  AND T14.ATTPLAN_ID = T20.ATTPLAN_ID ")
                    .Append("  AND T14.ATTPLAN_VERSION = T20.ATTPLAN_VERSION ")
                    .Append("  AND T18.SALES_ID = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupBox_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 044.希望車種の台数を取得　(移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSelectedCarNum(ByVal fllwupboxseqno As Decimal, ByVal seqno As Decimal) As SC3080203DataSet.SC3080203SelectedCarNumDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SelectedCarNumDataTable)("SC3080203_144")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCarNum_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_144 */ ")
                    .Append("   PREF_AMOUNT AS QUANTITY ")
                    .Append(" FROM ")
                    .Append("   TB_T_PREFER_VCL ")
                    .Append(" WHERE ")
                    .Append("       SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append("   AND PREF_VCL_SEQ = TO_NUMBER(:SEQNO) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCarNum_End")
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

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 057.接触方法マスタ取得 (移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActContact() As SC3080203DataSet.SC3080203ActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActContactDataTable)("SC3080203_057")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContact_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_057 */ ")
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
                    .Append("    SORT_ORDER  ")
                End With

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContact_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()


            End Using
        End Function

        ''' <summary>
        ''' 058.接触方法マスタ取得(次回活動)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNextActContact() As SC3080203DataSet.SC3080203NextActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NextActContactDataTable)("SC3080203_058")
                Dim sql As New StringBuilder
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNextActContact_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_058 */  ")
                    .Append("    CONTACT_MTD AS CONTACTNO,  ")
                    .Append("    CONTACT_NAME AS CONTACT,  ")
                    .Append("    START_END_TYPE AS FROMTO,  ")
                    .Append("    NEXT_ACT_TYPE AS NEXTACTIVITY  ")
                    .Append("FROM  ")
                    .Append("    TB_M_CONTACT_MTD  ")
                    .Append("WHERE  ")
                    .Append("    NEXT_ACT_TYPE IN (1,2) AND  ")
                    .Append("    INUSE_FLG = '1'  ")
                    .Append("ORDER BY  ")
                    .Append("    SORT_ORDER  ")
                End With
                query.CommandText = sql.ToString()
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNextActContact_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 059.接触方法マスタ取得(予約フォロー)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFollowContact() As SC3080203DataSet.SC3080203FollowContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FollowContactDataTable)("SC3080203_059")
                Dim sql As New StringBuilder
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowContact_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_059 */  ")
                    .Append("    CONTACT_MTD AS CONTACTNO,  ")
                    .Append("    CONTACT_NAME AS CONTACT,  ")
                    .Append("    START_END_TYPE AS FROMTO  ")
                    .Append("FROM  ")
                    .Append("    TB_M_CONTACT_MTD  ")
                    .Append("WHERE  ")
                    .Append("    FLLW_ACT_TYPE = '1' AND  ")
                    .Append("    INUSE_FLG = '1'  ")
                    .Append("ORDER BY  ")
                    .Append("    SORT_ORDER  ")
                End With
                query.CommandText = sql.ToString()
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowContact_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 060.Follow-upBox取得(活動結果登録用)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFollowCractstatus(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080201FollowStatusDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201FollowStatusDataTable)("SC3080203_060")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowCractstatus_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_160 */ ")
                    .Append("   T7.CRACTSTATUS , ")
                    .Append("   T7.CRACTCATEGORY , ")
                    .Append("   T7.PROMOTION_ID , ")
                    .Append("   T7.REQCATEGORY , ")
                    .Append("   T7.CRACTRESULT ")
                    .Append(" FROM ( ")
                    .Append(" SELECT ")
                    .Append("   T3.ACT_ID , ")
                    .Append("   T3.ACT_STATUS AS CRACTSTATUS , ")
                    .Append("   ' ' AS CRACTCATEGORY , ")
                    .Append("   0 AS PROMOTION_ID , ")
                    .Append("   CASE WHEN T3.RSLT_CONTACT_MTD = '11' THEN '1' ")
                    .Append("        WHEN T3.RSLT_CONTACT_MTD = '12' THEN '2' ")
                    .Append("        WHEN T3.RSLT_CONTACT_MTD = '13' THEN '3' ")
                    .Append("        ELSE '4' ")
                    .Append("        END AS REQCATEGORY , ")
                    .Append("   CASE WHEN T2.REQ_STATUS = '31' THEN '3' ")
                    .Append("        WHEN T2.REQ_STATUS = '32' THEN '5' ")
                    .Append("        ELSE ")
                    .Append("          CASE WHEN T1.SALES_PROSPECT_CD = '30' THEN '1' ")
                    .Append("               WHEN T1.SALES_PROSPECT_CD = '20' THEN '2' ")
                    .Append("               WHEN T1.SALES_PROSPECT_CD = '10' THEN '4' ")
                    .Append("               ELSE '4' END ")
                    .Append("        END AS CRACTRESULT ")
                    .Append(" FROM ")
                    .Append("   TB_T_SALES T1 , ")
                    .Append("   TB_T_REQUEST T2 , ")
                    .Append("   TB_T_ACTIVITY T3 ")
                    .Append(" WHERE ")
                    .Append("       T1.REQ_ID = T2.REQ_ID ")
                    .Append("   AND T1.REQ_ID = T3.REQ_ID ")
                    .Append("   AND T3.ACT_STATUS <> ' ' ")
                    .Append("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append(" UNION ALL ")
                    .Append("   SELECT ")
                    .Append("     T5.ACT_ID , ")
                    .Append("     T5.ACT_STATUS AS CRACTSTATUS , ")
                    .Append("     ' ' AS CRACTCATEGORY , ")
                    .Append("     T6.ATTPLAN_ID AS PROMOTION_ID , ")
                    .Append("     CASE WHEN T5.RSLT_CONTACT_MTD = '11' THEN '1' ")
                    .Append("          WHEN T5.RSLT_CONTACT_MTD = '12' THEN '2' ")
                    .Append("          WHEN T5.RSLT_CONTACT_MTD = '13' THEN '3' ")
                    .Append("          ELSE '4' ")
                    .Append("          END AS REQCATEGORY , ")
                    .Append("     CASE WHEN T6.CONTINUE_ACT_STATUS = '31' THEN '3' ")
                    .Append("          WHEN T6.CONTINUE_ACT_STATUS = '32' THEN '5' ")
                    .Append("          ELSE ")
                    .Append("          CASE WHEN T4.SALES_PROSPECT_CD = '30' THEN '1' ")
                    .Append("               WHEN T4.SALES_PROSPECT_CD = '20' THEN '2' ")
                    .Append("               WHEN T4.SALES_PROSPECT_CD = '10' THEN '4' ")
                    .Append("               ELSE '4' END ")
                    .Append("     END AS CRACTRESULT ")
                    .Append("   FROM ")
                    .Append("     TB_T_SALES T4 , ")
                    .Append("     TB_T_ACTIVITY T5 , ")
                    .Append("     TB_T_ATTRACT T6 ")
                    .Append("   WHERE ")
                    .Append("         T4.ATT_ID = T5.ATT_ID ")
                    .Append("     AND T4.ATT_ID = T6.ATT_ID ")
                    .Append("     AND T5.ACT_STATUS <> ' ' ")
                    .Append("     AND T4.SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append("   ) T7 ")
                    .Append("ORDER BY ")
                    .Append("     T7.ACT_ID DESC ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowCractstatus_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 061.文言取得　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContentWord(ByVal seqno As Decimal) As SC3080203DataSet.SC3080203ContentWordDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ContentWordDataTable)("SC3080203_061")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_061 */ ")
                    .Append("    DECODE(TRIM(ACTION_LOCAL), '', ACTION, ACTION_LOCAL) AS ACTION ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXCONTENT ")
                    .Append("WHERE ")
                    .Append("    SEQNO = :SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 062.日付フォーマット取得 (移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDateFormat() As SC3080203DataSet.SC3080203DateFormatDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203DateFormatDataTable)("SC3080203_062")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_062 */ ")
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
        ''' 063.自社客名前・敬称取得 (移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetOrgNameTitle(ByVal originalid As String) As SC3080203DataSet.SC3080203NameTitleDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NameTitleDataTable)("SC3080203_063")
                Dim sql As New StringBuilder
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgNameTitle_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_163 */ ")
                    .Append("   CST_NAME AS NAME , ")
                    .Append("   NAMETITLE_NAME AS NAMETITLE ")
                    .Append(" FROM ")
                    .Append("   TB_M_CUSTOMER ")
                    .Append(" WHERE ")
                    .Append("   CST_ID = :ORIGINALID ")
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
        ''' 064.未取引客名前・敬称取得 (移行済み)
        ''' </summary>
        ''' <param name="cstid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewNameTitle(ByVal cstid As String) As SC3080203DataSet.SC3080203NameTitleDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NameTitleDataTable)("SC3080203_064")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_064 */ ")
                    .Append("    NAME, ")
                    .Append("    NAMETITLE ")
                    .Append("FROM ")
                    .Append("    tbl_NEWCUSTOMER ")
                    .Append("WHERE ")
                    .Append("    CSTID = :CSTID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 065.CalDAV用接触方法名取得 (移行済み)
        ''' </summary>
        ''' <param name="contactno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContactNM(ByVal contactno As Long) As SC3080203DataSet.SC3080203GetContactNmDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203GetContactNmDataTable)("SC3080203_065")
                Dim sql As New StringBuilder
                '' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactNM_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_065 */  ")
                    .Append("    CONTACT_NAME AS CONTACT  ")
                    .Append("FROM  ")
                    .Append("    TB_M_CONTACT_MTD  ")
                    .Append("WHERE  ")
                    .Append("    CONTACT_MTD = TO_CHAR(:CONTACTNO)  ")

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
        ''' 066.CalDAV用ToDo背景色取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="createdatadiv"></param>
        ''' <param name="scheduledvs"></param>
        ''' <param name="nextactiondvs"></param>
        ''' <param name="contactno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetToDoColor(ByVal dlrcd As String, ByVal createdatadiv As String, ByVal scheduledvs As String,
                                     ByVal nextactiondvs As String, ByVal contactno As Long) As SC3080203DataSet.SC3080203TodoColorDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203TodoColorDataTable)("SC3080203_066")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_066 */ ")
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


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ''' <summary>
        ''' 069.アイコンのパス取得　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContentIconPath(ByVal seqno As Integer) As SC3080203DataSet.SC3080203ContentIconPathDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ContentIconPathDataTable)("SC3080203_069")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_069 */ ")
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


        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 071.入庫履歴よりサービススタッフ情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceStaff(ByVal originalid As String, ByVal vin As String) As SC3080203DataSet.SC3080203ServiceStaffDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ServiceStaffDataTable)("SC3080203_171")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetServiceStaff_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_171 */ ")
                    .Append("   T1.PIC_STF_CD AS SERVICESTAFFCD , ")
                    .Append("   T2.USERNAME AS SERVICESTAFFNM ")
                    .Append(" FROM ")
                    .Append("   TB_T_VEHICLE_SVCIN_HIS T1 , ")
                    .Append("   TBL_USERS T2 , ")
                    .Append("   TB_M_VEHICLE T3 ")
                    .Append(" WHERE ")
                    .Append("       T1.PIC_STF_CD = RTRIM(T2.ACCOUNT) ")
                    .Append("   AND T1.VCL_ID = T3.VCL_ID ")
                    .Append("   AND T1.CST_ID = :ORIGINALID ")
                    .Append("   AND T3.VCL_VIN = :VIN ")
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

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START DELL
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 073.活動実績登録用の希望車種情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisCarSeq(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203SeqDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeqDataTable)("SC3080203_173")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_173 */ ")
                    .Append("   T1.PREF_VCL_SEQ AS SEQNO ")
                    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                    .Append("   , T1.ROW_LOCK_VERSION AS LOCKVERSION ")
                    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                    .Append(" FROM ")
                    .Append("   TB_T_PREFER_VCL T1 ")
                    .Append(" WHERE ")
                    .Append("   T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                    .Append(" ORDER BY ")
                    .Append("   SEQNO ")
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

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 074.活動実績登録用の希望車種情報を取得 (移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisSelCarSeq(ByVal fllwupboxseqno As Decimal, ByVal seqno As Decimal) As SC3080203DataSet.SC3080203SeqDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeqDataTable)("SC3080203_174")
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
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisSelCarSeq_End")
                'ログ出力 End *****************************************************************************
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 075.活動実績登録用のフォローアップボックス情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="dlrcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisFllw(ByVal fllwupboxseqno As Decimal, ByVal dlrcd As String) As SC3080203DataSet.SC3080203ActHisFllwDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisFllwDataTable)("SC3080203_175")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisFllw_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append(" SELECT ")
                    .Append("   /* SC3080203_175 */ ")
                    .Append("   0 AS CRPLAN_ID , ")
                    .Append("   ' ' AS BFAFDVS , ")
                    .Append("   0 AS CRDVSID , ")
                    .Append("   T2.CST_ID AS INSDID , ")
                    .Append("   TO_CHAR(T3.MODEL_CD) AS SERIESCODE , ")
                    .Append("   CASE WHEN T3.MODEL_CD = ' ' THEN TO_CHAR(T3.NEWCST_MODEL_NAME) ")
                    .Append("        ELSE TO_CHAR(NVL(T4.MODEL_NAME,' ')) ")
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
                    .Append("   /* SC3080203_175 */ ")
                    .Append("   0 AS CRPLAN_ID , ")
                    .Append("   ' ' AS BFAFDVS , ")
                    .Append("   0 AS CRDVSID , ")
                    .Append("   T2.CST_ID AS INSDID , ")
                    .Append("   TO_CHAR(T3.MODEL_CD) AS SERIESCODE , ")
                    .Append("   CASE WHEN T3.MODEL_CD = ' ' THEN TO_CHAR(T3.NEWCST_MODEL_NAME) ")
                    .Append("        ELSE TO_CHAR(NVL(T4.MODEL_NAME,' ')) ")
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

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 076.Follow-up BOX活動内容取得(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisContent(ByVal seqno As Decimal) As SC3080203DataSet.SC3080203ActHisContentDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisContentDataTable)("SC3080203_076")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_076 */ ")
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
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
                ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 077.Follow-up Box選択車種取得(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisCarSeq(ByVal fllwupboxseqno As Decimal, ByVal seqno As Decimal, ByVal dlrcd As String) As SC3080203DataSet.SC3080203ActHisSelCarDataTable
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActHisCarSeq_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append(" SELECT ")
                .Append("   /* SC3080203_177 */ ")
                .Append("   T9.SERIESCD AS SERIESNM , ")
                .Append("   T9.VCLMODEL_NAME , ")
                .Append("   T9.COLORCD AS DISP_BDY_COLOR , ")
                .Append("   T9.QUANTITY ")
                .Append(" FROM ")
                .Append("   ( ")
                .Append("   SELECT ")
                .Append("     T8.SERIESCD , ")
                .Append("     T8.MODELCD AS VCLMODEL_NAME , ")
                .Append("     T8.SERIESNM , ")
                .Append("     T8.COLORCD , ")
                .Append("     T8.SEQNO , ")
                .Append("     T8.QUANTITY ")
                .Append("   FROM ")
                .Append("     ( ")
                .Append("     SELECT ")
                .Append("       T1.MODEL_CD AS SERIESCD , ")
                .Append("       T1.GRADE_CD AS MODELCD , ")
                .Append("       T1.BODYCLR_CD AS COLORCD , ")
                .Append("       T1.PREF_VCL_SEQ AS SEQNO , ")
                .Append("       T7.COMSERIESCD , ")
                .Append("       T7.SERIESNM , ")
                .Append("       T1.PREF_AMOUNT AS QUANTITY ")
                .Append("     FROM ")
                .Append("       TB_T_PREFER_VCL T1 , ")
                .Append("       ( ")
                .Append("       SELECT ")
                .Append("         T2.DLR_CD AS DLRCD , ")
                .Append("         T3.MODEL_CD AS SERIESCD , ")
                .Append("         T3.MODEL_NAME AS SERIESNM , ")
                .Append("         T4.MAKER_TYPE AS TOYOTABRAND , ")
                .Append("         T3.MODEL_PICTURE AS IMAGEPATH , ")
                .Append("         T3.COMMON_MODEL_CD AS COMSERIESCD , ")
                .Append("         T3.INUSE_FLG , ")
                .Append("         NULL AS DELDATE , ")
                .Append("         T3.ROW_CREATE_DATETIME AS CREATEDATE , ")
                .Append("         T3.ROW_UPDATE_DATETIME AS UPDATEDATE , ")
                .Append("         T3.MAKER_CD AS MAKERCD ")
                .Append("       FROM ")
                .Append("         TB_M_DEALER T2 , ")
                .Append("         TB_M_MODEL T3 , ")
                .Append("         TB_M_MAKER T4 , ")
                .Append("         TB_M_MODEL_DLR T5 ")
                .Append("       WHERE ")
                .Append("             T2.DLR_CD = T5.DLR_CD ")
                .Append("         AND T3.MODEL_CD = T5.MODEL_CD ")
                .Append("         AND T3.MAKER_CD = T4.MAKER_CD ")
                .Append("         AND T2.DLR_CD = :DLRCD ")
                .Append("         AND T2.INUSE_FLG = '1' ")
                .Append("         OR (T5.DLR_CD = 'XXXXX' ")
                .Append("         AND T2.DLR_CD = :DLRCD ")
                .Append("         AND T2.INUSE_FLG = '1' ")
                .Append("         AND NOT EXISTS ")
                .Append("           ( ")
                .Append("           SELECT ")
                .Append(" 1 ")
                .Append("           FROM ")
                .Append("             TB_M_MODEL_DLR T6 ")
                .Append("           WHERE ")
                .Append("                 T6.DLR_CD = T2.DLR_CD ")
                .Append("             AND T6.MODEL_CD = T3.MODEL_CD ")
                .Append("           )) ")
                .Append("       ) T7 ")
                .Append("   WHERE ")
                .Append("         T1.MODEL_CD = T7.SERIESCD ")
                .Append("     AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("     AND T1.PREF_VCL_SEQ = :SEQNO ")
                .Append("     ) T8 ")
                .Append("   ) T9 ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisSelCarDataTable)("SC3080203_177")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, seqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
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



        ''' <summary>
        ''' 080.競合車種取得(ALL)　(移行済み)
        ''' </summary>
        ''' <param name="competitionmakerno"></param>
        ''' <param name="competitorcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetition(ByVal competitionmakerno As String, ByVal competitorcd As String) As SC3080203DataSet.SC3080203CompetitionDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_080 */ ")
                .Append("    (SELECT COMPETITIONMAKER FROM TBL_COMPETITION_MAKERMASTER WHERE COMPETITIONMAKERNO = :COMPETITIONMAKERNO) AS COMPETITIONMAKER, ")
                .Append("    (SELECT COMPETITORNM FROM TBL_COMPETITORMASTER WHERE COMPETITORCD = :COMPETITORCD) AS COMPETITORNM ")
                .Append("FROM DUAL ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionDataTable)("SC3080203_080")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("COMPETITIONMAKERNO", OracleDbType.Char, competitionmakerno)
                query.AddParameterWithTypeValue("COMPETITORCD", OracleDbType.Char, competitorcd)
                Return query.GetData()
            End Using
        End Function

        ' ''' <summary>
        ' ''' 081. Follow-up Box商談 を削除
        ' ''' </summary>
        ' ''' <param name="dlrCD"></param>
        ' ''' <param name="strCD"></param>
        ' ''' <param name="fllwupboxSeqNo"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function DeleteFllwupboxSales(ByVal dlrCD As String, ByVal strCD As String, ByVal fllwupboxseqno As Decimal) As Integer

        '    Using query As New DBUpdateQuery("SC3080203_081")
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("DELETE /* SC3080203_081 */ ")
        '            .Append("FROM ")
        '            .Append("    TBL_FLLWUPBOX_SALES ")
        '            .Append("WHERE ")
        '            .Append("    DLRCD = :DLRCD AND ")
        '            .Append("    STRCD = :STRCD AND ")
        '            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
        '        End With
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
        '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxSeqNo)

        '        Return query.Execute()
        '    End Using
        'End Function

        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary> 
        ''' 081. Follow-up Box商談 を更新　(商談中⇒商談終了) (移行済み) 
        ''' </summary> 
        ''' <param name="fllwupboxSeqno"></param> 
        ''' <param name="actualaccount"></param> 
        ''' <param name="salesstarttime"></param> 
        ''' <param name="salesendtime"></param> 
        ''' <param name="account"></param> 
        ''' <param name="updateid"></param> 
        ''' <returns></returns> 
        ''' <remarks></remarks> 
        Public Shared Function UpdateFllwupboxSales(ByVal fllwupboxSeqno As Decimal, _
                            ByVal actualaccount As String, _
                            ByVal salesstarttime As Date, _
                            ByVal salesendtime As Date, _
                            ByVal account As String, _
                            ByVal updateid As String) As Integer

            Using query As New DBUpdateQuery("SC3080203_081")
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* SC3080203_081 */ ")
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
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxSeqno)

                query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Varchar2, actualaccount)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, salesstarttime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, salesendtime)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)
                '2013/06/30 TCS 松月 2013/10対応版　既存流用 END 
                Return query.Execute()
            End Using
        End Function

        '2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 082. Follow-up Box商談 を取得
        ''' </summary>
        ''' <param name="fllwupboxSeqNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupboxSales(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203FllwupboxSalesDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_082 */ ")
                '2012/09/13 TCS 山口 業活動開始時の活動開始時間の表示不良 START
                .Append("  NVL(STARTTIME,EIGYOSTARTTIME) AS STARTTIME, ")
                '2012/09/13 TCS 山口 業活動開始時の活動開始時間の表示不良 END
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
                .Append("  ENDTIME, ")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
                .Append("  WALKINNUM ")
                .Append("FROM TBL_FLLWUPBOX_SALES ")
                .Append("WHERE ")
                .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
                .Append("    REGISTFLG = '0'")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupboxSalesDataTable)("SC3080203_082")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                '2013/06/30 TCS 松月 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 083. 最大の活動終了時間を取得 (移行済み)
        ''' </summary>
        ''' <param name="fllwupboxSeqNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetLatestActTimeEnd(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203LatestActTimeDataTable
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLatestActTimeEnd_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT /* SC3080203_083 */")
                .Append("  MAX(LATEST_TIME_END) LATEST_TIME_END ")
                .Append("FROM ")
                .Append("(SELECT ")
                .Append("  T1.RSLT_DATETIME AS LATEST_TIME_END ")
                .Append("    FROM ")
                .Append("        TB_T_ACTIVITY T1, ")
                .Append("        TB_T_SALES T2 ")
                .Append("    WHERE ")
                .Append("         T1.REQ_ID = T2.REQ_ID ")
                .Append("        AND T1.ATT_ID = T2.ATT_ID ")
                .Append("        AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
                .Append("       AND T1.RSLT_FLG = '1' ")
                .Append("       ) ")

            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203LatestActTimeDataTable)("SC3080203_083")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLatestActTimeEnd_End")
                'ログ出力 End *****************************************************************************
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 084.接触方法マスタ取得
        ''' </summary>
        ''' <param name="contactno">接触方法No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActContactTitle(ByVal contactno As Long) As SC3080203DataSet.SC3080203NextActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NextActContactDataTable)("SC3080203_084")
                Dim sql As New StringBuilder
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContactTitle_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_084 */  ")
                    .Append("    CONTACT_MTD AS CONTACTNO,  ")
                    .Append("    CONTACT_NAME AS CONTACT,  ")
                    .Append("    START_END_TYPE AS FROMTO,  ")
                    .Append("    NEXT_ACT_TYPE AS NEXTACTIVITY  ")
                    .Append("FROM  ")
                    .Append("    TB_M_CONTACT_MTD  ")
                    .Append("WHERE  ")
                    .Append("    INUSE_FLG = '1'  ")
                    If contactno <> 0 Then
                        .Append("    AND  ")
                        .Append("        CONTACT_MTD = :CONTACTNO  ")
                    End If
                    .Append("ORDER BY  ")
                    .Append("    SORT_ORDER  ")
                End With
                query.CommandText = sql.ToString()
                If contactno <> 0 Then
                    query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                End If
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActContactTitle_End")
                'ログ出力 End *****************************************************************************
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                Return query.GetData()
            End Using
        End Function

        '2013/06/30 TCS 王 2013/10対応版　既存流用 START
        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        ''' <summary>
        ''' 085.見積車種取得
        ''' </summary>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetEstimateCar(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203PreferredCarDataTable
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateCar_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT /* SC3080203_185 */")
                .AppendLine("       T4.DLRCD")
                .AppendLine("     , T4.STRCD")
                .AppendLine("     , T4.FLLWUPBOX_SEQNO")
                .AppendLine("     , T4.ESTIMATEID")
                .AppendLine("     , T5.PREF_VCL_SEQ AS SEQNO")
                .AppendLine("     , T4.SERIESCD AS CAR_NAME_CD_AI21")
                .AppendLine("     , T4.SERIESCD")
                .AppendLine("     , T4.SERIESNM")
                .AppendLine("     , T4.MODELCD")
                .AppendLine("     , T4.EXTCOLORCD AS COLORCD")
                .AppendLine("     , T4.BASEPRICE")
                .AppendLine("     , NVL2(T5.PREF_VCL_SEQ,'1','0') AS IS_EXISTS_SELECTED_SERIES")
                .AppendLine("     , '1' AS IS_EXISTS_ESTIMATE")
                .AppendLine("     , NVL(T5.PREF_VCL_SEQ,'0') || 'E' || T4.ESTIMATEID AS KEYVALUE")
                .AppendLine("  FROM (SELECT T1.ESTIMATEID")
                .AppendLine("             , T1.DLRCD")
                .AppendLine("             , T1.STRCD")
                .AppendLine("             , T1.FLLWUPBOX_SEQNO")
                .AppendLine("             , T2.SERIESCD")
                .AppendLine("             , T2.SERIESNM")
                .AppendLine("             , T2.MODELCD")
                .AppendLine("             , T2.EXTCOLORCD")
                .AppendLine("             , T2.BASEPRICE")
                .AppendLine("             , NVL(T6.COLOR_CD,' ') AS COLOR_CD")
                .AppendLine("          FROM TBL_ESTIMATEINFO T1")
                .AppendLine("             , TBL_EST_VCLINFO T2")
                .AppendLine("             , TBL_MSTEXTERIOR T6")
                .AppendLine("         WHERE T2.ESTIMATEID = T1.ESTIMATEID")
                .AppendLine("           AND T2.MODELCD = T6.VCLMODEL_CODE(+)")
                .AppendLine("           AND T2.EXTCOLORCD = T6.BODYCLR_CD(+)")
                .AppendLine("           AND T1.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                .AppendLine("           AND T1.DELFLG = '0'")
                .AppendLine("       ) T4")
                .AppendLine("     , TB_T_PREFER_VCL T5")
                .AppendLine(" WHERE T5.SALES_ID(+) = T4.FLLWUPBOX_SEQNO")
                .AppendLine("   AND T5.MODEL_CD(+) = T4.SERIESCD")
                .AppendLine("   AND T5.GRADE_CD(+) = T4.MODELCD")
                .AppendLine("   AND T5.BODYCLR_CD(+) = T4.COLOR_CD")
            End With

            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203PreferredCarDataTable)("SC3080203_185")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateCar_End")
                'ログ出力 End *****************************************************************************
                Return query.GetData()
            End Using
        End Function
        '2013/06/30 TCS 王 2013/10対応版　既存流用 END

        '2013/06/30 TCS 庄 2013/10対応版 START
        'Public Shared Function GetSelectedCar(ByVal dlrcd As String, ByVal strcd As String, ByVal cntcd As String, ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203PreferredCarDataTable
        ''' <summary>
        ''' 086.選択車種取得
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSelectedCar(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.SC3080203PreferredCarDataTable

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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCar_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine(" SELECT /* SC3080203_186 */")
                .AppendLine("       :DLRCD AS DLRCD")
                .AppendLine("     , :STRCD AS STRCD")
                .AppendLine("     , T1.SALES_ID AS FLLWUPBOX_SEQNO")
                .AppendLine("     , NULL AS ESTIMATEID")
                .AppendLine("     , T1.PREF_VCL_SEQ AS SEQNO")
                .AppendLine("     , T1.MODEL_CD AS CAR_NAME_CD_AI21")
                .AppendLine("     , T1.MODEL_CD AS SERIESCD")
                .AppendLine("     , T2.MODEL_NAME AS SERIESNM")
                .AppendLine("     , T1.GRADE_CD AS MODELCD")
                .AppendLine("     , T1.BODYCLR_CD AS COLORCD")
                .AppendLine("     , NULL AS BASEPRICE")
                .AppendLine("     , '1' AS IS_EXISTS_SELECTED_SERIES")
                .AppendLine("     , '0' AS IS_EXISTS_ESTIMATE")
                .AppendLine("     , T1.PREF_VCL_SEQ AS KEYVALUE")
                .AppendLine("  FROM TB_T_PREFER_VCL T1")
                .AppendLine("     , TB_M_MODEL T2")
                .AppendLine(" WHERE T1.MODEL_CD = T2.MODEL_CD")
                .AppendLine("   AND T1.SALES_ID = :FLLWUPBOX_SEQNO")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 START
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 削除 END
                .AppendLine("   AND NOT EXISTS (")
                .AppendLine("        SELECT 1")
                .AppendLine("          FROM TBL_ESTIMATEINFO T6")
                .AppendLine("             , TBL_EST_VCLINFO T7")
                .AppendLine("         WHERE T7.ESTIMATEID = T6.ESTIMATEID")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .AppendLine("           AND T7.SERIESCD = T1.MODEL_CD ")
                .AppendLine("           AND T7.MODELCD = T1.GRADE_CD ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 START
                '.AppendLine("           AND SUBSTR(T7.EXTCOLORCD,1,3) = T1.BODYCLR_CD")
                If (extColor3Flg = "1") Then
                    .AppendLine("           AND SUBSTR(T7.EXTCOLORCD,1,3) = T1.BODYCLR_CD")
                Else
                    .AppendLine("           AND T7.EXTCOLORCD = T1.BODYCLR_CD")
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                .AppendLine("           AND T6.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                .AppendLine("           AND T6.DELFLG = '0'")
                .AppendLine("  )")
            End With

            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203PreferredCarDataTable)("SC3080203_086")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSelectedCar_End")
                'ログ出力 End *****************************************************************************
                '2013/06/30 TCS 庄 2013/10対応版 END
                Return query.GetData()
            End Using
        End Function

        '2013/06/30 TCS 庄 2013/10対応版 START
        ''' <summary>
        ''' 087.モデル名取得
        ''' </summary>
        ''' <param name="preferredCarRow"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetModelName(ByVal preferredCarRow As SC3080203DataSet.SC3080203PreferredCarRow) As SC3080203DataSet.SC3080203ModelNameDataTable
            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* SC3080203_087 */")
                .AppendLine("       GRADE_NAME AS VCLMODEL_NAME")
                .AppendLine("  FROM TB_M_GRADE")
                .AppendLine(" WHERE MODEL_CD = :CAR_NAME_CD_AI21")
                .AppendLine("   AND GRADE_CD = :VCLMODEL_CODE")
            End With


            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ModelNameDataTable)("SC3080203_087")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CAR_NAME_CD_AI21", OracleDbType.Char, preferredCarRow.CAR_NAME_CD_AI21)
                query.AddParameterWithTypeValue("VCLMODEL_CODE", OracleDbType.Char, preferredCarRow.MODELCD)

                Return query.GetData()
            End Using
        End Function
        '2013/06/30 TCS 庄 2013/10対応版 END

        '2013/06/30 TCS 庄 2013/10対応版 START
        ''' <summary>
        ''' 088.外装色取得
        ''' </summary>
        ''' <param name="preferredCarRow"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetExteriorColor(ByVal preferredCarRow As SC3080203DataSet.SC3080203PreferredCarRow) As SC3080203DataSet.SC3080203ExteriorColorDataTable
            Dim sql As New StringBuilder
            With sql
                .AppendLine("SELECT /* SC3080203_088 */")
                .AppendLine("       BODYCLR_NAME AS DISP_BDY_COLOR")
                .AppendLine("  FROM TB_M_BODYCOLOR")
                .AppendLine(" WHERE MODEL_CD = :CAR_NAME_CD_AI21")
                '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）START
                .AppendLine("   AND (GRADE_CD = :VCLMODEL_CODE OR GRADE_CD = 'X') ")
                '2014/02/02 TCS 松月 希望車表示不具合対応（号口切替BTS-39）END
                .AppendLine("   AND BODYCLR_CD = :BODYCLR_CD")
            End With


            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ExteriorColorDataTable)("SC3080203_088")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CAR_NAME_CD_AI21", OracleDbType.Char, preferredCarRow.CAR_NAME_CD_AI21)
                query.AddParameterWithTypeValue("VCLMODEL_CODE", OracleDbType.Char, preferredCarRow.MODELCD)
                query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Char, preferredCarRow.COLORCD)

                Return query.GetData()
            End Using
        End Function
        '2013/06/30 TCS 庄 2013/10対応版 END

        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        ' ''' <summary>
        ' ''' 089.選択車種登録
        ' ''' </summary>
        ' ''' <param name="preferredCarRow"></param>
        ' ''' <param name="seqno"></param>
        ' ''' <param name="updateAccount"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function InsertSelectedSeries(ByVal preferredCarRow As SC3080203DataSet.SC3080203PreferredCarRow,
        '                                            ByVal seqno As Long,
        '                                            ByVal updateAccount As String) As Integer

        '    ' SQL組み立て
        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine("INSERT /* SC3080203_089 */")
        '        .AppendLine("  INTO TBL_FLLWUPBOX_SELECTED_SERIES")
        '        .AppendLine("     (")
        '        .AppendLine("       DLRCD ")
        '        .AppendLine("     , STRCD ")
        '        .AppendLine("     , FLLWUPBOX_SEQNO ")
        '        .AppendLine("     , SEQNO ")
        '        .AppendLine("     , SERIESCD ")
        '        .AppendLine("     , MODELCD ")
        '        .AppendLine("     , COLORCD ")
        '        .AppendLine("     , CREATEDATE ")
        '        .AppendLine("     , UPDATEDATE ")
        '        .AppendLine("     , UPDATEACCOUNT ")
        '        .AppendLine("     )")
        '        .AppendLine("VALUES")
        '        .AppendLine("     (")
        '        .AppendLine("       :DLRCD ")
        '        .AppendLine("     , :STRCD ")
        '        .AppendLine("     , :FLLWUPBOX_SEQNO ")
        '        .AppendLine("     , :SEQNO ")
        '        .AppendLine("     , :SERIESCD  ")
        '        .AppendLine("     , :MODELCD ")
        '        .AppendLine("     , :COLORCD ")
        '        .AppendLine("     , SYSDATE ")
        '        .AppendLine("     , SYSDATE ")
        '        .AppendLine("     , :UPDATEACCOUNT ")
        '        .AppendLine(")")

        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBUpdateQuery("SC3080203_089")

        '        query.CommandText = sql.ToString()

        '        ' SQLパラメータ設定
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, preferredCarRow.DLRCD)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, preferredCarRow.STRCD)
        '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Long, preferredCarRow.FLLWUPBOX_SEQNO)
        '        query.AddParameterWithTypeValue("SEQNO", OracleDbType.Long, seqno)
        '        query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, preferredCarRow.SERIESCD)
        '        query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, preferredCarRow.MODELCD)
        '        query.AddParameterWithTypeValue("COLORCD", OracleDbType.Varchar2, preferredCarRow.COLORCD)
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)

        '        ' SQL実行（結果を返却）
        '        Return query.Execute()
        '    End Using

        'End Function
        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

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
                .AppendLine("UPDATE /* SC3080203_241 */")
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
                '.AppendLine("        AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
                If (extColor3Flg = "1") Then
                    .AppendLine("        AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
                Else
                    .AppendLine("        AND T2.BODYCLR_CD = T3.EXTCOLORCD ")
                End If
                ' 2014/03/07 TCS 各務 再構築不具合対応マージ版 END
                .AppendLine("        AND T1.ESTIMATEID = T3.ESTIMATEID ")
                .AppendLine("        AND T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .AppendLine(" ) ")
            End With

            Using query As New DBUpdateQuery("SC3080203_241")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
                Return query.Execute()
            End Using

        End Function
        '2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END


        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        ' ''' <summary>
        ' ''' 091.選択車種のシーケンス取得
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function GetSelectedCarSequence() As Long

        '    ' SQL組み立て
        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine("SELECT /* SC3080203_091 */")
        '        .AppendLine("       SEQ_SELECTEDSERIESNO.NEXTVAL AS SEQNO")
        '        .AppendLine("  FROM DUAL")
        '    End With

        '    ' DbUpdateQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of DataTable)("SC3080203_091")

        '        query.CommandText = sql.ToString()

        '        ' SQL実行（結果を返却）
        '        Return Long.Parse(query.GetData()(0)(0).ToString)
        '    End Using

        'End Function
        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
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
        ''' <param name="firstSalesActId">初回商談活動ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateSales(ByVal fllwupboxseqno As Decimal, ByVal prospectcd As String, ByVal completeflg As String, ByVal giveupvclseq As Long,
                                           ByVal giveupresion As String, ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long,
                                           ByRef firstSalesActId As Decimal) As Integer
            ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
            Using query As New DBUpdateQuery("SC3080203_213")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateSales_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203_213 */")
                    .AppendLine("    TB_T_SALES")
                    .AppendLine(" SET")
                    .AppendLine("    SALES_PROSPECT_CD = :PROSPECTCD ,")
                    .AppendLine("    SALES_COMPLETE_FLG = :COMPLETEFLG ,")
                    .AppendLine("    GIVEUP_COMP_VCL_SEQ = :GIVEUPVCLSEQ ,")
                    .AppendLine("    GIVEUP_REASON = :GIVEUPREASON ,")
                    ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
                    If firstSalesActId <> 0 Then
                        .AppendLine("    FIRST_SALES_ACT_ID = :FIRST_SALES_ACT_ID ,")
                    End If
                    ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
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
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
                If firstSalesActId <> 0 Then
                    query.AddParameterWithTypeValue("FIRST_SALES_ACT_ID", OracleDbType.Decimal, firstSalesActId)
                End If
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
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
            Using query As New DBUpdateQuery("SC3080203_217")
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateRequest_Start")
                'ログ出力 End *****************************************************************************
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203_217 */")
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
            Using query As New DBUpdateQuery("SC3080203_219")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateActivity_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203_219 */")
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
                    '2014/05/15 TCS 武田 受注後フォロー機能開発 START
                    .AppendLine("    RSLT_INPUT_DATETIME = SYSDATE ,")
                    '2014/05/15 TCS 武田 受注後フォロー機能開発 END
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                    .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ,")
                    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1,")
                    .AppendLine("    RSLT_SALES_PROSPECT_CD = :CREATE_CRACTRESULT")
                    ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
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
        ''' 4.93.商談ロック
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <param name="rowlockversion">行ロックバージョン</param>
        ''' <remarks></remarks>
        Public Shared Sub GetSalesLock(ByVal salesid As Decimal, ByVal rowlockversion As Long)

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_212")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesLock_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("SELECT")
                    .AppendLine("  /* SC3080203_212 */")
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
                .AppendLine("    /* SC3080203_233 */")
                .AppendLine(" INTO TB_T_SALES_ACT (")
                .AppendLine("    SALES_ACT_ID ,")
                .AppendLine("    SALES_ID ,")
                .AppendLine("    ACT_ID ,")
                .AppendLine("    RSLT_SALES_CAT ,")
                .AppendLine("    MODEL_CD ,")
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
            Using query As New DBUpdateQuery("SC3080203_233")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESACTID", OracleDbType.Decimal, salesactid)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
                If String.IsNullOrEmpty(rsltsalescat) Then
                    rsltsalescat = " "
                End If
                query.AddParameterWithTypeValue("RSLT_SALES_CAT", OracleDbType.NVarchar2, rsltsalescat)
                If String.IsNullOrEmpty(modelcode) Then
                    modelcode = " "
                End If
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcode)
                If String.IsNullOrEmpty(modelname) Then
                    modelname = " "
                End If
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


        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 START
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
                .AppendLine("  /* SC3080203_402 */ ")
                .AppendLine("  SQ_TESTDRIVE_ID.NEXTVAL AS SEQ ")
                .AppendLine(" FROM ")
                .AppendLine("  DUAL ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_402")
                query.CommandText = sql.ToString()
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetReqTestDriveId_End")
                'ログ出力 End *****************************************************************************
                Return Decimal.Parse(query.GetData()(0)(0).ToString)
            End Using

        End Function
        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 START
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        ''' <summary>
        ''' 試乗予約追加
        ''' </summary>
        ''' <param name="testdriveid">試乗予約ID</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="brncd">店舗コード</param>
        ''' <param name="modelcd">モデルコード</param>
        ''' <param name="modelname">グレードコード</param>
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
                                               ByVal modelcd As String, ByVal modelname As String, ByVal cstid As Decimal, ByVal salesid As Decimal, _
                                               ByVal rsltdate As Date, ByVal rsltfrom As Date, ByVal rsltto As Date, _
                                               ByVal stfcd As String, ByVal account As String, ByVal rowfunction As String, ByVal orgnzid As Decimal) As Integer
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertTestDrive_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT ")
                .AppendLine("    /* SC3080203_401 */ ")
                .AppendLine(" INTO TB_H_TESTDRIVE ( ")
                .AppendLine("    REQ_TESTDRIVE_ID, ")
                .AppendLine("    DLR_CD, ")
                .AppendLine("    VCL_TESTDRIVE_ID, ")
                .AppendLine("    VCL_TESTDRIVE_BRN_CD, ")
                .AppendLine("    PREF_MODEL_CD, ")
                .AppendLine("    PREF_GRADE_CD, ")
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
                .AppendLine("    :GRADECD, ")
                .AppendLine("    :CSTID, ")
                .AppendLine("    :SALESID, ")
                .AppendLine("    :SCHEFROMDATE, ")
                .AppendLine("    :SCHEFROMDATETIME, ")
                .AppendLine("    :SCHETODATETIME, ")
                .AppendLine("    :BRNCD, ")
                .AppendLine("    :STFCD, ")
                .AppendLine("    '1' , ")
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
            Using query As New DBUpdateQuery("SC3080203_401")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("TESTDRIVEID", OracleDbType.Decimal, testdriveid)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brncd)
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcd)
                query.AddParameterWithTypeValue("GRADECD", OracleDbType.NVarchar2, modelname)
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
        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 4.95.用件データロック
        ''' </summary>
        ''' <param name="reqid">用件ID</param>
        ''' <param name="rowlockversion">行ロックバージョン</param>
        ''' <remarks></remarks>
        Public Shared Sub GetRequestLock(ByVal reqid As Decimal, ByVal rowlockversion As Long)

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_216")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetRequestLock_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("SELECT")
                    .AppendLine("  /* SC3080203_216 */")
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


        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
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
        ''' <param name="rowfunction">行作成機能</param>
        ''' <param name="firstsalesactid">初回商談活動ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertSales(ByVal fllwupboxseqno As Decimal, ByVal dlrcd As String, ByVal brncd As String,
                                                       ByVal cstid As Decimal, ByVal prospectcd As String, ByVal reqid As Decimal,
                                                       ByVal compflg As String, ByVal giveupvclseq As Long, ByVal giveupresion As String,
                                                       ByVal acount As String, ByVal rowfunction As String,
                                                       ByVal firstsalesactid As Decimal) As Integer
            ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSales_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT")
                .AppendLine("    /* SC3080203_232 */")
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
                .AppendLine("    SALES_COMPLETE_FLG ,")
                .AppendLine("    DIRECT_SALES_FLG ,")
                .AppendLine("    GIVEUP_COMP_VCL_SEQ ,")
                .AppendLine("    GIVEUP_REASON ,")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                .AppendLine("    BRAND_RECOGNITION_ID , ")
                .AppendLine("    ACARD_NUM, ")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
                .AppendLine("    FIRST_SALES_ACT_ID, ")
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
                '2017/11/20 TCS 河原 TKM独自機能開発 START
                .AppendLine("    DIRECT_SALES_FLG_UPDATE_FLG ,")
                '2017/11/20 TCS 河原 TKM独自機能開発 END
                .AppendLine("    ROW_CREATE_DATETIME ,")
                .AppendLine("    ROW_CREATE_ACCOUNT ,")
                .AppendLine("    ROW_CREATE_FUNCTION ,")
                .AppendLine("    ROW_UPDATE_DATETIME ,")
                .AppendLine("    ROW_UPDATE_ACCOUNT ,")
                .AppendLine("    ROW_UPDATE_FUNCTION ,")
                .AppendLine("    ROW_LOCK_VERSION ")
                .AppendLine(")")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                .AppendLine(" SELECT ")
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
                .AppendLine("    :COMPFLG ,")
                '2017/11/20 TCS 河原 TKM独自機能開発 START
                .AppendLine("    DECODE(DIRECT_SALES_FLG,'1','1','0') ,")
                '2017/11/20 TCS 河原 TKM独自機能開発 END
                .AppendLine("    :GIVEUPVCLSEQ ,")
                .AppendLine("    :GIVEUPREASON ,")
                .AppendLine("    SLST.BRAND_RECOGNITION_ID , ")
                .AppendLine("    SLST.ACARD_NUM , ")
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
                .AppendLine("    :FIRST_SALES_ACT_ID , ")
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
                .AppendLine("    DECODE(DIRECT_SALES_FLG,'1','1','0') ,")
                .AppendLine("    SYSDATE ,")
                .AppendLine("    :ACCOUNT ,")
                .AppendLine("    :FUNCTION ,")
                .AppendLine("    SYSDATE ,")
                .AppendLine("    :ACCOUNT ,")
                .AppendLine("    :FUNCTION ,")
                .AppendLine("    0 ")
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_SALES_TEMP SLST ")
                .AppendLine(" WHERE ")
                .AppendLine("   SLST.SALES_ID = :SALES_ID ")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            End With
            Using query As New DBUpdateQuery("SC3080203_232")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brncd)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstid)
                query.AddParameterWithTypeValue("PROSPECTCD", OracleDbType.NVarchar2, prospectcd)
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
                If String.IsNullOrEmpty(compflg) Then
                    compflg = " "
                End If
                query.AddParameterWithTypeValue("COMPFLG", OracleDbType.NVarchar2, compflg)
                query.AddParameterWithTypeValue("GIVEUPVCLSEQ", OracleDbType.Long, giveupvclseq)
                query.AddParameterWithTypeValue("GIVEUPREASON", OracleDbType.NVarchar2, giveupresion)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
                query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, rowfunction)
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, fllwupboxseqno)
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
                query.AddParameterWithTypeValue("FIRST_SALES_ACT_ID", OracleDbType.Decimal, firstsalesactid)
                ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSales_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
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
                                                       ByVal lastactdatetime As Date, ByVal lastcallrsltid As String, ByVal lastactid As Decimal,
                                                       ByVal recldatetime As Date, ByVal recdatetime As Date, ByVal dlrcd As String,
                                                       ByVal brncd As String, ByVal staffcd As String, ByVal reccontactmtd As String, ByVal reqactid As Decimal,
                                                       ByVal acount As String, ByVal rowfunction As String, ByVal salesId As Decimal, ByVal orgnzid As Decimal) As Integer
            ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertRequest_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT")
                .AppendLine("    /* SC3080203_231 */")
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
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                .AppendLine(" SELECT ")
                .AppendLine("    :REQ_ID ,")
                .AppendLine("    :CRCUSTID ,")
                .AppendLine("    :VCL_ID ,")
                .AppendLine("    :CUSTOMERCLASS ,")
                .AppendLine("    '2' ,")
                .AppendLine("    SLST.SOURCE_1_CD ,")
                ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) Start
                .AppendLine("    NVL(SLLOCAL.SOURCE_2_CD,0) ,")
                ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) End
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
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_SALES_TEMP SLST ")
                ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) Start
                .AppendLine(" LEFT JOIN TB_LT_SALES SLLOCAL ")
                .AppendLine("   ON SLST.SALES_ID = SLLOCAL.SALES_ID ")
                ' 2020/01/28 TS 舩橋 TKM Change request development for Next Gen e-CRB (CR058,CR061) End
                .AppendLine(" WHERE ")
                .AppendLine("   SLST.SALES_ID = :SALES_ID ")
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
            End With
            Using query As New DBUpdateQuery("SC3080203_231")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crsuctid)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.NVarchar2, customerclass)
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
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
                ' 2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                query.AddParameterWithTypeValue("ORG_ID", OracleDbType.Decimal, orgnzid)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertRequest_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
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
                                                       ByVal orgnzid As Decimal, ByVal orgnzidplan As Decimal,
                                                       ByVal DateTime_Flg As Integer, ByVal WI_DateTime_Flg As Integer) As Integer
            ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertActivity_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT")
                .AppendLine("    /* SC3080203_234 */")
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
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .AppendLine("    SCHE_ORGNZ_ID ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
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
                .AppendLine("    RSLT_SALES_PROSPECT_CD ,")
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                '2014/05/15 TCS 武田 受注後フォロー機能開発 START
                .AppendLine("    RSLT_INPUT_DATETIME ")
                '2014/05/15 TCS 武田 受注後フォロー機能開発 END
                .AppendLine(")")
                .AppendLine("VALUES (:ACTID ,")
                .AppendLine("    :REQ_ID ,")
                .AppendLine("    :ATT_ID ,")
                .AppendLine("    :COUNT ,")
                .AppendLine("    :SCHE_DATEORTIME ,")
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
                .AppendLine("    :DATETIME_FLG ,")
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
                .AppendLine("    :WALKIN_SCHE_START_DATEORTIME ,")
                .AppendLine("    :WALKIN_SCHE_END_DATEORTIME ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .AppendLine("    :WALK_DT_FLG ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                .AppendLine("    :DLRCD_PLAN ,")
                .AppendLine("    :BRANCH_PLAN ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                .AppendLine("    :ORG_PLAN ,")
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
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
                .AppendLine("    :CREATE_CRACTRESULT ,")
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                '2014/05/15 TCS 武田 受注後フォロー機能開発 START
                If (rsltflg = "1") Then
                    .AppendLine("    SYSDATE ")
                Else
                    .AppendLine("    TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
                End If
                '2014/05/15 TCS 武田 受注後フォロー機能開発 END
                .AppendLine(")")
            End With
            Using query As New DBUpdateQuery("SC3080203_234")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
                query.AddParameterWithTypeValue("COUNT", OracleDbType.Long, count)
                query.AddParameterWithTypeValue("SCHE_DATEORTIME", OracleDbType.Date, schedatetime)
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
                If DateTime_Flg = 1 Then
                    query.AddParameterWithTypeValue("DATETIME_FLG", OracleDbType.NVarchar2, "1")
                Else
                    query.AddParameterWithTypeValue("DATETIME_FLG", OracleDbType.NVarchar2, "0")
                End If
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
                query.AddParameterWithTypeValue("WALKIN_SCHE_START_DATEORTIME", OracleDbType.Date, walkinschestart)
                query.AddParameterWithTypeValue("WALKIN_SCHE_END_DATEORTIME", OracleDbType.Date, walkinscheend)
                query.AddParameterWithTypeValue("DLRCD_PLAN", OracleDbType.NVarchar2, dlrcdplan)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.NVarchar2, brncdplan)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.NVarchar2, staffcdplan)
                query.AddParameterWithTypeValue("SCHE_CONTACT_MTD", OracleDbType.NVarchar2, schecontactmtd)
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）START
                If WI_DateTime_Flg = 1 Then
                    query.AddParameterWithTypeValue("WALK_DT_FLG", OracleDbType.NVarchar2, "1")
                Else
                    query.AddParameterWithTypeValue("WALK_DT_FLG", OracleDbType.NVarchar2, "0")
                End If
                ' 2014/04/14 TCS 松月 【A STEP2】活動予定接触方法設定不正不具合対応（問連TR-V4-GTMC140211005）END
                query.AddParameterWithTypeValue("RSLT_FLG", OracleDbType.NVarchar2, rsltflg)
                query.AddParameterWithTypeValue("RSLT_DATE", OracleDbType.NVarchar2, Format(rsltdate, "yyyyMMdd"))
                query.AddParameterWithTypeValue("RSLT_DATETIME", OracleDbType.Date, rsltdate)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, brncd)
                query.AddParameterWithTypeValue("RSLT_STF_CD", OracleDbType.NVarchar2, rsltstaffcd)
                If String.IsNullOrEmpty(rsltcontactmtd) Then
                    rsltcontactmtd = " "
                End If
                query.AddParameterWithTypeValue("RSLT_CONTACT_MTD", OracleDbType.NVarchar2, rsltcontactmtd)
                query.AddParameterWithTypeValue("CRACTSTATUS", OracleDbType.NVarchar2, cractstatus)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                If createctactresult <> " " Then
                    query.AddParameterWithTypeValue("RSLTID", OracleDbType.NVarchar2, rsltid)
                Else
                    query.AddParameterWithTypeValue("RSLTID", OracleDbType.NVarchar2, "0")
                End If
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, acount)
                query.AddParameterWithTypeValue("FUNTION", OracleDbType.NVarchar2, rowfunction)
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                query.AddParameterWithTypeValue("CREATE_CRACTRESULT", OracleDbType.NVarchar2, createctactresult)
                query.AddParameterWithTypeValue("RSLT_ORGNZ_ID", OracleDbType.Decimal, orgnzid)
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                query.AddParameterWithTypeValue("ORG_PLAN", OracleDbType.Decimal, orgnzidplan)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
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
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="brncd">店舗コード</param>
        ''' <param name="followupseq">商談ID</param>
        ''' <remarks></remarks>
        Public Shared Sub GetFollowupSalesLock(ByVal dlrcd As String, ByVal brncd As String, ByVal followupseq As Decimal)

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_240")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupSalesLock_Start")
                'ログ出力 End *****************************************************************************

                With sql
                    .AppendLine("SELECT")
                    .AppendLine("  /* SC3080203_240 */")
                    .AppendLine("1")
                    .AppendLine(" FROM")
                    .AppendLine("  TBL_FLLWUPBOX_SALES")
                    .AppendLine(" WHERE")
                    .AppendLine("      DLRCD = :DLRCD")
                    .AppendLine("  AND STRCD = :STRCD")
                    .AppendLine("  AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                    .AppendLine("  AND REGISTFLG = '0'")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, brncd)
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

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_241")

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
                    .AppendLine("  /* SC3080203_241 */")
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
                    '.AppendLine("      AND T2.BODYCLR_CD = SUBSTR(T3.EXTCOLORCD,1,3) ")
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



        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 4.101.誘致データロック
        ''' </summary>
        ''' <param name="attid">誘致ID</param>
        ''' <param name="rowlockversion">行ロックバージョン</param>
        ''' <remarks></remarks>
        Public Shared Sub GetAttractLock(ByVal attid As Decimal, ByVal rowlockversion As Long)

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_265")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAttractLock_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("SELECT")
                    .AppendLine("  /* SC3080203_265 */")
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
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


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
                .AppendLine("  /* SC3080203_266 */")
                .AppendLine("  SQ_REQUEST.NEXTVAL AS SEQ ")
                .AppendLine(" FROM ")
                .AppendLine("  DUAL")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_266")

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
                .AppendLine("  /* SC3080203_267 */")
                .AppendLine("  SQ_ACTIVITY.NEXTVAL AS SEQ ")
                .AppendLine(" FROM ")
                .AppendLine("  DUAL")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_267")

                query.CommandText = sql.ToString()
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqActId_End")
                'ログ出力 End *****************************************************************************

                Return Decimal.Parse(query.GetData()(0)(0).ToString)
            End Using

        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 予定活動ID取得
        ''' </summary>
        ''' <returns>予定活動情報関連データセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetScheSqActId(ByVal reqid As Decimal, ByVal attid As Decimal) As SC3080203DataSet.SC3080203GetScheDataDataTable

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetScheSqActId_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT ")
                .AppendLine("  /* SC3080203_270 */")
                .AppendLine("  ACT_ID, ")
                .AppendLine("  ROW_LOCK_VERSION ")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_ACTIVITY ")
                .AppendLine(" WHERE")
                .AppendLine("      REQ_ID = :REQ_ID ")
                .AppendLine("  AND ATT_ID = :ATT_ID ")
                .AppendLine("  AND RSLT_FLG = '0' ")
            End With

            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203GetScheDataDataTable)("SC3080203_270")

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
                .AppendLine("  /* SC3080203_268 */")
                .AppendLine("  SQ_SALES.NEXTVAL AS SEQ")
                .AppendLine(" FROM")
                .AppendLine("  DUAL")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_268")

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
                .AppendLine("  /* SC3080203_269 */")
                .AppendLine("  SQ_SALES_ACT.NEXTVAL AS SEQ")
                .AppendLine(" FROM")
                .AppendLine("  DUAL")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_269")

                query.CommandText = sql.ToString()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqSalesActId_End")
                'ログ出力 End *****************************************************************************
                Return Decimal.Parse(query.GetData()(0)(0).ToString)
            End Using

        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 4.106.誘致更新
        ''' </summary>
        ''' <param name="attid"></param>
        ''' <param name="attstatus">用件ステータス</param>
        ''' <param name="lastactdate"></param>
        ''' <param name="count"></param>
        ''' <param name="lastrsltid"></param>
        ''' <param name="lastactid"></param>
        ''' <param name="account">行更新アカウント</param>
        ''' <param name="rowuodatefunction">行更新機能</param>
        ''' <param name="rowlockversion">行ロックバージョン</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateAttract(ByVal attid As Decimal, ByVal attstatus As String, ByVal lastactdate As Date, ByVal count As Long,
                                                     ByVal lastrsltid As String, ByVal lastactid As Decimal,
                                                     ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long) As Integer
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            Using query As New DBUpdateQuery("SC3080203_242")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateAttract_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203_242 */")
                    .AppendLine("    TB_T_ATTRACT")
                    .AppendLine(" SET")
                    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                    .AppendLine("    CONTINUE_ACT_STATUS = :ATTSTATUS ,")
                    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
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
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                query.AddParameterWithTypeValue("ATTSTATUS", OracleDbType.NVarchar2, attstatus)
                '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
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
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertBrochure_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT")
                .AppendLine("    /* SC3080203_243 */")
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
            Using query As New DBUpdateQuery("SC3080203_243")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
                If String.IsNullOrEmpty(modelcd) Then
                    modelcd = " "
                End If
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelcd)
                query.AddParameterWithTypeValue("RSLTDATE", OracleDbType.Date, rsltdate)
                query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
                If String.IsNullOrEmpty(rsltcontactmtd) Then
                    rsltcontactmtd = " "
                End If
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
                .AppendLine("    /* SC3080203_244 */")
                .AppendLine("INTO TB_H_REQUEST")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_REQUEST")
                .AppendLine("    WHERE")
                .AppendLine("      REQ_ID = :REQID")
            End With
            Using query As New DBUpdateQuery("SC3080203_244")
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
                .AppendLine("    /* SC3080203_245 */")
                .AppendLine("INTO TB_H_ATTRACT")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ATTRACT")
                .AppendLine("    WHERE")
                .AppendLine("      ATT_ID = :ATTID")
            End With
            Using query As New DBUpdateQuery("SC3080203_245")
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
                .AppendLine("    /* SC3080203_246 */")
                .AppendLine("INTO TB_H_ATTRACT_CALL")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ATTRACT_CALL")
                .AppendLine("    WHERE")
                .AppendLine("      ATT_ID = :ATTID")
            End With
            Using query As New DBUpdateQuery("SC3080203_246")
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
                .AppendLine("    /* SC3080203_247 */")
                .AppendLine("INTO TB_H_ACTIVITY")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ACTIVITY")
                .AppendLine("    WHERE")
                .AppendLine("          REQ_ID = :REQID")
                .AppendLine("      AND ATT_ID = :ATTID")

            End With
            Using query As New DBUpdateQuery("SC3080203_247")
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
        ''' <param name="reqidno"></param>
        ''' <param name="attidno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SelectActionID(ByVal reqidno As Decimal, ByVal attidno As Decimal) As SC3080203DataSet.SC3080203ActionidDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActionidDataTable)("SC3080203_248")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectActionID_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("SELECT")
                    .AppendLine("  /* SC3080203_248 */")
                    .AppendLine("  ACT_ID")
                    .AppendLine(" FROM")
                    .AppendLine("  TB_T_ACTIVITY")
                    .AppendLine(" WHERE")
                    .AppendLine("      REQ_ID = :REQID")
                    .AppendLine("  AND ATT_ID = :ATTID")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQID", OracleDbType.Decimal, reqidno)
                query.AddParameterWithTypeValue("ATTID", OracleDbType.Decimal, attidno)
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
                .AppendLine("    /* SC3080203_249 */")
                .AppendLine("INTO TB_H_ACTIVITY_MEMO")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ACTIVITY_MEMO")
                .AppendLine("    WHERE")
                .AppendLine("      RELATION_ACT_ID = :ACTID")
            End With
            Using query As New DBUpdateQuery("SC3080203_249")
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
                .AppendLine("    /* SC3080203_250 */")
                .AppendLine("INTO TB_H_SALES")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_SALES")
                .AppendLine("    WHERE")
                .AppendLine("      SALES_ID = :SALESID")
            End With
            Using query As New DBUpdateQuery("SC3080203_250")
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
                .AppendLine("    /* SC3080203_251 */")
                .AppendLine("INTO TB_H_SALES_ACT")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_SALES_ACT")
                .AppendLine("    WHERE")
                .AppendLine("      SALES_ID = :SALESID")
            End With
            Using query As New DBUpdateQuery("SC3080203_251")
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
                .AppendLine("    /* SC3080203_252 */")
                .AppendLine("INTO TB_H_PREFER_VCL")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_PREFER_VCL")
                .AppendLine("    WHERE")
                .AppendLine("      SALES_ID = :SALESID")
            End With
            Using query As New DBUpdateQuery("SC3080203_252")
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
                .AppendLine("    /* SC3080203_253 */")
                .AppendLine("INTO TB_H_COMPETITOR_VCL")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_COMPETITOR_VCL")
                .AppendLine("    WHERE")
                .AppendLine("      SALES_ID = :SALESID")
            End With
            Using query As New DBUpdateQuery("SC3080203_253")
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
                .AppendLine("    /* SC3080203_254 */")
                .AppendLine("INTO TB_H_BROCHURE")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_BROCHURE")
                .AppendLine("    WHERE")
                .AppendLine("      SALES_ID = :SALESID")
            End With
            Using query As New DBUpdateQuery("SC3080203_254")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveBrochure_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 START
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
                .AppendLine("    /* SC3080203_403 */ ")
                .AppendLine(" INTO TB_H_TESTDRIVE ")
                .AppendLine("    SELECT ")
                .AppendLine("      * ")
                .AppendLine("    FROM ")
                .AppendLine("      TB_T_TESTDRIVE ")
                .AppendLine("    WHERE ")
                .AppendLine("      SALES_ID = :SALESID ")
            End With
            Using query As New DBUpdateQuery("SC3080203_403")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESID", OracleDbType.Decimal, salesid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveTestDrive_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 END

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
                .AppendLine("    /* SC3080203_405 */ ")
                .AppendLine(" INTO TB_H_ASSESSMENT_ACT ")
                .AppendLine("    SELECT ")
                .AppendLine("      * ")
                .AppendLine("    FROM ")
                .AppendLine("      TB_T_ASSESSMENT_ACT ")
                .AppendLine("    WHERE ")
                .AppendLine("      SALES_ID = :SALESID ")
            End With
            Using query As New DBUpdateQuery("SC3080203_405")
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
            Using query As New DBUpdateQuery("SC3080203_255")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteRequest_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_255 */")
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
            Using query As New DBUpdateQuery("SC3080203_256")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttract_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_256 */")
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
            Using query As New DBUpdateQuery("SC3080203_257")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractCall_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_257 */")
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
            Using query As New DBUpdateQuery("SC3080203_258")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivity_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_258 */")
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
            Using query As New DBUpdateQuery("SC3080203_259")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteActivityMemo_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_259 */")
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
            Using query As New DBUpdateQuery("SC3080203_260")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSales_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_260 */")
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
            Using query As New DBUpdateQuery("SC3080203_261")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteSalesAct_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_261 */")
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
            Using query As New DBUpdateQuery("SC3080203_262")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeletePreferVcl_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_262 */")
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
            Using query As New DBUpdateQuery("SC3080203_263")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCompetitorVcl_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_263 */")
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
            Using query As New DBUpdateQuery("SC3080203_264")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteBrochure_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_264 */")
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

        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 試乗予約テーブル削除
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DeleteTestDrive(ByVal salesid As Decimal) As Integer
            Using query As New DBUpdateQuery("SC3080203_404")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteTestDrive_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE ")
                    .AppendLine("    /* SC3080203_404 */ ")
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
        ' 2013/06/30 TCS 王 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 査定テーブル削除
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DeleteAssessmentAct(ByVal salesid As Decimal) As Integer
            Using query As New DBUpdateQuery("SC3080203_406")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAssessmentAct_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_406 */")
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
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
        ''' <summary>
        ''' 4.90.希望車更新
        ''' </summary>
        ''' <param name="fllwupboxseqno">商談ID</param>
        ''' <param name="seq">希望車シーケンス</param>
        ''' <param name="rsltcontactmtd">見積実施コンタクト方法</param>
        ''' <param name="estamount">見積金額</param>
        ''' <param name="rsltstaffcd">見積実施スタッフコード</param>
        ''' <param name="account">行更新アカウント</param>
        ''' <param name="rowuodatefunction">行更新機能</param>
        ''' <param name="rowlockversion">行ロックバージョン</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdatePrefer(ByVal fllwupboxseqno As Decimal, ByVal seq As String, ByVal rsltcontactmtd As String, ByVal estamount As Long, ByVal rsltstaffcd As String,
                                        ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long, ByVal orgnzid As Decimal) As Integer
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            Using query As New DBUpdateQuery("SC3080203_290")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203_290 */")
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
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 宋 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 査定情報取得
        ''' </summary>
        ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
        ''' <returns>下取り査定情報のデータセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetActAsmInfo(ByVal fllwupboxseqno As Decimal) As SC3080203DataSet.ActAsmInfoDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.ActAsmInfoDataTable)("SC3080203_271")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActAsmInfo_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .Append("SELECT /* SC3080203_271 */ ")
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
        ' 2013/06/30 TCS 宋 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 宋 2013/10対応版　既存流用 START
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
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetAssessmentAct_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("INSERT ")
                .AppendLine("    /* SC3080203_272 */ ")
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
            Using query As New DBUpdateQuery("SC3080203_272")
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
        ' 2013/06/30 TCS 宋 2013/10対応版　既存流用 END

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 競合車種登録
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <param name="seqno">競合車種連番</param>
        ''' <param name="modelcd">モデルコード</param>
        ''' <param name="account">作成アカウント</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function SetCompetitorvcl(ByVal salesid As String,
                                         ByVal seqno As String,
                                         ByVal modelcd As String,
                                         ByVal account As String) As Integer

            Dim sql As New StringBuilder

            With sql
                .Append("INSERT ")
                .Append("    /* SC3080203_273 */ ")
                .Append("INTO TB_T_COMPETITOR_VCL ( ")
                .Append("    SALES_ID, ")
                .Append("    COMP_VCL_SEQ, ")
                .Append("    MODEL_CD, ")
                .Append("    ROW_CREATE_DATETIME, ")
                .Append("    ROW_CREATE_ACCOUNT, ")
                .Append("    ROW_CREATE_FUNCTION, ")
                .Append("    ROW_UPDATE_DATETIME, ")
                .Append("    ROW_UPDATE_ACCOUNT, ")
                .Append("    ROW_UPDATE_FUNCTION, ")
                .Append("    ROW_LOCK_VERSION ")
                .Append(") ")
                .Append("VALUES ( ")
                .Append("    :FLLWUPBOX_SEQNO, ")
                .Append("    :SEQNO, ")
                .Append("    :SERIESCD, ")
                .Append("    SYSDATE, ")
                .Append("    :ACCOUNT, ")
                .Append("    'SC3080203', ")
                .Append("    SYSDATE, ")
                .Append("    :ACCOUNT, ")
                .Append("    'SC3080203', ")
                .Append("0 ")
                .Append(") ")
            End With


            Using query As New DBUpdateQuery("SC3080203_273")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesid)   '商談ID
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)                 '競合車種連番
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, modelcd)        'モデルコード
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)         '作成アカウント

                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 競合車種取得
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <param name="modelcode">モデルコード</param>
        ''' <remarks></remarks>
        Public Shared Function GetCompvalseq(ByVal salesid As String, ByVal modelcode As String) As Integer

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* SC3080203_274 */ ")
                .Append("       COUNT(COMP_VCL_SEQ)")
                .Append("  FROM TB_T_COMPETITOR_VCL ")
                .Append(" WHERE SALES_ID = :SALESEID ")
                .Append("   AND MODEL_CD = :SERIESCD ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_274")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESEID", OracleDbType.Decimal, salesid)               '商談ID
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, modelcode)           'モデルコード

                query.CommandText = sql.ToString()
                Return query.GetCount()

            End Using
        End Function
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 競合車種連番取得
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <remarks></remarks>
        Public Shared Function GetMaxcompvalseq(ByVal salesid As String) As Integer

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* SC3080203_275 */ ")
                .Append("       NVL( MAX(COMP_VCL_SEQ), 0) ")
                .Append("  FROM TB_T_COMPETITOR_VCL ")
                .Append(" WHERE SALES_ID = :SALESEID ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_275")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALESEID", OracleDbType.Decimal, salesid)               '商談ID

                query.CommandText = sql.ToString()
                Return query.GetCount()

            End Using
        End Function
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END



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
                .Append("SELECT /* SC3080203_501 */ ")
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

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_501")
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
                .AppendLine("    /* SC3080203_502 */")
                .AppendLine("INTO TB_H_ATTRACT_DM")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ATTRACT_DM")
                .AppendLine("    WHERE")
                .AppendLine("      ATT_ID = :ATTID")
            End With
            Using query As New DBUpdateQuery("SC3080203_502")
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
            Using query As New DBUpdateQuery("SC3080203_503")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractDM_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_503 */")
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
                .AppendLine("    /* SC3080203_504 */")
                .AppendLine("INTO TB_H_ATTRACT_RMM")
                .AppendLine("    SELECT")
                .AppendLine("      *")
                .AppendLine("    FROM")
                .AppendLine("      TB_T_ATTRACT_RMM")
                .AppendLine("    WHERE")
                .AppendLine("      ATT_ID = :ATTID")
            End With
            Using query As New DBUpdateQuery("SC3080203_504")
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
            Using query As New DBUpdateQuery("SC3080203_505")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAttractRMM_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("DELETE")
                    .AppendLine("    /* SC3080203_505 */")
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
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateSalesstatus(ByVal fllwupboxseqno As Decimal, ByVal seq As String, ByVal cractrslt As String, ByVal account As String, ByVal rowuodatefunction As String, ByVal rowlockversion As Long, ByVal actid As Decimal) As Integer
            Using query As New DBUpdateQuery("SC3080203_506")
                Dim sql As New StringBuilder
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_Start")
                'ログ出力 End *****************************************************************************
                With sql
                    .AppendLine("UPDATE")
                    .AppendLine("    /* SC3080203__506 */")
                    .AppendLine("    TB_T_PREFER_VCL")
                    .AppendLine(" SET")
                    .AppendLine("    SALES_STATUS = :SALESTATUS ,")
                    '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
                    .AppendLine("    SALESBKG_ACT_ID = :ACTID ,")
                    '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ACCOUNT ,")
                    .AppendLine("    ROW_UPDATE_DATETIME = SYSDATE ,")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION,")
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1")
                    .AppendLine(" WHERE")
                    .AppendLine("        SALES_ID = TO_NUMBER(:FLLWUPBOX_SEQNO)")
                    .AppendLine("    AND PREF_VCL_SEQ = TO_NUMBER(:SEQ)")
                    .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQ", OracleDbType.NVarchar2, seq)
                query.AddParameterWithTypeValue("SALESTATUS", OracleDbType.NVarchar2, cractrslt)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）START
                If cractrslt = "31" Then
                    query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, actid)
                Else
                    query.AddParameterWithTypeValue("ACTID", OracleDbType.Decimal, 0)
                End If
                '2014/01/29 TCS 松月 【A STEP2】成約活動ID設定漏れ対応（号口切替BTS-15）END
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowuodatefunction)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdatePrefer_End")
                'ログ出力 End *****************************************************************************
                Return query.Execute()
            End Using
        End Function
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END

#Region "Aカード情報相互連携開発"
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START

        ''' <summary>
        ''' 活動結果マスタ取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="otherActRsltId">（その他扱いとなる）活動結果ID</param>
        ''' <returns></returns>
        ''' <remarks>活動結果マスタ（断念理由のみ）を取得する。</remarks>
        Public Shared Function GetGiveupReasonMaster(ByVal dlrCd As String, ByVal brnCd As String _
                                                     , ByVal otherActRsltId As Decimal) As SC3080203DataSet.SC3080203GiveupReasonMasterDataTable

            Dim ret As SC3080203DataSet.SC3080203GiveupReasonMasterDataTable = Nothing
            Dim sql As New StringBuilder(10000)

            'ログ出力 Start ***************************ACT_STATUS_GIVEUP************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReasonMaster_Start")
            'ログ出力 End *****************************************************************************

            Try
                With sql
                    .AppendLine("SELECT /* SC3080203_601 */ ")
                    .AppendLine("    ACT_RSLT_ID ")
                    .AppendLine("    ,RSLT_CAT_NAME ")
                    .AppendLine("    ,CASE ACT_RSLT_ID WHEN :OTHER_ID THEN 'true' ELSE 'false' END AS OTHER_FLG ")
                    .AppendLine("FROM ")
                    .AppendLine("    TB_M_ACTIVITY_RESULT ")
                    .AppendLine("WHERE ")
                    .AppendLine("    DLR_CD IN (:DLR_CD, 'XXXXX') ")
                    .AppendLine("    AND BRN_CD IN (:BRN_CD, 'XXX') ")
                    .AppendLine("    AND INUSE_FLG = '1' ")
                    .AppendFormat("    AND BIZ_CAT_ID = {0} ", BIZCATID)
                    .AppendFormat("    AND ACT_STATUS = '{0}' ", ACT_STATUS_GIVEUP)
                    .AppendLine("ORDER BY  SORT_ORDER ")
                End With

                Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203GiveupReasonMasterDataTable)("SC3080203_601")

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                    query.AddParameterWithTypeValue("OTHER_ID", OracleDbType.Decimal, otherActRsltId)

                    ret = query.GetData()

                End Using
            Finally
                sql.Clear()
            End Try

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReasonMaster_End")
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
                    .AppendLine("SELECT /* SC3080203_604 */ ")
                    .AppendLine("   1 ")
                    .AppendLine("FROM TB_T_SALES_TEMP ")
                    .AppendLine("WHERE SALES_ID = :SALES_ID ")
                    .AppendFormat("FOR UPDATE WAIT {0} ", env.GetLockWaitTime())
                End With

                Using query As New DBSelectQuery(Of DataTable)("SC3080203_604")

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
                    .AppendLine("INSERT /* SC3080203_605 */ ")
                    .AppendLine("INTO TB_T_SALES_TEMP_DEL ")
                    .AppendLine("SELECT * ")
                    .AppendLine("FROM TB_T_SALES_TEMP ")
                    .AppendLine("WHERE SALES_ID = :SALES_ID ")
                End With

                Using query As New DBUpdateQuery("SC3080203_605")

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
                    .AppendLine("DELETE /* SC3080203_606 */  ")
                    .AppendLine("FROM TB_T_SALES_TEMP ")
                    .AppendLine("WHERE SALES_ID = :SALES_ID ")
                End With

                Using query As New DBUpdateQuery("SC3080203_606")

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
                    .AppendLine("UPDATE /* SC3080203_607 */  ")
                    .AppendLine("    TB_T_SALES ")
                    .AppendLine("SET ")
                    .AppendLine("    ACARD_NUM = :ACARD_NUM ")
                    .AppendLine("   ,ROW_UPDATE_DATETIME = SYSDATE ")
                    .AppendLine("   ,ROW_UPDATE_ACCOUNT = :STF_CD ")
                    .AppendLine("   ,ROW_UPDATE_FUNCTION = 'SC3080203' ")
                    .AppendLine("   ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE SALES_ID = :SALES_ID ")
                End With

                Using query As New DBUpdateQuery("SC3080203_607")

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
        ''' 成約以外の見積を削除する
        ''' </summary>
        ''' <param name="fllwupboxseqno">商談ID</param>
        ''' <param name="updateAccount"></param>
        ''' <param name="updateId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateEstimateDel(ByVal fllwupboxseqno As Decimal,
                                              ByVal updateAccount As String, ByVal updateId As String) As Integer

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimateDel_Start")
            'ログ出力 End *****************************************************************************
            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3080203_260 */ ")
                .AppendLine("       TBL_ESTIMATEINFO T1 ")
                .AppendLine("   SET T1.DELFLG = '1' ")
                .AppendLine("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT ")
                .AppendLine("     , T1.UPDATEDATE = SYSDATE ")
                .AppendLine("     , T1.UPDATEID  = :UPDATEID ")
                .AppendLine(" WHERE      T1.FLLWUPBOX_SEQNO = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
                .AppendLine("        AND T1.DELFLG = '0' ")
                .AppendLine("        AND T1.SUCCESSFLG = '0' ")
                .AppendLine("        AND T1.CONTRACTFLG <> '1' ")
            End With

            Using query As New DBUpdateQuery("SC3080203_260")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                Return query.Execute()
            End Using

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateEstimateDel_End")
            'ログ出力 End *****************************************************************************
        End Function

        ''' <summary>
        ''' 見積データロック(見積削除用)
        ''' </summary>
        ''' <param name="fllwupboxseqno">商談ID</param>
        ''' <remarks></remarks>
        Public Shared Sub GetEstimateDelLock(ByVal fllwupboxseqno As Decimal)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateLock_Start")
            'ログ出力 End *****************************************************************************

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_261")
                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder

                With sql
                    .AppendLine(" SELECT")
                    .AppendLine("  /* SC3080203_261 */")
                    .AppendLine(" 1")
                    .AppendLine(" FROM")
                    .AppendLine("     TBL_ESTIMATEINFO T1 ")
                    .AppendLine(" WHERE      T1.FLLWUPBOX_SEQNO = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
                    .AppendLine("        AND T1.DELFLG = '0' ")
                    .AppendLine("        AND T1.SUCCESSFLG = '0' ")
                    .AppendLine("        AND T1.CONTRACTFLG <> '1' ")
                    .AppendLine(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
                query.GetData()

            End Using

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateLock_End")
            'ログ出力 End *****************************************************************************

        End Sub

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
#End Region



        ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 4.507.組織ID取得
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
                .AppendLine("  /* SC3080203__507 */")
                .AppendLine("    ORGNZ_ID ")
                .AppendLine("FROM ")
                .AppendLine("    TB_M_STAFF ")
                .AppendLine("WHERE ")
                .AppendLine("    STF_CD = :STFCD ")
            End With
            Using query As New DBSelectQuery(Of DataTable)("SC3080203__507")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STFCD", OracleDbType.NVarchar2, stfcd)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetorgnzId_End")
                'ログ出力 End *****************************************************************************
                Return Decimal.Parse(query.GetData()(0)(0).ToString)
            End Using
        End Function
        ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 END

        ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 次回活動アカウント取得
        ''' </summary>
        ''' <returns>予定活動情報関連データセット</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStaffPlan(ByVal reqid As Decimal, ByVal attid As Decimal) As String

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffPlan")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT ")
                .AppendLine("  /* SC3080203_508 */")
                .AppendLine("  SCHE_STF_CD ")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_ACTIVITY ")
                .AppendLine(" WHERE")
                .AppendLine("      REQ_ID = :REQ_ID ")
                .AppendLine("  AND ATT_ID = :ATT_ID ")
                .AppendLine("  AND RSLT_FLG = '0' ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_508")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffPlan")
                'ログ出力 End *****************************************************************************
                Return query.GetData()(0)(0).ToString
            End Using

        End Function
        ' 2013/06/30 TCS 黄 2013/10対応版　既存流用 END


        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 活動データ存在スチェック
        ''' </summary>
        ''' <param name="attid">誘致ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCountStaffPlan(ByVal reqid As Decimal, ByVal attid As Decimal) As Integer

            Dim sql As New StringBuilder

            With sql
                .AppendLine("SELECT ")
                .AppendLine("  /* SC3080203_509 */")
                .AppendLine("    COUNT(*) ")
                .AppendLine(" FROM")
                .AppendLine("  TB_T_ACTIVITY ")
                .AppendLine(" WHERE")
                .AppendLine("      REQ_ID = :REQ_ID ")
                .AppendLine("  AND ATT_ID = :ATT_ID ")
                .AppendLine("  AND RSLT_FLG = '0' ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_509")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQ_ID", OracleDbType.Decimal, reqid)
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
                query.CommandText = sql.ToString()
                Return Integer.Parse(query.GetData()(0)(0).ToString)

            End Using

        End Function
        ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        ' 2014/05/23 TCS 安田 TMT不具合対応 START
        ''' <summary>
        ''' モデルコードに紐付く、競合車種連番取得
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <remarks></remarks>
        Public Shared Function GetModelCompvalseq(ByVal salesid As Decimal, ByVal modelcode As String) As Integer

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* SC3080203_276 */ ")
                .Append("       MAX(COMP_VCL_SEQ) ")
                .Append("  FROM TB_T_COMPETITOR_VCL ")
                .Append(" WHERE SALES_ID = :SALES_ID ")
                .Append("   AND MODEL_CD = :MODEL_CD ")
            End With

            Using query As New DBSelectQuery(Of DataTable)("SC3080203_276")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)               '商談ID
                query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, modelcode)           'モデルコード

                query.CommandText = sql.ToString()
                Return query.GetCount()

            End Using
        End Function
        ' 2014/05/23 TCS 安田 TMT不具合対応 END

        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
        ''' <summary>
        ''' 初回商談活動ID取得
        ''' </summary>
        ''' <param name="attid">誘致ID</param>
        ''' <returns>初回商談活動ID</returns>
        ''' <remarks></remarks>
        Public Shared Function GetFstSalesActId(ByVal attid As Decimal) As SC3080203DataSet.SC3080203FstSalesActIdMasterDataTable

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFstSalesActId_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT /* SC3080203_609 */ ")
                .AppendLine("       MIN(ACT_ID) AS ACT_ID ")
                .AppendLine("  FROM TB_T_ACTIVITY ")
                .AppendLine(" WHERE ACT_ID > ")
                .AppendLine("      (SELECT MIN(ACT_ID) ")
                .AppendLine("         FROM TB_T_ACTIVITY ")
                .AppendLine("        WHERE ATT_ID = :ATT_ID ")
                .AppendLine("          AND RSLT_SALES_PROSPECT_CD <> ' ') ")
                .AppendLine("   AND ATT_ID = :ATT_ID ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FstSalesActIdMasterDataTable)("SC3080203_609")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ATT_ID", OracleDbType.Decimal, attid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFstSalesActId_End")
                'ログ出力 End *****************************************************************************
                Return query.GetData()
            End Using

        End Function
        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END

        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 START
        ''' <summary>
        ''' 初回商談活動取得
        ''' </summary>
        ''' <param name="salesid">商談ID</param>
        ''' <returns>初回商談</returns>
        ''' <remarks></remarks>
        Public Shared Function GetFstSalesAct(ByVal salesid As Decimal) As SC3080203DataSet.SC3080203FstSalesActMasterDataTable

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsFstSalesAct_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .AppendLine("SELECT /* SC3080203_610 */  ")
                .AppendLine("       FIRST_SALES_ACT_ID ")
                .AppendLine("  FROM TB_T_SALES ")
                .AppendLine(" WHERE SALES_ID = :SALES_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FstSalesActMasterDataTable)("SC3080203_610")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("IsFstSalesAct_End")
                'ログ出力 End *****************************************************************************
                Return query.GetData()
            End Using

        End Function
        ' 2014/05/15 TCS 武田 受注後フォロー機能開発 END

        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) START
        ''' <summary>
        ''' 顧客メモ連番採番
        ''' </summary>
        ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCustmemoseq(ByVal dlrcd As String,
                                              ByVal crcustid As Decimal) As Long
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080203_030 */ ")
                .Append("    NVL(MAX(CST_MEMO_SEQ),0) + 1 AS SEQ ")
                .Append("FROM ")
                .Append("  TB_T_CUSTOMER_MEMO ")
                .Append("WHERE ")
                .Append("  DLR_CD = :DLRCD ")
                .Append("  AND CST_ID = :CSTID ")
            End With

            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203MemoSEQDataTable)("SC3080203_031")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)

                Dim seqTbl As SC3080203DataSet.SC3080203MemoSEQDataTable

                seqTbl = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_End")
                'ログ出力 End *****************************************************************************

                Return seqTbl.Item(0).SEQ

            End Using

        End Function

        ''' <summary>
        ''' 顧客メモ追加
        ''' </summary>
        ''' <param name="seqno">顧客メモ連番</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="crcustid">活動先顧客コード</param>
        ''' <param name="memo">メモ</param>
        ''' <param name="updateaccount">更新ユーザアカウント</param>
        ''' <returns>更新成功[True]/失敗[False]</returns>
        ''' <remarks></remarks>
        Public Shared Function InsertCustomerMemo(ByVal dlrcd As String,
                                                  ByVal crcustid As Decimal,
                                                  ByVal seqno As Long,
                                                  ByVal memo As String,
                                                  ByVal updateaccount As String) As Integer
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomerMemo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("INSERT ")
                .Append("    /* SC3080203_031 */ ")
                .Append("INTO TB_T_CUSTOMER_MEMO ( ")
                .Append("    DLR_CD , ")
                .Append("    CST_ID , ")
                .Append("    CST_MEMO_SEQ , ")
                .Append("    CST_MEMO , ")
                .Append("    CREATE_STF_CD , ")
                .Append("    CREATE_DATETIME , ")
                .Append("    ROW_CREATE_DATETIME , ")
                .Append("    ROW_CREATE_ACCOUNT , ")
                .Append("    ROW_CREATE_FUNCTION , ")
                .Append("    ROW_UPDATE_DATETIME , ")
                .Append("    ROW_UPDATE_ACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION , ")
                .Append("    ROW_LOCK_VERSION ")
                .Append(") ")
                .Append("VALUES ( ")
                .Append("    :DLRCD , ")
                .Append("    :INSDID , ")
                .Append("    :CUSTMEMOHIS_SEQNO , ")
                .Append("    :MEMO , ")
                .Append("    :ACCOUNT , ")
                .Append("    SYSDATE , ")
                .Append("    SYSDATE , ")
                .Append("    :ACCOUNT , ")
                .Append("    'SC3080203' , ")
                .Append("    SYSDATE , ")
                .Append("    :UPDATEACCOUNT , ")
                .Append("    'SC3080203' , ")
                .Append("      0 ")
                .Append(") ")
            End With

            Using query As New DBUpdateQuery("SC3080204_202")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Decimal, crcustid)
                query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateaccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomerMemo_End")
                'ログ出力 End *****************************************************************************

                Return query.Execute()

            End Using

        End Function
        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) END

    End Class

End Namespace
