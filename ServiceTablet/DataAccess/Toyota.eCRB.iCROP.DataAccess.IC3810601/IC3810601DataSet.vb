'-------------------------------------------------------------------------
'Partial Class IC3810601DataSet.vb
'-------------------------------------------------------------------------
'機能：ユーザーステータス取得取得API
'補足：
'作成：2012/07/04 TMEJ 河原 【servive_2】
'更新：
'
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace IC3810601DataSetTableAdapters

    ''' <summary>
    ''' ユーザーステータス取得APIデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3810601TableAdapter
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

#Region "メソッド"

        ''' <summary>
        ''' ユーザーステータス情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inOperationCodeList">操作権限コードリスト</param>
        ''' <returns>ユーザのスタッフ情報</returns>
        ''' <remarks></remarks>
        Public Function GetDBAcknowledgeStaffList(ByVal inDealerCode As String, _
                                                  ByVal inStoreCode As String, _
                                                  ByVal inOperationCodeList As List(Of Long)) _
                                                  As IC3810601DataSet.AcknowledgeStaffListDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                      , inDealerCode, inStoreCode, inOperationCodeList.Count))

            Using query As New DBSelectQuery(Of IC3810601DataSet.AcknowledgeStaffListDataTable)("IC3810601_001")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("   SELECT  /* IC3810601_001 */ ")
                    .AppendLine("           T1.OPERATIONCODE ")
                    .AppendLine("          ,T1.ACCOUNT  ")
                    .AppendLine("          ,T1.USERNAME ")
                    .AppendLine("          ,NVL(T1.PRESENCECATEGORY, :PRESENCECATEGORYOFF) AS PRESENCECATEGORY")
                    .AppendLine("          ,DECODE(T1.PRESENCECATEGORY, ")
                    .AppendLine("                 :PRESENCECATEGORYSTANDBY, '1', ")
                    .AppendLine("                 :PRESENCECATEGORYLEAVING, '2', ")
                    .AppendLine("                 :PRESENCECATEGORYOFF, '3', ")
                    .AppendLine("                 NULL, '3' ")
                    .AppendLine("                  ) AS CATEGORY ")
                    .AppendLine("     FROM  TBL_USERS T1 ")
                    .AppendLine("    WHERE  T1.DLRCD = :DLRCD ")
                    .AppendLine("      AND  T1.STRCD = :STRCD ")
                    .AppendLine("      AND  ((T1.PRESENCECATEGORY IN (:PRESENCECATEGORYSTANDBY, ")
                    .AppendLine("                                   :PRESENCECATEGORYLEAVING, ")
                    .AppendLine("                                   :PRESENCECATEGORYOFF) ")
                    .AppendLine("      AND  T1.PRESENCEDETAIL = :PRESENCEDETAIL) ")
                    .AppendLine("       OR  T1.PRESENCECATEGORY IS NULL) ")

                    '操作権限コードのリスト分SQLを作成
                    .AppendLine("      AND  T1.OPERATIONCODE IN (")
                    Dim i As Integer = 1
                    For Each operationCode As Long In inOperationCodeList
                        .AppendLine("       :OPERATIONCODE" & CStr(i))
                        query.AddParameterWithTypeValue("OPERATIONCODE" & CStr(i), OracleDbType.Int64, operationCode)
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OPERATIONCODE = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , operationCode))
                        If Not inOperationCodeList.Count() = i Then
                            .AppendLine(",")
                        End If
                        i = i + 1
                    Next
                    .AppendLine(" ) ")
                    .AppendLine("      AND  T1.DELFLG = :DELFLG ")

                    .AppendLine(" ORDER BY  CATEGORY ASC ")
                    .AppendLine("          ,T1.OPERATIONCODE DESC ")
                    .AppendLine("          ,T1.ACCOUNT ASC ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                query.AddParameterWithTypeValue("PRESENCECATEGORYSTANDBY", OracleDbType.Char, PresenceCategory.Standby)
                query.AddParameterWithTypeValue("PRESENCECATEGORYLEAVING", OracleDbType.Char, PresenceCategory.Suspend)
                query.AddParameterWithTypeValue("PRESENCECATEGORYOFF", OracleDbType.Char, PresenceCategory.Offline)
                query.AddParameterWithTypeValue("PRESENCEDETAIL", OracleDbType.Char, PresenceDetail)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlg)

                Dim dt As IC3810601DataSet.AcknowledgeStaffListDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt
            End Using
        End Function

#End Region

    End Class
End Namespace

Partial Class IC3810601DataSet
End Class
