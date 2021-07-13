
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3060101TableAdapter.vb
'─────────────────────────────────────
'機能： 査定チェックシートデータアクセス
'補足： 
'作成： 2011/11/29 KN 清水
'更新： 2012/03/19 KN 清水     【SALES_1B】SALES_1B UT(課題No.0023) TCV遷移対応
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $02
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Public NotInheritable Class SC3060101TableAdapter

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

    ''' <summary>開始ログ</summary>
    Private Const STARTLOG As String = "START "

    ''' <summary>終了ログ</summary>
    Private Const ENDLOG As String = "END "

    ''' <summary>終了ログRETURN</summary>
    Private Const ENDLOGRETURN As String = "RETURN "

#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

  

    ''' <summary>
    ''' 自社客取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustId">活動先顧客コード(オリジナルID)</param>
    ''' <returns>SC3060101OrgCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrgCustomer(ByVal dlrcd As String, _
                                          ByVal crcustId As String) As SC3060101DataSet.SC3060101OrgCustomerDataTable

        Const METHODNAME As String = "GetOrgCustomer "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101OrgCustomerDataTable)("SC3060101_001")

            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT  /* SC3060101_001 */ ")
                .Append("        CUST.NAMETITLE_NAME AS NAMETITLE ")
                .Append("      , CUST.CST_NAME AS NAME ")
                .Append("   FROM TB_M_CUSTOMER CUST ")
                .Append("  WHERE CUST.CST_ID = :CST_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, crcustId) 'オリジナルID
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 未取引客取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustId">活動先顧客コード(未取引客ユーザーID)</param>
    ''' <returns>SC3060101NewCustomerDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNewCustomer(ByVal dlrcd As String, _
                                          ByVal crcustId As String) As SC3060101DataSet.SC3060101NewCustomerDataTable

        Const METHODNAME As String = "GetOrgCustomer "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101NewCustomerDataTable)("SC3060101_002")

            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3060101_002 */ ")
                .Append("        CUST.NAMETITLE_NAME AS NAMETITLE ")
                .Append("      , CUST.CST_NAME AS NAME ")
                .Append("   FROM TB_M_CUSTOMER CUST ")
                .Append("  WHERE CUST.CST_ID = :CST_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, crcustId) '未取引客ユーザーID
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return query.GetData()
        End Using
    End Function


    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="strcd"></param>
    ''' <param name="fllwupboxSeqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal dlrcd As String _
                                              , ByVal strcd As String _
                                              , ByVal fllwupboxSeqno As Decimal) As SC3060101DataSet.SC3060101GetStatusToDataTable
        Dim sql As New StringBuilder

        Const METHODNAME As String = "GetFollowupboxStatus "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        '活動ステータス取得SQL
        With sql
            .Append(" SELECT /* SC3060101_003 */  ")
            .Append("        T2.REQ_STATUS AS CRACTRESULT ")
            .Append("      , NVL(TRIM(T3.RSLT_CONTACT_MTD), 0) AS REQCATEGORY ")
            .Append("      , T3.SCHE_STF_CD AS ACCOUNT_PLAN  ")
            .Append("   FROM ")
            .Append("        TB_T_SALES T1 ")
            .Append("      , TB_T_REQUEST T2 ")
            .Append("      , TB_T_ACTIVITY T3 ")
            .Append("  WHERE  ")
            .Append("        T1.REQ_ID(+) = T3.REQ_ID ")
            .Append("    AND T1.ATT_ID(+) = T3.ATT_ID ")
            .Append("    AND T2.REQ_ID(+) = T1.REQ_ID ")
            .Append("    AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101GetStatusToDataTable)("SC3060101_003")
            query.CommandText = sql.ToString()
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxSeqno)  '内部管理ID
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="strcd"></param>
    ''' <param name="fllwupboxSeqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatusPast(ByVal dlrcd As String _
                                                  , ByVal strcd As String _
                                                  , ByVal fllwupboxSeqno As Decimal) As SC3060101DataSet.SC3060101GetStatusToDataTable
        Dim sql As New StringBuilder

        Const METHODNAME As String = "GetFollowupboxStatusPast "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        '活動ステータス取得SQL
        With sql
            .Append(" SELECT /* SC3060101_004 */  ")
            .Append("        T2.REQ_STATUS AS CRACTRESULT ")
            .Append("      , NVL(TRIM(T3.RSLT_CONTACT_MTD), 0) AS REQCATEGORY ")
            .Append("      , T3.SCHE_STF_CD AS ACCOUNT_PLAN  ")
            .Append("   FROM ")
            .Append("        TB_H_SALES T1 ")
            .Append("      , TB_H_REQUEST T2 ")
            .Append("      , TB_H_ACTIVITY T3 ")
            .Append("  WHERE  ")
            .Append("        T1.REQ_ID(+) = T3.REQ_ID ")
            .Append("    AND T1.ATT_ID(+) = T3.ATT_ID ")
            .Append("    AND T2.REQ_ID(+) = T1.REQ_ID ")
            .Append("    AND T1.SALES_ID = :FLLWUPBOX_SEQNO ")
        End With
        '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101GetStatusToDataTable)("SC3060101_004")
            query.CommandText = sql.ToString()
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxSeqno)  '内部管理ID
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return query.GetData()
        End Using

    End Function

    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 START
    ''' <summary>
    ''' 見積ID取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxSeqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateInfo(ByVal dlrcd As String _
                                         , ByVal strcd As String _
                                         , ByVal fllwupboxSeqno As Decimal) As SC3060101DataSet.SC3060101GetEstimateidToDataTable
        Dim sql As New StringBuilder

        '見積ID取得SQL
        With sql
            .Append(" SELECT /* SC3060101_005 */ ")
            .Append("        ESTIMATEID ")
            .Append("   FROM TBL_ESTIMATEINFO ")
            .Append("  WHERE DLRCD = :DLRCD ")
            .Append("    AND STRCD = :STRCD ")
            .Append("    AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            .Append("    AND DELFLG = '0' ")
        End With

        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101GetEstimateidToDataTable)("SC3060101_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                  '店舗コード
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxSeqno)  '内部管理ID
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
            Return query.GetData()
        End Using
    End Function
    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <param name="EstimateId">見積もりID</param>
    ''' <returns>SC3080201ContactFlgDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal EstimateId As String) As SC3060101DataSet.SC3060101ContractDataTable
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3060101_006 */ ")
            .Append("    CONTRACTFLG ")                '契約状況フラグ
            .Append("FROM ")
            .Append("    TBL_ESTIMATEINFO ")
            .Append("WHERE ")
            .Append("    ESTIMATEID = :ESTIMATEID ")   '見積もりID
        End With
        Using query As New DBSelectQuery(Of SC3060101DataSet.SC3060101ContractDataTable)("SC3060101_006")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Char, EstimateId)  '見積もりID
            Dim rtnDt As SC3060101DataSet.SC3060101ContractDataTable = query.GetData()
            Return rtnDt
        End Using
    End Function
    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 END
End Class