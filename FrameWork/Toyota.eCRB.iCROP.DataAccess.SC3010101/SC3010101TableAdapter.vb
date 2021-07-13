'2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
Imports System.Globalization
Imports System.Reflection
'2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Public NotInheritable Class SC3010101TableAdapter

    Private Sub New()

    End Sub

    ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
#Region "定数"
    ''' <summary>
    ''' C_TYPE_VALUE_2 = "2"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TYPE_VALUE_2 As String = "2"
#End Region
    ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 END

#Region "Select"
    ''' <summary>
    ''' MacAddressに対応した販売店コードの取得処理
    ''' </summary>
    ''' <param name="macaddress">マックアドレス</param>
    ''' <returns>検索結果を格納したDatatable</returns>
    ''' <remarks></remarks>
    Public Shared Function SelDlrCD(ByVal macaddress As String) As SC3010101DataSet.SC3010101MacDataTableDataTable

        ' 2013/06/30 TCS 坂井 2013/10対応版　既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start, macaddress:[{1}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  macaddress))
        ' ======================== ログ出力 終了 ========================

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3010101DataSet.SC3010101MacDataTableDataTable)("SC3010101")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3010101_001 */ ")
                .Append("        T1.DLR_CD AS Dlrcd ")
                .Append("   FROM TB_M_CLIENT T1 ")
                .Append("  WHERE ")
                .Append("        T1.CLIENT_IDENTITY_VAL = :Macaddress ")
                .Append("    AND T1.CLIENT_IDENTITY_TYPE = :CLIENT_ID_TYPE_2 ")
            End With

            'Macアドレス
            query.AddParameterWithTypeValue("Macaddress", OracleDbType.NVarchar2, macaddress)   
            query.AddParameterWithTypeValue("CLIENT_ID_TYPE_2", OracleDbType.NVarchar2, C_TYPE_VALUE_2)
            query.CommandText = sql.ToString()
            ' SQL実行
            Dim rtnDt As SC3010101DataSet.SC3010101MacDataTableDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      rtnDt.Rows.Count))
            ' ======================== ログ出力 終了 ========================
            If Not rtnDt.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                        " TEST_VALUE = {0} ", rtnDt(0).Dlrcd))
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                        " TEST_VALUE = ", " "))
            End If

            Return rtnDt
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        End Using
    End Function
#End Region

End Class

