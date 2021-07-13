'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070402TableAdapter.vb
'─────────────────────────────────────
'機能： 顧客属性取得IF テーブルアダプタ
'補足： 
'作成： 2012/03/07 TCS 陳
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection


Public NotInheritable Class IC3070402TableAdapter

#Region "定数"
    ''' <summary>
    ''' 機能ID：顧客属性取得IF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpFunctionId As String = "IC3070402"

    ''' <summary>
    ''' 顧客種別:自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CSTKIND_ORIGINAL As String = "1"

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 機能ID：顧客属性取得IF
    ''' </summary>
    ''' <value></value>
    ''' <returns>機能ID：顧客属性取得IF</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property FunctionId() As String
        Get
            Return prpFunctionId
        End Get
    End Property
#End Region

#Region "001.顧客職業の取得"

    ''' <summary>
    ''' 001.顧客職業情報の取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>顧客職業情報</returns>
    ''' <remarks></remarks>
    Public Function GetCstOccupation(ByVal crCustId As String) As IC3070402DataSet.IC3070402CstoccupationDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[crCustId:{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, crCustId),
                                  True)
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of IC3070402DataSet.IC3070402CstoccupationDataTable)("IC3070402_001")
            Dim sql As New StringBuilder
            With sql
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                .Append("SELECT /* IC3070402_001 */ ")
                .Append("       T1.CST_OCCUPATION_ID AS OCCUPATIONNO ")   '職業No
                .Append("     , T2.OCCUPATION ")                          '職業
                .Append(" FROM  TB_M_CUSTOMER T1,TBL_OCCUPATIONMST T2 ")  '顧客テーブル、職業マスタテーブル
                .Append(" WHERE T1.CST_OCCUPATION_ID = T2.OCCUPATIONNO ")
                .Append(" AND   T1.CST_ID = :CRCUSTID ")
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            End With
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crCustId)
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 End

            ' SQLを実行
            Dim dt As IC3070402DataSet.IC3070402CstoccupationDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, dt.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Return dt

        End Using
    End Function

    ''' <summary>
    ''' 002.顧客家族情報の取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>顧客家族情報</returns>
    ''' <remarks></remarks>
    Public Function GetCstFamily(ByVal crCustId As String) As IC3070402DataSet.IC3070402CstfamilyDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[crCustId:{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, crCustId),
                                  True)
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of IC3070402DataSet.IC3070402CstfamilyDataTable)("IC3070402_002")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070402_002 */ ")
                .Append("       A.FAMILYRELATIONSHIPNO ")                          '家族続柄No
                .Append("     , B.FAMILYRELATIONSHIP ")                            '家族続柄
                .Append("     , A.BIRTHDAY ")                                      '生年月日
                .Append(" FROM  TBL_CSTFAMILY A,TBL_FAMILYRELATIONSHIPMST B ")     '顧客家族テーブル、家族続柄マスタ
                .Append(" WHERE A.FAMILYRELATIONSHIPNO = B.FAMILYRELATIONSHIPNO ")
                .Append(" AND   A.CRCUSTID = :CRCUSTID ")
            End With
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crCustId)
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 End

            ' SQLを実行
            Dim dt As IC3070402DataSet.IC3070402CstfamilyDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, dt.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 003.顧客趣味の取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>趣味DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCstHobby(ByVal crCustId As String) As IC3070402DataSet.IC3070402CsthobbyDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[crCustId:{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, crCustId),
                                  True)
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of IC3070402DataSet.IC3070402CsthobbyDataTable)("IC3070402_003")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070402_003 */ ")
                .Append("       A.HOBBYNO ")                        '趣味No
                .Append("     , B.HOBBY ")                          '趣味
                .Append(" FROM  TBL_CSTHOBBY A,TBL_HOBBYMST B")     '顧客趣味テーブル、趣味マスタテーブル
                .Append(" WHERE A.HOBBYNO = B.HOBBYNO ")
                .Append(" AND   A.CRCUSTID = :CRCUSTID ")
            End With
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crCustId)
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 End
            ' SQLを実行
            Dim dt As IC3070402DataSet.IC3070402CsthobbyDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, dt.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using
    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 004.顧客名の取得
    ''' </summary>
    ''' <param name="crcustId">活動先顧客コード</param>
    ''' <returns>顧客名DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCstName(ByVal crCustId As String) As IC3070402DataSet.IC3070402CstNameDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[crCustId:{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, crCustId),
                                  True)
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of IC3070402DataSet.IC3070402CstNameDataTable)("IC3070402_004")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070402_004 */ ")
                .Append("       CST_NAME AS NAME ")            '氏名
                .Append("     , NAMETITLE_NAME AS NAMETITLE ") '敬称
                .Append(" FROM  TB_M_CUSTOMER ")               '顧客テーブル
                .Append(" WHERE CST_ID = :CRCUSTID ")
            End With
            query.CommandText = sql.ToString()
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Decimal, crCustId)
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 End
            ' SQLを実行
            Dim dt As IC3070402DataSet.IC3070402CstNameDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, dt.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return dt
        End Using
    End Function
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
#End Region

End Class


