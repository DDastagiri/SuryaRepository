'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070204TableAdapter.vb
'─────────────────────────────────────
'機能： 見積書・契約書印刷処理
'補足： 
'作成： 2012/11/16 TCS 坪根
'更新： 2013/01/10 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'更新： 2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発
'更新： 2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' 見積書・契約書印刷のデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3070204TableAdapter

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "SC3070204"

#End Region
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 保険会社情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="insucomcd">保険会社コード</param>
    ''' <param name="insuKind">保険種別</param>
    ''' <returns>保険会社情報データ</returns>
    ''' <remarks></remarks>
    Public Function GetInsuranceCompanyInfo(ByVal dlrcd As String, _
                                            ByVal insucomcd As String, _
                                            ByVal insuKind As String) As SC3070204DataSet.SC3070204InsKindMastDataTable

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3070204_001 */ ")
            .Append("        T2.DLR_CD AS DLRCD , ")                 '販売店コード
            .Append("        T1.INS_COMPANY_CD AS INSUCOMCD , ")     '保険会社コード
            .Append("        T3.INSUKIND AS INSUKIND , ")            '保険種別
            .Append("        T3.INSUKINDNM AS INSUKINDNM , ")        '保険種別名称
            .Append("        T1.INS_COMPANY_NAME AS INSUCOMNM ")     '保険会社名
            .Append("   FROM TB_M_INSURANCE_COMPANY T1 , ")
            .Append("        TB_M_INSURANCE_COMPANY_DLR T2 , ")
            .Append("        TBL_EST_INSUKINDMAST T3 ")
            .Append("  WHERE T1.INS_COMPANY_CD = T2.INS_COMPANY_CD ")
            .Append("    AND T2.DLR_CD = RTRIM(T3.DLRCD(+)) ")
            .Append("    AND T2.INS_COMPANY_CD = RTRIM(T3.INSUCOMCD(+)) ")
            .Append("    AND T1.EST_FLG = '1' ")
            .Append("    AND T1.INS_COMPANY_CD = :INSUCOMCD ")
            .Append("    AND T2.DLR_CD = :DLRCD ")
            If Not String.IsNullOrEmpty(insuKind) Then
                .Append("    AND T3.INSUKIND = :INSUKIND ")
            End If
        End With

        Using query As New DBSelectQuery(Of SC3070204DataSet.SC3070204InsKindMastDataTable)("SC3070204_001")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, RTrim(dlrcd))         '販売店コード
            query.AddParameterWithTypeValue("INSUCOMCD", OracleDbType.NVarchar2, RTrim(insucomcd)) '保険会社コード
            If Not String.IsNullOrEmpty(insuKind) Then
                query.AddParameterWithTypeValue("INSUKIND", OracleDbType.Char, insuKind)           '保険種別
            End If

            '検索結果返却
            Dim rtnDt As SC3070204DataSet.SC3070204InsKindMastDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        End Using

    End Function

    ''' <summary>
    ''' 融資会社情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="financeComCd">融資会社コード</param>
    ''' <returns>融資会社情報データ</returns>
    ''' <remarks></remarks>
    Public Function GetFinanceCompanyInfo(ByVal dlrCD As String, ByVal financeComCD As String) As SC3070204DataSet.SC3070204FinanceComMastDataTable

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3070204_002 */ ")
            .Append("        T1.FNC_COMPANY_CD AS FINANCECOMCODE, ")  '融資会社コード
            .Append("        T1.FNC_COMPANY_NAME AS FINANCECOMNAME ") '融資会社名称
            .Append("   FROM TB_M_FINANCE_COMPANY T1, ")
            .Append("        TB_M_FINANCE_COMPANY_DLR T2 ")
            .Append("  WHERE T1.FNC_COMPANY_CD = T2.FNC_COMPANY_CD ")
            .Append("    AND T1.FNC_COMPANY_CD = :FINANCECOMCODE ")   '融資会社コード
            .Append("    AND T2.DLR_CD = :DLRCD ")                    '販売店コード
        End With

        Using query As New DBSelectQuery(Of SC3070204DataSet.SC3070204FinanceComMastDataTable)("SC3070204_002")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, RTrim(dlrCD))                      '販売店コード
            query.AddParameterWithTypeValue("FINANCECOMCODE", OracleDbType.NVarchar2, RTrim(financeComCD))      '融資会社コード

            '検索結果返却
            Dim rtnDt As SC3070204DataSet.SC3070204FinanceComMastDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        End Using

    End Function

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' キャンセル通知対象データ取得
    ''' </summary>
    ''' <param name="fllwupBoxSeqno">Follow-up Box内連番</param>
    ''' <returns>キャンセル通知対象データ</returns>
    ''' <remarks></remarks>
    Public Function GetNoticeRequest(ByVal fllwupBoxSeqno As Decimal) As SC3070204DataSet.SC3070204NoticeRequestDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3070204_003 */ ")
            .Append("        T1.NOTICEREQID , ")
            .Append("        DECODE(T1.STATUS, '1', T2.TOACCOUNT, '3', T2.FROMACCOUNT, '4', T2.FROMACCOUNT) AS TOACCOUNT , ")
            .Append("        T1.REQCLASSID ")
            .Append("   FROM TBL_NOTICEREQUEST T1 , ")
            .Append("        TBL_NOTICEINFO T2 , ")
            .Append("        TBL_ESTIMATEINFO T3 ")
            .Append("  WHERE T1.LASTNOTICEID = T2.NOTICEID(+) ")
            .Append("    AND T1.REQCLASSID = T3.ESTIMATEID  ")
            .Append("    AND T1.STATUS IN ('1', '3', '4') ")
            .Append("    AND T1.FLLWUPBOX = :FLLWUPBOX ")
        End With

        Using query As New DBSelectQuery(Of SC3070204DataSet.SC3070204NoticeRequestDataTable)("SC3070204_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX", OracleDbType.Decimal, fllwupBoxSeqno)

            '検索結果返却
            Dim rtnDt As SC3070204DataSet.SC3070204NoticeRequestDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            Return rtnDt
            '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        End Using

    End Function

    ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START DEL
    ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 見積印刷日更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="updateid">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Function UpdateEstimatePrintDate(ByVal estimateid As Long, _
                                            ByVal updateaccount As String, _
                                            ByVal updateid As String) As Integer
        'SQLの組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3070204_005 */ ")
            .Append("        TBL_ESTIMATEINFO ")
            .Append("    SET ESTPRINTDATE = SYSDATE ")         '見積印刷日
            .Append("      , UPDATEDATE = SYSDATE ")           '更新日
            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ") '更新アカウント
            .Append("      , UPDATEID = :UPDATEID ")           '更新機能ID
            '2013/01/10 TCS 橋本 【A.STEP2】Add Start
            .Append("      , EST_ACT_FLG = :EST_ACT_FLG ")      '見積実績フラグ
            '2013/01/10 TCS 橋本 【A.STEP2】Add End
            .Append(" WHERE ESTIMATEID = :ESTIMATEID ")        '見積管理ID
        End With

        Using query As New DBUpdateQuery("SC3070204_005")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)     '更新アカウント 
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, updateid)                    '更新機能ID
            '2013/01/10 TCS 橋本 【A.STEP2】Add Start
            query.AddParameterWithTypeValue("EST_ACT_FLG", OracleDbType.Char, "1")                      '見積実績フラグ
            '2013/01/10 TCS 橋本 【A.STEP2】Add End
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Varchar2, estimateid)            '見積管理ID

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Function UpdateContractPrintFlg(ByVal estimateId As Long,
                                            ByVal account As String,
                                            ByVal updateId As String) As Integer

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3070204_006 */ ")
            .Append("        TBL_ESTIMATEINFO ")
            .Append("    SET CONTPRINTFLG = :CONTPRINTFLG ")        '契約書印刷フラグ
            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")      '更新アカウント
            .Append("      , UPDATEID = :UPDATEID ")                '更新機能ID
            .Append("      , UPDATEDATE = SYSDATE ")                '更新日
            .Append("  WHERE ESTIMATEID = :ESTIMATEID ")            '見積管理ID
        End With

        Using query As New DBUpdateQuery("SC3070204_006")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)        '見積管理ID
            query.AddParameterWithTypeValue("CONTPRINTFLG", OracleDbType.Char, "1")             '契約書印刷フラグ
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)    '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)        '更新機能ID

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' 契約情報更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="constractFlg">契約状況フラグ</param>
    ''' <param name="constractDate">契約完了日</param>
    ''' <param name="constractNo">契約書No</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Function UpdateContractInfo(ByVal estimateId As Long,
                                        ByVal constractFlg As String,
                                        ByVal constractDate As Date,
                                        ByVal constractNo As String,
                                        ByVal account As String,
                                        ByVal updateId As String) As Integer

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3070204_007 */ ")
            .Append("        TBL_ESTIMATEINFO ")
            .Append("    SET CONTRACTFLG = :CONTRACTFLG ")
            .Append("      , CONTRACTDATE = :CONTRACTDATE ")
            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .Append("      , UPDATEID = :UPDATEID ")
            .Append("      , UPDATEDATE = SYSDATE ")
            .Append("      , CONTRACTNO = :CONTRACTNO ")
            .Append("  WHERE ESTIMATEID = :ESTIMATEID ")
        End With

        Using query As New DBUpdateQuery("SC3070204_007")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)            '見積管理ID
            query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.Char, constractNo)           '契約書No
            query.AddParameterWithTypeValue("CONTRACTFLG", OracleDbType.Char, constractFlg)         '契約状況フラグ

            If Not constractDate = Date.MinValue Then
                query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, constractDate)   '契約書完了日
            Else
                query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, DBNull.Value)    '契約書完了日
            End If

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)        '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)            '更新機能ID

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' 削除フラグ更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="paymentMethod">支払方法区分</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="updateId">更新機能ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Function UpdateDelFlg(ByVal estimateId As Long,
                                 ByVal paymentMethod As String,
                                 ByVal account As String,
                                 ByVal updateId As String) As Integer

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3070204_008 */ ")
            .Append("        TBL_EST_PAYMENTINFO ")
            .Append("    SET DELFLG = :DELFLG ")
            .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .Append("      , UPDATEID = :UPDATEID ")
            .Append("      , UPDATEDATE = SYSDATE ")
            .Append("  WHERE ESTIMATEID = :ESTIMATEID ")
            .Append("    AND PAYMENTMETHOD = :PAYMENTMETHOD ")
        End With

        Using query As New DBUpdateQuery("SC3070204_008")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)        '見積管理ID
            query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, paymentMethod)  '支払方法区分
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "1")                   '削除フラグ
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)    '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)        '更新機能ID

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
    Public Function GetColorCode(ByVal modelCode As String,
                                 ByVal bodyClrCD As String) As SC3070204DataSet.SC3070204MstextEriorDataTable
        Using query As New DBSelectQuery(Of SC3070204DataSet.SC3070204MstextEriorDataTable)("SC3070204_009")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3070204_009 */ ")
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

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 見積情報更新ロック取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <remarks></remarks>
    Public Sub GetEstimateinfoLock(ByVal estimateId As Long)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of DataTable)("SC3070204_010")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* SC3070204_010 */ ")
                    .Append("        1 ")
                    .Append("   FROM TBL_ESTIMATEINFO ")
                    .Append("  WHERE ESTIMATEID = :ESTIMATEID ")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
                query.GetData()

            End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, 1))
        ' ======================== ログ出力 終了 ========================

    End Sub
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

    '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
    ''' <summary>
    ''' 顧客テーブルから顧客情報を取得します。
    ''' </summary>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <returns>見積情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerInfo(ByVal crcustid As String) As SC3070204DataSet.SC3070204CustomerInfoDataTable

        Using query As New DBSelectQuery(Of SC3070204DataSet.SC3070204CustomerInfoDataTable)("SC3070204_010")

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* SC3070204_010 */ ")
                .Append("       CST_GENDER AS SEX ")                                  '性別
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）START
                '.Append("     , CST_BIRTH_DATE AS BIRTHDAY ")                         '生年月日
                .Append("  , CASE WHEN  T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T1.CST_BIRTH_DATE END AS BIRTHDAY ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）END
                .Append("  FROM TB_M_CUSTOMER ")                    '対象テーブル
                .Append(" WHERE CST_ID = :CRCUST_ID ")              '活動先顧客コード
            End With

            query.CommandText = sql.ToString()

            'バインド変数
            query.AddParameterWithTypeValue("CRCUST_ID", OracleDbType.Decimal, crcustid)  '活動先顧客コード

            Return query.GetData()
        End Using

    End Function
    '2013/10/25 TCS 葛西 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

End Class
