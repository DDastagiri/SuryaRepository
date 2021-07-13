'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3040802TableAdapter.vb
'─────────────────────────────────────
'機能： 通知一覧(MG用)
'補足： 
'作成： 2012/01/05 TCS 明瀬
'更新： 2012/12/02 TCS 森 Aカード情報相互連携開発
'更新： 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

''' <summary>
''' 通知送受信一覧(MG用)
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3040802TableAdapter

#Region "定数"
    ''' <summary>
    ''' 通知依頼種別　価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_DISCOUNTAPPROVAL As String = "02"

    ''' <summary>
    ''' 通知依頼種別　ヘルプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_HELP As String = "03"

    ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 通知依頼種別 注文承認
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_ORDER As String = "08"
    ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END

    ''' <summary>
    ''' ステータス　依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_REQUEST As String = "1"

    ''' <summary>
    ''' ステータス　受信
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_RECEIVE As String = "3"

    ''' <summary>
    ''' 店舗コード　000
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STRCD_000 As String = "000"

#End Region

#Region "SELECT"

    ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 依頼中の通知情報を取得する
    ''' </summary>
    ''' <param name="account"></param>
    ''' <param name="dlrCD"></param>
    ''' <param name="isTeamLeader"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeInfo(ByVal account As String, ByVal dlrCD As String, isTeamLeader As Boolean) As SC3040802DataSet.SC3040802NoticeInfoDataTable
        ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[account:{0}][dlrCd:{1}]", account, dlrCD))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_001 */ ")
            .Append("     A.NOTICEREQID ")
            .Append("   , A.NOTICEREQCTG ")
            .Append("   , A.REQCLASSID ")
            .Append("   , A.CUSTOMNAME ")
            .Append("   , A.CRCUSTID ")
            .Append("   , A.CUSTOMERCLASS ")
            .Append("   , A.CSTKIND ")
            .Append("   , B.SENDDATE ")
            .Append("   , B.FROMACCOUNTNAME ")
            .Append("   , B.FROMACCOUNT ")
            .Append("   , D.ICON_IMGFILE ")
            .Append("   , A.SALESSTAFFCD ")
            .Append("   , A.FLLWUPBOXSTRCD ")
            .Append("   , A.FLLWUPBOX ")
            .Append("   , A.STATUS ")
            .Append(" FROM ")
            .Append("     TBL_NOTICEREQUEST A ")
            .Append("   , TBL_NOTICEINFO B ")
            .Append("   , TBL_USERS C ")
            .Append("   , TBL_OPERATIONTYPE D ")
            .Append(" WHERE ")
            .Append("     A.NOTICEREQID = B.NOTICEREQID ")
            .Append(" AND A.DLRCD = C.DLRCD ")
            .Append(" AND B.FROMACCOUNT = C.ACCOUNT(+) ")
            .Append(" AND C.DLRCD = D.DLRCD(+) ")
            .Append(" AND C.OPERATIONCODE = D.OPERATIONCODE(+) ")
            .Append(" AND ( A.NOTICEREQCTG = :NOTICEREQCTG02 ")
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
            .Append(" OR  A.NOTICEREQCTG = :NOTICEREQCTG03 ")
            .Append(" OR  A.NOTICEREQCTG = :NOTICEREQCTG08 ) ")
            'TL→SCMへの依頼時、依頼者の通知データを表示対象から除外する。
            If isTeamLeader Then
                .Append(" AND B.TOACCOUNT <> B.FROMACCOUNT ")
            End If
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END
            .Append(" AND A.DLRCD = :DLRCD ")
            .Append(" AND ( A.STATUS = :STATUS1 ")
            .Append(" OR  A.STATUS = :STATUS3 ) ")
            .Append(" AND B.TOACCOUNT = :TOACCOUNT ")
            .Append(" AND B.STATUS = :STATUS1 ")
            .Append(" AND D.STRCD = :STRCD000 ")
            .Append(" ORDER BY ")
            .Append("    B.SENDDATE ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802NoticeInfoDataTable)("SC3040802_001")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("NOTICEREQCTG02", OracleDbType.Char, NOTICEREQ_DISCOUNTAPPROVAL)    '依頼種別ID(価格相談)
            query.AddParameterWithTypeValue("NOTICEREQCTG03", OracleDbType.Char, NOTICEREQ_HELP)                '依頼種別ID(ヘルプ)
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("NOTICEREQCTG08", OracleDbType.Char, NOTICEREQ_ORDER)               '依頼種別ID(注文承認)
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)                                  '販売店コード
            query.AddParameterWithTypeValue("STATUS1", OracleDbType.Char, STATUS_REQUEST)                       'ステータス(依頼)
            query.AddParameterWithTypeValue("STATUS3", OracleDbType.Char, STATUS_RECEIVE)                       'ステータス(受信)
            query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Char, account)                            'ログインアカウント
            query.AddParameterWithTypeValue("STRCD000", OracleDbType.Char, STRCD_000)                           '店舗コード"000"

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802NoticeInfoDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' 価格相談内容を取得する
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDiscountApproval(ByVal estimateId As Long) As SC3040802DataSet.SC3040802DiscountApprovalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & " _Start[estimateId:{0}]", estimateId))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_002 */ ")
            .Append("      A.SERIESNM ")
            .Append("    , A.MODELNM ")
            .Append("    , A.REQUESTPRICE ")
            .Append(" FROM ")
            .Append("     TBL_EST_DISCOUNTAPPROVAL A ")
            .Append(" WHERE ")
            .Append("     A.ESTIMATEID = :ESTMATEID ")
            .Append(" ORDER BY ")
            .Append("    A.SEQNO DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802DiscountApprovalDataTable)("SC3040802_002")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTMATEID", OracleDbType.Long, estimateId)  '見積管理ID

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802DiscountApprovalDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' ヘルプ依頼内容を取得する
    ''' </summary>
    ''' <param name="helpId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHelpInfo(ByVal helpId As Long, ByVal dlrCD As String) As SC3040802DataSet.SC3040802HelpInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & " _Start[helpId:{0}][dlrCD:{1}]", helpId.ToString(CultureInfo.CurrentCulture), dlrCD))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_003 */ ")
            .Append("      A.HELPNO ")
            .Append("    , B.MSG_DLR ")
            .Append(" FROM ")
            .Append("      TBL_NOTICEHELPINFO A ")
            .Append("    , TBL_REQUESTINFOMST B ")
            .Append(" WHERE ")
            .Append("     A.ID = B.ID ")
            .Append(" AND A.HELPNO = :HELPID ")
            .Append(" AND B.DLRCD = :DLRCD ")
            .Append(" AND B.STRCD = '000' ")
            .Append(" AND B.REQCLASS = :REQCLASS ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802HelpInfoDataTable)("SC3040802_003")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("HELPID", OracleDbType.Long, helpId)            'ヘルプID
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Long, dlrCD)              '販売店コード
            query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, NOTICEREQ_HELP)  '依頼種別ID(ヘルプ)

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802HelpInfoDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' 依頼中の通知情報件数を取得する
    ''' </summary>
    ''' <param name="account"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeInfoCount(ByVal account As String, ByVal dlrCD As String) As SC3040802DataSet.SC3040802NoticeCountDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[account:{0}][dlrCD:{1}]", account, dlrCD))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_004 */ ")
            .Append("    COUNT(1) COUNT")
            .Append(" FROM ")
            .Append("     TBL_NOTICEREQUEST A ")
            .Append("   , TBL_NOTICEINFO B ")
            .Append(" WHERE ")
            .Append("     A.NOTICEREQID = B.NOTICEREQID ")
            .Append(" AND ( A.NOTICEREQCTG = :NOTICEREQCTG02 ")
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
            .Append(" OR  A.NOTICEREQCTG = :NOTICEREQCTG03 ")
            .Append(" OR  A.NOTICEREQCTG = :NOTICEREQCTG08 ) ")
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END
            .Append(" AND A.DLRCD = :DLRCD ")
            .Append(" AND ( A.STATUS = :STATUS1 ")
            .Append(" OR  A.STATUS = :STATUS3 ) ")
            .Append(" AND B.TOACCOUNT = :TOACCOUNT ")
            .Append(" AND B.STATUS = :STATUS1 ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802NoticeCountDataTable)("SC3040802_004")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("NOTICEREQCTG02", OracleDbType.Char, NOTICEREQ_DISCOUNTAPPROVAL)    '依頼種別ID(価格相談)
            query.AddParameterWithTypeValue("NOTICEREQCTG03", OracleDbType.Char, NOTICEREQ_HELP)               '依頼種別ID(ヘルプ)
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("NOTICEREQCTG08", OracleDbType.Char, NOTICEREQ_ORDER)               '依頼種別ID(注文承認)
            ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END             '依頼種別ID(ヘルプ)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)                                  '販売店コード
            query.AddParameterWithTypeValue("STATUS1", OracleDbType.Char, STATUS_REQUEST)                       'ステータス(依頼)
            query.AddParameterWithTypeValue("STATUS3", OracleDbType.Char, STATUS_RECEIVE)                       'ステータス(受信)
            query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Char, account)                            'ログインアカウント

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802NoticeCountDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ' 2012/12/02 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 注文承認内容を取得する
    ''' </summary>
    ''' <param name="estimateID">見積管理ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEstimateApproval(ByVal estimateID As Integer) As SC3040802DataSet.SC3040802EstimateApprovalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[EstimateID:{0}]", estimateID))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_005 */ ")
            .Append("      SERIESNM ")
            .Append("    , MODELNM ")
            .Append(" FROM ")
            .Append(" TBL_EST_VCLINFO ")
            .Append(" WHERE ")
            .Append("     ESTIMATEID = :ESTMATEID ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802EstimateApprovalDataTable)("SC3040802_005")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTMATEID", OracleDbType.Char, estimateID)                        '見積管理ID

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802EstimateApprovalDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ' 2012/12/02 TCS 森 Aカード情報相互連携開発 END

    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 重要車両フラグを取得する
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="cstID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetImpVclFlg(ByVal dlrCD As String, ByVal cstID As Decimal) As SC3040802DataSet.SC3040802ImpVclFlgDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[dlrCD:{0}][cstID:{1}]", dlrCD, cstID))

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3040802_006 */ ")
            .Append("     A.IMP_VCL_FLG ")
            .Append(" FROM ")
            .Append("     TB_M_VEHICLE_DLR A ")
            .Append(" INNER JOIN ")
            .Append("     TB_M_CUSTOMER_VCL B ")
            .Append(" ON  A.DLR_CD = B.DLR_CD ")
            .Append(" AND A.VCL_ID = B.VCL_ID ")
            .Append(" WHERE ")
            .Append("     B.DLR_CD = :DLR_CD ")
            .Append(" AND B.CST_ID = :CST_ID ")
            .Append(" AND ROWNUM <= 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3040802DataSet.SC3040802ImpVclFlgDataTable)("SC3040802_006")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dlrCD)    '販売店コード
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstID)    '顧客コード

            'SQL実行
            Dim rtnDt As SC3040802DataSet.SC3040802ImpVclFlgDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function
    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

#End Region

End Class
