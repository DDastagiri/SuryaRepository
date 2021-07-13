'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040801Dataset.vb
'─────────────────────────────────────
'機能： 通知登録インターフェース
'補足： 
'作成： 2012/02/01 KN 小澤
'更新： 2012/03/05 KN 佐藤 【SERVICE_1】コメント見直し
'更新： 2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正
'更新： 2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $02
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

Namespace IC3040801DataSetTableAdapters

    ''' <summary>
    ''' 通知DB APIのデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040801TableAdapters
        Inherits Global.System.ComponentModel.Component

#Region "共通定数"
        ''' <summary>
        ''' 既読フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum Read As Integer

            ''' <summary>未読</summary>
            Unread = 0

            ''' <summary>既読</summary>
            Read = 1

        End Enum

        Private Const ZeroLong As Long = 0
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

        ''' <summary>
        ''' 通知依頼情報登録処理
        ''' </summary>
        ''' <param name="dr">通知依頼情報</param>
        ''' <returns>処理結果件数</returns>
        ''' <remarks></remarks>
        Public Function InsertNoticeRequest(ByVal dr As IC3040801DataSet.IC3040801NoticeRequestRow) As Integer

            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("dr", dr.ToString, False))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3040801_001")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* IC3040801_001 */ ")
                    .Append("  INTO TBL_NOTICEREQUEST(")
                    .Append("       NOTICEREQID")
                    .Append("     , NOTICEREQCTG")
                    .Append("     , REQCLASSID")
                    .Append("     , DLRCD")
                    .Append("     , STRCD")
                    .Append("     , CRCUSTID")
                    .Append("     , CUSTOMERCLASS")
                    .Append("     , CSTKIND")
                    .Append("     , LASTNOTICEID")
                    .Append("     , STATUS")
                    .Append("     , CUSTOMNAME")
                    .Append("     , SALESSTAFFCD")
                    .Append("     , VCLID")
                    .Append("     , FLLWUPBOXSTRCD")
                    .Append("     , FLLWUPBOX")
                    ' $01 start step2開発
                    .Append("     , CSPAPERNAME")
                    ' $01 end   step2開発
                    .Append("     , PUSHINFO")
                    .Append("     , CREATEDATE")
                    .Append("     , UPDATEDATE")
                    .Append("     , CREATEACCOUNT")
                    .Append("     , UPDATEACCOUNT")
                    .Append("     , CREATEID")
                    .Append("     , UPDATEID")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("       :NOTICEREQID")
                    .Append("     , :NOTICEREQCTG")
                    .Append("     , :REQCLASSID")
                    .Append("     , :DLRCD")
                    .Append("     , :STRCD")
                    .Append("     , :CRCUSTID")
                    .Append("     , :CUSTOMERCLASS")
                    .Append("     , :CSTKIND")
                    .Append("     , :LASTNOTICEID")
                    .Append("     , :STATUS")
                    .Append("     , :CUSTOMNAME")
                    .Append("     , :SALESSTAFFCD")
                    .Append("     , :VCLID")
                    .Append("     , :FLLWUPBOXSTRCD")
                    .Append("     , :FLLWUPBOX")
                    ' $01 start step2開発
                    .Append("     , :CSPAPERNAME")
                    ' $01 end   step2開発
                    .Append("     , :PUSHINFO")
                    .Append("     , SYSDATE")
                    .Append("     , SYSDATE")
                    .Append("     , :CREATEACCOUNT")
                    .Append("     , :UPDATEACCOUNT")
                    .Append("     , :CREATEID")
                    .Append("     , :UPDATEID")
                    .Append(") ")
                End With

                query.CommandText = sql.ToString()

                'データテーブルの検証
                If dr Is Nothing Then
                    Return -1
                End If

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, dr.NOTICEREQID)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, dr.NOTICEREQCTG)
                ' $02 start 依頼種別ID 桁数変更対応
                query.AddParameterWithTypeValue("REQCLASSID", OracleDbType.Decimal, dr.REQCLASSID)
                ' $02 end 依頼種別ID 桁数変更対応
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, dr.STRCD)
                query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, dr.CRCUSTID)
                query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, dr.CUSTOMERCLASS)
                query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, dr.CSTKIND)
                query.AddParameterWithTypeValue("LASTNOTICEID", OracleDbType.Long, dr.LASTNOTICEID)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, dr.STATUS)
                query.AddParameterWithTypeValue("CUSTOMNAME", OracleDbType.Varchar2, dr.CUSTOMNAME)
                query.AddParameterWithTypeValue("SALESSTAFFCD", OracleDbType.Varchar2, dr.SALESSTAFFCD)
                query.AddParameterWithTypeValue("VCLID", OracleDbType.Varchar2, dr.VCLID)
                query.AddParameterWithTypeValue("FLLWUPBOXSTRCD", OracleDbType.Char, dr.FLLWUPBOXSTRCD)
                ' $02 start FollowUp-Box 桁数変更対応
                query.AddParameterWithTypeValue("FLLWUPBOX", OracleDbType.Decimal, dr.FLLWUPBOX)
                ' $02 end FollowUp-Box 桁数変更対応
                ' $01 start step2開発
                query.AddParameterWithTypeValue("CSPAPERNAME", OracleDbType.NVarchar2, dr.CSPAPERNAME)
                ' $01 end   step2開発
                query.AddParameterWithTypeValue("PUSHINFO", OracleDbType.Varchar2, dr.PUSHINFO)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, dr.SYSTEM)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.SYSTEM)

                'SQL実行(影響行数を返却)
                Dim executeNumber As Integer = query.Execute()

                Logger.Info(getReturnParam(CStr(executeNumber)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return executeNumber
            End Using
        End Function

        ''' <summary>
        ''' 通知依頼情報更新処理
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns>処理結果件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateNoticeRequest(ByVal dr As IC3040801DataSet.IC3040801NoticeRequestRow) As Integer
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("dr", dr.ToString, False))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3040801_002")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* IC3040801_002 */ ")
                    .Append("       TBL_NOTICEREQUEST ")
                    .Append("   SET STATUS = :STATUS")
                    .Append("     , LASTNOTICEID = :LASTNOTICEID")
                    .Append("     , UPDATEDATE = SYSDATE")
                    .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("     , UPDATEID = :UPDATEID")
                    .Append(" WHERE ")
                    .Append("       NOTICEREQID = :NOTICEREQID")
                    '「通知依頼種別=01(査定) And ステータス=3(受付)」の場合は条件を追加
                    If "01".Equals(dr.NOTICEREQCTG) And "3".Equals(dr.STATUS) Then
                        .Append("   AND STATUS = '1'")
                    End If
                End With

                query.CommandText = sql.ToString()

                'データテーブルの検証
                If dr Is Nothing Then
                    Return -1
                End If

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("LASTNOTICEID", OracleDbType.Long, dr.LASTNOTICEID)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, dr.STATUS)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.SYSTEM)
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Char, dr.NOTICEREQID)

                'SQL実行(影響行数を返却)
                Dim executeNumber As Integer = query.Execute()

                Logger.Info(getReturnParam(CStr(executeNumber)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return executeNumber
            End Using
        End Function

        ''' <summary>
        ''' 通知情報登録処理
        ''' </summary>
        ''' <param name="dr">通知情報</param>
        ''' <param name="noticeId">通知ID</param>
        ''' <returns>処理結果件数</returns>
        ''' <remarks></remarks>
        Public Function InsertNoticeInfo(ByVal dr As IC3040801DataSet.IC3040801NoticeInfoRow, ByVal noticeId As Long) As Integer
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("dr", dr.ToString, False))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3040801_003")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT /* IC3040801_003 */ ")
                    .Append("  INTO TBL_NOTICEINFO(")
                    .Append("       NOTICEID")
                    .Append("     , NOTICEREQID")
                    .Append("     , FROMACCOUNT")
                    .Append("     , FROMCLIENTID")
                    .Append("     , FROMACCOUNTNAME")
                    .Append("     , TOACCOUNT")
                    .Append("     , TOCLIENTID")
                    .Append("     , TOACCOUNTNAME")
                    .Append("     , SENDDATE")
                    .Append("     , READFLG")
                    .Append("     , STATUS")
                    .Append("     , MESSAGE")
                    .Append("     , SESSIONVALUE")
                    .Append("     , CREATEDATE")
                    .Append("     , UPDATEDATE")
                    .Append("     , CREATEACCOUNT")
                    .Append("     , UPDATEACCOUNT")
                    .Append("     , CREATEID")
                    .Append("     , UPDATEID")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    If noticeId = ZeroLong Then
                        .Append("       SEQ_NOTICEINFO_NOTICEID.NEXTVAL")
                    Else
                        .Append("       :NOTICEID")
                    End If
                    .Append("     , :NOTICEREQID")
                    .Append("     , :FROMACCOUNT")
                    .Append("     , :FROMCLIENTID")
                    .Append("     , :FROMACCOUNTNAME")
                    .Append("     , :TOACCOUNT")
                    .Append("     , :TOCLIENTID")
                    .Append("     , :TOACCOUNTNAME")
                    .Append("     , :SENDDATE")
                    .Append("     , :READFLG")
                    .Append("     , :STATUS")
                    .Append("     , :MESSAGE")
                    .Append("     , :SESSIONVALUE")
                    .Append("     , SYSDATE")
                    .Append("     , SYSDATE")
                    .Append("     , :CREATEACCOUNT")
                    .Append("     , :UPDATEACCOUNT")
                    .Append("     , :CREATEID")
                    .Append("     , :UPDATEID")
                    .Append(") ")
                End With

                query.CommandText = sql.ToString()

                'データテーブルの検証
                If dr Is Nothing Then
                    Return -1
                End If

                'SQLパラメータ設定
                If noticeId <> ZeroLong Then
                    query.AddParameterWithTypeValue("NOTICEID", OracleDbType.Int64, noticeId)
                End If
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, dr.NOTICEREQID)
                query.AddParameterWithTypeValue("FROMACCOUNT", OracleDbType.Varchar2, dr.FROMACCOUNT)
                query.AddParameterWithTypeValue("FROMCLIENTID", OracleDbType.Varchar2, dr.FROMCLIENTID)
                query.AddParameterWithTypeValue("FROMACCOUNTNAME", OracleDbType.Varchar2, dr.FROMACCOUNTNAME)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, dr.TOACCOUNT)
                query.AddParameterWithTypeValue("TOCLIENTID", OracleDbType.Varchar2, dr.TOCLIENTID)
                query.AddParameterWithTypeValue("TOACCOUNTNAME", OracleDbType.Varchar2, dr.TOACCOUNTNAME)

                '2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正 START
                'If dr.FROMACCOUNT.Equals(dr.TOACCOUNT) Then
                '    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, CStr(Read.Read))
                'Else
                '    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, CStr(Read.Unread))
                'End If
                query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, dr.READFLAG)
                '2012/03/24 KN 小澤 【SALES_1】自分→自分の場合、未読にする修正 END

                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, dr.SENDDATE)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, dr.STATUS)
                query.AddParameterWithTypeValue("MESSAGE", OracleDbType.Varchar2, dr.MESSAGE)
                query.AddParameterWithTypeValue("SESSIONVALUE", OracleDbType.Varchar2, dr.SESSIONVALUE)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, dr.SYSTEM)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.SYSTEM)

                'SQL実行(影響行数を返却)
                Dim executeNumber As Integer = query.Execute()

                Logger.Info(getReturnParam(CStr(executeNumber)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return executeNumber
            End Using
        End Function

        ''' <summary>
        ''' 通知依頼ID取得処理
        ''' </summary>
        ''' <returns>通知依頼ID</returns>
        ''' <remarks></remarks>
        Public Function SelectNoticeRequestId() As Long
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040801DataSet.IC3040801SequenceNoticeRequestIdDataTable)("IC3040801_004")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* IC3040801_004 */")
                    .Append("        SEQ_NOTICEREQUEST_NOTICEREQID.NEXTVAL AS SEQ_NOTICEREQID")
                    .Append("   FROM DUAL")
                End With

                query.CommandText = sql.ToString()
                Dim dt As IC3040801DataSet.IC3040801SequenceNoticeRequestIdDataTable = query.GetData()

                Dim noticeRequestId As Long = CType(dt(0).SEQ_NOTICEREQID, Long)

                Logger.Info(getReturnParam(CStr(noticeRequestId)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return noticeRequestId
            End Using
        End Function

        ''' <summary>
        ''' 通知情報の既読フラグを更新する処理
        ''' </summary>
        ''' <param name="dr">通知情報</param>
        ''' <returns>処理結果件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateConfirmed(ByVal dr As IC3040801DataSet.IC3040801NoticeInfoRow) As Integer

            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("dr", dr.ToString, False))


            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3040801_005")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append("UPDATE /* IC3040801_005 */ ")
                    .Append("       TBL_NOTICEINFO ")
                    .Append("   SET READFLG = 1")
                    .Append("     , UPDATEDATE = SYSDATE")
                    .Append("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("     , UPDATEID = :UPDATEID")
                    .Append(" WHERE ")
                    .Append("       READFLG = 0")
                    .Append("   AND TOACCOUNT = :TOACCOUNT")
                End With

                'データテーブルの検証
                If dr Is Nothing Then
                    Return -1
                End If

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, dr.SYSTEM)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Char, dr.ACCOUNT)

                'SQL実行(影響行数を返却)
                Dim executeNumber As Integer = query.Execute()

                Logger.Info(getReturnParam(CStr(executeNumber)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return executeNumber
            End Using
        End Function

        ''' <summary>
        ''' 通知情報取得処理
        ''' </summary>
        ''' <param name="noticeRequestId"></param>
        ''' <returns>通知情報</returns>
        ''' <remarks></remarks>
        Public Function SelectNoticeInfo(ByVal noticeRequestId As Long) As IC3040801DataSet.IC3040801SelectNoticeInfoDataTable

            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("noticeRequestId", CStr(noticeRequestId), False))


            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040801DataSet.IC3040801SelectNoticeInfoDataTable)("IC3040801_006")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* IC3040801_006 */")
                    .Append("        T2.NOTICEREQID")
                    .Append("      , T2.NOTICEID")
                    .Append("      , T2.FROMACCOUNT")
                    .Append("      , T2.FROMCLIENTID")
                    .Append("      , T2.FROMACCOUNTNAME")
                    .Append("      , T2.TOACCOUNT")
                    .Append("      , T2.TOCLIENTID")
                    .Append("      , T2.TOACCOUNTNAME")
                    .Append("      , T2.STATUS")
                    .Append("   FROM TBL_NOTICEREQUEST T1")
                    .Append("      , TBL_NOTICEINFO T2")
                    .Append("  WHERE T1.NOTICEREQID = T2.NOTICEREQID")
                    .Append("    AND T1.STATUS = T2.STATUS")
                    .Append("    AND T1.NOTICEREQID = :NOTICEREQID")
                    .Append("    AND T1.LASTNOTICEID <= T2.NOTICEID")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Long, noticeRequestId)

                Dim dt As IC3040801DataSet.IC3040801SelectNoticeInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(getReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 通知ID取得処理
        ''' </summary>
        ''' <returns>通知ID</returns>
        ''' <remarks></remarks>
        Public Function SelectNoticeId() As Long
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040801DataSet.IC3040801SequenceNoticeIdDataTable)("IC3040801_007")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* IC3040801_007 */")
                    .Append("        SEQ_NOTICEINFO_NOTICEID.NEXTVAL AS SEQ_NOTICEID")
                    .Append("   FROM DUAL")
                End With

                query.CommandText = sql.ToString()
                Dim dt As IC3040801DataSet.IC3040801SequenceNoticeIdDataTable = query.GetData()

                Dim noticeId As Long = CType(dt(0).SEQ_NOTICEID, Long)

                Logger.Info(getReturnParam(CStr(noticeId)))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return noticeId
            End Using
        End Function

        ''' <summary>
        ''' Push情報取得処理
        ''' </summary>
        ''' <param name="noticeRequestID"></param>
        ''' <returns>通知依頼ID</returns>
        ''' <remarks></remarks>
        Public Function SelectPushInfo(ByVal noticeRequestId As Long, ByVal status As String) As IC3040801DataSet.IC3040801SelectNoticeRequestDataTable
            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040801DataSet.IC3040801SelectNoticeRequestDataTable)("IC3040801_008")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* IC3040801_008 */")
                    .Append("        T1.PUSHINFO ")
                    .Append("      , NVL(T2.NOTICEMSG_DLR,NVL(T2.NOTICEMSG_ENG,'NOMESSAGE')) AS NOTICEMSG ")
                    .Append("   FROM TBL_NOTICEREQUEST T1")
                    .Append("      , TBL_NOTICECTGMST T2")
                    .Append("  WHERE T1.NOTICEREQCTG = T2.NOTICEREQCTG")
                    .Append("    AND T1.NOTICEREQID = :NOTICEREQID")
                    .Append("    AND T2.NOTICESTATUSID = :STATUS")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Long, noticeRequestId)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, status)

                Dim dt As IC3040801DataSet.IC3040801SelectNoticeRequestDataTable = query.GetData()

                'Logger.Info(getReturnParam(pushInfo))
                Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

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

Partial Class IC3040801DataSet
End Class
