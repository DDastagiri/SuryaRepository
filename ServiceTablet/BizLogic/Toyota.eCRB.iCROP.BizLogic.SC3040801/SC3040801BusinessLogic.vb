'-------------------------------------------------------------------------
'SC3040801BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：通知履歴
'補足：
'作成：2012/02/3 KN 河原 【servive_1】
'更新：2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加
'更新：2014/04/14 TMEJ 丁 BTS-383 「通知履歴のレスポンスが遅い」対応
'
'─────────────────────────────────────
'

Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

''' <summary>
''' SC304081(通知履歴)
''' 通知履歴で使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3040801BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    Private dtSalesNoticeHistory As SC3040801DataSet.SalesNoticeHistoryDataTable
    Private dtGetSalesNotice As SC3040801DataSet.GetSalesNoticeDataTable
    Private dtGetPhotoPath As SC3040801DataSet.GetPhotoPathDataTable
    Private dtGetServiceNotice As SC3040801DataSet.GetServiceNoticeDataTable
    Private dtServiceNoticeHistory As SC3040801DataSet.ServiceNoticeHistoryDataTable
    Private dtGetLastStatus As SC3040801DataSet.GetLastStatusDataTable
    Private dtGetTransitionParameter As SC3040801DataSet.GetTransitionParameterDataTable
    Private dtGetCancelParameter As SC3040801DataSet.GetCancelParameterDataTable

    Private dtNoticeInfo As IC3040801DataSet.IC3040801NoticeInfoDataTable

    Private NoticeData As XmlNoticeData         'API用親クラス
    Private Account As XmlAccount               'API用子クラス
    Private RequestNotice As XmlRequestNotice   'API用子クラス

#Region "メンバー変数"

    Private Property timeNow As DateTime '現在の時間
    Private Property oneMinutesBefore As DateTime
    Private Property oneHourBefore As DateTime
    Private Property today As DateTime
    Private Property dayNow As Date      'フォーマット(yyyy/MM/dd)用
    Private Property oneDay As Date      '1日前
    Private Property twoDay As Date      '2日前
    Private Property staffConInfo As StaffContext
    '時間文言セット
    Private timeWord()() As String = {New String() {String.Empty, String.Empty},
                                      New String() {WebWordUtility.GetWord(WordIdPageId, WordIdNow), String.Empty},
                                      New String() {WebWordUtility.GetWord(WordIdPageId, WordIdMinutes), String.Empty},
                                      New String() {WebWordUtility.GetWord(WordIdPageId, WordIdAnHour), String.Empty},
                                      New String() {WebWordUtility.GetWord(WordIdPageId, WordIdAbout), WebWordUtility.GetWord(WordIdPageId, WordIdHour)},
                                      New String() {WebWordUtility.GetWord(WordIdPageId, WordIdYesterday), String.Empty}}

#End Region

    ''' <summary>
    ''' Enum時間計算
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum TimeStatus As Integer

        ''' <summary>たった今</summary>
        Now = 1

        ''' <summary>分前</summary>
        Minute = 2

        ''' <summary>1時間</summary>
        OneHour = 3

        ''' <summary>1時間以外の時間</summary>
        OtherHours = 4

        ''' <summary>昨日</summary>
        Yesterday = 5

    End Enum

    Public Sub New()
        dtSalesNoticeHistory = New SC3040801DataSet.SalesNoticeHistoryDataTable
        dtGetSalesNotice = New SC3040801DataSet.GetSalesNoticeDataTable
        dtGetPhotoPath = New SC3040801DataSet.GetPhotoPathDataTable
        dtGetServiceNotice = New SC3040801DataSet.GetServiceNoticeDataTable
        dtServiceNoticeHistory = New SC3040801DataSet.ServiceNoticeHistoryDataTable
        dtGetLastStatus = New SC3040801DataSet.GetLastStatusDataTable
        dtGetTransitionParameter = New SC3040801DataSet.GetTransitionParameterDataTable
        dtGetCancelParameter = New SC3040801DataSet.GetCancelParameterDataTable

        dtNoticeInfo = New IC3040801DataSet.IC3040801NoticeInfoDataTable

        NoticeData = New XmlNoticeData       
        RequestNotice = New XmlRequestNotice

        staffConInfo = StaffContext.Current

        timeNow = DateTimeFunc.Now(staffConInfo.DlrCD) '現在の時間
        timeNow = Date.Parse(Format(timeNow, DateFormatYYYYMMDDHHMM), _
                                    CultureInfo.CurrentCulture)     'フォーマット(yyyy/MM/dd hh:mm)
        '1分前
        oneMinutesBefore = timeNow.AddMinutes(-CountOne)

        '１時間前
        oneHourBefore = timeNow.AddHours(-CountOne)

        dayNow = Date.Parse(Format(timeNow, DateFormatYYYMMDD), _
                                   CultureInfo.CurrentCulture)      'フォーマット(yyyy/MM/dd) 

        '今日の0時0分0秒
        today = Date.Parse(Format(dayNow, DateFormatYYYYMMDDHHMM), _
                                    CultureInfo.CurrentCulture)     'フォーマット(yyyy/MM/dd hh:mm)

        oneDay = Date.Parse(Format(dayNow.AddDays(-CountOne), DateFormatYYYMMDD), _
                                   CultureInfo.CurrentCulture)      '1日前
        twoDay = Date.Parse(Format(dayNow.AddDays(-TwoDays), DateFormatYYYMMDD), _
                                   CultureInfo.CurrentCulture)      '2日前

    End Sub

#Region "定数"

#Region "文言ID定数 "

    ''' <summary>
    ''' 文言ID（査定結果）
    ''' </summary>
    Private Const WordIdAudit As Integer = 18

    ''' <summary>
    ''' 文言ID（価格相談）
    ''' </summary>
    Private Const WordIdPrice As Integer = 19

    ''' <summary>
    ''' 文言ID（たった今）
    ''' </summary>
    Private Const WordIdNow As Integer = 2

    ''' <summary>
    ''' 文言ID（分前）
    ''' </summary>
    Private Const WordIdMinutes As Integer = 3

    ''' <summary>
    ''' 文言ID（1時間前）
    ''' </summary>
    Private Const WordIdAnHour As Integer = 36

    ''' <summary>
    ''' 文言ID（約）
    ''' </summary>
    Private Const WordIdAbout As Integer = 4

    ''' <summary>
    ''' 文言ID（時間前）
    ''' </summary>
    Private Const WordIdHour As Integer = 23

    ''' <summary>
    ''' 文言ID（昨日）
    ''' </summary>
    Private Const WordIdYesterday As Integer = 5

    ''' <summary>
    ''' ページID
    ''' </summary>
    Private Const WordIdPageId As String = "SC3040801"

#End Region

#Region "Category定数"

    ''' <summary>
    ''' 小カテゴリー0
    ''' </summary>
    Private Const CategoryZero As String = "0"

    ''' <summary>
    ''' カテゴリー1
    ''' </summary>
    Private Const CategoryOne As String = "1"

    ''' <summary>
    ''' カテゴリー2
    ''' </summary>
    Private Const CategoryTwo As String = "2"

    ''' <summary>
    ''' カテゴリー2
    ''' </summary>
    Private Const CategoryThree As String = "3"

#End Region

#Region "メッセージ置換定数"

    ''' <summary>
    ''' メッセージ置換
    ''' </summary>
    Private Const PermutationCust As String = "%CUST%"

    ''' <summary>
    ''' メッセージ置換
    ''' </summary>
    Private Const PermutationFromStaffName As String = "%FROMSTAFF%"

    ''' <summary>
    ''' メッセージ置換
    ''' </summary>
    Private Const PermutationToStaffName As String = "%TOSTAFF%"

    ''' <summary>
    ''' メッセージ置換
    ''' </summary>
    Private Const PermutationReq As String = "%REQ%"

    ''' <summary>
    ''' メッセージ置換(顧客)
    ''' </summary>
    Private Const PermutationCustNothing As String = "<a href="""" onClick=""return SalesLinkClick(event,1)"">%CUST%</a>"

    ''' <summary>
    ''' メッセージ置換(査定)
    ''' </summary>
    Private Const PermutationAssessment As String = "<a href="""" onclick=""return SalesLinkClick(event,2)"">%REQ%</a>"

    ''' <summary>
    ''' メッセージ置換(価格相談)
    ''' </summary>
    Private Const PermutationConsultation As String = "<a href="""" onclick=""return SalesLinkClick(event,3)"">%REQ%</a>"

#End Region

#Region "時間表示定数"

    ''' <summary>
    ''' FORMAT
    ''' </summary>
    Private Const DateFormatYYYMMDD As String = "yyyy/MM/dd"

    ''' <summary>
    ''' FORMAT
    ''' </summary>
    Private Const DateFormatMMDD As String = "MM/dd"

    ''' <summary>
    ''' FORMAT
    ''' </summary>
    Private Const DateFormatYYYYMMDDHHMM As String = "yyyy/MM/dd HH:mm"

    ''' <summary>
    ''' 1分
    ''' </summary>
    Private Const CountOne As Integer = 1

    ''' <summary>
    ''' 60分
    ''' </summary>
    Private Const TimeHour As Integer = 60

    ''' <summary>
    ''' 30分
    ''' </summary>
    Private Const TimeHelfHour As Integer = 30

    ''' <summary>
    ''' 24時間
    ''' </summary>
    Private Const DayHour As Integer = 24

    ''' <summary>
    ''' 2日
    ''' </summary>
    Private Const TwoDays As Integer = 2

#End Region

#Region "写真定数"

    ''' <summary>
    ''' 写真
    ''' </summary>
    Private Const NoPhoto As String = "NOPHOTO"

    ''' <summary>
    ''' 写真のURL(写真なし用)
    ''' </summary>
    Private Const NoPhotoUrl As String = "~/Styles/Images/SC3040801/silhouette_person.png"

    ''' <summary>
    ''' 写真URLkey2
    ''' </summary>
    Private Const FileUrl As String = "URI_STAFFPHOTO"

#End Region

#Region "アイコン定数"

    ''' <summary>
    ''' アイコン査定
    ''' </summary>
    Private Const IconAss As String = "IconsImageAssessment"

    ''' <summary>
    ''' アイコン価格相談
    ''' </summary>
    Private Const IconCon As String = "IconsImageConsultation"

    ''' <summary>
    ''' アイコンヘルプ
    ''' </summary>
    Private Const IconHelp As String = "IconsImageHelp"

#End Region

#Region "ステータス定数"

    ''' <summary>
    ''' 査定
    ''' </summary>
    Private Const Assessment As String = "01"

    ''' <summary>
    ''' 価格相談
    ''' </summary>
    Private Const Consultation As String = "02"

    ''' <summary>
    ''' ヘルプ
    ''' </summary>
    Private Const Help As String = "03"

    ''' <summary>
    ''' 依頼
    ''' </summary>
    Private Const StatusRequest As String = "1"

    ''' <summary>
    ''' キャンセル
    ''' </summary>
    Private Const StatusCancel As String = "2"

    ''' <summary>
    ''' 受信
    ''' </summary>
    Private Const StatusGetReceive As String = "3"

    ''' <summary>
    ''' 受付
    ''' </summary>
    Private Const StatusAccept As String = "4"

#End Region

#Region "その他定数"

    ''' <summary>
    ''' カウント
    ''' </summary>
    Private Const InitCount As Integer = 0

    ''' <summary>
    ''' 表示日付設定key
    ''' </summary>
    Private Const DisplyDays As String = "NOTICE_DISP_DAYS"

#End Region

#End Region

#Region "通知履歴解析処理"

    ''' <summary>
    ''' セールス履歴の取得
    ''' </summary>
    ''' <param name="userAccount">ログインユーザーアカウント</param>
    ''' <param name="beginRowIndex">リピータのスタート行</param>
    ''' <param name="followBox">followBox連番</param>
    ''' <param name="nextRowButton">次へのボタン判定</param>
    ''' <returns>SalesNoticeHistoryDataTable</returns>
    ''' <remarks></remarks>
    Public Function ReadSalesNotification(ByVal userAccount As String, _
                                                  ByVal beginRowIndex As Integer, _
                                                  ByVal followBox As String, _
                                                  ByRef nextRowButton As Boolean) _
                                                  As SC3040801DataSet.SalesNoticeHistoryDataTable
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_userAccount=" & _
                    userAccount & _
                    "_beginRowIndex=" & _
                    CStr(beginRowIndex))

        Dim displayDays As Date
        '表示日付を取得する
        displayDays = GetNoticeDispDays()
        Dim staffInfo As StaffContext = StaffContext.Current

        Using dataSet As New SC3040801DataSetTableAdapters.SC3040801TableAdapter
            '履歴の取得
            dtGetSalesNotice = dataSet.GetSalesNotice(userAccount, _
                                                      beginRowIndex, _
                                                      displayDays, _
                                                      staffInfo.DlrCD, _
                                                      staffInfo.BrnCD)
            '写真のURLを取得
            'dtGetPhotoPath = dataSet.GetPhotoPath(staffInfo.DlrCD, staffInfo.BrnCD)'URL取得方法変更につきDRLENVSETTINGに変更
        End Using

        Dim rowindex As Integer = beginRowIndex - 1 'リピーターのスタート行
        Dim endindex As Integer                     'ループの最終行

        'カスタムリピーターのキャッシュ行とDBのカウントの行を比較して小さいほうをループの最後にする
        If (dtGetSalesNotice.Rows.Count - 1) <= rowindex Then
            endindex = dtGetSalesNotice.Rows.Count - 1 'ループ数セット
            Logger.Info("endindex=" & CStr(endindex))
            '次へボタン非表示
            nextRowButton = False
            Logger.Info("nextRowButton.Visible=False")
        Else
            endindex = rowindex 'ループ数セット
            Logger.Info("endindex=" & CStr(endindex))
            '次へボタン表示
            nextRowButton = True
            Logger.Info("nextRowButton.Visible=True")
        End If

        'Dim rowCount As Integer = dtGetPhotoPath.Rows.Count 'データベースのカウント
        Dim imagePath As StringBuilder = New StringBuilder  '写真パス
        Dim photoPathExistence As Boolean                   'パスの有無
        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow
        '写真URL取得
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, FileUrl)

        If IsNothing(dtrow) Then '取得できなかった場合
            photoPathExistence = False
            'NOPOTO
            imagePath.Append(NoPhotoUrl)
            Logger.Info("PhotoUrl=IsNothing")
        Else
            'URL有り
            photoPathExistence = True
            imagePath.Append("~/") '結合
            imagePath.Append(dtrow.PARAMVALUE.Trim)
            Logger.Info("PhotoUrl=" & CStr(dtrow.PARAMVALUE))
        End If

        '文言の取得
        Dim assessmentWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdAudit)
        Dim priceWord As String = WebWordUtility.GetWord(WordIdPageId, WordIdPrice)

        Dim categoryValue As String = staffInfo.PresenceCategory    'カテゴリー(大分類)
        Logger.Info("staffInfo.PresenceCategory=" & categoryValue)
        Dim detailValue As String = staffInfo.PresenceDetail        'カテゴリー(小分類)
        Logger.Info("staffInfo.PresenceDetail=" & detailValue)

        '取得してきた履歴の整形
        For i As Integer = 0 To endindex
            Dim historyRow As SC3040801DataSet.SalesNoticeHistoryRow
            historyRow = CType(dtSalesNoticeHistory.NewRow,  _
                               SC3040801DataSet.SalesNoticeHistoryRow)

            Dim sysImageFile As String = dtGetSalesNotice(i).ORG_IMGFILE.Trim       'PHOTOFile名
            Dim noticeMessage As String = dtGetSalesNotice(i).NOTICEMSG_DLR.Trim    'メッセージ
            Dim iconImage As String = String.Empty                                  'アイコンイメージ
            Dim activity As Boolean = False                                         'リンク活性フラグ

            Logger.Info("ROW" & CStr(i) & "_NOTICEREQCTG=" & dtGetSalesNotice(i).NOTICEREQCTG)

            'セッションの中のカテゴリー判定(リンクの制御)
            If (CategoryOne.Equals(categoryValue) _
                And CategoryZero.Equals(detailValue)) _
                OrElse (CategoryThree.Equals(categoryValue) _
                And CategoryZero.Equals(detailValue)) Then '★★★スタッフスタンバイ中(大カテゴリー1or3)(小カテゴリー0)★★★
                Logger.Info("ROW" & CStr(i) & "_Staff=Standby")
                '活動の確認(FLLWUPBOX_SEQNOの登録されているか確認)
                If dtGetSalesNotice(i).IsFLLWUPBOX_SEQNONull Then '●●●活動内容なし(リンクなし)●●●
                    Logger.Info("ROW" & CStr(i) & "_GetSalesNotice.FLLWUPBOX_SEQNO=Nothing")
                    Logger.Info("ROW" & CStr(i) & "_LinkNo")
                    activity = False
                Else        '●●●活動内容あり(リンクあり)●●●
                    Logger.Info("ROW" & CStr(i) & "_GetSalesNotice.FLLWUPBOX_SEQNO=Activity")
                    Logger.Info("ROW" & CStr(i) & "_LinkOk")
                    activity = True
                End If
            Else    '★★★商談中または営業活動中★★★
                Logger.Info("ROW" & CStr(i) & "_Staff=Activity")
                '活動が一致している場合(FLLWUPBOXとセッションのFLLWUPBOXが一致しているか確認)
                If dtGetSalesNotice(i).FLLWUPBOX = CLng(followBox) Then     '▲▲▲活動内容が一致している場合(リンクあり)▲▲▲
                    Logger.Info("ROW" & CStr(i) & "_dtGetSalesNotice.FLLWUPBOX=followBox=match")
                    Logger.Info("ROW" & CStr(i) & "_LinkOk")
                    Logger.Info("ROW" & CStr(i) & "_dtGetSalesNotice.FLLWUPBOX=" & CStr(dtGetSalesNotice(i).FLLWUPBOX))
                    activity = True
                Else        '▲▲▲活動内容が一致している場合(リンクなし)▲▲▲
                    Logger.Info("ROW" & CStr(i) & "_dtGetSalesNotice.FLLWUPBOX=followBox=anmatch")
                    Logger.Info("ROW" & CStr(i) & "_FLLWUPBOX_SEQNO=Nothing")
                    Logger.Info("ROW" & CStr(i) & "_LinkNo")
                    activity = False
                End If
            End If

            '送信者名をReplace
            noticeMessage = Replace(noticeMessage, _
                                    PermutationFromStaffName, _
                                    dtGetSalesNotice(i).FROMACCOUNTNAME)

            If activity Then '■■■リンクあり■■■
                '顧客名をReplace
                noticeMessage = Replace(noticeMessage, _
                                        PermutationCust, _
                                        dtGetSalesNotice(i).CUSTOMNAME)

                '受信者名をReplace
                noticeMessage = Replace(noticeMessage, _
                                        PermutationToStaffName, _
                                        dtGetSalesNotice(i).TOACCOUNTNAME)

                '依頼種別を置き換える
                Select Case dtGetSalesNotice(i).NOTICEREQCTG
                    Case Assessment   '査定
                        '文言から査定をReplace
                        noticeMessage = Replace(noticeMessage, PermutationReq, assessmentWord)
                        'アイコンの設定
                        iconImage = IconAss
                    Case Consultation '価格相談
                        '文言から価格相談をReplace
                        noticeMessage = Replace(noticeMessage, PermutationReq, priceWord)
                        'アイコンの設定
                        iconImage = IconCon
                    Case Help         'ヘルプ
                        'アイコンの設定
                        iconImage = IconHelp
                End Select
            Else '■■■リンクなし■■■
                '依頼種別を置き換える
                Select Case dtGetSalesNotice(i).NOTICEREQCTG
                    Case Assessment   '査定
                        '顧客名をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationCustNothing, _
                                                dtGetSalesNotice(i).CUSTOMNAME)

                        '文言から査定をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationAssessment, _
                                                assessmentWord)
                        'アイコンの設定
                        iconImage = IconAss
                    Case Consultation '価格相談
                        '顧客名をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationCustNothing, _
                                                dtGetSalesNotice(i).CUSTOMNAME)

                        '受信者名をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationToStaffName, _
                                                dtGetSalesNotice(i).TOACCOUNTNAME)

                        '文言から価格相談をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationConsultation, _
                                                priceWord)
                        'アイコンの設定
                        iconImage = IconCon
                    Case Help         'ヘルプ
                        '顧客名をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationCustNothing, _
                                                dtGetSalesNotice(i).CUSTOMNAME)

                        '受信者名をReplace
                        noticeMessage = Replace(noticeMessage, _
                                                PermutationToStaffName, _
                                                dtGetSalesNotice(i).TOACCOUNTNAME)

                        'アイコンの設定
                        iconImage = IconHelp
                End Select
            End If

            Dim canncelBotton As Boolean 'キャンセルフラグ
            'キャンセルボタン表示判定(送信者自分ANDｽﾃｰﾀｽ"1")
            If userAccount.Equals(dtGetSalesNotice(i).FROMACCOUNT) _
                And StatusRequest.Equals(dtGetSalesNotice(i).STATUS) Then
                Logger.Info("ROW" & CStr(i) & "_canncelBotton.Visible=True")
                canncelBotton = True 'キャンセルボタン表示
            Else
                Logger.Info("ROW" & CStr(i) & "_canncelBotton.Visible=False")
                canncelBotton = False 'キャンセルボタン非表示
            End If

            Dim photoFullPath As StringBuilder = New StringBuilder
            '表示写真の判定
            If String.IsNullOrEmpty(sysImageFile) _
            OrElse Not photoPathExistence Then '写真File名なし'URLの有無
                'NOPOTO                   
                photoFullPath.Append(NoPhotoUrl)
                Logger.Info("ROW" & CStr(i) & "_photoFullPath=" & photoFullPath.ToString)
            Else                                '写真File名あり'有り
                photoFullPath.Append(imagePath.ToString)
                photoFullPath.Append(sysImageFile)
                Logger.Info("ROW" & CStr(i) & "_sysImageFile=" & sysImageFile)
                Logger.Info("ROW" & CStr(i) & "_photoFullPath=" & photoFullPath.ToString)
            End If

            Dim timeMessage As String       'タイムメッセージ
            '時間計算
            timeMessage = ConvertSendTime(dtGetSalesNotice(i).SENDDATE)

            historyRow.LISTID = CStr(i)
            historyRow.NOTICEREQID = dtGetSalesNotice(i).NOTICEREQID             '通知依頼ID
            Logger.Info("ROW" & CStr(i) & "_NOTICEREQID=" & CStr(dtGetSalesNotice(i).NOTICEREQID))
            historyRow.NOTICEID = dtGetSalesNotice(i).NOTICEID                   '通知ID
            Logger.Info("ROW" & CStr(i) & "_NOTICEID=" & CStr(dtGetSalesNotice(i).NOTICEID))
            historyRow.ORG_IMGFILE = photoFullPath.ToString                      '写真名
            historyRow.READFLG = dtGetSalesNotice(i).READFLG.Trim                '既読フラグ
            historyRow.CANCELFLAG = canncelBotton                                'キャンセルボタンフラグ
            historyRow.TIMEMESSAGE = timeMessage                                 '時間表示
            historyRow.MESSAGE = noticeMessage                                   'テンプレート
            Logger.Info("ROW" & CStr(i) & "_MESSAGE=" & noticeMessage)
            historyRow.ICONIMAGE = iconImage                                     'アイコンイメージ
            historyRow.SESSIONVALUE = CStr(dtGetSalesNotice(i).NOTICEREQID)      'セッション情報

            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
            historyRow.SUPPORTSTATUS = "0"                                      '対応ステータス
            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End

            'Rowの追加
            dtSalesNoticeHistory.Rows.Add(historyRow)
        Next

        Logger.Info("return=dtSalesNoticeHistory__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")


        Return dtSalesNoticeHistory

    End Function

    ''' <summary>
    ''' サービス履歴の取得
    ''' </summary>
    ''' <param name="userAccount">ログインユーザーアカウント</param>
    ''' <param name="beginRowIndex">リピータのスタート行</param>
    ''' <param name="nextRowButton">次へのボタン判定</param>
    ''' <returns>ServiceNoticeHistoryDataTable</returns>
    ''' <remarks></remarks>
    Public Function ReadServiceNotification(ByVal userAccount As String, _
                                                    ByVal beginRowIndex As Integer, _
                                                    ByRef nextRowButton As Boolean) _
                                                As SC3040801DataSet.ServiceNoticeHistoryDataTable
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_userAccount=" & _
                    userAccount & _
                    "_beginRowIndex=" & _
                    CStr(beginRowIndex))

        Dim displayDays As Date
        'DBから設定日付を取得
        displayDays = GetNoticeDispDays()
        Dim staffInfo As StaffContext = StaffContext.Current

        Using Dataset As New SC3040801DataSetTableAdapters.SC3040801TableAdapter
            '検索処理
            dtGetServiceNotice = Dataset.GetServiceNotice(userAccount, _
                                                          beginRowIndex, _
                                                          displayDays)
            '写真のURLの取得
            'dtGetPhotoPath = Dataset.GetPhotoPath(staffInfo.DlrCD, _
            '                                      staffInfo.BrnCD)'URL取得方法変更につきDRLENVSETTINGに変更
        End Using

        Dim rowindex As Integer = beginRowIndex - 1 'リピーターのスタート行
        Dim endindex As Integer                     'リピーターの最終行

        'リピーターのキャッシュ行とDBのカウントの行を比較して小さいほうをループの最後にする
        If (dtGetServiceNotice.Rows.Count - 1) <= rowindex Then
            endindex = dtGetServiceNotice.Rows.Count - 1 'ループ数セット
            Logger.Info("endindex=" & CStr(endindex))
            '次へボタン非表示
            nextRowButton = False
            Logger.Info("nextRowButton.Visible=False")
        Else
            endindex = rowindex 'ループ数セット
            Logger.Info("endindex=" & CStr(endindex))
            '次へボタン表示
            nextRowButton = True
            Logger.Info("nextRowButton.Visible=True")
        End If

        'Dim rowCount As Integer = dtGetPhotoPath.Rows.Count 'データベースのカウント
        Dim imagePath As StringBuilder = New StringBuilder  '写真パス
        Dim photoPathExistence As Boolean                   'パスの有無
        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow
        '写真URL取得
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, FileUrl)

        If IsNothing(dtrow) Then '取得できなかった場合
            photoPathExistence = False
            'NOPOTO
            imagePath.Append(NoPhotoUrl)
            Logger.Info("PhotoUrl=IsNothing")
        Else
            'URL有り
            photoPathExistence = True
            imagePath.Append("~/")
            imagePath.Append(dtrow.PARAMVALUE.Trim)
            Logger.Info("PhotoUrl=" & CStr(dtrow.PARAMVALUE))
        End If

        '取得してきた履歴の整形
        For i As Integer = 0 To endindex
            Dim historyRow As SC3040801DataSet.ServiceNoticeHistoryRow
            historyRow = CType(dtServiceNoticeHistory.NewRow,  _
                               SC3040801DataSet.ServiceNoticeHistoryRow)            
            '写真名
            Dim sysImageFile As String = dtGetServiceNotice(i).ORG_IMGFILE.Trim

            Dim photoFullPath As StringBuilder = New StringBuilder
            '表示写真の判定
            If String.IsNullOrEmpty(sysImageFile) _
            OrElse Not photoPathExistence Then '写真File名なし'URLの無
                'NOPOTO                   
                photoFullPath.Append(NoPhotoUrl)
                Logger.Info("ROW" & CStr(i) & "_photoFullPath=" & photoFullPath.ToString)
            Else                                '写真File名あり'URL
                photoFullPath.Append(imagePath.ToString)
                photoFullPath.Append(sysImageFile)
                Logger.Info("ROW" & CStr(i) & "_sysImageFile=" & sysImageFile)
                Logger.Info("ROW" & CStr(i) & "_photoFullPath=" & photoFullPath.ToString)
            End If

            Dim timeMessage As String 'タイムメッセージ
            '時間計算
            timeMessage = ConvertSendTime(dtGetServiceNotice(i).SENDDATE)

            historyRow.LISTID = CStr(i)
            historyRow.NOTICEREQID = dtGetServiceNotice(i).NOTICEREQID          '通知依頼ID
            Logger.Info("ROW" & CStr(i) & "_NOTICEREQID=" & CStr(dtGetServiceNotice(i).NOTICEREQID))
            historyRow.NOTICEID = dtGetServiceNotice(i).NOTICEID                '通知ID
            Logger.Info("ROW" & CStr(i) & "_NOTICEREQID=" & CStr(dtGetServiceNotice(i).NOTICEID))
            historyRow.READFLG = dtGetServiceNotice(i).READFLG                  '既読フラグ
            historyRow.SESSIONVALUE = dtGetServiceNotice(i).SESSIONVALUE        'SESSIONVALUE
            Logger.Info("ROW" & CStr(i) & "_SESSIONVALUE=" & dtGetServiceNotice(i).SESSIONVALUE)
            historyRow.ORG_IMGFILE = photoFullPath.ToString                     '写真のフルパス
            historyRow.MESSAGE = dtGetServiceNotice(i).MESSAGE                  'メッセージ
            Logger.Info("ROW" & CStr(i) & "_MESSAGE=" & dtGetServiceNotice(i).MESSAGE)
            historyRow.TIMEMESSAGE = timeMessage                                '時間表示
            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
            historyRow.SUPPORTSTATUS = dtGetServiceNotice(i).SUPPORTSTATUS      '対応ステータス
            Logger.Info("ROW" & CStr(i) & "SUPPORTSTATUS=" & dtGetServiceNotice(i).SUPPORTSTATUS)
            '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End
            'ROWの追加
            dtServiceNoticeHistory.Rows.Add(historyRow)
        Next

        Logger.Info("return=dtServiceNoticeHistory__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")
        Return dtServiceNoticeHistory
    End Function

#End Region

#Region "メソッド"

    ''' <summary>
    ''' 最終ステータス取得
    ''' </summary>
    ''' <param name="noticeRequestId">通知依頼情報</param>
    ''' <returns>最終ステータス</returns>
    ''' <remarks></remarks>
    Public Function GetLastStatus(ByVal noticeRequestId As Long) As String
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_noticereqID=" & _
                    CStr(noticeRequestId))

        Using Dataset As New SC3040801DataSetTableAdapters.SC3040801TableAdapter
            '最終ステータスの取得
            dtGetLastStatus = Dataset.GetLastStatus(noticeRequestId)
        End Using

        Logger.Info("return=" & _
                    dtGetLastStatus(InitCount).STATUS & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")

        Return dtGetLastStatus(InitCount).STATUS
    End Function

    ''' <summary>
    ''' 時間メッセージ
    ''' </summary>
    ''' <param name="sendDate">送信日時</param>
    ''' <returns>時間表示メッセージ</returns>
    ''' <remarks></remarks>
    Private Function ConvertSendTime(ByVal SendDate As Date) As String
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_sendDate=" & _
                    CStr(SendDate))

        Dim timeMessage As StringBuilder = New StringBuilder
        Logger.Info("timeNow=" & CStr(timeNow))
        Dim timeNo As Integer = InitCount
        Dim minuteCalc As Integer = InitCount
        Dim hourDifference As Integer = InitCount

        Dim daySend As Date = Date.Parse(Format(SendDate, DateFormatYYYMMDD), _
                                         CultureInfo.CurrentCulture)  'sendDateフォーマット(yyyy/MM/dd) 

        If oneMinutesBefore <= SendDate Then '★★★★1分以内★★★★
            'どの文言を使用するか設定
            timeNo = TimeStatus.Now
        ElseIf oneHourBefore < SendDate Then '★★★★1時間以内★★★★
            '分の計算
            minuteCalc = CInt((timeNow - SendDate).TotalMinutes)
            'どの文言を使用するか設定
            timeNo = TimeStatus.Minute
        ElseIf today = daySend Then '★★★★当日★★★★

            Dim minutesDifference As Integer
            '時間の計算
            hourDifference = CInt(Math.Floor((timeNow - SendDate).TotalHours))
            '分の計算
            minutesDifference = CInt((timeNow - SendDate).TotalMinutes) Mod 60

            '30分以上なら繰上げ
            If TimeHelfHour <= minutesDifference Then
                '1時間繰り上げ
                hourDifference = hourDifference + CountOne
                If DayHour = hourDifference Then '24時以降になった場合
                    '昨日に変更
                    hourDifference = InitCount
                    'どの文言を使用するか設定
                    timeNo = TimeStatus.Yesterday
                Else
                    'どの文言を使用するか設定
                    timeNo = TimeStatus.OtherHours
                End If
            Else '30分未満はそのまま
                If hourDifference = CountOne Then '1時間前か判定
                    hourDifference = InitCount '1時間前のときはリセット
                    'どの文言を使用するか設定
                    timeNo = TimeStatus.OneHour '1時間用の文言セット
                Else
                    'どの文言を使用するか設定
                    timeNo = TimeStatus.OtherHours
                End If
            End If
        ElseIf oneDay = daySend Then '★★★★昨日★★★★★
            'どの文言を使用するか設定
            timeNo = TimeStatus.Yesterday
        ElseIf daySend <= twoDay Then '★★★★2日以上★★★★
            '1年以上か
            If timeNow.Year <> SendDate.Year Then 'yyyyが違う
                timeMessage.Append(DateTimeFunc.FormatDate(21, SendDate)) 'フォーマット変更(yyyy/MM/dd)
            Else 'yyyyが同じ
                timeMessage.Append(DateTimeFunc.FormatDate(11, SendDate)) 'フォーマット変更(MM/dd)
            End If
        End If
        '一時間以内の場合
        If minuteCalc <> 0 Then
            timeMessage.Append(CStr(minuteCalc))
        End If
        '文言から設定
        timeMessage.Append(timeWord(timeNo)(0))
        '当日の場合
        If hourDifference <> 0 Then
            timeMessage.Append(CStr(hourDifference))
        End If
        '文言から設定
        timeMessage.Append(timeWord(timeNo)(1))

        Logger.Info("return=" & _
                    timeMessage.ToString & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")

        Return timeMessage.ToString
    End Function

    ''' <summary>
    '''表示日付の取得
    ''' </summary>
    ''' <returns>表示設定日にち</returns>
    ''' <remarks>設定日</remarks>
    Private Function GetNoticeDispDays() As Date
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name)

        Dim dispDays As DateTime
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dlrSetteing As New BranchEnvSetting
        Dim dtrow As DlrEnvSettingDataSet.DLRENVSETTINGRow
        'DLRENVSETTINGより日数の取得
        dtrow = dlrSetteing.GetEnvSetting(staffInfo.DlrCD, staffInfo.BrnCD, DisplyDays)

        If IsNothing(dtrow) Then '取得できなかった場合
            Logger.Info("GetEnvSetting=IsNothing")
            Logger.Info("Deflut=0")
            dispDays = dayNow '初期値の0日の設定
        Else
            Logger.Info("GetEnvSetting.PARAMVALUE=" & CStr(dtrow.PARAMVALUE))
            dispDays = dayNow.AddDays(-CInt(dtrow.PARAMVALUE)) '日付の計算
        End If

        Logger.Info("return=" & _
                    CStr(dispDays) & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")

        Return dispDays
    End Function

    ''' <summary>
    ''' セッションに詰める値の取得
    ''' </summary>
    ''' <param name="noticeRequestId">通知依頼ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetTransitionParameter(ByVal noticeRequestId As Long) _
                              As SC3040801DataSet.GetTransitionParameterDataTable
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_noticeRequestId=" & _
                    CStr(noticeRequestId))

        Using Dataset As New SC3040801DataSetTableAdapters.SC3040801TableAdapter
            '検索処理
            dtGetTransitionParameter = Dataset.GetTransitionParameter(noticeRequestId)
        End Using

        Logger.Info("return=GetTransitionParameterDataTable=" & _
                    CStr(dtGetTransitionParameter.Rows.Count) & _
                    "_" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "__END")

        Return dtGetTransitionParameter
    End Function

    ''' <summary>
    ''' キャンセル処理
    ''' </summary>
    ''' <param name="noticeRequestId">通知依頼ID</param>
    ''' <remarks></remarks>
    Public Sub SetCancelParameter(ByVal noticeRequestId As Long)
        Logger.Info("START__" & _
                    System.Reflection.MethodBase.GetCurrentMethod.Name & _
                    "_noticeRequestId=" & _
                    CStr(noticeRequestId))

        Dim staffInfo As StaffContext = StaffContext.Current

        Using Dataset As New SC3040801DataSetTableAdapters.SC3040801TableAdapter
            'APIへ渡すパラメーターの取得
            dtGetCancelParameter = Dataset.GetCancelParameter(noticeRequestId, staffInfo.Account)
        End Using

        RequestNotice.DealerCode = staffInfo.DlrCD                          '販売店コード  
        Logger.Info("DealerCode=" & staffInfo.DlrCD)
        RequestNotice.StoreCode = staffInfo.BrnCD                           '店舗コード
        Logger.Info("StoreCode=" & staffInfo.BrnCD)
        RequestNotice.RequestClass = dtGetCancelParameter(0).NOTICEREQCTG   '依頼種別
        Logger.Info("RequestClass=" & dtGetCancelParameter(0).NOTICEREQCTG)
        RequestNotice.Status = StatusCancel                                 'ステータスキャンセル
        Logger.Info("Status=" & StatusCancel)
        RequestNotice.RequestId = noticeRequestId                           '通知依頼ID
        Logger.Info("RequestId=" & CStr(noticeRequestId))
        RequestNotice.RequestClassId = dtGetCancelParameter(0).REQCLASSID   '依頼種別ID
        Logger.Info("RequestClassId=" & CStr(dtGetCancelParameter(0).REQCLASSID))
        RequestNotice.FromAccountName = staffInfo.UserName                  '送信者名
        Logger.Info("FromAccountName=" & staffInfo.UserName)
        RequestNotice.FromAccount = staffInfo.Account                       '送信者アカウント
        Logger.Info("FromAccount=" & staffInfo.Account)
        RequestNotice.FromClientId = dtGetCancelParameter(0).FROMCLIENTID   '送信者端末
        Logger.Info("FromClientId=" & dtGetCancelParameter(0).FROMCLIENTID)
        RequestNotice.CustomName = dtGetCancelParameter(0).CUSTOMNAME       '顧客名
        Logger.Info("CustomName=" & dtGetCancelParameter(0).CUSTOMNAME)

        '受信者アカウント分ループ
        For i As Integer = 0 To dtGetCancelParameter.Rows.Count - 1
            Account = New XmlAccount
            Account.ToAccount = dtGetCancelParameter(i).TOACCOUNT           '受信者アカウント
            Logger.Info("ToAccount=" & dtGetCancelParameter(i).TOACCOUNT)
            Account.ToClientId = dtGetCancelParameter(i).TOCLIENTID         '受信者端末
            Logger.Info("ToClientId=" & dtGetCancelParameter(i).TOCLIENTID)
            Account.ToAccountName = dtGetCancelParameter(i).TOACCOUNTNAME   '受信者名
            Logger.Info("ToAccountName=" & dtGetCancelParameter(i).TOACCOUNTNAME)
            NoticeData.AccountList.Add(Account)
        Next
        '親クラスに詰める
        NoticeData.RequestNotice = RequestNotice
        NoticeData.PushInfo = Nothing

        Using BisLogic As New IC3040801BusinessLogic
            '通知API
            BisLogic.NoticeDisplay(NoticeData, NoticeDisposal.Peculiar) '固有
        End Using

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & _
        "__END")

    End Sub

    ''' <summary>
    ''' 通知情報既読フラグ更新処理
    ''' </summary>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Sub UpdateConfirmed()
        Logger.Info("START__" & _
        System.Reflection.MethodBase.GetCurrentMethod.Name)

        Try
            '通知情報の既読フラグを更新
            Using daDataSetTableAdapters As New IC3040801DataSetTableAdapters.IC3040801TableAdapters
                Dim drInfo As IC3040801DataSet.IC3040801NoticeInfoRow _
                     = DirectCast(dtNoticeInfo.NewRow, IC3040801DataSet.IC3040801NoticeInfoRow)
                Dim staffInfo As StaffContext = StaffContext.Current

                drInfo.ACCOUNT = staffInfo.Account              'ユーザーアカウント
                Logger.Info("ACCOUNT=" & staffInfo.Account)
                drInfo.SYSTEM = WordIdPageId                    'ページID
                Logger.Info("SYSTEM=" & WordIdPageId)

                '2014/04/14 TMEJ 丁 BTS-383 「通知履歴のレスポンスが遅い」対応 START
                drInfo.SENDDATE = DateTimeFunc.Now(staffInfo.DlrCD)  'SENDDATE
                Logger.Info("SENDDATE=" & drInfo.SENDDATE)
                '2014/04/14 TMEJ 丁 BTS-383 「通知履歴のレスポンスが遅い」対応 END

                '更新処理
                daDataSetTableAdapters.UpdateConfirmed(drInfo)
            End Using
        Catch ex As OracleExceptionEx
            Me.Rollback = True
            Logger.Error(ex.ToString, ex)
            Throw
        Catch ex As Exception
            Me.Rollback = True
            Logger.Error(ex.ToString, ex)
            Throw
        Finally
            Logger.Info("return=updateSuccess=" & _
            System.Reflection.MethodBase.GetCurrentMethod.Name & _
            "__END")
        End Try

    End Sub
    ''' <summary>
    '''　対応ステータス更新
    ''' </summary>
    ''' <param name="noticeId">通知ID</param>
    ''' <param name="supportStatus">SupportStatus</param>
    ''' <remarks></remarks>
    Public Sub UpdateSupportStatus(ByVal noticeId As Long, ByVal supportStatus As String)
        Logger.Info("START__" & _
        System.Reflection.MethodBase.GetCurrentMethod.Name)
        Dim errorType As String = "updateSuccess"
        Try
            'SuportStatusを更新
            Using daDataSetTableAdapters As New IC3040801DataSetTableAdapters.IC3040801TableAdapters
                Dim drInfo As IC3040801DataSet.IC3040801NoticeInfoRow _
                     = DirectCast(dtNoticeInfo.NewRow, IC3040801DataSet.IC3040801NoticeInfoRow)
                Dim staffInfo As StaffContext = StaffContext.Current

                drInfo.NOTICEID = noticeId                      '通知ID
                Logger.Info("NOTICEID=" & noticeId)
                drInfo.SUPPORTSTATUS = supportStatus            '対応ステータス
                Logger.Info("SUPPORTSTATUS=" & supportStatus)
                drInfo.ACCOUNT = staffInfo.Account              'ユーザーアカウント
                Logger.Info("ACCOUNT=" & staffInfo.Account)
                drInfo.SYSTEM = WordIdPageId                    'ページID
                Logger.Info("SYSTEM=" & WordIdPageId)
                '更新処理
                If daDataSetTableAdapters.UpdateSupportStatus(drInfo) <> 1 Then
                    Throw New Exception("Update Recode Noting")
                End If
            End Using

        Catch ex As OracleExceptionEx
            Me.Rollback = True
            Logger.Error(ex.ToString, ex)
            errorType = "updateError"
            Throw
        Catch ex As Exception
            Me.Rollback = True
            Logger.Error(ex.ToString, ex)
            errorType = "updateError"
            Throw
        Finally
            Logger.Info("return=" & errorType & "=" & _
            System.Reflection.MethodBase.GetCurrentMethod.Name & _
            "__END")
        End Try

    End Sub

#End Region

    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then

            dtSalesNoticeHistory.Dispose()
            dtGetSalesNotice.Dispose()
            dtGetPhotoPath.Dispose()
            dtGetServiceNotice.Dispose()
            dtServiceNoticeHistory.Dispose()
            dtGetLastStatus.Dispose()
            dtGetTransitionParameter.Dispose()
            dtNoticeInfo.Dispose()
            dtGetCancelParameter.Dispose()
            NoticeData.Dispose()

            If Not IsNothing(Account) Then
                Account.Dispose()
            End If

            RequestNotice.Dispose()

            dtSalesNoticeHistory = Nothing
            dtGetSalesNotice = Nothing
            dtGetPhotoPath = Nothing
            dtGetServiceNotice = Nothing
            dtServiceNoticeHistory = Nothing
            dtGetLastStatus = Nothing
            dtGetTransitionParameter = Nothing
            dtNoticeInfo = Nothing
            dtGetCancelParameter = Nothing
            NoticeData = Nothing
            Account = Nothing
            RequestNotice = Nothing

        End If

    End Sub

End Class