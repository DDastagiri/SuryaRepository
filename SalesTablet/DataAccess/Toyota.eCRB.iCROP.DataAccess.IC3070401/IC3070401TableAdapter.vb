'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070401TableAdapter.vb
'─────────────────────────────────────
'機能： オススメ情報取得IF テーブルアダプタ
'補足： 
'作成： 2012/03/01 TCS 陳
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


Public NotInheritable Class IC3070401TableAdapter

#Region "定数"
    ''' <summary>
    ''' 機能ID：オススメ情報取得IF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpFunctionId As String = "IC3070401"

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
    ''' <summary>
    ''' モデルコード　AHV41L-JEXGBC(SQL用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODEL_CD_HV_SQL As String = "AHV41L-JEXGBC%"

    ''' <summary>
    ''' モデルコード　AHV41L-JEXGBC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODEL_CD_HV As String = "AHV41L-JEXGBC"

    ''' <summary>
    ''' シリーズコード　CAMRY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CAMRY As String = "CAMRY"

    ''' <summary>
    ''' シリーズコード　CMYHV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CMYHV As String = "CMYHV"
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 機能ID：オススメ情報取得IF
    ''' </summary>
    ''' <value></value>
    ''' <returns>機能ID：オススメ情報取得IF</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property FunctionId() As String
        Get
            Return prpFunctionId
        End Get
    End Property
#End Region

#Region "001.オススメ情報取得"

    ''' <summary>
    ''' 001.オススメ情報取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="seriesCd">シリーズコード</param>
    ''' <param name="modelCd">モデルコード</param>
    ''' <returns>オススメ情報</returns>
    ''' <remarks></remarks>
    Public Function GetRecommendedInfo(ByVal dlrCD As String,
                                         ByVal seriesCD As String,
                                         ByVal modelCD As String) As IC3070401DataSet.IC3070401PurchaserateDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "[{0}]_[{1}]_Start,[DLRCD:{2},SERIESCD:{3},MODELCD:{4}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, dlrCD, seriesCD, modelCD),
                                  True)
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of IC3070401DataSet.IC3070401PurchaserateDataTable)("IC3070401_001")
            Dim sql As New StringBuilder
            With sql
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                .Append("SELECT /* IC3070401_001 */ ")
                .Append("       A.DLRCD ")                                  '販売店コード
                .Append("     , :SERIESCD AS SERIESCD ")                    'シリーズコード
                .Append("     , D.VCLMODEL_CODE AS MODELCD ")               'モデルコード
                .Append("     , A.OPTIONPART ")                             'オプション区分
                .Append("     , A.OPTIONCODE ")                             'オプションコード
                .Append("     , A.RATE ")                                   '購入率
                .Append("  FROM TBL_PURCHASERATE A ")                       '購入率集計情報テーブル
                .Append("     , TBL_MINPERCHASERATE B ")                    '最低購入率テーブル
                .Append("     , TBL_MSTCARNAME C ")                         'mst車名マスタ
                .Append("     , TBL_MSTVHCLMODEL D ")                       'mst型式マスタ
                .Append(" WHERE A.SERIESCD = B.SERIESCD(+) ")
                .Append("   AND A.MODELCD = B.MODELCD(+) ")
                .Append("   AND A.SERIESCD = C.CAR_NAME_CD_AI21 ")
                .Append("   AND A.MODELCD = D.MODEL_CD ")
                .Append("   AND (A.RATE >= B.MINPERCHASERATE OR A.RATE >= DECODE(B.MINPERCHASERATE,NULL,0)) ")
                .Append("   AND A.DLRCD = :DLRCD ")                         '販売店コード
                If (seriesCD.Equals(SERIES_CODE_CAMRY)) Then
                    If (Not String.IsNullOrEmpty(modelCD)) Then
                        If (modelCD.Contains(MODEL_CD_HV)) Then
                            .Append("   AND C.VCLSERIES_CD = :SERIES_CODE_CMYHV ")
                            .Append("   AND D.VCLMODEL_CODE = :MODELCD ")
                        Else
                            .Append("   AND C.VCLSERIES_CD = :SERIESCD ")
                            .Append("   AND D.VCLMODEL_CODE = :MODELCD ")
                        End If
                    Else
                        .Append("   AND ((C.VCLSERIES_CD = :SERIESCD) ")
                        .Append("    OR (C.VCLSERIES_CD = :SERIES_CODE_CMYHV ")
                        .Append("   AND D.VCLMODEL_CODE LIKE :MODEL_CD_HV_SQL)) ")
                    End If
                Else
                    If (Not String.IsNullOrEmpty(modelCD)) Then
                        .Append("   AND C.VCLSERIES_CD = :SERIESCD ")
                        .Append("   AND D.VCLMODEL_CODE = :MODELCD ")
                    Else
                        .Append("   AND C.VCLSERIES_CD = :SERIESCD ")
                    End If

                End If
                .Append(" ORDER BY A.MODELCD,A.OPTIONCODE ")
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            End With

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrCD)
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            query.AddParameterWithTypeValue("SERIESCD", OracleDbType.Varchar2, seriesCD)
            If (seriesCD.Equals(SERIES_CODE_CAMRY)) Then
                If (Not String.IsNullOrEmpty(modelCD)) Then
                    If (modelCD.Contains(MODEL_CD_HV)) Then
                        query.AddParameterWithTypeValue("SERIES_CODE_CMYHV", OracleDbType.Varchar2, SERIES_CODE_CMYHV)
                        query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, modelCD)
                    Else
                        query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, modelCD)
                    End If
                Else
                    query.AddParameterWithTypeValue("SERIES_CODE_CMYHV", OracleDbType.Varchar2, SERIES_CODE_CMYHV)
                    query.AddParameterWithTypeValue("MODEL_CD_HV_SQL", OracleDbType.Varchar2, MODEL_CD_HV_SQL)
                End If
            Else
                If (Not String.IsNullOrEmpty(modelCD)) Then
                    query.AddParameterWithTypeValue("MODELCD", OracleDbType.Varchar2, modelCD)
                End If
            End If
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            ' SQLを実行
            Dim dt As IC3070401DataSet.IC3070401PurchaserateDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, {2} RowCount:[{3}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, MethodBase.GetCurrentMethod.Name, dt.Rows.Count),
                                      True)
            ' ======================== ログ出力 終了 ========================

            ' 結果を返却
            Return dt

        End Using
    End Function

#End Region

End Class

