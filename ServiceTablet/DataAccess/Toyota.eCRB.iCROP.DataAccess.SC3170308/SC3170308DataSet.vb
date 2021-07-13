'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170308DataSet.vb
'─────────────────────────────────────
'機能： 承認者選択画面 データセット
'補足： 
'作成： 2014/01/21 TMEJ小澤	初版作成
'更新： 
'─────────────────────────────────────

Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.AddRepair.AddRepairConfirm.DataAccess.SC3170308DataSet
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Namespace SC3170308DataSetTableAdapters
    Public Class SC3170308DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' カテゴリ－(小カテゴリー)
        ''' </summary>
        Private Const PresenceDetail As String = "0"

        ''' <summary>
        ''' DELFLAG
        ''' </summary>
        Private Const DelFlg As String = "0"

#End Region

#Region "メイン"

        ''' <summary>
        ''' ユーザー情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>ユーザー情報</returns>
        ''' <remarks></remarks>
        Public Function GetUserInfo(ByVal inDealerCode As String, _
                                    ByVal inStoreCode As String) As SC3170308UserInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inStoreCode = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inStoreCode))

            'データ格納用
            Dim dt As SC3170308UserInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            sql.AppendLine("   SELECT  /* SC3170308_001 */ ")
            sql.AppendLine("           CASE ")
            sql.AppendLine("                WHEN T1.OPERATIONCODE = :OPERATIONCODE_FM THEN 1 ")
            sql.AppendLine("                WHEN T1.OPERATIONCODE = :OPERATIONCODE_CHT THEN 2 ")
            sql.AppendLine("                WHEN T1.OPERATIONCODE = :OPERATIONCODE_CT THEN 3 ")
            sql.AppendLine("                ELSE 99 ")
            sql.AppendLine("           END AS OPERATIONCODE ")
            sql.AppendLine("          ,T1.ACCOUNT  ")
            sql.AppendLine("          ,T1.USERNAME ")
            sql.AppendLine("          ,NVL(T1.PRESENCECATEGORY, :PRESENCECATEGORYOFF) AS PRESENCECATEGORY ")
            sql.AppendLine("          ,CASE ")
            sql.AppendLine("                WHEN T1.PRESENCECATEGORY = :PRESENCECATEGORYSTANDBY THEN 1 ")
            sql.AppendLine("                WHEN T1.PRESENCECATEGORY = :PRESENCECATEGORYLEAVING THEN 2 ")
            sql.AppendLine("                WHEN T1.PRESENCECATEGORY = :PRESENCECATEGORYOFF THEN 3 ")
            sql.AppendLine("                ELSE 3 ")
            sql.AppendLine("           END AS CATEGORY ")
            sql.AppendLine("     FROM  TBL_USERS T1 ")
            sql.AppendLine("    WHERE  T1.DLRCD = :DLRCD ")
            sql.AppendLine("      AND  T1.STRCD = :STRCD ")
            sql.AppendLine("      AND  ((T1.PRESENCECATEGORY IN (:PRESENCECATEGORYSTANDBY, ")
            sql.AppendLine("                                     :PRESENCECATEGORYLEAVING, ")
            sql.AppendLine("                                     :PRESENCECATEGORYOFF) ")
            sql.AppendLine("      AND  T1.PRESENCEDETAIL = :PRESENCEDETAIL) ")
            sql.AppendLine("       OR  T1.PRESENCECATEGORY IS NULL) ")
            sql.AppendLine("      AND  T1.OPERATIONCODE IN (:OPERATIONCODE_CT, :OPERATIONCODE_FM, :OPERATIONCODE_CHT) ")
            sql.AppendLine("      AND  T1.DELFLG = :DELFLG ")
            sql.AppendLine(" ORDER BY  CATEGORY ASC ")
            sql.AppendLine("          ,OPERATIONCODE ASC ")
            sql.AppendLine("          ,T1.ACCOUNT ASC ")

            Using query As New DBSelectQuery(Of SC3170308UserInfoDataTable)("SC3170308_001")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                query.AddParameterWithTypeValue("PRESENCECATEGORYSTANDBY", OracleDbType.Char, PresenceCategory.Standby)
                query.AddParameterWithTypeValue("PRESENCECATEGORYLEAVING", OracleDbType.Char, PresenceCategory.Suspend)
                query.AddParameterWithTypeValue("PRESENCECATEGORYOFF", OracleDbType.Char, PresenceCategory.Offline)
                query.AddParameterWithTypeValue("PRESENCEDETAIL", OracleDbType.Char, PresenceDetail)
                query.AddParameterWithTypeValue("OPERATIONCODE_CT", OracleDbType.Long, 55)
                query.AddParameterWithTypeValue("OPERATIONCODE_FM", OracleDbType.Long, 58)
                query.AddParameterWithTypeValue("OPERATIONCODE_CHT", OracleDbType.Long, 62)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlg)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

#End Region

    End Class

End Namespace

Partial Class SC3170308DataSet
End Class
