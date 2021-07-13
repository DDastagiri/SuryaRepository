'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'DmsCodeMapTableAdap.vb
'─────────────────────────────────────
'機能： DmsCodeMap
'補足： 
'作成： 2014/08/29 TCS 武田 Next追加要件
'       2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応
'─────────────────────────────────────
Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    ''' <summary>
    ''' TB_M_DMS_CODE_MAPから基幹コードマップを取得する共通クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class DmsCodeMapTableAdapter

        Private Sub New()

        End Sub

#Region "GetDmsCodeMapDataTable"
        ''' <summary>
        ''' TB_M_DMS_CODE_MAPから基幹コードマップを取得。
        ''' </summary>
        ''' <param name="dmsCodeType">基幹コード区分</param>
        ''' <param name="icropCode1">i-CROPコード1</param>
        ''' <param name="icropCode2">i-CROPコード2</param>
        ''' <param name="icropCode3">i-CROPコード3</param>
        ''' <returns>DMSCODEMAPDataTable</returns>
        ''' <remarks>
        ''' TB_M_DEALERから販売店リストを取得します。
        ''' </remarks>
        Public Shared Function GetDmsCodeMapDataTable(ByVal dmsCodeType As String, ByVal icropCode1 As String,
                                                      ByVal icropCode2 As String, ByVal icropCode3 As String) As DmsCodeMapDataSet.DMSCODEMAPDataTable
            Using query As New DBSelectQuery(Of DmsCodeMapDataSet.DMSCODEMAPDataTable)("DMSCODEMAP_001")

                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* DMSCODEMAP_001 */ ")
                    .Append("    DMS_CD_1, ")
                    .Append("    DMS_CD_2, ")
                    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START
                    .Append("    DMS_CD_3 ")
                    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END
                    .Append("FROM ")
                    .Append("    TB_M_DMS_CODE_MAP ")
                    .Append("WHERE ")
                    .Append("        ICROP_CD_1 = :ICROP_CD_1 ")
                    If Not String.IsNullOrWhiteSpace(icropCode2) Then
                        .Append("    AND ICROP_CD_2 = :ICROP_CD_2 ")
                    End If
                    If Not String.IsNullOrWhiteSpace(icropCode3) Then
                        .Append("    AND ICROP_CD_3 = :ICROP_CD_3 ")
                    End If
                    .Append("    AND DMS_CD_TYPE = :DMS_CD_TYPE ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, icropCode1)
                If Not String.IsNullOrWhiteSpace(icropCode2) Then
                    query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, icropCode2)
                End If
                If Not String.IsNullOrWhiteSpace(icropCode3) Then
                    query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.NVarchar2, icropCode3)
                End If
                query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)

                Dim dt As DmsCodeMapDataSet.DMSCODEMAPDataTable = query.GetData()
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt

            End Using

        End Function
#End Region

    End Class

End Namespace

