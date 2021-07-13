Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client

Namespace IC3040601.Api.DataAccess

    Public Class TblCardInfo
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' 連絡先情報データの取得（Select ALL：販売店コードと店舗コードを与えて、結果テーブルを得る）
        ''' </summary>
        ''' <param name="DLRCD">販売店コード</param>
        ''' <param name="STRCD">店舗コード</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' 連絡先情報データを取得する
        ''' </remarks>
        Public Function GetSelectTable(ByVal dlrcd As String, ByVal strcd As String) As IC3040601DataSet.TblCardInfoDataTable
            Using query As New DBSelectQuery(Of IC3040601DataSet.TblCardInfoDataTable)("IC3040601_001")
                Dim dataTable As IC3040601DataSet.TblCardInfoDataTable = Nothing

                Try
                    Dim sql As New StringBuilder
                    With sql
                        .Append("SELECT  /* IC3040601_001 */")
                        .Append("         CARDID")
                        .Append("       , DLRCD")
                        .Append("       , STRCD")
                        .Append("       , STAFFCD")
                        .Append("       , LASTNAME")
                        .Append("       , FIRSTNAME")
                        .Append("       , LASTNAMEKANA")
                        .Append("       , FIRSTNAMEKANA")
                        .Append("       , MEMO")
                        .Append("       , ORGANIZATION")
                        .Append("       , TITLE")
                        .Append("       , URL")
                        .Append("       , CREATEDATE")
                        .Append("       , UPDATEDATE")
                        .Append("       , CREATEACCOUNT")
                        .Append("       , UPDATEACCOUNT")
                        .Append("       , CREATEID")
                        .Append("       , UPDATEID ")
                        .Append("  FROM  TBL_CARD_INFO ")
                        .Append(" WHERE  DLRCD = :DLRCD")
                        .Append("   AND  STRCD = :STRCD")
                        .Append(" ORDER  BY CARDID ")
                    End With

                    'コマンド生成
                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)

                    'SQL実行（結果表を返却）
                    dataTable = query.GetData()

                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return dataTable
            End Using
        End Function

    End Class

    Public Class TblCardAddress
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' 連絡先住所情報データの取得（カードIDを与えて、結果テーブルを得る）
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' 連絡先住所情報データを取得する
        ''' </remarks>
        Public Function GetSelectTable(ByVal cardid As String) As IC3040601DataSet.TblCardAddressDataTable

            Using query As New DBSelectQuery(Of IC3040601DataSet.TblCardAddressDataTable)("IC3040601_002")
                Dim dataTable As IC3040601DataSet.TblCardAddressDataTable = Nothing
                Dim sql As New StringBuilder

                Try
                    With sql
                        .Append("SELECT  /* IC3040601_002 */")
                        .Append("         CARDID")
                        .Append("       , SEQNO")
                        .Append("       , ADDRESSTYPE")
                        .Append("       , ADDRESS")
                        .Append("       , X_ABADR")
                        .Append("       , CREATEDATE")
                        .Append("       , UPDATEDATE")
                        .Append("       , CREATEACCOUNT")
                        .Append("       , UPDATEACCOUNT")
                        .Append("       , CREATEID")
                        .Append("       , UPDATEID")
                        .Append("  FROM  TBL_CARD_ADDRESS ")
                        .Append(" WHERE  CARDID = :CARDID ")
                        .Append(" ORDER  BY SEQNO ")
                    End With

                    'コマンド生成
                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue("CARDID", OracleDbType.Varchar2, cardid)

                    'SQL実行（結果表を返却）
                    dataTable = query.GetData()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: tbl_CARD_ADDRESS: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: tbl_CARD_ADDRESS: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return dataTable

            End Using

        End Function

    End Class


    Public Class TblCardTel
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' 連絡先電話番号情報データの取得（カードIDを与えて、結果テーブルを得る）
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' 連絡先電話番号情報データを取得する
        ''' </remarks>
        Public Function GetSelectTable(ByVal cardid As String) As IC3040601DataSet.TblCardTelDataTable

            Using query As New DBSelectQuery(Of IC3040601DataSet.TblCardTelDataTable)("IC3040601_003")
                Dim dataTable As IC3040601DataSet.TblCardTelDataTable = Nothing
                Dim sql As New StringBuilder

                Try
                    With sql
                        .Append("SELECT /* IC3040601_003 */")
                        .Append("         CARDID")
                        .Append("       , SEQNO")
                        .Append("       , TELTYPE")
                        .Append("       , TEL")
                        .Append("       , CREATEDATE")
                        .Append("       , UPDATEDATE")
                        .Append("       , CREATEACCOUNT")
                        .Append("       , UPDATEACCOUNT")
                        .Append("       , CREATEID")
                        .Append("       , UPDATEID")
                        .Append("  FROM  TBL_CARD_TEL")
                        .Append(" WHERE  CARDID = :CARDID")
                        .Append(" ORDER  BY SEQNO")
                    End With

                    'コマンド生成
                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue("CARDID", OracleDbType.Varchar2, cardid)

                    'SQL実行（結果表を返却）
                    dataTable = query.GetData()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: tbl_CARD_TEL: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: tbl_CARD_TEL: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return dataTable
            End Using
        End Function

    End Class

    Public Class TblCardMail
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' 連絡先メール情報データの取得（カードIDを与えて、結果テーブルを得る）
        ''' </summary>
        ''' <param name="CARDID">カードID</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' 連絡先メール情報データを取得する
        ''' </remarks>
        Public Function GetSelectTable(ByVal cardid As String) As IC3040601DataSet.TblCardMailDataTable

            Using query As New DBSelectQuery(Of IC3040601DataSet.TblCardMailDataTable)("IC3040601_004")
                Dim dataTable As IC3040601DataSet.TblCardMailDataTable = Nothing
                Dim sql As New StringBuilder

                Try
                    With sql
                        .Append("SELECT  /* IC3040601_004 */")
                        .Append("         CARDID")
                        .Append("       , SEQNO")
                        .Append("       , MAILTYPE")
                        .Append("       , EMAIL")
                        .Append("       , CREATEDATE")
                        .Append("       , UPDATEDATE")
                        .Append("       , CREATEACCOUNT")
                        .Append("       , UPDATEACCOUNT")
                        .Append("       , CREATEID")
                        .Append("       , UPDATEID")
                        .Append("  FROM  TBL_CARD_MAIL")
                        .Append(" WHERE  CARDID = :CARDID")
                        .Append(" ORDER  BY SEQNO")
                    End With

                    'コマンド生成
                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue("CARDID", OracleDbType.Varchar2, cardid)

                    'SQL実行（結果表を返却）
                    dataTable = query.GetData()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: tbl_CARD_MAIL: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: tbl_CARD_MAIL: GetSelectTable:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try


                Return dataTable
            End Using

        End Function


    End Class


    Public Class TblCalCardLastModify
        Inherits Global.System.ComponentModel.Component

        Private Const ORACLE_EXCEPTION As Integer = 9000
        Private Const ORACLE_EXCEPTION_EX As Integer = 9100

        ''' <summary>
        ''' カレンダーアドレス最終更新日（スタッフコードを与えて、結果テーブルを得る）
        ''' </summary>
        ''' <param name="staffCd">スタッフコード</param>
        ''' <returns>取得したデータセット</returns>
        ''' <remarks>
        ''' アドレス最終更新日を取得する
        ''' </remarks>
        Public Function GetLastModifyInfo(ByVal staffcd As String) As IC3040601DataSet.TblCalCardLastModifyDataTable
            Using query As New DBSelectQuery(Of IC3040601DataSet.TblCalCardLastModifyDataTable)("IC3040601_005")
                Dim dataTable As IC3040601DataSet.TblCalCardLastModifyDataTable = Nothing
                Dim sql As New StringBuilder

                Try
                    'SQLを作成
                    With sql
                        '対象のデータを取得
                        .Append("SELECT /* IC3040601_005 */")
                        .Append("	     STAFFCD")
                        .Append("	   , CALUPDATEDATE")
                        .Append("	   , CARDUPDATEDATE")
                        .Append("	   , CREATEDATE")
                        .Append("	   , UPDATEDATE")
                        .Append("	   , CREATEACCOUNT")
                        .Append("	   , UPDATEACCOUNT")
                        .Append("	   , CREATEID")
                        .Append("	   , UPDATEID")
                        .Append("  FROM TBL_CAL_CARD_LASTMODIFY")
                        .Append(" WHERE STAFFCD = :STAFF")
                    End With

                    'コマンド生成
                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    query.AddParameterWithTypeValue("STAFF", OracleDbType.Char, staffcd)

                    'SQL実行（結果表を返却）
                    dataTable = query.GetData()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: tbl_CAL_CARD_LASTMODIFY: tbl_CAL_CARD_LASTMODIFY: GetLastModifyInfo:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: tbl_CAL_CARD_LASTMODIFY: tbl_CAL_CARD_LASTMODIFY: GetLastModifyInfo:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return dataTable

            End Using

        End Function

        ''' <summary>
        ''' カレンダーアドレス最終更新日レコード生成（レコードデータを与えて、新規作成）
        ''' </summary>
        ''' <param name="arg">レコードデータ</param>
        ''' <returns>生成レコード数</returns>
        ''' <remarks>
        ''' アドレス最終更新日レコードを生成する
        ''' </remarks>
        Public Function InsertTblCalCardLastModify(ByVal arg As IC3040601CalCradLastModify) As Integer

            Using query As New DBUpdateQuery("IC3040601_006")
                Dim Count As Integer = 0
                Dim sql As New StringBuilder

                Try
                    With sql
                        .Append("INSERT /* IC3040601_006 */ ")
                        .Append("  INTO TBL_CAL_CARD_LASTMODIFY ( ")
                        .Append("       STAFFCD")
                        .Append("     , CALUPDATEDATE")
                        .Append("     , CARDUPDATEDATE")
                        .Append("     , CREATEDATE")
                        .Append("     , UPDATEDATE")
                        .Append("     , CREATEACCOUNT")
                        .Append("     , UPDATEACCOUNT")
                        .Append("     , CREATEID")
                        .Append("     , UPDATEID")
                        .Append(")")
                        .Append("VALUES (")
                        .Append("       :STAFFCD ")
                        .Append("     , :CALUPDATEDATE ")
                        .Append("     , :CARDUPDATEDATE ")
                        .Append("     , :CREATEDATE ")
                        .Append("     , :UPDATEDATE ")
                        .Append("     , :CREATEACCOUNT ")
                        .Append("     , :UPDATEACCOUNT ")
                        .Append("     , :CREATEID ")
                        .Append("     , :UPDATEID ")
                        .Append(") ")
                    End With

                    query.CommandText = sql.ToString()

                    'SQLパラメータ設定
                    With arg
                        query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, .Staffcd)
                        query.AddParameterWithTypeValue("CALUPDATEDATE", OracleDbType.Date, .Calupdatedate)
                        query.AddParameterWithTypeValue("CARDUPDATEDATE", OracleDbType.Date, .Cardupdatedate)
                        query.AddParameterWithTypeValue("CREATEDATE", OracleDbType.Date, .Createdate)
                        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, .Updatedate)
                        query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, .Createaccount)
                        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, .Updateaccount)
                        query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, .Createid)
                        query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, .Updateid)
                    End With

                    'SQL実行（結果表を返却）

                    Count = query.Execute()
                Catch ex As OracleException
                    Logger.Error("OracleException  : IC30601dataSet: InsertTBL_CAL_CARD_LASTMODIFY:")
                    Throw New ApplicationException(ORACLE_EXCEPTION)

                Catch ex As OracleExceptionEx
                    Logger.Error("OracleExceptionEx: IC30601dataSet: InsertTBL_CAL_CARD_LASTMODIFY:")
                    Throw New ApplicationException(ORACLE_EXCEPTION_EX)

                End Try

                Return Count

            End Using

        End Function
    End Class


    ''' <summary>
    ''' TBL_CAL_CARD_LASTMODIFYの引数の構造体
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040601CalCradLastModify
        Private _STAFFCD As String
        Private _CALUPDATEDATE As DateTime
        Private _CARDUPDATEDATE As DateTime
        Private _CREATEDATE As DateTime
        Private _UPDATEDATE As DateTime
        Private _CREATEACCOUNT As String
        Private _UPDATEACCOUNT As String
        Private _CREATEID As String
        Private _UPDATEID As String

        'コンストラクタ
        Sub New()

        End Sub

        ''' <summary>
        ''' Getter Setter群
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Staffcd As String
            Get
                Return _STAFFCD
            End Get
            Set(ByVal value As String)
                _STAFFCD = value
            End Set
        End Property

        Property Calupdatedate As DateTime
            Get
                Return _CALUPDATEDATE
            End Get
            Set(ByVal value As DateTime)
                _CALUPDATEDATE = value
            End Set
        End Property

        Property Cardupdatedate As DateTime
            Get
                Return _CARDUPDATEDATE
            End Get
            Set(ByVal value As DateTime)
                _CARDUPDATEDATE = value
            End Set
        End Property

        Property Createdate As DateTime
            Get
                Return _CREATEDATE
            End Get
            Set(ByVal value As DateTime)
                _CREATEDATE = value
            End Set
        End Property

        Property Updatedate As DateTime
            Get
                Return _UPDATEDATE
            End Get
            Set(ByVal value As DateTime)
                _UPDATEDATE = value
            End Set
        End Property

        Property Createaccount As String
            Get
                Return _CREATEACCOUNT
            End Get
            Set(ByVal value As String)
                _CREATEACCOUNT = value
            End Set
        End Property

        Property Updateaccount As String
            Get
                Return _UPDATEACCOUNT
            End Get
            Set(ByVal value As String)
                _UPDATEACCOUNT = value
            End Set
        End Property

        Property Createid As String
            Get
                Return _CREATEID
            End Get
            Set(ByVal value As String)
                _CREATEID = value
            End Set
        End Property

        Property Updateid As String
            Get
                Return _UPDATEID
            End Get
            Set(ByVal value As String)
                _UPDATEID = value
            End Set
        End Property

    End Class

End Namespace


Partial Class IC3040601DataSet

    Partial Class TblCardTelDataTable

        Private Sub TblCardTelDataTable_TblCardTelRowChanging(ByVal sender As System.Object, ByVal e As TblCardTelRowChangeEvent) Handles Me.TblCardTelRowChanging

        End Sub

    End Class

    Partial Class TblCardAddressDataTable

        Private Sub TblCardAddressDataTable_TblCardAddressRowChanging(ByVal sender As System.Object, ByVal e As TblCardAddressRowChangeEvent) Handles Me.TblCardAddressRowChanging

        End Sub

    End Class

    Partial Class TblCardInfoDataTable

        Private Sub TblCardInfoDataTable_TblCardInfoRowChanging(ByVal sender As System.Object, ByVal e As TblCardInfoRowChangeEvent) Handles Me.TblCardInfoRowChanging

        End Sub

    End Class


End Class
