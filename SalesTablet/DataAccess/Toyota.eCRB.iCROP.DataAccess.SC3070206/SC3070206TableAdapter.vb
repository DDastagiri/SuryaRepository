'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070206TableAdapter.vb
'─────────────────────────────────────
'機能： 価格相談回答
'補足： 
'更新： 2013/12/09 TCS 外崎  Aカード情報相互連携開発
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public Class SC3070206TableAdapter
    ''' <summary>
    ''' 依頼内容マスタの価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REASON_DISCOUNT As String = "02"

    ''' <summary>
    ''' 見積価格相談テーブルの返答フラグ(1.回答済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESPONSEFLG_ON As String = "1"

    ''' <summary>
    ''' 見積価格相談情報取得
    ''' </summary>
    ''' <param name="NoticeReqId">通知依頼ID</param>
    ''' <returns>見積価格相談情報テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAnswer(ByVal noticeReqId As Long) As SC3070206DataSet.SC3070206EstDiscountApprovalDataTable
        Using query As New DBSelectQuery(Of SC3070206DataSet.SC3070206EstDiscountApprovalDataTable)("SC3070206_001")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070206_001 */ ")
                .Append("       A.ESTIMATEID ")                  '見積管理ID
                .Append("     , A.SEQNO ")                       '依頼連番
                .Append("     , A.DLRCD ")                       '販売店コード
                .Append("     , A.STRCD ")                       '店舗コード
                .Append("     , A.STAFFACCOUNT ")                'スタッフアカウント
                .Append("     , C.USERNAME ")                    'スタッフ名
                .Append("     , A.REQUESTPRICE ")                '依頼額
                .Append("     , A.REASONID ")                    '値引き理由ID
                .Append("     , B.MSG_DLR ")                     '内容(現地語)
                .Append("     , A.REQUESTDATE ")                 '依頼日時
                .Append("     , A.MANAGERACCOUNT ")              'マネージャアカウント
                .Append("     , A.APPROVEDPRICE ")               '承認額
                .Append("     , A.MANAGERMEMO ")                 'マネージャ入力メモ
                .Append("     , A.APPROVEDDATE ")                '承認日時
                .Append("     , A.RESPONSEFLG ")                 '返答フラグ
                .Append("     , A.NOTICEREQID ")                 '通知依頼ID
                .Append("     , A.SERIESCD ")                    'シリーズコード
                .Append("     , A.MODELCD ")                     'モデルコード
                .Append("  FROM TBL_EST_DISCOUNTAPPROVAL A ")
                .Append("     , TBL_REQUESTINFOMST B ")
                .Append("     , TBL_USERS C ")
                .Append(" WHERE A.DLRCD = B.DLRCD(+) ")
                .Append("   AND A.REASONID = B.ID(+) ")
                .Append("   AND A.STAFFACCOUNT = C.ACCOUNT(+) ")
                .Append("   AND B.REQCLASS(+) = :REQCLASS ")
                .Append("   AND A.NOTICEREQID = :NOTICEREQID ")
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, REASON_DISCOUNT)       '依頼種別
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeReqId)       '通知依頼ID

            Return query.GetData()
        End Using
    End Function


    ''' <summary>
    ''' マネージャー回答登録
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="seqNo">依頼連番</param>
    ''' <param name="managerAccount">マネージャアカウント</param>
    ''' <param name="approvedPrice">承認額</param>
    ''' <param name="managerMemo">マネージャ入力メモ</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateid">更新機能ID</param>
    ''' <returns>成功 : True / 失敗 : False</returns>
    Public Shared Function RegistAnswer(ByVal estimateId As Decimal, _
                                        ByVal seqNo As Long, _
                                        ByVal managerAccount As String, _
                                        ByVal approvedPrice As Nullable(Of Double), _
                                        ByVal managerMemo As String, _
                                        ByVal updateAccount As String, _
                                        ByVal updateid As String) As Boolean
        Using query As New DBUpdateQuery("SC3070206_003")
            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* SC3070206_003 */ ")
                .Append("       TBL_EST_DISCOUNTAPPROVAL ")
                .Append("   SET MANAGERACCOUNT = :MANAGERACCOUNT ")     'マネージャアカウント
                .Append("     , APPROVEDPRICE = :APPROVEDPRICE ")       '承認額
                .Append("     , MANAGERMEMO = :MANAGERMEMO ")           'マネージャ入力メモ
                .Append("     , APPROVEDDATE = SYSDATE ")               '承認日時
                .Append("     , RESPONSEFLG = :RESPONSEFLG ")           '返答フラグ
                .Append("     , UPDATEDATE = SYSDATE ")                 '更新日
                .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("     , UPDATEID = :UPDATEID ")                 '更新機能ID
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")             '見積管理ID
                .Append("   AND SEQNO = :SEQNO ")                       '依頼連番
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("MANAGERACCOUNT", OracleDbType.Varchar2, managerAccount) 'マネージャアカウント
            query.AddParameterWithTypeValue("APPROVEDPRICE", OracleDbType.Double, approvedPrice)     '承認額
            query.AddParameterWithTypeValue("MANAGERMEMO", OracleDbType.NVarchar2, managerMemo)      'マネージャ入力メモ
            query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, RESPONSEFLG_ON)        'Follow-up Box販売店コード
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)   '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)             '更新機能ID
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)            '見積管理ID
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqNo)                      '依頼連番

            If query.Execute() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    Public Shared Function LockEstimateInfo(ByVal estimateId As Long) As Boolean
        Using query As New DBSelectQuery(Of DataTable)("SC3070206_004")
            Dim sql As New StringBuilder
            Dim env As New SystemEnvSetting
            With sql
                .Append("SELECT /* SC3070206_004 */ ")
                .Append("       1 ")
                .Append("  FROM TBL_ESTIMATEINFO ")
                .Append("  WHERE ESTIMATEID = :ESTIMATEID  ")
                .Append("  FOR UPDATE  " & env.GetLockWaitTime())
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

            Return True
        End Using
    End Function

    ''' <summary>
    ''' マネージャー回答登録・見積金額更新
    ''' </summary>
    ''' <param name="estimateId">/見積管理ID</param>
    ''' <param name="discountPrice">承認額</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="updateid">更新機能ID</param>
    ''' <returns>通知依頼情報テーブル</returns>
    Public Shared Function RegistDiscountPrice(ByVal estimateId As Long, _
                                               ByVal discountPrice As Nullable(Of Double), _
                                               ByVal updateAccount As String, _
                                               ByVal updateid As String) As Boolean
        Using query As New DBUpdateQuery("SC3070206_005")
            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* SC3070206_005 */ ")
                .Append("       TBL_ESTIMATEINFO ")
                .Append("   SET DISCOUNTPRICE = :DISCOUNTPRICE ")       '承認額
                .Append("     , UPDATEDATE = SYSDATE ")                 '更新日
                .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("     , UPDATEID = :UPDATEID ")                 '更新機能ID
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")             '見積管理ID

            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DISCOUNTPRICE", OracleDbType.Double, discountPrice)     '承認額
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)   '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)             '更新機能ID
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)            '見積管理ID

            If query.Execute() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    '''契約状況取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>契約状況テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal estimateId As Long) As SC3070206DataSet.SC3070206ContractDataTable
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractFlg_Start")

        Using query As New DBSelectQuery(Of SC3070206DataSet.SC3070206ContractDataTable)("SC3070206_006")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070206_006 */ ")
                .Append("       CONTRACTFLG ")                      '契約状況フラグ
                .Append("  FROM TBL_ESTIMATEINFO ")
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID

            Return query.GetData()

        End Using
    End Function

    ''' <summary>
    ''' 通知依頼情報を取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <returns>NoticeRequestInfoDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNoticeRequestInfo(ByVal noticeReqId As Long) As SC3070206DataSet.SC3070206NoticeRequestInfoDataTable
        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3070206_008 */ ")
            .Append("        T1.CRCUSTID ")
            .Append("      , T1.CUSTOMNAME ")
            .Append("      , T1.CSTKIND ")
            .Append("      , T1.CUSTOMERCLASS ")
            .Append("      , T1.SALESSTAFFCD ")
            .Append("      , T1.VCLID ")
            .Append("      , T1.FLLWUPBOXSTRCD ")
            .Append("      , T1.FLLWUPBOX ")
            .Append("      , T1.STATUS ")
            .Append("      , T2.STAFFACCOUNT AS FROMACCOUNT ")
            .Append("      , T3.USERNAME AS FROMACCOUNTNAME ")
            .Append("   FROM TBL_NOTICEREQUEST T1 ")
            .Append("      , TBL_EST_DISCOUNTAPPROVAL T2 ")
            .Append("      , TBL_USERS T3 ")
            .Append("  WHERE T1.NOTICEREQID = :NOTICEREQID ")
            .Append("    AND T1.NOTICEREQID = T2.NOTICEREQID ")
            .Append("    AND T2.STAFFACCOUNT = T3.ACCOUNT(+) ")
        End With

        Using query As New DBSelectQuery(Of SC3070206DataSet.SC3070206NoticeRequestInfoDataTable)("SC3070206_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeReqId)

            Return query.GetData()
        End Using
    End Function
End Class
