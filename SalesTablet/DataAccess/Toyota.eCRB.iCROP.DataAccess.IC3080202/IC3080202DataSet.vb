'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080202DataSet.vb
'─────────────────────────────────────
'機能： 顧客詳細(車両ロゴ)
'補足： 
'作成： 2012/01/27 KN 佐藤（真）
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3080202DataSetTableAdapters

    Public Class IC3080202DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' モデルロゴ選択区分
        ''' </summary>
        ''' <remarks>0：未選択（黒字）、1：選択（白字）</remarks>
        Public Const ConstSelectionDvsBlack As String = "0"

        ''' <summary>
        ''' シリーズコード取得
        ''' </summary>
        ''' <param name="vin">VIN</param>
        ''' <param name="vehicleRegistrationNo">車両登録No.</param>
        ''' <returns>シリーズコード</returns>
        ''' <remarks>最新のシリーズコードを取得</remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetSeriesCD(ByVal vin As String, _
                                    ByVal vehicleRegistrationNo As String) As String

            '引数を編集
            Dim args As New List(Of String)
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, vin))
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(1).Name, vehicleRegistrationNo))
            '開始ログを出力
            OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT  /* IC3080202_001 */ ")
                .Append("         SERIESCD ")
                .Append("   FROM  TBLORG_VCLINFO ")
                .Append("  WHERE  VIN = :VIN ")
                .Append("     OR  VCLREGNO = :VCLREGNO ")
                .Append("  ORDER  BY UPDATEDATE DESC ")

            End With

            Using query As New DBSelectQuery(Of IC3080202DataSet.IC3080202ModelLogoDataTable)("IC3080202_001")
                'パラメータ設定
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vin) 'VIN
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, vehicleRegistrationNo) '車両登録No.

                'SQL実行
                Using dt As DataTable = query.GetData()
                    '終了ログを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, dt)

                    If dt.Rows.Count = 0 Then
                        Return Nothing
                    Else
                        Return dt.Rows(0)("SERIESCD").ToString
                    End If
                End Using
            End Using

        End Function

        ''' <summary>
        ''' 自社客車両取得（モデルロゴ）
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="seriesCD">シリーズコード</param>
        ''' <param name="selectionIndicator">モデルロゴ選択区分</param>
        ''' <returns>自社客車両情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Public Function GetModelLogo(ByVal dealerCD As String, _
                                    ByVal seriesCD As String, _
                                    ByVal selectionIndicator As String) As IC3080202DataSet.IC3080202ModelLogoDataTable

            '引数を編集
            Dim args As New List(Of String)
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(0).Name, dealerCD))
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(1).Name, seriesCD))
            args.Add(String.Format(CultureInfo.InvariantCulture, "{0} = {1}", MethodBase.GetCurrentMethod.GetParameters(2).Name, selectionIndicator))
            '開始ログを出力
            OutPutStartLog(MethodBase.GetCurrentMethod.Name, args)

            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT  /* IC3080202_002 */ ")
                '顔写真の選択判定
                If selectionIndicator = ConstSelectionDvsBlack Then
                    '未選択（黒字）
                    .Append("         LOGO_NOTSELECTED AS LOGO ")
                Else
                    '選択（白字）
                    .Append("         LOGO_SELECTED AS LOGO ")
                End If
                .Append("   FROM  TBL_MODELLOGO ")
                .Append("  WHERE  DLRCD = :DLRCD ")
                .Append("    AND  SERIESCD = :SERIESCD ")

            End With

            Using query As New DBSelectQuery(Of IC3080202DataSet.IC3080202ModelLogoDataTable)("IC3080202_002")
                'パラメータ設定
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCD) '販売店コード
                query.AddParameterWithTypeValue("SERIESCD", OracleDbType.NVarchar2, seriesCD) 'シリーズコード

                'SQL実行
                Using dt As IC3080202DataSet.IC3080202ModelLogoDataTable = query.GetData()
                    '終了ログを出力
                    OutPutEndLog(MethodBase.GetCurrentMethod.Name, dt)

                    Return dt
                End Using
            End Using

        End Function

        ''' <summary>
        ''' 開始ログ出力
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="args">引数</param>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Private Sub OutPutStartLog(ByVal methodName As String, ByVal args As List(Of String))

            '引数をログに出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , methodName _
                , String.Join(", ", args.ToArray())))

        End Sub

        ''' <summary>
        ''' 終了ログ出力
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="dt">取得データ</param>
        ''' <remarks></remarks>
        ''' 
        ''' <History>
        ''' </History>
        Private Sub OutPutEndLog(ByVal methodName As String, ByVal dt As DataTable)

            '取得件数をログに出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.{1} OUT:ROWSCOUNT = {2}" _
                , Me.GetType.ToString _
                , methodName _
                , dt.Rows.Count))

        End Sub

    End Class

End Namespace

Partial Class IC3080202DataSet

End Class