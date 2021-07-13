'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070207DataSet.vb
'─────────────────────────────────────
'機能： 注文承認
'補足： 
'作成： 2013/12/10 TCS 山口  Aカード情報相互連携開発
'更新： 2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）
'更新： 2014/08/01 TCS 山口 NextStep BTS-74
'─────────────────────────────────────
Imports System.Text
Imports System.Reflection.MethodBase
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public NotInheritable Class SC3070207TableAdapter

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ProgramId As String = "SC3070207"
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

    ''' <summary>
    ''' 商談情報更新対象
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SalesTemp
        Sales
        SalesHis
        SalesTemp
    End Enum

#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 見積情報更新ロック取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>EstimateinfoLockDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimateinfoLock(ByVal estimateId As Long) As SC3070207DataSet.SC3070207EstimateinfoLockDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Try
            Dim env As New SystemEnvSetting
            Dim sql As New StringBuilder
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            With sql
                .AppendLine(" SELECT /* SC3070207_001 */ ")
                .AppendLine("        FLLWUPBOX_SEQNO ")
                .AppendLine("      , CONTRACT_APPROVAL_STATUS ")
                .AppendLine("      , CONTRACT_APPROVAL_REQUESTSTAFF ")
                .AppendLine("   FROM TBL_ESTIMATEINFO    ")
                .AppendLine("  WHERE ESTIMATEID = :ESTIMATEID ")
                .AppendLine(sqlForUpdate)
            End With

            Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207EstimateinfoLockDataTable)("SC3070207_001")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

                Return query.GetData()
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 商談情報取得、商談ロック取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="tempFlg">商談情報対象</param>
    ''' <param name="lockFlg">ロックフラグ True:ロックする False:ロックしない</param>
    ''' <returns>SalesDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSales(ByVal salesId As Decimal, _
                                    ByVal tempFlg As SalesTemp, _
                                    Optional ByVal lockFlg As Boolean = False) As SC3070207DataSet.SC3070207SalesDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Try
            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* SC3070207_002 */ ")
                .AppendLine("        ACARD_NUM ")
                If SalesTemp.Sales = tempFlg Then
                    '商談情報
                    .AppendLine("   FROM TB_T_SALES ")
                ElseIf SalesTemp.SalesHis = tempFlg Then
                    '商談情報(History)
                    .AppendLine("   FROM TB_H_SALES ")
                Else
                    '商談一時情報
                    .AppendLine("   FROM TB_T_SALES_TEMP ")
                End If
                .AppendLine("  WHERE SALES_ID = :SALES_ID ")
                If lockFlg Then
                    'FOR UPDATE 追加
                    Dim env As New SystemEnvSetting
                    Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                    .AppendLine(sqlForUpdate)
                End If
            End With

            Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207SalesDataTable)("SC3070207_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

                Return query.GetData()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 商談情報取得、商談ロック取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="tempFlg">商談情報対象</param>
    ''' <param name="lockFlg">ロックフラグ True:ロックする False:ロックしない</param>
    ''' <returns>SalesDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSalesock(ByVal salesId As Decimal, _
                                       ByVal tempFlg As SalesTemp, _
                                        Optional ByVal lockFlg As Boolean = False) As SC3070207DataSet.SC3070207SalesDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Try
            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT /* SC3070207_002 */ ")
                .AppendLine("        ACARD_NUM ")
                If SalesTemp.Sales = tempFlg Then
                    '商談情報
                    .AppendLine("   FROM TB_T_SALES ")
                Else
                    '商談一時情報
                    .AppendLine("   FROM TB_T_SALES_TEMP ")
                End If
                .AppendLine("  WHERE SALES_ID = :SALES_ID ")
                If lockFlg Then
                    'FOR UPDATE 追加
                    Dim env As New SystemEnvSetting
                    Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()
                    .AppendLine(sqlForUpdate)
                End If
            End With

            Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207SalesDataTable)("SC3070207_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

                Return query.GetData()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' A-Card番号更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="aCardNum">A-Card番号</param>
    ''' <param name="stfcd">更新スタッフ</param>
    ''' <param name="tempFlg">商談情報対象</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateACardNo(ByVal salesId As Decimal, _
                                         ByVal aCardNum As String, _
                                         ByVal stfcd As String, _
                                         ByVal tempFlg As SalesTemp) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder
        With sql
            .AppendLine(" UPDATE /* SC3070207_003 */ ")
            If SalesTemp.Sales = tempFlg Then
                '商談情報
                .AppendLine("        TB_T_SALES ")
            Else
                '商談一時情報
                .AppendLine("        TB_T_SALES_TEMP ")
            End If
            .AppendLine("    SET ACARD_NUM = :ACARD_NUM, ")
            .AppendLine("        ROW_UPDATE_DATETIME = SYSDATE, ")
            .AppendLine("        ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT, ")
            .AppendLine("        ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION, ")
            .AppendLine("        ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
            .AppendLine("  WHERE SALES_ID = :SALES_ID ")
        End With

        Using query As New DBUpdateQuery("SC3070207_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACARD_NUM", OracleDbType.Char, aCardNum)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.Char, stfcd)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.Char, ProgramId)
            query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.Execute
        End Using
    End Function

    ''' <summary>
    ''' 見積情報更新
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="contactApprovalStatus">契約承認ステータス</param>
    ''' <param name="stfcd">更新スタッフ</param>
    ''' <param name="dlrcd">販売店コード(否認時は省略)</param>
    ''' <param name="contractNo">契約書No.(否認時は省略)</param>
    ''' <param name="contractFlg">契約状況フラグ(否認時は省略)</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateEstimateInfo(ByVal estimateId As Long, _
                                              ByVal contactApprovalStatus As String, _
                                              ByVal stfcd As String, _
                                              Optional ByVal dlrcd As String = "", _
                                              Optional ByVal contractNo As String = "", _
                                              Optional ByVal contractFlg As String = "") As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" UPDATE /* SC3070207_004 */ ")
            .AppendLine("        TBL_ESTIMATEINFO ")
            .AppendLine("    SET ")
            .AppendLine("        CONTRACT_APPROVAL_STATUS = :CONTRACT_APPROVAL_STATUS, ")
            If StatusApproval.Equals(contactApprovalStatus) Then
                '承認時
                .AppendLine("        CONTRACTNO = :CONTRACTNO, ")
                .AppendLine("        CONTRACTFLG = :CONTRACTFLG, ")
                .AppendLine("        CONTRACTDATE = :CONTRACTDATE, ")
            Else
                '否認時
                .AppendLine("        CONTRACT_APPROVAL_STAFF = :CONTRACT_APPROVAL_STAFF, ")
                .AppendLine("        CONTRACT_APPROVAL_REQUESTSTAFF = :CONTRACT_APPROVAL_REQUESTSTAFF, ")
                .AppendLine("        CONTRACT_APPROVAL_REQUESTDATE = :CONTRACT_APPROVAL_REQUESTDATE, ")
            End If
            .AppendLine("        UPDATEDATE = SYSDATE, ")
            .AppendLine("        UPDATEACCOUNT = :UPDATEACCOUNT, ")
            .AppendLine("        UPDATEID = :UPDATEID ")
            .AppendLine("  WHERE ESTIMATEID = :ESTIMATEID ")

        End With

        Using query As New DBUpdateQuery("SC3070207_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STATUS", OracleDbType.Char, contactApprovalStatus)
            If StatusApproval.Equals(contactApprovalStatus) Then
                '承認時
                query.AddParameterWithTypeValue("CONTRACTNO", OracleDbType.Char, contractNo)
                query.AddParameterWithTypeValue("CONTRACTFLG", OracleDbType.Char, contractFlg)
                query.AddParameterWithTypeValue("CONTRACTDATE", OracleDbType.Date, DateTimeFunc.Now(dlrcd))
            Else
                '否認時
                query.AddParameterWithTypeValue("CONTRACT_APPROVAL_STAFF", OracleDbType.Char, String.Empty)
                query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTSTAFF", OracleDbType.Char, String.Empty)
                query.AddParameterWithTypeValue("CONTRACT_APPROVAL_REQUESTDATE", OracleDbType.Date, DBNull.Value)
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, stfcd)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, ProgramId)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.Execute
        End Using
    End Function

    ''' <summary>
    ''' 見積支払情報削除
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="paymentMethod">支払方法区分</param>
    ''' <param name="stfcd">更新スタッフ</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteEstPaymentinfo(ByVal estimateId As Long, _
                                                ByVal paymentMethod As String, _
                                                ByVal stfcd As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .AppendLine(" UPDATE /* SC3070207_005 */ ")
            .AppendLine("        TBL_EST_PAYMENTINFO ")
            .AppendLine("    SET DELFLG = '1', ")
            .AppendLine("        UPDATEACCOUNT = :UPDATEACCOUNT, ")
            .AppendLine("        UPDATEID = :UPDATEID, ")
            .AppendLine("        UPDATEDATE = SYSDATE ")
            .AppendLine("  WHERE ESTIMATEID = :ESTIMATEID ")
            .AppendLine("    AND PAYMENTMETHOD = :PAYMENTMETHOD ")
        End With

        Using query As New DBUpdateQuery("SC3070207_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, stfcd)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, ProgramId)
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)
            query.AddParameterWithTypeValue("PAYMENTMETHOD", OracleDbType.Char, paymentMethod)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            Return query.Execute
        End Using
    End Function

    ''' <summary>
    ''' 基幹システムの外装色の取得します。
    ''' </summary>
    ''' <param name="modelCode">モデルコード</param>
    ''' <param name="bodyClrCd">外装色コード</param>
    ''' <returns>基幹システムデータテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetColorCode(ByVal modelCode As String,
                                 ByVal bodyClrCD As String) As SC3070207DataSet.SC3070207MstextEriorDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207MstextEriorDataTable)("SC3070207_006")

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3070207_006 */ ")
                .Append("        COLOR_CD ")
                .Append(" FROM   TBL_MSTEXTERIOR ")
                .Append(" WHERE  VCLMODEL_CODE = :VCLMODEL_CODE ")
                .Append(" AND    BODYCLR_CD = :BODYCLR_CD ")
            End With

            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("VCLMODEL_CODE", OracleDbType.Varchar2, modelCode)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, bodyClrCD)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 顧客テーブルから顧客情報を取得します。
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <returns>見積情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerInfo(ByVal dlrcd As String, ByVal crcustid As Decimal) As SC3070207DataSet.SC3070207CustomerInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)
        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207CustomerInfoDataTable)("SC3070207_007")

            Dim sql As New StringBuilder

            With sql
                .Append(" SELECT /* SC3070207_007 */ ")
                .Append("        T1.DMS_CST_CD_DISP ")
                .Append("      , T1.NEWCST_CD ")
                .Append("      , T1.CST_GENDER AS SEX ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）START
                '.Append("      , T1.CST_BIRTH_DATE AS BIRTHDAY ")
                .Append("  , CASE WHEN  T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE  T1.CST_BIRTH_DATE END AS BIRTHDAY ")
                '2014/07/24 TCS 外崎 不具合対応（TMT 切替BTS-89）END
                '2014/08/01 TCS 山口 NextStep BTS-74 START
                .Append("      , T1.CST_DOMICILE ")
                .Append("      , T1.CST_COUNTRY ")
                '2014/08/01 TCS 山口 NextStep BTS-74 END
                .Append("      , T2.CST_TYPE ")
                .Append("   FROM TB_M_CUSTOMER T1 ")
                .Append("      , TB_M_CUSTOMER_DLR T2 ")
                .Append("  WHERE T1.CST_ID = T2.CST_ID ")
                .Append("    AND T2.DLR_CD = :DLR_CD ")
                .Append("    AND T1.CST_ID = :CRCUST_ID ")
            End With

            query.CommandText = sql.ToString()

            'バインド変数
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)  '販売店コード
            query.AddParameterWithTypeValue("CRCUST_ID", OracleDbType.Decimal, crcustid)  '活動先顧客コード

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' ユーザマスタからユーザー情報を取得
    ''' </summary>
    ''' <param name="account">アカウント</param>
    ''' <returns>UsersInfoDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetUsersInfo(ByVal account As String) As SC3070207DataSet.SC3070207UsersInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3070207_008 */ ")
            .Append("        USERNAME ")
            .Append("   FROM TBL_USERS ")
            .Append("  WHERE ACCOUNT = :ACCOUNT ")
        End With

        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207UsersInfoDataTable)("SC3070207_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 見積支払情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>PaymentInfoDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPaymentInfo(ByVal estimateId As Long) As SC3070207DataSet.SC3070207PaymentInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3070207_009 */ ")
            .Append("        PAYMENTMETHOD, ")
            .Append("        DEPOSIT, ")
            .Append("        CASE WHEN DEPOSITPAYMENTMETHOD IS NULL THEN PAYMENTMETHOD ")
            .Append("             ELSE DEPOSITPAYMENTMETHOD ")
            .Append("        END AS DEPOSITPAYMENTMETHOD ")
            .Append("   FROM TBL_EST_PAYMENTINFO ")
            .Append("  WHERE ESTIMATEID = :ESTIMATEID ")
            .Append("    AND SELECTFLG = '1' ")
        End With

        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207PaymentInfoDataTable)("SC3070207_009")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 通知依頼情報を取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <returns>NoticeRequestInfoDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNoticeRequestInfo(ByVal noticeReqId As Long) As SC3070207DataSet.SC3070207NoticeRequestInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim sql As New StringBuilder

        With sql
            .Append(" SELECT /* SC3070207_010 */ ")
            .Append("        CRCUSTID ")
            .Append("      , CUSTOMNAME ")
            .Append("      , CSTKIND ")
            .Append("      , CUSTOMERCLASS ")
            .Append("      , SALESSTAFFCD ")
            .Append("      , VCLID ")
            .Append("      , FLLWUPBOXSTRCD ")
            .Append("      , FLLWUPBOX ")
            .Append("   FROM TBL_NOTICEREQUEST ")
            .Append("  WHERE NOTICEREQID = :NOTICEREQID ")
        End With

        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207NoticeRequestInfoDataTable)("SC3070207_010")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeReqId)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return query.GetData()
        End Using
    End Function

    '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
    ''' <summary>
    ''' マネージャー回答登録
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="contractApprovalSeqId">依頼連番</param>
    ''' <param name="managerAccount">マネージャアカウント</param>
    ''' <param name="managerMemo">マネージャ入力メモ</param>
    ''' <param name="contactApprovalStatus">契約承認ステータス</param>
    ''' <returns>成功 : True / 失敗 : False</returns>
    Public Shared Function RegistAnswer(ByVal estimateId As Decimal, _
                                        ByVal contractApprovalSeqId As Long, _
                                        ByVal managerAccount As String, _
                                        ByVal managerMemo As String, _
                                        ByVal contactApprovalStatus As String) As Boolean

        Using query As New DBUpdateQuery("SC3070207_011")
            Dim responseFlg As String
            If (StatusDenial.Equals(contactApprovalStatus)) Then
                responseFlg = "2"
            Else
                responseFlg = "1"
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("UPDATE /* SC3070207_011 */ ")
                .Append("       TBL_EST_CONTRACTAPPROVAL ")
                .Append("   SET MANAGERACCOUNT = :MANAGERACCOUNT ")     'マネージャアカウント
                .Append("     , MANAGERMEMO = :MANAGERMEMO ")           'マネージャ入力メモ
                .Append("     , APPROVEDDATE = SYSDATE ")               '承認日時
                .Append("     , RESPONSEFLG = :RESPONSEFLG ")           '返答フラグ
                .Append("     , UPDATEDATE = SYSDATE ")                 '更新日
                .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")       '更新ユーザアカウント
                .Append("     , UPDATEID = :UPDATEID ")                 '更新機能ID
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")             '見積管理ID
                .Append("   AND SEQNO = :SEQNO ")                       '依頼連番
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)             '見積管理ID
            query.AddParameterWithTypeValue("SEQNO", OracleDbType.Int64, contractApprovalSeqId)       '依頼連番

            query.AddParameterWithTypeValue("MANAGERACCOUNT", OracleDbType.Varchar2, managerAccount) 'マネージャアカウント
            query.AddParameterWithTypeValue("MANAGERMEMO", OracleDbType.NVarchar2, managerMemo)      'マネージャ入力メモ
            query.AddParameterWithTypeValue("RESPONSEFLG", OracleDbType.Char, responseFlg)        '返答フラグ

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, managerAccount)   '更新ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ProgramId)             '更新機能ID

            If query.Execute() > 0 Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' 注文承認情報の依頼連番を取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>注文承認情報の依頼連番</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractApprovalSequence(ByVal estimateId As Long) As SC3070207DataSet.SC3070207EstContractApprovalDataTable
        Using query As New DBSelectQuery(Of SC3070207DataSet.SC3070207EstContractApprovalDataTable)("SC3070207_012")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070207_012 */ ")
                .Append("       MAX(SEQNO) SEQNO ")                  '依頼連番
                .Append("  FROM TBL_EST_CONTRACTAPPROVAL ")
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            End With
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Int64, estimateId)       '依頼種別

            Return query.GetData()
        End Using
    End Function
    '2015/03/13 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
End Class
