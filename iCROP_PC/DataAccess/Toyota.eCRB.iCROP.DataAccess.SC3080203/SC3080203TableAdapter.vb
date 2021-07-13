'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動結果)
'補足： 
'作成： 2011/12/01 TCS 河原
'更新： 2012/02/15 TCS 河原 【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正
'更新： 2012/03/02 TCS 安田 【STEP2】接触方法マスタ・受注後フラグ条件追加 
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3080203DataSetTableAdapters
    Public NotInheritable Class SC3080203TableAdapter

#Region "定数"

        ''' <summary>
        ''' Follow-upBoxのCR活動スタータス
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CONSTFLLWUPHOT As String = "1"
        Public Const CONSTFLLWUPPROSPECT As String = "2"
        Public Const CONSTFLLWUPREPUCHASE As String = "3"
        Public Const CONSTFLLWUPPERIODICAL As String = "4"
        Public Const CONSTFLLWUPPROMOTION As String = "5"
        Public Const CONSTFLLWUPREQUEST As String = "6"
        Public Const CONSTFLLWUPWALKIN As String = "7"

        ''' <summary>
        ''' Follow-upBoxの活動結果
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CONSTCRACTRSLTHOT As String = "1"
        Public Const CONSTCRACTRSLTPROSPECT As String = "2"
        Public Const CONSTCRACTRSLTSUCCESS As String = "3"
        Public Const CONSTCRACTRSLTCONTINUE As String = "4"
        Public Const CONSTCRACTRSLTGIVEUP As String = "5"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CONSTSELECTTOHOT = "3"
        Public Const CONSTSELECTTOPROC = "2"
        Public Const CONSTSELECTTOWALK = "1"

        'Public Const FLLWUPTYP = ""

        Public Const CONSTCUSTOMERCLASSOWNER = "1"

        Public Const CONSTCUSTSEGMENTCUSTOMER = "1"
        Public Const CONSTCUSTSEGMENTNEWCSTOMER = "2"

#End Region

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
            '処理なし
        End Sub

        ''' <summary>
        ''' 001.アラームマスタ取得
        ''' </summary>
        ''' <param name="selection">0: どちらでも選択可、1: 時間指定がある場合のみ選択可、2: 時間指定が無い場合のみ選択可</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetAlarmMaster(ByVal selection As String) As SC3080203DataSet.SC3080203AlarmMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203AlarmMasterDataTable)("SC3080203_001")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_001 */ ")
                    .Append("    ALARMNO, ")
                    .Append("    UNIT, ")
                    .Append("    TIME ")
                    .Append("FROM ")
                    .Append("    TBL_ALARM ")
                    .Append("WHERE ")
                    .Append("    DELFLG = '0' AND ")
                    .Append("    SELECTION IN ('0',:SELECTION) ")
                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SELECTION", OracleDbType.Char, selection)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 002.競合メーカーマスタ取得(ALL)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionMakermasterDataTable)("SC3080203_002")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_002 */ ")
                    .Append("    A.COMPETITIONMAKERNO, ")
                    .Append("    A.COMPETITIONMAKER ")
                    .Append("FROM ")
                    .Append("    TBL_COMPETITION_MAKERMASTER A ")
                    .Append("WHERE ")
                    '.Append("    EXISTS ")
                    '.Append("        ( ")
                    '.Append("        SELECT ")
                    '.Append("            1 ")
                    '.Append("        FROM ")
                    '.Append("            TBL_COMPETITORMASTER B ")
                    '.Append("        WHERE ")
                    '.Append("            A.COMPETITIONMAKERNO = B.COMPETITIONMAKERNO AND ")
                    '.Append("            B.DELFLG = '0' ")
                    '.Append("        ) AND ")
                    .Append("    A.DELFLG = '0' ")
                    .Append("ORDER BY ")
                    .Append("    A.SORTNO ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 003.競合メーカーマスタ取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNoCompetitionMakermaster() As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionMakermasterDataTable)("SC3080203_003")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_003 */ ")
                    .Append("    A.COMPETITIONMAKERNO, ")
                    .Append("    A.COMPETITIONMAKER ")
                    .Append("FROM ")
                    .Append("    TBL_COMPETITION_MAKERMASTER A ")
                    .Append("WHERE ")
                    .Append("    NOT EXISTS ")
                    .Append("        ( ")
                    .Append("        SELECT ")
                    .Append("            1 ")
                    .Append("        FROM ")
                    .Append("            TBL_COMPETITORMASTER B ")
                    .Append("        WHERE ")
                    .Append("            A.COMPETITIONMAKERNO = B.COMPETITIONMAKERNO AND ")
                    .Append("            B.DELFLG = '0' ")
                    .Append("        ) AND ")
                    .Append("    A.DELFLG = '0' ")
                    .Append("ORDER BY ")
                    .Append("    A.SORTNO ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 004.競合車種マスタ取得
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetitorMaster() As SC3080203DataSet.SC3080203CompetitorMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitorMasterDataTable)("SC3080203_004")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_004 */ ")
                    .Append("    B.COMPETITIONMAKERNO, ")
                    .Append("    A.COMPETITIONMAKER, ")
                    .Append("    B.COMPETITORCD, ")
                    .Append("    B.COMPETITORNM ")
                    .Append("FROM ")
                    .Append("    TBL_COMPETITION_MAKERMASTER A, ")
                    .Append("    TBL_COMPETITORMASTER B ")
                    .Append("WHERE ")
                    .Append("    A.COMPETITIONMAKERNO = B.COMPETITIONMAKERNO AND ")
                    .Append("    A.DELFLG = '0' AND ")
                    .Append("    B.DELFLG = '0' ")
                    .Append("ORDER BY ")
                    .Append("    A.SORTNO, ")
                    .Append("    B.SORTNO ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 005.関連情報取得
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRelatedInfo(ByVal vin As String) As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_005")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_005 */ ")
                    .Append("    SUM(CNT) AS SEQ ")
                    .Append("FROM ")
                    .Append("    ( ")
                    .Append("    SELECT ")
                    .Append("        COUNT(1) AS CNT ")
                    .Append("    FROM ")
                    .Append("        TBL_INSURANCE ")
                    .Append("    WHERE ")
                    .Append("        VIN = :VIN AND ")
                    .Append("        ROWNUM = 1 ")
                    .Append("    UNION ALL ")
                    .Append("    SELECT ")
                    .Append("        COUNT(1) AS CNT ")
                    .Append("    FROM ")
                    .Append("        TBL_FINANCE ")
                    .Append("    WHERE ")
                    .Append("        VIN = :VIN AND ")
                    .Append("        ROWNUM = 1 ")
                    .Append("    ) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 006.Follow-up Box追加　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="cractivedate"></param>
        ''' <param name="appointtimeflg"></param>
        ''' <param name="cractlimitdate"></param>
        ''' <param name="untradedcstid"></param>
        ''' <param name="vclseqno"></param>
        ''' <param name="cractresult"></param>
        ''' <param name="branchplan"></param>
        ''' <param name="accountplan"></param>
        ''' <param name="insdid"></param>
        ''' <param name="vin"></param>
        ''' <param name="relatedinfoflg"></param>
        ''' <param name="nextcractivedate"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="createcractresult"></param>
        ''' <param name="prospectdate"></param>
        ''' <param name="hotdate"></param>
        ''' <param name="wicid"></param>
        ''' <param name="cractstatus"></param>
        ''' <param name="cractstatus1st"></param>
        ''' <param name="branchplan1st"></param>
        ''' <param name="accountplan1st"></param>
        ''' <param name="cractivedate1st"></param>
        ''' <param name="cractlimitdate1st"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="createdby"></param>
        ''' <param name="originalid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupbox(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                        ByVal cractivedate As Date, ByVal appointtimeflg As String, ByVal cractlimitdate As Date,
                                        ByVal untradedcstid As String, ByVal vclseqno As String, ByVal cractresult As String,
                                        ByVal branchplan As String, ByVal accountplan As String, ByVal insdid As String,
                                        ByVal vin As String, ByVal relatedinfoflg As String, ByVal nextcractivedate As Nullable(Of Date),
                                        ByVal updateaccount As String, ByVal createcractresult As String, ByVal prospectdate As Nullable(Of Date),
                                        ByVal hotdate As Nullable(Of Date), ByVal wicid As Long,
                                        ByVal cractstatus As String, ByVal cractstatus1ST As String, ByVal branchplan1ST As String,
                                        ByVal accountplan1ST As String, ByVal cractivedate1ST As Date, ByVal cractlimitdate1ST As Date,
                                        ByVal crcustid As String, ByVal createdby As String, ByVal originalid As String,
                                        ByVal servicestaffcd As String, ByVal servicestaffnm As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_006 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOX ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    PLANDVS, ")
                .Append("    CRACTIVEDATE, ")
                .Append("    APPOINTTIMEFLG, ")
                .Append("    SUBCTGCODE, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    CRACTCATEGORY, ")
                .Append("    CRACTCHARGDVS, ")
                .Append("    CRACTLIMITDATE, ")
                .Append("    PROMOTION_ID, ")
                .Append("    PROMOTION_DVS, ")
                .Append("    INSURANCEFLG, ")
                .Append("    FINANCEFLG, ")
                .Append("    REQCATEGORY, ")
                .Append("    REQUESTID, ")
                .Append("    UNTRADEDCSTID, ")
                .Append("    VCLSEQNO, ")
                .Append("    WALKINPURPOSE, ")
                .Append("    CRACTRESULT, ")
                .Append("    BRANCH_PLAN, ")
                .Append("    ACCOUNT_PLAN, ")
                .Append("    ACTUALFLG, ")
                .Append("    INSDID, ")
                .Append("    NAME, ")
                .Append("    VIN, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    MODELCODE, ")
                .Append("    VCLREGNO, ")
                .Append("    CRDATE_DM_D, ")
                .Append("    CRDATE_RMM_D, ")
                .Append("    CRDATE_1STCALL, ")
                .Append("    LAST_TELDATE, ")
                .Append("    LAST_TELACCOUNT, ")
                .Append("    LAST_SERVICEINDATE, ")
                .Append("    CRDATE, ")
                .Append("    MEMKIND, ")
                .Append("    CUSTSEGMENT, ")
                .Append("    ADDRESS, ")
                .Append("    ZIPCODE, ")
                .Append("    TEL, ")
                .Append("    FAXNO, ")
                .Append("    MOBILE, ")
                .Append("    BUSINESSTELNO, ")
                .Append("    CUSTID, ")
                .Append("    DATADVS, ")
                .Append("    RMBRANCH, ")
                .Append("    SALESSTAFFCD, ")
                .Append("    SALESSTAFFNM, ")
                .Append("    SERVICESTAFFCD, ")
                .Append("    SERVICESTAFFNM, ")
                .Append("    CUSTCHRGSTRCD, ")
                .Append("    CUSTCHRGSTRNM, ")
                .Append("    CUSTCHRGSTAFFCD, ")
                .Append("    CUSTCHRGSTAFFNM, ")
                .Append("    BIRTHDAY, ")
                .Append("    SEX, ")
                .Append("    RELATEDINFOFLG, ")
                .Append("    POLICYNO, ")
                .Append("    SUBNO, ")
                .Append("    LAST_SUBCTGCODE, ")
                .Append("    LAST_SERVICECODE, ")
                .Append("    LAST_SERVICENAME, ")
                .Append("    LAST_OPERATOR, ")
                .Append("    LAST_SERIESCODE, ")
                .Append("    LAST_SERIESNAME, ")
                .Append("    LAST_REGNO, ")
                .Append("    LAST_ACTIVITYRESULT, ")
                .Append("    LAST_CRDVSID, ")
                .Append("    LAST_CRACTIVEDATE, ")
                .Append("    NEXTCRACTIVEDATE, ")
                .Append("    EXESTAFFCODE, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    BEFORE_CRACTRESULT, ")
                .Append("    CR_SATRT_DATE, ")
                .Append("    PARENT_FLLWUPBOX_SEQNO, ")
                .Append("    CRACTRESULT_UPDATEDATE, ")
                .Append("    RETRY_DATE, ")
                .Append("    SALESBKGNO, ")
                .Append("    SALESBKGDATE, ")
                .Append("    SALESBKGNO_INPUT_ACCOUNT, ")
                .Append("    CREATE_CRACTRESULT, ")
                .Append("    PROSPECT_DATE, ")
                .Append("    HOT_DATE, ")
                .Append("    WICID, ")
                .Append("    TEAMCD, ")
                .Append("    REFERRAL_FLG, ")
                .Append("    INSURANCESTAFFFLG, ")
                .Append("    CRACTSTATUS, ")
                .Append("    CRACTSTATUS_1ST, ")
                .Append("    BRANCH_PLAN_1ST, ")
                .Append("    ACCOUNT_PLAN_1ST, ")
                .Append("    CRACTIVEDATE_1ST, ")
                .Append("    CRACTLIMITDATE_1ST, ")
                .Append("    DIRECT_BILLING, ")
                .Append("    WICID_2ND, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    EMPLOYEENAME, ")
                .Append("    EMPLOYEEDEPARTMENT, ")
                .Append("    EMPLOYEEPOSITION, ")
                .Append("    CREATEDBY ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :DLRCD, ")
                .Append("            :STRCD, ")
                .Append("            :FLLWUPBOX_SEQNO, ")
                .Append("            NULL AS CRPLAN_ID, ")
                .Append("            ' ' AS BFAFDVS, ")
                .Append("            4 AS CRDVSID, ")
                .Append("            '0' AS PLANDVS, ")
                .Append("            :CRACTIVEDATE, ")
                .Append("            :APPOINTTIMEFLG, ")
                .Append("            ' ' AS SUBCTGCODE, ")
                .Append("            ' ' AS SERVICECD, ")
                .Append("            ' ' AS SUBCTGORGNAME, ")
                .Append("            ' ' AS SUBCTGORGNAME_EX, ")
                .Append("            '0' AS CRACTCATEGORY, ")
                .Append("            '2' AS CRACTCHARGDVS, ")
                .Append("            :CRACTLIMITDATE, ")
                .Append("            NULL AS PROMOTION_ID, ")
                .Append("            ' ' AS PROMOTION_DVS, ")
                .Append("            '0' AS INSURANCEFLG, ")
                .Append("            '0' AS FINANCEFLG, ")
                .Append("            '1' AS REQCATEGORY, ")
                .Append("            ' ' AS REQUESTID, ")
                .Append("            :UNTRADEDCSTID, ")
                .Append("            :VCLSEQNO, ")
                .Append("            '1' AS WALKINPURPOSE, ")
                .Append("            :CRACTRESULT, ")
                .Append("            :BRANCH_PLAN, ")
                .Append("            :ACCOUNT_PLAN, ")
                .Append("            '0' AS ACTUALFLG, ")
                .Append("            :INSDID AS INSDID, ")
                .Append("            A.NAME, ")
                .Append("            :VIN, ")
                .Append("            B.SERIESCD AS SERIESCODE, ")
                .Append("            B.SERIESNM AS SERIESNAME, ")
                .Append("            B.MODELCD AS MODELCODE, ")
                .Append("            B.VCLREGNO AS VCLREGNO, ")
                .Append("            NULL AS CRDATE_DM_D, ")
                .Append("            NULL AS CRDATE_RMM_D, ")
                .Append("            NULL AS CRDATE_1STCALL, ")
                .Append("            NULL AS LAST_TELDATE, ")
                .Append("            ' ' AS LAST_TELACCOUNT, ")
                .Append("            NULL AS LAST_SERVICEINDATE, ")
                .Append("            NULL AS CRDATE, ")
                .Append("            '1' AS MEMKIND, ")
                .Append("            '1' AS CUSTSEGMENT, ")
                .Append("            A.ADDRESS, ")
                .Append("            A.ZIPCODE, ")
                .Append("            A.TELNO, ")
                .Append("            A.FAXNO, ")
                .Append("            A.MOBILE, ")
                .Append("            A.BUSINESSTELNO, ")
                .Append("            A.CUSTCD, ")
                .Append("            '0' AS DATADVS, ")
                .Append("            C.STRCD AS RMBRANCH, ")
                .Append("            NVL(D.SALESSTAFFCD,' '), ")
                .Append("            NVL(D.SALESSTAFFNM,' '), ")
                .Append("            :SERVICESTAFFCD, ")
                .Append("            :SERVICESTAFFNM, ")
                .Append("            A.strcdstaff AS CUSTCHRGSTRCD, ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                strnm_local ")
                .Append("            FROM ")
                .Append("                TBLM_BRANCH ")
                .Append("            WHERE ")
                .Append("                DLRCD = A.DLRCD AND ")
                .Append("                STRCD = A.strcdstaff ")
                .Append("            ) AS CUSTCHRGSTRNM, ")
                .Append("            A.staffcd AS CUSTCHRGSTAFFCD, ")
                .Append("            NVL(( ")
                .Append("            SELECT ")
                .Append("                USERNAME ")
                .Append("            FROM ")
                .Append("                TBL_USERS ")
                .Append("            WHERE ")
                .Append("                ACCOUNT = A.staffcd AND ")
                .Append("                DELFLG = '0' ")
                .Append("            ),' ') AS CUSTCHRGSTAFFNM, ")
                .Append("            A.BIRTHDAY, ")
                .Append("            A.SEX, ")
                .Append("            :RELATEDINFOFLG, ")
                .Append("            ' ' AS POLICYNO, ")
                .Append("            ' ' AS SUBNO, ")
                .Append("            ' ' AS LAST_SUBCTGCODE, ")
                .Append("            ' ' AS LAST_SERVICECODE, ")
                .Append("            ' ' AS LAST_SERVICENAME, ")
                .Append("            ' ' AS LAST_OPERATOR, ")
                .Append("            ' ' AS LAST_SERIESCODE, ")
                .Append("            ' ' AS LAST_SERIESNAME, ")
                .Append("            ' ' AS LAST_REGNO, ")
                .Append("            NULL AS LAST_ACTIVITYRESULT, ")
                .Append("            NULL AS LAST_CRDVSID, ")
                .Append("            NULL AS LAST_CRACTIVEDATE, ")
                .Append("            :NEXTCRACTIVEDATE, ")
                .Append("            '8' AS EXESTAFFCODE, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            :UPDATEACCOUNT, ")
                .Append("            ' ' AS BEFORE_CRACTRESULT, ")
                .Append("            SYSDATE AS CR_SATRT_DATE, ")
                .Append("            NULL AS PARENT_FLLWUPBOX_SEQNO, ")
                .Append("            SYSDATE AS CRACTRESULT_UPDATEDATE, ")
                .Append("            NULL AS RETRY_DATE, ")
                .Append("            ' ' AS SALESBKGNO, ")
                .Append("            NULL AS SALESBKGDATE, ")
                .Append("            ' ' AS SALESBKGNO_INPUT_ACCOUNT, ")
                .Append("            :CREATE_CRACTRESULT, ")
                .Append("            :PROSPECT_DATE, ")
                .Append("            :HOT_DATE, ")
                .Append("            :WICID, ")
                .Append("            ' ' AS TEAMCD, ")
                .Append("            '0' AS REFERRAL_FLG, ")
                .Append("            '0' AS INSURANCESTAFFFLG, ")
                .Append("            :CRACTSTATUS, ")
                .Append("            :CRACTSTATUS_1ST, ")
                .Append("            :BRANCH_PLAN_1ST, ")
                .Append("            :ACCOUNT_PLAN_1ST, ")
                .Append("            :CRACTIVEDATE_1ST, ")
                .Append("            :CRACTLIMITDATE_1ST, ")
                .Append("            '0' AS DIRECT_BILLING, ")
                .Append("            NULL AS WICID_2ND, ")
                .Append("            :CRCUSTID, ")
                .Append("            '1' AS CUSTOMERCLASS, ")
                .Append("            A.EMPLOYEENAME, ")
                .Append("            A.EMPLOYEEDEPARTMENT, ")
                .Append("            A.EMPLOYEEPOSITION, ")
                .Append("            :CREATEDBY ")
                .Append("        FROM ")
                .Append("            TBLORG_CUSTOMER A, ")
                .Append("            TBLORG_VCLINFO B, ")
                .Append("            TBLORG_BRANCHINFO C, ")
                .Append("            TBLORG_SALESBKG D ")
                .Append("        WHERE ")
                .Append("            A.ORIGINALID = :ORIGINALID AND ")
                .Append("            B.ORIGINALID = A.ORIGINALID AND ")
                .Append("            B.VIN = :VIN AND ")
                .Append("            B.DELFLG = '0' AND ")
                .Append("            C.DLRCD = B.DLRCD AND ")
                .Append("            C.ORIGINALID = B.ORIGINALID AND ")
                .Append("            C.VIN = B.VIN AND ")
                .Append("            C.RMFLG = '1' AND D.DLRCD(+) = B.DLRCD AND ")
                .Append("            D.SALESBKGNO(+) = B.SALESBKGNO ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("CRACTIVEDATE", OracleDbType.Date, cractivedate)
                query.AddParameterWithTypeValue("APPOINTTIMEFLG", OracleDbType.Char, appointtimeflg)
                query.AddParameterWithTypeValue("CRACTLIMITDATE", OracleDbType.Date, cractlimitdate)
                query.AddParameterWithTypeValue("UNTRADEDCSTID", OracleDbType.Char, untradedcstid)
                query.AddParameterWithTypeValue("VCLSEQNO", OracleDbType.Char, vclseqno)
                query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchplan)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, accountplan)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("RELATEDINFOFLG", OracleDbType.Char, relatedinfoflg)
                query.AddParameterWithTypeValue("NEXTCRACTIVEDATE", OracleDbType.Date, nextcractivedate)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("CREATE_CRACTRESULT", OracleDbType.Char, createcractresult)
                query.AddParameterWithTypeValue("PROSPECT_DATE", OracleDbType.Date, prospectdate)
                query.AddParameterWithTypeValue("HOT_DATE", OracleDbType.Date, hotdate)
                query.AddParameterWithTypeValue("WICID", OracleDbType.Int64, wicid)
                query.AddParameterWithTypeValue("CRACTSTATUS", OracleDbType.Char, cractstatus)
                query.AddParameterWithTypeValue("CRACTSTATUS_1ST", OracleDbType.Char, cractstatus1ST)
                query.AddParameterWithTypeValue("BRANCH_PLAN_1ST", OracleDbType.Char, branchplan1ST)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN_1ST", OracleDbType.Char, accountplan1ST)
                query.AddParameterWithTypeValue("CRACTIVEDATE_1ST", OracleDbType.Date, cractivedate1ST)
                query.AddParameterWithTypeValue("CRACTLIMITDATE_1ST", OracleDbType.Date, cractlimitdate1ST)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("CREATEDBY", OracleDbType.Char, createdby)
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("SERVICESTAFFCD", OracleDbType.Char, servicestaffcd)
                query.AddParameterWithTypeValue("SERVICESTAFFNM", OracleDbType.Char, servicestaffnm)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 007.Follow-up Box更新 (移行済み)
        ''' </summary>
        ''' <param name="thistimecractresult"></param>
        ''' <param name="strselecteddvs"></param>
        ''' <param name="cractlimitdate"></param>
        ''' <param name="nextactivitydatetime"></param>
        ''' <param name="crdvsid"></param>
        ''' <param name="account"></param>
        ''' <param name="thistimecractstatus"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateFllwupbox(ByVal thistimecractresult As String, ByVal strselecteddvs As String, ByVal cractlimitdate As String,
                                        ByVal nextactivitydatetime As String, ByVal crdvsid As Long, ByVal account As String,
                                        ByVal thistimecractstatus As String, ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                        ByVal fllwuptyp As String, ByVal appointtimeflg As String) As Integer

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3080203_007 */ ")
                .Append(" TBL_FLLWUPBOX SET ")
                .Append(" ACTUALFLG='1' ")
                .Append(",BEFORE_CRACTRESULT = CRACTRESULT ")
                .Append(",CRACTRESULT_UPDATEDATE = SYSDATE ")
                .Append(",UPDATEDATE = SYSDATE ")
                .Append(",UPDATEACCOUNT = :ACCOUNT ")
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTSUCCESS, CONSTCRACTRSLTGIVEUP
                        .Append(",CRACTRESULT = :THISTIME_CRACTRESULT ")
                    Case CONSTCRACTRSLTCONTINUE
                        Select Case fllwuptyp
                            Case CONSTFLLWUPHOT, CONSTFLLWUPPROSPECT
                                If strselecteddvs = CONSTSELECTTOWALK Then
                                    .Append(",CRACTRESULT = '4' ")
                                End If
                            Case Else
                                .Append(",CRACTRESULT = :THISTIME_CRACTRESULT ")
                        End Select
                        .Append(",CRDVSID = :CRDVSID + 1 ")
                        .Append(",CRACTIVEDATE = TO_DATE(:nextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            .Append(",CRACTLIMITDATE = TO_DATE(:nextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                .Append(",CRACTLIMITDATE = TO_DATE(:nextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                            End If
                        End If
                        If CLng(crdvsid) = 4 Then
                            .Append(",CRDATE_1STCALL = SYSDATE ")
                        End If
                        .Append(",APPOINTTIMEFLG = :APPOINTTIMEFLG ")
                        If strselecteddvs = CONSTSELECTTOWALK Then
                            .Append(",HOT_DATE = NULL")
                            .Append(",PROSPECT_DATE = NULL ")
                        End If
                    Case CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                        .Append(",CRACTRESULT = :THISTIME_CRACTRESULT ")
                        .Append(",CRDVSID = (:CRDVSID + 1) ")
                        .Append(",CRACTIVEDATE = TO_DATE(:NEXTACTIVITYDATETIME,'YYYY/MM/DD HH24:MI:SS') ")
                        .Append(",CRACTLIMITDATE = TO_DATE(:NEXTACTIVITYDATETIME,'YYYY/MM/DD HH24:MI:SS') ")
                        If CLng(crdvsid) = 4 Then
                            .Append(",CRDATE_1STCALL = SYSDATE ")
                        End If
                        .Append(",APPOINTTIMEFLG = :APPOINTTIMEFLG ")
                        Select Case thistimecractresult
                            Case CONSTCRACTRSLTHOT
                                .Append(",HOT_DATE = SYSDATE ")
                            Case CONSTCRACTRSLTPROSPECT
                                .Append(",PROSPECT_DATE = SYSDATE ")
                        End Select
                End Select
                .Append(",CRACTSTATUS = :THISTIME_CRACTSTATUS ")
                .Append(" WHERE DLRCD = :DLRCD ")
                .Append(" AND STRCD = :STRCD ")
                .Append(" AND FLLWUPBOX_SEQNO = TO_NUMBER(:FLLWUPBOX_SEQNO) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_007")
                'query.CommandText = sql.ToString()
                'query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                'Select Case thistimecractresult
                '    Case CONSTCRACTRSLTSUCCESS, CONSTCRACTRSLTGIVEUP
                '        query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                '    Case CONSTCRACTRSLTCONTINUE
                '        Select Case fllwuptyp
                '            Case CONSTFLLWUPHOT, CONSTFLLWUPPROSPECT
                '                If strselecteddvs = CONSTSELECTTOWALK Then
                '                End If
                '            Case Else
                '                query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                '        End Select
                '        query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Int64, crdvsid)
                '        query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                '        If String.IsNullOrEmpty(cractlimitdate) = False Then
                '            query.AddParameterWithTypeValue("nextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                '        Else
                '            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                '                query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                '            End If
                '        End If
                '    Case CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                '        query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                '        query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Char, crdvsid)
                '        query.AddParameterWithTypeValue("NEXTACTIVITYDATETIME", OracleDbType.Char, nextactivitydatetime)
                'End Select
                'query.AddParameterWithTypeValue("THISTIME_CRACTSTATUS", OracleDbType.Char, thistimecractstatus)
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                'query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Char, fllwupboxseqno)

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTSUCCESS, CONSTCRACTRSLTGIVEUP
                        query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                    Case CONSTCRACTRSLTCONTINUE
                        Select Case fllwuptyp
                            Case CONSTFLLWUPHOT, CONSTFLLWUPPROSPECT
                            Case Else
                                query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                        End Select
                        query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Char, crdvsid)
                        query.AddParameterWithTypeValue("nextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                        query.AddParameterWithTypeValue("APPOINTTIMEFLG", OracleDbType.Char, appointtimeflg)
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            query.AddParameterWithTypeValue("nextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                query.AddParameterWithTypeValue("nextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                            End If
                        End If
                    Case CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                        query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                        query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Char, crdvsid)
                        query.AddParameterWithTypeValue("NEXTACTIVITYDATETIME", OracleDbType.Char, nextactivitydatetime)
                        query.AddParameterWithTypeValue("APPOINTTIMEFLG", OracleDbType.Char, appointtimeflg)
                End Select
                query.AddParameterWithTypeValue("THISTIME_CRACTSTATUS", OracleDbType.Char, thistimecractstatus)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Char, fllwupboxseqno)

                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 008.Follow-up Box成約車種追加 (移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxSuccessSeries(ByVal seqno As Integer, ByVal updateaccount As String, ByVal dlrcd As String,
                                                            ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal seq As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_008")
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* SC3080203_008 */ ")
                    .Append("INTO ")
                    .Append("    TBL_FLLWUPBOX_SUCCESS_SERIES ")
                    .Append("( ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    FLLWUPBOX_SEQNO, ")
                    .Append("    SELECT_SERIES_SEQNO, ")
                    .Append("    SEQNO, ")
                    .Append("    SERIESCD, ")
                    .Append("    MODELCD, ")
                    .Append("    COLORCD, ")
                    .Append("    SALESBKGNO, ")
                    .Append("    SALESBKGDATE, ")
                    .Append("    SALESBKGNO_INPUT_ACCOUNT, ")
                    .Append("    CREATEDATE, ")
                    .Append("    UPDATEDATE, ")
                    .Append("    UPDATEACCOUNT, ")
                    .Append("    SUFFIX_CD, ")
                    .Append("    INTERCLR_CD ")
                    .Append(") ")
                    .Append("        ( ")
                    .Append("        SELECT ")
                    .Append("            DLRCD, ")
                    .Append("            STRCD, ")
                    .Append("            FLLWUPBOX_SEQNO, ")
                    .Append("            SEQNO AS SELECT_SERIES_SEQNO, ")
                    .Append("            :SEQNO, ")
                    .Append("            SERIESCD, ")
                    .Append("            MODELCD, ")
                    .Append("            COLORCD, ")
                    .Append("            ' ' AS SALESBKGNO, ")
                    .Append("            NULL AS SALESBKGDATE, ")
                    .Append("            ' ' AS SALESBKGNO_INPUT_ACCOUNT, ")
                    .Append("            SYSDATE AS CREATEDATE, ")
                    .Append("            SYSDATE AS UPDATEDATE, ")
                    .Append("            :UPDATEACCOUNT, ")
                    .Append("            ' ' AS SUFFIX_CD, ")
                    .Append("            ' ' AS INTERCLR_CD ")
                    .Append("        FROM ")
                    .Append("            TBL_FLLWUPBOX_SELECTED_SERIES A ")
                    .Append("        WHERE ")
                    .Append("            DLRCD = :DLRCD AND ")
                    .Append("            STRCD = :STRCD AND ")
                    .Append("            FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("            SEQNO = TO_NUMBER(:SEQ) ")
                    .Append("        ) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQ", OracleDbType.Char, seq)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 009.Follow-up Box活動履歴追加　(移行済み)
        ''' </summary>
        ''' <param name="calldate"></param>
        ''' <param name="account"></param>
        ''' <param name="crdvs"></param>
        ''' <param name="actualtimeend"></param>
        ''' <param name="actdate"></param>
        ''' <param name="method"></param>
        ''' <param name="action"></param>
        ''' <param name="actiontype"></param>
        ''' <param name="brnchaccount"></param>
        ''' <param name="actioncd"></param>
        ''' <param name="ctntseqno"></param>
        ''' <param name="selectseriesseqno"></param>
        ''' <param name="seriesnm"></param>
        ''' <param name="vclmodelname"></param>
        ''' <param name="dispbdycolor"></param>
        ''' <param name="quantity"></param>
        ''' <param name="fllwupboxrsltseqno"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxCRHis(ByVal calldate As Nullable(Of Date), ByVal account As String,
                                             ByVal crdvs As Nullable(Of Long), ByVal actualtimeend As Nullable(Of Date), ByVal actdate As Date,
                                             ByVal method As String, ByVal action As String, ByVal actiontype As String,
                                             ByVal brnchaccount As String, ByVal actioncd As String, ByVal ctntseqno As Nullable(Of Long),
                                             ByVal selectseriesseqno As Nullable(Of Long), ByVal seriesnm As String, ByVal vclmodelname As String,
                                             ByVal dispbdycolor As String, ByVal quantity As Nullable(Of Long), ByVal fllwupboxrsltseqno As Long,
                                             ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_009 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXCRHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    IDENTITYNO, ")
                .Append("    SEQNO, ")
                .Append("    INSDID, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    CALLDATE, ")
                .Append("    ACCOUNT, ")
                .Append("    REGNO, ")
                .Append("    SUBCTGCODE, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    PROMOTION_ID, ")
                .Append("    CRDVS, ")
                .Append("    ACTIVITYRESULT, ")
                .Append("    PLANDVS, ")
                .Append("    ACTUALTIME_END, ")
                .Append("    ACTDATE, ")
                .Append("    METHOD, ")
                .Append("    ACTION, ")
                .Append("    ACTIONTYPE, ")
                .Append("    HOACCOUNT, ")
                .Append("    BRNCHACCOUNT, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    ACTIONCD, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    CTNTSEQNO, ")
                .Append("    SELECT_SERIES_SEQNO, ")
                .Append("    SERIESNM, ")
                .Append("    VCLMODEL_NAME, ")
                .Append("    DISP_BDY_COLOR, ")
                .Append("    QUANTITY, ")
                .Append("    FLLWUPBOXRSLT_SEQNO, ")
                .Append("    SUFFIX_NM, ")
                .Append("    SUFFIX_CD, ")
                .Append("    INTERCLR_NAME ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            DLRCD, ")
                .Append("            STRCD, ")
                .Append("            FLLWUPBOX_SEQNO, ")
                .Append("            CRPLAN_ID, ")
                .Append("            BFAFDVS, ")
                .Append("            CRDVSID, ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                NVL(MAX(IDENTITYNO),'0') + 1 ")
                .Append("            FROM ")
                .Append("                TBL_FLLWUPBOXCRHIS ")
                .Append("            WHERE ")
                .Append("                DLRCD = :DLRCD AND ")
                .Append("                STRCD = :STRCD AND ")
                .Append("                FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("            ) AS IDENTITYNO, ")
                .Append("            1 AS SEQNO, ")
                .Append("            INSDID, ")
                .Append("            SERIESCODE, ")
                .Append("            SERIESNAME, ")
                .Append("            :CALLDATE, ")
                .Append("            :ACCOUNT, ")
                .Append("            VCLREGNO, ")
                .Append("            SUBCTGCODE, ")
                .Append("            SERVICECD, ")
                .Append("            SUBCTGORGNAME, ")
                .Append("            SUBCTGORGNAME_EX, ")
                .Append("            PROMOTION_ID, ")
                .Append("            :CRDVS, ")
                .Append("            CRACTRESULT AS ACTIVITYRESULT, ")
                .Append("            PLANDVS, ")
                .Append("            :ACTUALTIME_END, ")
                .Append("            :ACTDATE, ")
                .Append("            :METHOD, ")
                .Append("            :ACTION, ")
                .Append("            :ACTIONTYPE, ")
                .Append("            NULL AS HOACCOUNT, ")
                .Append("            :BRNCHACCOUNT, ")
                .Append("            NULL AS STALL_REZID, ")
                .Append("            NULL AS STALL_DLRCD, ")
                .Append("            NULL AS STALL_STRCD, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            :ACTIONCD, ")
                .Append("            NULL AS REC_ID, ")
                .Append("            NULL AS CMS_HISLINKID, ")
                .Append("            :CTNTSEQNO, ")
                .Append("            :SELECT_SERIES_SEQNO, ")
                .Append("            :SERIESNM, ")
                .Append("            :VCLMODEL_NAME, ")
                .Append("            :DISP_BDY_COLOR, ")
                .Append("            :QUANTITY, ")
                .Append("            :FLLWUPBOXRSLT_SEQNO, ")
                .Append("            NULL AS SUFFIX_NM, ")
                .Append("            NULL AS SUFFIX_CD, ")
                .Append("            NULL AS INTERCLR_NAME ")
                .Append("        FROM ")
                .Append("            TBL_FLLWUPBOX ")
                .Append("        WHERE ")
                .Append("            DLRCD = :DLRCD AND ")
                .Append("            STRCD =:STRCD AND ")
                .Append("            FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_009")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CALLDATE", OracleDbType.Date, calldate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("CRDVS", OracleDbType.Int64, crdvs)
                query.AddParameterWithTypeValue("ACTUALTIME_END", OracleDbType.Date, actualtimeend)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Date, actdate)
                query.AddParameterWithTypeValue("METHOD", OracleDbType.Char, method)
                query.AddParameterWithTypeValue("ACTION", OracleDbType.Char, action)
                query.AddParameterWithTypeValue("ACTIONTYPE", OracleDbType.Char, actiontype)
                query.AddParameterWithTypeValue("BRNCHACCOUNT", OracleDbType.Char, brnchaccount)
                query.AddParameterWithTypeValue("ACTIONCD", OracleDbType.Char, actioncd)
                query.AddParameterWithTypeValue("CTNTSEQNO", OracleDbType.Int64, ctntseqno)
                query.AddParameterWithTypeValue("SELECT_SERIES_SEQNO", OracleDbType.Int64, selectseriesseqno)
                query.AddParameterWithTypeValue("SERIESNM", OracleDbType.Char, seriesnm)
                query.AddParameterWithTypeValue("VCLMODEL_NAME", OracleDbType.Char, vclmodelname)
                query.AddParameterWithTypeValue("DISP_BDY_COLOR", OracleDbType.Char, dispbdycolor)
                query.AddParameterWithTypeValue("QUANTITY", OracleDbType.Int64, quantity)
                query.AddParameterWithTypeValue("FLLWUPBOXRSLT_SEQNO", OracleDbType.Int64, fllwupboxrsltseqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 010.Follow-up Box詳細追加　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="lastserviceindate"></param>
        ''' <param name="lastsubctgcode"></param>
        ''' <param name="lastservicecd"></param>
        ''' <param name="lastservicename"></param>
        ''' <param name="lastmileage"></param>
        ''' <param name="lastserviceinbranch"></param>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InserFllwupboxDetail(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                             ByVal lastserviceindate As String, ByVal lastsubctgcode As String, ByVal lastservicecd As String,
                                             ByVal lastservicename As String, ByVal lastmileage As String, ByVal lastserviceinbranch As String,
                                             ByVal originalid As String, ByVal vin As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_010 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXDETAIL ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    INSDID, ")
                .Append("    VIN, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    REGNO, ")
                .Append("    DELIDATE, ")
                .Append("    MODELCODE, ")
                .Append("    ENGINEPFX, ")
                .Append("    ENGINENO, ")
                .Append("    ACTIVITYCATEGORY, ")
                .Append("    SEVEREFLG, ")
                .Append("    PAYOFFDATE, ")
                .Append("    MEMREGSTATUS, ")
                .Append("    GBOOK_ID, ")
                .Append("    OWNCRD_ID, ")
                .Append("    LASTSERVICEINDATE, ")
                .Append("    LAST_SUBCTGCODE, ")
                .Append("    LAST_SERVICECD, ")
                .Append("    LAST_SERVICENAME, ")
                .Append("    LASTMILEAGE, ")
                .Append("    LASTSERVICEINBRANCH, ")
                .Append("    RMBRANCH, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE ")
                .Append(") ")
                .Append("SELECT ")
                .Append("    DISTINCT :DLRCD AS DLRCD, ")
                .Append("    :STRCD AS STRCD, ")
                .Append("    :FLLWUPBOX_SEQNO AS FLLWUPBOX_SEQNO, ")
                .Append("    NULL AS CRPLAN_ID, ")
                .Append("    ' ' AS BFAFDVS, ")
                .Append("    '4' AS CRDVSID, ")
                .Append("    A.ORIGINALID, ")
                .Append("    B.VIN, ")
                .Append("    B.SERIESCD, ")
                .Append("    B.SERIESNM, ")
                .Append("    B.VCLREGNO, ")
                .Append("    B.VCLDELIDATE, ")
                .Append("    B.MODELCD, ")
                .Append("    B.ENGINETYPE, ")
                .Append("    B.ENGINENO, ")
                .Append("    NVL(C.ACTVCTGRYID,'1'), ")
                .Append("    NVL(C.SEVEREFLG,'0'), ")
                .Append("    D.PAYOFFDATESC, ")
                .Append("    NVL(E.MEMREGSTATUS,' '), ")
                .Append("    NVL(G.GBOOKID,' ') AS GBOOK_ID, ")
                .Append("    B.MEMSYSTEMID, ")
                .Append("    TO_DATE(:LASTSERVICEINDATE,'YYYY/MM/DD HH24:MI:SS'), ")
                .Append("    :LAST_SUBCTGCODE, ")
                .Append("    :LAST_SERVICECD, ")
                .Append("    :LAST_SERVICENAME, ")
                .Append("    :LASTMILEAGE, ")
                .Append("    :LASTSERVICEINBRANCH, ")
                .Append("    A.STRCD, ")
                .Append("    SYSDATE AS CREATEDATE, ")
                .Append("    SYSDATE AS UPDATEDATE ")
                .Append("FROM ")
                .Append("    TBLORG_CUSTOMER A, ")
                .Append("    TBLORG_VCLINFO B, ")
                .Append("    TBLORG_DLRVIN C, ")
                .Append("    TBL_FINANCE D, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        ORIGINALID, ")
                .Append("        VIN, ")
                .Append("        DLRCD, ")
                .Append("        MEMREGSTATUS ")
                .Append("    FROM ")
                .Append("        TBLORG_MEMSTATUS ")
                .Append("    WHERE ")
                .Append("        REGSEQ IN ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                MAX(REGSEQ) ")
                .Append("            FROM ")
                .Append("                TBLORG_MEMSTATUS ")
                .Append("            WHERE ")
                .Append("                ORIGINALID = :ORIGINALID AND ")
                .Append("                VIN = :VIN AND ")
                .Append("                DLRCD = :DLRCD AND ")
                .Append("                DELFLG = '0' ")
                .Append("            ) ")
                .Append("    ) E, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        VIN, ")
                .Append("        DLRCD, ")
                .Append("        STRCD, ")
                .Append("        GBOOKID ")
                .Append("    FROM ")
                .Append("        TBL_GBKCNTINF ")
                .Append("    ) G ")
                .Append("WHERE ")
                .Append("    A.ORIGINALID = :ORIGINALID AND ")
                .Append("    B.ORIGINALID=A.ORIGINALID AND ")
                .Append("    B.DLRCD = :DLRCD AND ")
                .Append("    B.VIN = :VIN AND ")
                .Append("    C.DLRCD(+) = B.DLRCD AND ")
                .Append("    C.VIN(+) = B.VIN AND ")
                .Append("    D.VIN(+) = B.VIN AND ")
                .Append("    E.ORIGINALID(+) = B.ORIGINALID AND ")
                .Append("    E.VIN(+) = B.VIN AND ")
                .Append("    E.DLRCD(+) = B.DLRCD AND ")
                .Append("    G.VIN(+)= B.VIN AND ")
                .Append("    G.DLRCD(+)=B.DLRCD AND ")
                .Append("    G.STRCD(+)=B.STRCD ")

            End With
            Using query As New DBUpdateQuery("SC3080203_010")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("LASTSERVICEINDATE", OracleDbType.Char, lastserviceindate)
                query.AddParameterWithTypeValue("LAST_SUBCTGCODE", OracleDbType.Char, lastsubctgcode)
                query.AddParameterWithTypeValue("LAST_SERVICECD", OracleDbType.Char, lastservicecd)
                query.AddParameterWithTypeValue("LAST_SERVICENAME", OracleDbType.Char, lastservicename)
                query.AddParameterWithTypeValue("LASTMILEAGE", OracleDbType.Char, lastmileage)
                query.AddParameterWithTypeValue("LASTSERVICEINBRANCH", OracleDbType.Char, lastserviceinbranch)
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 011.Follow-up Boxデータ更新情報取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupboxEntry(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203CountDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CountDataTable)("SC3080203_011")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_011 */ ")
                    .Append("    1 ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXENTRY ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    ROWNUM >= 1 ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 012.Follow-up Boxデータ更新情報追加 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="updateaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxEntry(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal updateaccount As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_012")
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* SC3080203_012 */ ")
                    .Append("INTO ")
                    .Append("    TBL_FLLWUPBOXENTRY ")
                    .Append("( ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    FLLWUPBOX_SEQNO, ")
                    .Append("    UPDATEDATE, ")
                    .Append("    UPDATEACCOUNT ")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :DLRCD, ")
                    .Append("    :STRCD, ")
                    .Append("    :FLLWUPBOX_SEQNO, ")
                    .Append("    SYSDATE, ")
                    .Append("    :UPDATEACCOUNT ")
                    .Append(") ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 013.Follow-up Boxデータ更新情報更新 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="updateaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateFllwupboxEntry(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal updateaccount As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_013")
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* SC3080203_013 */ ")
                    .Append("    TBL_FLLWUPBOXENTRY ")
                    .Append("SET ")
                    .Append("    UPDATEDATE = SYSDATE, ")
                    .Append("    UPDATEACCOUNT = :UPDATEACCOUNT ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 014.Follow-up Box未取引客情報追加 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="cstid"></param>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxNewcst(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                              ByVal updateaccount As String, ByVal cstid As String, ByVal seqno As Nullable(Of Long)) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_014 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXNEWCST ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    NEWCUST_NAME, ")
                .Append("    NEWCUST_ZIPCODE, ")
                .Append("    NEWCUST_ADDRESS, ")
                .Append("    NEWCUST_TEL, ")
                .Append("    NEWCUST_FAXNO, ")
                .Append("    NEWCUST_MOBILE, ")
                .Append("    NEWCUST_BUSINESSTELNO, ")
                .Append("    NEWCUST_BIRTHDAY, ")
                .Append("    NEWCUST_SEX, ")
                .Append("    NEWCUST_SERIESCODE, ")
                .Append("    NEWCUST_SERIESNAME, ")
                .Append("    NEWCUST_VCLREGNO, ")
                .Append("    NEWCUST_MODELCODE, ")
                .Append("    NEWCUST_VIN, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    EMPLOYEENAME, ")
                .Append("    EMPLOYEEDEPARTMENT, ")
                .Append("    EMPLOYEEPOSITION ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :DLRCD, ")
                .Append("            :STRCD, ")
                .Append("            :FLLWUPBOX_SEQNO, ")
                .Append("            A.NAME, ")
                .Append("            A.ZIPCODE, ")
                .Append("            A.ADDRESS, ")
                .Append("            A.TELNO, ")
                .Append("            A.FAXNO, ")
                .Append("            A.MOBILE, ")
                .Append("            A.BUSINESSTELNO, ")
                .Append("            A.BIRTHDAY, ")
                .Append("            A.SEX, ")
                .Append("            NVL(B.SERIESCODE,' '), ")
                .Append("            NVL(B.SERIESNAME,' '), ")
                .Append("            NVL(B.VCLREGNO,' '), ")
                .Append("            NVL(B.MODELCODE,' '), ")
                .Append("            NVL(B.VIN,' '), ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            :UPDATEACCOUNT, ")
                .Append("            A.EMPLOYEENAME, ")
                .Append("            A.EMPLOYEEDEPARTMENT, ")
                .Append("            A.EMPLOYEEPOSITION ")
                .Append("        FROM ")
                .Append("            TBL_NEWCUSTOMER A, ")
                .Append("            TBL_NEWCUSTOMERVCLRE B ")
                .Append("        WHERE ")
                .Append("            A.CSTID = :CSTID AND ")
                .Append("            B.CSTID(+) = A.CSTID AND ")
                .Append("            B.SEQNO(+) = :SEQNO ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_014")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 015.Follow-up Box結果取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupboxRslt(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwupboxRsltDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupboxRsltDataTable)("SC3080203_015")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_015 */ ")
                    .Append("    NVL(MAX(SEQNO),0)+1 AS SEQNO ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXRSLT ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 016.Follow-up Box結果追加　(移行済み)
        ''' </summary>
        ''' <param name="memkind"></param>
        ''' <param name="thistimecractresult"></param>
        ''' <param name="stractualtimestart"></param>
        ''' <param name="strcalltime"></param>
        ''' <param name="strnextactivitydatetime"></param>
        ''' <param name="insuranceflg"></param>
        ''' <param name="strmemo"></param>
        ''' <param name="strpurchasedmakername"></param>
        ''' <param name="strpurchasedmodelcd"></param>
        ''' <param name="strpurchasedmodelname"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="fllwupseq"></param>
        ''' <param name="strseqno"></param>
        ''' <param name="crplanid"></param>
        ''' <param name="bfafdvs"></param>
        ''' <param name="crdvsid"></param>
        ''' <param name="plandvs"></param>
        ''' <param name="insdid"></param>
        ''' <param name="untradedcstid"></param>
        ''' <param name="vin"></param>
        ''' <param name="strcrrsltid"></param>
        ''' <param name="account"></param>
        ''' <param name="strtalkingtime"></param>
        ''' <param name="subctgcode"></param>
        ''' <param name="promotionid"></param>
        ''' <param name="strsuccessdate"></param>
        ''' <param name="strsuccesskind"></param>
        ''' <param name="strseriescode"></param>
        ''' <param name="strseriesname"></param>
        ''' <param name="strgiveupdate"></param>
        ''' <param name="opecd"></param>
        ''' <param name="servicecd"></param>
        ''' <param name="strsubctgorgname"></param>
        ''' <param name="subctgorgnameex"></param>
        ''' <param name="strstallreserveid"></param>
        ''' <param name="strstalldlrcd"></param>
        ''' <param name="strstallstrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="strrecid"></param>
        ''' <param name="strlogicflg"></param>
        ''' <param name="strcmshislinkid"></param>
        ''' <param name="thistimecractstatus"></param>
        ''' <param name="cractstatus"></param>
        ''' <param name="strpurchasedmakerno"></param>
        ''' <param name="strcrcustid"></param>
        ''' <param name="strcustomerclass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxRslt(ByVal memkind As String, ByVal thistimecractresult As String, ByVal stractualtimestart As String,
                                                   ByVal strcalltime As String, ByVal strnextactivitydatetime As String, ByVal insuranceflg As String,
                                                   ByVal strmemo As String, ByVal strpurchasedmakername As String, ByVal strpurchasedmodelcd As String,
                                                   ByVal strpurchasedmodelname As String, ByVal dlrcd As String, ByVal fllwupseq As String,
                                                   ByVal strseqno As String, ByVal crplanid As Nullable(Of Long), ByVal bfafdvs As String,
                                                   ByVal crdvsid As String, ByVal plandvs As String, ByVal insdid As String,
                                                   ByVal untradedcstid As String, ByVal vin As String, ByVal strcrrsltid As String,
                                                   ByVal account As String, ByVal strtalkingtime As String, ByVal subctgcode As String,
                                                   ByVal promotionid As Nullable(Of Long), ByVal strsuccessdate As String,
                                                   ByVal strsuccesskind As String, ByVal strseriescode As String, ByVal strseriesname As String,
                                                   ByVal strgiveupdate As String, ByVal opecd As String, ByVal servicecd As String,
                                                   ByVal strsubctgorgname As String, ByVal subctgorgnameex As String,
                                                   ByVal strstallreserveid As Nullable(Of Long), ByVal strstalldlrcd As String,
                                                   ByVal strstallstrcd As String, ByVal fllwstrcd As String, ByVal strrecid As Nullable(Of Long),
                                                   ByVal strlogicflg As Nullable(Of Integer), ByVal strcmshislinkid As Nullable(Of Long),
                                                   ByVal thistimecractstatus As String, ByVal cractstatus As String, ByVal strpurchasedmakerno As String,
                                                   ByVal strcrcustid As String, ByVal strcustomerclass As String, ByVal fllwuptyp As String,
                                                   ByVal nextappointtimeflg As String, ByVal contactno As Long, ByVal salesstarttime As String,
                                                   ByVal salesendtime As String, ByVal stractualtimeend As String, ByVal accountplan As String,
                                                   ByVal actaccount As String, ByVal strcd As String, walkinNum As Nullable(Of Integer)) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_016 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXRSLT ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    PLANDVS, ")
                .Append("    INSDID, ")
                .Append("    MEMKIND, ")
                .Append("    VIN, ")
                .Append("    ACTVCTGRYID, ")
                .Append("    SEVEREFLG, ")
                .Append("    MILEAGE, ")
                .Append("    CRRSLTID, ")
                .Append("    ACCOUNT_ACTUAL, ")
                .Append("    NEXTACTIVEDATE, ")
                .Append("    NEXTAPPOINTTIMEFLG, ")
                .Append("    ACTUALTIME_START, ")
                .Append("    ACTUALTIME_END, ")
                .Append("    TALKINGTIME, ")
                .Append("    CALLTIME, ")
                .Append("    SUBCTGCODE, ")
                .Append("    PROMOTION_ID, ")
                .Append("    STATUS, ")
                .Append("    CRACTRESULT, ")
                .Append("    CRACTLIMITDATE, ")
                .Append("    CRACTSUCCESSDATE, ")
                .Append("    CRACTSUCCESSKIND, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    SERIESOTHER, ")
                .Append("    CRACTGIVEUPSDATE, ")
                .Append("    CRACTGIVEUP, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    BRANCH_ACTUAL, ")
                .Append("    EXESTAFFCODE, ")
                .Append("    SRVSINFLG, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    CRACTRESULTDATE, ")
                .Append("    CRACTRESULTDETAILS, ")
                .Append("    OTHERDETAIL, ")
                .Append("    BRANCH_PLAN, ")
                .Append("    ACCOUNT_PLAN, ")
                .Append("    REC_ID, ")
                .Append("    REC_LINK, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    CRACTSTATUS, ")
                .Append("    BEFORE_CRACTSTATUS, ")
                .Append("    COMPETITONMAKERNO, ")
                .Append("    COMPETITONMAKER, ")
                .Append("    COMPETITORCD, ")
                .Append("    COMPETITORNM, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    CONTACTNO, ")
                .Append("    SALESSTARTTIME, ")
                .Append("    SALESENDTIME, ")
                .Append("    WALKINNUM ")
                .Append(") ")
                .Append(" VALUES ( ")
                .Append(" :DLRCD ")
                .Append(",:FLLWSTRCD ")
                .Append(",TO_NUMBER(:FLLWUPSEQ) ")
                .Append(",:strSeqNo ")
                .Append(",:CRPLAN_ID ")
                .Append(",:BFAFDVS ")
                .Append(",:CRDVSID ")
                .Append(",TO_NUMBER(:PLANDVS) ")
                If String.Equals(memkind, "3") = False Then
                    .Append(",:INSDID ")
                Else
                    .Append(",:UNTRADEDCSTID ")
                End If
                .Append(",:MEMKIND ")
                .Append(",:VIN ")
                .Append(",NULL ")
                .Append(",NULL ")
                .Append(",NULL ")
                .Append(",TO_NUMBER(:strCRRSLTID) ")
                .Append(",:ACTACCOUNT ")
                ''NEXTACTIVEDATE, NEXTAPPOINTTIMEFLG
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTCONTINUE, CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                        .Append(",TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        .Append(",:NEXTAPPOINTTIMEFLG ")
                    Case Else
                        .Append(",NULL ")
                        .Append(",NULL ")
                End Select
                ''ACTUALTIME_START, ACTUALTIME_END
                If String.IsNullOrEmpty(stractualtimestart) = False Then
                    .Append(",TO_DATE(:strACTUALTIME_START,'YYYY/MM/DD HH24:MI:SS') ")
                    .Append(",TO_DATE(:strACTUALTIME_END,'YYYY/MM/DD HH24:MI:SS') ")
                Else
                    .Append(",NULL ")
                    .Append(",NULL ")
                End If
                ''TALKINGTIME,CALLTIME
                If String.IsNullOrEmpty(Trim(strcalltime)) Then
                    .Append(",0 ")
                    .Append(",NULL ")
                Else
                    .Append(",TO_NUMBER(:STRTALKINGTIME) ")
                    .Append(",TO_DATE(:STRCALLTIME,'YYYY/MM/DD HH24:MI:SS') ")
                End If
                .Append(",:SUBCTGCODE ")
                .Append(",:PROMOTION_ID ")
                ''STATUS
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTCONTINUE, CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                        .Append(",'1' ")
                    Case Else
                        .Append(",'2' ")
                End Select
                .Append(",:THISTIME_CRACTRESULT ")
                If String.IsNullOrEmpty(strnextactivitydatetime) = False Then
                    .Append(",TO_DATE(:STRNEXTACTIVITYDATETIME,'YYYY/MM/DD HH24:MI:SS') ")
                Else
                    .Append(",Null ")
                End If
                ''CRACTSUCCESSDATE, CRACTSUCCESSKIND, SERIESCODE, SERIESNAME, SERIESOTHER
                If thistimecractresult = CONSTCRACTRSLTSUCCESS Then
                    .Append(",TO_DATE(:strSuccessDate,'YYYY/MM/DD') ")
                    Select Case fllwuptyp
                        Case CONSTFLLWUPPERIODICAL
                            If String.Equals(insuranceflg, "1") Then
                                .Append(",'4' ")
                            Else
                                .Append(",'3' ")
                            End If
                        Case CONSTFLLWUPPROMOTION
                            .Append(",'4' ")
                        Case Else
                            .Append(",:strSuccessKind ")
                    End Select
                    Select Case fllwuptyp
                        Case CONSTFLLWUPHOT, CONSTFLLWUPPROSPECT, CONSTFLLWUPREPUCHASE, CONSTFLLWUPWALKIN
                            .Append(",' ' ")
                            .Append(",N' ' ")
                        Case Else
                            .Append(",:strSeriesCode ")
                            .Append(",:strSeriesName ")
                    End Select
                    .Append(",' ' ")
                Else
                    .Append(",NULL ")
                    .Append(",NULL ")
                    .Append(",' ' ")
                    .Append(",' ' ")
                    .Append(",' ' ")
                End If
                ''CRACTGIVEUPSDATE, CRACTGIVEUP
                If thistimecractresult = CONSTCRACTRSLTGIVEUP Then
                    .Append(",TRUNC(TO_DATE(:strGiveupDate,'YYYY/MM/DD')) ")
                    If String.IsNullOrEmpty(Trim(strmemo)) = False Then
                        .Append(",:strMEMO ")
                    Else
                        .Append(",' ' ")
                    End If
                Else
                    .Append(",NULL ")
                    .Append(",' ' ")
                End If
                .Append(",SYSDATE ")
                .Append(",SYSDATE ")
                .Append(",:ACCOUNT ")
                .Append(",:STRCD ")
                .Append(",TO_NUMBER(:OPECD) ")
                .Append(",'0' ")
                .Append(",:SERVICECD ")
                .Append(",NVL(TRIM(:strSUBCTGORGNAME), ' ') ")
                .Append(",:SUBCTGORGNAME_EX ")
                .Append(",TO_NUMBER(:strSTALL_RESERVEID) ")
                .Append(",:strSTALL_DLRCD ")
                .Append(",:strSTALL_STRCD ")
                ''CRACTRESULTDATE, CRACTRESULTDETAILS
                .Append(",NULL ")
                .Append(",' ' ")
                .Append(",' ' ")
                .Append(",:STRCD ")
                .Append(",:ACCOUNTPLAN ")
                .Append(",TO_NUMBER(:strRECID) ")
                .Append(",TO_NUMBER(:strLogicFlg) ")
                .Append(",TO_NUMBER(:strCMSHISLINKID) ")
                .Append(",:THISTIME_CRACTSTATUS ")
                .Append(",:CRACTSTATUS ")
                .Append(",TO_NUMBER(:strPurchasedMakerNo) ")
                If String.IsNullOrEmpty(strpurchasedmakername) Then
                    .Append(",N' ' ")
                Else
                    .Append(",:strPurchasedMakerName ")
                End If
                If String.IsNullOrEmpty(strpurchasedmodelcd) Then
                    .Append(",N' ' ")
                Else
                    .Append(",:strPurchasedModelCd ")
                End If
                If String.IsNullOrEmpty(strpurchasedmodelname) Then
                    .Append(",N' ' ")
                Else
                    .Append(",:strPurchasedModelName ")
                End If
                .Append(",:strCRCUSTID ")
                .Append(",:strCUSTOMERCLASS ")
                .Append(",:CONTACTNO ")
                .Append(",TO_DATE(:SALESSTARTTIME,'YYYY/MM/DD HH24:MI:SS') ")
                .Append(",TO_DATE(:SALESENDTIME,'YYYY/MM/DD HH24:MI:SS') ")
                .Append(",:WALKINNUM ")
                .Append(" ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_016")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("FLLWSTRCD", OracleDbType.Char, fllwstrcd)
                query.AddParameterWithTypeValue("FLLWUPSEQ", OracleDbType.Char, fllwupseq)
                query.AddParameterWithTypeValue("strSeqNo", OracleDbType.Char, strseqno)
                query.AddParameterWithTypeValue("CRPLAN_ID", OracleDbType.Int64, crplanid)
                query.AddParameterWithTypeValue("BFAFDVS", OracleDbType.Char, bfafdvs)
                query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Char, crdvsid)
                query.AddParameterWithTypeValue("PLANDVS", OracleDbType.Char, plandvs)
                If String.Equals(memkind, "3") = False Then
                    query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                Else
                    query.AddParameterWithTypeValue("UNTRADEDCSTID", OracleDbType.Char, untradedcstid)
                End If
                query.AddParameterWithTypeValue("MEMKIND", OracleDbType.Char, memkind)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("strCRRSLTID", OracleDbType.Char, strcrrsltid)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("ACCOUNTPLAN", OracleDbType.Char, accountplan)
                query.AddParameterWithTypeValue("ACTACCOUNT", OracleDbType.Char, actaccount)
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTCONTINUE, CONSTCRACTRSLTHOT, CONSTCRACTRSLTPROSPECT
                        query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, strnextactivitydatetime)
                        query.AddParameterWithTypeValue("NEXTAPPOINTTIMEFLG", OracleDbType.Char, nextappointtimeflg)
                End Select
                If String.IsNullOrEmpty(stractualtimestart) = False Then
                    query.AddParameterWithTypeValue("strACTUALTIME_START", OracleDbType.Char, stractualtimestart)
                    query.AddParameterWithTypeValue("strACTUALTIME_END", OracleDbType.Char, stractualtimeend)
                End If
                If String.IsNullOrEmpty(Trim(strcalltime)) = False Then
                    query.AddParameterWithTypeValue("strTALKINGTIME", OracleDbType.Char, strtalkingtime)
                    query.AddParameterWithTypeValue("strCALLTIME", OracleDbType.Char, strcalltime)
                End If
                query.AddParameterWithTypeValue("SUBCTGCODE", OracleDbType.Char, subctgcode)
                query.AddParameterWithTypeValue("PROMOTION_ID", OracleDbType.Int64, promotionid)
                query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                If String.IsNullOrEmpty(strnextactivitydatetime) = False Then
                    query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, strnextactivitydatetime)
                End If
                If thistimecractresult = CONSTCRACTRSLTSUCCESS Then
                    query.AddParameterWithTypeValue("strSuccessDate", OracleDbType.Char, strsuccessdate)
                    Select Case fllwuptyp
                        Case CONSTFLLWUPPERIODICAL
                        Case CONSTFLLWUPPROMOTION
                        Case Else
                            query.AddParameterWithTypeValue("strSuccessKind", OracleDbType.Char, strsuccesskind)
                    End Select
                    Select Case fllwuptyp
                        Case CONSTFLLWUPHOT, CONSTFLLWUPPROSPECT, CONSTFLLWUPREPUCHASE, CONSTFLLWUPWALKIN

                        Case Else
                            query.AddParameterWithTypeValue("strSeriesCode", OracleDbType.Char, strseriescode)
                            query.AddParameterWithTypeValue("strSeriesName", OracleDbType.Char, strseriesname)
                    End Select
                Else
                End If
                ''CRACTGIVEUPSDATE, CRACTGIVEUP
                If thistimecractresult = CONSTCRACTRSLTGIVEUP Then
                    query.AddParameterWithTypeValue("strGiveupDate", OracleDbType.Char, strgiveupdate)
                    If String.IsNullOrEmpty(Trim(strmemo)) = False Then
                        query.AddParameterWithTypeValue("strMEMO", OracleDbType.Char, strmemo)
                    Else
                    End If
                Else
                End If
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("OPECD", OracleDbType.Char, opecd)
                query.AddParameterWithTypeValue("SERVICECD", OracleDbType.Char, servicecd)
                query.AddParameterWithTypeValue("strSUBCTGORGNAME", OracleDbType.Char, strsubctgorgname)
                query.AddParameterWithTypeValue("SUBCTGORGNAME_EX", OracleDbType.Char, subctgorgnameex)
                query.AddParameterWithTypeValue("strSTALL_RESERVEID", OracleDbType.Int64, strstallreserveid)
                query.AddParameterWithTypeValue("strSTALL_DLRCD", OracleDbType.Char, strstalldlrcd)
                query.AddParameterWithTypeValue("strSTALL_STRCD", OracleDbType.Char, strstallstrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("strRECID", OracleDbType.Int64, strrecid)
                query.AddParameterWithTypeValue("strLogicFlg", OracleDbType.Int32, strlogicflg)
                query.AddParameterWithTypeValue("strCMSHISLINKID", OracleDbType.Int64, strcmshislinkid)
                query.AddParameterWithTypeValue("THISTIME_CRACTSTATUS", OracleDbType.Char, thistimecractstatus)
                query.AddParameterWithTypeValue("CRACTSTATUS", OracleDbType.Char, cractstatus)
                query.AddParameterWithTypeValue("strPurchasedMakerNo", OracleDbType.Char, strpurchasedmakerno)
                If String.IsNullOrEmpty(strpurchasedmakername) Then
                Else
                    query.AddParameterWithTypeValue("strPurchasedMakerName", OracleDbType.Char, strpurchasedmakername)
                End If
                If String.IsNullOrEmpty(strpurchasedmodelcd) Then
                Else
                    query.AddParameterWithTypeValue("strPurchasedModelCd", OracleDbType.Char, strpurchasedmodelcd)
                End If
                If String.IsNullOrEmpty(strpurchasedmodelname) Then
                Else
                    query.AddParameterWithTypeValue("strPurchasedModelName", OracleDbType.Char, strpurchasedmodelname)
                End If
                query.AddParameterWithTypeValue("strCRCUSTID", OracleDbType.Char, strcrcustid)
                query.AddParameterWithTypeValue("strCUSTOMERCLASS", OracleDbType.Char, strcustomerclass)
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                query.AddParameterWithTypeValue("SALESSTARTTIME", OracleDbType.Char, salesstarttime)
                query.AddParameterWithTypeValue("SALESENDTIME", OracleDbType.Char, salesendtime)
                query.AddParameterWithTypeValue("WALKINNUM", OracleDbType.Int32, walkinNum)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 017.Follow-up Box活動実施取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupboxrsltDone(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                                    ByVal doneCategory As String) As SC3080203DataSet.SC3080203FllwupboxrsltDoneDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupboxrsltDoneDataTable)("SC3080203_017")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_017 */ ")
                    .Append("    ACTENDFLG ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXRSLT_DONE ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    CATEGORY = :DONECATEGORY AND ")
                    .Append("    TRUNC(CRACTDATE) = TO_DATE(SYSDATE,'YYY/MM/DD') ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("DONECATEGORY", OracleDbType.Char, doneCategory)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 018.Follow-up Box活動実施追加 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="category"></param>
        ''' <param name="actendflg"></param>
        ''' <param name="branchplan"></param>
        ''' <param name="accountplan"></param>
        ''' <param name="exestaffcode"></param>
        ''' <param name="updateaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxrsltDone(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                                        ByVal category As String, ByVal actendflg As String,
                                                       ByVal branchplan As String, ByVal accountplan As String, ByVal exestaffcode As String,
                                                       ByVal updateaccount As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_018")
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* SC3080203_018 */ ")
                    .Append("INTO ")
                    .Append("    TBL_FLLWUPBOXRSLT_DONE ")
                    .Append("( ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    FLLWUPBOX_SEQNO, ")
                    .Append("    CRACTDATE, ")
                    .Append("    CATEGORY, ")
                    .Append("    ACTENDFLG, ")
                    .Append("    BRANCH_PLAN, ")
                    .Append("    ACCOUNT_PLAN, ")
                    .Append("    EXESTAFFCODE, ")
                    .Append("    CREATEDATE, ")
                    .Append("    UPDATEDATE, ")
                    .Append("    UPDATEACCOUNT ")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :DLRCD, ")
                    .Append("    :STRCD, ")
                    .Append("    :FLLWUPBOX_SEQNO, ")
                    '.Append("    TO_DATE(:CRACTDATE,'YYYY/MM/DD HH24:MI:SS'), ")
                    .Append("    SYSDATE, ")
                    .Append("    :CATEGORY, ")
                    .Append("    :ACTENDFLG, ")
                    .Append("    :BRANCH_PLAN, ")
                    .Append("    :ACCOUNT_PLAN, ")
                    .Append("    :EXESTAFFCODE, ")
                    .Append("    SYSDATE, ")
                    .Append("    SYSDATE, ")
                    .Append("    :UPDATEACCOUNT ")
                    .Append(") ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                'query.AddParameterWithTypeValue("CRACTDATE", OracleDbType.Char, cractdate)
                query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                query.AddParameterWithTypeValue("ACTENDFLG", OracleDbType.Char, actendflg)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchplan)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, accountplan)
                query.AddParameterWithTypeValue("EXESTAFFCODE", OracleDbType.Char, exestaffcode)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 019.Follow-up Box活動実施更新 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="category"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateFllwupboxrsltDone(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal updateaccount As String,
                                                       ByVal category As String, ByVal actendflg As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_019")
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* SC3080203_019 */ ")
                    .Append("    TBL_FLLWUPBOXRSLT_DONE ")
                    .Append("SET ")
                    .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ")
                    .Append("    UPDATEDATE = SYSDATE, ")
                    .Append("    ACTENDFLG = :ACTENDFLG, ")
                    .Append("    CATEGORY = :CATEGORY ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    TRUNC(CRACTDATE) = TRUNC(SYSDATE) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Long, fllwupboxseqno)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                query.AddParameterWithTypeValue("ACTENDFLG", OracleDbType.Char, actendflg)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 020.Follow-up Box商談メモ追加　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="cstkind"></param>
        ''' <param name="customerclass"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="inputaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxSalesmemo(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                                  ByVal cstkind As String, ByVal customerclass As String, ByVal crcustid As String,
                                                  ByVal inputaccount As String, ByVal moduleid As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_020 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOX_SALESMEMO ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    SALESMEMONO, ")
                .Append("    CSTKIND, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    CRCUSTID, ")
                .Append("    INPUTDATE, ")
                .Append("    INPUTACCOUNT, ")
                .Append("    MEMO, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    CREATEACCOUNT, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    CREATEID, ")
                .Append("    UPDATEID ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            DLRCD, ")
                .Append("            STRCD, ")
                .Append("            FLLWUPBOX_SEQNO, ")
                .Append("            NVL(( ")
                .Append("            SELECT ")
                .Append("                MAX(SALESMEMONO) + 1 ")
                .Append("            FROM ")
                .Append("                TBL_FLLWUPBOX_SALESMEMO ")
                .Append("            WHERE ")
                .Append("                DLRCD = :DLRCD AND ")
                .Append("                STRCD = :STRCD AND ")
                .Append("                FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("            ),1), ")
                .Append("            :CSTKIND, ")
                .Append("            :CUSTOMERCLASS, ")
                .Append("            :CRCUSTID, ")
                .Append("            SYSDATE, ")
                .Append("            :INPUTACCOUNT, ")
                .Append("            MEMO, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            :INPUTACCOUNT, ")
                .Append("            :INPUTACCOUNT, ")
                .Append("            :MODULEID, ")
                .Append("            :MODULEID ")
                .Append("        FROM ")
                .Append("            TBL_FLLWUPBOX_SALESMEMO_WK ")
                .Append("        WHERE ")
                .Append("            DLRCD = :DLRCD AND ")
                .Append("            STRCD = :STRCD AND ")
                .Append("            FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                .Append("            TRIM(MEMO) IS NOT NULL")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_020")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstkind)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.Char, inputaccount)
                query.AddParameterWithTypeValue("MODULEID", OracleDbType.Char, moduleid)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 021.非活動対象要件情報追加　(移行済み)
        ''' </summary>
        ''' <param name="inreserveid"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="account"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="requestcategory"></param>
        ''' <param name="cstid"></param>
        ''' <param name="seqno"></param>
        ''' <param name="lastactivityday"></param>
        ''' <param name="resaccount"></param>
        ''' <param name="branch"></param>
        ''' <param name="staffaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertInreserveInfo(ByVal inreserveid As String, ByVal dlrcd As String, ByVal strcd As String, ByVal account As String,
                                                   ByVal fllwupboxseqno As Long, ByVal requestcategory As String, ByVal cstid As String,
                                                   ByVal seqno As Nullable(Of Long), ByVal lastactivityday As String, ByVal resaccount As String,
                                                   ByVal branch As String, ByVal staffaccount As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_021")
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* SC3080203_021 */ ")
                    .Append("INTO ")
                    .Append("    TBL_INRESERVEINFO ")
                    .Append("( ")
                    .Append("    INRESERVEID, ")
                    .Append("    REQUESTID, ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    ACCOUNT, ")
                    .Append("    FLLWUPBOX_SEQNO, ")
                    .Append("    PALACTSEQ, ")
                    .Append("    CATEGORY, ")
                    .Append("    REQUESTCATEGORY, ")
                    .Append("    CSTID, ")
                    .Append("    SEQNO, ")
                    .Append("    INRESERVEDATE, ")
                    .Append("    LASTACTIVITYDAY, ")
                    .Append("    RESACCOUNT, ")
                    .Append("    BRANCH, ")
                    .Append("    STAFFACCOUNT, ")
                    .Append("    DELFLG, ")
                    .Append("    CREATEDATE, ")
                    .Append("    UPDATEDATE ")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :INRESERVEID, ")
                    .Append("    ' ', ")
                    .Append("    :DLRCD, ")
                    .Append("    :STRCD, ")
                    .Append("    :ACCOUNT, ")
                    .Append("    :FLLWUPBOX_SEQNO, ")
                    .Append("    NULL, ")
                    .Append("    '3', ")
                    .Append("    :REQUESTCATEGORY, ")
                    .Append("    :CSTID, ")
                    .Append("    :SEQNO, ")
                    .Append("    SYSDATE, ")
                    .Append("    TO_DATE(:LASTACTIVITYDAY,'YYYY/MM/DD HH24:MI:SS'), ")
                    .Append("    :RESACCOUNT, ")
                    .Append("    :BRANCH, ")
                    .Append("    :STAFFACCOUNT, ")
                    .Append("    '0', ")
                    .Append("    SYSDATE, ")
                    .Append("    SYSDATE ")
                    .Append(") ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("INRESERVEID", OracleDbType.Char, inreserveid)
                'query.AddParameterWithTypeValue("REQUESTID", OracleDbType.Char, requestid)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                'query.AddParameterWithTypeValue("PALACTSEQ", OracleDbType.Char, palactseq)
                'query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                query.AddParameterWithTypeValue("REQUESTCATEGORY", OracleDbType.Char, requestcategory)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("LASTACTIVITYDAY", OracleDbType.Char, lastactivityday)
                query.AddParameterWithTypeValue("RESACCOUNT", OracleDbType.Char, resaccount)
                query.AddParameterWithTypeValue("BRANCH", OracleDbType.Char, branch)
                query.AddParameterWithTypeValue("STAFFACCOUNT", OracleDbType.Char, staffaccount)
                'query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, delflg)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 022.未取引客個人情報追加　(移行済み)
        ''' </summary>
        ''' <param name="cstid"></param>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertNweCustomer(ByVal cstid As String, ByVal originalid As String, ByVal vin As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_022 */ ")
                .Append("INTO ")
                .Append("    TBL_NEWCUSTOMER ")
                .Append("( ")
                .Append("    CSTID, ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    ACTVCTGRYID, ")
                .Append("    AC_MODFACCOUNT, ")
                .Append("    AC_MODFFUNCDVS, ")
                .Append("    AC_MODFDATE, ")
                .Append("    REASONID, ")
                .Append("    NAME, ")
                .Append("    ADDRESS, ")
                .Append("    ADDRESS1, ")
                .Append("    ADDRESS2, ")
                .Append("    ADDRESS3, ")
                .Append("    ZIPCODE, ")
                .Append("    TELNO, ")
                .Append("    MOBILE, ")
                .Append("    EMAIL1, ")
                .Append("    EMAIL2, ")
                .Append("    SEX, ")
                .Append("    BIRTHDAY, ")
                .Append("    FAXNO, ")
                .Append("    STRCDSTAFF, ")
                .Append("    STAFFCD, ")
                .Append("    SMSFLG, ")
                .Append("    EMAILFLG, ")
                .Append("    VEHICLEFLG, ")
                .Append("    ORIGINALID, ")
                .Append("    ORIGINALDLRCD, ")
                .Append("    ORIGINALSTRCD, ")
                .Append("    ORIGINALCUSTCODE, ")
                .Append("    PALCSTID, ")
                .Append("    SENDFLG, ")
                .Append("    GAZOOMEMBERID, ")
                .Append("    SACODE, ")
                .Append("    DELFLG, ")
                .Append("    DELDATE, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    IMPORTCD, ")
                .Append("    CUSTYPE, ")
                .Append("    BUSINESSTELNO, ")
                .Append("    CONTACTDMFLG, ")
                .Append("    CONTACTHOMEFLG, ")
                .Append("    CONTACTMOBILEFLG, ")
                .Append("    CONTACTEMAILFLG, ")
                .Append("    CONTACTSMSFLG, ")
                .Append("    CONTACTID, ")
                .Append("    SOCIALID, ")
                .Append("    CITYCODE, ")
                .Append("    TERRITORY_SEQNO, ")
                .Append("    SUBCUSTOMERID, ")
                .Append("    EMPLOYEENAME, ")
                .Append("    EMPLOYEEDEPARTMENT, ")
                .Append("    EMPLOYEEPOSITION, ")
                .Append("    STATE_CD, ")
                .Append("    DISTRICT_CD, ")
                .Append("    CITY_CD, ")
                .Append("    LOCATION_CD, ")
                .Append("    NAMETITLE_CD, ")
                .Append("    NAMETITLE, ")
                .Append("    IMAGEFILE_L, ")
                .Append("    IMAGEFILE_M, ")
                .Append("    IMAGEFILE_S, ")
                .Append("    NUMBEROFFAMILY ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :CSTID, ")
                .Append("            A.DLRCD, ")
                .Append("            A.STRCD, ")
                .Append("            D.ACTVCTGRYID, ")
                .Append("            ' ' AS AC_MODFACCOUNT, ")
                .Append("            ' ' AS AC_MODFFUNCDVS, ")
                .Append("            NULL AS AC_MODFDATE, ")
                .Append("            NULL AS REASONID, ")
                .Append("            A.NAME, ")
                .Append("            A.ADDRESS, ")
                .Append("            A.ADDRESS AS ADDRESS1, ")
                .Append("            ' ' AS ADDRESS2, ")
                .Append("            ' ' AS ADDRESS3, ")
                .Append("            A.ZIPCODE, ")
                .Append("            A.TELNO, ")
                .Append("            A.MOBILE, ")
                .Append("            A.EMAIL1, ")
                .Append("            A.EMAIL2, ")
                .Append("            A.SEX, ")
                .Append("            A.BIRTHDAY, ")
                .Append("            A.FAXNO, ")
                .Append("            A.STRCDSTAFF, ")
                .Append("            A.STAFFCD, ")
                .Append("            A.SMSFLG, ")
                .Append("            A.EMAILFLG, ")
                .Append("            '1' AS VEHICLEFLG, ")
                .Append("            A.ORIGINALID, ")
                .Append("            A.DLRCD AS ORIGINALDLRCD, ")
                .Append("            A.STRCD AS ORIGINALSTRCD, ")
                .Append("            A.CUSTCD AS ORIGINALCUSTCODE, ")
                .Append("            ' ' AS PALCSTID, ")
                .Append("            '1' AS SENDFLG, ")
                .Append("            NULL AS GAZOOMEMBERID, ")
                .Append("            C.SACODE, ")
                .Append("            '0', ")
                .Append("            NULL, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            '1' AS IMPORTCD, ")
                .Append("            A.CUSTYPE, ")
                .Append("            A.BUSINESSTELNO, ")
                .Append("            B.CONTACTDMFLG, ")
                .Append("            B.CONTACTHOMEFLG, ")
                .Append("            B.CONTACTMOBILEFLG, ")
                .Append("            B.CONTACTEMAILFLG, ")
                .Append("            B.CONTACTSMSFLG, ")
                .Append("            NULL AS CONTACTID, ")
                .Append("            A.SOCIALID, ")
                .Append("            NULL AS CITYCODE, ")
                .Append("            NULL AS TERRITORY_SEQNO, ")
                .Append("            ' ' AS SUBCUSTOMERID, ")
                .Append("            A.EMPLOYEENAME, ")
                .Append("            A.EMPLOYEEDEPARTMENT, ")
                .Append("            A.EMPLOYEEPOSITION, ")
                .Append("            NULL AS STATE_CD, ")
                .Append("            NULL AS DISTRICT_CD, ")
                .Append("            NULL AS CITY_CD, ")
                .Append("            NULL AS LOCATION_CD, ")
                .Append("            A.NAMETITLE_CD, ")
                .Append("            A.NAMETITLE, ")
                .Append("            B.IMAGEFILE_L, ")
                .Append("            B.IMAGEFILE_M, ")
                .Append("            B.IMAGEFILE_S, ")
                .Append("            B.NUMBEROFFAMILY ")
                .Append("        FROM ")
                .Append("            TBLORG_CUSTOMER A, ")
                .Append("            tblORG_CUSTOMER_APPEND B, ")
                .Append("            TBLORG_VCLINFO C, ")
                .Append("            TBLORG_DLRVIN D ")
                .Append("        WHERE ")
                .Append("            A.ORIGINALID = :ORIGINALID AND ")
                .Append("            B.ORIGINALID(+) = A.ORIGINALID AND ")
                .Append("            C.VIN = :VIN AND ")
                .Append("            C.ORIGINALID = A.ORIGINALID AND ")
                .Append("            D.DLRCD = C.DLRCD AND ")
                .Append("            D.VIN = C.VIN ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_022")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 023.未取引客車両情報追加 (移行済み)
        ''' </summary>
        Public Shared Function InsertNweCustomerVclre(ByVal cstid As String, ByVal seqno As Long, ByVal makername As String, ByVal originalid As String, ByVal vin As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_023 */ ")
                .Append("INTO ")
                .Append("    TBL_NEWCUSTOMERVCLRE ")
                .Append("( ")
                .Append("    CSTID, ")
                .Append("    SEQNO, ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    MODELCODE, ")
                .Append("    VIN, ")
                .Append("    VCLREGNO, ")
                .Append("    VCLTRANREGNO, ")
                .Append("    DELIDATE, ")
                .Append("    SEVERECONDITION, ")
                .Append("    ORIGINALVIN, ")
                .Append("    ORIGINALVCLREDLRCD, ")
                .Append("    ORIGINALVCLRESTRCD, ")
                .Append("    ETCID, ")
                .Append("    SENDFLG, ")
                .Append("    DELFLG, ")
                .Append("    DELDATE, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    IMPORTCD, ")
                .Append("    MAKERNAME ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :CSTID, ")
                .Append("            :SEQNO, ")
                .Append("            A.DLRCD, ")
                .Append("            A.STRCD, ")
                .Append("            A.SERIESCD AS SERIESCODE, ")
                .Append("            A.SERIESNM AS SERIESNAME, ")
                .Append("            A.MODELCD AS MODELCODE, ")
                .Append("            A.VIN, ")
                .Append("            A.VCLREGNO, ")
                .Append("            A.VCLTRANREGNO, ")
                .Append("            A.VCLDELIDATE AS DELIDATE, ")
                .Append("            B.SEVEREFLG AS SEVERECONDITION, ")
                .Append("            A.VIN AS ORIGINALVIN, ")
                .Append("            A.DLRCD AS ORIGINALVCLREDLRCD, ")
                .Append("            A.STRCD AS ORIGINALVCLRESTRCD, ")
                .Append("            A.ETCID, ")
                .Append("            A.SENDFLG, ")
                .Append("            '0' AS DELFLG, ")
                .Append("            NULL AS DELDATE, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            '1' AS IMPORTCD, ")
                .Append("            :MAKERNAME ")
                .Append("        FROM ")
                .Append("            TBLORG_VCLINFO A, ")
                .Append("            TBLORG_DLRVIN B ")
                .Append("        WHERE ")
                .Append("            A.ORIGINALID = :ORIGINALID AND ")
                .Append("            A.VIN = :VIN AND ")
                .Append("            B.DLRCD = A.DLRCD AND ")
                .Append("            B.VIN = A.VIN ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_023")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("MAKERNAME", OracleDbType.Char, makername)
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 024.その他計画(Follow-up)取得　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetOtherPlanFllw(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203CountDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CountDataTable)("SC3080203_024")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_024 */ ")
                    .Append("    1 ")
                    .Append("FROM ")
                    .Append("    TBL_OTHERPLAN_FLLW ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    ROWNUM <= 1 ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Long, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 025.その他計画(Follow-up)追加　(移行済み)
        ''' </summary>
        ''' <param name="createmodule"></param>
        ''' <param name="createaccount"></param>
        ''' <param name="updatemodule"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertOtherPlanFllw(ByVal createmodule As String, ByVal createaccount As String, ByVal updatemodule As String,
                                             ByVal updateaccount As String, ByVal dlrcd As String, ByVal strcd As String,
                                             ByVal fllwupboxseqno As Long) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_025 */ ")
                .Append("INTO ")
                .Append("    TBL_OTHERPLAN_FLLW ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    INSDID, ")
                .Append("    VIN, ")
                .Append("    NEWVCLSEQNO, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    MEMKIND, ")
                .Append("    PLANDVS, ")
                .Append("    SUBCTGCODE, ")
                .Append("    SERVICECD, ")
                .Append("    CRACTCATEGORY, ")
                .Append("    CRACTCHARGDVS, ")
                .Append("    PROMOTION_ID, ")
                .Append("    REQCATEGORY, ")
                .Append("    CRACTRESULT, ")
                .Append("    SERVICENAME, ")
                .Append("    CATEGORY, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    VCLREGNO, ")
                .Append("    ACTUALTIME_END, ")
                .Append("    BRANCH_ACTUAL, ")
                .Append("    ACCOUNT_ACTUAL, ")
                .Append("    CRRSLTID, ")
                .Append("    NEXTACTIVEDATE, ")
                .Append("    NEXTAPPOINTTIMEFLG, ")
                .Append("    CRTIMES, ")
                .Append("    PLANSTATUS, ")
                .Append("    CREATEDATE, ")
                .Append("    CREATEMODULE, ")
                .Append("    CREATEACCOUNT, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEMODULE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS ")
                .Append(") ")
                .Append("SELECT ")
                .Append("    A.DLRCD, ")
                .Append("    A.STRCD, ")
                .Append("    A.FLLWUPBOX_SEQNO, ")
                .Append("    A.CRPLAN_ID, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        A.UNTRADEDCSTID ")
                .Append("    ELSE ")
                .Append("        A.INSDID ")
                .Append("    END AS INSDID, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        B.NEWCUST_VIN ")
                .Append("    ELSE ")
                .Append("        A.VIN ")
                .Append("    END AS VIN, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        A.VCLSEQNO ")
                .Append("    END AS NEWVCLSEQNO, ")
                .Append("    A.BFAFDVS, ")
                .Append("    A.CRDVSID, ")
                .Append("    A.MEMKIND, ")
                .Append("    A.PLANDVS, ")
                .Append("    A.SUBCTGCODE, ")
                .Append("    A.SERVICECD, ")
                .Append("    A.CRACTCATEGORY, ")
                .Append("    A.CRACTCHARGDVS, ")
                .Append("    A.PROMOTION_ID, ")
                .Append("    A.REQCATEGORY, ")
                .Append("    A.CRACTRESULT, ")
                .Append("    CASE ")
                .Append("    WHEN A.PROMOTION_ID IS NULL THEN ")
                .Append("        A.SUBCTGORGNAME ")
                .Append("    ELSE ")
                .Append("        C.PROMOTIONNAME ")
                .Append("    END AS SERVICENAME, ")
                .Append("    CASE ")
                .Append("    WHEN A.CRACTRESULT = '1' THEN ")
                .Append("        '1' ")
                .Append("    WHEN A.CRACTRESULT = '2' THEN ")
                .Append("        '2' ")
                .Append("    ELSE ")
                .Append("        CASE ")
                .Append("        WHEN A.CRACTRESULT IN ('0','4') THEN ")
                .Append("            CASE ")
                .Append("            WHEN A.CRACTCATEGORY IN ('1','4') THEN ")
                .Append("                '3' ")
                .Append("            WHEN A.CRACTCATEGORY = '2' THEN ")
                .Append("                '4' ")
                .Append("            ELSE ")
                .Append("                CASE ")
                .Append("                WHEN A.PROMOTION_ID IS NOT NULL THEN ")
                .Append("                    '5' ")
                .Append("                ELSE ")
                .Append("                    CASE ")
                .Append("                    WHEN A.REQCATEGORY = '1' THEN ")
                .Append("                        '7' ")
                .Append("                    ELSE ")
                .Append("                        '6' ")
                .Append("                    END ")
                .Append("                END ")
                .Append("            END ")
                .Append("        ELSE ")
                .Append("            ' ' ")
                .Append("        END ")
                .Append("    END AS CATEGORY, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        B.NEWCUST_SERIESCODE ")
                .Append("    ELSE ")
                .Append("        A.SERIESCODE ")
                .Append("    END AS SERIESCODE, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        B.NEWCUST_SERIESNAME ")
                .Append("    ELSE ")
                .Append("        A.SERIESNAME ")
                .Append("    END AS SERIESNAME, ")
                .Append("    CASE ")
                .Append("    WHEN A.MEMKIND = '3' THEN ")
                .Append("        B.NEWCUST_VCLREGNO ")
                .Append("    ELSE ")
                .Append("        A.VCLREGNO ")
                .Append("    END AS VCLREGNO, ")
                .Append("    NULL AS ACTUALTIME_END, ")
                .Append("    NULL AS BRANCH_ACTUAL, ")
                .Append("    NULL AS ACCOUNT_ACTUAL, ")
                .Append("    NULL AS CRRSLTID, ")
                .Append("    A.CRACTIVEDATE AS NEXTACTIVEDATE, ")
                .Append("    A.APPOINTTIMEFLG AS NEXTAPPOINTTIMEFLG, ")
                .Append("    ( ")
                .Append("    A.CRDVSID - 3 ")
                .Append("    ) AS CRTIMES, ")
                .Append("    '0' AS PLANSTATUS, ")
                .Append("    SYSDATE AS CREATEDATE, ")
                .Append("    :CREATEMODULE AS CREATEMODULE, ")
                .Append("    :CREATEACCOUNT AS CREATEACCOUNT, ")
                .Append("    SYSDATE AS UPDATEDATE, ")
                .Append("    :UPDATEMODULE AS UPDATEMODULE, ")
                .Append("    :UPDATEACCOUNT AS UPDATEACCOUNT, ")
                .Append("    A.CRCUSTID, ")
                .Append("    A.CUSTOMERCLASS ")
                .Append("FROM ")
                .Append("    TBL_FLLWUPBOX A, ")
                .Append("    TBL_FLLWUPBOXNEWCST B, ")
                .Append("    TBL_CRPROMOTION C ")
                .Append("WHERE ")
                .Append("    A.DLRCD = B.DLRCD (+) AND ")
                .Append("    A.STRCD = B.STRCD (+) AND ")
                .Append("    A.FLLWUPBOX_SEQNO = B.FLLWUPBOX_SEQNO (+) AND ")
                .Append("    A.PROMOTION_ID = C.PROMOTION_ID (+) AND ")
                .Append("    A.DLRCD = :DLRCD AND ")
                .Append("    A.STRCD = :STRCD AND ")
                .Append("    A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            End With
            Using query As New DBUpdateQuery("SC3080203_025")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CREATEMODULE", OracleDbType.Char, createmodule)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, createaccount)
                query.AddParameterWithTypeValue("UPDATEMODULE", OracleDbType.Char, updatemodule)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 026.サービスマスタ取得 (移行済み)
        ''' </summary>
        ''' <param name="mntncd"></param>
        ''' <param name="dlrcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceMaster(ByVal mntncd As String, ByVal dlrcd As String) As SC3080203DataSet.SC3080203ServiceMasterDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ServiceMasterDataTable)("SC3080203_026")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_026 */ ")
                    .Append("    SERVICECD, ")
                    .Append("    SERVICENAME ")
                    .Append("FROM ")
                    .Append("    TBL_SERVICEMASTER ")
                    .Append("WHERE ")
                    .Append("    MNTNCD = :MNTNCD AND ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = '000' ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MNTNCD", OracleDbType.Char, mntncd)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 027.中項目マスタ取得 (移行済み)
        ''' </summary>
        ''' <param name="servicecd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSubCategory(ByVal servicecd As String) As SC3080203DataSet.SC3080203SubCategoryDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SubCategoryDataTable)("SC3080203_027")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_027 */ ")
                    .Append("    SUBCTGCODE ")
                    .Append("FROM ")
                    .Append("    TBL_SUBCATEGORY ")
                    .Append("WHERE ")
                    .Append("    SERVICECD = :SERVICECD ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SERVICECD", OracleDbType.Char, servicecd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 028.Total履歴追加　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="insdid"></param>
        ''' <param name="seqno"></param>
        ''' <param name="contactdate"></param>
        ''' <param name="categoryid"></param>
        ''' <param name="categorydvsid"></param>
        ''' <param name="vin"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="status"></param>
        ''' <param name="servicenm"></param>
        ''' <param name="account"></param>
        ''' <param name="crcustid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertTotalHis(ByVal dlrcd As String, ByVal strcd As String, ByVal insdid As String,
                                       ByVal seqno As Long, ByVal contactdate As Date, ByVal categoryid As String,
                                       ByVal categorydvsid As String, ByVal vin As String, ByVal seriesname As String,
                                       ByVal status As String, ByVal servicenm As String, ByVal account As String,
                                       ByVal crcustid As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_028 */ ")
                .Append("INTO ")
                .Append("    TBL_TOTALHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    INSDID, ")
                .Append("    SEQNO, ")
                .Append("    CONTACTDATE, ")
                .Append("    CATEGORYID, ")
                .Append("    CATEGORYDVSID, ")
                .Append("    VIN, ")
                .Append("    SERIESNAME, ")
                .Append("    STATUS, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    APPOINTMENTDATE, ")
                .Append("    PROMOTION_NM, ")
                .Append("    SERVICE_NM, ")
                .Append("    MILEAGE, ")
                .Append("    SERVICEINDATE, ")
                .Append("    SNDMAILID, ")
                .Append("    RECMAILID, ")
                .Append("    REQUESTID, ")
                .Append("    ACCOUNT_NM, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS ")
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :DLRCD, ")
                .Append("    :STRCD, ")
                .Append("    :INSDID, ")
                .Append("    :SEQNO, ")
                .Append("    :CONTACTDATE, ")
                .Append("    :CATEGORYID, ")
                .Append("    :CATEGORYDVSID, ")
                .Append("    :VIN, ")
                .Append("    :SERIESNAME, ")
                .Append("    :STATUS, ")
                .Append("    NULL, ")   'STALL_REZID
                .Append("    NULL, ")   'STALL_DLRCD
                .Append("    NULL, ")   'STALL_STRCD
                .Append("    NULL, ")   'APPOINTMENTDATE
                .Append("    NULL, ")   'PROMOTION_NM
                .Append("    :SERVICE_NM, ")   'SERVICE_NM
                .Append("    NULL, ")   'MILEAGE
                .Append("    NULL, ")   'SERVICEINDATE
                .Append("    NULL, ")   'SNDMAILID
                .Append("    NULL, ")   'RECMAILID
                .Append("    NULL, ")   'REQUESTID
                .Append("    (SELECT USERNAME FROM TBL_USERS WHERE ACCOUNT = :ACCOUNT), ") 'ACCOUNT_NM
                .Append("    SYSDATE, ")
                .Append("    SYSDATE, ")
                .Append("    NULL, ")   'REC_ID
                .Append("    NULL, ")   'CMS_HISLINKID
                .Append("    :CRCUSTID, ")   'CRCUSTID
                .Append("    '1' ")  'CUSTOMERCLASS
                .Append(") ")
            End With
            Using query As New DBUpdateQuery("SC3080203_028")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("CONTACTDATE", OracleDbType.Date, contactdate)
                query.AddParameterWithTypeValue("CATEGORYID", OracleDbType.Char, categoryid)
                query.AddParameterWithTypeValue("CATEGORYDVSID", OracleDbType.Char, categorydvsid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, status)
                query.AddParameterWithTypeValue("SERVICE_NM", OracleDbType.Char, servicenm)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 029.担当スタッフ取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetUsers(ByVal dlrcd As String, ByVal strcd As String) As SC3080203DataSet.SC3080203UsersDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203UsersDataTable)("SC3080203_029")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_029 */ ")
                    .Append("    A.USERNAME, ")
                    .Append("    REPLACE(A.ACCOUNT ,'@' || :DLRCD ,'') AS ACCOUNT ")
                    .Append("FROM ")
                    .Append("    TBL_USERS A, ")
                    .Append("    TBL_USERDISPLAY B ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.OPERATIONCODE = '8' AND ")
                    .Append("    A.DELFLG = '0' AND ")
                    .Append("    B.ACCOUNT(+) = A.ACCOUNT ")
                    .Append("ORDER BY ")
                    .Append("    B.SORTNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 030.ウォークイン要件情報追加 (移行済み)
        ''' </summary>
        ''' <param name="walkinid"></param>
        ''' <param name="cstid"></param>
        ''' <param name="seqno"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="walkindate"></param>
        ''' <param name="seriescode"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="registrationtype"></param>
        ''' <param name="lastactivityday"></param>
        ''' <param name="wicid"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="originalid"></param>
        ''' <param name="account"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertWalkInPerson(ByVal walkinid As String, ByVal cstid As String, ByVal seqno As Nullable(Of Long), ByVal dlrcd As String,
                                                  ByVal strcd As String, ByVal walkindate As Date, ByVal seriescode As String,
                                                  ByVal seriesname As String, ByVal registrationtype As String, ByVal lastactivityday As Date,
                                                  ByVal wicid As Integer, ByVal fllwupboxseqno As Long, ByVal crcustid As String,
                                                  ByVal originalid As String, ByVal account As String, ByVal usernm As String,
                                                  ByVal contactno As Long, ByVal salesstarttime As String, ByVal salesendtime As String,
                                                  ByVal actaccount As String, walkinNum As Nullable(Of Integer)) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_030 */ ")
                .Append("INTO ")
                .Append("    TBL_WALKINPERSON ")
                .Append("( ")
                .Append("    WALKINID, ")
                .Append("    CSTID, ")
                .Append("    SEQNO, ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    ACCOUNT, ")
                .Append("    WALKINDATE, ")
                .Append("    RESPONSIBLE, ")
                .Append("    CHANGE_BRANCH, ")
                .Append("    CHANGE_BRANCHNAME, ")
                .Append("    CHANGE_STAFF, ")
                .Append("    CHANGE_STAFFNAME, ")
                .Append("    PURPOSEID, ")
                .Append("    PURPOSESUBID, ")
                .Append("    NEWCAR_BROCHURE, ")
                .Append("    NEWCAR_BRSERIES, ")
                .Append("    NEWCAR_INQUIRY, ")
                .Append("    NEWCAR_INQSERIES, ")
                .Append("    NEWCAR_TESTDRIVE, ")
                .Append("    NEWCAR_TDRSERIES, ")
                .Append("    NEWCAR_TDRNAME, ")
                .Append("    NEWCAR_RESERVEID, ")
                .Append("    NEWCAR_QUOTATION, ")
                .Append("    NEWCAR_TRADEIN, ")
                .Append("    NEWCAR_TRADEPRICE, ")
                .Append("    NEWCAR_CLOSING, ")
                .Append("    NEWCAR_CLOSINGPRICE, ")
                .Append("    NEWCAR_PRESEN, ")
                .Append("    SERVICE_APPOINT, ")
                .Append("    SERVICE_REZID, ")
                .Append("    SERVICE_DLRCD, ")
                .Append("    SERVICE_STRCD, ")
                .Append("    SERVICE_SUBCTGCODE, ")
                .Append("    SERVICE_DATE, ")
                .Append("    SERVICE_TIME, ")
                .Append("    SERVICE_ACCESSORY, ")
                .Append("    SERVICE_INQUIRY, ")
                .Append("    SERVICE_PRESENTATION, ")
                .Append("    OTHER_INSURANCE, ")
                .Append("    OTHER_INSCONTINUE, ")
                .Append("    OTHER_INSINQUIRY, ")
                .Append("    OTHER_MOBILE, ")
                .Append("    OTHER_MOBPURCHASE, ")
                .Append("    OTHER_MOBILECD, ")
                .Append("    OTHER_MOBILENAME, ")
                .Append("    OTHER_MOBINQUIRY, ")
                .Append("    OTHER_TESTDRIVE, ")
                .Append("    OTHER_TDRSERIES, ")
                .Append("    OTHER_TDRNAME, ")
                .Append("    OTHER_RESERVEID, ")
                .Append("    OTHER_CAMPAIGN, ")
                .Append("    OTHER_INQUIRY, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    POINTCARDFLG, ")
                .Append("    REGISTRATIONTYPE, ")
                .Append("    STATUS, ")
                .Append("    LASTACTIVITYDAY, ")
                .Append("    WICID, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    WICID_2ND, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    ORIGINALID, ")
                .Append("    COMPLAINTNO, ")
                .Append("    CONTACTNO, ")
                .Append("    SALESSTARTTIME, ")
                .Append("    SALESENDTIME, ")
                .Append("    WALKINNUM ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :WALKINID, ")
                .Append("            :CSTID, ")
                .Append("            :SEQNO, ")
                .Append("            :DLRCD, ")
                .Append("            :STRCD, ")
                .Append("            A.ACCOUNT AS ACCOUNT, ")
                .Append("            :WALKINDATE, ")
                .Append("            A.USERNAME AS RESPONSIBLE, ")
                .Append("            B.STRCD AS CHANGE_BRANCH, ")
                .Append("            B.STRNM_LOCAL AS CHANGE_BRANCHNAME, ")
                .Append("            :ACCOUNT AS CHANGE_STAFF, ")
                .Append("            :USERNAME AS CHANGE_STAFFNAME, ")
                .Append("            '1' AS PURPOSEID, ")
                .Append("            '0' AS PURPOSESUBID, ")
                .Append("            '0' AS NEWCAR_BROCHURE, ")
                .Append("            ' ' AS NEWCAR_BRSERIES, ")
                .Append("            '0' AS NEWCAR_INQUIRY, ")
                .Append("            ' ' AS NEWCAR_INQSERIES, ")
                .Append("            '0' AS NEWCAR_TESTDRIVE, ")
                .Append("            NULL AS NEWCAR_TDRSERIES, ")
                .Append("            ' ' AS NEWCAR_TDRNAME, ")
                .Append("            NULL AS NEWCAR_RESERVEID, ")
                .Append("            '0' AS NEWCAR_QUOTATION, ")
                .Append("            '0' AS NEWCAR_TRADEIN, ")
                .Append("            ' ' AS NEWCAR_TRADEPRICE, ")
                .Append("            '0' AS NEWCAR_CLOSING, ")
                .Append("            ' ' AS NEWCAR_CLOSINGPRICE, ")
                .Append("            '0' AS NEWCAR_PRESEN, ")
                .Append("            '0' AS SERVICE_APPOINT, ")
                .Append("            NULL AS SERVICE_REZID, ")
                .Append("            ' ' AS SERVICE_DLRCD, ")
                .Append("            ' ' AS SERVICE_STRCD, ")
                .Append("            ' ' AS SERVICE_SUBCTGCODE, ")
                .Append("            ' ' AS SERVICE_DATE, ")
                .Append("            ' ' AS SERVICE_TIME, ")
                .Append("            '0' AS SERVICE_ACCESSORY, ")
                .Append("            '0' AS SERVICE_INQUIRY, ")
                .Append("            '0' AS SERVICE_PRESENTATION, ")
                .Append("            '0' AS OTHER_INSURANCE, ")
                .Append("            '0' AS OTHER_INSCONTINUE, ")
                .Append("            '0' AS OTHER_INSINQUIRY, ")
                .Append("            '0' AS OTHER_MOBILE, ")
                .Append("            '0' AS OTHER_MOBPURCHASE, ")
                .Append("            ' ' AS OTHER_MOBILECD, ")
                .Append("            ' ' AS OTHER_MOBILENAME, ")
                .Append("            '0' AS OTHER_MOBINQUIRY, ")
                .Append("            '0' AS OTHER_TESTDRIVE, ")
                .Append("            NULL AS OTHER_TDRSERIES, ")
                .Append("            ' ' AS OTHER_TDRNAME, ")
                .Append("            NULL AS OTHER_RESERVEID, ")
                .Append("            '0' AS OTHER_CAMPAIGN, ")
                .Append("            '0' AS OTHER_INQUIRY, ")
                .Append("            :SERIESCODE, ")
                .Append("            :SERIESNAME, ")
                .Append("            '0' AS POINTCARDFLG, ")
                .Append("            :REGISTRATIONTYPE, ")
                .Append("            '1' AS STATUS, ")
                .Append("            :LASTACTIVITYDAY, ")
                .Append("            :WICID, ")
                .Append("            :FLLWUPBOX_SEQNO, ")
                .Append("            SYSDATE, ")
                .Append("            SYSDATE, ")
                .Append("            NULL AS WICID_2ND, ")
                .Append("            :CRCUSTID, ")
                .Append("            '1' AS CUSTOMERCLASS, ")
                .Append("            :ORIGINALID, ")
                .Append("            NULL AS COMPLAINTNO, ")
                .Append("            :CONTACTNO, ")
                .Append("            TO_DATE(:SALESSTARTTIME,'YYYY/MM/DD HH24:MI:SS'), ")
                .Append("            TO_DATE(:SALESENDTIME,'YYYY/MM/DD HH24:MI:SS'), ")
                .Append("            :WALKINNUM ")
                .Append("        FROM ")
                .Append("            TBL_USERS A, ")
                .Append("            TBLM_BRANCH B ")
                .Append("        WHERE ")
                .Append("            A.ACCOUNT= :ACTACCOUNT AND ")
                .Append("            B.DLRCD = A.DLRCD AND ")
                .Append("            B.STRCD = A.STRCD ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_030")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("WALKINID", OracleDbType.Char, walkinid)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("ACTACCOUNT", OracleDbType.Char, actaccount)
                query.AddParameterWithTypeValue("WALKINDATE", OracleDbType.Date, walkindate)
                query.AddParameterWithTypeValue("SERIESCODE", OracleDbType.Char, seriescode)
                query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
                query.AddParameterWithTypeValue("REGISTRATIONTYPE", OracleDbType.Char, registrationtype)
                query.AddParameterWithTypeValue("LASTACTIVITYDAY", OracleDbType.Date, lastactivityday)
                query.AddParameterWithTypeValue("WICID", OracleDbType.Int64, wicid)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("USERNAME", OracleDbType.Char, usernm)
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                query.AddParameterWithTypeValue("SALESSTARTTIME", OracleDbType.Char, salesstarttime)
                query.AddParameterWithTypeValue("SALESENDTIME", OracleDbType.Char, salesendtime)
                query.AddParameterWithTypeValue("WALKINNUM", OracleDbType.Int32, walkinNum)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 031.ウォークイン要件メモ追加　(移行済み)
        ''' </summary>
        ''' <param name="walkinid"></param>
        ''' <param name="dlrcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertWalkInPersonMemo(ByVal walkinid As String, ByVal dlrcd As String) As Integer
            Using query As New DBUpdateQuery("SC3080203_031")
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* SC3080203_031 */ ")
                    .Append("INTO ")
                    .Append("    TBL_WALKINPERSONMEMO ")
                    .Append("( ")
                    .Append("    WALKINID, ")
                    .Append("    DLRCD, ")
                    .Append("    MEMO, ")
                    .Append("    SERVICE_ACSDETAIL, ")
                    .Append("    SERVICE_INQUIRYDETAIL, ")
                    .Append("    OTHER_INSINQDETAIL, ")
                    .Append("    OTHER_MOBINQDETAIL, ")
                    .Append("    OTHER_INQUIRYDETAIL, ")
                    .Append("    CREATEDATE, ")
                    .Append("    UPDATEDATE ")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :WALKINID, ")
                    .Append("    :DLRCD, ")
                    .Append("    ' ', ")
                    .Append("    ' ', ")
                    .Append("    ' ', ")
                    .Append("    ' ', ")
                    .Append("    ' ', ")
                    .Append("    ' ', ")
                    .Append("    SYSDATE, ")
                    .Append("    SYSDATE ")
                    .Append(") ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("WALKINID", OracleDbType.Char, walkinid)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 032.自社客走行距離履歴取得 (移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMileageHis(ByVal originalid As String, ByVal vin As String) As SC3080203DataSet.SC3080201MileageHisDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201MileageHisDataTable)("SC3080203_032")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_032 */ ")
                    .Append("    DLRCD, ")
                    .Append("    STRCD, ")
                    .Append("    MILEAGESEQ, ")
                    .Append("    JOBNO, ")
                    .Append("    MILEAGE, ")
                    .Append("    TO_CHAR(REGISTDATE,'YYYY/MM/DD HH24:MI:SS') AS REGISTDATE ")
                    .Append("FROM ")
                    .Append("    TBLORG_MILEAGEHIS ")
                    .Append("WHERE ")
                    .Append("    ORIGINALID = :ORIGINALID AND ")
                    .Append("    VIN = :VIN AND ")
                    .Append("    ACQUDVSN = '1' ")
                    .Append("ORDER BY ")
                    .Append("    REGISTDATE DESC ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 033.自社客点検履歴取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="jobno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceHis(ByVal dlrcd As String, ByVal jobno As String) As SC3080203DataSet.SC3080201ServiceHisDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201ServiceHisDataTable)("SC3080203_033")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_033 */ ")
                    .Append("    DLRCD, ")
                    .Append("    JOBNO, ")
                    .Append("    INSPECSEQ, ")
                    .Append("    SERVICECD ")
                    .Append("FROM ")
                    .Append("    TBLORG_SERVICEHIS ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    JOBNO = :JOBNO AND ")
                    .Append("    INSPECSEQ IN ")
                    .Append("        ( ")
                    .Append("        SELECT ")
                    .Append("            MAX(INSPECSEQ) ")
                    .Append("        FROM ")
                    .Append("            TBLORG_SERVICEHIS ")
                    .Append("        WHERE ")
                    .Append("            DLRCD = :DLRCD AND ")
                    .Append("            JOBNO = :JOBNO ")
                    .Append("        GROUP BY ")
                    .Append("            DLRCD, ")
                    .Append("            JOBNO ")
                    .Append("        ) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("JOBNO", OracleDbType.Char, jobno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 034.Walk-in Person SeqNo取得 (移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSeqWalkInPersonWalkInId() As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_034")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_034 */ ")
                    .Append("    SEQ_WALKINPERSON_WALKINID.NEXTVAL AS SEQ ")
                    .Append("FROM ")
                    .Append("    DUAL ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 035.非活動対象要件情報SeqNo取得　(移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSeqInreserveInfoInreserveId() As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_035")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_035 */ ")
                    .Append("    SEQ_INRESERVEINFO_INRESERVEID.NEXTVAL AS SEQ ")
                    .Append("FROM ")
                    .Append("    DUAL ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 036.未取引客個人情報追加SeqNo取得　(移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSeqNewcustomerCstId() As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_036")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_036 */ ")
                    .Append("    SEQ_NEWCUSTOMER_CSTID.NEXTVAL AS SEQ ")
                    .Append("FROM ")
                    .Append("    DUAL ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 037.未取引客車両情報追加SeqNo取得　(移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSeqNewcustomerVclreSeqno() As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_037")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_037 */ ")
                    .Append("    SEQ_NEWCUSTOMERVCLRE_SEQNO.NEXTVAL AS SEQ ")
                    .Append("FROM ")
                    .Append("    DUAL ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 038.Follow-up Box商談メモWK削除 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DeleteFllwupboxSalesmemowk(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As Integer
            Using query As New DBUpdateQuery("SC3080203_038")
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE /* SC3080203_038 */ ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX_SALESMEMO_WK ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 039.メーカー名取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="cntcd"></param>
        ''' <param name="seriescd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetMakername(ByVal dlrcd As String, ByVal cntcd As String, ByVal seriescd As String) As SC3080203DataSet.SC3080201MakernameDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_039 */ ")
                .Append("    M.MAKERNAME ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        A.DLRCD, ")
                .Append("        B.SERIESCD, ")
                .Append("        B.SERIESNM, ")
                .Append("        B.TOYOTABRAND, ")
                .Append("        B.IMAGEPATH, ")
                .Append("        B.COMSERIESCD, ")
                .Append("        B.DELFLG, ")
                .Append("        B.DELDATE, ")
                .Append("        B.CREATEDATE, ")
                .Append("        B.UPDATEDATE, ")
                .Append("        B.MAKERCD ")
                .Append("    FROM ")
                .Append("        TBLM_DEALER A, ")
                .Append("        tblORG_SERIESMASTER B ")
                '.Append("    WHERE ")
                '.Append("        A.DLRCD = B.DLRCD AND ")
                '.Append("        A.DLRCD = :DLRCD AND ")
                '.Append("        A.DELFLG = '0' AND ")
                '.Append("        A.CNTCD = :CNTCD OR ")
                '.Append("        (B.DLRCD = '00000' AND A.DLRCD = :DLRCD AND A.DELFLG = '0' AND A.CNTCD = :CNTCD AND NOT EXISTS ( ")
                '.Append("                                                                                                       SELECT ")
                '.Append("                                                                                                           1 ")
                '.Append("                                                                                                       FROM ")
                '.Append("                                                                                                           tblORG_SERIESMASTER C ")
                '.Append("                                                                                                       WHERE ")
                '.Append("                                                                                                           C.DLRCD = A.DLRCD AND ")
                '.Append("                                                                                                           C.SERIESCD = B.SERIESCD ")
                '.Append("                                                                                                       )) ")
                .Append("    WHERE ")
                .Append("        A.DLRCD = B.DLRCD AND ")
                .Append("        A.DLRCD = :DLRCD AND ")
                .Append("        A.DELFLG = '0' AND ")
                .Append("        A.CNTCD = :CNTCD OR ")
                .Append("        (B.DLRCD = '00000'  ")
                .Append("            AND A.DLRCD =  :DLRCD  ")
                .Append("            AND A.DELFLG = '0'  ")
                .Append("            AND A.CNTCD =  :CNTCD  ")
                .Append("            AND NOT EXISTS (SELECT 1   ")
                .Append("                              FROM TBLORG_SERIESMASTER C  ")
                .Append("                              WHERE C.DLRCD = A.DLRCD  ")
                .Append("                              AND C.SERIESCD = B.SERIESCD)) ")
                .Append("    ) S, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        A.DLRCD, ")
                .Append("        B.MAKERCD, ")
                .Append("        B.MAKERNAME, ")
                .Append("        B.TOYOTABRAND, ")
                .Append("        B.CREATEDATE, ")
                .Append("        B.UPDATEDATE ")
                .Append("    FROM ")
                .Append("        TBLM_DEALER A, ")
                .Append("        TBLORG_MAKERMASTER B ")
                .Append("    WHERE ")
                .Append("        A.DLRCD = B.DLRCD AND ")
                .Append("        A.DLRCD = :DLRCD AND ")
                .Append("        A.DELFLG = '0' AND ")
                .Append("        A.CNTCD = :CNTCD OR ")
                .Append("        (B.DLRCD = '00000' AND A.DLRCD = :DLRCD AND A.DELFLG = '0' AND A.CNTCD =:CNTCD AND NOT EXISTS ( ")
                .Append("                                                                                                      SELECT ")
                .Append("                                                                                                          1 ")
                .Append("                                                                                                      FROM ")
                .Append("                                                                                                          TBLORG_MAKERMASTER C ")
                .Append("                                                                                                      WHERE ")
                .Append("                                                                                                          C.DLRCD = A.DLRCD AND ")
                .Append("                                                                                                          C.MAKERCD = B.MAKERCD ")
                .Append("                                                                                                      )) ")
                .Append("    ) M ")
                .Append("WHERE ")
                .Append("    S.DLRCD = :DLRCD AND ")
                .Append("    S.SERIESCD = :SERIESCD AND ")
                .Append("    S.MAKERCD = M.MAKERCD ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201MakernameDataTable)("SC3080203_039")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.Char, seriescd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 040.TotalHisSqeNo取得 (移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSeqTotalhisSeqno() As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_040")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_040 */ ")
                    .Append("    SEQ_TOTALHIS_SEQNO.NEXTVAL AS SEQ ")
                    .Append("FROM ")
                    .Append("    DUAL ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 041.来店区分取得　(移行済み)
        ''' </summary>
        ''' <param name="wicid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetWinclass(ByVal wicid As String) As SC3080203DataSet.SC3080203WinclassDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203WinclassDataTable)("SC3080203_041")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_041 */ ")
                    .Append("    WICNAME, ")
                    .Append("    ACTIONCD ")
                    .Append("FROM ")
                    .Append("    TBL_WINCLASS ")
                    .Append("WHERE ")
                    .Append("    WICID = :WICID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("WICID", OracleDbType.Char, wicid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 042.Follow-up Box追加　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="cractivedate"></param>
        ''' <param name="appointtimeflg"></param>
        ''' <param name="cractlimitdate"></param>
        ''' <param name="untradedcstid"></param>
        ''' <param name="vclseqno"></param>
        ''' <param name="cractresult"></param>
        ''' <param name="branchplan"></param>
        ''' <param name="accountplan"></param>
        ''' <param name="relatedinfoflg"></param>
        ''' <param name="updateaccount"></param>
        ''' <param name="createcractresult"></param>
        ''' <param name="prospectdate"></param>
        ''' <param name="hotdate"></param>
        ''' <param name="wicid"></param>
        ''' <param name="cractstatus"></param>
        ''' <param name="cractstatus1st"></param>
        ''' <param name="branchplan1st"></param>
        ''' <param name="accountplan1st"></param>
        ''' <param name="cractivedate1st"></param>
        ''' <param name="cractlimitdate1st"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="createdby"></param>
        ''' <param name="cstid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks> 
        ''' <History>
        ''' 2012/02/15 TCS 河原【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正
        ''' </History>
        Public Shared Function InsertNewCustFllwupbox(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                               ByVal cractivedate As Date, ByVal appointtimeflg As String, ByVal cractlimitdate As Date,
                                               ByVal untradedcstid As String, ByVal vclseqno As Nullable(Of Long), ByVal cractresult As String,
                                               ByVal branchplan As String, ByVal accountplan As String, ByVal relatedinfoflg As String,
                                               ByVal nextcractivedate As Nullable(Of Date), ByVal updateaccount As String, ByVal createcractresult As String,
                                               ByVal prospectdate As Nullable(Of Date), ByVal hotdate As Nullable(Of Date), ByVal wicid As Long, ByVal cractstatus As String,
                                               ByVal cractstatus1ST As String, ByVal branchplan1ST As String, ByVal accountplan1ST As String,
                                               ByVal cractivedate1ST As Date, ByVal cractlimitdate1ST As Date, ByVal crcustid As String,
                                               ByVal createdby As String, ByVal cstid As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_042 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOX ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    PLANDVS, ")
                .Append("    CRACTIVEDATE, ")
                .Append("    APPOINTTIMEFLG, ")
                .Append("    SUBCTGCODE, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    CRACTCATEGORY, ")
                .Append("    CRACTCHARGDVS, ")
                .Append("    CRACTLIMITDATE, ")
                .Append("    PROMOTION_ID, ")
                .Append("    PROMOTION_DVS, ")
                .Append("    INSURANCEFLG, ")
                .Append("    FINANCEFLG, ")
                .Append("    REQCATEGORY, ")
                .Append("    REQUESTID, ")
                .Append("    UNTRADEDCSTID, ")
                .Append("    VCLSEQNO, ")
                .Append("    WALKINPURPOSE, ")
                .Append("    CRACTRESULT, ")
                .Append("    BRANCH_PLAN, ")
                .Append("    ACCOUNT_PLAN, ")
                .Append("    ACTUALFLG, ")
                .Append("    INSDID, ")
                .Append("    NAME, ")
                .Append("    VIN, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    MODELCODE, ")
                .Append("    VCLREGNO, ")
                .Append("    CRDATE_DM_D, ")
                .Append("    CRDATE_RMM_D, ")
                .Append("    CRDATE_1STCALL, ")
                .Append("    LAST_TELDATE, ")
                .Append("    LAST_TELACCOUNT, ")
                .Append("    LAST_SERVICEINDATE, ")
                .Append("    CRDATE, ")
                .Append("    MEMKIND, ")
                .Append("    CUSTSEGMENT, ")
                .Append("    ADDRESS, ")
                .Append("    ZIPCODE, ")
                .Append("    TEL, ")
                .Append("    FAXNO, ")
                .Append("    MOBILE, ")
                .Append("    BUSINESSTELNO, ")
                .Append("    CUSTID, ")
                .Append("    DATADVS, ")
                .Append("    RMBRANCH, ")
                .Append("    SALESSTAFFCD, ")
                .Append("    SALESSTAFFNM, ")
                .Append("    SERVICESTAFFCD, ")
                .Append("    SERVICESTAFFNM, ")
                .Append("    CUSTCHRGSTRCD, ")
                .Append("    CUSTCHRGSTRNM, ")
                .Append("    CUSTCHRGSTAFFCD, ")
                .Append("    CUSTCHRGSTAFFNM, ")
                .Append("    BIRTHDAY, ")
                .Append("    SEX, ")
                .Append("    RELATEDINFOFLG, ")
                .Append("    POLICYNO, ")
                .Append("    SUBNO, ")
                .Append("    LAST_SUBCTGCODE, ")
                .Append("    LAST_SERVICECODE, ")
                .Append("    LAST_SERVICENAME, ")
                .Append("    LAST_OPERATOR, ")
                .Append("    LAST_SERIESCODE, ")
                .Append("    LAST_SERIESNAME, ")
                .Append("    LAST_REGNO, ")
                .Append("    LAST_ACTIVITYRESULT, ")
                .Append("    LAST_CRDVSID, ")
                .Append("    LAST_CRACTIVEDATE, ")
                .Append("    NEXTCRACTIVEDATE, ")
                .Append("    EXESTAFFCODE, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    BEFORE_CRACTRESULT, ")
                .Append("    CR_SATRT_DATE, ")
                .Append("    PARENT_FLLWUPBOX_SEQNO, ")
                .Append("    CRACTRESULT_UPDATEDATE, ")
                .Append("    RETRY_DATE, ")
                .Append("    SALESBKGNO, ")
                .Append("    SALESBKGDATE, ")
                .Append("    SALESBKGNO_INPUT_ACCOUNT, ")
                .Append("    CREATE_CRACTRESULT, ")
                .Append("    PROSPECT_DATE, ")
                .Append("    HOT_DATE, ")
                .Append("    WICID, ")
                .Append("    TEAMCD, ")
                .Append("    REFERRAL_FLG, ")
                .Append("    INSURANCESTAFFFLG, ")
                .Append("    CRACTSTATUS, ")
                .Append("    CRACTSTATUS_1ST, ")
                .Append("    BRANCH_PLAN_1ST, ")
                .Append("    ACCOUNT_PLAN_1ST, ")
                .Append("    CRACTIVEDATE_1ST, ")
                .Append("    CRACTLIMITDATE_1ST, ")
                .Append("    DIRECT_BILLING, ")
                .Append("    WICID_2ND, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    EMPLOYEENAME, ")
                .Append("    EMPLOYEEDEPARTMENT, ")
                .Append("    EMPLOYEEPOSITION, ")
                .Append("    CREATEDBY ")
                .Append(") ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            :DLRCD, ")
                .Append("            :STRCD, ")
                .Append("            :FLLWUPBOX_SEQNO, ")
                .Append("            NULL AS CRPLAN_ID, ")
                .Append("            ' ' AS BFAFDVS, ")
                .Append("            4 AS CRDVSID, ")
                .Append("            '0' AS PLANDVS, ")
                .Append("            :CRACTIVEDATE, ")
                .Append("            :APPOINTTIMEFLG, ")
                .Append("            ' ' AS SUBCTGCODE, ")
                .Append("            ' ' AS SERVICECD, ")
                .Append("            ' ' AS SUBCTGORGNAME, ")
                .Append("            ' ' AS SUBCTGORGNAME_EX, ")
                .Append("            '0' AS CRACTCATEGORY, ")
                .Append("            '2' AS CRACTCHARGDVS, ")
                .Append("            :CRACTLIMITDATE, ")
                .Append("            NULL AS PROMOTION_ID, ")
                .Append("            ' ' AS PROMOTION_DVS, ")
                .Append("            '0' AS INSURANCEFLG, ")
                .Append("            '0' AS FINANCEFLG, ")
                .Append("            '1' AS REQCATEGORY, ")
                .Append("            ' ' AS REQUESTID, ")
                .Append("            :UNTRADEDCSTID, ")
                .Append("            :VCLSEQNO, ")
                .Append("            '1' AS WALKINPURPOSE, ")
                .Append("            :CRACTRESULT, ")
                .Append("            :BRANCH_PLAN, ")
                .Append("            :ACCOUNT_PLAN, ")
                .Append("            '0' AS ACTUALFLG, ")
                .Append("            ' ' AS INSDID, ")
                .Append("            ' ' AS NAME, ")
                .Append("            ' ' AS VIN, ")
                .Append("            ' ' AS SERIESCODE, ")
                .Append("            ' ' AS SERIESNAME, ")
                .Append("            ' ' AS MODELCODE, ")
                .Append("            ' ' AS VCLREGNO, ")
                .Append("            NULL AS CRDATE_DM_D, ")
                .Append("            NULL AS CRDATE_RMM_D, ")
                .Append("            NULL AS CRDATE_1STCALL, ")
                .Append("            NULL AS LAST_TELDATE, ")
                .Append("            ' ' AS LAST_TELACCOUNT, ")
                .Append("            NULL AS LAST_SERVICEINDATE, ")
                .Append("            NULL AS CRDATE, ")
                .Append("            '3' AS MEMKIND, ")
                .Append("            '2' AS CUSTSEGMENT, ")
                .Append("            ' ' AS ADDRESS, ")
                .Append("            ' ' AS ZIPCODE, ")
                .Append("            ' ' AS TEL, ")
                .Append("            ' ' AS FAXNO, ")
                .Append("            ' ' AS MOBILE, ")
                .Append("            ' ' AS BUSINESSTELNO, ")
                .Append("            ' ' AS CUSTID, ")
                .Append("            '0' AS DATADVS, ")
                .Append("            ' ' AS RMBRANCH, ")
                .Append("            ' ' AS SALESSTAFFCD, ")
                .Append("            ' ' AS SALESSTAFFNM, ")
                .Append("            ' ' AS SERVICESTAFFCD, ")
                .Append("            ' ' AS SERVICESTAFFNM, ")
                '2012/02/15 TCS 河原【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正 START
                '.Append("            B.STRCD AS CUSTCHRGSTRCD, ")
                '.Append("            B.STRNM_LOCAL AS CUSTCHRGSTRNM, ")
                .Append("            NVL(B.STRCD,' ') AS CUSTCHRGSTRCD, ")
                .Append("            NVL(B.STRNM_LOCAL,' ') AS CUSTCHRGSTRNM, ")
                '2012/02/15 TCS 河原【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正 END
                .Append("            NVL(C.ACCOUNT,' ') AS CUSTCHRGSTAFFCD, ")
                .Append("            NVL(C.USERNAME,' ') AS CUSTCHRGSTAFFNM, ")
                .Append("            NULL AS BIRTHDAY, ")
                .Append("            ' ' AS SEX, ")
                .Append("            :RELATEDINFOFLG, ")
                .Append("            ' ' AS POLICYNO, ")
                .Append("            ' ' AS SUBNO, ")
                .Append("            ' ' AS LAST_SUBCTGCODE, ")
                .Append("            ' ' AS LAST_SERVICECODE, ")
                .Append("            ' ' AS LAST_SERVICENAME, ")
                .Append("            ' ' AS LAST_OPERATOR, ")
                .Append("            ' ' AS LAST_SERIESCODE, ")
                .Append("            ' ' AS LAST_SERIESNAME, ")
                .Append("            ' ' AS LAST_REGNO, ")
                .Append("            NULL AS LAST_ACTIVITYRESULT, ")
                .Append("            NULL AS LAST_CRDVSID, ")
                .Append("            NULL AS LAST_CRACTIVEDATE, ")
                .Append("            :NEXTCRACTIVEDATE, ")
                .Append("            '8' AS EXESTAFFCODE, ")
                .Append("            SYSDATE AS CREATEDATE, ")
                .Append("            SYSDATE AS UPDATEDATE, ")
                .Append("            :UPDATEACCOUNT, ")
                .Append("            ' ' AS BEFORE_CRACTRESULT, ")
                .Append("            SYSDATE AS CR_SATRT_DATE, ")
                .Append("            NULL AS PARENT_FLLWUPBOX_SEQNO, ")
                .Append("            SYSDATE AS CRACTRESULT_UPDATEDATE, ")
                .Append("            NULL AS RETRY_DATE, ")
                .Append("            ' ' AS SALESBKGNO, ")
                .Append("            NULL AS SALESBKGDATE, ")
                .Append("            ' ' AS SALESBKGNO_INPUT_ACCOUNT, ")
                .Append("            :CREATE_CRACTRESULT, ")
                .Append("            :PROSPECT_DATE, ")
                .Append("            :HOT_DATE, ")
                .Append("            :WICID, ")
                .Append("            ' ' AS TEAMCD, ")
                .Append("            '0' AS REFERRAL_FLG, ")
                .Append("            NULL AS INSURANCESTAFFFLG, ")
                .Append("            :CRACTSTATUS, ")
                .Append("            :CRACTSTATUS_1ST, ")
                .Append("            :BRANCH_PLAN_1ST, ")
                .Append("            :ACCOUNT_PLAN_1ST, ")
                .Append("            :CRACTIVEDATE_1ST, ")
                .Append("            :CRACTLIMITDATE_1ST, ")
                .Append("            '0' AS DIRECT_BILLING, ")
                .Append("            NULL AS WICID_2ND, ")
                .Append("            :CRCUSTID, ")
                .Append("            '1' AS CUSTOMERCLASS, ")
                .Append("            A.EMPLOYEENAME, ")
                .Append("            A.EMPLOYEEDEPARTMENT, ")
                .Append("            A.EMPLOYEEPOSITION, ")
                .Append("            :CREATEDBY ")
                .Append("        FROM ")
                .Append("            TBL_NEWCUSTOMER A, ")
                .Append("            TBLM_BRANCH B, ")
                .Append("            TBL_USERS C ")
                .Append("        WHERE ")
                .Append("            A.CSTID = :CSTID AND ")
                '2012/02/15 TCS 河原【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正 START
                '.Append("            B.DLRCD = A.DLRCD AND ")
                '.Append("            B.STRCD = A.STRCD AND ")
                .Append("            B.DLRCD(+) = A.DLRCD AND ")
                .Append("            B.STRCD(+) = A.STRCDSTAFF AND ")
                '2012/02/15 TCS 河原【SALES_1A】店舗コード000の未取引客で活動結果登録エラーの不具合修正 END
                .Append("            C.ACCOUNT(+) = A.STAFFCD AND ")
                .Append("            C.DELFLG(+) = '0' ")
                .Append("        ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_042")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("CRACTIVEDATE", OracleDbType.Date, cractivedate)
                query.AddParameterWithTypeValue("APPOINTTIMEFLG", OracleDbType.Char, appointtimeflg)
                query.AddParameterWithTypeValue("CRACTLIMITDATE", OracleDbType.Date, cractlimitdate)
                query.AddParameterWithTypeValue("UNTRADEDCSTID", OracleDbType.Char, untradedcstid)
                query.AddParameterWithTypeValue("VCLSEQNO", OracleDbType.Int64, vclseqno)
                query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, branchplan)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, accountplan)
                query.AddParameterWithTypeValue("RELATEDINFOFLG", OracleDbType.Char, relatedinfoflg)
                query.AddParameterWithTypeValue("NEXTCRACTIVEDATE", OracleDbType.Date, nextcractivedate)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("CREATE_CRACTRESULT", OracleDbType.Char, createcractresult)
                query.AddParameterWithTypeValue("PROSPECT_DATE", OracleDbType.Date, prospectdate)
                query.AddParameterWithTypeValue("HOT_DATE", OracleDbType.Date, hotdate)
                query.AddParameterWithTypeValue("WICID", OracleDbType.Int64, wicid)
                query.AddParameterWithTypeValue("CRACTSTATUS", OracleDbType.Char, cractstatus)
                query.AddParameterWithTypeValue("CRACTSTATUS_1ST", OracleDbType.Char, cractstatus1ST)
                query.AddParameterWithTypeValue("BRANCH_PLAN_1ST", OracleDbType.Char, branchplan1ST)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN_1ST", OracleDbType.Char, accountplan1ST)
                query.AddParameterWithTypeValue("CRACTIVEDATE_1ST", OracleDbType.Date, cractivedate1ST)
                query.AddParameterWithTypeValue("CRACTLIMITDATE_1ST", OracleDbType.Date, cractlimitdate1ST)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("CREATEDBY", OracleDbType.Char, createdby)
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 043.Follow-upBox取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupBox(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwupBoxDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupBoxDataTable)("SC3080203_043")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_043 */ ")
                    .Append("    A.CRPLAN_ID, ")
                    .Append("    A.BFAFDVS, ")
                    .Append("    A.CRDVSID, ")
                    .Append("    A.PLANDVS, ")
                    .Append("    A.SUBCTGCODE, ")
                    .Append("    A.PROMOTION_ID, ")
                    .Append("    A.REQUESTID, ")
                    .Append("    A.UNTRADEDCSTID, ")
                    .Append("    A.VCLSEQNO, ")
                    .Append("    ACCOUNT_PLAN, ")
                    .Append("    A.INSDID, ")
                    .Append("    A.VIN, ")
                    .Append("    A.MEMKIND, ")
                    .Append("    A.CUSTCHRGSTRCD, ")
                    .Append("    A.CUSTCHRGSTAFFCD, ")
                    .Append("    A.CUSTSEGMENT, ")
                    .Append("    A.VCLREGNO, ")
                    .Append("    A.SERIESNAME, ")
                    .Append("    A.BRANCH_PLAN, ")
                    .Append("    A.SERVICECD, ")
                    .Append("    A.SUBCTGORGNAME, ")
                    .Append("    A.SUBCTGORGNAME_EX, ")
                    .Append("    A.INSURANCEFLG, ")
                    .Append("    TO_CHAR(A.CRACTLIMITDATE,'YYYY/MM/DD HH24:MI:SS') AS CRACTLIMITDATE, ")
                    .Append("    A.CRACTCATEGORY, ")
                    .Append("    A.REQCATEGORY, ")
                    .Append("    A.CRACTRESULT, ")
                    .Append("    A.SERIESCODE, ")
                    .Append("    B.PROMOTIONNAME, ")
                    .Append("    B.CONDITION, ")
                    .Append("    D.REQUESTNM, ")
                    .Append("    A.CRACTSTATUS ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX A, ")
                    .Append("    TBL_CRPROMOTION B, ")
                    .Append("    TBL_FLREQUEST C, ")
                    .Append("    TBL_REQUESTDIV D ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    B.PROMOTION_ID(+) = A.PROMOTION_ID AND ")
                    .Append("    B.DLRCD(+) = A.DLRCD AND ")
                    .Append("    C.DLRCD(+) = A.DLRCD AND ")
                    .Append("    C.STRCD(+) = A.STRCD AND ")
                    .Append("    C.FLLWUPBOX_SEQNO(+) = A.FLLWUPBOX_SEQNO AND ")
                    .Append("    D.REQUESTDIV(+) = C.REQUESTDIVID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 044.希望車種の台数を取得　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSelectedCarNum(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal seqno As String) As SC3080203DataSet.SC3080203SelectedCarNumDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SelectedCarNumDataTable)("SC3080203_044")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_044 */ ")
                    .Append("    QUANTITY ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX_SELECTED_SERIES ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    SEQNO = TO_NUMBER(:SEQNO) ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Char, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 045.その他計画(Follow-up)追加(活動結果登録時)　(移行済み)
        ''' </summary>
        ''' <param name="strcategory"></param>
        ''' <param name="strcrresult"></param>
        ''' <param name="strplanstatus"></param>
        ''' <param name="category"></param>
        ''' <param name="actualtimeend"></param>
        ''' <param name="branchactual"></param>
        ''' <param name="accountactual"></param>
        ''' <param name="planstatus"></param>
        ''' <param name="createmodule"></param>
        ''' <param name="createaccount"></param>
        ''' <param name="updatemodule"></param>
        ''' <param name="updateaccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertOtherPlanFllwRslt(ByVal strcategory As String, ByVal strcrresult As String,
                                                       ByVal strplanstatus As String, ByVal category As String,
                                                       ByVal actualtimeend As Date, ByVal branchactual As String,
                                                       ByVal accountactual As String, ByVal planstatus As String,
                                                       ByVal createmodule As String, ByVal createaccount As String,
                                                       ByVal updatemodule As String, ByVal updateaccount As String,
                                                       ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT INTO /* SC3080203_045 */ TBL_OTHERPLAN_FLLW ")
                .Append(" ( ")
                .Append("  DLRCD ")
                .Append(" ,STRCD ")
                .Append(" ,FLLWUPBOX_SEQNO ")
                .Append(" ,CRPLAN_ID ")
                .Append(" ,INSDID ")
                .Append(" ,VIN ")
                .Append(" ,NEWVCLSEQNO ")
                .Append(" ,BFAFDVS ")
                .Append(" ,CRDVSID ")
                .Append(" ,MEMKIND ")
                .Append(" ,PLANDVS ")
                .Append(" ,SUBCTGCODE ")
                .Append(" ,SERVICECD ")
                .Append(" ,CRACTCATEGORY ")
                .Append(" ,CRACTCHARGDVS ")
                .Append(" ,PROMOTION_ID ")
                .Append(" ,REQCATEGORY ")
                .Append(" ,CRACTRESULT ")
                .Append(" ,SERVICENAME ")
                .Append(" ,CATEGORY ")
                .Append(" ,SERIESCODE ")
                .Append(" ,SERIESNAME ")
                .Append(" ,VCLREGNO ")
                .Append(" ,ACTUALTIME_END ")
                .Append(" ,BRANCH_ACTUAL ")
                .Append(" ,ACCOUNT_ACTUAL ")
                .Append(" ,CRRSLTID ")
                .Append(" ,NEXTACTIVEDATE ")
                .Append(" ,NEXTAPPOINTTIMEFLG ")
                .Append(" ,CRTIMES ")
                .Append(" ,PLANSTATUS ")
                .Append(" ,CREATEDATE ")
                .Append(" ,CREATEMODULE ")
                .Append(" ,CREATEACCOUNT ")
                .Append(" ,UPDATEDATE ")
                .Append(" ,UPDATEMODULE ")
                .Append(" ,UPDATEACCOUNT ")
                .Append(" ,CRCUSTID ")
                .Append(" ,CUSTOMERCLASS ")
                .Append(" ) ")
                .Append(" SELECT ")
                .Append("  A.DLRCD ")
                .Append(" ,A.STRCD ")
                .Append(" ,A.FLLWUPBOX_SEQNO ")
                .Append(" ,A.CRPLAN_ID ")
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN A.UNTRADEDCSTID ")
                .Append("  ELSE ")
                .Append("       A.INSDID ")
                .Append("  END AS INSDID ")
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN B.NEWCUST_VIN ")
                .Append("  ELSE ")
                .Append("       A.VIN ")
                .Append("  END AS VIN ")
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN A.VCLSEQNO ")
                .Append("  END AS NEWVCLSEQNO ")
                .Append(" ,A.BFAFDVS ")
                .Append(" ,A.CRDVSID ")
                .Append(" ,A.MEMKIND ")
                .Append(" ,A.PLANDVS ")
                .Append(" ,A.SUBCTGCODE ")
                .Append(" ,A.SERVICECD ")
                .Append(" ,A.CRACTCATEGORY ")
                .Append(" ,A.CRACTCHARGDVS ")
                .Append(" ,A.PROMOTION_ID ")
                .Append(" ,A.REQCATEGORY ")
                .Append(" ,A.CRACTRESULT ")
                .Append(" ,CASE WHEN A.PROMOTION_ID IS NULL THEN ")
                .Append("            A.SUBCTGORGNAME ")
                .Append("  ELSE ")
                .Append("       C.PROMOTIONNAME ")
                .Append("  END AS SERVICENAME ")
                If String.IsNullOrEmpty(strcategory) Then
                    .Append(" ,' ' AS CATEGORY")
                Else
                    .Append(" ,:CATEGORY AS CATEGORY")
                End If
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN ")
                .Append("            B.NEWCUST_SERIESCODE ")
                .Append("       ELSE ")
                .Append("            A.SERIESCODE ")
                .Append("  END AS SERIESCODE ")
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN ")
                .Append("            B.NEWCUST_SERIESNAME ")
                .Append("       ELSE ")
                .Append("            A.SERIESNAME ")
                .Append("  END AS SERIESNAME ")
                .Append(" ,CASE WHEN A.MEMKIND = '3' THEN ")
                .Append("            B.NEWCUST_VCLREGNO ")
                .Append("       ELSE ")
                .Append("            A.VCLREGNO ")
                .Append("  END AS VCLREGNO ")

                'If String.IsNullOrEmpty(strcrenddate) Then
                '.Append(" ,NULL AS ACTUALTIME_END ")
                'Else
                '.Append(" ,TO_DATE(:ACTUALTIME_END,'YYYY/MM/DD HH24:MI:SS') AS ACTUALTIME_END ")
                .Append(" ,:ACTUALTIME_END AS ACTUALTIME_END")
                'End If

                .Append(" ,:BRANCH_ACTUAL AS BRANCH_ACTUAL ")
                .Append(" ,:ACCOUNT_ACTUAL AS ACCOUNT_ACTUAL ")
                If String.IsNullOrEmpty(Trim(strcrresult)) Then
                    .Append(" ,NULL AS CRRSLTID ")
                Else
                    .Append(" ,:CRRSLTID AS CRRSLTID ")
                End If
                If String.Equals(strplanstatus, "1") Then
                    '完了済み計画は次回分のデータをNULL
                    .Append(" ,NULL AS NEXTACTIVEDATE ")
                    .Append(" ,'0' AS NEXTAPPOINTTIMEFLG ")
                    .Append(" ,NULL AS CRTIMES ")
                    .Append(" ,:PLANSTATUS AS PLANSTATUS ")
                Else
                    .Append(" ,A.CRACTIVEDATE AS NEXTACTIVEDATE ")
                    .Append(" ,A.APPOINTTIMEFLG AS NEXTAPPOINTTIMEFLG ")
                    .Append(" ,(A.CRDVSID - 3) AS CRTIMES ")
                    .Append(" ,'0' AS PLANSTATUS ")
                End If
                .Append(" ,SYSDATE AS CREATEDATE ")
                .Append(" ,:CREATEMODULE AS CREATEMODULE ")
                .Append(" ,:CREATEACCOUNT AS CREATEACCOUNT ")
                .Append(" ,SYSDATE AS UPDATEDATE ")
                .Append(" ,:UPDATEMODULE AS UPDATEMODULE ")
                .Append(" ,:UPDATEACCOUNT AS UPDATEACCOUNT ")
                .Append(" ,A.CRCUSTID ")
                .Append(" ,A.CUSTOMERCLASS ")
                .Append(" FROM TBL_FLLWUPBOX A ")
                .Append(" ,TBL_FLLWUPBOXNEWCST B ")
                .Append(" ,TBL_CRPROMOTION C ")
                .Append(" WHERE ")
                .Append("     A.DLRCD = B.DLRCD (+) ")
                .Append(" AND A.STRCD = B.STRCD (+) ")
                .Append(" AND A.FLLWUPBOX_SEQNO = B.FLLWUPBOX_SEQNO (+) ")
                .Append(" AND A.PROMOTION_ID = C.PROMOTION_ID (+) ")
                .Append(" AND A.DLRCD = :DLRCD ")
                .Append(" AND A.STRCD = :STRCD ")
                .Append(" AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            End With
            Using query As New DBUpdateQuery("SC3080203_045")
                query.CommandText = sql.ToString()
                If String.IsNullOrEmpty(strcategory) = False Then
                    query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                End If
                'If String.IsNullOrEmpty(strcrenddate) = False Then
                query.AddParameterWithTypeValue("ACTUALTIME_END", OracleDbType.Date, actualtimeend)
                'End If
                query.AddParameterWithTypeValue("BRANCH_ACTUAL", OracleDbType.Char, branchactual)
                query.AddParameterWithTypeValue("ACCOUNT_ACTUAL", OracleDbType.Char, accountactual)
                If String.IsNullOrEmpty(Trim(strcrresult)) = False Then
                    query.AddParameterWithTypeValue("CRRSLTID", OracleDbType.Char, strcrresult)
                End If
                If String.Equals(strplanstatus, "1") Then
                    query.AddParameterWithTypeValue("PLANSTATUS", OracleDbType.Char, planstatus)
                End If
                query.AddParameterWithTypeValue("CREATEMODULE", OracleDbType.Char, createmodule)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Char, createaccount)
                query.AddParameterWithTypeValue("UPDATEMODULE", OracleDbType.Char, updatemodule)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, updateaccount)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 046.その他計画(Follow-up)更新(活動結果登録時)　(移行済み)
        ''' </summary>
        ''' <param name="planstatus"></param>
        ''' <param name="cractresult"></param>
        ''' <param name="beforecractresult"></param>
        ''' <param name="cractresultflg"></param>
        ''' <param name="category"></param>
        ''' <param name="crenddate"></param>
        ''' <param name="crresult"></param>
        ''' <param name="crdvs"></param>
        ''' <param name="actbranch"></param>
        ''' <param name="accountactual"></param>
        ''' <param name="account"></param>
        ''' <param name="nextdate"></param>
        ''' <param name="timeflg"></param>
        ''' <param name="moduleid"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateOtherPlanFllwRslt(ByVal planstatus As String, ByVal cractresult As String, ByVal beforecractresult As String,
                                                ByVal cractresultflg As String, ByVal category As String, ByVal crenddate As Date,
                                                ByVal crresult As String, ByVal crdvs As Long, ByVal actbranch As String,
                                                ByVal accountactual As String, ByVal account As String, ByVal nextdate As String,
                                                ByVal timeflg As String, ByVal moduleid As String, ByVal dlrcd As String,
                                                ByVal strcd As String, ByVal fllwupboxseqno As Long) As Integer

            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3080203_046 */ ")
                .Append("  TBL_OTHERPLAN_FLLW ")
                .Append(" SET ")
                If String.Equals(planstatus, "1") Then
                    .Append(" CRDVSID = :CRDVS ")
                Else
                    .Append(" CRDVSID = (:CRDVS + 1) ")
                End If
                ''完了する計画は更新しない
                If String.Equals(planstatus, "1") = False Then
                    ''HOT/PROSPECTはカテゴリ更新
                    If String.Equals(cractresult, "1") Or String.Equals(cractresult, "2") Then
                        .Append(" ,CRACTRESULT = :CRACTRESULT ")
                        'category = mtdgetcategory(cractresult, cractcategory, promotion_id, reqcategory)
                        .Append(" ,CATEGORY = :CATEGORY ")
                        ''HOT/PROSPECTからREQUEST、WALK-INに戻す場合はカテゴリ更新
                    ElseIf String.Equals(cractresult, "4") And (String.Equals(beforecractresult, "1") Or String.Equals(beforecractresult, "2")) Then
                        If String.Equals(cractresultflg, "1") Then
                            .Append(" ,CRACTRESULT = :CRACTRESULT ")
                            'category = mtdgetcategory(cractresult, cractcategory, promotion_id, reqcategory)
                            If String.IsNullOrEmpty(category) Then
                                .Append(" ,CATEGORY = ' ' ")
                            Else
                                .Append(" ,CATEGORY = :CATEGORY ")
                            End If
                        End If
                    Else
                        .Append(" ,CRACTRESULT = :CRACTRESULT ")
                    End If
                Else
                    .Append(" ,CRACTRESULT = :CRACTRESULT ")
                End If
                'If String.IsNullOrEmpty(crenddate) Then
                '    .Append(" ,ACTUALTIME_END = NULL ")
                'Else
                .Append(" ,ACTUALTIME_END = :CRENDDATE ")
                '  End If
                .Append(" ,BRANCH_ACTUAL = :ACTBRANCH ")
                .Append(" ,ACCOUNT_ACTUAL = :ACCOUNT_ACTUAL ")
                If String.IsNullOrEmpty(Trim(crresult)) Then
                    .Append(" ,CRRSLTID = NULL ")
                Else
                    .Append(" ,CRRSLTID = :CRRESULT ")
                End If
                If String.Equals(planstatus, "1") Then
                    .Append(" ,NEXTACTIVEDATE = NULL ")
                    .Append(" ,NEXTAPPOINTTIMEFLG = '0' ")
                    .Append(" ,CRTIMES = NULL ")
                    .Append(" ,PLANSTATUS = '1' ")
                Else
                    .Append(" ,NEXTACTIVEDATE = TO_DATE(:NEXTDATE,'YYYY/MM/DD HH24:MI:SS')  ")
                    .Append(" ,NEXTAPPOINTTIMEFLG = :TIMEFLG ")
                    .Append(" ,CRTIMES = (:CRDVS - 2) ")
                    .Append(" ,PLANSTATUS = '0' ")
                End If
                .Append(" ,UPDATEDATE = SYSDATE ")
                .Append(" ,UPDATEMODULE = :MODULEID ")
                .Append(" ,UPDATEACCOUNT = :ACCOUNT ")
                .Append(" WHERE ")
                .Append("     DLRCD = :DLRCD ")
                .Append(" AND STRCD = :STRCD ")
                .Append(" AND FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
            End With
            Using query As New DBUpdateQuery("SC3080203_046")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CRDVS", OracleDbType.Int64, crdvs)
                If String.Equals(planstatus, "1") = False Then
                    If String.Equals(cractresult, "1") Or String.Equals(cractresult, "2") Then
                        query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                        query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                    ElseIf String.Equals(cractresult, "4") And (String.Equals(beforecractresult, "1") Or String.Equals(beforecractresult, "2")) Then
                        If String.Equals(cractresultflg, "1") Then
                            query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                            If String.IsNullOrEmpty(category) Then
                            Else
                                query.AddParameterWithTypeValue("CATEGORY", OracleDbType.Char, category)
                            End If
                        End If
                    Else
                        query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                    End If
                Else
                    query.AddParameterWithTypeValue("CRACTRESULT", OracleDbType.Char, cractresult)
                End If
                'If String.IsNullOrEmpty(crenddate) = False Then
                query.AddParameterWithTypeValue("CRENDDATE", OracleDbType.Date, crenddate)
                'End If
                query.AddParameterWithTypeValue("ACTBRANCH", OracleDbType.Char, actbranch)
                query.AddParameterWithTypeValue("ACCOUNT_ACTUAL", OracleDbType.Char, accountactual)
                If String.IsNullOrEmpty(Trim(crresult)) = False Then
                    query.AddParameterWithTypeValue("CRRESULT", OracleDbType.Char, crresult)
                End If
                If String.Equals(planstatus, "1") = False Then
                    query.AddParameterWithTypeValue("NEXTDATE", OracleDbType.Char, nextdate)
                    query.AddParameterWithTypeValue("TIMEFLG", OracleDbType.Char, timeflg)
                End If
                query.AddParameterWithTypeValue("MODULEID", OracleDbType.Char, moduleid)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 047.Follow-upBox結果の最大SeqNo+1を取得　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwRsltSeq(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203SequenceDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SequenceDataTable)("SC3080203_047")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_047 */ ")
                    .Append("    NVL(MAX(SEQNO),0)+1 AS SEQ ")
                    .Append("FROM ")
                    .Append("    tbl_FLLWUPBOXRSLT ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 048.Follow-up Box結果追加(活動結果登録)　(移行済み)
        ''' </summary>
        ''' <param name="insuranceflg"></param>
        ''' <param name="actualtimestart"></param>
        ''' <param name="thistimecractresult"></param>
        ''' <param name="selecteddvs"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupseq"></param>
        ''' <param name="crplanid"></param>
        ''' <param name="bfafdvs"></param>
        ''' <param name="crdvsid"></param>
        ''' <param name="insdid"></param>
        ''' <param name="seriescode"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="uid"></param>
        ''' <param name="vclregno"></param>
        ''' <param name="subctgcode"></param>
        ''' <param name="promotionid"></param>
        ''' <param name="crrsltid"></param>
        ''' <param name="plandvs"></param>
        ''' <param name="actdate"></param>
        ''' <param name="action"></param>
        ''' <param name="accountplan"></param>
        ''' <param name="servicecd"></param>
        ''' <param name="subctgorgname"></param>
        ''' <param name="subctgorgnameex"></param>
        ''' <param name="stallreserveid"></param>
        ''' <param name="stalldlrcd"></param>
        ''' <param name="stallstrcd"></param>
        ''' <param name="recid"></param>
        ''' <param name="cmshislinkid"></param>
        ''' <param name="rsltseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertFllwupboxCRHisRslt(ByVal insuranceflg As String, ByVal actualtimestart As String, ByVal thistimecractresult As String,
                                                  ByVal selecteddvs As String, ByVal dlrcd As String, ByVal strcd As String,
                                                  ByVal fllwupseq As Long, ByVal crplanid As Nullable(Of Long), ByVal bfafdvs As String,
                                                  ByVal crdvsid As String, ByVal insdid As String,
                                                  ByVal seriescode As String, ByVal seriesname As String, ByVal uid As String,
                                                  ByVal vclregno As String, ByVal subctgcode As String, ByVal promotionid As Nullable(Of Long),
                                                  ByVal crrsltid As String, ByVal plandvs As String, ByVal actdate As String, ByVal action As String,
                                                  ByVal accountplan As String, ByVal servicecd As String, ByVal subctgorgname As String,
                                                  ByVal subctgorgnameex As String, ByVal stallreserveid As Nullable(Of Long), ByVal stalldlrcd As String,
                                                  ByVal stallstrcd As String, ByVal recid As Nullable(Of Long), ByVal cmshislinkid As Nullable(Of Long),
                                                  ByVal rsltseqno As Long, ByVal strActDateTo As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT ")
                .Append("INTO /* SC3080203_048 */ ")
                .Append("    TBL_FLLWUPBOXCRHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    IDENTITYNO, ")
                .Append("    SEQNO, ")
                .Append("    INSDID, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    CALLDATE, ")
                .Append("    ACCOUNT, ")
                .Append("    REGNO, ")
                .Append("    SUBCTGCODE, ")
                .Append("    PROMOTION_ID, ")
                .Append("    CRDVS, ")
                .Append("    ACTIVITYRESULT, ")
                .Append("    PLANDVS, ")
                .Append("    ACTUALTIME_END, ")
                .Append("    ACTDATE, ")
                .Append("    METHOD, ")
                .Append("    ACTION, ")
                .Append("    ACTIONTYPE, ")
                .Append("    HOACCOUNT, ")
                .Append("    BRNCHACCOUNT, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    ACTIONCD, ")
                .Append("    FLLWUPBOXRSLT_SEQNO ")
                .Append(") ")
                .Append(" VALUES ( ")
                .Append(" :DLRCD ")                     ''DLRCD
                .Append(",:STRCD ")                     ''STRCD
                .Append(",TO_NUMBER(:FLLWUPSEQ) ")      ''FLLWUPBOX_SEQNO
                .Append(",TO_NUMBER(:CRPLAN_ID) ")      ''CRPLAN_ID
                .Append(",:BFAFDVS ")                       ''BFAFDVS
                .Append(",TO_NUMBER(:CRDVSID) ")            ''CRDVSID
                .Append(",(SELECT NVL(MAX(IDENTITYNO),0) + 1 FROM TBL_FLLWUPBOXCRHIS WHERE DLRCD = :DLRCD AND STRCD = :STRCD AND FLLWUPBOX_SEQNO = TO_NUMBER(:FLLWUPSEQ)) ")  ''IDENTITYNO
                .Append(",1 ")                          ''SEQNO
                ''INSDID
                If String.Equals(insuranceflg, "1") Then
                    .Append(",' ' ")
                Else
                    .Append(",:INSDID ")
                End If
                ''SERIESCODE,SERIESNAME
                If String.IsNullOrEmpty(Trim(seriescode)) = False Then
                    .Append(",:strSeriesCode ")
                    .Append(",:strSeriesName ")
                Else
                    .Append(",' ' ")
                    .Append(",' ' ")
                End If
                .Append(",SYSDATE ") ''CALLDATE
                .Append(",:ACCOUNT ")                           ''ACCOUNT
                .Append(",:VCLREGNO ")                  ''REGNO
                .Append(",:SUBCTGCODE ")                    ''SUBCTGCODE
                .Append(",TO_NUMBER(:PROMOTION_ID) ")       ''PROMOTION_ID
                .Append(",TO_NUMBER(:strCRRSLTID) ")        ''CRDVS
                .Append(",TO_NUMBER(:THISTIME_CRACTRESULT) ")   ''ACTIVITYRESULT
                .Append(",:PLANDVS ")                       ''PLANDVS
                ''ACTUALTIME_END
                If String.IsNullOrEmpty(actualtimestart) = False Then
                    .Append(",TO_DATE(:strActDateTo,'YYYY/MM/DD HH24:MI:SS') ")
                Else
                    .Append(",NULL ")
                End If
                If thistimecractresult = CONSTCRACTRSLTSUCCESS Or thistimecractresult = CONSTCRACTRSLTGIVEUP Then
                    '' Success/Give-up
                    .Append(",TO_DATE(:strActDate,'YYYY/MM/DD') ")  ''ACTDATE
                Else
                    .Append(",SYSDATE ") ''ACTDATE
                End If
                .Append(",' ' ")                                                                ''METHOD
                ''ACTION, ACTIONTYPE
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTHOT
                        .Append(",:ACTION ")
                        .Append(",'5' ")
                    Case CONSTCRACTRSLTPROSPECT
                        .Append(",:ACTION ")
                        .Append(",'5' ")
                    Case CONSTCRACTRSLTSUCCESS
                        .Append(",:ACTION ")
                        .Append(",'6' ")
                    Case CONSTCRACTRSLTCONTINUE
                        .Append(",:ACTION ")
                        .Append(",'5' ")
                    Case CONSTCRACTRSLTGIVEUP
                        .Append(",:ACTION ")
                        .Append(",'5' ")
                End Select
                .Append(",NULL ")                                                               ''HOACCOUNT
                .Append(",:ACCOUNT_PLAN ")              ''BRNCHACCOUNT
                .Append(",SYSDATE ")                                                            ''CREATEDATE
                .Append(",SYSDATE ")                                                            ''UPDATEDATE
                .Append(",:SERVICECD ")                 ''SERVICECD
                .Append(",NVL(TRIM(:strSUBCTGORGNAME), ' ') ") ''SUBCTGORGNAME
                .Append(",:SUBCTGORGNAME_EX ")         ''SUBCTGORGNAME_EX
                .Append(",TO_NUMBER(:strSTALL_RESERVEID) ") ''STALL_REZID
                .Append(",:strSTALL_DLRCD ")                ''STALL_DLRCD
                .Append(",:strSTALL_STRCD ")                ''STALL_STRCD
                .Append(",TO_NUMBER(:strRECID) ")           ''REC_ID
                .Append(",TO_NUMBER(:strCMSHISLINKID) ")    ''CMS_HISLINKID
                ''ACTIONCD
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTHOT
                        .Append(",'D06' ")
                    Case CONSTCRACTRSLTPROSPECT
                        .Append(",'D05' ")
                    Case CONSTCRACTRSLTSUCCESS
                        .Append(",'D01' ")
                    Case CONSTCRACTRSLTCONTINUE
                        Select Case selecteddvs
                            Case CONSTSELECTTOWALK
                                .Append(",'D08' ")
                            Case Else
                                .Append(",'D07' ")
                        End Select
                    Case CONSTCRACTRSLTGIVEUP
                        .Append(",'D02' ")
                    Case Else   ''存在しないが、Sql Errorになら無い為設定
                        .Append(",'D09' ")
                End Select
                .Append(",TO_NUMBER(:strRsltSeqno) ")       ''FLLWUPBOXRSLT_SEQNO
                .Append(" ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_048")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPSEQ", OracleDbType.Char, fllwupseq)
                query.AddParameterWithTypeValue("CRPLAN_ID", OracleDbType.Int64, crplanid)
                query.AddParameterWithTypeValue("BFAFDVS", OracleDbType.Char, bfafdvs)
                query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Char, crdvsid)
                If String.Equals(insuranceflg, "1") = False Then
                    query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                End If
                If String.IsNullOrEmpty(Trim(seriescode)) = False Then
                    query.AddParameterWithTypeValue("strSeriesCode", OracleDbType.Char, seriescode)
                    query.AddParameterWithTypeValue("strSeriesName", OracleDbType.Char, seriesname)
                End If
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, uid)
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Char, vclregno)
                query.AddParameterWithTypeValue("SUBCTGCODE", OracleDbType.Char, subctgcode)
                query.AddParameterWithTypeValue("PROMOTION_ID", OracleDbType.Int64, promotionid)
                query.AddParameterWithTypeValue("strCRRSLTID", OracleDbType.Char, crrsltid)
                query.AddParameterWithTypeValue("THISTIME_CRACTRESULT", OracleDbType.Char, thistimecractresult)
                query.AddParameterWithTypeValue("PLANDVS", OracleDbType.Char, plandvs)
                If String.IsNullOrEmpty(actualtimestart) = False Then
                    query.AddParameterWithTypeValue("strActDateTo", OracleDbType.Char, strActDateTo)
                End If
                If thistimecractresult = CONSTCRACTRSLTSUCCESS Or thistimecractresult = CONSTCRACTRSLTGIVEUP Then
                    query.AddParameterWithTypeValue("strActDate", OracleDbType.Char, actdate)
                End If
                query.AddParameterWithTypeValue("ACTION", OracleDbType.Char, action)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, accountplan)
                query.AddParameterWithTypeValue("SERVICECD", OracleDbType.Char, servicecd)
                query.AddParameterWithTypeValue("strSUBCTGORGNAME", OracleDbType.Char, subctgorgname)
                query.AddParameterWithTypeValue("SUBCTGORGNAME_EX", OracleDbType.Char, subctgorgnameex)
                query.AddParameterWithTypeValue("strSTALL_RESERVEID", OracleDbType.Int64, stallreserveid)
                query.AddParameterWithTypeValue("strSTALL_DLRCD", OracleDbType.Char, stalldlrcd)
                query.AddParameterWithTypeValue("strSTALL_STRCD", OracleDbType.Char, stallstrcd)
                query.AddParameterWithTypeValue("strRECID", OracleDbType.Int64, recid)
                query.AddParameterWithTypeValue("strCMSHISLINKID", OracleDbType.Int64, cmshislinkid)
                query.AddParameterWithTypeValue("strRsltSeqno", OracleDbType.Int64, rsltseqno)

                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 049.顧客メモ履歴登録 (移行済み)
        ''' </summary>
        ''' <param name="customerclass"></param>
        ''' <param name="cractname"></param>
        ''' <param name="vclinforegistflg"></param>
        ''' <param name="custsegment"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="account"></param>
        ''' <param name="memo"></param>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertCustMemohis(ByVal customerclass As String, ByVal cractname As String, ByVal vclinforegistflg As String,
                                           ByVal custsegment As String, ByVal crcustid As String, ByVal dlrcd As String,
                                           ByVal strcd As String, ByVal account As String, ByVal memo As String,
                                           ByVal originalid As String, ByVal vin As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT ")
                .Append("INTO /*SC3080203_049*/ ")
                .Append("    TBL_CUSTMEMOHIS ")
                .Append("( ")
                .Append("    CUSTMEMOHIS_SEQNO, ")
                .Append("    INSDID, ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    ACCOUNT, ")
                .Append("    MEMO, ")
                .Append("    CRACTNAME, ")
                .Append("    TARGETVCL, ")
                .Append("    VCLREGNO, ")
                .Append("    DELFLG, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    UPDATEACCOUNT, ")
                .Append("    CRCUSTNAME, ")
                .Append("    CRVIN, ")
                .Append("    CUSTSEGMENT, ")
                .Append("    CUSTOMERCLASS, ")
                .Append("    CRCUSTID ")
                .Append(") ")
                .Append(" VALUES (")
                .Append(" TO_NUMBER(SEQ_CUSTMEMOHIS_SEQNO.NEXTVAL), ")
                ''顧客分類が所有者か、未取引客の場合
                If customerclass = CONSTCUSTOMERCLASSOWNER Then
                    .Append(" :strCRCUSTID, ")
                Else
                    .Append(" :strORIGINALID, ")
                End If
                .Append(" :strDLRCD, ")
                .Append(" :strSTRCD, ")
                .Append(" :strACCOUNT, ")
                .Append(" :strMEMO, ")
                ''活動名が空文字の場合
                If String.IsNullOrEmpty(Trim(cractname)) Then
                    .Append(" NULL, ")
                Else
                    .Append(" :strCRACTNAME, ")
                End If
                ''車両情報登録フラグがTrue
                If String.Equals(vclinforegistflg, "1") Then
                    .Append(" (SELECT SERIESNM FROM TBLORG_VCLINFO WHERE ORIGINALID = :strORIGINALID AND VIN = :strVIN),  ")
                    .Append(" NVL((SELECT VCLREGNO FROM TBLORG_VCLINFO WHERE ORIGINALID = :strORIGINALID AND VIN = :strVIN), NULL) , ")
                Else
                    .Append(" NULL, ")
                    .Append(" NULL, ")
                End If
                .Append(" '0', ")
                .Append(" SYSDATE, ")
                .Append(" SYSDATE, ")
                .Append(" :strACCOUNT, ")
                ''顧客分類が所有者か、未取引客の場合
                If customerclass = CONSTCUSTOMERCLASSOWNER Then
                    ''顧客種別が自社客の場合
                    If custsegment = CONSTCUSTSEGMENTCUSTOMER Then
                        .Append(" (SELECT NAME FROM TBLORG_CUSTOMER WHERE ORIGINALID = :strCRCUSTID), ")
                        ''顧客種別が見取引客の場合
                    ElseIf custsegment = CONSTCUSTSEGMENTNEWCSTOMER Then
                        .Append(" (SELECT NAME FROM TBL_NEWCUSTOMER WHERE CSTID = :strCRCUSTID), ")
                    End If
                Else
                    .Append(" (SELECT NAME FROM TBLORG_SUBCUSTOMER WHERE SUBCUSTID = :strCRCUSTID), ")
                End If
                ''車両情報登録フラグがTrue
                If String.Equals(vclinforegistflg, "1") Then
                    .Append(" :strVIN, ")
                Else
                    .Append(" NULL, ")
                End If
                .Append(" :strCUSTSEGMENT, ")
                .Append(" :strCUSTOMERCLASS, ")
                .Append(" :strCRCUSTID ")
                .Append(" ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_049")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("strCRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("strDLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("strSTRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("strACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("strMEMO", OracleDbType.Char, memo)
                If String.IsNullOrEmpty(Trim(cractname)) = False Then
                    query.AddParameterWithTypeValue("strCRACTNAME", OracleDbType.Char, cractname)
                End If
                If String.Equals(vclinforegistflg, "1") Then
                    query.AddParameterWithTypeValue("strORIGINALID", OracleDbType.Char, originalid)
                    query.AddParameterWithTypeValue("strVIN", OracleDbType.Char, vin)
                End If
                query.AddParameterWithTypeValue("strCUSTSEGMENT", OracleDbType.Char, custsegment)
                query.AddParameterWithTypeValue("strCUSTOMERCLASS", OracleDbType.Char, customerclass)
                query.AddParameterWithTypeValue("strCRCUSTID", OracleDbType.Char, crcustid)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 050.トータル履歴追加　(移行済み)
        ''' </summary>
        ''' <param name="insdid"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="untradedcstid"></param>
        ''' <param name="vin"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="totalstatus"></param>
        ''' <param name="promotionid"></param>
        ''' <param name="servicenm"></param>
        ''' <param name="accountname"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InsertTotalHisRslt(ByVal insdid As String, ByVal dlrcd As String, ByVal strcd As String,
                                            ByVal untradedcstid As String, ByVal vin As String, ByVal seriesname As String,
                                            ByVal totalstatus As String, ByVal promotionid As Nullable(Of Long), ByVal servicenm As String,
                                            ByVal accountname As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT ")
                .Append("INTO /* SC3080203_050 */ ")
                .Append("    TBL_TOTALHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    INSDID, ")
                .Append("    SEQNO, ")
                .Append("    CONTACTDATE, ")
                .Append("    CATEGORYID, ")
                .Append("    CATEGORYDVSID, ")
                .Append("    VIN, ")
                .Append("    SERIESNAME, ")
                .Append("    STATUS, ")
                .Append("    APPOINTMENTDATE, ")
                .Append("    PROMOTION_NM, ")
                .Append("    SERVICE_NM, ")
                .Append("    MILEAGE, ")
                .Append("    SERVICEINDATE, ")
                .Append("    SNDMAILID, ")
                .Append("    RECMAILID, ")
                .Append("    ACCOUNT_NM, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID ")
                .Append(") ")
                .Append("  VALUES ( ")
                .Append(" :DLRCD ")                             ''DLRCD
                .Append(",:STRCD ")                             ''STRCD
                If String.IsNullOrEmpty(Trim(insdid)) = False Then                      ''INSDID
                    .Append(",:INSDID ")
                Else
                    .Append(",:UNTRADEDCSTID ")
                End If
                .Append(",SEQ_TOTALHIS_SEQNO.NEXTVAL ")         ''SEQNO
                .Append(",SYSDATE ")   ''CONTACTDATE
                .Append(",'5' ")                                ''CATEGORYID
                .Append(",NULL ")                               ''CATEGORYDVSID
                .Append(",:VIN ")                               ''VIN
                .Append(",:SERIESNAME ")                       ''SERIESNAME
                .Append(",:TOTALSTATUS ")                       ''STATUS
                .Append(",NULL ")                               ''APPOINTMENTDATE
                .Append(",(SELECT PROMOTIONNAME FROM TBL_CRPROMOTION WHERE PROMOTION_ID = :PROMOTION_ID) ")
                .Append(",NVL(TRIM(:STRSERVICE_NM), ' ') ")                ''SERVICE_NM
                .Append(",NULL ")                                           ''MILEAGE
                .Append(",NULL ")                                           ''SERVICEINDATE
                .Append(",NULL ")                                           ''SNDMAILID
                .Append(",NULL ")                                           ''RECMAILID
                .Append(",(SELECT USERNAME FROM TBL_USERS WHERE ACCOUNT = :ACCOUNT_NAME) ") 'ACCOUNT_NM ")                                  ''ACCOUNT_NM
                .Append(",SYSDATE ")                                        ''CREATEDATE
                .Append(",SYSDATE ")                                        ''UPDATEDATE
                .Append(",NULL ")                                           ''STALL_REZID
                .Append(",NULL ")                                           ''STALL_DLRCD
                .Append(",NULL ")                                           ''STALL_STRCD
                .Append(",NULL ")                                           ''REC_ID
                .Append(",NULL ")                                           ''CMS_HISLINKID
                .Append(" ) ")
            End With
            Using query As New DBUpdateQuery("SC3080203_050")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                If String.IsNullOrEmpty(Trim(insdid)) = False Then
                    query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                Else
                    query.AddParameterWithTypeValue("UNTRADEDCSTID", OracleDbType.Char, untradedcstid)
                End If
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
                query.AddParameterWithTypeValue("TOTALSTATUS", OracleDbType.Char, totalstatus)
                query.AddParameterWithTypeValue("PROMOTION_ID", OracleDbType.Int64, promotionid)
                query.AddParameterWithTypeValue("STRSERVICE_NM", OracleDbType.Char, servicenm)
                query.AddParameterWithTypeValue("ACCOUNT_NAME", OracleDbType.Char, accountname)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 051.TBLFLREQUEST追加 (移行済み)
        ''' </summary>
        ''' <param name="thistimecractresult"></param>
        ''' <param name="cractlimitdate"></param>
        ''' <param name="nextactivitydatetime"></param>
        ''' <param name="insuranceflg"></param>
        ''' <param name="seriescode"></param>
        ''' <param name="successkind"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="successdate"></param>
        ''' <param name="memo"></param>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupseq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateFLRequest(ByVal thistimecractresult As String, ByVal cractlimitdate As String, ByVal nextactivitydatetime As String,
                                        ByVal insuranceflg As String, ByVal seriescode As String, ByVal successkind As String,
                                        ByVal seriesname As String, ByVal successdate As String, ByVal memo As String,
                                        ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupseq As Long, ByVal fllwuptyp As String,
                                        ByVal giveupdate As String) As Integer

            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* SC3080203_051 */ ")
                .Append("    TBL_FLREQUEST ")
                .Append("SET ")
                .Append("    UPDATEDATE = SYSDATE, ")
                .Append("    LSTFLDATE = SYSDATE ")
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTHOT
                        .Append(",ACTRESULT = '6' ")
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                            End If
                        End If
                    Case CONSTCRACTRSLTPROSPECT
                        .Append(",ACTRESULT = '7' ")
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                            End If
                        End If
                    Case CONSTCRACTRSLTSUCCESS
                        .Append(",ACTRESULT = '1' ")
                        .Append(",STTSCODE = '18' ")
                        Select Case fllwuptyp
                            Case CONSTFLLWUPPERIODICAL
                                If String.Equals(insuranceflg, "1") Then
                                    .Append(",SUCCESSVHCL = '4' ")  ''other
                                Else
                                    .Append(",SUCCESSVHCL = '3' ")  ''service
                                End If
                            Case CONSTFLLWUPPROMOTION
                                .Append(",SUCCESSVHCL = '4' ")  ''other
                            Case Else
                                .Append(",SUCCESSVHCL = :strSuccessKind ")
                        End Select
                        If String.IsNullOrEmpty(Trim(seriescode)) = False Then
                            .Append(",SUCCESSCARCD = :strSeriesCode ")
                            .Append(",SUCCESSCARNM = ' ' ")
                        Else
                            .Append(",SUCCESSCARCD = ' ' ")
                            .Append(",SUCCESSCARNM = :strSeriesName ")
                        End If
                        .Append(",SUCCESSDATE = TRUNC(TO_DATE(:strSuccessDate,'YYYY/MM/DD HH24:MI:SS')) ")
                    Case CONSTCRACTRSLTCONTINUE
                        .Append(",ACTRESULT = '2' ")
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                .Append(",DEADLINEDATE = TO_DATE(:strnextactivitydatetime,'YYYY/MM/DD HH24:MI:SS') ")
                            End If
                        End If
                    Case CONSTCRACTRSLTGIVEUP
                        .Append(",ACTRESULT = '3' ")
                        .Append(",STTSCODE = '18' ")
                        .Append(",GIVEUPDATE = TRUNC(TO_DATE(:GIVEUPDATE,'YYYY/MM/DD HH24:MI:SS')) ")
                        .Append(",REASONGIVEUP = :strMemo ")
                End Select
                .Append(" WHERE DLRCD = :DLRCD ")
                .Append(" AND STRCD = :STRCD ")
                .Append(" AND FLLWUPBOX_SEQNO = TO_NUMBER(:FLLWUPSEQ) ")

            End With
            Using query As New DBUpdateQuery("SC3080203_051")
                query.CommandText = sql.ToString()
                Select Case thistimecractresult
                    Case CONSTCRACTRSLTHOT
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                            End If
                        End If
                    Case CONSTCRACTRSLTPROSPECT
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                            End If
                        End If
                    Case CONSTCRACTRSLTSUCCESS
                        Select Case fllwuptyp
                            Case CONSTFLLWUPPERIODICAL
                            Case CONSTFLLWUPPROMOTION
                            Case Else
                                query.AddParameterWithTypeValue("strSuccessKind", OracleDbType.Char, successkind)
                        End Select
                        If String.IsNullOrEmpty(Trim(seriescode)) = False Then
                            query.AddParameterWithTypeValue("strSeriesCode", OracleDbType.Char, seriescode)
                        Else
                            query.AddParameterWithTypeValue("strSeriesName", OracleDbType.Char, seriesname)
                        End If
                        query.AddParameterWithTypeValue("strSuccessDate", OracleDbType.Char, successdate)
                    Case CONSTCRACTRSLTCONTINUE
                        If String.IsNullOrEmpty(cractlimitdate) = False Then
                            query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                        Else
                            If CDate(nextactivitydatetime) > CDate(cractlimitdate) Then
                                query.AddParameterWithTypeValue("strnextactivitydatetime", OracleDbType.Char, nextactivitydatetime)
                            End If
                        End If
                    Case CONSTCRACTRSLTGIVEUP
                        query.AddParameterWithTypeValue("strMemo", OracleDbType.NVarchar2, memo)
                        query.AddParameterWithTypeValue("GIVEUPDATE", OracleDbType.Char, giveupdate)
                End Select
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPSEQ", OracleDbType.Int64, fllwupseq)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 052.シリーズ取得(自社客)　(移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetVclinfo(ByVal originalid As String, ByVal vin As String) As SC3080203DataSet.SC3080203SeriesDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeriesDataTable)("SC3080203_052")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_052 */ ")
                    .Append("    SERIESCD AS SERIESCD, ")
                    .Append("    SERIESNM AS SERIESNM ")
                    .Append("FROM ")
                    .Append("    TBLORG_VCLINFO ")
                    .Append("WHERE ")
                    .Append("    ORIGINALID = :ORIGINALID AND ")
                    .Append("    VIN = :VIN ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 053.シリーズ取得(未取引客)　(移行済み)
        ''' </summary>
        ''' <param name="cstid"></param>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewcustVclinfo(ByVal cstid As String, ByVal seqno As Long) As SC3080203DataSet.SC3080203SeriesDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_053 */ ")
                .Append("    SERIESCODE AS SERIESCD, ")
                .Append("    SERIESNAME AS SERIESNM ")
                .Append("FROM ")
                .Append("    TBL_NEWCUSTOMERVCLRE ")
                .Append("WHERE ")
                .Append("    CSTID = :CSTID AND ")
                .Append("    SEQNO = :SEQNO ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeriesDataTable)("SC3080203_053")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 054.Follow-up Box選択車種取得(シリーズ)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="cntcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwSeries(ByVal dlrcd As String, ByVal strcd As String, ByVal cntcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwSeriesDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwSeriesDataTable)("SC3080203_054")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_054 */ ")
                    .Append("    E.SEQNO, ")
                    .Append("    E.SERIESCD, ")
                    .Append("    D.SERIESNM ")
                    .Append("FROM ")
                    .Append("    ( ")
                    .Append("    SELECT ")
                    .Append("        MIN(SEQNO) AS SEQNO, ")
                    .Append("        SERIESCD ")
                    .Append("    FROM ")
                    .Append("        TBL_FLLWUPBOX_SELECTED_SERIES ")
                    .Append("    WHERE ")
                    .Append("        DLRCD = :DLRCD AND ")
                    .Append("        STRCD = :STRCD AND ")
                    .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                    .Append("    GROUP BY ")
                    .Append("        SERIESCD ")
                    .Append("    ORDER BY ")
                    .Append("        SEQNO ")
                    .Append("    ) E, ")
                    .Append("    ( ")
                    .Append("    SELECT ")
                    .Append("        A.DLRCD, ")
                    .Append("        B.SERIESCD, ")
                    .Append("        B.SERIESNM ")
                    .Append("    FROM ")
                    .Append("        TBLM_DEALER A, ")
                    .Append("        TBLORG_SERIESMASTER B ")
                    '.Append("    WHERE ")
                    '.Append("        A.DLRCD = B.DLRCD AND ")
                    '.Append("        A.DLRCD = :DLRCD AND ")
                    '.Append("        A.DELFLG = '0' AND ")
                    '.Append("        A.CNTCD = :CNTCD OR ")
                    '.Append("        (B.DLRCD = :DLRCD AND A.DLRCD = :DLRCD AND A.DELFLG = '0' AND A.CNTCD = :CNTCD AND NOT EXISTS ( ")
                    '.Append("                                                                                                      SELECT ")
                    '.Append("                                                                                                          1 ")
                    '.Append("                                                                                                      FROM ")
                    '.Append("                                                                                                          TBLORG_SERIESMASTER C ")
                    '.Append("                                                                                                      WHERE ")
                    '.Append("                                                                                                          C.DLRCD = A.DLRCD AND ")
                    '.Append("                                                                                                          C.SERIESCD = B.SERIESCD ")
                    '.Append("                                                                                                      )) ")
                    .Append("    WHERE ")
                    .Append("        A.DLRCD = B.DLRCD AND ")
                    .Append("        A.DLRCD = :DLRCD AND ")
                    .Append("        A.DELFLG = '0' AND ")
                    .Append("        A.CNTCD = :CNTCD OR ")
                    .Append("        (B.DLRCD = '00000'  ")
                    .Append("            AND A.DLRCD =  :DLRCD  ")
                    .Append("            AND A.DELFLG = '0'  ")
                    .Append("            AND A.CNTCD =  :CNTCD  ")
                    .Append("            AND NOT EXISTS (SELECT 1   ")
                    .Append("                              FROM TBLORG_SERIESMASTER C  ")
                    .Append("                              WHERE C.DLRCD = A.DLRCD  ")
                    .Append("                              AND C.SERIESCD = B.SERIESCD)) ")
                    .Append("    ) D ")
                    .Append("WHERE ")
                    .Append("    D.DLRCD = :DLRCD AND ")
                    .Append("    E.SERIESCD = D.SERIESCD ")
                    .Append("ORDER BY ")
                    .Append("    SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 055.Follow-up Box選択車種取得(モデル)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="cntcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwModel(ByVal dlrcd As String, ByVal strcd As String, ByVal cntcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwModelDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_055 */ ")
                .Append("    SEL.SEQNO, ")
                .Append("    SEL.SERIESCD, ")
                .Append("    SEL.SERIESNM, ")
                .Append("    SEL.MODELCD, ")
                .Append("    MDL.VCLMODEL_NAME ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        A.SEQNO, ")
                .Append("        A.SERIESCD, ")
                .Append("        A.MODELCD, ")
                .Append("        B.COMSERIESCD, ")
                .Append("        B.SERIESNM ")
                .Append("    FROM ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            MIN(SEQNO) AS SEQNO, ")
                .Append("            SERIESCD, ")
                .Append("            MODELCD ")
                .Append("        FROM ")
                .Append("            TBL_FLLWUPBOX_SELECTED_SERIES ")
                .Append("        WHERE ")
                .Append("            DLRCD = :DLRCD AND ")
                .Append("            STRCD = :STRCD AND ")
                .Append("            FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("        GROUP BY ")
                .Append("            SERIESCD, ")
                .Append("            MODELCD ")
                .Append("        ORDER BY ")
                .Append("            SEQNO ")
                .Append("        ) A, ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            A.DLRCD, ")
                .Append("            B.SERIESCD, ")
                .Append("            B.SERIESNM, ")
                .Append("            B.TOYOTABRAND, ")
                .Append("            B.IMAGEPATH, ")
                .Append("            B.COMSERIESCD, ")
                .Append("            B.DELFLG, ")
                .Append("            B.DELDATE, ")
                .Append("            B.CREATEDATE, ")
                .Append("            B.UPDATEDATE, ")
                .Append("            B.MAKERCD ")
                .Append("        FROM ")
                .Append("            TBLM_DEALER A, ")
                .Append("            TBLORG_SERIESMASTER B ")
                '.Append("        WHERE ")
                '.Append("            A.DLRCD = B.DLRCD AND ")
                '.Append("            A.DLRCD = :DLRCD AND ")
                '.Append("            A.DELFLG = '0' AND ")
                '.Append("            A.CNTCD = :CNTCD OR ")
                '.Append("            (B.DLRCD = :DLRCD AND A.DLRCD = :DLRCD AND A.DELFLG = '0' AND A.CNTCD = :CNTCD AND NOT EXISTS ( ")
                '.Append("                                                                                                          SELECT ")
                '.Append("                                                                                                              1 ")
                '.Append("                                                                                                          FROM ")
                '.Append("                                                                                                              TBLORG_SERIESMASTER C ")
                '.Append("                                                                                                          WHERE ")
                '.Append("                                                                                                              C.DLRCD = A.DLRCD AND ")
                '.Append("                                                                                                              C.SERIESCD = B.SERIESCD ")
                '.Append("                                                                                                          )) ")
                .Append("       WHERE ")
                .Append("           A.DLRCD = B.DLRCD AND ")
                .Append("           A.DLRCD = :DLRCD AND ")
                .Append("           A.DELFLG = '0' AND ")
                .Append("           A.CNTCD = :CNTCD OR ")
                .Append("           (B.DLRCD = '00000'  ")
                .Append("               AND A.DLRCD =  :DLRCD  ")
                .Append("               AND A.DELFLG = '0'  ")
                .Append("               AND A.CNTCD =  :CNTCD  ")
                .Append("               AND NOT EXISTS (SELECT 1   ")
                .Append("                                 FROM TBLORG_SERIESMASTER C  ")
                .Append("                                 WHERE C.DLRCD = A.DLRCD  ")
                .Append("                                 AND C.SERIESCD = B.SERIESCD)) ")
                .Append("        ) B ")
                .Append("    WHERE ")
                .Append("        B.DLRCD = :DLRCD AND ")
                .Append("        A.SERIESCD = B.SERIESCD ")
                .Append("    ) SEL, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        C.CAR_NAME_CD_AI21, ")
                .Append("        D.VCLMODEL_CODE, ")
                .Append("        D.VCLMODEL_NAME ")
                .Append("    FROM ")
                .Append("        TBL_MSTCARNAME C, ")
                .Append("        TBL_MSTVHCLMODEL D ")
                .Append("    WHERE ")
                .Append("        C.VCLCLASS_CODE = D.VCLCLASS_CODE AND ")
                .Append("        C.VCLCLASS_GENE = D.VCLCLASS_GENE ")
                .Append("    ) MDL ")
                .Append("WHERE ")
                .Append("    SEL.COMSERIESCD = MDL.CAR_NAME_CD_AI21(+) AND ")
                .Append("    SEL.MODELCD = MDL.VCLMODEL_CODE(+) ")
                .Append("ORDER BY ")
                .Append("    SEQNO ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwModelDataTable)("SC3080203_055")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 056.Follow-up Box選択車種取得(カラー)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="cntcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwColor(ByVal dlrcd As String, ByVal strcd As String, ByVal cntcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203FllwColorDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_056 */ ")
                .Append("    FLL.SEQNO, ")
                .Append("    FLL.SERIESCD, ")
                .Append("    FLL.SERIESNM, ")
                .Append("    FLL.MODELCD, ")
                .Append("    FLL.VCLMODEL_NAME, ")
                .Append("    FLL.COLORCD, ")
                .Append("    E.DISP_BDY_COLOR ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        SEL.SEQNO, ")
                .Append("        SEL.SERIESNM, ")
                .Append("        MDL.VCLMODEL_NAME, ")
                .Append("        SEL.SERIESCD, ")
                .Append("        SEL.MODELCD, ")
                .Append("        SEL.COLORCD ")
                .Append("    FROM ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            A.SEQNO, ")
                .Append("            A.SERIESCD, ")
                .Append("            A.MODELCD, ")
                .Append("            A.COLORCD, ")
                .Append("            B.COMSERIESCD, ")
                .Append("            B.SERIESNM ")
                .Append("        FROM ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                SEQNO, ")
                .Append("                SERIESCD, ")
                .Append("                MODELCD, ")
                .Append("                COLORCD ")
                .Append("            FROM ")
                .Append("                TBL_FLLWUPBOX_SELECTED_SERIES ")
                .Append("            WHERE ")
                .Append("                DLRCD = :DLRCD AND ")
                .Append("                STRCD = :STRCD AND ")
                .Append("                FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("            ORDER BY ")
                .Append("                SEQNO ")
                .Append("            ) A, ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                A.DLRCD, ")
                .Append("                B.SERIESCD, ")
                .Append("                B.SERIESNM, ")
                .Append("                B.TOYOTABRAND, ")
                .Append("                B.IMAGEPATH, ")
                .Append("                B.COMSERIESCD, ")
                .Append("                B.DELFLG, ")
                .Append("                B.DELDATE, ")
                .Append("                B.CREATEDATE, ")
                .Append("                B.UPDATEDATE, ")
                .Append("                B.MAKERCD ")
                .Append("            FROM ")
                .Append("                TBLM_DEALER A, ")
                .Append("                TBLORG_SERIESMASTER B ")
                '.Append("            WHERE ")
                '.Append("                A.DLRCD = B.DLRCD AND ")
                '.Append("                A.DLRCD = :DLRCD AND ")
                '.Append("                A.DELFLG = '0' AND ")
                '.Append("                A.CNTCD = :CNTCD OR ")
                '.Append("                (B.DLRCD = :DLRCD AND A.DLRCD = :DLRCD AND A.DELFLG = '0' AND A.CNTCD = :CNTCD AND NOT EXISTS ( ")
                '.Append("                                                                                                              SELECT ")
                '.Append("                                                                                                                  1 ")
                '.Append("                                                                                                              FROM ")
                '.Append("                                                                                                                  TBLORG_SERIESMASTER C ")
                '.Append("                                                                                                              WHERE ")
                '.Append("                                                                                                                  C.DLRCD = A.DLRCD AND ")
                '.Append("                                                                                                                  C.SERIESCD = B.SERIESCD ")
                '.Append("                                                                                                              )) ")
                .Append("            WHERE ")
                .Append("                A.DLRCD = B.DLRCD AND ")
                .Append("                A.DLRCD = :DLRCD AND ")
                .Append("                A.DELFLG = '0' AND ")
                .Append("                A.CNTCD = :CNTCD OR ")
                .Append("                (B.DLRCD = '00000'  ")
                .Append("                    AND A.DLRCD =  :DLRCD  ")
                .Append("                    AND A.DELFLG = '0'  ")
                .Append("                    AND A.CNTCD =  :CNTCD  ")
                .Append("                    AND NOT EXISTS (SELECT 1   ")
                .Append("                                      FROM TBLORG_SERIESMASTER C  ")
                .Append("                                      WHERE C.DLRCD = A.DLRCD  ")
                .Append("                                      AND C.SERIESCD = B.SERIESCD)) ")

                .Append("            ) B ")
                .Append("        WHERE ")
                .Append("            B.DLRCD = :DLRCD AND ")
                .Append("            A.SERIESCD = B.SERIESCD ")
                .Append("        ) SEL, ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            C.CAR_NAME_CD_AI21, ")
                .Append("            D.VCLMODEL_CODE, ")
                .Append("            D.VCLMODEL_NAME ")
                .Append("        FROM ")
                .Append("            TBL_MSTCARNAME C, ")
                .Append("            TBL_MSTVHCLMODEL D ")
                .Append("        WHERE ")
                .Append("            C.VCLCLASS_CODE = D.VCLCLASS_CODE AND ")
                .Append("            C.VCLCLASS_GENE = D.VCLCLASS_GENE ")
                .Append("        ) MDL ")
                .Append("    WHERE ")
                .Append("        SEL.COMSERIESCD = MDL.CAR_NAME_CD_AI21(+) AND ")
                .Append("        SEL.MODELCD = MDL.VCLMODEL_CODE(+) ")
                .Append("    ) FLL, ")
                .Append("    TBL_MSTEXTERIOR ")
                .Append("    E ")
                .Append("WHERE ")
                .Append("    FLL.MODELCD = E.VCLMODEL_CODE (+) AND ")
                .Append("    FLL.COLORCD = E.BODYCLR_CD (+) ")
                .Append("ORDER BY ")
                .Append("    SEQNO ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwColorDataTable)("SC3080203_056")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 057.接触方法マスタ取得 (移行済み)
        ''' </summary>
        ''' <param name="bookedafterflg">受注後フラグ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActContact(ByVal bookedafterflg As String) As SC3080203DataSet.SC3080203ActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActContactDataTable)("SC3080203_057")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_057 */ ")
                    .Append("    CONTACTNO, ")
                    .Append("    CONTACT, ")
                    .Append("    PROCESS, ")
                    .Append("    FIRSTSELECT_WALKIN, ")
                    .Append("    FIRSTSELECT_NOTWALKIN ")
                    .Append("FROM ")
                    .Append("    TBL_CONTACTMETHOD ")
                    .Append("WHERE ")
                    .Append("    DELFLG = '0' ")

                    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
                    .Append("AND ")
                    .Append("    BOOKEDAFTERFLG = :BOOKEDAFTERFLG ")
                    '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End

                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("BOOKEDAFTERFLG", OracleDbType.Char, bookedafterflg)

                Return query.GetData()


            End Using
        End Function

        ''' <summary>
        ''' 058.接触方法マスタ取得(次回活動)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNextActContact() As SC3080203DataSet.SC3080203NextActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NextActContactDataTable)("SC3080203_058")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_058 */ ")
                    .Append("    CONTACTNO, ")
                    .Append("    CONTACT, ")
                    .Append("    FROMTO, ")
                    .Append("    NEXTACTIVITY ")
                    .Append("FROM ")
                    .Append("    TBL_CONTACTMETHOD ")
                    .Append("WHERE ")
                    .Append("    NEXTACTIVITY IN (1,2) AND ")
                    .Append("    DELFLG = '0' ")
                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 059.接触方法マスタ取得(予約フォロー)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFollowContact() As SC3080203DataSet.SC3080203FollowContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FollowContactDataTable)("SC3080203_059")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_059 */ ")
                    .Append("    CONTACTNO, ")
                    .Append("    CONTACT, ")
                    .Append("    FROMTO ")
                    .Append("FROM ")
                    .Append("    TBL_CONTACTMETHOD ")
                    .Append("WHERE ")
                    .Append("    APPOINTMENTFOLLOW = '1' AND ")
                    .Append("    DELFLG = '0' ")
                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 060.Follow-upBox取得(活動結果登録用)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFollowCractstatus(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080201FollowStatusDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080201FollowStatusDataTable)("SC3080203_060")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_060 */ ")
                    .Append("    CRACTSTATUS, ")
                    .Append("    CRACTCATEGORY, ")
                    .Append("    PROMOTION_ID, ")
                    .Append("    REQCATEGORY, ")
                    .Append("    CRACTRESULT ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    BRANCH_PLAN = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 061.文言取得　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContentWord(ByVal seqno As Long) As SC3080203DataSet.SC3080203ContentWordDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ContentWordDataTable)("SC3080203_061")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_061 */ ")
                    .Append("    DECODE(TRIM(ACTION_LOCAL), '', ACTION, ACTION_LOCAL) AS ACTION ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXCONTENT ")
                    .Append("WHERE ")
                    .Append("    SEQNO = :SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 062.日付フォーマット取得 (移行済み)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDateFormat() As SC3080203DataSet.SC3080203DateFormatDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203DateFormatDataTable)("SC3080203_062")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_062 */ ")
                    .Append("    FORMAT ")
                    .Append("FROM ")
                    .Append("    TBL_DATETIMEFORM ")
                    .Append("WHERE ")
                    .Append("    CONVID = 11 ")
                End With
                query.CommandText = sql.ToString()
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 063.自社客名前・敬称取得 (移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetOrgNameTitle(ByVal originalid As String) As SC3080203DataSet.SC3080203NameTitleDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NameTitleDataTable)("SC3080203_063")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_063 */ ")
                    .Append("    NAME, ")
                    .Append("    NAMETITLE ")
                    .Append("FROM ")
                    .Append("    TBLORG_CUSTOMER ")
                    .Append("WHERE ")
                    .Append("    ORIGINALID = :ORIGINALID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 064.未取引客名前・敬称取得 (移行済み)
        ''' </summary>
        ''' <param name="cstid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewNameTitle(ByVal cstid As String) As SC3080203DataSet.SC3080203NameTitleDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NameTitleDataTable)("SC3080203_064")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_064 */ ")
                    .Append("    NAME, ")
                    .Append("    NAMETITLE ")
                    .Append("FROM ")
                    .Append("    tbl_NEWCUSTOMER ")
                    .Append("WHERE ")
                    .Append("    CSTID = :CSTID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 065.CalDAV用接触方法名取得 (移行済み)
        ''' </summary>
        ''' <param name="contactno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContactNM(ByVal contactno As Long) As SC3080203DataSet.SC3080203GetContactNmDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203GetContactNmDataTable)("SC3080203_065")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_065 */ ")
                    .Append("    CONTACT ")
                    .Append("FROM ")
                    .Append("    TBL_CONTACTMETHOD ")
                    .Append("WHERE ")
                    .Append("    CONTACTNO = :CONTACTNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 066.CalDAV用ToDo背景色取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="createdatadiv"></param>
        ''' <param name="scheduledvs"></param>
        ''' <param name="nextactiondvs"></param>
        ''' <param name="contactno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetToDoColor(ByVal dlrcd As String, ByVal createdatadiv As String, ByVal scheduledvs As String,
                                     ByVal nextactiondvs As String, ByVal contactno As Long) As SC3080203DataSet.SC3080203TodoColorDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203TodoColorDataTable)("SC3080203_066")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_066 */ ")
                    .Append("    BACKGROUNDCOLOR ")
                    .Append("FROM ")
                    .Append("    TBL_TODO_TIP_COLOR ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    CREATEDATADIV= :CREATEDATADIV AND ")
                    .Append("    SCHEDULEDVS= :SCHEDULEDVS AND ")
                    .Append("    NEXTACTIONDVS= :NEXTACTIONDVS AND ")
                    .Append("    CONTACTNO= :CONTACTNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("CREATEDATADIV", OracleDbType.Char, createdatadiv)
                query.AddParameterWithTypeValue("SCHEDULEDVS", OracleDbType.Char, scheduledvs)
                query.AddParameterWithTypeValue("NEXTACTIONDVS", OracleDbType.Char, nextactiondvs)
                query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 067.未取引客存在確認　(移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewCustID(ByVal originalid As String) As SC3080203DataSet.SC3080203NewCustIDDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NewCustIDDataTable)("SC3080203_067")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_067 */ ")
                    .Append("    CSTID ")
                    .Append("FROM ")
                    .Append("    TBL_NEWCUSTOMER ")
                    .Append("WHERE ")
                    .Append("    ORIGINALID = :ORIGINALID ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 068.未取引客車両存在確認 (移行済み)
        ''' </summary>
        ''' <param name="cstid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewVclID(ByVal cstid As String, ByVal vin As String) As SC3080203DataSet.SC3080203NewVclIDDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_068 */ ")
                .Append("    SEQNO ")
                .Append("FROM ")
                .Append("    TBL_NEWCUSTOMERVCLRE ")
                .Append("WHERE ")
                .Append("    CSTID = :CSTID AND ")
                .Append("    VIN = :VIN ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NewVclIDDataTable)("SC3080203_068")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 069.アイコンのパス取得　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetContentIconPath(ByVal seqno As Integer) As SC3080203DataSet.SC3080203ContentIconPathDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ContentIconPathDataTable)("SC3080203_069")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_069 */ ")
                    .Append("    ICONPATH_RESULT_NOTSELECTED, ")
                    .Append("    ICONPATH_RESULT_SELECTED ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXCONTENT ")
                    .Append("WHERE ")
                    .Append("    SEQNO = :SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Char, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 070.Follow-up Box走行距離履歴の追加 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="insdid"></param>
        ''' <param name="vin"></param>
        ''' <param name="seqno"></param>
        ''' <param name="inputdate"></param>
        ''' <param name="mileage"></param>
        ''' <param name="system"></param>
        ''' <param name="jobno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function insertFllwupboxMilehis(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                                       ByVal insdid As String, ByVal vin As String, ByVal seqno As Long,
                                                       ByVal inputdate As String, ByVal mileage As Double, ByVal system As String,
                                                       ByVal jobno As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_070 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXMILEHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    INSDID, ")
                .Append("    VIN, ")
                .Append("    SEQNO, ")
                .Append("    INPUTDATE, ")
                .Append("    MILEAGE, ")
                .Append("    SYSTEM, ")
                .Append("    JOBNO, ")
                .Append("    WAREDLRCD, ")
                .Append("    WARESTRCD, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE ")
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :DLRCD, ")
                .Append("    :STRCD, ")
                .Append("    :FLLWUPBOX_SEQNO, ")
                .Append("    NULL, ")
                .Append("    ' ', ")
                .Append("    '4', ")
                .Append("    :INSDID, ")
                .Append("    :VIN, ")
                .Append("    :SEQNO, ")
                .Append("    TO_DATE(:INPUTDATE,'YYYY/MM/DD HH24:MI:SS'), ")
                .Append("    :MILEAGE, ")
                .Append("    :SYSTEM, ")
                .Append("    :JOBNO, ")
                .Append("    :DLRCD, ")
                .Append("    :STRCD, ")
                .Append("    SYSDATE, ")
                .Append("    SYSDATE ")
                .Append(") ")
            End With
            Using query As New DBUpdateQuery("SC3080203_070")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("INPUTDATE", OracleDbType.Char, inputdate)
                query.AddParameterWithTypeValue("MILEAGE", OracleDbType.Double, mileage)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.Char, system)
                query.AddParameterWithTypeValue("JOBNO", OracleDbType.Char, jobno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 071.入庫履歴よりサービススタッフ情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <param name="vin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetServiceStaff(ByVal originalid As String, ByVal vin As String) As SC3080203DataSet.SC3080203ServiceStaffDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ServiceStaffDataTable)("SC3080203_071")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_071 */ ")
                    .Append("    A.SERVICESTAFFCD, ")
                    .Append("    A.SERVICESTAFFNM ")
                    .Append("FROM ")
                    .Append("    TBLORG_MILEAGEHIS A, ")
                    .Append("    ( ")
                    .Append("    SELECT ")
                    .Append("        MAX(MILEAGESEQ) AS MILEAGESEQ ")
                    .Append("    FROM ")
                    .Append("        TBLORG_MILEAGEHIS ")
                    .Append("    WHERE ")
                    .Append("        ORIGINALID = :ORIGINALID AND ")
                    .Append("        VIN = :VIN AND ")
                    .Append("        DELFLG = '0' ")
                    .Append("    GROUP BY ")
                    .Append("        ORIGINALID, ")
                    .Append("        VIN ")
                    .Append("    ) B ")
                    .Append("WHERE ")
                    .Append("    A.ORIGINALID = :ORIGINALID AND ")
                    .Append("    A.VIN = :VIN AND ")
                    .Append("    A.MILEAGESEQ = B.MILEAGESEQ ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 072.自社客連番に紐付くVINを取得 (移行済み)
        ''' </summary>
        ''' <param name="originalid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetVin(ByVal originalid As String) As SC3080203DataSet.SC3080203VinDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203VinDataTable)("SC3080203_072")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_080 */ ")
                    .Append("    VIN ")
                    .Append("FROM ")
                    .Append("    TBLORG_VCLINFO ")
                    .Append("WHERE ")
                    .Append("    ORIGINALID = :ORIGINALID AND ")
                    .Append("    DELFLG = '0' ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 073.活動実績登録用の希望車種情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisCarSeq(ByVal dlrcd As String, ByVal strcd As String,
                                               ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203SeqDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeqDataTable)("SC3080203_073")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_073 */ ")
                    .Append("    SEQNO ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX_SELECTED_SERIES A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                    .Append("ORDER BY ")
                    .Append("    SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 074.活動実績登録用の希望車種情報を取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seqno"></param>
        ''' <param name="div"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisSelCarSeq(ByVal dlrcd As String, ByVal strcd As String,
                                                  ByVal fllwupboxseqno As Long, ByVal seqno As String,
                                                  ByVal div As String) As SC3080203DataSet.SC3080203SeqDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203SeqDataTable)("SC3080203_074")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_074 */ ")
                    .Append("    A.SEQNO ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX_SELECTED_SERIES A, ")
                    .Append("    ( ")
                    .Append("    SELECT ")
                    If String.Equals(div, "1") Then
                        .Append("        SERIESCD ")
                    ElseIf String.Equals(div, "2") Then
                        .Append("        SERIESCD, ")
                        .Append("        MODELCD ")
                    ElseIf String.Equals(div, "4") Then
                        .Append("        SERIESCD, ")
                        .Append("        MODELCD, ")
                        .Append("        COLORCD ")
                    End If
                    .Append("    FROM ")
                    .Append("        TBL_FLLWUPBOX_SELECTED_SERIES ")
                    .Append("    WHERE ")
                    .Append("        DLRCD = :DLRCD AND ")
                    .Append("        STRCD = :STRCD AND ")
                    .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("        SEQNO = :SEQNO ")
                    .Append("    ) B ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD AND ")
                    .Append("    A.STRCD = :STRCD AND ")
                    .Append("    A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                    If String.Equals(div, "1") Then
                        .Append("    AND A.SERIESCD = B.SERIESCD ")
                    ElseIf String.Equals(div, "2") Then
                        .Append("    AND A.SERIESCD = B.SERIESCD AND ")
                        .Append("    A.MODELCD = B.MODELCD ")
                    ElseIf String.Equals(div, "4") Then
                        .Append("    AND A.SERIESCD = B.SERIESCD AND ")
                        .Append("    A.MODELCD = B.MODELCD AND ")
                        .Append("    A.COLORCD = B.COLORCD ")
                    End If
                    .Append("ORDER BY ")
                    .Append("    SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Char, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 075.活動実績登録用のフォローアップボックス情報を取得　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisFllw(ByVal dlrcd As String, ByVal strcd As String,
                                               ByVal fllwupboxseqno As Long) As SC3080203DataSet.SC3080203ActHisFllwDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisFllwDataTable)("SC3080203_075")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_075 */ ")
                    .Append("    CRPLAN_ID, ")
                    .Append("    BFAFDVS, ")
                    .Append("    CRDVSID, ")
                    .Append("    DECODE(MEMKIND,3,UNTRADEDCSTID,INSDID) INSDID, ")
                    .Append("    SERIESCODE, ")
                    .Append("    SERIESNAME, ")
                    .Append("    VCLREGNO, ")
                    .Append("    SUBCTGCODE, ")
                    .Append("    SERVICECD, ")
                    .Append("    SUBCTGORGNAME, ")
                    .Append("    SUBCTGORGNAME_EX, ")
                    .Append("    PROMOTION_ID, ")
                    .Append("    CRACTRESULT, ")
                    .Append("    PLANDVS, ")
                    .Append("    VIN, ")
                    .Append("    CUSTCHRGSTAFFNM, ")
                    .Append("    ACCOUNT_PLAN, ")
                    .Append("    CRCUSTID, ")
                    .Append("    CUSTOMERCLASS ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOX ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD =:STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 076.Follow-up BOX活動内容取得(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="seqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisContent(ByVal seqno As Long) As SC3080203DataSet.SC3080203ActHisContentDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisContentDataTable)("SC3080203_076")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_076 */ ")
                    .Append("    ACTIONTYPE, ")
                    .Append("    DECODE(ACTION_LOCAL,' ', ACTION, ACTION_LOCAL) AS ACTION, ")
                    .Append("    METHOD, ")
                    .Append("    ACTIONCD, ")
                    .Append("    CATEGORYID, ")
                    .Append("    CATEGORYDVSID ")
                    .Append("FROM ")
                    .Append("    TBL_FLLWUPBOXCONTENT ")
                    .Append("WHERE ")
                    .Append("    SEQNO = :SEQNO ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 077.Follow-up Box選択車種取得(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="seqno"></param>
        ''' <param name="cntcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActHisCarSeq(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long, ByVal seqno As Long, ByVal cntcd As String) As SC3080203DataSet.SC3080203ActHisSelCarDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_077 */ ")
                .Append("    FLL.SERIESNM, ")
                .Append("    FLL.VCLMODEL_NAME, ")
                .Append("    E.DISP_BDY_COLOR, ")
                .Append("    FLL.QUANTITY ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        SEL.SERIESNM, ")
                .Append("        SEL.MODELCD, ")
                .Append("        MDL.VCLMODEL_NAME, ")
                .Append("        SEL.COLORCD, ")
                .Append("        SEL.SEQNO, ")
                .Append("        SEL.QUANTITY ")
                .Append("    FROM ")
                .Append("        ( ")
                .Append("        SELECT ")
                .Append("            A.SERIESCD, ")
                .Append("            A.MODELCD, ")
                .Append("            A.COLORCD, ")
                .Append("            A.SEQNO, ")
                .Append("            B.COMSERIESCD, ")
                .Append("            B.SERIESNM, ")
                .Append("            A.QUANTITY ")
                .Append("        FROM ")
                .Append("            TBL_FLLWUPBOX_SELECTED_SERIES A, ")
                .Append("            ( ")
                .Append("            SELECT ")
                .Append("                A.DLRCD, ")
                .Append("                B.SERIESCD, ")
                .Append("                B.SERIESNM, ")
                .Append("                B.TOYOTABRAND, ")
                .Append("                B.IMAGEPATH, ")
                .Append("                B.COMSERIESCD, ")
                .Append("                B.DELFLG, ")
                .Append("                B.DELDATE, ")
                .Append("                B.CREATEDATE, ")
                .Append("                B.UPDATEDATE, ")
                .Append("                B.MAKERCD ")
                .Append("            FROM ")
                .Append("                TBLM_DEALER A, ")
                .Append("                TBLORG_SERIESMASTER B ")
                .Append("         WHERE ")
                .Append("             A.DLRCD = B.DLRCD AND ")
                .Append("             A.DLRCD = :DLRCD AND ")
                .Append("             A.DELFLG = '0' AND ")
                .Append("             A.CNTCD = :CNTCD OR ")
                .Append("             (B.DLRCD = '00000'  ")
                .Append("                 AND A.DLRCD =  :DLRCD  ")
                .Append("                 AND A.DELFLG = '0'  ")
                .Append("                 AND A.CNTCD =  :CNTCD  ")
                .Append("                 AND NOT EXISTS (SELECT 1   ")
                .Append("                                 FROM TBLORG_SERIESMASTER C  ")
                .Append("                                 WHERE C.DLRCD = A.DLRCD  ")
                .Append("                                 AND C.SERIESCD = B.SERIESCD)) ")
                .Append("            ) B ")
                .Append("        WHERE ")
                .Append("            A.SERIESCD = B.SERIESCD AND ")
                .Append("            A.DLRCD = :DLRCD AND ")
                .Append("            A.STRCD = :STRCD AND ")
                .Append("            A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                .Append("            A.SEQNO = :SEQNO ")
                .Append("        ) SEL, ")
                .Append("        (SELECT ")
                .Append("            C.CAR_NAME_CD_AI21, ")
                .Append("            D.VCLMODEL_CODE, ")
                .Append("            D.VCLMODEL_NAME ")
                .Append("        FROM ")
                .Append("            TBL_MSTCARNAME C, ")
                .Append("            TBL_MSTVHCLMODEL D ")
                .Append("        WHERE ")
                .Append("            C.VCLCLASS_CODE = D.VCLCLASS_CODE AND ")
                .Append("            C.VCLCLASS_GENE = D.VCLCLASS_GENE ")
                .Append("        ) MDL ")
                .Append("    WHERE ")
                .Append("        SEL.COMSERIESCD = MDL.CAR_NAME_CD_AI21 (+) AND ")
                .Append("        SEL.MODELCD = MDL.VCLMODEL_CODE (+) ")
                .Append("    ) FLL, ")
                .Append("    TBL_MSTEXTERIOR E ")
                .Append("WHERE ")
                .Append("    FLL.MODELCD = E.VCLMODEL_CODE (+) AND ")
                .Append("    FLL.COLORCD = E.BODYCLR_CD (+) ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203ActHisSelCarDataTable)("SC3080203_077")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("CNTCD", OracleDbType.Char, cntcd)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 078.Follow-up Box活動履歴(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="fllwupboxseqno"></param>
        ''' <param name="crplanid"></param>
        ''' <param name="bfafdvs"></param>
        ''' <param name="crdvsid"></param>
        ''' <param name="insdid"></param>
        ''' <param name="seriescode"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="account"></param>
        ''' <param name="regno"></param>
        ''' <param name="subctgcode"></param>
        ''' <param name="servicecd"></param>
        ''' <param name="subctgorgname"></param>
        ''' <param name="subctgorgnameex"></param>
        ''' <param name="promotionid"></param>
        ''' <param name="activityresult"></param>
        ''' <param name="plandvs"></param>
        ''' <param name="actdate"></param>
        ''' <param name="method"></param>
        ''' <param name="action"></param>
        ''' <param name="actiontype"></param>
        ''' <param name="brnchaccount"></param>
        ''' <param name="actioncd"></param>
        ''' <param name="ctntseqno"></param>
        ''' <param name="selectseriesseqno"></param>
        ''' <param name="seriesnm"></param>
        ''' <param name="vclmodelname"></param>
        ''' <param name="dispbdycolor"></param>
        ''' <param name="quantity"></param>
        ''' <param name="fllwupboxrsltseqno"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InserActHisFllwCrHis(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Long,
                                                      ByVal crplanid As Nullable(Of Long), ByVal bfafdvs As String, ByVal crdvsid As Long,
                                                      ByVal insdid As String, ByVal seriescode As String, ByVal seriesname As String,
                                                      ByVal account As String, ByVal regno As String, ByVal subctgcode As String,
                                                      ByVal servicecd As String, ByVal subctgorgname As String, ByVal subctgorgnameex As String,
                                                      ByVal promotionid As Nullable(Of Long), ByVal activityresult As String, ByVal plandvs As String,
                                                      ByVal actdate As Date, ByVal method As String, ByVal action As String,
                                                      ByVal actiontype As String, ByVal brnchaccount As String, ByVal actioncd As String,
                                                      ByVal ctntseqno As Long, ByVal selectseriesseqno As Long, ByVal seriesnm As String,
                                                      ByVal vclmodelname As String, ByVal dispbdycolor As String, ByVal quantity As Long,
                                                      ByVal fllwupboxrsltseqno As Long) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_078 */ ")
                .Append("INTO ")
                .Append("    TBL_FLLWUPBOXCRHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    FLLWUPBOX_SEQNO, ")
                .Append("    CRPLAN_ID, ")
                .Append("    BFAFDVS, ")
                .Append("    CRDVSID, ")
                .Append("    IDENTITYNO, ")
                .Append("    SEQNO, ")
                .Append("    INSDID, ")
                .Append("    SERIESCODE, ")
                .Append("    SERIESNAME, ")
                .Append("    CALLDATE, ")
                .Append("    ACCOUNT, ")
                .Append("    REGNO, ")
                .Append("    SUBCTGCODE, ")
                .Append("    SERVICECD, ")
                .Append("    SUBCTGORGNAME, ")
                .Append("    SUBCTGORGNAME_EX, ")
                .Append("    PROMOTION_ID, ")
                .Append("    CRDVS, ")
                .Append("    ACTIVITYRESULT, ")
                .Append("    PLANDVS, ")
                .Append("    ACTUALTIME_END, ")
                .Append("    ACTDATE, ")
                .Append("    METHOD, ")
                .Append("    ACTION, ")
                .Append("    ACTIONTYPE, ")
                .Append("    HOACCOUNT, ")
                .Append("    BRNCHACCOUNT, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    ACTIONCD, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    CTNTSEQNO, ")
                .Append("    SELECT_SERIES_SEQNO, ")
                .Append("    SERIESNM, ")
                .Append("    VCLMODEL_NAME, ")
                .Append("    DISP_BDY_COLOR, ")
                .Append("    QUANTITY, ")
                .Append("    FLLWUPBOXRSLT_SEQNO ")
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :DLRCD, ")
                .Append("    :STRCD, ")
                .Append("    :FLLWUPBOX_SEQNO, ")
                .Append("    :CRPLAN_ID, ")
                .Append("    :BFAFDVS, ")
                .Append("    :CRDVSID, ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        NVL( ")
                .Append("        MAX( ")
                .Append("        IDENTITYNO ")
                .Append("    ), ")
                .Append("        '0' ")
                .Append("    ) + 1 ")
                .Append("    FROM ")
                .Append("        TBL_FLLWUPBOXCRHIS ")
                .Append("    WHERE ")
                .Append("        DLRCD = :DLRCD AND ")
                .Append("        STRCD = :STRCD AND ")
                .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
                .Append("    ), ")
                .Append("    1, ")
                .Append("    :INSDID, ")
                .Append("    :SERIESCODE, ")
                .Append("    :SERIESNAME, ")
                .Append("    NULL, ")
                .Append("    :ACCOUNT, ")
                .Append("    :REGNO, ")
                .Append("    :SUBCTGCODE, ")
                .Append("    :SERVICECD, ")
                .Append("    :SUBCTGORGNAME, ")
                .Append("    :SUBCTGORGNAME_EX, ")
                .Append("    :PROMOTION_ID, ")
                .Append("    NULL, ")
                .Append("    :ACTIVITYRESULT, ")
                .Append("    :PLANDVS, ")
                .Append("    NULL, ")
                .Append("    :ACTDATE, ")
                .Append("    :METHOD, ")
                .Append("    :ACTION, ")
                .Append("    :ACTIONTYPE, ")
                .Append("    '', ")
                .Append("    :BRNCHACCOUNT, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    SYSDATE, ")
                .Append("    SYSDATE, ")
                .Append("    :ACTIONCD, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    :CTNTSEQNO, ")
                .Append("    :SELECT_SERIES_SEQNO, ")
                .Append("    :SERIESNM, ")
                .Append("    :VCLMODEL_NAME, ")
                .Append("    :DISP_BDY_COLOR, ")
                .Append("    :QUANTITY, ")
                .Append("    :FLLWUPBOXRSLT_SEQNO ")
                .Append(") ")
            End With
            Using query As New DBUpdateQuery("SC3080203_078")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxseqno)
                query.AddParameterWithTypeValue("CRPLAN_ID", OracleDbType.Int64, crplanid)
                query.AddParameterWithTypeValue("BFAFDVS", OracleDbType.Char, bfafdvs)
                query.AddParameterWithTypeValue("CRDVSID", OracleDbType.Int64, crdvsid)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                query.AddParameterWithTypeValue("SERIESCODE", OracleDbType.Char, seriescode)
                query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                query.AddParameterWithTypeValue("REGNO", OracleDbType.Char, regno)
                query.AddParameterWithTypeValue("SUBCTGCODE", OracleDbType.Char, subctgcode)
                query.AddParameterWithTypeValue("SERVICECD", OracleDbType.Char, servicecd)
                query.AddParameterWithTypeValue("SUBCTGORGNAME", OracleDbType.Char, subctgorgname)
                query.AddParameterWithTypeValue("SUBCTGORGNAME_EX", OracleDbType.Char, subctgorgnameex)
                query.AddParameterWithTypeValue("PROMOTION_ID", OracleDbType.Int64, promotionid)
                query.AddParameterWithTypeValue("ACTIVITYRESULT", OracleDbType.Char, activityresult)
                query.AddParameterWithTypeValue("PLANDVS", OracleDbType.Char, plandvs)
                query.AddParameterWithTypeValue("ACTDATE", OracleDbType.Date, actdate)
                query.AddParameterWithTypeValue("METHOD", OracleDbType.Char, method)
                query.AddParameterWithTypeValue("ACTION", OracleDbType.Char, action)
                query.AddParameterWithTypeValue("ACTIONTYPE", OracleDbType.Char, actiontype)
                query.AddParameterWithTypeValue("BRNCHACCOUNT", OracleDbType.Char, brnchaccount)
                query.AddParameterWithTypeValue("ACTIONCD", OracleDbType.Char, actioncd)
                query.AddParameterWithTypeValue("CTNTSEQNO", OracleDbType.Int64, ctntseqno)
                query.AddParameterWithTypeValue("SELECT_SERIES_SEQNO", OracleDbType.Int64, selectseriesseqno)
                query.AddParameterWithTypeValue("SERIESNM", OracleDbType.Char, seriesnm)
                query.AddParameterWithTypeValue("VCLMODEL_NAME", OracleDbType.Char, vclmodelname)
                query.AddParameterWithTypeValue("DISP_BDY_COLOR", OracleDbType.Char, dispbdycolor)
                query.AddParameterWithTypeValue("QUANTITY", OracleDbType.Long, quantity)
                query.AddParameterWithTypeValue("FLLWUPBOXRSLT_SEQNO", OracleDbType.Long, fllwupboxrsltseqno)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 079.Total履歴追加(活動結果登録用)　(移行済み)
        ''' </summary>
        ''' <param name="dlrcd"></param>
        ''' <param name="strcd"></param>
        ''' <param name="insdid"></param>
        ''' <param name="contactdate"></param>
        ''' <param name="categoryid"></param>
        ''' <param name="categorydvsid"></param>
        ''' <param name="vin"></param>
        ''' <param name="seriesname"></param>
        ''' <param name="accountnm"></param>
        ''' <param name="crcustid"></param>
        ''' <param name="customerclass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function InserActHisFllwTotalHis(ByVal dlrcd As String, ByVal strcd As String, ByVal insdid As String,
                                                       ByVal contactdate As Date, ByVal categoryid As String, ByVal categorydvsid As Nullable(Of Long),
                                                       ByVal vin As String, ByVal seriesname As String, ByVal accountnm As String,
                                                       ByVal crcustid As String, ByVal customerclass As String) As Integer
            Dim sql As New StringBuilder
            With sql
                .Append("INSERT /* SC3080203_079 */ ")
                .Append("INTO ")
                .Append("    TBL_TOTALHIS ")
                .Append("( ")
                .Append("    DLRCD, ")
                .Append("    STRCD, ")
                .Append("    INSDID, ")
                .Append("    SEQNO, ")
                .Append("    CONTACTDATE, ")
                .Append("    CATEGORYID, ")
                .Append("    CATEGORYDVSID, ")
                .Append("    VIN, ")
                .Append("    SERIESNAME, ")
                .Append("    STATUS, ")
                .Append("    STALL_REZID, ")
                .Append("    STALL_DLRCD, ")
                .Append("    STALL_STRCD, ")
                .Append("    APPOINTMENTDATE, ")
                .Append("    PROMOTION_NM, ")
                .Append("    SERVICE_NM, ")
                .Append("    MILEAGE, ")
                .Append("    SERVICEINDATE, ")
                .Append("    SNDMAILID, ")
                .Append("    RECMAILID, ")
                .Append("    REQUESTID, ")
                .Append("    ACCOUNT_NM, ")
                .Append("    CREATEDATE, ")
                .Append("    UPDATEDATE, ")
                .Append("    REC_ID, ")
                .Append("    CMS_HISLINKID, ")
                .Append("    CRCUSTID, ")
                .Append("    CUSTOMERCLASS ")
                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("    :DLRCD, ")
                .Append("    :STRCD, ")
                .Append("    :INSDID, ")
                .Append("    SEQ_TOTALHIS_SEQNO.NEXTVAL, ")
                .Append("    :CONTACTDATE, ")
                .Append("    :CATEGORYID, ")
                .Append("    :CATEGORYDVSID, ")
                .Append("    :VIN, ")
                .Append("    :SERIESNAME, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    :ACCOUNT_NM, ")
                .Append("    SYSDATE, ")
                .Append("    SYSDATE, ")
                .Append("    NULL, ")
                .Append("    NULL, ")
                .Append("    :CRCUSTID, ")
                .Append("    :CUSTOMERCLASS ")
                .Append(") ")
            End With
            Using query As New DBUpdateQuery("SC3080203_079")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, insdid)
                query.AddParameterWithTypeValue("CONTACTDATE", OracleDbType.Date, contactdate)
                query.AddParameterWithTypeValue("CATEGORYID", OracleDbType.Char, categoryid)
                query.AddParameterWithTypeValue("CATEGORYDVSID", OracleDbType.Int64, categorydvsid)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Char, vin)
                query.AddParameterWithTypeValue("SERIESNAME", OracleDbType.Char, seriesname)
                query.AddParameterWithTypeValue("ACCOUNT_NM", OracleDbType.Char, accountnm)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 080.競合車種取得(ALL)　(移行済み)
        ''' </summary>
        ''' <param name="competitionmakerno"></param>
        ''' <param name="competitorcd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCompetition(ByVal competitionmakerno As String, ByVal competitorcd As String) As SC3080203DataSet.SC3080203CompetitionDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_080 */ ")
                .Append("    (SELECT COMPETITIONMAKER FROM TBL_COMPETITION_MAKERMASTER WHERE COMPETITIONMAKERNO = :COMPETITIONMAKERNO) AS COMPETITIONMAKER, ")
                .Append("    (SELECT COMPETITORNM FROM TBL_COMPETITORMASTER WHERE COMPETITORCD = :COMPETITORCD) AS COMPETITORNM ")
                .Append("FROM DUAL ")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203CompetitionDataTable)("SC3080203_080")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("COMPETITIONMAKERNO", OracleDbType.Char, competitionmakerno)
                query.AddParameterWithTypeValue("COMPETITORCD", OracleDbType.Char, competitorcd)
                Return query.GetData()
            End Using
        End Function

        ' ''' <summary>
        ' ''' 081. Follow-up Box商談 を削除
        ' ''' </summary>
        ' ''' <param name="dlrCD"></param>
        ' ''' <param name="strCD"></param>
        ' ''' <param name="fllwupboxSeqNo"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Shared Function DeleteFllwupboxSales(ByVal dlrCD As String, ByVal strCD As String, ByVal fllwupboxSeqNo As Long) As Integer

        '    Using query As New DBUpdateQuery("SC3080203_081")
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("DELETE /* SC3080203_081 */ ")
        '            .Append("FROM ")
        '            .Append("    TBL_FLLWUPBOX_SALES ")
        '            .Append("WHERE ")
        '            .Append("    DLRCD = :DLRCD AND ")
        '            .Append("    STRCD = :STRCD AND ")
        '            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")
        '        End With
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
        '        query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxSeqNo)

        '        Return query.Execute()
        '    End Using
        'End Function

        ''' <summary>
        ''' 081. Follow-up Box商談 を更新　(商談中⇒商談終了) (移行済み)
        ''' </summary>
        ''' <param name="dlrCD"></param>
        ''' <param name="strCD"></param>
        ''' <param name="fllwupbox_seqno"></param>
        ''' <param name="actualaccount"></param>
        ''' <param name="salesstarttime"></param>
        ''' <param name="salesendtime"></param>
        ''' <param name="account"></param>
        ''' <param name="updateid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateFllwupboxSales(ByVal dlrcd As String, _
                            ByVal strcd As String, _
                            ByVal fllwupbox_seqno As Long, _
                            ByVal actualaccount As String, _
                            ByVal salesstarttime As Date, _
                            ByVal salesendtime As Date, _
                            ByVal account As String, _
                            ByVal updateid As String) As Integer

            Using query As New DBUpdateQuery("SC3080203_081")
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* SC3080203_081 */ ")
                    .Append("    TBL_FLLWUPBOX_SALES ")
                    .Append("SET ")
                    .Append("    NEWFLLWUPBOXFLG = '0', ")
                    .Append("    REGISTFLG = '1', ")
                    .Append("    ACTUALACCOUNT = :ACTUALACCOUNT, ")
                    .Append("    STARTTIME = :STARTTIME, ")
                    .Append("    ENDTIME = :ENDTIME, ")
                    .Append("    UPDATEID = :UPDATEID, ")
                    .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ")
                    .Append("    UPDATEDATE = SYSDATE ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    STRCD = :STRCD AND ")
                    .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                    .Append("    REGISTFLG = '0' ")
                End With
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupbox_seqno)

                query.AddParameterWithTypeValue("ACTUALACCOUNT", OracleDbType.Varchar2, actualaccount)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, salesstarttime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, salesendtime)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)
                Return query.Execute()
            End Using
        End Function

        ''' <summary>
        ''' 082. Follow-up Box商談 を取得
        ''' </summary>
        ''' <param name="dlrCD"></param>
        ''' <param name="strCD"></param>
        ''' <param name="fllwupboxSeqNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFllwupboxSales(dlrCD As String, strCD As String, fllwupboxSeqNo As Long) As SC3080203DataSet.SC3080203FllwupboxSalesDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_082 */ ")
                .Append("  STARTTIME, ")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
                .Append("  ENDTIME, ")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
                .Append("  WALKINNUM ")
                .Append("FROM TBL_FLLWUPBOX_SALES ")
                .Append("WHERE ")
                .Append("    DLRCD = :DLRCD AND ")
                .Append("    STRCD = :STRCD AND ")
                .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO AND ")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
                .Append("    REGISTFLG = '0'")
                '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203FllwupboxSalesDataTable)("SC3080203_082")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxSeqNo)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 083. 最大の活動終了時間を取得 (移行済み)
        ''' </summary>
        ''' <param name="dlrCD"></param>
        ''' <param name="strCD"></param>
        ''' <param name="fllwupboxSeqNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetLatestActTimeEnd(dlrCD As String, strCD As String, fllwupboxSeqNo As Long) As SC3080203DataSet.SC3080203LatestActTimeDataTable
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080203_083 */")
                .Append("  MAX(LATEST_TIME_END) LATEST_TIME_END ")
                .Append("FROM ")
                .Append("(")
                .Append("  SELECT WALKINDATE LATEST_TIME_END")
                .Append("  FROM TBL_WALKINPERSON")
                .Append("  WHERE DLRCD = :DLRCD AND ")
                .Append("        STRCD = :STRCD AND ")
                .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                .Append("  UNION ALL")
                .Append("  SELECT ACTUALTIME_END LATEST_TIME_END ")
                .Append("  FROM TBL_FLLWUPBOXRSLT")
                .Append("  WHERE DLRCD = :DLRCD AND ")
                .Append("        STRCD = :STRCD AND ")
                .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                '2012/04/05 河原 【SALES_2】 START

                .Append("  UNION ALL")
                .Append("  SELECT CREATEDATE LATEST_TIME_END ")
                .Append("  FROM TBL_FLLWUPBOX")
                .Append("  WHERE DLRCD = :DLRCD AND ")
                .Append("        STRCD = :STRCD AND ")
                .Append("        FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")

                '2012/04/05 河原 【SALES_2】 END
                .Append(")")
            End With
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203LatestActTimeDataTable)("SC3080203_083")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCD)
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwupboxSeqNo)
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 084.接触方法マスタ取得
        ''' </summary>
        ''' <param name="contactno">接触方法No</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetActContactTitle(ByVal contactno As Long) As SC3080203DataSet.SC3080203NextActContactDataTable
            Using query As New DBSelectQuery(Of SC3080203DataSet.SC3080203NextActContactDataTable)("SC3080203_084")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080203_084 */ ")
                    .Append("    CONTACTNO, ")
                    .Append("    CONTACT, ")
                    .Append("    FROMTO, ")
                    .Append("    NEXTACTIVITY ")
                    .Append("FROM ")
                    .Append("    TBL_CONTACTMETHOD ")
                    .Append("WHERE ")
                    .Append("    DELFLG = '0' ")
                    If contactno <> 0 Then
                        .Append("AND ")
                        .Append("    CONTACTNO = :CONTACTNO ")
                    End If
                    .Append("ORDER BY ")
                    .Append("    SORTNO ")
                End With
                query.CommandText = sql.ToString()
                If contactno <> 0 Then
                    query.AddParameterWithTypeValue("CONTACTNO", OracleDbType.Int64, contactno)
                End If
                Return query.GetData()
            End Using
        End Function

    End Class

End Namespace
