'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客メモ (データ)
'補足： 
'作成： 2011/11/24 TCS 安田
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2014/11/20 TCS 河原  TMT B案
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 趙 2013/10対応版 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

Public NotInheritable Class SC3080204TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 顧客メモ履歴取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustid">内部管理ID</param>
    ''' <returns>SC3080205CustDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerMemo(ByVal dlrcd As String, ByVal crcustid As Decimal) As SC3080204DataSet.SC3080204CustMemoDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080204_201 */ ")
            .Append("  T1.CST_MEMO_SEQ AS CUSTMEMOHIS_SEQNO , ")
            .Append("  T1.ROW_UPDATE_DATETIME AS UPDATEDATE , ")
            .Append("  T1.CST_MEMO AS MEMO , ")
            .Append("  T1.ROW_LOCK_VERSION , ")
            .Append("  T2.ROW_LOCK_VERSION AS CST_ROW_LOCK_VERSION, ")
            .Append("  'V4' AS DBDiv ")
            .Append("FROM ")
            .Append("  TB_T_CUSTOMER_MEMO T1 , ")
            .Append("  TB_M_CUSTOMER T2 ")
            .Append("WHERE ")
            .Append("      T1.DLR_CD = :DLRCD ")
            .Append("  AND T1.CST_ID = :INSDID ")
            .Append("  AND T2.CST_ID = T1.CST_ID ")
            .Append("ORDER BY ")
            .Append("  T1.ROW_UPDATE_DATETIME DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustMemoDataTable)("SC3080204_201")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)     '販売店コード
            query.AddParameterWithTypeValue("INSDID", OracleDbType.Decimal, crcustid)   '内部管理ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

            Return query.GetData()

        End Using

    End Function


    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
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
    Public Shared Function InsertCustomerMemo(ByVal dlrcd As String, _
                                ByVal crcustid As Decimal, _
                                ByVal seqno As Long, _
                                ByVal memo As String, _
                                ByVal updateaccount As String) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomerMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080204_202 */ ")
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
            .Append("    'SC3080204' , ")
            .Append("    SYSDATE , ")
            .Append("    :UPDATEACCOUNT , ")
            .Append("    'SC3080204' , ")
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
            '2013/06/30 TCS 趙 2013/10対応版 既存流用 END

            Return query.Execute()

        End Using

    End Function


    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 顧客メモ更新
    ''' </summary>
    ''' <param name="seqno">顧客メモ連番</param>
    ''' <param name="memo">メモ</param>
    ''' <param name="updateaccount">更新ユーザアカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateCustomerMemo(ByVal seqno As Long, _
                                ByVal memo As String, _
                                ByVal updateaccount As String, _
                                ByVal dlrcd As String, _
                                ByVal crcustid As Decimal, _
                                ByVal lockversion As Long) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080204_203 */ ")
            .Append("    TB_T_CUSTOMER_MEMO ")
            .Append("SET ")
            .Append("    CST_MEMO = :MEMO , ")
            .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080204' , ")
            .Append("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
            .Append("WHERE ")
            .Append("        CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
            .Append("　　AND DLR_CD = :DLRCD ")
            .Append("　　AND CST_ID = :CSTID ")
            .Append("　　AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
        End With

        Using query As New DBUpdateQuery("SC3080204_203")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lockversion)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerMemo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

            Return query.Execute()

        End Using

    End Function


    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 顧客メモ削除
    ''' </summary>
    ''' <param name="seqno">顧客メモ連番</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteCustomerMemo(ByVal seqno As Long, _
                                ByVal dlrcd As String, _
                                ByVal crcustid As Decimal) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCustomerMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("DELETE ")
            .Append("    /* SC3080204_204 */ ")
            .Append("FROM ")
            .Append("    TB_T_CUSTOMER_MEMO ")
            .Append("WHERE ")
            .Append("        CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
            .Append("    AND DLR_CD = :DLRCD ")
            .Append("    AND CST_ID = :CSTID ")
        End With

        Using query As New DBUpdateQuery("SC3080204_204")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCustomerMemo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

            Return query.Execute()

        End Using

    End Function


    '2013/06/30 TCS 趙 2013/10対応版 START
    ''' <summary>
    ''' 顧客メモ連番採番
    ''' </summary>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustmemoseq(ByVal dlrcd As String, _
                                ByVal crcustid As Decimal) As Long
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080204_205 */ ")
            .Append("    NVL(MAX(CST_MEMO_SEQ),0) + 1 AS SEQ ")
            .Append("FROM ")
            .Append("  TB_T_CUSTOMER_MEMO ")
            .Append("WHERE ")
            .Append("  DLR_CD = :DLRCD ")
            .Append("  AND CST_ID = :CSTID ")
        End With

        Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204SeqDataTable)("SC3080204_205")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)

            Dim seqTbl As SC3080204DataSet.SC3080204SeqDataTable

            seqTbl = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

            Return seqTbl.Item(0).Seq

        End Using

    End Function

    ''' 2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
    ''' <summary>
    ''' 親顧客データロック
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub GetCustomerLock(ByVal crcustid As Decimal)
        Using query As New DBSelectQuery(Of DataTable)("SC3080204_206")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerLock_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080204_206 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :CST_ID ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, crcustid)

            query.GetData()

        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerLock_End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 


    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 顧客メモ履歴移動
    ''' </summary>
    ''' <param name="seqno">顧客メモ連番</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function MoveCustomerMemo(ByVal seqno As Long, _
                                ByVal dlrcd As String, _
                                ByVal crcustid As Decimal) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveCustomerMemo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080204_207 */ ")
            .Append("INTO TB_T_CUSTOMER_MEMO_DEL ")
            .Append("    SELECT ")
            .Append("        * ")
            .Append("    FROM ")
            .Append("      TB_T_CUSTOMER_MEMO T1 ")
            .Append("    WHERE ")
            .Append("          CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
            .Append("　  　AND DLR_CD = :DLRCD ")
            .Append("　　  AND CST_ID = :CSTID ")
        End With

        Using query As New DBUpdateQuery("SC3080204_207")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)
            query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("MoveCustomerMemo_End")
            'ログ出力 End *****************************************************************************
            Return query.Execute()

        End Using

    End Function
    ' 2013/06/30 TCS 趙 2013/10対応版　既存流用 END 

    '2014/11/20 TCS 河原  TMT B案 START
    ''' <summary>
    ''' V3顧客ID取得
    ''' </summary>
    ''' <param name="crcustid">顧客ID</param>
    ''' <returns>SC3080204Cst_CDDataTable</returns>
    ''' <remarks>V3用の顧客IDを取得</remarks>
    Public Shared Function GetV3CustomerCD(ByVal crcustid As Decimal, ByVal dlr_cd As String) As SC3080204DataSet.SC3080204Cst_CDDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080204_208 */ ")
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
        Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204Cst_CDDataTable)("SC3080204_208")
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
    Public Shared Function GetV3NewCustomerCD(ByVal originalid As String) As SC3080204DataSet.SC3080204Cst_CDDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080204_209 */ ")
            .Append("    CSTID AS CST_CD ")
            .Append("FROM ")
            .Append("    TBL_NEWCUSTOMER ")
            .Append("WHERE ")
            .Append("    TRIM(ORIGINALID) = :ORIGINALID ")
        End With
        Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204Cst_CDDataTable)("SC3080204_209", DBQueryTarget.DMS)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Varchar2, originalid)   '内部管理ID
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' V3顧客メモ履歴取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustid">内部管理ID</param>
    ''' <param name="newcustid">未取引客ID</param>
    ''' <returns>SC3080204CustMemoDataTable</returns>
    ''' <remarks>V3のDBより顧客メモを取得</remarks>
    Public Shared Function GetV3CustomerMemo(ByVal dlrcd As String, ByVal crcustid As String, ByVal newcustid As String) As SC3080204DataSet.SC3080204CustMemoDataTable
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3080204_210 */ ")
            .Append("    A.CUSTMEMOHIS_SEQNO AS CUSTMEMOHIS_SEQNO, ")      '顧客メモ連番
            .Append("    A.UPDATEDATE AS UPDATEDATE, ")                    '更新日
            .Append("    A.MEMO AS MEMO, ")                                'メモ
            .Append("    '0' AS ROW_LOCK_VERSION, ")
            .Append("    '0' AS CST_ROW_LOCK_VERSION, ")
            .Append("    'V3' AS DBDiv ")
            .Append("FROM ")
            .Append("    TBL_CUSTMEMOHIS A ")
            .Append("WHERE ")
            .Append("    A.DLRCD = :DLRCD ")
            .Append("AND ")
            If String.IsNullOrEmpty(newcustid) Then
                .Append("    A.INSDID = :INSDID ")
            Else
                .Append("    A.INSDID IN (:INSDID,:NEWCUSTID) ")
            End If
            .Append("AND ")
            .Append("    DELFLG = '0' ")
            .Append("ORDER BY ")
            .Append("    A.UPDATEDATE DESC ")
        End With
        Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustMemoDataTable)("SC3080204_210", DBQueryTarget.DMS)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)         '販売店コード
            query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, crcustid)     '内部管理ID
            If Not String.IsNullOrEmpty(newcustid) Then
                query.AddParameterWithTypeValue("NEWCUSTID", OracleDbType.Char, newcustid) '自社客に紐付く未取引客ID
            End If
            Return query.GetData()
        End Using
    End Function

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

    '2014/11/20 TCS 河原  TMT B案 END

End Class


