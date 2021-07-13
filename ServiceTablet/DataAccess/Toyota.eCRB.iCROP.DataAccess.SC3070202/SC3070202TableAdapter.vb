Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' 見積印刷
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3070202TableAdapter

#Region "SELECT"
    ''' <summary>
    ''' 保険会社の情報を取得
    ''' </summary>
    ''' <param name="dlrcd"></param>
    ''' <param name="insucomcd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInsuranceComInfo(ByVal dlrcd As String, ByVal insucomcd As String, ByVal insuKind As String) As SC3070202DataSet.SC3070202InsKindMastDataTable

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3070202_003 */ ")
            .Append("    A.DLRCD ")                  '販売店コード
            .Append("    ,A.INSUCOMCD ")             '保険会社コード
            .Append("    ,B.INSUKIND ")              '保険種別
            .Append("    ,B.INSUKINDNM ")            '保険種別名称
            .Append("    ,A.INSUCOMNM ")             '保険会社名
            .Append("FROM ")
            .Append("    TBL_EST_INSUCOMMAST A ")
            .Append("   ,TBL_EST_INSUKINDMAST B ")
            .Append("WHERE ")
            .Append("    A.DLRCD = B.DLRCD(+) ")
            .Append("AND A.INSUCOMCD = B.INSUCOMCD(+) ")
            .Append("AND A.DLRCD = :DLRCD ")
            .Append("AND A.INSUCOMCD = :INSUCOMCD ")
            If Not String.IsNullOrEmpty(insuKind) Then
                .Append("AND B.INSUKIND = :INSUKIND ")
            End If
        End With

        Using query As New DBSelectQuery(Of SC3070202DataSet.SC3070202InsKindMastDataTable)("SC3070202_003")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                '販売店コード
            query.AddParameterWithTypeValue("INSUCOMCD", OracleDbType.Char, insucomcd)        '保険会社コード
            If Not String.IsNullOrEmpty(insuKind) Then
                query.AddParameterWithTypeValue("INSUKIND", OracleDbType.Char, insuKind)          '保険種別
            End If
            Return query.GetData()
        End Using

    End Function

    ''' <summary>
    ''' 融資会社の情報を取得
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="financeComCd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFinanceComInfo(ByVal dlrCD As String, ByVal financeComCD As String) As SC3070202DataSet.SC3070202FinanceComMastDataTable

        'SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3070202_005 */ ")
            .Append("    A.FINANCECOMCODE ")                        '融資会社コード
            .Append("    ,A.FINANCECOMNAME ")                       '融資会社名称
            .Append("FROM ")
            .Append("    TBL_EST_FINANCECOMMAST A ")
            .Append("WHERE ")
            .Append("    A.DLRCD = :DLRCD ")                        '販売店コード
            .Append("AND A.FINANCECOMCODE = :FINANCECOMCODE ")      '融資会社コード
        End With

        Using query As New DBSelectQuery(Of SC3070202DataSet.SC3070202FinanceComMastDataTable)("SC3070202_005")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)                      '販売店コード
            query.AddParameterWithTypeValue("FINANCECOMCODE", OracleDbType.Char, financeComCD)      '融資会社コード

            Return query.GetData()
        End Using

    End Function
#End Region
#Region "UPDATE"
    ''' <summary>
    ''' 見積情報テーブルの印刷日を更新
    ''' </summary>
    ''' <param name="estimateid"></param>
    ''' <param name="updateaccount"></param>
    ''' <param name="updateid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePrintDate(ByVal estimateid As Long, _
                                    ByVal updateaccount As String, _
                                    ByVal updateid As String) As Integer
        'SQLの組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3070202_006 */ ")
            .Append("    TBL_ESTIMATEINFO ")
            .Append("SET ")
            .Append("    ESTPRINTDATE = SYSDATE, ")         '見積印刷日
            .Append("    UPDATEDATE = SYSDATE, ")           '更新日
            .Append("    UPDATEACCOUNT = :UPDATEACCOUNT, ") '更新ユーザアカウント
            .Append("    UPDATEID = :UPDATEID ")            '更新機能ID
            .Append("WHERE ")
            .Append("    ESTIMATEID = :ESTIMATEID ")        '見積管理ID
        End With

        Using query As New DBUpdateQuery("SC3070202_006")
            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)     '更新ユーザアカウント 
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Char, updateid)                    '最終更新機能
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Varchar2, estimateid)            '担当者役職

            Return query.Execute()
        End Using

    End Function
#End Region
End Class
