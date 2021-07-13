Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3040802DataSetTableAdapters

    ''' <summary>
    ''' 通知DB APIのデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040802TableAdapters
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>通知依頼種別：査定</summary>
        Private Const noticeClassAssessment As String = "01"
        ''' <summary>通知依頼種別：価格相談</summary>
        Private Const noticeClassPriceConsultation As String = "02"
        ''' <summary>通知依頼種別：ヘルプ</summary>
        Private Const noticeClassHelp As String = "03"

#End Region

#Region "デフォルトコンストラクタ"

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub
#End Region

#Region "通知未読件数取得処理"

        ''' <summary>
        ''' 通知未読件数取得処理
        ''' </summary>
        ''' <param name="account">ユーザーID</param>
        ''' <param name="sendDate">受信日時</param>
        ''' <param name="staffAuthority">ユーザー権限情報</param>
        ''' <returns>通知未読件数</returns>
        ''' <remarks></remarks>
        Public Function SelectUnreadNotice(ByVal account As String,
                                           ByVal sendDate As Date,
                                           ByVal staffAuthority As Boolean) As Long
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("account", account, False) & _
                        getLogParam("sendDate", CStr(sendDate), True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040802DataSet.IC3040802UnreadNoticeCountDataTable)("IC3040802_001")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* IC3040802_001 */")
                    .Append("        COUNT(1) AS CNT")
                    .Append("   FROM TBL_NOTICEREQUEST T1")
                    .Append("      , TBL_NOTICEINFO T2")
                    .Append("  WHERE T1.NOTICEREQID = T2.NOTICEREQID")
                    'セールスの場合は条件追加
                    If staffAuthority Then
                        .Append("    AND T1.STATUS = T2.STATUS")
                        .Append("    AND T1.NOTICEREQCTG IN(:NOTICEREQCTG1,:NOTICEREQCTG3,:NOTICEREQCTG4)")
                    End If
                    .Append("    AND T2.READFLG = 0")
                    .Append("    AND T2.TOACCOUNT = :TOACCOUNT")
                    .Append("    AND T2.SENDDATE >= TRUNC(:SENDDATE)")
                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定
                'セールスの場合は条件追加
                If staffAuthority Then
                    query.AddParameterWithTypeValue("NOTICEREQCTG1", OracleDbType.Char, noticeClassAssessment)
                    query.AddParameterWithTypeValue("NOTICEREQCTG3", OracleDbType.Char, noticeClassPriceConsultation)
                    query.AddParameterWithTypeValue("NOTICEREQCTG4", OracleDbType.Char, noticeClassHelp)
                End If
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, sendDate)

                Dim dt As IC3040802DataSet.IC3040802UnreadNoticeCountDataTable = query.GetData()

                Dim executeNumber As Long = CLng(dt(0).CNT)
                Logger.Info(getReturnParam(dt(0).CNT))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return executeNumber
            End Using
        End Function

#End Region

#Region "ログデータ加工処理"
        ''' <summary>
        ''' ログデータ（メソッド）
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogMethod(ByVal methodName As String,
                                    ByVal startEndFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                .Append("[")
                .Append(methodName)
                .Append("]")
                If startEndFlag Then
                    .Append(" method start")
                Else
                    .Append(" method end")
                End If
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（引数）
        ''' </summary>
        ''' <param name="paramName">引数名</param>
        ''' <param name="paramData">引数値</param>
        ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogParam(ByVal paramName As String,
                                     ByVal paramData As String,
                                     ByVal kanmaFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                If kanmaFlag Then
                    .Append(",")
                End If
                .Append(paramName)
                .Append("=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（戻り値）
        ''' </summary>
        ''' <param name="paramData">引数値</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getReturnParam(ByVal paramData As String) As String
            Dim sb As New StringBuilder
            With sb
                .Append("Return=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function
#End Region

    End Class
End Namespace

Partial Class IC3040802DataSet
End Class
