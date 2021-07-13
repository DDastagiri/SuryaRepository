'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3040802.aspx.vb
'─────────────────────────────────────
'機能： 通知一覧(MG用)
'補足： 
'作成： 2012/01/05 TCS 明瀬
'更新： 2012/04/18 TCS 明瀬 HTMLエンコード対応
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'更新： 2013/12/02 TCS 森   Aカード情報相互連携開発
'更新： 2014/05/10 TCS 武田 受注後フォロー機能開発
'更新： 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.BizLogic
Imports Toyota.eCRB.Tool.Notify.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic

''' <summary>
''' 通知送受信一覧(MG用)
''' プレゼンテーションクラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3040802
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 1ページあたりの表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGEMAXLINE As String = "10"

    ''' <summary>
    ''' 遷移先画面ID　見積作成画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_ESTIMATE As String = "SC3070201"

    ''' <summary>
    ''' 遷移先画面ID　顧客詳細(顧客情報)画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPID_CUSTDETAIL As String = "SC3080201"

    ''' <summary>
    ''' 通知依頼種別　価格相談
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_DISCOUNTAPPROVAL As String = "02"

    ''' <summary>
    ''' 通知依頼種別　ヘルプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_HELP As String = "03"

    ' 2013/12/02 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 通知依頼種別 注文依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_ORDER As String = "08"
    ' 2013/12/02 TCS 森 Aカード情報相互連携開発 END


    ''' <summary>
    ''' 通知最終ステータス　依頼　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_REQUEST As String = "1"

    ''' <summary>
    ''' セッションキー　見積管理ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_ESTIMATEID As String = "EstimateId"

    ''' <summary>
    ''' セッションキー　メニューロックフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_MENULOCKFLG As String = "MenuLockFlag"

    ''' <summary>
    ''' セッションキー　オペレーションコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_OPERATIONCD As String = "OperationCode"

    ''' <summary>
    ''' セッションキー　商談中フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_BUSINESSFLG As String = "BusinessFlg"

    ''' <summary>
    ''' セッションキー　読取専用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_READONLYFLG As String = "ReadOnlyFlg"

    ''' <summary>
    ''' セッションキー　依頼ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_REQUESTID As String = "NoticeReqId"

    ''' <summary>
    ''' セッションキー　顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTSEGMENT As String = "SearchKey.CUSTSEGMENT"

    ''' <summary>
    ''' セッションキー　顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"

    ''' <summary>
    ''' セッションキー　顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' セッションキー　活動先顧客コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"

    ''' <summary>
    ''' セッションキー　活動先顧客名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CRCUSTNAME As String = "SearchKey.CRCUSTNAME"

    ''' <summary>
    ''' セッションキー　顧客担当セールススタッフコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    ''' <summary>
    ''' セッションキー　Follow-up Box店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"

    ''' <summary>
    ''' セッションキー　Follow-up Box内連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' I/F結果ID　成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IF_SUCCESS As String = "000000"

    ''' <summary>
    ''' I/F結果ID　DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IF_DBTIMEOUT As String = "006000"

    ''' <summary>
    ''' メッセージID　通知IFエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERRMSGID_NOTICEIF As Integer = 9001

    ''' <summary>
    ''' メッセージID　システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERRMSGID_SYSTEM As Integer = 9999

#End Region

#Region "イベント処理"
    ''' <summary>
    ''' ロードの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/18 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString(), e.ToString))

        If Not Page.IsPostBack Then

            Dim bizClass As New SC3040802BusinessLogic
            Dim staff As StaffContext = StaffContext.Current

            '通知の件数を取得する
            Dim noticeCountDt As SC3040802DataSet.SC3040802NoticeCountDataTable = bizClass.GetNoticeInfoCountDT(staff.Account, staff.DlrCD)

            '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
            '2014/05/10 TCS 武田 受注後フォロー機能開発 END

            '通知件数が１件以上なら通知データを表示
            noticeInfoPanel.Visible = True
            '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
            '2014/05/10 TCS 武田 受注後フォロー機能開発 END

            '通知件数をHiddenに保存
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
            Me.noticeCountHidden.Value = HttpUtility.HtmlEncode(noticeCountDt.Item(0).COUNT.ToString(CultureInfo.CurrentCulture))
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 End

            '次の{0}件を読み込む...
            Me.noticeRepeater.ForwardPagerLabel = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(2), PAGEMAXLINE)
            '前の{0}件を読み込む...
            Me.noticeRepeater.RewindPagerLabel = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(8), PAGEMAXLINE)

            '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
            '2014/05/10 TCS 武田 受注後フォロー機能開発 END

        End If

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' 依頼通知一覧(noticeRepeater)の表示イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/18 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Protected Sub NoticeRepeater_ClientCallback(sender As Object, e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles noticeRepeater.ClientCallback

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString, e.ToString))

        Dim beginRowIndex As Integer = 0

        Try
            If (Integer.TryParse(CType(e.Arguments("beginRowIndex"), String), beginRowIndex)) _
            AndAlso beginRowIndex < Me.noticeCountHidden.Value Then

                Dim sbRows As New StringBuilder(2000)
                Dim blnFirstElement As Boolean = True

                Dim bizClass As New SC3040802BusinessLogic
                Dim noticeInfoDs As SC3040802DataSet

                '初期表示情報取得
                noticeInfoDs = bizClass.GetInitialData()

                If noticeInfoDs IsNot Nothing Then

                    Dim noticeInfoDt As SC3040802DataSet.SC3040802ReturnNoticeInfoDataTable = noticeInfoDs.SC3040802ReturnNoticeInfo

                    '通知情報のテーブル件数分ループ処理
                    For i As Integer = beginRowIndex To noticeInfoDt.Rows.Count - 1

                        '２個目以降の要素には、先頭にカンマを付加する
                        If blnFirstElement Then
                            blnFirstElement = False
                        Else
                            sbRows.Append(",")
                        End If

                        '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
                        '2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                        sbRows.AppendFormat(CultureInfo.CurrentCulture, _
                                            "{{ ""NO"" : {0}, " & _
                                            """REQNOTICE"" : {1}, " & _
                                            """REQCTGID"" : ""{2}""," & _
                                            """REQCLASSID"" : {3}," & _
                                            """CSTKIND"" : ""{4}""," & _
                                            """CSTCLASS"" : ""{5}""," & _
                                            """CRCSTID"" : ""{6}""," & _
                                            """FROMSTAFFNAME"" : ""{7}""," & _
                                            """CUSTOMERNAME"" : ""{8}""," & _
                                            """TIMEMESSAGE"" : ""{9}""," & _
                                            """MESSAGE1"" : ""{10}""," & _
                                            """MESSAGE2"" : ""{11}""," & _
                                            """MAXROW"" : {12}," & _
                                            """ICON_IMGFILE"" : ""{13}""," & _
                                            """FROMSTAFFACCOUNT"" : ""{14}""," & _
                                            """SALESSTAFFCD"" : ""{15}""," & _
                                            """FLLWUPBOXSTRCD"" : ""{16}""," & _
                                            """FLLWUPBOX"" : ""{17}""," & _
                                            """STATUS"" : ""{18}""," & _
                                            """LOYALCUSTOMER_FLG"" : {19} }}", _
                                            (i + 1), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).NOTICEREQID), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).NOTICEREQCTG), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).REQCLASSID), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).CUSTOMERKIND), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).CUSTOMERCLASS), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).CRCUSTID.Trim), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).FROMSTAFFNAME), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).CUSTOMERNAME), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).TIMEMESSAGE), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).MESSAGE1), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).MESSAGE2), _
                                            noticeInfoDt.Rows.Count, _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).STAFFICON), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).FROMSTAFFACCOUNT), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).SALESSTAFFCD), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).FLLWUPBOXSTRCD.Trim), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).FLLWUPBOX), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).STATUS), _
                                            HttpUtility.HtmlEncode(noticeInfoDt(i).LOYALCUSTOMERFLG))
                        ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                        '2012/04/18 TCS 明瀬 HTMLエンコード対応 End
                    Next i

                    'ここで設定する値を使用して、aspx内のjavascriptでHTMLを動的に生成する
                    e.Results("@rows") = "[" & sbRows.ToString() & "]"

                End If

            Else
                e.Results("@rows") = "[]"
            End If

            Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

        Catch ex As Exception
            Dim errStrings As New StringBuilder(500)
            errStrings.AppendFormat(CultureInfo.CurrentCulture, _
                                    "{{ ""NO"" : {0}, " & _
                                    """ERRMESSAGE"" : ""{1}"" }}",
                                    ERRMSGID_SYSTEM, HttpUtility.JavaScriptStringEncode(ex.Message))

            e.Results("@rows") = "[" & errStrings.ToString() & "]"

            Logger.Error(System.Reflection.MethodBase.GetCurrentMethod.Name & "_" & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' 依頼通知一覧(noticeRepeater)の通知選択イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/18 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Protected Sub NextButton_Click(sender As Object, e As System.EventArgs) Handles nextButton.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString, e.ToString))

        '最終ステータスが"1"のときのみ通知登録IFの処理を呼び出す
        If Me.lastStatusHidden.Value = STATUS_REQUEST Then
            '通知I/Fの処理
            Dim bizClass As New SC3040802BusinessLogic

            Using IFParamDt As New SC3040802DataSet.SC3040802ParamIfDataTable

                '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
                IFParamDt.AddSC3040802ParamIfRow(HttpUtility.HtmlDecode(Me.toAccountHidden.Value), _
                                                 HttpUtility.HtmlDecode(Me.toAccountNameHidden.Value), _
                                                 HttpUtility.HtmlDecode(Me.reqCtgIdHidden.Value), _
                                                 HttpUtility.HtmlDecode(Me.reqNoticeHidden.Value), _
                                                 HttpUtility.HtmlDecode(Me.reqClassIdHidden.Value), _
                                                 HttpUtility.HtmlDecode(Me.cstNameHidden.Value))
                '2012/04/18 TCS 明瀬 HTMLエンコード対応 End

                Dim rsltXml As XmlCommon = bizClass.SetRequestNoticeInfo(IFParamDt)

                If Not IsNothing(rsltXml.ResultId) Then
                    If rsltXml.ResultId.Equals(IF_DBTIMEOUT) Then
                        'IFの結果がDBタイムアウトの場合
                        ShowMessageBox(ERRMSGID_NOTICEIF)
                        Return
                    End If
                End If

            End Using
        End If

        ' 2013/12/02 TCS 森 Aカード情報相互連携開発 STRAT
        If Me.reqCtgIdHidden.Value = NOTICEREQ_DISCOUNTAPPROVAL Or _
            Me.reqCtgIdHidden.Value = NOTICEREQ_ORDER Then
            '通知依頼種別が"02"(価格相談)の場合
            'または通知依頼種別が"08"(注文承認)の場合
            ' ※条件２つ目追加
            ' 2013/12/02 TCS 森 Aカード情報相互連携開発 END

            'セッション情報をセット
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
            Me.SetValue(ScreenPos.Next, SESSION_KEY_ESTIMATEID, CLng(HttpUtility.HtmlDecode(Me.reqClassIdHidden.Value)))    '見積管理ID(依頼種別ID)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_REQUESTID, CLng(HttpUtility.HtmlDecode(Me.reqNoticeHidden.Value)))      '依頼ID
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 End
            Me.SetValue(ScreenPos.Next, SESSION_KEY_OPERATIONCD, StaffContext.Current.OpeCD)        'オペレーションコード
            Me.SetValue(ScreenPos.Next, SESSION_KEY_MENULOCKFLG, "False")                           'メニューロックフラグ
            Me.SetValue(ScreenPos.Next, SESSION_KEY_BUSINESSFLG, "False")                           '商談中フラグ
            Me.SetValue(ScreenPos.Next, SESSION_KEY_READONLYFLG, "True")                            '読取専用フラグ

            '見積作成画面へ遷移
            Me.RedirectNextScreen(DISPID_ESTIMATE)

        ElseIf Me.reqCtgIdHidden.Value = NOTICEREQ_HELP Then
            '通知依頼種別が"03"(ヘルプ)の場合

            'セッション情報をセット
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, HttpUtility.HtmlDecode(Me.cstKindHidden.Value))                    '顧客種別(1:自社客 / 2:未取引客)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTSEGMENT, HttpUtility.HtmlDecode(Me.cstKindHidden.Value))                '顧客種別(1:自社客 / 2:未取引客)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, HttpUtility.HtmlDecode(Me.cstClassHidden.Value))             '顧客分類(1:所有者 / 2:使用者 / 3:その他)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, HttpUtility.HtmlDecode(Me.crCstIdHidden.Value))                   '活動先顧客コード(オリジナルID:自社客 / 未取引客連番:未取引客)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTNAME, HttpUtility.HtmlDecode(Me.cstNameHidden.Value))                 '活動先顧客名
            Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, HttpUtility.HtmlDecode(Me.salesStaffCDHidden.Value))          '顧客担当セールススタッフコード
            Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD, HttpUtility.HtmlDecode(Me.fllwUpBoxStrCDHidden.Value))     'Follow-up Box店舗コード
            '2012/04/18 TCS 明瀬 HTMLエンコード対応 End

            '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
            If Not CDec(Me.fllwUpBoxHidden.Value) = -1 Then
                '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
                '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
                Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, HttpUtility.HtmlDecode(Me.fllwUpBoxHidden.Value))        'Follow-up Box内連番
                '2012/04/18 TCS 明瀬 HTMLエンコード対応 End
            End If

            '------------------------------------------tet module str
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                    "CSTKIND={0}, CUSTSEGMENT={1}, CUSTOMERCLASS={2}, CRCUSTID={3}, CRCUSTNAME={4}, SALESSTAFFCD={5}, FLLWUPBOX_STRCD={6}, FOLLOW_UP_BOX={7}",
                    HttpUtility.HtmlDecode(Me.cstKindHidden.Value), HttpUtility.HtmlDecode(Me.cstKindHidden.Value), HttpUtility.HtmlDecode(Me.cstClassHidden.Value),
                    HttpUtility.HtmlDecode(Me.crCstIdHidden.Value), HttpUtility.HtmlDecode(Me.cstNameHidden.Value), HttpUtility.HtmlDecode(Me.salesStaffCDHidden.Value),
                    HttpUtility.HtmlDecode(Me.fllwUpBoxStrCDHidden.Value), HttpUtility.HtmlDecode(Me.fllwUpBoxHidden.Value)))
            '------------------------------------------tet module end

            '顧客詳細画面へ遷移
            Me.RedirectNextScreen(DISPID_CUSTDETAIL)

        End If

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' 基盤から通知連絡があったときに行う処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/18 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Protected Sub PostBackButton_Click(sender As Object, e As System.EventArgs) Handles postBackButton.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[sender:{0}][e:{1}]", sender.ToString, e.ToString))

        Dim bizClass As New SC3040802BusinessLogic
        Dim staff As StaffContext = StaffContext.Current

        '通知の件数を取得する
        Dim noticeCountDt As SC3040802DataSet.SC3040802NoticeCountDataTable = bizClass.GetNoticeInfoCountDT(staff.Account, staff.DlrCD)

        '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
        '2014/05/10 TCS 武田 受注後フォロー機能開発 END

        '通知件数が１件以上なら通知データを表示
        noticeInfoPanel.Visible = True
        '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
        '2014/05/10 TCS 武田 受注後フォロー機能開発 END

        '通知件数をHiddenに保存
        '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
        Me.noticeCountHidden.Value = HttpUtility.HtmlEncode(noticeCountDt.Item(0).COUNT.ToString(CultureInfo.CurrentCulture))
        '2012/04/18 TCS 明瀬 HTMLエンコード対応 End

        '2014/05/10 TCS 武田 受注後フォロー機能開発 START DEL
        '2014/05/10 TCS 武田 受注後フォロー機能開発 END

        '画面を全体表示にする
        Me.openWindow()

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 画面を全体表示にする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub openWindow()

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        Dim script As New StringBuilder
        script.Append("<script type='text/javascript'>")
        script.Append("$('#noticeListFrame', parent.document).css('left', '700px');")
        script.Append("$('#noticeListFrame', parent.document).width('325');")
        '2012/04/18 TCS 明瀬 HTMLエンコード対応 Start
        script.Append("$('#openCloseHidden').val(HtmlEncodeSC3040802('open'));")
        '2012/04/18 TCS 明瀬 HTMLエンコード対応 End
        script.Append("</script>")

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "openScript", script.ToString())

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub
#End Region

End Class
