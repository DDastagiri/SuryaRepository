'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080216DataTableTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細(受注後工程フォロー)
'補足： 
'作成： 2014/02/13 TCS 森   受注後フォロー機能開発
'更新： 2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' SC3080216(顧客詳細(受注後工程フォロー))
''' Webページで使用するデータ層
''' </summary>
''' <remarks></remarks>
''' 
Public NotInheritable Class SC3080216TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' Follow-up Box商談取得
    ''' </summary>
    ''' <param name="seqno">Follow-up Box内連番</param>>
    ''' <returns>SC3080216FllwSalesDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetFllwupboxSales(ByVal seqno As Decimal) As SC3080216DataSet.SC3080216FllwSalesDataTable

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216FllwSalesDataTable)("SC3080216_001")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSales_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080216_001 */ ")
                .Append("       CUSTSEGMENT, ")      '顧客区分"
                .Append("       CUSTOMERCLASS,")    '顧客分類
                .Append("       CRCUSTID,")         '活動先顧客コード
                .Append("       ACTUALACCOUNT,")    '対応アカウント
                .Append("       STARTTIME,")        '開始時間
                .Append("       ENDTIME,")          '終了時間
                .Append("       WALKINNUM,")        '来店人数
                .Append("       NEWFLLWUPBOXFLG,")  '新規活動フラグ
                .Append("       REGISTFLG,")        '登録フラグ
                .Append("       EIGYOSTARTTIME")    '営業活動開始時間
                .Append("  FROM TBL_FLLWUPBOX_SALES")
                .Append("  WHERE ")
                .Append("   FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO") 'Follow-up Box内連番
                .Append("   AND REGISTFLG = '0'")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, seqno)       'Follow-up Box内連番
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSales_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' 受注後工程フォロー結果の実績連番
    ''' </summary>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedafterFollowrsltMax() As Long

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216SeqDataTable)("SC3080216_006")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080216_006 */ ")
                .Append("    SEQ_BOOKEDAFTERFOLLOW.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("    DUAL")
            End With

            query.CommandText = sql.ToString()

            Dim seqTbl As SC3080216DataSet.SC3080216SeqDataTable

            seqTbl = query.GetData()

            Return seqTbl.Item(0).Seq

        End Using

    End Function

    ''' <summary>
    ''' 受注後工程フォロー結果追加
    ''' </summary>
    ''' <param name="row">データセット(インプット)</param>
    ''' <param name="updateid">機能ID</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <History></History>
    Public Shared Function InsertBookedafterFollowrslt(ByVal row As SC3080216DataSet.SC3080216PlanRow, _
                                                       ByVal updateid As String) As Integer

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("INTO ")
            .Append("    TBL_BOOKEDAFTERFOLLOWRSLT /* SC3080216_007 */ ")
            .Append("( ")
            .Append("   DLRCD,")            '販売店コード
            .Append("   STRCD,")            '店舗コード
            .Append("   FLLWUPBOX_SEQNO,")  'Follow-up Box内連番
            .Append("   SEQNO,")            '実績連番
            .Append("   CUSTSEGMENT,")      '顧客区分
            .Append("   CUSTOMERCLASS,")    '顧客分類
            .Append("   CRCUSTID,")         '活動先顧客コード
            .Append("   SALESBKGNO,")       '注文番号
            .Append("   WAITING_OBJECT,")   'イベント待ち状態区分
            .Append("   ACTUALACCOUNT,")    '対応アカウント
            .Append("   CONTACTNO,")        '接触方法No
            .Append("   SALESSTARTTIME,")   '商談開始時間
            .Append("   SALESENDTIME,")     '商談終了時間
            .Append("   WALKINNUM,")        '来店人数
            .Append("   ACTUALTIME_END,")   '活動終了日時
            .Append("   CREATEDATE,")       '作成日
            .Append("   UPDATEDATE,")       '更新日
            .Append("   CREATEACCOUNT,")    '作成者
            .Append("   UPDATEACCOUNT,")    '更新者
            .Append("   CREATEID,")         '作成機能ＩＤ
            .Append("   UPDATEID")          '更新機能ＩＤ
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("   :DLRCD,")
            .Append("   :STRCD,")
            .Append("   :FLLWUPBOX_SEQNO,")
            .Append("   :SEQNO,")
            .Append("   :CUSTSEGMENT,")
            .Append("   :CUSTOMERCLASS,")
            .Append("   :CRCUSTID,")
            .Append("   :SALESBKGNO,")
            .Append("   :WAITING_OBJECT,")
            .Append("   :ACTUALACCOUNT,")
            .Append("   :CONTACTNO,")
            .Append("   :SALESSTARTTIME,")
            .Append("   :SALESENDTIME,")
            .Append("   :WALKINNUM,")
            .Append("   :ACTUALTIME_END,")
            .Append("   SYSDATE,")
            .Append("   SYSDATE,")
            .Append("   :ACCOUNT,")
            .Append("   :ACCOUNT,")
            .Append("   :UPDATEID,")
            .Append("   :UPDATEID")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080216_007")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, row.DLRCD)                              '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, row.STRCD)                              '店舗コード
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, row.FLLWUPBOX_SEQNO)         'Follow-up Box内連番
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, row.SEQNO)                             '実績連番
            query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, row.CUSTSEGMENT)                  '顧客区分
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, row.CUSTOMERCLASS)              '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, row.CRCUSTID)                        '活動先顧客コード
            query.AddParameterWithTypeValue("SALESBKGNO", OracleDbType.NVarchar2, row.SALESBKGNO)               '注文番号
            query.AddParameterWithTypeValue("WAITING_OBJECT", OracleDbType.Char, DBNull.Value)                  'イベント待ち状態区分
            query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Varchar2, row.ACTUALACCOUNT)          '対応アカウント
            query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, row.CONTACTNO)                     '接触方法No
            query.AddParameterWithTypeValue("SALESSTARTTIME", OracleDbType.Date, row.SALESSTARTTIME)            '商談開始時間
            query.AddParameterWithTypeValue("SALESENDTIME", OracleDbType.Date, row.SALESENDTIME)                '商談終了時間
            Dim walkinnum As Nullable(Of Short)
            If (Not row.IsWALKINNUMNull) Then
                walkinnum = row.WALKINNUM
            End If
            query.AddParameterWithTypeValue("WALKINNUM", OracleDbType.Int16, walkinnum)                     '来店人数
            query.AddParameterWithTypeValue("ACTUALTIME_END", OracleDbType.Date, row.SALESENDTIME)           '活動終了日時
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, row.ACCOUNT)                  'アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)                    '機能ID

            Return query.Execute()

        End Using
    End Function


    ''' <summary>
    ''' Follow-upBox取得(活動結果登録用)
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowCractstatus(ByVal fllwupboxseqno As Decimal) As SC3080216DataSet.SC3080216CrstatusDataTable

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216CrstatusDataTable)("SC3080216_202")
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowCractstatus_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080216_202 */ ")
                .Append("  T2.ACT_STATUS AS CRACTSTATUS ")
                .Append("FROM ")
                .Append("  TB_H_SALES T1 , ")
                .Append("  TB_H_ACTIVITY T2 ")
                .Append("WHERE ")
                .Append("      T1.ATT_ID = T2.ATT_ID ")
                .Append("  AND T1.REQ_ID = T2.REQ_ID ")
                .Append("  AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowCractstatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' 受注後工程活動取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="mode">取得モード(1:日付別、2:工程別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedafterActivityInfo(ByVal salesId As Decimal, ByVal mode As String) As SC3080216DataSet.SC3080216AfterOdracTDataTable


        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedafterActivityInfo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT ")
            .Append("  /* SC3080216_203 */ ")
            .Append("    T2.AFTER_ODR_ACT_ID, ")
            .Append("    T3.AFTER_ODR_PRCS_CD, ")
            .Append("    CASE WHEN T7.WORD_VAL IS NULL THEN NULL ")
            .Append("         WHEN T7.WORD_VAL = ' ' THEN TRIM(T7.WORD_VAL_ENG) ")
            .Append("         ELSE TRIM(T7.WORD_VAL) ")
            .Append("    END AS AFTER_ODR_PRCS_NAME, ")
            .Append("    CASE ")
            .Append("    WHEN T4.AFTER_ODR_ACT_CD IS NOT NULL THEN T4.AFTER_ODR_ACT_CD ")
            .Append("    WHEN T4.AFTER_ODR_ACT_CD IS NULL THEN TO_NCHAR('0') ")
            .Append("    END AS AFTER_ODR_ACT_CD, ")
            .Append("    CASE WHEN T2.VOLUNTARYINS_ACT_NAME <> ' ' THEN VOLUNTARYINS_ACT_NAME ")
            .Append("         WHEN T8.WORD_VAL IS NULL THEN NULL ")
            .Append("         WHEN T8.WORD_VAL = ' ' THEN TRIM(T8.WORD_VAL_ENG) ")
            .Append("         ELSE TRIM(T8.WORD_VAL) ")
            .Append("    END AS AFTER_ODR_ACT_NAME, ")
            .Append("    CASE ")
            .Append("    WHEN AFTER_ODR_ACT_STATUS = '1' THEN ")
            .Append("    RSLT_DATEORTIME_FLG ")
            .Append("    ELSE ")
            .Append("    SCHE_DATEORTIME_FLG ")
            .Append("    END AS DATEORTIME_FLG, ")
            .Append("    CASE ")
            .Append("    WHEN AFTER_ODR_ACT_STATUS = '1' THEN ")
            .Append("    RSLT_START_DATEORTIME ")
            .Append("    ELSE ")
            .Append("    SCHE_START_DATEORTIME ")
            .Append("    END AS START_DATEORTIME, ")
            .Append("    CASE ")
            .Append("    WHEN AFTER_ODR_ACT_STATUS = '1' THEN ")
            .Append("    RSLT_END_DATEORTIME ")
            .Append("    ELSE ")
            .Append("    SCHE_END_DATEORTIME ")
            .Append("    END AS END_DATEORTIME, ")
            .Append("    CASE ")
            .Append("    WHEN AFTER_ODR_ACT_STATUS = '1' THEN ")
            .Append("    '1' ")
            .Append("    ELSE ")
            .Append("    '0' ")
            .Append("    END AS COMPLETION_FLG, ")
            .Append("    CASE ")
            .Append("    WHEN T4.AFTER_ODR_ACT_INPUT_TYPE IS NOT NULL THEN AFTER_ODR_ACT_INPUT_TYPE ")
            .Append("    WHEN T4.AFTER_ODR_ACT_INPUT_TYPE IS NULL THEN TO_NCHAR('1') ")
            .Append("    END AS AFTER_ODR_ACT_INPUT_TYPE, ")
            .Append("    T5.ICON_PATH AS ICON_PATH_CONTACT_MTD, ")
            .Append("    T6.ICON_PATH AS ICON_PATH_AFTER_ODR_PRCS_CD, ")
            .Append("    T3.AFTER_ODR_PRCS_TYPE, ")
            .Append("    T2.STD_VOLUNTARYINS_ACT_TYPE ")
            .Append(" FROM ")
            .Append("    TB_T_AFTER_ODR T1, ")
            .Append("    TB_T_AFTER_ODR_ACT T2, ")
            .Append("    TB_M_AFTER_ODR_PROC T3, ")
            .Append("    TB_M_AFTER_ODR_ACT T4, ")
            .Append("    TB_M_IMG_PATH_CONTROL T5, ")
            .Append("    TB_M_IMG_PATH_CONTROL T6, ")
            .Append("    TB_M_WORD T7, ")
            .Append("    TB_M_WORD T8 ")
            .Append(" WHERE ")
            .Append("    T1.SALES_ID = :SALES_ID ")
            .Append("    AND T2.AFTER_ODR_ID = T1.AFTER_ODR_ID ")
            .Append("    AND T3.AFTER_ODR_PRCS_CD = T2.AFTER_ODR_PRCS_CD ")
            .Append("    AND T4.AFTER_ODR_ACT_CD(+) = T2.AFTER_ODR_ACT_CD ")
            .Append("    AND T5.TYPE_CD = 'CONTACT_MTD' ")
            .Append("    AND T5.DEVICE_TYPE = '01' ")
            .Append("    AND T5.FIRST_KEY = DECODE(T2.AFTER_ODR_ACT_STATUS,'1',RSLT_CONTACT_MTD,SCHE_CONTACT_MTD) ")
            .Append("    AND T5.SECOND_KEY = '00' ")
            .Append("    AND T6.TYPE_CD = 'AFTER_ODR_PRCS_CD' ")
            .Append("    AND T6.DEVICE_TYPE = '01' ")
            .Append("    AND T6.FIRST_KEY = T2.AFTER_ODR_PRCS_CD ")
            .Append("    AND T6.SECOND_KEY = '10' ")
            .Append("    AND T3.AFTER_ODR_PRCS_NAME = T7.WORD_CD(+) ")
            .Append("    AND T4.AFTER_ODR_ACT_NAME = T8.WORD_CD(+) ")
            .Append("    AND T2.SCHE_START_DATEORTIME <> TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
            .Append(" ORDER BY ")
            ' 工程別、日付別でソート対象を変更する
            If mode.Equals("1") Then
                ' 日付順の場合
                .Append("    TRUNC(START_DATEORTIME), ")
                .Append("    DATEORTIME_FLG DESC, ")
                .Append("    START_DATEORTIME, ")
                .Append("    END_DATEORTIME, ")
                .Append("    T3.SORT_ORDER, ")
                .Append("    T2.STD_VOLUNTARYINS_ACT_TYPE, ")
                .Append("    T4.SORT_ORDER, ")
                .Append("    T2.AFTER_ODR_ACT_ID ")
            Else
                ' 工程別の場合
                .Append("    T3.SORT_ORDER, ")
                .Append("    TRUNC(START_DATEORTIME), ")
                .Append("    DATEORTIME_FLG DESC, ")
                .Append("    START_DATEORTIME, ")
                .Append("    END_DATEORTIME, ")
                .Append("    T2.STD_VOLUNTARYINS_ACT_TYPE, ")
                .Append("    T4.SORT_ORDER, ")
                .Append("    T2.AFTER_ODR_ACT_ID ")
            End If
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdracTDataTable)("SC3080216_203")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedafterActivityInfo_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function


    ''' <summary>
    ''' 受注後工程活動更新
    ''' </summary>
    ''' <param name="input">更新データ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateBookedafterActivityInfo(ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveRow) As Integer

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216UpdBookdafActiveDataTable)("SC3080216_204")
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateBookedafterActivityInfo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append(" UPDATE /* SC3080216_204 */ ")
                .Append("    TB_T_AFTER_ODR_ACT T1 ")
                .Append(" SET ")

                ' 引数に応じて更新データを設定する
                If input.AFTER_ODR_ACT_STATUS.Equals("1") And Not input.ACT_ID = 0 Then
                    ' 受注時登録の場合
                    .Append("    T1.AFTER_ODR_ACT_STATUS = '1', ")
                    .Append("    T1.ACT_ID = :ACT_ID, ")
                    .Append("    T1.RSLT_DATEORTIME_FLG = :RSLT_DATEORTIME_FLG, ")
                    .Append("    T1.RSLT_START_DATEORTIME = :RSLT_START_DATEORTIME, ")
                    .Append("    T1.RSLT_END_DATEORTIME = :RSLT_END_DATEORTIME, ")
                    .Append("    T1.RSLT_CONTACT_MTD = :RSLT_CONTACT_MTD, ")
                    .Append("    T1.RSLT_DLR_CD = :RSLT_DLR_CD, ")
                    .Append("    T1.RSLT_BRN_CD = :RSLT_BRN_CD, ")
                    .Append("    T1.RSLT_ORGNZ_ID = :RSLT_ORGNZ_ID, ")
                    .Append("    T1.RSLT_STF_CD = :RSLT_STF_CD, ")

                ElseIf input.AFTER_ODR_ACT_STATUS.Equals("1") And Not input.AFTER_ODR_FLLW_SEQ = 0 Then

                    '受注後登録の場合
                    .Append("    T1.AFTER_ODR_ACT_STATUS = '1', ")
                    .Append("    T1.AFTER_ODR_FLLW_SEQ = :BOOKED_AFTER_FLLW_SEQ, ")
                    .Append("    T1.RSLT_DATEORTIME_FLG = :RSLT_DATEORTIME_FLG, ")
                    .Append("    T1.RSLT_START_DATEORTIME = :RSLT_START_DATEORTIME, ")
                    .Append("    T1.RSLT_END_DATEORTIME = :RSLT_END_DATEORTIME, ")
                    .Append("    T1.RSLT_CONTACT_MTD = :RSLT_CONTACT_MTD, ")
                    .Append("    T1.RSLT_DLR_CD = :RSLT_DLR_CD, ")
                    .Append("    T1.RSLT_BRN_CD = :RSLT_BRN_CD, ")
                    .Append("    T1.RSLT_ORGNZ_ID = :RSLT_ORGNZ_ID, ")
                    .Append("    T1.RSLT_STF_CD = :RSLT_STF_CD, ")

                ElseIf input.AFTER_ODR_ACT_STATUS.Equals("0") Then

                    '完了済みを未完了へ変更
                    .Append("    T1.AFTER_ODR_ACT_STATUS = '0', ")
                    .Append("    T1.ACT_ID = 0, ")
                    .Append("    T1.AFTER_ODR_FLLW_SEQ = 0, ")
                    .Append("    T1.RSLT_DATEORTIME_FLG = '0', ")
                    .Append("    T1.RSLT_START_DATEORTIME = TO_DATE('1900/01/01','YYYY/MM/DD'), ")
                    .Append("    T1.RSLT_END_DATEORTIME = TO_DATE('1900/01/01','YYYY/MM/DD'), ")
                    .Append("    T1.RSLT_CONTACT_MTD = ' ', ")
                    .Append("    T1.RSLT_DLR_CD = ' ', ")
                    .Append("    T1.RSLT_BRN_CD = ' ', ")
                    .Append("    T1.RSLT_ORGNZ_ID = 0, ")
                    .Append("    T1.RSLT_STF_CD = ' ', ")
                End If
                .Append(" T1.ROW_UPDATE_DATETIME = SYSDATE, ")
                .Append(" T1.ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                .Append(" T1.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                .Append(" T1.ROW_LOCK_VERSION = T1.ROW_LOCK_VERSION + 1 ")
                .Append("WHERE ")
                .Append("    T1.AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")

            End With
            query.CommandText = sql.ToString()
            If input.AFTER_ODR_ACT_STATUS.Equals("1") And Not input.ACT_ID = 0 Then
                query.AddParameterWithTypeValue("ACT_ID", OracleDbType.Decimal, input.ACT_ID)
            ElseIf input.AFTER_ODR_ACT_STATUS.Equals("1") And Not input.AFTER_ODR_FLLW_SEQ = 0 Then
                query.AddParameterWithTypeValue("BOOKED_AFTER_FLLW_SEQ", OracleDbType.Decimal, input.AFTER_ODR_FLLW_SEQ)
            End If
            If input.AFTER_ODR_ACT_STATUS.Equals("1") Then
                query.AddParameterWithTypeValue("RSLT_DATEORTIME_FLG", OracleDbType.NVarchar2, input.RSLT_DATEORTIME_FLG)
                query.AddParameterWithTypeValue("RSLT_START_DATEORTIME", OracleDbType.Date, input.RSLT_START_DATEORTIME)
                query.AddParameterWithTypeValue("RSLT_END_DATEORTIME", OracleDbType.Date, input.RSLT_END_DATEORTIME)
                query.AddParameterWithTypeValue("RSLT_CONTACT_MTD", OracleDbType.NVarchar2, input.RSLT_CONTACT_MTD)
                query.AddParameterWithTypeValue("RSLT_DLR_CD", OracleDbType.NVarchar2, input.RSLT_DLR_CD)
                query.AddParameterWithTypeValue("RSLT_BRN_CD", OracleDbType.NVarchar2, input.RSLT_BRN_CD)
                query.AddParameterWithTypeValue("RSLT_ORGNZ_ID", OracleDbType.Decimal, input.RSLT_ORGNZ_ID)
                query.AddParameterWithTypeValue("RSLT_STF_CD", OracleDbType.NVarchar2, input.RSLT_STF_CD)

            End If
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_ID", OracleDbType.Decimal, input.AFTER_ODR_ACT_ID)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, input.ROW_UPDATE_ACCOUNT)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, input.ROW_UPDATE_FUNCTION)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateBookedafterActivityInfo_End")
            'ログ出力 End *****************************************************************************

            Return query.GetCount()


        End Using


    End Function

    ''' <summary>
    ''' 受注後活動紐付更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="input">更新データ</param>
    ''' <param name="prcsCDContract">契約活動コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateLinkBookedafterActivityInfo(ByVal salesId As Decimal, ByVal input As SC3080216DataSet.SC3080216UpdBookdafActiveRow,
                                                             ByVal prcsCDContract As String) As Integer

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216UpdBookdafActiveDataTable)("SC3080216_205")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateLinkBookedafterActivityInfo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append(" UPDATE ")
                .Append(" /* SC3080216_205 */ ")
                .Append("    TB_T_AFTER_ODR_ACT T1 ")
                .Append(" SET ")
                If Not input.ACT_ID = 0 Then
                    ' 活動IDが存在する場合、更新する
                    .Append("    T1.ACT_ID = :ACT_ID, ")
                ElseIf Not input.AFTER_ODR_FLLW_SEQ = 0 Then
                    ' 受注後工程フォロー結果連番が存在する場合、更新する
                    .Append("    T1.AFTER_ODR_FLLW_SEQ = :AFTER_ODR_FLLW_SEQ, ")
                End If
                .Append("    T1.ROW_UPDATE_DATETIME = SYSDATE, ")
                .Append("    T1.ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                .Append("    T1.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                .Append("    T1.ROW_LOCK_VERSION = T1.ROW_LOCK_VERSION + 1 ")
                .Append(" WHERE ")
                .Append("    (T1.ACT_ID = 0 ")
                .Append("    AND T1.AFTER_ODR_FLLW_SEQ = 0) ")
                .Append("    AND T1.AFTER_ODR_ID = ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            T2.AFTER_ODR_ID ")
                .Append("        FROM ")
                .Append("            TB_T_AFTER_ODR T2 ")
                .Append("        WHERE ")
                .Append("            T2.SALES_ID = :SALES_ID ")
                .Append("        ) ")
                .Append("    AND T1.AFTER_ODR_ACT_STATUS = '1' ")
                .Append("    AND T1.AFTER_ODR_ACT_CD <> :AFTER_ODR_ACT_CD ")
            End With
            query.CommandText = sql.ToString()
            If Not input.ACT_ID = 0 Then
                ' 活動IDが存在する場合、更新する
                query.AddParameterWithTypeValue("ACT_ID", OracleDbType.Decimal, input.ACT_ID)
            ElseIf Not input.AFTER_ODR_FLLW_SEQ = 0 Then
                ' 受注後工程フォロー結果連番が存在する場合、更新する
                query.AddParameterWithTypeValue("AFTER_ODR_FLLW_SEQ", OracleDbType.Decimal, input.AFTER_ODR_FLLW_SEQ)
            End If
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, prcsCDContract)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, input.ROW_UPDATE_ACCOUNT)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, input.ROW_UPDATE_FUNCTION)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateLinkBookedafterActivityInfo_End")
            'ログ出力 End *****************************************************************************

            Return query.GetCount()

        End Using

    End Function


    ''' <summary>
    ''' 受注後工程必須活動未完了件数取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountMandatoryBookedAfterProcess(ByVal salesId As Decimal) As Decimal

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCountMandatoryBookedAfterProcess_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080202_206 */ ")
            .Append("     COUNT(1) ")
            .Append(" FROM ")
            .Append("     TB_M_AFTER_ODR_PROC T1, ")
            .Append("     TB_M_AFTER_ODR_ACT T2, ")
            .Append("     TB_T_AFTER_ODR T3, ")
            .Append("     TB_T_AFTER_ODR_ACT T4 ")
            .Append(" WHERE ")
            .Append("     T3.SALES_ID = :SALES_ID ")
            .Append("     AND T1.AFTER_ODR_PRCS_CD = T2.AFTER_ODR_PRCS_CD ")
            .Append("     AND T2.MANDATORY_ACT_FLG = '1'")
            .Append("     AND T2.AFTER_ODR_ACT_CD = T4.AFTER_ODR_ACT_CD ")
            .Append("     AND T4.AFTER_ODR_ACT_STATUS <> '1' ")
            .Append("    AND T3.AFTER_ODR_ID = T4.AFTER_ODR_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdracTDataTable)("SC3080202_206")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCountMandatoryBookedAfterProcess_End")
            'ログ出力 End *****************************************************************************

            Return query.GetCount()

        End Using

    End Function

    ''' <summary>
    ''' 受注後ロック取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <remarks></remarks>
    Public Shared Function GetLockAfterOdr(ByVal salesId As Decimal) As SC3080216DataSet.SC3080216GetAfterActDataTable

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append(" /* SC3080216_207 */ ")
            .Append("    T1.AFTER_ODR_ID ")
            .Append(" FROM ")
            .Append("    TB_T_AFTER_ODR T1 ")
            .Append("WHERE ")
            .Append("    T1.SALES_ID = :SALES_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216GetAfterActDataTable)("SC3080216_207")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)        '商談ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdr_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' 受注後活動ロック取得
    ''' </summary>
    ''' <param name="afterOdrid">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetLockAfterOdrAct(ByVal afterOdrid As Decimal)

        Dim env As New SystemEnvSetting
        Dim sql As New StringBuilder
        Dim sqlForUpdata As String = "FOR UPDATE WAIT " + env.GetLockWaitTime()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLockAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append(" /* SC3080216_208 */ ")
            .Append(" 1 ")
            .Append(" FROM ")
            .Append("   TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("   T1.AFTER_ODR_ID = :AFTER_ODR_ID ")
            .Append(sqlForUpdata)
        End With

        Using query As New DBUpdateQuery("SC3080216_208")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrid)        '商談ID

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
    ''' <param name="actAccount">アカウント</param>
    ''' <param name="actFunction">画面ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOdr(ByVal salesId As Decimal, ByVal actAccount As String, ByVal actFunction As String)

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT /* SC3080216_209 */ ")
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
            .Append("    ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append("  ( ")
            .Append("        SELECT ")
            .Append("            T1.AFTER_ODR_ID, ")
            .Append("            T1.SALES_ID, ")
            .Append("            T1.DLR_CD, ")
            .Append("            T1.SALESBKG_NUM, ")
            .Append("            T1.DELI_SCHE_TERM_YEARMONTH, ")
            .Append("            T1.DELI_SCHE_TERM_WEEKLY, ")
            .Append("            T1.TENTATIVE_DELI_SCHE_DATE_FLG, ")
            .Append("            T1.MODEL_CD, ")
            .Append("            T1.REMAINDER_AMOUNT, ")
            .Append("            SYSDATE, ")
            .Append("            :ACCOUNT, ")
            .Append("            :FUNCTION, ")
            .Append("            SYSDATE, ")
            .Append("            :ACCOUNT, ")
            .Append("            :FUNCTION, ")
            .Append("            0 ")
            .Append("        FROM ")
            .Append("            TB_T_AFTER_ODR T1 ")
            .Append("        WHERE ")
            .Append("            T1.SALES_ID = :SALES_ID ")
            .Append(" ) ")
        End With

        Using query As New DBUpdateQuery("SC3080216_209")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)        '商談ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, actAccount)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdr_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub


    ''' <summary>
    ''' 受注後活動History移行
    ''' </summary>
    ''' <param name="afterOdrid">受注後ID</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="actFunction">画面ID</param>
    ''' <remarks></remarks>
    Public Shared Sub MoveHistoryAfterOdrAct(ByVal afterOdrid As Decimal, ByVal account As String, ByVal actFunction As String)

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveHistoryAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT /* SC3080216_210 */ ")
            .Append("INTO ")
            .Append("    TB_H_AFTER_ODR_ACT T2 ")
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
            .Append("    TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID ")

        End With

        Using query As New DBUpdateQuery("SC3080216_210")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrid)        '受注後ID
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("FUNCTION", OracleDbType.NVarchar2, actFunction)

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

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdr_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE /* SC3080216_211 */")
            .Append("FROM ")
            .Append("    TB_T_AFTER_ODR T1 ")
            .Append("WHERE ")
            .Append("    T1.SALES_ID = :SALES_ID ")
        End With

        Using query As New DBUpdateQuery("SC3080216_211")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)        '商談ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdr_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub


    ''' <summary>
    ''' 受注後活動削除
    ''' </summary>
    ''' <param name="afterOdrid">受注後ID</param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteAfterOdrAct(ByVal afterOdrid As Decimal)

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdrAct_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE /* SC3080216_212 */")
            .Append("FROM ")
            .Append("   TB_T_AFTER_ODR_ACT T1 ")
            .Append("WHERE ")
            .Append("    T1.AFTER_ODR_ID = :AFTER_ODR_ID")
        End With

        Using query As New DBUpdateQuery("SC3080216_211")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("AFTER_ODR_ID", OracleDbType.Decimal, afterOdrid)        '商談ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteAfterOdrAct_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using

    End Sub


    ''' <summary>
    ''' 接触方法名取得
    ''' </summary>
    ''' <param name="contactMtd">コンタクト方法</param>
    ''' <returns>コンタクト名</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactName(ByVal contactMtd As String) As SC3080216DataSet.SC3080216AfterOdrContactDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactName_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080216_213 */")
            .Append("    T1.CONTACT_NAME ")
            .Append(" FROM ")
            .Append("    TB_M_CONTACT_MTD T1 ")
            .Append(" WHERE ")
            .Append("    T1.CONTACT_MTD = :CONTACT_MTD ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdrContactDataTable)("SC3080216_213")

            query.CommandText = sql.ToString()

            'コンタクト方法
            query.AddParameterWithTypeValue("CONTACT_MTD", OracleDbType.NVarchar2, contactMtd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactName_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' 受注時活動ID取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActId(ByVal salesId As Decimal) As SC3080216DataSet.SC3080216ActIdDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActId_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080216_214 */")
            .Append("    T1.ACT_ID ")
            .Append(" FROM ")
            .Append("    TB_H_ACTIVITY T1, ")
            .Append("    TB_H_SALES T2 ")
            .Append(" WHERE ")
            .Append("    T1.REQ_ID = T2.REQ_ID ")
            .Append("    AND T1.ATT_ID = T2.ATT_ID ")
            .Append("    AND T2.SALES_ID = :SALES_ID ")
            .Append("    ORDER BY ACT_COUNT DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216ActIdDataTable)("SC3080216_214")

            query.CommandText = sql.ToString()

            '商談ID
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetActId_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function


    ''' <summary>
    ''' 受注後活動CalDAV用情報取得
    ''' </summary>
    ''' <param name="afterOdrActId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOrderProc(ByVal afterOdrActId As Decimal) As SC3080216DataSet.SC3080216AfterOdrProcDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAfterOrderProc_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080216_215 */")
            .Append("  T1.SCHE_START_DATEORTIME, ")
            .Append("  T1.SCHE_END_DATEORTIME, ")
            .Append("  T1.SCHE_DATEORTIME_FLG, ")
            .Append("  T1.SCHE_CONTACT_MTD, ")
            .Append("  T1.SCHE_BRN_CD, ")
            .Append("  T1.SCHE_STF_CD, ")
            .Append("  CASE ")
            .Append("  WHEN T4.AFTER_ODR_ACT_CD IS NOT NULL THEN T4.AFTER_ODR_ACT_CD ")
            .Append("  WHEN T4.AFTER_ODR_ACT_CD IS NULL THEN TO_NCHAR('0') ")
            .Append("  END AS AFTER_ODR_ACT_CD, ")
            .Append("  T1.AFTER_ODR_PRCS_CD, ")
            .Append("  T2.AFTER_ODR_PRCS_TYPE, ")
            .Append("  CASE WHEN T1.VOLUNTARYINS_ACT_NAME <> ' ' THEN VOLUNTARYINS_ACT_NAME ")
            .Append("       WHEN T3.WORD_VAL IS NULL THEN NULL ")
            .Append("       WHEN T3.WORD_VAL = ' ' THEN TRIM(T3.WORD_VAL_ENG) ")
            .Append("  ELSE TRIM(T3.WORD_VAL) ")
            .Append("  END AS AFTER_ODR_ACT_NAME, ")
            '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 START
            .Append("  CASE ")
            .Append("  WHEN T4.SC_RSLT_INPUT_FLG IS NOT NULL THEN T4.SC_RSLT_INPUT_FLG ")
            .Append("  WHEN T4.SC_RSLT_INPUT_FLG IS NULL THEN TO_NCHAR('0') ")
            .Append("  END AS SC_RSLT_INPUT_FLG, ")
            .Append("  T1.STD_VOLUNTARYINS_ACT_TYPE ")
            '2014/12/25 TCS 外崎 車両ステイタス連携機能各国展開に向けた追加機能開発 END
            .Append(" FROM ")
            .Append("  TB_T_AFTER_ODR_ACT T1, ")
            .Append("  TB_M_AFTER_ODR_PROC T2, ")
            .Append("  TB_M_WORD T3, ")
            .Append("  TB_M_AFTER_ODR_ACT T4 ")
            .Append(" WHERE ")
            .Append("  T1.AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
            .Append("  AND T1.AFTER_ODR_PRCS_CD = T2.AFTER_ODR_PRCS_CD ")
            .Append("  AND T1.AFTER_ODR_ACT_CD = T4.AFTER_ODR_ACT_CD(+) ")
            .Append("  AND T4.AFTER_ODR_ACT_NAME = T3.WORD_CD(+) ")
            .Append("  AND T1.SCHE_START_DATEORTIME <> TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdrProcDataTable)("SC3080216_215")

            query.CommandText = sql.ToString()

            '受注後活動ID
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_ID", OracleDbType.Decimal, afterOdrActId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAfterOrderProc_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function


    ''' <summary>
    ''' 顧客名称取得
    ''' </summary>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCstName(ByVal cstId As Decimal) As SC3080216DataSet.SC3080216AfterOdrCstInfoDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCstName_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080216_216 */")
            .Append("  DMS_CST_CD, ")
            .Append("  CST_NAME ")
            .Append(" FROM ")
            .Append("  TB_M_CUSTOMER ")
            .Append(" WHERE ")
            .Append("    CST_ID = :CST_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdrCstInfoDataTable)("SC3080216_216")

            query.CommandText = sql.ToString()

            '顧客ID
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCstName_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' ToDoチップ色情報取得
    ''' </summary>
    ''' <param name="processcd">工程コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetToDoColor(ByVal processcd As String) As SC3080216DataSet.SC3080216AfterOdrToDoColorDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetToDoColor_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT /* SC3080216_217 */")
            .Append("  T1.BACKGROUNDCOLOR ")
            .Append(" FROM ")
            .Append("  TBL_TODO_TIP_COLOR T1 ")
            .Append(" WHERE ")
            .Append("  T1.DLRCD = 'XXXXX' ")
            .Append("  AND T1.CREATEDATADIV = '1' ")
            .Append("  AND T1.SCHEDULEDVS = '2' ")
            .Append("  AND T1.NEXTACTIONDVS = 'X' ")
            .Append("  AND T1.CONTACTNO = 0 ")
            .Append("  AND T1.PROCESSCD = :PROCESSCD ")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdrToDoColorDataTable)("SC3080216_217")

            query.CommandText = sql.ToString()

            '工程コード
            query.AddParameterWithTypeValue("PROCESSCD", OracleDbType.Char, processcd.PadRight(3))

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetToDoColor_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ''' <summary>
    ''' 未完了受注後活動取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterActCalDav(ByVal salesId As Decimal) As SC3080216DataSet.SC3080216AfterOdrActCalDAVDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAfterActCalDav_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append(" SELECT /* SC3080216_218 */")
            .Append("  T1.AFTER_ODR_ACT_ID ")
            .Append(" FROM ")
            .Append("  TB_T_AFTER_ODR_ACT T1, ")
            .Append("  TB_T_AFTER_ODR T2 ")
            .Append(" WHERE ")
            .Append("  T2.SALES_ID = :SALES_ID ")
            .Append("  AND T1.AFTER_ODR_ID = T2.AFTER_ODR_ID ")
            .Append("  AND T1.AFTER_ODR_ACT_STATUS <> '1' ")
            .Append("  AND T1.SCHE_START_DATEORTIME <> TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS')")
        End With

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216AfterOdrActCalDAVDataTable)("SC3080216_218")

            query.CommandText = sql.ToString()

            '商談ID
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAfterActCalDav_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function


    ''' <summary>
    ''' 書類回収活動更新
    ''' </summary>
    ''' <param name="afterOdrDocument">書類回収活動情報</param>
    ''' <remarks></remarks>
    Public Shared Sub UpdAfterOdrDocument(ByVal afterOdrDocument As SC3080216DataSet.SC3080216UpdAfterOdrDocumentDataTable)

        Using query As New DBUpdateQuery("SC3080216_219")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdAfterOdrDocument_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append(" UPDATE ")
                .Append(" /* SC3080216_219 */ ")
                .Append("  TB_T_AFTER_ODR_NEED_DOC T1 ")
                .Append(" SET ")
                If afterOdrDocument.Item(0).AFTER_ODR_ACT_STATUS = 1 Then
                    '受注後活動ステータスが完了の場合
                    .Append("  T1.ARRIVAL_AMOUNT = T1.NEED_AMOUNT, ")
                    .Append("  T1.RSLT_ARRIVAL_DATE = :RSLT_ARRIVAL_DATE, ")
                Else
                    '受注後活動ステータスが未完了の場合
                    .Append("  T1.ARRIVAL_AMOUNT = '0', ")
                    .Append("  T1.RSLT_ARRIVAL_DATE =  TO_DATE('1900/01/01', 'YYYY/MM/DD'), ")
                End If
                .Append(" T1.ROW_LOCK_VERSION = T1.ROW_LOCK_VERSION + 1, ")
                .Append(" T1.ROW_UPDATE_DATETIME = SYSDATE, ")
                .Append(" T1.ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                .Append(" T1.ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .Append(" WHERE ")
                .Append("  T1.AFTER_ODR_ID = ")
                .Append(" (SELECT  ")
                .Append("   T2.AFTER_ODR_ID ")
                .Append("  FROM ")
                .Append("   TB_T_AFTER_ODR_ACT T2 ")
                .Append("  WHERE ")
                .Append("   T2.AFTER_ODR_ACT_ID = :AFTER_ODR_ACT_ID ")
                .Append(" AND T2.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD) ")
                .Append(" AND T1.SCHE_ARRIVAL_DATE = :SCHE_ARRIVAL_DATE ")
                If afterOdrDocument.Item(0).AFTER_ODR_ACT_STATUS = 1 Then
                    .Append(" AND T1.RSLT_ARRIVAL_DATE = TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
                End If
            End With

            query.CommandText = sql.ToString()
            If afterOdrDocument.Item(0).AFTER_ODR_ACT_STATUS = 1 Then
                '受注後活動ステータスが完了の場合
                query.AddParameterWithTypeValue("RSLT_ARRIVAL_DATE", OracleDbType.Date, afterOdrDocument.Item(0).RSLT_SEND_DATE)
            End If
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_ID", OracleDbType.Decimal, afterOdrDocument.Item(0).AFTER_ODR_ACT_ID)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, afterOdrDocument.Item(0).AFTER_ODR_ACT_CD)
            query.AddParameterWithTypeValue("SCHE_ARRIVAL_DATE", OracleDbType.Date, afterOdrDocument.Item(0).SCHE_SEND_DATE)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, afterOdrDocument.Item(0).ROW_UPDATE_ACCOUNT)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, afterOdrDocument.Item(0).ROW_UPDATE_FUNCTION)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdAfterOdrDocument_End")
            'ログ出力 End *****************************************************************************

            query.Execute()

        End Using
    End Sub

    ''' <summary>
    ''' 受注後連番の最新を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterFollowSeqNoMax(ByVal dlrcd As String, ByVal salesId As Decimal) As Decimal

        Using query As New DBSelectQuery(Of SC3080216DataSet.SC3080216BookedAfterFollowSeqNoDataTable)("SC3080216_220")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterFollowSeqNoMax_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append(" SELECT /* SC3080216_220 */ ")
                .Append(" SEQNO ")
                .Append(" FROM TBL_BOOKEDAFTERFOLLOWRSLT　")
                .Append(" WHERE DLRCD = :DLRCD ")
                .Append(" AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append(" ORDER BY SEQNO DESC ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, salesId)

            Dim seqTbl As SC3080216DataSet.SC3080216BookedAfterFollowSeqNoDataTable

            seqTbl = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBookedAfterFollowSeqNoMax_End")
            'ログ出力 End *****************************************************************************

            Return seqTbl.Item(0).SEQNO

        End Using

    End Function

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
    Public Shared Function InsertFllwupboxSalesmemoHis(ByVal folloupseqno As Decimal,
                                                        ByVal crcstid As Decimal, ByVal vclid As Decimal,
                                                        ByVal actid As Decimal, ByVal acount As String) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertFllwupboxSalesmemo_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("INSERT /* SC3080216_204 */ ")
            .Append("     INTO TB_H_ACTIVITY_MEMO ( ")
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
        Using query As New DBUpdateQuery("SC3080216_204")
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


    ' 2013/06/30 TCS TCS 三宅 2013/10対応版　既存流用 START
    ''' <summary>
    ''' ActId取得
    ''' </summary>
    ''' <param name="salesid">商談ID</param>
    ''' <remarks></remarks>
    Public Shared Function GetMaxActId(ByVal salesid As Decimal) As Decimal

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMaxActId")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080216_205 */ ")
            .Append("       NVL( MAX(ACT_ID), 0) ")
            .Append("  FROM TB_H_SALES_ACT ")
            .Append(" WHERE SALES_ID = :SALESEID ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3080216_205")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESEID", OracleDbType.Decimal, salesid)               '商談ID
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetMaxActId")
            'ログ出力 End *****************************************************************************
            Return Decimal.Parse(query.GetData()(0)(0).ToString)
        End Using
    End Function
    ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

End Class
