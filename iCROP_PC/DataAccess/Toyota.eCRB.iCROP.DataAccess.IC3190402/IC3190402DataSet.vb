'-------------------------------------------------------------------------
'Partial Class IC3190402DataSet.vb
'-------------------------------------------------------------------------
'機能：部品ステータス情報取得用DataSet
'補足：
'作成：2014/02/XX NEC 村瀬 初版
'更新：2017/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $01
'─────────────────────────────────────
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class IC3190402DataSet

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

#Region "メソッド"

    ''' <summary>
    ''' ServiceCommonClass_001:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
    ''' </summary>
    ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <returns>DmsCodeMapDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetIcropToDmsCode(ByVal allDealerCD As String, _
                                      ByVal dealerCD As String, _
                                      ByVal dmsCodeType As Integer, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String) As IC3190402DataSet.DmsCodeMapDataTable

        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} P6:{6} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          allDealerCD, _
        '                          dealerCD, _
        '                          dmsCodeType, _
        '                          icropCD1, _
        '                          icropCD2, _
        '                          icropCD3))
        '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* IC3190402_001 */")
            .Append(" DMS_CD_1 CODE1")                '基幹コード1
            .Append(",DMS_CD_2 CODE2")                '基幹コード2
            .Append(",DMS_CD_3 CODE3")                '基幹コード3
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_DMS_CODE_MAP")             '基幹コードマップ
            .Append(" ")
            .Append("WHERE")
            .Append(" DLR_CD IN (:DLR_CD, :ALL_DLR_CD)")
            .Append(" AND DMS_CD_TYPE = :DMS_CD_TYPE")
            .Append(" AND ICROP_CD_1 = :ICROP_CD_1")

            If Not String.IsNullOrEmpty(icropCD2) Then
                .Append(" AND ICROP_CD_2 = :ICROP_CD_2")
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                .Append(" AND ICROP_CD_3 = :ICROP_CD_3")
            End If

            .Append(" ")
            .Append("ORDER BY")
            .Append(" DLR_CD ASC")
        End With

        Using query As New DBSelectQuery(Of IC3190402DataSet.DmsCodeMapDataTable)("ServiceCommonClass_001")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCD)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.Char, allDealerCD)
            query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.Int32, dmsCodeType)
            query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.Char, icropCD1)

            If Not String.IsNullOrEmpty(icropCD2) Then
                query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.Char, icropCD2)
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.Char, icropCD3)
            End If

            sql = Nothing

            Using dt As IC3190402DataSet.DmsCodeMapDataTable = query.GetData

                '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function

#End Region

End Class

