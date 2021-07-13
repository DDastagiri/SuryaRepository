'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040802Dataset.vb
'─────────────────────────────────────
'機能： 通知件数インターフェース
'補足： 
'作成： -
'更新： -          TMEJ t.shimamura サービス入庫追加 $02
'更新： 2014/01/10 TMEJ t.shimamura セールスタブレット契約承認機能開発$03
'更新： 2018/11/12 NSK  m.sakamoto  17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常$04 
'─────────────────────────────────────

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

        ' $01 start step2開発
        ''' <summary>通知依頼種別：苦情</summary>
        Private Const noticeClassClaim As String = "05"
        ''' <summary>通知依頼種別：CSSurvey</summary>
        Private Const noticeClassCSSurvey As String = "06"
        ' $01 end   step2開発

        ' $02 start サービス入庫
        ''' <summary>通知依頼種別：サービス入庫</summary>
        Private Const noticeClassSurviceStore As String = "07"
        ' $02 end サービス入庫

        ' $02 start 契約承認依頼
        ''' <summary>通知依頼種別：契約承認依頼</summary>
        Private Const NoticeClassContractApprovalRequest As String = "08"
        ''' <summary>ステータス：依頼</summary>
        Private Const StatusRequest As String = "1"
        ''' <summary>ステータス：キャンセル</summary>
        Private Const StatusCanncel As String = "2"


        ' $02 end 契約承認依頼

        ' $04 start 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常 
        ''' <summary>注文情報登録・変更</summary>
        Private Const noticeClassOrderInfomation As String = "09"

        ''' <summary>受注後フォロー</summary>
        Private Const noticeClassAfterOdrFollow As String = "10"

        ''' <summary>納車予定日変更</summary>
        Private Const noticeClassDeliScheDateChg As String = "11"

        ''' <summary>フォローアップメモ更新</summary>
        Private Const noticeClassFllwupMemoUpdate As String = "12"
        ' $04 end 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常

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
        ' $03 start リーダフラグ対応
        ''' <summary>
        ''' 通知未読件数取得処理
        ''' </summary>
        ''' <param name="account">ユーザーID</param>
        ''' <param name="sendDate">受信日時</param>
        ''' <param name="staffAuthority">ユーザー権限情報</param>
        ''' <param name="LeaderFlag">リーダフラグ</param>
        ''' <returns>通知未読件数</returns>
        ''' <remarks></remarks>
        Public Function SelectUnreadNotice(ByVal account As String,
                                           ByVal sendDate As Date,
                                           ByVal staffAuthority As Boolean,
                                           ByVal leaderFlag As Boolean) As Long
            ' $03 end リーダフラグ対応

            Logger.Info(getLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(getLogParam("account", account, False) & _
                        getLogParam("sendDate", CStr(sendDate), True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040802DataSet.IC3040802UnreadNoticeCountDataTable)("IC3040802_001")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql

                    ' $03 start チームリーダ対応
                    If staffAuthority And leaderFlag Then
                        .Append(" SELECT /* IC3040802_001 */ ")
                        .Append("        A.CNT - B.CNT AS CNT ")
                        .Append("   FROM ( ")
                    End If
                    ' $03 emd
                    .Append("     SELECT /* IC3040802_001 */ ")
                    .Append("            COUNT(1) AS CNT ")
                    .Append("      FROM TBL_NOTICEREQUEST T1 ")
                    .Append("         , TBL_NOTICEINFO T2 ")
                    .Append("     WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                    'セールスの場合は条件追加
                    If staffAuthority Then
                        .Append("       AND T1.STATUS = T2.STATUS ")
                        ' $04 start 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常
                        '.Append("       AND T1.NOTICEREQCTG <> :NOTICEREQCTG4 ")
                        .Append("       AND T1.NOTICEREQCTG IN(:ASSESSMENT,:CONSULTATION,:HELP,:CLAIM,:CSSURVEY,:SURVICESTORE,:CONTRACTAPPROVAL,:ORDERINFOMATION,:AFTERODRFOLLOW,:DELISCHEDATECHG,:FLLWUPMEMOUPDATE) ")
                        ' $04 end 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常
                    End If
                    .Append("       AND T2.READFLG = 0 ")
                    .Append("       AND T2.TOACCOUNT = :TOACCOUNT ")
                    .Append("       AND T2.SENDDATE >= TRUNC(:SENDDATE) ")
                    ' $03 start チームリーダ対応
                    If staffAuthority And leaderFlag Then
                        .Append("        ) A ")
                        .Append("      ,( ")
                        .Append("     SELECT ")
                        .Append("            COUNT(1) AS CNT ")
                        .Append("       FROM TBL_NOTICEREQUEST T1 ")
                        .Append("          , TBL_NOTICEINFO T2 ")
                        .Append("      WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                        .Append("        AND T1.STATUS = T2.STATUS ")
                        .Append("        AND T1.NOTICEREQCTG IN(:NOTICEREQCTG2,:NOTICEREQCTG3,:NOTICEREQCTG8) ")
                        .Append("        AND T2.READFLG = 0 ")
                        .Append("        AND T1.STATUS IN (:REQUEST, :CANNCEL) ")
                        .Append("        AND T2.TOACCOUNT = :TOACCOUNT ")
                        .Append("        AND T2.SENDDATE >= TRUNC(:SENDDATE) ")
                        .Append("      ) B ")
                    End If
                    ' $03 end

                End With
                query.CommandText = sql.ToString()
                'SQLパラメータ設定
                'セールスの場合は条件追加
                If staffAuthority Then

                    ' $04 start 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常
                    'query.AddParameterWithTypeValue("NOTICEREQCTG4", OracleDbType.Char, noticeClassHelp)
                    query.AddParameterWithTypeValue("ASSESSMENT", OracleDbType.Char, noticeClassAssessment)
                    query.AddParameterWithTypeValue("CONSULTATION", OracleDbType.Char, noticeClassPriceConsultation)
                    query.AddParameterWithTypeValue("HELP", OracleDbType.Char, noticeClassHelp)
                    query.AddParameterWithTypeValue("CLAIM", OracleDbType.Char, noticeClassClaim)
                    query.AddParameterWithTypeValue("CSSURVEY", OracleDbType.Char, noticeClassCSSurvey)
                    query.AddParameterWithTypeValue("SURVICESTORE", OracleDbType.Char, noticeClassSurviceStore)
                    query.AddParameterWithTypeValue("CONTRACTAPPROVAL", OracleDbType.Char, NoticeClassContractApprovalRequest)
                    query.AddParameterWithTypeValue("ORDERINFOMATION", OracleDbType.Char, noticeClassOrderInfomation)
                    query.AddParameterWithTypeValue("AFTERODRFOLLOW", OracleDbType.Char, noticeClassAfterOdrFollow)
                    query.AddParameterWithTypeValue("DELISCHEDATECHG", OracleDbType.Char, noticeClassDeliScheDateChg)
                    query.AddParameterWithTypeValue("FLLWUPMEMOUPDATE", OracleDbType.Char, noticeClassFllwupMemoUpdate)
                    ' $04 end 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 SIT_ISSUE No.24 通知履歴の表示異常

                    ' $03 start 契約承認依頼
                    If leaderFlag Then
                        query.AddParameterWithTypeValue("NOTICEREQCTG3", OracleDbType.Char, noticeClassPriceConsultation)
                        query.AddParameterWithTypeValue("NOTICEREQCTG2", OracleDbType.Char, noticeClassHelp)
                        query.AddParameterWithTypeValue("NOTICEREQCTG8", OracleDbType.Char, NoticeClassContractApprovalRequest)
                        query.AddParameterWithTypeValue("REQUEST", OracleDbType.Char, StatusRequest)
                        query.AddParameterWithTypeValue("CANNCEL", OracleDbType.Char, StatusCanncel)
                        ' $03 end 契約承認依頼
                    End If

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
