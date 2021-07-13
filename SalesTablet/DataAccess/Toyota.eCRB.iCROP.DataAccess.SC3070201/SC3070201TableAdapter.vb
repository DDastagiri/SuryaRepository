'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070201TableAdapter.vb
'─────────────────────────────────────
'機能： 見積作成
'補足： 
'作成： 2011/12/01 TCS 葛西
'更新： 2013/12/17 TCS 河原
'更新： 2014/03/18 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応
'更新： 2019/04/17 TS  村井 (FS)次世代タブレット新興国向けの性能評価
'更新： 2019/05/21 TS  舩橋 PostUAT-3092対応
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' SCメインのデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3070201TableAdapter

#Region "定数定義"
    ''' <summary>
    ''' 依頼内容マスタの価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REASON_DISCOUNT As String = "02"

    ''' <summary>
    ''' 中古車査定テーブルと結合時参照の通知情報テーブル・最終ステータス(4.受付)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_STATUS As String = "4"

    ''' <summary>
    ''' 見積価格相談テーブルの返答フラグ(1.回答済)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESPONSEFLG_ON As String = "1"

    ''' <summary>
    ''' 更新機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODULEID As String = "SC3070201"

#End Region

#Region "メソッド"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        'デフォルトコンストラクタ
    End Sub

    ''' <summary>
    ''' CR活動結果取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal estimateId As Long) As SC3070201DataSet.SC3070201FllwUpBoxDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201FllwUpBoxDataTable)("SC3070201_207")

            Dim sql As New StringBuilder
            ' 2014/03/18 TCS 松月 TMT不具合対応 Modify Start
            With sql
                .Append("SELECT /* SC3070201_207 */ ")
                .Append("       CASE ")
                .Append("            WHEN T3.REQ_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.REQ_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_T_SALES T2 ")
                .Append("     , TB_T_REQUEST T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.REQ_ID = T3.REQ_ID ")
                .Append("   AND T2.REQ_ID <> 0 ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                .Append("UNION ALL ")
                .Append("SELECT  ")
                .Append("       CASE ")
                .Append("            WHEN T3.REQ_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.REQ_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_H_SALES T2 ")
                .Append("     , TB_H_REQUEST T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.REQ_ID = T3.REQ_ID ")
                .Append("   AND T2.REQ_ID <> 0 ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                .Append("UNION ALL ")
                .Append("SELECT  ")
                .Append("       CASE ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_T_SALES T2 ")
                .Append("     , TB_T_ATTRACT T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.ATT_ID = T3.ATT_ID ")
                .Append("   AND T2.ATT_ID <> 0 ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
                .Append("UNION ALL ")
                .Append("SELECT  ")
                .Append("       CASE ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '31' THEN ")
                .Append("                 '4' ")
                .Append("            WHEN T3.CONTINUE_ACT_STATUS = '32' THEN ")
                .Append("                 '5' ")
                .Append("            ELSE '1' ")
                .Append("       END AS CRACTRESULT ")
                .Append("  FROM TBL_ESTIMATEINFO T1 ")
                .Append("     , TB_H_SALES T2 ")
                .Append("     , TB_H_ATTRACT T3 ")
                .Append(" WHERE T1.FLLWUPBOX_SEQNO = T2.SALES_ID ")
                .Append("   AND T2.ATT_ID = T3.ATT_ID ")
                .Append("   AND T2.ATT_ID <> 0 ")
                .Append("   AND T1.ESTIMATEID = :ESTIMATEID ")
            End With
            ' 2014/03/18 TCS 松月 TMT不具合対応 Modify End
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFollowupboxStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function

    ''' <summary>
    '''契約状況取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>契約状況テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal estimateId As Long) As SC3070201DataSet.SC3070201ContractDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractFlg_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201ContractDataTable)("SC3070201_014")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070201_014 */ ")
                .Append("       CONTRACTFLG ")                      '契約状況フラグ
                .Append("  FROM TBL_ESTIMATEINFO ")
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)       '見積管理ID
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractFlg_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function

    '2019/04/17 TS 村井 DEL (FS)次世代タブレット新興国向けの性能評価

    ''' <summary>
    ''' 見積管理ID取得
    ''' </summary>
    ''' <param name="fllwupboxSeqNo">Follow-up Box内連番</param>
    ''' <returns>見積管理IDテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateId(ByVal fllwUpBoxSeqNo As Decimal) As SC3070201DataSet.SC3070201EstimateIdDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201EstimateIdDataTable)("SC3070201_019")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070201_019 */ ")
                .Append("       ESTIMATEID ")                 '見積管理ID
                .Append("  FROM TBL_ESTIMATEINFO ")
                .Append(" WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append(" ORDER BY ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwUpBoxSeqNo)      'Follow-up Box内連番
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimateId_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()


        End Using


    End Function

    ''' <summary>
    ''' 見積情報ロック処理
    ''' </summary>
    ''' <param name="estimateid">見積ID </param>
    ''' <remarks></remarks>
    Public Shared Sub EstimateinfoLock(ByVal estimateid As Long)
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("EstimateinfoLock_Start")
        'ログ出力 End *****************************************************************************
        Using query As New DBSelectQuery(Of DataTable)("SC3070201_211")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* SC3070201_211 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TBL_ESTIMATEINFO ")
                .Append("WHERE ")
                .Append("  ESTIMATEID = :ESTIMATEID ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateid)
            query.GetData()

        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("EstimateinfoLock_End")
        'ログ出力 End *****************************************************************************



    End Sub

    ''' <summary>
    ''' 契約承認情報取得
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractApproval(ByVal estimateid As Long) As SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201CONTRACTAPPROVALDataTable)("SC3070201_023")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070201_023 */ ")
                '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
                .Append("    CONTRACTNO, ")
                '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END
                .Append("    CONTRACT_APPROVAL_STATUS, ")
                .Append("    CONTRACT_APPROVAL_STAFF, ")
                .Append("    CONTRACT_APPROVAL_REQUESTSTAFF ")
                '2019/05/17 TS 舩橋 PostUAT-3092対応 START
                .Append("   ,DELFLG ")
                '2019/05/17 TS 舩橋 PostUAT-3092対応 END
                .Append("FROM ")
                .Append("    TBL_ESTIMATEINFO ")
                .Append("WHERE ")
                .Append("    ESTIMATEID = :ESTIMATEID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContractApproval_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using


    End Function

    ''' <summary>
    ''' 削除フラグ更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="updateaccount">更新ユーザアカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateDelFlg(ByVal estimateid As Long, ByVal updateaccount As String) As Integer

        'SQLの組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3070201_030 */ ")
            .Append("    TBL_EST_PAYMENTINFO ")
            .Append("SET ")
            .Append("    DELFLG = '0', ")
            .Append("    UPDATEDATE = SYSDATE, ")
            .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ")
            .Append("    UPDATEID = :UPDATEID ")
            .Append("WHERE ")
            .Append("    ESTIMATEID = :ESTIMATEID ")
        End With

        Using query As New DBUpdateQuery("SC3070201_030")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MODULEID)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Varchar2, estimateid)

            Return query.Execute()
        End Using

    End Function

    ''' <summary>
    ''' 契約承認ステータス更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="updateaccount">更新ユーザアカウント</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateContractApprovalStatus(ByVal estimateid As Long, ByVal updateaccount As String) As Integer

        'SQLの組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3070201_024 */ ")
            .Append("    TBL_ESTIMATEINFO ")
            .Append("SET ")
            .Append("    CONTRACT_APPROVAL_STATUS = '0', ")
            .Append("    CONTRACT_APPROVAL_STAFF = NULL, ")
            .Append("    CONTRACT_APPROVAL_REQUESTDATE = NULL, ")
            .Append("    CONTRACT_APPROVAL_REQUESTSTAFF = NULL, ")
            .Append("    UPDATEDATE = SYSDATE, ")
            .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ")
            .Append("    UPDATEID = :UPDATEID ")
            .Append("WHERE ")
            .Append("    ESTIMATEID = :ESTIMATEID ")
        End With

        Using query As New DBUpdateQuery("SC3070201_024")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, MODULEID)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Varchar2, estimateid)

            Return query.Execute()
        End Using

    End Function

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START
    '共通基盤使用により削除

    ' ''' <summary>
    ' ''' 基幹販売店情報取得
    ' ''' </summary>
    ' ''' <param name="dlrcd"></param>
    ' ''' <param name="strcd"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetDmsCodeMap(ByVal dlrcd As String, ByVal strcd As String) As SC3070201DataSet.SC3070201DMSCODEMAPDataTable

    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCodeMap_Start")
    '    'ログ出力 End *****************************************************************************

    '    Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201DMSCODEMAPDataTable)("SC3070201_025")

    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* SC3070201_025 */ ")
    '            .Append("    DMS_CD_1, ")
    '            .Append("    DMS_CD_2 ")
    '            .Append("FROM ")
    '            .Append("    TB_M_DMS_CODE_MAP ")
    '            .Append("WHERE ")
    '            .Append("        ICROP_CD_1 = :ICROP_CD_1 ")
    '            .Append("    AND ICROP_CD_2 = :ICROP_CD_2 ")
    '            .Append("    AND DMS_CD_TYPE = '2' ")
    '        End With

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, dlrcd)
    '        query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, strcd)

    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCodeMap_End")
    '        'ログ出力 End *****************************************************************************

    '        Return query.GetData()

    '    End Using

    'End Function

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

    ''' <summary>
    ''' 自社客個人情報取得
    ''' </summary>
    ''' <param name="originalid">自社客コード</param>
    ''' <returns>SC3070205ORG_CUSTOMER</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgCustomerNametitle(ByVal originalid As String) As SC3070201DataSet.SC3070201CustNametitleDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomerNametitle_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201CustNametitleDataTable)("SC3070205_016")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_016 */ ")
                .Append("       CST_NAME AS NAME   ")
                .Append("     , NAMETITLE_NAME AS NAMETITLE  ")
                .Append("  FROM TB_M_CUSTOMER ")
                .Append(" WHERE CST_ID = :ORIGINALID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomerNametitle_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 未取引客敬称取得
    ''' </summary>
    ''' <param name="cstId">未取引客コード</param>
    ''' <returns>SC3070205NEWCUSTOMER</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomerNametitle(ByVal cstId As String) As SC3070201DataSet.SC3070201CustNametitleDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomerNametitle_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201CustNametitleDataTable)("SC3070205_017")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070205_017 */ ")
                .Append("       CST_NAME AS NAME ")
                .Append("     , NAMETITLE_NAME AS NAMETITLE ")
                .Append("  FROM TB_M_CUSTOMER ")
                .Append(" WHERE CST_ID = :CSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomerNametitle_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 顧客担当スタッフコード取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetStaffCD(ByVal dlrcd As String, ByVal cstid As Decimal) As SC3070201DataSet.SC3070201StaffCdDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffCd_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201StaffCdDataTable)("SC3070205_018")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("    SLS_PIC_STF_CD ")
                .Append("FROM ")
                .Append("    TB_M_CUSTOMER_VCL A, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        VCL_ID, ")
                .Append("        ROW_NUMBER() OVER (ORDER BY ROW_UPDATE_DATETIME DESC) ROW_NUM ")
                .Append("    FROM ")
                .Append("        TB_M_CUSTOMER_VCL ")
                .Append("    WHERE ")
                .Append("            DLR_CD = :DLR_CD ")
                .Append("        AND CST_ID = :CST_ID ")
                .Append("        AND CST_VCL_TYPE = '1' ")
                .Append("    ) B ")
                .Append("WHERE ")
                .Append("        A.DLR_CD = :DLR_CD ")
                .Append("    AND A.CST_ID = :CST_ID ")
                .Append("    AND A.CST_VCL_TYPE = '1' ")
                .Append("    AND B.ROW_NUM = 1 ")
                .Append("    AND A.VCL_ID = B.VCL_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dlrcd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetStaffCd_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 見積価格相談情報取得
    ''' </summary>
    ''' <param name="NoticeReqId">通知依頼ID</param>
    ''' <returns>見積価格相談情報テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAnswer(ByVal noticeReqId As Long) As SC3070201DataSet.SC3070201EstDiscountApprovalDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAnswer_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201EstDiscountApprovalDataTable)("SC3070201_009")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070201_009 */ ")
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

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAnswer_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using
    End Function

    ''' <summary>
    ''' 通知キャンセル済みチェック
    ''' </summary>
    ''' <param name="NoticeReqId">通知依頼ID</param>
    ''' <returns>通知依頼情報テーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAdviceStatus(ByVal noticeReqId As Long) As SC3070201DataSet.SC3070201NoticeRequestDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAdviceStatus_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201NoticeRequestDataTable)("SC3070201_010")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070201_010 */ ")
                .Append("       STATUS ")                      '最終ステータス
                .Append("  FROM TBL_NOTICEREQUEST ")
                .Append(" WHERE NOTICEREQID = :NOTICEREQID ")
            End With

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeReqId)       '通知依頼ID
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAdviceStatus_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    ''' <summary>
    ''' DMSID取得(自社客)
    ''' </summary>
    ''' <param name="originalId">顧客コード</param>
    ''' <returns>SC3070201DmsIdDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdOrg(ByVal originalId As String) As SC3070201DataSet.SC3070201DmsIdDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3070201_011 */ ")
            .Append("    DMS_CST_CD_DISP AS CUSTCD ")    '基幹顧客コード
            .Append("FROM ")
            .Append("    TB_M_CUSTOMER ")
            .Append("WHERE ")
            .Append("    CST_ID = :ORIGINALID ")         '顧客コード
        End With
        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201DmsIdDataTable)("SC3070201_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalId)  '顧客コード
            Dim rtnDt As SC3070201DataSet.SC3070201DmsIdDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function

    ''' <summary>
    ''' DMSID取得(未取引客)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesBkgNo">注文番号</param>
    ''' <returns>SC3070201DmsIdDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdNew(ByVal dlrcd As String, ByVal salesBkgNo As String) As SC3070201DataSet.SC3070201DmsIdDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3070201_012 */ ")
            .Append("    A.DMS_CST_CD_DISP AS CUSTCD ")  '基幹顧客コード
            .Append("FROM ")
            .Append("    TB_M_CUSTOMER A ")
            .Append("  , TB_T_SALESBOOKING B ")
            .Append("WHERE ")
            .Append("    A.CST_ID = B.CST_ID ")          '顧客コード
            .Append("AND ")
            .Append("    B.DLR_CD = :DLRCD ")            '販売店コード
            .Append("AND ")
            .Append("    B.SALESBKG_NUM = :SALESBKGNO ") '注文番号
        End With
        Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201DmsIdDataTable)("SC3070201_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)  '販売店コード
            query.AddParameterWithTypeValue("SALESBKGNO", OracleDbType.Char, salesBkgNo)  '注文番号
            Dim rtnDt As SC3070201DataSet.SC3070201DmsIdDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

#End Region

End Class
