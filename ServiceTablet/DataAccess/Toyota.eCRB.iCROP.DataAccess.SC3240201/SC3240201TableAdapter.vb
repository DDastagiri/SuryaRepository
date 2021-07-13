'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240201TableAdapter.vb
'─────────────────────────────────────
'機能： チップ詳細
'補足： 
'作成： 2013/07/31 TMEJ 岩城 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発
'更新： 2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

''' <summary>
''' チップ詳細
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3240201TableAdapter

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3240201"

    ''' <summary>
    ''' キャンセルフラグ（0：有効）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CANCEL_TYPE_EFFECTIVE As String = "0"

    ''' <summary>
    ''' 使用中フラグ（1：使用中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INUSE_TYPE_USE As String = "1"

    ''' <summary>
    ''' 休憩取得フラグ（0：休憩を取得しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_USE_REST As String = "0"

    ''' <summary>
    ''' 休憩取得フラグ（1：休憩を取得する）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_REST As String = "1"

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ''' <summary>
    ''' 仮置きフラグ（0：仮置きでない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_TEMP As String = "0"
    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    ''' <summary>
    ''' 日付最小値文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_MIN_VALUE As String = "1900/01/01 00:00:00"

    '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' ストール利用ステータス"00":着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_00 As String = "00"

    ''' <summary>
    ''' ストール利用ステータス"01":作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_01 As String = "01"

    ''' <summary>
    ''' ストール利用ステータス"02":作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_02 As String = "02"

    ''' <summary>
    ''' ストール利用ステータス"04":作業指示の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_04 As String = "04"
    '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
#End Region

#Region "Selectメソッド"

    ''' <summary>
    ''' ストール利用IDに紐付くチップ情報を取得する
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChipBaseInfo(ByVal dlrCD As String, ByVal strCD As String, ByVal stallUseId As Decimal) As SC3240201DataSet.SC3240201ChipBaseInfoDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[stallUseId:{0}]", stallUseId)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append(" SELECT /* SC3240201_001 */ ")
            .Append(" SELECT DISTINCT /* SC3240201_001 */ ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("        A.DLR_CD AS DLR_CD ")                                           'サービス入庫.販売店コード
            .Append("      , A.SVCIN_ID AS SVCIN_ID ")                                       'サービス入庫.サービス入庫ID
            .Append("      , A.CST_ID AS CST_ID ")                                           'サービス入庫.顧客ID
            .Append("      , A.VCL_ID AS VCL_ID ")                                           'サービス入庫.車両ID
            .Append("      , D.REG_NUM AS REG_NUM ")                                         '販売店車両.車両登録番号
            .Append("      , E.VCL_VIN AS VCL_VIN ")                                         '車両.VIN
            .Append("      , E.VCL_KATASHIKI AS VCL_KATASHIKI ")                             '車両.車両型式
            .Append("      , NVL(TRIM(F.MODEL_NAME), E.NEWCST_MODEL_NAME) AS MODEL_NAME ")   'モデルマスタ.モデル名称
            .Append("      , G.CST_NAME AS CST_NAME ")                                       '顧客.顧客氏名
            .Append("      , G.CST_MOBILE AS CST_MOBILE ")                                   '顧客.顧客携帯電話番号
            .Append("      , G.CST_PHONE AS CST_PHONE ")                                     '顧客.顧客電話番号
            .Append("      , H.STF_NAME AS STF_NAME ")                                       'スタッフマスタ.スタッフ名称
            .Append("      , A.SCHE_SVCIN_DATETIME AS PLAN_VISITDATE ")                      'サービス入庫.予定入庫日時
            .Append("      , C.SCHE_START_DATETIME AS PLAN_STARTDATE ")                      'ストール利用.予定開始日時
            .Append("      , C.SCHE_END_DATETIME AS PLAN_ENDDATE ")                          'ストール利用.予定終了日時
            .Append("      , A.SCHE_DELI_DATETIME AS PLAN_DELIDATE ")                        'サービス入庫.予定納車日時
            .Append("      , A.RSLT_SVCIN_DATETIME AS RESULT_VISITDATE ")                    'サービス入庫.実績入庫日時
            .Append("      , C.RSLT_START_DATETIME AS RESULT_STARTDATE ")                    'ストール利用.実績開始日時
            .Append("      , C.RSLT_END_DATETIME AS RESULT_ENDDATE ")                        'ストール利用.実績終了日時
            .Append("      , A.RSLT_DELI_DATETIME AS RESULT_DELIDATE ")                      'サービス入庫.実績納車日時
            .Append("      , B.SVC_CLASS_ID AS SVC_CLASS_ID ")                               '作業内容.サービス分類ID
            .Append("      , K.SVCID_TIME AS SVCID_TIME ")                                   '店舗サービス分類.標準作業時間
            .Append("      , K.SVC_CLASS_NAME AS SVC_CLASS_NAME ")                           'サービス分類マスタ.サービス分類名称
            .Append("      , B.MERC_ID AS MERC_ID ")                                         '作業内容.表示商品ID
            .Append("      , N.MERCID_TIME AS MERCID_TIME ")                                 '店舗商品.標準作業時間
            .Append("      , N.MERC_NAME AS MERC_NAME ")                                     '商品マスタ.商品名称
            .Append("      , C.SCHE_WORKTIME AS WORKTIME ")                                  'ストール利用.予定作業時間
            .Append("      , A.ACCEPTANCE_TYPE AS REZFLAG ")                                 'サービス入庫.受付区分（0:予約客／1:Walk-in）
            .Append("      , A.CARWASH_NEED_FLG AS WASHFLAG ")                               'サービス入庫.洗車必要フラグ
            .Append("      , A.PICK_DELI_TYPE AS WAITTYPE ")                                 'サービス入庫.引取納車区分（0:Waiting／4:Drop off）
            .Append("      , C.STALL_USE_ID AS STALL_USE_ID ")                               'ストール利用.ストール利用ID
            .Append("      , A.SVC_STATUS AS SVC_STATUS ")                                   'サービス入庫.サービスステータス
            .Append("      , A.RO_NUM AS RO_NUM ")                                           'サービス入庫.RO番号
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append("      , B.RO_JOB_SEQ AS RO_JOB_SEQ ")                                   '作業内容.作業連番
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("      , B.INSPECTION_STATUS AS INSPECTION_STATUS ")                     '作業内容.完成検査承認フラグ
            .Append("      , C.STALL_USE_STATUS AS STALL_USE_STATUS ")                       'ストール利用.ストール利用ステータス
            .Append("      , A.RESV_STATUS AS RESV_STATUS ")                                 'サービス入庫.予約ステータス
            .Append("      , A.ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")                       'サービス入庫.行ロックバージョン
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , B.INSPECTION_NEED_FLG AS INSPECTIONFLG ")                       '作業内容.完成検査必要フラグ
            .Append("      , G.CST_ADDRESS AS CST_ADDRESS ")                                 '顧客.顧客住所
            .Append("      , G.FLEET_FLG AS FLEET_FLG ")                                     '顧客.法人フラグ(0:個人／1:法人)
            .Append("      , NVL(TRIM(G.DMS_CST_CD), O.DMSID) AS DMS_CST_CD ")               '基幹顧客コード 
            .Append("      , P.CST_TYPE ")                                                   '販売店顧客.顧客種別
            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
            '.Append("      , D.VIP_FLG AS JDP_FLG ")                                         '販売店車両.JDPフラグ
            .Append("      , D.IMP_VCL_FLG AS JDP_FLG ")                                         '販売店車両.JDPフラグ
            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
            .Append("      , A.NEXT_SVCIN_INSPECTION_ADVICE AS NEXT_SVCIN_INSPECTION_ADVICE ") 'サービス入庫.次回入庫点検アドバイス 
            .Append("      , A.ADD_JOB_ADVICE AS ADVICE ")                                   'サービス入庫.アドバイス 
            .Append("      , B.JOB_DTL_MEMO AS JOB_DTL_MEMO ")                               '作業内容.作業内容メモ 
            .Append("      , G.NAMETITLE_NAME AS NAMETITLE_NAME ")                           '顧客.敬称
            .Append("      , Q.POSITION_TYPE AS POSITION_TYPE ")                             '敬称マスタ.配置区分
            .Append("      , B.DMS_JOB_DTL_ID AS DMS_JOB_DTL_ID ")                           '作業内容.基幹作業内容ID
            .Append("      , A.INVOICE_PREP_COMPL_DATETIME AS INVOICE_DATETIME ")            'サービス入庫.清算準備完了日時 
            .Append("      , R.VISIT_ID AS VISIT_ID ")                                       'RO情報.訪問ID 
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            .Append("      , E.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")                        '車両.SSC対象フラグ
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")                    'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")                      '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")                    'ストール利用テーブル
            .Append("      , TB_M_VEHICLE_DLR D ")                  '販売店車両テーブル
            .Append("      , TB_M_VEHICLE E ")                      '車両テーブル
            .Append("      , TB_M_MODEL F ")                        'モデルマスタテーブル
            .Append("      , TB_M_CUSTOMER G ")                     '顧客テーブル
            .Append("      , TB_M_STAFF H ")                        'スタッフマスタテーブル
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , TBL_SERVICE_VISIT_MANAGEMENT O ")      'サービス来店者管理テーブル
            .Append("      , TB_M_CUSTOMER_DLR P ")                 '販売店顧客テーブル
            .Append("      , TB_M_NAMETITLE Q ")                    '敬称マスタテーブル
            .Append("      , (SELECT ")
            .Append("                SVCIN_ID ")
            .Append("              , VISIT_ID ")
            .Append("           FROM ")
            .Append("                TB_T_RO_INFO ")                'RO情報テーブル
            .Append("           WHERE ")
            .Append("                RO_STATUS <> N'99' ")
            .Append("         ) R ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("      , (SELECT ")
            .Append("                J.SVC_CLASS_ID ")
            .Append("              , J.SVC_CLASS_ID || ',' || I.STD_WORKTIME AS SVCID_TIME ")
            .Append("              , NVL(TRIM(J.SVC_CLASS_NAME), J.SVC_CLASS_NAME_ENG) AS SVC_CLASS_NAME ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("              , J.SVC_CLASS_TYPE ")           'サービス分類区分 (1:EM 2:PM 3:GR 4:PDS 5:BP)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("           FROM ")
            .Append("                TB_M_BRANCH_SERVICE_CLASS I ")     '店舗サービス分類テーブル
            .Append("              , TB_M_SERVICE_CLASS J ")            'サービス分類マスタテーブル
            .Append("          WHERE ")
            .Append("                I.SVC_CLASS_ID = J.SVC_CLASS_ID ")
            .Append("            AND I.DLR_CD = :DLR_CD ")
            .Append("            AND I.BRN_CD = :BRN_CD ")
            .Append("            AND J.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("         ) K ")
            .Append("      , (SELECT ")
            .Append("                M.MERC_ID ")
            .Append("              , M.MERC_ID || ',' || L.STD_WORKTIME AS MERCID_TIME ")
            .Append("              , NVL(TRIM(M.MERC_NAME), M.MERC_NAME_ENG) AS MERC_NAME ")
            .Append("           FROM ")
            .Append("                TB_M_BRANCH_MERCHANDISE L ")      '店舗商品テーブル
            .Append("              , TB_M_MERCHANDISE M ")             '商品マスタテーブル
            .Append("          WHERE ")
            .Append("                L.MERC_ID = M.MERC_ID ")
            .Append("            AND L.DLR_CD = :DLR_CD ")
            .Append("            AND L.BRN_CD = :BRN_CD ")
            .Append("            AND M.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("         ) N ")
            .Append("  WHERE ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID  ")
            .Append("    AND A.DLR_CD = D.DLR_CD(+)  ")
            .Append("    AND A.VCL_ID = D.VCL_ID(+)  ")
            .Append("    AND A.VCL_ID = E.VCL_ID(+)  ")
            .Append("    AND E.MODEL_CD = F.MODEL_CD(+) ")
            .Append("    AND A.CST_ID = G.CST_ID(+) ")
            .Append("    AND A.PIC_SA_STF_CD = H.STF_CD(+) ")
            .Append("    AND B.SVC_CLASS_ID = K.SVC_CLASS_ID(+) ")
            .Append("    AND B.MERC_ID = N.MERC_ID(+) ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("    AND A.SVCIN_ID = O.FREZID(+) ")
            .Append("    AND A.CST_ID = P.CST_ID(+) ")
            .Append("    AND A.DLR_CD = P.DLR_CD(+) ")
            .Append("    AND G.NAMETITLE_CD = Q.NAMETITLE_CD(+) ")
            .Append("    AND A.SVCIN_ID = R.SVCIN_ID(+) ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("    AND C.STALL_USE_ID = :STALL_USE_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201ChipBaseInfoDataTable)("SC3240201_001")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE)         '使用中フラグ
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Long, stallUseId)                 'ストール利用ID
            query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)              'ストール利用ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201ChipBaseInfoDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' サービス入庫IDに紐付くチップ情報を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChipBaseInfoBySvcInId(ByVal svcInId As Decimal, ByVal roNum As String) As SC3240201DataSet.SC3240201ChipBaseInfoDataTable

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}]", svcInId)
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][roNum:{1}]", svcInId, roNum)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append(" SELECT /* SC3240201_002 */ ")
            .Append(" SELECT DISTINCT /* SC3240201_002 */ ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("        A.DLR_CD AS DLR_CD ")                                            'サービス入庫.販売店コード
            .Append("      , A.SVCIN_ID AS SVCIN_ID ")                                        'サービス入庫.サービス入庫ID
            .Append("      , A.CST_ID AS CST_ID ")                                            'サービス入庫.顧客ID
            .Append("      , A.VCL_ID AS VCL_ID ")                                            'サービス入庫.車両ID
            .Append("      , D.REG_NUM AS REG_NUM ")                                          '販売店車両.車両登録番号
            .Append("      , E.VCL_VIN AS VCL_VIN ")                                          '車両.VIN
            .Append("      , E.VCL_KATASHIKI AS VCL_KATASHIKI ")                              '車両.車両型式
            .Append("      , NVL(TRIM(F.MODEL_NAME), E.NEWCST_MODEL_NAME) AS MODEL_NAME ")    'モデルマスタ.モデル名称
            .Append("      , G.CST_NAME AS CST_NAME ")                                        '顧客.顧客氏名
            .Append("      , G.CST_MOBILE AS CST_MOBILE ")                                    '顧客.顧客携帯電話番号
            .Append("      , G.CST_PHONE AS CST_PHONE ")                                      '顧客.顧客電話番号
            .Append("      , H.STF_NAME AS STF_NAME ")                                        'スタッフマスタ.スタッフ名称
            .Append("      , A.SCHE_SVCIN_DATETIME AS PLAN_VISITDATE ")                       'サービス入庫.予定入庫日時
            .Append("      , A.SCHE_DELI_DATETIME AS PLAN_DELIDATE ")                         'サービス入庫.予定納車日時
            .Append("      , A.RSLT_SVCIN_DATETIME AS RESULT_VISITDATE ")                     'サービス入庫.実績入庫日時
            .Append("      , A.RSLT_DELI_DATETIME AS RESULT_DELIDATE ")                       'サービス入庫.実績納車日時
            .Append("      , A.ACCEPTANCE_TYPE AS REZFLAG ")                                  'サービス入庫.受付区分（0:予約客／1:Walk-in）
            .Append("      , A.CARWASH_NEED_FLG AS WASHFLAG ")                                'サービス入庫.洗車必要フラグ
            .Append("      , A.PICK_DELI_TYPE AS WAITTYPE ")                                  'サービス入庫.引取納車区分（0:Waiting／4:Drop off）
            .Append("      , A.SVC_STATUS AS SVC_STATUS ")                                    'サービス入庫.サービスステータス
            .Append("      , A.RO_NUM AS RO_NUM ")                                            'サービス入庫.RO番号
            .Append("      , A.RESV_STATUS AS RESV_STATUS ")                                  'サービス入庫.予約ステータス
            .Append("      , A.ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")                        'サービス入庫.行ロックバージョン
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , K.INSPECTION_NEED_FLG AS INSPECTIONFLG ")                        '作業内容.完成検査必要フラグ
            .Append("      , G.CST_ADDRESS AS CST_ADDRESS ")                                  '顧客.顧客住所
            .Append("      , G.FLEET_FLG AS FLEET_FLG ")                                      '顧客.法人フラグ(0:個人／1:法人)
            .Append("      , NVL(TRIM(G.DMS_CST_CD), O.DMSID) AS DMS_CST_CD ")                '基幹顧客コード 
            .Append("      , P.CST_TYPE ")                                                    '顧客種別
            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
            '.Append("      , D.VIP_FLG AS JDP_FLG ")                                         '販売店車両.JDPフラグ
            .Append("      , D.IMP_VCL_FLG AS JDP_FLG ")                                         '販売店車両.JDPフラグ
            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END
            .Append("      , A.NEXT_SVCIN_INSPECTION_ADVICE AS NEXT_SVCIN_INSPECTION_ADVICE ") 'サービス入庫.次回入庫点検アドバイス 
            .Append("      , A.ADD_JOB_ADVICE AS ADVICE ")                                    'サービス入庫.アドバイス 
            .Append("      , K.JOB_DTL_MEMO AS JOB_DTL_MEMO ")                                '作業内容.作業内容メモ 
            .Append("      , G.NAMETITLE_NAME AS NAMETITLE_NAME ")                            '顧客.敬称
            .Append("      , Q.POSITION_TYPE AS POSITION_TYPE ")                              '敬称マスタ.配置区分
            .Append("      , K.DMS_JOB_DTL_ID AS DMS_JOB_DTL_ID ")                            '作業内容.基幹作業内容ID
            .Append("      , A.INVOICE_PREP_COMPL_DATETIME AS INVOICE_DATETIME ")             'サービス入庫.清算準備完了日時 
            .Append("      , R.VISIT_ID AS VISIT_ID ")                                        'RO情報.訪問ID 
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
            .Append("      , E.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")                         '車両.SSC対象フラグ
            '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("   FROM  ")
            .Append("        TB_T_SERVICEIN A ")                     'サービス入庫テーブル
            .Append("      , TB_M_VEHICLE_DLR D ")                   '販売店車両テーブル
            .Append("      , TB_M_VEHICLE E ")                       '車両テーブル
            .Append("      , TB_M_MODEL F ")                         'モデルマスタテーブル
            .Append("      , TB_M_CUSTOMER G ")                      '顧客テーブル
            .Append("      , TB_M_STAFF H ")                         'スタッフマスタテーブル
            .Append("      , TB_M_NAMETITLE Q ")                     '敬称マスタテーブル
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , (SELECT ")
            .Append("                SVCIN_ID ")
            .Append("              , VISIT_ID ")
            .Append("           FROM ")
            .Append("                TB_T_RO_INFO ")                'RO情報テーブル
            .Append("           WHERE ")
            .Append("                RO_STATUS <> N'99' ")
            .Append("         ) R ")
            .Append("      , TBL_SERVICE_VISIT_MANAGEMENT O ")       'サービス来店者管理
            .Append("      , TB_M_CUSTOMER_DLR P ")                  '販売店顧客
            .Append("      , ( SELECT ")
            .Append("                B.INSPECTION_NEED_FLG ")                                 '完成検査必要フラグ
            .Append("              , B.JOB_DTL_MEMO ")                                        '作業内容メモ
            .Append("              , B.DMS_JOB_DTL_ID ")                                      '基幹作業内容ID
            .Append("          FROM ")
            .Append("                TB_T_JOB_DTL B ")                                        '作業内容テーブル
            .Append("          WHERE ")
            .Append("                B.JOB_DTL_ID = ( SELECT MIN(J.JOB_DTL_ID) ")
            .Append("                                   FROM ")
            .Append("                                 ( SELECT ")
            .Append("                                         C.JOB_DTL_ID ")                 '作業内容ID
            .Append("                                   FROM ")
            .Append("                                         TB_T_JOB_DTL C ")               '作業内容テーブル
            .Append("                                        ,TB_T_SERVICEIN I ")             'サービス入庫テーブル
            .Append("                                   WHERE ")
            .Append("                                         C.SVCIN_ID = I.SVCIN_ID ")      'サービス入庫ID
            .Append("                                     AND C.CANCEL_FLG = N'0' ")          'キャンセルフラグ (0:有効 1:キャンセル)
            .Append("                                     AND C.SVCIN_ID = :SVCIN_ID ")
            .Append("                                     AND I.RO_NUM = :RO_NUM ")           'RO番号
            .Append("                                  ) J ")
            .Append("                                ) ")
            .Append("        ) K ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("  WHERE ")
            .Append("        A.DLR_CD = D.DLR_CD(+) ")
            .Append("    AND A.VCL_ID = D.VCL_ID(+) ")
            .Append("    AND A.VCL_ID = E.VCL_ID(+) ")
            .Append("    AND E.MODEL_CD = F.MODEL_CD(+) ")
            .Append("    AND A.CST_ID = G.CST_ID(+) ")
            .Append("    AND A.PIC_SA_STF_CD = H.STF_CD(+) ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("    AND A.SVCIN_ID = O.FREZID(+) ")
            .Append("    AND A.CST_ID = P.CST_ID(+) ")
            .Append("    AND A.DLR_CD = P.DLR_CD(+) ")
            .Append("    AND G.NAMETITLE_CD = Q.NAMETITLE_CD(+) ")
            .Append("    AND A.SVCIN_ID = R.SVCIN_ID(+) ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201ChipBaseInfoDataTable)("SC3240201_002")
            query.CommandText = sql.ToString()

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)      'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)   'サービス入庫ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Varchar2, roNum)

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201ChipBaseInfoDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' サービス入庫IDに紐付く来店情報を取得する
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVisitInfo(ByVal dlrCD As String, ByVal strCD As String, ByVal svcInId As Decimal) As SC3240201DataSet.SC3240201VisitInfoDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[dlrCD:{0}][strCD:{1}][svcInId:{2}]", dlrCD, strCD, svcInId)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append(" SELECT /* SC3240201_003 */ ")
            '.Append("        VISITSEQ ")                         '来店実績連番
            '.Append("      , VCLREGNO ")                         '車両登録番号
            '.Append("      , VIN ")                              'VIN
            '.Append("      , TELNO ")                            '電話番号
            '.Append("      , MOBILE ")                           '携帯番号
            '.Append("      , ASSIGNSTATUS  ")                    '振当ステータス
            '.Append("   FROM ")
            '.Append("        TBL_SERVICE_VISIT_MANAGEMENT ")     'サービス来店者管理
            '.Append("  WHERE ")
            '.Append("        DLRCD = :DLRCD ")
            '.Append("    AND STRCD = :STRCD ")
            '.Append("    AND FREZID = :SVCINID ")
            '.Append("  ORDER BY ")
            '.Append("        UPDATEDATE DESC ")
            '.Append("      , VISITSEQ DESC ")
            .Append(" SELECT /* SC3240201_003 */ ")
            .Append("        A.VISITSEQ ")                         '来店実績連番
            .Append("      , A.VCLREGNO ")                         '車両登録番号
            .Append("      , A.VIN ")                              'VIN
            .Append("      , A.TELNO ")                            '電話番号
            .Append("      , A.MOBILE ")                           '携帯番号
            .Append("      , A.ASSIGNSTATUS  ")                    '振当ステータス
            .Append("      , B.RO_RELATION_ID  ")
            .Append("   FROM ")
            .Append("        TBL_SERVICE_VISIT_MANAGEMENT A ")     'サービス来店者管理
            .Append("      , (SELECT ")
            .Append("                RO_RELATION_ID ")
            .Append("              , VISIT_ID ")
            .Append("           FROM ")
            .Append("                TB_T_RO_INFO ")                'RO情報テーブル
            .Append("           WHERE ")
            .Append("                RO_STATUS <> N'99' ")
            .Append("         ) B ")
            .Append("  WHERE ")
            .Append("        A.VISITSEQ = B.VISIT_ID(+) ")
            .Append("    AND A.DLRCD = :DLRCD ")
            .Append("    AND A.STRCD = :STRCD ")
            .Append("    AND A.FREZID = :SVCINID ")
            .Append("  ORDER BY ")
            .Append("        A.UPDATEDATE DESC ")
            .Append("      , A.VISITSEQ DESC ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201VisitInfoDataTable)("SC3240201_003")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)       '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strCD)       '店舗コード
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCINID", OracleDbType.Int64, svcInId)       'サービス入庫ID
            query.AddParameterWithTypeValue("SVCINID", OracleDbType.Decimal, svcInId)     'サービス入庫ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201VisitInfoDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' 販売店コード・店舗コードに紐付く整備種類情報を取得する
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSvcClassList(ByVal dlrCD As String, ByVal strCD As String) As SC3240201DataSet.SC3240201SvcClassListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[dlrCD:{0}][strCD:{1}]", dlrCD, strCD)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_004 */ ")
            .Append("        B.SVC_CLASS_ID || ',' || A.STD_WORKTIME AS SVCID_TIME ")                'サービス分類ID,標準作業時間
            .Append("      , NVL(TRIM(B.SVC_CLASS_NAME), B.SVC_CLASS_NAME_ENG) AS SVC_CLASS_NAME ")  'サービス分類名称
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , B.SVC_CLASS_TYPE ")                                                     'サービス分類区分 (1:EM 2:PM 3:GR 4:PDS 5:BP)
            .Append("      , A.CARWASH_NEED_FLG ")                                                   '洗車必要フラグ (1:EM 2:PM 3:GR 4:PDS 5:BP)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("   FROM ")
            .Append("        TB_M_BRANCH_SERVICE_CLASS A ")     '店舗サービス分類
            .Append("      , TB_M_SERVICE_CLASS B ")            'サービス分類マスタ
            .Append("  WHERE ")
            .Append("        A.SVC_CLASS_ID = B.SVC_CLASS_ID ")
            .Append("    AND A.DLR_CD = :DLR_CD ")
            .Append("    AND A.BRN_CD = :BRN_CD ")
            .Append("    AND B.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("  ORDER BY ")
            .Append("        A.SORT_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201SvcClassListDataTable)("SC3240201_004")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)               '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)               '店舗コード
            query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE) '使用中フラグ

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201SvcClassListDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' サービス分類IDを条件にサービス分類コードを取得する
    ''' </summary>
    ''' <param name="svcClassId">サービス分類ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSvcClassCD(ByVal svcClassId As Decimal) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcClassId:{0}]", svcClassId)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_005 */ ")
            .Append("        SVC_CLASS_CD ")                    'サービス分類コード
            .Append("   FROM ")
            .Append("        TB_M_SERVICE_CLASS ")              'サービス分類マスタ
            .Append("  WHERE ")
            .Append("        SVC_CLASS_ID = :SVC_CLASS_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201SvcClassCDDataTable)("SC3240201_005")
            query.CommandText = sql.ToString()

            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Long, svcClassId)      'サービス分類ID
            query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Decimal, svcClassId)           'サービス分類ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim dt As SC3240201DataSet.SC3240201SvcClassCDDataTable = query.GetData()

            '戻り値
            Dim retValue As String = String.Empty

            If 0 < dt.Count Then
                'サービス分類コードを取得
                retValue = dt.Rows(0).Item(0).ToString()
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[retValue:{0}]", retValue)

            Return retValue

        End Using

    End Function

    ''' <summary>
    ''' 販売店コード・店舗コード・サービス分類IDに紐付く商品情報を取得する
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="svcClassId">サービス分類ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMercList(ByVal dlrCD As String, ByVal strCD As String, ByVal svcClassId As Decimal) As SC3240201DataSet.SC3240201MercListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[dlrCD:{0}][strCD:{1}][svcClassId:{2}]", dlrCD, strCD, svcClassId)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_006 */ ")
            .Append("        B.MERC_ID || ',' || A.STD_WORKTIME AS MERCID_TIME ")       '商品ID,標準作業時間
            .Append("      , NVL(TRIM(B.MERC_NAME), B.MERC_NAME_ENG) AS MERC_NAME ")    '商品名称
            .Append("   FROM ")
            .Append("        TB_M_BRANCH_MERCHANDISE A ")        '店舗商品
            .Append("      , TB_M_MERCHANDISE B ")               '商品マスタ
            .Append("  WHERE ")
            .Append("        A.MERC_ID = B.MERC_ID ")
            .Append("    AND A.DLR_CD = :DLR_CD ")
            .Append("    AND A.BRN_CD = :BRN_CD ")
            .Append("    AND B.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("    AND B.SVC_CLASS_ID = :SVC_CLASS_ID")
            .Append("  ORDER BY ")
            .Append("        A.SORT_ORDER ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201MercListDataTable)("SC3240201_006")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)               '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)               '店舗コード
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Long, svcClassId)         'サービス分類ID
            query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Decimal, svcClassId)      'サービス分類ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE) '使用中フラグ

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201MercListDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' サービス入庫IDに紐付くリレーションチップ情報(自分自身を含む)を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="roJobSeq">RO作業連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRelatedChipInfo(ByVal svcInId As Decimal, ByVal dlrCD As String, ByVal strCD As String, ByVal roJobSeq As Long) As SC3240201DataSet.SC3240201RelatedChipInfoDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][dlrCD:{1}][strCD:{2}][roJobSeq:{3}]", svcInId, dlrCD, strCD, roJobSeq)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append(" SELECT /* SC3240201_007 */ ")
            .Append(" SELECT DISTINCT /* SC3240201_007 */ ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("        A.SVCIN_ID ")                               'サービス入庫ID
            .Append("      , B.JOB_DTL_ID ")                             '作業内容ID
            .Append("      , C.STALL_USE_ID ")                           'ストール利用ID
            .Append("      , NVL(TRIM(D.STALL_NAME_SHORT), SUBSTR(D.STALL_NAME, 1, 3)) AS STALL_NAME_SHORT ")  'ストール短縮名称
            .Append("      , C.SCHE_START_DATETIME AS PLAN_STARTDATE ")  '予定開始日時
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append("      , B.RO_JOB_SEQ ")                             '作業連番
            .Append("      , MAX( CASE  ")
            .Append("             WHEN  ")
            .Append("                 E.STARTWORK_INSTRUCT_FLG = '1' AND E.RO_SEQ <> :RO_SEQ  ")
            .Append("                 THEN '1' ")
            .Append("                 ELSE '0' ")
            .Append("             END ) AS INVISIBLE_INSTRUCT_FLG ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , C.PARTS_FLG ")                              '部品準備完了フラグ
            .Append("      , C.STALL_USE_STATUS ")                       'ストール利用ステータス
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            .Append("      , TB_M_STALL D ")            'ストールマスタテーブル
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , TB_T_JOB_INSTRUCT E ")     '作業指示テーブル
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("    AND C.STALL_USE_ID = ")
            .Append("        (SELECT  ")
            .Append("                MAX(E.STALL_USE_ID) ")
            .Append("           FROM  ")
            .Append("                TB_T_STALL_USE E ")
            .Append("          WHERE  ")
            .Append("                E.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("        ) ")
            .Append("    AND C.STALL_ID = D.STALL_ID ")
            .Append("    AND C.DLR_CD = D.DLR_CD ")
            .Append("    AND C.BRN_CD = D.BRN_CD ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("    AND B.JOB_DTL_ID = E.JOB_DTL_ID(+) ")
            .Append("  GROUP BY ")
            .Append("        A.SVCIN_ID ")
            .Append("      , B.JOB_DTL_ID ")
            .Append("      , C.STALL_USE_ID ")
            .Append("      , NVL(TRIM(D.STALL_NAME_SHORT), SUBSTR(D.STALL_NAME, 1, 3)) ")
            .Append("      , C.SCHE_START_DATETIME ")
            .Append("      , C.PARTS_FLG ")
            .Append("      , C.STALL_USE_STATUS ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("  ORDER BY ")
            .Append("        C.STALL_USE_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RelatedChipInfoDataTable)("SC3240201_007")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)                     'サービス入庫ID
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int64, roJobSeq)                        'RO作業連番
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' リレーションを含めたチップの作業予定日時が入力した来店予定(または実績)日時から納車予定日時の間に収まっているかチェック
    '''   ※自チップはチェック対象から省く（自チップはクライアント側でチェックしている）
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="planVisit">チップ詳細で入力した来店予定日時</param>
    ''' <param name="planDeli">チップ詳細で入力した納車予定日時</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOverSrvDateTimeChipCount(ByVal svcInId As Decimal, ByVal planVisit As String, ByVal planDeli As String, ByVal dlrCD As String, ByVal strCD As String, ByVal stallUseId As Decimal) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[svcInId:{0}][planVisit:{1}][planDeli:{2}][dlrCD:{3}][strCD:{4}][stallUseId:{5}]", _
                      svcInId, planVisit, planDeli, dlrCD, strCD, stallUseId)

        Dim count As Integer = 0

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_008 */ ")
            .Append("        B.JOB_DTL_ID ")            '作業内容ID
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("    AND C.STALL_USE_ID = ")
            .Append("        (SELECT  ")
            .Append("             MAX(E.STALL_USE_ID) ")
            .Append("         FROM  ")
            .Append("             TB_T_STALL_USE E ")
            .Append("         WHERE  ")
            .Append("             E.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("        ) ")
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND ((CASE WHEN C.RSLT_START_DATETIME = :MINDATE ")
            .Append("               THEN C.SCHE_START_DATETIME ")
            .Append("               ELSE C.RSLT_START_DATETIME END ")
            .Append("           < ")
            .Append("          CASE WHEN A.RSLT_SVCIN_DATETIME = :MINDATE ")
            .Append("               THEN :PLAN_VISITDATE ")
            .Append("               ELSE A.RSLT_SVCIN_DATETIME END) ")
            .Append("          OR ")
            .Append("         (:PLAN_DELIDATE ")
            .Append("           <  ")
            .Append("          CASE WHEN C.RSLT_END_DATETIME = :MINDATE ")
            .Append("               THEN C.SCHE_END_DATETIME ")
            .Append("               ELSE C.RSLT_END_DATETIME END) ")
            .Append("        ) ")
            .Append("    AND C.STALL_USE_ID <> :STALL_USE_ID ")
            .Append("    AND ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RowCount1DataTable)("SC3240201_008")
            query.CommandText = sql.ToString()

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)                        'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)                     'サービス入庫ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, svcInId)                   'ストール利用ID
            query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, svcInId)                 'ストール利用ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture))      '最小日付

            '来店予定日時
            If String.IsNullOrEmpty(planVisit) Then
                query.AddParameterWithTypeValue("PLAN_VISITDATE", OracleDbType.Date, Date.MinValue)
            Else
                query.AddParameterWithTypeValue("PLAN_VISITDATE", OracleDbType.Date, Date.Parse(planVisit, CultureInfo.InvariantCulture))
            End If

            '納車予定日時
            If String.IsNullOrEmpty(planDeli) Then
                query.AddParameterWithTypeValue("PLAN_DELIDATE", OracleDbType.Date, Date.MaxValue)
            Else
                query.AddParameterWithTypeValue("PLAN_DELIDATE", OracleDbType.Date, Date.Parse(planDeli, CultureInfo.InvariantCulture))
            End If

            'SQL実行
            Dim dt As SC3240201DataSet.SC3240201RowCount1DataTable = query.GetData()

            count = dt.Rows.Count

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[count:{0}]", count)

            Return count

        End Using

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' サービス入庫IDに紐付く、作業開始済みの予約ID（作業内容ID）リストを取得
    ' '''　 →(登録後、整備に紐付いていなければならない予約ID)リストを取得
    ' ''' </summary>
    ' ''' <param name="svcInId">サービス入庫ID</param>
    ' ''' <param name="roJobSeq">作業連番</param>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="strCD">店舗コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetStartedRezIdList(ByVal svcInId As Decimal, ByVal roJobSeq As Long, ByVal dlrCD As String, ByVal strCD As String) As SC3240201DataSet.SC3240201RezIdListDataTable
    'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][roJobSeq:{1}][dlrCD:{2}][strCD:{3}]", svcInId, roJobSeq, dlrCD, strCD)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' サービス入庫IDに紐付く、作業開始済みの予約ID（作業内容ID）リストを取得
    '''　 →(登録後、整備に紐付いていなければならない予約ID)リストを取得
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStartedRezIdList(ByVal svcInId As Decimal, ByVal dlrCD As String, ByVal strCD As String) As SC3240201DataSet.SC3240201RezIdListDataTable
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][dlrCD:{1}][strCD:{2}]", svcInId, dlrCD, strCD)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_009 */ ")
            .Append("        B.JOB_DTL_ID AS REZID ")   '作業内容ID
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("    AND C.STALL_USE_ID = ")
            .Append("        (SELECT  ")
            .Append("             MAX(E.STALL_USE_ID) ")
            .Append("         FROM  ")
            .Append("             TB_T_STALL_USE E ")
            .Append("         WHERE  ")
            .Append("             E.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("        ) ")
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND C.RSLT_START_DATETIME <> :MINDATE ")    '実績開始あり

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'If roJobSeq <> -1 Then
            '    .Append(" AND B.RO_JOB_SEQ = :RO_JOB_SEQ ")       '作業連番
            'End If
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            .Append("  ORDER BY ")
            .Append("        B.JOB_DTL_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RezIdListDataTable)("SC3240201_009")
            query.CommandText = sql.ToString()

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)                        'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)                     'サービス入庫ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture))      '最小日付

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'If roJobSeq <> -1 Then
            '    query.AddParameterWithTypeValue("RO_JOB_SEQ", OracleDbType.Long, roJobSeq)                 '作業連番
            'End If
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201RezIdListDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' リレーションを含めた全チップについて、１つでも実績開始日時が入っているかチェック
    ' ''' </summary>
    ' ''' <param name="svcInId">サービス入庫ID</param>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="strCD">店舗コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetProcChipCount(ByVal svcInId As Decimal, ByVal dlrCD As String, ByVal strCD As String) As Integer

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][dlrCD:{1}][strCD:{2}]", svcInId, dlrCD, strCD)

    ''' <summary>
    ''' リレーションを含めた全チップについて、１つでも実績開始日時が入っているかチェック
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcChipCount(ByVal svcInId As Decimal, ByVal dlrCD As String, ByVal strCD As String, ByVal stallUseId As Decimal) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][dlrCD:{1}][strCD:{2}][stallUseId:{3}]", svcInId, dlrCD, strCD, stallUseId)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        Dim count As Integer = 0

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_010 */ ")
            .Append("        B.JOB_DTL_ID ")            '作業内容ID
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '.Append("    AND C.STALL_USE_ID = ")
            .Append("    AND ( C.STALL_USE_ID = ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("        (SELECT  ")
            .Append("             MAX(E.STALL_USE_ID) ")
            .Append("         FROM  ")
            .Append("             TB_T_STALL_USE E ")
            .Append("         WHERE  ")
            .Append("             E.JOB_DTL_ID = C.JOB_DTL_ID ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("        ) ")
            .Append("         OR ")
            .Append("             C.STALL_USE_ID = :STALL_USE_ID ")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("        ) ")
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND C.RSLT_START_DATETIME <> :MINDATE ")    '実績開始あり
            .Append("    AND ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RowCount1DataTable)("SC3240201_010")
            query.CommandText = sql.ToString()

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)                        'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)                     'サービス入庫ID
            query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, stallUseId)              'ストール利用ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture))      '最小日付

            'SQL実行
            Dim dt As SC3240201DataSet.SC3240201RowCount1DataTable = query.GetData()

            count = dt.Rows.Count

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[count:{0}]", count)

            Return count

        End Using

    End Function

    ''' <summary>
    ''' 実績開始以降で、既に存在する実績チップを確認
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="procStart">チップ詳細で入力した作業開始実績日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProcChipCollisionCount(ByVal dlrCD As String, ByVal strCD As String, ByVal stallId As Decimal, ByVal procStart As String) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[dlrCD:{0}][strCD:{1}][stallId:{2}][procStart:{3}]", _
                      dlrCD, strCD, stallId, procStart)

        Dim count As Integer = 0

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_011 */ ")
            .Append("        B.JOB_DTL_ID ")            '作業内容ID
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("    AND C.STALL_ID = :STALL_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND C.RSLT_END_DATETIME > :PROCSTART ")
            .Append("    AND ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RowCount1DataTable)("SC3240201_011")
            query.CommandText = sql.ToString()

            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Long, stallId)                        'ストールID
            query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)                     'ストールID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("PROCSTART", OracleDbType.Date, Date.Parse(procStart, CultureInfo.InvariantCulture))      '作業開始実績日時

            'SQL実行
            Dim dt As SC3240201DataSet.SC3240201RowCount1DataTable = query.GetData()

            count = dt.Rows.Count

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[count:{0}]", count)

            Return count

        End Using

    End Function

    ''' <summary>
    ''' 作業内容IDリストを元に、該当チップのストールIDリストを取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlIdList">作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallIdListByJobDtlId(ByVal dlrCD As String, _
                                               ByVal strCD As String, _
                                               ByVal svcInId As Decimal, _
                                               ByVal jobDtlIdList As Dictionary(Of Decimal, String)) As SC3240201DataSet.SC3240201PushStallIdDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[dlrCD:{0}][strCD:{1}][svcInId:{2}]", _
                      dlrCD, strCD, svcInId)

        '作業内容IDを「jobDtlId1, jobDtlId2, …jobDtlIdN」の文字列に変換する
        Dim sbJobDtlId As New StringBuilder
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'For Each jobDtlId As String In jobDtlIdList
        '    sbJobDtlId.Append(jobDtlId)
        '    sbJobDtlId.Append(",")
        'Next
        'チップエリアに表示されている予約IDリストの件数分Loop
        For i = 0 To jobDtlIdList.Count - 1
            sbJobDtlId.Append(jobDtlIdList.Keys(i))
            sbJobDtlId.Append(",")

        Next
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        Dim strJobDtlIdList As String = sbJobDtlId.ToString()

        '最後のコンマを削除する
        strJobDtlIdList = strJobDtlIdList.Substring(0, strJobDtlIdList.Length - 1)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[strJobDtlIdList:{0}]", strJobDtlIdList)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3240201_012 */ ")
            .Append("        C.STALL_ID ")              'ストールID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , C.JOB_DTL_ID ")            '作業内容ID
            .Append("      , C.SCHE_START_DATETIME ")   '予定開始日時
            .Append("      , C.SCHE_END_DATETIME ")     '予定終了日時
            .Append("      , B.DMS_JOB_DTL_ID ")        '基幹作業内容ID
            .Append("      , F.SVC_CLASS_NAME ")        'サービス分類名称
            .Append("      , I.MERC_NAME ")             '商品名称
            .Append("      , I.UPPER_DISP ")            '商品マーク上部表示文字列
            .Append("      , I.LOWER_DISP ")            '商品マーク下部表示文字列
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("   FROM ")
            .Append("        TB_T_SERVICEIN A ")        'サービス入庫テーブル
            .Append("      , TB_T_JOB_DTL B ")          '作業内容テーブル
            .Append("      , TB_T_STALL_USE C ")        'ストール利用テーブル
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , (SELECT ")
            .Append("                E.SVC_CLASS_ID ")
            .Append("              , NVL(TRIM(E.SVC_CLASS_NAME), E.SVC_CLASS_NAME_ENG) AS SVC_CLASS_NAME ")
            .Append("           FROM ")
            .Append("                TB_M_BRANCH_SERVICE_CLASS D ")     '店舗サービス分類テーブル
            .Append("              , TB_M_SERVICE_CLASS E ")            'サービス分類マスタテーブル
            .Append("          WHERE ")
            .Append("                D.SVC_CLASS_ID = E.SVC_CLASS_ID ")
            .Append("            AND D.DLR_CD = :DLR_CD ")
            .Append("            AND D.BRN_CD = :BRN_CD ")
            .Append("            AND E.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("         ) F ")
            .Append("      , (SELECT ")
            .Append("                H.MERC_ID ")
            .Append("              , H.UPPER_DISP ")
            .Append("              , H.LOWER_DISP ")
            .Append("              , NVL(TRIM(H.MERC_NAME), H.MERC_NAME_ENG) AS MERC_NAME ")
            .Append("           FROM ")
            .Append("                TB_M_BRANCH_MERCHANDISE G ")       '店舗商品テーブル
            .Append("              , TB_M_MERCHANDISE H ")              '商品マスタテーブル
            .Append("          WHERE ")
            .Append("                G.MERC_ID = H.MERC_ID ")
            .Append("            AND G.DLR_CD = :DLR_CD ")
            .Append("            AND G.BRN_CD = :BRN_CD ")
            .Append("            AND H.INUSE_FLG = :INUSE_FLG_1 ")
            .Append("         ) I ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("  WHERE  ")
            .Append("        A.SVCIN_ID = B.SVCIN_ID ")
            .Append("    AND B.JOB_DTL_ID = C.JOB_DTL_ID ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("    AND B.SVC_CLASS_ID = F.SVC_CLASS_ID(+) ")
            .Append("    AND B.MERC_ID = I.MERC_ID(+) ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("    AND C.STALL_USE_ID = ")
            .Append("        (SELECT  ")
            .Append("             MAX(E.STALL_USE_ID) ")
            .Append("         FROM  ")
            .Append("             TB_T_STALL_USE E ")
            .Append("         WHERE  ")
            .Append("             E.JOB_DTL_ID = C.JOB_DTL_ID ")
            .Append("        ) ")
            .Append("    AND A.SVCIN_ID = :SVCIN_ID ")
            .Append("    AND B.CANCEL_FLG = :CANCEL_FLG_0 ")
            .Append("    AND C.DLR_CD = :DLR_CD ")
            .Append("    AND C.BRN_CD = :BRN_CD ")
            .Append("    AND B.JOB_DTL_ID IN ( ")
            .Append(strJobDtlIdList)
            .Append("                        ) ")
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            .Append("    AND C.TEMP_FLG = :TEMP_FLG_OFF ")
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
            .Append("GROUP BY C.STALL_ID ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("       , C.JOB_DTL_ID ")
            .Append("       , C.SCHE_START_DATETIME ")
            .Append("       , C.SCHE_END_DATETIME ")
            .Append("       , B.DMS_JOB_DTL_ID ")
            .Append("       , F.SVC_CLASS_NAME ")
            .Append("       , I.MERC_NAME ")
            .Append("       , I.UPPER_DISP ")
            .Append("       , I.LOWER_DISP ")
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201PushStallIdDataTable)("SC3240201_012")
            query.CommandText = sql.ToString()

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, svcInId)                        'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)                     'サービス入庫ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                       '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)                       '店舗コード
            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_TYPE_EFFECTIVE) 'キャンセルフラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE)         '使用中フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            query.AddParameterWithTypeValue("TEMP_FLG_OFF", OracleDbType.NVarchar2, NOT_TEMP) '仮置きフラグ
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201PushStallIdDataTable = query.GetData()

            ' ''SQL実行結果をリストで返却
            ''Dim returnList As New List(Of Long)
            ''For Each drQuery As SC3240201DataSet.SC3240201PushStallIdRow In dt
            ''    returnList.Add(drQuery.STALLID)
            ''Next

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

    ''2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' RO番号に紐づくRO作業連番を全て取得する
    ' ''' </summary>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetRONumInfo(ByVal roNum As String) As SC3240201DataSet.SC3240201RONumInfoDataTable

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
    '                  "[roNum:{0}]", _
    '                  roNum)

    '    'SQL組み立て
    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append("    SELECT /* SC3240201_013 */ ")
    '        .Append("           RO_NUM AS R_O ")                  'RO番号
    '        .Append("         , RO_SEQ AS R_O_SEQNO ")            'RO作業連番
    '        .Append("      FROM ")
    '        .Append("           TB_T_RO_INFO ")                   'RO情報
    '        .Append("     WHERE  ")
    '        .Append("           RO_STATUS <> N'99' ")
    '        .Append("     AND   RO_NUM = :RO_NUM ")
    '        .Append("  ORDER BY RO_SEQ ASC ")
    '    End With

    '    Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RONumInfoDataTable)("SC3240201_013")
    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)      'RO番号

    '        'SQL実行
    '        Dim dt As SC3240201DataSet.SC3240201RONumInfoDataTable = query.GetData()

    '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[count:{0}]", dt.Rows.Count)

    '        Return dt

    '    End Using

    'End Function
    ''2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' RO番号に紐づくRO作業連番を全て取得する
    ''' </summary>
    ''' <param name="inRepairOrderNo">RO番号</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRONumInfo(ByVal inRepairOrderNo As String, _
                                 ByVal inDealerCode As String, _
                                 ByVal inBranchCode As String) As SC3240201DataSet.SC3240201RONumInfoDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, _
                      True, _
                      "[inRepairOrderNo:{0}][inDealerCode:{1}][inBranchCode:{2}]", _
                      inRepairOrderNo, _
                      inDealerCode, _
                      inBranchCode)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3240201_013 */ ")
            .Append("          RO_NUM AS R_O ")                  'RO番号
            .Append("        , RO_SEQ AS R_O_SEQNO ")            'RO作業連番
            .Append("     FROM ")
            .Append("          TB_T_RO_INFO ")                   'RO情報
            .Append("    WHERE  ")
            .Append("          RO_STATUS <> N'99' ")
            .Append("      AND RO_NUM = :RO_NUM ")
            .Append("      AND DLR_CD = :DLR_CD ")
            .Append("      AND BRN_CD = :BRN_CD ")
            .Append(" ORDER BY RO_SEQ ASC ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201RONumInfoDataTable)("SC3240201_013")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRepairOrderNo)   'RO番号
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)      '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)      '店舗コード

            'SQL実行
            Dim dt As SC3240201DataSet.SC3240201RONumInfoDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[count:{0}]", dt.Rows.Count)

            Return dt

        End Using

    End Function

    '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

    '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

    ''2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 作業指示を取得する
    ' ''' </summary>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="jobDtlId">作業内容ID</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetJobInstruct(ByVal roNum As String, ByVal jobDtlId As Decimal) As SC3240201DataSet.SC3240201JobInstructListDataTable

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[roNum:{0}][jobDtlId:{1}]", roNum, jobDtlId)

    '    'SQL組み立て
    '    Dim sql As New StringBuilder
    '    With sql
    '        .AppendLine("    SELECT DISTINCT /* SC3240201_014 */ ")
    '        .AppendLine("          T1.JOB_DTL_ID ")
    '        .AppendLine("        , T1.JOB_INSTRUCT_ID ")
    '        .AppendLine("        , T1.JOB_INSTRUCT_SEQ ")
    '        .AppendLine("        , T1.RO_SEQ ")
    '        .AppendLine("        , T1.JOB_CD ")
    '        .AppendLine("        , T1.JOB_NAME ")
    '        .AppendLine("        , T1.OPERATION_TYPE_NAME ")
    '        .AppendLine("        , CASE WHEN T1.STARTWORK_INSTRUCT_FLG = N'1' ")
    '        .AppendLine("               THEN T1.JOB_DTL_ID ")
    '        .AppendLine("               ELSE TO_NUMBER(-1) ")
    '        .AppendLine("               END AS SELECT_JOB_DTL_ID ")
    '        .AppendLine("        , NVL(T2.JOB_STATUS, '-1') AS JOB_STATUS ")
    '        .AppendLine("        , CASE WHEN (     T1.STARTWORK_INSTRUCT_FLG = N'1' ")
    '        .AppendLine("                      AND T1.JOB_DTL_ID = :JOB_DTL_ID ) ")
    '        .AppendLine("               THEN 0")
    '        .AppendLine("               ELSE 1")
    '        .AppendLine("               END AS SORT_KEY_1")
    '        .AppendLine("     FROM ")
    '        .AppendLine("          TB_T_JOB_INSTRUCT T1 ")
    '        .AppendLine("        , TB_T_JOB_RESULT T2 ")
    '        .AppendLine("        , TB_T_RO_INFO T3 ")
    '        '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 START
    '        '.AppendLine("        , (   SELECT MAX(JOB_RSLT_ID) AS MAX_JOB_RSLT_ID ")
    '        '.AppendLine("                FROM TB_T_JOB_RESULT ")
    '        '.AppendLine("            GROUP BY JOB_DTL_ID, JOB_INSTRUCT_ID, JOB_INSTRUCT_SEQ ) T4")
    '        '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 END
    '        .AppendLine("      WHERE ")
    '        .AppendLine("            T1.RO_NUM = T3.RO_NUM ")
    '        .AppendLine("        AND T1.RO_SEQ = T3.RO_SEQ ")
    '        .AppendLine("        AND T1.JOB_DTL_ID = T2.JOB_DTL_ID(+) ")
    '        .AppendLine("        AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID(+) ")
    '        .AppendLine("        AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ(+) ")
    '        .AppendLine("        AND T1.RO_NUM = :RO_NUM ")
    '        .AppendLine("        AND T3.RO_STATUS <> N'99' ")
    '        '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 START
    '        '.AppendLine("        AND (    T2.JOB_RSLT_ID = T4.MAX_JOB_RSLT_ID ")
    '        .AppendLine("        AND (    T2.JOB_RSLT_ID = ( SELECT MAX(JOB_RSLT_ID) ")
    '        .AppendLine("                                      FROM TB_T_JOB_RESULT A ")
    '        .AppendLine("                                     WHERE A.JOB_DTL_ID = T2.JOB_DTL_ID ")
    '        .AppendLine("                                       AND A.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID ")
    '        .AppendLine("                                       AND A.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ ")
    '        .AppendLine("                                  GROUP BY A.JOB_DTL_ID ")
    '        .AppendLine("                                         , A.JOB_INSTRUCT_ID ")
    '        .AppendLine("                                         , A.JOB_INSTRUCT_SEQ ) ")
    '        '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 END
    '        .AppendLine("        	   OR T2.JOB_RSLT_ID IS NULL ) ")
    '        .AppendLine("    ORDER BY ")
    '        .AppendLine("            SORT_KEY_1, T1.RO_SEQ, T1.JOB_INSTRUCT_ID, T1.JOB_INSTRUCT_SEQ ")
    '    End With

    '    Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201JobInstructListDataTable)("SC3240201_014")
    '        query.CommandText = sql.ToString()

    '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
    '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)

    '        'SQL実行
    '        Dim rtnDt As SC3240201DataSet.SC3240201JobInstructListDataTable = query.GetData()

    '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

    '        Return rtnDt

    '    End Using

    'End Function

    ''' <summary>
    ''' 作業指示を取得する
    ''' </summary>
    ''' <param name="inRepairOrderNo">RO番号</param>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJobInstruct(ByVal inRepairOrderNo As String, _
                                   ByVal inJobDetailId As Decimal, _
                                   ByVal inDealerCode As String, _
                                   ByVal inBranchCode As String) As SC3240201DataSet.SC3240201JobInstructListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, _
                      True, _
                      "[inRepairOrderNo:{0}][inJobDetailId:{1}][inDealerCode:{2}][inBranchCode:{3}]", _
                      inRepairOrderNo, _
                      inJobDetailId, _
                      inDealerCode, _
                      inBranchCode)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .AppendLine("   SELECT DISTINCT /* SC3240201_014 */ ")
            .AppendLine("          T1.JOB_DTL_ID ")
            .AppendLine("        , T1.JOB_INSTRUCT_ID ")
            .AppendLine("        , T1.JOB_INSTRUCT_SEQ ")
            .AppendLine("        , T1.RO_SEQ ")
            .AppendLine("        , T1.JOB_CD ")
            .AppendLine("        , T1.JOB_NAME ")
            .AppendLine("        , T1.OPERATION_TYPE_NAME ")
            .AppendLine("        , CASE WHEN T1.STARTWORK_INSTRUCT_FLG = N'1' ")
            .AppendLine("               THEN T1.JOB_DTL_ID ")
            .AppendLine("               ELSE TO_NUMBER(-1) ")
            .AppendLine("               END AS SELECT_JOB_DTL_ID ")
            .AppendLine("        , NVL(T2.JOB_STATUS, '-1') AS JOB_STATUS ")
            .AppendLine("        , CASE WHEN (     T1.STARTWORK_INSTRUCT_FLG = N'1' ")
            .AppendLine("                      AND T1.JOB_DTL_ID = :JOB_DTL_ID ) ")
            .AppendLine("               THEN 0")
            .AppendLine("               ELSE 1")
            .AppendLine("               END AS SORT_KEY_1")
            .AppendLine("     FROM ")
            .AppendLine("          TB_T_JOB_INSTRUCT T1 ")
            .AppendLine("        , TB_T_JOB_RESULT T2 ")
            .AppendLine("        , TB_T_RO_INFO T3 ")
            .AppendLine("        , TB_T_JOB_DTL T5 ")
            '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 START
            '.AppendLine("        , (   SELECT MAX(JOB_RSLT_ID) AS MAX_JOB_RSLT_ID ")
            '.AppendLine("                FROM TB_T_JOB_RESULT ")
            '.AppendLine("            GROUP BY JOB_DTL_ID, JOB_INSTRUCT_ID, JOB_INSTRUCT_SEQ ) T4")
            '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 END
            .AppendLine("    WHERE ")
            .AppendLine("          T1.RO_NUM = T3.RO_NUM ")
            .AppendLine("      AND T1.RO_SEQ = T3.RO_SEQ ")
            .AppendLine("      AND T1.JOB_DTL_ID = T2.JOB_DTL_ID(+) ")
            .AppendLine("      AND T1.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID(+) ")
            .AppendLine("      AND T1.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ(+) ")
            '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 START
            '.AppendLine("        AND (    T2.JOB_RSLT_ID = T4.MAX_JOB_RSLT_ID ")
            .AppendLine("      AND (    T2.JOB_RSLT_ID = ( SELECT MAX(JOB_RSLT_ID) ")
            .AppendLine("                                    FROM TB_T_JOB_RESULT A ")
            .AppendLine("                                   WHERE A.JOB_DTL_ID = T2.JOB_DTL_ID ")
            .AppendLine("                                     AND A.JOB_INSTRUCT_ID = T2.JOB_INSTRUCT_ID ")
            .AppendLine("                                     AND A.JOB_INSTRUCT_SEQ = T2.JOB_INSTRUCT_SEQ ")
            .AppendLine("                                GROUP BY A.JOB_DTL_ID ")
            .AppendLine("                                       , A.JOB_INSTRUCT_ID ")
            .AppendLine("                                       , A.JOB_INSTRUCT_SEQ ) ")
            '2014/07/17 TMEJ 明瀬 タブレットSMB Job Dispatch機能開発 END
            .AppendLine("        	 OR T2.JOB_RSLT_ID IS NULL ) ")
            .AppendLine("      AND T1.JOB_DTL_ID = T5.JOB_DTL_ID ")
            .AppendLine("      AND T1.RO_NUM = :RO_NUM ")
            .AppendLine("      AND T3.RO_STATUS <> N'99' ")
            .AppendLine("      AND T3.DLR_CD = :DLR_CD ")
            .AppendLine("      AND T3.BRN_CD = :BRN_CD ")
            .AppendLine("      AND T5.DLR_CD = :DLR_CD ")
            .AppendLine("      AND T5.BRN_CD = :BRN_CD ")
            .AppendLine(" ORDER BY ")
            .AppendLine("          SORT_KEY_1 ")
            .AppendLine("        , T1.RO_SEQ ")
            .AppendLine("        , T1.JOB_INSTRUCT_ID ")
            .AppendLine("        , T1.JOB_INSTRUCT_SEQ ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201JobInstructListDataTable)("SC3240201_014")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRepairOrderNo)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDetailId)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201JobInstructListDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function

    '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

    ''' <summary>
    ''' 作業指示を取得する(退避用)
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inInstructId">作業指示ID</param>
    ''' <param name="inInstructSeq">作業指示枝番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJobInstructBackup(ByVal inJobDtlId As Decimal, _
                                          ByVal inInstructId As String, _
                                          ByVal inInstructSeq As Long) As SC3240201DataSet.SC3240201JobInstructBakupListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[inJobDtlId:{0}][inInstructId:{0}][inInstructSeq:{0}]", inJobDtlId, inInstructId, inInstructSeq)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* SC3240201_015 */ ")
            .AppendLine("        T1.JOB_DTL_ID ")
            .AppendLine("      , T1.JOB_INSTRUCT_ID ")
            .AppendLine("      , T1.JOB_INSTRUCT_SEQ ")
            .AppendLine("      , T1.RO_NUM ")
            .AppendLine("      , T1.RO_SEQ ")
            .AppendLine("      , T1.JOB_CD ")
            .AppendLine("      , T1.JOB_NAME ")
            .AppendLine("      , T1.STD_WORKTIME ")
            .AppendLine("      , T1.JOB_STF_GROUP_ID ")
            .AppendLine("      , T1.JOB_STF_GROUP_NAME ")
            .AppendLine("      , T1.STARTWORK_INSTRUCT_FLG ")
            .AppendLine("      , T1.OPERATION_TYPE_ID ")
            .AppendLine("      , T1.OPERATION_TYPE_NAME ")
            .AppendLine("      , T1.WORK_UNIT_PRICE ")
            .AppendLine("      , T1.WORK_PRICE ")
            .AppendLine("   FROM ")
            .AppendLine("        TB_T_JOB_INSTRUCT T1 ")
            .AppendLine("  WHERE  ")
            .AppendLine("        T1.JOB_DTL_ID=:JOB_DTL_ID ")
            .AppendLine("    AND T1.JOB_INSTRUCT_ID=:JOB_INSTRUCT_ID ")
            .AppendLine("    AND T1.JOB_INSTRUCT_SEQ=:JOB_INSTRUCT_SEQ ")
        End With

        Using query As New DBSelectQuery(Of SC3240201DataSet.SC3240201JobInstructBakupListDataTable)("SC3240201_015")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inInstructId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inInstructSeq)

            'SQL実行
            Dim rtnDt As SC3240201DataSet.SC3240201JobInstructBakupListDataTable = query.GetData()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

            Return rtnDt

        End Using

    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "Updateメソッド"

    ''' <summary>
    ''' サービス入庫を更新する
    ''' </summary>
    ''' <param name="arg">コールバック引数クラス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdServiceIn(ByVal arg As CallBackArgumentClass) As Integer

        With arg
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                          "[VisitPlanTime:{0}][DeriveredPlanTime:{1}][WaitingFlg:{2}][RezFlg:{3}][CarWashFlg:{4}][SvcInId:{5}]",
                          .VisitPlanTime, .DeriveredPlanTime, .WaitingFlg, .RezFlg, .CarWashFlg, .SvcInId)
        End With

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3240201_101 */ ")
            .Append("        TB_T_SERVICEIN ")                  'サービス入庫テーブル
            .Append("    SET ")

            '来店予定日時
            If Not String.IsNullOrEmpty(arg.VisitPlanTime) Then
                .Append("        SCHE_SVCIN_DATETIME = TO_DATE(:PLAN_VISITDATE,'YYYY/MM/DD HH24:MI:SS') ")
            Else
                .Append("        SCHE_SVCIN_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
            End If

            '納車予定日時
            If Not String.IsNullOrEmpty(arg.DeriveredPlanTime) Then
                .Append("      , SCHE_DELI_DATETIME = TO_DATE(:PLAN_DELIDATE,'YYYY/MM/DD HH24:MI:SS') ")
            Else
                .Append("      , SCHE_DELI_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
            End If

            .Append("      , PICK_DELI_TYPE = :WAITTYPE ")               '待ち方
            .Append("      , ACCEPTANCE_TYPE = :REZFLAG ")               '予約フラグ
            .Append("      , CARWASH_NEED_FLG = :WASHFLAG ")             '洗車フラグ

            '以下の項目は、SMBCommonClassでサービス入庫をロックする際にUpdateするため、コメント化
            '.Append("   , UPDATE_DATETIME = :UPDATEDATE ")            '更新日時
            '.Append("   , UPDATE_STF_CD = :UPDATEACCOUNT ")           '更新スタッフコード
            '.Append("   , ROW_UPDATE_DATETIME = :UPDATEDATE ")        '行更新日時
            '.Append("   , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")      '行更新アカウント
            '.Append("   , ROW_UPDATE_FUNCTION = :SYSTEM ")            '行更新機能  
            '.Append("   , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")  '行ロックバージョン

            .Append("  WHERE ")
            .Append("        SVCIN_ID = :SVCIN_ID ")
        End With

        Using query As New DBUpdateQuery("SC3240201_101")
            query.CommandText = sql.ToString()

            '来店予定日時
            If Not String.IsNullOrEmpty(arg.VisitPlanTime) Then
                query.AddParameterWithTypeValue("PLAN_VISITDATE", OracleDbType.NVarchar2, arg.VisitPlanTime)
            End If

            '納車予定日時
            If Not String.IsNullOrEmpty(arg.DeriveredPlanTime) Then
                query.AddParameterWithTypeValue("PLAN_DELIDATE", OracleDbType.NVarchar2, arg.DeriveredPlanTime)
            End If

            query.AddParameterWithTypeValue("WAITTYPE", OracleDbType.NVarchar2, arg.WaitingFlg)              '待ち方
            query.AddParameterWithTypeValue("REZFLAG", OracleDbType.NVarchar2, arg.RezFlg)                   '予約フラグ
            query.AddParameterWithTypeValue("WASHFLAG", OracleDbType.NVarchar2, arg.CarWashFlg)              '洗車フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Long, arg.SvcInId)                 'サービス入庫ID
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, arg.SvcInId)                   'サービス入庫ID
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function

    ''' <summary>
    ''' 作業内容を更新する
    ''' </summary>
    ''' <param name="arg">コールバック引数クラス</param>
    ''' <param name="updDate">登録用更新日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdJobDtl(ByVal arg As CallBackArgumentClass, ByVal updDate As Date) As Integer

        With arg
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
            '              "[SvcClassId:{0}][MercId:{1}][Account:{2}][JobDtlId:{3}][updDate:{4}]",
            '              .SvcClassId, .MercId, .Account, .JobDtlId, updDate)
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                          "[SvcClassId:{0}][MercId:{1}][Account:{2}][JobDtlId:{3}][updDate:{4}][CompleteExaminationFlg:{5}]",
                          .SvcClassId, .MercId, .Account, .JobDtlId, updDate, .CompleteExaminationFlg)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End With

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3240201_102 */ ")
            .Append("        TB_T_JOB_DTL ")                  '作業内容テーブル
            .Append("    SET ")

            .Append("        SVC_CLASS_ID = :SVC_CLASS_ID ")               '表示サービス分類ID
            .Append("      , MERC_ID = :MERC_ID ")                       '表示商品ID
            .Append("      , UPDATE_DATETIME = :UPDATEDATE ")            '更新日時
            .Append("      , UPDATE_STF_CD = :UPDATEACCOUNT ")           '更新スタッフコード
            .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")        '行更新日時
            .Append("      , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")      '行更新アカウント
            .Append("      , ROW_UPDATE_FUNCTION = :SYSTEM ")            '行更新機能  
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")  '行ロックバージョン
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            .Append("      , INSPECTION_NEED_FLG = :INSPECTIONFLG ")     '完成検査フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            .Append("  WHERE ")
            .Append("        JOB_DTL_ID = :JOB_DTL_ID ")
        End With

        Using query As New DBUpdateQuery("SC3240201_102")
            query.CommandText = sql.ToString()
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Long, arg.SvcClassId)          '表示サービス分類ID
            query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Decimal, arg.SvcClassId)       '表示サービス分類ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            query.AddParameterWithTypeValue("MERC_ID", OracleDbType.Long, arg.MercId)                   '表示商品ID
            query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updDate)                   '更新日時、行更新日時
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, arg.Account)       '更新スタッフコード、行更新アカウント
            query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, MY_PROGRAMID)             '行更新機能
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, arg.JobDtlId)              '作業内容ID
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, arg.JobDtlId)           '作業内容ID
            query.AddParameterWithTypeValue("INSPECTIONFLG", OracleDbType.NVarchar2, arg.CompleteExaminationFlg)   '完成検査フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function

    ' ''' <summary>
    ' ''' ストール利用を更新する
    ' ''' </summary>
    ' ''' <param name="arg">コールバック引数クラス</param>
    ' ''' <param name="updDate">登録用更新日時</param>
    ' ''' <param name="startPlanTime">作業開始予定日時</param>
    ' ''' <param name="finishPlanTime">作業完了予定日時</param>
    ' ''' <param name="prmsEndTime">見込終了日時</param>
    ' ''' <param name="procTime">実績作業時間</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function UpdStallUse(ByVal arg As CallBackArgumentClass, _
    '                            ByVal updDate As Date, _
    '                            ByVal startPlanTime As Date, _
    '                            ByVal finishPlanTime As Date, _
    '                            ByVal prmsEndTime As Date, _
    '                            ByVal procTime As Long) As Integer
    ''' <summary>
    ''' ストール利用を更新する
    ''' </summary>
    ''' <param name="arg">コールバック引数クラス</param>
    ''' <param name="updDate">登録用更新日時</param>
    ''' <param name="startPlanTime">作業開始予定日時</param>
    ''' <param name="finishPlanTime">作業完了予定日時</param>
    ''' <param name="prmsEndTime">見込終了日時</param>
    ''' <param name="procTime">実績作業時間</param>
    ''' <param name="restAutoJudgeFlg">休憩取得自動判定フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdStallUse(ByVal arg As CallBackArgumentClass, _
                                ByVal updDate As Date, _
                                ByVal startPlanTime As Date, _
                                ByVal finishPlanTime As Date, _
                                ByVal prmsEndTime As Date, _
                                ByVal procTime As Long, _
                                ByVal restAutoJudgeFlg As String) As Integer

        With arg
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                          "[StartPlanTime:{0}][StartProcessTime:{1}][PlanWorkTime:{2}][StallUseStatus:{3}][RestFlg:{4}][Account:{5}]" & _
                          "[StallUseId:{6}][updDate:{7}][startPlanTime:{8}][finishPlanTime:{9}][prmsEndTime:{10}][procTime:{11}]",
                          .StartPlanTime, .StartProcessTime, .PlanWorkTime, .StallUseStatus, .RestFlg, .Account, _
                          .StallUseId, updDate, startPlanTime, finishPlanTime, prmsEndTime, procTime)
        End With

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3240201_103 */ ")
            .Append("        TB_T_STALL_USE ")                  'ストール利用テーブル
            .Append("    SET ")

            '予定開始日時、予定開始日
            If startPlanTime <> DateTime.MinValue Then
                .Append("        SCHE_START_DATETIME = :SCHE_START_DATETIME ")
                .Append("      , SCHE_START_DATE = TO_CHAR(:SCHE_START_DATETIME,'YYYYMMDD') ")
            Else
                .Append("        SCHE_START_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
                .Append("      , SCHE_START_DATE = '19000101' ")
            End If

            '予定終了日時
            If finishPlanTime <> DateTime.MinValue Then
                .Append("      , SCHE_END_DATETIME = :SCHE_END_DATETIME ")
            Else
                .Append("      , SCHE_END_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
            End If

            '実績開始日時
            If Not String.IsNullOrEmpty(arg.StartProcessTime) Then
                .Append("      , RSLT_START_DATETIME = TO_DATE(:RSLT_START_DATETIME,'YYYY/MM/DD HH24:MI:SS') ")
            Else
                .Append("      , RSLT_START_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
            End If

            '実績終了日時
            If Not String.IsNullOrEmpty(arg.FinishProcessTime) Then
                .Append("      , RSLT_END_DATETIME = TO_DATE(:RSLT_END_DATETIME,'YYYY/MM/DD HH24:MI:SS') ")
            Else
                .Append("      , RSLT_END_DATETIME = TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
            End If

            '予定作業時間
            If Not String.IsNullOrEmpty(arg.PlanWorkTime) Then
                .Append("      , SCHE_WORKTIME = :SCHE_WORKTIME ")
            End If

            '実績作業時間
            .Append("      , RSLT_WORKTIME = :RSLT_WORKTIME ")


            '見込終了日時
            '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ''ストール利用ステータス=02:作業中の場合
            'If arg.StallUseStatus.Equals("02") Then
            'ストール利用ステータス=02:作業中、04:作業指示の一部の作業が中断 の場合のみ、更新する
            If arg.StallUseStatus.Equals(STALL_USE_STATUS_02) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_04) Then
                '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                .Append("      , PRMS_END_DATETIME = :PRMS_END_DATETIME ")
            End If

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            '休憩取得フラグ
            '休憩を自動判定しない場合または
            'ストール利用ステータスが00:着工指示待ち、01:作業開始待ち、02:作業中、04:作業指示の一部の作業が中断 の場合のみ更新する
            If Not restAutoJudgeFlg.Equals("1") Or _
                arg.StallUseStatus.Equals(STALL_USE_STATUS_00) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_01) Or _
                arg.StallUseStatus.Equals(STALL_USE_STATUS_02) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_04) Then
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                '休憩取得フラグ(0:取得しない／1:取得する)　※1がデフォルト
                .Append("      , REST_FLG = :REST_FLG ")
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            End If
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            .Append("      , UPDATE_DATETIME = :UPDATEDATE ")            '更新日時
            .Append("      , UPDATE_STF_CD = :UPDATEACCOUNT ")           '更新スタッフコード
            .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")        '行更新日時
            .Append("      , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")      '行更新アカウント
            .Append("      , ROW_UPDATE_FUNCTION = :SYSTEM ")            '行更新機能  
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")  '行ロックバージョン

            .Append("  WHERE ")
            .Append("        STALL_USE_ID = :STALL_USE_ID ")
        End With

        Using query As New DBUpdateQuery("SC3240201_103")
            query.CommandText = sql.ToString()

            '予定開始日時、予定開始日
            If startPlanTime <> DateTime.MinValue Then
                query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, startPlanTime)
            End If

            '予定終了日時
            If finishPlanTime <> DateTime.MinValue Then
                query.AddParameterWithTypeValue("SCHE_END_DATETIME", OracleDbType.Date, finishPlanTime)
            End If

            '実績開始日時
            If Not String.IsNullOrEmpty(arg.StartProcessTime) Then
                query.AddParameterWithTypeValue("RSLT_START_DATETIME", OracleDbType.NVarchar2, arg.StartProcessTime)
            End If

            '実績終了日時
            If Not String.IsNullOrEmpty(arg.FinishProcessTime) Then
                query.AddParameterWithTypeValue("RSLT_END_DATETIME", OracleDbType.NVarchar2, arg.FinishProcessTime)
            End If

            '予定作業時間
            If Not String.IsNullOrEmpty(arg.PlanWorkTime) Then
                query.AddParameterWithTypeValue("SCHE_WORKTIME", OracleDbType.Long, arg.PlanWorkTime)
            End If

            '実績作業時間
            query.AddParameterWithTypeValue("RSLT_WORKTIME", OracleDbType.Long, procTime)

            '見込終了日時
            '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ''ストール利用ステータス=02:作業中の場合のみ、更新する
            'If arg.StallUseStatus.Equals("02") Then
            ''ストール利用ステータス=02:作業中、04:作業指示の一部の作業が中断 の場合のみ、更新する
            If arg.StallUseStatus.Equals(STALL_USE_STATUS_02) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_04) Then
                '2019/08/06 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                query.AddParameterWithTypeValue("PRMS_END_DATETIME", OracleDbType.Date, prmsEndTime)
            End If

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            '休憩取得フラグ
            '休憩を自動判定しない場合または
            'ストール利用ステータスが00:着工指示待ち、01:作業開始待ち、02:作業中、04:作業指示の一部の作業が中断 の場合のみ更新する
            If Not restAutoJudgeFlg.Equals("1") Or _
                arg.StallUseStatus.Equals(STALL_USE_STATUS_00) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_01) Or _
                arg.StallUseStatus.Equals(STALL_USE_STATUS_02) Or arg.StallUseStatus.Equals(STALL_USE_STATUS_04) Then
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                '休憩を取得しない場合
                If arg.RestFlg = 0 Then
                    query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, NOT_USE_REST)
                Else
                    query.AddParameterWithTypeValue("REST_FLG", OracleDbType.NVarchar2, USE_REST)
                End If
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            End If
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updDate)                   '更新日時、行更新日時
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, arg.Account)       '更新スタッフコード、行更新アカウント
            query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, MY_PROGRAMID)             '行更新機能
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Long, arg.StallUseId)          'ストール利用ID
            query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, arg.StallUseId)       'ストール利用ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' RO作業連番を更新する
    ' ''' </summary>
    ' ''' <param name="arg">コールバック引数クラス</param>
    ' ''' <param name="updDate">登録用更新日時</param>
    ' ''' <param name="dispRezId">作業内容ID</param>
    ' ''' <param name="jobSeq">登録用RO作業連番</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function UpdROJobSeq(ByVal arg As CallBackArgumentClass, _
    '                            ByVal updDate As Date, _
    '                            ByVal dispRezId As Decimal, _
    '                            ByVal jobSeq As Long) As Integer

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
    '              "[Account:{0}][updDate:{1}][dispRezId:{2}][jobSeq:{3}]",
    '              arg.Account, updDate, dispRezId, jobSeq)

    '    'SQL組み立て
    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append(" UPDATE /* SC3240201_104 */ ")
    '        .Append("        TB_T_JOB_DTL ")                  '作業内容テーブル
    '        .Append("    SET ")
    '        .Append("        RO_JOB_SEQ = :RO_JOB_SEQ ")                   'RO作業連番
    '        .Append("      , UPDATE_DATETIME = :UPDATEDATE ")            '更新日時
    '        .Append("      , UPDATE_STF_CD = :UPDATEACCOUNT ")           '更新スタッフコード
    '        .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")        '行更新日時
    '        .Append("      , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")      '行更新アカウント
    '        .Append("      , ROW_UPDATE_FUNCTION = :SYSTEM ")            '行更新機能  
    '        .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")  '行ロックバージョン
    '        .Append("  WHERE")
    '        .Append("        JOB_DTL_ID = :JOB_DTL_ID ")
    '    End With

    '    Using query As New DBUpdateQuery("SC3240201_104")
    '        query.CommandText = sql.ToString()

    '        query.AddParameterWithTypeValue("RO_JOB_SEQ", OracleDbType.Long, jobSeq)                    'RO作業連番
    '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updDate)                   '更新日時、行更新日時
    '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, arg.Account)       '更新スタッフコード、行更新アカウント
    '        query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, MY_PROGRAMID)             '行更新機能
    '        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '        'query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, dispRezId)                 '作業内容ID
    '        query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, dispRezId)              '作業内容ID
    '        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '        'SQL実行
    '        Dim rtnCD As Integer = query.Execute()

    '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

    '        Return rtnCD

    '    End Using

    'End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' ストール利用ステータスを更新する
    ''' </summary>
    ''' <param name="arg">コールバック引数クラス</param>
    ''' <param name="updDate">登録用更新日時</param>
    ''' <param name="dispRezId">作業内容ID</param>
    ''' <param name="status">登録用ストール利用ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdStallUseStatus(ByVal arg As CallBackArgumentClass, _
                                      ByVal updDate As Date, _
                                      ByVal dispRezId As Decimal, _
                                      ByVal status As String) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[Account:{0}][updDate:{1}][dispRezId:{2}][status:{3}]",
                      arg.Account, updDate, dispRezId, status)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3240201_105 */ ")
            .Append("        TB_T_STALL_USE ")                  'ストール利用テーブル
            .Append("    SET ")
            .Append("        STALL_USE_STATUS = :STALL_USE_STATUS ")       'ストール利用ステータス
            .Append("      , UPDATE_DATETIME = :UPDATEDATE ")            '更新日時
            .Append("      , UPDATE_STF_CD = :UPDATEACCOUNT ")           '更新スタッフコード
            .Append("      , ROW_UPDATE_DATETIME = :UPDATEDATE ")        '行更新日時
            .Append("      , ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT ")      '行更新アカウント
            .Append("      , ROW_UPDATE_FUNCTION = :SYSTEM ")            '行更新機能  
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")  '行ロックバージョン
            .Append("  WHERE ")
            .Append("        STALL_USE_ID = ")
            .Append("        (SELECT  ")
            .Append("                MAX(X.STALL_USE_ID) ")
            .Append("           FROM  ")
            .Append("                TB_T_STALL_USE X ")
            .Append("          WHERE  ")
            .Append("                X.JOB_DTL_ID = :JOB_DTL_ID ")
            .Append("        ) ")
        End With

        Using query As New DBUpdateQuery("SC3240201_105")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("STALL_USE_STATUS", OracleDbType.NVarchar2, status)         'ストール利用ステータス
            query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updDate)                   '更新日時、行更新日時
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, arg.Account)       '更新スタッフコード、行更新アカウント
            query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, MY_PROGRAMID)             '行更新機能
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Long, dispRezId)                 '作業内容ID
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, dispRezId)              '作業内容ID
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業指示を更新する
    ''' </summary>
    ''' <param name="inNow">更新日時</param>
    ''' <param name="inAccount">スタッフコード</param>
    ''' <param name="inStructFlg">着工指示フラグ</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inInstructId">作業指示ID</param>
    ''' <param name="inInstructSeq">作業指示枝番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdJobInstruct(ByVal inNow As Date, _
                                   ByVal inAccount As String, _
                                   ByVal inStructFlg As String, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inInstructId As String, _
                                   ByVal inInstructSeq As Long) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[inNow:{0}][inAccount:{1}][inStructFlg:{2}][inJobDtlId:{3}][inInstructId:{4}][inInstructSeq:{5}]",
                      inNow, inAccount, inStructFlg, inJobDtlId, inInstructId, inInstructSeq)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3240201_106 */ ")
            .Append("        TB_T_JOB_INSTRUCT ")                                    '作業指示テーブル
            .Append(" SET ")
            .Append("        STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG ")     '着工指示フラグ
            .Append("      , ROW_CREATE_DATETIME = :ROW_CREATE_DATETIME ")           '行作成日時 
            .Append("      , ROW_CREATE_ACCOUNT = :ROW_CREATE_ACCOUNT ")             '行作成アカウント
            .Append("      , ROW_CREATE_FUNCTION = :ROW_CREATE_FUNCTION ")           '行作成機能
            .Append("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")           '行更新日時
            .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")             '行更新アカウント
            .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")           '行更新機能  
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")              '行ロックバージョン
            .Append(" WHERE ")
            .Append("        JOB_DTL_ID = :JOB_DTL_ID ")                             '作業内容ID
            .Append("    AND JOB_INSTRUCT_ID = :JOB_INSTRUCT_ID ")                   '作業指示ID
            .Append("    AND JOB_INSTRUCT_SEQ = :JOB_INSTRUCT_SEQ ")                 '作業指示枝番
        End With

        Using query As New DBUpdateQuery("SC3240201_106")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, inStructFlg)
            query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inNow)
            query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
            query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNow)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inInstructId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inInstructSeq)

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "Deleteメソッド"
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業指示を削除する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inInstructId">作業指示ID</param>
    ''' <param name="inInstructSeq">作業指示枝番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteJobInstruct(ByVal inJobDtlId As Decimal, _
                                        ByVal inInstructId As String, _
                                        ByVal inInstructSeq As Long) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[inJobDtlId:{0}][inInstructId:{1}][inInstructSeq:{2}]",
                      inJobDtlId, inInstructId, inInstructSeq)

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" DELETE FROM /* SC3240201_201 */ ")
            .Append("        TB_T_JOB_INSTRUCT ")                                    '作業指示テーブル
            .Append(" WHERE ")
            .Append("        JOB_DTL_ID = :JOB_DTL_ID ")                             '作業内容ID
            .Append("    AND JOB_INSTRUCT_ID = :JOB_INSTRUCT_ID ")                   '作業指示ID
            .Append("    AND JOB_INSTRUCT_SEQ = :JOB_INSTRUCT_SEQ ")                 '作業指示枝番
        End With

        Using query As New DBUpdateQuery("SC3240201_201")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inInstructId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inInstructSeq)

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using
    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "Insertメソッド"
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業指示を挿入する
    ''' </summary>
    ''' <param name="inJobInstructRow">退避した作業指示テーブル情報</param>
    ''' <param name="inNow">更新日時</param>
    ''' <param name="inAccount">スタッフコード</param>
    ''' <param name="inStructFlg">着工指示フラグ</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertJobInstruct(ByVal inJobInstructRow As SC3240201DataSet.SC3240201JobInstructBakupListRow, _
                                        ByVal inNow As Date, _
                                        ByVal inAccount As String, _
                                        ByVal inStructFlg As String, _
                                        ByVal inJobDtlId As Decimal) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[inNow:{0}][inAccount:{1}][inStructFlg:{2}][inJobDtlId:{3}]",
                      inNow, inAccount, inStructFlg, inJobDtlId)

        Dim rowlockversion As Long = 0

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" INSERT INTO /* SC3240201_301 */ ")
            .Append("        TB_T_JOB_INSTRUCT  ")
            .Append(" (                         ")
            .Append("        JOB_DTL_ID         ")
            .Append("      , JOB_INSTRUCT_ID    ")
            .Append("      , JOB_INSTRUCT_SEQ   ")
            .Append("      , RO_NUM             ")
            .Append("      , RO_SEQ         ")
            .Append("      , JOB_CD             ")
            .Append("      , JOB_NAME           ")
            .Append("      , STD_WORKTIME       ")
            .Append("      , JOB_STF_GROUP_ID   ")
            .Append("      , JOB_STF_GROUP_NAME ")
            .Append("      , STARTWORK_INSTRUCT_FLG ")
            .Append("      , OPERATION_TYPE_ID  ")
            .Append("      , OPERATION_TYPE_NAME ")
            .Append("      , WORK_UNIT_PRICE         ")
            .Append("      , WORK_PRICE          ")
            .Append("      , ROW_CREATE_DATETIME ")
            .Append("      , ROW_CREATE_ACCOUNT ")
            .Append("      , ROW_CREATE_FUNCTION ")
            .Append("      , ROW_UPDATE_DATETIME ")
            .Append("      , ROW_UPDATE_ACCOUNT ")
            .Append("      , ROW_UPDATE_FUNCTION ")
            .Append("      , ROW_LOCK_VERSION   ")
            .Append(" )                         ")
            .Append(" VALUES(                   ")
            .Append("          :JOB_DTL_ID        ")
            .Append("        , :JOB_INSTRUCT_ID ")
            .Append("        , :JOB_INSTRUCT_SEQ ")
            .Append("        , :RO_NUM ")
            .Append("        , :RO_SEQ ")
            .Append("        , :JOB_CD ")
            .Append("        , :JOB_NAME ")
            .Append("        , :STD_WORKTIME ")
            .Append("        , :JOB_STF_GROUP_ID ")
            .Append("        , :JOB_STF_GROUP_NAME ")
            .Append("        , :STARTWORK_INSTRUCT_FLG ")
            .Append("        , :OPERATION_TYPE_ID ")
            .Append("        , :OPERATION_TYPE_NAME ")
            .Append("        , :WORK_UNIT_PRICE ")
            .Append("        , :WORK_PRICE ")
            .Append("        , :ROW_CREATE_DATETIME ")
            .Append("        , :ROW_CREATE_ACCOUNT ")
            .Append("        , :ROW_CREATE_FUNCTION ")
            .Append("        , :ROW_UPDATE_DATETIME ")
            .Append("        , :ROW_UPDATE_ACCOUNT ")
            .Append("        , :ROW_UPDATE_FUNCTION ")
            .Append("        , :ROW_LOCK_VERSION ")
            .Append("  ) ")
        End With

        Using query As New DBUpdateQuery("SC3240201_301")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, inJobDtlId)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_ID", OracleDbType.NVarchar2, inJobInstructRow.JOB_INSTRUCT_ID)
            query.AddParameterWithTypeValue("JOB_INSTRUCT_SEQ", OracleDbType.Long, inJobInstructRow.JOB_INSTRUCT_SEQ)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inJobInstructRow.RO_NUM)
            query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Long, inJobInstructRow.RO_SEQ)
            query.AddParameterWithTypeValue("JOB_CD", OracleDbType.NVarchar2, inJobInstructRow.JOB_CD)
            query.AddParameterWithTypeValue("JOB_NAME", OracleDbType.NVarchar2, inJobInstructRow.JOB_NAME)
            query.AddParameterWithTypeValue("STD_WORKTIME", OracleDbType.Long, inJobInstructRow.STD_WORKTIME)
            query.AddParameterWithTypeValue("JOB_STF_GROUP_ID", OracleDbType.NVarchar2, inJobInstructRow.JOB_STF_GROUP_ID)
            query.AddParameterWithTypeValue("JOB_STF_GROUP_NAME", OracleDbType.NVarchar2, inJobInstructRow.JOB_STF_GROUP_NAME)
            query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.NVarchar2, inStructFlg)
            query.AddParameterWithTypeValue("OPERATION_TYPE_ID", OracleDbType.NVarchar2, inJobInstructRow.OPERATION_TYPE_ID)
            query.AddParameterWithTypeValue("OPERATION_TYPE_NAME", OracleDbType.NVarchar2, inJobInstructRow.OPERATION_TYPE_NAME)
            query.AddParameterWithTypeValue("WORK_UNIT_PRICE", OracleDbType.Long, inJobInstructRow.WORK_UNIT_PRICE)
            query.AddParameterWithTypeValue("WORK_PRICE", OracleDbType.Long, inJobInstructRow.WORK_PRICE)
            query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, inNow)
            query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
            query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, inNow)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, inAccount)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, MY_PROGRAMID)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, rowlockversion)

            'SQL実行
            Dim rtnCD As Integer = query.Execute()

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnCD:{0}]", rtnCD)

            Return rtnCD

        End Using

    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region


#Region "ログ出力メソッド"

    ' ''' <summary>
    ' ''' 引数のないInfoレベルのログを出力する
    ' ''' </summary>
    ' ''' <param name="method">メソッド名</param>
    ' ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ' ''' <remarks></remarks>
    'Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean)

    '    If isStart Then
    '        Logger.Info(MY_PROGRAMID & ".ascx " & method & "_Start")
    '    Else
    '        Logger.Info(MY_PROGRAMID & ".ascx " & method & "_End")
    '    End If

    'End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = MY_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = MY_PROGRAMID & ".ascx " & method & "_End" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        End If

    End Sub

    ' ''' <summary>
    ' ''' エラーログを出力する
    ' ''' </summary>
    ' ''' <param name="method">メソッド名</param>
    ' ''' <param name="ex">例外オブジェクト</param>
    ' ''' <param name="argString">フォーマット用文字列</param>
    ' ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ' ''' <remarks></remarks>
    'Private Sub OutputErrLog(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ParamArray args() As Object)

    '    Dim logString As String = String.Empty

    '    logString = MY_PROGRAMID & ".ascx " & method & "_Error" & argString
    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

    'End Sub

#End Region

End Class
