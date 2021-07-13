'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' SC3080204DataSet.vb
'─────────────────────────────────────
'機能： 顧客メモ (データ)
'補足： 
'作成： 2011/12/??  ????
'更新： 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応
'更新： 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization


Namespace SC3080204DataSetTableAdapters
    Public Class SC3080204DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 顧客メモ履歴取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="crcustid">活動先顧客コード</param>
        ''' <returns>SC3080205CustDataTable</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Shared Function GetCustomerMemo(ByVal dlrcd As String, _
                                               ByVal crcustid As Decimal) _
                                               As SC3080204DataSet.SC3080204CustMemoDataTable

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            Dim sql As New StringBuilder

            'With sql
            '    .Append(" SELECT /* SC3080204_001 */ ")
            '    .Append("    A.CUSTMEMOHIS_SEQNO, ")            '顧客メモ連番
            '    .Append("    A.UPDATEDATE, ")                   '更新日
            '    .Append("    A.MEMO ")                          'メモ
            '    .Append("FROM ")
            '    .Append("    TBL_CUSTMEMOHIS A ")
            '    .Append("WHERE ")
            '    .Append("    A.DLRCD = :DLRCD ")
            '    .Append("AND ")
            '    .Append("    A.INSDID = :INSDID ")
            '    .Append("AND ")
            '    .Append("    DELFLG = '0' ")
            '    .Append("ORDER BY ")
            '    .Append("    A.UPDATEDATE DESC ")
            'End With

            'Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustMemoDataTable)("SC3080204_001")

            '    query.CommandText = sql.ToString()
            '    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)         '販売店コード
            '    query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, crcustid)     '活動先顧客コード

            '    Return query.GetData()

            'End Using

            With sql
                .AppendLine("   SELECT  /* SC3080204_001 */ ")
                .AppendLine("           T1.CST_MEMO_SEQ AS CUSTMEMOHIS_SEQNO ")
                .AppendLine("          ,T1.ROW_UPDATE_DATETIME AS UPDATEDATE ")
                .AppendLine("          ,T1.CST_MEMO AS MEMO ")
                .AppendLine("          ,T1.ROW_LOCK_VERSION ")
                .AppendLine("          ,T2.ROW_LOCK_VERSION AS CST_ROW_LOCK_VERSION ")
                '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
                .AppendLine("          ,'V4' AS DBDiv ")
                '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END
                .AppendLine("    FROM ")
                .AppendLine("           TB_T_CUSTOMER_MEMO T1 ")
                .AppendLine("          ,TB_M_CUSTOMER T2 ")
                .AppendLine("   WHERE   T1.DLR_CD = :DLRCD ")
                .AppendLine("     AND   T1.CST_ID = :INSDID ")
                .AppendLine("     AND   T2.CST_ID = T1.CST_ID ")
                .AppendLine("ORDER BY   T1.ROW_UPDATE_DATETIME DESC ")
            End With

            Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustMemoDataTable)("SC3080204_001")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)     '販売店コード
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Decimal, crcustid)   '内部管理ID

                Return query.GetData()

            End Using

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

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
        ''' <history>
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Shared Function InsertCustomerMemo(ByVal dlrcd As String, _
                                                  ByVal crcustid As Decimal, _
                                                  ByVal seqno As Long, _
                                                  ByVal memo As String, _
                                                  ByVal updateaccount As String) As Integer


            Dim sql As New StringBuilder

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("INSERT /* SC3080204_002 */ ")
            '    .Append("INTO ")
            '    .Append("    TBL_CUSTMEMOHIS ")
            '    .Append("( ")
            '    .Append("    CUSTMEMOHIS_SEQNO, ")  '顧客メモ連番
            '    .Append("    INSDID, ")             '内部管理ID
            '    .Append("    DLRCD, ")              '販売店コード
            '    .Append("    STRCD, ")              '店舗コード
            '    .Append("    ACCOUNT, ")            'アカウント
            '    .Append("    MEMO, ")               'メモ
            '    .Append("    CREATEDATE, ")         '作成日
            '    .Append("    UPDATEDATE, ")         '更新日
            '    .Append("    UPDATEACCOUNT, ")      '更新ユーザアカウント
            '    .Append("    CRCUSTNAME, ")         '活動先顧客名
            '    .Append("    CUSTSEGMENT, ")        '顧客区分
            '    .Append("    CUSTOMERCLASS, ")      '顧客分類
            '    .Append("    CRCUSTID ")            '活動先顧客コード
            '    .Append(") ")
            '    .Append("VALUES ")
            '    .Append("( ")
            '    .Append("    :CUSTMEMOHIS_SEQNO, ")  '顧客メモ連番
            '    .Append("    :INSDID, ")             '内部管理ID
            '    .Append("    :DLRCD, ")              '販売店コード
            '    .Append("    :STRCD, ")              '店舗コード
            '    .Append("    :ACCOUNT, ")            'アカウント
            '    .Append("    :MEMO, ")               'メモ
            '    .Append("    SYSDATE, ")             '作成日
            '    .Append("    SYSDATE, ")             '更新日
            '    .Append("    :UPDATEACCOUNT, ")      '更新ユーザアカウント
            '    .Append("    :CRCUSTNAME, ")         '活動先顧客名
            '    .Append("    :CUSTSEGMENT, ")        '顧客区分
            '    .Append("    :CUSTOMERCLASS, ")      '顧客分類
            '    .Append("    :CRCUSTID ")            '活動先顧客コード
            '    .Append(") ")

            'End With

            'Using query As New DBUpdateQuery("SC3080204_002")

            '    query.CommandText = sql.ToString()

            '    query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)             '顧客メモ連番
            '    query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, crcustid)                      '内部管理ID
            '    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                          '販売店コード
            '    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                          '店舗コード
            '    query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, updateaccount)            'アカウント
            '    query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)                       'メモ
            '    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)      '更新ユーザアカウント
            '    query.AddParameterWithTypeValue("CRCUSTNAME", OracleDbType.NVarchar2, crcustname)           'CR活動名
            '    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, custsegment)              '顧客区分
            '    query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)          '顧客区分
            '    query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)                    '活動先顧客コード


            '    Return query.Execute()

            'End Using

            With sql
                .AppendLine("INSERT  /* SC3080204_002 */ ")
                .AppendLine("  INTO  TB_T_CUSTOMER_MEMO ( ")
                .AppendLine("        DLR_CD ")
                .AppendLine("       ,CST_ID ")
                .AppendLine("       ,CST_MEMO_SEQ ")
                .AppendLine("       ,CST_MEMO ")
                .AppendLine("       ,CREATE_STF_CD ")
                .AppendLine("       ,CREATE_DATETIME ")
                .AppendLine("       ,ROW_CREATE_DATETIME ")
                .AppendLine("       ,ROW_CREATE_ACCOUNT ")
                .AppendLine("       ,ROW_CREATE_FUNCTION ")
                .AppendLine("       ,ROW_UPDATE_DATETIME ")
                .AppendLine("       ,ROW_UPDATE_ACCOUNT ")
                .AppendLine("       ,ROW_UPDATE_FUNCTION ")
                .AppendLine("       ,ROW_LOCK_VERSION ")
                .AppendLine("        ) ")
                .AppendLine("VALUES ( ")
                .AppendLine("       :DLRCD ")
                .AppendLine("      ,:INSDID ")
                .AppendLine("      ,:CUSTMEMOHIS_SEQNO ")
                .AppendLine("      ,:MEMO ")
                .AppendLine("      ,:ACCOUNT ")
                .AppendLine("      ,SYSDATE ")
                .AppendLine("      ,SYSDATE ")
                .AppendLine("      ,:ACCOUNT ")
                .AppendLine("      ,'SC3080204' ")
                .AppendLine("      ,SYSDATE ")
                .AppendLine("      ,:UPDATEACCOUNT ")
                .AppendLine("      ,'SC3080204' ")
                .AppendLine("      ,0 ")
                .AppendLine("        ) ")
            End With

            Using query As New DBUpdateQuery("SC3080204_002")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Decimal, crcustid)
                query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateaccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

                Return query.Execute()

            End Using

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End Function

        ''' <summary>
        ''' 顧客メモ更新
        ''' </summary>
        ''' <param name="seqno">顧客メモ連番</param>
        ''' <param name="memo">メモ</param>
        ''' <param name="updateaccount">更新ユーザアカウント</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="crcustid">顧客ID</param>
        ''' <param name="lockversion">更新カウント</param>
        ''' <returns>更新成功[True]/失敗[False]</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Shared Function UpdateCustomerMemo(ByVal seqno As Long, _
                                                  ByVal memo As String, _
                                                  ByVal updateaccount As String, _
                                                  ByVal dlrcd As String, _
                                                  ByVal crcustid As Decimal, _
                                                  ByVal lockversion As Long) As Integer


            Dim sql As New StringBuilder

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("UPDATE /* SC3080204_003 */ ")
            '    .Append("    TBL_CUSTMEMOHIS ")
            '    .Append("SET ")
            '    .Append("    MEMO = :MEMO, ")               'メモ
            '    .Append("    UPDATEACCOUNT = :UPDATEACCOUNT , ")      '更新ユーザアカウント
            '    .Append("    UPDATEDATE = SYSDATE ")         '更新日
            '    .Append("WHERE ")
            '    .Append("    CUSTMEMOHIS_SEQNO = :CUSTMEMOHIS_SEQNO ")

            'End With

            'Using query As New DBUpdateQuery("SC3080204_003")

            '    query.CommandText = sql.ToString()
            '    query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)                       'メモ
            '    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)      '更新ユーザアカウント

            '    query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)             '顧客メモ連番

            '    Return query.Execute()

            'End Using

            With sql
                .AppendLine("UPDATE  /* SC3080204_003 */ ")
                .AppendLine("        TB_T_CUSTOMER_MEMO ")
                .AppendLine("   SET ")
                .AppendLine("        CST_MEMO = :MEMO ")
                .AppendLine("       ,ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")
                .AppendLine("       ,ROW_UPDATE_DATETIME = SYSDATE ")
                .AppendLine("       ,ROW_UPDATE_FUNCTION = 'SC3080204' ")
                .AppendLine("       ,ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .AppendLine(" WHERE ")
                .AppendLine("        CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
                .AppendLine("　 AND  DLR_CD = :DLRCD ")
                .AppendLine("　 AND  CST_ID = :CSTID ")
                .AppendLine("　 AND  ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With

            Using query As New DBUpdateQuery("SC3080204_003")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MEMO", OracleDbType.NVarchar2, memo)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
                query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lockversion)

                Return query.Execute()

            End Using

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End Function

        ''' <summary>
        ''' 顧客メモ削除
        ''' </summary>
        ''' <param name="seqno">顧客メモ連番</param>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="crcustid">活動先顧客コード</param>
        ''' <returns>更新成功[True]/失敗[False]</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Shared Function DeleteCustomerMemo(ByVal seqno As Long, _
                                                  ByVal dlrcd As String, _
                                                  ByVal crcustid As Decimal) As Integer


            Dim sql As New StringBuilder

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("UPDATE /* SC3080204_004 */ ")
            '    .Append("    TBL_CUSTMEMOHIS ")
            '    .Append("SET ")
            '    .Append("    DELFLG = '1', ")               'メモ
            '    .Append("    UPDATEACCOUNT = :UPDATEACCOUNT , ")      '更新ユーザアカウント
            '    .Append("    UPDATEDATE = SYSDATE ")         '更新日
            '    .Append("WHERE ")
            '    .Append("    CUSTMEMOHIS_SEQNO = :CUSTMEMOHIS_SEQNO ")

            'End With

            'Using query As New DBUpdateQuery("SC3080204_004")

            '    query.CommandText = sql.ToString()
            '    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)      '更新ユーザアカウント

            '    query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)             '顧客メモ連番

            '    Return query.Execute()

            'End Using

            With sql
                .AppendLine("DELETE  /* SC3080204_004 */ ")
                .AppendLine("  FROM  TB_T_CUSTOMER_MEMO ")
                .AppendLine(" WHERE  CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
                .AppendLine("   AND  DLR_CD = :DLRCD ")
                .AppendLine("   AND  CST_ID = :CSTID ")
            End With

            Using query As New DBUpdateQuery("SC3080204_004")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)

                Return query.Execute()

            End Using

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End Function

        ''' <summary>
        ''' 顧客メモ連番采番
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="crcustid">活動先顧客コード</param>
        ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Shared Function GetCustmemoseq(ByVal dlrcd As String, ByVal crcustid As String) As Long

            Dim sql As New StringBuilder

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .Append("SELECT /* SC3080204_005 */ ")
            '    .Append("       SEQ_CUSTMEMOHIS_SEQNO.NEXTVAL AS SEQ ")
            '    .Append("  FROM DUAL")
            'End With

            'Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204SeqDataTable)("SC3080204_005")

            '    query.CommandText = sql.ToString()

            '    Dim seqTbl As SC3080204DataSet.SC3080204SeqDataTable

            '    seqTbl = query.GetData()

            '    Return seqTbl.Item(0).Seq

            'End Using

            With sql
                .AppendLine("SELECT  /* SC3080204_205 */ ")
                .AppendLine("        NVL(MAX(T1.CST_MEMO_SEQ), 0) + 1 AS SEQ ")
                .AppendLine("  FROM  TB_T_CUSTOMER_MEMO T1 ")
                .AppendLine(" WHERE  T1.DLR_CD = :DLRCD ")
                .AppendLine("   AND  T1.CST_ID = :CSTID ")
            End With

            Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204SeqDataTable)("SC3080204_205")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)

                Dim seqTbl As SC3080204DataSet.SC3080204SeqDataTable

                seqTbl = query.GetData()

                Return seqTbl.Item(0).Seq

            End Using

            '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        End Function

        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START 
        ''' <summary>
        ''' 親顧客データロック
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub GetCustomerLock(ByVal crcustid As Decimal)

            Using query As New DBSelectQuery(Of DataTable)("SC3080204_006")

                Dim env As New Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.SystemEnvSetting

                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT  /* SC3080204_006 */ ")
                    .AppendLine("        1 ")
                    .AppendLine("  FROM  TB_M_CUSTOMER ")
                    .AppendLine(" WHERE  CST_ID = :CST_ID ")
                    .AppendLine(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, crcustid)

                query.GetData()

            End Using

        End Sub
        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END 


        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
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

            With sql
                .AppendLine("INSERT  /* SC3080204_007 */ ")
                .AppendLine("  INTO  TB_T_CUSTOMER_MEMO_DEL ")
                .AppendLine("        SELECT  DLR_CD ")
                .AppendLine("               ,CST_ID ")
                .AppendLine("               ,CST_MEMO_SEQ ")
                .AppendLine("               ,CST_MEMO ")
                .AppendLine("               ,CREATE_STF_CD ")
                .AppendLine("               ,CREATE_DATETIME ")
                .AppendLine("               ,ROW_CREATE_DATETIME ")
                .AppendLine("               ,ROW_CREATE_ACCOUNT ")
                .AppendLine("               ,ROW_CREATE_FUNCTION ")
                .AppendLine("               ,ROW_UPDATE_DATETIME ")
                .AppendLine("               ,ROW_UPDATE_ACCOUNT ")
                .AppendLine("               ,ROW_UPDATE_FUNCTION ")
                .AppendLine("               ,ROW_LOCK_VERSION ")
                .AppendLine("          FROM  TB_T_CUSTOMER_MEMO T1 ")
                .AppendLine("         WHERE  T1.CST_MEMO_SEQ = :CUSTMEMOHIS_SEQNO ")
                .AppendLine("　  　     AND  T1.DLR_CD = :DLRCD ")
                .AppendLine("　　       AND  T1.CST_ID = :CSTID ")
            End With

            Using query As New DBUpdateQuery("SC3080204_007")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, crcustid)
                query.AddParameterWithTypeValue("CUSTMEMOHIS_SEQNO", OracleDbType.Int64, seqno)

                Return query.Execute()

            End Using

        End Function
        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
        ''' <summary>
        ''' V3顧客ID取得
        ''' </summary>
        ''' <param name="customerId">顧客ID</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <returns>SC3080204CustomerCodeDataTable</returns>
        ''' <remarks>V3用の顧客IDを取得</remarks>
        Public Shared Function GetV3CustomerCD(ByVal customerId As Decimal, ByVal dealerCode As String) As SC3080204DataSet.SC3080204CustomerCodeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0} START CST_ID:{1}, DLR_CD:{2}. " _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , customerId.ToString(CultureInfo.CurrentCulture()) _
                                     , dealerCode))


            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080204_008 */ ")
                ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
                '.Append("    NVL(TRIM(NEWCST_CD),ORGCST_CD) AS CST_CD, ")
                .Append("    NEWCST_CD, ")
                .Append("    ORGCST_CD, ")
                ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END
                .Append("    CST_TYPE ")
                .Append("FROM ")
                .Append("    TB_M_CUSTOMER A, ")
                .Append("    TB_M_CUSTOMER_DLR B ")
                .Append("WHERE ")
                .Append("    A.CST_ID = :CST_ID ")
                .Append("AND B.CST_ID = A.CST_ID ")
                .Append("AND B.DLR_CD = :DLR_CD ")
            End With

            Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustomerCodeDataTable)("SC3080204_008")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, customerId)   '顧客ID
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)   '販売店コード

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0} END" _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' V3未取引客の顧客ID取得
        ''' </summary>
        ''' <param name="originalid">顧客ID</param>
        ''' <returns>SC3080204CustomerCodeDataTable</returns>
        ''' <remarks>V3用の顧客IDを取得</remarks>
        Public Shared Function GetV3NewCustomerCD(ByVal originalid As String) As SC3080204DataSet.SC3080204CustomerCodeDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}  START CST_ID:{1}. " _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , originalid))

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT /* SC3080204_009 */ ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
                '.Append("    CSTID AS CST_CD ")
                .Append("    CSTID AS NEWCST_CD ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END
                .Append("FROM ")
                .Append("    TBL_NEWCUSTOMER ")
                .Append("WHERE ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
                '.Append("    TRIM(ORIGINALID) = :ORIGINALID ")
                .Append("    ORIGINALID = :ORIGINALID ")
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END
            End With

            Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustomerCodeDataTable)("SC3080204_009", DBQueryTarget.DMS)

                query.CommandText = sql.ToString()
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
                'query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Varchar2, originalid)   '内部管理ID
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid.Trim)   '内部管理ID
                '2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0} END" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' V3顧客メモ履歴取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="customerId">内部管理ID</param>
        ''' <param name="newcustid">未取引客ID</param>
        ''' <returns>SC3080204CustMemoDataTable</returns>
        ''' <remarks>V3のDBより顧客メモを取得</remarks>
        Public Shared Function GetV3CustomerMemo(ByVal dealerCode As String, ByVal customerId As String, ByVal newcustid As String) As SC3080204DataSet.SC3080204CustMemoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}  START DLR_CD:{1},CST_ID:{2}, NEW_CST_ID:{3}." _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , dealerCode _
                         , customerId _
                         , newcustid))

            Dim sql As New StringBuilder

            With sql
                .Append(" SELECT /* SC3080204_010 */ ")
                .Append("    CUSTMEMOHIS_SEQNO AS CUSTMEMOHIS_SEQNO, ")
                .Append("    UPDATEDATE AS UPDATEDATE, ")
                .Append("    MEMO AS MEMO, ")
                .Append("    '0' AS ROW_LOCK_VERSION, ")
                .Append("    '0' AS CST_ROW_LOCK_VERSION, ")
                .Append("    'V3' AS DBDiv ")
                .Append("FROM ")
                .Append("    TBL_CUSTMEMOHIS ")
                .Append("WHERE ")
                .Append("    DLRCD = :DLRCD ")
                .Append("AND ")

                If String.IsNullOrEmpty(newcustid) Then
                    .Append("    INSDID = :INSDID ")
                Else
                    .Append("    INSDID IN (:INSDID,:NEWCUSTID) ")
                End If

                .Append("AND ")
                .Append("    DELFLG = '0' ")
                .Append("ORDER BY ")
                .Append("    UPDATEDATE DESC ")
            End With

            Using query As New DBSelectQuery(Of SC3080204DataSet.SC3080204CustMemoDataTable)("SC3080204_010", DBQueryTarget.DMS)

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)      '販売店コード
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, customerId)     '内部管理ID

                If Not String.IsNullOrEmpty(newcustid) Then
                    query.AddParameterWithTypeValue("NEWCUSTID", OracleDbType.Char, newcustid) '自社客に紐付く未取引客ID
                End If

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0} END" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return query.GetData()

            End Using
        End Function

        '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END
    End Class

End Namespace

Partial Class SC3080204DataSet
End Class
