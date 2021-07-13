'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070101SearchConditionDataSet.vb
'──────────────────────────────────
'機能： 在庫状況
'補足： 
'作成： -
'更新： 2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $01
'──────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3070101SearchConditionDataSetTableAdapters

    ''' <summary>
    ''' SC3070101(在庫状況)のデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3070101SearchConditionTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "検索条件グレードの一覧取得"

        ''' <summary>
        ''' 検索条件グレードの一覧を取得する
        ''' </summary>
        ''' <param name="carName">車名</param>
        ''' <returns>取得結果</returns>
        ''' <remarks></remarks>
        Public Function GetGradeList(ByVal carName As String) As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable

            Logger.Info("GetSearchConditionGradeList_Start Pram[carName=" & carName & "]")
            Dim dt As SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3070101SearchConditionDataSet.GradeConditionDataTableDataTable)("SC3070101_001")
                Dim sql As New StringBuilder

                ' $01 start
                'SQL文作成
                With sql
                    .Append("SELECT DISTINCT /* SC3070101_001 */ ")
                    .Append("       SUBSTR(T2.GRADE_CD, 0,INSTR(T2.GRADE_CD,'/', 1, 1) -1) AS MODELCODE")
                    .Append("     , T2.GRADE_NAME AS GRADENAME")
                    .Append("     , T1.MODEL_CD   AS VCLSERIESCODE")
                    .Append("     , SUBSTR(T2.GRADE_CD, INSTR(T2.GRADE_CD,'/', 1, 1) + 1)  AS SUFFIX")
                    .Append("  FROM TB_M_MODEL  T1")
                    .Append("     , TB_M_GRADE  T2")
                    .Append(" WHERE T1.MODEL_CD   = T2.MODEL_CD")
                    .Append("   AND T1.MODEL_NAME   = :CAR_NAME")
                    .Append("   AND T1.INUSE_FLG  = :INUSEFLG")
                    .Append("   AND T2.INUSE_FLG  = :INUSEFLG")
                    ' $01 end
                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CAR_NAME", OracleDbType.Varchar2, carName)
                query.AddParameterWithTypeValue("INUSEFLG", OracleDbType.Char, "1")
                'SQL実行
                dt = query.GetData()

                '検索結果返却
                Logger.Info("GetSearchConditionGradeList_End Ret[" & (dt IsNot Nothing) & "]")
                Return dt
            End Using
        End Function

#End Region

#Region "検索条件SFXの一覧取得"

        ''' <summary>
        ''' 検索条件SFXの一覧を取得する
        ''' </summary>
        ''' <param name="carName">車名</param>
        ''' <param name="grade">グレード</param>
        ''' <returns>取得結果</returns>
        ''' <remarks></remarks>
        Public Function GetSuffixList(ByVal carName As String, ByVal grade As String) As SC3070101SearchConditionDataSet.SuffixConditionDataTableDataTable

            Logger.Info("GetSearchConditionSfxList_Start Pram[carName=" & carName & ", grade=" & grade & "]")
            Dim dt As SC3070101SearchConditionDataSet.SuffixConditionDataTableDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3070101SearchConditionDataSet.SuffixConditionDataTableDataTable)("SC3070101_002")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    ' $01 start 
                    .Append(" SELECT DISTINCT /* SC3070101_002 */  ")
                    .Append("       T3.SUFFIX_NAME AS SUFFIXNAME ")
                    .Append("  FROM TB_M_MODEL T1  ")
                    .Append("     , TB_M_GRADE T2  ")
                    .Append("     , TB_M_SUFFIX T3  ")
                    .Append(" WHERE T1.MODEL_CD = T2.MODEL_CD")
                    .Append("   AND T2.MODEL_CD = T3.MODEL_CD")
                    .Append("   AND T2.GRADE_CD = T3.GRADE_CD")
                    .Append("   AND T1.MODEL_NAME = :CAR_NAME ")
                    .Append("  AND SUBSTR(T2.GRADE_CD, 0,INSTR(T2.GRADE_CD,'/', 1, 1) -1) = :GRADE_CD  ")
                    .Append("   AND T3.INUSE_FLG = :INUSEFLG")
                    .Append("   AND T2.INUSE_FLG = :INUSEFLG")
                    .Append("   AND T1.INUSE_FLG = :INUSEFLG")
                    .Append(" ORDER BY ")
                    .Append(" T3.SUFFIX_NAME")
                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CAR_NAME", OracleDbType.Varchar2, carName)
                query.AddParameterWithTypeValue("GRADE_CD", OracleDbType.Varchar2, grade)
                query.AddParameterWithTypeValue("INUSEFLG", OracleDbType.Char, "1")
                ' $01 end
                'SQL実行
                dt = query.GetData()

                '検索結果返却
                Logger.Info("GetSearchConditionSfxList_End Ret[" & (dt IsNot Nothing) & "]")
                Return dt
            End Using
        End Function

#End Region

#Region "検索条件外装色の一覧取得"

        ''' <summary>
        ''' 検索条件外装色の一覧を取得する
        ''' </summary>
        ''' <param name="carName">車名</param>
        ''' <param name="grade">グレード</param>
        ''' <param name="suffix">サフィックス</param>
        ''' <returns>取得結果</returns>
        ''' <remarks></remarks>
        Public Function GetBodyColorList(ByVal carName As String, ByVal grade As String _
                                         , ByVal suffix As String, ByVal colorCode As String) As SC3070101SearchConditionDataSet.ExteriorConditionDataTableDataTable

            Logger.Info("GetBodyColorList_Start Pram[carName=" & carName & ", grade=" & grade & ", suffix=" & suffix & "]")
            Dim dt As SC3070101SearchConditionDataSet.ExteriorConditionDataTableDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3070101SearchConditionDataSet.ExteriorConditionDataTableDataTable)("SC3070101_003")
                Dim sql As New StringBuilder

                ' $01 start
                ' SQL文作成
                With sql
                    .Append(" SELECT DISTINCT /* SC3070101_003 */ ")
                    .Append("        T5.COLOR_CD AS EXTERIORCOLORCODE ")
                    .Append("      , T5.COLOR_NAME AS EXTERIORCOLORNAME ")
                    .Append("   FROM TB_M_MODEL T1 ")
                    .Append("      , TB_M_GRADE T2 ")
                    .Append("      , TB_M_SUFFIX T3 ")
                    .Append("      , TB_M_BODYCOLOR T4 ")
                    .Append("      , TBL_MSTEXTERIOR T5 ")
                    .Append("  WHERE T1.MODEL_CD = T2.MODEL_CD ")
                    .Append("    AND T2.MODEL_CD = T3.MODEL_CD ")
                    .Append("    AND T2.GRADE_CD = T3.GRADE_CD ")
                    .Append("    AND T3.MODEL_CD = T4.MODEL_CD ")
                    .Append("    AND T3.GRADE_CD = T4.GRADE_CD ")
                    .Append("    AND T5.VCLMODEL_CODE = T4.GRADE_CD ")
                    .Append("    AND T5.BODYCLR_CD = T4.BODYCLR_CD ")
                    .Append("    AND T1.MODEL_NAME = :CAR_NAME ")
                    .Append("    AND SUBSTR(T2.GRADE_CD, 0,INSTR(T2.GRADE_CD, '/', 1, 1) -1) = :VCLMODEL_CODE ")

                    ' モデルサフィックスが指定されている場合のみモデルサフィックスも条件に加える
                    If Not String.IsNullOrEmpty(suffix) Then
                        .Append("    AND SUBSTR(T2.GRADE_CD, INSTR(T2.GRADE_CD, '/', 1, 1) + 1) = :SUFFIX_CODE ")
                    End If

                    ' カラーコードが指定されている場合のみカラーコードも条件に加える
                    If Not String.IsNullOrEmpty(colorCode) Then
                        .Append("    AND T4.BODYCLR_CD = :BODYCLR_CD ")
                    End If

                    .Append("    AND T1.INUSE_FLG = :INUSEFLG ")
                    .Append("    AND T2.INUSE_FLG = :INUSEFLG ")
                    .Append("    AND T3.INUSE_FLG = :INUSEFLG ")
                    .Append("    AND T4.INUSE_FLG = :INUSEFLG ")
                    .Append("    AND T5.DELETE_FLAG IS NULL ")
                    .Append("  ORDER BY T5.COLOR_NAME ")
                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CAR_NAME", OracleDbType.Varchar2, carName)
                query.AddParameterWithTypeValue("VCLMODEL_CODE", OracleDbType.Varchar2, grade)

                ' モデルサフィックスが指定されている場合のみモデルサフィックスも条件に加える
                If Not String.IsNullOrEmpty(suffix) Then
                    query.AddParameterWithTypeValue("SUFFIX_CODE", OracleDbType.Varchar2, suffix)
                End If

                ' カラーコードが指定されている場合のみカラーコードも条件に加える
                If Not String.IsNullOrEmpty(colorCode) Then
                    query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.Varchar2, colorCode)
                End If

                query.AddParameterWithTypeValue("INUSEFLG", OracleDbType.Char, "1")
                ' $01 end
                'SQL実行
                dt = query.GetData()

                '検索結果返却
                Logger.Info("GetBodyColorList_End Ret[" & (dt IsNot Nothing) & "]")
                Return dt
            End Using
        End Function
#End Region

#Region "基幹販売店コード取得"
        ''' <summary>
        ''' 基幹販売店コード取得
        ''' </summary>
        ''' <param name="DmsCdType">DmsCdType</param>
        ''' <param name="IcropCd1">IcropCd1</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDmsCd1(ByVal DmsCdType As String, ByVal IcropCd1 As String) As SC3070101SearchConditionDataSet.SC3070101DmsCodeMapDataTable

            Dim query As New DBSelectQuery(Of SC3070101SearchConditionDataSet.SC3070101DmsCodeMapDataTable)("SC3070101_004")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070101_004 */ ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE	= :bindDmsCdType ")
                .Append("AND ICROP_CD_1	= :bindIcropCd1 ")
            End With

            'バインド変数
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindIcropCd1", OracleDbType.NVarchar2, IcropCd1)

            query.CommandText() = sql.ToString()
            Return query.GetData()
        End Function

#End Region

#Region "基幹店舗コード取得"

        ''' <summary>
        ''' 基幹店舗コード取得
        ''' </summary>
        ''' <param name="DmsCdType">DmsCdType</param>
        ''' <param name="IcropCd1">IcropCd1</param>
        ''' <param name="IcropCd2">IcropCd2</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDmsCd2(ByVal DmsCdType As String, ByVal IcropCd1 As String, ByVal IcropCd2 As String) As  _
            SC3070101SearchConditionDataSet.SC3070101DmsCodeMapDataTable

            Dim query As New DBSelectQuery(Of SC3070101SearchConditionDataSet.SC3070101DmsCodeMapDataTable)("SC3070101_005")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3070101_005 */ ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE	= :bindDmsCdType ")
                .Append("AND ICROP_CD_1	= :bindIcropCd1 ")
                .Append("AND ICROP_CD_2	= :bindIcropCd2 ")
            End With

            'バインド変数
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindIcropCd1", OracleDbType.NVarchar2, IcropCd1)
            query.AddParameterWithTypeValue("bindIcropCd2", OracleDbType.NVarchar2, IcropCd2)

            query.CommandText() = sql.ToString()
            Return query.GetData()
        End Function

#End Region

    End Class

End Namespace
Partial Class SC3070101SearchConditionDataSet
End Class
