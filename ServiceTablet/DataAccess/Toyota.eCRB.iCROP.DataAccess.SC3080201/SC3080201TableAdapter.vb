'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080201TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/01/27 TCS 河原 【SALES_1B】
'更新： 2012/04/24 TCS 河原 【SALES_2】営業キャンセルでシステムエラー (号口課題 No.111) 本対応
'更新： 2012/06/01 TCS 河原 FS開発
'更新： 2012/12/10 TCS 坪根 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2013/06/30 TCS 庄   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Public NotInheritable Class SC3080201TableAdapter

#Region "定数"
    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORGCUSTFLG As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCUSTFLG As String = "2"

    ''' <summary>
    ''' 仮DLRCD、STRCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DLRCDXXXXX As String = "XXXXX"
    Private Const STRCDXXX As String = "XXX"

    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' コンタクト履歴タブ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTACTHISTORY_TAB_ALL As String = "0"
    Public Const CONTACTHISTORY_TAB_SALES As String = "1"
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    Public Const CONTACTHISTORY_TAB_SERVICE As String = "2"
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
    Public Const CONTACTHISTORY_TAB_CR As String = "3"


    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    Public Const C_PARAMKEY_MILEAGE_SHARE = "MILEAGE_SHARE"
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END


    ''' <summary>
    ''' 固定STRCD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRCD000 As String = "000"
    '2012/02/15 TCS 山口 【SALES_2】 END
#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 活動に対する誘致先の顧客情報取得
    ''' </summary>
    ''' <param name="custId">顧客ID</param>
    ''' <param name="dlr_cd">販売店コード</param>
    ''' <param name="vcl_id">車両ID</param>
    ''' <returns>顧客担当セールススタッフ</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfo(ByVal custKind As String, _
                                       ByVal custId As String, _
                                       ByVal dlr_cd As String, _
                                       ByVal vcl_id As String) As SC3080201DataSet.SC3080201CustInfoDataTable

        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustInfoDataTable)("SC3080201_100")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustInfo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_100 */ ")
                .Append("    DLR_CD AS DLRCD ")
                .Append("  , SLS_PIC_BRN_CD AS STRCD ")
                .Append("  , 0 AS FLLWUPBOX_SEQNO ")
                .Append("  , SLS_PIC_STF_CD AS STAFFCD ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      CST_ID = :CUSTID ")
                .Append("  AND DLR_CD = :DLR_CD ")
                If Not (vcl_id Is Nothing) Then
                    .Append("  AND VCL_ID = :VCL_ID ")
                End If
                If custKind.Equals("1") Then
                    '自社客
                    With sql
                        .Append("  AND CST_VCL_TYPE = '1' ")
                    End With
                ElseIf custKind.Equals("2") Then
                    '未取引客
                    With sql
                        .Append("  AND CST_VCL_TYPE = '2' ")
                    End With
                Else
                    '引数エラー
                    Throw New ArgumentException(GetType(SC3080201TableAdapter).FullName, "custKind")
                End If
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, custId)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlr_cd)
            If Not (vcl_id Is Nothing) Then
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vcl_id)
            End If
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            '検索結果返却
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="vcl_id">車両ID</param>
    ''' <returns>SC3080201OrgCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgCustomer(ByVal dlrcd As String, _
                                   ByVal originalid As String, _
                                   ByVal vcl_id As String) As SC3080201DataSet.SC3080201OrgCustomerDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201OrgCustomerDataTable)("SC3080201_101")
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetOrgCustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_101 */ ")
                .Append("  T3.IMG_FILE_LARGE AS IMAGEFILE_L , ")
                .Append("  T3.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
                .Append("  T3.IMG_FILE_SMALL AS IMAGEFILE_S , ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE , ")
                .Append("  T1.CST_NAME AS NAME , ")
                .Append("  T1.CST_ID AS CUSTCD , ")
                .Append("  T1.CST_PHONE AS TELNO , ")
                .Append("  T1.CST_MOBILE AS MOBILE , ")
                .Append("  T1.CST_ZIPCD AS ZIPCODE , ")
                .Append("  T1.CST_ADDRESS AS ADDRESS , ")
                .Append("  T1.CST_EMAIL_1 AS EMAIL1 , ")
                If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
                    .Append("  ' ' AS STAFFCD , ")
                    .Append("  ' ' AS USERNAME , ")
                Else
                    .Append("  T2.SLS_PIC_STF_CD AS STAFFCD , ")
                    .Append("  T4.USERNAME AS USERNAME , ")
                End If
                .Append("  T1.CST_BIRTH_DATE AS BIRTHDAY , ")
                .Append("  T3.FAMILY_AMOUNT AS NUMBEROFFAMILY , ")
                .Append("  CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T3.SNS_1_ACCOUNT AS SNSID_RENREN , ")
                .Append("  T3.SNS_2_ACCOUNT AS SNSID_KAIXIN , ")
                .Append("  T3.SNS_3_ACCOUNT AS SNSID_WEIBO , ")
                .Append("  T3.INTERNET_KEYWORD AS KEYWORD , ")
                .Append("  T1.ROW_LOCK_VERSION AS CUSTOMERLOCKVERSION , ")
                .Append("  T3.ROW_LOCK_VERSION AS CUSTOMERDLRLOCKVERSION ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER T1 , ")
                If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
                Else
                    .Append("  TB_M_CUSTOMER_VCL T2 , ")
                    .Append("  TBL_USERS T4 , ")
                End If
                .Append("  TB_M_CUSTOMER_DLR T3 ")
                .Append("WHERE ")
                .Append("     T3.DLR_CD = :DLRCD ")
                .Append(" AND T1.CST_ID = :ORIGINALID ")
                .Append(" AND T1.CST_ID(+) = T3.CST_ID ")
                If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
                Else
                    .Append(" AND T2.VCL_ID = :VCL_ID ")
                    .Append(" AND T2.CST_VCL_TYPE = '1' ")
                    .Append(" AND T3.DLR_CD(+) = T2.DLR_CD ")
                    .Append(" AND T3.CST_ID(+) = T2.CST_ID ")
                    .Append(" AND RTRIM(T4.ACCOUNT(+)) = T2.SLS_PIC_STF_CD ")
                    .Append(" AND T4.DELFLG(+) = '0' ")
                End If
                .Append(" AND T3.CST_TYPE = '1' ")

            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
            Else
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vcl_id)
            End If
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetOrgCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="vcl_id">車両ID</param>
    ''' <returns>SC3080201NewCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomer(ByVal dlrcd As String, _
                                          ByVal cstid As String, _
                                          ByVal vcl_id As String) As SC3080201DataSet.SC3080201NewCustomerDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201NewCustomerDataTable)("SC3080201_109")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewCustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_109 */ ")
                .Append("  T2.IMG_FILE_LARGE AS IMAGEFILE_L , ")
                .Append("  T2.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
                .Append("  T2.IMG_FILE_SMALL AS IMAGEFILE_S , ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE , ")
                .Append("  T1.CST_NAME AS NAME , ")
                .Append("  ' ' AS ORIGINALCUSTCODE , ")
                .Append("  T1.CST_PHONE AS TELNO , ")
                .Append("  T1.CST_MOBILE AS MOBILE , ")
                .Append("  T1.CST_ZIPCD AS ZIPCODE , ")
                .Append("  T1.CST_ADDRESS AS ADDRESS , ")
                .Append("  T1.CST_EMAIL_1 AS EMAIL1 , ")
                .Append("  T3.SLS_PIC_STF_CD AS STAFFCD , ")
                .Append("  T4.USERNAME , ")
                .Append("  T3.SVC_PIC_STF_CD AS SACODE , ")
                .Append("  T5.USERNAME AS SAUSERNAME , ")
                .Append("  T1.CST_BIRTH_DATE AS BIRTHDAY , ")
                .Append("  T2.FAMILY_AMOUNT AS NUMBEROFFAMILY , ")
                .Append("  CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T2.SNS_1_ACCOUNT AS SNSID_RENREN , ")
                .Append("  T2.SNS_2_ACCOUNT AS SNSID_KAIXIN , ")
                .Append("  T2.SNS_3_ACCOUNT AS SNSID_WEIBO , ")
                .Append("  T2.INTERNET_KEYWORD AS KEYWORD , ")
                .Append("  T1.ROW_LOCK_VERSION AS CUSTOMERLOCKVERSION , ")
                .Append("  T2.ROW_LOCK_VERSION AS CUSTOMERDLRLOCKVERSION ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER T1 , ")
                .Append("  TB_M_CUSTOMER_DLR T2 , ")
                .Append("  TB_M_CUSTOMER_VCL T3 , ")
                .Append("  TBL_USERS T4 , ")
                .Append("  TBL_USERS T5 ")
                .Append("WHERE ")
                .Append("      T2.DLR_CD = :DLRCD ")
                .Append("  AND T1.CST_ID = :CSTID ")
                If vcl_id <> String.Empty Then
                    .Append("  AND T3.VCL_ID = :VCL_ID ")
                End If
                .Append("  AND T3.CST_VCL_TYPE = '1' ")
                .Append("  AND T1.CST_ID(+) = T2.CST_ID ")
                .Append("  AND T2.CST_TYPE = '2' ")
                .Append("  AND T2.DLR_CD(+) = T3.DLR_CD ")
                .Append("  AND T2.CST_ID(+) = T3.CST_ID ")
                .Append("  AND RTRIM(T4.ACCOUNT(+)) = T3.SLS_PIC_STF_CD ")
                .Append("  AND T4.DELFLG(+) = '0' ")
                .Append("  AND T5.ACCOUNT(+) = T3.SVC_PIC_STF_CD ")
                .Append("  AND T5.DELFLG(+) = '0' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            If vcl_id <> String.Empty Then
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vcl_id)
            End If
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客車両取得
    ''' </summary>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3080201OrgVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgCustomerVehicle(ByVal cstid As String, ByVal dlrcd As String) As SC3080201DataSet.SC3080201OrgVehicleDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201OrgVehicleDataTable)("SC3080201_113")
            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetOrgCustomerVehicle_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_113 */ ")
                .Append("  T1.SVC_PIC_STF_CD AS SACODE , ")
                .Append("  T6.USERNAME , ")
                .Append("  T5.LOGO_PICTURE AS LOGO_NOTSELECTED , ")
                .Append("  T5.LOGO_PICTURE_SEL AS LOGO_SELECTED , ")
                .Append("  T7.MAKER_NAME AS SERIESCD , ")
                .Append("  T5.MODEL_NAME AS SERIESNM , ")
                .Append("  T2.GRADE_CD AS GRADE , ")
                .Append("  T4.BODYCLR_NAME AS BDYCLRNM , ")
                .Append("  T3.REG_NUM AS VCLREGNO , ")
                .Append("  T2.VCL_VIN AS VIN , ")
                .Append("  T3.DELI_DATE AS VCLDELIDATE , ")
                .Append("  T11.REG_MILE AS MILEAGE , ")
                .Append("  T11.UPDATEDATE , ")
                .Append("  T2.VCL_VIN AS KEY, ")
                .Append("  T2.VCL_ID AS KEY_VCL ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL T1 , ")
                .Append("  TB_M_VEHICLE T2 , ")
                .Append("  TB_M_VEHICLE_DLR T3 , ")
                .Append("  TB_M_BODYCOLOR T4 , ")
                .Append("  TB_M_MODEL T5 , ")
                .Append("  TBL_USERS T6 , ")
                .Append("  TB_M_MAKER T7 , ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("      T10.UPDATEDATE , ")
                .Append("      T10.REG_MILE , ")
                .Append("      T10.VCL_VIN ")
                .Append("    FROM ")
                .Append("      ( ")
                .Append("      SELECT ")
                .Append("        T8.REG_DATE AS UPDATEDATE , ")
                .Append("        T8.REG_MILE , ")
                .Append("        T9.VCL_VIN , ")
                .Append("        DENSE_RANK() OVER(PARTITION BY T9.VCL_VIN ")
                .Append("      ORDER BY ")
                .Append("        T8.REG_DATE DESC ) AS NO ")
                .Append("      FROM ")
                .Append("        TB_T_VEHICLE_MILEAGE T8 , ")
                .Append("        TB_M_VEHICLE T9 ")
                .Append("      WHERE ")
                .Append("            T8.CST_ID = :CSTID ")
                .Append("        AND T8.VCL_ID = T9.VCL_ID ")
                .Append("      ) T10 ")
                .Append("    WHERE ")
                .Append("      NO = 1 ")
                .Append("    ) T11 ")
                .Append("WHERE ")
                .Append("      T1.CST_ID = :CSTID ")
                .Append("  AND T1.OWNER_CHG_FLG = '0' ")
                .Append("  AND T1.CST_VCL_TYPE = '1' ")
                .Append("  AND Trim(T2.VCL_VIN) IS NOT NULL ")
                .Append("  AND T1.VCL_ID = T2.VCL_ID ")
                .Append("  AND T1.DLR_CD = T3.DLR_CD ")
                .Append("  AND T1.DLR_CD = :DLRCD ")
                .Append("  AND T2.VCL_ID = T3.VCL_ID ")
                .Append("  AND T2.BODYCLR_CD = T4.BODYCLR_CD(+) ")
                .Append("  AND T2.MODEL_CD = T4.MODEL_CD(+) ")
                .Append("  AND T2.GRADE_CD = T4.GRADE_CD(+) ")
                .Append("  AND T2.SUFFIX_CD = T4.SUFFIX_CD(+) ")
                .Append("  AND T2.MODEL_CD = T5.MODEL_CD(+) ")
                .Append("  AND RTRIM(T6.ACCOUNT(+)) = T1.SVC_PIC_STF_CD ")
                .Append("  AND T6.DELFLG(+) = '0' ")
                .Append("  AND T5.MAKER_CD = T7.MAKER_CD(+) ")
                .Append("  AND T2.VCL_VIN = T11.VCL_VIN(+) ")
                .Append("ORDER BY ")
                .Append("  T3.DELI_DATE , ")
                .Append("  T3.REG_NUM , ")
                .Append("  T11.UPDATEDATE DESC ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetOrgCustomerVehicle_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両取得
    ''' </summary>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3080201NewVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomerVehicle(ByVal cstid As String, ByVal dlrcd As String) As SC3080201DataSet.SC3080201NewVehicleDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201NewVehicleDataTable)("SC3080201_114")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewCustomerVehicle_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080201_114 */ ")
                .Append("       '' AS LOGO_NOTSELECTED ")
                .Append("     , '' AS LOGO_SELECTED ")
                .Append("     , T4.NEWCST_MAKER_NAME AS SERIESCD ")
                .Append("     , T4.NEWCST_MODEL_NAME AS SERIESNM ")
                .Append("     , T3.REG_NUM AS VCLREGNO ")
                .Append("     , T4.VCL_VIN AS VIN ")
                .Append("     , CASE WHEN T3.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("                   NULL ")
                .Append("            ELSE T3.DELI_DATE END AS VCLDELIDATE ")
                .Append("     , T3.VCL_ID AS KEY ")
                .Append("  FROM TB_M_CUSTOMER_VCL T1 ")
                .Append("     , TB_M_VEHICLE_DLR T3 ")
                .Append("     , TB_M_VEHICLE T4 ")
                .Append(" WHERE T1.CST_ID = :CSTID ")
                .Append("   AND T1.VCL_ID(+) = T3.VCL_ID ")
                .Append("   AND T1.DLR_CD(+) = T3.DLR_CD ")
                .Append("   AND T3.VCL_ID(+) = T4.VCL_ID ")
                .Append("   AND T1.DLR_CD = :DLRCD ")
                .Append("   AND T1.CST_VCL_TYPE = '1' ")
                .Append(" ORDER BY VCLDELIDATE ")
                .Append("     , T3.REG_NUM ")
                .Append("     , T1.ROW_UPDATE_DATETIME DESC ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewCustomerVehicle_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END


            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客職業取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="crcustId">顧客ID</param>
    ''' <returns>SC3080201CustomerOccupationDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerOccupation(ByVal dlrcd As String, _
                                          ByVal strcd As String, _
                                          ByVal crcustId As String) As SC3080201DataSet.SC3080201CustomerOccupationDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustomerOccupationDataTable)("SC3080201_115")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerOccupation_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_115 */ ")
                .Append("  T2.OCCUPATIONNO , ")
                .Append("  T2.OCCUPATION , ")
                .Append("  NVL(( ")
                .Append("    SELECT ")
                .Append("      '1' ")
                .Append("    FROM ")
                .Append("      TB_M_CUSTOMER T1 ")
                .Append("    WHERE ")
                .Append("          T1.CST_OCCUPATION_ID = T2.OCCUPATIONNO ")
                .Append("      AND T1.CST_ID = :CRCUSTID) , ")
                .Append("    '0' ) AS SELECTION , ")
                .Append("    '1'   AS SORTNO_1ST , ")
                .Append("    T2.SORTNO AS SORTNO_2ND , ")
                .Append("    T2.OTHER , ")
                .Append("    T2.ICONPATH_VIEWONLY , ")
                .Append("    T2.ICONPATH_NOTSELECTED , ")
                .Append("    T2.ICONPATH_SELECTED ")
                .Append("FROM ")
                .Append("    TBL_OCCUPATIONMST T2 ")
                .Append("WHERE ")
                .Append("        T2.DLRCD = :DLRCD ")
                .Append("    AND T2.STRCD = :STRCD ")
                .Append("    AND T2.DELFLG = '0' ")
                .Append("UNION ALL ")
                .Append("  SELECT ")
                .Append("      T3.CST_OCCUPATION_ID AS OCCUPATIONNO , ")
                .Append("      T3.CST_OCCUPATION AS OCCUPATION , ")
                .Append("      '1' AS SELECTION , ")
                .Append("      '2' AS SORTNO_1ST , ")
                .Append("      0 AS SORTNO_2ND , ")
                .Append("      NULL AS OTHER , ")
                .Append("      NULL AS ICONPATH_VIEWONLY , ")
                .Append("      NULL AS ICONPATH_NOTSELECTED , ")
                .Append("      NULL AS ICONPATH_SELECTED ")
                .Append("    FROM ")
                .Append("      TB_M_CUSTOMER T3 ")
                .Append("    WHERE ")
                .Append("          T3.CST_ID = :CRCUSTID ")
                .Append("      AND TRIM(T3.CST_OCCUPATION) IS NOT NULL ")
                .Append("    ORDER BY ")
                .Append("      SORTNO_1ST , ")
                .Append("      SORTNO_2ND ")
            End With

            dlrcd = DLRCDXXXXX
            strcd = STRCDXXX

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerOccupation_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 家族続柄マスタ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <returns>SC3080201CustomerFamilyMstDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerFamilyMst(ByVal dlrcd As String, _
                                         ByVal strcd As String) As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable

        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustomerFamilyMstDataTable)("SC3080201_018")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3080201_018 */ ")
                .Append("        FAMILYRELATIONSHIPNO ")
                .Append("      , FAMILYRELATIONSHIP ")
                .Append("      , OTHERUNKNOWN ")
                .Append(" FROM   TBL_FAMILYRELATIONSHIPMST ")
                .Append(" WHERE  DLRCD = :DLRCD ")
                .Append(" AND    STRCD = :STRCD ")
                .Append(" AND    DELFLG = '0' ")
                .Append(" ORDER BY SORTNO ")

            End With

            dlrcd = DLRCDXXXXX
            strcd = STRCDXXX

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd) '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd) '店舗コード

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客家族構成取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>SC3080201CustomerFamilyDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerFamily(ByVal dlrcd As String, _
                                      ByVal strcd As String, _
                                      ByVal cstKind As String, _
                                      ByVal customerClass As String, _
                                      ByVal crcustId As String) As SC3080201DataSet.SC3080201CustomerFamilyDataTable

        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustomerFamilyDataTable)("SC3080201_019")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerFamily_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_119 */ ")
                .Append("  T5.FAMILYNO , ")
                .Append("  T5.FAMILYRELATIONSHIPNO , ")
                .Append("  T5.OTHERFAMILYRELATIONSHIP , ")
                .Append("  T5.FAMILYRELATIONSHIP , ")
                .Append("  T5.BIRTHDAY , ")
                .Append("  T5.SORTNO ")
                .Append("FROM ")
                .Append("  ( ")
                .Append("  SELECT ")
                .Append("    T2.FAMILYNO , ")
                .Append("    T2.FAMILYRELATIONSHIPNO , ")
                .Append("    T2.OTHERFAMILYRELATIONSHIP , ")
                .Append("    NVL(( ")
                .Append("      SELECT ")
                .Append("        T1.FAMILYRELATIONSHIP ")
                .Append("      FROM ")
                .Append("        TBL_FAMILYRELATIONSHIPMST T1 ")
                .Append("      WHERE ")
                .Append("            T2.FAMILYRELATIONSHIPNO = T1.FAMILYRELATIONSHIPNO ")
                .Append("        AND T1.DLRCD = :DLRCD ")
                .Append("        AND T1.STRCD = :STRCD ")
                .Append("        AND T1.DELFLG = '0') , ")
                .Append("      '' ) AS FAMILYRELATIONSHIP , ")
                .Append("      T2.BIRTHDAY , ")
                .Append("      1 AS SORTNO ")
                .Append("FROM ")
                .Append("  TBL_CSTFAMILY T2 ")
                .Append("WHERE ")
                .Append("      T2.CSTKIND = :CSTKIND ")
                .Append("  AND T2.CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append("  AND T2.CRCUSTID = :CRCUSTID_LEG ")
                .Append("UNION ALL ")
                .Append("  SELECT ")
                .Append("    0 AS FAMILYNO , ")
                .Append("    0 AS FAMILYRELATIONSHIPNO , ")
                .Append("    NULL AS OTHERFAMILYRELATIONSHIP , ")
                .Append("    NULL AS FAMILYRELATIONSHIP , ")
                .Append("    T3.CST_BIRTH_DATE AS BIRTHDAY , ")
                .Append("    0 AS SORTNO ")
                .Append("  FROM ")
                .Append("    TB_M_CUSTOMER T3 , ")
                .Append("    TB_M_CUSTOMER_DLR T4 ")
                .Append("  WHERE ")
                .Append("        T4.DLR_CD = :DLRCD_CUSTOMER ")
                .Append("    AND T3.CST_ID = :CRCUSTID ")
                .Append("    AND T3.CST_ID = T4.CST_ID ")
                .Append("  ) T5 ")
                .Append("ORDER BY ")
                .Append("  T5.SORTNO , ")
                .Append("  T5.FAMILYNO ")
            End With

            strcd = STRCDXXX

            query.CommandText = sql.ToString()

            Dim i As Integer = 0
            Dim crcustId_builder As New StringBuilder

            crcustId_builder.Append(crcustId)

            For i = 0 To 20 - crcustId.Length() - 1
                crcustId_builder.Append(" ")
            Next

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, DLRCDXXXXX)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)
            query.AddParameterWithTypeValue("DLRCD_CUSTOMER", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.NVarchar2, cstKind)
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.NVarchar2, customerClass)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("CRCUSTID_LEG", OracleDbType.Char, crcustId_builder.ToString)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerFamily_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 顧客趣味取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>SC3080201CustomerHobbyDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerHobby(ByVal dlrcd As String, _
                                     ByVal strcd As String, _
                                     ByVal cstKind As String, _
                                     ByVal customerClass As String, _
                                     ByVal crcustId As String) As SC3080201DataSet.SC3080201CustomerHobbyDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustomerHobbyDataTable)("SC3080201_022")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3080201_022 */ ")
                .Append("        A.HOBBYNO ")
                .Append("      , A.HOBBY ")
                .Append("      , NVL((SELECT '1' ")
                .Append("             FROM   TBL_CSTHOBBY ")
                .Append("             WHERE  HOBBYNO = A.HOBBYNO ")
                .Append("             AND    CSTKIND = :CSTKIND ")
                .Append("             AND    CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append("             AND    CRCUSTID = :CRCUSTID) ")
                .Append("            ,'0') AS SELECTION ")
                .Append("      , '1' AS SORTNO_1ST ")
                .Append("      , A.SORTNO AS SORTNO_2ND ")
                .Append("      , A.OTHER ")
                .Append("      , A.ICONPATH_VIEWONLY ")
                .Append("      , A.ICONPATH_NOTSELECTED ")
                .Append("      , A.ICONPATH_SELECTED ")
                .Append(" FROM   TBL_HOBBYMST A ")
                .Append(" WHERE  A.DLRCD = :DLRCD ")
                .Append(" AND    A.STRCD = :STRCD ")
                .Append(" AND    A.DELFLG = '0' ")
                .Append(" UNION ALL ")
                .Append(" SELECT HOBBYNO ")
                .Append("      , OTHERHOBBY ")
                .Append("      , '1' AS SELECTION ")
                .Append("      , '2' AS SORTNO_1ST ")
                .Append("      , 0 AS SORTNO_2ND ")
                .Append("      , NULL AS OTHER ")
                .Append("      , NULL AS ICONPATH_VIEWONLY ")
                .Append("      , NULL AS ICONPATH_NOTSELECTED ")
                .Append("      , NULL AS ICONPATH_SELECTED ")
                .Append(" FROM   TBL_CSTHOBBY ")
                .Append(" WHERE  CSTKIND = :CSTKIND ")
                .Append(" AND    CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append(" AND    CRCUSTID = :CRCUSTID ")
                .Append(" AND    TRIM(OTHERHOBBY) IS NOT NULL ")
                .Append(" ORDER BY SORTNO_1ST ")
                .Append("        , SORTNO_2ND ")

            End With

            dlrcd = DLRCDXXXXX
            strcd = STRCDXXX

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd) '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd) '店舗コード
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望コンタクト方法取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactFlg(ByVal crcustId As String, _
                                         ByVal dlrcd As String) As SC3080201DataSet.SC3080201ContactFlgDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ContactFlgDataTable)("SC3080201_133")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactFlg_Start")
            'ログ出力 End *****************************************************************************

            With sql                .Append("SELECT ")
                .Append("  /* SC3080201_133 */ ")
                .Append("  CONTACT_MTD_DM AS CONTACTDMFLG , ")
                .Append("  CONTACT_MTD_PHONE AS CONTACTHOMEFLG , ")
                .Append("  CONTACT_MTD_MOBILE AS CONTACTMOBILEFLG , ")
                .Append("  CONTACT_MTD_EMAIL AS CONTACTEMAILFLG , ")
                .Append("  CONTACT_MTD_SMS AS CONTACTSMSFLG , ")
                .Append("  ROW_LOCK_VERSION ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_DLR ")
                .Append("WHERE ")
                .Append("      CST_ID = :CRCUSTID ")
                .Append("  AND DLR_CD = :DLRCD ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactFlg_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望連絡時間帯取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="crcustId">顧客ID</param>
    ''' <param name="timeZoneClass">時間帯分類</param>
    ''' <returns>SC3080201ContactTimeZoneDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactTimeZone(ByVal dlrcd As String, _
                                       ByVal strcd As String, _
                                       ByVal crcustId As String, _
                                       ByVal timeZoneClass As String) As SC3080201DataSet.SC3080201ContactTimeZoneDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ContactTimeZoneDataTable)("SC3080201_125")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactTimeZone_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_125 */ ")
                .Append("  :TIMEZONECLASS AS TIMEZONECLASS , ")
                .Append("  T2.CONTACTTIMEZONENO, ")
                .Append("  T2.CONTACTTIMEZONEFROM , ")
                .Append("  T2.CONTACTTIMEZONETO , ")
                .Append("  T2.CONTACTTIMEZONETITLE , ")
                .Append("  NVL(( ")
                .Append("    SELECT ")
                .Append("      '1' ")
                .Append("    FROM ")
                .Append("      TB_M_CST_CONTACT_TIMESLOT T1 ")
                .Append("    WHERE ")
                .Append("          T1.CONTACT_TIMESLOT = T2.CONTACTTIMEZONENO ")
                .Append("      AND T1.CST_ID = :CRCUSTID ")
                .Append("      AND T1.TIMESLOT_CLASS = :TIMEZONECLASS), ")
                .Append("    '0' ) AS CONTACTTIMEZONESELECT , ")
                .Append("    T2.SORTNO ")
                .Append("FROM ")
                .Append("  TBL_CONTACTTIMEZONEMST T2 ")
                .Append("WHERE ")
                .Append("      T2.DLRCD = :DLRCD ")
                .Append("  AND T2.STRCD = :STRCD ")
                .Append("  AND T2.DELFLG = '0' ")
            End With

            dlrcd = DLRCDXXXXX
            strcd = STRCDXXX

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strcd)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("TIMEZONECLASS", OracleDbType.NVarchar2, timeZoneClass)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactTimeZone_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 希望連絡曜日取得
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="timeZoneClass">時間帯クラス</param>
    ''' <returns>SC3080201ContactWeekOfDayDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactWeekOfDay(ByVal cstKind As String, _
                                        ByVal customerClass As String, _
                                        ByVal crcustId As String, _
                                        ByVal timeZoneClass As String) As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ContactWeekOfDayDataTable)("SC3080201_026")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactWeekOfDay_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append(" SELECT /* SC3080201_026 */ ")
                .Append("        TIMEZONECLASS ")
                .Append("      , MONDAY ")
                .Append("      , TUESWDAY ")
                .Append("      , WEDNESDAY ")
                .Append("      , THURSDAY ")
                .Append("      , FRIDAY ")
                .Append("      , SATURDAY ")
                .Append("      , SUNDAY ")
                .Append(" FROM TBL_CSTCONTACTWEEKOFDAY ")
                .Append(" WHERE CSTKIND = :CSTKIND ")
                .Append(" AND   CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append(" AND   TRIM(CRCUSTID) = :CRCUSTID ")
                .Append(" AND   TIMEZONECLASS = :TIMEZONECLASS ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
            query.AddParameterWithTypeValue("TIMEZONECLASS", OracleDbType.Int16, timeZoneClass) '時間帯クラス
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactWeekOfDay_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 最新顧客メモ取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>SC3080201LastCustomerMemoDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLastCustomerMemo(ByVal crcustId As String) As SC3080201DataSet.SC3080201LastCustomerMemoDataTable
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201LastCustomerMemoDataTable)("SC3080201_131")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetLastCustomerMemo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080201_131 */ ")
                .Append("       T2.ROW_UPDATE_DATETIME AS UPDATEDATE ")
                .Append("     , T2.CST_MEMO AS MEMO ")
                .Append("  FROM (SELECT T1.ROW_UPDATE_DATETIME ")
                .Append("             , T1.CST_MEMO ")
                .Append("          FROM TB_T_CUSTOMER_MEMO T1 ")
                .Append("         WHERE T1.CST_ID = :CRCUSTID ")
                .Append("         ORDER BY T1.ROW_UPDATE_DATETIME DESC) T2 ")
                .Append(" WHERE ROWNUM = 1 ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetLastCustomerMemo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' 重要連絡取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="dateCount">表示日数幅</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns>SC3080201ImportantContactDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetImportantContact(ByVal crcustId As String, _
                                               ByVal cstKind As String, _
                                               ByVal newCustId As String, _
                                               ByVal dateCount As String, _
                                               ByVal dlrcd As String) As SC3080201DataSet.SC3080201ImportantContactDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetImportantContact_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_146 */ ")
            .Append("       CLMCATEGORY ")
            .Append("     , RCP_DATE ")
            .Append("     , COMPLAINT_OVERVIEW ")
            .Append("     , COMPLAINT_DETAIL ")
            .Append("     , STATUS ")
            .Append("     , USERNAME ")
            .Append("     , ICON_IMGFILE ")
            .Append("  FROM (SELECT '%1' || '%2' || NVL(T4.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T5.CMPL_CAT_NAME,'-') AS CLMCATEGORY ")
            .Append("             , T3.REC_DATETIME AS RCP_DATE ")
            .Append("             , T1.CMPL_OVERVIEW AS COMPLAINT_OVERVIEW ")
            .Append("             , T1.CMPL_DETAIL AS COMPLAINT_DETAIL ")
            .Append("             , T1.CMPL_STATUS AS STATUS ")
            .Append("             , T6.USERNAME ")
            .Append("             , T7.ICON_IMGFILE ")
            .Append("             , T1.ROW_UPDATE_DATETIME AS UPDATEDATE ")
            .Append("          FROM TB_M_CUSTOMER_VCL T0 ")
            .Append("             , TB_T_COMPLAINT T1 ")
            .Append("             , TB_M_CUSTOMER_DLR T2 ")
            .Append("             , TB_T_REQUEST T3 ")
            .Append("             , TB_M_COMPLAINT_IMPORTANCE T4 ")
            .Append("             , TB_M_COMPLAINT_CAT T5 ")
            .Append("             , TBL_USERS T6 ")
            .Append("             , TBL_OPERATIONTYPE T7 ")
            .Append("         WHERE T0.DLR_CD = :DLRCD ")
            .Append("           AND T0.CST_ID = :CRCUSTID ")
            .Append("           AND T0.CST_VCL_TYPE = '1' ")
            .Append("           AND T0.VCL_ID = T1.VCL_ID ")
            .Append("           AND T0.VCL_ID <> 0 ")
            .Append("           AND T0.DLR_CD = T2.DLR_CD ")
            .Append("           AND T0.CST_ID = T2.CST_ID ")
            .Append("           AND T2.CST_TYPE = :CSTKIND ")
            .Append("           AND T1.RELATION_TYPE IN ('0','1') ")
            .Append("           AND ((T1.CMPL_STATUS IN ('1','2')) ")
            .Append("            OR (T1.CMPL_STATUS = '3' ")
            .Append("           AND (EXISTS (SELECT 1 ")
            .Append("                          FROM TB_T_COMPLAINT_DETAIL T8 ")
            .Append("                         WHERE T8.CMPL_ID = T1.CMPL_ID ")
            .Append("                           AND T8.FIRST_LAST_ACT_TYPE = '2' ")
            .Append("                           AND T8.CMPL_DETAIL_ID = (SELECT MAX(T9.CMPL_DETAIL_ID) ")
            .Append("                                                      FROM TB_T_COMPLAINT_DETAIL T9 ")
            .Append("                                                     WHERE T1.CMPL_ID = T9.CMPL_ID ) ")
            .Append("                           AND T8.ACT_DATETIME - :DATECOUNT <= SYSDATE ")
            .Append("                       ) ")
            .Append("               ))) ")
            .Append("           AND T1.REQ_ID(+) = T3.REQ_ID ")
            .Append("           AND T4.CMPL_IMPORTANCE_ID(+) = T1.CMPL_IMPORTANCE_ID ")
            .Append("           AND T5.CMPL_CAT_ID(+) = T1.CMPL_CAT_ID ")
            .Append("           AND T5.INUSE_FLG(+) = '1' ")
            .Append("           AND T6.ACCOUNT(+) = T1.PIC_STF_CD ")
            .Append("           AND T6.DELFLG(+) = '0' ")
            .Append("           AND T7.OPERATIONCODE(+) = T6.OPERATIONCODE ")
            .Append("           AND T7.DLRCD(+) = :DLRCD ")
            .Append("           AND T7.STRCD(+) = :STRCD ")
            .Append("           AND T7.DELFLG(+) = '0' ")
            .Append("         ORDER BY T1.ROW_UPDATE_DATETIME DESC ")
            .Append("       ) ")
            .Append(" WHERE ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ImportantContactDataTable)("SC3080201_146")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Decimal, cstKind)
            query.AddParameterWithTypeValue("DATECOUNT", OracleDbType.Char, dateCount)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, STRCD000)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetImportantContact_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function


    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' コンタクト履歴取得
    ''' </summary>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="tabIndex">検索対象のタブ</param>
    ''' <returns>SC3080201ContactHistoryDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContactHistory(ByVal customerClass As String, _
                                             ByVal crcustId As String, _
                                             ByVal dlrCD As String, _
                                             ByVal cstKind As String, _
                                             ByVal newCustId As String, _
                                             ByVal tabIndex As String, _
                                             ByVal vin As String) As SC3080201DataSet.SC3080201ContactHistoryDataTable

        Dim sql As New StringBuilder
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactHistory_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append(" SELECT /* SC3080201_132 */ ")
            .Append("        ACTUALKIND ") '活動種類
            .Append("      , ACTUALDATE ") '活動日
            .Append("      , CONTACTNO ") '接触方法No
            .Append("      , COUNTVIEW ") 'カウント表示
            .Append("      , CONTACT ") '接触方法
            .Append("      , CRACTSTATUS ") 'ステータス
            .Append("      , USERNAME ") '実施者名
            .Append("      , ICON_IMGFILE ") '権限アイコンパス
            .Append("      , ROW_NUMBER() OVER(PARTITION BY CONTACTNO, FLLWUPBOX_SEQNO ORDER BY ACTUALDATE,UPDATEDATE) AS CONTACTCOUNT ") 'カウント
            .Append("      , COMPLAINT_OVERVIEW ") '苦情概要
            .Append("      , ACTUAL_DETAIL ") '苦情対応内容
            .Append("      , MEMO ") '苦情メモ
            .Append("      , MILEAGE ") '走行距離
            .Append("      , DLRNICNM_LOCAL ") '販売店名
            .Append("      , MAINTEAMOUNT ") '整備費用
            .Append("      , JOBNO ") '整備番号
            .Append("      , MILEAGESEQ ") '入庫番号
            .Append("      , DLRCD ") '販売店コード
            .Append("      , ORIGINALID ") '自社客連番
            .Append("      , VIN ") 'VIN
            .Append("      , VCLREGNO ") 'VIN
            .Append("   FROM ( ")
            'tabIndexで分岐
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Or _
               String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '全てタブ、セールスタブSQL
                'FOLLOW-UP BOX
                .Append(ContactHistoryFollowSqlCreate(False))
                .Append("     UNION ALL ")
                'FOLLOW-UP BOX PAST
                .Append(ContactHistoryFollowSqlCreate(True))
                'Follow-upBoxベース
                .Append("     UNION ALL ")
                .Append(ContactHistoryFollowupBoxSqlCreate())
                '受注後(計画)
                .Append("     UNION ALL ")
                .Append(ContactHistoryPlanSqlCreate(cstKind))
                '受注後(実績)
                .Append("     UNION ALL ")
                .Append(ContactHistoryPerformanceSqlCreate(False))
                '受注後(実績) PAST
                .Append("     UNION ALL ")
                .Append(ContactHistoryPerformanceSqlCreate(True))
                '受注後(キャンセル)
                .Append("     UNION ALL ")
                .Append(ContactHistoryCancelSqlCreate(False))
                '受注後(キャンセル) PAST
                .Append("     UNION ALL ")
                .Append(ContactHistoryCancelSqlCreate(True))
                If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                    '全てタブの場合、サービス追加
                    .Append("     UNION ALL ")
                    .Append(ContactHistoryServiceSqlCreate(tabIndex))
                End If

                If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                    '全てタブの場合、CR追加
                    .Append("     UNION ALL ")
                    .Append(ContactHistoryCRSqlCreate())
                End If
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SERVICE) Then
                'サービスタブSQL
                .Append(ContactHistoryServiceSqlCreate(tabIndex))
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                'CRタブSQL
                .Append(ContactHistoryCRSqlCreate())
            End If
            .Append(" ) ")
            .Append("  ORDER BY ACTUALDATE DESC, UPDATEDATE DESC, ORDER_NO DESC")
        End With
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ContactHistoryDataTable)("SC3080201_032")
            query.CommandText = sql.ToString()

            '共通パラメータ
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD) '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, STRCD000) '店舗コード
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                '全てタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在しない
                        query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                    Else
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                Else
                    '未取引客
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類

                '選択タブがALLの場合、全ての保有車両を対象にする
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, crcustId)
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SALES) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                'セールスタブパラメータ
                If String.Equals(cstKind, ORGCUSTFLG) Then
                    '自社客
                    If String.IsNullOrEmpty(newCustId) Then
                        '自社客に紐付く未取引客IDが存在しない
                        query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                    Else
                        '自社客に紐付く未取引客IDが存在する
                        query.AddParameterWithTypeValue("NEW_CUST_ID", OracleDbType.Char, newCustId) '自社客に紐付く未取引客ID
                    End If
                Else
                    '未取引客
                    query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
                End If
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_SERVICE) Then
                'サービスタブパラメータ
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, crcustId)
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
            ElseIf String.Equals(tabIndex, CONTACTHISTORY_TAB_CR) Then
                '共通パラメータ
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            End If
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetContactHistory_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function
    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 FOLLOW-UP BOX
    ''' </summary>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryFollowSqlCreate(ByVal pastFlg As Boolean) As String
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryFollowSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT DISTINCT /* SC3080201_170 */ /* FOLLOW-UP BOX */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T1.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T1.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T1.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , T3.SALES_ID AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , CASE ")
            .Append("            WHEN T1.ACT_STATUS = '31' THEN ")
            .Append("                 '4' ")
            .Append("            WHEN T1.ACT_STATUS = '32' THEN ")
            .Append("                 '5' ")
            .Append("       ELSE ")
            .Append("            CASE ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '30' THEN ")
            .Append("                      '3' ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '20' THEN ")
            .Append("                      '2' ")
            .Append("                 ELSE '1' ")
            .Append("            END ")
            .Append("       END AS CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T1.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , ' ' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            If pastFlg = True Then
                .Append("  FROM TB_H_ACTIVITY T1 ")
                .Append("     , TB_H_REQUEST T2 ")
                .Append("     , TB_H_SALES T3 ")
            Else
                .Append("  FROM TB_T_ACTIVITY T1 ")
                .Append("     , TB_T_REQUEST T2 ")
                .Append("     , TB_T_SALES T3 ")
            End If
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T1.REQ_ID = T3.REQ_ID ")
            .Append("   AND T1.RSLT_CONTACT_MTD = T4.CONTACT_MTD ")
            .Append("   AND T1.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T2.CST_ID = :CRCUSTID ")
            .Append("   AND T2.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T2.BIZ_TYPE = '2' ")
            .Append("   AND T3.SALES_PROSPECT_CD <> '0' ")
            .Append("   AND T4.INUSE_FLG(+) = '1' ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT DISTINCT ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T1.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T1.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T1.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , T3.SALES_ID AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , CASE ")
            .Append("            WHEN T1.ACT_STATUS = '31' THEN ")
            .Append("                 '4' ")
            .Append("            WHEN T1.ACT_STATUS = '32' THEN ")
            .Append("                 '5' ")
            .Append("       ELSE ")
            .Append("            CASE ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '30' THEN ")
            .Append("                      '3' ")
            .Append("                 WHEN T1.RSLT_SALES_PROSPECT_CD = '20' THEN ")
            .Append("                      '2' ")
            .Append("                 ELSE '1' ")
            .Append("            END ")
            .Append("       END AS CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T1.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , ' ' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            If pastFlg = True Then
                .Append("  FROM TB_H_ACTIVITY T1 ")
                .Append("     , TB_H_ATTRACT T2 ")
                .Append("     , TB_H_SALES T3 ")
            Else
                .Append("  FROM TB_T_ACTIVITY T1 ")
                .Append("     , TB_T_ATTRACT T2 ")
                .Append("     , TB_T_SALES T3 ")
            End If
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T1.ATT_ID = T3.ATT_ID ")
            .Append("   AND T1.RSLT_CONTACT_MTD = T4.CONTACT_MTD ")
            .Append("   AND T1.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T2.CST_ID = :CRCUSTID ")
            .Append("   AND T2.BIZ_TYPE = '2' ")
            .Append("   AND T3.SALES_PROSPECT_CD <> '0' ")
            .Append("   AND T4.INUSE_FLG(+) = '1' ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryFollowSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()
    End Function


    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 受注後(計画)
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryPlanSqlCreate(ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 受注後(計画) */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.ACTUALTIME_END AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , TO_CHAR(B.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("          , TO_CHAR(B.CONTACT_NAME) AS CONTACT ")
            .Append("          , CASE WHEN A.WAITING_OBJECT = '001' THEN '6' ")
            .Append("                 WHEN A.WAITING_OBJECT = '002' THEN '7' ")
            .Append("                 WHEN A.WAITING_OBJECT = '005' THEN '8' ")
            .Append("          END AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            .Append("          , TO_CHAR(D.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            .Append("          ,'' AS MILEAGE, ")
            .Append("           '' AS DLRNICNM_LOCAL, ")
            .Append("           '' AS MAINTEAMOUNT, ")
            .Append("           '' AS JOBNO, ")
            .Append("           '' AS MILEAGESEQ, ")
            .Append("           '' AS DLRCD, ")
            .Append("           '' AS ORIGINALID, ")
            .Append("           '' AS VIN, ")
            .Append("           '' AS VCLREGNO ")
            '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            .Append("       FROM TBL_BOOKEDAFTERFOLLOWRSLT A ")
            .Append("          , TB_M_CONTACT_MTD B ")
            .Append("          , tbl_USERS C ")
            .Append("          , TBL_OPERATIONTYPE D ")
            .Append("      WHERE A.DLRCD = :DLRCD ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND A.CUSTSEGMENT = :CSTKIND ")
            .Append("        AND A.CRCUSTID = :CRCUSTID ")
            .Append("        AND B.CONTACT_MTD(+) = TO_CHAR(A.CONTACTNO) ")
            .Append("        AND B.INUSE_FLG(+) = '1' ")
            .Append("        AND C.ACCOUNT(+) = A.ACTUALACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND D.DLRCD(+) = :DLRCD ")
            .Append("        AND D.STRCD(+) = :STRCD ")
            .Append("        AND D.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 受注後(実績)
    ''' </summary>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryPerformanceSqlCreate(ByVal pastFlg As Boolean) As String
        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryPerformanceSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /*SC3080201_181  */ /* 受注後(実績 振当てTACT) */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.VCLASIDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '9' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 1 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_REQUEST T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_REQUEST T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT IN ('002','005','007') ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append(" UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.VCLASIDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '9' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 1 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_ATTRACT T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_ATTRACT T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT IN ('002','005','007') ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append(" UNION ALL ")
            .Append("SELECT /* 受注後(実績 入金TACT) */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.SALESDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '10' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 2 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_REQUEST T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_REQUEST T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT IN ('005','007') ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append(" UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.SALESDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '10' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 2 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_ATTRACT T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_ATTRACT T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT IN ('005','007') ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append(" UNION ALL ")
            .Append("SELECT /* 受注後(実績 納車TACT) */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.VCLDELIDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '11' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 3 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_REQUEST T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_REQUEST T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT = '007' ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append(" UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T4.VCLDELIDATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '11' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 3 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_ATTRACT T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_ATTRACT T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T4.WAITING_OBJECT = '007' ")
            .Append("   AND T4.DELFLG = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryPerformanceSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 受注後(キャンセル)
    ''' </summary>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryCancelSqlCreate(ByVal pastFlg As Boolean) As String
        Dim sql As New StringBuilder

        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryCancelSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_182 */ /* 受注後(キャンセル) */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T5.CANCEL_DATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '12' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_REQUEST T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_REQUEST T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append("     , TB_T_SALESBOOKING T5 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T4.DLRCD = T5.DLR_CD ")
            .Append("   AND T4.SALESBKGNO = T5.SALESBKG_NUM ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T5.CANCEL_FLG = '1' ")
            .Append("   AND T4.DELFLG = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T5.CANCEL_DATE AS ACTUALDATE ")
            .Append("     , 0 AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , '0' AS COUNTVIEW ")
            .Append("     , '%3' AS CONTACT ")
            .Append("     , '12' AS CRACTSTATUS ")
            .Append("     , '' AS USERNAME ")
            .Append("     , '' AS ICON_IMGFILE ")
            .Append("     , T4.UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM ")
            If pastFlg = True Then
                .Append("       TB_H_ATTRACT T1 ")
                .Append("     , TB_H_SALES T2        ")
            Else
                .Append("       TB_T_ATTRACT T1 ")
                .Append("     , TB_T_SALES T2        ")
            End If
            .Append("     , TBL_ESTIMATEINFO T3 ")
            .Append("     , TBL_SALESBKGTALLY T4 ")
            .Append("     , TB_T_SALESBOOKING T5 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T2.SALES_ID = T3.FLLWUPBOX_SEQNO ")
            .Append("   AND TRIM(T3.CONTRACTNO) = TRIM(T4.SALESBKGNO) ")
            .Append("   AND T3.DLRCD = T4.DLRCD ")
            .Append("   AND T4.DLRCD = T5.DLR_CD ")
            .Append("   AND T4.SALESBKGNO = T5.SALESBKG_NUM ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '2' ")
            .Append("   AND T3.CONTRACTFLG = '1' ")
            .Append("   AND T3.DELFLG = '0' ")
            .Append("   AND T5.CANCEL_FLG = '1' ")
            .Append("   AND T4.DELFLG = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryCancelSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴　CR用SQL作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function ContactHistoryCRSqlCreate() As String
        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryCRSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("    /* SC3080201_183 */ /* 苦情 */ ")
            .Append("    '3' AS ACTUALKIND      , ")
            .Append("    T4.ACT_DATETIME AS ACTUALDATE      , ")
            .Append("    0 AS CONTACTNO      , ")
            .Append("    0 AS FLLWUPBOX_SEQNO      , ")
            .Append("    '0' AS COUNTVIEW      , ")
            .Append("    TO_CHAR('%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-')) AS CONTACT      , ")
            .Append("    TO_CHAR(T3.CMPL_STATUS) AS CRACTSTATUS      , ")
            .Append("    TO_CHAR(T7.USERNAME) AS USERNAME      , ")
            .Append("    TO_CHAR(T8.ICON_IMGFILE) AS ICON_IMGFILE      , ")
            .Append("    T3.UPDATE_DATETIME AS UPDATEDATE     , ")
            .Append("    TO_CHAR(T3.CMPL_OVERVIEW) AS COMPLAINT_OVERVIEW      , ")
            .Append("    TO_CHAR(T4.ACT_CONTENT) AS ACTUAL_DETAIL      , ")
            .Append("    TO_CHAR(T9.CST_MEMO) AS MEMO      , ")
            .Append("    0 AS ORDER_NO      , ")
            .Append("    '' AS MILEAGE      , ")
            .Append("    '' AS DLRNICNM_LOCAL      , ")
            .Append("    '' AS MAINTEAMOUNT      , ")
            .Append("    '' AS JOBNO      , ")
            .Append("    '' AS MILEAGESEQ      , ")
            .Append("    '' AS DLRCD      , ")
            .Append("    '' AS ORIGINALID      , ")
            .Append("    '' AS VIN      , ")
            .Append("    '' AS VCLREGNO ")
            .Append("FROM ")
            .Append("    TB_T_ACTIVITY T1      , ")
            .Append("    TB_T_REQUEST T2      , ")
            .Append("    TB_T_COMPLAINT T3      , ")
            .Append("    TB_T_COMPLAINT_DETAIL T4      , ")
            .Append("    TB_M_COMPLAINT_IMPORTANCE T5      , ")
            .Append("    TB_M_COMPLAINT_CAT T6      , ")
            .Append("    TBL_USERS T7      , ")
            .Append("    TBL_OPERATIONTYPE T8      , ")
            .Append("    TB_T_ACTIVITY_MEMO T9 ")
            .Append("WHERE ")
            .Append("    T1.REQ_ID = T2.REQ_ID AND ")
            .Append("    T1.REQ_ID = T3.REQ_ID AND ")
            .Append("    T1.ACT_ID = T4.ACT_ID    AND ")
            .Append("    T3.CMPL_IMPORTANCE_ID = T5.CMPL_IMPORTANCE_ID(+)    AND ")
            .Append("    T3.CMPL_CAT_ID = T6.CMPL_CAT_ID(+)    AND ")
            .Append("    T4.ACT_STF_CD = T7.ACCOUNT(+)     AND ")
            .Append("    T7.OPERATIONCODE = T8.OPERATIONCODE(+) AND ")
            .Append("    T1.ACT_ID = T9.RELATION_ACT_ID(+) AND ")
            .Append("    T2.CST_ID = :CRCUSTID    AND ")
            .Append("    T2.REC_CST_VCL_TYPE = '1'    AND ")
            .Append("    T2.BIZ_TYPE = '3'    AND ")
            .Append("    T3.RELATION_TYPE <> 2    AND ")
            .Append("    T4.DIST_FLG(+) = '0'    AND ")
            .Append("    T5.INUSE_FLG(+) = '1'    AND ")
            .Append("    T6.INUSE_FLG(+) = '1'    AND ")
            .Append("    T7.DELFLG(+) = '0'    AND ")
            .Append("    T8.DLRCD(+) = :DLRCD    AND ")
            .Append("    T8.STRCD(+) = :STRCD ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("    /* SC3080201_183 */ /* 苦情 */ ")
            .Append("    '3' AS ACTUALKIND      , ")
            .Append("    T4.ACT_DATETIME AS ACTUALDATE      , ")
            .Append("    0 AS CONTACTNO      , ")
            .Append("    0 AS FLLWUPBOX_SEQNO      , ")
            .Append("    '0' AS COUNTVIEW      , ")
            .Append("    TO_CHAR('%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-')) AS CONTACT      , ")
            .Append("    TO_CHAR(T3.CMPL_STATUS) AS CRACTSTATUS      , ")
            .Append("    TO_CHAR(T7.USERNAME) AS USERNAME      , ")
            .Append("    TO_CHAR(T8.ICON_IMGFILE) AS ICON_IMGFILE      , ")
            .Append("    T3.UPDATE_DATETIME AS UPDATEDATE     , ")
            .Append("    TO_CHAR(T3.CMPL_OVERVIEW) AS COMPLAINT_OVERVIEW      , ")
            .Append("    TO_CHAR(T4.ACT_CONTENT) AS ACTUAL_DETAIL      , ")
            .Append("    TO_CHAR(T9.CST_MEMO) AS MEMO      , ")
            .Append("    0 AS ORDER_NO      , ")
            .Append("    '' AS MILEAGE      , ")
            .Append("    '' AS DLRNICNM_LOCAL      , ")
            .Append("    '' AS MAINTEAMOUNT      , ")
            .Append("    '' AS JOBNO      , ")
            .Append("    '' AS MILEAGESEQ      , ")
            .Append("    '' AS DLRCD      , ")
            .Append("    '' AS ORIGINALID      , ")
            .Append("    '' AS VIN      , ")
            .Append("    '' AS VCLREGNO ")
            .Append("FROM ")
            .Append("    TB_H_ACTIVITY T1      , ")
            .Append("    TB_H_REQUEST T2      , ")
            .Append("    TB_H_COMPLAINT T3      , ")
            .Append("    TB_H_COMPLAINT_DETAIL T4      , ")
            .Append("    TB_M_COMPLAINT_IMPORTANCE T5      , ")
            .Append("    TB_M_COMPLAINT_CAT T6      , ")
            .Append("    TBL_USERS T7      , ")
            .Append("    TBL_OPERATIONTYPE T8      , ")
            .Append("    TB_H_ACTIVITY_MEMO T9 ")
            .Append("WHERE ")
            .Append("    T1.REQ_ID = T2.REQ_ID AND ")
            .Append("    T1.REQ_ID = T3.REQ_ID AND ")
            .Append("    T1.ACT_ID = T4.ACT_ID    AND ")
            .Append("    T3.CMPL_IMPORTANCE_ID = T5.CMPL_IMPORTANCE_ID(+)    AND ")
            .Append("    T3.CMPL_CAT_ID = T6.CMPL_CAT_ID(+)    AND ")
            .Append("    T4.ACT_STF_CD = T7.ACCOUNT(+)     AND ")
            .Append("    T7.OPERATIONCODE = T8.OPERATIONCODE(+) AND ")
            .Append("    T1.ACT_ID = T9.RELATION_ACT_ID(+) AND ")
            .Append("    T2.CST_ID = :CRCUSTID    AND ")
            .Append("    T2.REC_CST_VCL_TYPE = '1'    AND ")
            .Append("    T2.BIZ_TYPE = '3'    AND ")
            .Append("    T3.RELATION_TYPE <> 2    AND ")
            .Append("    T4.DIST_FLG(+) = '0'    AND ")
            .Append("    T5.INUSE_FLG(+) = '1'    AND ")
            .Append("    T6.INUSE_FLG(+) = '1'    AND ")
            .Append("    T7.DELFLG(+) = '0'    AND ")
            .Append("    T8.DLRCD(+) = :DLRCD    AND ")
            .Append("    T8.STRCD(+) = :STRCD ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryCRSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Return sql.ToString()
    End Function

    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 Follow-upBox
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Follow-upBoxベースで履歴取得</remarks>
    Public Shared Function ContactHistoryFollowupBoxSqlCreate() As String

        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryFollowupBoxSqlCreate_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_184 */ ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T2.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T2.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T2.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , '1' CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T2.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append(" FROM TB_T_REQUEST T1 ")
            .Append("     , TB_T_ACTIVITY T2 ")
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append(" WHERE T1.LAST_ACT_ID = T2.ACT_ID ")
            .Append("   AND T2.RSLT_CONTACT_MTD = T4.CONTACT_MTD ")
            .Append("   AND T2.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '4' ")
            .Append("   AND T4.INUSE_FLG(+) = '1' ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
            .Append("UNION ALL ")
            .Append("SELECT  ")
            .Append("       '1' AS ACTUALKIND ")
            .Append("     , T2.RSLT_DATETIME AS ACTUALDATE ")
            .Append("     , CASE WHEN T2.RSLT_CONTACT_MTD = ' ' THEN 0 ")
            .Append("            ELSE TO_NUMBER(T2.RSLT_CONTACT_MTD) ")
            .Append("       END AS CONTACTNO ")
            .Append("     , 0 AS FLLWUPBOX_SEQNO ")
            .Append("     , TO_CHAR(T4.COUNT_DISP_FLG) AS COUNTVIEW ")
            .Append("     , TO_CHAR(T4.CONTACT_NAME) AS CONTACT ")
            .Append("     , '7' CRACTSTATUS ")
            .Append("     , TO_CHAR(T5.USERNAME) AS USERNAME ")
            .Append("     , TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("     , T2.RSLT_DATETIME AS UPDATEDATE ")
            .Append("     , '' AS COMPLAINT_OVERVIEW ")
            .Append("     , '' AS ACTUAL_DETAIL ")
            .Append("     , '' AS MEMO ")
            .Append("     , 0 AS ORDER_NO ")
            .Append("     , '' AS MILEAGE ")
            .Append("     , '' AS DLRNICNM_LOCAL ")
            .Append("     , '' AS MAINTEAMOUNT ")
            .Append("     , '' AS JOBNO ")
            .Append("     , '' AS MILEAGESEQ ")
            .Append("     , '' AS DLRCD ")
            .Append("     , '' AS ORIGINALID ")
            .Append("     , '' AS VIN ")
            .Append("     , '' AS VCLREGNO ")
            .Append("  FROM TB_H_REQUEST T1 ")
            .Append("     , TB_H_ACTIVITY T2 ")
            .Append("     , TB_M_CONTACT_MTD T4      ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append(" WHERE T1.LAST_ACT_ID = T2.ACT_ID ")
            .Append("   AND T2.RSLT_CONTACT_MTD = T4.CONTACT_MTD ")
            .Append("   AND T2.RSLT_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T1.CST_ID = :CRCUSTID ")
            .Append("   AND T1.REC_CST_VCL_TYPE = :CUSTOMERCLASS ")
            .Append("   AND T1.BIZ_TYPE = '4' ")
            .Append("   AND T4.INUSE_FLG(+) = '1' ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryFollowupBoxSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Return sql.ToString()
    End Function

    '更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' コンタクト履歴 サービス用SQL作成
    ''' </summary>
    ''' <param name="tabIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryServiceSqlCreate(ByVal tabIndex As String) As String
        Dim sql As New StringBuilder

        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryServiceSqlCreate_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_185 */  ")
            .Append("    '2' AS ACTUALKIND,  ")
            .Append("    T4.REG_DATE AS ACTUALDATE,  ")
            .Append("    0 AS CONTACTNO,  ")
            .Append("    0 AS FLLWUPBOX_SEQNO,  ")
            .Append("    '0' AS COUNTVIEW,  ")
            .Append("    '' AS CONTACT,  ")
            .Append("    '' AS CRACTSTATUS,  ")
            .Append("    TO_CHAR(T5.USERNAME) AS USERNAME,  ")
            .Append("    TO_CHAR(T6.ICON_IMGFILE) AS ICON_IMGFILE,  ")
            .Append("    T4.REG_DATE AS UPDATEDATE,  ")
            .Append("    '' AS COMPLAINT_OVERVIEW,  ")
            .Append("    '' AS ACTUAL_DETAIL,  ")
            .Append("    '' AS MEMO,  ")
            .Append("    0 AS ORDER_NO,  ")
            .Append("    TO_CHAR(T4.REG_MILE,'9G999G999G999G999G999') AS MILEAGE,  ")
            .Append("    TO_CHAR(T7.DLR_NAME) AS DLRNICNM_LOCAL,  ")
            .Append("    TO_CHAR(T3.MAINTE_AMOUNT) AS MAINTEAMOUNT,  ")
            .Append("    TO_CHAR(T3.SVCIN_NUM) AS JOBNO,  ")
            .Append("    TO_CHAR(T3.VCL_MILE_ID) AS MILEAGESEQ,  ")
            .Append("    TO_CHAR(T3.DLR_CD) AS DLRCD,  ")
            .Append("    TO_CHAR(T1.CST_ID) AS ORIGINALID,  ")
            .Append("    TO_CHAR(T2.VCL_VIN) AS VIN,  ")
            .Append("    TO_CHAR(T8.REG_NUM) AS VCLREGNO  ")
            .Append("  FROM TB_M_CUSTOMER_VCL T1 ")
            .Append("     , TB_M_VEHICLE T2 ")
            .Append("     , TB_T_VEHICLE_SVCIN_HIS T3 ")
            .Append("     , TB_T_VEHICLE_MILEAGE T4 ")
            .Append("     , TBL_USERS T5 ")
            .Append("     , TBL_OPERATIONTYPE T6 ")
            .Append("     , TB_M_DEALER T7 ")
            .Append("     , TB_M_VEHICLE_DLR T8 ")
            .Append(" WHERE T1.VCL_ID = T2.VCL_ID ")
            .Append("   AND T1.DLR_CD = T3.DLR_CD ")
            .Append("   AND T1.VCL_ID = T3.VCL_ID ")
            .Append("   AND T1.CST_ID = T3.CST_ID ")
            .Append("   AND T3.VCL_MILE_ID = T4.VCL_MILE_ID ")
            .Append("   AND T3.PIC_STF_CD = T5.ACCOUNT(+) ")
            .Append("   AND T5.OPERATIONCODE = T6.OPERATIONCODE(+) ")
            .Append("   AND T3.DLR_CD = T7.DLR_CD ")
            .Append("   AND T1.DLR_CD = T8.DLR_CD ")
            .Append("   AND T1.VCL_ID = T8.VCL_ID ")
            .Append("   AND T1.CST_ID = :ORIGINALID ")
            .Append("   AND T1.OWNER_CHG_FLG = '0' ")
            .Append("   AND T1.CST_VCL_TYPE = '1' ")
            If String.Equals(tabIndex, CONTACTHISTORY_TAB_ALL) Then
                '選択タブがALLの場合、全ての保有車両を対象にする
            Else
                '選択タブがALL以外(サービス)の場合、選択中の車両のみを対称にする
                .Append("   AND T2.VCL_VIN = :VIN ")
            End If
            .Append("   AND T4.REG_MTD = '1'  ")
            .Append("   AND T5.DELFLG(+) = '0' ")
            .Append("   AND T6.DLRCD(+) = :DLRCD ")
            .Append("   AND T6.STRCD(+) = :STRCD ")
            .Append("   AND T6.DELFLG(+) = '0' ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("ContactHistoryServiceSqlCreate_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return sql.ToString()

    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END


    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客職業登録
    ''' </summary>
    ''' <param name="occupationNo">顧客職業ID</param>
    ''' <param name="otherOccupation">顧客職業</param>
    ''' <param name="row_update_account">更新アカウント</param>
    ''' <param name="crcustId">顧客ID</param>
    ''' <param name="row_lock_version">ロックバージョン</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateOrgCustomerOccupation(ByVal occupationNo As String, _
                                             ByVal otherOccupation As String, _
                                             ByVal row_update_account As String, _
                                             ByVal crcustId As String, _
                                             ByVal row_lock_version As Long) As Integer
        Using query As New DBUpdateQuery("SC3080201_117")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerOccupation_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("UPDATE ")
                .Append("    /* SC3080201_117 */ ")
                .Append("    TB_M_CUSTOMER ")
                .Append("SET ")
                .Append("    CST_OCCUPATION_ID = :OCCUPATIONNO , ")
                .Append("    CST_OCCUPATION = :OTHEROCCUPATION , ")
                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION = 'SC3080201', ")
                .Append("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .Append("WHERE ")
                .Append("        CST_ID = :CRCUSTID ")
                .Append("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("OCCUPATIONNO", OracleDbType.Decimal, occupationNo)
            query.AddParameterWithTypeValue("OTHEROCCUPATION", OracleDbType.NVarchar2, otherOccupation)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, row_update_account)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerOccupation_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 顧客家族構成削除
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteCustomerFamily(ByVal cstKind As String, _
                                         ByVal customerClass As String, _
                                         ByVal crcustId As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_020")

            Dim sql As New StringBuilder
            With sql
                .Append(" DELETE /* SC3080201_020 */ ")
                .Append(" FROM   TBL_CSTFAMILY ")
                .Append(" WHERE  CSTKIND = :CSTKIND ")
                .Append(" AND    CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append(" AND    CRCUSTID = :CRCUSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 顧客家族構成登録
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="familyNo">家族No</param>
    ''' <param name="familyRelationShipNo">家族続柄No</param>
    ''' <param name="otherFamilyRelationShip">その他家族続柄</param>
    ''' <param name="birthday">生年月日</param>
    ''' <param name="accunt">更新アカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertCustomerFamily(ByVal cstKind As String, _
                                         ByVal customerClass As String, _
                                         ByVal crcustId As String, _
                                         ByVal familyNo As Integer, _
                                         ByVal familyRelationShipNo As Integer, _
                                         ByVal otherFamilyRelationShip As String, _
                                         ByVal birthday As Nullable(Of DateTime), _
                                         ByVal accunt As String, _
                                         ByVal id As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_021")

            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* SC3080201_021 */ ")
                .Append(" INTO   TBL_CSTFAMILY ")
                .Append(" (      CSTKIND ")
                .Append("      , CUSTOMERCLASS ")
                .Append("      , CRCUSTID ")
                .Append("      , FAMILYNO ")
                .Append("      , FAMILYRELATIONSHIPNO ")
                .Append("      , OTHERFAMILYRELATIONSHIP ")
                .Append("      , BIRTHDAY ")
                .Append("      , CREATEDATE ")
                .Append("      , UPDATEDATE ")
                .Append("      , CREATEACCOUNT ")
                .Append("      , UPDATEACCOUNT ")
                .Append("      , CREATEID ")
                .Append("      , UPDATEID ")
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" (      :CSTKIND ")
                .Append("      , :CUSTOMERCLASS ")
                .Append("      , :CRCUSTID ")
                .Append("      , :FAMILYNO ")
                .Append("      , :FAMILYRELATIONSHIPNO ")
                .Append("      , :OTHERFAMILYRELATIONSHIP ")
                .Append("      , :BIRTHDAY ")
                .Append("      , SYSDATE ")
                .Append("      , SYSDATE ")
                .Append("      , :CREATEACCOUNT ")
                .Append("      , :UPDATEACCOUNT ")
                .Append("      , :CREATEID ")
                .Append("      , :UPDATEID ")
                .Append(" ) ")

            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
            query.AddParameterWithTypeValue("FAMILYNO", OracleDbType.Int32, familyNo) '家族No
            query.AddParameterWithTypeValue("FAMILYRELATIONSHIPNO", OracleDbType.Int32, familyRelationShipNo) '家族続柄No
            query.AddParameterWithTypeValue("OTHERFAMILYRELATIONSHIP", OracleDbType.Char, otherFamilyRelationShip) 'その他家族続柄
            query.AddParameterWithTypeValue("BIRTHDAY", OracleDbType.Date, birthday) '生年月日
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, accunt) '作成アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, accunt) '更新アカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Char, id) '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, id) '更新機能ID

            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END


    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2013/06/30 TCS 小幡 2013/10対応版 START
    ''' <summary>
    ''' 自社客付加情報登録(家族構成)
    ''' </summary>
    ''' <param name="numberOfFamily">家族人数</param>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="row_lock_version">ロックバージョン</param>
    '''  <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateOrgCustomerFamily(ByVal numberOfFamily As String, _
                                                  ByVal originalid As String, _
                                                  ByVal row_lock_version As Long, _
                                                  ByVal updateaccount As String,
                                                  ByVal dlrcd As String) As Integer
        '2013/06/30 TCS 小幡 2013/10対応版 END
        Using query As New DBUpdateQuery("SC3080201_105")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerFamily_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("UPDATE ")
                .Append("    /* SC3080201_105 */ ")
                .Append("    TB_M_CUSTOMER_DLR ")
                .Append("SET ")
                '2013/06/30 TCS 小幡 2013/10対応版　削除 START
                '2013/06/30 TCS 小幡 2013/10対応版　削除 END
                .Append("    FAMILY_AMOUNT = :NUMBEROFFAMILY , ")
                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION = 'SC3080201', ")
                .Append("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .Append("WHERE ")
                .Append("    CST_ID = :ORIGINALID ")
                '2013/06/30 TCS 小幡 2013/10対応版 START
                .Append("AND DLR_CD = :DLRCD ")
                '2013/06/30 TCS 小幡 2013/10対応版 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NUMBEROFFAMILY", OracleDbType.Decimal, numberOfFamily)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            '2013/06/30 TCS 小幡 2013/10対応版 START
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            '2013/06/30 TCS 小幡 2013/10対応版 END
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerFamily_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END


    ''' <summary>
    ''' 顧客趣味削除
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteCustomerHobby(ByVal cstKind As String, _
                                        ByVal customerClass As String, _
                                        ByVal crcustId As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_023")

            Dim sql As New StringBuilder
            With sql
                .Append(" DELETE /* SC3080201_023 */ ")
                .Append(" FROM   TBL_CSTHOBBY ")
                .Append(" WHERE  CSTKIND = :CSTKIND ")
                .Append(" AND    CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append(" AND    CRCUSTID = :CRCUSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード

            Return query.Execute()
        End Using
    End Function

    ''' <summary>
    ''' 顧客趣味登録
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="hobbyNo">趣味No</param>
    ''' <param name="otherHobby">その他趣味</param>
    ''' <param name="accunt">更新アカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertCustomerHobby(ByVal cstKind As String, _
                                        ByVal customerClass As String, _
                                        ByVal crcustId As String, _
                                        ByVal hobbyNo As Integer, _
                                        ByVal otherHobby As String, _
                                        ByVal accunt As String, _
                                        ByVal id As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_024")

            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* SC3080201_024 */ ")
                .Append(" INTO   TBL_CSTHOBBY ")
                .Append(" (      CSTKIND ")
                .Append("      , CUSTOMERCLASS ")
                .Append("      , CRCUSTID ")
                .Append("      , HOBBYNO ")
                .Append("      , OTHERHOBBY ")
                .Append("      , CREATEDATE ")
                .Append("      , UPDATEDATE ")
                .Append("      , CREATEACCOUNT ")
                .Append("      , UPDATEACCOUNT ")
                .Append("      , CREATEID ")
                .Append("      , UPDATEID ")
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" (      :CSTKIND ")
                .Append("      , :CUSTOMERCLASS ")
                .Append("      , :CRCUSTID ")
                .Append("      , :HOBBYNO ")
                .Append("      , :OTHERHOBBY ")
                .Append("      , SYSDATE ")
                .Append("      , SYSDATE ")
                .Append("      , :CREATEACCOUNT ")
                .Append("      , :UPDATEACCOUNT ")
                .Append("      , :CREATEID ")
                .Append("      , :UPDATEID ")
                .Append(" ) ")

            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
            query.AddParameterWithTypeValue("HOBBYNO", OracleDbType.Int32, hobbyNo) '趣味No
            query.AddParameterWithTypeValue("OTHERHOBBY", OracleDbType.Char, otherHobby) 'その他趣味
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, accunt) '作成アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, accunt) '更新アカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Char, id) '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, id) '更新機能ID

            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 小幡 2013/10対応版　削除 START
    '2013/06/30 TCS 小幡 2013/10対応版　削除 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客付加情報更新(希望コンタクト方法)
    ''' </summary>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="contactDMFlg">希望コンタクト方法（DM）</param>
    ''' <param name="contactHomeFlg">希望コンタクト方法（自宅電話）</param>
    ''' <param name="contactMobileFlg">希望コンタクト方法（携帯電話）</param>
    ''' <param name="contactEMailFlg">希望コンタクト方法（e-mail）</param>
    ''' <param name="contactSmsFlg">希望コンタクト方法（SMS）</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="row_lock_version">ロックバージョン</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateOrgCustomerAppnedContact(ByVal originalid As String, _
                                                   ByVal contactDMFlg As String, _
                                                   ByVal contactHomeFlg As String, _
                                                   ByVal contactMobileFlg As String, _
                                                   ByVal contactEMailFlg As String, _
                                                   ByVal contactSmsFlg As String, _
                                                   ByVal row_lock_version As Long, _
                                                   ByVal updateaccount As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_108")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerAppnedContact_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("UPDATE ")
                .Append("    /* SC3080201_108 */ ")
                .Append("    TB_M_CUSTOMER_DLR ")
                .Append("SET ")
                .Append("    CONTACT_MTD_DM = :CONTACTDMFLG , ")
                .Append("    CONTACT_MTD_PHONE = :CONTACTHOMEFLG , ")
                .Append("    CONTACT_MTD_MOBILE = :CONTACTMOBILEFLG , ")
                .Append("    CONTACT_MTD_EMAIL = :CONTACTEMAILFLG , ")
                .Append("    CONTACT_MTD_SMS = :CONTACTSMSFLG , ")
                .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_FUNCTION = 'SC3080201', ")
                .Append("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION+1 ")
                .Append("WHERE ")
                .Append("        CST_ID = :ORIGINALID ")
                .Append("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CONTACTDMFLG", OracleDbType.NVarchar2, contactDMFlg)
            query.AddParameterWithTypeValue("CONTACTHOMEFLG", OracleDbType.NVarchar2, contactHomeFlg)
            query.AddParameterWithTypeValue("CONTACTMOBILEFLG", OracleDbType.NVarchar2, contactMobileFlg)
            query.AddParameterWithTypeValue("CONTACTEMAILFLG", OracleDbType.NVarchar2, contactEMailFlg)
            query.AddParameterWithTypeValue("CONTACTSMSFLG", OracleDbType.NVarchar2, contactSmsFlg)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerAppnedContact_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望連絡時間帯削除
    ''' </summary>
    ''' <param name="crcustId">顧客ID</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteContactTimeZone(ByVal crcustId As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_127")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("DeleteContactTimeZone_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("DELETE ")
                .Append("    /* SC3080201_127 */ ")
                .Append("FROM ")
                .Append("    TB_M_CST_CONTACT_TIMESLOT ")
                .Append("WHERE ")
                .Append("        CST_ID = :CRCUSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("DeleteContactTimeZone_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 希望連絡時間帯登録
    ''' </summary>
    ''' <param name="crcustId">顧客ID</param>
    ''' <param name="timeZoneClass">時間帯分類</param>
    ''' <param name="contactTimeZoneNo">連絡時間帯ID</param>
    ''' <param name="createaccount">作成アカウント</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertContactTimeZone(ByVal crcustId As String, _
                                          ByVal timeZoneClass As String, _
                                          ByVal contactTimeZoneNo As String, _
                                          ByVal createaccount As String, _
                                          ByVal updateaccount As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_129")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertContactTimeZone_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("INSERT ")
                .Append("    /* SC3080201_129 */ ")
                .Append("INTO TB_M_CST_CONTACT_TIMESLOT ( ")
                .Append("    CST_ID , ")
                .Append("    TIMESLOT_CLASS , ")
                .Append("    CONTACT_TIMESLOT , ")
                .Append("    ROW_CREATE_DATETIME , ")
                .Append("    ROW_CREATE_ACCOUNT , ")
                .Append("    ROW_CREATE_FUNCTION , ")
                .Append("    ROW_UPDATE_DATETIME , ")
                .Append("    ROW_UPDATE_ACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION , ")
                .Append("    ROW_LOCK_VERSION ")
                .Append(") ")
                .Append("VALUES ( ")
                .Append("    :CRCUSTID , ")
                .Append("    :TIMEZONECLASS , ")
                .Append("    :CONTACTTIMEZONENO , ")
                .Append("    SYSDATE , ")
                .Append("    :CREATEACCOUNT , ")
                .Append("     'SC3080201', ")
                .Append("    SYSDATE , ")
                .Append("    :UPDATEACCOUNT , ")
                .Append("     'SC3080201', ")
                .Append("0 ")
                .Append(") ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("TIMEZONECLASS", OracleDbType.NVarchar2, timeZoneClass)
            query.AddParameterWithTypeValue("CONTACTTIMEZONENO", OracleDbType.Decimal, contactTimeZoneNo)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, createaccount)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertContactTimeZone_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.Execute()
        End Using
    End Function


    ''' <summary>
    ''' 希望連絡曜日削除
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteContactWeekOfDay(ByVal cstKind As String, _
                                           ByVal customerClass As String, _
                                           ByVal crcustId As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_028")

            Dim sql As New StringBuilder
            With sql
                .Append(" DELETE /* SC3080201_028 */ ")
                .Append(" FROM   TBL_CSTCONTACTWEEKOFDAY ")
                .Append(" WHERE  CSTKIND = :CSTKIND ")
                .Append(" AND    CUSTOMERCLASS = :CUSTOMERCLASS ")
                .Append(" AND    CRCUSTID = :CRCUSTID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード

            Return query.Execute()
        End Using
    End Function


    ''' <summary>
    ''' 希望連絡時間帯登録
    ''' </summary>
    ''' <param name="cstKind">顧客種別</param>
    ''' <param name="customerClass">顧客分類</param>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <param name="timeZoneClass">時間帯クラス</param>
    ''' <param name="monday">月曜日</param>
    ''' <param name="tueswday">火曜日</param>
    ''' <param name="wednesday">水曜日</param>
    ''' <param name="thursday">木曜日</param>
    ''' <param name="friday">金曜日</param>
    ''' <param name="saturday">土曜日</param>
    ''' <param name="sunday">日曜日</param>
    ''' <param name="accunt">更新アカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertContactWeekOfDay(ByVal cstKind As String, _
                                           ByVal customerClass As String, _
                                           ByVal crcustId As String, _
                                           ByVal timeZoneClass As Integer, _
                                           ByVal monday As String, _
                                           ByVal tueswday As String, _
                                           ByVal wednesday As String, _
                                           ByVal thursday As String, _
                                           ByVal friday As String, _
                                           ByVal saturday As String, _
                                           ByVal sunday As String, _
                                           ByVal accunt As String, _
                                           ByVal id As String) As Integer
        Using query As New DBUpdateQuery("SC3080201_030")

            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* SC3080201_030 */ ")
                .Append(" INTO   TBL_CSTCONTACTWEEKOFDAY ")
                .Append(" (      CSTKIND ")
                .Append("      , CUSTOMERCLASS ")
                .Append("      , CRCUSTID ")
                .Append("      , TIMEZONECLASS ")
                .Append("      , MONDAY ")
                .Append("      , TUESWDAY ")
                .Append("      , WEDNESDAY ")
                .Append("      , THURSDAY ")
                .Append("      , FRIDAY ")
                .Append("      , SATURDAY ")
                .Append("      , SUNDAY ")
                .Append("      , CREATEDATE ")
                .Append("      , UPDATEDATE ")
                .Append("      , CREATEACCOUNT ")
                .Append("      , UPDATEACCOUNT ")
                .Append("      , CREATEID ")
                .Append("      , UPDATEID ")
                .Append(" ) ")
                .Append(" VALUES ")
                .Append(" (      :CSTKIND ")
                .Append("      , :CUSTOMERCLASS ")
                .Append("      , :CRCUSTID ")
                .Append("      , :TIMEZONECLASS ")
                .Append("      , :MONDAY ")
                .Append("      , :TUESWDAY ")
                .Append("      , :WEDNESDAY ")
                .Append("      , :THURSDAY ")
                .Append("      , :FRIDAY ")
                .Append("      , :SATURDAY ")
                .Append("      , :SUNDAY ")
                .Append("      , SYSDATE ")
                .Append("      , SYSDATE ")
                .Append("      , :CREATEACCOUNT ")
                .Append("      , :UPDATEACCOUNT ")
                .Append("      , :CREATEID ")
                .Append("      , :UPDATEID ")
                .Append(" ) ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstKind) '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerClass) '顧客分類
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustId) '活動先顧客コード
            query.AddParameterWithTypeValue("TIMEZONECLASS", OracleDbType.Int32, timeZoneClass) '時間帯クラス
            query.AddParameterWithTypeValue("MONDAY", OracleDbType.Char, monday) '月曜日
            query.AddParameterWithTypeValue("TUESWDAY", OracleDbType.Char, tueswday) '火曜日
            query.AddParameterWithTypeValue("WEDNESDAY", OracleDbType.Char, wednesday) '水曜日
            query.AddParameterWithTypeValue("THURSDAY", OracleDbType.Char, thursday) '木曜日
            query.AddParameterWithTypeValue("FRIDAY", OracleDbType.Char, friday) '金曜日
            query.AddParameterWithTypeValue("SATURDAY", OracleDbType.Char, saturday) '土曜日
            query.AddParameterWithTypeValue("SUNDAY", OracleDbType.Char, sunday) '日曜日
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, accunt) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, accunt) '更新アカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Char, id) '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, id) '更新機能ID

            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客付加情報更新(顔写真)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="originalid">'顧客ID</param>
    ''' <param name="imageFileL">イメージ画像(大)</param>
    ''' <param name="imageFileM">イメージ画像(中)</param>
    ''' <param name="imageFileS">イメージ画像(小)</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="row_lock_version">ロックバージョン</param>
    ''' <returns>処理件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateOrgCustomerAppnedFace(ByVal dlrcd As String, _
                                         ByVal originalid As String, _
                                         ByVal imageFileL As String, _
                                         ByVal imageFileM As String, _
                                         ByVal imageFileS As String, _
                                         ByVal updateaccount As String, _
                                         ByVal row_lock_version As Long) As Integer
        Using query As New DBUpdateQuery("SC3080201_104")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerAppnedFace_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("UPDATE ")
                .Append("    /* SC3080201_104 */ ")
                .Append("    TB_M_CUSTOMER_DLR ")
                .Append("SET ")
                .Append("    IMG_FILE_LARGE = :IMAGEFILE_L , ")
                .Append("    IMG_FILE_MEDIUM = :IMAGEFILE_M , ")
                .Append("    IMG_FILE_SMALL = :IMAGEFILE_S , ")
                .Append("    ROW_UPDATE_DATETIME = SYSDATE , ")
                .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT , ")
                .Append("    ROW_UPDATE_FUNCTION = 'SC3080201', ")
                .Append("    ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
                .Append("WHERE ")
                .Append("       DLR_CD = :DLRCD ")
                .Append("   AND CST_ID = :ORIGINALID ")
                .Append("   AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("IMAGEFILE_L", OracleDbType.NVarchar2, imageFileL)
            query.AddParameterWithTypeValue("IMAGEFILE_M", OracleDbType.NVarchar2, imageFileM)
            query.AddParameterWithTypeValue("IMAGEFILE_S", OracleDbType.NVarchar2, imageFileS)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateOrgCustomerAppnedFace_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.Execute()

        End Using

    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2012/01/24 TCS 河原 【SALES_1B】 START
    ''' <summary>
    ''' Follow-up Box商談取得
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns>件数</returns>
    ''' <remarks>Follow-up Box商談の存在確認</remarks>
    Public Shared Function GetFllwupboxSales(ByVal fllwupboxseqno As Decimal) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwupboxSales_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_037 */ ")
            .Append("    COUNT(1) AS CNT ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
            .Append("    ROWNUM <= 1 ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CountDataTable)("SC3080201_037")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwupboxSales_End")
            'ログ出力 End *****************************************************************************
            Return query.GetCount()
        End Using
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ''' <summary>
    ''' Follow-up Box商談削除
    ''' </summary>
    ''' <param name="fllwupboxseqno"></param>
    ''' <returns></returns>
    ''' <remarks>Follow-up Box商談を削除</remarks>
    Public Shared Function DeleteFllwupboxSales(ByVal fllwupboxseqno As Decimal) As Integer
        Dim sql As New StringBuilder
        With sql
            .Append("DELETE /* SC3080201_038 */ ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            '.Append("    SALES_SEQNO = :SALES_SEQNO ")
            .Append("    REGISTFLG = '0' ")
            ' 2012/02/15 TCS 相田 【SALES_2】 END
        End With
        Using query As New DBUpdateQuery("SC3080201_038")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            'query.AddParameterWithTypeValue("SALES_SEQNO", OracleDbType.Int64, salesseqno)
            ' 2012/02/15 TCS 相田 【SALES_2】 END
            Return query.Execute()
        End Using
    End Function

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談追加
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="custsegment">顧客種別</param>
    ''' <param name="customerclass">顧客分類</param>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <param name="account">対応アカウント</param>
    ''' <param name="walkinnum">来店人数</param>
    ''' <param name="moduleid">作成機能ID</param>
    ''' <param name="newfllwupboxflg">新規活動フラグ</param>
    ''' <param name="registflg">登録フラグ</param>
    ''' <param name="branchplan">予定店舗コード</param>
    ''' <param name="accountplan">予定アカウント</param>
    ''' <param name="salesFlg">商談フラグ</param>
    ''' <returns></returns>
    ''' <remarks>Follow-up Box商談を追加</remarks>
    Public Shared Function InsertFllwupboxSales(ByVal dlrcd As String,
                                                ByVal strcd As String,
                                                ByVal fllwupboxseqno As Decimal,
                                                ByVal custsegment As String,
                                                ByVal customerclass As String,
                                                ByVal crcustid As String,
                                                ByVal account As String,
                                                ByVal walkinnum As Nullable(Of Integer),
                                                ByVal moduleid As String,
                                                ByVal newfllwupboxflg As String,
                                                ByVal registflg As String,
                                                ByVal branchplan As String,
                                                ByVal accountplan As String,
                                                ByVal salesFlg As Boolean) As Integer
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Dim sql As New StringBuilder
        With sql
            .Append("INSERT /* SC3080201_039 */ ")
            .Append("INTO TBL_FLLWUPBOX_SALES ")
            .Append("( ")
            .Append("    DLRCD, ")
            .Append("    STRCD, ")
            .Append("    FLLWUPBOX_SEQNO, ")
            .Append("    CUSTSEGMENT, ")
            .Append("    CUSTOMERCLASS, ")
            .Append("    CRCUSTID, ")
            .Append("    ACTUALACCOUNT, ")
            .Append("    STARTTIME, ")
            .Append("    ENDTIME, ")
            .Append("    WALKINNUM, ")
            .Append("    CREATEDATE, ")
            .Append("    UPDATEDATE, ")
            .Append("    CREATEACCOUNT, ")
            .Append("    UPDATEACCOUNT, ")
            .Append("    CREATEID, ")
            .Append("    UPDATEID, ")

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            .Append("    NEWFLLWUPBOXFLG, ")
            .Append("    REGISTFLG, ")
            .Append("    EIGYOSTARTTIME, ")
            .Append("    SALES_SEQNO, ")
            .Append("    BRANCH_PLAN, ")
            .Append("    ACCOUNT_PLAN ")
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("    :DLRCD, ")
            .Append("    :STRCD, ")
            .Append("    :FLLWUPBOX_SEQNO, ")
            .Append("    :CUSTSEGMENT, ")
            .Append("    :CUSTOMERCLASS, ")
            .Append("    :CRCUSTID, ")
            .Append("    :ACCOUNT, ")
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            If salesFlg Then
                '商談開始の場合
                .Append("    SYSDATE, ")
                .Append("    NULL, ")
            Else
                '営業活動開始の場合
                .Append("    NULL, ")
                .Append("    NULL, ")
            End If
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            .Append("    :WALKINNUM, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :ACCOUNT, ")
            .Append("    :ACCOUNT, ")
            .Append("    :MODULEID, ")
            .Append("    :MODULEID, ")
            ' 2012/02/15 TCS 相田 【SALES_2】 START
            .Append("    :NEWFLLWUPBOXFLG, ")
            .Append("    :REGISTFLG, ")
            If salesFlg Then
                '商談開始の場合
                .Append("    NULL, ")
            Else
                '営業活動開始の場合
                .Append("    SYSDATE, ")
            End If
            '.Append("    :SALES_SEQNO, ")
            .Append("    SEQ_FOLLOWUPBOXSALES.NEXTVAL, ")
            .Append("    :BRANCH_PLAN, ")
            .Append("    :ACCOUNT_PLAN ")
            ' 2012/02/15 TCS 相田 【SALES_2】 END
            .Append(") ")
        End With
        Using query As New DBUpdateQuery("SC3080201_039")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
            query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, custsegment)
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
            query.AddParameterWithTypeValue("WALKINNUM", OracleDbType.Int32, walkinnum)
            query.AddParameterWithTypeValue("MODULEID", OracleDbType.Char, moduleid)

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            query.AddParameterWithTypeValue("NEWFLLWUPBOXFLG", OracleDbType.Char, newfllwupboxflg)
            query.AddParameterWithTypeValue("REGISTFLG", OracleDbType.Char, registflg)
            'query.AddParameterWithTypeValue("SALES_SEQNO", OracleDbType.Int64, salesseqno)
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
            If branchplan = String.Empty Or branchplan Is Nothing Then
                branchplan = " "
            End If
            If accountplan = String.Empty Or accountplan Is Nothing Then
                accountplan = " "
            End If
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchplan)
            query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, accountplan)
            ' 2012/02/15 TCS 相田 【SALES_2】 END

            Return query.Execute()
        End Using
    End Function
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    ''' <summary>
    ''' 来店実績取得
    ''' </summary>
    ''' <param name="visitseq">来店実績連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetVisitResult(ByVal visitseq As Long) As SC3080201DataSet.SC3080201VisitResultDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_040 */ ")
            .Append("    VCLREGNO, ")
            .Append("    VISITPERSONNUM, ")
            .Append("    TENTATIVENAME ")
            .Append("FROM ")
            .Append("    TBL_VISIT_SALES ")
            .Append("WHERE ")
            .Append("    VISITSEQ = :VISITSEQ ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201VisitResultDataTable)("SC3080201_040")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitseq) 'アカウント
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' Follow-up BoxシーケンスNo取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupboxSeqno() As SC3080201DataSet.SC3080201SeqDataTable
        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START   
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwupboxSeqno_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_141 */  ")
            .Append("     SQ_SALES.NEXTVAL SEQ  ")
            .Append(" FROM  ")
            .Append("     DUAL ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwupboxSeqno_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201SeqDataTable)("SC3080201_141")
            query.CommandText = sql.ToString()
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary> 
    ''' 顧客に対し、継続中の活動が存在するかを判定
    ''' </summary>
    ''' <param name="cstid">活動先顧客コード</param>
    ''' <param name="account">スタッフアカウント</param>
    ''' <returns></returns>
    ''' <remarks>件数(返り値)>0の場合存在する。以外は存在しない。</remarks>
    Public Shared Function CountFllwupboxNotComplete(ByVal cstid As String,
                                                     ByVal account As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("CountFllwupboxNotComplete_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT 1 /* SC3080201_142 */ ")
            .Append("  FROM TB_T_REQUEST T1 ")
            .Append("     , TB_T_ACTIVITY T2 ")
            .Append("     , TB_T_SALES T3 ")
            .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
            .Append("   AND T1.REQ_ID = T3.REQ_ID ")
            .Append("   AND T1.CST_ID = :CUSTID ")
            .Append("   AND T1.REQ_STATUS = '21' ")
            .Append("   AND T2.RSLT_FLG = '0' ")
            If Not String.IsNullOrEmpty(account) Then
                .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
            End If
            .Append("   AND T3.SALES_PROSPECT_CD IN ('20','30') ")
            .Append("   AND ROWNUM <= 1 ")
            .Append("UNION ALL ")
            .Append("SELECT 1 ")
            .Append("  FROM TB_T_ATTRACT T1 ")
            .Append("     , TB_T_ACTIVITY T2 ")
            .Append("     , TB_T_SALES T3 ")
            .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
            .Append("   AND T1.ATT_ID = T3.ATT_ID ")
            .Append("   AND T1.CST_ID = :CUSTID ")
            .Append("   AND T1.ATT_STATUS = '21' ")
            .Append("   AND T2.RSLT_FLG = '0' ")
            If Not String.IsNullOrEmpty(account) Then
                .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
            End If
            .Append("   AND T3.SALES_PROSPECT_CD IN ('20','30') ")
            .Append("   AND ROWNUM <= 1 ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("CountFllwupboxNotComplete_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CountDataTable)("SC3080201_142")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, cstid)
            If Not String.IsNullOrEmpty(account) Then
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Varchar2, account)
            End If
            Return query.GetCount()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' Follow-upBox存在判定
    ''' </summary>
    ''' <param name="fllwupbox_seqno">商談ID</param>
    ''' <returns></returns>
    ''' <remarks>Follow-upBoxが存在するか判定</remarks>
    Public Shared Function CountFllwupbox(ByVal fllwupbox_seqno As Decimal) As Integer
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("CountFllwupbox_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080201_143 */ ")
            .Append("1 ")
            .Append("FROM ")
            .Append("  TB_T_SALES ")
            .Append("WHERE ")
            .Append("      SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("  AND ROWNUM <= 1 ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CountDataTable)("SC3080201_143")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("CountFllwupbox_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetCount()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 削除対象通知依頼情報取得
    ''' </summary>
    ''' <param name="strcd">販売店コード</param>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNop</param>
    ''' <returns>データセット</returns>
    ''' <remarks>削除対象の通知依頼情報を取得</remarks>
    Public Shared Function GetNoticeRequest(ByVal strcd As String, ByVal fllwupboxseqno As Decimal) As SC3080201DataSet.SC3080201NoticeRequestDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_044 */ ")
            .Append("    A.NOTICEREQID, ")
            .Append("    A.NOTICEREQCTG, ")
            .Append("    A.REQCLASSID, ")
            .Append("    A.CUSTOMNAME, ")

            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            '.Append("    DECODE(A.STATUS,'1',TOACCOUNT,'3',FROMACCOUNT) AS TOACCOUNT ")
            .Append("    DECODE(A.STATUS,'1',TOACCOUNT,'3',FROMACCOUNT,'4',FROMACCOUNT) AS TOACCOUNT ")
            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

            .Append("FROM ")
            .Append("    TBL_NOTICEREQUEST A, ")
            .Append("    TBL_NOTICEINFO B ")
            .Append("WHERE ")
            .Append("    A.FLLWUPBOXSTRCD = :FLLWUPBOXSTRCD AND ")
            .Append("    A.FLLWUPBOX = :FLLWUPBOX AND ")

            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            '.Append("    A.STATUS IN ('1','3') AND ")
            .Append("    A.STATUS IN ('1','3','4') AND ")
            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

            .Append("    B.NOTICEID(+) = A.LASTNOTICEID ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201NoticeRequestDataTable)("SC3080201_044")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOXSTRCD", OracleDbType.Char, strcd)
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
            query.AddParameterWithTypeValue("FLLWUPBOX", OracleDbType.Decimal, fllwupboxseqno)
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談テーブルNo.取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>データセット</returns>
    ''' <remarks>商談テーブルNo.を取得する</remarks>
    Public Shared Function GetVisitSales(ByVal fllwupboxseqno As Decimal) As SC3080201DataSet.SC3080201VisitSalesDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetVisitSales_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_045 */ ")
            .Append("    SALESTABLENO ")
            .Append("FROM ")
            .Append("    TBL_VISIT_SALES ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
            .Append("    VISITSTATUS = '07' ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201VisitSalesDataTable)("SC3080201_045")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)   'Follow-up Box内連番
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetVisitSales_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using

    End Function
    ''' <summary>
    ''' 端末ID取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <returns>SC3080201TerminalIdDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetUcarTerminal(ByVal dlrcd As String,
                                           ByVal strcd As String) As SC3080201DataSet.SC3080201TerminalIdDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_046 */ ")
            .Append("    TERMINALID ")                '端末ID
            .Append("FROM ")
            .Append("    TBL_UCARTERMINAL ")
            .Append("WHERE ")
            .Append("    DLRCD = :DLRCD ")            '販売店コード
            .Append("AND STRCD = :STRCD ")            '店舗コード
            .Append("AND DELFLG = '0' ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201TerminalIdDataTable)("SC3080201_046")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                       '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                       '店舗コード
            Dim rtnDt As SC3080201DataSet.SC3080201TerminalIdDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function

    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' Follow-up Box商談更新
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <param name="salesFlg">商談フラグ</param>
    ''' <param name="startFlg">開始フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal dlrcd As String, _
                                          ByVal strcd As String, _
                                          ByVal fllwupboxseqno As Decimal, _
                                          ByVal account As String, _
                                          ByVal id As String, _
                                          ByVal salesFlg As Boolean, _
                                          ByVal startFlg As Boolean) As Integer
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END    
        Using query As New DBUpdateQuery("SC3080201_046")

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3080201_046 */ ")
                .Append("        TBL_FLLWUPBOX_SALES ")
                .Append(" SET    ACTUALACCOUNT = :ACTUALACCOUNT ")

                If salesFlg Then
                    '商談の場合
                    If startFlg Then
                        '開始の場合
                        .Append("      , STARTTIME = SYSDATE ")
                        .Append("      , ENDTIME = NULL ")
                    Else
                        '終了の場合
                        .Append("      , ENDTIME = SYSDATE ")
                    End If
                Else
                    '営業活動開始の場合
                    If startFlg Then
                        '開始の場合
                        .Append("      , EIGYOSTARTTIME = SYSDATE ")
                    Else
                        '終了の場合
                        .Append("      , EIGYOSTARTTIME = NULL ")
                    End If
                End If

                .Append("      , UPDATEDATE = SYSDATE ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                .Append(" WHERE  FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                .Append(" AND    REGISTFLG = '0' ")
                '.Append(" AND    SALES_SEQNO = :SALES_SEQNO ")


            End With

            query.CommandText = sql.ToString()
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno) 'Follow-up Box内連番
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END    
            query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Char, account) '対応アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, account) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, id) '機能ID

            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　削除 START
    '2013/06/30 TCS 内藤 2013/10対応版　削除 END


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談開始時間の取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>データセット</returns>
    ''' <remarks>商談開始時間を取得する</remarks>
    Public Shared Function GetSalesTime(ByVal fllwupboxseqno As Decimal) As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesTime_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_049 */ ")
            .Append("    STARTTIME ")
            .Append("FROM ")
            .Append("    TBL_FLLWUPBOX_SALES ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
            .Append("    SALES_SEQNO = ")
            .Append("        ( ")
            .Append("        SELECT ")
            .Append("            MAX(SALES_SEQNO) ")
            .Append("        FROM ")
            .Append("            TBL_FLLWUPBOX_SALES ")
            .Append("        WHERE ")
            .Append("            FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("        ) ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable)("SC3080201_049")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesTime_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談シーケンスNo取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>データセット</returns>
    ''' <remarks>商談開始時間を取得する</remarks>
    Public Shared Function GetSalesSeqNoByRegitFlg(ByVal fllwupboxseqno As Decimal) As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable
        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesSeqNoByRegitFlg_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_050 */ ")
            .Append("       SALES_SEQNO ")
            .Append("FROM   TBL_FLLWUPBOX_SALES ")
            .Append("WHERE   ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("AND    REGISTFLG = :REGISTFLG ")

        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable)("SC3080201_050")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("REGISTFLG", OracleDbType.Char, "0")
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSalesSeqNoByRegitFlg_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客担当情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店CD</param>
    ''' <param name="custId">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustchrgInfo(ByVal dlrcd As String, _
                                           ByVal custId As String, _
                                           ByVal cstvcltype As String) As SC3080201DataSet.SC3080201CustStrDataTable
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustchrgInfo_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_151 */ ")
            .Append("       SLS_PIC_BRN_CD AS STRCDSTAFF ")
            .Append("  FROM TB_M_CUSTOMER_VCL ")
            .Append(" WHERE CST_ID = :CSTID ")
            .Append("   AND DLR_CD = :DLRCD ")
            .Append("   AND CST_VCL_TYPE = :CSTVCLTYPE ")
            .Append("   AND ROW_UPDATE_DATETIME = (SELECT MAX(ROW_UPDATE_DATETIME) FROM TB_M_CUSTOMER_VCL ")
            .Append("                               WHERE CST_ID = :CSTID ")
            .Append("                                 AND DLR_CD = :DLRCD ")
            .Append("                                 AND CST_VCL_TYPE = :CSTVCLTYPE) ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustStrDataTable)("SC3080201_151")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, custId)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTVCLTYPE", OracleDbType.NVarchar2, cstvcltype)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustchrgInfo_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box顧客担当情報取得
    ''' </summary>
    ''' <param name="fllwupbox_seqno">商談ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFllwUpBoxCustchrgInfo(ByVal fllwupbox_seqno As Decimal) As SC3080201DataSet.SC3080201CustStrDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwUpBoxCustchrgInfo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080201_152 */ ")
            .Append("  T1.SLS_PIC_BRN_CD AS CUSTCHRGSTRCD ")
            .Append("FROM ")
            .Append("  TB_M_CUSTOMER_VCL T1 , ")
            .Append("  TB_T_SALES T2 ")
            .Append("WHERE ")
            .Append("      T1.CST_ID(+) = T2.CST_ID ")
            .Append("  AND T2.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CustStrDataTable)("SC3080201_152")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetFllwUpBoxCustchrgInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 商談シーケンスNo取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesSeqNo() As SC3080201DataSet.SC3080201SeqDataTable
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3080201_053 */ ")
            .Append("     SEQ_FOLLOWUPBOXSALES.NEXTVAL SEQ ")
            .Append(" FROM ")
            .Append("     DUAL ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201SeqDataTable)("SC3080201_053")
            query.CommandText = sql.ToString()
            Return query.GetData()
        End Using
    End Function
    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2012/01/27 TCS 平野 【SALES_1B】 START
    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <param name="estimateId">見積もりID</param>
    ''' <returns>SC3080201ContactFlgDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal estimateId As String) As SC3080201DataSet.SC3080201ContractDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_047 */ ")
            .Append("    CONTRACTFLG ")                '契約状況フラグ
            .Append("FROM ")
            .Append("    TBL_ESTIMATEINFO ")
            .Append("WHERE ")
            .Append("    ESTIMATEID = :ESTIMATEID ")   '見積もりID
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ContractDataTable)("SC3080201_047")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Char, estimateId)  '見積もりID
            Dim rtnDt As SC3080201DataSet.SC3080201ContractDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function
    '2012/01/27 TCS 平野 【SALES_1B】 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetServiceInInfo(ByVal originalid As String, _
                                            ByVal vin As String, _
                                            ByVal dlrcd As String) As SC3080201DataSet.SC3080201ServiceInInfoDataTable

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetServiceInInfo_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT ")
            .Append("  /* SC3080201_160 */ ")
            .Append("  T6.INSPECTNM , ")
            .Append("  T6.SERVICECD , ")
            .Append("  T6.SV_PR , ")
            .Append("  T6.SERVICENAME , ")
            .Append("  T6.INSPECSEQ ")
            .Append("FROM ")
            .Append("  ( ")
            .Append("  SELECT ")
            .Append("    T1.MAINTE_NAME AS INSPECTNM , ")
            .Append("    T2.SVC_CD AS SERVICECD , ")
            .Append("    ROW_NUMBER() OVER (ORDER BY NVL('',9999)) AS SV_PR , ")
            .Append("    NVL(T5.SVC_NAME_MILE,T1.MAINTE_NAME) AS SERVICENAME , ")
            .Append("    T2.INSPEC_SEQ AS INSPECSEQ ")
            .Append("  FROM ")
            .Append("    TB_M_MAINTE T1 , ")
            .Append("    TB_T_VEHICLE_MAINTE_HIS T2 , ")
            .Append("    TB_T_VEHICLE_SVCIN_HIS T3 , ")
            .Append("    TB_M_VEHICLE T4 , ")
            .Append("    TB_M_SERVICE T5 ")
            .Append("  WHERE ")
            .Append("        T3.CST_ID = :ORIGINALID ")
            .Append("    AND T4.VCL_VIN = :VIN ")
            .Append("    AND T2.DLR_CD = :DLRCD ")
            .Append("    AND T3.DLR_CD = T2.DLR_CD ")
            .Append("    AND T2.DLR_CD = T5.DLR_CD ")
            .Append("    AND T5.DLR_CD = T1.DLR_CD ")
            .Append("    AND T1.DLR_CD = T5.DLR_CD ")
            .Append("    AND T3.VCL_ID = T4.VCL_ID ")
            .Append("    AND T3.SVC_CD = T5.SVC_CD ")
            .Append("    AND T3.SVCIN_NUM = T2.SVCIN_NUM ")
            .Append("    AND T2.MAINTE_CD = T1.MAINTE_CD ")
            .Append("  ) T6 ")
            .Append("ORDER BY ")
            .Append("  T6.INSPECSEQ ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ServiceInInfoDataTable)("SC3080201_160")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetServiceInInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Dim rtnDt As SC3080201DataSet.SC3080201ServiceInInfoDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function

    Public Shared Function GetBasesystemNM() As SC3080201DataSet.SC3080201BasesystemNMDataTable
        Dim sql As New StringBuilder
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetBasesystemNM_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT /* SC3080201_061 */ ")
            .Append("    WORD_VAL AS BASESYSTEMNM ")
            .Append("FROM ")
            .Append("    TB_M_WORD ")
            .Append("WHERE ")
            .Append("    WORD_CD = '50070' ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201BasesystemNMDataTable)("SC3080201_061")
            query.CommandText = sql.ToString()
            Dim rtnDt As SC3080201DataSet.SC3080201BasesystemNMDataTable = query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetBasesystemNM_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            Return rtnDt
        End Using
    End Function
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発

    '2013/03/06 TCS 河原 GL0874 START
    Public Shared Function GetVisitFllwSeq(ByVal visiteqno As Long) As SC3080201DataSet.SC3080201VisitFllwSeqDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_060 */ ")
            .Append("    A.FLLWUPBOX_STRCD, ")
            .Append("    A.FLLWUPBOX_SEQNO ")
            .Append("FROM ")
            .Append("    TBL_VISIT_SALES A ")
            .Append("WHERE ")
            .Append("        A.VISITSEQ = :VISITEQNO ")
            .Append("    AND A.VISITSTATUS = '09' ")
            .Append("    AND ")
            .Append("        ( ")
            .Append("        SELECT ")
            .Append("            COUNT(1) ")
            .Append("        FROM ")
            .Append("            TBL_ESTIMATEINFO ")
            .Append("        WHERE ")
            .Append("                STRCD = A.FLLWUPBOX_STRCD ")
            .Append("            AND FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("            AND CONTRACTFLG = '1' ")
            .Append("        ) = 0 ")
            .Append("    AND ")
            .Append("        ( ")
            .Append("        SELECT ")
            .Append("            COUNT(1) ")
            .Append("        FROM ")
            .Append("            TBL_ESTIMATEINFO ")
            .Append("        WHERE ")
            .Append("                STRCD = A.FLLWUPBOX_STRCD ")
            .Append("            AND FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("            AND CONTRACTFLG = '2' ")
            .Append("        ) > 0 ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201VisitFllwSeqDataTable)("SC3080201_060")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VISITEQNO", OracleDbType.Int64, visiteqno)  '見積もりID
            Dim rtnDt As SC3080201DataSet.SC3080201VisitFllwSeqDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function
    '2013/03/06 TCS 河原 GL0874 END


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客データロック
    ''' </summary>
    ''' <param name="crcustid">顧客ID </param>
    ''' <remarks></remarks>
    Public Shared Sub GetCustomerLock(ByVal crcustid As String)

        Using query As New DBSelectQuery(Of DataTable)("SC3080201_200")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerLock_Start")
            'ログ出力 End *****************************************************************************
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_200 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER T1 ")
                .Append("WHERE ")
                .Append("  T1.CST_ID = :CRCUSTID ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustid)
            query.GetCount()
        End Using
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerLock_End")
        'ログ出力 End *****************************************************************************

    End Sub
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END













    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 来店受付  →　未使用判断
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="pastFlg">PASTテーブル検索用</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryWalkInSqlCreate(ByVal newCustId As String, _
                                                         ByVal pastFlg As Boolean, _
                                                         ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 来店受付 */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.WALKINDATE AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , D.COUNTVIEW ")
            .Append("          , TO_CHAR(D.CONTACT) AS CONTACT ")
            .Append("          , CASE WHEN B.CREATE_CRACTRESULT = '0' THEN '1' ")
            .Append("                 WHEN B.CREATE_CRACTRESULT = '1' THEN '3' ")
            .Append("                 WHEN B.CREATE_CRACTRESULT = '2' THEN '2' ")
            .Append("            END AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            .Append("          , TO_CHAR(F.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("       FROM TBL_WALKINPERSON A ")
            If pastFlg = True Then
                'PAST
                .Append("          , TBL_FLLWUPBOX_PAST B ")
            Else
                .Append("          , TBL_FLLWUPBOX B ")
            End If
            .Append("          , TBL_USERS C ")
            .Append("          , TBL_CONTACTMETHOD D ")
            .Append("          , TBL_NEWCUSTOMER E ")
            .Append("          , TBL_OPERATIONTYPE F ")
            .Append("      WHERE A.REGISTRATIONTYPE <> '3' ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND B.DLRCD = A.DLRCD ")
            .Append("        AND B.STRCD = A.STRCD ")
            .Append("        AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("        AND C.ACCOUNT(+) = A.ACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.CONTACTNO(+) = A.CONTACTNO ")
            .Append("        AND D.DELFLG(+) = '0' ")
            .Append("        AND E.DLRCD = :DLRCD ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("        AND E.ORIGINALID = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("        AND (E.ORIGINALID = :CRCUSTID OR E.CSTID = :NEW_CUST_ID) ")
                End If
            Else
                '未取引客
                .Append("        AND E.CSTID = :CRCUSTID ")
            End If
            .Append("        AND A.CSTID = E.CSTID ")
            .Append("        AND F.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND F.DLRCD(+) = :DLRCD ")
            .Append("        AND F.STRCD(+) = :STRCD ")
            .Append("        AND F.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function
    ''' <summary>
    ''' コンタクト履歴 セールス用SQL作成 来店受付 Follow Null
    ''' </summary>
    ''' <param name="newCustId">自社客に紐付く未取引客ID</param>
    ''' <param name="cstKind">顧客種別</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ContactHistoryWalkInNullSqlCreate(ByVal newCustId As String, _
                                                                ByVal cstKind As String) As String
        Dim sql As New StringBuilder
        With sql
            .Append("     SELECT /* 来店受付 */ ")
            .Append("            '1' AS ACTUALKIND ")
            .Append("          , A.WALKINDATE AS ACTUALDATE ")
            .Append("          , A.CONTACTNO ")
            .Append("          , A.FLLWUPBOX_SEQNO ")
            .Append("          , D.COUNTVIEW ")
            .Append("          , TO_CHAR(D.CONTACT) AS CONTACT ")
            .Append("          , null AS CRACTSTATUS ")
            .Append("          , TO_CHAR(C.USERNAME) AS USERNAME ")
            .Append("          , TO_CHAR(F.ICON_IMGFILE) AS ICON_IMGFILE ")
            .Append("          , A.UPDATEDATE ")
            .Append("          , '' AS COMPLAINT_OVERVIEW ")
            .Append("          , '' AS ACTUAL_DETAIL ")
            .Append("          , '' AS MEMO ")
            .Append("          , 0 AS ORDER_NO ")
            .Append("       FROM TBL_WALKINPERSON A ")
            .Append("          , TBL_USERS C ")
            .Append("          , TBL_CONTACTMETHOD D ")
            .Append("          , TBL_NEWCUSTOMER E ")
            .Append("          , TBL_OPERATIONTYPE F ")
            .Append("      WHERE A.REGISTRATIONTYPE <> '3' ")
            .Append("        AND A.CUSTOMERCLASS = :CUSTOMERCLASS ")
            .Append("        AND NOT EXISTS(SELECT 1 FROM TBL_FLLWUPBOX B ")
            .Append("                        WHERE B.DLRCD = A.DLRCD ")
            .Append("                          AND B.STRCD = A.STRCD ")
            .Append("                          AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("                       ) ")
            .Append("        AND NOT EXISTS(SELECT 1 FROM TBL_FLLWUPBOX_PAST B ")
            .Append("                        WHERE B.DLRCD = A.DLRCD ")
            .Append("                          AND B.STRCD = A.STRCD ")
            .Append("                          AND B.FLLWUPBOX_SEQNO = A.FLLWUPBOX_SEQNO ")
            .Append("                       )")
            .Append("        AND C.ACCOUNT(+) = A.ACCOUNT ")
            .Append("        AND C.DELFLG(+) = '0' ")
            .Append("        AND D.CONTACTNO(+) = A.CONTACTNO ")
            .Append("        AND D.DELFLG(+) = '0' ")
            .Append("        AND E.DLRCD = :DLRCD ")
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客
                If String.IsNullOrEmpty(newCustId) Then
                    '自社客に紐付く未取引客IDが存在しない
                    .Append("        AND E.ORIGINALID = :CRCUSTID ")
                Else
                    '自社客に紐付く未取引客IDが存在する
                    .Append("        AND (E.ORIGINALID = :CRCUSTID OR E.CSTID = :NEW_CUST_ID) ")
                End If
            Else
                '未取引客
                .Append("        AND E.CSTID = :CRCUSTID ")
            End If
            .Append("        AND A.CSTID = E.CSTID ")
            .Append("        AND F.OPERATIONCODE(+) = C.OPERATIONCODE ")
            .Append("        AND F.DLRCD(+) = :DLRCD ")
            .Append("        AND F.STRCD(+) = :STRCD ")
            .Append("        AND F.DELFLG(+) = '0' ")
        End With

        Return sql.ToString()
    End Function



#Region "FS開発"
    '2012/06/01 TCS 河原 FS開発 START
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' SNDIDを更新する
    ''' </summary>
    ''' <param name="CstId">未取引客ID</param>
    ''' <param name="Mode">更新モード(1:renren、2:kaixin、3:weibo)</param>
    ''' <param name="SnsId">SNSID</param>
    ''' <param name="dlrcd">販売店CD</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNewCustomerSnsId(ByVal cstid As String, _
                                                  ByVal mode As String, _
                                                  ByVal snsid As String, _
                                                  ByVal dlrcd As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewCustomerSnsId_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBUpdateQuery("SC3080201_054")
            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* SC3080201_154 */ ")
                .Append("       TB_M_CUSTOMER_DLR ")
                .Append("   SET ")
                If mode.Equals("1") Then
                    .Append("    SNS_1_ACCOUNT = :SNSID ")
                ElseIf mode.Equals("2") Then
                    .Append("    SNS_2_ACCOUNT = :SNSID ")
                Else
                    .Append("    SNS_3_ACCOUNT = :SNSID ")
                End If
                .Append("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .Append("     , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
                .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
                .Append("     , ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .Append(" WHERE DLR_CD = :DLRCD ")
                .Append("   AND CST_ID = :CSTID ")
                .Append("   AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("SNSID", OracleDbType.NVarchar2, snsid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewCustomerSnsId_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END    
            Return query.Execute()
        End Using
    End Function


    '2013/06/30 TCS 内藤 2013/10対応版　START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　END


    '2013/06/30 TCS 内藤 2013/10対応版　START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　END


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客のKeywordを更新する
    ''' </summary>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="Keyword">インターネットキーワード</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="dlrcd">販売店CD</param>
    ''' <param name="row_lock_version">ロックバージョン</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNewCustomerKeyword(ByVal cstid As String, _
                                                    ByVal keyword As String, _
                                                    ByVal account As String, _
                                                    ByVal dlrcd As String, _
                                                    ByVal row_lock_version As Long) As Integer
        Using query As New DBUpdateQuery("SC3080201_157")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewCustomerKeyword_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("UPDATE /* SC3080201_157 */ ")
                .Append("       TB_M_CUSTOMER_DLR ")
                .Append("   SET ")
                .Append("       INTERNET_KEYWORD = :KEYWORD ")
                .Append("     , ROW_UPDATE_DATETIME = SYSDATE ")
                .Append("     , ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                .Append("     , ROW_UPDATE_FUNCTION = 'SC3080201' ")
                .Append("     , ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .Append(" WHERE DLR_CD = :DLRCD  ")
                .Append("   AND CST_ID = :CSTID ")
                .Append("   AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("KEYWORD", OracleDbType.NVarchar2, keyword)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewCustomerKeyword_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
            Return query.Execute()
        End Using
    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　END


    '2013/06/30 TCS 内藤 2013/10対応版　START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　END


#End Region



End Class
