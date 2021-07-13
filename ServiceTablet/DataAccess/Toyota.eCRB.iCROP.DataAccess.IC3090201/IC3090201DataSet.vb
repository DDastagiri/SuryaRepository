Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3090201DataSetTableAdapters


    ''' <summary>
    ''' IC3090201 来店通知送信IF データ層クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3090201DataSetTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AppId As String = "IC3090201"

        ''' <summary>
        ''' 対応フラグ
        ''' </summary>
        ''' <remarks>対応フラグには0を入れる</remarks>
        Private Const Deal As String = "0"

#End Region

#Region "コンストラクタ"

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>処理なし</remarks>
        Public Sub New()

        End Sub

#End Region

#Region "シーケンスの次の番号取得"

        ''' <summary>
        ''' シーケンスの次の番号を取得
        ''' </summary>
        ''' <returns>取得した次の番号</returns>
        ''' <remarks></remarks>
        Public Function GetSeqNextValue() As Long

            'GetSeqNextValue開始ログ
            Logger.Info("GetSeqNextValue_Start ")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3090201_001 */")
                .Append("        SEQ_VISIT_VEHICLE_VISITVCLSEQ.NEXTVAL AS SEQUENCE")
                .Append("   FROM")
                .Append("        DUAL")
            End With

            'DbUpdateQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3090201DataSet.IC3090201DualDataTable)("IC3090201_001")
                query.CommandText = sql.ToString()

                'IC3090201SeqDataTableから情報を取得
                Dim seqTbl As IC3090201DataSet.IC3090201DualDataTable
                seqTbl = query.GetData()

                'GetSeqNextValue開始ログ
                Logger.Info("GetSeqNextValue_End Ret[" & seqTbl.Item(0).SEQUENCE & "]")

                '次のシーケンス番号を返却
                Return CLng(seqTbl.Item(0).SEQUENCE)
            End Using

        End Function

#End Region

#Region "来店車両実績登録"

        ''' <summary>
        '''  来店車両実績情報を登録
        ''' </summary>
        ''' <param name="seq">来店車両実績連番</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="vehicleRegNo">車両登録No.</param>
        ''' <param name="visitTimeStamp">来店日時</param>
        ''' <remarks></remarks>
        Public Sub InsertVisitVehicle(ByVal seq As Long, _
                                            ByVal dealerCode As String, _
                                            ByVal storeCode As String, _
                                            ByVal vehicleRegNo As String, _
                                             ByVal visitTimestamp As Date
                                             )

            'マスタチェック開始
            Dim insertVisitVehicleStartLogMaster As New StringBuilder
            insertVisitVehicleStartLogMaster.Append("InsertVisitVehicle_Start ")
            insertVisitVehicleStartLogMaster.Append("param1[" & seq & "]")
            insertVisitVehicleStartLogMaster.Append(",param2[" & dealerCode & "]")
            insertVisitVehicleStartLogMaster.Append(",param3[" & storeCode & "]")
            insertVisitVehicleStartLogMaster.Append(",param4[" & vehicleRegNo & "]")
            insertVisitVehicleStartLogMaster.Append(",param5[" & visitTimestamp & "]")
            Logger.Info(insertVisitVehicleStartLogMaster.ToString())

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* IC3090201_002 */")
                .Append("   INTO tbl_VISIT_VEHICLE (")
                .Append("        VISITVCLSEQ")         'シーケンス番号
                .Append("      , DLRCD")               '販売店コード
                .Append("      , STRCD")               '店舗コード
                .Append("      , VISITTIMESTAMP")      '来店日時
                .Append("      , VCLREGNO")            '車両登録No.
                .Append("      , DEALFLG")             '対応フラグ(0:未対応、1:対応済)
                .Append("      , CREATEDATE")          '作成日
                .Append("      , UPDATEDATE")          '更新日
                .Append("      , CREATEACCOUNT")       '作成アカウント
                .Append("      , UPDATEACCOUNT")       '更新アカウント
                .Append("      , CREATEID")            '作成機能ID
                .Append("      , UPDATEID")            '更新機能ID
                .Append(" )")
                .Append(" VALUES")
                .Append(" (")
                .Append("        :VISITVCLSEQ")        'シーケンス番号
                .Append("      , :DLRCD")              '販売店コード
                .Append("      , :STRCD")              '店舗コード
                .Append("      , :VISITTIMESTAMP")     '来店日時
                .Append("      , :VCLREGNO")           '車両登録No.
                .Append("      , :DEALFLG")            '対応フラグ(0:未対応、1:対応済)
                .Append("      , SYSDATE")             '作成日
                .Append("      , SYSDATE")             '更新日
                .Append("      , :CREATEACCOUNT")      '作成アカウント
                .Append("      , :UPDATEACCOUNT")      '更新アカウント
                .Append("      , :CREATEID")           '作成機能ID
                .Append("      , :UPDATEID")           '更新機能ID
                .Append(" )")
            End With

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3090201_002")
                query.CommandText = sql.ToString()

                ' SQLパラメータ設定
                query.AddParameterWithTypeValue("VISITVCLSEQ", OracleDbType.Long, seq)                  'シーケンス番号
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)                 '販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)                  '店舗コード
                query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, visitTimestamp)    '来店日時日時
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, vehicleRegNo)       '車両登録No.
                query.AddParameterWithTypeValue("DEALFLG", OracleDbType.Char, Deal)                  '対応フラグ
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, " ")            '作成アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, " ")            '更新アカウント
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, AppId)               '作成機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, AppId)               '更新機能ID

                ' SQL実行（結果を返却）
                If query.Execute() > 0 Then

                    Logger.Info("InsertVisitVehicle_End OK ")
                    Return
                Else

                    Logger.Info("InsertVisitVehicle_End NG ")
                    Throw New OracleExceptionEx
                End If
            End Using
        End Sub
#End Region

    End Class
End Namespace
Partial Class IC3090201DataSet
End Class
