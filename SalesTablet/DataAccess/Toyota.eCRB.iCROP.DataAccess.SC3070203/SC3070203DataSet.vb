'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070203DataSet.vb
'─────────────────────────────────────
'機能： 価格相談
'補足： 
'更新： 2013/06/30 TCS 葛西　  2013/10対応版　既存流用
'更新： 2013/11/28 TCS 森      Aカード情報相互連携開発
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.SC3070203DataSet
'2013/06/30 TCS 葛西 2013/10対応版　既存流用 START
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 葛西 2013/10対応版　既存流用 END

Namespace SC3070203DataSetTableAdapters


    Public Class SC3070203DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数/Enum"
        'プログラムID
        Private Const PROG_ID As String = "SC3070203"
        'セールスマネージャー権限
        Private Const SALES_MANAGER As Integer = 7
        '依頼内容マスタの価格相談
        Private Const REASON_DISCOUNT As String = "02"
        '価格相談の回答ステータス（未回答）
        Private Const NOT_RESPONSE As String = "0"
        '価格相談の回答ステータス（回答済）
        Private Const RESPONSED As String = "1"
        '価格相談のキャンセルステータス（キャンセル以外）
        Private Const NOT_CANCEL As String = "2"

        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
        ''' <summary>
        ''' リーダフラグ:リーダ以外
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LeaderFlgOff As String = "0"

        ''' <summary>
        ''' 操作権限コード：Sales Consultant Manager
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationSCM As Short = 7

        ''' <summary>
        ''' 操作権限コード：Sales Staff
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationSC As Short = 8
        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END


#End Region

#Region "メンバ変数"
        Private DlrCd As String
        Private StrCd As String
        Private UserId As String

        '見積作成画面からの引継ぎ情報
        Private TakingOverInfo As SC3070203TakingOverInfoRow
#End Region

#Region "コンストラクタ"
        Public Sub New(ByVal dlrcd As String, _
           ByVal strcd As String, _
           ByVal userid As String, _
           ByVal takingOverInfo As SC3070203TakingOverInfoRow)
            Me.DlrCd = dlrcd
            Me.StrCd = strcd
            Me.UserId = userid
            Me.TakingOverInfo = takingOverInfo
        End Sub
#End Region

#Region "メソッド"
        ''' <summary>
        ''' セールスマネージャー一覧取得
        ''' </summary>
        ''' <returns>SC3070203SalesManagerDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectSalesManagerList(ByVal leaderFlg As Boolean) As SC3070203DataSet.SC3070203SalesManagerDataTable

            Dim sql As New StringBuilder
            With sql
                .AppendLine("    SELECT /* SC3070203_001 */")
                .AppendLine("           A.ACCOUNT")
                .AppendLine("         , A.USERNAME")
                .AppendLine("         , A.PRESENCECATEGORY")
                .AppendLine("      FROM TBL_USERS A ")
                .AppendLine("         , TBL_USERDISPLAY B")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
                .AppendLine("         , TB_M_STAFF C")
                .Append("             , TB_M_ORGANIZATION D")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
                .AppendLine("     WHERE B.ACCOUNT = A.ACCOUNT")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
                .Append("            AND RTRIM(A.ACCOUNT) = C.STF_CD")
                .Append("            AND C.ORGNZ_ID = D.ORGNZ_ID(+)")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
                .AppendLine("       AND A.DLRCD = :DLRCD")
                .AppendLine("       AND A.STRCD = :STRCD")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
                .AppendLine("       AND C.DISCOUNT_APPROVAL_FLG = :DISCOUNT_APPROVAL_FLG")
                .Append("AND (")
                .Append("    A.OPERATIONCODE = :OPERATIONSCM")
                If Not (leaderFlg) Then
                    .Append("    OR (")
                    .Append("               C.BRN_MANAGER_FLG = :BRN_MANAGER_FLG ")
                    .Append("               AND C.BRN_SC_FLG = :BRN_SC_FLG ")
                    .Append("               AND D.ORGNZ_SC_FLG = :ORGNZ_SC_FLG ")
                    .Append("               AND D.INUSE_FLG = :INUSE_FLG ")
                    .Append("               AND A.OPERATIONCODE = :OPERATIONSC")
                    .Append("    )")
                End If
                .Append(")")
                .Append("AND A.DELFLG = :DELFLG")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
                .AppendLine("  ORDER BY CASE")
                .AppendLine("                WHEN A.PRESENCECATEGORY IN (:PRESENCECATEGORY_1,:PRESENCECATEGORY_2,:PRESENCECATEGORY_3) THEN 0")
                .AppendLine("                ELSE 1")
                .AppendLine("           END")
                .AppendLine("         , B.SORTNO")
            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203SalesManagerDataTable)("SC3070203_001")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, Me.DlrCd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, Me.StrCd)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
                query.AddParameterWithTypeValue("DISCOUNT_APPROVAL_FLG", OracleDbType.Char, "1")
                query.AddParameterWithTypeValue("OPERATIONSCM", OracleDbType.Int64, OperationSCM)
                If Not (leaderFlg) Then
                    query.AddParameterWithTypeValue("BRN_MANAGER_FLG", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("BRN_SC_FLG", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("ORGNZ_SC_FLG", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("OPERATIONSC", OracleDbType.Int64, OperationSC)
                End If
                ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
                query.AddParameterWithTypeValue("PRESENCECATEGORY_1", OracleDbType.Char, "1")
                query.AddParameterWithTypeValue("PRESENCECATEGORY_2", OracleDbType.Char, "2")
                query.AddParameterWithTypeValue("PRESENCECATEGORY_3", OracleDbType.Char, "3")
                Return query.GetData()
            End Using

        End Function


        ''' <summary>
        ''' 値引き理由一覧取得
        ''' </summary>
        ''' <returns>SC3070203DiscountReasonDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectPriceConsultationResonList() As SC3070203DataSet.SC3070203PriceConsultationReasonDataTable

            Dim sql As New StringBuilder
            With sql
                .AppendLine("  SELECT /* SC3070203_002 */")
                .AppendLine("         ID")
                .AppendLine("       , MSG_DLR")
                .AppendLine("    FROM TBL_REQUESTINFOMST")
                .AppendLine("   WHERE DLRCD = :DLRCD")
                .AppendLine("     AND REQCLASS = :REQCLASS")
                .AppendLine("     AND DELFLG = :DELFLG")
                .AppendLine("ORDER BY SORTNO")
            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203PriceConsultationReasonDataTable)("SC3070203_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, Me.DlrCd)
                query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, REASON_DISCOUNT)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
                Return query.GetData()
            End Using

        End Function


        ''' <summary>
        ''' 価格相談情報取得
        ''' </summary>
        ''' <returns>SC3070203DiscountInfoDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectUnderPriceConsultationInfo() As SC3070203DataSet.SC3070203PriceConsultationInfoDataTable

            Dim sql As New StringBuilder

            With sql
                .AppendLine("    SELECT /* SC3070203_003 */")
                .AppendLine("           A.ESTIMATEID")
                .AppendLine("         , A.SEQNO")
                .AppendLine("         , A.REASONID")
                .AppendLine("         , B.MSG_DLR REASON_MSG_DLR")
                .AppendLine("         , A.MANAGERACCOUNT MANAGER_ACCOUNT")
                .AppendLine("         , C.USERNAME MANAGER_NAME")
                .AppendLine("         , A.REQUESTPRICE")
                '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                .AppendLine("         , A.STAFFMEMO")
                '2015/03/06 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
                .AppendLine("         , A.REQUESTDATE")
                .AppendLine("         , A.APPROVEDPRICE")
                .AppendLine("         , A.APPROVEDDATE")
                .AppendLine("         , A.NOTICEREQID")
                .AppendLine("      FROM TBL_EST_DISCOUNTAPPROVAL A ")
                .AppendLine("         , TBL_REQUESTINFOMST B")
                .AppendLine("         , TBL_USERS C ")
                .AppendLine("         , TBL_NOTICEREQUEST D")
                .AppendLine("     WHERE B.DLRCD(+) = A.DLRCD")
                .AppendLine("       AND B.REQCLASS(+) = :REQCLASS")
                .AppendLine("       AND B.ID(+) = A.REASONID")
                .AppendLine("       AND C.ACCOUNT = A.MANAGERACCOUNT")
                .AppendLine("       AND D.NOTICEREQID = A.NOTICEREQID")
                .AppendLine("       AND A.ESTIMATEID = :ESTIMATEID")
                .AppendLine("       AND A.RESPONSEFLG = :RESPONSEFLG")
                .AppendLine("       AND D.STATUS <> :STATUS")

            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203PriceConsultationInfoDataTable)("SC3070203_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, REASON_DISCOUNT)
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Char, Me.TakingOverInfo.ESTIMATEID)
                query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, NOT_RESPONSE)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, NOT_CANCEL)

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 価格相談履歴情報取得
        ''' </summary>
        ''' <returns>SC3070203DiscountInfoDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectPriceConsultationNewestHistory() As SC3070203DataSet.SC3070203PriceConsultationInfoDataTable

            Dim sql As New StringBuilder

            With sql
                .AppendLine("    SELECT /* SC3070203_004 */")
                .AppendLine("           A.ESTIMATEID")
                .AppendLine("         , A.SEQNO")
                .AppendLine("         , A.REASONID")
                .AppendLine("         , B.MSG_DLR AS REASON_MSG_DLR")
                .AppendLine("         , A.MANAGERACCOUNT AS MANAGER_ACCOUNT")
                .AppendLine("         , C.USERNAME AS MANAGER_NAME")
                .AppendLine("         , A.REQUESTPRICE")
                .AppendLine("         , A.REQUESTDATE")
                .AppendLine("         , A.APPROVEDPRICE")
                .AppendLine("         , A.APPROVEDDATE")
                .AppendLine("         , A.NOTICEREQID")
                .AppendLine("         , C.PRESENCECATEGORY")
                .AppendLine("      FROM TBL_EST_DISCOUNTAPPROVAL A ")
                .AppendLine("         , TBL_REQUESTINFOMST B")
                .AppendLine("         , TBL_USERS C ")
                .AppendLine("     WHERE B.DLRCD(+) = A.DLRCD")
                .AppendLine("       AND B.REQCLASS(+) = :REQCLASS")
                .AppendLine("       AND B.ID(+) = A.REASONID")
                .AppendLine("       AND C.ACCOUNT = A.MANAGERACCOUNT")
                .AppendLine("       AND A.ESTIMATEID = :ESTIMATEID")
                .AppendLine("       AND A.SEQNO = (")
                .AppendLine("                        SELECT MAX(SEQNO) ")
                .AppendLine("                          FROM TBL_EST_DISCOUNTAPPROVAL A1")
                .AppendLine("                             , TBL_NOTICEREQUEST B1")
                .AppendLine("                         WHERE B1.NOTICEREQID = A1.NOTICEREQID")
                .AppendLine("                           AND A1.ESTIMATEID = :ESTIMATEID")
                .AppendLine("                           AND A1.RESPONSEFLG = :RESPONSEFLG")
                .AppendLine("                           AND B1.STATUS <> :STATUS")
                .AppendLine("                     )")

            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203PriceConsultationInfoDataTable)("SC3070203_004")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, REASON_DISCOUNT)
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Char, Me.TakingOverInfo.ESTIMATEID)
                query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, RESPONSED)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, NOT_CANCEL)

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 価格相談中件数取得
        ''' </summary>
        ''' <returns>SC3070203DiscountCountDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectUnderPriceConsultationCount() As SC3070203DataSet.SC3070203PriceConsultationCountDataTable

            Dim sql As New StringBuilder
            With sql
                .AppendLine("    SELECT /* SC3070203_005 */")
                .AppendLine("           COUNT(1) AS COUNT ")
                .AppendLine("      FROM TBL_EST_DISCOUNTAPPROVAL A")
                .AppendLine("         , TBL_NOTICEREQUEST B")
                .AppendLine("     WHERE B.NOTICEREQID = A.NOTICEREQID")
                .AppendLine("       AND A.ESTIMATEID = :ESTIMATEID")
                .AppendLine("       AND A.RESPONSEFLG = :RESPONSEFLG")
                .AppendLine("       AND B.STATUS <> :STATUS")

            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203PriceConsultationCountDataTable)("SC3070203_005")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, Me.TakingOverInfo.ESTIMATEID)
                query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, NOT_RESPONSE)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, NOT_CANCEL)

                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' 見積価格相談登録処理
        ''' </summary>
        ''' <returns>成功した場合、依頼連番
        ''' 		　失敗した場合、Executeの戻り値
        ''' </returns>
        ''' <remarks></remarks>
        Public Function InsertPriceConsultationInfo() As Long

            Dim seqno As Long = SelectSequence()
            Dim sql As New StringBuilder
            With sql


                .AppendLine("INSERT INTO /* SC3070203_006 */")
                .AppendLine("   TBL_EST_DISCOUNTAPPROVAL")
                .AppendLine("(")
                .AppendLine("     ESTIMATEID")
                .AppendLine("   , SEQNO")
                .AppendLine("   , DLRCD")
                .AppendLine("   , STRCD")
                .AppendLine("   , STAFFACCOUNT")
                .AppendLine("   , REQUESTPRICE")
                .AppendLine("   , REASONID")
                .AppendLine("   , REQUESTDATE")
                .AppendLine("   , SERIESCD")
                .AppendLine("   , SERIESNM")
                .AppendLine("   , MODELCD")
                .AppendLine("   , MODELNM")
                .AppendLine("   , MANAGERACCOUNT")
                .AppendLine("   , APPROVEDPRICE")
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
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                .AppendLine("   , STAFFMEMO")
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
                .AppendLine(")")
                .AppendLine("VALUES")
                .AppendLine("(")
                .AppendLine("     :ESTIMATEID")
                .AppendLine("   , :SEQNO")
                .AppendLine("   , :DLRCD")
                .AppendLine("   , :STRCD")
                .AppendLine("   , :STAFFACCOUNT")
                .AppendLine("   , :REQUESTPRICE")
                .AppendLine("   , :REASONID")
                .AppendLine("   , :REQUESTDATE")
                .AppendLine("   , :SERIESCD")
                .AppendLine("   , :SERIESNM")
                .AppendLine("   , :MODELCD")
                .AppendLine("   , :MODELNM")
                .AppendLine("   , :MANAGERACCOUNT")
                .AppendLine("   , :APPROVEDPRICE")
                .AppendLine("   , :MANAGERMEMO")
                .AppendLine("   , :APPROVEDDATE")
                .AppendLine("   , :RESPONSEFLG")
                .AppendLine("   , :NOTICEREQID")
                .AppendLine("   , SYSDATE")
                .AppendLine("   , SYSDATE")
                .AppendLine("   , :CREATEACCOUNT")
                .AppendLine("   , :UPDATEACCOUNT")
                .AppendLine("   , :CREATEID")
                .AppendLine("   , :UPDATEID")
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                .AppendLine("   , :STAFFMEMO")
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
                .AppendLine(")")
            End With

            Using query As New DBUpdateQuery("SC3070203_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, Me.TakingOverInfo.ESTIMATEID)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, seqno)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, Me.DlrCd)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, Me.StrCd)
                query.AddParameterWithTypeValue("STAFFACCOUNT", OracleDbType.Varchar2, Me.UserId)
                If Me.TakingOverInfo.IsREQUESTPRICENull Then
                    query.AddParameterWithTypeValue("REQUESTPRICE", OracleDbType.Double, 0)
                Else
                    query.AddParameterWithTypeValue("REQUESTPRICE", OracleDbType.Double, Me.TakingOverInfo.REQUESTPRICE)
                End If
                If Me.TakingOverInfo.IsReasonidNull Then
                    query.AddParameterWithTypeValue("REASONID", OracleDbType.Int64, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("REASONID", OracleDbType.Int64, Me.TakingOverInfo.Reasonid)
                End If
                query.AddParameterWithTypeValue("REQUESTDATE", OracleDbType.Date, DateTimeFunc.Now(Me.DlrCd))
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.Varchar2, Me.TakingOverInfo.SeriesCode)
                query.AddParameterWithTypeValue("SERIESNM", OracleDbType.Varchar2, Me.TakingOverInfo.SeriesName)
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, Me.TakingOverInfo.ModelCode)
                query.AddParameterWithTypeValue("MODELNM", OracleDbType.Varchar2, Me.TakingOverInfo.ModelName)
                query.AddParameterWithTypeValue("MANAGERACCOUNT", OracleDbType.Varchar2, Me.TakingOverInfo.ManagerAccount)
                query.AddParameterWithTypeValue("APPROVEDPRICE", OracleDbType.Double, DBNull.Value)
                query.AddParameterWithTypeValue("MANAGERMEMO", OracleDbType.Varchar2, DBNull.Value)
                query.AddParameterWithTypeValue("APPROVEDDATE", OracleDbType.Date, DBNull.Value)
                query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, NOT_RESPONSE)
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Decimal, DBNull.Value)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, Me.UserId)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, Me.UserId)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, PROG_ID)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, PROG_ID)
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                If Me.TakingOverInfo.IsRequestStaffMemoNull Then
                    query.AddParameterWithTypeValue("STAFFMEMO", OracleDbType.Varchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("STAFFMEMO", OracleDbType.Varchar2, Me.TakingOverInfo.RequestStaffMemo)
                End If
                '2015/03/05 TCS 鈴木 【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

                Dim ret As Long = query.Execute
                If ret > 0 Then
                    Return seqno
                Else
                    Return ret
                End If

            End Using
        End Function


        ''' <summary>
        ''' 通知依頼ID更新
        ''' </summary>
        ''' <returns>Boolean</returns>
        ''' <remarks></remarks>
        Public Function UpdateNoticeid() As Integer

            Dim sql As New StringBuilder
            With sql
                .AppendLine("UPDATE /* SC3070203_007 */")
                .AppendLine("       TBL_EST_DISCOUNTAPPROVAL")
                .AppendLine("   SET NOTICEREQID = :NOTICEREQID")
                .AppendLine("     , UPDATEDATE = SYSDATE")
                .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                .AppendLine("     , UPDATEID = :UPDATEID")
                .AppendLine(" WHERE ESTIMATEID = :ESTIMATEID")
                .AppendLine("   AND SEQNO = :SEQNO")
            End With

            Using query As New DBUpdateQuery("SC3070203_007")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Decimal, Me.TakingOverInfo.ESTIMATEID)
                query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, Me.TakingOverInfo.Seqno)
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Decimal, Me.TakingOverInfo.NoticeRequestid)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, Me.UserId)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, PROG_ID)

                Return query.Execute

            End Using
        End Function

        ''' <summary>
        ''' シーケンス取得処理
        ''' </summary>
        ''' <returns>Long</returns>
        ''' <remarks></remarks>
        Private Function SelectSequence() As Long

            Dim sql As New StringBuilder

            With sql
                .AppendLine("SELECT /* SC3070203_008 */")
                .AppendLine("	   SEQ_DISCOUNTAPPROVAL_SEQNO.NEXTVAL AS SEQNO")
                .AppendLine("  FROM DUAL")
            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203SequenceDataTable)("SC3070203_008")
                query.CommandText = sql.ToString()

                Dim dt As SC3070203DataSet.SC3070203SequenceDataTable = query.GetData()

                Return dt(0).SEQNO
            End Using
        End Function


        '2013/06/30 TCS 葛西 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 通知依頼ID更新ロック取得
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GetEstimateinfoLock()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

            Try
                Using query As New DBSelectQuery(Of DataTable)("SC3070203_009")

                    Dim env As New SystemEnvSetting

                    Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                    Dim sql As New StringBuilder

                    With sql
                        .AppendLine("    SELECT /* SC3070203_009 */ ")
                        .AppendLine("           1 ")
                        .AppendLine("      FROM TBL_ESTIMATEINFO ")
                        .AppendLine("     WHERE ESTIMATEID = :ESTIMATEID ")
                        .AppendLine(sqlForUpdate)
                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, Me.TakingOverInfo.ESTIMATEID)
                    query.GetData()

                End Using

            Catch ex As Exception
                Throw
            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        End Sub
        '2013/06/30 TCS 葛西 2013/10対応版　既存流用 END

        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START

        ''' <summary>
        ''' 見積値引き額を取得する
        ''' </summary>
        ''' <returns>値引き額</returns>
        ''' <remarks></remarks>
        Public Function GetEstDiscountPrice() As Integer

            Dim sql As New StringBuilder
            With sql
                .AppendLine("    SELECT /* SC3070203_010 */")
                .AppendLine("           DISCOUNTPRICE ")
                .AppendLine("      FROM TBL_ESTIMATEINFO A")
                .AppendLine("     WHERE A.ESTIMATEID = :ESTIMATEID")

            End With

            Using query As New DBSelectQuery(Of SC3070203DataSet.SC3070203DiscountPriceDataTable)("SC3070203_010")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, Me.TakingOverInfo.ESTIMATEID)
                'Return CInt(query.GetData().Item(0).DISCOUNTPRICE)
                Dim dt As SC3070203DataSet.SC3070203DiscountPriceDataTable = query.GetData()
                If Not dt.Item(0).IsDISCOUNTPRICENull Then
                    Return CInt(dt.Item(0).DISCOUNTPRICE)
                Else
                    Return 0
                End If
            End Using

        End Function

        ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END

#End Region

    End Class

End Namespace


