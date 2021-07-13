'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3040802BusinessLogic.vb
'─────────────────────────────────────
'機能： 通知一覧(MG用)
'補足： 
'作成： 2012/01/05 TCS 明瀬
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'更新： 2013/12/02 TCS 森   Aカード情報相互連携開発
'更新： 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.BizLogic
Imports Toyota.eCRB.Tool.Notify.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic

''' <summary>
''' 通知送受信一覧(MG用)
''' ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3040802BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 書式　数値(金額)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_NUMBER As String = "#,#0"

    ''' <summary>
    ''' 日時変換ID　年月日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERTDATE_YMD As Integer = 3

    ''' <summary>
    ''' 日時変換ID　月日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERTDATE_MD As Integer = 11

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

    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
    ''' <summary>
    ''' 通知依頼種別  注文承認
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICEREQ_ORDER As String = "08"
    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END


    ''' <summary>
    ''' 顧客種別　自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTKIND_ORG As String = "1"

    ''' <summary>
    ''' 顧客種別　未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUSTKIND_NEW As String = "2"

    ''' <summary>
    ''' I/Fパラメータ　ステータス(受信)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_STATUS_RECEIVE As String = "3"

    ''' <summary>
    ''' I/Fパラメータ　ステータス(受付)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_STATUS_RECEPTION As String = "4"

    ''' <summary>
    ''' I/Fパラメータ　カテゴリータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_CATEGORY As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPPOSITION As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPTIME As Long = 3

    ''' <summary>
    ''' I/Fパラメータ　表示タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPTYPE As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_COLOR As String = "2"

    ''' <summary>
    ''' I/Fパラメータ　表示時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_DISPFUNCTION As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' I/Fパラメータ　アクション時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFPARAM_ACTFUNCTION As String = "icropScript.ui.openNoticeDialog()"

    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 重要車両フラグ　LoyalCustomer
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IVF_LOYALCUSTOMER As String = "2"
    ''' <summary>
    ''' LoyalCustomerフラグ　TRUE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LCF_IS_LOYAL_CUSTOMER As String = "1"
    ''' <summary>
    ''' LoyalCustomerフラグ　FALSE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LCF_IS_NOT_LOYAL_CUSTOMER As String = "0"
    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

#End Region

#Region "メンバ変数"

    ' ''' <summary>
    ' ''' 現在時刻
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private mNowTime As Date

#End Region

#Region "Publicメソッド"

    ' ''' <summary>
    ' ''' コンストラクタ
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Sub New()
    '    'TODO:テスト用
    '    'mNowTime = CType("2012/01/10 20:30:00", Date)
    '    mNowTime = DateTimeFunc.Now(StaffContext.Current.DlrCD)
    'End Sub

    ''' <summary>
    ''' 初期表示データを取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInitialData() As SC3040802DataSet

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        'スタッフ情報取得
        Dim staff As StaffContext = StaffContext.Current

        '通知情報データテーブルを取得 
        '2013/12/02 TCS 森   Aカード情報相互連携開発 START
        Dim noticeInfoDt As SC3040802DataSet.SC3040802NoticeInfoDataTable = Me.GetNoticeInfoDt(staff.Account, staff.DlrCD, staff.TeamLeader)
        '2013/12/02 TCS 森   Aカード情報相互連携開発 END

        '通知情報データテーブルが取得できたかチェック
        If noticeInfoDt Is Nothing OrElse noticeInfoDt.Rows.Count = 0 Then
            '取得できていなければ何も返さない
            Return Nothing
        End If

        '返却用データテーブルを生成
        Using rtnDs As New SC3040802DataSet

            '通知情報データテーブルの取得件数分ループ
            For Each noticeInfoRow In noticeInfoDt

                Dim discountApprovalDt As SC3040802DataSet.SC3040802DiscountApprovalDataTable = Nothing
                Dim helpInfoDt As SC3040802DataSet.SC3040802HelpInfoDataTable = Nothing
                ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
                Dim estimateDt As SC3040802DataSet.SC3040802EstimateApprovalDataTable = Nothing
                ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END

                ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                'LoyalCustomer判定
                Dim loyalCustomerFlg As String = LCF_IS_NOT_LOYAL_CUSTOMER
                '通知情報データテーブル.顧客コードがDBNULLでなければSQL発行
                If Not noticeInfoRow.IsCRCUSTIDNull Then
                    '重要車両フラグの取得
                    Dim loyalCustomerDt As SC3040802DataSet.SC3040802ImpVclFlgDataTable = Me.GetImpVclFlgDt(staff.DlrCD, noticeInfoRow.CRCUSTID)
                    '重要車両フラグ＝"2"(LoyalCustomer)の場合
                    If loyalCustomerDt.Rows.Count > 0 AndAlso loyalCustomerDt(0).IMP_VCL_FLG.Equals(IVF_LOYALCUSTOMER) Then
                        loyalCustomerFlg = LCF_IS_LOYAL_CUSTOMER
                    End If

                End If
                ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

                If noticeInfoRow.NOTICEREQCTG.Equals(NOTICEREQ_DISCOUNTAPPROVAL) Then
                    '通知情報データテーブル.通知依頼種別が"02"(価格相談)の場合

                    '依頼種別IDがDBNULLでなければSQL発行
                    If Not noticeInfoRow.IsREQCLASSIDNull Then
                        discountApprovalDt = Me.GetDiscountApprovalDt(noticeInfoRow.REQCLASSID)
                    End If

                ElseIf noticeInfoRow.NOTICEREQCTG.Equals(NOTICEREQ_HELP) Then
                    '通知情報データテーブル.通知依頼種別が"03"(ヘルプ)の場合

                    '依頼種別IDがDBNULLでなければSQL発行
                    If Not noticeInfoRow.IsREQCLASSIDNull Then
                        helpInfoDt = Me.GetHelpInfoDt(noticeInfoRow.REQCLASSID, staff.DlrCD)
                    End If

                    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
                ElseIf noticeInfoRow.NOTICEREQCTG.Equals(NOTICEREQ_ORDER) Then
                    '通知情報データテーブル.通知依頼種別が"08"(注文承認)の場合

                    '依頼種別IDがDBNULLでなければSQL発行
                    If Not noticeInfoRow.IsREQCLASSIDNull Then
                        estimateDt = Me.GetEstimateApprovalDt(noticeInfoRow.REQCLASSID)
                    End If
                    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END
                End If

                ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
                '返却値を返却用データセットに設定する
                ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                Me.SetReturnData(rtnDs, noticeInfoRow, helpInfoDt, discountApprovalDt, estimateDt, loyalCustomerFlg)
                ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
                ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDs.Tables.Count.ToString(CultureInfo.CurrentCulture)))

            'データセットの返却
            Return rtnDs
        End Using

    End Function

    ''' <summary>
    ''' 通知情報の件数を取得する
    ''' </summary>
    ''' <param name="account"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeInfoCountDT(ByVal account As String, ByVal dlrCD As String) As SC3040802DataSet.SC3040802NoticeCountDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[account:{0}][dlrCD:{1}]", account, dlrCD))

        Dim ta As New SC3040802TableAdapter

        '検索処理
        Dim rtnDt As SC3040802DataSet.SC3040802NoticeCountDataTable = ta.GetNoticeInfoCount(account, dlrCD)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function

    ''' <summary>
    ''' 通知登録I/Fを呼び、通知情報を登録する
    ''' </summary>
    ''' <param name="paramIfDt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetRequestNoticeInfo(ByVal paramIfDT As SC3040802DataSet.SC3040802ParamIfDataTable) As XmlCommon

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[Count:{0}]", paramIfDT.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Using noticeData As New XmlNoticeData

            '送信日付
            noticeData.TransmissionDate = DateTimeFunc.Now(StaffContext.Current.DlrCD)

            Dim paramIfRow As SC3040802DataSet.SC3040802ParamIfRow = paramIfDT.Item(0)

            'accountにデータを格納
            Using account As New XmlAccount

                account.ToAccount = paramIfRow.TOACCOUNT           'スタッフコード（受信先）
                account.ToAccountName = paramIfRow.TOACCOUNTNAME   '受信者名（受信先）

                '格納したデータを親クラスに格納
                noticeData.AccountList.Add(account)
            End Using

            Dim staff As StaffContext = StaffContext.Current

            ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
            'requestNoticeにデータを格納
            Using requestNotice As New XmlRequestNotice

                requestNotice.DealerCode = staff.DlrCD                  '販売店コード
                requestNotice.StoreCode = staff.BrnCD                   '店舗コード
                requestNotice.RequestClass = paramIfRow.REQUESTCLASS    '依頼種別(02:価格相談、03:ヘルプ、08:契約承認)

                'ステータス(価格相談、契約承認:3(受信)、ヘルプ：4(受付))
                If requestNotice.RequestClass.Equals(NOTICEREQ_DISCOUNTAPPROVAL) Or _
                    requestNotice.RequestClass.Equals(NOTICEREQ_ORDER) Then
                    requestNotice.Status = IFPARAM_STATUS_RECEIVE
                ElseIf requestNotice.RequestClass.Equals(NOTICEREQ_HELP) Then
                    requestNotice.Status = IFPARAM_STATUS_RECEPTION
                Else
                    requestNotice.Status = String.Empty
                End If
                requestNotice.RequestId = CLng(paramIfRow.REQUESTID)            '依頼ID
                requestNotice.RequestClassId = CLng(paramIfRow.REQUESTCLASSID)  '依頼種別ID
                requestNotice.FromAccount = staff.Account                       'スタッフコード（送信元：通知の返却者）
                requestNotice.FromAccountName = staff.UserName                  'スタッフ名（送信元：通知の返却者）

                noticeData.RequestNotice = requestNotice

            End Using

            'pushInfoにデータを格納
            Using pushInfo As New XmlPushInfo

                pushInfo.PushCategory = IFPARAM_CATEGORY        'カテゴリータイプ
                pushInfo.PositionType = IFPARAM_DISPPOSITION    '表示位置
                pushInfo.Time = IFPARAM_DISPTIME                '表示時間
                pushInfo.DisplayType = IFPARAM_DISPTYPE         '表示タイプ

                '表示内容(価格相談は価格相談用、ヘルプはヘルプ用、契約承認は契約承認用)
                If paramIfRow.REQUESTCLASS.Equals(NOTICEREQ_DISCOUNTAPPROVAL) Then
                    '{0}さんが{1}の価格相談依頼を受付ました
                    pushInfo.DisplayContents = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(9), _
                                                             staff.UserName, paramIfRow.CUSTNAME)
                ElseIf paramIfRow.REQUESTCLASS.Equals(NOTICEREQ_HELP) Then
                    '{0}さんが{1}のヘルプ依頼を受付ました
                    pushInfo.DisplayContents = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(11), _
                                                             staff.UserName, paramIfRow.CUSTNAME)
                ElseIf paramIfRow.REQUESTCLASS.Equals(NOTICEREQ_ORDER) Then
                    '{0}さんが{1}の契約承認を受付ました。
                    pushInfo.DisplayContents = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(12), _
                                                             staff.UserName, paramIfRow.CUSTNAME)
                Else
                    '通常入らない
                    pushInfo.DisplayContents = String.Empty
                End If

                pushInfo.Color = IFPARAM_COLOR                     '色
                pushInfo.DisplayFunction = IFPARAM_DISPFUNCTION    '表示時関数
                pushInfo.ActionFunction = IFPARAM_ACTFUNCTION      'アクション時関数

                noticeData.PushInfo = pushInfo
            End Using
            ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END

            'ロジックを呼ぶ
            Using apiBiz As New IC3040801BusinessLogic

                'i-CROPへ送信
                Dim rtnXml As XmlCommon = apiBiz.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_End[Message:{0}][NoticeRequestId:{1}][ResultId:{2}]", rtnXml.Message, rtnXml.NoticeRequestId.ToString(CultureInfo.CurrentCulture), rtnXml.ResultId))

                Return rtnXml
            End Using

        End Using

    End Function
#End Region

#Region "Privateメソッド"

    '2013/12/02 TCS 森   Aカード情報相互連携開発 START
    ''' <summary>
    ''' 通知情報を取得する
    ''' </summary>
    ''' <param name="account"></param>
    ''' <param name="dlrCD"></param>
    ''' <param name="isTeamLeader"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetNoticeInfoDt(ByVal account As String, ByVal dlrCD As String, ByVal isTeamLeader As Boolean) As SC3040802DataSet.SC3040802NoticeInfoDataTable
        '2013/12/02 TCS 森   Aカード情報相互連携開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[account:{0}][dlrCD:{1}]", account, dlrCD))

        Dim ta As New SC3040802TableAdapter

        '検索処理
        '2013/12/02 TCS 森   Aカード情報相互連携開発 START
        Dim rtnDt As SC3040802DataSet.SC3040802NoticeInfoDataTable = ta.GetNoticeInfo(account, dlrCD, isTeamLeader)
        '2013/12/02 TCS 森   Aカード情報相互連携開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function

    ''' <summary>
    ''' 見積情報を取得する
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDiscountApprovalDt(ByVal estimateId As Integer) As SC3040802DataSet.SC3040802DiscountApprovalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[estimateId:{0}]", estimateId))

        Dim ta As New SC3040802TableAdapter
        '検索処理
        Dim rtnDt As SC3040802DataSet.SC3040802DiscountApprovalDataTable = ta.GetDiscountApproval(estimateId)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function

    ''' <summary>
    ''' ヘルプ情報を取得する
    ''' </summary>
    ''' <param name="helpId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetHelpInfoDt(ByVal helpId As Integer, ByVal dlrCD As String) As SC3040802DataSet.SC3040802HelpInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[helpId:{0}][dlrCD:{1}]", helpId.ToString(CultureInfo.CurrentCulture), dlrCD))

        Dim ta As New SC3040802TableAdapter
        '検索処理
        Dim rtnDt As SC3040802DataSet.SC3040802HelpInfoDataTable = ta.GetHelpInfo(helpId, dlrCD)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function

    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
    ''' <summary>
    ''' 注文情報を取得する
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetEstimateApprovalDt(ByVal estimateId As Integer) As SC3040802DataSet.SC3040802EstimateApprovalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[estimateId:{0}]", estimateId))

        Dim ta As New SC3040802TableAdapter
        '検索処理
        Dim rtnDt As SC3040802DataSet.SC3040802EstimateApprovalDataTable = ta.GetEstimateApproval(estimateId)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function
    ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END

    ' ''' <summary>
    ' ''' 経過時間を表示する文言を取得する
    ' ''' </summary>
    ' ''' <param name="time"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetDiffTimeMessage(ByVal time As DateTime) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[time:{0}]", time.ToString(CultureInfo.CurrentCulture)))

    '    '返却値
    '    Dim rtnVal As String = String.Empty

    '    '現在時刻との差分時間(絶対値)
    '    Dim diffTime As TimeSpan = mNowTime.Subtract(time).Duration

    '    If 365 <= diffTime.Days Then
    '        '１年以上前の場合は、引数の年月日を返却する
    '        rtnVal = DateTimeFunc.FormatDate(CONVERTDATE_YMD, time)

    '    ElseIf 2 <= diffTime.Days Then
    '        '２日以上３６５日未満の場合は、引数の月日を返却する
    '        rtnVal = DateTimeFunc.FormatDate(CONVERTDATE_MD, time)

    '    ElseIf 1 <= diffTime.Days Then
    '        '１日以上２日未満の場合は、「昨日」を返却する
    '        rtnVal = WebWordUtility.GetWord(3)

    '    ElseIf 1 <= diffTime.Hours Then
    '        '１時間以上２４時間未満の場合は、「約{0}時間前」を返却する
    '        Dim intHour As Integer = diffTime.Hours
    '        If 30 <= diffTime.Minutes Then
    '            intHour += 1
    '        End If
    '        rtnVal = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(4), intHour.ToString(CultureInfo.CurrentCulture))

    '    ElseIf 1 <= diffTime.Minutes Then
    '        '１分以上６０分未満の場合は、「{0}分前」を返却する
    '        rtnVal = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(5), diffTime.Minutes.ToString(CultureInfo.CurrentCulture))

    '    Else
    '        '１分未満の場合は、「たった今」を返却する
    '        rtnVal = WebWordUtility.GetWord(6)

    '    End If

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[getMessage:{0}]", rtnVal))

    '    Return rtnVal

    'End Function

    ''' <summary>
    ''' 通知データの下半分に表示するメッセージを取得する
    ''' </summary>
    ''' <param name="strNoticeReqCtg"></param>
    ''' <param name="helpInfoDt"></param>
    ''' <param name="discountApprovalDt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDispMessage(ByVal strNoticeReqCtg As String, ByVal helpInfoDt As SC3040802DataSet.SC3040802HelpInfoDataTable, _
                                    ByVal discountApprovalDt As SC3040802DataSet.SC3040802DiscountApprovalDataTable, _
                                    ByVal estimateDt As SC3040802DataSet.SC3040802EstimateApprovalDataTable) As String()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[strNoticeReqCtg:{0}]", strNoticeReqCtg))

        Dim rtnMessage As String() = {String.Empty, String.Empty}

        'メッセージ１と２を取得する
        If strNoticeReqCtg.Equals(NOTICEREQ_DISCOUNTAPPROVAL) Then
            '通知依頼種別が"02"(価格相談)の場合

            If discountApprovalDt Is Nothing OrElse discountApprovalDt.Rows.Count = 0 Then
                rtnMessage(0) = String.Empty
                rtnMessage(1) = String.Empty
            Else
                Dim discountRow As SC3040802DataSet.SC3040802DiscountApprovalRow = discountApprovalDt.Item(0)
                rtnMessage(0) = discountRow.SERIESNM + "|" + discountRow.MODELNM
                rtnMessage(1) = WebWordUtility.GetWord(7) + discountRow.REQUESTPRICE.ToString(FORMAT_NUMBER, CultureInfo.CurrentCulture)
            End If

        ElseIf strNoticeReqCtg.Equals(NOTICEREQ_HELP) Then
            '通知依頼種別が"03"(ヘルプ)の場合

            If helpInfoDt Is Nothing OrElse helpInfoDt.Rows.Count = 0 Then
                rtnMessage(0) = String.Empty
                rtnMessage(1) = String.Empty
            Else
                Dim helpRow As SC3040802DataSet.SC3040802HelpInfoRow = helpInfoDt.Item(0)

                '現地語を出力
                rtnMessage(0) = helpRow.MSG_DLR
                'ヘルプの場合はstrMessage2はEmpt
                rtnMessage(1) = String.Empty
            End If

            ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
        ElseIf strNoticeReqCtg.Equals(NOTICEREQ_ORDER) Then
            '通知依頼種別が"08"(注文承認)の場合

            If estimateDt Is Nothing OrElse estimateDt.Rows.Count = 0 Then
                rtnMessage(0) = String.Empty
                rtnMessage(1) = String.Empty
            Else
                Dim estimateRow As SC3040802DataSet.SC3040802EstimateApprovalRow = estimateDt.Item(0)
                rtnMessage(0) = estimateRow.SERIESNM + "|" + estimateRow.MODELNM
                rtnMessage(1) = String.Empty
            End If

            ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[message1:{0}][message2:{1}]", rtnMessage(0), rtnMessage(1)))

        Return rtnMessage

    End Function

    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 返却用データセットにデータを設定
    ''' </summary>
    ''' <param name="rtnDs"></param>
    ''' <param name="noticeInfoRow"></param>
    ''' <param name="helpInfoDt"></param>
    ''' <param name="discountApprovalDt"></param>
    ''' <param name="estimateDt"></param>
    ''' <param name="loyalCustomerFlg"></param>
    ''' <remarks></remarks>
    Private Sub SetReturnData(ByVal rtnDs As SC3040802DataSet, ByVal noticeInfoRow As SC3040802DataSet.SC3040802NoticeInfoRow, _
                              ByVal helpInfoDt As SC3040802DataSet.SC3040802HelpInfoDataTable, _
                              ByVal discountApprovalDt As SC3040802DataSet.SC3040802DiscountApprovalDataTable, _
                              ByVal estimateDt As SC3040802DataSet.SC3040802EstimateApprovalDataTable, _
                              ByVal loyalCustomerFlg As String)
        ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        Dim intNoticeReqId As Integer = noticeInfoRow.NOTICEREQID                               '通知依頼ID
        Dim strNoticeReqCtg As String = noticeInfoRow.NOTICEREQCTG                              '通知依頼種別
        Dim strTimeMsg As String = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                  noticeInfoRow.SENDDATE, _
                                                                  StaffContext.Current.DlrCD)   '経過時間

        '依頼種別ID
        Dim intReqClassId As Integer = 0
        '依頼種別IDのDBNULLチェック
        If Not noticeInfoRow.IsREQCLASSIDNull Then
            intReqClassId = noticeInfoRow.REQCLASSID
        End If

        'Follow-up Box内連番
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        Dim intFllwUpBox As Decimal = -1
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END
        'Follow-up Box内連番のDBNULLチェック
        If Not noticeInfoRow.IsFLLWUPBOXNull Then
            intFllwUpBox = noticeInfoRow.FLLWUPBOX
        End If

        '通知者名
        Dim strStaffName As String = String.Empty
        '通知者名のDBNULLチェック
        If Not noticeInfoRow.IsFROMACCOUNTNAMENull Then
            strStaffName = noticeInfoRow.FROMACCOUNTNAME
        Else
            'DBNULLならハイフンで表示
            strStaffName = WebWordUtility.GetWord(7)
        End If

        '※DBNULLならEMPTYを返却する共通処理でDBNULLチェック START
        '顧客名
        Dim strCustName As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("CUSTOMNAME"))
        '顧客コード
        Dim strCustCD As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("CRCUSTID"))
        '顧客分類
        Dim strCustClass As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("CUSTOMERCLASS"))
        '顧客種別
        Dim strCustKind As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("CSTKIND"))
        '通知者アカウント
        Dim strStaffAccount As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("FROMACCOUNT"))
        'スタッフアイコンファイル名
        Dim strStaffIcon As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("ICON_IMGFILE"))
        '顧客担当セールススタッフコード
        Dim strSalesStaffCD As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("SALESSTAFFCD"))
        'Follow-up Box店舗コード
        Dim strFllwUpBoxStrCD As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("FLLWUPBOXSTRCD"))
        '最終ステータス
        Dim strLastStatus As String = Me.GetDbNullCheckedString(noticeInfoRow.Item("STATUS"))
        '※DBNULLならEMPTYを返却する共通処理でDBNULLチェック END

        '通知の下半分に表示するメッセージを取得する
        ' 2013/12/02 TCS 森   Aカード情報相互連携開発 START
        Dim arrMessage As String() = Me.GetDispMessage(strNoticeReqCtg, helpInfoDt, discountApprovalDt, estimateDt)
        ' 2013/12/02 TCS 森   Aカード情報相互連携開発 END
        Dim strMessage1 As String = arrMessage(0)   'メッセージ１
        Dim strMessage2 As String = arrMessage(1)   'メッセージ２

        'データセット内の返却用テーブルにデータ行を追加する
        ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        rtnDs.SC3040802ReturnNoticeInfo.AddSC3040802ReturnNoticeInfoRow(intNoticeReqId, strNoticeReqCtg, intReqClassId, _
                                                                        strCustCD, strCustClass, strCustKind, strStaffIcon, _
                                                                        strStaffName, strStaffAccount, strCustName, strTimeMsg, _
                                                                        strMessage1, strMessage2, strSalesStaffCD, strFllwUpBoxStrCD, _
                                                                        intFllwUpBox, strLastStatus, loyalCustomerFlg)
        ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        Logger.Info(System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' DBNULLチェックをした値を返却する（String）
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDbNullCheckedString(ByVal objColumn As Object) As String

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) Then
            rtnVal = CStr(objColumn)
        End If

        Return rtnVal

    End Function

    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 重要車両フラグを取得する
    ''' </summary>
    ''' <param name="dlrCD"></param>
    ''' <param name="cstID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetImpVclFlgDt(ByVal dlrCD As String, ByVal cstID As String) As SC3040802DataSet.SC3040802ImpVclFlgDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start[dlrCD:{0}][cstID:{1}]", dlrCD, cstID))

        Dim ta As New SC3040802TableAdapter
        Dim rtnDt As SC3040802DataSet.SC3040802ImpVclFlgDataTable = Nothing
        Dim parsedCstID As Decimal

        If Decimal.TryParse(cstID, parsedCstID) Then
            '検索処理
            rtnDt = ta.GetImpVclFlg(dlrCD, parsedCstID)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

        Return rtnDt

    End Function
    ' 2018/07/19 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

#End Region

End Class
