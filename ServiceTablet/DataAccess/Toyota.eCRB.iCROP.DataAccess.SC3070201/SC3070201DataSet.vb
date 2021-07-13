Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


Namespace SC3070201DataSetTableAdapters


    Public Class SC3070201DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "メソッド"
        ''' <summary>
        ''' 自社客個人情報取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="originalid">自社客連番</param>
        ''' <returns>SC3070201ORG_CUSTOMER</returns>
        ''' <remarks></remarks>
        Public Shared Function GetOrgCustomer(ByVal dlrcd As String, _
                                    ByVal originalid As String) As SC3070201DataSet.SC3070201ORGCUSTOMERDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201ORGCUSTOMERDataTable)("SC3070201_001")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_001 */ ")
                    .Append("    A.DLRCD, ")            '販売店コード
                    .Append("    A.ORIGINALID, ")       '自社客連番
                    .Append("    A.CUSTYPE, ")          '個人/法人区分
                    .Append("    A.SOCIALID, ")         '国民ID、免許証番号等
                    .Append("    A.NAME, ")             '顧客氏名
                    .Append("    A.ADDRESS, ")          '住所
                    .Append("    A.ZIPCODE, ")          '郵便番号
                    .Append("    A.TELNO, ")            '自宅電話番号
                    .Append("    A.MOBILE, ")           '携帯電話番号
                    .Append("    A.EMAIL1 ")           'E-mailアドレス１
                    .Append("FROM ")
                    .Append("    TBLORG_CUSTOMER A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD ")
                    .Append("AND A.ORIGINALID = :ORIGINALID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, originalid)        '自社客連番

                Return query.GetData()

            End Using


        End Function


        ''' <summary>
        ''' 未取引客個人情報取得
        ''' </summary>
        ''' <param name="cstId">未取引客ユーザーID</param>
        ''' <returns>SC3070201NEWCUSTOMER</returns>
        ''' <remarks></remarks>
        Public Shared Function GetNewCustomer(ByVal cstId As String) As SC3070201DataSet.SC3070201NEWCUSTOMERDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201NEWCUSTOMERDataTable)("SC3070201_002")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_002 */ ")
                    .Append("    A.CSTID, ")            '未取引客ユーザID
                    .Append("    A.NAME, ")             '顧客氏名
                    .Append("    A.ADDRESS, ")          '住所
                    .Append("    A.ZIPCODE, ")          '郵便番号
                    .Append("    A.TELNO, ")            '自宅電話番号
                    .Append("    A.MOBILE, ")           '携帯電話番号
                    .Append("    A.EMAIL1, ")           'E-mailアドレス１
                    .Append("    A.CUSTYPE, ")          '個人/法人区分
                    .Append("    A.SOCIALID ")         '国民ID、免許証番号等
                    .Append("FROM ")
                    .Append("    TBL_NEWCUSTOMER A ")
                    .Append("WHERE ")
                    .Append("    A.CSTID = :CSTID ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, cstId)        '未取引客ユーザーID

                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 見積保険会社マスタ取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <returns>SC3070201ESTINSUCOMMAST</returns>
        ''' <remarks></remarks>
        Public Shared Function GetEstInsuranceComMst(ByVal dlrcd As String) As SC3070201DataSet.SC3070201ESTINSUCOMMASTDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201ESTINSUCOMMASTDataTable)("SC3070201_003")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_003 */ ")
                    .Append("    A.DLRCD, ")            '販売店コード
                    .Append("    A.INSUCOMCD, ")        '保険会社コード
                    .Append("    A.INSUDVS, ")          '保険区分
                    .Append("    A.INSUCOMNM ")       '保険会社名
                    .Append("FROM ")
                    .Append("    TBL_EST_INSUCOMMAST A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 見積保険種別マスタ取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <returns>SC3070201INSUKINDMAST</returns>
        ''' <remarks></remarks>
        Public Shared Function GetInsuKindMst(ByVal dlrcd As String) As SC3070201DataSet.SC3070201ESTINSUKINDMASTDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201ESTINSUKINDMASTDataTable)("SC3070201_004")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_004 */ ")
                    .Append("    A.DLRCD, ")            '販売店コード
                    .Append("    A.INSUCOMCD, ")         '保険会社コード
                    .Append("    A.INSUKIND, ")          '保険種別
                    .Append("    A.INSUKINDNM ")        '保険種別名称
                    .Append("FROM ")
                    .Append("    TBL_EST_INSUKINDMAST A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 融資会社マスタ取得
        ''' </summary>
        ''' <returns>SC3070201FINANCECOMMAST</returns>
        ''' <remarks></remarks>
        Public Shared Function GetFinanceComMst(ByVal dlrcd As String) As SC3070201DataSet.SC3070201FINANCECOMMASTDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201FINANCECOMMASTDataTable)("SC3070201_005")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_005 */ ")
                    .Append("    A.DLRCD, ")                '販売店コード
                    .Append("    A.FINANCECOMCODE, ")       '融資会社コード
                    .Append("    A.FINANCECOMNAME ")        '融資会社名
                    .Append("FROM ")
                    .Append("    TBL_EST_FINANCECOMMAST A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = :DLRCD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                  '販売店コード

                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' モデル写真取得
        ''' </summary>
        ''' <returns>SC3070201MODELPICTURE</returns>
        ''' <remarks></remarks>
        Public Shared Function GetModelPicture(ByVal modelCD As String, ByVal colorCD As String) As SC3070201DataSet.SC3070201MODELPICTUREDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201MODELPICTUREDataTable)("SC3070201_006")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3070201_006 */ ")
                    .Append("    A.DLRCD, ")        '販売店コード
                    .Append("    A.SERIESCD, ")      'シリーズコード
                    .Append("    A.MODELCD, ")       'モデルコード
                    .Append("    A.COLORCD, ")       '色コード
                    .Append("    A.IMAGEFILE ")     'イメージ画像
                    .Append("FROM ")
                    .Append("    TBL_MODELPICTURE A ")
                    .Append("WHERE ")
                    .Append("    A.DLRCD = 'XXXXX' ")
                    .Append("AND A.MODELCD = :MODELCD ")
                    .Append("AND A.COLORCD = :COLORCD ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("MODELCD", OracleDbType.Char, modelCD)              'モデルコード
                query.AddParameterWithTypeValue("COLORCD", OracleDbType.Char, colorCD)              '色コード

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 見積車両購入税マスタ取得
        ''' </summary>
        ''' <param name="seriesCD">シリーズコード</param>
        ''' <param name="modelCD">モデルコード</param>
        ''' <returns>見積車両購入税マスタデータテーブル</returns>
        ''' <remarks></remarks>
        Public Shared Function GetVclPurchaseTaxMst(ByVal seriesCD As String, ByVal modelCD As String) As SC3070201DataSet.SC3070201VCLPURCHASETAXMASTDataTable

            Using query As New DBSelectQuery(Of SC3070201DataSet.SC3070201VCLPURCHASETAXMASTDataTable)("SC3070201_007")

                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3070201_007 */ ")
                    .Append("    SERIESCD ")                'シリーズコード
                    .Append("  , MODELCD ")                 'モデルコード
                    .Append("  , MINIMUMPRICE ")            '最低価格
                    .Append("FROM ")
                    .Append("    TBL_EST_VCLPURCHASETAXMAST ")
                    .Append("WHERE ")
                    .Append("    SERIESCD = :SERIESCD ")
                    .Append("AND MODELCD = :MODELCD ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriesCD)       'シリーズコード
                query.AddParameterWithTypeValue("MODELCD", OracleDbType.NVarchar2, modelCD)         'モデルコード

                Return query.GetData()

            End Using

        End Function

#End Region

    End Class

End Namespace
Partial Class SC3070201DataSet

End Class
