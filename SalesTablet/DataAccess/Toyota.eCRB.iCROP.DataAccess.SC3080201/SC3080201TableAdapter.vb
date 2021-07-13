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
'更新： 2013/11/06 TCS 山田 i-CROP再構築後の新車納車システムに追加したリンク対応
'更新： 2013/11/27 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 高橋 受注後フォロー機能開発
'更新： 2014/03/18 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/07/09 TCS 高橋 受注後活動完了条件変更対応
'更新： 2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）
'更新： 2014/08/28 TCS 外崎 TMT NextStep2 UAT-BTS D-117
'更新： 2014/11/20 TCS 河原  TMT B案
'更新： 2015/01/06 TCS 外崎 TMT2販社 ST-BTS 36
'更新： 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2016/05/16 TCS 鈴木 BTS-28(TMT-106DLR) 基幹連携の取り込みでエラー
'更新： 2016/09/14 TCS 河原 TMTタブレット性能改善
'更新： 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/11/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'削除： 2019/09/25 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2019/11/26 TS  髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001)
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustInfo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_100 */ ")
                .Append("    T1.DLR_CD AS DLRCD ")
                .Append("  , T1.SLS_PIC_BRN_CD AS STRCD ")
                .Append("  , 0 AS FLLWUPBOX_SEQNO ")
                .Append("  , T1.SLS_PIC_STF_CD AS STAFFCD ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL T1 ")
                .Append("  ,TB_M_CUSTOMER_DLR T2 ")
                .Append("WHERE ")
                .Append("      T1.CST_ID = T2.CST_ID ")
                .Append("  AND T1.DLR_CD = T2.DLR_CD ")
                .Append("  AND T1.CST_ID = :CUSTID ")
                .Append("  AND T1.DLR_CD = :DLR_CD ")
                If Not (vcl_id Is Nothing) Then
                    .Append("  AND T1.VCL_ID = :VCL_ID ")
                End If

                If custKind.Equals("1") Then
                    '自社客
                    With sql
                        .Append("  AND T2.CST_TYPE = '1' ")
                    End With
                ElseIf custKind.Equals("2") Then
                    '未取引客
                    With sql
                        .Append("  AND T2.CST_TYPE = '2' ")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustInfo_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_101 */ ")
                .Append("  T3.IMG_FILE_LARGE AS IMAGEFILE_L , ")
                .Append("  T3.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
                .Append("  T3.IMG_FILE_SMALL AS IMAGEFILE_S , ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                .Append("  T1.FIRST_NAME || ' ' || T1.MIDDLE_NAME || ' ' || T1.LAST_NAME AS NAME , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                ' 2014/03/18 TCS 松月 TMT不具合対応 Modify Start
                .Append("  T1.DMS_CST_CD_DISP AS CUSTCD , ")
                ' 2014/03/18 TCS 松月 TMT不具合対応 Modify End
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
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .Append("  CASE WHEN  T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T1.CST_BIRTH_DATE END AS BIRTHDAY, ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                .Append("  T3.FAMILY_AMOUNT AS NUMBEROFFAMILY , ")
                .Append("  CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T3.SNS_1_ACCOUNT AS SNSID_RENREN , ")
                .Append("  T3.SNS_2_ACCOUNT AS SNSID_KAIXIN , ")
                .Append("  T3.SNS_3_ACCOUNT AS SNSID_WEIBO , ")
                .Append("  T3.INTERNET_KEYWORD AS KEYWORD , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                .Append("  T3.VIP_FLG , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                .Append("  T1.ROW_LOCK_VERSION AS CUSTOMERLOCKVERSION , ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  T3.ROW_LOCK_VERSION AS CUSTOMERDLRLOCKVERSION , ")
                .Append("  T1.FLEET_FLG AS CUSTCATEGORY , ")
                .Append("  NVL(T10.WORD_VAL_ENG, ' ') AS CUSTSUBCAT1 , ")
                .Append("  CASE WHEN T6.CST_ORGNZ_INPUT_TYPE = '1' THEN NVL(T6.CST_ORGNZ_CD, ' ') ELSE N' ' END AS CUSTORGNZCD , ")
                .Append("  CASE WHEN T6.CST_ORGNZ_INPUT_TYPE = '2' AND (T8.CST_ORGNZ_NAME_INPUT_TYPE = '0' OR T8.CST_ORGNZ_NAME_INPUT_TYPE = '2') THEN NVL(T6.CST_ORGNZ_NAME, ' ')  ")
                .Append("       WHEN T6.CST_ORGNZ_INPUT_TYPE = '1' THEN NVL(T9.CST_ORGNZ_NAME, ' ') ELSE N' ' END AS CUSTORGNZNAME ,")
                .Append("  NVL(T6.ROW_LOCK_VERSION, -1) AS LCUSTOMERLOCKVERSION, ")
                .Append("  NVL(T8.CST_JOIN_TYPE, ' ') AS CST_JOIN_TYPE ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER T1 ")
                .Append("  LEFT JOIN TB_M_CUSTOMER_DLR T3 ON T3.DLR_CD = :DLRCD AND T3.CST_ID = T1.CST_ID AND T3.CST_TYPE = '1' ")
                If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
                Else
                    .Append("  LEFT JOIN TB_M_CUSTOMER_VCL T2 ON T2.VCL_ID = :VCL_ID AND T2.DLR_CD = T3.DLR_CD AND T2.CST_ID = T3.CST_ID AND T2.CST_VCL_TYPE = '1' ")
                    .Append("  LEFT JOIN TBL_USERS T4 ON RTRIM(T4.ACCOUNT) = T2.SLS_PIC_STF_CD AND T4.DELFLG ='0' ")
                End If
                .Append("  LEFT JOIN TB_LM_CUSTOMER T6 ON T6.CST_ID = T1.CST_ID ")
                .Append("  LEFT JOIN TB_M_PRIVATE_FLEET_ITEM T7 ON T7.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND T7.FLEET_FLG = T1.FLEET_FLG  AND T7.INUSE_FLG = '1' ")
                .Append("  LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM T8 ON T8.PRIVATE_FLEET_ITEM_CD = T7.PRIVATE_FLEET_ITEM_CD ")
                .Append("  LEFT JOIN TB_LM_CUSTOMER_ORGANIZATION T9 ON T9.CST_ORGNZ_CD = T6.CST_ORGNZ_CD AND T9.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND T9.PRIVATE_FLEET_ITEM_CD = T7.PRIVATE_FLEET_ITEM_CD AND T9.INUSE_FLG = '1' ")
                .Append("  LEFT JOIN TB_M_WORD T10 ON T10.WORD_CD = T7.PRIVATE_FLEET_ITEM ")
                .Append("WHERE ")
                .Append("         T1.CST_ID = :ORIGINALID ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)
            If Trim(vcl_id) Is Nothing Or Trim(vcl_id) = "" Then
            Else
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vcl_id)
            End If
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomer_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080201_109 */ ")
                .Append("  T2.IMG_FILE_LARGE AS IMAGEFILE_L , ")
                .Append("  T2.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
                .Append("  T2.IMG_FILE_SMALL AS IMAGEFILE_S , ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                .Append("  T1.FIRST_NAME || ' ' || T1.MIDDLE_NAME || ' ' || T1.LAST_NAME AS NAME , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
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
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                .Append("  CASE WHEN  T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T1.CST_BIRTH_DATE END AS BIRTHDAY, ")
                ' 2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                .Append("  T2.FAMILY_AMOUNT AS NUMBEROFFAMILY , ")
                .Append("  CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T2.SNS_1_ACCOUNT AS SNSID_RENREN , ")
                .Append("  T2.SNS_2_ACCOUNT AS SNSID_KAIXIN , ")
                .Append("  T2.SNS_3_ACCOUNT AS SNSID_WEIBO , ")
                .Append("  T2.INTERNET_KEYWORD AS KEYWORD , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                .Append("  T2.VIP_FLG , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                .Append("  T1.ROW_LOCK_VERSION AS CUSTOMERLOCKVERSION , ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  T2.ROW_LOCK_VERSION AS CUSTOMERDLRLOCKVERSION , ")
                .Append("  T1.FLEET_FLG AS CUSTCATEGORY , ")
                .Append("  NVL(T10.WORD_VAL_ENG, ' ') AS CUSTSUBCAT1 , ")
                .Append("  CASE WHEN T6.CST_ORGNZ_INPUT_TYPE = '1' THEN NVL(T9.CST_ORGNZ_CD, ' ') ELSE N' ' END AS CUSTORGNZCD , ")
                .Append("  CASE WHEN T6.CST_ORGNZ_INPUT_TYPE = '2' AND (T8.CST_ORGNZ_NAME_INPUT_TYPE = '0' OR T8.CST_ORGNZ_NAME_INPUT_TYPE = '2') THEN NVL(T6.CST_ORGNZ_NAME, ' ')  ")
                .Append("       WHEN T6.CST_ORGNZ_INPUT_TYPE = '1' THEN NVL(T9.CST_ORGNZ_NAME, ' ') ELSE N' ' END AS CUSTORGNZNAME ,")
                .Append("  NVL(T6.ROW_LOCK_VERSION, -1) AS LCUSTOMERLOCKVERSION, ")
                .Append("  NVL(T8.CST_JOIN_TYPE, ' ') AS CST_JOIN_TYPE ")
                .Append("FROM ")
                .Append("            TB_M_CUSTOMER T1 ")
                .Append("  LEFT JOIN TB_M_CUSTOMER_DLR T2 ON T2.DLR_CD = :DLRCD AND T2.CST_ID = T1.CST_ID AND T2.CST_TYPE = '2' ")
                .Append("  LEFT JOIN TB_M_CUSTOMER_VCL T3 ON T3.DLR_CD = T2.DLR_CD AND T3.CST_ID = T2.CST_ID AND T3.CST_VCL_TYPE = '1' ")
                If vcl_id <> String.Empty Then
                    .Append(" AND T3.VCL_ID = :VCL_ID ")
                End If
                .Append("  LEFT JOIN TBL_USERS T4 ON RTRIM(T4.ACCOUNT) = T3.SLS_PIC_STF_CD AND T4.DELFLG ='0' ")
                .Append("  LEFT JOIN TBL_USERS T5 ON T5.ACCOUNT = T3.SVC_PIC_STF_CD AND T5.DELFLG ='0' ")
                .Append("  LEFT JOIN TB_LM_CUSTOMER T6 ON T6.CST_ID = T1.CST_ID ")
                .Append("  LEFT JOIN TB_M_PRIVATE_FLEET_ITEM T7 ON T7.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND T7.FLEET_FLG = T1.FLEET_FLG  AND T7.INUSE_FLG = '1' ")
                .Append("  LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM T8 ON T8.PRIVATE_FLEET_ITEM_CD = T7.PRIVATE_FLEET_ITEM_CD ")
                .Append("  LEFT JOIN TB_LM_CUSTOMER_ORGANIZATION T9 ON T9.CST_ORGNZ_CD = T6.CST_ORGNZ_CD AND T9.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND T9.PRIVATE_FLEET_ITEM_CD = T7.PRIVATE_FLEET_ITEM_CD AND T9.INUSE_FLG = '1' ")
                .Append("  LEFT JOIN TB_M_WORD T10 ON T10.WORD_CD = T7.PRIVATE_FLEET_ITEM ")
                .Append("WHERE ")
                .Append("          T1.CST_ID = :CSTID ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            If vcl_id <> String.Empty Then
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vcl_id)
            End If
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

    ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 車両ID取得
    ''' </summary>
    ''' <returns>車両ID</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVclId(ByVal vcl_vin As String) As String

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclId_Start")
        'ログ出力 End *****************************************************************************
        With sql
            .Append("SELECT ")
            .Append("  /* SC3080201_190 */ ")
            .Append("  T1.VCL_ID ")
            .Append("FROM ")
            .Append("  TB_M_VEHICLE T1 ")
            .Append("      WHERE ")
            .Append("            T1.VCL_VIN = :VCL_VIN ")
            .Append("        AND T1.DMS_TAKEIN_DATETIME <> TO_DATE('1900/01/01 00:00:00','YYYY/MM/DD HH24:MI:SS') ")
        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3080201_190")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vcl_vin)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclId_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()(0)(0).ToString
        End Using

    End Function
    ' 2013/06/30 TCS 三宅 2013/10対応版　既存流用 END


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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomerVehicle_Start")
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
                .Append("  T2.GRADE_NAME AS GRADE , ")
                .Append("  T2.BODYCLR_NAME AS BDYCLRNM , ")
                .Append("  T3.REG_NUM AS VCLREGNO , ")
                .Append("  T2.VCL_VIN AS VIN , ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）START
                .Append("  CASE WHEN T3.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE T3.DELI_DATE END AS VCLDELIDATE, ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）END
                .Append("  T11.REG_MILE AS MILEAGE , ")
                .Append("  T11.UPDATEDATE , ")
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                ' TCS武田 マージ前暫定対応 START
                .Append("  T3.IMP_VCL_FLG , ")
                ' TCS武田 マージ前暫定対応 END
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                .Append("  T2.VCL_VIN AS KEY, ")
                .Append("  T2.VCL_ID AS KEY_VCL, ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  CASE ")
                .Append("    WHEN T12.VCL_MILE = 0 THEN ' ' ")
                .Append("    ELSE NVL(TO_CHAR(T12.VCL_MILE), ' ') ")
                .Append("  END AS VCL_MILE, ")
                .Append("  NVL(T12.MODEL_YEAR, ' ') AS MODEL_YEAR ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL T1 , ")
                .Append("  TB_M_VEHICLE T2 , ")
                .Append("  TB_M_VEHICLE_DLR T3 , ")
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
                .Append("    ) T11 , ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  TB_LM_VEHICLE_DLR T12 ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("WHERE ")
                .Append("      T1.CST_ID = :CSTID ")
                .Append("  AND T1.OWNER_CHG_FLG = '0' ")
                .Append("  AND T1.CST_VCL_TYPE = '1' ")
                .Append("  AND Trim(T2.VCL_VIN) IS NOT NULL ")
                .Append("  AND T1.VCL_ID = T2.VCL_ID ")
                .Append("  AND T1.DLR_CD = T3.DLR_CD ")
                .Append("  AND T1.DLR_CD = :DLRCD ")
                .Append("  AND T2.VCL_ID = T3.VCL_ID ")
                .Append("  AND T2.MODEL_CD = T5.MODEL_CD(+) ")
                .Append("  AND RTRIM(T6.ACCOUNT(+)) = T1.SVC_PIC_STF_CD ")
                .Append("  AND T6.DELFLG(+) = '0' ")
                .Append("  AND T5.MAKER_CD = T7.MAKER_CD(+) ")
                .Append("  AND T2.VCL_VIN = T11.VCL_VIN(+) ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  AND T3.DLR_CD = T12.DLR_CD(+) ")
                .Append("  AND T3.VCL_ID = T12.VCL_ID(+) ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("ORDER BY ")
                .Append("  T3.DELI_DATE , ")
                .Append("  T3.REG_NUM , ")
                .Append("  T11.UPDATEDATE DESC ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgCustomerVehicle_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomerVehicle_Start")
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
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                ' TCS武田 マージ前暫定対応 START
                .Append("     , T3.IMP_VCL_FLG ")
                ' TCS武田 マージ前暫定対応 END
                ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                .Append("     , T3.VCL_ID AS KEY ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("     , CASE ")
                .Append("         WHEN T5.VCL_MILE = 0 THEN ' ' ")
                .Append("         ELSE NVL(TO_CHAR(T5.VCL_MILE), ' ') ")
                .Append("       END AS VCL_MILE ")
                .Append("     , NVL(T5.MODEL_YEAR, ' ') AS MODEL_YEAR ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("  FROM TB_M_CUSTOMER_VCL T1 ")
                .Append("     , TB_M_VEHICLE_DLR T3 ")
                .Append("     , TB_M_VEHICLE T4 ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("     , TB_LM_VEHICLE_DLR T5 ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append(" WHERE T1.CST_ID = :CSTID ")
                .Append("   AND T1.VCL_ID(+) = T3.VCL_ID ")
                .Append("   AND T1.DLR_CD(+) = T3.DLR_CD ")
                .Append("   AND T3.VCL_ID(+) = T4.VCL_ID ")
                .Append("   AND T1.DLR_CD = :DLRCD ")
                .Append("   AND T1.CST_VCL_TYPE = '1' ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("   AND T3.DLR_CD = T5.DLR_CD(+) ")
                .Append("   AND T3.VCL_ID = T5.VCL_ID(+) ")
                ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append(" ORDER BY VCLDELIDATE ")
                .Append("     , T3.REG_NUM ")
                .Append("     , T1.ROW_UPDATE_DATETIME DESC ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewCustomerVehicle_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerOccupation_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerOccupation_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerFamily_Start")
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
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）START
                .Append("  CASE WHEN  T3.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T3.CST_BIRTH_DATE END AS BIRTHDAY, ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）END
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerFamily_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactFlg_Start")
            'ログ出力 End *****************************************************************************

            With sql.Append("SELECT ")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactFlg_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactTimeZone_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactTimeZone_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactWeekOfDay_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetContactWeekOfDay_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLastCustomerMemo_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLastCustomerMemo_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetImportantContact_Start")
        'ログ出力 End *****************************************************************************
        ' 2014/03/18 TCS 松月 TMT不具合対応 Modify Start
        With sql
            .Append("SELECT /* SC3080201_146 */  ")
            .Append("       CLMCATEGORY  ")
            .Append("    , RCP_DATE  ")
            .Append("    , COMPLAINT_OVERVIEW  ")
            .Append("    , COMPLAINT_DETAIL  ")
            .Append("    , STATUS  ")
            .Append("    , USERNAME  ")
            .Append("    , ICON_IMGFILE  ")
            .Append("FROM ( ")
            .Append("    SELECT  ")
            .Append("          CLMCATEGORY  ")
            .Append("        , RCP_DATE  ")
            .Append("        , COMPLAINT_OVERVIEW  ")
            .Append("        , COMPLAINT_DETAIL  ")
            .Append("        , STATUS  ")
            .Append("        , USERNAME  ")
            .Append("        , ICON_IMGFILE  ")
            .Append("        , UPDATEDATE ")
            .Append("    FROM( ")
            .Append("       SELECT '%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-') AS CLMCATEGORY  ")
            .Append("             , T2.REC_DATETIME AS RCP_DATE  ")
            .Append("             , T4.CMPL_OVERVIEW AS COMPLAINT_OVERVIEW  ")
            .Append("             , T4.CMPL_DETAIL AS COMPLAINT_DETAIL  ")
            .Append("             , T4.CMPL_STATUS AS STATUS  ")
            .Append("             , T7.USERNAME  ")
            .Append("             , T8.ICON_IMGFILE  ")
            .Append("             , T4.ROW_UPDATE_DATETIME AS UPDATEDATE  ")
            .Append("          FROM TB_M_CUSTOMER_VCL T1  ")
            .Append("             , TB_T_REQUEST T2 ")
            .Append("             , TB_M_BUSSINES_CATEGORY T3 ")
            .Append("             , TB_T_COMPLAINT T4 ")
            .Append("             , TB_M_COMPLAINT_IMPORTANCE T5 ")
            .Append("             , TB_M_COMPLAINT_CAT T6  ")
            .Append("             , TBL_USERS T7  ")
            .Append("             , TBL_OPERATIONTYPE T8  ")
            .Append("         WHERE T1.DLR_CD = :DLRCD  ")
            .Append("           AND T1.CST_ID = :CRCUSTID ")
            .Append("           AND T1.CST_ID = :CRCUSTID ")
            .Append("           AND T3.BIZ_TYPE = '3' ")
            .Append("           AND T1.CST_ID = T2.CST_ID  ")
            .Append("           AND T1.VCL_ID = T2.VCL_ID ")
            .Append("           AND T3.BIZ_CAT_ID = T2.BIZ_CAT_ID ")
            .Append("           AND T2.REQ_ID = T4.REQ_ID ")
            .Append("           AND T4.RELATION_TYPE IN ('0','1')  ")
            .Append("           AND ((T4.CMPL_STATUS IN ('1','2'))  ")
            .Append("            OR (T4.CMPL_STATUS = '3'  ")
            .Append("           AND (EXISTS (SELECT 1  ")
            .Append("                          FROM TB_T_COMPLAINT_DETAIL T9  ")
            .Append("                         WHERE T9.CMPL_ID = T4.CMPL_ID  ")
            .Append("                           AND T9.FIRST_LAST_ACT_TYPE = '2'  ")
            .Append("                           AND T9.CMPL_DETAIL_ID = (SELECT MAX(T10.CMPL_DETAIL_ID)  ")
            .Append("                                                      FROM TB_T_COMPLAINT_DETAIL T10  ")
            .Append("                                                     WHERE T4.CMPL_ID = T10.CMPL_ID )  ")
            .Append("                           AND T9.ACT_DATETIME + :DATECOUNT >= SYSDATE  ")
            .Append("                       )  ")
            .Append("               )))  ")
            .Append("           AND T5.CMPL_IMPORTANCE_ID(+) = T4.CMPL_IMPORTANCE_ID  ")
            .Append("           AND T6.CMPL_CAT_ID(+) = T4.CMPL_CAT_ID  ")
            .Append("           AND T6.INUSE_FLG(+) = '1'  ")
            .Append("           AND T7.ACCOUNT(+) = T4.PIC_STF_CD  ")
            .Append("           AND T7.DELFLG(+) = '0'  ")
            .Append("           AND T8.OPERATIONCODE(+) = T7.OPERATIONCODE  ")
            .Append("           AND T8.DLRCD(+) = :DLRCD  ")
            .Append("           AND T8.STRCD(+) = :STRCD  ")
            .Append("           AND T8.DELFLG(+) = '0'  ")
            .Append("        UNION ALL ")
            .Append("       SELECT '%1' || '%2' || NVL(T5.CMPL_IMPORTANCE_NAME,'-') || '%2' || NVL(T6.CMPL_CAT_NAME,'-') AS CLMCATEGORY  ")
            .Append("             , T2.REC_DATETIME AS RCP_DATE  ")
            .Append("             , T4.CMPL_OVERVIEW AS COMPLAINT_OVERVIEW  ")
            .Append("             , T4.CMPL_DETAIL AS COMPLAINT_DETAIL  ")
            .Append("             , T4.CMPL_STATUS AS STATUS  ")
            .Append("             , T7.USERNAME  ")
            .Append("             , T8.ICON_IMGFILE  ")
            .Append("             , T4.ROW_UPDATE_DATETIME AS UPDATEDATE  ")
            .Append("          FROM TB_M_CUSTOMER_VCL T1  ")
            .Append("             , TB_H_REQUEST T2 ")
            .Append("             , TB_M_BUSSINES_CATEGORY T3 ")
            .Append("             , TB_H_COMPLAINT T4 ")
            .Append("             , TB_M_COMPLAINT_IMPORTANCE T5 ")
            .Append("             , TB_M_COMPLAINT_CAT T6  ")
            .Append("             , TBL_USERS T7  ")
            .Append("             , TBL_OPERATIONTYPE T8  ")
            .Append("         WHERE T1.DLR_CD = :DLRCD  ")
            .Append("           AND T1.CST_ID = :CRCUSTID ")
            .Append("           AND T1.CST_ID = :CRCUSTID ")
            .Append("           AND T3.BIZ_TYPE = '3' ")
            .Append("           AND T1.CST_ID = T2.CST_ID  ")
            .Append("           AND T1.VCL_ID = T2.VCL_ID ")
            .Append("           AND T3.BIZ_CAT_ID = T2.BIZ_CAT_ID ")
            .Append("           AND T2.REQ_ID = T4.REQ_ID ")
            .Append("           AND T4.RELATION_TYPE IN ('0','1')  ")
            .Append("           AND ((T4.CMPL_STATUS IN ('1','2'))  ")
            .Append("            OR (T4.CMPL_STATUS = '3'  ")
            .Append("           AND (EXISTS (SELECT 1  ")
            .Append("                          FROM TB_H_COMPLAINT_DETAIL T9  ")
            .Append("                         WHERE T9.CMPL_ID = T4.CMPL_ID  ")
            .Append("                           AND T9.FIRST_LAST_ACT_TYPE = '2'  ")
            .Append("                           AND T9.CMPL_DETAIL_ID = (SELECT MAX(T10.CMPL_DETAIL_ID)  ")
            .Append("                                                      FROM TB_H_COMPLAINT_DETAIL T10  ")
            .Append("                                                     WHERE T4.CMPL_ID = T10.CMPL_ID )  ")
            .Append("                           AND T9.ACT_DATETIME + :DATECOUNT >= SYSDATE  ")
            .Append("                       )  ")
            .Append("               )))  ")
            .Append("           AND T5.CMPL_IMPORTANCE_ID(+) = T4.CMPL_IMPORTANCE_ID  ")
            .Append("           AND T6.CMPL_CAT_ID(+) = T4.CMPL_CAT_ID  ")
            .Append("           AND T6.INUSE_FLG(+) = '1'  ")
            .Append("           AND T7.ACCOUNT(+) = T4.PIC_STF_CD  ")
            .Append("           AND T7.DELFLG(+) = '0'  ")
            .Append("           AND T8.OPERATIONCODE(+) = T7.OPERATIONCODE  ")
            .Append("           AND T8.DLRCD(+) = :DLRCD  ")
            .Append("           AND T8.STRCD(+) = :STRCD  ")
            .Append("           AND T8.DELFLG(+) = '0'  ) ")
            .Append("         ORDER BY UPDATEDATE DESC  ")
            .Append("    )  ")
            .Append("WHERE ROWNUM <= 1  ")
        End With
        ' 2014/03/18 TCS 松月 TMT不具合対応 Modify End
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201ImportantContactDataTable)("SC3080201_146")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crcustId)
            query.AddParameterWithTypeValue("DATECOUNT", OracleDbType.Char, dateCount)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, STRCD000)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetImportantContact_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            Return query.GetData()
        End Using
    End Function

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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerOccupation_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerOccupation_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerFamily_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerFamily_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerAppnedContact_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerAppnedContact_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteContactTimeZone_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteContactTimeZone_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertContactTimeZone_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertContactTimeZone_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerAppnedFace_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateOrgCustomerAppnedFace_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSales_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSales_End")
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
            .Append("    REGISTFLG = '0' ")
            ' 2012/02/15 TCS 相田 【SALES_2】 END
        End With
        Using query As New DBUpdateQuery("SC3080201_038")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            ' 2012/02/15 TCS 相田 【SALES_2】 DELETE
            Return query.Execute()
        End Using
    End Function

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
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
    ''' <param name="cstServiceType">接客区分</param>
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
                                                ByVal salesFlg As Boolean, ByVal cstServiceType As String) As Integer
        '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
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

            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("  , CST_SERVICE_TYPE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

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
            .Append("    SEQ_FOLLOWUPBOXSALES.NEXTVAL, ")
            .Append("    :BRANCH_PLAN, ")
            .Append("    :ACCOUNT_PLAN ")
            ' 2012/02/15 TCS 相田 【SALES_2】 END
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            .Append("  , :CST_SERVICE_TYPE ")
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
            .Append(") ")
        End With
        Using query As New DBUpdateQuery("SC3080201_039")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, custsegment)
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
            query.AddParameterWithTypeValue("WALKINNUM", OracleDbType.Int32, walkinnum)
            query.AddParameterWithTypeValue("MODULEID", OracleDbType.Char, moduleid)

            ' 2012/02/15 TCS 相田 【SALES_2】 START
            query.AddParameterWithTypeValue("NEWFLLWUPBOXFLG", OracleDbType.Char, newfllwupboxflg)
            query.AddParameterWithTypeValue("REGISTFLG", OracleDbType.Char, registflg)
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
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            If salesFlg Then
                query.AddParameterWithTypeValue("CST_SERVICE_TYPE", OracleDbType.Char, cstServiceType)
            Else
                Dim defaultValue As String = " "
                query.AddParameterWithTypeValue("CST_SERVICE_TYPE", OracleDbType.Char, defaultValue)
            End If
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

            Return query.Execute()
        End Using
    End Function
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
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
            .Append("          A.VCLREGNO, ")
            .Append("          A.VISITPERSONNUM, ")
            .Append("          A.TENTATIVENAME, ")
            .Append("          B.TELNO ")
            .Append("FROM ")
            .Append("          TBL_VISIT_SALES    A ")
            .Append("LEFT JOIN TBL_LC_VISIT_SALES B ON  A.VISITSEQ = B.VISITSEQ ")
            .Append("WHERE ")
            .Append("          A.VISITSEQ = :VISITSEQ ")
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201VisitResultDataTable)("SC3080201_040")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitseq) 'アカウント
            Return query.GetData()
        End Using
    End Function
    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)

    ''' <summary>
    ''' Follow-up BoxシーケンスNo取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetFllwupboxSeqno() As SC3080201DataSet.SC3080201SeqDataTable
        Dim sql As New StringBuilder
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START   
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSeqno_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("SELECT /* SC3080201_141 */  ")
            .Append("     SQ_SALES.NEXTVAL SEQ  ")
            .Append(" FROM  ")
            .Append("     DUAL ")
        End With
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwupboxSeqno_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201SeqDataTable)("SC3080201_141")
            query.CommandText = sql.ToString()
            Return query.GetData()
        End Using
    End Function

    '2016/09/14 TCS 河原 TMTタブレット性能改善 START
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応（引数追加）MOD START
    ''' <summary>
    ''' 顧客に対し、継続中の活動が存在するかを判定
    ''' </summary>
    ''' <param name="cstid">活動先顧客コード</param>
    ''' <param name="account">スタッフアカウント</param>
    ''' <param name="mode"></param>
    ''' <returns></returns>
    ''' <remarks>件数(返り値)>0の場合存在する。以外は存在しない。</remarks>
    Public Shared Function CountFllwupboxNotComplete(ByVal cstid As String,
                                                     ByVal account As String,
                                                     ByVal mode As String) As Integer

        Dim sql As New StringBuilder

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupboxNotComplete_Start")
        'ログ出力 End *****************************************************************************

        With sql
            Select Case mode
                Case "1"
                    .Append("SELECT 1 /* SC3080201_142_1 */ ")
                    .Append("  FROM TB_T_REQUEST T1 ")
                    .Append("     , TB_T_ACTIVITY T2 ")
                    .Append("     , TB_T_SALES T3 ")
                    .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
                    .Append("   AND T1.REQ_ID = T3.REQ_ID ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T1.REQ_STATUS = '21' ")
                    .Append("   AND T2.RSLT_FLG = '0' ")
                    .Append("   AND T3.SALES_PROSPECT_CD <> ' ' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append("   AND ROWNUM <= 1 ")
                    .Append("UNION ALL ")
                    .Append("SELECT 1 ")
                    .Append("  FROM TB_T_ATTRACT T1 ")
                    .Append("     , TB_T_ACTIVITY T2 ")
                    .Append("     , TB_T_SALES T3 ")
                    .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
                    .Append("   AND T1.ATT_ID = T3.ATT_ID ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T1.ATT_STATUS = '31' ")
                    .Append("   AND T1.CONTINUE_ACT_STATUS = '21' ")
                    .Append("   AND T2.RSLT_FLG = '0' ")
                    .Append("   AND T3.SALES_PROSPECT_CD <> ' ' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append("   AND ROWNUM <= 1 ")
                Case "2"
                    '受注後（用件・ヒストリー）
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
                    .Append("SELECT /*+ LEADING(T3 T4) USE_NL(T3 T4) INDEX(T3 TB_H_SALES_IX1)*/ 1 /* SC3080201_142_2 */ ")
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
                    .Append("  FROM TB_H_REQUEST T1 ")
                    .Append("     , TB_H_SALES T3 ")
                    .Append("     , TBL_ESTIMATEINFO T4 ")
                    .Append("     , TB_T_SALESBOOKING T5 ")
                    .Append(" WHERE T1.REQ_ID = T3.REQ_ID ")
                    .Append("   AND T3.SALES_ID = T4.FLLWUPBOX_SEQNO ")
                    .Append("   AND TRIM(T4.DLRCD) = T5.DLR_CD(+) ")
                    .Append("   AND TRIM(T4.CONTRACTNO) = T5.SALESBKG_NUM(+) ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T1.REQ_STATUS = '31' ")
                    .Append("   AND T4.CONTRACTFLG = '1' ")
                    .Append("   AND T4.CONTRACTNO IS NOT NULL ")
                    .Append("   AND T4.DELFLG = '0' ")
                    .Append("   AND T5.CANCEL_FLG(+) = '0' ")
                    .Append("   AND EXISTS ( ")
                    .Append("     SELECT ")
                    .Append("          1 ")
                    .Append("     FROM TB_T_AFTER_ODR T6 ")
                    .Append("        , TB_T_AFTER_ODR_ACT T7 ")
                    .Append("        , TB_M_AFTER_ODR_ACT T8 ")
                    .Append("     WHERE T6.AFTER_ODR_ID = T7.AFTER_ODR_ID ")
                    .Append("     AND T7.AFTER_ODR_ACT_CD = T8.AFTER_ODR_ACT_CD ")
                    .Append("       AND T6.SALES_ID = T3.SALES_ID ")
                    .Append("       AND T7.AFTER_ODR_ACT_STATUS <> 1 ")
                    .Append("       AND T8.MANDATORY_ACT_FLG = '1' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("      AND T6.AFTER_ODR_PIC_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append(" ) ")
                    .Append("   AND ROWNUM <= 1 ")
                Case "3"
                    '受注後（誘致・ヒストリー）
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
                    .Append("SELECT /*+ LEADING(T3 T4) USE_NL(T3 T4) INDEX(T3 TB_H_SALES_IX1)*/ 1 /* SC3080201_142_3 */ ")
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
                    .Append("  FROM TB_H_ATTRACT T1 ")
                    .Append("     , TB_H_SALES T3 ")
                    .Append("     , TBL_ESTIMATEINFO T4 ")
                    .Append("     , TB_T_SALESBOOKING T5 ")
                    .Append(" WHERE T1.ATT_ID = T3.ATT_ID ")
                    .Append("   AND T3.SALES_ID = T4.FLLWUPBOX_SEQNO ")
                    .Append("   AND TRIM(T4.DLRCD) = T5.DLR_CD(+) ")
                    .Append("   AND TRIM(T4.CONTRACTNO) = T5.SALESBKG_NUM(+) ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T1.ATT_STATUS = '31' ")
                    .Append("   AND T1.CONTINUE_ACT_STATUS = '31' ")
                    .Append("   AND T4.CONTRACTFLG = '1' ")
                    .Append("   AND T4.CONTRACTNO IS NOT NULL ")
                    .Append("   AND T4.DELFLG = '0' ")
                    .Append("   AND T5.CANCEL_FLG(+) = '0' ")
                    .Append("   AND EXISTS ( ")
                    .Append("     SELECT ")
                    .Append("          1 ")
                    .Append("     FROM TB_T_AFTER_ODR T6 ")
                    .Append("        , TB_T_AFTER_ODR_ACT T7 ")
                    .Append("        , TB_M_AFTER_ODR_ACT T8 ")
                    .Append("     WHERE T6.AFTER_ODR_ID = T7.AFTER_ODR_ID ")
                    .Append("     AND T7.AFTER_ODR_ACT_CD = T8.AFTER_ODR_ACT_CD ")
                    .Append("       AND T6.SALES_ID = T3.SALES_ID ")
                    .Append("       AND T7.AFTER_ODR_ACT_STATUS <> 1 ")
                    .Append("       AND T8.MANDATORY_ACT_FLG = '1' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("      AND T6.AFTER_ODR_PIC_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append(" ) ")
                Case "4"
                    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
                    '受注後（用件・ヒストリー）過渡期対応
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
                    .Append("SELECT /*+ LEADING(T3 T4) USE_NL(T3 T4) INDEX(T3 TB_H_SALES_IX1)*/ 1 /* SC3080201_142_4 */ ")
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
                    .Append("  FROM TB_H_REQUEST T1 ")
                    .Append("     , TB_H_ACTIVITY T2 ")
                    .Append("     , TB_H_SALES T3 ")
                    .Append("     , TBL_ESTIMATEINFO T4 ")
                    .Append("     , TB_T_SALESBOOKING T5 ")
                    .Append(" WHERE T1.REQ_ID = T2.REQ_ID ")
                    .Append("   AND T1.REQ_ID = T3.REQ_ID ")
                    .Append("   AND T3.SALES_ID = T4.FLLWUPBOX_SEQNO ")
                    .Append("   AND TRIM(T4.DLRCD) = T5.DLR_CD(+) ")
                    .Append("   AND TRIM(T4.CONTRACTNO) = T5.SALESBKG_NUM(+) ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T2.ACT_STATUS = '31' ")
                    .Append("   AND T4.CONTRACTFLG = '1' ")
                    .Append("   AND T4.CONTRACTNO IS NOT NULL ")
                    .Append("   AND T4.DELFLG = '0' ")
                    .Append("   AND T5.CANCEL_FLG(+) = '0' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append("   AND NOT EXISTS(SELECT 1 FROM TB_T_AFTER_ODR T7 WHERE T7.SALES_ID = T3.SALES_ID) ")
                    .Append("   AND NOT EXISTS(SELECT 1 FROM TB_H_AFTER_ODR T8 WHERE T8.SALES_ID = T3.SALES_ID) ")
                    .Append("   AND ROWNUM <= 1 ")
                    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
                    .Append("   AND ROWNUM <= 1 ")
                Case "5"
                    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 START
                    '受注後（誘致・ヒストリー）過渡期対応
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) START
                    .Append("SELECT /*+ LEADING(T3 T4) USE_NL(T3 T4) INDEX(T3 TB_H_SALES_IX1)*/ 1 /* SC3080201_142_5 */ ")
                    '2019/11/26 TS 髙橋(龍) SQL性能改善(TR-SLT-TMT-20190503-001) END
                    .Append("  FROM TB_H_ATTRACT T1 ")
                    .Append("     , TB_H_ACTIVITY T2 ")
                    .Append("     , TB_H_SALES T3 ")
                    .Append("     , TBL_ESTIMATEINFO T4 ")
                    .Append("     , TB_T_SALESBOOKING T5 ")
                    .Append(" WHERE T1.ATT_ID = T2.ATT_ID ")
                    .Append("   AND T1.ATT_ID = T3.ATT_ID ")
                    .Append("   AND T3.SALES_ID = T4.FLLWUPBOX_SEQNO ")
                    .Append("   AND TRIM(T4.DLRCD) = T5.DLR_CD(+) ")
                    .Append("   AND TRIM(T4.CONTRACTNO) = T5.SALESBKG_NUM(+) ")
                    .Append("   AND T1.CST_ID = :CUSTID ")
                    .Append("   AND T3.CST_ID = :CUSTID ")
                    .Append("   AND T1.ATT_STATUS = '31' ")
                    .Append("   AND T1.CONTINUE_ACT_STATUS = '31' ")
                    .Append("   AND T2.ACT_STATUS = '31' ")
                    .Append("   AND T4.CONTRACTFLG = '1' ")
                    .Append("   AND T4.CONTRACTNO IS NOT NULL ")
                    .Append("   AND T4.DELFLG = '0' ")
                    .Append("   AND T5.CANCEL_FLG(+) = '0' ")
                    If Not String.IsNullOrEmpty(account) Then
                        .Append("   AND T2.SCHE_STF_CD = :ACCOUNT_PLAN ")
                    End If
                    .Append("   AND NOT EXISTS(SELECT 1 FROM TB_T_AFTER_ODR T7 WHERE T7.SALES_ID = T3.SALES_ID) ")
                    .Append("   AND NOT EXISTS(SELECT 1 FROM TB_H_AFTER_ODR T8 WHERE T8.SALES_ID = T3.SALES_ID) ")
                    .Append("   AND ROWNUM <= 1 ")
                    '2014/07/09 TCS 高橋 受注後活動完了条件変更対応 END
            End Select
        End With

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupboxNotComplete_End")
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
    '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応（引数追加）MOD END
    '2016/09/14 TCS 河原 TMTタブレット性能改善 END

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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupbox_Start")
        'ログ出力 End *****************************************************************************
        ' 2014/03/18 TCS 松月 TMT不具合対応 Modify Start 
        With sql
            .Append("SELECT /* SC3080201_143 */ COUNT(1) ")
            .Append("FROM ")
            .Append("(SELECT ")
            .Append("1 ")
            .Append("FROM ")
            .Append("  TB_T_SALES ")
            .Append("WHERE ")
            .Append("      SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("  AND ROWNUM <= 1 ")
            .Append("UNION ALL ")
            .Append("SELECT ")
            .Append("1 ")
            .Append("FROM ")
            .Append("  TB_H_SALES ")
            .Append("WHERE ")
            .Append("      SALES_ID = :FLLWUPBOX_SEQNO ")
            .Append("  AND ROWNUM <= 1) ")
        End With
        ' 2014/03/18 TCS 松月 TMT不具合対応 Modify End
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201CountDataTable)("SC3080201_143")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupbox_seqno)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountFllwupbox_End")
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
            .Append("    DECODE(A.STATUS,'1',TOACCOUNT,'3',FROMACCOUNT,'4',FROMACCOUNT) AS TOACCOUNT ")
            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

            .Append("FROM ")
            .Append("    TBL_NOTICEREQUEST A, ")
            .Append("    TBL_NOTICEINFO B ")
            .Append("WHERE ")
            .Append("    A.FLLWUPBOXSTRCD = :FLLWUPBOXSTRCD AND ")
            .Append("    A.FLLWUPBOX = :FLLWUPBOX AND ")

            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            .Append("    A.STATUS IN ('1','3','4') AND ")  '1:依頼、3:受信、4:回答(承認)
            '2012/12/10 TCS 坪根 【A.STEP2】MOD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END
            '2018/11/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
            .Append("    A.NOTICEREQCTG IN ('01', '02', '03', '08') AND ")  '01:査定、02:価格相談、03:ヘルプ、08:契約承認
            '2018/11/26 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVisitSales_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVisitSales_End")
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
    '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
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
    ''' <param name="cstServiceType">接客区分</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateFllwupboxSales(ByVal dlrcd As String, _
                                          ByVal strcd As String, _
                                          ByVal fllwupboxseqno As Decimal, _
                                          ByVal account As String, _
                                          ByVal id As String, _
                                          ByVal salesFlg As Boolean, _
                                          ByVal startFlg As Boolean,
                                          ByVal cstServiceType As String) As Integer
        '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
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
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
                If startFlg AndAlso salesFlg Then
                    '商談、納車活動開始の場合、接客区分を更新する
                    .Append("      , CST_SERVICE_TYPE = :CST_SERVICE_TYPE ")
                End If
                '2014/02/12 TCS 高橋 受注後フォロー機能開発 END
                .Append(" WHERE  FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                .Append(" AND    REGISTFLG = '0' ")


            End With

            query.CommandText = sql.ToString()
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno) 'Follow-up Box内連番
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END    
            query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Char, account) '対応アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, account) '更新アカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, id) '機能ID
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 START
            If startFlg AndAlso salesFlg Then
                '商談、納車活動開始の場合、接客区分を更新する
                query.AddParameterWithTypeValue("CST_SERVICE_TYPE", OracleDbType.Char, cstServiceType) '接客区分
            End If
            '2014/02/12 TCS 高橋 受注後フォロー機能開発 END

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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesTime_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesTime_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesSeqNoByRegitFlg_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSalesSeqNoByRegitFlg_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustchrgInfo_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustchrgInfo_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwUpBoxCustchrgInfo_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetFllwUpBoxCustchrgInfo_End")
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
            .Append("    CONTRACTFLG , ")                '契約状況フラグ
            .Append("    CONTRACT_APPROVAL_STATUS ")     '契約承認ステータス
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
            .Append("            AND DELFLG = '0' ")
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
            .Append("            AND DELFLG = '0' ")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerLock_Start")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerLock_End")
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

    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 START
    ''' <summary>
    ''' DMSID取得(自社客)
    ''' </summary>
    ''' <param name="originalId">顧客コード</param>
    ''' <returns>SC3080201DmsIdDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdOrg(ByVal originalId As String) As SC3080201DataSet.SC3080201DmsIdDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_062 */ ")
            .Append("    DMS_CST_CD_DISP AS CUSTCD ")    '基幹顧客コード
            .Append("FROM ")
            .Append("    TB_M_CUSTOMER ")
            .Append("WHERE ")
            .Append("    CST_ID = :ORIGINALID ")         '顧客コード
        End With
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201DmsIdDataTable)("SC3080201_062")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalId)  '顧客コード
            Dim rtnDt As SC3080201DataSet.SC3080201DmsIdDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function

    ''' <summary>
    ''' DMSID取得(未取引客)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="salesBkgNo">注文番号</param>
    ''' <returns>SC3080201DmsIdDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDmsIdNew(ByVal dlrcd As String, ByVal salesBkgNo As String) As SC3080201DataSet.SC3080201DmsIdDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080201_063 */ ")
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
        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201DmsIdDataTable)("SC3080201_063")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)  '販売店コード
            query.AddParameterWithTypeValue("SALESBKGNO", OracleDbType.Char, salesBkgNo)  '注文番号
            Dim rtnDt As SC3080201DataSet.SC3080201DmsIdDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function
    '2013/11/06 TCS 山田 ADD i-CROP再構築後の新車納車システムに追加したリンク対応 END

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
                                                  ByVal dlrcd As String, _
                                                  ByVal row_update_account As String, _
                                                  ByVal row_lock_version As Long) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewCustomerSnsId_Start")
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
                .Append("     , ROW_UPDATE_FUNCTION = 'SC3080201' ")
                .Append("     , ROW_LOCK_VERSION = :ROW_LOCK_VERSION +1 ")
                .Append(" WHERE DLR_CD = :DLRCD ")
                .Append("   AND CST_ID = :CSTID ")
                .Append("   AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("SNSID", OracleDbType.NVarchar2, snsid)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, row_update_account)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, row_lock_version)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewCustomerSnsId_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewCustomerKeyword_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewCustomerKeyword_End")
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

#Region "Aカード情報相互連携開発"
    ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 商談データ件数取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>商談テーブル行数・商談一時テーブル行数</returns>
    ''' <remarks></remarks>
    Public Shared Function CountSalesInfo(ByVal salesId As Decimal) As SC3080201DataSet.SC3080201SalesInfoDataTable

        Using query As New DBSelectQuery(Of SC3080201DataSet.SC3080201SalesInfoDataTable)("SC3080201_201")

            Dim sql As New StringBuilder(10000)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountSalesInfo_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .AppendLine("SELECT /* SC3080201_201 */ ")
                .AppendLine("   SLS.SALES_ROWS_COUNT")
                .AppendLine("   ,SLST.SALES_TEMP_ROWS_COUNT")
                .AppendLine("   ,SLSH.SALES_HIS_ROWS_COUNT")
                .AppendLine("FROM ")
                .AppendLine("   (SELECT ")
                .AppendLine("       COUNT(1) AS SALES_ROWS_COUNT")
                .AppendLine("    FROM TB_T_SALES ")
                .AppendLine("    WHERE SALES_ID = :SALES_ID")
                .AppendLine("               AND ROWNUM<=1")
                .AppendLine("   ) SLS")
                .AppendLine("   ,(SELECT ")
                .AppendLine("    	COUNT(1) AS SALES_TEMP_ROWS_COUNT")
                .AppendLine("    FROM TB_T_SALES_TEMP ")
                .AppendLine("    WHERE SALES_ID = :SALES_ID")
                .AppendLine("               AND ROWNUM<=1")
                .AppendLine("   ) SLST")
                .AppendLine("   ,(SELECT ")
                .AppendLine("    	COUNT(1) AS SALES_HIS_ROWS_COUNT")
                .AppendLine("    FROM TB_H_SALES ")
                .AppendLine("    WHERE SALES_ID = :SALES_ID")
                .AppendLine("               AND ROWNUM<=1")
                .AppendLine("   ) SLSH")
            End With

            query.CommandText = sql.ToString()
            sql.Clear()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CountSalesInfo_Start")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    ' 2016/05/16 TCS 鈴木 BTS-28(TMT-106DLR) 基幹連携の取り込みでエラー START
    ''' <summary>
    ''' 商談一時情報登録処理
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function InsertSalesTemp(ByVal salesId As Decimal, ByVal updateAccount As String, ByVal dlrCd As String) As Integer

        Using query As New DBUpdateQuery("SC3080201_202")

            Dim sql As New StringBuilder(10000)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSalesTemp_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .AppendLine("INSERT /* SC3080201_202 */ ")
                .AppendLine("INTO TB_T_SALES_TEMP ")
                .AppendLine("( ")
                .AppendLine("    SALES_ID,")
                .AppendLine("    ROW_CREATE_DATETIME , ")
                .AppendLine("    ROW_CREATE_ACCOUNT , ")
                .AppendLine("    ROW_CREATE_FUNCTION , ")
                .AppendLine("    ROW_UPDATE_DATETIME , ")
                .AppendLine("    ROW_UPDATE_ACCOUNT , ")
                .AppendLine("    ROW_UPDATE_FUNCTION , ")
                .AppendLine("    ROW_LOCK_VERSION , ")
                .AppendLine("    DLR_CD ")
                .AppendLine(") VALUES ( ")
                .AppendLine("    :SALES_ID , ")
                .AppendLine("    SYSDATE , ")
                .AppendLine("    :ACCOUNT , ")
                .AppendLine("   'SC3080201' , ")
                .AppendLine("    SYSDATE , ")
                .AppendLine("    :ACCOUNT , ")
                .AppendLine("    'SC3080201' , ")
                .AppendLine("    0 , ")
                .AppendLine("    :DLR_CD")
                .AppendLine(")")
            End With

            query.CommandText = sql.ToString()
            sql.Clear()

            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertSalesTemp_End")
            'ログ出力 End *****************************************************************************
            Dim ret As Integer = 0
            ret = query.Execute()
            Return ret

        End Using
    End Function
    ' 2016/05/16 TCS 鈴木 BTS-28(TMT-106DLR) 基幹連携の取り込みでエラー END

    ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
#End Region

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

End Class
