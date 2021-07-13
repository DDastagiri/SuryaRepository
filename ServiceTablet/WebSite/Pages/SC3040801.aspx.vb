'-------------------------------------------------------------------------
'SC3040801aspx.vb
'-------------------------------------------------------------------------
'機能：通知履歴
'補足：
'作成：2012/02/03 KN 河原 【servive_1】
'更新：2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加
'更新：2012/11/13 TMEJ 河原 クルクルリトライ対応
'更新：2014/04/08 TMEJ 小澤 BTS-370対応
'
'─────────────────────────────────────


Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Tool.Notify.BizLogic.SC3040801
Imports Toyota.eCRB.Tool.Notify.DataAccess

''' <summary>
''' SC3040801通知履歴プレゼンテーション層
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3040801
    Inherits BasePage
    Implements IDisposable

    Private dtSalesNoticeHistory As SC3040801DataSet.SalesNoticeHistoryDataTable
    Private dtServiceNoticeHistory As SC3040801DataSet.ServiceNoticeHistoryDataTable
    Private dtGetTransitionParameter As SC3040801DataSet.GetTransitionParameterDataTable

#Region " 定数 "

#Region "カウント"

    ''' <summary>
    ''' フラグ
    ''' </summary>
    Private Const ConstOne As String = "1"

    ''' <summary>
    ''' カウント
    ''' </summary>
    Private Const ConstCount As Integer = 0

    ''' <summary>
    ''' 次の6件
    ''' </summary>
    Private Const NextRows As Integer = 6

#End Region

#Region "画面遷移定数"

    ''' <summary>
    ''' 顧客詳細
    ''' </summary>
    Private Const CustomerDetails As String = "1"

    ''' <summary>
    ''' 査定結果
    ''' </summary>
    Private Const AssessmentResult As String = "2"

    ''' <summary>
    ''' 価格相談
    ''' </summary>
    Private Const PriceConsultation As String = "3"

    ''' <summary>
    ''' 顧客詳細
    ''' </summary>
    Private Const CustomerPageId As String = "SC3080201"

    ''' <summary>
    ''' カーチェックシート
    ''' </summary>
    Private Const AssessmentPageId As String = "SC3060101"

    ''' <summary>
    ''' 見積もり画面
    ''' </summary>
    Private Const PricePageId As String = "SC3070201"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyCustomId As String = "SearchKey.CRCUSTID"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyKind As String = "SearchKey.CSTKIND"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyCustomerClass As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeySalesStaffCode As String = "SearchKey.SALESSTAFFCD"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyVclId As String = "SearchKey.VCLID"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyFollowUpBox As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyFollowUpBoxSales As String = "SearchKey.FOLLOW_UP_BOX_SALES"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyFllwUpBoxStoreCode As String = "SearchKey.FLLWUPBOX_STRCD"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyRequestId As String = "SearchKey.REQUESTID"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyAssessmentNo As String = "SearchKey.ASSESSMENTNO"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyEstimateId As String = "EstimateId"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyNoticereqId As String = "NoticeReqId"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    Private Const SearchKeyDISPPAGE As String = "SearchKey.DISPPAGE"

#End Region

#Region "画面表示用定数"

    ''' <summary>
    ''' 既読
    ''' </summary>
    Private Const ReadList As String = "ReadList"

    ''' <summary>
    ''' 未読
    ''' </summary>
    Private Const UnReadList As String = "UnReadList"

    Private Const SUPPORT As String = "SupportStatusCheckBox_Green"
    Private Const UNSUPPORT As String = "SupportStatusCheckBox_Gray"

#End Region

#Region "文言ID"

    ''' <summary>
    ''' 文言ID（キャンセルできない）
    ''' </summary>
    Private Const WordIdDbErrCancel As Integer = 21

    ''' <summary>
    ''' 文言ID（キャンセルできない）
    ''' </summary>
    Private Const WordIdNoCancel As Integer = 35

    ''' <summary>
    ''' 文言ID（履歴無し）
    ''' </summary>
    Private Const WordIdNoList As Integer = 22

    ''' <summary>
    ''' 文言ID（キャンセルテキスト）
    ''' </summary>
    Private Const WordIdCancelText As Integer = 8

    ''' <summary>
    ''' 文言ID（キャンセルテキスト）
    ''' </summary>
    Private Const WordIdNextText As Integer = 9

    ''' <summary>
    ''' 文言ID（キャンセルテキスト）
    ''' </summary>
    Private Const WordIdLoadText As Integer = 10

    ''' <summary>
    ''' ページID
    ''' </summary>
    Private Const WordIdPageId As String = "SC3040801"

#End Region

#End Region

#Region "イベント"

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)
        If Not IsPostBack And Not IsCallback Then
            '初期表示
            ScriptManager.RegisterStartupScript(Me, _
                                                Me.GetType, _
                                                "PageLoad", _
                                                "setTimeout('LoadingScreen();', 500);", _
                                                True)
        End If

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__" & _
                    "END")
    End Sub

    ''' <summary>
    ''' 初期表示ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/08 TMEJ 小澤 BTS-370対応
    ''' </history>
    Protected Sub LoadButton_Click(sender As Object, e As System.EventArgs) _
        Handles LoadButton.Click
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim pageLoad As Boolean = True

        '初期表示
        NoticeHistory(pageLoad)

        'Load画面非表示
        '2014/04/08 TMEJ 小澤 BTS-370対応 START
        'ScriptManager.RegisterStartupScript(Me, _
        '                                    Me.GetType, _
        '                                    "LoadPanel", _
        '                                    "$('div#LoadPanel').hide();" & _
        '                                    True)
        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "LoadPanel", _
                                            "$('div#LoadPanel').hide();" & _
                                            "clearTimer();", _
                                            True)
        '2014/04/08 TMEJ 小澤 BTS-370対応 END

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__" & _
                    "END")
    End Sub

    ''' <summary>
    ''' リンククリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub LinkButton_Click(sender As Object, e As System.EventArgs) _
        Handles LinkButton.Click
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim staffInfo As StaffContext = StaffContext.Current    'スタッフ情報                
        Dim staffAuthority As Boolean = StaffOperationCode()    'スタッフ区分の取得

        Dim pageId As String 'ページ遷移先ID

        If staffAuthority Then 'セールス
            Logger.Info("TYPE=SalesSetSessionValues")
            'セッション情報を詰める
            pageId = SalesSetSessionValues()
            '画面遷移
            Me.RedirectNextScreen(pageId)
        Else 'サービス
            Logger.Info("TYPE=ServiceSetSessionValues")
            'セッション情報を詰める
            pageId = ServiceSetSessionValues()
            '画面遷移
            Me.RedirectNextScreen(pageId)
        End If

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__" & _
                    "END")
    End Sub

    ''' <summary>
    ''' キャンセルボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/13 TMEJ 河原 クルクルリトライ対応
    ''' 2014/04/08 TMEJ 小澤 BTS-370対応
    ''' </History>
    Protected Sub HideCancelButton_Click(sender As Object, e As System.EventArgs) _
        Handles HideCancelButton.Click
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim pageLoad As Boolean = True
        Dim updataSuccess As Boolean = False

        Try
            'リストのhiddenFiledから値の取得
            Dim noticeId As String = CStr(CancelField.Value)

            'NULLの制御
            If String.IsNullOrEmpty(noticeId) Then
                Logger.Info("CancelField.Value=Empty")
                Logger.Info("Cancel=No")
                'キャンセルできませんの表示
                Me.ShowMessageBox(WordIdDbErrCancel)

            Else
                Logger.Info("CancelField.Value=" & noticeId)
                Dim lastStatus As String

                '最終ステータスの確認
                Using BisLogic As New SC3040801BusinessLogic
                    lastStatus = BisLogic.GetLastStatus(noticeId)
                End Using

                'キャンセル処理
                If ConstOne.Equals(lastStatus) Then '依頼のときのみキャンセル実行
                    Logger.Info("lastStatus=" & lastStatus)
                    Logger.Info("Cancel=Ok")
                    Using business As New SC3040801BusinessLogic
                        'キャンセルのパラメータをセット
                        business.SetCancelParameter(CLng(noticeId))
                    End Using

                Else
                    Logger.Info("lastStatus=" & lastStatus)
                    Logger.Info("Cancel=No")
                    'キャンセルできませんの表示
                    Me.ShowMessageBox(WordIdNoCancel)

                End If

            End If

            NoticeHistory(pageLoad) '画面再表示

            '2014/04/08 TMEJ 小澤 BTS-370対応 START
            '2012/11/13 TMEJ 河原 クルクルリトライ対応 START
            'ScriptManager.RegisterStartupScript(Me, _
            '                                    Me.GetType, _
            '                                    "LoadPanel", _
            '                                    "$('div#LoadPanel').hide();", _
            '                                    True) 'ロード中画面の削除
            'ScriptManager.RegisterStartupScript(Me, _
            '                                    Me.GetType, _
            '                                    "LoadPanel", _
            '                                    "$('div#LoadPanel').hide();" _
            '                                    & "window.parent.commonClearTimer();", _
            '                                    True) 'ロード中画面の削除
            ScriptManager.RegisterStartupScript(Me, _
                                                Me.GetType, _
                                                "LoadPanel", _
                                                "$('div#LoadPanel').hide();" & _
                                                "clearTimer();", _
                                                True) 'ロード中画面の削除
            '2012/11/13 TMEJ 河原 クルクルリトライ対応 END
            '2014/04/08 TMEJ 小澤 BTS-370対応 END

        Catch ex As OracleExceptionEx
            Logger.Error(ex.ToString, ex)
            'キャンセルできませんの表示
            Me.ShowMessageBox(WordIdDbErrCancel)

            '2014/04/08 TMEJ 小澤 BTS-370対応 START
            '2012/11/13 TMEJ 河原 クルクルリトライ対応 START
            'ScriptManager.RegisterStartupScript(Me, _
            '                                    Me.GetType, _
            '                                    "LoadPanel", _
            '                                    "$('div#LoadPanel').hide();", _
            '                                    True) 'ロード中画面の削除
            'ScriptManager.RegisterStartupScript(Me, _
            '                                    Me.GetType, _
            '                                    "LoadPanel", _
            '                                    "$('div#LoadPanel').hide();" _
            '                                    & "window.parent.commonClearTimer();", _
            '                                    True) 'ロード中画面の削除
            ScriptManager.RegisterStartupScript(Me, _
                                                Me.GetType, _
                                                "LoadPanel", _
                                                "$('div#LoadPanel').hide();" & _
                                                "clearTimer();", _
                                                True) 'ロード中画面の削除
            '2012/11/13 TMEJ 河原 クルクルリトライ対応 END
            '2014/04/08 TMEJ 小澤 BTS-370対応 END

        Finally
            Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "__" & _
                        "END")

        End Try

    End Sub

    ''' <summary>
    ''' 次の6件ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/08 TMEJ 小澤 BTS-370対応
    ''' </history>
    Protected Sub HideNextButton_Click(sender As Object, e As System.EventArgs) _
        Handles HideNextButton.Click
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim pageLoad As Boolean = False

        '再表示
        NoticeHistory(pageLoad)

        'クライアント側のスクリプト作成
        Dim javaScriptWord As StringBuilder = New StringBuilder
        javaScriptWord.Append("$('div.NextLoad').hide();")
        javaScriptWord.Append("$('div.DisabledDiv').hide();")

        '2014/04/08 TMEJ 小澤 BTS-370対応 START
        javaScriptWord.Append("clearTimer();")
        '2014/04/08 TMEJ 小澤 BTS-370対応 END

        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "Next", _
                                            javaScriptWord.ToString, _
                                            True)

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
            "__" & _
            "END")

    End Sub

    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
    ''' <summary>
    ''' 明細チェックボックスボタン（仮想ボタン）
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/08 TMEJ 小澤 BTS-370対応
    ''' </history>
    Protected Sub DetailCheckBoxButton_Click(sender As Object, e As System.EventArgs) _
        Handles DetailCheckBoxButton.Click
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim ServiceReception As Control = RepeaterNoticeInfo.Items(ListIndex.Value)
        Dim javaScriptWord As StringBuilder = New StringBuilder

        Using bisLogic As New SC3040801BusinessLogic
            '元がチェックなしの場合
            If "0".Equals(DirectCast(ServiceReception.FindControl("SupportStatusList"), HiddenField).Value) Then
                'SupportStatus更新
                bisLogic.UpdateSupportStatus(SupportStatusNoticeId.Value, _
                                            "1")

                Logger.Info("SupportStatusList=0")
                DirectCast(ServiceReception.FindControl("SupportStatusCheckBox"), HtmlContainerControl).Attributes("class") = SUPPORT
                DirectCast(ServiceReception.FindControl("SupportStatusList"), HiddenField).Value = "1"

            Else
                '元がチェックありの場合
                Logger.Info("SupportStatusList=1")
                'SupportStatus更新
                bisLogic.UpdateSupportStatus(SupportStatusNoticeId.Value, _
                                            "0")
                DirectCast(ServiceReception.FindControl("SupportStatusCheckBox"), HtmlContainerControl).Attributes("class") = UNSUPPORT
                DirectCast(ServiceReception.FindControl("SupportStatusList"), HiddenField).Value = "0"

            End If

        End Using

        '再表示
        'NoticeHistory(False, 0)

        javaScriptWord.Append("$('div#LoadPanel').hide();")

        '2014/04/08 TMEJ 小澤 BTS-370対応 START
        javaScriptWord.Append("clearTimer();")
        '2014/04/08 TMEJ 小澤 BTS-370対応 END

        ScriptManager.RegisterStartupScript(Me, _
                                            Me.GetType, _
                                            "SuportStatus", _
                                            javaScriptWord.ToString, _
                                            True)

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
            "__" & _
            "END")

    End Sub
    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End

#End Region

#Region "メソッド"

    ''' <summary>
    ''' 履歴表示
    ''' </summary>
    ''' <param name="pageLoad">初期表示</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/04/08 TMEJ 小澤 BTS-370対応
    ''' </history>
    Protected Sub NoticeHistory(ByVal pageLoad As Boolean, Optional ByVal addRows As Integer = NextRows)
        'Protected Sub NoticeHistory(ByVal pageLoad As Boolean)
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_pageLoad=" & _
                    CStr(pageLoad))

        Try
            Dim beginRowIndex As Integer

            If pageLoad Then '初期表示
                '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
                'beginRowIndex = NextRows
                beginRowIndex = addRows
                '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End
                Logger.Info("BigenDisplay")
                Logger.Info("DisplayRowIndex=" & CStr(beginRowIndex))

            Else             '次の6件の表示               
                '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
                'beginRowIndex = RepeaterNoticeInfo.Items.Count + nextRows '現時点の行に6行追加
                beginRowIndex = RepeaterNoticeInfo.Items.Count + addRows '現時点の行に6行追加
                '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End
                Logger.Info("NextButtonDisplay")
                Logger.Info("DisplayRowIndex=" & CStr(beginRowIndex))

            End If

            Dim staffInfo As StaffContext = StaffContext.Current    'スタッフ情報
            Dim staffAuthority As Boolean = StaffOperationCode()    'スタッフ区分の取得
            Dim nextRowButton As Boolean = False                    '次の6件表示区分
            Dim javaScriptWord As StringBuilder = New StringBuilder

            'セールス
            If staffAuthority Then
                Logger.Info("DISPLAYTYPE=Sales")

                'セッションキーの存在チェック
                Dim sessionKeyValue As Boolean = Me.ContainsKey(ScreenPos.Current, SearchKeyFollowUpBoxSales)
                Dim followBox As String = "-1" 'followBox連番

                'セッションキーのチェック
                If sessionKeyValue Then 'キーがあり                    
                    followBox = Me.GetValue(ScreenPos.Current, SearchKeyFollowUpBoxSales, False)
                    Logger.Info(SearchKeyFollowUpBoxSales & "=" & followBox)

                Else 'キーなし                    
                    'セッションキーの存在チェック
                    Dim sessionKeySearchKeyFollowUpBox As Boolean = Me.ContainsKey(ScreenPos.Current, SearchKeyFollowUpBox)
                    If sessionKeySearchKeyFollowUpBox Then 'キーがあり
                        followBox = Me.GetValue(ScreenPos.Current, SearchKeyFollowUpBox, False)
                        Logger.Info(SearchKeyFollowUpBoxSales & "=" & followBox)

                    End If

                End If

                '通知履歴取得
                Using bisLogic As New SC3040801BusinessLogic
                    dtSalesNoticeHistory = bisLogic.ReadSalesNotification(staffInfo.Account, _
                                                                          beginRowIndex, _
                                                                          followBox, _
                                                                          nextRowButton)

                End Using

                If ConstCount = dtSalesNoticeHistory.Count Then '履歴なし
                    Logger.Info("SalesNoticeHistory=NODATE")
                    javaScriptWord.Append("$('div#DataPanel').hide();")
                    javaScriptWord.Append("$('div#NoDataPanel').show();") '履歴なしの表示

                Else '履歴有り
                    Logger.Info("SalesNoticeHistoryDisplay_COUNT=" & CStr(dtSalesNoticeHistory.Count))
                    'リピーターへバインド
                    RepeaterNoticeInfo.DataSource = dtSalesNoticeHistory
                    RepeaterNoticeInfo.DataBind()

                    'コントロール制御
                    For i As Integer = 0 To RepeaterNoticeInfo.Items.Count - 1
                        Dim SalesReception As Control = RepeaterNoticeInfo.Items(i)

                        'アイコンの表示
                        DirectCast(SalesReception.FindControl("Icons"),  _
                                   HtmlContainerControl).Attributes("class") _
                                                            = dtSalesNoticeHistory(i).ICONIMAGE

                        Logger.Info("ROW" & CStr(i) & "_ICONIMAGE=" & dtSalesNoticeHistory(i).ICONIMAGE)

                        '既読・未読
                        If ConstOne.Equals(dtSalesNoticeHistory(i).READFLG) Then '既読
                            Logger.Info("ROW" & CStr(i) & "_DataList=" & ReadList)
                            DirectCast(SalesReception.FindControl("DataList"),  _
                                       HtmlContainerControl).Attributes("class") = ReadList

                        Else                                                     '未読
                            Logger.Info("ROW" & CStr(i) & "_DataList=" & UnReadList)
                            DirectCast(SalesReception.FindControl("DataList"),  _
                                       HtmlContainerControl).Attributes("class") = UnReadList

                        End If

                        'キャンセルボタン
                        If Not dtSalesNoticeHistory(i).CANCELFLAG Then
                            Logger.Info("ROW" & CStr(i) & "_CancelButton.Visible=False")
                            DirectCast(SalesReception.FindControl("CancelButton"),  _
                                       HtmlControl).Visible = False 'キャンセルボタンの非表示   

                        End If

                    Next

                End If

            Else 'サービス
                Logger.Info("DISPLAYTYPE=Service")

                '通知履歴取得
                Using bisLogic As New SC3040801BusinessLogic
                    dtServiceNoticeHistory = bisLogic.ReadServiceNotification(staffInfo.Account, _
                                                                              beginRowIndex, _
                                                                              nextRowButton)

                End Using

                If ConstCount = dtServiceNoticeHistory.Count Then '履歴なし
                    Logger.Info("ServiceNoticeHistory=NODATE")

                    javaScriptWord.Append("$('div#DataPanel').hide();")
                    javaScriptWord.Append("$('div#NoDataPanel').show();") '履歴なしの表示

                Else '履歴有り
                    Logger.Info("ServiceNoticeHistoryDisplay_COUNT=" & CStr(dtServiceNoticeHistory.Count))

                    'リピーターへバインド
                    RepeaterNoticeInfo.DataSource = dtServiceNoticeHistory
                    RepeaterNoticeInfo.DataBind()

                    'コントロール制御
                    For j As Integer = 0 To RepeaterNoticeInfo.Items.Count - 1
                        Dim ServiceReception As Control = RepeaterNoticeInfo.Items(j)

                        DirectCast(ServiceReception.FindControl("Icons"),  _
                                                HtmlContainerControl).Visible = False        'アイコンの非表示
                        DirectCast(ServiceReception.FindControl("CancelButton"),  _
                                                HtmlContainerControl).Visible = False        'キャンセルボタンの非表示                        
                        ' 既読か未読          
                        If ConstOne.Equals(dtServiceNoticeHistory(j).READFLG) Then  '既読
                            Logger.Info("ROW" & CStr(j) & "_DataList=" & ReadList)
                            DirectCast(ServiceReception.FindControl("DataList"),  _
                                       HtmlContainerControl).Attributes("class") = ReadList

                        Else                                                        '未読
                            Logger.Info("ROW" & CStr(j) & "_DataList=" & UnReadList)
                            DirectCast(ServiceReception.FindControl("DataList"),  _
                                       HtmlContainerControl).Attributes("class") = UnReadList

                        End If

                        '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
                        '権限がCT及びFMの場合に表示する
                        If StaffContext.Current.OpeCD = Operation.CT Or StaffContext.Current.OpeCD = Operation.FM Then
                            Logger.Info("ROW" & CStr(j) & "_OpeCD=" & Operation.CT)
                            Logger.Info("ROW" & CStr(j) & "_OpeCD=" & Operation.FM)
                            DirectCast(ServiceReception.FindControl("SupportStatusCheckBox"),  _
                                    HtmlContainerControl).Visible = True 'チェックボックス表示

                            If "0".Equals(dtServiceNoticeHistory(j).SUPPORTSTATUS) Then
                                Logger.Info("ROW" & CStr(j) & "_DataList=" & UNSUPPORT)
                                DirectCast(ServiceReception.FindControl("SupportStatusCheckBox"),  _
                                    HtmlContainerControl).Attributes("class") = UNSUPPORT

                            Else
                                Logger.Info("ROW" & CStr(j) & "_DataList=" & SUPPORT)
                                DirectCast(ServiceReception.FindControl("SupportStatusCheckBox"),  _
                                    HtmlContainerControl).Attributes("class") = SUPPORT

                            End If

                        End If

                        '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End

                    Next

                End If

            End If

            '次へボタンの表示
            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
            'If nextRowButton Then
            '    '表示
            '    javaScriptWord.Append("$('div.NextButton').show();")
            'Else
            '    '非表示
            '    javaScriptWord.Append("$('div.NextButton').hide();")
            'End If
            If nextRowButton Then
                '表示
                NextButton.Visible = True

            Else
                '非表示
                NextButton.Visible = False

            End If
            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End

            '初期表示のみ既読に更新
            If beginRowIndex = NextRows Then
                '更新処理
                Using bisLogic As New SC3040801BusinessLogic
                    bisLogic.UpdateConfirmed()
                End Using
                'ヘッダー件数の再取得
                javaScriptWord.Append("updateNoticeIcon();")

            End If

            'JSを呼ぶ
            ScriptManager.RegisterStartupScript(Me, _
                                    Me.GetType, _
                                    "DataPanel", _
                                    javaScriptWord.ToString, _
                                    True)

            'エラー制御
        Catch ex As OracleExceptionEx
            Logger.Error(ex.ToString, ex)

            '2014/04/08 TMEJ 小澤 BTS-370対応 START
            'ScriptManager.RegisterStartupScript(Me, _
            '                                    Me.GetType, _
            '                                    "DataPanel", _
            '                                    "$('div#DataPanel').hide();", _
            '                                    True)
            ScriptManager.RegisterStartupScript(Me, _
                                                Me.GetType, _
                                                "DataPanel", _
                                                "$('div#DataPanel').hide()" & _
                                                "clearTimer();", _
                                                True)
            '2014/04/08 TMEJ 小澤 BTS-370対応 END

            '通知履歴を取得できませんの表示
            Me.ShowMessageBox(WordIdNoList)

        Finally
            Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "__" & _
                        "END")

        End Try
    End Sub

    ''' <summary>
    ''' セールスセッションセット
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function SalesSetSessionValues() As String
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim pageId As String = ""

        Dim noticeRequestId As Long = CLng(LinkValueField.Value) '通知依頼ID
        Dim linkId As String = CStr(LinkIdField.Value)           'リンクID

        Using BisLogic As New SC3040801BusinessLogic
            'セッション情報の取得
            dtGetTransitionParameter = BisLogic.GetTransitionParameter(noticeRequestId)
        End Using

        Dim reqestId As String = Nothing            'REQESTID
        Dim customId As String = Nothing            '顧客ID
        Dim customKind As String = Nothing          '顧客種別
        Dim customerClass As String = Nothing       '顧客クラス
        Dim salesStaffCode As String = Nothing      '対応スタッフコード
        Dim vclId As String = Nothing               '車両シーケンス
        Dim followUpBox As String = Nothing         '連番
        Dim fllWupBoxStoreCode As String = Nothing  '店舗コード

        'セッションに詰めるため'NULLチェック
        'REQESTID
        If Not dtGetTransitionParameter(0).IsREQCLASSIDNull Then
            reqestId = CStr(dtGetTransitionParameter(0).REQCLASSID)
            Logger.Info("reqestId=" & reqestId)
        End If
        '顧客ID
        If Not dtGetTransitionParameter(0).IsCRCUSTIDNull Then
            customId = dtGetTransitionParameter(0).CRCUSTID.Trim
            Logger.Info("SearchKeyCustomId=" & customId)
        End If
        '顧客種別
        If Not dtGetTransitionParameter(0).IsCSTKINDNull Then
            customKind = dtGetTransitionParameter(0).CSTKIND.Trim
            Logger.Info("SearchKeyKind=" & customKind)
        End If
        '顧客クラス
        If Not dtGetTransitionParameter(0).IsCUSTOMERCLASSNull Then
            customerClass = dtGetTransitionParameter(0).CUSTOMERCLASS.Trim
            Logger.Info("SearchKeyCustomerClass=" & customerClass)
        End If
        '対応スタッフコード
        If Not dtGetTransitionParameter(0).IsSALESSTAFFCDNull Then
            salesStaffCode = dtGetTransitionParameter(0).SALESSTAFFCD.Trim
            Logger.Info("SearchKeySalesStaffCode=" & salesStaffCode)
        End If
        '車両シーケンス
        If Not dtGetTransitionParameter(0).IsVCLIDNull Then
            vclId = dtGetTransitionParameter(0).VCLID.Trim
            Logger.Info("SearchKeyVclId=" & vclId)
        End If
        'セッションに詰める
        Me.SetValue(ScreenPos.Next, SearchKeyCustomId, customId)                '顧客ID
        Me.SetValue(ScreenPos.Next, SearchKeyKind, customKind)                  '顧客種別
        Me.SetValue(ScreenPos.Next, SearchKeyCustomerClass, customerClass)      '顧客クラス
        Me.SetValue(ScreenPos.Next, SearchKeySalesStaffCode, salesStaffCode)    '対応スタッフコード
        Me.SetValue(ScreenPos.Next, SearchKeyVclId, vclId)                      '車両シーケンス
        'シーケンス(DBNULLの場合はセッションに詰めない)
        If Not dtGetTransitionParameter(0).IsFLLWUPBOXNull Then
            followUpBox = CStr(dtGetTransitionParameter(0).FLLWUPBOX)
            Logger.Info("SearchKeyFollowUpBox=" & followUpBox)
            Me.SetValue(ScreenPos.Next, SearchKeyFollowUpBox, followUpBox)
        End If
        '店舗コード(DBNULLの場合はセッションに詰めない)
        If Not dtGetTransitionParameter(0).IsFLLWUPBOXSTRCDNull Then
            fllWupBoxStoreCode = dtGetTransitionParameter(0).FLLWUPBOXSTRCD.Trim
            Logger.Info("SearchKeyFllwUpBoxStoreCode=" & fllWupBoxStoreCode)
            Me.SetValue(ScreenPos.Next, SearchKeyFllwUpBoxStoreCode, fllWupBoxStoreCode)
        End If

        Select Case linkId '画面遷移先ごと残りのセッションを詰める
            Case CustomerDetails    '顧客詳細
                Logger.Info("SearchKeyDISPPAGE=" & CustomerDetails)
                Logger.Info("pageId=" & CustomerPageId)
                Me.SetValue(ScreenPos.Next, SearchKeyDISPPAGE, CustomerDetails) 'DISPPAGE
                pageId = CustomerPageId 'ページID(SC3080201)
            Case AssessmentResult   'カーチェックシート(査定)
                Logger.Info("SearchKeyRequestId=" & CStr(noticeRequestId))
                Logger.Info("SearchKeyAssessmentNo=" & reqestId)
                Logger.Info("pageId=" & AssessmentPageId)
                Me.SetValue(ScreenPos.Next, SearchKeyRequestId, CStr(noticeRequestId))  '依頼ID
                Me.SetValue(ScreenPos.Next, SearchKeyAssessmentNo, reqestId)            'REQESTID
                pageId = AssessmentPageId 'ページID(SC3060101)
            Case PriceConsultation '見積画面(価格相談)
                Logger.Info("SearchKeyNoticereqId=" & reqestId)
                Logger.Info("SearchKeyEstimateId=" & CStr(noticeRequestId))
                Logger.Info("pageId=" & PricePageId)
                Me.SetValue(ScreenPos.Next, SearchKeyEstimateId, CLng(reqestId))    'REQESTID
                Me.SetValue(ScreenPos.Next, SearchKeyNoticereqId, noticeRequestId)  '依頼ID
                pageId = PricePageId    'ページID(SC3070201)
        End Select

        Logger.Info("return=Pageid=" & _
                    pageId & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "__END")

        Return pageId
    End Function

    ''' <summary>
    ''' サービスセッションセット
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ServiceSetSessionValues() As String
        Logger.Info("START__" & _
                     System.Reflection.MethodBase.GetCurrentMethod.Name)

        '遷移先ページ
        Dim pageId As String = CStr(PageIdField.Value)
        Logger.Info("pageId=" & pageId)
        'セッショングループ
        Dim sessionValueGroup As New List(Of String)(LinkValueField.Value.Split(vbTab)) 'タブで分ける
        Logger.Info("LinkValueField.Value=" & CStr(LinkValueField.Value))
        'リンクID
        Dim linkId As String = CStr(LinkIdField.Value)
        'リンクNoの取得
        Dim linkNo As Integer = CInt(Replace(linkId, pageId, ""))
        Logger.Info("linkNo=" & CStr(linkNo))
        'セッション情報
        Dim sessionValueList As New List(Of String)(sessionValueGroup(linkNo).Split(",")) 'セッション情報をカンマで分ける

        For i As Integer = 0 To sessionValueList.Count - 1 Step 3
            'LISTが無いときの制御
            If Not String.IsNullOrEmpty(sessionValueList(i)) Then
                'セッションに詰める
                Me.SetValue(ScreenPos.Next, sessionValueList(i), sessionValueList(i + 2))
                Logger.Info("KeyName=" & sessionValueList(i) & "_KeyValue=" & sessionValueList(i + 2))
            End If
        Next

        Logger.Info("return=Pageid=" & _
                    pageId & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "__END")
        Return pageId
    End Function

    ''' <summary>
    ''' スタッフオペコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Function StaffOperationCode() As Boolean
        Logger.Info("START__" & _
                     System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim staffInfo As StaffContext = StaffContext.Current    'スタッフ情報
        Dim staffAuthority As Boolean = False                   'スタッフ区分
        Logger.Info("StaffOperationCodeNo=" & CStr(staffInfo.OpeCD))

        '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
        If (New CommonMasterPage)._CheckSalesService() Then
            staffAuthority = False 'サービス
            Logger.Info("StaffOperationCode=Service")
        Else
            staffAuthority = True 'セールス
            Logger.Info("StaffOperationCode=Sales")
        End If
        'Select Case staffInfo.OpeCD
        '    Case Operation.SSM,
        '         Operation.SSF
        '        staffAuthority = True 'セールス
        '        Logger.Info("StaffOperationCode=Sales")
        '    Case Operation.SA,
        '         Operation.SM,
        '         Operation.TEC,
        '         Operation.SVR,
        '         Operation.PS,
        '         Operation.CT,
        '        Operation.FM
        '        staffAuthority = False 'サービス
        '        Logger.Info("StaffOperationCode=Service")
        'End Select
        '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End

        Logger.Info("return=staffAuthority=" & _
                    CStr(staffAuthority) & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__" & _
                    "END")
        Return staffAuthority

    End Function

#End Region

    ''' <summary>
    ''' IDisosable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then

            dtSalesNoticeHistory.Dispose()
            dtServiceNoticeHistory.Dispose()
            dtGetTransitionParameter.Dispose()

            dtSalesNoticeHistory = Nothing
            dtServiceNoticeHistory = Nothing
            dtGetTransitionParameter = Nothing

        End If

    End Sub

End Class