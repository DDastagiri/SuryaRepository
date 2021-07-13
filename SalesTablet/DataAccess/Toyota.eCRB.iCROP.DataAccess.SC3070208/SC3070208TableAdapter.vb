'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070208DataSet.vb
'─────────────────────────────────────
'機能： 注文承認依頼
'補足： 
'作成： 2013/11/26 TCS 山口   Aカード情報相互連携開発
'更新： 2014/05/28 TCS 安田   受注時説明機能開発（受注後工程スケジュール）
'更新： 2015/03/17 TCS 鈴木   次世代e-CRB 価格相談履歴参照機能開発
'更新： 2017/12/20 TCS 河原   TKM独自機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)
'─────────────────────────────────────
Imports System.Text
Imports System.Reflection.MethodBase
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3070208TableAdapter

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ProgramId As String = "SC3070208"

    ''' <summary>
    ''' 契約承認ステータス 0: 未承認
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StatusAnapproved As String = "0"
    ''' <summary>
    ''' 契約承認ステータス 1: 承認依頼中
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StatusApprovalRequest As String = "1"
    ''' <summary>
    ''' 契約承認ステータス 2: 承認
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StatusApproval As String = "2"
    ''' <summary>
    ''' 契約承認ステータス 3: 否認
    ''' </summary>
    ''' <remarks></remarks>
    Public Const StatusDenial As String = "3"
#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 注文承認スタッフ一覧取得
    ''' </summary>
    ''' <param name="accountList">アカウントリスト</param>
    ''' <param name="myaccount">ログインアカウント</param>
    ''' <returns>ApprovalStaffListDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetApprovalStaffList(ByVal accountList As List(Of String), _
                                                ByVal myaccount As String) As SC3070208DataSet.SC3070208ApprovalStaffListDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        'IN句用の文字列作成
        Dim editAccount As New StringBuilder
        If accountList.Count > 0 Then
            '引数.アカウントリストをIN句用に変換
            For Each account In accountList
                editAccount.Append("'").Append(account).Append("'").Append(",")
            Next
            editAccount.Remove(editAccount.Length - 1, 1)
        End If

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_001 */ ")
            .AppendLine("        T1.ACCOUNT ")
            .AppendLine("      , T1.USERNAME ")
            .AppendLine("      , T1.PRESENCECATEGORY ")
            .AppendLine("   FROM TBL_USERS T1 ")
            .AppendLine("      , TBL_USERDISPLAY T2 ")
            .AppendLine("      , TB_M_STAFF T3 ")
            .AppendLine("  WHERE T2.ACCOUNT = T1.ACCOUNT ")
            .AppendLine("    AND T1.DELFLG = '0' ")
            .AppendLine("    AND T1.ACCOUNT IN ( ")
            .AppendLine(editAccount.ToString)
            .AppendLine("                      ) ")
            .AppendLine("    AND T3.STF_CD = T1.ACCOUNT ")
            .AppendLine("    AND T3.CONTRACT_APPROVAL_FLG = '1' ")
            .AppendLine("  ORDER BY CASE WHEN T1.ACCOUNT = :MYACCOUNT ")
            .AppendLine("        THEN 0 ELSE 1 END ")
            .AppendLine("      , CASE WHEN T1.PRESENCECATEGORY IN ('1','2','3') ")
            .AppendLine("        THEN 0 ELSE 1 END ")
            .AppendLine("      , OPERATIONCODE ")
            .AppendLine("      , T2.SORTNO ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208ApprovalStaffListDataTable)("SC3070208_001")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("MYACCOUNT", OracleDbType.NVarchar2, myaccount)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 注文承認依頼取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>ContractApprovalDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractApproval(ByVal estimateId As Long) As SC3070208DataSet.SC3070208ContractApprovalDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_002 */ ")
            .AppendLine("        CASE WHEN T2.NOTICEREQID IS NULL THEN 0 ")
            .AppendLine("             ELSE T2.NOTICEREQID ")
            .AppendLine("        END AS NOTICEREQID ")
            .AppendLine("      , T1.CONTRACT_APPROVAL_STATUS ")
            .AppendLine("      , T1.CONTRACT_APPROVAL_STAFF ")
            .AppendLine("      , T1.CONTRACT_APPROVAL_REQUESTDATE ")
            .AppendLine("      , T3.USERNAME ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 START
            .AppendLine("      , NVL(T4.STAFFMEMO, '') AS STAFFMEMO ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 END
            .AppendLine("   FROM TBL_ESTIMATEINFO T1 ")
            .AppendLine("      , TBL_NOTICEREQUEST T2 ")
            .AppendLine("      , TBL_USERS T3 ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 START
            .AppendLine("      , TBL_EST_CONTRACTAPPROVAL T4 ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 END
            .AppendLine("  WHERE T1.ESTIMATEID = :ESTIMATEID ")
            .AppendLine("    AND T1.ESTIMATEID = T2.REQCLASSID(+) ")
            .AppendLine("    AND T2.NOTICEREQCTG(+) = '08' ")
            .AppendLine("    AND T2.STATUS(+) IN ('1','3') ")
            .AppendLine("    AND T1.CONTRACT_APPROVAL_STAFF = T3.ACCOUNT(+) ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 START
            .AppendLine("    AND T2.NOTICEREQID = T4.NOTICEREQID(+) ")
            '2015/03/17 TCS 鈴木  次世代e-CRB 価格相談履歴参照機能開発 END
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208ContractApprovalDataTable)("SC3070208_002")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 入力項目設定マスタ取得
    ''' </summary>
    ''' <returns>InputItemSettingDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInputItemSetting() As SC3070208DataSet.SC3070208InputItemSettingDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_007 */ ")
            .AppendLine("        TGT_ITEM_ID ")
            .AppendLine("      , TGT_ITEM_DETAIL_ID ")
            .AppendLine("      , TGT_ITEM ")
            .AppendLine("   FROM TBL_INPUT_ITEM_SETTING ")
            .AppendLine("  WHERE CHECK_TIMING_TYPE = '04' ")
            .AppendLine("    AND DISP_SETTING_STATUS = '2' ")
            .AppendLine("  ORDER BY TGT_ITEM_ID")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208InputItemSettingDataTable)("SC3070208_007")
            query.CommandText = sql.ToString()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 入力チェック用情報取得（顧客）
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="cstId">店舗コード</param>
    ''' <returns>InputCheckCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInputCheckCustomer(ByVal dlrcd As String, ByVal cstId As Decimal) As SC3070208DataSet.SC3070208InputCheckCustomerDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        With sql
            .AppendLine(" SELECT /* SC3070208_008 */ ")
            .AppendLine("        T1.FIRST_NAME ")
            .AppendLine("      , T1.MIDDLE_NAME ")
            .AppendLine("      , T1.LAST_NAME ")
            .AppendLine("      , T1.CST_GENDER ")
            .AppendLine("      , T1.NAMETITLE_CD ")
            .AppendLine("      , T1.FLEET_FLG ")
            .AppendLine("      , NVL(T4.PRIVATE_FLEET_ITEM_CD, ' ') AS PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("      , T1.FLEET_PIC_NAME ")
            .AppendLine("      , T1.FLEET_PIC_DEPT ")
            .AppendLine("      , T1.FLEET_PIC_POSITION ")
            .AppendLine("      , T1.CST_MOBILE ")
            .AppendLine("      , T1.CST_PHONE ")
            .AppendLine("      , T1.CST_BIZ_PHONE ")
            .AppendLine("      , T1.CST_FAX ")
            .AppendLine("      , T1.CST_ZIPCD ")
            .AppendLine("      , T1.CST_ADDRESS_1 ")
            .AppendLine("      , T1.CST_ADDRESS_2 ")
            .AppendLine("      , T1.CST_ADDRESS_3 ")
            .AppendLine("      , T1.CST_ADDRESS_STATE ")
            .AppendLine("      , T1.CST_ADDRESS_DISTRICT ")
            .AppendLine("      , T1.CST_ADDRESS_CITY ")
            .AppendLine("      , T1.CST_ADDRESS_LOCATION ")
            .AppendLine("      , T1.CST_DOMICILE ")
            .AppendLine("      , T1.CST_EMAIL_1 ")
            .AppendLine("      , T1.CST_EMAIL_2 ")
            .AppendLine("      , T1.CST_COUNTRY ")
            .AppendLine("      , T1.CST_SOCIALNUM ")
            .AppendLine("      , T1.CST_BIRTH_DATE ")
            .AppendLine("      , T2.ACT_CAT_TYPE ")
            .AppendLine("      , NVL(T3.CST_TYPE, ' ') AS CST_TYPE ")
            .AppendLine("      , NVL(T5.CST_ORGNZ_CD, ' ') AS CST_ORGNZ_CD ")
            .AppendLine("      , CASE WHEN (T1.CST_ORGNZ_INPUT_TYPE = '1' AND (T4.CST_ORGNZ_NAME_INPUT_TYPE = '1' OR T4.CST_ORGNZ_NAME_INPUT_TYPE = '2')) ")
            .AppendLine("               OR (T1.CST_ORGNZ_INPUT_TYPE = '2' AND (T4.CST_ORGNZ_NAME_INPUT_TYPE = '0' OR T4.CST_ORGNZ_NAME_INPUT_TYPE = '2')) ")
            .AppendLine("             THEN TO_CHAR(NVL(T1.CST_ORGNZ_INPUT_TYPE, ' ')) ")
            .AppendLine("             ELSE ' ' ")
            .AppendLine("        END AS CST_ORGNZ_INPUT_TYPE ")
            .AppendLine("      , NVL(T1.CST_ORGNZ_NAME, ' ') AS CST_ORGNZ_NAME ")
            .AppendLine("      , CASE WHEN T1.CST_ORGNZ_INPUT_TYPE = '2' ")
            .AppendLine("             THEN TO_CHAR(NVL(T6.CST_SUBCAT2_CD, ' ')) ")
            .AppendLine("             WHEN T1.CST_ORGNZ_INPUT_TYPE = '1' ")
            .AppendLine("             THEN CASE WHEN T1.CST_ORGNZ_CD = T6.CST_ORGNZ_CD ")
            .AppendLine("                       THEN TO_CHAR(NVL(T6.CST_SUBCAT2_CD, ' ')) ")
            .AppendLine("                       ELSE ' ' ")
            .AppendLine("                  END ")
            .AppendLine("             ELSE ' ' ")
            .AppendLine("        END AS CST_SUBCAT2_CD ")
            .AppendLine("   FROM (SELECT CS.CST_ID ")
            .AppendLine("              , CS.FIRST_NAME ")
            .AppendLine("              , CS.MIDDLE_NAME ")
            .AppendLine("              , CS.LAST_NAME ")
            .AppendLine("              , CS.CST_GENDER ")
            .AppendLine("              , CS.NAMETITLE_CD ")
            .AppendLine("              , CS.FLEET_FLG ")
            .AppendLine("              , CS.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("              , CS.FLEET_PIC_NAME ")
            .AppendLine("              , CS.FLEET_PIC_DEPT ")
            .AppendLine("              , CS.FLEET_PIC_POSITION ")
            .AppendLine("              , CS.CST_MOBILE ")
            .AppendLine("              , CS.CST_PHONE ")
            .AppendLine("              , CS.CST_BIZ_PHONE ")
            .AppendLine("              , CS.CST_FAX ")
            .AppendLine("              , CS.CST_ZIPCD ")
            .AppendLine("              , CS.CST_ADDRESS_1 ")
            .AppendLine("              , CS.CST_ADDRESS_2 ")
            .AppendLine("              , CS.CST_ADDRESS_3 ")
            .AppendLine("              , CS.CST_ADDRESS_STATE ")
            .AppendLine("              , CS.CST_ADDRESS_DISTRICT ")
            .AppendLine("              , CS.CST_ADDRESS_CITY ")
            .AppendLine("              , CS.CST_ADDRESS_LOCATION ")
            .AppendLine("              , CS.CST_DOMICILE ")
            .AppendLine("              , CS.CST_EMAIL_1 ")
            .AppendLine("              , CS.CST_EMAIL_2 ")
            .AppendLine("              , CS.CST_COUNTRY ")
            .AppendLine("              , CS.CST_SOCIALNUM ")
            .AppendLine("              , CS.CST_BIRTH_DATE ")
            .AppendLine("              , LC.CST_ORGNZ_CD ")
            .AppendLine("              , LC.CST_ORGNZ_INPUT_TYPE ")
            .AppendLine("              , LC.CST_ORGNZ_NAME ")
            .AppendLine("              , LC.CST_SUBCAT2_CD ")
            .AppendLine("           FROM TB_M_CUSTOMER CS ")
            .AppendLine("              , TB_LM_CUSTOMER LC ")
            .AppendLine("          WHERE CS.CST_ID = LC.CST_ID(+) ")
            .AppendLine("        ) T1 ")
            .AppendLine("      , TB_M_CUSTOMER_VCL T2 ")
            .AppendLine("      , TB_M_CUSTOMER_DLR T3 ")
            .AppendLine("      , (SELECT PF.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("              , PF.FLEET_FLG ")
            .AppendLine("              , LP.CST_ORGNZ_NAME_INPUT_TYPE ")
            .AppendLine("           FROM TB_M_PRIVATE_FLEET_ITEM PF ")
            .AppendLine("          INNER JOIN TB_LM_PRIVATE_FLEET_ITEM LP ")
            .AppendLine("             ON PF.PRIVATE_FLEET_ITEM_CD = LP.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("            AND PF.INUSE_FLG = '1' ")
            .AppendLine("        ) T4 ")
            .AppendLine("      , (SELECT CO.CST_ORGNZ_CD ")
            .AppendLine("              , CO.CST_ORGNZ_NAME ")
            .AppendLine("              , CO.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("           FROM TB_LM_CUSTOMER_ORGANIZATION CO ")
            .AppendLine("          INNER JOIN TB_M_PRIVATE_FLEET_ITEM PF ")
            .AppendLine("             ON CO.PRIVATE_FLEET_ITEM_CD = PF.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("            AND CO.INUSE_FLG = '1' ")
            .AppendLine("            AND PF.INUSE_FLG = '1' ")
            .AppendLine("          INNER JOIN TB_LM_PRIVATE_FLEET_ITEM LP ")
            .AppendLine("             ON PF.PRIVATE_FLEET_ITEM_CD = LP.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("        ) T5 ")
            .AppendLine("      , (SELECT CS.CST_SUBCAT2_CD ")
            .AppendLine("              , CS.CST_SUBCAT2_NAME ")
            .AppendLine("              , CS.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("              , CS.CST_ORGNZ_CD ")
            .AppendLine("           FROM TB_LM_CUSTOMER_SUBCATEGORY2 CS ")
            .AppendLine("          INNER JOIN TB_M_PRIVATE_FLEET_ITEM PF ")
            .AppendLine("             ON CS.PRIVATE_FLEET_ITEM_CD = PF.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("            AND CS.INUSE_FLG = '1' ")
            .AppendLine("            AND PF.INUSE_FLG = '1' ")
            .AppendLine("          INNER JOIN TB_LM_PRIVATE_FLEET_ITEM LP ")
            .AppendLine("             ON PF.PRIVATE_FLEET_ITEM_CD = LP.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("          WHERE EXISTS ")
            .AppendLine("                (SELECT 1 ")
            .AppendLine("                   FROM TB_LM_CUSTOMER_ORGANIZATION CO ")
            .AppendLine("                  INNER JOIN TB_M_PRIVATE_FLEET_ITEM PF ")
            .AppendLine("                     ON CO.PRIVATE_FLEET_ITEM_CD = PF.PRIVATE_FLEET_ITEM_CD")
            .AppendLine("                    AND CO.INUSE_FLG = '1' ")
            .AppendLine("                    AND PF.INUSE_FLG = '1' ")
            .AppendLine("                  INNER JOIN TB_LM_PRIVATE_FLEET_ITEM LP ")
            .AppendLine("                     ON PF.PRIVATE_FLEET_ITEM_CD = LP.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("                  WHERE CS.CST_ORGNZ_CD = CO.CST_ORGNZ_CD ")
            .AppendLine("                    AND CS.PRIVATE_FLEET_ITEM_CD = CO.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("                ) ")
            .AppendLine("             OR CS.CST_ORGNZ_CD = ' '  ")
            .AppendLine("        ) T6 ")
            .AppendLine("  WHERE T1.CST_ID = :CST_ID ")
            .AppendLine("    AND T1.CST_ID = T2.CST_ID ")
            .AppendLine("    AND T2.DLR_CD = :DLR_CD ")
            .AppendLine("    AND T2.CST_VCL_TYPE = '1' ")
            .AppendLine("    AND T2.OWNER_CHG_FLG = '0' ")
            .AppendLine("    AND T1.CST_ID = T3.CST_ID ")
            .AppendLine("    AND T1.PRIVATE_FLEET_ITEM_CD = T4.PRIVATE_FLEET_ITEM_CD(+) ")
            .AppendLine("    AND T1.FLEET_FLG = T4.FLEET_FLG(+) ")
            .AppendLine("    AND T1.CST_ORGNZ_CD = T5.CST_ORGNZ_CD(+) ")
            .AppendLine("    AND T1.PRIVATE_FLEET_ITEM_CD = T5.PRIVATE_FLEET_ITEM_CD(+) ")
            .AppendLine("    AND T1.CST_SUBCAT2_CD = T6.CST_SUBCAT2_CD(+) ")
            .AppendLine("    AND T1.PRIVATE_FLEET_ITEM_CD = T6.PRIVATE_FLEET_ITEM_CD(+) ")
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208InputCheckCustomerDataTable)("SC3070208_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 入力チェック用情報取得（商談）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>InputCheckSalesDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInputCheckSales(ByVal salesId As Decimal) As SC3070208DataSet.SC3070208InputCheckSalesDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_009 */ ")
            .AppendLine("        CASE WHEN T2.SOURCE_1_CD IS NOT NULL ")
            .AppendLine("             THEN T2.SOURCE_1_CD ")
            .AppendLine("             ELSE T3.SOURCE_1_CD ")
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine("        END AS SOURCE_1_CD, ")
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine("        CASE WHEN T2.SOURCE_2_CD IS NOT NULL ")
            .AppendLine("             THEN T2.SOURCE_2_CD ")
            .AppendLine("             ELSE T3.SOURCE_2_CD ")
            .AppendLine("        END AS SOURCE_2_CD ")
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END
            .AppendLine("   FROM TB_T_SALES T1 ")
            .AppendLine("      , TB_T_REQUEST T2 ")
            .AppendLine("      , TB_T_ATTRACT T3 ")
            .AppendLine("  WHERE T1.SALES_ID = :SALES_ID ")
            .AppendLine("    AND T1.REQ_ID = T2.REQ_ID(+) ")
            .AppendLine("    AND T1.ATT_ID = T3.ATT_ID(+) ")
            .AppendLine("  UNION ALL ")
            .AppendLine(" SELECT SOURCE_1_CD ")
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            .AppendLine("      , SOURCE_2_CD ")
            .AppendLine("   FROM TB_T_SALES_TEMP T4 ")
            .AppendLine("      ,TB_LT_SALES T5 ")
            .AppendLine("  WHERE T4.SALES_ID = :SALES_ID ")
            .AppendLine("    AND T4.SALES_ID = T5.SALES_ID(+) ")
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208InputCheckSalesDataTable)("SC3070208_009")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function

    '2017/12/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 入力チェック用情報取得（希望車）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>InputCheckSelectedCarDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInputCheckSelectedCar(ByVal salesId As Decimal) As SC3070208DataSet.SC3070208InputCheckSelectedCarDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_010 */ ")
            .AppendLine("        MODEL_CD ")
            .AppendLine("      , GRADE_CD ")
            .AppendLine("      , SUFFIX_CD ")
            .AppendLine("      , BODYCLR_CD ")
            .AppendLine("      , INTERIORCLR_CD ")
            .AppendLine("   FROM TB_T_PREFER_VCL ")
            .AppendLine("  WHERE SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208InputCheckSelectedCarDataTable)("SC3070208_010")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function
    '2017/12/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 見積情報更新ロック取得
    ''' </summary>
    ''' <param name="fllwUpBoxSeqNo">Follow-up Box内連番</param>
    ''' <remarks></remarks>
    Public Shared Function GetLockEstimateInfo(ByVal fllwUpBoxSeqNo As Decimal) As SC3070208DataSet.SC3070208LockEstimateInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Try
            Dim env As New SystemEnvSetting
            Dim sql As New StringBuilder
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            With sql
                .AppendLine(" SELECT /* SC3070208_006 */ ")
                .AppendLine("        ESTIMATEID ")
                .AppendLine("      , CONTRACT_APPROVAL_STATUS ")
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
                .AppendLine("      , CONTRACT_COND_CHG_FLG ")
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
                .AppendLine("   FROM TBL_ESTIMATEINFO ")
                .AppendLine("  WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .AppendLine("    AND DELFLG = '0' ")
                .AppendLine(sqlForUpdate)
            End With

            Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208LockEstimateInfoDataTable)("SC3070208_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwUpBoxSeqNo)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

                Return query.GetData()
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 承認ステータス更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="contactApprovalStatus">契約承認ステータス</param>
    ''' <param name="staff">更新スタッフ</param>
    ''' <param name="contactApprovalStaff">契約承認スタッフ(キャンセル時は省略)</param>
    ''' <param name="contactApprovalRequestStaff">契約承認依頼スタッフ(キャンセル時は省略)</param>
    ''' <param name="dlrcd">販売店コード(キャンセル時は省略)</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateContractApprovalStatus(ByVal estimateId As Long,
                                                        ByVal contactApprovalStatus As String,
                                                        ByVal staff As String,
                                                        Optional ByVal contactApprovalStaff As String = "",
                                                        Optional ByVal contactApprovalRequestStaff As String = "",
                                                        Optional ByVal dlrcd As String = "") As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" UPDATE /* SC3070208_003 */ ")
            .AppendLine("        TBL_ESTIMATEINFO ")
            .AppendLine("    SET CONTRACT_APPROVAL_STATUS = :CONTRACT_APPROVAL_STATUS ")
            .AppendLine("      , CONTRACT_APPROVAL_STAFF = :CONTRACT_APPROVAL_STAFF ")
            .AppendLine("      , CONTRACT_APPROVAL_REQUESTDATE = :CONTRACT_APPROVAL_REQUESTDATE ")
            .AppendLine("      , CONTRACT_APPROVAL_REQUESTSTAFF = :CONTRACT_APPROVAL_REQUESTSTAFF ")
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            If StatusApprovalRequest.Equals(contactApprovalStatus) Then
                .AppendLine("      , CONTRACT_COND_CHG_FLG = :CONTRACT_COND_CHG_FLG ")
            End If
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

            .AppendLine("      , UPDATEDATE = SYSDATE ")
            .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .AppendLine("      , UPDATEID = :UPDATEID ")
            .AppendLine("  WHERE ESTIMATEID = :ESTIMATEID ")
        End With

        Using query As New DBUpdateQuery("SC3070208_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STATUS", OracleDbType.Char, contactApprovalStatus)
            query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STAFF", OracleDbType.Char, contactApprovalStaff)
            query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTSTAFF", OracleDbType.Char, contactApprovalRequestStaff)
            If StatusApprovalRequest.Equals(contactApprovalStatus) Then
                '承認依頼
                query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTDATE", OracleDbType.Date, DateTimeFunc.Now(dlrcd))
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
                query.AddParameterWithTypeValue("CONTRACT_COND_CHG_FLG", OracleDbType.Char, "0")
                '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
            Else
                '承認依頼キャンセル
                query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTDATE", OracleDbType.Date, DBNull.Value)
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, staff)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, ProgramId)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.Execute
        End Using
    End Function

    ''' <summary>
    ''' その他見積り情報削除
    ''' </summary>
    ''' <param name="fllwUpBoxSeqNo">Follow-up Box内連番</param>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="staff">更新スタッフ</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteOtherEstimate(ByVal fllwUpBoxSeqNo As Decimal,
                                               ByVal estimateId As Long,
                                               ByVal staff As String) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" UPDATE /* SC3070208_005 */ ")
            .AppendLine("        TBL_ESTIMATEINFO ")
            .AppendLine("    SET DELFLG = '1' ")
            .AppendLine("      , UPDATEDATE = SYSDATE ")
            .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
            .AppendLine("      , UPDATEID = :UPDATEID ")
            .AppendLine("  WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .AppendLine("    AND ESTIMATEID <> :ESTIMATEID ")
            .AppendLine("    AND DELFLG = '0' ")
        End With

        Using query As New DBUpdateQuery("SC3070208_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwUpBoxSeqNo)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, staff)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ProgramId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.Execute
        End Using
    End Function

    ''' <summary>
    ''' キャンセル通知対象データ取得
    ''' </summary>
    ''' <param name="fllwUpBox">Follow-up Box</param>
    ''' <returns>NoticeRequestDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNoticeRequest(ByVal fllwUpBox As Decimal) As SC3070208DataSet.SC3070208NoticeRequestDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_004 */ ")
            .AppendLine("        T.NOTICEREQID ")
            .AppendLine("      , T.TOACCOUNT ")
            .AppendLine("      , T.REQCLASSID ")
            .AppendLine("      , T.NOTICEREQCTG ")
            .AppendLine("      , T4.USERNAME ")
            .AppendLine("   FROM (SELECT T1.NOTICEREQID ")
            .AppendLine("              , DECODE(T1.STATUS,'1', T2.TOACCOUNT,'3', T2.FROMACCOUNT,'4', T2.FROMACCOUNT,'5', T2.FROMACCOUNT) AS TOACCOUNT ")
            .AppendLine("              , T1.REQCLASSID ")
            .AppendLine("              , T1.NOTICEREQCTG ")
            .AppendLine("           FROM TBL_NOTICEREQUEST T1 ")
            .AppendLine("              , TBL_NOTICEINFO T2 ")
            .AppendLine("              , TBL_ESTIMATEINFO T3 ")
            .AppendLine("          WHERE T1.LASTNOTICEID = T2.NOTICEID(+) ")
            .AppendLine("            AND T1.REQCLASSID = T3.ESTIMATEID ")
            .AppendLine("            AND T1.STATUS IN ('1','3') ")
            .AppendLine("            AND T1.NOTICEREQCTG IN ('02','08') ")
            .AppendLine("            AND T1.FLLWUPBOX = :FLLWUPBOX) T ")
            .AppendLine("      , TBL_USERS T4 ")
            .AppendLine("  WHERE T.TOACCOUNT = T4.ACCOUNT ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208NoticeRequestDataTable)("SC3070208_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX", OracleDbType.Decimal, fllwUpBox)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 入力チェック用情報取得（商談条件）
    ''' </summary>
    ''' <param name="fllwUpBox">Follow-up Box内連番</param>
    ''' <returns>InputCheckSalesConditionDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetInputCheckSalesCondition(ByVal fllwUpBox As Decimal) As SC3070208DataSet.SC3070208InputCheckSalesConditionDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_011 */ ")
            .AppendLine("        SALESCONDITIONNO ")
            .AppendLine("   FROM TBL_FLLWUPBOX_SALESCONDITION ")
            .AppendLine("  WHERE FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208InputCheckSalesConditionDataTable)("SC3070208_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwUpBox)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 商談条件マスタ取得
    ''' </summary>
    ''' <param name="salesCondition">商談条件No</param>
    ''' <returns>SalesConditionDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSalesCondition(ByVal salesCondition As Long) As SC3070208DataSet.SC3070208SalesConditionDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" SELECT /* SC3070208_012 */ ")
            .AppendLine("        TITLE ")
            .AppendLine("   FROM TBL_SALESCONDITION ")
            .AppendLine("  WHERE SALESCONDITIONNO = :SALESCONDITIONNO ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208SalesConditionDataTable)("SC3070208_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALESCONDITIONNO", OracleDbType.Int64, salesCondition)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' 受注後データ取得
    ''' </summary>
    ''' <param name="salesId">salesId</param>
    ''' <param name="afterActCd">afterActCd</param>
    ''' <returns>NoticeRequestDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOder(ByVal salesId As Decimal, ByVal afterActCd As String) As SC3070208DataSet.SC3070208AfterOderDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT  /* SC3070208_013 */ ")
            .AppendLine("  A.MODEL_CD AS ODR_MODEL_CD, ")
            .AppendLine("  D.MODELCD AS EST_MODEL_CD, ")
            .AppendLine("  B.SCHE_START_DATEORTIME ")
            .AppendLine("FROM ")
            .AppendLine("  TB_T_AFTER_ODR A, ")
            .AppendLine("  TB_T_AFTER_ODR_ACT B, ")
            .AppendLine("  TBL_ESTIMATEINFO C, ")
            .AppendLine("  TBL_EST_VCLINFO D ")
            .AppendLine("WHERE ")
            .AppendLine("      A.AFTER_ODR_ID = B.AFTER_ODR_ID ")
            .AppendLine("  AND A.SALES_ID = C.FLLWUPBOX_SEQNO ")
            .AppendLine("  AND C.ESTIMATEID = D.ESTIMATEID ")
            .AppendLine("  AND A.SALES_ID = :SALES_ID ")
            .AppendLine("  AND B.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")
            .AppendLine("  AND C.DELFLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208AfterOderDataTable)("SC3070208_013")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, afterActCd)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 受注後データのデータ件数
    ''' </summary>
    ''' <param name="salesId">salesId</param>
    ''' <returns>受注後データのデータ件数</returns>
    ''' <remarks></remarks>
    Public Shared Function CountAfterOder(ByVal salesId As Decimal) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT  /* SC3070208_014 */ ")
            .AppendLine("  1 ")
            .AppendLine("FROM ")
            .AppendLine("  TB_T_AFTER_ODR ")
            .AppendLine("WHERE ")
            .AppendLine("      SALES_ID = :SALES_ID ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3070208_014")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.GetCount()
        End Using
    End Function
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
    ''' <summary>
    ''' シーケンス取得処理
    ''' </summary>
    ''' <returns>SEQ_CONTRACTAPPROVAL_SEQNO.NEXTVAL</returns>
    ''' <remarks></remarks>
    Public Shared Function SelectSequence() As Long

        Dim sql As New StringBuilder

        With sql
            .AppendLine("SELECT /* SC3070208_015 */")
            .AppendLine("	   SEQ_CONTRACTAPPROVAL_SEQNO.NEXTVAL AS SEQNO")
            .AppendLine("  FROM DUAL")
        End With

        Using query As New DBSelectQuery(Of SC3070208DataSet.SC3070208SequenceDataTable)("SC3070208_015")
            query.CommandText = sql.ToString()

            Dim dt As SC3070208DataSet.SC3070208SequenceDataTable = query.GetData()

            Return dt(0).SEQNO
        End Using
    End Function

    ''' <summary>
    ''' 契約承認登録処理
    ''' </summary>
    ''' <returns>レコードの更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertContractApproval(ByVal EstimateId As Long, _
                                           ByVal SeqNo As Long, _
                                           ByVal DlrCd As String, _
                                           ByVal StrCd As String, _
                                           ByVal Account As String, _
                                           ByVal StaffMemo As String, _
                                           ByVal MgrAccount As String) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql

            .AppendLine("INSERT INTO /* SC3070208_016 */")
            .AppendLine("   TBL_EST_CONTRACTAPPROVAL")
            .AppendLine("(")
            .AppendLine("     ESTIMATEID")
            .AppendLine("   , SEQNO")
            .AppendLine("   , DLRCD")
            .AppendLine("   , STRCD")
            .AppendLine("   , STAFFACCOUNT")
            .AppendLine("   , STAFFMEMO")
            .AppendLine("   , REQUESTDATE")
            .AppendLine("   , MANAGERACCOUNT")
            .AppendLine("   , MANAGERMEMO")
            .AppendLine("   , APPROVEDDATE")
            .AppendLine("   , RESPONSEFLG")
            .AppendLine("   , NOTICEREQID")
            .AppendLine("   , CREATEDATE")
            .AppendLine("   , UPDATEDATE")
            .AppendLine("   , CREATEACCOUNT")
            .AppendLine("   , UPDATEACCOUNT")
            .AppendLine("   , CREATEID")
            .AppendLine("   , UPDATEID")
            .AppendLine(")")
            .AppendLine("VALUES")
            .AppendLine("(")
            .AppendLine("     :ESTIMATEID")
            .AppendLine("   , :SEQNO")
            .AppendLine("   , :DLRCD")
            .AppendLine("   , :STRCD")
            .AppendLine("   , :STAFFACCOUNT")
            .AppendLine("   , :STAFFMEMO")
            .AppendLine("   , :REQUESTDATE")
            .AppendLine("   , :MANAGERACCOUNT")
            .AppendLine("   , :MANAGERMEMO")
            .AppendLine("   , :APPROVEDDATE")
            .AppendLine("   , :RESPONSEFLG")
            .AppendLine("   , :NOTICEREQID")
            .AppendLine("   ,  SYSDATE")
            .AppendLine("   ,  SYSDATE")
            .AppendLine("   , :CREATEACCOUNT")
            .AppendLine("   , :UPDATEACCOUNT")
            .AppendLine("   , :CREATEID")
            .AppendLine("   , :UPDATEID")
            .AppendLine(")")
        End With

        Using query As New DBUpdateQuery("SC3070208_016")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, EstimateId)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, SeqNo)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, DlrCd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, StrCd)
            query.AddParameterWithTypeValue("STAFFACCOUNT", OracleDbType.Varchar2, Account)
            query.AddParameterWithTypeValue("STAFFMEMO", OracleDbType.Varchar2, StaffMemo)
            query.AddParameterWithTypeValue("REQUESTDATE", OracleDbType.Date, DateTimeFunc.Now(DlrCd))

            query.AddParameterWithTypeValue("MANAGERACCOUNT", OracleDbType.Varchar2, MgrAccount)
            query.AddParameterWithTypeValue("MANAGERMEMO", OracleDbType.Varchar2, DBNull.Value)
            query.AddParameterWithTypeValue("APPROVEDDATE", OracleDbType.Date, DBNull.Value)
            query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, StatusAnapproved)

            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Decimal, DBNull.Value)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, Account)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, Account)
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, ProgramId)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ProgramId)

            Return query.Execute()
        End Using
    End Function


    Public Shared Function UndoContractApproval(ByVal estimateId As Long) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine("DELETE /* SC3070208_018 */ ")
            .AppendLine(" FROM TBL_EST_CONTRACTAPPROVAL ")
            .AppendLine(" WHERE ESTIMATEID = :ESTIMATEID ")
            .AppendLine("   AND RESPONSEFLG = '0' ")
            .AppendLine("   AND NOTICEREQID IS NULL ")
        End With

        Using query As New DBUpdateQuery("SC3070208_018")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, estimateId)
            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 通知依頼ID更新
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNoticeid(ByVal EstimateId As Long, _
                                   ByVal Seqno As Long, _
                                   ByVal Account As String, _
                                   ByVal NoticeRequestid As Long) As Integer

        Dim sql As New StringBuilder
        With sql
            .AppendLine("UPDATE /* SC3070208_017 */")
            .AppendLine("       TBL_EST_CONTRACTAPPROVAL")
            .AppendLine("   SET NOTICEREQID = :NOTICEREQID")
            .AppendLine("     , UPDATEDATE = SYSDATE")
            .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
            .AppendLine("     , UPDATEID = :UPDATEID")
            .AppendLine(" WHERE ESTIMATEID = :ESTIMATEID")
            .AppendLine("   AND SEQNO = :SEQNO")
        End With

        Using query As New DBUpdateQuery("SC3070208_017")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, EstimateId)
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, Seqno)
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Decimal, NoticeRequestid)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, Account)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, ProgramId)

            Return query.Execute

        End Using
    End Function
    '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
End Class
