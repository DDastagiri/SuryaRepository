'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290102DataSet.vb
'─────────────────────────────────────
'機能： リマインダーデータセット
'補足： 
'作成： 2014/05/30 TMEJ t.nagata
'─────────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization

Namespace SC3290102DataSetTableAdapters

    ''' <summary>
    ''' リマインダーのデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Partial Public Class SC3290102TableAdapter
        Inherits Global.System.ComponentModel.Component
#Region "定数"

        ''' <summary>
        ''' フォロー完了フラグ：未完了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FllwCompleteFlgNotComplete As String = "0"

#End Region


#Region "コンストラクタ"

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>処理なし</remarks>
        Public Sub New()

        End Sub

#End Region


#Region "公開メソッド"

        ''' <summary>
        ''' フォロー一覧の行数を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <returns>フォロー数</returns>
        ''' <remarks></remarks>
        Public Function GetIrregularFollowListCount(ByVal dealerCode As String, _
                                                        ByVal brnCode As String, _
                                                        ByVal staffCode As String) As SC3290102DataSet.SC3290102FollowListCountDataTable

            Dim dt As SC3290102DataSet.SC3290102FollowListCountDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290102DataSet.SC3290102FollowListCountDataTable)("SC3290102DataSet_001")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3290102_001 */")
                    .Append("        COUNT(1) AS FOLLOWLISTCOUNT")
                    .Append("   FROM")
                    .Append("        TB_T_IRREG_FLLW FLLW")
                    .Append("      , TB_M_STAFF STAFF")
                    .Append("      , TB_M_WORD WORD")
                    .Append("      , TB_M_IRREG_BRN_SETTING BRN")
                    .Append("      , TB_M_SLS_MANAGER_IRREG_MNG SMNG")
                    .Append("  WHERE BRN.DLR_CD = :DLRCD")
                    .Append("    AND BRN.BRN_CD = :BRNCD")
                    .Append("    AND SMNG.IRREG_CLASS_CD = BRN.IRREG_CLASS_CD")
                    .Append("    AND SMNG.IRREG_ITEM_CD = BRN.IRREG_ITEM_CD")
                    .Append("    AND FLLW.IRREG_ITEM_CD = SMNG.IRREG_ITEM_CD")
                    .Append("    AND FLLW.STF_CD = STAFF.STF_CD")
                    .Append("    AND STAFF.INUSE_FLG = '1'")
                    .Append("    AND FLLW.IRREG_CLASS_CD = SMNG.IRREG_CLASS_CD")
                    .Append("    AND FLLW.IRREG_ITEM_CD = SMNG.IRREG_ITEM_CD")
                    .Append("    AND SMNG.IRREG_LIST_DISP_NAME =  WORD.WORD_CD(+)")
                    .Append("    AND FLLW.FLLW_PIC_STF_CD = :STFCD")
                    .Append("    AND FLLW.FLLW_COMPLETE_FLG = :FLLWCMP")

                End With

                query.CommandText = Sql.ToString()
                Sql = Nothing

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brnCode)
                query.AddParameterWithTypeValue("STFCD", OracleDbType.NVarchar2, staffCode)
                query.AddParameterWithTypeValue("FLLWCMP", OracleDbType.NVarchar2, FllwCompleteFlgNotComplete)

                ' SQLの実行
                dt = query.GetData()

            End Using

            ' 検索結果返却
            Return dt

        End Function

        ''' <summary>
        ''' フォロー一覧を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="brnCode">店舗コード</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="getbeginLine">取得開始行番号</param>
        ''' <param name="getEndLine">取得終了行番号</param>
        ''' <returns>フォロー一覧結果</returns>
        ''' <remarks></remarks>
        Public Function GetIrregularFollowList(ByVal dealerCode As String, _
                                                ByVal brnCode As String, _
                                                ByVal staffCode As String, _
                                                ByVal getBeginLine As Integer, _
                                                ByVal getEndLine As Integer) As SC3290102DataSet.SC3290102FollowListDataTable


            Dim followListData As SC3290102DataSet.SC3290102FollowListDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3290102DataSet.SC3290102FollowListDataTable)("SC3290102DataSet_002")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3290102_002 */")
                    .Append("        IRREG_FLLW_ID")
                    .Append("      , IRREG_CLASS_CD")
                    .Append("      , IRREG_ITEM_CD")
                    .Append("      , STF_CD")
                    .Append("      , FLLW_EXPR_DATE")
                    .Append("      , IRREG_ITEM_NAME")
                    .Append("      , STF_NAME")
                    .Append("      , SORT_ORDER")
                    .Append("   FROM")
                    .Append("   (")
                    .Append("     SELECT")
                    .Append("            FLLW.IRREG_FLLW_ID")
                    .Append("          , FLLW.IRREG_CLASS_CD")
                    .Append("          , FLLW.IRREG_ITEM_CD")
                    .Append("          , FLLW.STF_CD")
                    .Append("          , FLLW.FLLW_EXPR_DATE")
                    .Append("          , DECODE(WORD.WORD_VAL, ' ',WORD.WORD_VAL_ENG, WORD.WORD_VAL) AS IRREG_ITEM_NAME")
                    .Append("          , STAFF.STF_NAME")
                    .Append("          , SMNG.SORT_ORDER")
                    .Append("          , ROW_NUMBER() OVER (")
                    .Append("                ORDER BY ")
                    .Append("                         FLLW.FLLW_EXPR_DATE")
                    .Append("                       , STAFF.STF_NAME")
                    .Append("                       , SMNG.SORT_ORDER")
                    .Append("            ) ROWNUM1")
                    .Append("       FROM")
                    .Append("            TB_T_IRREG_FLLW FLLW")
                    .Append("          , TB_M_STAFF STAFF")
                    .Append("          , TB_M_WORD WORD")
                    .Append("          , TB_M_IRREG_BRN_SETTING BRN")
                    .Append("          , TB_M_SLS_MANAGER_IRREG_MNG SMNG")
                    .Append("      WHERE BRN.DLR_CD = :DLRCD")
                    .Append("        AND BRN.BRN_CD = :BRNCD")
                    .Append("        AND SMNG.IRREG_CLASS_CD = BRN.IRREG_CLASS_CD")
                    .Append("        AND SMNG.IRREG_ITEM_CD = BRN.IRREG_ITEM_CD")
                    .Append("        AND FLLW.IRREG_ITEM_CD = SMNG.IRREG_ITEM_CD")
                    .Append("        AND FLLW.STF_CD = STAFF.STF_CD")
                    .Append("        AND STAFF.INUSE_FLG = '1'")
                    .Append("        AND FLLW.IRREG_CLASS_CD = SMNG.IRREG_CLASS_CD")
                    .Append("        AND FLLW.IRREG_ITEM_CD = SMNG.IRREG_ITEM_CD")
                    .Append("        AND SMNG.IRREG_LIST_DISP_NAME =  WORD.WORD_CD(+)")
                    .Append("        AND FLLW.FLLW_PIC_STF_CD = :STFCD")
                    .Append("        AND FLLW.FLLW_COMPLETE_FLG = :FLLWCMP")
                    .Append("   )")
                    .Append("  WHERE ROWNUM1 BETWEEN :BEGINLINE AND :ENDLINE ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brnCode)
                query.AddParameterWithTypeValue("STFCD", OracleDbType.NVarchar2, staffCode)
                query.AddParameterWithTypeValue("FLLWCMP", OracleDbType.NVarchar2, FllwCompleteFlgNotComplete)
                query.AddParameterWithTypeValue("BEGINLINE", OracleDbType.Decimal, getBeginLine)
                query.AddParameterWithTypeValue("ENDLINE", OracleDbType.Decimal, getEndLine)

                ' SQLの実行
                followListData = query.GetData()

            End Using

            ' 検索結果返却
            Return followListData
        End Function

#End Region

    End Class

End Namespace