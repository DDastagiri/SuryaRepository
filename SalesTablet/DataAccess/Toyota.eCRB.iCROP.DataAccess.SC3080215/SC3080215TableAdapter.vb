'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080215TableAdapter.vb
'─────────────────────────────────────
'機能： CSSurvey一覧・詳細
'補足： 
'作成： 2012/02/20 TCS 明瀬
'更新： 2013/06/30 TCS 坂井 2013/10対応版 既存流用
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

''' <summary>
''' CSSurvey一覧・詳細
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3080215TableAdapter

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムファイル名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMFILE As String = "SC3080215.ascx "

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    ''' <summary>
    ''' CSアンケート回答結果 - 使用中フラグ（使用中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INUSE_FLG = "1"

    ''' <summary>
    ''' CSアンケート用紙 - リリースフラグ（リリース済み）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RELEASED_FLG = "1"

    ''' <summary>
    ''' CSアンケート用紙 - 表示フラグ（表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISP_FLG = "1"
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    ''' <summary>
    ''' 店舗コード　000
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRCD_000 = "000"

#End Region

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    ''' <summary>
    ''' CSアンケート回答結果の一覧件数を取得する
    ''' </summary>
    ''' <param name="custId"></param>
    ''' <returns>検索結果を格納したDatatable</returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionListCountDT(ByVal custId As String) As SC3080215DataSet.SC3080215CSQuestionListCountDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[custId:{0}]", custId))
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        'SQL組み立て
        Dim sql As New StringBuilder
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        With sql
            .Append(" SELECT /* SC3080215_001 */ ")
            .Append("        COUNT(1) AS COUNT ")
            .Append("   FROM TB_T_CSQUESTION_ANS T1 ")
            .Append("      , TB_M_CSQUESTION T2 ")
            .Append("  WHERE T1.CSQUE_ID = T2.CSQUE_ID ")
            .Append("    AND T1.CST_ID = :CST_ID ")
            .Append("    AND T2.RELEASE_FLG = :RELEASED ")
            .Append("    AND T2.INUSE_FLG = :INUSE ")
        End With
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        Using query As New DBSelectQuery(Of SC3080215DataSet.SC3080215CSQuestionListCountDataTable)("SC3080215_001")
            query.CommandText = sql.ToString()

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, Trim(custId))          '顧客ID
            query.AddParameterWithTypeValue("RELEASED", OracleDbType.NVarchar2, INUSE_FLG)         'リリース済
            query.AddParameterWithTypeValue("INUSE", OracleDbType.NVarchar2, INUSE_FLG)            '使用中
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            'SQL実行
            Dim rtnDt As SC3080215DataSet.SC3080215CSQuestionListCountDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                      "_End[GetRowsCount:{0}]", rtnDt.Rows.Count))

            Return rtnDt

        End Using

    End Function

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    ''' <summary>
    ''' CSアンケート回答結果の一覧を取得する
    ''' </summary>
    ''' <param name="custId"></param>
    ''' <returns>検索結果を格納したDatatable</returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionListDT(ByVal custId As String) As SC3080215DataSet.SC3080215CSQuestionListDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[custId:{0}]", custId))
        'SQL組み立て
        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3080215_002 */ ")
            .Append("        T1.CSQUE_ANS_ID AS ANSWERID ")
            .Append("      , T1.ROW_UPDATE_DATETIME AS UPDATEDATE ")
            .Append("      , T2.CSQUE_NAME AS PAPERNAME ")
            .Append("      , T2.CSQUE_TYPE AS TARGETFLG ")
            .Append("      , NVL(NVL(T5.MODEL_NAME, T3.NEWCST_MODEL_NAME), ' ') AS SERIESNAME ")
            .Append("      , NVL(T4.REG_NUM, ' ') AS VCLREGNO ")
            .Append("      , T6.USERNAME ")
            .Append("      , T7.ICON_IMGFILE ")
            .Append("  FROM  TB_T_CSQUESTION_ANS T1 ")
            .Append("      , TB_M_CSQUESTION T2 ")
            .Append("      , TB_M_VEHICLE T3 ")
            .Append("      , TB_M_VEHICLE_DLR T4 ")
            .Append("      , TB_M_MODEL T5 ")
            .Append("      , TBL_USERS T6 ")
            .Append("      , TBL_OPERATIONTYPE T7 ")
            .Append(" WHERE T1.VCL_ID = T3.VCL_ID(+) ")
            .Append("   AND T1.CSQUE_ID = T2.CSQUE_ID ")
            .Append("   AND T3.MODEL_CD = T5.MODEL_CD(+) ")
            .Append("   AND T1.VCL_ID = T4.VCL_ID(+) ")
            .Append("   AND T1.DLR_CD = T4.DLR_CD(+) ")
            .Append("   AND DECODE(T1.ROW_UPDATE_ACCOUNT, ' ', T1.ROW_CREATE_ACCOUNT, T1.ROW_UPDATE_ACCOUNT) = T6.ACCOUNT ")
            .Append("   AND T6.OPERATIONCODE = T7.OPERATIONCODE(+) ")
            .Append("   AND T6.DLRCD = T7.DLRCD(+) ")
            .Append("   AND T1.CST_ID = :CST_ID ")
            .Append("   AND T2.RELEASE_FLG = :RELEASED ")
            .Append("   AND T2.INUSE_FLG = :INUSE_FLG ")
            .Append("   AND T7.STRCD = :STRCD000 ")
            .Append(" ORDER BY T1.ROW_UPDATE_DATETIME DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3080215DataSet.SC3080215CSQuestionListDataTable)("SC3080215_002")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, Trim(custId))       '顧客ID
            query.AddParameterWithTypeValue("RELEASED", OracleDbType.Char, RELEASED_FLG)        'リリース済み
            query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.Char, INUSE_FLG)          '使用中
            query.AddParameterWithTypeValue("STRCD000", OracleDbType.Char, STRCD_000)           '店舗コード（000固定）

            'SQL実行
            Dim rtnDt As SC3080215DataSet.SC3080215CSQuestionListDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                      "_End[GetRowsCount:{0}]", rtnDt.Rows.Count))
            Return rtnDt

        End Using

    End Function
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START DEL
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    ''' <summary>
    ''' CSアンケート回答結果の詳細を取得する
    ''' </summary>
    ''' <param name="answerId"></param>
    ''' <returns>検索結果を格納したDatatable</returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionDetailDT(ByVal answerId As Long) As SC3080215DataSet.SC3080215CSQuestionDetailDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[answerId:{0}]", answerId))

        'SQL組み立て
        Dim sql As New StringBuilder

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        With sql
            .Append(" SELECT /* SC3080215_004 */ ")
            .Append("        T1.CSQUE_ITEM_SEQ AS QUESTIONITEMID ")
            .Append("      , T1.CSQUE_CONTENT AS QUESTIONCONTENT ")
            .Append("      , T1.ANS_ITEM_TYPE AS ANSWERTYPE ")
            .Append("      , T1.ANS_ITEM_COUNT AS ANSWERCOUNT ")
            .Append("      , T1.SORT_ORDER AS SORTNO ")
            .Append("      , T2.ITEM_CONTENT AS ANSWERCONTENT ")
            .Append("      , T4.ANS_RSLT_TYPE AS CHECKVAL ")
            .Append("      , T4.ANS_CONTENT AS TEXTRESULT ")
            .Append(" FROM   TB_M_CSQUESTION_ITEM T1 ")
            .Append("      , TB_M_CSQUESTION_ITEM_SEL T2 ")
            .Append("      , TB_T_CSQUESTION_ANS T3 ")
            .Append("      , TB_T_CSQUESTION_ANS_ITEM T4 ")
            .Append(" WHERE  T1.CSQUE_ID = T2.CSQUE_ID ")
            .Append("   AND  T1.RELEASE_FLG = T2.RELEASE_FLG ")
            .Append("   AND  T1.CSQUE_ITEM_SEQ = T2.CSQUE_ITEM_SEQ ")
            .Append("   AND  T1.CSQUE_ID = T3.CSQUE_ID ")
            .Append("   AND  T3.CSQUE_ANS_ID = T4.CSQUE_ANS_ID ")
            .Append("   AND  T2.CSQUE_ITEM_SEQ = T4.CSQUE_ITEM_SEQ ")
            .Append("   AND  T2.CSQUE_ITEM_SEL_SEQ = T4.CSQUE_ITEM_SEL_SEQ ")
            .Append("   AND  T1.RELEASE_FLG = :RELEASED ")
            .Append("   AND  T1.DISP_FLG = :DISP_ON ")
            .Append("   AND  T4.CSQUE_ANS_ID = :ANSWERID ")
            .Append(" ORDER BY T1.SORT_ORDER, T2.CSQUE_ITEM_SEL_SEQ ")
        End With
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        Using query As New DBSelectQuery(Of SC3080215DataSet.SC3080215CSQuestionDetailDataTable)("SC3080215_004")
            query.CommandText = sql.ToString()

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            query.AddParameterWithTypeValue("RELEASED", OracleDbType.NVarchar2, RELEASED_FLG)        'リリース済み
            query.AddParameterWithTypeValue("DISP_ON", OracleDbType.NVarchar2, DISP_FLG)             '表示
            query.AddParameterWithTypeValue("ANSWERID", OracleDbType.Decimal, answerId)              '回答ID
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            'SQL実行
            Dim rtnDt As SC3080215DataSet.SC3080215CSQuestionDetailDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                      "_End[GetRowsCount:{0}]", rtnDt.Rows.Count))

            Return rtnDt

        End Using

    End Function

End Class
