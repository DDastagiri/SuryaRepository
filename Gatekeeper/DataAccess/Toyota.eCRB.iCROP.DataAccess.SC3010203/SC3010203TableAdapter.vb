Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' SCメインのデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010203TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' チップ背景色を取得します。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function ReadChipColorSetting() As SC3010203DataSet.SC3010203TodoColorDataTable

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203TodoColorDataTable)("SC3010203_001")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_001 */")
                .Append("        A.CREATEDATADIV")
                .Append("      , A.SCHEDULEDVS")
                .Append("      , A.CONTACTNO")
                .Append("      , A.BACKGROUNDCOLOR")
                .Append("      , B.ICONPATH")
                .Append("   FROM TBL_TODO_TIP_COLOR A")
                .Append("      , TBL_CONTACTMETHOD B")
                .Append("  WHERE A.CONTACTNO = B.CONTACTNO(+)")
                .Append("    AND A.DLRCD = '").Append(ConstantDealerCD.AllDealerCD).Append("'")
                .Append("    AND A.NEXTACTIONDVS IN('0', 'X')")
            End With
            query.CommandText = sql.ToString()

            '検索結果返却
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 活動に対する誘致先の顧客情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwUpBoxSeqNo">Follow-up Box連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfo(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwUpBoxSeqNo As Long) As SC3010203DataSet.SC3010203CustInfoDataTable

        Using query As New DBSelectQuery(Of SC3010203DataSet.SC3010203CustInfoDataTable)("SC3010203_002")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010203_002 */")
                .Append("        A.DLRCD")
                .Append("      , A.STRCD")
                .Append("      , A.FLLWUPBOX_SEQNO")
                .Append("      , A.CUSTSEGMENT")
                .Append("      , A.CRCUSTID")
                .Append("      , A.CUSTOMERCLASS")
                .Append("      , DECODE(A.CUSTSEGMENT, '1', B.STAFFCD, C.STAFFCD) AS STAFFCD")
                .Append("   FROM TBL_FLLWUPBOX A")
                .Append("      , TBLORG_CUSTOMER B")
                .Append("      , TBL_NEWCUSTOMER C")
                .Append("  WHERE A.INSDID = B.ORIGINALID(+)")
                .Append("    AND A.UNTRADEDCSTID = C.CSTID(+)")
                .Append("    AND A.DLRCD = :DLRCD")
                .Append("    AND A.STRCD = :STRCD")
                .Append("    AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Int64, fllwUpBoxSeqNo)

            '検索結果返却
            Return query.GetData()
        End Using
    End Function

End Class


