'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060103TableAdapter.vb
'─────────────────────────────────────
'機能： 査定価格登録IF
'補足： 
'作成：  
'更新： 2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 坂井 2013/10対応版 既存流用
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' 中古車査定情報登録データアクセスクラス
''' </summary>
''' <remarks>中古車査定情報の登録を行います。</remarks>
Public Class IC3060103TableAdapter

#Region "定数"
    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 査定実績フラグ(実績あり)
    ''' </summary>
    Private Const C_ASMACTFLG_ON As String = "1"
    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "IC3060103"

    ''' <summary>
    ''' CSKIND = "1"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CSTKIND_ONE As String = "1"

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' 中古車査定情報登録データテーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private ucaAsesssmentDt As IC3060103DataSet.IC3060103SetUcarAssessmentDataTable

    ''' <summary>
    ''' 中古車査定情報登録対象項目リスト
    ''' </summary>
    ''' <remarks>更新対象項目名をリストに設定してください。</remarks>
    Private ucaAsesssmentUpColumnList As List(Of String)

#End Region

#Region "001.中古車査定情報取得"

    ''' <summary>
    ''' 中古車査定情報取得
    ''' </summary>
    ''' <returns>中古車査定情報取得DataTable</returns>
    ''' <remarks>中古車査定情報テーブルからのデータ取得を行います。</remarks>
    Public Function GetUcarAssessment() As IC3060103DataSet.IC3060103GetUcarAssessmentDataTable

        Logger.Info("GetUcarAssessment Start")

        Using query As New DBSelectQuery(Of IC3060103DataSet.IC3060103GetUcarAssessmentDataTable)("IC3060103_001")

            Dim sql As New StringBuilder

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            With sql
                .Append(" SELECT /* IC3060103_001 */ ")
                .Append("       NRQ.STATUS ")
                .Append("     , UAS.ASM_ACT_FLG ")
                .Append("     , UAS.CSTKIND ")
                .Append("  FROM TBL_UCARASSESSMENT UAS ")
                .Append("     , TBL_NOTICEREQUEST NRQ ")
                .Append(" WHERE UAS.NOTICEREQID = NRQ.NOTICEREQID ")
                .Append("   AND UAS.ASSESSMENTNO = :ASSESSMENTNO ")
            End With
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))

            Return query.GetData()
        End Using

        Logger.Info("GetUcarAssessment Was Normal End")

    End Function

#End Region

#Region "002.中古車査定情報更新"

    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
    'Public Function SetUcarAssessment() As Integer

    ''' <summary>
    ''' 中古車査定情報更新
    ''' </summary>
    ''' <returns>中古車査定情報取得DataTable</returns>
    ''' <remarks>中古車査定情報テーブルへの更新処理を行います。</remarks>
    Public Function SetUcarAssessment(ByVal asmActFlg As String) As Integer
        '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

        Logger.Info("SetUcarAssessment Start")

        Using query As New DBUpdateQuery("IC3060103_002")

            Dim collon As String = ""
            Dim sql As New StringBuilder
            With sql
                'UPDATE句----------------------------------------------
                .Append("UPDATE /* IC3060103_002 */ ")
                .Append("       TBL_UCARASSESSMENT ")

                'SET句----------------------------------------------
                .Append("   SET ")
                'メーカー名
                If ucaAsesssmentUpColumnList.Contains(ucaAsesssmentDt.MAKERNAMEColumn.ColumnName) Then '更新対象チェック（XMLタグ無しチェック）
                    .Append(collon)
                    .Append("       MAKERNAME = :MAKERNAME ")
                    collon = ","
                    query.AddParameterWithTypeValue("MAKERNAME", OracleDbType.NVarchar2, GetSqlData(ucaAsesssmentDt.MAKERNAMEColumn.ColumnName))
                End If

                '車名
                If ucaAsesssmentUpColumnList.Contains(ucaAsesssmentDt.VEHICLENAMEColumn.ColumnName) Then '更新対象チェック（XMLタグ無しチェック）
                    .Append(collon)
                    .Append("       VEHICLENAME = :VEHICLENAME ")
                    collon = ","
                    query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.NVarchar2, GetSqlData(ucaAsesssmentDt.VEHICLENAMEColumn.ColumnName))
                End If

                '登録番号
                If ucaAsesssmentUpColumnList.Contains(ucaAsesssmentDt.REGISTRATIONNOColumn.ColumnName) Then '更新対象チェック（XMLタグ無しチェック）
                    .Append(collon)
                    .Append("       REGISTRATIONNO = :REGISTRATIONNO ")
                    collon = ","
                    query.AddParameterWithTypeValue("REGISTRATIONNO", OracleDbType.NVarchar2, GetSqlData(ucaAsesssmentDt.REGISTRATIONNOColumn.ColumnName))
                End If

                '検査日
                If ucaAsesssmentUpColumnList.Contains(ucaAsesssmentDt.INSPECTIONDATEColumn.ColumnName) Then '更新対象チェック（XMLタグ無しチェック）
                    .Append(collon)
                    .Append("       INSPECTIONDATE = :INSPECTIONDATE ")
                    collon = ","
                    query.AddParameterWithTypeValue("INSPECTIONDATE", OracleDbType.Date, GetSqlData(ucaAsesssmentDt.INSPECTIONDATEColumn.ColumnName))
                End If

                '提示価格
                If ucaAsesssmentUpColumnList.Contains(ucaAsesssmentDt.APPRISAL_PRICEColumn.ColumnName) Then '更新対象チェック（XMLタグ無しチェック）
                    .Append(collon)
                    .Append("       APPRISAL_PRICE = :APPRISAL_PRICE ")
                    collon = ","
                    query.AddParameterWithTypeValue("APPRISAL_PRICE", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.APPRISAL_PRICEColumn.ColumnName))
                End If

                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
                '査定回答済フラグ
                If asmActFlg.Equals(C_ASMACTFLG_ON) Then '査定実績がある場合
                    .Append(collon)
                    .Append("       ASM_ANSWERED_FLG = '1' ")       '査定回答済
                    collon = ","
                End If
                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

                .Append("     , UPDATEDATE = SYSDATE ")             '更新日
                '更新アカウント
                .Append(collon)
                .Append("       UPDATEACCOUNT = :UPDATEACCOUNT ")
                collon = ","
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, GetSqlData(ucaAsesssmentDt.STAFFCDColumn.ColumnName))
                '更新機能ID
                .Append(collon)
                .Append("       UPDATEID = :UPDATEID ")
                collon = ","
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, GetSqlData(ucaAsesssmentDt.UPDATEIDColumn.ColumnName))

                'WHERE句----------------------------------------------
                .Append(" WHERE ASSESSMENTNO = :ASSESSMENTNO ") '査定No
                query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))
            End With

            query.CommandText = sql.ToString()

            Return query.Execute()
        End Using

        Logger.Info("SetUcarAssessment Was Normal End")

    End Function

#End Region


    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
#Region "003.査定モデル取得(自社客)"

    ''' <summary>
    ''' 査定モデル取得
    ''' </summary>
    ''' <returns>処理結果を格納したDataSet</returns>
    ''' <remarks>査定モデルを取得します。</remarks>
    Public Function GetModelName() As IC3060103DataSet.IC3060103GetModelNameDataTable

        Logger.Info("GetModelName Start")

        Using query As New DBSelectQuery(Of IC3060103DataSet.IC3060103GetModelNameDataTable)("IC3060103_007")

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* IC3060103_007 */ ")
                .Append("       T1.MODEL_NAME ")
                .Append("  FROM TB_M_MODEL T1 ")
                .Append(" WHERE T1.MODEL_CD = ")
                .Append("       ( ")
                .Append("        SELECT ")
                .Append("               T3.MODEL_CD ")
                .Append("          FROM TBL_UCARASSESSMENT T2 ")
                .Append("             , TB_M_VEHICLE T3 ")
                .Append("         WHERE T2.ORGCSTVCL_VIN = T3.VCL_VIN  ")
                .Append("           AND T2.ASSESSMENTNO = :ASSESSMENTNO ")
                .Append("       ) ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))
            Return query.GetData()

        End Using

        Logger.Info("GetModelName Was Normal End")

    End Function

#End Region
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
#Region "004.査定モデル取得(未取引客)"

    ''' <summary>
    ''' 査定モデル取得
    ''' </summary>
    ''' <returns>処理結果を格納したDataSet</returns>
    ''' <remarks>査定モデルを取得します。</remarks>
    Public Function GetModelNameNewCust() As IC3060103DataSet.IC3060103GetModelNameDataTable

        Logger.Info("GetModelNameNewCust Start")

        Using query As New DBSelectQuery(Of IC3060103DataSet.IC3060103GetModelNameDataTable)("IC3060103_008")

            Dim sql As New StringBuilder

            With sql
                .Append(" SELECT /* IC3060103_008 */ ")
                .Append("        T2.NEWCST_MODEL_NAME AS MODEL_NAME")
                .Append("   FROM TBL_UCARASSESSMENT T1 ")
                .Append("      , TB_M_VEHICLE T2 ")
                .Append("  WHERE T1.NEWCSTVCL_SEQNO = T2.VCL_ID ")
                .Append("    AND T1.ASSESSMENTNO = :ASSESSMENTNO ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))
            Return query.GetData()

        End Using

        Logger.Info("GetModelNameNewCust Was Normal End")

    End Function

#End Region
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END


    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
#Region "005.商談活動登録"

    ''' <summary>
    ''' 商談活動登録
    ''' </summary>
    ''' <returns>処理件数</returns>
    ''' <remarks>商談活動を登録します。</remarks>
    ''' 2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
    Public Function SetCRHis(ByVal modelName As String) As Integer

        Logger.Info("SetCRHis Start")

        Using query As New DBUpdateQuery("IC3060103_003")

            Dim sql As New StringBuilder

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            With sql
                .Append(" INSERT /* IC3060103_003 */ ")
                .Append(" INTO TB_T_SALES_ACT ")
                .Append(" ( ")
                .Append("     SALES_ACT_ID ")
                .Append("   , SALES_ID ")
                .Append("   , ACT_ID ")
                .Append("   , RSLT_SALES_CAT ")
                .Append("   , MODEL_CD ")
                .Append("   , ASSMNT_VCL_NAME ")
                .Append("   , DMS_TAKEIN_DATETIME ")
                .Append("   , CREATE_DATETIME ")
                .Append("   , ROW_CREATE_DATETIME ")
                .Append("   , ROW_CREATE_ACCOUNT ")
                .Append("   , ROW_CREATE_FUNCTION ")
                .Append("   , ROW_UPDATE_DATETIME ")
                .Append("   , ROW_UPDATE_ACCOUNT ")
                .Append("   , ROW_UPDATE_FUNCTION ")
                .Append("   , ROW_LOCK_VERSION ")
                .Append(" ) ")
                .Append(" ( ")
                .Append("     SELECT SQ_SALES_ACT.NEXTVAL ")
                .Append("          , FLLWUPBOX_SEQNO ")
                .Append("          , ACT_ID ")
                .Append("          , '7' ")
                .Append("          , ' ' ")
                .Append("          , :ASSMNT_VCL_NAME ")
                .Append("          , TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
                .Append("          , SYSDATE ")
                .Append("          , SYSDATE ")
                .Append("          , :ROW_CREATE_ACCOUNT ")
                .Append("          , :ROW_CREATE_FUNCTION ")
                .Append("          , SYSDATE ")
                .Append("          , :ROW_CREATE_ACCOUNT ")
                .Append("          , :ROW_CREATE_FUNCTION ")
                .Append("          , 0 ")
                .Append("       FROM TBL_FLLWUPBOXCRHIS_ASM ")
                .Append("      WHERE ASSESSMENTNO = :ASSESSMENTNO ")
                .Append(" ) ")
            End With
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            query.CommandText = sql.ToString()

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            query.AddParameterWithTypeValue("ASSMNT_VCL_NAME", OracleDbType.NVarchar2, modelName)
            query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, GetSqlData(ucaAsesssmentDt.STAFFCDColumn.ColumnName))
            query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, C_SYSTEM)
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            Return query.Execute()

        End Using

        Logger.Info("SetCRHis Was Normal End")

    End Function

#End Region

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START DEL
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
#Region "006.査定依頼中活動履歴削除"
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

    ''' <summary>
    ''' 査定依頼中活動履歴削除
    ''' </summary>
    ''' <returns>処理件数</returns>
    ''' <remarks>査定依頼中活動履歴を物理削除します。</remarks>
    Public Function DelFllwupBoxCRHisAsm() As Integer

        Logger.Info("DelFllwupBoxCRHisAsm Start")

        Using query As New DBUpdateQuery("IC3060103_005")

            Dim sql As New StringBuilder

            With sql
                .Append("DELETE /* IC3060103_005 */ ")
                .Append("  FROM TBL_FLLWUPBOXCRHIS_ASM ")
                .Append(" WHERE ASSESSMENTNO = :ASSESSMENTNO ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))

            Return query.Execute()

        End Using

        Logger.Info("DelFllwupBoxCRHisAsm Was Normal End")

    End Function

#End Region
    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END


    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
#Region "007.下取り査定情報 ロック取得"

    ''' <summary>
    ''' 下取り査定情報 ロック取得
    ''' </summary>
    ''' <remarks>下取り査定情報の更新対象レコードのロックを取得します。</remarks>
    Public Sub GetUcarAssessmentLock()

        Logger.Info("GetUcarAssessmentLock Start")
        Using query As New DBSelectQuery(Of DataTable)("IC3060103_006")
            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder

            With sql
                .Append(" SELECT /* IC3060103_006 */ ")
                .Append(" 1 ")
                .Append(" FROM TBL_UCARASSESSMENT ")
                .Append("  WHERE ASSESSMENTNO = :ASSESSMENTNO  ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Decimal, GetSqlData(ucaAsesssmentDt.ASSESSMENTNOColumn.ColumnName))
            query.GetData()

        End Using

        Logger.Info("GetUcarAssessmentLock Was Normal End")

    End Sub

#End Region
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

#Region "Privateクラス"
    ''' <summary>
    ''' SQL実行用にDBNULLをNothingへ変換
    ''' </summary>
    ''' <param name="targetName">値</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function GetSqlData(ByVal targetName As String) As Object

        Dim retObj As Object

        retObj = ucaAsesssmentDt.Rows(0).Item(targetName)

        'DBNULLはNothingへ変換
        If retObj Is System.Convert.DBNull Then
            retObj = Nothing
        End If

        Return retObj
    End Function

#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' イニシャライズ
    ''' </summary>
    ''' <param name="dt">中古車査定情報DataTable</param>
    ''' <param name="lt">中古車査定情報登録対象項目リスト</param>
    ''' <remarks>データアクセスクラスの中古車査定情報DataTableと、中古車査定情報登録対象項目リストをセットします。</remarks>
    Public Sub New(ByVal dt As IC3060103DataSet.IC3060103SetUcarAssessmentDataTable, ByVal lt As List(Of String))
        ucaAsesssmentDt = dt
        ucaAsesssmentUpColumnList = lt
    End Sub

#End Region
End Class
