'-------------------------------------------------------------------------
'IC3800903TableAdapter.vb
'-------------------------------------------------------------------------
'機能：予約情報送信(データアクセス)
'補足：
'作成：2013/11/21 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応
'更新：
'─────────────────────────────────────

Imports Oracle.DataAccess.Client
Imports System.Text
Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.DataAccess.IC3800903DataSet

Public Class IC3800903TableAdapter
    Implements IDisposable

#Region "Select"

    ''' <summary>
    ''' サービス基幹連携送信設定から送信フラグを取得する
    ''' </summary>
    ''' <param name="inDealerCD">販売店コード</param>
    ''' <param name="inBranchCD">店舗コード</param>
    ''' <param name="inAllDealerCD">全販売店を示すコード</param>
    ''' <param name="inAllBranchCD">全店舗を示すコード</param>
    ''' <param name="inInterfaceType">インターフェース区分(1:予約送信/2:ステータス送信/3:作業実績送信)</param>
    ''' <param name="inPrevStatus">更新前サービス連携ステータス</param>
    ''' <param name="inCrntStatus">更新後サービス連携ステータス</param>
    ''' <returns>IC3800903LinkSendSettingsDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetLinkSettings(ByVal inDealerCD As String, _
                                    ByVal inBranchCD As String, _
                                    ByVal inAllDealerCD As String, _
                                    ByVal inAllBranchCD As String, _
                                    ByVal inInterfaceType As String, _
                                    ByVal inPrevStatus As String, _
                                    ByVal inCrntStatus As String) As IC3800903LinkSendSettingsDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S inDealerCD={1}, inBranchCD={2}, inAllDealerCD={3}, inAllBranchCD={4}, inInterfaceType={5}, inPrevStatus={6}, inCrntStatus={7}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inDealerCD, _
                                  inBranchCD, _
                                  inAllDealerCD, _
                                  inAllBranchCD, _
                                  inInterfaceType, _
                                  inPrevStatus, _
                                  inCrntStatus))

        Dim sql As New StringBuilder
        With sql
            .AppendLine("   SELECT /* IC3800903_001 */ ")
            .AppendLine(" 		   SEND_FLG ")
            .AppendLine(" 		 , DLR_CD ")
            .AppendLine(" 		 , BRN_CD ")
            .AppendLine(" 		 , WALKIN_SEND_FLG ")
            .AppendLine(" 		 , PDS_SEND_FLG ")
            .AppendLine("     FROM ")
            .AppendLine(" 		   TB_M_SVC_LINK_SEND_SETTING ")
            .AppendLine("    WHERE ")
            .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
            .AppendLine(" 	   AND INTERFACE_TYPE = :INTERFACE_TYPE ")
            .AppendLine(" 	   AND BEFORE_SVC_LINK_STATUS = :BEFORE_SVC_LINK_STATUS ")
            .AppendLine(" 	   AND AFTER_SVC_LINK_STATUS = :AFTER_SVC_LINK_STATUS ")
            .AppendLine(" ORDER BY ")
            .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
        End With

        Dim returnTable As IC3800903LinkSendSettingsDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903LinkSendSettingsDataTable)("IC3800903_001")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCD)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, inAllDealerCD)
            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, inAllBranchCD)
            query.AddParameterWithTypeValue("INTERFACE_TYPE", OracleDbType.NVarchar2, inInterfaceType)
            query.AddParameterWithTypeValue("BEFORE_SVC_LINK_STATUS", OracleDbType.NVarchar2, inPrevStatus)
            query.AddParameterWithTypeValue("AFTER_SVC_LINK_STATUS", OracleDbType.NVarchar2, inCrntStatus)

            returnTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  returnTable.Rows.Count))

        Return returnTable

    End Function

    ''' <summary>
    ''' 顧客、車両、販売店車両、販売店顧客車両情報を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSendDmsCstVclInfo(ByVal svcInId As Decimal, _
                                         ByVal dealerCD As String) As IC3800903DmsSendCstVclInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S svcInId={1}, dealerCD={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  dealerCD))

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_002 */ ")
            .AppendLine(" 	      B.DMS_CST_CD ")                                               '基幹顧客コード
            .AppendLine("       , B.NEWCST_CD ")                                                '未取引客コード
            .AppendLine(" 	    , B.CST_NAME ")                                                 '顧客氏名
            .AppendLine(" 	    , B.CST_PHONE ")                                                '顧客電話番号
            .AppendLine(" 	    , B.CST_MOBILE ")                                               '顧客携帯電話番号
            .AppendLine(" 	    , B.CST_EMAIL_1 ")                                              '顧客EMAILアドレス1
            .AppendLine(" 		, B.CST_ZIPCD ")                                                '顧客郵便番号
            .AppendLine(" 		, B.CST_ADDRESS ")                                              '顧客住所
            .AppendLine(" 		, C.VCL_VIN ")                                                  'VIN(車両マスタ)
            .AppendLine(" 		, C.VCL_KATASHIKI ")                                            '車両型式
            .AppendLine(" 		, C.MODEL_CD ")                                                 'モデルコード
            .AppendLine(" 		, D.REG_NUM ")                                                  '車両登録番号
            .AppendLine(" 		, E.CST_VCL_TYPE ")                                             '顧客車両区分
            .AppendLine(" 		, E.SVC_PIC_STF_CD ")                                           'サービス担当スタッフコード
            .AppendLine(" 		, NVL(TRIM(F.MODEL_NAME), C.NEWCST_MODEL_NAME) MODEL_NAME ")    'モデル名
            .AppendLine(" 		, G.VCL_VIN VCL_VIN_SALESBOOKING ")                             'VIN(注文マスタ)
            .AppendLine(" 		, H.CST_TYPE  ")                                                '顧客種別
            .AppendLine("   FROM  ")
            .AppendLine(" 		  TB_T_SERVICEIN A ")                                           'サービス入庫
            .AppendLine(" 	    , TB_M_CUSTOMER B ")                                            '顧客
            .AppendLine(" 	    , TB_M_VEHICLE C ")                                             '車両
            .AppendLine(" 	    , TB_M_VEHICLE_DLR D ")                                         '販売店車両
            .AppendLine(" 	    , TB_M_CUSTOMER_VCL E ")                                        '販売店顧客車両
            .AppendLine(" 	    , TB_M_CUSTOMER_DLR H ")                                        '販売店顧客
            .AppendLine(" 	    , TB_M_MODEL F ")                                               'モデルマスタ
            .AppendLine(" 	    , TB_T_SALESBOOKING G ")                                        '注文
            .AppendLine("  WHERE  A.CST_ID = B.CST_ID ")
            .AppendLine("    AND  A.VCL_ID = C.VCL_ID ")
            .AppendLine("    AND  A.VCL_ID = D.VCL_ID ")
            .AppendLine("    AND  A.CST_ID = E.CST_ID ")
            .AppendLine("    AND  A.VCL_ID = E.VCL_ID ")
            .AppendLine("    AND  A.DLR_CD = H.DLR_CD ")
            .AppendLine("    AND  A.CST_ID = H.CST_ID ")
            .AppendLine("    AND  C.MODEL_CD = F.MODEL_CD(+) ")
            .AppendLine("    AND  D.DLR_CD = G.DLR_CD(+) ")
            .AppendLine("    AND  D.SALESBKG_NUM = G.SALESBKG_NUM(+) ")
            .AppendLine("    AND  A.SVCIN_ID = :SVCIN_ID ")
            .AppendLine("    AND  D.DLR_CD = :DLR_CD ")
            .AppendLine("    AND  E.DLR_CD = :DLR_CD ")
            .AppendLine("    AND  H.DLR_CD = :DLR_CD ")
        End With

        Dim getTable As IC3800903DmsSendCstVclInfoDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903DmsSendCstVclInfoDataTable)("IC3800903_002")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)

            getTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  getTable.Rows.Count))

        Return getTable

    End Function

    ''' <summary>
    ''' 予約情報、サービス分類情報を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSendDmsReserveInfo(ByVal svcInId As Decimal, _
                                          ByVal prevCancelJobDtlIdList As List(Of Decimal)) As IC3800903DmsSendReserveInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S svcInId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId))

        '予約情報の絞込み文字列を作成する
        Dim selectString As New StringBuilder

        If IsNothing(prevCancelJobDtlIdList) Then
            '元々キャンセルだった作業内容IDのリストがない場合
            selectString.Append("-1")
        Else
            '元々キャンセルだった作業内容IDのリストがある場合、
            'それらの作業内容IDに該当する予約情報は除く
            For Each canceledJobDtlId In prevCancelJobDtlIdList
                selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                selectString.Append(",")
            Next
            '最後のカンマを削除
            selectString.Remove(selectString.Length - 1, 1)
        End If

        Dim sql As New StringBuilder
        With sql
            .AppendLine("   SELECT /* IC3800903_003 */ ")
            .AppendLine("   	    A.SVCIN_MILE ")                                                                   '入庫時走行距離
            .AppendLine("         , A.CARWASH_NEED_FLG ")                                                             '洗車必要フラグ
            .AppendLine("   	  , A.PICK_DELI_TYPE ")                                                               '引取納車区分
            .AppendLine("   	  , A.PIC_SA_STF_CD ")                                                                '担当SAスタッフコード
            .AppendLine("   	  , A.RESV_STATUS ")                                                                  '予約ステータス
            .AppendLine("   	  , A.ACCEPTANCE_TYPE ")                                                              '受付区分
            .AppendLine("   	  , A.SMS_TRANSMISSION_FLG ")                                                         'SMS送信可フラグ
            .AppendLine("   	  , A.SVCIN_CREATE_TYPE ")                                                            'サービス入庫作成元区分
            .AppendLine("   	  , A.SCHE_SVCIN_DATETIME ")                                                          '入庫予定日時
            .AppendLine("   	  , A.SCHE_DELI_DATETIME ")                                                           '納車予定日時
            .AppendLine("   	  , A.ROW_LOCK_VERSION ")                                                             'サービス入庫テーブル.行ロックバージョン
            .AppendLine("   	  , B.INSPECTION_NEED_FLG ")                                                          '検査必要フラグ
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 START
            '.AppendLine("   	  , B.MERC_ID ")                                                                      '表示商品ID
            .AppendLine("   	  , G.MERC_CD ")                                                                      '商品コード
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 END
            .AppendLine("   	  , B.MAINTE_CD ")                                                                    '整備コード
            .AppendLine("   	  , B.JOB_DTL_ID ")                                                                   '作業内容ID
            .AppendLine("   	  , B.DMS_JOB_DTL_ID ")                                                               '基幹作業内容ID
            .AppendLine("   	  , B.JOB_DTL_MEMO ")                                                                 '作業内容メモ
            .AppendLine("   	  , B.CREATE_STF_CD  ")                                                               '作成スタッフコード
            .AppendLine("   	  , B.UPDATE_STF_CD ")                                                                '更新スタッフコード
            .AppendLine("    	  , B.CANCEL_FLG ")                                                                   'キャンセルフラグ
            .AppendLine("   	  , B.CREATE_DATETIME ")                                                              '作成日時
            .AppendLine("   	  , B.UPDATE_DATETIME ")                                                              '更新日時
            .AppendLine("   	  , C.SCHE_START_DATETIME ")                                                          '予定開始日時
            .AppendLine("   	  , C.SCHE_END_DATETIME ")                                                            '予定終了日時
            .AppendLine("   	  , C.SCHE_WORKTIME ")                                                                '予定作業時間
            .AppendLine("   	  , C.STALL_ID ")                                                                     'ストールID
            .AppendLine("   	  , C.TEMP_FLG ")                                                                     '仮置きフラグ
            .AppendLine("   	  , C.STALL_USE_STATUS ")                                                             'ストール利用ステータス
            .AppendLine("   	  , D.SVC_CLASS_CD ")                                                                 'サービス分類コード
            .AppendLine("   	  , NVL(TRIM(D.SVC_CLASS_NAME), D.SVC_CLASS_NAME_ENG) SVC_CLASS_NAME")                'サービス分類名称
            .AppendLine("   	  , E.PICK_PREF_DATETIME ")                                                           '引取希望日時
            .AppendLine("   	  , E.PICK_DESTINATION ")                                                             '引取先
            .AppendLine("   	  , E.PICK_WORKTIME ")                                                                '引取作業時間
            .AppendLine("   	  , F.DELI_PREF_DATETIME ")                                                           '配送希望日時
            .AppendLine("   	  , F.DELI_DESTINATION ")                                                             '配送先
            .AppendLine("   	  , F.DELI_WORKTIME ")                                                                '配送作業時間
            .AppendLine("     FROM   ")
            .AppendLine("   	    TB_T_SERVICEIN A  ")                                                              'サービス入庫
            .AppendLine("   	  , TB_T_JOB_DTL B  ")                                                                '作業内容
            .AppendLine("   	  , TB_T_STALL_USE C  ")                                                              'ストール利用
            .AppendLine("   	  , TB_M_SERVICE_CLASS D  ")                                                          'サービス分類マスタ
            .AppendLine("   	  , TB_T_VEHICLE_PICKUP E ")                                                          '車両引取
            .AppendLine("   	  , TB_T_VEHICLE_DELIVERY F ")                                                        '車両配送
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 START
            .AppendLine("   	  , TB_M_BRANCH_MERCHANDISE G ")                                                      '店舗商品
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 END
            .AppendLine("    WHERE  A.SVCIN_ID = B.SVCIN_ID ")
            .AppendLine("      AND  B.JOB_DTL_ID = C.JOB_DTL_ID ")
            .AppendLine("      AND  B.SVC_CLASS_ID = D.SVC_CLASS_ID(+) ")
            .AppendLine("      AND  A.SVCIN_ID = E.SVCIN_ID(+) ")
            .AppendLine("      AND  A.SVCIN_ID = F.SVCIN_ID(+)  ")
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 START
            .AppendLine("      AND  B.DLR_CD = G.DLR_CD(+)  ")
            .AppendLine("      AND  B.BRN_CD = G.BRN_CD(+)  ")
            .AppendLine("      AND  B.MERC_ID = G.MERC_ID(+)  ")
            '2014/04/23 TMEJ 明瀬 サービスタブレットDMS連携コード変換対応 END
            .AppendLine("      AND  A.SVCIN_ID = :SVCIN_ID ")
            .AppendLine("      AND  C.STALL_USE_ID = ( SELECT MAX(STALL_USE_ID) ")
            .AppendLine("                                FROM TB_T_STALL_USE ")
            .AppendLine("                               WHERE B.JOB_DTL_ID = JOB_DTL_ID ) ")
            .AppendLine("      AND  B.JOB_DTL_ID NOT IN ( ")
            .AppendLine(selectString.ToString())
            .AppendLine("                               ) ")
            .AppendLine(" ORDER BY ")
            .AppendLine("           B.JOB_DTL_ID ASC ")
        End With

        Dim getTable As IC3800903DmsSendReserveInfoDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903DmsSendReserveInfoDataTable)("IC3800903_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

            getTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  getTable.Rows.Count))

        Return getTable

    End Function

    ''' <summary>
    ''' 自分の作業内容IDの子予約連番を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' キャンセルでない関連チップを作業内容IDの昇順で抽出し、
    ''' 抽出データに対して連番を1から割り当てる。
    ''' 自分の作業内容IDに割り当てられた連番を子予約連番とする。
    ''' </remarks>
    Public Function GetRezChildNo(ByVal svcInId As Decimal, _
                                  ByVal jobDtlId As Decimal, _
                                  ByVal prevCancelJobDtlIdList As List(Of Decimal)) As IC3800903NumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S svcInId={1}, jobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  jobDtlId))

        '予約情報の絞込み文字列を作成する
        Dim selectString As New StringBuilder

        If IsNothing(prevCancelJobDtlIdList) Then
            '元々キャンセルだった作業内容IDのリストがない場合
            selectString.Append("-1")
        Else
            '元々キャンセルだった作業内容IDのリストがある場合、
            'それらの作業内容IDに該当する予約情報は除く
            For Each canceledJobDtlId In prevCancelJobDtlIdList
                selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                selectString.Append(",")
            Next
            '最後のカンマを削除
            selectString.Remove(selectString.Length - 1, 1)
        End If

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_004 */ ")
            .AppendLine(" 		 REZCHILDNO COL1 ")
            .AppendLine("   FROM ( ")
            .AppendLine(" 		     SELECT  ")
            .AppendLine(" 			          ROWNUM REZCHILDNO ")
            .AppendLine(" 			        , JOB_DTL_ID ")
            .AppendLine(" 		       FROM ( ")
            .AppendLine(" 					   SELECT JOB_DTL_ID, CANCEL_FLG ")
            .AppendLine(" 					     FROM TB_T_JOB_DTL ")
            .AppendLine(" 					    WHERE SVCIN_ID = :SVCIN_ID ")
            .AppendLine(" 					      AND JOB_DTL_ID NOT IN ( ")
            .AppendLine(selectString.ToString())
            .AppendLine(" 					                            ) ")
            .AppendLine(" 			         ORDER BY JOB_DTL_ID ASC ")
            .AppendLine(" 				    )  ")
            .AppendLine(" 	     ) A ")
            .AppendLine("  WHERE A.JOB_DTL_ID = :JOB_DTL_ID ")
        End With

        Dim getTable As IC3800903NumberValueDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903NumberValueDataTable)("IC3800903_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)

            getTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  getTable.Rows.Count))

        Return getTable

    End Function

    ''' <summary>
    ''' 自分が子チップかどうかの情報を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJudgeChildChip(ByVal svcInId As Decimal, _
                                      ByVal jobDtlId As Decimal, _
                                      ByVal prevCancelJobDtlIdList As List(Of Decimal)) As IC3800903NumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S svcInId={1}, jobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId, _
                                  jobDtlId))

        '予約情報の絞込み文字列を作成する
        Dim selectString As New StringBuilder

        If IsNothing(prevCancelJobDtlIdList) Then
            '元々キャンセルだった作業内容IDのリストがない場合
            selectString.Append("-1")
        Else
            '元々キャンセルだった作業内容IDのリストがある場合、
            'それらの作業内容IDに該当する予約情報は除く
            For Each canceledJobDtlId In prevCancelJobDtlIdList
                selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                selectString.Append(",")
            Next
            '最後のカンマを削除
            selectString.Remove(selectString.Length - 1, 1)
        End If

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_005 */ ")
            .AppendLine(" 		 JOB_DTL_ID ")
            .AppendLine("   FROM ")
            .AppendLine(" 		 TB_T_JOB_DTL ")
            .AppendLine("  WHERE ")
            .AppendLine(" 		 SVCIN_ID = :SVCIN_ID ")
            .AppendLine(" 	 AND JOB_DTL_ID < :JOB_DTL_ID ")
            .AppendLine("    AND JOB_DTL_ID NOT IN ( ")
            .AppendLine(selectString.ToString())
            .AppendLine("                          ) ")
        End With

        Dim getTable As IC3800903NumberValueDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903NumberValueDataTable)("IC3800903_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)

            getTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E retValue={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  getTable.Rows.Count))

        Return getTable

    End Function

    ''' <summary>
    ''' 関連チップ内で最も小さい作業内容ID(管理作業内容ID)を取得する
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMinimumJobDtlId(ByVal svcInId As Decimal, _
                                       ByVal prevCancelJobDtlIdList As List(Of Decimal)) As IC3800903NumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S svcInId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcInId))

        '予約情報の絞込み文字列を作成する
        Dim selectString As New StringBuilder

        If IsNothing(prevCancelJobDtlIdList) Then
            '元々キャンセルだった作業内容IDのリストがない場合
            selectString.Append("-1")
        Else
            '元々キャンセルだった作業内容IDのリストがある場合、
            'それらの作業内容IDに該当する予約情報は除く
            For Each canceledJobDtlId In prevCancelJobDtlIdList
                selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                selectString.Append(",")
            Next
            '最後のカンマを削除
            selectString.Remove(selectString.Length - 1, 1)
        End If

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_006 */ ")
            .AppendLine(" 		 MIN(JOB_DTL_ID) COL1 ")
            .AppendLine("   FROM ")
            .AppendLine(" 		 TB_T_JOB_DTL ")
            .AppendLine("  WHERE ")
            .AppendLine(" 		 SVCIN_ID = :SVCIN_ID ")
            .AppendLine("    AND JOB_DTL_ID NOT IN ( ")
            .AppendLine(selectString.ToString())
            .AppendLine("                          ) ")

        End With

        Dim getTable As IC3800903NumberValueDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3800903NumberValueDataTable)("IC3800903_006")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcInId)

            getTable = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E Count={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  getTable.Rows.Count))

        Return getTable

    End Function

    ''' <summary>
    ''' 関連チップが存在するか否かの情報を取得する
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="prevCancelJobDtlIdList">元々キャンセルだった作業内容IDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJudgeExistRelationChipInfo(ByVal svcinId As Decimal, _
                                                  ByVal prevCancelJobDtlIdList As List(Of Decimal)) As IC3800903NumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S. svcinId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcinId))

        '予約情報の絞込み文字列を作成する
        Dim selectString As New StringBuilder

        If IsNothing(prevCancelJobDtlIdList) Then
            '元々キャンセルだった作業内容IDのリストがない場合
            selectString.Append("-1")
        Else
            '元々キャンセルだった作業内容IDのリストがある場合、
            'それらの作業内容IDに該当する予約情報は除く
            For Each canceledJobDtlId In prevCancelJobDtlIdList
                selectString.Append(canceledJobDtlId.ToString(CultureInfo.CurrentCulture))
                selectString.Append(",")
            Next
            '最後のカンマを削除
            selectString.Remove(selectString.Length - 1, 1)
        End If

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_007 */ ")
            .AppendLine("        JOB_DTL_ID COL1 ")
            .AppendLine("   FROM ")
            .AppendLine("        TB_T_JOB_DTL ")
            .AppendLine("  WHERE  ")
            .AppendLine("        SVCIN_ID = :SVCIN_ID ")
            .AppendLine("    AND JOB_DTL_ID NOT IN ( ")
            .AppendLine(selectString.ToString())
            .AppendLine("                          ) ")

        End With

        Dim tblResult As IC3800903NumberValueDataTable
        Using query As New DBSelectQuery(Of IC3800903NumberValueDataTable)("IC3800903_007")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)

            tblResult = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  tblResult.Rows.Count))

        Return tblResult

    End Function

    ''' <summary>
    ''' 予約送信を行うか否かを判断する情報を取得する
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>IC3800903JudgeSendReserveDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetJudgeSendReserveData(ByVal svcinId As Decimal, _
                                            ByVal jobDtlId As Decimal) As IC3800903JudgeSendReserveDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S. svcinId={1}, jobDtlId={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcinId, _
                                  jobDtlId))

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" SELECT /* IC3800903_008 */ ")
            .AppendLine("        A.ACCEPTANCE_TYPE ")
            .AppendLine("      , A.RESV_STATUS ")
            .AppendLine(" 	   , C.SVC_CLASS_TYPE ")
            .AppendLine("   FROM ")
            .AppendLine("        TB_T_SERVICEIN A ")
            .AppendLine(" 	   , TB_T_JOB_DTL B ")
            .AppendLine(" 	   , TB_M_SERVICE_CLASS C ")
            .AppendLine("  WHERE ")
            .AppendLine(" 	     A.SVCIN_ID = B.SVCIN_ID ")
            .AppendLine("    AND B.SVC_CLASS_ID = C.SVC_CLASS_ID(+) ")
            .AppendLine("    AND A.SVCIN_ID = :SVCIN_ID ")
            .AppendLine("    AND B.JOB_DTL_ID = :JOB_DTL_ID ")
        End With

        Dim tblResult As IC3800903JudgeSendReserveDataTable
        Using query As New DBSelectQuery(Of IC3800903JudgeSendReserveDataTable)("IC3800903_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, svcinId)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)

            tblResult = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E RowCount={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  tblResult.Rows.Count))

        Return tblResult

    End Function

#End Region

#Region "Update"

    ''' <summary>
    ''' 基幹作業内容IDを更新する
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="newDmsJobDtlId">基幹作業内容ID</param>
    ''' <param name="account">ログインアカウント</param>
    ''' <param name="nowDataTime">現在日時</param>
    ''' <param name="functionId">機能ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDmsJobDtlId(ByVal jobDtlId As Decimal, _
                                      ByVal newDmsJobDtlId As String, _
                                      ByVal account As String, _
                                      ByVal nowDataTime As Date, _
                                      ByVal functionId As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S. jobDtlId={1}, newDmsJobDtlId={2}, account={3}, nowDataTime={4}, functionId={5}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  jobDtlId, _
                                  newDmsJobDtlId, _
                                  account, _
                                  nowDataTime, _
                                  functionId))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* IC3800903_201 */ ")
            .Append("       TB_T_JOB_DTL ")
            .Append("    SET ")
            .Append("       DMS_JOB_DTL_ID = :DMS_JOB_DTL_ID ")             '基幹作業内容ID
            .Append("     , UPDATE_DATETIME = :UPDATE_DATETIME ")           '更新日時
            .Append("     , UPDATE_STF_CD = :ACCOUNT ")                     '更新スタッフコード
            .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")       '行更新日時
            .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")                '行更新アカウント
            .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")   '行更新機能
            .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")      '行ロックバージョン
            .Append("  WHERE ")
            .Append("       JOB_DTL_ID = :JOB_DTL_ID ")
        End With

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("IC3800903_201")

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, newDmsJobDtlId)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDataTime)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, functionId)
            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.Decimal, jobDtlId)

            'SQL実行
            Dim result As Integer = query.Execute()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_E. result={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      result))

            Return result

        End Using

    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
