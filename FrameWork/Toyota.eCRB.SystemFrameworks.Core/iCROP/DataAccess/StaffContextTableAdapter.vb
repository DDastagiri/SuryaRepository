Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class StaffContextTableAdapter

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
#Region "定数"

        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_SYSTEM = "STAFFCONTEXT"

#End Region
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        Private Sub New()

        End Sub

        Public Shared Function GetUserInfo(ByVal account As String) As StaffContextDataSet.USERINFODataTable

            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of StaffContextDataSet.USERINFODataTable)("STAFFCONTEXT_005")

                Dim sql As New StringBuilder

                '2013/11/26 TCS 河原 Aカード情報相互連携開発 START
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* STAFFCONTEXT_005 */ ")
                    .Append("        A.ACCOUNT ")
                    .Append("      , A.USERNAME ")
                    .Append("      , A.DLRCD ")
                    .Append("      , B.DLR_NAME AS DLRNM_LOCAL ")
                    .Append("      , A.STRCD ")
                    .Append("      , C.BRN_NAME AS STRNM_LOCAL ")
                    .Append("      , A.OPERATIONCODE ")
                    .Append("      , D.OPERATIONNAME ")
                    .Append("      , NVL(F.ORGNZ_ID, 0) AS TEAMCD ")
                    .Append("      , E.ORGNZ_SHORT_NAME AS TEAMNAME ")
                    .Append("      , CASE WHEN E.ORGNZ_SC_FLG='1' AND F.BRN_MANAGER_FLG = '1' AND F.BRN_SC_FLG = '1' THEN 1 ELSE 0 END AS LEADERFLG ")
                    If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
                        .Append("      , A.PRESENCECATEGORY ")
                        .Append("      , A.PRESENCEDETAIL ")
                        .Append("      , A.PRESENCEUPDATEDATE ")
                    End If
                    .Append(" FROM   TBL_USERS A ")
                    .Append("      , TB_M_DEALER B ")
                    .Append("      , TB_M_BRANCH C ")
                    .Append("      , TBL_OPERATIONTYPE D ")
                    .Append("      , TB_M_ORGANIZATION E ")
                    .Append("      , TB_M_STAFF F ")
                    .Append(" WHERE  RTRIM(A.DLRCD) = B.DLR_CD (+) ")
                    .Append(" AND    RTRIM(A.DLRCD) = C.DLR_CD (+) ")
                    .Append(" AND    RTRIM(A.STRCD) = C.BRN_CD (+) ")
                    .Append(" AND    A.OPERATIONCODE = D.OPERATIONCODE(+) ")
                    .Append(" AND    A.DLRCD = D.DLRCD(+) ")
                    .Append(" AND    RTRIM(A.ACCOUNT) = F.STF_CD(+) ")
                    .Append(" AND    F.ORGNZ_ID = E.ORGNZ_ID(+) ")
                    .Append(" AND    B.INUSE_FLG(+) = '1' ")
                    .Append(" AND    C.INUSE_FLG(+) = '1' ")
                    .Append(" AND    D.STRCD(+) = :STRCD ")
                    .Append(" AND    E.INUSE_FLG(+) = '1' ")
                    .Append(" AND    A.ACCOUNT = :ACCOUNT ")
                    .Append(" AND    A.DELFLG = '0' ")
                    .Append(" AND    F.INUSE_FLG(+) = '1' ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, ConstantBranchCD.BranchHO)

                Dim dt As StaffContextDataSet.USERINFODataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END
                '2013/11/26 TCS 河原 Aカード情報相互連携開発 END

            End Using

        End Function

        Public Shared Function IsUser(ByVal account As String, ByVal pass As String) As Integer

            Using query As New DBSelectQuery(Of StaffContextDataSet.USERINFODataTable)("STAFFCONTEXT_004")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* STAFFCONTEXT_004 */ ")
                    .Append("        COUNT(1) ")
                    .Append("   FROM TBL_USERS A ")
                    .Append("  WHERE A.ACCOUNT = :ACCOUNT ")
                    .Append("    AND A.PASSWORD = :PASSWORD ")
                    .Append("    AND A.DELFLG = '0' ")
                    .Append("    AND ROWNUM <= 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("PASSWORD", OracleDbType.NVarchar2, pass)

                Return query.GetCount()

            End Using

        End Function

'2013/06/30 TCS 山田 2013/10対応版　既存流用 START DEL
'2013/06/30 TCS 山田 2013/10対応版　既存流用 DEL

        Public Shared Function GetTimeDiffData(ByVal dlrCD As String, ByVal strCD As String) As StaffContextDataSet.TIMEDIFFDataTable

           ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 START
           ' ======================== ログ出力 開始 ========================
           Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                     " {0}_Start",
                                     MethodBase.GetCurrentMethod.Name))
           ' ======================== ログ出力 終了 ========================
           ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

           Using query As New DBSelectQuery(Of StaffContextDataSet.TIMEDIFFDataTable)("STAFFCONTEXT_001")

                Dim sql As New StringBuilder

                '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
                With sql
                    .Append(" SELECT /* STAFFCONTEXT_001 */ ")
                    .Append("        T1.TIME_DIFF AS TIMEDIFF ")
                    .Append(" FROM   TB_M_BRANCH T1 ")
                    .Append(" WHERE  T1.DLR_CD = :DLRCD ")
                    .Append(" AND    T1.BRN_CD = (SELECT NVL((SELECT T2.BRN_CD FROM TB_M_BRANCH T2 WHERE T2.DLR_CD = :DLRCD AND T2.BRN_CD = :STRCD), '000') FROM DUAL) ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strCD)

                Dim dt As StaffContextDataSet.TIMEDIFFDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 山田 2013/10対応版　既存流用 END

            End Using

        End Function

        Public Shared Function UpdatePresence(ByVal account As String, ByVal presenceCategory As String, ByVal presenceDetail As String, ByVal catDateUpFlg As Boolean) As Integer
            Using query As New DBUpdateQuery("STAFFCONTEXT_007")
                Dim sql As New StringBuilder

                With sql
                    .Append(" UPDATE /* STAFFCONTEXT_007 */ ")
                    .Append("     TBL_USERS ")
                    .Append(" SET PRESENCECATEGORY = :PRESENCECATEGORY ")
                    .Append("    ,PRESENCEDETAIL = :PRESENCEDETAIL ")
                    .Append("    ,PRESENCEUPDATEDATE = SYSDATE ")
                    If (catDateUpFlg) Then
                        .Append("    ,PRESENCECATEGORYDATE = SYSDATE ")
                    End If
                    .Append("    ,UPDATEDATE = SYSDATE ")
                    .Append("    ,UPDATEACCOUNT = ACCOUNT ")
                    .Append(" WHERE  ACCOUNT = :ACCOUNT ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("PRESENCECATEGORY", OracleDbType.Char, presenceCategory)
                query.AddParameterWithTypeValue("PRESENCEDETAIL", OracleDbType.Char, presenceDetail)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)

                Return query.Execute()

            End Using
        End Function

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ''' <summary>
        ''' ユーザマスタのロック取得
        ''' </summary>
        ''' <param name="account">ユーザアカウント</param>
        ''' <remarks></remarks>
        Public Shared Sub GetUsersLock(ByVal account As String)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start",
                                      MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================

                Using query As New DBSelectQuery(Of DataTable)("STAFFCONTEXT_008")

                    Dim env As New SystemEnvSetting
                    Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                    Dim sql As New StringBuilder

                    With sql
                        .Append(" SELECT /* STAFFCONTEXT_008 */ ")
                        .Append("        1 ")
                        .Append("   FROM TBL_USERS ")
                        .Append("  WHERE ACCOUNT = :ACCOUNT ")
                        .Append(sqlForUpdate)
                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                    query.GetData()

                End Using

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, 1))
            ' ======================== ログ出力 終了 ========================

        End Sub
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '2013/11/26 TCS 河原 Aカード情報相互連携開発 START
        ''' <summary>
        ''' 店舗の全マネージャ・チームリーダー取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="brncd">店舗コード</param>
        ''' <param name="stfcd">スタッフコード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetBranchSuperiors(ByVal dlrcd As String, ByVal brncd As String, ByVal stfcd As String) As StaffContextDataSet.BRANCHSUPERIORSDataTable

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_Start", MethodBase.GetCurrentMethod.Name))
            ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of StaffContextDataSet.BRANCHSUPERIORSDataTable)("STAFFCONTEXT_009")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* STAFFCONTEXT_009 */ ")
                    .Append("    T1.STF_CD, ")
                    .Append("    T3.OPERATIONCODE, ")
                    .Append("    T1.ORGNZ_ID ")
                    .Append("FROM ")
                    .Append("    TB_M_STAFF T1, ")
                    .Append("    TB_M_ORGANIZATION T2, ")
                    .Append("    TBL_USERS T3 ")
                    .Append("WHERE ")
                    .Append("        T1.ORGNZ_ID = T2.ORGNZ_ID(+) ")
                    .Append("    AND T1.DLR_CD = :DLR_CD ")
                    .Append("    AND T1.BRN_CD = :BRN_CD ")
                    .Append("    AND T1.INUSE_FLG = '1' ")
                    .Append("    AND T1.STF_CD = RTRIM(T3.ACCOUNT) ")
                    .Append("    AND T1.STF_CD <> :STF_CD ")
                    .Append("    AND T3.DELFLG <> '1' ")
                    .Append("    AND ((T1.BRN_MANAGER_FLG = '1' AND T1.BRN_SC_FLG = '1' AND T2.ORGNZ_SC_FLG = '1' AND T2.INUSE_FLG = '1' AND T3.OPERATIONCODE = 8) OR T3.OPERATIONCODE = 7) ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Varchar2, dlrcd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Varchar2, brncd)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.Varchar2, stfcd)

                Dim dt As StaffContextDataSet.BRANCHSUPERIORSDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture, " {0}_End, Return:[{1}]", MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt

            End Using

        End Function
        '2013/11/26 TCS 河原 Aカード情報相互連携開発 END

    End Class

End Namespace
