Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Public NotInheritable Class SC3040101TableAdapter

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

#Region "連絡事項登録処理"

    ''' <summary>
    ''' 連絡事項登録処理
    ''' </summary>
    ''' <param name="dt">連絡事項登録データテーブル</param>
    ''' <returns>処理結果件数</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertMessages(ByVal dt As SC3040101DataSet.SC3040101MessageInfoDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3040101_001")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("INSERT /* SC3040101_001 */ ")
                .AppendLine("  INTO TBL_MESSAGEINFO(")
                .AppendLine("       DLRCD")
                .AppendLine("     , STRCD")
                .AppendLine("     , MESSAGENO")
                .AppendLine("     , TITLE")
                .AppendLine("     , MESSAGE")
                .AppendLine("     , TIMELIMIT")
                .AppendLine("     , CREATESTAFFCD")
                .AppendLine("     , DELFLG")
                .AppendLine("     , CREATEDATE")
                .AppendLine("     , UPDATEDATE")
                .AppendLine("     , CREATEACCOUNT")
                .AppendLine("     , UPDATEACCOUNT")
                .AppendLine("     , CREATEID")
                .AppendLine("     , UPDATEID")
                .AppendLine(") ")
                .AppendLine("VALUES ")
                .AppendLine("( ")
                .AppendLine("       :DLRCD")
                .AppendLine("     , :STRCD")
                .AppendLine("     , SEQ_MESSAGEINFONO.NEXTVAL")
                .AppendLine("     , :TITLE")
                .AppendLine("     , :MESSAGE")
                .AppendLine("     , :TIMELIMIT")
                .AppendLine("     , :CREATESTAFFCD")
                .AppendLine("     , :DELFLG")
                .AppendLine("     , SYSDATE")
                .AppendLine("     , SYSDATE")
                .AppendLine("     , :CREATEACCOUNT")
                .AppendLine("     , :UPDATEACCOUNT")
                .AppendLine("     , :CREATEID")
                .AppendLine("     , :UPDATEID")
                .AppendLine(") ")
            End With

            query.CommandText = sql.ToString()

            'データテーブルの検証
            If dt Is Nothing Then
                Return -1
            End If

            'SQLパラメータ設定
            Dim dr As SC3040101DataSet.SC3040101MessageInfoRow = CType(dt.Rows(0), SC3040101DataSet.SC3040101MessageInfoRow)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, dr.STRCD)
            query.AddParameterWithTypeValue("TITLE", OracleDbType.Varchar2, dr.TITLE)
            query.AddParameterWithTypeValue("MESSAGE", OracleDbType.NVarchar2, dr.MESSAGE)
            query.AddParameterWithTypeValue("TIMELIMIT", OracleDbType.Date, dr.TIMELIMIT)
            query.AddParameterWithTypeValue("CREATESTAFFCD", OracleDbType.Varchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, "0")
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, dr.SYSTEM)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.SYSTEM)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using

    End Function

#End Region

End Class