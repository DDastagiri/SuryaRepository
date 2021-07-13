'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250101DataSet.vb
'─────────────────────────────────────
'機能： 商品訴求メイン（車両）DataSet.vb
'補足： 
'作成： 2014/02/XX NEC 鈴木
'更新： 2014/03/xx NEC 上野
'更新： 2014/04/xx NEC 脇谷
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text
Imports Toyota.eCRB.iCROP.DataAccess.SC3250101
Imports System.Globalization
Imports Oracle.DataAccess.Client

Partial Class SC3250101DataSet

    '2015/04/14 新販売店追加対応 start
    ''' <summary>
    ''' 全販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealer As String = "XXXXX"
    '2015/04/14 新販売店追加対応 end

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' VCL_KATASHIKIの初期値(半角スペース) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_KATASHIKI_SPACE As String = " "

    
#Region "Private変数"

    ''' <summary>
    ''' 型式使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private useFlgKatashiki As Boolean

#End Region

    '''-------------------------------------------------------
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="isUseFlgKatashiki">型式使用フラグ</param>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Public Sub New(ByVal isUseFlgKatashiki As Boolean)

       useFlgKatashiki = isUseFlgKatashiki

    End Sub

    ''' <summary>
    ''' 型式使用フラグを設定する
    ''' </summary>
    ''' <param name="useFlgKatashiki">型式使用フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub SetUseFlgKatashiki(ByVal isUseFlgKatashiki As Boolean)
        useFlgKatashiki = isUseFlgKatashiki
    End Sub

	'2019/07/05　TKM要件:型式対応　END　↑↑↑
	
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 001:商品訴求登録実績データ登録
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ''' <param name="strUpdateaccount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_T_REPAIR_SUGGESTION_RSLT_Insert(
                                                ByVal strDLR_CD As String _
                                              , ByVal strBRN_CD As String _
                                              , ByVal strSTF_CD As String _
                                              , ByVal strSAChipID As String _
                                              , ByVal strSVC_CD As String _
                                              , ByVal strINSPEC_ITEM_CD As String _
                                              , ByVal strSUGGEST_ICON As String _
                                              , ByVal strUpdateaccount As String
                                              ) As Integer

        Dim No As String = "SC3250101_001"
        Dim strMethodName As String = "TB_T_REPAIR_SUGGESTION_RSLT_Insert"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append(" INSERT /* SC3250101_001 */ ")
            .Append(" INTO TB_T_REPAIR_SUGGESTION_RSLT( ")
            .Append("      DLR_CD ")
            .Append("     ,BRN_CD ")
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            '.Append("     ,STF_CD ")
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            .Append("     ,RO_NUM ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("     ,SVC_CD ")
            '.Append("     ,INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("     ,INSPEC_ITEM_CD ")
            .Append("     ,SUGGEST_ICON ")
            .Append("     ,ROW_CREATE_DATETIME ")
            .Append("     ,ROW_CREATE_ACCOUNT ")
            .Append("     ,ROW_CREATE_FUNCTION ")
            .Append("     ,ROW_UPDATE_DATETIME ")
            .Append("     ,ROW_UPDATE_ACCOUNT ")
            .Append("     ,ROW_UPDATE_FUNCTION ")
            .Append("     ,ROW_LOCK_VERSION ")
            .Append(" ) ")
            .Append(" VALUES ")
            .Append(" ( ")
            .Append("      :DLR_CD ")
            .Append("     ,:BRN_CD ")
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            '.Append("     ,:STF_CD ")
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            .Append("     ,:SA_CHIP_ID ")
            .Append("     ,:SVC_CD ")
            .Append("     ,:INSPEC_ITEM_CD ")
            .Append("     ,:SUGGEST_ICON ")
            .Append("     ,SYSDATE ")
            .Append("     ,:Updateaccount ")
            .Append("     ,'SC3250101' ")
            .Append("     ,SYSDATE ")
            .Append("     ,:Updateaccount ")
            .Append("     ,'SC3250101' ")
            .Append("     ,0 ")
            .Append(" ) ")

            '.AppendFormat("INSERT /* {0} */ ", No)
            '.Append("INTO TB_T_REPAIR_SUGGESTION_RSLT( ")
            '.Append("     DLR_CD ")
            '.Append("    ,BRN_CD ")
            '.Append("    ,STF_CD ")
            '.Append("    ,RO_NUM ")
            '.Append("    ,INSPEC_TYPE ")
            '.Append("    ,INSPEC_ITEM_CD ")
            '.Append("    ,SUGGEST_ICON ")
            '.Append("    ,ROW_CREATE_DATETIME ")
            '.Append("    ,ROW_CREATE_ACCOUNT ")
            '.Append("    ,ROW_CREATE_FUNCTION ")
            '.Append("    ,ROW_UPDATE_DATETIME ")
            '.Append("    ,ROW_UPDATE_ACCOUNT ")
            '.Append("    ,ROW_UPDATE_FUNCTION ")
            '.Append("    ,ROW_LOCK_VERSION ")
            '.Append(") ")
            '.Append("VALUES ")
            '.Append("( ")
            '.AppendFormat("     '{0}' ", strDLR_CD)
            '.AppendFormat("    ,'{0}' ", strBRN_CD)
            '.AppendFormat("    ,'{0}' ", strSTF_CD)
            '.AppendFormat("    ,'{0}' ", strRO_NUM)
            '.AppendFormat("    ,'{0}' ", strINSPEC_TYPE)
            '.AppendFormat("    ,'{0}' ", strINSPEC_ITEM_CD)
            '.AppendFormat("    ,'{0}' ", strSUGGEST_ICON)
            '.Append("    ,SYSDATE ")
            '.AppendFormat("    ,'{0}' ", strUpdateaccount)
            '.Append("    ,'SC3250101' ")
            '.Append("    ,SYSDATE ")
            '.AppendFormat("    ,'{0}' ", strUpdateaccount)
            '.Append("    ,'SC3250101' ")
            '.Append("    ,0 ")
            '.Append(") ")
        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            'query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            query.AddParameterWithTypeValue("SUGGEST_ICON", OracleDbType.NVarchar2, strSUGGEST_ICON)
            query.AddParameterWithTypeValue("Updateaccount", OracleDbType.NVarchar2, strUpdateaccount)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 002:商品訴求登録実績データ取得
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks>2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化（一覧を取得する機能を追加するため）</remarks>
    Public Function TB_T_REPAIR_SUGGESTION_RSLT_Select(
                                              ByVal strDLR_CD As String _
                                            , ByVal strBRN_CD As String _
                                            , ByVal strSTF_CD As String _
                                            , ByVal strSAChipID As String _
                                            , Optional ByVal strSVC_CD As String = "" _
                                            , Optional ByVal strINSPEC_ITEM_CD As String = ""
                                            ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable

        Dim No As String = "SC3250101_002"
        Dim strMethodName As String = "TB_T_REPAIR_SUGGESTION_RSLT_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_002 */ ")
            .Append("    DLR_CD ")
            .Append("   ,BRN_CD ")
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            '.Append("   ,STF_CD ")
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            .Append("   ,RO_NUM ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("   ,SVC_CD ")
            '.Append("   ,SVC_CD AS INSPEC_TYPE ")
            '.Append("   ,INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("   ,INSPEC_ITEM_CD ")
            .Append("   ,SUGGEST_ICON ")
            .Append("   ,ROW_CREATE_DATETIME ")
            .Append("   ,ROW_CREATE_ACCOUNT ")
            .Append("   ,ROW_CREATE_FUNCTION ")
            .Append("   ,ROW_UPDATE_DATETIME ")
            .Append("   ,ROW_UPDATE_ACCOUNT ")
            .Append("   ,ROW_UPDATE_FUNCTION ")
            .Append("   ,ROW_LOCK_VERSION ")
            .Append(" FROM  ")
            .Append("  TB_T_REPAIR_SUGGESTION_RSLT ")
            .Append(" WHERE ")
            .Append("       DLR_CD = :DLR_CD ")
            .Append("   AND BRN_CD = :BRN_CD ")
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            '.Append("   AND STF_CD = :STF_CD ")
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            .Append("   AND RO_NUM = :SA_CHIP_ID ")
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　START　↓↓↓
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            If strSVC_CD <> "" Then
                .Append("   AND SVC_CD = :SVC_CD ")
            End If
            '.Append("   AND INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            If strINSPEC_ITEM_CD <> "" Then
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            End If
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　END 　↑↑↑

            .Append(" UNION ALL ")

            .Append("SELECT ")
            .Append("    DLR_CD ")
            .Append("   ,BRN_CD ")
            .Append("   ,RO_NUM ")
            .Append("   ,SVC_CD ")
            .Append("   ,INSPEC_ITEM_CD ")
            .Append("   ,SUGGEST_ICON ")
            .Append("   ,ROW_CREATE_DATETIME ")
            .Append("   ,ROW_CREATE_ACCOUNT ")
            .Append("   ,ROW_CREATE_FUNCTION ")
            .Append("   ,ROW_UPDATE_DATETIME ")
            .Append("   ,ROW_UPDATE_ACCOUNT ")
            .Append("   ,ROW_UPDATE_FUNCTION ")
            .Append("   ,ROW_LOCK_VERSION ")
            .Append(" FROM  ")
            .Append("  TB_H_REPAIR_SUGGESTION_RSLT ")
            .Append(" WHERE ")
            .Append("       DLR_CD = :DLR_CD ")
            .Append("   AND BRN_CD = :BRN_CD ")
            .Append("   AND RO_NUM = :SA_CHIP_ID ")
            If strSVC_CD <> "" Then
                .Append("   AND SVC_CD = :SVC_CD ")
            End If
            If strINSPEC_ITEM_CD <> "" Then
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            End If

        End With


        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            'query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　START　↓↓↓
            If strSVC_CD <> "" Then
                query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            End If
            If strINSPEC_ITEM_CD <> "" Then
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            End If
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　END　　↑↑↑

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 003:商品訴求登録実績データ更新
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_T_REPAIR_SUGGESTION_RSLT_Update(
                                            ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSTF_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strINSPEC_ITEM_CD As String _
                                          , ByVal strSUGGEST_ICON As String _
                                          , ByVal strUpdateaccount As String
                                          ) As Integer

        Dim No As String = "SC3250101_003"
        Dim strMethodName As String = "TB_T_REPAIR_SUGGESTION_RSLT_Update"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("UPDATE /* SC3250101_003 */ ")
            .Append(" TB_T_REPAIR_SUGGESTION_RSLT ")
            .Append("SET ")
            .Append("  SUGGEST_ICON = :SUGGEST_ICON ")
            .Append(" ,ROW_UPDATE_DATETIME = SYSDATE ")
            .Append(" ,ROW_UPDATE_ACCOUNT = :Updateaccount ")
            .Append(" ,ROW_UPDATE_FUNCTION = 'SC3250101' ")
            .Append("WHERE ")
            .Append("      DLR_CD = :DLR_CD ")
            .Append("  AND BRN_CD = :BRN_CD ")
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            '.Append("  AND STF_CD = :STF_CD ")
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            .Append("  AND RO_NUM = :SA_CHIP_ID ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  AND SVC_CD = :SVC_CD ")
            '.Append("  AND INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SUGGEST_ICON", OracleDbType.NVarchar2, strSUGGEST_ICON)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            '2014/05/26 「STF_CD」の消去　START　↓↓↓
            'query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            query.AddParameterWithTypeValue("Updateaccount", OracleDbType.NVarchar2, strUpdateaccount)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 004:商品訴求画面データWK登録
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ''' <param name="strUpdateaccount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_W_REPAIR_SUGGESTION_Insert(
                                            ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSTF_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strINSPEC_ITEM_CD As String _
                                          , ByVal strSUGGEST_ICON As String _
                                          , ByVal strUpdateaccount As String
                                          ) As Integer

        Dim No As String = "SC3250101_004"
        Dim strMethodName As String = "TB_W_REPAIR_SUGGESTION_Insert"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("INSERT /* SC3250101_004 */ ")
            .Append("INTO TB_W_REPAIR_SUGGESTION( ")
            .Append("     DLR_CD ")
            .Append("    ,BRN_CD ")
            .Append("    ,STF_CD ")
            .Append("    ,RO_NUM ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("    ,SVC_CD ")
            '.Append("    ,INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("    ,INSPEC_ITEM_CD ")
            .Append("    ,SUGGEST_ICON ")
            .Append("    ,ROW_CREATE_DATETIME ")
            .Append("    ,ROW_CREATE_ACCOUNT ")
            .Append("    ,ROW_CREATE_FUNCTION ")
            .Append("    ,ROW_UPDATE_DATETIME ")
            .Append("    ,ROW_UPDATE_ACCOUNT ")
            .Append("    ,ROW_UPDATE_FUNCTION ")
            .Append("    ,ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("     :DLR_CD ")
            .Append("    ,:BRN_CD ")
            .Append("    ,:STF_CD ")
            .Append("    ,:SA_CHIP_ID ")
            .Append("    ,:SVC_CD ")
            .Append("    ,:INSPEC_ITEM_CD ")
            .Append("    ,:SUGGEST_ICON ")
            .Append("    ,SYSDATE ")
            .Append("    ,:Updateaccount ")
            .Append("    ,'SC3250101' ")
            .Append("    ,SYSDATE ")
            .Append("    ,:Updateaccount ")
            .Append("    ,'SC3250101' ")
            .Append("    ,0 ")
            .Append(") ")

            '.AppendFormat("INSERT /* {0} */ ", No)
            '.Append("INTO TB_W_REPAIR_SUGGESTION( ")
            '.Append("     DLR_CD ")
            '.Append("    ,BRN_CD ")
            '.Append("    ,STF_CD ")
            '.Append("    ,RO_NUM ")
            '.Append("    ,INSPEC_TYPE ")
            '.Append("    ,INSPEC_ITEM_CD ")
            '.Append("    ,SUGGEST_ICON ")
            '.Append("    ,ROW_CREATE_DATETIME ")
            '.Append("    ,ROW_CREATE_ACCOUNT ")
            '.Append("    ,ROW_CREATE_FUNCTION ")
            '.Append("    ,ROW_UPDATE_DATETIME ")
            '.Append("    ,ROW_UPDATE_ACCOUNT ")
            '.Append("    ,ROW_UPDATE_FUNCTION ")
            '.Append("    ,ROW_LOCK_VERSION ")
            '.Append(") ")
            '.Append("VALUES ")
            '.Append("( ")
            '.AppendFormat("     '{0}' ", strDLR_CD)
            '.AppendFormat("    ,'{0}' ", strBRN_CD)
            '.AppendFormat("    ,'{0}' ", strSTF_CD)
            '.AppendFormat("    ,'{0}' ", strRO_NUM)
            '.AppendFormat("    ,'{0}' ", strINSPEC_TYPE)
            '.AppendFormat("    ,'{0}' ", strINSPEC_ITEM_CD)
            '.AppendFormat("    ,'{0}' ", strSUGGEST_ICON)
            '.Append("    ,SYSDATE ")
            '.AppendFormat("    ,'{0}' ", strUpdateaccount)
            '.Append("    ,'SC3250101' ")
            '.Append("    ,SYSDATE ")
            '.AppendFormat("    ,'{0}' ", strUpdateaccount)
            '.Append("    ,'SC3250101' ")
            '.Append("    ,0 ")
            '.Append(") ")
        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            query.AddParameterWithTypeValue("SUGGEST_ICON", OracleDbType.NVarchar2, strSUGGEST_ICON)
            query.AddParameterWithTypeValue("Updateaccount", OracleDbType.NVarchar2, strUpdateaccount)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 005:商品訴求画面データWK取得
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks>2014/07/08 strINSPEC_ITEM_CDをオプション化（WKに保存されている一覧を取得する機能を追加するため）</remarks>
    Public Function TB_W_REPAIR_SUGGESTION_Select(
                                              ByVal strDLR_CD As String _
                                            , ByVal strBRN_CD As String _
                                            , ByVal strSTF_CD As String _
                                            , ByVal strSAChipID As String _
                                            , Optional ByVal strSVC_CD As String = "" _
                                            , Optional ByVal strINSPEC_ITEM_CD As String = ""
                                            ) As SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable

        Dim No As String = "SC3250101_005"
        Dim strMethodName As String = "TB_W_REPAIR_SUGGESTION_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql

            .Append("SELECT /* SC3250101_005 */ ")
            .Append("   DLR_CD ")
            .Append("  ,BRN_CD ")
            .Append("  ,STF_CD ")
            .Append("  ,RO_NUM ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  ,SVC_CD ")
            '.Append("  ,SVC_CD AS INSPEC_TYPE ")
            '.Append("  ,INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("  ,INSPEC_ITEM_CD ")
            .Append("  ,SUGGEST_ICON ")
            .Append("  ,ROW_CREATE_DATETIME ")
            .Append("  ,ROW_CREATE_ACCOUNT ")
            .Append("  ,ROW_CREATE_FUNCTION ")
            .Append("  ,ROW_UPDATE_DATETIME ")
            .Append("  ,ROW_UPDATE_ACCOUNT ")
            .Append("  ,ROW_UPDATE_FUNCTION ")
            .Append("  ,ROW_LOCK_VERSION ")
            .Append("FROM ")
            .Append("   TB_W_REPAIR_SUGGESTION ")
            .Append("WHERE ")
            .Append("       DLR_CD = :DLR_CD ")
            .Append("   AND BRN_CD = :BRN_CD ")
            .Append("   AND STF_CD = :STF_CD ")
            .Append("   AND RO_NUM = :SA_CHIP_ID ")
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　START　↓↓↓
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            If strSVC_CD <> "" Then
                .Append("   AND SVC_CD = :SVC_CD ")
            End If
            '.Append("   AND INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            If strINSPEC_ITEM_CD <> "" Then
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            End If
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　END　　↑↑↑

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("   DLR_CD ")
            '.Append("  ,BRN_CD ")
            '.Append("  ,STF_CD ")
            '.Append("  ,RO_NUM ")
            '.Append("  ,INSPEC_TYPE ")
            '.Append("  ,INSPEC_ITEM_CD ")
            '.Append("  ,SUGGEST_ICON ")
            '.Append("  ,ROW_CREATE_DATETIME ")
            '.Append("  ,ROW_CREATE_ACCOUNT ")
            '.Append("  ,ROW_CREATE_FUNCTION ")
            '.Append("  ,ROW_UPDATE_DATETIME ")
            '.Append("  ,ROW_UPDATE_ACCOUNT ")
            '.Append("  ,ROW_UPDATE_FUNCTION ")
            '.Append("  ,ROW_LOCK_VERSION ")
            '.Append("FROM  ")
            '.Append(" TB_W_REPAIR_SUGGESTION ")
            '.Append("WHERE ")
            '.AppendFormat("      DLR_CD = '{0}' ", strDLR_CD)
            '.AppendFormat("  AND BRN_CD = '{0}' ", strBRN_CD)
            '.AppendFormat("  AND STF_CD = '{0}' ", strSTF_CD)
            '.AppendFormat("  AND RO_NUM = '{0}' ", strRO_NUM)
            '.AppendFormat("  AND INSPEC_TYPE = '{0}' ", strINSPEC_TYPE)
            '.AppendFormat("  AND INSPEC_ITEM_CD = '{0}' ", strINSPEC_ITEM_CD)
        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable)(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　START　↓↓↓
            If strSVC_CD <> "" Then
                query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            End If
            If strINSPEC_ITEM_CD <> "" Then
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            End If
            '2014/07/08 strSVC_CDとstrINSPEC_ITEM_CDをオプション化　END　　↑↑↑

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 006:商品訴求画面データWK更新
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_W_REPAIR_SUGGESTION_Update(
                                            ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSTF_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strINSPEC_ITEM_CD As String _
                                          , ByVal strSUGGEST_ICON As String _
                                          , ByVal strUpdateaccount As String
                                          ) As Integer

        Dim No As String = "SC3250101_006"
        Dim strMethodName As String = "TB_W_REPAIR_SUGGESTION_Update"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("UPDATE /* SC3250101_006 */ ")
            .Append("  TB_W_REPAIR_SUGGESTION ")
            .Append("SET ")
            .Append("  SUGGEST_ICON = :SUGGEST_ICON ")
            .Append(" ,ROW_UPDATE_DATETIME = SYSDATE ")
            .Append(" ,ROW_UPDATE_ACCOUNT = :Updateaccount ")
            .Append(" ,ROW_UPDATE_FUNCTION = 'SC3250101' ")
            .Append("WHERE ")
            .Append("      DLR_CD = :DLR_CD ")
            .Append("  AND BRN_CD = :BRN_CD ")
            .Append("  AND STF_CD = :STF_CD ")
            .Append("  AND RO_NUM = :SA_CHIP_ID ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  AND SVC_CD = :SVC_CD ")
            '.Append("  AND INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")

            '.AppendFormat("UPDATE /* {0} */ ", No)
            '.Append(" TB_W_REPAIR_SUGGESTION ")
            '.Append("SET ")
            '.AppendFormat("  SUGGEST_ICON = '{0}' ", strSUGGEST_ICON)
            '.Append(" ,ROW_UPDATE_DATETIME = SYSDATE ")
            '.Append("WHERE ")
            '.AppendFormat("      DLR_CD = '{0}' ", strDLR_CD)
            '.AppendFormat("  AND BRN_CD = '{0}' ", strBRN_CD)
            '.AppendFormat("  AND STF_CD = '{0}' ", strSTF_CD)
            '.AppendFormat("  AND RO_NUM = '{0}' ", strRO_NUM)
            '.AppendFormat("  AND INSPEC_TYPE = '{0}' ", strINSPEC_TYPE)
            '.AppendFormat("  AND INSPEC_ITEM_CD = '{0}' ", strINSPEC_ITEM_CD)
        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SUGGEST_ICON", OracleDbType.NVarchar2, strSUGGEST_ICON)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)
            query.AddParameterWithTypeValue("Updateaccount", OracleDbType.NVarchar2, strUpdateaccount)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 007:商品訴求画面データWK削除
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_W_REPAIR_SUGGESTION_Delete(
                                            ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSTF_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strINSPEC_ITEM_CD As String _
                                          ) As Integer

        Dim No As String = "SC3250101_007"
        Dim strMethodName As String = "TB_W_REPAIR_SUGGESTION_Delete"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("DELETE FROM /* SC3250101_007 */ ")
            .Append("  TB_W_REPAIR_SUGGESTION ")
            .Append("WHERE ")
            .Append("      DLR_CD = :DLR_CD ")
            .Append("  AND BRN_CD = :BRN_CD ")
            .Append("  AND STF_CD = :STF_CD ")
            .Append("  AND RO_NUM = :SA_CHIP_ID ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  AND SVC_CD = :SVC_CD ")
            '.Append("  AND INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")

            '.AppendFormat("DELETE FROM /* {0} */ ", No)
            '.Append(" TB_W_REPAIR_SUGGESTION ")
            '.Append("WHERE ")
            '.AppendFormat("      DLR_CD = '{0}' ", strDLR_CD)
            '.AppendFormat("  AND BRN_CD = '{0}' ", strBRN_CD)
            '.AppendFormat("  AND STF_CD = '{0}' ", strSTF_CD)
            '.AppendFormat("  AND RO_NUM = '{0}' ", strRO_NUM)
            '.AppendFormat("  AND INSPEC_TYPE = '{0}' ", strINSPEC_TYPE)
            '.AppendFormat("  AND INSPEC_ITEM_CD = '{0}' ", strINSPEC_ITEM_CD)

        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    ''' <summary>
    ''' 008:モデルマスタ取得
    ''' </summary>
    ''' <param name="strVCL_VIN">VIN</param>
    ''' <returns></returns>
    ''' <remarks>2014/06/11　引数をR/O→VINに変更</remarks>
    ''' 
    Public Function TB_M_MODEL_Select(
                                          ByVal strVCL_VIN As String
                                        ) As SC3250101DataSet.TB_M_MODELDataTable

        Dim No As String = "SC3250101_008"
        Dim strMethodName As String = "TB_M_MODEL_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_M_MODELDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            '2014/06/11 パラメータをVINに変更　START　↓↓↓
            .Append("SELECT /* SC3250101_008 */ ")
            .Append("   NVL(M_VEHICLE.MODEL_CD,' ') AS MODEL_CD ")
            .Append("  ,NVL(M_MODEL.MODEL_NAME,' ') AS MODEL_NAME ")
            .Append("  ,NVL(M_MODEL.MAKER_CD,' ') AS MAKER_CD ")
            .Append("  ,NVL(M_MODEL.COMMON_MODEL_CD,' ') AS COMMON_MODEL_CD ")
            .Append("  ,NVL(M_MODEL.MODEL_PICTURE,' ') AS MODEL_PICTURE ")
            .Append("  ,NVL(M_MODEL.LOGO_PICTURE,' ') AS LOGO_PICTURE ")
            .Append("  ,NVL(M_MODEL.LOGO_PICTURE_SEL,' ') AS LOGO_PICTURE_SEL ")
            .Append("  ,NVL(M_MODEL.AUTOCAR_TYPE,' ') AS AUTOCAR_TYPE ")
            .Append("  ,M_VEHICLE.VCL_ID ")
            '2014/06/12 グレード名→型式に変更　START　↓↓↓
            .Append("  ,M_VEHICLE.VCL_KATASHIKI ")
            '2014/06/12 グレード名→型式に変更　END　　↑↑↑
            .Append("FROM ")
            .Append("   TB_M_VEHICLE M_VEHICLE ")
            .Append("  ,TB_M_MODEL M_MODEL ")
            .Append("WHERE ")
            .Append("      M_VEHICLE.VCL_VIN = :VCL_VIN ")
            .Append("  AND M_VEHICLE.MODEL_CD = M_MODEL.MODEL_CD(+) ")

            '.Append("SELECT /* SC3250101_008 */ ")
            '.Append("   NVL(M_VEHICLE.MODEL_CD,' ') AS MODEL_CD ")
            '.Append("  ,NVL(M_MODEL.MODEL_NAME,' ') AS MODEL_NAME ")
            '.Append("  ,NVL(M_MODEL.MAKER_CD,' ') AS MAKER_CD ")
            '.Append("  ,NVL(M_MODEL.COMMON_MODEL_CD,' ') AS COMMON_MODEL_CD ")
            '.Append("  ,NVL(M_MODEL.MODEL_PICTURE,' ') AS MODEL_PICTURE ")
            '.Append("  ,NVL(M_MODEL.LOGO_PICTURE,' ') AS LOGO_PICTURE ")
            '.Append("  ,NVL(M_MODEL.LOGO_PICTURE_SEL,' ') AS LOGO_PICTURE_SEL ")
            '.Append("  ,NVL(M_MODEL.AUTOCAR_TYPE,' ') AS AUTOCAR_TYPE ")
            '.Append("  ,T_SERVICEIN.VCL_ID ")
            '.Append("FROM ")
            '.Append("   TB_T_RO_INFO T_RO_INFO ")
            '.Append("  ,TB_T_SERVICEIN T_SERVICEIN ")
            '.Append("  ,TB_M_VEHICLE M_VEHICLE ")
            '.Append("  ,TB_M_MODEL M_MODEL ")
            '.Append("WHERE ")
            '.Append("      T_RO_INFO.RO_NUM = :RO_NUM ")
            '.Append("  AND T_RO_INFO.RO_NUM = T_SERVICEIN.RO_NUM ")
            '.Append("  AND T_SERVICEIN.VCL_ID = M_VEHICLE.VCL_ID(+) ")
            '.Append("  AND M_VEHICLE.MODEL_CD = M_MODEL.MODEL_CD(+) ")

            '2014/06/11 パラメータをVINに変更　END　↑↑↑

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("   M_VEHICLE.MODEL_CD ")
            '.Append("  ,M_MODEL.MODEL_NAME ")
            '.Append("  ,M_MODEL.MAKER_CD ")
            '.Append("  ,M_MODEL.COMMON_MODEL_CD ")
            '.Append("  ,M_MODEL.MODEL_PICTURE ")
            '.Append("  ,M_MODEL.LOGO_PICTURE ")
            '.Append("  ,M_MODEL.LOGO_PICTURE_SEL ")
            '.Append("  ,M_MODEL.AUTOCAR_TYPE ")
            '.Append("  ,M_MODEL.DMS_TAKEIN_DATETIME ")
            '.Append("  ,M_MODEL.INUSE_FLG ")
            '.Append("  ,M_MODEL.ROW_CREATE_DATETIME ")
            '.Append("  ,M_MODEL.ROW_CREATE_ACCOUNT ")
            '.Append("  ,M_MODEL.ROW_CREATE_FUNCTION ")
            '.Append("  ,M_MODEL.ROW_UPDATE_DATETIME ")
            '.Append("  ,M_MODEL.ROW_UPDATE_ACCOUNT ")
            '.Append("  ,M_MODEL.ROW_UPDATE_FUNCTION ")
            '.Append("  ,M_MODEL.ROW_LOCK_VERSION ")
            '.Append("FROM  ")
            '.Append("  TB_T_RO_INFO T_RO_INFO ")
            '.Append(" ,TB_T_SERVICEIN T_SERVICEIN ")
            '.Append(" ,TB_M_VEHICLE M_VEHICLE ")
            '.Append(" ,TB_M_MODEL M_MODEL ")
            '.Append("WHERE ")
            '.AppendFormat("      T_RO_INFO.RO_NUM = '{0}' ", strRO_NUM)
            '.Append("  AND T_RO_INFO.RO_NUM = T_SERVICEIN.RO_NUM ")
            '.Append("  AND T_SERVICEIN.VCL_ID = M_VEHICLE.VCL_ID(+) ")
            '.Append("  AND M_VEHICLE.MODEL_CD = M_MODEL.MODEL_CD(+) ")
        End With


        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_MODELDataTable)(No)
            query.CommandText = sql.ToString()

            '2014/06/11 パラメータをVINに変更　START　↓↓↓
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, strVCL_VIN)
            'query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRO_NUM)
            '2014/06/11 パラメータをVINに変更　END　↑↑↑

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function

#Region "未使用メソッド"
    '2014/06/09 未使用メソッドのコメント化　START　↓↓↓

    ' ''' <summary>
    ' ''' 車輌マスタ取得
    ' ''' </summary>
    ' ''' <param name="strVCL_VIN">モデルコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    'Public Function TB_M_VEHICLE_Select(
    '                                      ByVal strVCL_VIN As String
    '                                    ) As SC3250101DataSet.TB_M_VEHICLEDataTable

    '    Dim No As String = "SC3250101_009"
    '    Dim strMethodName As String = "TB_M_VEHICLE_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_VEHICLEDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_009 */ ")
    '            .Append("   VCL_ID ")
    '            .Append("  ,VCL_VIN ")
    '            .Append("  ,VCL_VIN_SEARCH_REV ")
    '            .Append("  ,MODEL_CD ")
    '            .Append("  ,NEWCST_MODEL_NAME ")
    '            .Append("  ,NEWCST_MAKER_NAME ")
    '            .Append("  ,VCL_KATASHIKI ")
    '            .Append("  ,GRADE_NAME ")
    '            .Append("FROM ")
    '            .Append("  TB_M_VEHICLE ")
    '            .Append("WHERE ")
    '            .Append("  VCL_VIN = :VCL_VIN ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("   M_VEHICLE.VCL_ID ")
    '            '.Append("  ,M_VEHICLE.VCL_VIN ")
    '            '.Append("  ,M_VEHICLE.VCL_VIN_SEARCH_REV ")
    '            '.Append("  ,M_VEHICLE.MODEL_CD ")
    '            '.Append("  ,M_VEHICLE.NEWCST_MODEL_NAME ")
    '            '.Append("  ,M_VEHICLE.NEWCST_MAKER_NAME ")
    '            '.Append("  ,M_VEHICLE.VCL_KATASHIKI ")
    '            '.Append("  ,M_VEHICLE.GRADE_NAME ")
    '            '.Append("  ,M_VEHICLE.SUFFIX_CD ")
    '            '.Append("  ,M_VEHICLE.ENGINE_CD ")
    '            '.Append("  ,M_VEHICLE.FUEL_TYPE ")
    '            '.Append("  ,M_VEHICLE.BODYCLR_CD ")
    '            '.Append("  ,M_VEHICLE.BODYCLR_NAME ")
    '            '.Append("  ,M_VEHICLE.INTERIORCLR_CD ")
    '            '.Append("  ,M_VEHICLE.MISSION_NAME ")
    '            '.Append("  ,M_VEHICLE.UCAR_WARRANTY_TYPE ")
    '            '.Append("  ,M_VEHICLE.VCL_PROD_YEARMONTH ")
    '            '.Append("  ,M_VEHICLE.VCL_CREATE_DATE ")
    '            '.Append("  ,M_VEHICLE.LINEOFF_DATE ")
    '            '.Append("  ,M_VEHICLE.COMP_DATE ")
    '            '.Append("  ,M_VEHICLE.FIRST_DELI_DATE ")
    '            '.Append("  ,M_VEHICLE.FIRST_REG_DATE ")
    '            '.Append("  ,M_VEHICLE.CPO_TYPE ")
    '            '.Append("  ,M_VEHICLE.DMS_TAKEIN_DATETIME ")
    '            '.Append("  ,M_VEHICLE.UPDATE_FUNCTION_JUDGE ")
    '            '.Append("  ,M_VEHICLE.VCL_VIN_SEARCH ")
    '            '.Append("  ,M_VEHICLE.VCL_KATASHIKI_SEARCH ")
    '            '.Append("  ,M_VEHICLE.ROW_CREATE_DATETIME ")
    '            '.Append("  ,M_VEHICLE.ROW_CREATE_ACCOUNT ")
    '            '.Append("  ,M_VEHICLE.ROW_CREATE_FUNCTION ")
    '            '.Append("  ,M_VEHICLE.ROW_UPDATE_DATETIME ")
    '            '.Append("  ,M_VEHICLE.ROW_UPDATE_ACCOUNT ")
    '            '.Append("  ,M_VEHICLE.ROW_UPDATE_FUNCTION ")
    '            '.Append("  ,M_VEHICLE.ROW_LOCK_VERSION ")
    '            '.Append("FROM  ")
    '            '.Append(" TB_M_VEHICLE M_VEHICLE")
    '            '.Append(" ,TB_M_MODEL M_MODEL")
    '            '.Append("WHERE ")
    '            '.AppendFormat("      M_VEHICLE.MODEL_CD = '{0}' ", strMODEL_CD)
    '            '.Append("  AND M_VEHICLE.MODEL_CD = M_MODEL.MODEL_CD ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_VEHICLEDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, strVCL_VIN)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function

    '2014/06/09 未使用メソッドのコメント化　END　　↑↑↑
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 010:グレードマスタ取得
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_GRADE_Select(
    '                                   ByVal strMODEL_CD As String
    '                                   ) As SC3250101DataSet.TB_M_GRADEDataTable

    '    Dim No As String = "SC3250101_010"
    '    Dim strMethodName As String = "TB_M_GRADE_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_GRADEDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_010 */ ")
    '            .Append("   MODEL_CD ")
    '            .Append("  ,GRADE_CD ")
    '            .Append("  ,GRADE_NAME ")
    '            .Append("  ,INUSE_FLG ")
    '            .Append("FROM ")
    '            .Append("  TB_M_GRADE ")
    '            .Append("WHERE ")
    '            .Append("  MODEL_CD = :MODEL_CD ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("   M_GRADE.MODEL_CD ")
    '            '.Append("  ,M_GRADE.GRADE_CD ")
    '            '.Append("  ,M_GRADE.GRADE_NAME ")
    '            '.Append("  ,M_GRADE.INUSE_FLG ")
    '            '.Append("  ,M_GRADE.ROW_CREATE_DATETIME ")
    '            '.Append("  ,M_GRADE.ROW_CREATE_ACCOUNT ")
    '            '.Append("  ,M_GRADE.ROW_CREATE_FUNCTION ")
    '            '.Append("  ,M_GRADE.ROW_UPDATE_DATETIME ")
    '            '.Append("  ,M_GRADE.ROW_UPDATE_ACCOUNT ")
    '            '.Append("  ,M_GRADE.ROW_UPDATE_FUNCTION ")
    '            '.Append("  ,M_GRADE.ROW_LOCK_VERSION ")
    '            '.Append("FROM  ")
    '            '.Append(" TB_M_GRADE M_GRADE ")
    '            '.Append(" ,TB_M_MODEL M_MODEL ")
    '            '.Append("WHERE ")
    '            '.AppendFormat("      M_MODEL.MODEL_CD = '{0}' ", strMODEL_CD)
    '            '.Append("  AND M_MODEL.MODEL_CD = M_GRADE.MODEL_CD ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_GRADEDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strMODEL_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

    ''' <summary>
    ''' 011:メーカーマスタ取得
    ''' </summary>
    ''' <param name="strMODEL_CD">モデルコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_M_MAKER_Select(
                                          ByVal strMODEL_CD As String
                                        ) As SC3250101DataSet.TB_M_MAKERDataTable

        Dim No As String = "SC3250101_011"
        Dim strMethodName As String = "TB_M_MAKER_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_M_MAKERDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_011 */ ")
            .Append("   M_MAKER.MAKER_CD ")
            .Append("  ,M_MAKER.MAKER_NAME ")
            .Append("  ,M_MAKER.MAKER_TYPE ")
            .Append("FROM ")
            .Append("   TB_M_MAKER M_MAKER ")
            .Append("  ,TB_M_MODEL M_MODEL ")
            .Append("WHERE ")
            .Append("      M_MODEL.MODEL_CD = :MODEL_CD ")
            .Append("  AND M_MODEL.MAKER_CD = M_MAKER.MAKER_CD ")

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("  M_MAKER.MAKER_CD")
            '.Append(" ,M_MAKER.MAKER_NAME")
            '.Append(" ,M_MAKER.MAKER_TYPE")
            '.Append(" ,M_MAKER.SORT_ORDER")
            '.Append(" ,M_MAKER.DMS_TAKEIN_DATETIME")
            '.Append(" ,M_MAKER.ROW_CREATE_DATETIME")
            '.Append(" ,M_MAKER.ROW_CREATE_ACCOUNT")
            '.Append(" ,M_MAKER.ROW_CREATE_FUNCTION")
            '.Append(" ,M_MAKER.ROW_UPDATE_DATETIME")
            '.Append(" ,M_MAKER.ROW_UPDATE_ACCOUNT")
            '.Append(" ,M_MAKER.ROW_UPDATE_FUNCTION")
            '.Append(" ,M_MAKER.ROW_LOCK_VERSION ")
            '.Append("FROM ")
            '.Append("  TB_M_MAKER M_MAKER ")
            '.Append(" ,TB_M_MODEL M_MODEL ")
            '.Append("WHERE ")
            '.AppendFormat(" M_MODEL.MODEL_CD = '{0}' ", strMODEL_CD)
            '.Append(" AND M_MODEL.MAKER_CD = M_MAKER.MAKER_CD ")

        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_MAKERDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strMODEL_CD)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 登録済データ取得処理
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function RESULT_DATA_Select(
    '                              ByVal strDLR_CD As String
    '                            ) As SC3250101DataSet.RESULT_DATADataTable

    '    Dim No As String = "SC3250101_012"
    '    Dim strMethodName As String = "RESULT_DATA_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.RESULT_DATADataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("   T_VEHICLE_SVCIN_HIS.SVCIN_DELI_DATE ")
    '            '.Append("  ,M_MAINTE.MAINTE_NAME ")
    '            '.Append("  ,T_VEHICLE_MILEAGE.REG_MILE ")
    '            '.Append("FROM  ")
    '            '.Append(" TB_T_VEHICLE_SVCIN_HIS T_VEHICLE_SVCIN_HIS")
    '            '.Append(" ,TB_M_MAINTE M_MAINTE")
    '            '.Append(" ,TB_T_VEHICLE_MAINTE_HIS T_VEHICLE_MAINTE_HIS")
    '            '.Append(" ,TB_T_VEHICLE_MILEAGE T_VEHICLE_MILEAGE")
    '            '.Append("WHERE ")
    '            '.AppendFormat("      T_VEHICLE_SVCIN_HIS.DLR_CD = '{0}' ", strDLR_CD)
    '            '.Append("  AND T_VEHICLE_SVCIN_HIS.DLR_CD = M_MAINTE.DLR_CD ")
    '            '.Append("  AND T_VEHICLE_SVCIN_HIS.DLR_CD = T_VEHICLE_MAINTE_HIS.DLR_CD ")
    '            '.Append("  AND T_VEHICLE_SVCIN_HIS.DLR_CD = TB_T_VEHICLE_MILEAGE.DLR_CD ")
    '            '.Append("  AND T_VEHICLE_MAINTE_HIS.DLR_CD = TB_T_VEHICLE_MILEAGE.DLR_CD ")
    '            '.Append("  AND M_MAINTE.MAINTE_CD = T_VEHICLE_MAINTE_HIS.MAINTE_CD ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.RESULT_DATADataTable)(No)
    '            query.CommandText = sql.ToString()

    '            'query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 点検順番取得
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_ORDER_Select() As SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable

    '    Dim No As String = "SC3250101_013"
    '    Dim strMethodName As String = "TB_M_INSPECTION_ORDER_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
    '        Dim sql As New StringBuilder

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable)(No)
    '            'SQL文作成
    '            With sql
    '                .Append("SELECT /* SC3250101_013 */ ")
    '                .Append("   INSPEC_ORDER ")
    '                '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '                .Append("  ,SVC_CD ")
    '                '.Append("  ,SVC_CD AS INSPEC_TYPE ")
    '                '.Append("  ,INSPEC_TYPE ")
    '                '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '                .Append("  ,M_GRADE.ROW_CREATE_DATETIME ")
    '                .Append("  ,M_GRADE.ROW_CREATE_ACCOUNT ")
    '                .Append("  ,M_GRADE.ROW_CREATE_FUNCTION ")
    '                .Append("  ,M_GRADE.ROW_UPDATE_DATETIME ")
    '                .Append("  ,M_GRADE.ROW_UPDATE_ACCOUNT ")
    '                .Append("  ,M_GRADE.ROW_UPDATE_FUNCTION ")
    '                .Append("  ,M_GRADE.ROW_LOCK_VERSION ")
    '                .Append("FROM ")
    '                .Append("  TB_M_INSPECTION_ORDER ")

    '            End With

    '            query.CommandText = sql.ToString()
    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 点検組み合わせマスタより直近で更新されたデータを取得
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <param name="strGRADE_CD">グレードコード</param>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_COMB_LATEST_Select( _
    '                                ByVal strMODEL_CD As String _
    '                              , ByVal strGRADE_CD As String _
    '                              , ByVal strDLR_CD As String _
    '                              , ByVal strBRN_CD As String _
    '                              ) As SC3250101DataSet.TB_M_INSPECTION_COMBDataTable

    '    Dim No As String = "SC3250101_014"
    '    Dim strMethodName As String = "TB_M_INSPECTION_COMB_LATEST_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_COMBDataTable
    '        Dim sql As New StringBuilder

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_COMBDataTable)(No)
    '            'SQL文作成
    '            With sql

    '                '.AppendFormat("SELECT /* {0} */ ", No)
    '                '.Append("   INSPEC_ITEM_CD ")
    '                '.Append("FROM  ")
    '                '.Append(" TB_M_INSPECTION_COMB M_INSPECTION_COMB ")
    '                '.Append(" TB_M_OPERATION_CHANGE M_OPERATION_CHANGE ")
    '                '.Append("WHERE ")
    '                '.AppendFormat("          M_INSPECTION_COMB.MODEL_CD = '{0}' ", strMODEL_CD)
    '                '.AppendFormat("      AND M_INSPECTION_COMB.GRADE_CD = '{0}' ", strGRADE_CD)
    '                '.AppendFormat("      AND M_INSPECTION_COMB.DLR_CD = '{0}' ", strDLR_CD)
    '                '.AppendFormat("      AND M_INSPECTION_COMB.BRN_CD = '{0}' ", strBRN_CD)
    '                '.Append("      AND M_INSPECTION_COMB.GRADE_CD = M_OPERATION_CHANGE.GRADE_CD")
    '                '.Append("      AND M_INSPECTION_COMB.MODEL_CD = M_OPERATION_CHANGE.MODEL_CD")
    '                '.Append("      AND M_INSPECTION_COMB.INSPEC_TYPE = M_OPERATION_CHANGE.INSPEC_TYPE")
    '                '.Append("      AND ROW_UPDATE_DATETIME = ")
    '                '.Append("          ( ")
    '                '.Append("            SELECT ")
    '                '.Append("              MAX(ROW_UPDATE_DATETIME) ")
    '                '.Append("             FROM  ")
    '                '.Append("              TB_M_INSPECTION_COMB M_INSPECTION_COMB ")
    '                '.Append("              ,TB_M_OPERATION_CHANGE M_OPERATION_CHANGE ")
    '                '.Append("             WHERE ")
    '                '.AppendFormat("            M_INSPECTION_COMB.MODEL_CD = '{0}' ", strMODEL_CD)
    '                '.AppendFormat("        AND M_INSPECTION_COMB.GRADE_CD = '{0}' ", strGRADE_CD)
    '                '.AppendFormat("        AND M_INSPECTION_COMB.DLR_CD = '{0}' ", strDLR_CD)
    '                '.AppendFormat("        AND M_INSPECTION_COMB.BRN_CD = '{0}' ", strBRN_CD)
    '                '.Append("              AND M_INSPECTION_COMB.GRADE_CD = M_OPERATION_CHANGE.GRADE_CD")
    '                '.Append("              AND M_INSPECTION_COMB.MODEL_CD = M_OPERATION_CHANGE.MODEL_CD")
    '                '.Append("              AND M_INSPECTION_COMB.INSPEC_TYPE = M_OPERATION_CHANGE.INSPEC_TYPE")
    '                '.Append("          ) ")

    '            End With

    '            query.CommandText = sql.ToString()
    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

    ''' <summary>
    ''' 015:サービス入庫マスタ取得
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号取得</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_T_SERVICEIN_Select(ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strRO_NUM As String
                                            ) As SC3250101DataSet.TB_T_SERVICEINDataTable
        Dim No As String = "SC3250101_015"
        Dim strMethodName As String = "TB_T_SERVICEIN_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_T_SERVICEINDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            .Append("SELECT /* SC3250101_015 */ ")
            .Append("	NVL(T2.ADD_JOB_ADVICE, ' ') AS ADD_JOB_ADVICE ")
            .Append("	,NVL(T2.NEXT_SVCIN_INSPECTION_ADVICE, ' ') AS NEXT_SVCIN_INSPECTION_ADVICE ")
            .Append("FROM ")
            .Append("	TB_T_RO_INFO T1 ")
            .Append("	,TB_T_SERVICEIN T2 ")
            .Append("WHERE ")
            .Append("	T1.DLR_CD = :DLR_CD ")
            .Append("	AND T1.BRN_CD = :BRN_CD ")
            .Append("	AND T1.RO_NUM = :RO_NUM ")
            .Append("	AND T1.RO_SEQ = 0")
            .Append("	AND T1.SVCIN_ID = T2.SVCIN_ID ")

            '.Append("SELECT /* SC3250101_015 */ ")
            '.Append("   ADD_JOB_ADVICE ")
            '.Append("  ,NEXT_SVCIN_INSPECTION_ADVICE ")
            '.Append("FROM ")
            '.Append("  TB_T_SERVICEIN ")
            '.Append("WHERE ")
            '.Append("  RO_NUM = :RO_NUM ")
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("    ADD_JOB_ADVICE ")
            '.Append("   ,NEXT_SVCIN_INSPECTION_ADVICE ")
            '.Append("FROM  ")
            '.Append(" TB_T_SERVICEIN ")
            '.Append("WHERE ")
            '.AppendFormat("          RO_NUM = '{0}' ", strRO_NUM)

            .Append(" UNION ALL ")

            .Append("SELECT ")
            .Append("    NVL(TH2.ADD_JOB_ADVICE, ' ') AS ADD_JOB_ADVICE ")
            .Append("    ,NVL(TH2.NEXT_SVCIN_INSPECTION_ADVICE, ' ') AS NEXT_SVCIN_INSPECTION_ADVICE ")
            .Append("FROM ")
            .Append("    TB_H_RO_INFO TH1 ")
            .Append("    ,TB_H_SERVICEIN TH2 ")
            .Append("WHERE ")
            .Append("    TH1.DLR_CD = :DLR_CD ")
            .Append("    AND TH1.BRN_CD = :BRN_CD ")
            .Append("    AND TH1.RO_NUM = :RO_NUM ")
            .Append("    AND TH1.RO_SEQ = 0")
            .Append("    AND TH1.SVCIN_ID = TH2.SVCIN_ID ")

        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_SERVICEINDataTable)(No)
            query.CommandText = sql.ToString()
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRO_NUM)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function

    ''' <summary>
    ''' 016:タイミング取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTiming_Select(ByVal strDLR_CD As String, ByVal strBRN_CD As String, ByVal strRO_NUM As String) As SC3250101DataSet.GetTimingDataTable
        Dim No As String = "SC3250101_016"
        Dim strMethodName As String = "GetTiming_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.GetTimingDataTable
        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            '2014/06/13 ROステータスによってアドバイス表示を変更　START　↓↓↓
            '.Append("SELECT /* SC3250101_016 */ ")
            '.Append("  T2.RO_STATUS ")
            '.Append("FROM ")
            '.Append("   TB_T_SERVICEIN T1 ")
            '.Append("  ,TB_T_RO_INFO T2 ")
            '.Append("WHERE ")
            '.Append("      T1.DLR_CD = :DLR_CD ")
            '.Append("  AND T1.BRN_CD = :BRN_CD ")
            '.Append("  AND T1.RO_NUM = :RO_NUM ")
            '.Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
            '.Append("  AND T1.RO_NUM = T2.RO_NUM ")
            '.Append("  AND T1.DLR_CD = T2.DLR_CD ")
            '.Append("  AND T1.BRN_CD = T2.BRN_CD ")

            .Append("SELECT /* SC3250101_016 */ ")
            .Append("  T2.RO_STATUS ")
            .Append("FROM ")
            .Append("  TB_T_RO_INFO T2 ")
            .Append("WHERE ")
            .Append("      T2.DLR_CD = :DLR_CD ")
            .Append("  AND T2.BRN_CD = :BRN_CD ")
            .Append("  AND T2.RO_NUM = :RO_NUM ")
            .Append("  AND T2.RO_SEQ = 0 ")
            '2014/06/17 「RO_SEQ=0」を追加
            '顧客承認待ち（40）はRO_SEQ=0が一番最初に40になる。（RO_SEQ=1以上がRO_SEQ=0より先にステータス40以上にはならない）
            '納車作業（85）はすべてのRO_SEQが一斉にステータス85となる
            '　→RO_SEQ=0だけ参照すればROステータスが参照できるため「RO_SEQ=0」固定としています。

            '2014/06/13 ROステータスによってアドバイス表示を変更　END　　↑↑↑

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("T2.RO_STATUS                     ")
            '.Append("FROM                             ")
            '.Append("TB_T_SERVICEIN T1,               ")
            '.Append("TB_T_RO_INFO T2                  ")
            '.Append("WHERE                            ")
            '.AppendFormat("T1.DLR_CD='{0}' AND            ", strDLR_CD)
            '.AppendFormat("T1.BRN_CD='{0}' AND               ", strBRN_CD)
            '.AppendFormat("T1.RO_NUM='{0}' AND    ", strRO_NUM)
            '.Append("T1.SVCIN_ID = T2.SVCIN_ID AND    ")
            '.Append("T1.RO_NUM = T2.RO_NUM AND        ")
            '.Append("T1.DLR_CD = T2.DLR_CD AND        ")
            '.Append("T1.BRN_CD = T2.BRN_CD            ")

            .Append(" UNION ALL ")

            .Append("SELECT ")
            .Append("  TH2.RO_STATUS ")
            .Append("FROM ")
            .Append("  TB_H_RO_INFO TH2 ")
            .Append("WHERE ")
            .Append("      TH2.DLR_CD = :DLR_CD ")
            .Append("  AND TH2.BRN_CD = :BRN_CD ")
            .Append("  AND TH2.RO_NUM = :RO_NUM ")
            .Append("  AND TH2.RO_SEQ = 0 ")

        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.GetTimingDataTable)(No)
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRO_NUM)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function

    ''' <summary>
    ''' 017:商品訴求部位マスタ取得
    ''' </summary>
    ''' <param name="strPART_NAME">商品訴求部位番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_REPAIR_SUGGESTION_PART_Select( _
                                                        ByVal strPART_NAME As String
                                                    ) As SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        Dim No As String = "SC3250101_017"
        Dim strMethodName As String = "TB_M_REPAIR_SUGGESTION_PART_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_017 */ ")
            .Append("   PART_CD ")
            '2014/05/22 文言DBから取得　START　↓↓↓
            '.Append("  ,PART_NAME ")
            .Append("  ,PART_NAME_NO ")
            '2014/05/22 文言DBから取得　END　　↑↑↑
            .Append("  ,POPUP_URL ")
            .Append("FROM ")
            .Append("  TB_M_REPAIR_SUGGESTION_PART ")
            .Append("WHERE ")
            .Append("  PART_CD = :PART_CD ")

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("    PART_CD ")
            '.Append("   ,PART_NAME ")
            '.Append("   ,POPUP_URL ")
            '.Append("FROM  ")
            '.Append(" TB_M_REPAIR_SUGGESTION_PART ")
            '.Append("WHERE ")
            '.AppendFormat("          PART_CD = '{0}' ", strPART_NAME)
        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("PART_CD", OracleDbType.NVarchar2, strPART_NAME)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function

    '2014/05/29 レスポンス対策　START　↓↓↓
    ''' <summary>
    ''' 018:商品訴求部位マスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_REPAIR_SUGGESTION_ALL_PART_Select(ByVal listPART_NAME As List(Of String)) As SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        Dim No As String = "SC3250101_018"
        Dim strMethodName As String = "TB_M_REPAIR_SUGGESTION_ALL_PART_Select"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_018 */ ")
            .Append("   PART_CD ")
            .Append("  ,PART_NAME_NO ")
            .Append("  ,POPUP_URL ")
            .Append("FROM ")
            .Append("  TB_M_REPAIR_SUGGESTION_PART ")
            .Append("WHERE ")
            For i As Integer = 0 To listPART_NAME.Count - 1
                If 0 < i Then
                    .Append(" OR  ")
                End If
                .AppendFormat("  PART_CD = :PART_CD{0} ", i.ToString)
            Next

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append("    PART_CD ")
            '.Append("   ,PART_NAME ")
            '.Append("   ,POPUP_URL ")
            '.Append("FROM  ")
            '.Append(" TB_M_REPAIR_SUGGESTION_PART ")
            '.Append("WHERE ")
            '.AppendFormat("          PART_CD = '{0}' ", strPART_NAME)
        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable)(No)

            query.CommandText = sql.ToString()
            For i As Integer = 0 To listPART_NAME.Count - 1
                query.AddParameterWithTypeValue(String.Format("PART_CD{0}", i.ToString), OracleDbType.NVarchar2, listPART_NAME(i))
            Next

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using

    End Function
    '2014/05/29 レスポンス対策　　END　↑↑↑

#Region "未使用メソッド"
    '2014/06/09 未使用メソッドのコメント化　START　↓↓↓

    ' ''' <summary>
    ' ''' 完成検査点検内容マスタ取得
    ' ''' </summary>
    ' ''' <param name="strPART_NAME">点検項目名称</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_DETAIL_Select( _
    '                                ByVal strPART_NAME As String _
    '                              ) As SC3250101DataSet.TB_M_INSPECTION_DETAILDataTable

    '    Dim No As String = "SC3250101_018"
    '    Dim strMethodName As String = "TB_M_INSPECTION_DETAIL_Select"
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_DETAILDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_018 */ ")
    '            .Append("   M_INSPECTION_COMB.MODEL_CD ")
    '            .Append("  ,M_INSPECTION_COMB.GRADE_CD ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  ,M_INSPECTION_COMB.SVC_CD ")
    '            '.Append("  ,M_INSPECTION_COMB.SVC_CD AS INSPEC_TYPE ")
    '            '.Append("  ,M_INSPECTION_COMB.INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  ,M_INSPECTION_COMB.INSPEC_ITEM_CD ")
    '            .Append("  ,M_INSPECTION_COMB.DLR_CD ")
    '            .Append("  ,M_INSPECTION_COMB.BRN_CD ")
    '            .Append("  ,M_INSPECTION_COMB.PART_CD ")
    '            .Append("  ,M_INSPECTION_COMB.REQ_PART_CD ")
    '            .Append("  ,M_INSPECTION_COMB.REQ_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　START　↓↓↓
    '            .Append("  ,M_INSPECTION_COMB.INSPEC_ITEM_CD AS SERVICE_ITEM_CD ")
    '            '.Append("  ,M_INSPECTION_COMB.SERVICE_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　END　　↑↑↑
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　START　↓↓↓
    '            .Append("  ,M_INSPECTION_COMB.SUGGEST_FLAG AS SUGGEST_STATUS ")
    '            '.Append("  ,M_INSPECTION_COMB.SUGGEST_STATUS ")
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　END　　↑↑↑
    '            .Append("  ,M_INSPECTION_COMB.REQ_ITEM_DISP_SEQ ")
    '            .Append("FROM ")
    '            .Append("   TB_M_INSPECTION_COMB M_INSPECTION_COMB ")
    '            .Append("  ,TB_M_REPAIR_SUGGESTION_PART M_REPAIR_SUGGESTION_PART ")
    '            .Append("WHERE ")
    '            .Append("      M_INSPECTION_COMB.PART_CD = :PART_CD ")
    '            .Append("  AND M_INSPECTION_COMB.REQ_PART_CD = M_INSPECTION_COMB.REQ_PART_CD ")
    '            .Append("ORDER BY ")
    '            .Append("  REQ_ITEM_DISP_SEQ ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("    MODEL_CD ")
    '            '.Append("   ,GRADE_CD ")
    '            '.Append("   ,INSPEC_TYPE ")
    '            '.Append("   ,INSPEC_ITEM_CD ")
    '            '.Append("   ,DLR_CD ")
    '            '.Append("   ,BRN_CD ")
    '            '.Append("   ,PART_CD ")
    '            '.Append("   ,REQ_PART_CD ")
    '            '.Append("   ,REQ_ITEM_CD ")
    '            '.Append("   ,SERVICE_ITEM_CD ")
    '            '.Append("   ,SUGGEST_STATUS ")
    '            '.Append("   ,REQ_ITEM_DISP_SEQ ")
    '            '.Append("FROM  ")
    '            '.Append(" TB_M_INSPECTION_COMB M_INSPECTION_COMB ")
    '            '.Append(" ,TB_M_REPAIR_SUGGESTION_PART M_REPAIR_SUGGESTION_PART ")
    '            '.Append("WHERE ")
    '            '.AppendFormat("          AND M_INSPECTION_COMB.PART_NAME = '{0}' ", strPART_NAME)
    '            '.Append("          AND M_INSPECTION_COMB.REQ_PART_CD = M_INSPECTION_COMB.REQ_PART_CD ")
    '            '.Append("ORDER BY ")
    '            '.Append(" REQ_ITEM_DISP_SEQ ")

    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_DETAILDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("PART_CD", OracleDbType.NVarchar2, strPART_NAME)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求登録実績データ取得
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select(
    '                                          ByVal strDLR_CD As String _
    '                                        , ByVal strBRN_CD As String _
    '                                        , ByVal strSTF_CD As String _
    '                                        , ByVal strRO_NUM As String _
    '                                        , ByVal strSVC_CD As String
    '                                        ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable

    '    Dim No As String = "SC3250101_019"
    '    Dim strMethodName As String = "TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_019 */ ")
    '            .Append("   T_REPAIR_SUGGESTION_RSLT.DLR_CD ")
    '            .Append("  ,T_REPAIR_SUGGESTION_RSLT.BRN_CD ")
    '            '2014/05/26 「STF_CD」の消去　START　↓↓↓
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.STF_CD ")
    '            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
    '            .Append("  ,T_REPAIR_SUGGESTION_RSLT.RO_NUM ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  ,T_REPAIR_SUGGESTION_RSLT.SVC_CD ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.SVC_CD AS INSPEC_TYPE ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  ,T_REPAIR_SUGGESTION_RSLT.INSPEC_ITEM_CD ")
    '            .Append("  ,NVL(M_INSPECTION_DETAIL.INSPEC_ITEM_NAME,' ') AS INSPEC_ITEM_NAME ")
    '            .Append("  ,T_REPAIR_SUGGESTION_RSLT.SUGGEST_ICON ")
    '            .Append("FROM ")
    '            .Append("   TB_T_REPAIR_SUGGESTION_RSLT T_REPAIR_SUGGESTION_RSLT ")
    '            .Append("  ,TB_M_INSPECTION_DETAIL M_INSPECTION_DETAIL ")
    '            .Append("WHERE ")
    '            .Append("      T_REPAIR_SUGGESTION_RSLT.DLR_CD = :DLR_CD ")
    '            .Append("  AND T_REPAIR_SUGGESTION_RSLT.BRN_CD = :BRN_CD ")
    '            '2014/05/26 「STF_CD」の消去　START　↓↓↓
    '            '.Append("  AND T_REPAIR_SUGGESTION_RSLT.STF_CD = :STF_CD ")
    '            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
    '            .Append("  AND T_REPAIR_SUGGESTION_RSLT.RO_NUM = :RO_NUM ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  AND T_REPAIR_SUGGESTION_RSLT.SVC_CD = :SVC_CD ")
    '            '.Append("  AND T_REPAIR_SUGGESTION_RSLT.INSPEC_TYPE = :INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  AND T_REPAIR_SUGGESTION_RSLT.INSPEC_ITEM_CD = M_INSPECTION_DETAIL.INSPEC_ITEM_CD(+) ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("   T_REPAIR_SUGGESTION_RSLT.DLR_CD ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.BRN_CD ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.STF_CD ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.RO_NUM ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.INSPEC_TYPE ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.INSPEC_ITEM_CD ")
    '            '.Append("  ,M_INSPECTION_DETAIL.INSPEC_ITEM_NAME ")
    '            '.Append("  ,T_REPAIR_SUGGESTION_RSLT.SUGGEST_ICON ")
    '            '.Append("FROM  ")
    '            '.Append(" TB_T_REPAIR_SUGGESTION_RSLT T_REPAIR_SUGGESTION_RSLT")
    '            '.Append(" ,TB_M_INSPECTION_DETAIL M_INSPECTION_DETAIL ")
    '            '.Append("WHERE ")
    '            '.AppendFormat("      T_REPAIR_SUGGESTION_RSLT.DLR_CD = '{0}' ", strDLR_CD)
    '            '.AppendFormat("  AND T_REPAIR_SUGGESTION_RSLT.BRN_CD = '{0}' ", strBRN_CD)
    '            '.AppendFormat("  AND T_REPAIR_SUGGESTION_RSLT.STF_CD = '{0}' ", strSTF_CD)
    '            '.AppendFormat("  AND T_REPAIR_SUGGESTION_RSLT.RO_NUM = '{0}' ", strRO_NUM)
    '            '.AppendFormat("  AND T_REPAIR_SUGGESTION_RSLT.INSPEC_TYPE = '{0}' ", strINSPEC_TYPE)
    '            '.Append("        AND T_REPAIR_SUGGESTION_RSLT.INSPEC_ITEM_CD = M_INSPECTION_DETAIL.INSPEC_ITEM_CD ")

    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
    '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
    '            '2014/05/26 「STF_CD」の消去　START　↓↓↓
    '            'query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
    '            '2014/05/26 「STF_CD」の消去　END　　↑↑↑
    '            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRO_NUM)
    '            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求登録一時データ取得
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_W_REPAIR_SUGGESTION_Select_List(
    '                                          ByVal strDLR_CD As String _
    '                                        , ByVal strBRN_CD As String _
    '                                        , ByVal strSTF_CD As String _
    '                                        , ByVal strRO_NUM As String _
    '                                        , ByVal strSVC_CD As String
    '                                        ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable

    '    Dim No As String = "SC3250101_020"
    '    Dim strMethodName As String = "TB_W_REPAIR_SUGGESTION_Select_List"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql

    '            .Append("SELECT /* SC3250101_020 */ ")
    '            .Append("   W_REPAIR_SUGGESTION.DLR_CD ")
    '            .Append("  ,W_REPAIR_SUGGESTION.BRN_CD ")
    '            .Append("  ,W_REPAIR_SUGGESTION.STF_CD ")
    '            .Append("  ,W_REPAIR_SUGGESTION.RO_NUM ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  ,W_REPAIR_SUGGESTION.SVC_CD ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.SVC_CD AS INSPEC_TYPE ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  ,W_REPAIR_SUGGESTION.INSPEC_ITEM_CD ")
    '            .Append("  ,M_INSPECTION_DETAIL.INSPEC_ITEM_NAME ")
    '            .Append("  ,W_REPAIR_SUGGESTION.SUGGEST_ICON ")
    '            .Append("FROM ")
    '            .Append("   TB_W_REPAIR_SUGGESTION W_REPAIR_SUGGESTION ")
    '            .Append("  ,TB_M_INSPECTION_DETAIL M_INSPECTION_DETAIL ")
    '            .Append("WHERE ")
    '            .Append("      W_REPAIR_SUGGESTION.DLR_CD = :DLR_CD ")
    '            .Append("  AND W_REPAIR_SUGGESTION.BRN_CD = :BRN_CD ")
    '            .Append("  AND W_REPAIR_SUGGESTION.STF_CD = :STF_CD ")
    '            .Append("  AND W_REPAIR_SUGGESTION.RO_NUM = :RO_NUM ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  AND W_REPAIR_SUGGESTION.SVC_CD = :SVC_CD ")
    '            '.Append("  AND W_REPAIR_SUGGESTION.INSPEC_TYPE = :INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  AND W_REPAIR_SUGGESTION.INSPEC_ITEM_CD = M_INSPECTION_DETAIL.INSPEC_ITEM_CD ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append("   W_REPAIR_SUGGESTION.DLR_CD                       ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.BRN_CD                       ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.STF_CD                       ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.RO_NUM                       ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.INSPEC_TYPE                  ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.INSPEC_ITEM_CD               ")
    '            '.Append("  ,M_INSPECTION_DETAIL.INSPEC_ITEM_NAME             ")
    '            '.Append("  ,W_REPAIR_SUGGESTION.SUGGEST_ICON                 ")
    '            '.Append("FROM                                                ")
    '            '.Append(" TB_W_REPAIR_SUGGESTION W_REPAIR_SUGGESTION         ")
    '            '.Append(" ,TB_M_INSPECTION_DETAIL M_INSPECTION_DETAIL        ")
    '            '.Append("WHERE                                               ")
    '            '.AppendFormat("     W_REPAIR_SUGGESTION.DLR_CD = '{0}'          ", strDLR_CD)
    '            '.AppendFormat(" AND W_REPAIR_SUGGESTION.BRN_CD = '{0}'          ", strBRN_CD)
    '            '.AppendFormat(" AND W_REPAIR_SUGGESTION.STF_CD = '{0}'          ", strSTF_CD)
    '            '.AppendFormat(" AND W_REPAIR_SUGGESTION.RO_NUM = '{0}'          ", strRO_NUM)
    '            '.AppendFormat(" AND W_REPAIR_SUGGESTION.INSPEC_TYPE = '{0}'     ", strINSPEC_TYPE)
    '            '.Append(" AND W_REPAIR_SUGGESTION.INSPEC_ITEM_CD = M_INSPECTION_DETAIL.INSPEC_ITEM_CD")

    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
    '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
    '            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
    '            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRO_NUM)
    '            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求点検内容データ取得
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_COMB_Select(
    '                                          ByVal strModel_CD As String _
    '                                        , ByVal strGrade_CD As String _
    '                                        , ByVal strSVC_CD As String
    '                                        ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    Dim No As String = "SC3250101_021"
    '    Dim strMethodName As String = "TB_M_INSPECTION_COMB_Select"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_021 */ ")
    '            .Append("   M1.MODEL_CD ")
    '            .Append("  ,M1.GRADE_CD ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  ,M1.SVC_CD ")
    '            '.Append("  ,M1.SVC_CD AS INSPEC_TYPE ")
    '            '.Append("  ,M1.INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  ,M1.INSPEC_ITEM_CD ")
    '            .Append("  ,M1.DLR_CD ")
    '            .Append("  ,M1.BRN_CD ")
    '            .Append("  ,M1.PART_CD ")
    '            .Append("  ,M1.REQ_PART_CD ")
    '            .Append("  ,M1.REQ_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　START　↓↓↓
    '            .Append("  ,M1.INSPEC_ITEM_CD AS SERVICE_ITEM_CD ")
    '            '.Append("  ,M1.SERVICE_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　END　　↑↑↑
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　START　↓↓↓
    '            .Append("  ,M1.SUGGEST_FLAG AS SUGGEST_STATUS ")
    '            '.Append("  ,M1.SUGGEST_STATUS ")
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　END　　↑↑↑
    '            .Append("  ,M1.REQ_ITEM_DISP_SEQ ")
    '            .Append("  ,M2.INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.SUB_INSPEC_ITEM_NAME ")
    '            .Append("FROM ")
    '            .Append("   TB_M_INSPECTION_COMB M1 ")
    '            .Append("  ,TB_M_INSPECTION_DETAIL M2 ")
    '            .Append("WHERE ")
    '            .Append("  M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '            .Append("ORDER BY ")
    '            .Append("   M1.REQ_PART_CD ")
    '            .Append("  ,M1.REQ_ITEM_DISP_SEQ ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append(" M1.MODEL_CD")              '--モデルコード
    '            '.Append(", M1.GRADE_CD          ")   '--グレードコード
    '            '.Append(", M1.INSPEC_TYPE       ")   '--点検種類
    '            '.Append(", M1.INSPEC_ITEM_CD    ")   '--点検項目コード
    '            '.Append(", M1.DLR_CD            ")   '--販売店コード
    '            '.Append(", M1.BRN_CD            ")   '--店舗コード
    '            '.Append(", M1.PART_CD           ")   '--完成検査部位コード
    '            '.Append(", M1.REQ_PART_CD       ")   '--商品訴求用部位コード
    '            '.Append(", M1.REQ_ITEM_CD       ")   '--商品訴求初期表示用アイテム
    '            '.Append(", M1.SERVICE_ITEM_CD   ")   '--商品訴求用サービスアイテムコード
    '            '.Append(", M1.SUGGEST_STATUS    ")   '--推奨点検ステータス
    '            '.Append(", M1.REQ_ITEM_DISP_SEQ ")   '--商品訴求用アイテム表示順
    '            '.Append(", M2.INSPEC_ITEM_NAME   ")  '--点検項目名称
    '            '.Append(", M2.SUB_INSPEC_ITEM_NAME ") '--サブ点検項目名称
    '            '.Append(" FROM           TB_M_INSPECTION_COMB    M1")
    '            '.Append(" LEFT JOIN TB_M_INSPECTION_DETAIL  M2 ON M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD")
    '            '.Append(" ORDER BY  M1.REQ_PART_CD, M1.REQ_ITEM_DISP_SEQ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function

    '2014/06/09 未使用メソッドのコメント化　END　　↑↑↑
#End Region
#Region "2016/10/5　未使用メソッドのコメント化"
    ''2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)(SQL文の変更)　START　↓↓↓
    ''2014/05/29 レスポンス対策　START　↓↓↓
    ' ''' <summary>
    ' ''' 022:商品訴求点検リスト取得
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_COMB_SelectList(
    '                                          ByVal strModel_CD As String _
    '                                        , ByVal strGrade_CD As String _
    '                                        , ByVal strDLR_CD As String _
    '                                        , ByVal strBRN_CD As String _
    '                                        , ByVal strSTF_CD As String _
    '                                        , ByVal strSAChipID As String _
    '                                        , ByVal strSVC_CD As String _
    '                                        , ByVal strDefaultModel_CD As String _
    '                                        , ByVal strDLR_CD_M As String _
    '                                        , ByVal strBRN_CD_M As String _
    '                                        ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

    '    Dim No As String = "SC3250101_022"
    '    Dim strMethodName As String = "TB_M_INSPECTION_COMB_SelectList"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Dim dt As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable
    '    Dim sql As New StringBuilder

    '    'SQL文作成
    '    With sql
    '        '2014/06/17　点検項目が重複する可能性がある内容を修正　START　↓↓↓
    '        .Append("SELECT /* SC3250101_022 */ DISTINCT ")
    '        .Append("	 MM1.INSPEC_ITEM_CD ")
    '        .Append("	,MM1.REQ_PART_CD ")
    '        .Append("	,MM1.INSPEC_ITEM_NAME ")
    '        .Append("	,MM1.SUB_INSPEC_ITEM_NAME ")
    '        .Append("	,MM1.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        .Append("	,MM1.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        .Append("	,MM1.DISP_INSPEC_ITEM_NEED_FIX ")
    '        .Append("	,MM1.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        .Append("	,MM1.DISP_INSPEC_ITEM_NEED_SWAP ")
    '        .Append("	,MM1.REQ_ITEM_DISP_SEQ ")
    '        .Append("	,MSM1.REQ_ITEM_CD_DEFAULT ")
    '        .Append("	,MSM1.SVC_CD_DEFAULT ")
    '        .Append("	,MSM1.SUGGEST_FLAG_DEFAULT ")
    '        .Append("	,MSM2.REQ_ITEM_CD ")
    '        .Append("	,MSM2.SVC_CD ")
    '        .Append("	,MSM2.SUGGEST_FLAG ")
    '        .Append("	,MM3.R_SUGGEST_ICON ")
    '        .Append("	,MM3.R_SVC_CD ")
    '        .Append("	,MM4.W_SUGGEST_ICON ")
    '        .Append("	,MM4.W_SVC_CD ")
    '        .Append("FROM ")
    '        .Append("	(SELECT ")
    '        .Append("		 M1.INSPEC_ITEM_CD ")
    '        .Append("		,M1.REQ_PART_CD ")
    '        .Append("		,M2.INSPEC_ITEM_NAME ")
    '        .Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        .Append("		,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_INSPEC, '0')) AS DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        .Append("		,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_REPLACE, '0')) AS DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        .Append("		,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_FIX, '0')) AS DISP_INSPEC_ITEM_NEED_FIX ")
    '        .Append("		,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_CLEAN, '0')) AS DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        .Append("		,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_SWAP, '0')) AS DISP_INSPEC_ITEM_NEED_SWAP ")
    '        .Append("		,MAX(CASE ")
    '        .Append("		 WHEN M1.MODEL_CD = :MODEL_CD THEN NVL(M1.REQ_ITEM_DISP_SEQ, 0) ")
    '        .Append("		 ELSE NULL ")
    '        .Append("		 END )AS REQ_ITEM_DISP_SEQ ")
    '        .Append("	FROM ")
    '        .Append("		 TB_M_INSPECTION_COMB M1 ")
    '        .Append("		,TB_M_FINAL_INSPECTION_DETAIL M2 ")
    '        .Append("	WHERE ")
    '        .Append("		M1.MODEL_CD IN (:MODEL_CD, :DEFAULT_MODEL_CD) ")
    '        .Append("		AND M1.DLR_CD = :DLR_CD_M ")
    '        .Append("		AND M1.BRN_CD = :BRN_CD_M ")
    '        .Append("		AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '        .Append("    GROUP BY ")
    '        .Append("		 M1.INSPEC_ITEM_CD ")
    '        .Append("		,M1.REQ_PART_CD ")
    '        .Append("		,M2.INSPEC_ITEM_NAME ")
    '        .Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        .Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        .Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        .Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        .Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        .Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP) MM1 ")
    '        .Append("	,(SELECT ")
    '        .Append("		 SM1.INSPEC_ITEM_CD ")
    '        .Append("		,SM1.REQ_ITEM_CD AS REQ_ITEM_CD_DEFAULT ")
    '        .Append("		,SM1.SVC_CD AS SVC_CD_DEFAULT")
    '        '2014/07/01　SUGGEST_FLAG → SUGGEST_FLGに変更　↓↓↓
    '        .Append("		,NVL(SM1.SUGGEST_FLG, 0) AS SUGGEST_FLAG_DEFAULT ")
    '        .Append("	FROM ")
    '        .Append("		TB_M_INSPECTION_COMB SM1 ")
    '        .Append("	WHERE ")
    '        .Append("		SM1.MODEL_CD = :DEFAULT_MODEL_CD ")
    '        .Append("		AND SM1.SVC_CD = :SVC_CD ")
    '        .Append("		AND SM1.DLR_CD = :DLR_CD_M ")
    '        .Append("		AND SM1.BRN_CD = :BRN_CD_M ) MSM1 ")
    '        .Append("	,(SELECT ")
    '        .Append("		 SM1.INSPEC_ITEM_CD ")
    '        .Append("		,SM1.REQ_ITEM_CD ")
    '        .Append("		,SM1.SVC_CD ")
    '        '2014/07/01　SUGGEST_FLAG → SUGGEST_FLGに変更　↓↓↓
    '        .Append("		,NVL(SM1.SUGGEST_FLG, 0) AS SUGGEST_FLAG ")
    '        .Append("	FROM ")
    '        .Append("		TB_M_INSPECTION_COMB SM1 ")
    '        .Append("	WHERE ")
    '        .Append("		SM1.MODEL_CD = :MODEL_CD ")
    '        .Append("		AND SM1.SVC_CD = :SVC_CD ")
    '        .Append("		AND SM1.DLR_CD = :DLR_CD_M ")
    '        .Append("		AND SM1.BRN_CD = :BRN_CD_M ) MSM2 ")
    '        .Append("	,(SELECT ")
    '        .Append("		M3.INSPEC_ITEM_CD  AS R_INSPEC_ITEM_CD ")
    '        .Append("		,M3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
    '        .Append("		,M3.SVC_CD AS R_SVC_CD ")
    '        .Append("	FROM ")
    '        .Append("		TB_T_REPAIR_SUGGESTION_RSLT M3 ")
    '        .Append("	WHERE ")
    '        .Append("		M3.DLR_CD = :DLR_CD ")
    '        .Append("		AND M3.BRN_CD = :BRN_CD ")
    '        .Append("		AND M3.RO_NUM = :SA_CHIP_ID ")
    '        .Append("		AND M3.SVC_CD = :SVC_CD) MM3 ")
    '        .Append("	,(SELECT ")
    '        .Append("		M4.INSPEC_ITEM_CD  AS W_INSPEC_ITEM_CD ")
    '        .Append("		,M4.SUGGEST_ICON  AS W_SUGGEST_ICON ")
    '        .Append("		,M4.SVC_CD AS W_SVC_CD ")
    '        .Append("	FROM ")
    '        .Append("		TB_W_REPAIR_SUGGESTION M4 ")
    '        .Append("	WHERE ")
    '        .Append("		M4.DLR_CD = :DLR_CD ")
    '        .Append("		AND M4.BRN_CD = :BRN_CD ")
    '        .Append("		AND M4.STF_CD = :STF_CD ")
    '        .Append("		AND M4.RO_NUM = :SA_CHIP_ID ")
    '        .Append("		AND M4.SVC_CD = :SVC_CD ) MM4 ")
    '        .Append("WHERE ")
    '        .Append("	 MM1.INSPEC_ITEM_CD = MM3.R_INSPEC_ITEM_CD (+) ")
    '        .Append("	AND MM1.INSPEC_ITEM_CD = MM4.W_INSPEC_ITEM_CD (+) ")
    '        .Append("	AND MM1.INSPEC_ITEM_CD = MSM1.INSPEC_ITEM_CD (+) ")
    '        .Append("	AND MM1.INSPEC_ITEM_CD = MSM2.INSPEC_ITEM_CD (+) ")
    '        .Append("ORDER BY ")
    '        .Append("	 MM1.REQ_PART_CD ")
    '        .Append("	,MM1.REQ_ITEM_DISP_SEQ ")
    '        .Append("	,MM1.INSPEC_ITEM_NAME ")

    '        '.Append("SELECT /* SC3250101_022 */ DISTINCT ")
    '        '.Append("	 MM1.INSPEC_ITEM_CD ")
    '        '.Append("	,MM1.REQ_PART_CD ")
    '        '.Append("	,MM1.INSPEC_ITEM_NAME ")
    '        '.Append("	,MM1.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("	,MM1.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("	,MM1.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("	,MM1.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("	,MM1.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("	,MM1.DISP_INSPEC_ITEM_NEED_SWAP ")
    '        '.Append("	,MM1.REQ_ITEM_DISP_SEQ ")
    '        '.Append("	,MSM1.REQ_ITEM_CD ")
    '        '.Append("	,MSM1.SVC_CD ")
    '        '.Append("	,MSM1.SUGGEST_STATUS ")
    '        '.Append("	,MM3.R_SUGGEST_ICON ")
    '        '.Append("	,MM3.R_SVC_CD ")
    '        '.Append("	,MM4.W_SUGGEST_ICON ")
    '        '.Append("	,MM4.W_SVC_CD ")
    '        '.Append("FROM ")
    '        '.Append("	(SELECT ")
    '        '.Append("		 M1.INSPEC_ITEM_CD ")
    '        '.Append("		,M1.REQ_PART_CD ")
    '        '.Append("		,M2.INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
    '        '.Append("		,MAX(CASE ")
    '        '.Append("		 WHEN M1.MODEL_CD = :MODEL_CD THEN M1.REQ_ITEM_DISP_SEQ ")
    '        '.Append("		 ELSE NULL ")
    '        '.Append("		 END )AS REQ_ITEM_DISP_SEQ ")
    '        '.Append("	FROM ")
    '        '.Append("		 TB_M_INSPECTION_COMB M1 ")
    '        '.Append("		,TB_M_INSPECTION_DETAIL M2 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M1.MODEL_CD IN (:MODEL_CD, :DEFAULT_MODEL_CD) ")
    '        '.Append("		AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '        '.Append("    GROUP BY")
    '        '.Append("		 M1.INSPEC_ITEM_CD ")
    '        '.Append("		,M1.REQ_PART_CD ")
    '        '.Append("		,M2.INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP ) MM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		SM1.INSPEC_ITEM_CD  AS S_INSPEC_ITEM_CD ")
    '        '.Append("		,SM1.REQ_ITEM_CD ")
    '        '.Append("		,SM1.SVC_CD ")
    '        '.Append("		,NVL(SM1.SUGGEST_FLAG, 0) AS SUGGEST_STATUS ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_M_INSPECTION_COMB SM1 ")
    '        '.Append("	WHERE ")
    '        '.Append("		SM1.MODEL_CD IN (:MODEL_CD, :DEFAULT_MODEL_CD) ")
    '        '.Append("		AND SM1.SVC_CD = :SVC_CD) MSM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		M3.INSPEC_ITEM_CD  AS R_INSPEC_ITEM_CD ")
    '        '.Append("		,M3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
    '        '.Append("		,M3.SVC_CD AS R_SVC_CD ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_T_REPAIR_SUGGESTION_RSLT M3 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M3.DLR_CD = :DLR_CD ")
    '        '.Append("		AND M3.BRN_CD = :BRN_CD ")
    '        '.Append("		AND M3.RO_NUM = :SA_CHIP_ID ")
    '        '.Append("		AND M3.SVC_CD = :SVC_CD) MM3 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		M4.INSPEC_ITEM_CD  AS W_INSPEC_ITEM_CD ")
    '        '.Append("		,M4.SUGGEST_ICON  AS W_SUGGEST_ICON ")
    '        '.Append("		,M4.SVC_CD AS W_SVC_CD ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_W_REPAIR_SUGGESTION M4 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M4.DLR_CD = :DLR_CD ")
    '        '.Append("		AND M4.BRN_CD = :BRN_CD ")
    '        '.Append("		AND M4.STF_CD = :STF_CD ")
    '        '.Append("		AND M4.RO_NUM = :SA_CHIP_ID ")
    '        '.Append("		AND M4.SVC_CD = :SVC_CD ) MM4 ")
    '        '.Append("WHERE ")
    '        '.Append("	MM1.INSPEC_ITEM_CD = MM3.R_INSPEC_ITEM_CD (+) ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MM4.W_INSPEC_ITEM_CD (+) ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MSM1.S_INSPEC_ITEM_CD (+) ")
    '        '.Append("ORDER BY ")
    '        '.Append("	 MM1.REQ_PART_CD ")
    '        '.Append("	,MM1.REQ_ITEM_DISP_SEQ ")
    '        '.Append("	,MM1.INSPEC_ITEM_NAME ")
    '        '2014/06/17　点検項目が重複する可能性がある内容を修正　END　　↑↑↑

    '        '.Append("SELECT /* SC3250101_022 */ DISTINCT ")
    '        '.Append(" * ")
    '        '.Append("FROM ")
    '        '.Append("	(SELECT ")
    '        '.Append("		M1.INSPEC_ITEM_CD ")
    '        '.Append("		,M1.REQ_PART_CD ")
    '        '.Append("		,M2.INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
    '        '.Append("        ,M1.REQ_ITEM_DISP_SEQ ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_M_INSPECTION_COMB M1 ")
    '        '.Append("		,TB_M_INSPECTION_DETAIL M2 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M1.MODEL_CD = :MODEL_CD ")
    '        '.Append("		AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ) MM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		SM1.INSPEC_ITEM_CD  AS S_INSPEC_ITEM_CD ")
    '        '.Append("		,SM1.REQ_ITEM_CD ")
    '        '.Append("		,SM1.SVC_CD ")
    '        '.Append("        ,NVL(SM1.SUGGEST_FLAG, 0) AS SUGGEST_STATUS ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_M_INSPECTION_COMB SM1 ")
    '        '.Append("	WHERE ")
    '        '.Append("		SM1.MODEL_CD = :MODEL_CD ")
    '        '.Append("		AND SM1.SVC_CD = :SVC_CD) MSM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		M3.INSPEC_ITEM_CD  AS R_INSPEC_ITEM_CD ")
    '        '.Append("		,M3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
    '        '.Append("		,M3.SVC_CD AS R_SVC_CD ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_T_REPAIR_SUGGESTION_RSLT M3 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M3.DLR_CD = :DLR_CD ")
    '        '.Append("		AND M3.BRN_CD = :BRN_CD ")
    '        '.Append("		AND M3.RO_NUM = :SA_CHIP_ID ")
    '        '.Append("		AND M3.SVC_CD = :SVC_CD) MM3 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		M4.INSPEC_ITEM_CD  AS W_INSPEC_ITEM_CD ")
    '        '.Append("		,M4.SUGGEST_ICON  AS W_SUGGEST_ICON ")
    '        '.Append("		,M4.SVC_CD AS W_SVC_CD ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_W_REPAIR_SUGGESTION M4 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M4.DLR_CD = :DLR_CD ")
    '        '.Append("		AND M4.BRN_CD = :BRN_CD ")
    '        '.Append("		AND M4.STF_CD = :STF_CD ")
    '        '.Append("		AND M4.RO_NUM = :SA_CHIP_ID ")
    '        '.Append("		AND M4.SVC_CD = :SVC_CD ) MM4 ")
    '        '.Append("WHERE ")
    '        '.Append("	MM1.INSPEC_ITEM_CD = MM3.R_INSPEC_ITEM_CD (+) ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MM4.W_INSPEC_ITEM_CD (+) ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MSM1.S_INSPEC_ITEM_CD (+) ")
    '        '.Append("ORDER BY ")
    '        '.Append("	MM1.REQ_PART_CD ")
    '        '.Append("	,MM1.REQ_ITEM_DISP_SEQ ")

    '        '.Append("SELECT DISTINCT ")
    '        '.Append("	*  ")
    '        '.Append("FROM ")
    '        '.Append("	(SELECT /* SC3250101_022 */ ")
    '        '.Append("		M1.INSPEC_ITEM_CD ")
    '        '.Append("		,M1.REQ_PART_CD ")
    '        '.Append("		,M1.INSPEC_ITEM_CD AS SERVICE_ITEM_CD ")
    '        '.Append("		,M2.INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_M_INSPECTION_COMB M1 ")
    '        '.Append("		,TB_M_INSPECTION_DETAIL M2 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M1.MODEL_CD = :MODEL_CD ")
    '        '.Append("		AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '        '.Append("	GROUP BY ")
    '        '.Append("		M1.INSPEC_ITEM_CD ")
    '        '.Append("		,M1.REQ_PART_CD ")
    '        '.Append("		,M2.INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.SUB_INSPEC_ITEM_NAME ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '        '.Append("		,M2.DISP_INSPEC_ITEM_NEED_SWAP) MM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		SM1.INSPEC_ITEM_CD  AS S_INSPEC_ITEM_CD ")
    '        '.Append("		,SM1.REQ_ITEM_CD ")
    '        '.Append("		,SM1.SVC_CD ")
    '        '.Append("        ,NVL(SM1.SUGGEST_FLAG, 0) AS SUGGEST_STATUS ")
    '        '.Append("	FROM ")
    '        '.Append("		TB_M_INSPECTION_COMB SM1 ")
    '        '.Append("	WHERE ")
    '        '.Append("		SM1.MODEL_CD = :MODEL_CD ")
    '        '.Append("		AND SM1.SVC_CD = :SVC_CD) MSM1 ")
    '        '.Append("	,(SELECT ")
    '        '.Append("		M3.INSPEC_ITEM_CD  AS R_INSPEC_ITEM_CD ")
    '        '.Append("		,M3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
    '        '.Append("		,M3.SVC_CD AS R_SVC_CD")
    '        '.Append("	FROM ")
    '        '.Append("		TB_T_REPAIR_SUGGESTION_RSLT M3 ")
    '        '.Append("	WHERE ")
    '        '.Append("		M3.DLR_CD = :DLR_CD ")
    '        '.Append("		AND M3.BRN_CD = :BRN_CD ")
    '        '.Append("		AND M3.RO_NUM = :RO_NUM ")
    '        '.Append("		AND M3.SVC_CD = :SVC_CD) MM3 ")
    '        '.Append("	,( SELECT ")
    '        '.Append("		M4.INSPEC_ITEM_CD  AS W_INSPEC_ITEM_CD  ")
    '        '.Append("		,M4.SUGGEST_ICON  AS W_SUGGEST_ICON     ")
    '        '.Append("		,M4.SVC_CD AS W_SVC_CD")
    '        '.Append("	FROM                          ")
    '        '.Append("		TB_W_REPAIR_SUGGESTION M4 ")
    '        '.Append("	WHERE                         ")
    '        '.Append("		M4.DLR_CD = :DLR_CD       ")
    '        '.Append("		AND M4.BRN_CD = :BRN_CD   ")
    '        '.Append("		AND M4.STF_CD = :STF_CD   ")
    '        '.Append("		AND M4.RO_NUM = :RO_NUM   ")
    '        '.Append("		AND M4.SVC_CD = :SVC_CD ) MM4 ")
    '        '.Append("WHERE     ")
    '        '.Append("	MM1.INSPEC_ITEM_CD = MM3.R_INSPEC_ITEM_CD (+)     ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MM4.W_INSPEC_ITEM_CD (+) ")
    '        '.Append("	AND MM1.INSPEC_ITEM_CD = MSM1.S_INSPEC_ITEM_CD (+) ")
    '        '.Append("ORDER BY                ")
    '        '.Append("	MM1.REQ_PART_CD      ")
    '        '.Append("	, MM1.INSPEC_ITEM_CD ")
    '    End With

    '    Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable)(No)

    '        query.CommandText = sql.ToString()

    '        query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strModel_CD)
    '        'query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, strGrade_CD)
    '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
    '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
    '        query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
    '        query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
    '        query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
    '        query.AddParameterWithTypeValue("DEFAULT_MODEL_CD", OracleDbType.NVarchar2, strDefaultModel_CD)
    '        query.AddParameterWithTypeValue("DLR_CD_M", OracleDbType.NVarchar2, strDLR_CD_M)
    '        query.AddParameterWithTypeValue("BRN_CD_M", OracleDbType.NVarchar2, strBRN_CD_M)
    '        dt = query.GetData()

    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
    '        'ログ出力 End *****************************************************************************

    '        Return dt
    '    End Using

    'End Function
#End Region

	'2019/07/05　TKM要件:型式対応　START　↓↓
    '2016/10/05 新規追加　START　　↓↓↓
    ''' <summary>
    ''' 038:商品訴求点検リスト取得(母数)
    ''' </summary>
    ''' <param name="strModel_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <param name="strDLR_CD_M">販売店コード</param>
    ''' <param name="strBRN_CD_M">店舗コード</param>
    ''' <returns>商品訴求点検リスト</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_COMB_MM1_Select(
                                              ByVal strModel_CD As String _
                                            , ByVal strKatashiki As String _
                                            , ByVal strDefaultModel_CD As String _
                                            , ByVal strDLR_CD_M As String _
                                            , ByVal strBRN_CD_M As String _
                                            ) As SC3250101DataSet.MM1DataTable

        Dim No As String = "SC3250101_038"
        Dim strMethodName As String = "TB_M_INSPECTION_COMB_MM1"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.MM1DataTable
        Dim sql As New StringBuilder

        '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
        '型式を使用しない場合、紐づかない場合
        If useFlgKatashiki = False Then
            '型式値を初期値（半角スペース）にする
            strKatashiki = DEFAULT_KATASHIKI_SPACE
        End If
        '2019/12/02 NCN 吉川 TKM要件：型式対応 End
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_038 */ ")
            .Append("	 M1.INSPEC_ITEM_CD ")
            .Append("	,M1.REQ_PART_CD ")
            .Append("	,M2.INSPEC_ITEM_NAME ")
            .Append("	,M2.SUB_INSPEC_ITEM_NAME ")
            .Append("	,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_INSPEC, '0')) AS DISP_INSPEC_ITEM_NEED_INSPEC ")
            .Append("	,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_REPLACE, '0')) AS DISP_INSPEC_ITEM_NEED_REPLACE ")
            .Append("	,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_FIX, '0')) AS DISP_INSPEC_ITEM_NEED_FIX ")
            .Append("	,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_CLEAN, '0')) AS DISP_INSPEC_ITEM_NEED_CLEAN ")
            .Append("	,TO_NUMBER(NVL(M2.DISP_INSPEC_ITEM_NEED_SWAP, '0')) AS DISP_INSPEC_ITEM_NEED_SWAP ")
            .Append("	,MAX(CASE ")
            .Append("	 WHEN M1.MODEL_CD = :MODEL_CD THEN NVL(M1.REQ_ITEM_DISP_SEQ, 0) ")
            .Append("	 ELSE NULL ")
            .Append("	 END )AS REQ_ITEM_DISP_SEQ ")
            .Append("FROM ")
            .Append("	 TB_M_INSPECTION_COMB M1 ")
            .Append("	,TB_M_FINAL_INSPECTION_DETAIL M2 ")
            .Append("WHERE ")
            .Append("	M1.MODEL_CD IN (:MODEL_CD, :DEFAULT_MODEL_CD) ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("	AND M1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("	AND M1.DLR_CD = :DLR_CD_M ")
            .Append("	AND M1.BRN_CD = :BRN_CD_M ")
            .Append("	AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
            .Append("GROUP BY ")
            .Append("	 M1.INSPEC_ITEM_CD ")
            .Append("	,M1.REQ_PART_CD ")
            .Append("	,M2.INSPEC_ITEM_NAME ")
            .Append("	,M2.SUB_INSPEC_ITEM_NAME ")
            .Append("	,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
            .Append("	,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
            .Append("	,M2.DISP_INSPEC_ITEM_NEED_FIX ")
            .Append("	,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
            .Append("	,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
            .Append("ORDER BY ")
            .Append("	 REQ_PART_CD ")
            .Append("	,REQ_ITEM_DISP_SEQ ")
            .Append("	,INSPEC_ITEM_NAME ")

        End With
        Using query As New DBSelectQuery(Of SC3250101DataSet.MM1DataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strModel_CD)
            query.AddParameterWithTypeValue("DEFAULT_MODEL_CD", OracleDbType.NVarchar2, strDefaultModel_CD)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, strKatashiki)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            query.AddParameterWithTypeValue("DLR_CD_M", OracleDbType.NVarchar2, strDLR_CD_M)
            query.AddParameterWithTypeValue("BRN_CD_M", OracleDbType.NVarchar2, strBRN_CD_M)
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 039:商品訴求点検リスト取得(デフォルトモデルコード)
    ''' </summary>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDLR_CD_M">販売店コード（商品訴求初期表示用アイテム取得時）</param>
    ''' <param name="strBRN_CD_M">店舗コード（商品訴求初期表示用アイテム取得時）</param>
    ''' <returns>商品訴求点検リスト(デフォルトモデルコード)</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_COMB_MSM1_Select(
                                             ByVal strDefaultModel_CD As String _
                                            , ByVal strSVC_CD As String _
                                            , ByVal strDLR_CD_M As String _
                                            , ByVal strBRN_CD_M As String _
                                            ) As SC3250101DataSet.MSM1DataTable

        Dim No As String = "SC3250101_039"
        Dim strMethodName As String = "TB_M_INSPECTION_COMB_MSM1"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.MSM1DataTable
        Dim sql As New StringBuilder




        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_039 */ ")
            .Append("	 SM1.INSPEC_ITEM_CD ")
            .Append("	,SM1.REQ_ITEM_CD AS REQ_ITEM_CD_DEFAULT ")
            .Append("	,SM1.SVC_CD AS SVC_CD_DEFAULT")
            .Append("	,NVL(SM1.SUGGEST_FLG, 0) AS SUGGEST_FLAG_DEFAULT ")
            .Append("FROM ")
            .Append("	TB_M_INSPECTION_COMB SM1 ")
            .Append("WHERE ")
            .Append("	SM1.MODEL_CD = :DEFAULT_MODEL_CD ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("	AND SM1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("	AND SM1.SVC_CD = :SVC_CD ")
            .Append("	AND SM1.DLR_CD = :DLR_CD_M ")
            .Append("	AND SM1.BRN_CD = :BRN_CD_M ")

        End With
        Using query As New DBSelectQuery(Of SC3250101DataSet.MSM1DataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DEFAULT_MODEL_CD", OracleDbType.NVarchar2, strDefaultModel_CD)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, DEFAULT_KATASHIKI_SPACE)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("DLR_CD_M", OracleDbType.NVarchar2, strDLR_CD_M)
            query.AddParameterWithTypeValue("BRN_CD_M", OracleDbType.NVarchar2, strBRN_CD_M)
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using
    End Function
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 040:商品訴求点検リスト取得(モデルコード)
    ''' </summary>
    ''' <param name="strModel_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDLR_CD_M">販売店コード（商品訴求初期表示用アイテム取得時）</param>
    ''' <param name="strBRN_CD_M">店舗コード（商品訴求初期表示用アイテム取得時）</param>
    ''' <returns>商品訴求点検リスト(モデルコード)</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_COMB_MSM2_Select(
                                              ByVal strModel_CD As String _
                                            , ByVal strKatashiki As String _
                                            , ByVal strSVC_CD As String _
                                            , ByVal strDLR_CD_M As String _
                                            , ByVal strBRN_CD_M As String _
                                            ) As SC3250101DataSet.MSM2DataTable

        Dim No As String = "SC3250101_040"
        Dim strMethodName As String = "TB_M_INSPECTION_COMB_MSM2"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.MSM2DataTable
        Dim sql As New StringBuilder

        '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
        '型式を使用しない場合、紐づかない場合
        If useFlgKatashiki = False Then
            '型式値を初期値（半角スペース）にする
            strKatashiki = DEFAULT_KATASHIKI_SPACE
        End If
        '2019/12/02 NCN 吉川 TKM要件：型式対応 End

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_040 */ ")
            .Append("	 SM1.INSPEC_ITEM_CD ")
            .Append("	,SM1.REQ_ITEM_CD ")
            .Append("	,SM1.SVC_CD ")
            .Append("	,NVL(SM1.SUGGEST_FLG, 0) AS SUGGEST_FLAG ")
            .Append("FROM ")
            .Append("	TB_M_INSPECTION_COMB SM1 ")
            .Append("WHERE ")
            .Append("	SM1.MODEL_CD = :MODEL_CD ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("	 AND    SM1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("	AND SM1.SVC_CD = :SVC_CD ")
            .Append("	AND SM1.DLR_CD = :DLR_CD_M ")
            .Append("	AND SM1.BRN_CD = :BRN_CD_M ")

        End With
        Using query As New DBSelectQuery(Of SC3250101DataSet.MSM2DataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strModel_CD)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, strKatashiki)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("DLR_CD_M", OracleDbType.NVarchar2, strDLR_CD_M)
            query.AddParameterWithTypeValue("BRN_CD_M", OracleDbType.NVarchar2, strBRN_CD_M)
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 041:商品訴求点検リスト取得(実績データ)
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <returns>商品訴求点検リスト(実績データ)</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_COMB_MM3_Select(
                                             ByVal strDLR_CD As String _
                                            , ByVal strBRN_CD As String _
                                            , ByVal strSAChipID As String _
                                            , ByVal strSVC_CD As String _
                                            ) As SC3250101DataSet.MM3DataTable

        Dim No As String = "SC3250101_041"
        Dim strMethodName As String = "TB_M_INSPECTION_COMB_MM3"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.MM3DataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_041 */ ")
            .Append("	M3.INSPEC_ITEM_CD  AS INSPEC_ITEM_CD ")
            .Append("	,M3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
            .Append("	,M3.SVC_CD AS R_SVC_CD ")
            .Append("FROM ")
            .Append("   TB_T_REPAIR_SUGGESTION_RSLT M3 ")
            .Append("WHERE ")
            .Append("	M3.DLR_CD = :DLR_CD ")
            .Append("	AND M3.BRN_CD = :BRN_CD ")
            .Append("	AND M3.RO_NUM = :SA_CHIP_ID ")
            .Append("	AND M3.SVC_CD = :SVC_CD ")

            .Append(" UNION ALL ")

            .Append("SELECT ")
            .Append("    TH3.INSPEC_ITEM_CD  AS INSPEC_ITEM_CD ")
            .Append("    ,TH3.SUGGEST_ICON  AS R_SUGGEST_ICON ")
            .Append("    ,TH3.SVC_CD AS R_SVC_CD ")
            .Append("FROM ")
            .Append("   TB_H_REPAIR_SUGGESTION_RSLT TH3 ")
            .Append("WHERE ")
            .Append("    TH3.DLR_CD = :DLR_CD ")
            .Append("    AND TH3.BRN_CD = :BRN_CD ")
            .Append("    AND TH3.RO_NUM = :SA_CHIP_ID ")
            .Append("    AND TH3.SVC_CD = :SVC_CD ")

        End With
        Using query As New DBSelectQuery(Of SC3250101DataSet.MM3DataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using
    End Function
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 042:商品訴求点検リスト取得(一時WK)
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <returns>商品訴求点検リスト(一時WK)</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_COMB_MM4_Select(
                                             ByVal strDLR_CD As String _
                                            , ByVal strBRN_CD As String _
                                            , ByVal strSTF_CD As String _
                                            , ByVal strSAChipID As String _
                                            , ByVal strSVC_CD As String _
                                            ) As SC3250101DataSet.MM4DataTable

        Dim No As String = "SC3250101_042"
        Dim strMethodName As String = "TB_M_INSPECTION_COMB_MM4"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.MM4DataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_042 */ ")
            .Append("	M4.INSPEC_ITEM_CD  AS INSPEC_ITEM_CD ")
            .Append("	,M4.SUGGEST_ICON  AS W_SUGGEST_ICON ")
            .Append("	,M4.SVC_CD AS W_SVC_CD ")
            .Append("FROM ")
            .Append("	TB_W_REPAIR_SUGGESTION M4 ")
            .Append("WHERE ")
            .Append("	M4.DLR_CD = :DLR_CD ")
            .Append("	AND M4.BRN_CD = :BRN_CD ")
            .Append("	AND M4.STF_CD = :STF_CD ")
            .Append("	AND M4.RO_NUM = :SA_CHIP_ID ")
            .Append("	AND M4.SVC_CD = :SVC_CD ")


        End With
        Using query As New DBSelectQuery(Of SC3250101DataSet.MM4DataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return dt
        End Using
    End Function

    '2016/10/05 新規追加　END　　↑↑↑


#Region "未使用メソッド"
    '2014/06/09 未使用メソッドのコメント化　START　↓↓↓
    ' ''' <summary>
    ' ''' 商品訴求点検リスト取得
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_COMB_SelectList(
    '                                          ByVal strModel_CD As String _
    '                                        , ByVal strGrade_CD As String _
    '                                        ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    Dim No As String = "SC3250101_022"
    '    Dim strMethodName As String = "TB_M_INSPECTION_COMB_SelectList"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_022 */ ")
    '            .Append("   M1.INSPEC_ITEM_CD ")
    '            .Append("  ,M1.REQ_PART_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　START　↓↓↓
    '            .Append("  ,M1.INSPEC_ITEM_CD AS SERVICE_ITEM_CD ")
    '            '.Append("  ,M1.SERVICE_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　END　　↑↑↑
    '            .Append("  ,M2.INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.SUB_INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
    '            .Append("FROM ")
    '            .Append("   TB_M_INSPECTION_COMB M1 ")
    '            .Append("  ,TB_M_INSPECTION_DETAIL M2 ")
    '            .Append("WHERE ")
    '            .Append("      M1.MODEL_CD = :MODEL_CD ")
    '            '.Append("  AND M1.GRADE_CD = :GRADE_CD ")
    '            .Append("  AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '            .Append("GROUP BY ")
    '            .Append("   M1.INSPEC_ITEM_CD ")
    '            .Append("  ,M1.REQ_PART_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　START　↓↓↓
    '            '.Append("  ,M1.SERVICE_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　END　　↑↑↑
    '            .Append("  ,M2.INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.SUB_INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_INSPEC ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_REPLACE ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_FIX ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_CLEAN ")
    '            .Append("  ,M2.DISP_INSPEC_ITEM_NEED_SWAP ")
    '            .Append("ORDER BY ")
    '            .Append("  M1.REQ_PART_CD,M1.INSPEC_ITEM_CD ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append(" M1.INSPEC_ITEM_CD                           ")
    '            '.Append(", M1.REQ_PART_CD                             ")
    '            '.Append(", M1.SERVICE_ITEM_CD                         ")
    '            '.Append(", M2.INSPEC_ITEM_NAME                        ")
    '            '.Append(", M2.SUB_INSPEC_ITEM_NAME                    ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_INSPEC")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_REPLACE            ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_FIX                ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_CLEAN              ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_SWAP               ")
    '            '.Append(" FROM                                        ")
    '            '.Append(" TB_M_INSPECTION_COMB    M1,                 ")
    '            '.Append(" TB_M_INSPECTION_DETAIL  M2                  ")
    '            '.Append(" WHERE                                       ")
    '            '.AppendFormat(" M1.MODEL_CD='{0}' AND                         ", strModel_CD)
    '            ''.AppendFormat(" M1.GRADE_CD='{0}' AND                         ", strGrade_CD)
    '            '.Append(" M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD       ")
    '            '.Append("GROUP BY                                     ")
    '            '.Append(" M1.INSPEC_ITEM_CD                           ")
    '            '.Append(", M1.REQ_PART_CD                             ")
    '            '.Append(", M1.SERVICE_ITEM_CD                         ")
    '            '.Append(", M2.INSPEC_ITEM_NAME                        ")
    '            '.Append(", M2.SUB_INSPEC_ITEM_NAME                    ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_INSPEC")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_REPLACE            ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_FIX                ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_CLEAN              ")
    '            '.Append(", M2.DISP_INSPEC_ITEM_NEED_SWAP               ")
    '            '.Append("ORDER BY  M1.REQ_PART_CD,M1.INSPEC_ITEM_CD   ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strModel_CD)
    '            'query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, strGrade_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求初期表示用アイテムを取得
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function TB_M_INSPECTION_COMB_DefaultList(
    '                                          ByVal strModel_CD As String _
    '                                        , ByVal strGrade_CD As String _
    '                                        , ByVal strSVC_CD As String _
    '                                        ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    Dim No As String = "SC3250101_023"
    '    Dim strMethodName As String = "TB_M_INSPECTION_COMB_DefaultList"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_023 */ ")
    '            .Append("   M1.INSPEC_ITEM_CD ")
    '            .Append("  ,M1.REQ_PART_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　START　↓↓↓
    '            .Append("  ,M1.INSPEC_ITEM_CD AS SERVICE_ITEM_CD ")
    '            '.Append("  ,M1.SERVICE_ITEM_CD ")
    '            '2014/05/26 「SERVICE_ITEM_CD」を「INSPEC_ITEM_CD」から取得　END　　↑↑↑
    '            .Append("  ,M2.INSPEC_ITEM_NAME ")
    '            .Append("  ,M2.SUB_INSPEC_ITEM_NAME ")
    '            .Append("  ,M1.REQ_ITEM_CD ")
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　START　↓↓↓
    '            .Append("  ,M1.SUGGEST_FLAG AS SUGGEST_STATUS ")
    '            '.Append("  ,M1.SUGGEST_STATUS ")
    '            '2014/05/26 カラム名変更「SUGGEST_STATUS→SUGGEST_FLAG」　END　　↑↑↑
    '            .Append("FROM ")
    '            .Append("   TB_M_INSPECTION_COMB M1 ")
    '            .Append("  ,TB_M_INSPECTION_DETAIL M2 ")
    '            .Append("WHERE ")
    '            .Append("      M1.MODEL_CD = :MODEL_CD ")
    '            '.Append("  AND M1.GRADE_CD = :GRADE_CD ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
    '            .Append("  AND M1.SVC_CD = :SVC_CD ")
    '            '.Append("  AND M1.INSPEC_TYPE = :INSPEC_TYPE ")
    '            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
    '            .Append("  AND M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
    '            '.Append("ORDER BY ")
    '            '.Append("   M1.REQ_PART_CD ")
    '            '.Append("  ,M1.INSPEC_ITEM_CD ")

    '            '.AppendFormat("SELECT /* {0} */ ", No)
    '            '.Append(" M1.INSPEC_ITEM_CD                           ")
    '            '.Append(", M1.REQ_PART_CD                             ")
    '            '.Append(", M1.SERVICE_ITEM_CD                         ")
    '            '.Append(", M2.INSPEC_ITEM_NAME                        ")
    '            '.Append(", M2.SUB_INSPEC_ITEM_NAME                    ")
    '            '.Append(", M1.REQ_ITEM_CD      ") '--商品訴求初期表示用アイテム
    '            '.Append(", M1.SUGGEST_STATUS   ") '--推奨点検ステータス
    '            '.Append(" FROM                                        ")
    '            '.Append(" TB_M_INSPECTION_COMB    M1,                 ")
    '            '.Append(" TB_M_INSPECTION_DETAIL  M2                  ")
    '            '.Append(" WHERE                                       ")
    '            '.AppendFormat(" M1.MODEL_CD='{0}' AND                         ", strModel_CD)
    '            ''.AppendFormat(" M1.GRADE_CD='{0}' AND                         ", strGrade_CD)
    '            '.AppendFormat(" M1.INSPEC_TYPE='{0}' AND                         ", strInspec_Type)
    '            '.Append(" M1.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD       ")
    '            '.Append("ORDER BY  M1.REQ_PART_CD,M1.INSPEC_ITEM_CD   ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strModel_CD)
    '            'query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.NVarchar2, strGrade_CD)
    '            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
    '2014/06/09 未使用メソッドのコメント化　END　　↑↑↑
#End Region

    ''' <summary>
    ''' 024:完成検査結果詳細データ取得
    ''' </summary>
    ''' <param name="strVIN_NO">VINコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_T_FINAL_INSPECTION_DETAIL_Select(
                                              ByVal strVIN_NO As String _
                                            ) As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable

        Dim No As String = "SC3250101_024"
        Dim strMethodName As String = "TB_T_FINAL_INSPECTION_DETAIL_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql

            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更（RO_NUMを追加）　START　↓↓↓
            '2014/05/20 完成検査結果データ取得変更　START　↓↓↓
            .Append("SELECT /* SC3250101_024 */ ")
            .Append("   T1.SVCIN_ID ")
            .Append("  ,T1.DLR_CD ")
            .Append("  ,T1.BRN_CD ")
            .Append("  ,T4.RO_NUM ")
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            '.Append("  ,T1.RO_NUM ")
            '.Append("  ,T1.VCL_ID ")
            '.Append("  ,M1.VCL_VIN ")
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑
            .Append("  ,T3.JOB_DTL_ID ")
            .Append("  ,T3.INSPEC_ITEM_CD ")
            .Append("  ,T3.INSPEC_RSLT_CD ")
            .Append("  ,T3.OPERATION_RSLT_ALREADY_REPLACE ")
            .Append("  ,T3.OPERATION_RSLT_ALREADY_FIX ")
            .Append("  ,T3.OPERATION_RSLT_ALREADY_CLEAN ")
            .Append("  ,T3.OPERATION_RSLT_ALREADY_SWAP ")
            .Append("  ,TO_NUMBER(NVL(M2.DISP_OPE_ITEM_ALREADY_REPLACE, '0')) AS DISP_OPE_ITEM_ALREADY_REPLACE ")
            .Append("  ,TO_NUMBER(NVL(M2.DISP_OPE_ITEM_ALREADY_FIX, '0')) AS DISP_OPE_ITEM_ALREADY_FIX ")
            .Append("  ,TO_NUMBER(NVL(M2.DISP_OPE_ITEM_ALREADY_CLEAN, '0')) AS DISP_OPE_ITEM_ALREADY_CLEAN ")
            .Append("  ,TO_NUMBER(NVL(M2.DISP_OPE_ITEM_ALREADY_SWAP, '0')) AS DISP_OPE_ITEM_ALREADY_SWAP ")
            .Append("FROM ")
            .Append("   TB_M_VEHICLE M1 ")
            .Append("  ,TB_T_SERVICEIN T1 ")
            .Append("  ,TB_T_JOB_DTL T2 ")
            .Append("  ,TB_T_FINAL_INSPECTION_DETAIL T3 ")
            .Append("  ,TB_M_FINAL_INSPECTION_DETAIL M2 ")
            .Append("  ,TB_T_RO_INFO T4 ")
            .Append("WHERE ")
            .Append("      M1.VCL_VIN = :VCL_VIN ")
            .Append("  AND M1.VCL_ID = T1.VCL_ID ")
            .Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
            '2014/05/26 「DLR_CD」と「BRN_CD」を削除　START　↓↓↓
            '.Append("  AND T1.DLR_CD = T3.DLR_CD ")
            '.Append("  AND T1.BRN_CD = T3.BRN_CD ")
            '2014/05/26 「DLR_CD」と「BRN_CD」を削除　END　　↑↑↑
            .Append("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            .Append("  AND T3.INSPEC_ITEM_CD = M2.INSPEC_ITEM_CD ")
            .Append("  AND T1.SVCIN_ID = T4.SVCIN_ID ")
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

            '.Append("SELECT /* SC3250101_024 */ ")
            '.Append("   T1.SVCIN_ID ")
            '.Append("  ,T1.DLR_CD ")
            '.Append("  ,T1.BRN_CD ")
            '.Append("  ,T1.RO_NUM ")
            '.Append("  ,T1.VCL_ID ")
            '.Append("  ,M1.VCL_VIN ")
            '.Append("  ,T3.JOB_DTL_ID ")
            '.Append("  ,T3.INSPEC_ITEM_CD ")
            '.Append("  ,T3.INSPEC_RSLT_CD ")
            '.Append("  ,T3.OPERATION_RSLT_ALREADY_REPLACE ")
            '.Append("  ,T3.OPERATION_RSLT_ALREADY_FIX ")
            '.Append("  ,T3.OPERATION_RSLT_ALREADY_CLEAN ")
            '.Append("  ,T3.OPERATION_RSLT_ALREADY_SWAP ")
            '.Append("FROM ")
            '.Append("   TB_M_VEHICLE M1 ")
            '.Append("  ,TB_T_SERVICEIN T1 ")
            '.Append("  ,TB_T_JOB_DTL T2 ")
            '.Append("  ,TB_T_INSPECTION_DETAIL T3 ")
            '.Append("WHERE ")
            '.Append("      M1.VCL_VIN = :VCL_VIN ")
            '.Append("  AND M1.VCL_ID = T1.VCL_ID ")
            '.Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
            '.Append("  AND T1.DLR_CD = T3.DLR_CD ")
            '.Append("  AND T1.BRN_CD = T3.BRN_CD ")
            '.Append("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            '2014/05/20 完成検査結果データ取得変更　　END　↑↑↑

            '.AppendFormat("SELECT /* {0} */ ", No)
            '.Append(" T1.SVCIN_ID,                      ")
            '.Append(" T1.DLR_CD,                        ")
            '.Append(" T1.BRN_CD,                        ")
            '.Append(" T1.RO_NUM,                        ")
            '.Append(" T1.VCL_ID,                        ")
            '.Append(" M1.VCL_VIN,                       ")
            '.Append(" T3.JOB_DTL_ID,                    ")
            '.Append(" T3.INSPEC_ITEM_CD,                ")
            '.Append(" T3.INSPEC_RSLT_CD                 ")
            '.Append(" FROM                              ")
            '.Append(" TB_M_VEHICLE M1,                  ")
            '.Append(" TB_T_SERVICEIN T1,                ")
            '.Append(" TB_T_JOB_DTL T2,                  ")
            '.Append(" TB_T_INSPECTION_DETAIL T3         ")
            '.Append(" WHERE                             ")
            '.AppendFormat(" M1.VCL_VIN = '{0}' AND         ", strVIN_NO)
            '.Append(" M1.VCL_ID = T1.VCL_ID AND         ")
            '.Append(" T1.SVCIN_ID = T2.SVCIN_ID AND     ")
            '.Append(" T1.DLR_CD = T3.DLR_CD AND         ")
            '.Append(" T1.BRN_CD = T3.BRN_CD AND         ")
            '.Append(" T2.JOB_DTL_ID = T3.JOB_DTL_ID     ")

            .Append(" UNION ALL ")

            .Append("SELECT ")
            .Append("   H1.SVCIN_ID ")
            .Append("  ,H1.DLR_CD ")
            .Append("  ,H1.BRN_CD ")
            .Append("  ,H4.RO_NUM ")
            .Append("  ,H3.JOB_DTL_ID ")
            .Append("  ,H3.INSPEC_ITEM_CD ")
            .Append("  ,H3.INSPEC_RSLT_CD ")
            .Append("  ,H3.OPERATION_RSLT_ALREADY_REPLACE ")
            .Append("  ,H3.OPERATION_RSLT_ALREADY_FIX ")
            .Append("  ,H3.OPERATION_RSLT_ALREADY_CLEAN ")
            .Append("  ,H3.OPERATION_RSLT_ALREADY_SWAP ")
            .Append("  ,TO_NUMBER(NVL(HM2.DISP_OPE_ITEM_ALREADY_REPLACE, '0')) AS DISP_OPE_ITEM_ALREADY_REPLACE ")
            .Append("  ,TO_NUMBER(NVL(HM2.DISP_OPE_ITEM_ALREADY_FIX, '0')) AS DISP_OPE_ITEM_ALREADY_FIX ")
            .Append("  ,TO_NUMBER(NVL(HM2.DISP_OPE_ITEM_ALREADY_CLEAN, '0')) AS DISP_OPE_ITEM_ALREADY_CLEAN ")
            .Append("  ,TO_NUMBER(NVL(HM2.DISP_OPE_ITEM_ALREADY_SWAP, '0')) AS DISP_OPE_ITEM_ALREADY_SWAP ")
            .Append("FROM ")
            .Append("   TB_M_VEHICLE HM1 ")
            .Append("  ,TB_H_SERVICEIN H1 ")
            .Append("  ,TB_H_JOB_DTL H2 ")
            .Append("  ,TB_H_FINAL_INSPECTION_DETAIL H3 ")
            .Append("  ,TB_M_FINAL_INSPECTION_DETAIL HM2 ")
            .Append("  ,TB_H_RO_INFO H4 ")
            .Append("WHERE ")
            .Append("      HM1.VCL_VIN = :VCL_VIN ")
            .Append("  AND HM1.VCL_ID = H1.VCL_ID ")
            .Append("  AND H1.SVCIN_ID = H2.SVCIN_ID ")
            .Append("  AND H2.JOB_DTL_ID = H3.JOB_DTL_ID ")
            .Append("  AND H3.INSPEC_ITEM_CD = HM2.INSPEC_ITEM_CD ")
            .Append("  AND H1.SVCIN_ID = H4.SVCIN_ID ")

        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, strVIN_NO)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function


    ''' <summary>
    ''' 025:画面URL情報取得
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_DISP_RELATION_Select(ByVal inDisplayNumber As Long) As SC3250101DisplayRelationDataTable

        Dim No As String = "SC3250101_025"
        Dim strMethodName As String = "TB_M_DISP_RELATION_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As SC3250101DisplayRelationDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_025 */ ")
            .Append("   DMS_DISP_ID ")
            .Append("  ,DMS_DISP_URL ")
            .Append("FROM ")
            .Append("  TB_M_DISP_RELATION ")
            .Append("WHERE ")
            .Append("  DMS_DISP_ID = :DMS_DISP_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3250101DisplayRelationDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DMS_DISP_ID", OracleDbType.Long, inDisplayNumber)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function


    ''' <summary>
    ''' 026:過去実績一覧（Result一覧）取得
    ''' </summary>
    ''' <param name="strVIN_NO">VINコード</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function ResultList_Select(
                                              ByVal strVIN_NO As String _
                                            , ByVal specifyDlrCdFlgs As Boolean _
                                            ) As SC3250101DataSet.ResultListDataTable

        Dim No As String = "SC3250101_026"
        Dim strMethodName As String = " TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim dt As New SC3250101DataSet.ResultListDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
            .Append("SELECT DISTINCT  /* SC3250101_026 */ ")
            .Append("	M2.SVC_CD ")
            .Append("	,T1.SVCIN_ID ")
            .Append("	,T1.DLR_CD ")
            .Append("	,T1.BRN_CD ")
            .Append("	,T6.RO_NUM ")
            .Append("	,T2.JOB_DTL_ID ")
            .Append("	,T1.RSLT_SVCIN_DATETIME ")
            .Append("	,NVL(T4.REG_MILE, 0) AS REG_MILE ")
            .Append("	,NVL(CONCAT(TRIM(M2.UPPER_DISP), TRIM(M2.LOWER_DISP)), ' ') AS MERCHANDISENAME ")
            .Append("	,M2.MERC_NAME ")
            .Append("	,T1.SVCIN_MILE ")
            .Append("	,(SELECT COUNT(1) FROM TB_M_SERVICE WHERE DLR_CD = T1.DLR_CD AND SVC_CD=M2.SVC_CD) AS SERVICE ")
            .Append("	,M4.INSPEC_ORDER ")
            .Append("FROM ")
            .Append("	TB_M_VEHICLE M1 ")
            .Append("	,TB_T_SERVICEIN T1 ")
            .Append("	,TB_T_JOB_DTL T2 ")
            .Append("	,TB_T_VEHICLE_SVCIN_HIS T3 ")
            .Append("	,TB_T_VEHICLE_MILEAGE T4 ")
            .Append("	,TB_T_JOB_INSTRUCT T5 ")
            .Append("	,TB_M_MERCHANDISE M2 ")
            .Append("	,TB_M_MAINTE_ATTR M3 ")
            .Append("	,(SELECT RO_NUM, SVCIN_ID FROM TB_T_RO_INFO WHERE RO_SEQ=0) T6 ")
            .Append("	,TB_M_INSPECTION_ORDER M4 ")
            .Append("WHERE ")
            .Append("	M1.VCL_VIN = :VCL_VIN ")
            .Append("	AND M1.VCL_ID = T1.VCL_ID ")
            .Append("	AND T1.SVCIN_ID = T2.SVCIN_ID ")
            .Append("	AND T2.JOB_DTL_ID = T5.JOB_DTL_ID ")
            .Append("	AND T1.DLR_CD = T3.DLR_CD(+) ")
            .Append("	AND T1.CST_ID = T3.CST_ID(+) ")
            .Append("	AND T1.VCL_ID = T3.VCL_ID(+) ")
            .Append("	AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+) ")
            .Append("	AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ")
            '2015/04/14 新販売店追加対応 start
            '.Append("	AND M3.DLR_CD = T1.DLR_CD ")
            '整備属性マスタに登録済みならば指定販売店で検索、無ければ全販売店で検索
            If specifyDlrCdFlgs = True Then
                .Append("	AND M3.DLR_CD = T1.DLR_CD ")
            Else
                .Append("	AND M3.DLR_CD = '" & AllDealer & "' ")
            End If
            '2015/04/14 新販売店追加対応 end
            .Append("	AND M3.MAINTE_CD = T5.JOB_CD ")
            .Append("	AND (M3.MAINTE_KATASHIKI = SUBSTR(M1.VCL_KATASHIKI, 1, INSTR(M1.VCL_KATASHIKI, '-') - 1 ) ")
            .Append("		OR M3.MAINTE_KATASHIKI = 'X') ")
            .Append("	AND M3.MERC_ID = M2.MERC_ID ")
            .Append("	AND T1.SVCIN_ID = T6.SVCIN_ID(+) ")
            .Append("	AND M2.SVC_CD = M4.SVC_CD(+) ")

            ''2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            '.Append("SELECT DISTINCT  /* SC3250101_026 */ ")
            '.Append("	T1.SVCIN_ID ")
            '.Append("	,T1.DLR_CD ")
            '.Append("	,T1.BRN_CD ")
            '.Append("	,T6.RO_NUM ")
            '.Append("	,T2.JOB_DTL_ID ")
            '.Append("	,T1.RSLT_SVCIN_DATETIME ")
            '.Append("	,NVL(T4.REG_MILE, 0) AS REG_MILE ")
            '.Append("	,T3.SVCIN_NUM ")
            '.Append("	,SUBSTR(M1.VCL_KATASHIKI, 1, INSTR(M1.VCL_KATASHIKI, '-') - 1 ) AS VCL_KATASHIKI ")
            '.Append("	,T5.JOB_CD ")
            '.Append("	,NVL(CONCAT(TRIM(M2.UPPER_DISP), TRIM(M2.LOWER_DISP)), ' ') AS MERCHANDISENAME ")
            '.Append("	,M2.MERC_NAME ")
            '.Append("	,T1.SVCIN_MILE ")
            '.Append("FROM ")
            '.Append("	TB_M_VEHICLE M1 ")
            '.Append("	,TB_T_SERVICEIN T1 ")
            '.Append("	,TB_T_JOB_DTL T2 ")
            '.Append("	,TB_T_VEHICLE_SVCIN_HIS T3 ")
            '.Append("	,TB_T_VEHICLE_MILEAGE T4 ")
            '.Append("	,TB_T_JOB_INSTRUCT T5 ")
            '.Append("	,TB_M_MERCHANDISE M2 ")
            '.Append("	,(SELECT * FROM TB_T_RO_INFO WHERE RO_SEQ=0) T6 ")
            '.Append("WHERE ")
            '.Append("	M1.VCL_VIN = :VCL_VIN ")
            '.Append("	AND M1.VCL_ID = T1.VCL_ID(+) ")
            '.Append("	AND T1.SVCIN_ID = T2.SVCIN_ID(+) ")
            '.Append("	AND T2.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
            '.Append("	AND T1.DLR_CD = T3.DLR_CD(+) ")
            '.Append("	AND T1.CST_ID = T3.CST_ID(+) ")
            '.Append("	AND T1.VCL_ID = T3.VCL_ID(+) ")
            '.Append("	AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+) ")
            '.Append("	AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ")
            '.Append("	AND T2.MERC_ID=M2.MERC_ID(+) ")
            '.Append("	AND T1.SVCIN_ID = T6.SVCIN_ID(+) ")
            '.Append("ORDER BY ")
            '.Append("	T1.RSLT_SVCIN_DATETIME ")
            '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

            '2014/05/21 「Result一覧」に商品名追加　START　↓↓↓

            '.AppendLine("SELECT DISTINCT  /* SC3250101_026 */ ")
            '.AppendLine("            M1.VCL_VIN ")
            '.AppendLine("           ,T1.DLR_CD ")
            '.AppendLine("           ,T1.BRN_CD ")
            '.AppendLine("           ,T1.RO_NUM ")
            '.AppendLine("           ,T2.JOB_DTL_ID ")
            '.AppendLine("           ,T1.RSLT_SVCIN_DATETIME ")
            '.AppendLine("           ,NVL(T4.REG_MILE, 0) AS REG_MILE ")
            '.AppendLine("           ,T3.SVCIN_NUM ")
            '.AppendLine("           ,SUBSTR(M1.VCL_KATASHIKI, 1, INSTR(M1.VCL_KATASHIKI, '-') - 1 ) AS VCL_KATASHIKI ")
            '.AppendLine("           ,T5.JOB_CD")
            '.AppendLine("           ,NVL(CONCAT(TRIM(M2.UPPER_DISP), TRIM(M2.LOWER_DISP)), ' ') AS MERCHANDISENAME ")
            '.AppendLine("           ,M2.MERC_NAME")
            '.AppendLine("           ,T1.SVCIN_MILE ")
            '.AppendLine("        FROM ")
            '.AppendLine("            TB_M_VEHICLE M1 ")
            '.AppendLine("           ,TB_T_SERVICEIN T1 ")
            '.AppendLine("           ,TB_T_JOB_DTL T2 ")
            '.AppendLine("           ,TB_T_VEHICLE_SVCIN_HIS T3 ")
            '.AppendLine("           ,TB_T_VEHICLE_MILEAGE T4 ")
            '.AppendLine("           ,TB_T_JOB_INSTRUCT T5 ")
            '.AppendLine("           ,TB_M_MERCHANDISE M2")
            '.AppendLine("        WHERE ")
            '.AppendLine("            M1.VCL_VIN = :VCL_VIN ")
            '.AppendLine("        AND M1.VCL_ID = T1.VCL_ID(+) ")
            '.AppendLine("        AND T1.SVCIN_ID = T2.SVCIN_ID(+) ")
            '.AppendLine("        AND T2.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
            '.AppendLine("        AND T1.DLR_CD = T3.DLR_CD(+) ")
            '.AppendLine("        AND T1.CST_ID = T3.CST_ID(+) ")
            '.AppendLine("        AND T1.VCL_ID = T3.VCL_ID(+) ")
            '.AppendLine("        AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+) ")
            '.AppendLine("        AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ")
            '.AppendLine("        AND T2.MERC_ID=M2.MERC_ID(+)")
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑

            '.Append("SELECT /* SC3250101_026 */  ")
            '.Append("   T7.DLR_CD  ")
            '.Append("  ,T7.BRN_CD  ")
            '.Append("  ,T7.RO_NUM  ")
            '.Append("  ,T7.JOB_DTL_ID  ")
            '.Append("  ,T7.RSLT_SVCIN_DATETIME  ")
            '.Append("  ,T7.REG_MILE  ")
            '.Append("  ,T7.SVCIN_NUM  ")
            '.Append("  ,T7.SVC_CD ")
            '.Append("  ,T7.MERCHANDISENAME  ")
            '.Append("  ,T7.MERC_NAME ")
            '.Append("  ,M5.SVC_NAME_MILE ")
            '.Append("FROM ")
            '.Append("   (SELECT  ")
            '.Append("        T6.DLR_CD  ")
            '.Append("       ,T6.BRN_CD  ")
            '.Append("       ,T6.RO_NUM  ")
            '.Append("       ,T6.JOB_DTL_ID  ")
            '.Append("       ,T6.RSLT_SVCIN_DATETIME  ")
            '.Append("       ,T6.REG_MILE  ")
            '.Append("       ,T6.SVCIN_NUM  ")
            '.Append("       ,M4.SVC_CD ")
            '.Append("       ,NVL(CONCAT(TRIM(M4.UPPER_DISP), TRIM(M4.LOWER_DISP)), ' ') AS MERCHANDISENAME  ")
            '.Append("       ,M4.MERC_NAME ")
            '.Append("       ,M3.MAINTE_KATASHIKI ")
            '.Append("       ,T6.VCL_KATASHIKI ")
            '.Append("    FROM  ")
            '.Append("       (SELECT DISTINCT  ")
            '.Append("            T1.DLR_CD  ")
            '.Append("           ,T1.BRN_CD  ")
            '.Append("           ,T1.RO_NUM  ")
            '.Append("           ,T2.JOB_DTL_ID  ")
            '.Append("           ,T1.RSLT_SVCIN_DATETIME  ")
            '.Append("           ,NVL(T4.REG_MILE, 0) AS REG_MILE  ")
            '.Append("           ,T3.SVCIN_NUM  ")
            '.Append("           ,SUBSTR(M1.VCL_KATASHIKI, 1, INSTR(M1.VCL_KATASHIKI, '-') - 1 ) AS VCL_KATASHIKI  ")
            '.Append("           ,T5.JOB_CD ")
            '.Append("        FROM  ")
            '.Append("            TB_M_VEHICLE M1  ")
            '.Append("           ,TB_T_SERVICEIN T1  ")
            '.Append("           ,TB_T_JOB_DTL T2  ")
            '.Append("           ,TB_T_VEHICLE_SVCIN_HIS T3  ")
            '.Append("           ,TB_T_VEHICLE_MILEAGE T4  ")
            '.Append("           ,TB_T_JOB_INSTRUCT T5  ")
            '.Append("        WHERE  ")
            '.Append("            M1.VCL_VIN = :VCL_VIN  ")
            '.Append("        AND M1.VCL_ID = T1.VCL_ID  ")
            '.Append("        AND T1.SVCIN_ID = T2.SVCIN_ID  ")
            '.Append("        AND T2.JOB_DTL_ID = T5.JOB_DTL_ID  ")
            '.Append("        AND T1.DLR_CD = T3.DLR_CD(+)  ")
            '.Append("        AND T1.CST_ID = T3.CST_ID(+)  ")
            '.Append("        AND T1.VCL_ID = T3.VCL_ID(+)  ")
            '.Append("        AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+)  ")
            '.Append("        AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ) T6 ")
            '.Append("       ,TB_M_MAINTE_ATTR M3 ")
            '.Append("       ,TB_M_MERCHANDISE M4 ")
            '.Append("    WHERE ")
            '.Append("        T6.DLR_CD = M3.DLR_CD(+) ")
            '.Append("    AND T6.JOB_CD = M3.MAINTE_CD(+) ")
            ''.Append("    AND T6.VCL_KATASHIKI = M3.MAINTE_KATASHIKI(+) ")
            '.Append("    AND M3.MERC_ID = M4.MERC_ID(+)) T7 ")
            '.Append("  ,TB_M_SERVICE M5 ")
            '.Append("WHERE  ")
            '.Append("    M5.DLR_CD(+) = T7.DLR_CD ")
            '.Append("AND T7.SVC_CD = M5.SVC_CD(+) ")
            '.Append("ORDER BY  ")
            '.Append("  T7.RSLT_SVCIN_DATETIME ")


            '.Append("SELECT /* SC3250101_026 */ ")
            '.Append("   T1.RSLT_SVCIN_DATETIME ")
            '.Append("  ,NVL(M2.UPPER_DISP,' ') AS UPPER_DISP ")
            '.Append("  ,NVL(M2.LOWER_DISP,' ') AS LOWER_DISP ")
            '.Append("  ,NVL(T4.REG_MILE, 0) AS REG_MILE ")
            '.Append("  ,T1.DLR_CD ")
            '.Append("  ,T1.BRN_CD ")
            '.Append("  ,T2.JOB_DTL_ID ")
            '.Append("  ,T3.SVCIN_NUM ")
            '.Append("  ,NVL(M3.INSPEC_TYPE,' ') AS INSPEC_TYPE ")
            '.Append("  ,T1.RO_NUM ")
            '.Append("  ,NVL(M2.MERC_NAME,' ') AS MERC_NAME ")
            '.Append("FROM ")
            '.Append("   TB_M_VEHICLE M1 ")
            '.Append("  ,TB_T_SERVICEIN T1 ")
            '.Append("  ,TB_T_JOB_DTL T2 ")
            '.Append("  ,TB_M_MERCHANDISE M2 ")
            '.Append("  ,TB_T_VEHICLE_SVCIN_HIS T3 ")
            '.Append("  ,TB_T_VEHICLE_MILEAGE T4 ")
            '.Append("  ,TB_T_JOB_INSTRUCT T5 ")
            '.Append("  ,TB_M_OPERATION_CHANGE M3 ")
            '.Append("WHERE ")
            '.Append("      M1.VCL_VIN = :VCL_VIN ")
            '.Append("  AND M1.VCL_ID = T1.VCL_ID ")
            '.Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
            '.Append("  AND T2.MERC_ID = M2.MERC_ID(+) ")
            '.Append("  AND T2.JOB_DTL_ID = T5.JOB_DTL_ID ")
            '.Append("  AND T5.JOB_CD = M3.MAINTE_CD(+) ")
            '.Append("  AND T1.DLR_CD = T3.DLR_CD(+) ")
            '.Append("  AND T1.CST_ID = T3.CST_ID(+) ")
            '.Append("  AND T1.VCL_ID = T3.VCL_ID(+) ")
            '.Append("  AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+) ")
            '.Append("  AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ")
            '.Append("ORDER BY ")
            '.Append("  T3.SVCIN_DELI_DATE ")

            '.Append("SELECT /* SC3250101_026 */ ")
            '.Append("   T1.RSLT_SVCIN_DATETIME ")
            '.Append("  ,NVL(M2.UPPER_DISP,' ') AS UPPER_DISP ")
            '.Append("  ,NVL(M2.LOWER_DISP,' ') AS LOWER_DISP ")
            '.Append("  ,NVL(T4.REG_MILE, 0) AS REG_MILE ")
            '.Append("  ,T1.DLR_CD ")
            '.Append("  ,T1.BRN_CD ")
            '.Append("  ,T2.JOB_DTL_ID ")
            '.Append("  ,T3.SVCIN_NUM ")
            '.Append("  ,NVL(M3.INSPEC_TYPE,' ') AS INSPEC_TYPE ")
            '.Append("  ,T1.RO_NUM ")
            '.Append("FROM ")
            '.Append("   TB_M_VEHICLE M1 ")
            '.Append("  ,TB_T_SERVICEIN T1 ")
            '.Append("  ,TB_T_JOB_DTL T2 ")
            '.Append("  ,TB_M_MERCHANDISE M2 ")
            '.Append("  ,TB_T_VEHICLE_SVCIN_HIS T3 ")
            '.Append("  ,TB_T_VEHICLE_MILEAGE T4 ")
            '.Append("  ,TB_T_JOB_INSTRUCT T5 ")
            '.Append("  ,TB_M_OPERATION_CHANGE M3 ")
            '.Append("WHERE ")
            '.Append("      M1.VCL_VIN = :VCL_VIN ")
            '.Append("  AND M1.VCL_ID = T1.VCL_ID ")
            '.Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
            '.Append("  AND T2.MERC_ID = M2.MERC_ID(+) ")
            '.Append("  AND T2.JOB_DTL_ID = T5.JOB_DTL_ID ")
            '.Append("  AND T5.JOB_CD = M3.MAINTE_CD(+) ")
            '.Append("  AND T1.DLR_CD = T3.DLR_CD(+) ")
            '.Append("  AND T1.CST_ID = T3.CST_ID(+) ")
            '.Append("  AND T1.VCL_ID = T3.VCL_ID(+) ")
            '.Append("  AND T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE(+) ")
            '.Append("  AND T3.VCL_MILE_ID = T4.VCL_MILE_ID(+) ")
            '.Append("ORDER BY ")
            '.Append("  T3.SVCIN_DELI_DATE ")
            '2014/05/21 「Result一覧」に商品名追加　　END　↑↑↑

            '.Append("SELECT                                            ")
            '.Append("T3.SVCIN_DELI_DATE,                               ")
            '.Append("M2.UPPER_DISP,                                    ")
            '.Append("M2.LOWER_DISP,                                    ")
            '.Append("T4.REG_MILE,                                      ")
            '.Append("T1.DLR_CD,                                        ")
            '.Append("T1.BRN_CD,                                        ")
            '.Append("T2.JOB_DTL_ID,                                    ")
            '.Append("T3.SVCIN_NUM                                      ")
            '.Append("FROM                                              ")
            '.Append("TB_M_VEHICLE M1,                                  ")
            '.Append("TB_T_SERVICEIN T1,                                ")
            '.Append("TB_T_JOB_DTL T2,                                  ")
            '.Append("TB_M_MERCHANDISE M2,                              ")
            '.Append("TB_T_VEHICLE_SVCIN_HIS T3,                        ")
            '.Append("TB_T_VEHICLE_MILEAGE T4                           ")
            '.Append("WHERE                                             ")
            '.AppendFormat("M1.VCL_VIN = '{0}' AND                       ", strVIN_NO)
            '.Append("M1.VCL_ID = T1.VCL_ID AND                         ")
            '.Append("T1.SVCIN_ID = T2.SVCIN_ID AND                     ")
            '.Append("T2.MERC_ID = M2.MERC_ID AND                       ")
            '.Append("T1.DLR_CD = T3.DLR_CD AND                         ")
            '.Append("T1.CST_ID = T3.CST_ID AND                         ")
            '.Append("T1.VCL_ID = T3.VCL_ID AND                         ")
            '.Append("T1.RSLT_DELI_DATETIME = T3.SVCIN_DELI_DATE AND    ")
            '.Append("T3.VCL_MILE_ID = T4.VCL_MILE_ID                   ")
            '.Append("ORDER BY                                          ")
            '.Append("T3.SVCIN_DELI_DATE                                ")

            .Append(" UNION ALL ")

            .Append("SELECT DISTINCT ")
            .Append("    MH2.SVC_CD ")
            .Append("    ,H1.SVCIN_ID ")
            .Append("    ,H1.DLR_CD ")
            .Append("    ,H1.BRN_CD ")
            .Append("    ,H6.RO_NUM ")
            .Append("    ,H2.JOB_DTL_ID ")
            .Append("    ,H1.RSLT_SVCIN_DATETIME ")
            .Append("    ,NVL(H4.REG_MILE, 0) AS REG_MILE ")
            .Append("    ,NVL(CONCAT(TRIM(MH2.UPPER_DISP), TRIM(MH2.LOWER_DISP)), ' ') AS MERCHANDISENAME ")
            .Append("    ,MH2.MERC_NAME ")
            .Append("    ,H1.SVCIN_MILE ")
            .Append("    ,(SELECT COUNT(1) FROM TB_M_SERVICE WHERE DLR_CD = H1.DLR_CD AND SVC_CD=MH2.SVC_CD) AS SERVICE ")
            .Append("    ,MH4.INSPEC_ORDER ")
            .Append("FROM ")
            .Append("    TB_M_VEHICLE MH1 ")
            .Append("    ,TB_H_SERVICEIN H1 ")
            .Append("    ,TB_H_JOB_DTL H2 ")
            .Append("    ,TB_T_VEHICLE_SVCIN_HIS H3 ")
            .Append("    ,TB_T_VEHICLE_MILEAGE H4 ")
            .Append("    ,TB_H_JOB_INSTRUCT H5 ")
            .Append("    ,TB_M_MERCHANDISE MH2 ")
            .Append("    ,TB_M_MAINTE_ATTR MH3 ")
            .Append("    ,(SELECT RO_NUM, SVCIN_ID FROM TB_H_RO_INFO WHERE RO_SEQ=0) H6 ")
            .Append("    ,TB_M_INSPECTION_ORDER MH4 ")
            .Append("WHERE ")
            .Append("    MH1.VCL_VIN = :VCL_VIN ")
            .Append("    AND MH1.VCL_ID = H1.VCL_ID ")
            .Append("    AND H1.SVCIN_ID = H2.SVCIN_ID ")
            .Append("    AND H2.JOB_DTL_ID = H5.JOB_DTL_ID ")
            .Append("    AND H1.DLR_CD = H3.DLR_CD(+) ")
            .Append("    AND H1.CST_ID = H3.CST_ID(+) ")
            .Append("    AND H1.VCL_ID = H3.VCL_ID(+) ")
            .Append("    AND H1.RSLT_DELI_DATETIME = H3.SVCIN_DELI_DATE(+) ")
            .Append("    AND H3.VCL_MILE_ID = H4.VCL_MILE_ID(+) ")
            If specifyDlrCdFlgs = True Then
                .Append("    AND MH3.DLR_CD = H1.DLR_CD ")
            Else
                .Append("    AND MH3.DLR_CD = '" & AllDealer & "' ")
            End If
            .Append("    AND MH3.MAINTE_CD = H5.JOB_CD ")
            .Append("    AND (MH3.MAINTE_KATASHIKI = SUBSTR(MH1.VCL_KATASHIKI, 1, INSTR(MH1.VCL_KATASHIKI, '-') - 1 ) ")
            .Append("        OR MH3.MAINTE_KATASHIKI = 'X') ")
            .Append("    AND MH3.MERC_ID = MH2.MERC_ID ")
            .Append("    AND H1.SVCIN_ID = H6.SVCIN_ID(+) ")
            .Append("    AND MH2.SVC_CD = MH4.SVC_CD(+) ")

            .Append("ORDER BY ")
            .Append("    RSLT_SVCIN_DATETIME ")
            .Append("    ,INSPEC_ORDER ")

        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.ResultListDataTable)(No)
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Varchar2, strVIN_NO)

            'データ取得
            dt = query.GetData()

        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
        'ログ出力 End *****************************************************************************
        Return dt

    End Function


    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 027:点検順序から点検種類名を取得
    ''' </summary>

    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strMODEL_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strINSPEC_ORDER">オーダー</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(ByVal strDLR_CD As String _
                                                            , ByVal strBRN_CD As String _
                                                            , ByVal strMODEL_CD As String _
                                                            , ByVal strKatashiki As String _
                                                            , ByVal strINSPEC_ORDER As String) As SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable

        Dim No As String = "SC3250101_027"
        Dim strMethodName As String = "TB_M_INSPECTION_ORDER_From_INSPEC_ORDER"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
        Dim sql As New StringBuilder

        '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
        '型式を使用しない場合、紐づかない場合
        If useFlgKatashiki = False Then
            '型式値を初期値（半角スペース）にする
            strKatashiki = DEFAULT_KATASHIKI_SPACE
        End If
        '2019/12/02 NCN 吉川 TKM要件：型式対応 End

        'SQL文作成
        With sql

            .Append("SELECT /* SC3250101_027 */ DISTINCT ")
            .Append("	M1.SVC_CD ")
            .Append("	,M2.INSPEC_ORDER ")
            .Append("FROM ")
            .Append("	 TB_M_INSPECTION_COMB M1 ")
            .Append("	,TB_M_INSPECTION_ORDER M2 ")
            .Append("WHERE ")
            .Append("	    M1.MODEL_CD = :MODEL_CD ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("	AND  M1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("	AND M1.DLR_CD   = :DLR_CD ")
            .Append("	AND M1.BRN_CD   = :BRN_CD ")
            .Append("    AND M1.SVC_CD = M2.SVC_CD ")
            .Append("	AND (INSPEC_ORDER > :INSPEC_ORDER ")
            .Append("		OR INSPEC_ORDER = ")
            .Append("		(SELECT ")
            .Append("			MAX(M2.INSPEC_ORDER) ")
            .Append("		FROM ")
            .Append("			TB_M_INSPECTION_COMB M1 ")
            .Append("			,TB_M_INSPECTION_ORDER M2 ")
            .Append("		WHERE ")
            .Append("			M1.MODEL_CD = :MODEL_CD ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("			AND M1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("			AND M1.DLR_CD   = :DLR_CD ")
            .Append("			AND M1.BRN_CD   = :BRN_CD ")
            .Append("			AND M1.SVC_CD = M2.SVC_CD )) ")
            .Append("ORDER BY ")
            .Append("	M2.INSPEC_ORDER ")

            '.Append("SELECT /* SC3250101_027 */ ")
            '.Append("   INSPEC_ORDER ")
            ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            '.Append("  ,SVC_CD ")
            ''.Append("  ,SVC_CD AS INSPEC_TYPE ")
            ''.Append("  ,INSPEC_TYPE ")
            ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            '.Append("FROM ")
            '.Append("  TB_M_INSPECTION_ORDER ")
            '.Append("WHERE ")
            '.Append("  INSPEC_ORDER > :INSPEC_ORDER ")
            '.Append("ORDER BY ")
            '.Append("  INSPEC_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable)(No)
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strMODEL_CD)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, strKatashiki)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("INSPEC_ORDER", OracleDbType.Int32, strINSPEC_ORDER)

            'データ取得
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 028:点検種類名から点検順番を取得
    ''' </summary>
    ''' <param name="strSVC_CD">点検順序</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    Public Function TB_M_INSPECTION_ORDER_From_INSPEC_TYPE(ByVal strSVC_CD As String) As SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable

        Dim No As String = "SC3250101_028"
        Dim strMethodName As String = "TB_M_INSPECTION_ORDER_From_INSPEC_TYPE"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_028 */ ")
            .Append("   INSPEC_ORDER ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  ,SVC_CD ")
            '.Append("  ,SVC_CD AS INSPEC_TYPE ")
            '.Append("  ,INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
            .Append("FROM ")
            .Append("  TB_M_INSPECTION_ORDER ")
            .Append("WHERE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
            .Append("  SVC_CD = :SVC_CD ")
            '.Append("  INSPEC_TYPE = :INSPEC_TYPE ")
            '2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
        End With

        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable)(No)
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.Varchar2, strSVC_CD)

            'データ取得
            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function

#Region "未使用メソッド"
    '2014/06/09 未使用メソッドのコメント化　START　↓↓↓
    ' ''' <summary>
    ' ''' モデル名からオペレーション変換マスタによる変換後の情報を取得
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <returns>オペレーション変換マスタ</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    'Public Function TB_M_OPERATION_CHANGE_Select(ByVal strMODEL_CD As String) As SC3250101DataSet.TB_M_OPERATION_CHANGEDataTable

    '    Dim No As String = "SC3250101_029"
    '    Dim strMethodName As String = "TB_M_OPERATION_CHANGE_Select"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        'データ格納用
    '        Dim dt As SC3250101DataSet.TB_M_OPERATION_CHANGEDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_029 */ ")
    '            .Append("   M2.MAINTE_CD ")
    '            .Append("  ,M2.INSPEC_TYPE ")
    '            .Append("  ,M2.MODEL_CD ")
    '            .Append("  ,M2.GRADE_CD ")
    '            .Append("FROM ")
    '            .Append("   TB_M_VEHICLE M1 ")
    '            .Append("  ,TB_T_SERVICEIN T1 ")
    '            .Append("  ,TB_T_JOB_DTL T2 ")
    '            .Append("  ,TB_T_JOB_INSTRUCT T3 ")
    '            .Append("  ,TB_M_OPERATION_CHANGE M2 ")
    '            .Append("WHERE ")
    '            .Append("      M1.MODEL_CD = :MODEL_CD ")
    '            .Append("  AND M1.VCL_ID = T1.VCL_ID ")
    '            .Append("  AND T1.SVCIN_ID = T2.SVCIN_ID ")
    '            .Append("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
    '            .Append("  AND T3.JOB_CD = M2.MAINTE_CD ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_M_OPERATION_CHANGEDataTable)(No)
    '            query.CommandText = sql.ToString()
    '            'バインド変数
    '            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.Varchar2, strMODEL_CD)

    '            'データ取得
    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************
    '            Return dt

    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function

#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 文言マスタから文言を取得する
    ' ''' </summary>
    ' ''' <param name="strDISPLAYID">ID番号</param>
    ' ''' <param name="strDISPLAYNO">文言No</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    'Public Function TBL_WORD_INI_Select(ByVal strDISPLAYID As String, ByVal strDISPLAYNO As String) As SC3250101DataSet.TBL_WORD_INIDataTable

    '    Dim No As String = "SC3250101_030"
    '    Dim strMethodName As String = "TBL_WORD_INI_Select"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        'データ格納用
    '        Dim dt As SC3250101DataSet.TBL_WORD_INIDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_030 */ ")
    '            .Append("  WORD ")
    '            .Append("FROM ")
    '            .Append("  TBL_WORD_INI ")
    '            .Append("WHERE ")
    '            .Append("      DISPLAYID = :DISPLAYID ")
    '            .Append("  AND DISPLAYNO = :DISPLAYNO ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TBL_WORD_INIDataTable)(No)
    '            query.CommandText = sql.ToString()
    '            'バインド変数
    '            query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.Varchar2, strDISPLAYID)
    '            query.AddParameterWithTypeValue("DISPLAYNO", OracleDbType.Int32, strDISPLAYNO)

    '            'データ取得
    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************
    '            Return dt

    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
    '2014/06/09 未使用メソッドのコメント化　END　　↑↑↑
#End Region

    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　START　↓↓↓

    ''' <summary>
    ''' 031:販売店システム設定から設定値を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
    ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
    ''' <param name="settingName">販売店システム設定名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal allDealerCode As String, _
                                             ByVal allBranchCode As String, _
                                             ByVal settingName As String) As SC3250101DataSet.SystemSettingDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode, _
                                  allDealerCode, _
                                  allBranchCode, _
                                  settingName))

        Dim sql As New StringBuilder
        With sql
            '2014/06/09 「AppendLine」→「Append」に変更　START　↓↓↓
            .Append("   SELECT /* SC3250101_031 */ ")
            .Append(" 		   SETTING_VAL ")
            .Append("     FROM ")
            .Append(" 		   TB_M_SYSTEM_SETTING_DLR ")
            .Append("    WHERE ")
            .Append(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
            .Append("      AND SETTING_NAME = :SETTING_NAME ")
            .Append(" ORDER BY ")
            .Append("          DLR_CD ASC, BRN_CD ASC ")
            '2014/06/09 「AppendLine」→「Append」に変更　END　　↑↑↑
        End With

        Dim dt As SC3250101DataSet.SystemSettingDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250101DataSet.SystemSettingDataTable)("SC3250101_031")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

            dt = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' 032:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
    ''' </summary>
    ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <returns>DmsCodeMapDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetIcropToDmsCode(ByVal allDealerCD As String, _
                                      ByVal dealerCD As String, _
                                      ByVal dmsCodeType As Integer, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String) As SC3250101DataSet.DmsCodeMapDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  allDealerCD, _
                                  dealerCD, _
                                  dmsCodeType, _
                                  icropCD1, _
                                  icropCD2, _
                                  icropCD3))

        Dim sql As New StringBuilder
        With sql
            '2014/06/09 「AppendLine」→「Append」に変更　START　↓↓↓
            .Append("   SELECT /* SC3250101_032 */ ")
            .Append(" 		     DMS_CD_1 CODE1 ")                '基幹コード1
            .Append(" 		   , DMS_CD_2 CODE2 ")                '基幹コード2
            .Append(" 		   , DMS_CD_3 CODE3 ")                '基幹コード3
            .Append("     FROM ")
            .Append(" 		     TB_M_DMS_CODE_MAP ")             '基幹コードマップ
            .Append("    WHERE ")
            .Append(" 		     DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
            .Append(" 	   AND   ICROP_CD_1 = :ICROP_CD_1 ")
            '2014/06/09 「AppendLine」→「Append」に変更　END　　↑↑↑

            If Not String.IsNullOrEmpty(icropCD2) Then
                .AppendLine(" 	   AND   ICROP_CD_2 = :ICROP_CD_2 ")
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                .AppendLine(" 	   AND   ICROP_CD_3 = :ICROP_CD_3 ")
            End If

            .AppendLine(" ORDER BY DLR_CD ASC ")
        End With

        Dim dt As SC3250101DataSet.DmsCodeMapDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250101DataSet.DmsCodeMapDataTable)("SC3250101_032")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCD)
            query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
            query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, icropCD1)

            If Not String.IsNullOrEmpty(icropCD2) Then
                query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, icropCD2)
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.NVarchar2, icropCD3)
            End If

            dt = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function
    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

    ''' <summary>
    ''' 033:定期点検サービスマスタの取得
    ''' </summary>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_SERVICE_Select(ByVal strDLR_CD As String _
                                            , ByVal strMAINTE_KATASHIKI As String _
                                            , ByVal strMAINTE_CD As String) As TB_M_SERVICEDataTable

        Dim No As String = "SC3250101_033"
        Dim strMethodName As String = "TB_M_SERVICE_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As TB_M_SERVICEDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            '2014/06/09 「AppendLine」→「Append」に変更　START　↓↓↓
            .Append("SELECT /* SC3250101_033 */ ")
            .Append("	M1.DLR_CD ")
            .Append("	,M1.MAINTE_CD ")
            .Append("	,M1.MAINTE_KATASHIKI ")
            .Append("	,M2.MERC_ID ")
            .Append("    ,M2.SVC_CD ")
            .Append("    ,M3.SVC_NAME_MILE ")
            .Append("FROM ")
            .Append("	TB_M_MAINTE_ATTR M1 ")
            .Append("    ,TB_M_MERCHANDISE M2 ")
            .Append("    ,TB_M_SERVICE M3 ")
            .Append("WHERE ")
            .Append("    M1.DLR_CD IN (:DLR_CD , 'XXXXX') ")
            .Append("    AND M1.MAINTE_KATASHIKI IN (:MAINTE_KATASHIKI, 'X') ")
            .Append("    AND M1.MAINTE_CD = :MAINTE_CD ")
            .Append("    AND M1.MERC_ID = M2.MERC_ID ")
            .Append("    AND M3.DLR_CD IN (:DLR_CD , 'XXXXX') ")
            .Append("    AND M2.SVC_CD = M3.SVC_CD ")
            '2014/06/09 「AppendLine」→「Append」に変更　END　　↑↑↑
        End With

        Using query As New DBSelectQuery(Of TB_M_SERVICEDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("MAINTE_KATASHIKI", OracleDbType.NVarchar2, strMAINTE_KATASHIKI)
            query.AddParameterWithTypeValue("MAINTE_CD", OracleDbType.NVarchar2, strMAINTE_CD)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function

    '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
    'TODO: ★製造中：【■追加要件３．デフォルトのカムリを変更する】　DB特定後、すぐに対応できるようにしておく
    ''' <summary>
    ''' 034:販売店マスタの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_DEALER_Select(ByVal strDLR_CD As String) As TB_M_DEALERDataTable

        Dim No As String = "SC3250101_034"
        Dim strMethodName As String = "TB_M_DEALER_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As TB_M_DEALERDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT /* SC3250101_034 */ ")
            .Append("	M1.DLR_CD ")
            .Append("FROM ")
            .Append("	TB_M_DEALER M1 ")
            .Append("WHERE ")
            .Append("	M1.DLR_CD = :DLR_CD")
        End With

        Using query As New DBSelectQuery(Of TB_M_DEALERDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑

    '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
    ''' <summary>
    ''' 035.Suggestリストの取得
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strMODEL_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_INSPECTION_ORDER_ListSelect(ByVal strDLR_CD As String _
                                                     , ByVal strBRN_CD As String _
                                                     , ByVal strMODEL_CD As String _
                                                     , ByVal strKatashiki As String _
                                                     ) As TB_M_INSPECTION_ORDER_ListDataTable

        Dim No As String = "SC3250101_035"
        Dim strMethodName As String = "TB_M_INSPECTION_ORDER_ListSelect"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As TB_M_INSPECTION_ORDER_ListDataTable
        Dim sql As New StringBuilder

        '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
        '型式を使用しない場合、紐づかない場合
        If useFlgKatashiki = False Then
            '型式値を初期値（半角スペース）にする
            strKatashiki = DEFAULT_KATASHIKI_SPACE
        End If
        '2019/12/02 NCN 吉川 TKM要件：型式対応 End

        'SQL文作成
        With sql
            .Append("SELECT DISTINCT /* SC3250101_035 */ ")
            .Append("	M1.SVC_CD ")
            .Append("	,M2.INSPEC_ORDER ")
            .Append("   ,CONCAT(TRIM(M3.UPPER_DISP),TRIM(M3.LOWER_DISP)) AS MERCHANDISENAME ")
            .Append("FROM ")
            .Append("	 TB_M_INSPECTION_COMB M1 ")
            .Append("	,TB_M_INSPECTION_ORDER M2 ")
            .Append("   ,TB_M_MERCHANDISE M3 ")
            .Append("   ,TB_M_MAINTE_ATTR M4 ")
            .Append("WHERE ")
            .Append("	    M1.MODEL_CD = :MODEL_CD ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            .Append("    AND M1.VCL_KATASHIKI = :VCL_KATASHIKI ")
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            .Append("	AND M1.DLR_CD   = :DLR_CD ")
            .Append("	AND M1.BRN_CD   = :BRN_CD ")
            .Append("	AND M1.SVC_CD   = M2.SVC_CD ")
            .Append("   AND M1.SVC_CD   = M3.SVC_CD ")
            .Append("   AND M1.DLR_CD   = M4.DLR_CD ")
            .Append("   AND M3.MERC_ID  = M4.MERC_ID ")
            .Append("ORDER BY ")
            .Append("	M2.INSPEC_ORDER ")

            '.Append("SELECT /* SC3250101_035 */ ")
            '.Append("	M2.SVC_CD ")
            '.Append("	,M2.INSPEC_ORDER ")
            '.Append("FROM ")
            '.Append("	TB_M_INSPECTION_ORDER M2 ")
            '.Append("ORDER BY ")
            '.Append("	M2.INSPEC_ORDER ")

        End With

        Using query As New DBSelectQuery(Of TB_M_INSPECTION_ORDER_ListDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, strMODEL_CD)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, strKatashiki)
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 036.商品訴求画面データ実績削除
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TB_T_REPAIR_SUGGESTION_RSLT_Delete(
                                            ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strINSPEC_ITEM_CD As String _
                                          ) As Integer

        Dim No As String = "SC3250101_036"
        Dim strMethodName As String = "TB_T_REPAIR_SUGGESTION_RSLT_Delete"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("DELETE FROM /* SC3250101_036 */ ")
            .Append("  TB_T_REPAIR_SUGGESTION_RSLT ")
            .Append("WHERE ")
            .Append("      DLR_CD = :DLR_CD ")
            .Append("  AND BRN_CD = :BRN_CD ")
            .Append("  AND RO_NUM = :SA_CHIP_ID ")
            .Append("  AND SVC_CD = :SVC_CD ")
            .Append("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
        End With

        Using query As New DBUpdateQuery(No)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)

            Dim ret As Integer = query.Execute()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************

            Return ret
        End Using

    End Function

    '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
    ''' <summary>
    ''' 037.R/O番号一覧の取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function GetRoListData(ByVal strVCL_VIN As String) As RO_NUM_ListDataTable

        Dim No As String = "SC3250101_037"
        Dim strMethodName As String = "Get_RO_NUM_List"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        'データ格納用
        Dim dt As RO_NUM_ListDataTable
        Dim sql As New StringBuilder

        'SQL文作成
        With sql
            .Append("SELECT DISTINCT /* SC3250101_037 */ ")
            .Append("	T1.RSLT_SVCIN_DATETIME ")
            .Append("	,T2.RO_NUM ")
            .Append("	,T2.RO_STATUS ")
            .Append("FROM ")
            .Append("	TB_M_VEHICLE M1 ")
            .Append("	,TB_T_SERVICEIN T1 ")
            .Append("	,TB_T_RO_INFO T2 ")
            .Append("WHERE ")
            .Append("	M1.VCL_VIN = :VCL_VIN ")
            .Append("	AND M1.VCL_ID = T1.VCL_ID ")
            .Append("	AND T1.SVCIN_ID = T2.SVCIN_ID ")

            .Append(" UNION ALL ")

            .Append("SELECT DISTINCT ")
            .Append("    H1.RSLT_SVCIN_DATETIME ")
            .Append("    ,H2.RO_NUM ")
            .Append("	 ,H2.RO_STATUS ")
            .Append("FROM ")
            .Append("    TB_M_VEHICLE HM1 ")
            .Append("    ,TB_H_SERVICEIN H1 ")
            .Append("    ,TB_H_RO_INFO H2 ")
            .Append("WHERE ")
            .Append("    HM1.VCL_VIN = :VCL_VIN ")
            .Append("    AND HM1.VCL_ID = H1.VCL_ID ")
            .Append("    AND H1.SVCIN_ID = H2.SVCIN_ID ")

            .Append("ORDER BY ")
            .Append("    RSLT_SVCIN_DATETIME ")

        End With

        Using query As New DBSelectQuery(Of RO_NUM_ListDataTable)(No)

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, strVCL_VIN)

            dt = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info(String.Format("{0}_End", strMethodName))
            'ログ出力 End *****************************************************************************
            Return dt

        End Using

    End Function
    '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 037.指定したサービスコード以外の商品訴求登録実績データ取得
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSAChipID">来店実績連番</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function REPAIR_SUGGESTION_RSLT_Expect_SVC_CD(ByVal strDLR_CD As String _
    '                                                   , ByVal strBRN_CD As String _
    '                                                   , ByVal strSAChipID As String _
    '                                                   , ByVal strSVC_CD As String _
    '                                                   ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable

    '    Dim No As String = "SC3250101_037"
    '    Dim strMethodName As String = "REPAIR_SUGGESTION_RSLT_Expect_SVC_CD"

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
    '    'ログ出力 End *****************************************************************************

    '    Try
    '        Dim dt As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
    '        Dim sql As New StringBuilder

    '        'SQL文作成
    '        With sql
    '            .Append("SELECT /* SC3250101_037 */ ")
    '            .Append("    DLR_CD ")
    '            .Append("   ,BRN_CD ")
    '            .Append("   ,RO_NUM ")
    '            .Append("   ,SVC_CD ")
    '            .Append("   ,INSPEC_ITEM_CD ")
    '            .Append("   ,SUGGEST_ICON ")
    '            .Append("   ,ROW_CREATE_DATETIME ")
    '            .Append("   ,ROW_CREATE_ACCOUNT ")
    '            .Append("   ,ROW_CREATE_FUNCTION ")
    '            .Append("   ,ROW_UPDATE_DATETIME ")
    '            .Append("   ,ROW_UPDATE_ACCOUNT ")
    '            .Append("   ,ROW_UPDATE_FUNCTION ")
    '            .Append("   ,ROW_LOCK_VERSION ")
    '            .Append("FROM ")
    '            .Append("    TB_T_REPAIR_SUGGESTION_RSLT ")
    '            .Append("WHERE ")
    '            .Append("       DLR_CD = :DLR_CD ")
    '            .Append("   AND BRN_CD = :BRN_CD ")
    '            .Append("   AND RO_NUM = :SA_CHIP_ID ")
    '            .Append("   AND SVC_CD <> :SVC_CD ")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable)(No)

    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
    '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBRN_CD)
    '            query.AddParameterWithTypeValue("SA_CHIP_ID", OracleDbType.NVarchar2, strSAChipID)
    '            query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, strSVC_CD)

    '            dt = query.GetData()

    '            'ログ出力 Start ***************************************************************************
    '            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
    '            'ログ出力 End *****************************************************************************

    '            Return dt
    '        End Using

    '    Catch ex As Exception
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
    '        'ログ出力 End *****************************************************************************
    '        Return Nothing
    '    End Try

    'End Function
#End Region

    '【追加要件１】．今回の点検以外の点検を選択できるようにする　END　　↑↑↑

    '2015/04/14 新販売店追加対応 start
    ''' <summary>
    ''' マスタに販売店が登録されているか判定する
    ''' </summary>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <returns>登録状態</returns>
    ''' <remarks>整備属性マスタに指定の販売店データが登録されているかをフラグで取得する</remarks>
    Public Function ChkDlrCdExistMst(strDlrCd As String) As Boolean

        Dim ret As Boolean = False
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using query As New DBSelectQuery(Of DataTable)("SC3250101_043")
            Dim sql As New StringBuilder

            With sql
                .AppendLine("SELECT ")
                .AppendLine("        MAINTE_CD")
                .AppendLine("    FROM")
                .AppendLine("        TB_M_MAINTE_ATTR")
                .AppendLine("    WHERE ")
                .AppendLine("        DLR_CD = :DLR_CD")
                .AppendLine("        AND ROWNUM=1 ")
            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDlrCd)     '販売店
            '結果取得
            Dim dt As DataTable = query.GetData()

            If 0 < dt.Rows.Count Then
                ret = True
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ret

    End Function

    '2015/04/14 新販売店追加対応 end

    '2017/XX/XX ライフサイクル対応　↓
    Public Function ChkExistParamRoActive(ByVal argDlrCd As String, _
                                          ByVal argBrncd As String, _
                                          ByVal argRoNum As String) As Boolean

        Dim ret As Boolean = False
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using query As New DBSelectQuery(Of DataTable)("SC3250101_044")
            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT ")
                .AppendLine("        COUNT(1) AS CNT")
                .AppendLine("    FROM")
                .AppendLine("        TB_T_SERVICEIN")
                .AppendLine("    WHERE ")
                .AppendLine("        DLR_CD = :DLR_CD")
                .AppendLine("    AND BRN_CD = :BRN_CD")
                .AppendLine("    AND RO_NUM = :RO_NUM")
            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, argDlrCd)     '販売店
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, argBrncd)     '店舗
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, argRoNum)     'RO番号
            '結果取得
            Dim dt As DataTable = query.GetData()

            If dt.Rows(0).Item("CNT").ToString <> CStr(0) Then
                ret = True
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ret

    End Function
    '2017/XX/XX ライフサイクル対応　↑

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' サービス入庫行ロックバージョン取得
    ''' </summary>
    ''' <param name="DealerCode ">販売店コード</param>
    ''' <param name="BranchCode">店舗コード</param>
    ''' <param name="RoNum">RO番号</param>
    ''' <returns>行ロックバージョン</returns>
    ''' <remarks></remarks>
    Public Function GetServiceinRowLockVertion(
                                           ByVal DealerCode As String,
                                           ByVal BranchCode As String,
                                           ByVal RoNum As String) As Long

        Dim No As String = "SC3250101_045"
        Dim strMethodName As String
        Dim nowLockver As Long

        strMethodName = "GetServiceinRowLockVertion"
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '行ロックバージョン取得処理
        Using query As New DBSelectQuery(Of DataTable)("SC3250101_047")
            Dim sql As New StringBuilder

            With sql
                .AppendLine("SELECT /* SC3250101_047 */ ")
                .AppendLine("        ROW_LOCK_VERSION")
                .AppendLine("    FROM")
                .AppendLine("        TB_T_SERVICEIN")
                .AppendLine("    WHERE ")
                .AppendLine("           DLR_CD =:DLR_CD")
                .AppendLine("       AND BRN_CD =:BR_CD")
                .AppendLine("       AND RO_NUM =:RO_CD")

            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, DealerCode)     '販売店
            query.AddParameterWithTypeValue("BR_CD", OracleDbType.NVarchar2, BranchCode)      '店舗
            query.AddParameterWithTypeValue("RO_CD", OracleDbType.NVarchar2, RoNum)       'RO　


            '結果取得
            Dim dt As DataTable = query.GetData()

            If dt.Rows.Count > 0 Then

                nowLockver = Long.Parse(dt.Rows(0).Item("ROW_LOCK_VERSION").ToString)

            Else
                
                '行ロックバージョンが取得できない場合エラーとなるため-1を返す
                nowLockver = -1

            End If

        End Using


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return nowLockver

    End Function
    '【***完成検査_排他制御***】 end
    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 型式使用フラグの取得
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <returns>登録状態 DataTable TRANSACTION_EXIST : 1 or 0 , HISTORY_EXIST : 1 or 0 </returns>
    ''' <remarks>点検組み合わせマスタ、整備属性マスタ、車両マスタと紐づく型式値を取得する</remarks>
    Public Function GetDlrCdExistMst(ByVal strRoNum As String, _
                                     ByVal strDlrCd As String, _
                                     ByVal strBrnCd As String) As DataTable

        Dim dt As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using query As New DBSelectQuery(Of DataTable)("SC3250101_045")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT")
                .Append("    /* SC3250101_045 */")
                .Append("    CASE")
                .Append("        WHEN IC.VCL_KATASHIKI <> MV.VCL_KATASHIKI")
                .Append("    OR  NVL(IC.VCL_KATASHIKI, ' ') = ' '")
                .Append("    OR  NVL(MV.VCL_KATASHIKI, ' ') = ' ' THEN '0'")
                .Append("        ELSE '1'")
                .Append("    END KATASHIKI_EXIST")
                .Append(" FROM")
                .Append("    TB_M_VEHICLE MV")
                .Append("    LEFT OUTER JOIN")
                .Append("        (")
                .Append("            SELECT")
                .Append("                IC.MODEL_CD,")
                .Append("                IC.DLR_CD,")
                .Append("                IC.BRN_CD,")
                .Append("                IC.VCL_KATASHIKI")
                .Append("            FROM")
                .Append("                TB_M_INSPECTION_COMB IC")
                .Append("            WHERE")
                .Append("                IC.DLR_CD IN(:DLR_CD, 'XXXXX')")
                .Append("            AND IC.BRN_CD IN(:BRN_CD, 'XXX')")
                .Append("            ORDER BY")
                .Append("                IC.VCL_KATASHIKI DESC")
                .Append("        ) IC")
                .Append("    ON  IC.MODEL_CD = MV.MODEL_CD")
                .Append("    AND IC.VCL_KATASHIKI = MV.VCL_KATASHIKI")
                .Append("    LEFT OUTER JOIN")
                .Append("        (")
                .Append("            SELECT")
                .Append("                TSI.VCL_ID,")
                .Append("                TSI.RO_NUM")
                .Append("            FROM")
                .Append("                TB_T_SERVICEIN TSI")
                .Append("            WHERE")
                .Append("                TSI.RO_NUM = :RO_NUM")
                .Append("            AND TSI.DLR_CD = :DLR_CD")
                .Append("            AND TSI.BRN_CD = :BRN_CD")
                .Append("    UNION ")
                .Append("            SELECT")
                .Append("                HSI.VCL_ID,")
                .Append("                HSI.RO_NUM")
                .Append("            FROM")
                .Append("                TB_H_SERVICEIN HSI")
                .Append("            WHERE")
                .Append("                HSI.RO_NUM = :RO_NUM")
                .Append("            AND HSI.DLR_CD = :DLR_CD")
                .Append("            AND HSI.BRN_CD = :BRN_CD")
                .Append("            AND ROWNUM = 1")
                .Append("        ) SI")
                .Append("    ON  SI.RO_NUM = :RO_NUM")
                .Append(" WHERE")
                .Append("    ROWNUM = 1")
                .Append("    AND MV.VCL_ID = SI.VCL_ID")
            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRoNum)     'R/O番号
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDlrCd)     '販売店
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBrnCd)     '店舗
            '結果取得
            dt = query.GetData()
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function
    '2019/07/05　TKM要件:型式対応　END    ↑↑↑
    
End Class
