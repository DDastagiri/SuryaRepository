'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3100301DataSet.vb
'──────────────────────────────────
'機能： 来店実績データ退避バッチ
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/23 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
'更新： 2020/03/06 NSK  s.natsume TKM Change request development for Next Gen e-CRB (CR060) $03
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace MC3100301DataSetTableAdapters

    ''' <summary>
    ''' MC3100301 来店実績データ退避バッチ データ層
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MC3100301DataSetTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 対応フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEALFLG As String = "1"

#End Region

#Region "来店車両実績移行"

        ''' <summary>
        '''  来店車両実績移行
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>来店車両実績退避テーブルにデータを登録する。</remarks>
        Public Sub CopyVisitVehicle(ByVal delDate As Date)

            Using query As New DBUpdateQuery("MC3100301_001")
                Dim sql As New StringBuilder

                With sql
                    .Append(" INSERT /* MC3100301_001 */")
                    .Append("   INTO tbl_VISIT_VEHICLE_PAST (")
                    .Append("        VISITVCLSEQ")
                    .Append("      , DLRCD")
                    .Append("      , STRCD")
                    .Append("      , VISITTIMESTAMP")
                    .Append("      , VCLREGNO")
                    .Append("      , DEALFLG")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" )")
                    .Append(" SELECT")
                    .Append("        VISITVCLSEQ")
                    .Append("      , DLRCD")
                    .Append("      , STRCD")
                    .Append("      , VISITTIMESTAMP")
                    .Append("      , VCLREGNO")
                    .Append("      , DEALFLG")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append("   FROM tbl_VISIT_VEHICLE")
                    .Append("  WHERE VISITTIMESTAMP < TO_DATE(:DELETEDATE)")
                    .Append("    AND DEALFLG = '1'")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "来店車両実績削除"

        ''' <summary>
        ''' 来店車両実績削除
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>来店車両実績テーブルの移行したデータを物理削除する。</remarks>
        Public Sub DeleteVisitVehicle(ByVal delDate As Date)

            Using query As New DBUpdateQuery("MC3100301_002")
                Dim sql As New StringBuilder

                With sql
                    .Append(" DELETE /* MC3100301_002 */")
                    .Append("   FROM tbl_VISIT_VEHICLE")
                    .Append("  WHERE VISITTIMESTAMP < TO_DATE(:DELETEDATE)")
                    .Append("    AND DEALFLG =" & DEALFLG)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "セールス来店実績移行"

        ''' <summary>
        ''' セールス来店実績移行
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>セールス来店実績退避テーブルにデータを登録する。</remarks>
        Public Sub CopyVisitSales(ByVal delDate As Date)

            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* MC3100301_003 */")
                .Append("   INTO TBL_VISIT_SALES_PAST (")
                .Append("        VISITSEQ")
                .Append("      , DLRCD")
                .Append("      , STRCD")
                .Append("      , VISITTIMESTAMP")
                .Append("      , VCLREGNO")
                .Append("      , CUSTSEGMENT")
                .Append("      , CUSTID")
                .Append("      , STAFFCD")
                .Append("      , VISITPERSONNUM")
                .Append("      , VISITMEANS")
                .Append("      , VISITSTATUS")
                .Append("      , BROUDCASTFLG")
                .Append("      , TENTATIVENAME")
                .Append("      , ACCOUNT")
                .Append("      , SALESTABLENO")
                .Append("      , FLLWUPBOX_DLRCD")
                .Append("      , FLLWUPBOX_STRCD")
                .Append("      , FLLWUPBOX_SEQNO")
                .Append("      , SALESSTART")
                .Append("      , SALESEND")
                .Append("      , CREATEDATE")
                .Append("      , UPDATEDATE")
                .Append("      , CREATEACCOUNT")
                .Append("      , UPDATEACCOUNT")
                .Append("      , CREATEID")
                .Append("      , UPDATEID")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("      , STOPTIME")
                .Append("      , FIRST_SALESSTART")
                ' $01 end   複数顧客に対する商談平行対応
                ' $02 start 新車タブレットショールーム管理機能開発 
                .Append("      , UNNECESSARYCOUNT")
                .Append("      , UNNECESSARYDATE")
                .Append("      , SC_ASSIGNDATE")
                ' $02 end   新車タブレットショールーム管理機能開発
                .Append(" )")
                .Append(" SELECT")
                .Append("        VISITSEQ")
                .Append("      , DLRCD")
                .Append("      , STRCD")
                .Append("      , VISITTIMESTAMP")
                .Append("      , VCLREGNO")
                .Append("      , CUSTSEGMENT")
                .Append("      , CUSTID")
                .Append("      , STAFFCD")
                .Append("      , VISITPERSONNUM")
                .Append("      , VISITMEANS")
                .Append("      , VISITSTATUS")
                .Append("      , BROUDCASTFLG")
                .Append("      , TENTATIVENAME")
                .Append("      , ACCOUNT")
                .Append("      , SALESTABLENO")
                .Append("      , FLLWUPBOX_DLRCD")
                .Append("      , FLLWUPBOX_STRCD")
                .Append("      , FLLWUPBOX_SEQNO")
                .Append("      , SALESSTART")
                .Append("      , SALESEND")
                .Append("      , CREATEDATE")
                .Append("      , UPDATEDATE")
                .Append("      , CREATEACCOUNT")
                .Append("      , UPDATEACCOUNT")
                .Append("      , CREATEID")
                .Append("      , UPDATEID")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("      , STOPTIME")
                .Append("      , FIRST_SALESSTART")
                ' $01 end   複数顧客に対する商談平行対応
                ' $02 start 新車タブレットショールーム管理機能開発 
                .Append("      , UNNECESSARYCOUNT")
                .Append("      , UNNECESSARYDATE")
                .Append("      , SC_ASSIGNDATE")
                ' $02 end   新車タブレットショールーム管理機能開発
                .Append("   FROM TBL_VISIT_SALES")
                .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")
            End With

            Using query As New DBUpdateQuery("SC3100301_003")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "セールス来店実績削除"

        ''' <summary>
        ''' セールス来店実績削除
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>セールス来店実績テーブルの移行したデータを物理削除する。</remarks>
        Public Sub DeleteVisitSales(ByVal delDate As Date)

            Using query As New DBUpdateQuery("MC3100301_004")
                Dim sql As New StringBuilder
                With sql
                    .Append(" DELETE /* MC3100301_004 */")
                    .Append("   FROM tbl_VISIT_SALES")
                    .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "対応依頼通知移行"

        ''' <summary>
        ''' 対応依頼通知移行
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>対応依頼通知退避テーブルにデータを登録する。</remarks>
        Public Sub CopyVisitDealNotice(ByVal delDate As Date)

            Using query As New DBUpdateQuery("SC3100301_005")
                Dim sql As New StringBuilder
                With sql
                    .Append(" INSERT /* MC3100301_005 */")
                    .Append("   INTO tbl_VISITDEAL_NOTICE_PAST (")
                    .Append("        VISITSEQ")
                    .Append("      , ACCOUNT")
                    .Append("      , DELFLG")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" )")
                    .Append(" SELECT")
                    .Append("        VISITSEQ")
                    .Append("      , ACCOUNT")
                    .Append("      , DELFLG")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append("   FROM tbl_VISITDEAL_NOTICE")
                    .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")

                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "対応依頼通知削除"

        ''' <summary>
        ''' 対応依頼通知削除
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>対応依頼通知テーブルの移行したデータを物理削除する。</remarks>
        Public Sub DeleteVisitDealNotice(ByVal delDate As Date)

            Using query As New DBUpdateQuery("MC3100301_006")
                Dim sql As New StringBuilder
                With sql
                    .Append(" DELETE /* MC3100301_006 */")
                    .Append("   FROM tbl_VISITDEAL_NOTICE")
                    .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub

#End Region

        '$03 start TKM Change request development for Next Gen e-CRB (CR060)
#Region "セールス来店実績ローカル移行"

        ''' <summary>
        ''' セールス来店実績ローカル移行
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>セールス来店実績ローカル退避テーブルにデータを登録する。</remarks>
        Public Sub CopyLcVisitSales(ByVal delDate As Date)

            Dim sql As New StringBuilder
            With sql
                .Append(" INSERT /* MC3100301_007 */")
                .Append("   INTO TBL_LC_VISIT_SALES_PAST (")
                .Append("        VISITSEQ")
                .Append("      , TELNO")
                .Append("      , CREATEDATE")
                .Append("      , UPDATEDATE")
                .Append("      , CREATEACCOUNT")
                .Append("      , UPDATEACCOUNT")
                .Append("      , CREATEID")
                .Append("      , UPDATEID")
                .Append(" )")
                .Append(" SELECT")
                .Append("        VISITSEQ")
                .Append("      , TELNO")
                .Append("      , CREATEDATE")
                .Append("      , UPDATEDATE")
                .Append("      , CREATEACCOUNT")
                .Append("      , UPDATEACCOUNT")
                .Append("      , CREATEID")
                .Append("      , UPDATEID")
                .Append("   FROM TBL_LC_VISIT_SALES")
                .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")
            End With

            Using query As New DBUpdateQuery("SC3100301_007")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region

#Region "セールス来店実績ローカル削除"

        ''' <summary>
        ''' セールス来店実績ローカル削除
        ''' </summary>
        ''' <param name="delDate">過去データと判断する日付</param>
        ''' <remarks>セールス来店実績ローカルテーブルの移行したデータを物理削除する。</remarks>
        Public Sub DeleteLcVisitSales(ByVal delDate As Date)

            Using query As New DBUpdateQuery("MC3100301_008")
                Dim sql As New StringBuilder
                With sql
                    .Append(" DELETE /* MC3100301_008 */")
                    .Append("   FROM TBL_LC_VISIT_SALES")
                    .Append("  WHERE UPDATEDATE < TO_DATE(:DELETEDATE)")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DELETEDATE", OracleDbType.Date, delDate) '判断日付
                query.Execute()
            End Using
        End Sub
#End Region
        '$03 end TKM Change request development for Next Gen e-CRB (CR060)

    End Class
End Namespace
