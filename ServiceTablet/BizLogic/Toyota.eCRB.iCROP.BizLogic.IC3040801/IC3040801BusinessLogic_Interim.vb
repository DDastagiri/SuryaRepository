'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040801BusinessLogic_Interim.vb
'─────────────────────────────────────
'機能： 通知登録インターフェース暫定対応
'補足： BMTSの要望で、PUSHを2回実行する処理を暫定的に作成
'作成： 2012/03/13 KN 佐藤
'更新： 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
'2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
'2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

'IC3040801 通知APIクラスの暫定対応
Partial Public Class IC3040801BusinessLogic

#Region "共通定数(暫定)"

    ''' <summary>通知件数取得メソッド名</summary>
    Private Const C_SETNOTICE As String = "icropScript.ui.setNotice()"

#End Region

#Region "通知DB API(暫定)"

    ''' <summary>
    ''' サービス用通知メイン(画面用)(暫定)
    ''' </summary>
    ''' <param name="xmlDataClass">通知情報</param>
    ''' <param name="noticeDisposalMode">固有、汎用フラグ</param>
    ''' <returns>戻り値情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Public Function InterimNoticeDisplayService(ByVal xmlDataClass As XmlNoticeData,
                                  ByVal noticeDisposalMode As NoticeDisposal) As XmlCommon
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogNoticeData(xmlDataClass) &
                    GetLogParam("noticeDisposalMode", CStr(noticeDisposalMode), True))

        Dim returnXml As New Response()

        Try
            '値チェック
            CheckXmlDataClass(xmlDataClass, noticeDisposalMode)

            '通知DB処理
            Me.noticeDBClone = New IC3040801BusinessLogic
            If Me.accountCheck OrElse xmlDataClass.AccountList.Count <> 0 Then
                Me.noticeDBClone.RegistsNoticeDB(xmlDataClass, noticeDisposalMode)
            Else
                Me.noticeDBClone.RegistsNoticeDBNoAccount(xmlDataClass)
            End If

            'PushServer処理
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
            'InterimSendPushServerService(xmlDataClass)
            Me.InterimSendPushServerService(xmlDataClass)
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

            '成功情報を格納
            Me.errorInfo.ResultId = RESULTID_SUCCESS_CONST
            Me.errorInfo.Message = MESSAGE_SUCCESS_CONST

        Catch ex As ArgumentException
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Catch ex As OracleExceptionEx
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.ResultId = Me.noticeDBClone.errorInfo.ResultId
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            '失敗情報を格納
            Me.errorInfo.ResultId = RESULTID_FAILURE_CONST
            Me.errorInfo.Message = MESSAGE_FAILURE_CONST
            Throw

        Finally
            'XML作成
            Me.errorInfo.NoticeRequestId = xmlDataClass.RequestNotice.RequestId
            returnXml = CreateReturnXml()
            'ログ出力
            Using writer As New StringWriter(CultureInfo.InvariantCulture())
                Dim outXml As New XmlSerializer(GetType(Response))
                outXml.Serialize(writer, returnXml)
                If RESULTID_SUCCESS_CONST.Equals(Me.errorInfo.ResultId) Then
                    '成功
                    Logger.Info(writer.ToString)
                Else
                    '失敗
                    Logger.Error(writer.ToString)
                End If
            End Using
        End Try
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return Me.errorInfo
    End Function

#End Region

#Region "PushServer処理(暫定)"

    ''' <summary>
    ''' サービス用Push送信(暫定)
    ''' </summary>
    ''' <param name="xmlDataClass">通知情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20）
    ''' </history>
    Private Sub InterimSendPushServerService(ByVal xmlDataClass As XmlNoticeData)
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("xmlDataClass", xmlDataClass.ToString, False))

        '通知履歴キャンセルでPush情報がない場合は処理しない
        If String.IsNullOrEmpty(xmlDataClass.RequestNotice.PushInfo) Then
            Return
        End If

        'popup用Push情報を作成
        Dim pushPopUp As New StringBuilder
        With pushPopUp
            'ヘッダー作成(cat：type：sub)
            .Append(GetCategory(xmlDataClass.PushInfo.PushCategory))
            .Append(GetPushType(xmlDataClass.PushInfo.PositionType))
            .Append(GetSub(xmlDataClass.PushInfo.DisplayType))
            'ユーザー情報作成(uid)
            .Append(GetUserId(ReplaceAccount))
            'フッター作成(time：color：width：height：pox：poy：msg、url、fname：js1：js2)
            .Append(GetTime(CStr(xmlDataClass.PushInfo.Time)))
            .Append(GetColor(xmlDataClass.PushInfo.Color))
            .Append(GetWidth(CStr(xmlDataClass.PushInfo.PopWidth)))
            .Append(GetHeight(CStr(xmlDataClass.PushInfo.PopHeight)))
            .Append(GetPositionX(CStr(xmlDataClass.PushInfo.PopX)))
            .Append(GetPositionY(CStr(xmlDataClass.PushInfo.PopY)))
            .Append(GetDisplayContents(xmlDataClass.PushInfo.DisplayType, _
                                       xmlDataClass.PushInfo.DisplayContents))
            .Append(GetJavaScript1(C_SETNOTICE))
            .Append(GetJavaScript2(xmlDataClass.PushInfo.ActionFunction))
        End With

        '表示時関数が存在する場合
        Dim pushAction As New StringBuilder
        If Not String.IsNullOrEmpty(xmlDataClass.PushInfo.DisplayFunction) Then
            'action用Push情報を作成
            With pushAction
                'ヘッダー作成(cat：type：sub)
                .Append(GetCategory(CStr(PushConstCategory.action)))
                .Append(GetPushType(CStr(PushConstType.main)))
                .Append(GetSub(CStr(PushConstSub.js)))
                'ユーザー情報作成(uid)
                .Append(GetUserId(ReplaceAccount))
                'フッター作成(js1)
                .Append(GetJavaScript1(xmlDataClass.PushInfo.DisplayFunction))
            End With
        End If

        'Push送信用クラスの生成
        Dim visitUtility As New VisitUtility

        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
        '権限情報を取得する
        Dim sendOperationList As String() = Nothing
        Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            (New SystemEnvSetting).GetSystemEnvSetting("SEND_OPERATION")
        '情報が取得できれば配列に格納
        If Not (IsNothing(drSystemEnvSetting)) AndAlso Not (String.IsNullOrEmpty(drSystemEnvSetting.PARAMVALUE)) Then
            sendOperationList = Split(drSystemEnvSetting.PARAMVALUE, ",")
        End If
        '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

        'Account分をPushする
        For Each sendAccountList As XmlAccount In xmlDataClass.AccountList
            'popup用Push情報を取得
            Dim sendPushPopUp As String = pushPopUp.ToString
            'action用Push情報を取得
            Dim sendPushAction As String = pushAction.ToString
            'USERを置換する
            Dim account As String
            If Not String.IsNullOrEmpty(sendAccountList.ToAccount) Then
                account = sendAccountList.ToAccount
            Else
                account = sendAccountList.ToClientId
            End If

            sendPushPopUp = Replace(sendPushPopUp, ReplaceAccount, account)

            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） START
            '汎用の場合はJS2を置換する
            If Me.isJavaScript2Replace(account, sendOperationList) Then
                If String.IsNullOrEmpty(xmlDataClass.PushInfo.ActionFunction) Then
                    sendPushPopUp = Replace(sendPushPopUp, ReplaceJavascript2, ReplaceJavascript2After)
                Else
                    sendPushPopUp = Replace(sendPushPopUp, ReplaceJavascript2, ReplaceJavascript2AfterComma)
                End If
            Else
                sendPushPopUp = Replace(sendPushPopUp, ReplaceJavascript2, String.Empty)
            End If
            '2012/09/05 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.20） END

            'popup用Pushを実行する
            visitUtility.SendPush(sendPushPopUp, xmlDataClass.RequestNotice.DealerCode)

            'action用Pushを実行する
            If String.IsNullOrEmpty(sendPushAction) = False Then
                sendPushAction = Replace(sendPushAction, ReplaceAccount, account)
                visitUtility.SendPush(sendPushAction, xmlDataClass.RequestNotice.DealerCode)
            End If
        Next
        Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
    End Sub

#End Region

End Class