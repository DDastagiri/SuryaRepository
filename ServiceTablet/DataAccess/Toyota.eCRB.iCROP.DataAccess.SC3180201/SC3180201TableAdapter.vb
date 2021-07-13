'-------------------------------------------------------------------------
'SC3180201TableAdapter.vb
'-------------------------------------------------------------------------
'機能：完成検査承認画面(ビジネスロジック)
'補足：
'作成：2014/02/25 AZ宮澤	初版作成
'更新：2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────
Option Strict On
Option Explicit On
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3180201DataSetTableAdapter

    Public NotInheritable Class SC3180201TableAdapter

#Region "規定値"
        'Private Const UpdateTypeSend As Long = 1         ' Send更新処理
        'Private Const UpdateTypeRegist As Long = 2       ' regist更新処理
        Private Const UpdateTypeApprove As Long = 3      ' Approve更新処理
        Private Const UpdateTypeReject As Long = 4       ' Reject更新処理
        Private Const DefaultPreviousReplaceMile As Decimal = -1             ' 前回走行距離初期値

        ''' <summary>
        ''' ROステータス
        ''' </summary>
        ''' <remarks>RO情報のROステータス</remarks>
        Private Structure RO_Status
            ''' <summary>
            ''' 着工指示待ち(顧客承認完了)
            ''' </summary>
            ''' <remarks>着工指示待ち(顧客承認完了)："50"</remarks>
            Public Const Wait_Startwork As String = "50"

            ''' <summary>
            ''' R/Oキャンセル
            ''' </summary>
            ''' <remarks>R/Oキャンセル："99"</remarks>
            Public Const RO_Cancel As String = "99"
        End Structure

        ''' <summary>
        ''' 着工指示フラグ
        ''' </summary>
        ''' <remarks>作業指示の着工指示フラグ</remarks>
        Private Structure Startwork
            ''' <summary>
            ''' 未指示
            ''' </summary>
            ''' <remarks>未指示："0"</remarks>
            Public Const UnInstruct As String = "0"
        End Structure

        '2015/04/14 新販売店追加対応 start
        ''' <summary>
        ''' 全販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AllDealer As String = "XXXXX"

        ''' <summary>
        ''' 全店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AllBranch As String = "XXX"

        ''' <summary>
        ''' マスタ登録状態フラグ（登録なし）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ExistFlag As String = "0"
        '2015/04/14 新販売店追加対応 end

#End Region

        ''' <summary>
        ''' GetDBHederInfo(ヘッダ情報取得)
        ''' </summary> 
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>ヘッダー情報</returns>
        ''' <remarks></remarks>
        Public Function GetDBHederInfo(ByVal dlrCD As String, _
                                       ByVal brnCD As String, _
                                       ByVal roNum As String) As SC3180201HederInfoDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201HederInfoDataTable)("SC3180201_001")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_001 */ ")
                    .AppendLine("       DISTINCT(B.REG_NUM), ")
                    .AppendLine("       A.ACCEPTANCE_TYPE, ")
                    .AppendLine("       C.MODEL_CD, ")
                    .AppendLine("       I.MODEL_NAME, ")
                    .AppendLine("       A.SVCIN_ID, ")
                    .AppendLine("       G.SVC_CLASS_NAME, ")
                    .AppendLine("       A.SCHE_DELI_DATETIME, ")
                    .AppendLine("       A.CONTACT_PERSON_NAME, ")
                    .AppendLine("       A.RSLT_SVCIN_DATETIME, ")
                    .AppendLine("       D.USERNAME, ")
                    .AppendLine("       A.CONTACT_PHONE, ")
                    .AppendLine("       A.RSLT_DELI_DATETIME, ")
                    .AppendLine("       E.UPDATE_DATETIME, ")                                  '2019/06/10 ジョブ名複数時対応
                    '.AppendLine("       B.VIP_FLG, ")

                    'TKMローカル対応
                    '.AppendLine("       B.IMP_VCL_FLG, ")

                    .AppendLine("       A.ROW_LOCK_VERSION AS SRV_ROW_LOCK_VERSION, ")
                    .AppendLine("       H.ROW_LOCK_VERSION AS RO_ROW_LOCK_VERSION ")
                    .AppendLine(" FROM ")
                    .AppendLine("     TB_T_SERVICEIN A ")
                    .AppendLine("    ,TB_M_VEHICLE_DLR B ")
                    .AppendLine("    ,TB_M_VEHICLE C ")
                    .AppendLine("    ,TBL_USERS D ")
                    .AppendLine("    ,TB_T_JOB_DTL E ")
                    .AppendLine("    ,TB_M_MERCHANDISE F ")
                    .AppendLine("    ,TB_M_SERVICE_CLASS G ")
                    .AppendLine("    ,TB_T_RO_INFO H ")
                    .AppendLine("    ,TB_M_MODEL I ")
                    .AppendLine(" WHERE ")
                    .AppendLine("        B.VCL_ID(+)=A.VCL_ID ")
                    .AppendLine("    AND B.DLR_CD=A.DLR_CD ")
                    .AppendLine("    AND C.VCL_ID(+)=A.VCL_ID ")
                    .AppendLine("    AND D.ACCOUNT(+)=A.PIC_SA_STF_CD ")
                    .AppendLine("    AND E.SVCIN_ID(+)=A.SVCIN_ID ")
                    .AppendLine("    AND F.MERC_ID(+)=E.MERC_ID ")
                    .AppendLine("    AND G.SVC_CLASS_ID(+)=F.SVC_CLASS_ID ")
                    .AppendLine("    AND A.RO_NUM=H.RO_NUM(+) ")
                    .AppendLine("    AND C.MODEL_CD = I.MODEL_CD(+) ")
                    .AppendLine("    AND A.DLR_CD= :DLR_CD  ")
                    .AppendLine("    AND A.BRN_CD= :BRN_CD  ")
                    .AppendLine("    AND A.RO_NUM= :RO_NUM  ")
                    .AppendLine("    ORDER BY E.UPDATE_DATETIME ASC ")                          '2019/06/10 ジョブ名複数時対応
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        '2015/04/14 新販売店追加対応 start

        ''' <summary>
        ''' GetDBInspecCode(点検明細項目取得)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
        ''' <returns>点検項目情報リストデータセット</returns>
        ''' <remarks></remarks>
        Public Function GetDBInspectCode(ByVal dlrCD As String, _
                                         ByVal brnCD As String, _
                                         ByVal roNum As String, _
                                         ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean)) As SC3180201InspectCodeDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201InspectCodeDataTable)("SC3180201_002")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    ' '' ''.AppendLine("  /* SC3180201_002 */ ")
                    ' '' ''.AppendLine("SELECT  A.DLR_CD ")
                    ' '' ''.AppendLine("       ,A.BRN_CD ")
                    ' '' ''.AppendLine("       ,A.SVCIN_ID ")
                    ' '' ''.AppendLine("       ,B.JOB_DTL_ID ")
                    ' '' ''.AppendLine("       ,B.INSPECTION_NEED_FLG ")
                    ' '' ''.AppendLine("       ,B.INSPECTION_STATUS ")
                    ' '' ''.AppendLine("       ,C.STALL_USE_ID ")
                    ' '' ''.AppendLine("       ,C.STALL_USE_STATUS ")
                    ' '' ''.AppendLine("       ,D.JOB_INSTRUCT_ID ")
                    ' '' ''.AppendLine("       ,D.JOB_INSTRUCT_SEQ ")
                    ' '' ''.AppendLine("       ,D.RO_NUM ")
                    ' '' ''.AppendLine("       ,D.RO_SEQ ")
                    ' '' ''.AppendLine("       ,D.JOB_CD ")
                    ' '' ''.AppendLine("       ,D.JOB_NAME ")
                    ' '' ''.AppendLine("       ,K.INSPEC_TYPE ")
                    ' '' ''.AppendLine("       ,K.MODEL_CD ")
                    ' '' ''.AppendLine("       ,K.GRADE_CD ")
                    ' '' ''.AppendLine("       ,K.INSPEC_ITEM_CD ")
                    ' '' ''.AppendLine("       ,K.INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,K.SUB_INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_FIX ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    ' '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_SWAP ")
                    ' '' ''.AppendLine("       ,K.DISP_TEXT_PERM ")
                    ' '' ''.AppendLine("       ,K.PART_CD ")
                    ' '' ''.AppendLine("       ,K.PART_NAME ")
                    ' '' ''.AppendLine("       ,N.APROVAL_STATUS ")
                    ' '' ''.AppendLine("       ,N.ADVICE_CONTENT ")
                    ' '' ''.AppendLine("       ,N.JOB_INSTRUCT_ID ")
                    ' '' ''.AppendLine("       ,N.JOB_INSTRUCT_SEQ ")
                    ' '' ''.AppendLine("       ,N.INSPEC_ITEM_CD ")
                    ' '' ''.AppendLine("       ,N.INSPEC_RSLT_CD ")
                    ' '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_REPLACE ")
                    ' '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_FIX ")
                    ' '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_CLEAN ")
                    ' '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_SWAP ")
                    ' '' ''.AppendLine("       ,N.RSLT_BEFORE_TEXT ")
                    ' '' ''.AppendLine("       ,N.RSLT_AFTER_TEXT ")
                    ' '' ''.AppendLine("       ,N.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION ")
                    ' '' ''.AppendLine("FROM  TB_T_SERVICEIN A ")
                    ' '' ''.AppendLine("     ,TB_T_JOB_DTL B ")
                    ' '' ''.AppendLine("     ,TB_T_STALL_USE C ")
                    ' '' ''.AppendLine("     ,TB_T_JOB_INSTRUCT D ")
                    ' '' ''.AppendLine("     ,TB_M_OPERATION_CHANGE E ")
                    ' '' ''.AppendLine("     ,(SELECT  F.JOB_DTL_ID ")
                    ' '' ''.AppendLine("       ,H.INSPEC_TYPE ")
                    ' '' ''.AppendLine("       ,H.MODEL_CD ")
                    ' '' ''.AppendLine("       ,H.GRADE_CD ")
                    ' '' ''.AppendLine("       ,H.INSPEC_ITEM_CD ")
                    ' '' ''.AppendLine("       ,I.INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,I.SUB_INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_FIX ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_SWAP ")
                    ' '' ''.AppendLine("       ,I.DISP_TEXT_PERM ")
                    ' '' ''.AppendLine("       ,J.PART_CD ")
                    ' '' ''.AppendLine("       ,J.PART_NAME ")
                    ' '' ''.AppendLine("       FROM TB_T_JOB_INSTRUCT F ")
                    ' '' ''.AppendLine("            ,TB_M_OPERATION_CHANGE G ")
                    ' '' ''.AppendLine("            ,TB_M_INSPECTION_COMB H ")
                    ' '' ''.AppendLine("            ,TB_M_INSPECTION_DETAIL I ")
                    ' '' ''.AppendLine("            ,TB_M_PARTNAME J ")
                    ' '' ''.AppendLine("       WHERE F.JOB_CD = G.MAINTE_CD ")
                    ' '' ''.AppendLine("         AND G.INSPEC_TYPE = H.INSPEC_TYPE ")
                    ' '' ''.AppendLine("         AND G.MODEL_CD = H.MODEL_CD ")
                    ' '' ''.AppendLine("         AND G.GRADE_CD = H.GRADE_CD ")
                    ' '' ''.AppendLine("         AND H.INSPEC_ITEM_CD = I.INSPEC_ITEM_CD(+) ")
                    ' '' ''.AppendLine("         AND H.PART_CD = J.PART_CD)K ")
                    ' '' ''.AppendLine("     ,(SELECT L.JOB_DTL_ID ")
                    ' '' ''.AppendLine("         ,L.DLR_CD ")
                    ' '' ''.AppendLine("         ,L.BRN_CD ")
                    ' '' ''.AppendLine("         ,L.RO_NUM ")
                    ' '' ''.AppendLine("         ,L.APROVAL_STATUS ")
                    ' '' ''.AppendLine("         ,L.ADVICE_CONTENT ")
                    ' '' ''.AppendLine("         ,M.JOB_INSTRUCT_ID ")
                    ' '' ''.AppendLine("         ,M.JOB_INSTRUCT_SEQ ")
                    ' '' ''.AppendLine("         ,M.INSPEC_ITEM_CD ")
                    ' '' ''.AppendLine("         ,M.INSPEC_RSLT_CD ")
                    ' '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_REPLACE ")
                    ' '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_FIX ")
                    ' '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_CLEAN ")
                    ' '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_SWAP ")
                    ' '' ''.AppendLine("         ,M.RSLT_BEFORE_TEXT ")
                    ' '' ''.AppendLine("         ,M.RSLT_AFTER_TEXT ")
                    ' '' ''.AppendLine("         ,L.ROW_LOCK_VERSION ")
                    ' '' ''.AppendLine("       FROM  TB_T_INSPECTION_HEAD L ")
                    ' '' ''.AppendLine("            ,TB_T_INSPECTION_DETAIL M ")
                    ' '' ''.AppendLine("       WHERE L.JOB_DTL_ID = M.JOB_DTL_ID ")
                    ' '' ''.AppendLine("         AND L.DLR_CD = M.DLR_CD ")
                    ' '' ''.AppendLine("         AND L.BRN_CD = M.BRN_CD ")
                    ' '' ''.AppendLine("         AND L.DLR_CD = :DLR_CD ")
                    ' '' ''.AppendLine("         AND L.BRN_CD = :BRN_CD ")
                    ' '' ''.AppendLine("         AND L.RO_NUM = :RO_NUM) N ")
                    ' '' ''.AppendLine("WHERE A.SVCIN_ID = B.SVCIN_ID ")
                    ' '' ''.AppendLine("  AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
                    ' '' ''.AppendLine("  AND C.JOB_DTL_ID = D.JOB_DTL_ID ")
                    ' '' ''.AppendLine("  AND D.JOB_CD = E.MAINTE_CD ")
                    ' '' ''.AppendLine("  AND E.INSPEC_TYPE = K.INSPEC_TYPE ")
                    ' '' ''.AppendLine("  AND E.MODEL_CD = K.MODEL_CD ")
                    ' '' ''.AppendLine("  AND E.GRADE_CD = K.GRADE_CD ")
                    ' '' ''.AppendLine("  AND K.JOB_DTL_ID = N.JOB_DTL_ID(+) ")
                    ' '' ''.AppendLine("  AND K.INSPEC_ITEM_CD = N.INSPEC_ITEM_CD(+) ")
                    ' '' ''.AppendLine("  AND A.DLR_CD = :DLR_CD ")
                    ' '' ''.AppendLine("  AND A.BRN_CD = :BRN_CD ")
                    ' '' ''.AppendLine("  AND A.RO_NUM = :RO_NUM ")
                    ' '' ''.AppendLine("  AND C.STALL_USE_ID IN  ")
                    ' '' ''.AppendLine("(SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE GROUP BY JOB_DTL_ID) ")
                    ' '' ''If partCD IsNot "" Then
                    ' '' ''    .AppendLine("      AND K.PART_CD=:PART_CD ")
                    ' '' ''End If
                    ' '' ''.AppendLine("ORDER BY K.INSPEC_ITEM_CD,C.STALL_USE_STATUS DESC ,C.JOB_DTL_ID ")

                    '' ''.AppendLine("  /* SC3180201_002 */ ")
                    '' ''.AppendLine("SELECT  A.DLR_CD ")
                    '' ''.AppendLine("       ,A.BRN_CD ")
                    '' ''.AppendLine("       ,A.SVCIN_ID ")
                    '' ''.AppendLine("       ,B.JOB_DTL_ID ")
                    '' ''.AppendLine("       ,B.INSPECTION_NEED_FLG ")
                    '' ''.AppendLine("       ,B.INSPECTION_STATUS ")
                    '' ''.AppendLine("       ,C.STALL_USE_ID ")
                    '' ''.AppendLine("       ,C.STALL_USE_STATUS ")
                    '' ''.AppendLine("       ,D.JOB_INSTRUCT_ID ")
                    '' ''.AppendLine("       ,D.JOB_INSTRUCT_SEQ ")
                    '' ''.AppendLine("       ,D.RO_NUM ")
                    '' ''.AppendLine("       ,D.RO_SEQ ")
                    '' ''.AppendLine("       ,D.JOB_CD ")
                    '' ''.AppendLine("       ,D.JOB_NAME ")
                    ' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
                    ' '' ''.AppendLine("       ,K.INSPEC_TYPE ")
                    '' ''.AppendLine("       ,K.SVC_CD AS INSPEC_TYPE ")
                    ' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
                    '' ''.AppendLine("       ,K.MODEL_CD ")
                    '' ''.AppendLine("       ,K.GRADE_CD ")
                    '' ''.AppendLine("       ,K.INSPEC_ITEM_CD ")
                    '' ''.AppendLine("       ,K.INSPEC_ITEM_NAME ")
                    '' ''.AppendLine("       ,K.SUB_INSPEC_ITEM_NAME ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_FIX ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    '' ''.AppendLine("       ,K.DISP_INSPEC_ITEM_NEED_SWAP ")
                    '' ''.AppendLine("       ,K.DISP_TEXT_PERM ")
                    '' ''.AppendLine("       ,K.PART_CD ")
                    '' ''.AppendLine("       ,K.PART_NAME ")
                    '' ''.AppendLine("       ,N.APROVAL_STATUS ")
                    '' ''.AppendLine("       ,N.ADVICE_CONTENT ")
                    '' ''.AppendLine("       ,N.JOB_INSTRUCT_ID ")
                    '' ''.AppendLine("       ,N.JOB_INSTRUCT_SEQ ")
                    '' ''.AppendLine("       ,N.INSPEC_ITEM_CD ")
                    '' ''.AppendLine("       ,N.INSPEC_RSLT_CD ")
                    '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_REPLACE ")
                    '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_FIX ")
                    '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_CLEAN ")
                    '' ''.AppendLine("       ,N.OPERATION_RSLT_ALREADY_SWAP ")
                    ' '' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　START　↓↓↓
                    '' ''.AppendLine("       ,N.RSLT_VALUE_BEFORE AS RSLT_BEFORE_TEXT ")
                    '' ''.AppendLine("       ,N.RSLT_VALUE_AFTER AS RSLT_AFTER_TEXT ")
                    ' '' ''.AppendLine("       ,N.RSLT_BEFORE_TEXT ")
                    ' '' ''.AppendLine("       ,N.RSLT_AFTER_TEXT ")
                    ' '' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　END　　↑↑↑
                    '' ''.AppendLine("       ,N.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION ")
                    '' ''.AppendLine("FROM  TB_T_SERVICEIN A ")
                    '' ''.AppendLine("     ,TB_T_JOB_DTL B ")
                    '' ''.AppendLine("     ,TB_T_STALL_USE C ")
                    '' ''.AppendLine("     ,TB_T_JOB_INSTRUCT D ")

                    ' '' ''2015/05/28 Edit
                    ' '' ''.AppendLine("     ,TB_M_OPERATION_CHANGE E ")

                    ' '' ''.AppendLine("     ,(SELECT  F.JOB_DTL_ID ")
                    '' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
                    '' '' ''.AppendLine("       ,H.SVC_CD ")
                    ' '' ''.AppendLine("       ,H.INSPEC_TYPE ")
                    '' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
                    ' '' ''.AppendLine("       ,H.MODEL_CD ")
                    ' '' ''.AppendLine("       ,H.GRADE_CD ")
                    ' '' ''.AppendLine("       ,H.INSPEC_ITEM_CD ")
                    ' '' ''.AppendLine("       ,I.INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,I.SUB_INSPEC_ITEM_NAME ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_FIX ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    ' '' ''.AppendLine("       ,I.DISP_INSPEC_ITEM_NEED_SWAP ")
                    ' '' ''.AppendLine("       ,I.DISP_TEXT_PERM ")
                    ' '' ''.AppendLine("       ,J.PART_CD ")
                    ' '' ''.AppendLine("       ,J.PART_NAME ")
                    ' '' ''.AppendLine("       FROM TB_T_JOB_INSTRUCT F ")
                    ' '' ''.AppendLine("            ,TB_M_OPERATION_CHANGE G ")
                    ' '' ''.AppendLine("            ,TB_M_INSPECTION_COMB H ")
                    ' '' ''.AppendLine("            ,TB_M_INSPECTION_DETAIL I ")
                    ' '' ''.AppendLine("            ,TB_M_PARTNAME J ")
                    ' '' ''.AppendLine("       WHERE F.JOB_CD = G.MAINTE_CD ")
                    '' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
                    '' '' ''.AppendLine("         AND G.INSPEC_TYPE = H.SVC_CD ")
                    ' '' ''.AppendLine("         AND G.INSPEC_TYPE = H.INSPEC_TYPE ")
                    '' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
                    '' '' ''.AppendLine("         AND G.MODEL_CD = H.MODEL_CD ")
                    '' '' ''.AppendLine("         AND G.GRADE_CD = H.GRADE_CD ")
                    ' '' ''.AppendLine("         AND H.INSPEC_ITEM_CD = I.INSPEC_ITEM_CD(+) ")
                    ' '' ''.AppendLine("         AND H.PART_CD = J.PART_CD)K ")

                    '' ''.AppendLine("  ,(SELECT DISTINCT ")
                    '' ''.AppendLine("      TB_M_VEHICLE.MODEL_CD ")
                    '' ''.AppendLine("    , TB_M_VEHICLE.GRADE_NAME AS GRADE_CD ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_COMB.INSPEC_ITEM_CD ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.INSPEC_ITEM_NAME ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.SUB_INSPEC_ITEM_NAME ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_FIX ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_SWAP ")
                    '' ''.AppendLine("    , TB_M_INSPECTION_DETAIL.DISP_TEXT_PERM ")
                    '' ''.AppendLine("    , TB_M_PARTNAME.PART_CD ")
                    '' ''.AppendLine("    , TB_M_PARTNAME.PART_NAME  ")
                    '' ''.AppendLine("    , TB_T_SERVICEIN.SVCIN_ID ,TB_M_INSPECTION_COMB.SVC_CD")
                    '' ''.AppendLine("  FROM ")
                    '' ''.AppendLine("     TB_T_SERVICEIN  ")
                    '' ''.AppendLine("    ,TB_M_VEHICLE  ")
                    '' ''.AppendLine("    ,TB_M_INSPECTION_COMB  ")
                    '' ''.AppendLine("    ,TB_M_INSPECTION_DETAIL  ")
                    '' ''.AppendLine("    ,TB_M_PARTNAME  ")
                    '' ''.AppendLine("  WHERE ")
                    '' ''.AppendLine("      TB_T_SERVICEIN.VCL_ID = TB_M_VEHICLE.VCL_ID ")
                    '' ''.AppendLine("  AND TB_M_VEHICLE.MODEL_CD = TB_M_INSPECTION_COMB.MODEL_CD ")
                    '' ''.AppendLine("  AND TB_M_VEHICLE.GRADE_NAME = TB_M_INSPECTION_COMB.GRADE_CD ")
                    '' ''.AppendLine("  AND TB_M_INSPECTION_COMB.INSPEC_ITEM_CD = TB_M_INSPECTION_DETAIL.INSPEC_ITEM_CD   ")
                    '' ''.AppendLine("  AND TB_M_INSPECTION_COMB.DLR_CD=TB_T_SERVICEIN.DLR_CD ")
                    '' ''.AppendLine("  AND TB_M_INSPECTION_COMB.BRN_CD=TB_T_SERVICEIN.BRN_CD ")
                    '' ''.AppendLine("  AND TB_M_INSPECTION_COMB.PART_CD = TB_M_PARTNAME.PART_CD ")
                    '' ''If partCD IsNot "" Then
                    '' ''    .AppendLine("  AND TB_M_INSPECTION_COMB.PART_CD=:PART_CD ")
                    '' ''End If
                    '' ''.AppendLine("  AND TB_T_SERVICEIN.DLR_CD=:DLR_CD ")
                    '' ''.AppendLine("  AND TB_T_SERVICEIN.BRN_CD=:BRN_CD ")
                    '' ''.AppendLine("  AND TB_T_SERVICEIN.RO_NUM=:RO_NUM ")
                    '' ''.AppendLine(" )K ")


                    '' ''.AppendLine("     ,(SELECT L.JOB_DTL_ID ")
                    '' ''.AppendLine("         ,L.DLR_CD ")
                    '' ''.AppendLine("         ,L.BRN_CD ")
                    '' ''.AppendLine("         ,L.RO_NUM ")
                    '' ''.AppendLine("         ,L.APROVAL_STATUS ")
                    '' ''.AppendLine("         ,L.ADVICE_CONTENT ")
                    '' ''.AppendLine("         ,M.JOB_INSTRUCT_ID ")
                    '' ''.AppendLine("         ,M.JOB_INSTRUCT_SEQ ")
                    '' ''.AppendLine("         ,M.INSPEC_ITEM_CD ")
                    '' ''.AppendLine("         ,M.INSPEC_RSLT_CD ")
                    '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_REPLACE ")
                    '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_FIX ")
                    '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_CLEAN ")
                    '' ''.AppendLine("         ,M.OPERATION_RSLT_ALREADY_SWAP ")
                    ' '' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　START　↓↓↓
                    '' ''.AppendLine("         ,M.RSLT_VALUE_BEFORE ")
                    '' ''.AppendLine("         ,M.RSLT_VALUE_AFTER ")
                    ' '' ''.AppendLine("         ,M.RSLT_BEFORE_TEXT ")
                    ' '' ''.AppendLine("         ,M.RSLT_AFTER_TEXT ")
                    ' '' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　END　　↑↑↑
                    '' ''.AppendLine("         ,L.ROW_LOCK_VERSION ")
                    '' ''.AppendLine("       FROM  TB_T_INSPECTION_HEAD L ")
                    '' ''.AppendLine("            ,TB_T_INSPECTION_DETAIL M ")
                    '' ''.AppendLine("       WHERE L.JOB_DTL_ID = M.JOB_DTL_ID ")
                    ' '' ''2014/05/26 「DLR_CD」と「BRN_CD」を削除　START　↓↓↓
                    ' '' ''.AppendLine("         AND L.DLR_CD = M.DLR_CD ")
                    ' '' ''.AppendLine("         AND L.BRN_CD = M.BRN_CD ")
                    ' '' ''2014/05/26 「DLR_CD」と「BRN_CD」を削除　END　　↑↑↑
                    '' ''.AppendLine("         AND L.DLR_CD = :DLR_CD ")
                    '' ''.AppendLine("         AND L.BRN_CD = :BRN_CD ")
                    '' ''.AppendLine("         AND L.RO_NUM = :RO_NUM) N ")
                    '' ''.AppendLine("WHERE A.SVCIN_ID = B.SVCIN_ID ")
                    '' ''.AppendLine("  AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
                    '' ''.AppendLine("  AND C.JOB_DTL_ID = D.JOB_DTL_ID ")
                    ' '' ''.AppendLine("  AND D.JOB_CD = E.MAINTE_CD ")
                    ' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　START　↓↓↓
                    ' '' ''.AppendLine("  AND E.INSPEC_TYPE = K.SVC_CD ")
                    ' '' ''.AppendLine("  AND E.INSPEC_TYPE = K.INSPEC_TYPE ")
                    ' '' ''2014/05/26 カラム名変更「INSPEC_TYPE→SVC_CD」　END　　↑↑↑
                    ' '' ''.AppendLine("  AND E.MODEL_CD = K.MODEL_CD ")
                    ' '' ''.AppendLine("  AND E.GRADE_CD = K.GRADE_CD ")
                    ' '' ''.AppendLine("  AND K.JOB_DTL_ID = N.JOB_DTL_ID(+) ")
                    '' ''.AppendLine("  AND K.INSPEC_ITEM_CD = N.INSPEC_ITEM_CD(+) ")
                    '' ''.AppendLine("  AND A.DLR_CD = :DLR_CD ")
                    '' ''.AppendLine("  AND A.BRN_CD = :BRN_CD ")
                    '' ''.AppendLine("  AND A.RO_NUM = :RO_NUM ")
                    '' ''.AppendLine("  AND C.STALL_USE_ID IN  ")
                    '' ''.AppendLine("(SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE T1 ")
                    '' ''.AppendLine(" WHERE B.JOB_DTL_ID = T1.JOB_DTL_ID")
                    '' ''.AppendLine(" GROUP BY JOB_DTL_ID) ")
                    '' ''If partCD IsNot "" Then
                    '' ''    .AppendLine("      AND K.PART_CD=:PART_CD ")
                    '' ''End If
                    '' ''.AppendLine("ORDER BY K.INSPEC_ITEM_CD,C.STALL_USE_STATUS DESC ,C.JOB_DTL_ID ")




                    '.AppendLine("  /* SC3180201_002 */ ")
                    '.AppendLine(" SELECT ")
                    '.AppendLine("      TB_T_SERVICEIN.DLR_CD ")
                    '.AppendLine("     ,TB_T_SERVICEIN.BRN_CD ")
                    '.AppendLine("     ,TB_T_SERVICEIN.SVCIN_ID ")
                    '.AppendLine("     ,TB_T_JOB_DTL.JOB_DTL_ID ")
                    '.AppendLine("     ,TB_T_JOB_DTL.INSPECTION_NEED_FLG ")
                    '.AppendLine("     ,TB_T_JOB_DTL.INSPECTION_STATUS ")
                    '.AppendLine("     ,TB_T_STALL_USE.STALL_USE_ID ")
                    '.AppendLine("     ,TB_T_STALL_USE.STALL_USE_STATUS ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.JOB_INSTRUCT_ID ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.JOB_INSTRUCT_SEQ ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.RO_NUM ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.RO_SEQ ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.JOB_CD ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT.JOB_NAME ")
                    ''.AppendLine("     ,' ' SVC_CD ")
                    '.AppendLine("     ,TB_M_INSPECTION_COMB.SVC_CD ")
                    '.AppendLine("     ,TB_M_INSPECTION_COMB.MODEL_CD ")
                    '.AppendLine("     ,TB_M_INSPECTION_COMB.GRADE_CD ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.INSPEC_ITEM_CD ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.INSPEC_ITEM_NAME ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.SUB_INSPEC_ITEM_NAME ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_FIX ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_INSPEC_ITEM_NEED_SWAP ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_TEXT_PERM ")
                    '.AppendLine("     ,TB_M_INSPECTION_COMB.PART_CD ")
                    '.AppendLine("     ,TB_M_PARTNAME.PART_NAME ")
                    '.AppendLine("     ,INSPECT_DATA.APROVAL_STATUS ")
                    '.AppendLine("     ,INSPECT_DATA.ADVICE_CONTENT ")
                    '.AppendLine("     ,INSPECT_DATA.JOB_INSTRUCT_ID ")
                    '.AppendLine("     ,INSPECT_DATA.JOB_INSTRUCT_SEQ ")
                    '.AppendLine("     ,INSPECT_DATA.INSPEC_ITEM_CD ")
                    '.AppendLine("     ,INSPECT_DATA.INSPEC_RSLT_CD ")
                    '.AppendLine("     ,INSPECT_DATA.OPERATION_RSLT_ALREADY_REPLACE ")
                    '.AppendLine("     ,INSPECT_DATA.OPERATION_RSLT_ALREADY_FIX ")
                    '.AppendLine("     ,INSPECT_DATA.OPERATION_RSLT_ALREADY_CLEAN ")
                    '.AppendLine("     ,INSPECT_DATA.OPERATION_RSLT_ALREADY_SWAP ")
                    ''.AppendLine("     ,INSPECT_DATA.RSLT_VALUE_BEFORE ")
                    ''.AppendLine("     ,INSPECT_DATA.RSLT_VALUE_AFTER ")
                    '.AppendLine("     ,INSPECT_DATA.RSLT_VALUE_BEFORE AS RSLT_BEFORE_TEXT ")
                    '.AppendLine("     ,INSPECT_DATA.RSLT_VALUE_AFTER AS RSLT_AFTER_TEXT ")
                    '.AppendLine("     ,INSPECT_DATA.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION ")

                    ''2014/06/27 項目追加　Start
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_OPE_ITEM_ALREADY_REPLACE ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_OPE_ITEM_ALREADY_FIX ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_OPE_ITEM_ALREADY_CLEAN ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL.DISP_OPE_ITEM_ALREADY_SWAP ")
                    ''2014/06/27 項目追加　End

                    '.AppendLine(" FROM ")
                    '.AppendLine("      TB_T_SERVICEIN ")
                    '.AppendLine("     ,TB_M_VEHICLE ")
                    '.AppendLine("     ,TB_T_JOB_DTL ")
                    '.AppendLine("     ,TB_T_STALL_USE ")
                    '.AppendLine("     ,TB_T_JOB_INSTRUCT ")
                    '.AppendLine("     ,TB_M_INSPECTION_COMB ")
                    '.AppendLine("     ,TB_M_MERCHANDISE ")
                    '.AppendLine("     ,TB_M_INSPECTION_DETAIL ")
                    '.AppendLine("     ,TB_M_PARTNAME ")
                    '.AppendLine("     ,TB_M_MAINTE_ATTR ")
                    '.AppendLine("    ,(SELECT  ")
                    '.AppendLine("         TB_T_INSPECTION_HEAD.JOB_DTL_ID ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.DLR_CD ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.BRN_CD ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.RO_NUM ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.APROVAL_STATUS ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.ADVICE_CONTENT ")
                    '.AppendLine("        ,TB_T_INSPECTION_HEAD.ROW_LOCK_VERSION ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.JOB_INSTRUCT_ID ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.JOB_INSTRUCT_SEQ ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.INSPEC_ITEM_CD ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.INSPEC_RSLT_CD ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.OPERATION_RSLT_ALREADY_REPLACE ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.OPERATION_RSLT_ALREADY_FIX ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.OPERATION_RSLT_ALREADY_CLEAN ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.OPERATION_RSLT_ALREADY_SWAP ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.RSLT_VALUE_BEFORE ")
                    '.AppendLine("        ,TB_T_INSPECTION_DETAIL.RSLT_VALUE_AFTER ")
                    '.AppendLine("      FROM  TB_T_INSPECTION_HEAD ")
                    '.AppendLine("           ,TB_T_INSPECTION_DETAIL ")
                    '.AppendLine("      WHERE TB_T_INSPECTION_HEAD.JOB_DTL_ID = TB_T_INSPECTION_DETAIL.JOB_DTL_ID  ")
                    '.AppendLine("        AND TB_T_INSPECTION_HEAD.DLR_CD = :DLR_CD ")
                    '.AppendLine("        AND TB_T_INSPECTION_HEAD.BRN_CD = :BRN_CD ")
                    '.AppendLine("        AND TB_T_INSPECTION_HEAD.RO_NUM = :RO_NUM) INSPECT_DATA ")

                    '.AppendLine(" WHERE ")
                    '.AppendLine("         TB_T_SERVICEIN.DLR_CD=:DLR_CD ")
                    '.AppendLine("     AND TB_T_SERVICEIN.BRN_CD=:BRN_CD ")
                    '.AppendLine("     AND TB_T_SERVICEIN.RO_NUM=:RO_NUM ")
                    '.AppendLine("     AND TB_T_SERVICEIN.VCL_ID=TB_M_VEHICLE.VCL_ID ")
                    '.AppendLine("     AND TB_T_SERVICEIN.SVCIN_ID=TB_T_JOB_DTL.SVCIN_ID ")
                    '.AppendLine("     AND TB_T_JOB_DTL.JOB_DTL_ID=TB_T_STALL_USE.JOB_DTL_ID ")
                    '.AppendLine("     AND TB_T_JOB_DTL.JOB_DTL_ID=TB_T_JOB_INSTRUCT.JOB_DTL_ID ")
                    '.AppendLine("     AND TB_T_JOB_DTL.MERC_ID=TB_M_MERCHANDISE.MERC_ID ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.MODEL_CD=TB_M_VEHICLE.MODEL_CD ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.GRADE_CD=TB_M_VEHICLE.GRADE_NAME ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.SVC_CD=TB_M_MERCHANDISE.UPPER_DISP || TB_M_MERCHANDISE.LOWER_DISP  ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.DLR_CD=TB_T_SERVICEIN.DLR_CD ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.BRN_CD=TB_T_SERVICEIN.BRN_CD ")

                    'If partCD <> "" Then
                    '    .AppendLine("     AND TB_M_INSPECTION_COMB.PART_CD=:PART_CD ")
                    'End If

                    '.AppendLine("     AND TB_M_INSPECTION_COMB.INSPEC_ITEM_CD=  TB_M_INSPECTION_DETAIL.INSPEC_ITEM_CD ")
                    '.AppendLine("     AND TB_M_INSPECTION_COMB.PART_CD=TB_M_PARTNAME.PART_CD ")

                    '.AppendLine("     AND INSPECT_DATA.DLR_CD=:DLR_CD ")
                    '.AppendLine("     AND INSPECT_DATA.BRN_CD=:BRN_CD ")
                    '.AppendLine("     AND INSPECT_DATA.INSPEC_ITEM_CD=TB_M_INSPECTION_COMB.INSPEC_ITEM_CD ")

                    '.AppendLine("     AND TB_M_MAINTE_ATTR.DLR_CD=TB_T_SERVICEIN.DLR_CD ")
                    '.AppendLine("     AND TB_M_MAINTE_ATTR.MAINTE_CD=TB_T_JOB_INSTRUCT.JOB_CD ")
                    '.AppendLine("     AND (TB_M_MAINTE_ATTR.MAINTE_KATASHIKI='X' OR TB_M_MAINTE_ATTR.MAINTE_KATASHIKI= TB_M_VEHICLE.VCL_KATASHIKI) ")
                    '.AppendLine("     AND TB_M_MAINTE_ATTR.MERC_ID=TB_M_MERCHANDISE.MERC_ID ")
                    '.AppendLine("     AND TB_T_STALL_USE.STALL_USE_ID IN (SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE WHERE TB_T_STALL_USE.JOB_DTL_ID=TB_T_JOB_DTL.JOB_DTL_ID GROUP BY JOB_DTL_ID) ")
                    '.AppendLine("     AND EXISTS (SELECT 'X' FROM TB_M_SERVICE WHERE TB_M_SERVICE.DLR_CD=TB_M_MAINTE_ATTR.DLR_CD AND TB_M_SERVICE.SVC_CD=TB_M_MERCHANDISE.SVC_CD) ")
                    '.AppendLine(" ORDER BY TB_M_INSPECTION_DETAIL.INSPEC_ITEM_CD,TB_T_STALL_USE.STALL_USE_STATUS DESC ,TB_T_JOB_DTL.JOB_DTL_ID ")
                    ''2014/06/03 レスポンス対応　End


                    .AppendLine(" SELECT ")
                    .AppendLine(" /* SC3180201_002 */ ")
                    .AppendLine("      TB_T_SERVICEIN.DLR_CD ")
                    .AppendLine("     ,TB_T_SERVICEIN.BRN_CD ")
                    .AppendLine("     ,TB_T_SERVICEIN.SVCIN_ID ")
                    .AppendLine("     ,XXX.JOB_DTL_ID ")
                    .AppendLine("     ,XXX.INSPECTION_NEED_FLG ")
                    .AppendLine("     ,XXX.INSPECTION_STATUS ")
                    .AppendLine("     ,F.STALL_USE_ID ")
                    .AppendLine("     ,F.STALL_USE_STATUS ")
                    .AppendLine("     ,C.JOB_INSTRUCT_ID ")
                    .AppendLine("     ,C.JOB_INSTRUCT_SEQ ")
                    .AppendLine("     ,C.RO_NUM ")
                    .AppendLine("     ,C.RO_SEQ ")
                    .AppendLine("     ,C.JOB_CD ")
                    .AppendLine("     ,C.JOB_NAME ")
                    .AppendLine("     ,M.SVC_CD ")
                    .AppendLine("     ,M.MODEL_CD ")
                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
                    '.AppendLine("     ,M.GRADE_CD ")
                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
                    .AppendLine("     ,YYY.INSPEC_ITEM_CD ")
                    .AppendLine("     ,YYY.INSPEC_ITEM_NAME ")
                    .AppendLine("     ,YYY.SUB_INSPEC_ITEM_NAME ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NO_PROBLEM ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NEED_INSPEC ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NEED_REPLACE ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NEED_FIX ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NEED_CLEAN ")
                    .AppendLine("     ,YYY.DISP_INSPEC_ITEM_NEED_SWAP ")
                    .AppendLine("     ,M.DISP_INSPEC_ITEM_NO_ACTION ")
                    .AppendLine("     ,YYY.DISP_TEXT_PERM ")
                    .AppendLine("     ,M.PART_CD ")
                    .AppendLine("     ,TB_M_FINAL_INSPECTION_PARTNAME.PART_NAME ")
                    .AppendLine("     ,(SELECT APPROVAL_STATUS FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = XXX.JOB_DTL_ID) AS APPROVAL_STATUS ")
                    '.AppendLine("     ,(SELECT RTRIM(ADVICE_CONTENT) FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = XXX.JOB_DTL_ID) AS ADVICE_CONTENT ")
                    .AppendLine("     ,C.JOB_INSTRUCT_ID ")
                    .AppendLine("     ,C.JOB_INSTRUCT_SEQ ")
                    .AppendLine("     ,M.INSPEC_ITEM_CD ")
                    '2014/06/06 結合条件変更　Start
                    '.AppendLine("     ,(SELECT INSPEC_RSLT_CD FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS INSPEC_RSLT_CD ")
                    '.AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_REPLACE FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_REPLACE ")
                    '.AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_FIX FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_FIX ")
                    '.AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_CLEAN FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_CLEAN ")
                    '.AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_SWAP FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_SWAP ")
                    '.AppendLine("     ,(SELECT RSLT_VALUE_BEFORE FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VALUE_BEFORE ")
                    '.AppendLine("     ,(SELECT RSLT_VALUE_AFTER FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VALUE_AFTER ")

                    .AppendLine("     ,(SELECT INSPEC_RSLT_CD FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS INSPEC_RSLT_CD ")
                    .AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_REPLACE FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_REPLACE ")
                    .AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_FIX FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_FIX ")
                    .AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_CLEAN FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_CLEAN ")
                    .AppendLine("     ,(SELECT OPERATION_RSLT_ALREADY_SWAP FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS OPERATION_RSLT_ALREADY_SWAP ")
                    '.AppendLine("     ,(SELECT RSLT_VALUE_BEFORE FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID  AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VALUE_BEFORE ")
                    '.AppendLine("     ,(SELECT RSLT_VALUE_AFTER FROM TB_T_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID  AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VALUE_AFTER ")
                    .AppendLine("     ,(SELECT RSLT_VAL_BEFORE FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VAL_BEFORE ")
                    .AppendLine("     ,(SELECT RSLT_VAL_AFTER FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD=M.INSPEC_ITEM_CD) AS RSLT_VAL_AFTER ")
                    '2014/06/06 結合条件変更　End

                    .AppendLine("     ,(SELECT ROW_LOCK_VERSION FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = XXX.JOB_DTL_ID) AS TRN_ROW_LOCK_VERSION ")
                    .AppendLine("     , I.RO_STATUS ")

                    '2014/06/27 項目追加　Start
                    .AppendLine("     ,YYY.DISP_OPE_ITEM_ALREADY_REPLACE ")
                    .AppendLine("     ,YYY.DISP_OPE_ITEM_ALREADY_FIX ")
                    .AppendLine("     ,YYY.DISP_OPE_ITEM_ALREADY_CLEAN ")
                    .AppendLine("     ,YYY.DISP_OPE_ITEM_ALREADY_SWAP ")
                    '2014/06/27 項目追加　End

                    '2014/08/14 ソートキー、検査用アイテム表示順と[代表整備]フラグ追加　Start
                    .AppendLine("     ,M.INSPEC_ITEM_DISP_SEQ ")
                    .AppendLine("     ,CASE WHEN XXX.MAINTE_CD = C.JOB_CD THEN '1' ELSE '0' END AS DAIHYO_SEIBI ")
                    '2014/08/14 ソートキー、検査用アイテム表示順と[代表整備]フラグ追加　End

                    .AppendLine(" FROM ")
                    .AppendLine("      TB_T_SERVICEIN ")
                    .AppendLine("     ,TB_M_VEHICLE ")
                    .AppendLine("     ,TB_T_JOB_DTL XXX ")
                    .AppendLine("     ,TB_T_STALL_USE F ")
                    .AppendLine("     ,TB_T_JOB_INSTRUCT C ")
                    .AppendLine("     ,TB_M_INSPECTION_COMB M ")
                    .AppendLine("     ,TB_M_MERCHANDISE ")
                    .AppendLine("     ,TB_M_FINAL_INSPECTION_DETAIL YYY ")
                    .AppendLine("     ,TB_M_FINAL_INSPECTION_PARTNAME ")
                    .AppendLine("     ,TB_M_MAINTE_ATTR ")
                    .AppendLine("     ,TB_T_RO_INFO I ")
                    .AppendLine(" WHERE ")
                    .AppendLine("         TB_T_SERVICEIN.DLR_CD=:DLR_CD ")
                    .AppendLine("     AND TB_T_SERVICEIN.BRN_CD=:BRN_CD ")
                    '.AppendLine("     AND TB_T_SERVICEIN.RO_NUM=:RO_NUM ")
                    .AppendLine("     AND I.RO_NUM=:RO_NUM ")
                    .AppendLine("     AND TB_M_VEHICLE.VCL_ID=TB_T_SERVICEIN.VCL_ID ")
                    .AppendLine("     AND XXX.SVCIN_ID=TB_T_SERVICEIN.SVCIN_ID ")
                    .AppendLine("     AND C.JOB_DTL_ID=XXX.JOB_DTL_ID ")
                    '.AppendLine("     AND XXX.MERC_ID=TB_M_MERCHANDISE.MERC_ID ")      '2014/07/31 [作業内容.表示商品ID]は代表整備のみしかない為、定期点検/一般整備の判定より除外
                    '整備属性マスタに販売店登録が無い場合は全販売店で検索
                    If specifyDlrCdFlgs("MAINTE_CD_EXIST") = True Then
                        .AppendLine("     AND TB_M_MAINTE_ATTR.DLR_CD=TB_T_SERVICEIN.DLR_CD ")
                    Else
                        .AppendLine("     AND TB_M_MAINTE_ATTR.DLR_CD='" & AllDealer & "'")
                    End If
                    .AppendLine("     AND TB_M_MAINTE_ATTR.MAINTE_CD=C.JOB_CD ")
                    .AppendLine("     AND (TB_M_MAINTE_ATTR.MAINTE_KATASHIKI='X' OR TB_M_MAINTE_ATTR.MAINTE_KATASHIKI= TB_M_VEHICLE.VCL_KATASHIKI) ")
                    .AppendLine("     AND TB_M_MERCHANDISE.MERC_ID=TB_M_MAINTE_ATTR.MERC_ID ")
                    '2019/12/02 NCN 吉川　TKM要件：型式対応 Start 
                    '型式使用時
                    If specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
                        '点検組み合わせマスタと車両マスタの型式を条件にする 
                        .AppendLine("     AND M.VCL_KATASHIKI = TB_M_VEHICLE.VCL_KATASHIKI ")

                    Else 'モデルコード使用時または2回目
                        '型式を半角スペースを条件にする
                        .AppendLine("     AND M.VCL_KATASHIKI = ' ' ")
                    End If
                    ''2019/12/02 NCN 吉川　TKM要件：型式対応 End 

                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
                    '.AppendLine("     AND M.GRADE_CD=TB_M_VEHICLE.GRADE_NAME ")
                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End

                    .AppendLine("     AND M.MODEL_CD=TB_M_VEHICLE.MODEL_CD ")
                    .AppendLine("     AND M.SVC_CD=TB_M_MERCHANDISE.SVC_CD ")
                    '点検組み合わせマスタに指定販売店が登録されている場合は指定販売店で検索
                    If specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = True Then
                        .AppendLine("     AND M.DLR_CD=TB_T_SERVICEIN.DLR_CD ")
                        .AppendLine("     AND M.BRN_CD=TB_T_SERVICEIN.BRN_CD ")
                    Else
                        '未登録の場合は全販売店、全店舗で点検組み合わせマスタから点検項目を検索する
                        .AppendLine("     AND M.DLR_CD='" & AllDealer & "'")
                        .AppendLine("     AND M.BRN_CD='" & AllBranch & "'")
                    End If
                    '2015/04/14 新販売店追加対応 end

                    'If partCD <> "" Then
                    '    .AppendLine("     AND M.PART_CD=:PART_CD ")
                    'End If

                    .AppendLine("     AND TB_M_FINAL_INSPECTION_PARTNAME.PART_CD=M.PART_CD ")
                    .AppendLine("     AND YYY.INSPEC_ITEM_CD=M.INSPEC_ITEM_CD ")
                    .AppendLine("     AND I.SVCIN_ID = XXX.SVCIN_ID ")
                    .AppendLine("     AND I.RO_NUM = C.RO_NUM ")
                    .AppendLine("     AND I.RO_SEQ = C.RO_SEQ ")
                    .AppendLine("     AND F.JOB_DTL_ID = XXX.JOB_DTL_ID ")
                    .AppendLine("     AND F.STALL_USE_ID = (SELECT MAX(STALL_USE_ID) FROM TB_T_STALL_USE WHERE JOB_DTL_ID = XXX.JOB_DTL_ID) ")
                    'マスタのデータ登録前提条件の変更によりマスタ整合性チェック条件を削除
                    '.AppendLine("     AND EXISTS (SELECT 'X' FROM TB_M_SERVICE WHERE TB_M_SERVICE.DLR_CD=TB_M_MAINTE_ATTR.DLR_CD AND TB_M_SERVICE.SVC_CD=TB_M_MERCHANDISE.SVC_CD) ")
                    .AppendLine("     AND C.STARTWORK_INSTRUCT_FLG = '1' ")     '着工指示フラグ：指示済
                    '2014/08/14 ソート条件変更　Start
                    '.AppendLine(" ORDER BY YYY.INSPEC_ITEM_CD,F.STALL_USE_STATUS DESC ,XXX.JOB_DTL_ID ")
                    .AppendLine(" ORDER BY YYY.INSPEC_ITEM_CD, XXX.JOB_DTL_ID, M.INSPEC_ITEM_DISP_SEQ, DAIHYO_SEIBI DESC ")
                    '2014/08/14 ソート条件変更　End

                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号

                'If partCD IsNot "" Then
                '    query.AddParameterWithTypeValue("PART_CD", OracleDbType.NVarchar2, partCD)                '部位コード
                'End If

                ''SQL実行
                'Return query.GetData()

                'SQL実行
                '2014/06/10 表示の仕様変更　Start
                'Return query.GetData()
                Dim InspectData As New SC3180201InspectCodeDataTable
                InspectData = query.GetData()
                ' 2015/02/18 BTS167 追加作業のGSジョブが、完成検査一覧に表示されない。Start
                '一般整備同様の横展開修正 追加Jobの場合の対応
                '                RemoveInspectData(InspectData)
                ' 2015/02/18 BTS167 追加作業のGSジョブが、完成検査一覧に表示されない。End
                Return InspectData
                '2014/06/10 表示の仕様変更　End
            End Using

        End Function

        ''' <summary>
        ''' GetDBRoState(ROステータス取得)
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <returns>ROステータス情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetDBRoState(ByVal jobDtlId As Decimal) As SC3180201RoStateDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201RoStateDataTable)("SC3180201_003")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    'TMT2販社 BTS135 完成検査承認時にR/Oステータスが更新されない START
                    '.AppendLine("SELECT ")
                    '.AppendLine("  /* SC3180201_003 */ ")
                    '.AppendLine("       B.RO_STATUS ")
                    '.AppendLine("FROM TB_T_JOB_INSTRUCT A, ")
                    '.AppendLine("     TB_T_RO_INFO B ")
                    '.AppendLine("WHERE A.JOB_DTL_ID=:JOB_DTL_ID ")
                    '.AppendLine("  AND A.RO_NUM=B.RO_NUM ")
                    '.AppendLine("  AND A.RO_SEQ=B.RO_SEQ ")
                    .AppendLine("SELECT /* SC3180201_003 */  ")
                    .AppendLine("	RI.RO_STATUS ")
                    .AppendLine("FROM  ")
                    .AppendLine("	TB_T_SERVICEIN SI,  ")
                    .AppendLine("    TB_T_RO_INFO RI, ")
                    .AppendLine("	TB_T_JOB_DTL JD ")
                    .AppendLine("WHERE JD.JOB_DTL_ID=:JOB_DTL_ID  ")
                    .AppendLine("	AND	JD.SVCIN_ID=RI.SVCIN_ID ")
                    .AppendLine("	AND SI.SVCIN_ID=RI.SVCIN_ID ")
                    .AppendLine("	AND RI.DLR_CD=SI.DLR_CD ")
                    .AppendLine("	AND RI.BRN_CD=SI.BRN_CD ")
                    'TMT2販社 RO_CANCEL条件を追加 START
                    .AppendLine("	AND RI.RO_STATUS <> :RO_CANCEL ")
                    'TMT2販社 RO_CANCEL条件を追加 END
                    'TMT2販社 BTS135 完成検査承認時にR/Oステータスが更新されない END
                    '親ROのステータスを取得する為、RO_SEQによるソートを行う(TR-SVT-TMT-20160216-001)
                    .AppendLine("	ORDER BY RI.RO_SEQ ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                      '作業内容ID
                'TMT2販社 RO_CANCEL条件を追加 START
                query.AddParameterWithTypeValue("RO_CANCEL", OracleDbType.NVarchar2, RO_Status.RO_Cancel)          'ROキャンセル
                'TMT2販社 RO_CANCEL条件を追加 END

                'SQL実行
                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' SetDBCmpChkResultUpt(完成検査結果データ更新)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="approvalStatus">承認ステータス</param>
        ''' <param name="accountName">アカウント</param>
        ''' <param name="updateTime">更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <returns>更新行0件：False / 1件以上：True</returns>
        ''' <remarks></remarks>
        Public Function SetDBCmpChkResultUpt(ByVal dlrCD As String, _
                                             ByVal brnCD As String, _
                                             ByVal jobDtlId As Decimal, _
                                             ByVal roNum As String, _
                                             ByVal approvalStatus As Integer, _
                                             ByVal accountName As String, _
                                             ByVal updateTime As Date, _
                                             ByVal lockVersion As Long, _
                                             ByVal updateFlg As Integer) As Boolean

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_004")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_004 */ ")
                    .AppendLine("       TB_T_FINAL_INSPECTION_HEAD ")
                    .AppendLine("SET RO_NUM = :RO_NUM, ")
                    .AppendLine("    APPROVAL_STATUS = :APPROVAL_STATUS, ")
                    If updateFlg = UpdateTypeApprove Then
                        .AppendLine("    INSPECTION_APPROVAL_STF_CD = :ROW_UPDATE_ACCOUNT, ")
                        .AppendLine("    INSPECTION_APPROVAL_DATETIME = :ROW_UPDATE_DATETIME, ")
                    End If
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION ='SC3180201' ")
                    .AppendLine("WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("  AND DLR_CD = :DLR_CD ")
                    .AppendLine("  AND BRN_CD = :BRN_CD ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                 '作業内容ID
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号
                query.AddParameterWithTypeValue("APPROVAL_STATUS", OracleDbType.Decimal, approvalStatus)      '承認ステータス
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)    '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)         '行更新日時
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, lockVersion)        '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using

        End Function

        ''' <summary>
        ''' SetDBCmpChkResultDetailUpt (完成検査結果詳細データ更新)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="jobInstructId">作業指示ID</param>
        ''' <param name="jobInstructSeq">作業指示枝番</param>
        ''' <param name="inspecItemCD">点検項目コード</param>
        ''' <param name="inspecRsltCD">点検結果</param>
        ''' <param name="alreadyReplace">alreadyReplace選択状態</param>
        ''' <param name="alreadyFixed">alreadyFixed選択状態</param>
        ''' <param name="alreadyCelaning">alreadyCelaning選択状態</param>
        ''' <param name="alreadySwapped">alreadySwapped選択状態</param>
        ''' <param name="beforeText">Before入力値</param>
        ''' <param name="afterText">After入力値</param>
        ''' <param name="accountName">アカウント</param>
        ''' <param name="updateTime">更新日時</param>
        ''' <returns>更新行0件：False / 1件以上：True</returns>
        ''' <remarks></remarks>
        Public Function SetDBCmpChkResultDetailUpt(ByVal dlrCD As String, _
                                                   ByVal brnCD As String, _
                                                   ByVal jobDtlId As Decimal, _
                                                   ByVal jobInstructId As String, _
                                                   ByVal jobInstructSeq As Long, _
                                                   ByVal inspecItemCD As String, _
                                                   ByVal inspecRsltCD As Long, _
                                                   ByVal alreadyReplace As Long, _
                                                   ByVal alreadyFixed As Long, _
                                                   ByVal alreadyCelaning As Long, _
                                                   ByVal alreadySwapped As Long, _
                                                   ByVal beforeText As Decimal, _
                                                   ByVal afterText As Decimal, _
                                                   ByVal accountName As String, _
                                                   ByVal updateTime As Date) As Boolean

            Dim queryResult As Boolean = False
            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_005")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_005 */ ")
                    .AppendLine("       TB_T_FINAL_INSPECTION_DETAIL ")
                    .AppendLine("SET INSPEC_RSLT_CD = :INSPEC_RSLT_CD, ")
                    .AppendLine("    OPERATION_RSLT_ALREADY_REPLACE = :ALREADY_REPLACE, ")
                    .AppendLine("    OPERATION_RSLT_ALREADY_FIX = :ALREADY_FIXED, ")
                    .AppendLine("    OPERATION_RSLT_ALREADY_CLEAN = :ALREADY_CELANING, ")
                    .AppendLine("    OPERATION_RSLT_ALREADY_SWAP = :ALREADY_SWAPPED, ")
                    '2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」　START　↓↓↓
                    .AppendLine("    RSLT_VAL_BEFORE= :BEFORE_TEXT, ")
                    '.AppendLine("    RSLT_BEFORE_TEXT= :BEFORE_TEXT, ")
                    '2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」　END　　↑↑↑
                    '2014/05/26 カラム名変更「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　START　↓↓↓
                    .AppendLine("    RSLT_VAL_AFTER= :AFTER_TEXT, ")
                    '.AppendLine("    RSLT_AFTER_TEXT= :AFTER_TEXT, ")
                    '2014/05/26 カラム名変更「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　END　　↑↑↑
                    .AppendLine("    ROW_UPDATE_DATETIME= :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT= :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION = 'SC3180201' ")
                    .AppendLine("WHERE JOB_DTL_ID= :JOB_DTL_ID ")
                    .AppendLine("  AND JOB_INSTRUCT_ID= :JOB_INSTRUCT_ID")
                    .AppendLine("  AND JOB_INSTRUCT_SEQ= :JOB_INSTRUCT_SEQ ")
                    .AppendLine("  AND INSPEC_ITEM_CD= :INSPEC_ITEM_CD ")
                    '2014/05/26 「DLR_CD」と「BRN_CD」を削除　START　↓↓↓
                    '.AppendLine("  AND DLR_CD= :DLR_CD ")
                    '.AppendLine("  AND BRN_CD= :BRN_CD ")
                    '2014/05/26 「DLR_CD」と「BRN_CD」を削除　END　　↑↑↑
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                '2014/05/26 「DLR_CD」と「BRN_CD」を削除　START　↓↓↓
                'query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                'query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                '2014/05/26 「DLR_CD」と「BRN_CD」を削除　END　　↑↑↑
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                '作業内容ID
                query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, jobInstructId)    '作業指示ID
                query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Decimal, jobInstructSeq)    '作業指示枝番
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCD)      '点検項目コード
                query.AddParameterWithTypeValue("INSPEC_RSLT_CD", OracleDbType.Decimal, inspecRsltCD)        '点検結果
                query.AddParameterWithTypeValue("ALREADY_REPLACE", OracleDbType.Decimal, alreadyReplace)      'ALREADY_REPLACE  選択状態
                query.AddParameterWithTypeValue("ALREADY_FIXED", OracleDbType.Decimal, alreadyFixed)          'ALREADY_FIXED    選択状態
                query.AddParameterWithTypeValue("ALREADY_CELANING", OracleDbType.Decimal, alreadyCelaning)    'ALREADY_CELANING 選択状態
                query.AddParameterWithTypeValue("ALREADY_SWAPPED", OracleDbType.Decimal, alreadySwapped)      'ALREADY_SWAPPED  選択状態
                query.AddParameterWithTypeValue("BEFORE_TEXT", OracleDbType.Decimal, beforeText)              'BEFORE_TEXT      入力値
                query.AddParameterWithTypeValue("AFTER_TEXT", OracleDbType.Decimal, afterText)                'AFTER_TEXT       入力値
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)    '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)              '行更新日時

                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using

        End Function

        '2015/04/14 新販売店追加対応 start

        ''' <summary>
        ''' GetDBMainteCodeList (整備明細項目取得)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
        ''' <returns>整備情報リストデータセット</returns>
        ''' <remarks></remarks>
        Public Function GetDBMainteCodeList(ByVal dlrCD As String, _
                                            ByVal brnCD As String, _
                                            ByVal roNum As String, _
                                            ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean)) As SC3180201MainteCodeListDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201MainteCodeListDataTable)("SC3180201_006")

                Dim sql As New StringBuilder
                '2019/12/02 NCN吉川 TKM要件：型式対応 Start

                With sql
                    ''.AppendLine("  /* SC3180201_006 */ ")
                    ''.AppendLine(" SELECT A.DLR_CD,  ")
                    ''.AppendLine("        A.BRN_CD,  ")
                    ''.AppendLine("        A.SVCIN_ID,  ")
                    ''.AppendLine("        B.JOB_DTL_ID,  ")
                    ''.AppendLine("        B.INSPECTION_NEED_FLG,  ")
                    ''.AppendLine("        B.INSPECTION_STATUS,  ")
                    ''.AppendLine("        C.JOB_INSTRUCT_ID,  ")
                    ''.AppendLine("        C.JOB_INSTRUCT_SEQ,  ")
                    ''.AppendLine("        C.JOB_INSTRUCT_SEQ,  ")
                    ''.AppendLine("        C.JOB_INSTRUCT_SEQ,  ")
                    ''.AppendLine("        C.RO_NUM,  ")
                    ''.AppendLine("        C.RO_SEQ,  ")
                    ''.AppendLine("        C.JOB_CD,  ")
                    ''.AppendLine("        C.JOB_NAME,  ")
                    ''.AppendLine("        C.JOB_STF_GROUP_NAME,  ")
                    ''.AppendLine("        C.OPERATION_TYPE_NAME,  ")
                    ''.AppendLine("        D.ACCOUNT,  ")
                    ''.AppendLine("        D.USERNAME,  ")
                    ''.AppendLine("        E.INSPEC_RSLT_CD,  ")
                    ''.AppendLine("        E.ADVICE_CONTENT,  ")
                    ''.AppendLine("        E.APROVAL_STATUS,  ")
                    ''.AppendLine("        F.STALL_USE_ID,  ")
                    ''.AppendLine("        F.STALL_USE_STATUS,  ")
                    ''.AppendLine("        E.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION  ")
                    ''.AppendLine(" FROM TB_T_SERVICEIN A,  ")
                    ''.AppendLine("      TBL_USERS D,  ")
                    ''.AppendLine("      TB_T_JOB_DTL B,  ")
                    ''.AppendLine("      TB_T_STALL_USE F , ")
                    ''.AppendLine("      TB_T_JOB_INSTRUCT C, ")
                    ''.AppendLine("      (SELECT J.JOB_DTL_ID  ")
                    ''.AppendLine("                       ,J.DLR_CD  ")
                    ''.AppendLine("                       ,J.BRN_CD  ")
                    ''.AppendLine("                       ,J.RO_NUM  ")
                    ''.AppendLine("                       ,J.APROVAL_STATUS  ")
                    ''.AppendLine("                       ,J.ADVICE_CONTENT  ")
                    ''.AppendLine("                       ,K.JOB_INSTRUCT_ID  ")
                    ''.AppendLine("                       ,K.JOB_INSTRUCT_SEQ  ")
                    ''.AppendLine("                       ,K.INSPEC_ITEM_CD  ")
                    ''.AppendLine("                       ,K.INSPEC_RSLT_CD  ")
                    ''.AppendLine("                       ,K.OPERATION_RSLT_ALREADY_REPLACE  ")
                    ''.AppendLine("                       ,K.OPERATION_RSLT_ALREADY_FIX  ")
                    ''.AppendLine("                       ,K.OPERATION_RSLT_ALREADY_CLEAN  ")
                    ''.AppendLine("                       ,K.OPERATION_RSLT_ALREADY_SWAP  ")
                    ' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」　START　↓↓↓
                    ''.AppendLine("                       ,K.RSLT_VALUE_BEFORE AS RSLT_BEFORE_TEXT  ")
                    ' ''.AppendLine("                       ,K.RSLT_BEFORE_TEXT  ")
                    ' ''2014/05/26 カラム名変更「RSLT_BEFORE_TEXT→RSLT_VALUE_BEFORE」　END　　↑↑↑
                    ' ''2014/05/26 カラム名変更「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　START　↓↓↓
                    ''.AppendLine("                       ,K.RSLT_VALUE_AFTER AS RSLT_AFTER_TEXT  ")
                    ' ''.AppendLine("                       ,K.RSLT_AFTER_TEXT  ")
                    ' ''2014/05/26 カラム名変更「RSLT_AFTER_TEXT→RSLT_VALUE_AFTER」　END　　↑↑↑
                    ''.AppendLine("                       ,J.ROW_LOCK_VERSION  ")
                    ''.AppendLine("                 FROM TB_T_INSPECTION_HEAD J  ")
                    ''.AppendLine("                     ,TB_T_INSPECTION_DETAIL K ")
                    ''.AppendLine("                 WHERE ")
                    ''.AppendLine("                       J.JOB_DTL_ID=K.JOB_DTL_ID(+)  ")
                    ' ''2014/05/26 「DLR_CD」と「BRN_CD」を削除　START　↓↓↓
                    ' ''.AppendLine("                   AND J.DLR_CD=K.DLR_CD(+)  ")
                    ' ''.AppendLine("                   AND J.BRN_CD=K.BRN_CD(+)  ")
                    ' ''2014/05/26 「DLR_CD」と「BRN_CD」を削除　END　　↑↑↑
                    ''.AppendLine("      ) E  ")
                    ''.AppendLine(" WHERE   A.DLR_CD=:DLR_CD  ")
                    ''.AppendLine("     AND A.BRN_CD=:BRN_CD  ")
                    ''.AppendLine("     AND A.RO_NUM=:RO_NUM  ")
                    ''.AppendLine("     AND A.SVCIN_ID=B.SVCIN_ID  ")
                    ''.AppendLine("     AND B.JOB_DTL_ID=C.JOB_DTL_ID  ")
                    ''.AppendLine("     AND C.JOB_DTL_ID=F.JOB_DTL_ID  ")
                    ''.AppendLine("     AND A.PIC_SA_STF_CD=D.ACCOUNT  ")
                    ''.AppendLine("     AND C.JOB_INSTRUCT_ID = E.JOB_INSTRUCT_ID(+)  ")
                    ''.AppendLine("     AND C.JOB_INSTRUCT_SEQ = E.JOB_INSTRUCT_SEQ(+)  ")
                    ''.AppendLine("     AND C.JOB_DTL_ID = E.JOB_DTL_ID(+)  ")
                    ''.AppendLine("     AND F.STALL_USE_ID IN (SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE GROUP BY JOB_DTL_ID)  ")

                    ' ''2015/05/28 Edit
                    ' ''.AppendLine("     AND NOT EXISTS(SELECT * FROM TB_M_OPERATION_CHANGE WHERE MAINTE_CD=C.JOB_CD)  ")
                    ''.AppendLine("      AND NOT EXISTS(SELECT * FROM TB_M_MAINTE_ATTR WHERE MAINTE_CD=C.JOB_CD)  ")




                    '.AppendLine(" SELECT /* SC3180201_003 */ ")
                    '.AppendLine("        A.DLR_CD, ")
                    '.AppendLine("        A.BRN_CD, ")
                    '.AppendLine("        A.SVCIN_ID, ")
                    '.AppendLine("        B.JOB_DTL_ID, ")
                    '.AppendLine("        B.INSPECTION_NEED_FLG, ")
                    '.AppendLine("        B.INSPECTION_STATUS, ")
                    '.AppendLine("        C.JOB_INSTRUCT_ID, ")
                    '.AppendLine("        C.JOB_INSTRUCT_SEQ, ")
                    '.AppendLine("        C.RO_NUM, ")
                    '.AppendLine("        C.RO_SEQ, ")
                    '.AppendLine("        C.JOB_CD, ")
                    '.AppendLine("        C.JOB_NAME, ")
                    '.AppendLine("        C.JOB_STF_GROUP_NAME, ")
                    '.AppendLine("        C.OPERATION_TYPE_NAME, ")
                    '.AppendLine("        D.ACCOUNT, ")
                    '.AppendLine("        D.USERNAME, ")
                    '.AppendLine("        E.INSPEC_RSLT_CD, ")
                    '.AppendLine("        E.ADVICE_CONTENT, ")
                    '.AppendLine("        E.APROVAL_STATUS, ")
                    '.AppendLine("        F.STALL_USE_ID, ")
                    '.AppendLine("        F.STALL_USE_STATUS, ")
                    '.AppendLine("        E.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION  ")
                    '.AppendLine(" FROM TB_T_SERVICEIN A, ")
                    '.AppendLine("      TBL_USERS D, ")
                    '.AppendLine("      TB_T_JOB_DTL B, ")
                    '.AppendLine("      TB_T_STALL_USE F , ")
                    '.AppendLine("      TB_T_JOB_INSTRUCT C, ")
                    '.AppendLine("      TB_M_MAINTE_ATTR G, ")
                    '.AppendLine("      TB_M_MERCHANDISE H, ")
                    '.AppendLine("      TB_M_VEHICLE I, ")
                    '.AppendLine("      (SELECT J.JOB_DTL_ID  ")
                    '.AppendLine("             ,J.DLR_CD  ")
                    '.AppendLine("             ,J.BRN_CD  ")
                    '.AppendLine("             ,J.RO_NUM  ")
                    '.AppendLine("             ,J.APROVAL_STATUS  ")
                    '.AppendLine("             ,J.ADVICE_CONTENT  ")
                    '.AppendLine("             ,K.JOB_INSTRUCT_ID  ")
                    '.AppendLine("             ,K.JOB_INSTRUCT_SEQ  ")
                    '.AppendLine("             ,K.INSPEC_ITEM_CD  ")
                    '.AppendLine("             ,K.INSPEC_RSLT_CD  ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_REPLACE  ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_FIX  ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_CLEAN  ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_SWAP  ")
                    '.AppendLine("             ,J.ROW_LOCK_VERSION  ")
                    '.AppendLine("      FROM TB_T_INSPECTION_HEAD J  ")
                    '.AppendLine("          ,TB_T_INSPECTION_DETAIL K ")
                    '.AppendLine("      WHERE ")
                    '.AppendLine("         J.JOB_DTL_ID=K.JOB_DTL_ID(+)  ")
                    '.AppendLine("      ) E  ")
                    '.AppendLine(" WHERE   A.DLR_CD=:DLR_CD  ")
                    '.AppendLine("     AND A.BRN_CD=:BRN_CD  ")
                    '.AppendLine("     AND A.RO_NUM=:RO_NUM  ")
                    '.AppendLine("     AND A.SVCIN_ID=B.SVCIN_ID ")
                    '.AppendLine("     AND A.VCL_ID=I.VCL_ID ")
                    '.AppendLine("     AND G.DLR_CD=A.DLR_CD  ")
                    '.AppendLine("     AND G.MAINTE_CD=C.JOB_CD ")
                    '.AppendLine("     AND (G.MAINTE_KATASHIKI='X' OR I.VCL_KATASHIKI=G.MAINTE_KATASHIKI) ")
                    '.AppendLine("     AND G.MERC_ID=H.MERC_ID ")
                    '.AppendLine("     AND B.JOB_DTL_ID=C.JOB_DTL_ID  ")
                    '.AppendLine("     AND C.JOB_DTL_ID=F.JOB_DTL_ID  ")
                    '.AppendLine("     AND A.PIC_SA_STF_CD=D.ACCOUNT  ")
                    '.AppendLine("     AND C.JOB_INSTRUCT_ID = E.JOB_INSTRUCT_ID(+)  ")
                    '.AppendLine("     AND C.JOB_INSTRUCT_SEQ = E.JOB_INSTRUCT_SEQ(+)  ")
                    '.AppendLine("     AND C.JOB_DTL_ID = E.JOB_DTL_ID(+)  ")
                    '.AppendLine("     AND F.STALL_USE_ID IN (SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE GROUP BY JOB_DTL_ID)  ")
                    '.AppendLine("     AND NOT EXISTS (SELECT 'X' FROM TB_M_SERVICE WHERE TB_M_SERVICE.DLR_CD=G.DLR_CD AND TB_M_SERVICE.SVC_CD=H.SVC_CD) ")

                    '.AppendLine(" SELECT /* SC3180201_006 */ ")
                    '.AppendLine(" 	  A.DLR_CD ")
                    '.AppendLine(" 	, A.BRN_CD ")
                    '.AppendLine(" 	, A.SVCIN_ID ")
                    '.AppendLine(" 	, B.JOB_DTL_ID ")
                    '.AppendLine(" 	, B.INSPECTION_NEED_FLG ")
                    '.AppendLine(" 	, B.INSPECTION_STATUS ")
                    '.AppendLine(" 	, C.JOB_INSTRUCT_ID ")
                    '.AppendLine(" 	, C.JOB_INSTRUCT_SEQ ")
                    '.AppendLine(" 	, C.RO_NUM ")
                    '.AppendLine(" 	, C.RO_SEQ ")
                    '.AppendLine(" 	, C.JOB_CD ")
                    '.AppendLine(" 	, C.JOB_NAME ")
                    '.AppendLine(" 	, C.JOB_STF_GROUP_NAME ")
                    '.AppendLine(" 	, C.OPERATION_TYPE_NAME ")
                    '.AppendLine(" 	, E.INSPEC_RSLT_CD  ")
                    '.AppendLine(" 	, E.ADVICE_CONTENT  ")
                    '.AppendLine(" 	, E.APROVAL_STATUS  ")
                    '.AppendLine(" 	, F.STALL_USE_ID ")
                    '.AppendLine(" 	, F.STALL_USE_STATUS ")
                    '.AppendLine(" 	, E.ROW_LOCK_VERSION AS TRN_ROW_LOCK_VERSION   ")
                    '.AppendLine(" 	, I.RO_STATUS ")
                    '.AppendLine(" FROM  ")
                    '.AppendLine(" 	TB_T_RO_INFO I  ")
                    '.AppendLine(" 	, TB_T_JOB_DTL B ")
                    '.AppendLine(" 	, TB_T_JOB_INSTRUCT C  ")
                    '.AppendLine(" 	, TB_T_SERVICEIN A  ")
                    '.AppendLine(" 	, TB_T_STALL_USE F ")
                    '.AppendLine("     ,(SELECT J.JOB_DTL_ID   ")
                    '.AppendLine("             ,J.DLR_CD   ")
                    '.AppendLine("             ,J.BRN_CD   ")
                    '.AppendLine("             ,J.RO_NUM   ")
                    '.AppendLine("             ,J.APROVAL_STATUS   ")
                    '.AppendLine("             ,J.ADVICE_CONTENT   ")
                    '.AppendLine("             ,K.JOB_INSTRUCT_ID   ")
                    '.AppendLine("             ,K.JOB_INSTRUCT_SEQ   ")
                    '.AppendLine("             ,K.INSPEC_ITEM_CD   ")
                    '.AppendLine("             ,K.INSPEC_RSLT_CD   ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_REPLACE   ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_FIX   ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_CLEAN   ")
                    '.AppendLine("             ,K.OPERATION_RSLT_ALREADY_SWAP   ")
                    '.AppendLine("             ,J.ROW_LOCK_VERSION   ")
                    '.AppendLine("      FROM TB_T_INSPECTION_HEAD J   ")
                    '.AppendLine("          ,TB_T_INSPECTION_DETAIL K  ")
                    '.AppendLine("      WHERE  ")
                    '.AppendLine("         J.JOB_DTL_ID=K.JOB_DTL_ID(+) ")
                    '.AppendLine("       ) E   ")
                    '.AppendLine(" WHERE ")
                    '.AppendLine("     	A.DLR_CD = :DLR_CD   ")
                    '.AppendLine(" 	AND A.BRN_CD = :BRN_CD  ")
                    '.AppendLine(" 	AND A.RO_NUM = :RO_NUM ")
                    '.AppendLine(" 	AND B.SVCIN_ID = A.SVCIN_ID ")
                    '.AppendLine(" 	AND C.JOB_DTL_ID = B.JOB_DTL_ID ")
                    '.AppendLine(" 	AND NOT EXISTS (SELECT 1 FROM TB_M_MERCHANDISE V ,TB_M_SERVICE W WHERE V.MERC_ID = B.MERC_ID AND V.SVC_CD = W.SVC_CD) ")
                    '.AppendLine(" 	AND I.SVCIN_ID = B.SVCIN_ID ")
                    '.AppendLine(" 	AND I.RO_NUM = C.RO_NUM ")
                    '.AppendLine(" 	AND I.RO_SEQ = C.RO_SEQ ")
                    '.AppendLine(" 	AND I.DLR_CD = A.DLR_CD ")
                    '.AppendLine(" 	AND I.BRN_CD = A.BRN_CD ")
                    '.AppendLine(" 	AND E.JOB_INSTRUCT_ID(+) = C.JOB_INSTRUCT_ID ")
                    '.AppendLine(" 	AND E.JOB_INSTRUCT_SEQ(+) = C.JOB_INSTRUCT_SEQ ")
                    '.AppendLine(" 	AND E.JOB_DTL_ID(+) = C.JOB_DTL_ID ")
                    '.AppendLine(" 	AND F.JOB_DTL_ID = B.JOB_DTL_ID ")
                    '.AppendLine(" 	AND F.STALL_USE_ID = (SELECT MAX(STALL_USE_ID) FROM TB_T_STALL_USE WHERE JOB_DTL_ID = B.JOB_DTL_ID) ")

                    .AppendLine(" SELECT /* SC3180201_006 */ ")
                    .AppendLine(" 	  A.DLR_CD ")
                    .AppendLine(" 	, A.BRN_CD ")
                    .AppendLine(" 	, A.SVCIN_ID ")
                    .AppendLine(" 	, B.JOB_DTL_ID ")
                    .AppendLine(" 	, B.INSPECTION_NEED_FLG ")
                    .AppendLine(" 	, B.INSPECTION_STATUS ")
                    .AppendLine(" 	, C.JOB_INSTRUCT_ID ")
                    .AppendLine(" 	, C.JOB_INSTRUCT_SEQ ")
                    .AppendLine(" 	, C.RO_NUM ")
                    .AppendLine(" 	, C.RO_SEQ ")
                    .AppendLine(" 	, C.JOB_CD ")
                    .AppendLine(" 	, C.JOB_NAME ")
                    .AppendLine(" 	, C.JOB_STF_GROUP_NAME ")
                    .AppendLine(" 	, C.OPERATION_TYPE_NAME ")
                    .AppendLine("   ,(SELECT INSPEC_RSLT_CD FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND ROWNUM=1) AS INSPEC_RSLT_CD ")
                    '.AppendLine("   ,(SELECT RTRIM(ADVICE_CONTENT) FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = B.JOB_DTL_ID) AS ADVICE_CONTENT ")
                    .AppendLine("   ,(SELECT APPROVAL_STATUS FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = B.JOB_DTL_ID) AS APPROVAL_STATUS ")
                    .AppendLine(" 	, F.STALL_USE_ID ")
                    .AppendLine(" 	, F.STALL_USE_STATUS ")
                    .AppendLine("   ,(SELECT ROW_LOCK_VERSION FROM TB_T_FINAL_INSPECTION_HEAD WHERE JOB_DTL_ID = B.JOB_DTL_ID) AS TRN_ROW_LOCK_VERSION ")
                    .AppendLine(" 	, I.RO_STATUS ")
                    .AppendLine(" FROM  ")
                    .AppendLine(" 	TB_T_RO_INFO I  ")
                    .AppendLine(" 	, TB_T_JOB_DTL B ")
                    .AppendLine(" 	, TB_T_JOB_INSTRUCT C  ")
                    .AppendLine(" 	, TB_T_SERVICEIN A  ")
                    .AppendLine(" 	, TB_T_STALL_USE F ")
                    .AppendLine(" WHERE ")
                    .AppendLine("     	A.DLR_CD = :DLR_CD   ")
                    .AppendLine(" 	AND A.BRN_CD = :BRN_CD  ")
                    '.AppendLine(" 	AND A.RO_NUM = :RO_NUM ")
                    .AppendLine(" 	AND I.RO_NUM = :RO_NUM ")
                    .AppendLine(" 	AND B.SVCIN_ID = A.SVCIN_ID ")
                    .AppendLine(" 	AND C.JOB_DTL_ID = B.JOB_DTL_ID ")
                    .AppendLine(" 	AND I.SVCIN_ID = B.SVCIN_ID ")
                    .AppendLine(" 	AND I.RO_NUM = C.RO_NUM ")
                    .AppendLine(" 	AND I.RO_SEQ = C.RO_SEQ ")
                    .AppendLine(" 	AND I.DLR_CD = A.DLR_CD ")
                    .AppendLine(" 	AND I.BRN_CD = A.BRN_CD ")
                    .AppendLine(" 	AND F.JOB_DTL_ID = B.JOB_DTL_ID ")
                    .AppendLine(" 	AND F.STALL_USE_ID = (SELECT MAX(STALL_USE_ID) FROM TB_T_STALL_USE WHERE JOB_DTL_ID = B.JOB_DTL_ID) ")
                    .AppendLine("   AND C.STARTWORK_INSTRUCT_FLG = '1' ")     '着工指示フラグ：指示済
                    '2014/07/31 [作業内容.表示商品ID]は代表整備のみしかない為、一般整備の判定より除外／SQL記述を規約遵守に変更　Start
                    '.AppendLine(" AND (EXISTS(SELECT 1 FROM TB_T_FINAL_INSPECTION_DETAIL WHERE JOB_DTL_ID = C.JOB_DTL_ID AND JOB_INSTRUCT_ID=C.JOB_INSTRUCT_ID  ")
                    '.AppendLine(" AND JOB_INSTRUCT_SEQ=C.JOB_INSTRUCT_SEQ AND INSPEC_ITEM_CD='                    ') ")

                    .AppendLine("   AND ")
                    .AppendLine("      ( EXISTS( ")
                    .AppendLine("            SELECT 1 ")
                    .AppendLine("              FROM TB_T_FINAL_INSPECTION_DETAIL EXD ")
                    .AppendLine("             WHERE EXD.JOB_DTL_ID = C.JOB_DTL_ID ")
                    .AppendLine("               AND EXD.JOB_INSTRUCT_ID = C.JOB_INSTRUCT_ID  ")
                    .AppendLine("               AND EXD.JOB_INSTRUCT_SEQ = C.JOB_INSTRUCT_SEQ ")
                    .AppendLine("               AND EXD.INSPEC_ITEM_CD = '                    ' ")
                    .AppendLine("              ) ")
                    '.AppendLine(" OR (NOT EXISTS (SELECT 1 FROM TB_M_MERCHANDISE V ,TB_M_SERVICE W WHERE V.MERC_ID = B.MERC_ID AND V.SVC_CD = W.SVC_CD) ")
                    '2014/07/31 [作業内容.表示商品ID]は代表整備のみしかない為、一般整備の判定より除外／SQL記述を規約遵守に変更　End
                    .AppendLine(" OR NOT EXISTS( ")
                    .AppendLine(" SELECT 1 FROM ")
                    .AppendLine("      TB_M_VEHICLE ")
                    .AppendLine("     ,TB_M_INSPECTION_COMB M ")
                    .AppendLine("     ,TB_M_MERCHANDISE ")
                    .AppendLine("     ,TB_M_MAINTE_ATTR ")
                    .AppendLine(" WHERE ")
                    .AppendLine("         TB_M_VEHICLE.VCL_ID=A.VCL_ID ")
                    '整備属性マスタに販売店登録が無い場合は全販売店で検索
                    If specifyDlrCdFlgs("MAINTE_CD_EXIST") = True Then
                        .AppendLine("     AND TB_M_MAINTE_ATTR.DLR_CD=A.DLR_CD ")
                    Else
                        .AppendLine("     AND TB_M_MAINTE_ATTR.DLR_CD='" & AllDealer & "'")
                    End If
                    .AppendLine("     AND TB_M_MAINTE_ATTR.MAINTE_CD=C.JOB_CD ")
                    .AppendLine("     AND (TB_M_MAINTE_ATTR.MAINTE_KATASHIKI='X' OR TB_M_MAINTE_ATTR.MAINTE_KATASHIKI= TB_M_VEHICLE.VCL_KATASHIKI) ")
                    .AppendLine("     AND TB_M_MERCHANDISE.MERC_ID=TB_M_MAINTE_ATTR.MERC_ID ")
                    '2019/12/02 NCN吉川　TKM要件：型式対応 Start
                    '型式使用時
                    If specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
                        '点検組み合わせマスタと車両マスタの型式を条件にする
                        .AppendLine("     AND M.VCL_KATASHIKI = TB_M_VEHICLE.VCL_KATASHIKI")

                    Else 'モデルコード使用時または2回目
                        '型式を半角スペースを条件にする
                        .AppendLine("     AND M.VCL_KATASHIKI = ' '")
                    End If
                    '2019/12/02 NCN吉川　TKM要件：型式対応 End
                    .AppendLine("     AND M.MODEL_CD=TB_M_VEHICLE.MODEL_CD ")

                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
                    '.AppendLine("     AND M.GRADE_CD=TB_M_VEHICLE.GRADE_NAME ")
                    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End

                    .AppendLine("     AND M.SVC_CD=TB_M_MERCHANDISE.SVC_CD ")
                    '点検組み合わせマスタに登録がある場合は指定販売店コードで、無い場合は全販売店コードで絞り込む
                    If specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = True Then
                        .AppendLine("     AND M.DLR_CD=A.DLR_CD ")
                        .AppendLine("     AND M.BRN_CD=A.BRN_CD)) ")
                    Else
                        .AppendLine("     AND M.DLR_CD='" & AllDealer & "'")
                        .AppendLine("     AND M.BRN_CD='" & AllBranch & "')) ")
                    End If
                    '2015/04/14 新販売店追加対応 end
                    '.AppendLine(" ) ")     '2014/07/31 [作業内容.表示商品ID]は代表整備のみしかない為、一般整備の判定より除外／SQL記述を規約遵守に変更
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号

                ''SQL実行
                'Return query.GetData()

                'SQL実行
                Dim MainteData As New SC3180201MainteCodeListDataTable
                MainteData = query.GetData()
                ' 2015/02/18 BTS167 追加作業のGSジョブが、完成検査一覧に表示されない。Start
                ' 追加Jobの場合にレコードが消されてしまう為、削除ロジックをコメントアウト
                'RemoveMainteData(MainteData)
                ' 2015/02/18 BTS167 追加作業のGSジョブが、完成検査一覧に表示されない。End
                Return MainteData

            End Using

        End Function

        '2014/06/10 表示の仕様変更のため追加　Start
        ''' <summary>
        ''' RemoveInspectCodeData
        ''' </summary>
        ''' <param name="InspectData">SC3180201InspectCodeDataTable</param>
        ''' <remarks></remarks>
        Private Sub RemoveInspectData(ByRef InspectData As SC3180201InspectCodeDataTable)

            Dim intIdx As Integer = 0

            If 0 < InspectData.Count Then
                Do While InspectData.Count > intIdx
                    If InspectData(intIdx).IsAPPROVAL_STATUSNull = False Then
                        If InspectData(intIdx).IsINSPEC_RSLT_CDNull = True Then
                            InspectData.RemoveSC3180201InspectCodeRow(InspectData(intIdx))
                        Else
                            intIdx += 1
                        End If
                    Else
                        intIdx += 1
                    End If
                Loop
            End If
        End Sub

        ''' <summary>
        ''' RemoveInspectCodeData
        ''' </summary>
        ''' <param name="MainteData">SC3180201MainteCodeListDataTable</param>
        ''' <remarks></remarks>
        Private Sub RemoveMainteData(ByRef MainteData As SC3180201MainteCodeListDataTable)

            Dim intIdx As Integer = 0

            If 0 < MainteData.Count Then
                Do While MainteData.Count > intIdx
                    If MainteData(intIdx).IsAPPROVAL_STATUSNull = False Then
                        If MainteData(intIdx).IsINSPEC_RSLT_CDNull = True Then
                            MainteData.RemoveSC3180201MainteCodeListRow(MainteData(intIdx))
                        Else
                            intIdx += 1
                        End If
                    Else
                        intIdx += 1
                    End If
                Loop
            End If
        End Sub
        '2014/06/10 表示の仕様変更のため追加　End

        ''' <summary>
        ''' GetDBInspecCodeList(作業一覧取得)
        ''' ※ServiceItems(整備・点検項目一覧)に表示するデータを取得する
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>作業一覧情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetDBInspecCodeList(ByVal dlrCD As String, _
                                            ByVal brnCD As String, _
                                            ByVal roNum As String) As SC3180201InspecCodeListDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201InspecCodeListDataTable)("SC3180201_007")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_007 */ ")
                    .AppendLine("       A.DLR_CD, ")
                    .AppendLine("       A.BRN_CD, ")
                    .AppendLine("       A.SVCIN_ID, ")
                    .AppendLine("       B.JOB_DTL_ID, ")
                    .AppendLine("       C.JOB_INSTRUCT_ID, ")
                    .AppendLine("       C.JOB_INSTRUCT_SEQ, ")
                    .AppendLine("       C.RO_NUM, ")
                    .AppendLine("       C.RO_SEQ, ")
                    .AppendLine("       C.JOB_CD, ")
                    .AppendLine("       C.JOB_NAME, ")
                    .AppendLine("       C.JOB_STF_GROUP_NAME, ")
                    .AppendLine("       C.OPERATION_TYPE_NAME, ")
                    .AppendLine("       NVL(D.ACCOUNT,  ' ') as ACCOUNT, ")
                    .AppendLine("       NVL(D.USERNAME, ' ') as USERNAME ")
                    .AppendLine("FROM TB_T_SERVICEIN A, ")
                    .AppendLine("     TB_T_JOB_DTL B, ")
                    .AppendLine("     TB_T_JOB_INSTRUCT C, ")
                    .AppendLine("     TB_T_FINAL_INSPECTION_HEAD E, ")
                    .AppendLine("     TBL_USERS D ")
                    .AppendLine("WHERE A.DLR_CD=:DLR_CD ")
                    .AppendLine("      AND A.BRN_CD=:BRN_CD ")
                    .AppendLine("      AND A.RO_NUM=:RO_NUM ")
                    .AppendLine("      AND A.SVCIN_ID=B.SVCIN_ID ")
                    .AppendLine("      AND B.JOB_DTL_ID=C.JOB_DTL_ID ")
                    .AppendLine("      AND B.JOB_DTL_ID=E.JOB_DTL_ID (+) ")
                    .AppendLine("      AND E.INSPECTION_APPROVAL_REQ_STF_CD=D.ACCOUNT (+) ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' GetDBChkLastChip(最終チップ判定)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>判定結果データセット</returns>
        ''' <remarks></remarks>
        Public Function GetDBChkLastChip(ByVal dlrCD As String, _
                                       ByVal brnCD As String, _
                                       ByVal roNum As String) As SC3180201ChkLastChipDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201ChkLastChipDataTable)("SC3180201_008")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '.AppendLine("SELECT  /* SC3180201_008 */ ")
                    '.AppendLine("       COUNT(*) AS COUNT ")
                    '.AppendLine("FROM TB_T_SERVICEIN A, ")
                    '.AppendLine("     TB_T_JOB_DTL B ")
                    '.AppendLine("WHERE A.DLR_CD=:DLR_CD ")
                    '.AppendLine("      AND A.BRN_CD=:BRN_CD ")
                    '.AppendLine("      AND A.RO_NUM=:RO_NUM ")
                    '.AppendLine("      AND A.SVCIN_ID=B.SVCIN_ID ")
                    '.AppendLine("      AND B.INSPECTION_STATUS='1'  ")       '1：完成検査承認待ち
                    '.AppendLine("      AND NOT EXISTS ")
                    '.AppendLine("      ( ")
                    '.AppendLine("      SELECT * FROM  ")
                    '.AppendLine("      TB_T_SERVICEIN D, ")
                    '.AppendLine("      TB_T_JOB_DTL E ")
                    '.AppendLine("WHERE D.DLR_CD=:DLR_CD ")
                    '.AppendLine("      AND D.BRN_CD=:BRN_CD ")
                    '.AppendLine("      AND D.RO_NUM=:RO_NUM ")
                    '.AppendLine("      AND D.SVCIN_ID=E.SVCIN_ID ")
                    '.AppendLine("      AND E.INSPECTION_STATUS = '0' ")      '0：完成検査未完了
                    '.AppendLine("      ) ")

                    .AppendLine("SELECT  /* SC3180201_008 */ ")
                    .AppendLine("       COUNT(*) AS COUNT ")
                    .AppendLine("FROM TB_T_SERVICEIN SI, ")
                    .AppendLine("     TB_T_JOB_DTL JD ")
                    .AppendLine("WHERE SI.SVCIN_ID = ")
                    .AppendLine("   (SELECT SVCIN_ID ")
                    .AppendLine("      FROM TB_T_RO_INFO RO ")
                    .AppendLine("     WHERE RO_NUM = :RO_NUM ")
                    .AppendLine("       AND DLR_CD = :DLR_CD ")
                    .AppendLine("       AND BRN_CD = :BRN_CD ")
                    .AppendLine("       AND ROWNUM = 1 ) ")
                    .AppendLine("  AND SI.SVCIN_ID = JD.SVCIN_ID ")
                    .AppendLine("  AND JD.INSPECTION_STATUS = '1'  ")     '1：完成検査承認待ち
                    .AppendLine("  AND NOT EXISTS ( ")
                    .AppendLine("    SELECT 1 ")
                    .AppendLine("      FROM TB_T_JOB_DTL E ")
                    .AppendLine("     WHERE E.SVCIN_ID=SI.SVCIN_ID ")
                    .AppendLine("       AND E.CANCEL_FLG = '0' ")         '0：キャンセルフラグ
                    .AppendLine("       AND E.INSPECTION_STATUS = '0') ") '0：完成検査未完了
                    .AppendLine("       ")
                    .AppendLine("  AND NOT EXISTS ( ")
                    .AppendLine("    SELECT 1 ")
                    '2015/01/28 販売店、店舗の条件漏れ修正(2販売店展開ST BTS#056対応)　Start
                    '.AppendLine("      FROM TB_T_RO_INFO F, ")
                    '.AppendLine("           TB_T_JOB_INSTRUCT G ")
                    ''2014/07/29 部品のみのケース対応(UAT#0190対応)　Start
                    ''.AppendLine("     WHERE F.RO_NUM = G.RO_NUM (+) ")
                    ''.AppendLine("       AND F.RO_SEQ = G.RO_SEQ (+) ")
                    ''.AppendLine("       AND F.RO_NUM = :RO_NUM ")
                    ''.AppendLine("       AND F.RO_STATUS >= 50 ")            '50：着工指示待ち(顧客承認完了)
                    ''.AppendLine("       AND F.RO_STATUS <> '99' ")          '99：ROキャンセル
                    ''.AppendLine("       AND NVL(G.STARTWORK_INSTRUCT_FLG, '0') = '0' )") '0：着工指示フラグ未指示 or 作業指示データそのものが存在しない場合

                    '.AppendLine("     WHERE F.RO_NUM = G.RO_NUM ")
                    '.AppendLine("       AND F.RO_SEQ = G.RO_SEQ ")
                    '.AppendLine("       AND F.RO_NUM = :RO_NUM ")
                    '.AppendLine("       AND F.RO_STATUS >= :WAIT_STARTWORK")
                    '.AppendLine("       AND F.RO_STATUS <> :RO_CANCEL ")
                    '.AppendLine("       AND G.STARTWORK_INSTRUCT_FLG = :STARTWORK ) ")
                    ''2014/07/29 部品のみのケース対応(UAT#0190対応)　End

                    .AppendLine("      FROM TB_T_SERVICEIN F, ")
                    .AppendLine("           TB_T_RO_INFO G, ")
                    .AppendLine("           TB_T_JOB_DTL H, ")
                    .AppendLine("           TB_T_JOB_INSTRUCT I ")
                    .AppendLine("     WHERE F.SVCIN_ID = SI.SVCIN_ID ")
                    .AppendLine("       AND F.SVCIN_ID = G.SVCIN_ID ")
                    .AppendLine("       AND F.SVCIN_ID = H.SVCIN_ID ")
                    .AppendLine("       AND H.JOB_DTL_ID = I.JOB_DTL_ID ")
                    .AppendLine("       AND G.RO_NUM = I.RO_NUM ")
                    .AppendLine("       AND G.RO_SEQ = I.RO_SEQ ")
                    .AppendLine("       AND G.RO_STATUS >= :WAIT_STARTWORK")
                    .AppendLine("       AND G.RO_STATUS <> :RO_CANCEL ")
                    .AppendLine("       AND I.STARTWORK_INSTRUCT_FLG = :STARTWORK ) ")
                    '2015/01/28 販売店、店舗の条件漏れ修正(2販売店展開ST BTS#056対応)　End
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                            '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                            '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                            'RO番号
                query.AddParameterWithTypeValue("WAIT_STARTWORK", OracleDbType.NVarchar2, RO_Status.Wait_Startwork) '着工指示待ち(顧客承認完了)
                query.AddParameterWithTypeValue("RO_CANCEL", OracleDbType.NVarchar2, RO_Status.RO_Cancel)           'ROキャンセル
                query.AddParameterWithTypeValue("STARTWORK", OracleDbType.NVarchar2, Startwork.UnInstruct)          '着工指示フラグ未指示

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' SetInspectionUpt(作業内容テーブル更新)
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="accountName">アカウント</param>
        ''' <param name="updateFlg">更新フラグ</param>
        ''' <param name="updateTime">更新日時</param>
        ''' <returns>更新0件：False / 更新1件以上：True</returns>
        ''' <remarks></remarks>
        Public Function SetDBInspectionUpt(ByVal jobDtlId As Decimal, _
                                           ByVal accountName As String, _
                                           ByVal updateFlg As Integer, _
                                           ByVal updateTime As Date) As Boolean

            Dim queryResult As Boolean = False
            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_009")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE /* SC3180201_009 */ ")
                    .AppendLine("       TB_T_JOB_DTL ")
                    Select Case updateFlg
                        'Case UpdateTypeSend
                        '    '承認依頼時（Send）更新パターン
                        '    .AppendLine("SET  INSPECTION_STATUS = '1'")        '1：完成検査承認待ち
                        '    .AppendLine("    ,INSPECTION_REQ_STF_CD = :ACCOUNT_NAME")
                        '    .AppendLine("    ,INSPECTION_REQ_DATETIME = :ROW_UPDATE_DATETIME")
                        '    .AppendLine("    ,INSPECTION_APPROVAL_STF_CD = :ACCOUNT_NAME")
                        '    .AppendLine("    ,ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                        '    .AppendLine("    ,ROW_UPDATE_ACCOUNT = :ACCOUNT_NAME ")
                        '    .AppendLine("    ,ROW_UPDATE_FUNCTION = 'SC3180201' ")
                        'Case UpdateTypeRegist
                        '    '検査完了時（Regist）更新パターン
                        '    .AppendLine("SET  INSPECTION_STATUS = '2'")         '2：完成検査完了
                        '    .AppendLine("    ,INSPECTION_REQ_STF_CD = :ACCOUNT_NAME")
                        '    .AppendLine("    ,INSPECTION_REQ_DATETIME = :ROW_UPDATE_DATETIME")
                        '    .AppendLine("    ,INSPECTION_APPROVAL_STF_CD = :ACCOUNT_NAME")
                        '    .AppendLine("    ,INSPECTION_APPROVAL_DATETIME = :ROW_UPDATE_DATETIME")
                        '    .AppendLine("    ,ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                        '    .AppendLine("    ,ROW_UPDATE_ACCOUNT = :ACCOUNT_NAME ")
                        '    .AppendLine("    ,ROW_UPDATE_FUNCTION = 'SC3180201' ")
                        Case UpdateTypeApprove
                            '検査承認時（Aprove）更新パターン
                            .AppendLine("SET  INSPECTION_STATUS = '2'")        '2：完成検査完了
                            .AppendLine("    ,INSPECTION_APPROVAL_STF_CD = :ACCOUNT_NAME")
                            .AppendLine("    ,INSPECTION_APPROVAL_DATETIME = :ROW_UPDATE_DATETIME")
                            .AppendLine("    ,ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                            .AppendLine("    ,ROW_UPDATE_ACCOUNT = :ACCOUNT_NAME ")
                            .AppendLine("    ,ROW_UPDATE_FUNCTION = 'SC3180201' ")
                        Case UpdateTypeReject
                            '検査否認時（Reject）更新パターン
                            .AppendLine("SET  INSPECTION_STATUS = '0'")        '0：完成検査未完了
                            .AppendLine("    ,INSPECTION_REQ_STF_CD = ' '")
                            .AppendLine("    ,INSPECTION_REQ_DATETIME = to_date('1900/01/01 00:00:00','yyyy/mm/dd hh24:mi:ss') ")
                            .AppendLine("    ,ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
                            .AppendLine("    ,ROW_UPDATE_ACCOUNT = :ACCOUNT_NAME ")
                            .AppendLine("    ,ROW_UPDATE_FUNCTION = 'SC3180201' ")
                    End Select
                    .AppendLine("WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                    If updateFlg = UpdateTypeApprove Then
                        '検査承認時（Aprove）更新パターン
                        .AppendLine("AND  INSPECTION_STATUS = '1' ")        '1：完成検査承認待ち
                    End If
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                  'RO番号
                query.AddParameterWithTypeValue("ACCOUNT_NAME", OracleDbType.NVarchar2, accountName)           '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)          '更新日付

                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using

        End Function

        ''' <summary>
        ''' SelectSvcinLock(サービス入庫 ロック処理)
        ''' </summary>
        ''' <param name="svcinid">サービス入庫ID</param>
        ''' <remarks></remarks>
        Public Shared Sub SelectSvcinLock(ByVal svcinId As Decimal)

            Using query As New DBSelectQuery(Of DataTable)("SC3180201_010")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT ")
                    .Append("  /* SC3180201_010 */ ")
                    .Append("1 ")
                    .Append("FROM ")
                    .Append("  TB_T_SERVICEIN ")
                    .Append("WHERE ")
                    .Append("  SVCIN_ID = :SVCINID  ")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCINID", OracleDbType.Decimal, svcinId)

                query.GetData()
            End Using

        End Sub

        ''' <summary>
        ''' SelectInspectionHeadLock(完成検査ヘッダ　ロック処理)
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <remarks></remarks>
        Public Shared Sub SelectInspectionHeadLock(ByVal jobDtlId As Decimal)

            Using query As New DBSelectQuery(Of DataTable)("SC3180201_011")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT ")
                    .Append("  /* SC3180201_011 */ ")
                    .Append("1 ")
                    .Append("FROM ")
                    .Append("  TB_T_FINAL_INSPECTION_HEAD ")
                    .Append("WHERE ")
                    .Append("  JOB_DTL_ID = :JOBDTLID  ")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("JOBDTLID", OracleDbType.Decimal, jobDtlId)

                query.GetData()
            End Using

        End Sub

        ''' <summary>
        ''' SetInspectionHeadLockUpt(完成検査ヘッダ行ロックバージョン更新ロジック)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <returns>更新0件：False / 更新1件以上：True</returns>
        ''' <remarks></remarks>
        Public Function SetInspectionHeadLockUpt(ByVal dlrCD As String, _
                                                 ByVal brnCD As String, _
                                                 ByVal jobDtlId As Decimal, _
                                                 ByVal lockVersion As Long) As Boolean

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_012")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE /* SC3180201_012 */ ")
                    .AppendLine("       TB_T_FINAL_INSPECTION_HEAD ")
                    .AppendLine("SET ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("  AND DLR_CD = :DLR_CD ")
                    .AppendLine("  AND BRN_CD = :BRN_CD ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                '作業内容ID
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, lockVersion)         '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using
        End Function

        ''' <summary>
        ''' SetServiceInLockUpt(サービスイン行ロックバージョン更新ロジック)
        ''' </summary>
        ''' <param name="svcinid">サービス入庫ID</param>
        ''' <returns>更新0件：False / 更新1件以上：True</returns>
        ''' <remarks></remarks>
        Public Function SetServiceInLockUpt(ByVal svcinId As Decimal) As Boolean

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_013")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE /* SC3180201_013 */ ")
                    .AppendLine("       TB_T_SERVICEIN ")
                    .AppendLine("SET ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE   SVCIN_ID = :SVCINID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("SVCINID", OracleDbType.Decimal, svcinId)                      'サービスインID

                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using
        End Function

        ''' <summary>
        ''' GetServiceLockVersion(サービスイン行ロックバージョン取得ロジック)
        ''' </summary>
        ''' <param name="svcinid">サービス入庫ID</param>
        ''' <returns>サービス入庫行ロック情報</returns>
        ''' <remarks></remarks>
        Public Function GetServiceLockVersion(ByVal svcinId As Decimal) As SC3180201ServiceLockVersionDataTable

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201ServiceLockVersionDataTable)("SC3180201_014")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3180201_014 */ ")
                    .AppendLine("       ROW_LOCK_VERSION ")
                    .AppendLine("FROM TB_T_SERVICEIN ")
                    .AppendLine("WHERE  SVCIN_ID = :SVCINID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("SVCINID", OracleDbType.Decimal, svcinId)                      'サービスインID

                'SQL実行
                Return query.GetData

            End Using
        End Function

        ''' <summary>
        ''' GetPicSaStf(担当SAスタッフ取得)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>担当SAスタッフ情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetPicSaStf(ByVal dlrCD As String, _
                                    ByVal brnCD As String, _
                                    ByVal roNum As String, _
                                    ByVal jobDtlId As String) As SC3180201PicClientDataTable

            ' DBSelectQueryインスタンス生成
            'Using query As New DBSelectQuery(Of SC3180201PicSaStfDataTable)("SC3180201_015")
            Using query As New DBSelectQuery(Of SC3180201PicClientDataTable)("SC3180201_015")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    '.AppendLine("SELECT /* SC3180201_015 */ ")
                    '.AppendLine("       T1.PIC_SA_STF_CD ")
                    '.AppendLine("      ,T2.USERNAME ")
                    '.AppendLine("FROM   TB_T_SERVICEIN T1 ")
                    '.AppendLine("      ,TBL_USERS T2 ")
                    '.AppendLine("WHERE T1.PIC_SA_STF_CD = T2.ACCOUNT ")
                    '.AppendLine("  AND DLR_CD=:dlr_cd  ")
                    '.AppendLine("  AND BRN_CD=:brn_cd ")
                    '.AppendLine("  AND RO_NUM=:ro_num ")

                    .AppendLine("SELECT /* SC3180201_015 */ ")
                    .AppendLine("       T2.ACCOUNT ")
                    .AppendLine("      ,T2.USERNAME ")
                    .AppendLine("      ,T2.OPERATIONCODE ")
                    .AppendLine("FROM   TB_T_SERVICEIN T1 ")
                    .AppendLine("      ,TBL_USERS T2 ")
                    .AppendLine("WHERE T1.PIC_SA_STF_CD = T2.ACCOUNT ")
                    .AppendLine("  AND DLR_CD=:dlr_cd  ")
                    .AppendLine("  AND BRN_CD=:brn_cd ")
                    .AppendLine("  AND RO_NUM=:ro_num ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号
                'query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.NVarchar2, jobDtlId)              '作業内容ID

                'SQL実行
                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' ストール利用IDの取得
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <returns>ストール情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GteStallUse(ByVal jobDtlId As Decimal, _
                                    ByVal dlrCD As String, _
                                    ByVal brnCD As String) As SC3180201StallUseDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201StallUseDataTable)("SC3180201_016")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_016 */ ")
                    .AppendLine("       STALL_USE_ID, ")
                    .AppendLine("       STALL_USE_STATUS ")
                    .AppendLine("FROM TB_T_STALL_USE ")
                    .AppendLine("WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("      AND DLR_CD = :DLR_CD ")
                    .AppendLine("      AND BRN_CD = :BRN_CD ")
                    .AppendLine("      AND STALL_USE_ID IN ( ")
                    .AppendLine("      SELECT MAX(STALL_USE_ID) AS STALL_USE_ID FROM TB_T_STALL_USE GROUP BY JOB_DTL_ID ")
                    .AppendLine("      ) ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                  '作業内容ID
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード


                'SQL実行
                Return query.GetData()

            End Using
        End Function

        ''' <summary>
        ''' UpdateDBServiceINAdviceComment(サービス入庫テーブル、アドバイス更新)
        ''' <param name="svcinId">サービス入庫ID</param>
        ''' <param name="advicdContent">アドバイス</param>
        ''' <param name="accountName">アカウント</param>
        ''' <param name="updateTime">更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <returns>更新0件：False / 更新1件以上：True</returns>
        ''' </summary>
        Public Function SetDBServiceINAdviceComment(ByVal svcinId As Decimal, _
                                                    ByVal advicdContent As String, _
                                                    ByVal accountName As String, _
                                                    ByVal updateTime As Date, _
                                                    ByVal lockVersion As Long) As Boolean

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_017")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_017 */ ")
                    .AppendLine("       TB_T_SERVICEIN ")
                    .AppendLine("SET NEXT_SVCIN_INSPECTION_ADVICE=:ADVICE, ")
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROWUPDATEDATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROWUPDATEACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION ='SC3180201', ")
                    .AppendLine("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE SVCIN_ID=:SVCINID ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROWLOCKVERSION ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("SVCINID", OracleDbType.Decimal, svcinId)                   'サービス入庫ID
                query.AddParameterWithTypeValue("ADVICE", OracleDbType.NVarchar2, advicdContent)            'アドバイスコメント
                query.AddParameterWithTypeValue("ROWUPDATEDATETIME", OracleDbType.Date, updateTime)         '行更新日時
                query.AddParameterWithTypeValue("ROWUPDATEACCOUNT", OracleDbType.NVarchar2, accountName)    '行更新日時
                query.AddParameterWithTypeValue("ROWLOCKVERSION", OracleDbType.Decimal, lockVersion)        '行ロックバージョン
                'SQL実行
                If query.Execute() > 0 Then
                    queryResult = True
                Else
                    queryResult = False
                End If

                Return queryResult

            End Using

        End Function

#Region "通知送信用情報取得"

        ''' <summary>
        ''' GetNoticeProcessingInfo(通知送信用情報取得)
        ''' </summary>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <returns>通知送信用情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetNoticeProcessingInfo(ByVal roNum As String _
                                              , ByVal dlrCD As String _
                                              , ByVal brnCD As String _
                                              , ByVal jobDtlId As Decimal) As SC3180201NoticeProcessingInfoDataTable

            Using query As New DBSelectQuery(Of SC3180201NoticeProcessingInfoDataTable)("SC3180201_018")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT /* SC3180201_018 */ ")
                    .AppendLine("      TRIM(T1.RO_NUM) AS RO_NUM ")
                    .AppendLine("      ,TRIM(T2.REG_NUM) AS REG_NO ")
                    .AppendLine("      ,TRIM(T3.CST_NAME) AS CST_NAME ")
                    .AppendLine("	   ,TRIM(T4.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                    .AppendLine("	   ,TRIM(T4.POSITION_TYPE) AS POSITION_TYPE ")
                    .AppendLine("	   ,NVL(CONCAT(TRIM(T6.UPPER_DISP), TRIM(T6.LOWER_DISP)), ' ') AS MERCHANDISENAME ")
                    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
                    .AppendLine("      ,NVL(TRIM(T3.DMS_CST_CD), ' ') AS DMS_CST_CD ")
                    '2014/07/16　セッション情報作成処理変更　END　　↑↑↑
                    .AppendLine("FROM  TB_T_SERVICEIN T1 ")
                    .AppendLine("     ,TB_M_VEHICLE_DLR T2 ")
                    .AppendLine("	  ,TB_M_CUSTOMER T3 ")
                    .AppendLine("     ,TB_M_NAMETITLE T4 ")
                    .AppendLine("     ,(SELECT SVCIN_ID,MERC_ID FROM TB_T_JOB_DTL WHERE JOB_DTL_ID = :JOBDTLID) T5 ")
                    .AppendLine("     ,TB_M_MERCHANDISE T6 ")
                    .AppendLine("WHERE  T1.CST_ID = T3.CST_ID(+) ")
                    .AppendLine("  AND  T1.VCL_ID = T2.VCL_ID(+) ")
                    .AppendLine("  AND  T1.DLR_CD = T2.DLR_CD(+) ")
                    .AppendLine("  AND  T3.NAMETITLE_CD = T4.NAMETITLE_CD(+) ")
                    .AppendLine("  AND  T1.SVCIN_ID  = T5.SVCIN_ID(+) ")
                    .AppendLine("  AND  T5.MERC_ID = T6.MERC_ID(+) ")
                    .AppendLine("  AND  T1.RO_NUM = :RONUM ")
                    .AppendLine("  AND  T1.DLR_CD = :DLRCD ")
                    .AppendLine("  AND  T1.BRN_CD = :BRNCD ")
                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("RONUM", OracleDbType.NVarchar2, roNum)                'RO番号
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)                '販売店コード
                query.AddParameterWithTypeValue("BRNCD", OracleDbType.NVarchar2, brnCD)                '店舗コード
                query.AddParameterWithTypeValue("JOBDTLID", OracleDbType.Decimal, jobDtlId)                '作業内容ID
                '実行
                Dim dt As SC3180201NoticeProcessingInfoDataTable = query.GetData()

                Return dt

            End Using

        End Function

#End Region

        ''' <summary>
        ''' GetPicClient(依頼者取得)
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="brnCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>依頼者情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetPicClient(ByVal dlrCD As String, _
                                    ByVal brnCD As String, _
                                    ByVal roNum As String, _
                                    ByVal jobDtlId As String) As SC3180201PicClientDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201PicClientDataTable)("SC3180201_019")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT /* SC3180201_019 */ ")
                    .AppendLine("       T2.ACCOUNT ")
                    .AppendLine("      ,T2.USERNAME ")
                    .AppendLine("      ,T2.OPERATIONCODE ")
                    .AppendLine("FROM   TB_T_FINAL_INSPECTION_HEAD T1 ")
                    .AppendLine("      ,TBL_USERS T2 ")
                    '.AppendLine("WHERE T1.ROW_CREATE_ACCOUNT = T2.ACCOUNT ")
                    .AppendLine("WHERE T1.INSPECTION_APPROVAL_REQ_STF_CD = T2.ACCOUNT ")
                    .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                    .AppendLine("  AND T1.RO_NUM = :RO_NUM ")
                    .AppendLine("  AND T1.JOB_DTL_ID = :JOB_DTL_ID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                      '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                      '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)                      'RO番号
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.NVarchar2, jobDtlId)               '作業内容ID

                'SQL実行
                Return query.GetData()

            End Using
        End Function

        '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 Start
        ''' <summary>
        ''' RO番号をキーに、[完成検査結果データ]テーブルに登録された[アドバイス]を取得する
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="branchCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>[完成検査結果データ].[アドバイス]</returns>
        ''' <remarks></remarks>
        Public Function GetAdviceContent(ByVal dealerCD As String, ByVal branchCD As String, ByVal roNum As String) As String

            Using query As New DBSelectQuery(Of SC3180201AdviceDataTable)("SC3180201_020")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_020 */ ")
                    .AppendLine("       RTRIM(FH.ADVICE_CONTENT) as ADVICE_CONTENT ")
                    .AppendLine("  FROM TB_T_RO_INFO RI ")
                    .AppendLine("     , TB_T_SERVICEIN SI ")
                    .AppendLine("     , TB_T_JOB_DTL JD ")
                    .AppendLine("     , TB_T_FINAL_INSPECTION_HEAD FH ")
                    .AppendLine(" WHERE RI.SVCIN_ID = SI.SVCIN_ID ")
                    .AppendLine("   AND SI.SVCIN_ID = JD.SVCIN_ID ")
                    .AppendLine("   AND JD.JOB_DTL_ID = FH.JOB_DTL_ID ")
                    .AppendLine("   AND RI.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND RI.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND RI.RO_NUM = :RO_NUM ")
                    .AppendLine("   AND FH.ADVICE_CONTENT <> ' ' ")
                    .AppendLine("   AND ROWNUM = 1 ")           ' 登録値が存在する場合は全件同じになる為、１件目の値を取得
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)     '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCD)     '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)        'RO番号

                Dim rtn As String = ""
                Dim dt As SC3180201AdviceDataTable = query.GetData()

                If 0 < dt.Count Then
                    rtn = dt(0).ADVICE_CONTENT
                End If

                Return rtn
            End Using

        End Function
        '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 End

        ''' <summary>
        ''' マスタに販売店が登録されているか判定する
        ''' </summary>
        ''' <param name="strRoNum">R/O番号</param>
        ''' <param name="strDlrCd">販売店コード</param>
        ''' <param name="strBrnCd">店舗コード</param>
        ''' <returns>登録状態 DataTable TRANSACTION_EXIST : 1 or 0 , HISTORY_EXIST : 1 or 0, MAINTE_CD_EXIST : 1 or 0, 
        ''' COMB_DLR_AND_BRN_EXIST : 1 or 0</returns>
        ''' <remarks>点検組み合わせマスタ、整備属性マスタに指定の販売店データが登録されているかをフラグで取得する</remarks>
        Public Function GetDlrCdExistMst(ByVal strRoNum As String, _
                                         ByVal strDlrCd As String, _
                                         ByVal strBrnCd As String) As DataTable

            Dim dt As DataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Using query As New DBSelectQuery(Of DataTable)("SC3180201_021")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT DISTINCT")
                    .Append("/* SC3180201_021 */ ")
                    .Append("   CASE WHEN SI.TSI_VCL_ID IS NULL THEN '0' ELSE '1' END AS TRANSACTION_EXIST,")
                    .Append("   CASE WHEN SI.HSI_VCL_ID IS NULL THEN '0' ELSE '1' END AS HISTORY_EXIST,")
                    .Append("   CASE WHEN MA.MAINTE_CD IS NULL THEN '0' ELSE '1' END AS MAINTE_CD_EXIST,")
                    .Append("   CASE WHEN IC.VCL_KATASHIKI <> MV.VCL_KATASHIKI")
                    .Append("      OR NVL(IC.VCL_KATASHIKI, ' ') = ' '")
                    .Append("      OR NVL(MV.VCL_KATASHIKI, ' ') = ' ' THEN '0' ELSE '1' END KATASHIKI_EXIST,")
                    .Append("   CASE WHEN IC.DLR_CD = 'XXXXX'")
                    .Append("      OR IC.BRN_CD = 'XXX'")
                    .Append("      OR IC.DLR_CD IS NULL")
                    .Append("      OR IC.BRN_CD IS NULL THEN '0' ELSE '1' END COMB_DLR_AND_BRN_EXIST")
                    .Append(" FROM")
                    .Append("    TB_M_VEHICLE MV")
                    .Append(" LEFT OUTER JOIN     (")
                    .Append("        SELECT DISTINCT")
                    .Append("            IC.MODEL_CD,")
                    .Append("            IC.DLR_CD,")
                    .Append("            IC.BRN_CD,")
                    .Append("            IC.VCL_KATASHIKI")
                    .Append("        FROM")
                    .Append("            TB_M_INSPECTION_COMB IC")
                    .Append("        WHERE")
                    .Append("            IC.DLR_CD IN(:DLR_CD, 'XXXXX')")
                    .Append("        AND IC.BRN_CD IN(:BRN_CD, 'XXX')")
                    .Append("        ORDER BY IC.DLR_CD ASC,")
                    .Append("                 IC.BRN_CD ASC,")
                    .Append("                 IC.VCL_KATASHIKI DESC")
                    .Append("     ) IC ON IC.MODEL_CD = MV.MODEL_CD")
                    .Append("     AND IC.VCL_KATASHIKI IN ( MV.VCL_KATASHIKI, ' ')")
                    .Append(" LEFT OUTER JOIN     (")
                    .Append("        SELECT")
                    .Append("            TSI.VCL_ID TSI_VCL_ID,")
                    .Append("            HSI.VCL_ID HSI_VCL_ID,")
                    .Append("            CASE")
                    .Append("                WHEN TSI.VCL_ID IS NOT NULL THEN  TSI.VCL_ID")
                    .Append("                WHEN HSI.VCL_ID IS NOT NULL THEN  HSI.VCL_ID")
                    .Append("                ELSE 0")
                    .Append("            END AS VCL_ID")
                    .Append("        FROM")
                    .Append("            (SELECT :RO_NUM  AS RO_NUM FROM DUAL) VERTUAL")
                    .Append("            LEFT OUTER JOIN TB_T_SERVICEIN TSI")
                    .Append("                ON  VERTUAL.RO_NUM = TSI.RO_NUM")
                    .Append("                AND TSI.DLR_CD = :DLR_CD")
                    .Append("                AND TSI.BRN_CD = :BRN_CD")
                    .Append("                AND ROWNUM = 1")
                    .Append("            LEFT OUTER JOIN TB_H_SERVICEIN HSI")
                    .Append("                ON  VERTUAL.RO_NUM = HSI.RO_NUM")
                    .Append("                AND HSI.DLR_CD = :DLR_CD")
                    .Append("                AND HSI.BRN_CD = :BRN_CD")
                    .Append("                AND ROWNUM = 1")
                    .Append("     ) SI ON MV.VCL_ID = SI.VCL_ID")
                    .Append(" LEFT OUTER JOIN     (")
                    .Append("        SELECT")
                    .Append("            HSI.VCL_ID,")
                    .Append("            HSI.RO_NUM")
                    .Append("        FROM")
                    .Append("            TB_H_SERVICEIN HSI")
                    .Append("        WHERE")
                    .Append("            HSI.RO_NUM = :RO_NUM")
                    .Append("        AND HSI.DLR_CD = :DLR_CD")
                    .Append("        AND HSI.BRN_CD = :BRN_CD")
                    .Append("        AND ROWNUM = 1")
                    .Append("    ) HSI ON HSI.RO_NUM = :RO_NUM")
                    .Append(" LEFT OUTER JOIN     (")
                    .Append("        SELECT")
                    .Append("            MAINTE_CD,")
                    .Append("            DLR_CD")
                    .Append("        FROM")
                    .Append("            TB_M_MAINTE_ATTR")
                    .Append("        WHERE ")
                    .Append("            DLR_CD = :DLR_CD")
                    .Append("            AND ROWNUM=1")
                    .Append("    ) MA ON MA.DLR_CD = :DLR_CD")
                    .Append(" WHERE")
                    .Append("    MV.VCL_ID = SI.VCL_ID")
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

        '2015/04/14 新販売店追加対応 end


        '2016/11/08 (TR-SVT-TMT-20160512-001) サービス来店者管理にデータが無い場合はSA通知を送らない
        ''' <summary>
        ''' 対象のROにおいて、サービス来店者管理にデータが存在するか確認する
        ''' </summary>
        ''' <param name="strRoNum"></param>
        ''' <param name="strDlrCd"></param>
        ''' <param name="strBrnCd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSvcVisitManagementExist(ByVal strRoNum As String, ByVal strDlrCd As String, ByVal strBrnCd As String) As Boolean

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Dim rtnBool As Boolean = True
            Dim dt As DataTable

            Using query As New DBSelectQuery(Of DataTable)("SC3180201_022")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("    /* SC3180201_022 */ ")
                    .AppendLine("    COUNT(1) AS EXIST_COUNT ")
                    .AppendLine("FROM ")
                    .AppendLine("    TB_T_RO_INFO RI, ")
                    .AppendLine("    TBL_SERVICE_VISIT_MANAGEMENT SVM ")
                    .AppendLine("WHERE ")
                    .AppendLine("    RI.RO_NUM = :RO_NUM ")
                    .AppendLine("AND RI.DLR_CD = :DLR_CD ")
                    .AppendLine("AND RI.BRN_CD = :BRN_CD ")
                    .AppendLine("AND RI.VISIT_ID = SVM.VISITSEQ ")
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

            If dt.Rows(0).Item("EXIST_COUNT").ToString = "0" Then
                rtnBool = False
            Else
                rtnBool = True
            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return rtnBool

        End Function

        '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する Start
        ''' <summary>
        ''' GetHeadLockVersion(完成検査結果データ行ロックバージョン取得ロジック)
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <returns>ロックバージョン</returns>
        ''' <remarks></remarks>
        Public Function GetHeadLockVersion(ByVal jobDtlId As Decimal) As SC3180201ServiceLockVersionDataTable

            Dim queryResult As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201ServiceLockVersionDataTable)("SC3180201_030")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_030 */ ")
                    .AppendLine("       ROW_LOCK_VERSION ")
                    .AppendLine("FROM TB_T_FINAL_INSPECTION_HEAD ")
                    .AppendLine("WHERE  JOB_DTL_ID = :JOB_DTL_ID ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)                      'サービスインID

                'SQL実行
                Return query.GetData

            End Using
        End Function

        ''' <summary>
        ''' アドバイス更新対象の取得
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="branchCD">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>アドバイス更新対象データ</returns>
        ''' <remarks></remarks>
        Public Function SelectInspectionHeadList(ByVal dealerCD As String, ByVal branchCD As String, ByVal roNum As String) As SC3180201AdviceJobDataTable

            ' DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3180201AdviceJobDataTable)("SC3180201_031")

                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT /* SC3180201_031 */  ")
                    .AppendLine("    JOB_DTL_ID ")
                    .AppendLine("FROM ")
                    .AppendLine("     TB_T_FINAL_INSPECTION_HEAD ")
                    .AppendLine("WHERE")
                    .AppendLine("        DLR_CD =:DLR_CD ")
                    .AppendLine("    AND BRN_CD =:BRN_CD ")
                    .AppendLine("    AND RO_NUM =:RO_NUM ")
                End With

                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)        '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCD)        '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)           'RO番号

                'SQL実行
                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 完成検査結果アドバイス更新
        ''' 指定されたJOB_DTL_IDでアドバイスの更新を行う
        ''' </summary>
        ''' <param name="jobDtlId">作業内容ID</param>
        ''' <param name="adviceContent">アドバイス</param>
        ''' <param name="accountName">行更新アカウント</param>
        ''' <param name="updateTime">行更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <param name="updateFunction">行更新機能</param>
        ''' <returns>True：成功、False：失敗</returns>
        ''' <remarks></remarks>
        Public Function SetDBInspectionAdviceUpt(ByVal jobDtlId As String, _
                                                 ByVal adviceContent As String, _
                                                 ByVal accountName As String, _
                                                 ByVal updateTime As Date, _
                                                 ByVal lockVersion As Decimal, _
                                                 Optional ByVal updateFunction As String = "SC3180201") As Boolean

            Dim isSuccessUpt As Boolean = False

            ' DBSelectQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3180201_032")

                Dim sql As New StringBuilder

                ' SQL文の作成
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_032 */ ")
                    .AppendLine("       TB_T_FINAL_INSPECTION_HEAD ")
                    .AppendLine("SET ADVICE_CONTENT = :ADVICE_CONTENT, ")
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE JOB_DTL_ID = :JOB_DTL_ID ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()

                'NULL登録回避処理
                If String.IsNullOrEmpty(adviceContent) Then
                    adviceContent = " "
                End If

                ' バインド変数定義
                query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.NVarchar2, jobDtlId)     '作業内容ID
                query.AddParameterWithTypeValue("ADVICE_CONTENT", OracleDbType.NVarchar2, adviceContent)        'アドバイスコメント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)           '更新日付
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)      '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, updateFunction)  '行更新機能
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, lockVersion)          '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    isSuccessUpt = True
                Else
                    isSuccessUpt = False
                End If

                Return isSuccessUpt

            End Using

        End Function
        '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する End

        '2017/2/20 ライフサイクル対応 走行距離を完成検査で登録する Start

        ''' <summary>
        ''' 前回部品交換情報リスト取得
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <returns>前回部品交換情報リスト</returns>
        ''' <remarks></remarks>
        Public Function GetPreviousPartsReplace(ByVal vin As String) As SC3180201PreviousPartsReplaceDataTable

            Using query As New DBSelectQuery(Of SC3180201PreviousPartsReplaceDataTable)("SC3180201_023")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_023 */ ")
                    .AppendLine("    INSPEC_ITEM_CD, ")
                    .AppendLine("    RO_NUM, ")
                    .AppendLine("    REPLACE_MILE, ")
                    .AppendLine("    REPLACE_DATE, ")
                    .AppendLine("    PREVIOUS_REPLACE_MILE, ")
                    .AppendLine("    PREVIOUS_REPLACE_DATE, ")
                    .AppendLine("    ROW_LOCK_VERSION")
                    .AppendLine("FROM TB_T_PREVIOUS_PARTS_REPLACE ")
                    .AppendLine("WHERE ")
                    .AppendLine("   VCL_VIN = :VCL_VIN ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)

                Return query.GetData()
            End Using

        End Function

        ''' <summary>
        ''' 前回部品交換情報 ロック処理
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <remarks></remarks>
        Public Shared Sub SelectPartsReplaceLock(ByVal vin As String, ByVal inspecItemCd As String)

            Using query As New DBSelectQuery(Of DataTable)("SC3180201_024")
                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_024 */ ")
                    .AppendLine("       1 ")
                    .AppendLine("FROM TB_T_PREVIOUS_PARTS_REPLACE ")
                    .AppendLine("WHERE ")
                    .AppendLine("   VCL_VIN = :VCL_VIN ")
                    .AppendLine("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)

                query.GetData()

            End Using
        End Sub

        '【***完成検査_排他制御***】 start
        ''' <summary>
        ''' 完成検査結果更新用サービス入庫行ロックバージョン取得
        ''' <param name="roNum">RO番号</param>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="branchCD">店舗コード</param>
        ''' </summary>
        ''' <returns>行ロックバージョン取得</returns>
        ''' <remarks></remarks>
        Public Function GetAndLockServiceinRow(ByVal roNum As String,
                                               ByVal dealerCD As String,
                                               ByVal branchCD As String) As DataTable

            ' DBSelectQueryインスタンス生
            Using query As New DBSelectQuery(Of DataTable)("SC3180201_088")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                Dim dt As DataTable

                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT ")
                    .AppendLine("  /* SC3180201_088 */ ")
                    .AppendLine("  ROW_LOCK_VERSION ")
                    .AppendLine("FROM ")
                    .AppendLine("  TB_T_SERVICEIN ")
                    .AppendLine("WHERE ")
                    .AppendLine("    DLR_CD =:DLR_CD ")
                    .AppendLine("    AND BRN_CD =:BRN_CD ")
                    .AppendLine("    AND RO_NUM =:RO_NUM ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)        '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCD)        '店舗コード
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)           'RO番号       'サービスインID

                dt = query.GetData()

                Return dt

            End Using

        End Function
        '【***完成検査_排他制御***】 end

        ''' <summary>
        ''' 前回部品交換情報 登録処理
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="replaceMile">交換走行距離</param>
        ''' <param name="accountName">行更新アカウント</param>
        ''' <param name="updateTime">行更新日時</param>
        ''' <param name="updateFunction">行更新機能</param>
        ''' <returns>登録の成功：True　登録の失敗：False</returns>
        ''' <remarks></remarks>
        Public Function SetPartsReplaceIns(ByVal vin As String, _
                                           ByVal inspecItemCd As String, _
                                           ByVal roNum As String, _
                                           ByVal replaceMile As Decimal, _
                                           ByVal accountName As String, _
                                           ByVal updateTime As Date, _
                                           ByVal updateFlg As Long, _
                                           Optional ByVal updateFunction As String = "SC3180201") As Boolean

            Dim isSuccessIns As Boolean = False

            Using query As New DBUpdateQuery("SC3180201_025")
                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .AppendLine("INSERT INTO ")
                    .AppendLine("  /* SC3180201_025 */ ")
                    .AppendLine("            TB_T_PREVIOUS_PARTS_REPLACE( ")
                    .AppendLine(" VCL_VIN, ")
                    .AppendLine(" INSPEC_ITEM_CD, ")
                    .AppendLine(" RO_NUM, ")
                    .AppendLine(" REPLACE_MILE, ")
                    If updateFlg = UpdateTypeApprove Then
                        .AppendLine(" REPLACE_DATE, ")
                    End If
                    .AppendLine(" PREVIOUS_REPLACE_MILE, ")
                    .AppendLine(" ROW_CREATE_DATETIME, ")
                    .AppendLine(" ROW_CREATE_ACCOUNT, ")
                    .AppendLine(" ROW_CREATE_FUNCTION, ")
                    .AppendLine(" ROW_UPDATE_DATETIME, ")
                    .AppendLine(" ROW_UPDATE_ACCOUNT, ")
                    .AppendLine(" ROW_UPDATE_FUNCTION, ")
                    .AppendLine(" ROW_LOCK_VERSION")
                    .AppendLine(") ")
                    .AppendLine("VALUES( ")
                    .AppendLine(" :VCL_VIN, ")
                    .AppendLine(" :INSPEC_ITEM_CD, ")
                    .AppendLine(" :RO_NUM, ")
                    .AppendLine(" :REPLACE_MILE, ")
                    If updateFlg = UpdateTypeApprove Then
                        .AppendLine(" :ROW_UPDATE_DATETIME, ")
                    End If
                    .AppendLine(" :PREVIOUS_REPLACE_MILE, ")
                    .AppendLine(" :ROW_UPDATE_DATETIME, ")
                    .AppendLine(" :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine(" :ROW_UPDATE_FUNCTION, ")
                    .AppendLine(" :ROW_UPDATE_DATETIME, ")
                    .AppendLine(" :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine(" :ROW_UPDATE_FUNCTION, ")
                    .AppendLine("  0")
                    .AppendLine(") ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
                query.AddParameterWithTypeValue("REPLACE_MILE", OracleDbType.Decimal, replaceMile)
                query.AddParameterWithTypeValue("PREVIOUS_REPLACE_MILE", OracleDbType.Decimal, DefaultPreviousReplaceMile)

                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)      '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)           '更新日付
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, updateFunction)  '行作成機能、行更新機能

                'SQL実行
                If query.Execute() > 0 Then
                    isSuccessIns = True
                Else
                    isSuccessIns = False
                End If
                Return isSuccessIns

            End Using
        End Function

        ''' <summary>
        ''' 前回部品交換情報 更新処理(別RO)
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="replaceMile">交換走行距離</param>
        ''' <param name="preReplaceMile">前回交換走行距離</param>
        ''' <param name="preReplaceDate">前回交換日時</param>
        ''' <param name="accountName">行更新アカウント</param>
        ''' <param name="updateTime">行更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <param name="updateFunction">行更新機能</param>
        ''' <returns>更新結果</returns>
        ''' <remarks></remarks>
        Public Overloads Function SetPartsReplaceUpt(ByVal vin As String, _
                                                       ByVal inspecItemCd As String, _
                                                       ByVal roNum As String, _
                                                       ByVal replaceMile As Decimal, _
                                                       ByVal replaceDate As Date, _
                                                       ByVal preReplaceMile As Decimal, _
                                                       ByVal preReplaceDate As Date, _
                                                       ByVal accountName As String, _
                                                       ByVal updateTime As Date, _
                                                       ByVal lockVersion As Long, _
                                                       Optional ByVal updateFunction As String = "SC3180201") As Boolean

            Dim isSuccessUpt As Boolean = False

            Using query As New DBUpdateQuery("SC3180201_026")

                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_026 */ ")
                    .AppendLine("       TB_T_PREVIOUS_PARTS_REPLACE ")
                    .AppendLine("SET RO_NUM = :RO_NUM, ")
                    .AppendLine("    REPLACE_MILE = :REPLACE_MILE, ")
                    .AppendLine("    REPLACE_DATE = :REPLACE_DATE, ")
                    .AppendLine("    PREVIOUS_REPLACE_MILE = :PREVIOUS_REPLACE_MILE, ")
                    .AppendLine("    PREVIOUS_REPLACE_DATE = :PREVIOUS_REPLACE_DATE, ")
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE VCL_VIN = :VCL_VIN ")
                    .AppendLine("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
                query.AddParameterWithTypeValue("REPLACE_MILE", OracleDbType.Decimal, replaceMile)
                query.AddParameterWithTypeValue("REPLACE_DATE", OracleDbType.Date, replaceDate)
                query.AddParameterWithTypeValue("PREVIOUS_REPLACE_MILE", OracleDbType.Decimal, preReplaceMile)
                query.AddParameterWithTypeValue("PREVIOUS_REPLACE_DATE", OracleDbType.Date, preReplaceDate)

                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)      '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)           '更新日付
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, updateFunction)  '行作成機能、行更新機能
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lockVersion)          '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    isSuccessUpt = True
                Else
                    isSuccessUpt = False
                End If

                Return isSuccessUpt

            End Using

        End Function

        ''' <summary>
        ''' 前回部品交換情報 更新処理(同一RO)
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <param name="accountName">行更新アカウント</param>
        ''' <param name="updateTime">行更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <param name="updateFunction">行更新機能</param>
        ''' <returns>更新結果</returns>
        ''' <remarks></remarks>
        Public Overloads Function SetPartsReplaceUpt(ByVal vin As String, _
                                                       ByVal inspecItemCd As String, _
                                                       ByVal replaceDate As Date, _
                                                       ByVal accountName As String, _
                                                       ByVal updateTime As Date, _
                                                       ByVal lockVersion As Long, _
                                                       Optional ByVal updateFunction As String = "SC3180201") As Boolean

            Dim isSuccessUpt As Boolean = False

            Using query As New DBUpdateQuery("SC3180201_027")

                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_027 */ ")
                    .AppendLine("       TB_T_PREVIOUS_PARTS_REPLACE ")
                    .AppendLine("SET ")
                    .AppendLine("    REPLACE_DATE = :REPLACE_DATE, ")
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE VCL_VIN = :VCL_VIN ")
                    .AppendLine("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)
                query.AddParameterWithTypeValue("REPLACE_DATE", OracleDbType.Date, replaceDate)

                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)      '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)           '更新日付
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, updateFunction)  '行作成機能、行更新機能
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lockVersion)          '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    isSuccessUpt = True
                Else
                    isSuccessUpt = False
                End If

                Return isSuccessUpt

            End Using

        End Function

        ''' <summary>
        ''' 販売店システム設定から設定値を取得する
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
        ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
        ''' <param name="settingName">販売店システム設定名</param>
        ''' <returns>SystemSettingDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
                                                 ByVal branchCode As String, _
                                                 ByVal allDealerCode As String, _
                                                 ByVal allBranchCode As String, _
                                                 ByVal settingName As String) As SC3180201SystemSettingDataTable

            Dim sql As New StringBuilder
            With sql
                .Append("   SELECT /* SC3180201_028 */ ")
                .Append(" 		   SETTING_VAL ")
                .Append("     FROM ")
                .Append(" 		   TB_M_SYSTEM_SETTING_DLR ")
                .Append("    WHERE ")
                .Append(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                .Append(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
                .Append("      AND SETTING_NAME = :SETTING_NAME ")
                .Append(" ORDER BY ")
                .Append("          DLR_CD ASC, BRN_CD ASC ")
            End With

            Dim dt As SC3180201SystemSettingDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3180201SystemSettingDataTable)("SC3180201_028")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
                query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
                query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

                dt = query.GetData()
            End Using

            Return dt

        End Function

        ''' <summary>
        ''' 前回部品交換時の走行距離を取得する
        ''' </summary>
        ''' <param name="strDLR_CD">販売店コード</param>
        ''' <param name="strSVCIN_NUM">入庫管理番号</param>
        ''' <returns>走行距離情報</returns>
        ''' <remarks></remarks>
        Public Function GetPreviosReplacementMileage(ByVal strDLR_CD As String, ByVal strSVCIN_NUM As String) As SC3180201PreviosReplacementMileageDataTable

            Using query As New DBSelectQuery(Of SC3180201PreviosReplacementMileageDataTable)("SC3180201_029")
                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .Append("SELECT /* SC3180201_029 */ ")
                    .Append("	NVL(T2.REG_MILE, 0) AS REG_MILE ")
                    .Append("FROM ")
                    .Append("	 TB_T_VEHICLE_SVCIN_HIS T1 ")
                    .Append("	,TB_T_VEHICLE_MILEAGE T2 ")
                    .Append("WHERE ")
                    .Append("	T1.DLR_CD = :DLR_CD ")
                    .Append("	AND T1.SVCIN_NUM = :SVCIN_NUM ")
                    .Append("	AND T1.VCL_MILE_ID = T2.VCL_MILE_ID(+) ")

                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDLR_CD)
                query.AddParameterWithTypeValue("SVCIN_NUM", OracleDbType.NVarchar2, strSVCIN_NUM)

                sql = Nothing

                Using dt As SC3180201PreviosReplacementMileageDataTable = query.GetData
                    Return dt
                End Using
            End Using
        End Function

        '2017/2/20 ライフサイクル対応 走行距離を完成検査で登録する End

        ''' <summary>
        ''' 前回部品交換情報リスト削除
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DelPreviousPartsReplace(ByVal vin As String, ByVal inspecItemCd As String) As Boolean

            Dim sql As New StringBuilder
            'SQL文作成
            With sql
                .Append("DELETE FROM /* SC3180204_044 */ ")
                .Append("	TB_T_PREVIOUS_PARTS_REPLACE ")
                .Append("WHERE ")
                .Append("   VCL_VIN = :VCL_VIN ")
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            End With

            Dim result As Integer = 0

            Using query As New DBUpdateQuery("SC3180204_044")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)
                result = query.Execute()
            End Using

            'SQL実行
            If result > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' 前回部品交換情報 更新処理(Replace→非Replace)
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="inspecItemCd">点検項目コード</param>
        ''' <param name="previousReplaceMile">前回交換走行距離</param>
        ''' <param name="previousReplaceDate">前回交換日時</param>
        ''' <param name="accountName">行更新アカウント</param>
        ''' <param name="updateTime">行更新日時</param>
        ''' <param name="lockVersion">行ロックバージョン</param>
        ''' <param name="updateFunction">行更新機能</param>
        ''' <returns>更新結果</returns>
        ''' <remarks></remarks>
        Public Overloads Function SetDelPartsReplaceUpt(ByVal vin As String, _
                                                        ByVal inspecItemCd As String, _
                                                        ByVal previousReplaceMile As Decimal, _
                                                        ByVal previousReplaceDate As Date, _
                                                        ByVal accountName As String, _
                                                        ByVal updateTime As Date, _
                                                        ByVal lockVersion As Long, _
                                                        Optional ByVal updateFunction As String = "SC3180204") As Boolean

            Dim isSuccessUpt As Boolean = False

            Using query As New DBUpdateQuery("SC3180201_034")

                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE ")
                    .AppendLine("  /* SC3180201_034 */ ")
                    .AppendLine("       TB_T_PREVIOUS_PARTS_REPLACE ")
                    .AppendLine("SET RO_NUM = :RO_NUM, ")
                    .AppendLine("    REPLACE_MILE = :REPLACE_MILE, ")
                    .AppendLine("    REPLACE_DATE = :REPLACE_DATE, ")
                    .AppendLine("    PREVIOUS_REPLACE_MILE = :PREVIOUS_REPLACE_MILE, ")
                    .AppendLine("    PREVIOUS_REPLACE_DATE = TO_DATE('1900/01/01', 'YYYY/MM/DD'), ")
                    .AppendLine("    ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME, ")
                    .AppendLine("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
                    .AppendLine("    ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
                    .AppendLine("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
                    .AppendLine("WHERE VCL_VIN = :VCL_VIN ")
                    .AppendLine("  AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("  AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vin)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)

                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, " ")
                query.AddParameterWithTypeValue("REPLACE_MILE", OracleDbType.Decimal, previousReplaceMile)
                query.AddParameterWithTypeValue("REPLACE_DATE", OracleDbType.Date, previousReplaceDate)
                query.AddParameterWithTypeValue("PREVIOUS_REPLACE_MILE", OracleDbType.Decimal, DefaultPreviousReplaceMile)

                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, accountName)      '行更新アカウント
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, updateTime)           '更新日付
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, updateFunction)  '行作成機能、行更新機能
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, lockVersion)          '行ロックバージョン

                'SQL実行
                If query.Execute() > 0 Then
                    isSuccessUpt = True
                Else
                    isSuccessUpt = False
                End If

                Return isSuccessUpt

            End Using

        End Function


    End Class
End Namespace

