'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070301TableAdapter.vb
'─────────────────────────────────────
'機能： 契約書印刷
'補足： 
'作成： 2011/12/01 TCS 相田
'更新： 2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' 契約書印刷のデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3070301TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 契約書印刷フラグを印刷済みに更新します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdatePrintFlg(ByVal estimateId As Long,
                                    ByVal account As String,
                                    ByVal updateId As String) As Integer

        Using query As New DBUpdateQuery("SC3070301_001")

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3070301_001 */ ")
                .Append("        TBL_ESTIMATEINFO ")
                .Append(" SET    CONTPRINTFLG = :CONTPRINTFLG ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                .Append("      , UPDATEDATE = SYSDATE ")
                .Append(" WHERE  ESTIMATEID = :ESTIMATEID ")

            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId) '見積管理ID
            query.AddParameterWithTypeValue("CONTPRINTFLG", OracleDbType.Char, "1") '契約書印刷フラグ
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId) '更新ID

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 見積情報の更新を行います。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="constractFlg">契約状況フラグ</param>
    ''' <param name="constractDate">契約完了日</param>
    ''' <param name="constractNo">契約No.</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateConstractInfo(ByVal estimateId As Long,
                                        ByVal constractFlg As String,
                                        ByVal constractDate As Date,
                                        ByVal constractNo As String,
                                        ByVal account As String,
                                        ByVal updateId As String) As Integer

        Using query As New DBUpdateQuery("SC3070301_002")

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3070301_002 */ ")
                .Append("        TBL_ESTIMATEINFO ")
                .Append(" SET    CONTRACTFLG = :CONTRACTFLG ")
                .Append("      , CONTRACTDATE = :CONTRACTDATE ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                .Append("      , UPDATEDATE = SYSDATE ")
                .Append("      , CONTRACTNO = :CONTRACTNO ")
                .Append(" WHERE  ESTIMATEID = :ESTIMATEID ")

            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId) '見積管理ID
            query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.Char, constractNo) '契約No
            query.AddParameterWithTypeValue("CONTRACTFLG", OracleDbType.Char, constractFlg) '契約状況フラグ

            If Not constractDate = Date.MinValue Then
                query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, constractDate) '契約書完了日
            Else
                query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, DBNull.Value) '契約書完了日
            End If


            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId) '更新ID

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 削除フラグを更新します。
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="paymentMethod">支払方法区分</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDelFlg(ByVal estimateId As Long,
                                 ByVal paymentMethod As String,
                                 ByVal account As String,
                                 ByVal updateId As String) As Integer

        Using query As New DBUpdateQuery("SC3070301_003")

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3070301_003 */ ")
                .Append("        TBL_EST_PAYMENTINFO ")
                .Append(" SET    DELFLG = :DELFLG ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                .Append("      , UPDATEDATE = SYSDATE ")
                .Append(" WHERE  ESTIMATEID = :ESTIMATEID ")
                .Append(" AND    PAYMENTMETHOD = :PAYMENTMETHOD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId) '見積管理ID
            query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, paymentMethod) '支払方法区分
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "1") '削除フラグ
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId) '更新ID

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 基幹システムの外装色の取得します。
    ''' </summary>
    ''' <param name="modelCode">モデルコード</param>
    ''' <param name="bodyClrCd">外装色コード</param>
    ''' <returns>基幹システムデータテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetColorCode(ByVal modelCode As String,
                                 ByVal bodyClrCD As String) As SC3070301DataSet.MstextEriorDataTable
        Using query As New DBSelectQuery(Of SC3070301DataSet.MstextEriorDataTable)("SC3070301_004")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3070301_004 */ ")
                .Append("        COLOR_CD ")
                .Append(" FROM   TBL_MSTEXTERIOR ")
                .Append(" WHERE  VCLMODEL_CODE = :VCLMODEL_CODE ")
                .Append(" AND    BODYCLR_CD = :BODYCLR_CD ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("VCLMODEL_CODE", OracleDbType.Varchar2, modelCode)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, bodyClrCD)

            Return query.GetData()
        End Using
    End Function

    '2012/02/23 TCS 河原 【SALES_1B】Add Start
    ''' <summary>
    ''' キャンセル通知対象データ取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>キャンセル通知対象データ</returns>
    ''' <remarks></remarks>
    ''' <History>2012/02/23 TCS 河原 【SALES_1B】</History> 
    Public Shared Function GetNoticeRequest(ByVal estimateId As Long) As SC3070301DataSet.NoticeRequestDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3070301_005 */ ")
            .Append("    A.NOTICEREQID, ")
            .Append("    DECODE(A.STATUS,'1',TOACCOUNT,'3',FROMACCOUNT) AS TOACCOUNT ")
            .Append("FROM ")
            .Append("    TBL_NOTICEREQUEST A, ")
            .Append("    TBL_NOTICEINFO B ")
            .Append("WHERE ")
            .Append("    A.REQCLASSID = :REQCLASSID AND ")
            .Append("    A.STATUS IN ('1','3') AND ")
            .Append("    B.NOTICEID(+) = A.LASTNOTICEID ")
        End With
        Using query As New DBSelectQuery(Of SC3070301DataSet.NoticeRequestDataTable)("SC3070301_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("REQCLASSID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function
    '2012/02/23 TCS 河原 【SALES_1B】Add End

    '2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正 Add Start
    ''' <summary>
    ''' mst車名マスタテーブルから基幹車名コードを取得します。
    ''' </summary>
    ''' <param name="seriescd">シリーズコード</param>
    ''' <returns>基幹車名コードデータテーブル</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正
    ''' </History>
    Public Shared Function GetCarNameCode(ByVal seriescd As String) As SC3070301DataSet.SeriesCodeDataTable

        Using query As New DBSelectQuery(Of SC3070301DataSet.SeriesCodeDataTable)("SC3070301_005")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070301_005 */ ")
                .Append("    CAR_NAME_CD_AI21 ")            '基幹車名コード
                .Append("  FROM ")
                .Append("    TBL_MSTCARNAME ")
                .Append(" WHERE ")
                .Append("    VCLSERIES_CD = :VCLSERIES_CD ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("VCLSERIES_CD", OracleDbType.NVarchar2, seriescd)      'シリーズコード

            Return query.GetData()
        End Using
    End Function
    '2012/02/29 TCS 藤井 【SALES_1A】TACT連携時に正しいシリーズコードが連携できない不具合修正 Add End
End Class

