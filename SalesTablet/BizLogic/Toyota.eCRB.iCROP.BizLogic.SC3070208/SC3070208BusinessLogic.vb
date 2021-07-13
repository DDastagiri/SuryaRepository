'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070208BusinessLogic.vb
'─────────────────────────────────────
'機能： 注文承認依頼
'補足： 
'作成： 2013/11/26 TCS 山口   Aカード情報相互連携開発
'更新： 2014/05/28 TCS 安田   受注時説明機能開発（受注後工程スケジュール）
'更新： 2015/03/17 TCS 鈴木   次世代e-CRB 価格相談履歴参照機能開発
'更新： 2015/03/20 TCS 藤井   セールスタブレット：0118
'更新： 2017/12/20 TCS 河原   TKM独自機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)
'─────────────────────────────────────


Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports System.Globalization
Imports System.Reflection.MethodBase

'2015/03/20 TCS 藤井 セールスタブレット：0118 ADD START
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
'2015/03/20 TCS 藤井 セールスタブレット：0118 ADD END
''' <summary>
''' SC3070208(注文承認依頼)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3070208BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' ログ出力メッセージ1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_MSG1 As String = "NoticeRequestI/F Error : ReturnId = "

    ''' <summary>
    ''' 文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MsgId901 As Integer = 901 '新しい要求を受けました。
    Public Const MsgId902 As Integer = 902 '要求はキャンセルされました。
    Public Const MsgId903 As Integer = 903 '承認もしくは否認済みなので、注文承認依頼をキャンセルすることができません。
    Public Const MsgId904 As Integer = 904 '注文承認依頼に失敗しました。
    Public Const MsgId905 As Integer = 905 '注文承認依頼のキャンセルに失敗しました。
    Public Const MsgId906 As Integer = 906 'ファーストネームが入力されていません。
    Public Const MsgId907 As Integer = 907 'ミドルネームが入力されていません。
    Public Const MsgId908 As Integer = 908 'ラストネームが入力されていません。
    Public Const MsgId909 As Integer = 909 '性別が入力されていません。
    Public Const MsgId910 As Integer = 910 '敬称が入力されていません。
    Public Const MsgId911 As Integer = 911 '個人/法人が入力されていません。
    Public Const MsgId912 As Integer = 912 '個人法人項目が入力されていません。
    Public Const MsgId913 As Integer = 913 '担当者氏名(法人)が入力されていません。
    Public Const MsgId914 As Integer = 914 '担当者部署名(法人)が入力されていません。
    Public Const MsgId915 As Integer = 915 '役職(法人)が入力されていません。
    Public Const MsgId916 As Integer = 916 '携帯番号が入力されていません。
    Public Const MsgId917 As Integer = 917 '自宅電話番号が入力されていません。
    Public Const MsgId918 As Integer = 918 '勤務先電話番号が入力されていません。
    Public Const MsgId919 As Integer = 919 '自宅FAX番号が入力されていません。
    Public Const MsgId920 As Integer = 920 '郵便番号が入力されていません。
    Public Const MsgId921 As Integer = 921 '住所1が入力されていません。
    Public Const MsgId922 As Integer = 922 '住所2が入力されていません。
    Public Const MsgId923 As Integer = 923 '住所3が入力されていません。
    Public Const MsgId924 As Integer = 924 '住所(州)が入力されていません。
    Public Const MsgId925 As Integer = 925 '住所(地域)が入力されていません。
    Public Const MsgId926 As Integer = 926 '住所(市)が入力されていません。
    Public Const MsgId927 As Integer = 927 '住所(地区)が入力されていません。
    Public Const MsgId928 As Integer = 928 '本籍が入力されていません。
    Public Const MsgId929 As Integer = 929 'e-Mail1が入力されていません。
    Public Const MsgId930 As Integer = 930 'e-Mail2が入力されていません。
    Public Const MsgId931 As Integer = 931 '国籍が入力されていません。
    Public Const MsgId932 As Integer = 932 '国民IDが入力されていません。
    Public Const MsgId933 As Integer = 933 '誕生日が入力されていません。
    Public Const MsgId934 As Integer = 934 '活動区分が入力されていません。
    Public Const MsgId935 As Integer = 935 '希望車種グレードが入力されていません。
    Public Const MsgId936 As Integer = 936 '希望車種カラーが入力されていません。
    Public Const MsgId937 As Integer = 937 'ソース選択が入力されていません。
    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
    Public Const MsgId939 As Integer = 939 '{0}が入力されていません。
    Public Const MsgId940 As Integer = 940 '携帯番号または自宅電話番号が入力されていません。
    Public Const MsgId941 As Integer = 941 '承認済みなので、注文承認依頼をすることができません。
    '2017/12/20 TCS 河原 TKM独自機能開発 START
    Public Const MsgId946 As Integer = 946 '希望車種サフィックスが入力されていません。
    Public Const MsgId947 As Integer = 947 '希望車種内装色が入力されていません。
    '2017/12/20 TCS 河原 TKM独自機能開発 END

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    Public Const MsgId942 As Integer = 942 'Chg Est.
    Public Const MsgId943 As Integer = 943 'Chg Plan.
    Public Const MsgId944 As Integer = 944 '受注後工程未実施

    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    Public Const MsgId10901 As Integer = 10901 '顧客組織が入力されていません。
    Public Const MsgId10902 As Integer = 10902 'サブカテゴリ2が入力されていません。
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
    Public Const MsgId10903 As Integer = 10903 'ソース選択2が入力されていません。
    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END

    ''' <summary>
    ''' 成約活動コード：システム設定の成約活動コードキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AFTER_ODR_ACT_CD_CONTRACT As String = "AFTER_ODR_ACT_CD_CONTRACT"

    ''' <summary>
    ''' 見積変更フラグがON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EST_CHG_FLG_ON As String = "1"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

    ''' <summary>
    ''' 対象項目ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TGT_ITEM_ID_FIRST_NAME As String = "01" 'ファーストネーム
    Private Const TGT_ITEM_ID_MIDDLE_NAME As String = "02" 'ミドルネーム
    Private Const TGT_ITEM_ID_LAST_NAME As String = "03" 'ラストネーム
    Private Const TGT_ITEM_ID_CST_GENDER As String = "04" '性別
    Private Const TGT_ITEM_ID_NAMETITLE_CD As String = "05" '敬称
    Private Const TGT_ITEM_ID_FLEET_FLG As String = "06" '個人/法人
    Private Const TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD As String = "07" '個人法人項目
    Private Const TGT_ITEM_ID_FLEET_PIC_NAME As String = "08" '担当者氏名(法人)
    Private Const TGT_ITEM_ID_FLEET_PIC_DEPT As String = "09" '担当者部署名(法人)
    Private Const TGT_ITEM_ID_FLEET_PIC_POSITION As String = "10" '役職(法人)
    Private Const TGT_ITEM_ID_CST_MOBILE As String = "11" '携帯番号
    Private Const TGT_ITEM_ID_CST_PHONE As String = "12" '自宅電話番号
    Private Const TGT_ITEM_ID_CST_BIZ_PHONE As String = "13" '勤務先電話番号
    Private Const TGT_ITEM_ID_CST_FAX As String = "14" '自宅FAX番号
    Private Const TGT_ITEM_ID_CST_ZIPCD As String = "15" '郵便番号
    Private Const TGT_ITEM_ID_CST_ADDRESS_1 As String = "16" '住所1
    Private Const TGT_ITEM_ID_CST_ADDRESS_2 As String = "17" '住所2
    Private Const TGT_ITEM_ID_CST_ADDRESS_3 As String = "18" '住所3
    Private Const TGT_ITEM_ID_CST_ADDRESS_STATE As String = "19" '住所(州)
    Private Const TGT_ITEM_ID_CST_ADDRESS_LOCATION As String = "20" '住所(地域)
    Private Const TGT_ITEM_ID_CST_ADDRESS_CITY As String = "21" '住所(市)
    Private Const TGT_ITEM_ID_CST_ADDRESS_DISTRICT As String = "22" '住所(地区)
    Private Const TGT_ITEM_ID_CST_DOMICILE As String = "23" '本籍
    Private Const TGT_ITEM_ID_CST_EMAIL_1 As String = "24" 'e-Mail1
    Private Const TGT_ITEM_ID_CST_EMAIL_2 As String = "25" 'e-Mail2
    Private Const TGT_ITEM_ID_CST_COUNTRY As String = "26" '国籍
    Private Const TGT_ITEM_ID_CST_SOCIALNUM As String = "27" '国民ID
    Private Const TGT_ITEM_ID_CST_BIRTH_DATE As String = "28" '誕生日
    Private Const TGT_ITEM_ID_ACT_CAT_TYPE As String = "29" '活動区分
    Private Const TGT_ITEM_ID_GRADE_CD As String = "31" '希望車種グレード
    Private Const TGT_ITEM_ID_BODYCLR_CD As String = "32" '希望車種カラー
    '2017/12/20 TCS 河原 TKM独自機能開発 START
    Private Const TGT_ITEM_ID_SUFFIX_CD As String = "37" '希望車種サフィックス
    Private Const TGT_ITEM_ID_INTERIOR_BODYCLR_CD As String = "38" '希望車種内装色
    '2017/12/20 TCS 河原 TKM独自機能開発 END
    Private Const TGT_ITEM_ID_SOURCE_1_CD As String = "33" 'ソース選択
    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
    Private Const TGT_ITEM_ID_SALESCONDITIONNO As String = "35" '商談条件
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const TGT_ITEM_ID_CST_TYPE As String = "L01" '顧客種別
    Private Const TGT_ITEM_ID_CST_ORGNZ_CD As String = "L02" '顧客組織コード
    Private Const TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE As String = "L03" '顧客組織入力区分
    Private Const TGT_ITEM_ID_CST_ORGNZ_NAME As String = "L04" '顧客組織名称
    Private Const TGT_ITEM_ID_CST_SUBCAT2_CD As String = "L05" 'サブカテゴリ2コード
    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
    Private Const TGT_ITEM_ID_SOURCE_2_CD As String = "L06" 'ソース選択2
    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    Private Enum CstType
        ''' <summary>未取引客</summary>
        NonTrading = 2
    End Enum

    ''' <summary>
    ''' 顧客組織入力区分
    ''' </summary>
    Private Enum CstOrgnzInputType
        ''' <summary>マスタから選択</summary>
        FromMaster = 1
        ''' <summary>手入力</summary>
        Manual = 2
    End Enum
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum RequestTypeEnum
        Request
        Cancel
    End Enum

    ''' <summary>
    ''' 法人フラグ：法人
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FLEET_FLG_FLEET As String = "1"

#Region "通知依頼IF用の定数"
    ''' <summary>
    ''' 通知 000000:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESULTID_SUCCESS As String = "000000"

    ''' <summary>
    ''' 依頼種別（契約承認）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ORDER_APPROVAL As String = "08"

    ''' <summary>
    ''' 依頼種別（注文情報登録・変更）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_SALES_UPDATE As String = "09"

    ''' <summary>
    ''' ステータス（依頼）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_REQUEST As String = "1"

    ''' <summary>
    ''' ステータス（キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_CANCEL As String = "2"

    ''' <summary>
    ''' (カテゴリータイプ)  : Popup
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PUSHCATEGORY_POPUP As String = "1"

    ''' <summary>
    ''' (表示位置) : header
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_POSITION_HEADER As String = "1"

    ''' <summary>
    ''' (表示時間) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_TIME As Long = 3

    ''' <summary>
    ''' (表示タイプ) : Text()
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_TYPE_TEXT As String = "1"

    ''' <summary>
    ''' (色) : 薄い黄色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_COLOR As String = "1"

    ''' <summary>
    ''' (表示時関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_FUNCTION As String = "icropScript.ui.openNoticeList()"

    ''' <summary>
    ''' (アクション時関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ACTION_FUNCTION As String = "icropScript.ui.openNoticeList()"
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Private _msgId As Integer = 0

    ''' <summary>
    ''' メッセージ(置換用)
    ''' </summary>
    ''' <remarks></remarks>
    Private _msg As String = String.Empty

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' メッセージ出力フラグ（更新が成功した場合でもメッセージを出力させたいため）
    ''' </summary>
    ''' <remarks></remarks>
    Private _msgOutFlg As Boolean = False
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END


#End Region

#Region "プロパティ"
    Dim MsgId950 As Object

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <value>メッセージID</value>
    ''' <returns></returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property MsgId() As Integer
        Get
            Return Me._msgId
        End Get
    End Property

    ''' <summary>
    ''' メッセージ(置換用)
    ''' </summary>
    ''' <value>メッセージ</value>
    ''' <returns></returns>
    ''' <remarks>ブランクの場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property Msg() As String
        Get
            Return Me._msg
        End Get
    End Property

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' メッセージ出力フラグ
    ''' </summary>
    ''' <value>メッセージフラグ</value>
    ''' <returns></returns>
    ''' <remarks>メッセージ出力フラグ（更新が成功した場合でもメッセージを出力させたいため）</remarks>
    Public ReadOnly Property MsgOutFlg() As Boolean
        Get
            Return Me._msgOutFlg
        End Get
    End Property
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
#End Region

#End Region

#Region "Public"
    ''' <summary>
    ''' 注文承認スタッフ一覧取得
    ''' </summary>
    ''' <returns>ApprovalStaffListDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetStaffList() As SC3070208DataSet.SC3070208ApprovalStaffListDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim staffListDataTable As SC3070208DataSet.SC3070208ApprovalStaffListDataTable = Nothing
        Dim staffList = New List(Of String)

        '検索用スタッフリストを取得
        staffList = StaffContext.GetMySuperiors(StaffContext.Current.DlrCD, _
                                                StaffContext.Current.BrnCD, _
                                                StaffContext.Current.OpeCD, _
                                                StaffContext.Current.TeamLeader, _
                                                StaffContext.Current.Account)
        'スタッフリストに自分を追加(スタッフリストが0件の場合、SQLのインデックスが効かなくなるため)
        staffList.Add(StaffContext.Current.Account)

        staffListDataTable = SC3070208TableAdapter.GetApprovalStaffList(staffList, StaffContext.Current.Account)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        Return staffListDataTable
    End Function

    ''' <summary>
    ''' 注文承認依頼取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>ContractApprovalDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetContractApproval(ByVal estimateId As Long) As SC3070208DataSet.SC3070208ContractApprovalDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        '注文承認依頼取得
        Return SC3070208TableAdapter.GetContractApproval(estimateId)

    End Function

    ''' <summary>
    ''' 注文承認依頼登録
    ''' </summary>
    ''' <param name="parameter">パラメータDataTable</param>
    ''' <returns>処理結果 正常終了はTrue</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function InsertContractApproval(ByVal parameter As SC3070208DataSet.SC3070208ParameterDataTable) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim drParameter As SC3070208DataSet.SC3070208ParameterRow
        drParameter = CType(parameter.Rows(0), SC3070208DataSet.SC3070208ParameterRow)

        '各種入力チェック
        Dim errorItem As String = InputCheck(drParameter)

        'エラーありの場合
        If String.IsNullOrWhiteSpace(errorItem) = False Then
            'エラーメッセージID取得
            Dim msgIdList As Dictionary(Of String, Integer) = GetErrorMsgNo()
            Me._msgId = msgIdList.Item(errorItem)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
            Return False
        End If

        Try
            '処理結果
            Dim result As Integer = 0

            '見積情報更新ロック取得
            Dim dtLockEstimateInfo As SC3070208DataSet.SC3070208LockEstimateInfoDataTable = _
                SC3070208TableAdapter.GetLockEstimateInfo(drParameter.FLLWUPBOX_SEQNO)

            If dtLockEstimateInfo.Rows.Count = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId904
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            Dim drLockEstimateInfo As SC3070208DataSet.SC3070208LockEstimateInfoRow() _
                = CType(dtLockEstimateInfo.Select("ESTIMATEID = " & drParameter.ESTIMATEID), SC3070208DataSet.SC3070208LockEstimateInfoRow())
            Dim status As String = drLockEstimateInfo(0).CONTRACT_APPROVAL_STATUS
            'ステータスが承認の場合、処理終了
            If SC3070208TableAdapter.StatusApproval.Equals(status) Then
                Me._msgId = MsgId941
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '承認ステータス更新
            result = SC3070208TableAdapter.UpdateContractApprovalStatus(drParameter.ESTIMATEID, _
                                                                        SC3070208TableAdapter.StatusApprovalRequest, _
                                                                        drParameter.ACCOUNT, _
                                                                        drParameter.TOACCOUNT, _
                                                                        drParameter.ACCOUNT, _
                                                                        drParameter.DLR_CD)
            If result = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId904
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
            '契約承認テーブル登録
            Dim seqno As Long = SC3070208TableAdapter.SelectSequence()
            result = SC3070208TableAdapter.InsertContractApproval(drParameter.ESTIMATEID, _
                                                seqno, _
                                                drParameter.DLR_CD, _
                                                drParameter.BRN_CD, _
                                                drParameter.ACCOUNT, _
                                                drParameter.STAFFMEMO, _
                                                drParameter.TOACCOUNT)
            If result <> 1 Then
                Me.Rollback = True
                Me._msgId = MsgId904
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If
            '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            'キャンセル通知対象データ取得
            Dim dtNoticeRequest As SC3070208DataSet.SC3070208NoticeRequestDataTable = _
                                SC3070208TableAdapter.GetNoticeRequest(drParameter.FLLWUPBOX_SEQNO)



            Dim returnXmlNotice As XmlCommon
            For Each drNoticeRequest In dtNoticeRequest
                '価格相談依頼キャンセル処理
                returnXmlNotice = NoticeRequest(drNoticeRequest.TOACCOUNT, _
                                                drNoticeRequest.USERNAME, _
                                                RequestTypeEnum.Cancel, _
                                                drNoticeRequest.NOTICEREQID, _
                                                drNoticeRequest.REQCLASSID, _
                                                drParameter, _
                                                drNoticeRequest.NOTICEREQCTG)

                If RESULTID_SUCCESS.Equals(returnXmlNotice.ResultId) = False Then
                    '通知が失敗した場合
                    Me.Rollback = True
                    Me._msgId = MsgId904
                    Logger.Error(ERROR_MSG1 & returnXmlNotice.ResultId)
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
                    Return False
                End If
            Next

            'その他見積り情報削除
            SC3070208TableAdapter.DeleteOtherEstimate(drParameter.FLLWUPBOX_SEQNO, _
                                                      drParameter.ESTIMATEID, _
                                                      drParameter.ACCOUNT)

            '依頼先がログインユーザーの場合、通知を行わない
            If Not drParameter.ACCOUNT.Equals(drParameter.TOACCOUNT) Then
                '注文承認依頼処理
                returnXmlNotice = NoticeRequest(drParameter.TOACCOUNT, _
                                                drParameter.TOACCOUNTNAME, _
                                                RequestTypeEnum.Request, _
                                                0, _
                                                drParameter.ESTIMATEID, _
                                                drParameter)

                If RESULTID_SUCCESS.Equals(returnXmlNotice.ResultId) = False Then
                    '通知が失敗した場合
                    Me.Rollback = True
                    Me._msgId = MsgId904
                    Logger.Error(ERROR_MSG1 & returnXmlNotice.ResultId)
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
                    Return False
                End If

                '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                Dim NoticeRequestId As Long = returnXmlNotice.NoticeRequestId
                '通知依頼情報テーブル（通知依頼ID）更新
                Dim updateCount As Integer
                updateCount = SC3070208TableAdapter.UpdateNoticeid(drParameter.ESTIMATEID, _
                                                seqno, _
                                                drParameter.ACCOUNT, _
                                                NoticeRequestId)
                '2015/03/16 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END

            End If

            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
            '見積変更フラグの場合
            Me._msgOutFlg = False
            Dim estChgFlg As String = drLockEstimateInfo(0).CONTRACT_COND_CHG_FLG
            If EST_CHG_FLG_ON.Equals(estChgFlg) Then

                '契約条件の変更
                Me._msgId = MsgId942
                Me._msgOutFlg = True

            Else

                '受注後.モデルコード　≠　見積車両情報.モデルコード
                '受注後活動.予定開始日時（受注時活動コード = 「ご契約」のレコード）　≠　当日（システム日付）

                Dim sysEnvVal As String = GetSysEnvSettingValue(AFTER_ODR_ACT_CD_CONTRACT)

                If (Not String.IsNullOrEmpty(sysEnvVal)) Then

                    Dim afterOdr As SC3070208DataSet.SC3070208AfterOderDataTable =
                        SC3070208TableAdapter.GetAfterOder(drParameter.FLLWUPBOX_SEQNO, sysEnvVal)

                    If (afterOdr.Count() > 0) Then

                        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                        Logger.Info("10.DateTimeFunc.Now()" & DateTimeFunc.Now().ToString("yyyyMMdd HHmmss", CultureInfo.InvariantCulture))

                        Dim nowDay As String = DateTimeFunc.Now().ToString("yyyyMMdd", CultureInfo.InvariantCulture)
                        Dim echeDay As String = afterOdr.Item(0).SCHE_START_DATEORTIME.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
                        If (Not afterOdr.Item(0).ODR_MODEL_CD.Equals(afterOdr.Item(0).EST_MODEL_CD)) OrElse _
                           (Not nowDay.Equals(echeDay)) Then

                            '前提条件の変更
                            Me._msgId = MsgId943
                            Me._msgOutFlg = True

                            Logger.Info("11.前提条件の変更")
                            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

                        End If

                    End If
                End If

            End If
            '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            '正常終了
            Return True

        Catch ex As OracleExceptionEx
            Me.Rollback = True
            Me._msgId = MsgId904
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 注文承認依頼キャンセル
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <param name="parameter">パラメータDataTable</param>
    ''' <returns>処理結果 正常終了はTrue</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function CancelContractApproval(ByVal estimateId As Long, _
                                           ByVal noticeReqId As Long, _
                                           ByVal parameter As SC3070208DataSet.SC3070208ParameterDataTable) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim drParameter As SC3070208DataSet.SC3070208ParameterRow
        drParameter = CType(parameter.Rows(0), SC3070208DataSet.SC3070208ParameterRow)

        Try
            '処理結果
            Dim result As Integer = 0

            '見積情報更新ロック取得
            Dim dtLockEstimateInfo As SC3070208DataSet.SC3070208LockEstimateInfoDataTable = _
                SC3070208TableAdapter.GetLockEstimateInfo(drParameter.FLLWUPBOX_SEQNO)
            If dtLockEstimateInfo.Rows.Count = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId905
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            Dim drLockEstimateInfo As SC3070208DataSet.SC3070208LockEstimateInfoRow() _
                = CType(dtLockEstimateInfo.Select("ESTIMATEID = " & estimateId), SC3070208DataSet.SC3070208LockEstimateInfoRow())
            Dim status As String = drLockEstimateInfo(0).CONTRACT_APPROVAL_STATUS
            'ステータスが承認または否認の場合、処理終了
            If SC3070208TableAdapter.StatusApproval.Equals(status) Or SC3070208TableAdapter.StatusDenial.Equals(status) Then
                Me._msgId = MsgId903
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '承認ステータス更新
            result = SC3070208TableAdapter.UpdateContractApprovalStatus(estimateId, _
                                                                        SC3070208TableAdapter.StatusAnapproved, _
                                                                        drParameter.ACCOUNT)
            If result = 0 Then
                Me.Rollback = True
                Me._msgId = MsgId905
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                Return False
            End If

            '依頼先がログインユーザーの場合、通知を行わない
            If Not drParameter.ACCOUNT.Equals(drParameter.TOACCOUNT) Then
                Dim returnXmlNotice As XmlCommon
                '価格相談依頼キャンセル処理
                returnXmlNotice = NoticeRequest(drParameter.TOACCOUNT, _
                                  drParameter.TOACCOUNTNAME, _
                                  RequestTypeEnum.Cancel, _
                                  noticeReqId, _
                                  estimateId, _
                                  drParameter)

                If RESULTID_SUCCESS.Equals(returnXmlNotice.ResultId) = False Then
                    '通知が失敗した場合
                    Me.Rollback = True
                    Me._msgId = MsgId905
                    Logger.Error(ERROR_MSG1 & returnXmlNotice.ResultId)
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
                    Return False
                End If
            Else
                '2015/03/17 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD START
                'Self承認をキャンセルした場合は、契約承認テーブルから履歴を削除する
                SC3070208TableAdapter.UndoContractApproval(estimateId)
                '2015/03/17 TCS 鈴木【TMT課題対応(#72 セールスタブレットの価格相談履歴表示)】ADD END
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

            '正常終了
            Return True
        Catch ex As OracleExceptionEx
            Me.Rollback = True
            Me._msgId = MsgId905
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name))
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
            Return False
        End Try
    End Function
#End Region

#Region "Private"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' システム設定値を取得する
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing

        env = New SystemEnvSetting()
        dr = env.GetSystemEnvSetting(sysEnvName)
        If Not dr Is Nothing Then
            Return dr.PARAMVALUE.Trim()
        End If

        Return String.Empty

    End Function
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

    ''' <summary>
    ''' 入力チェック用リスト作成
    ''' </summary>
    ''' <returns>入力チェック用リスト</returns>
    ''' <remarks></remarks>
    Private Function editCheckItemList() As Dictionary(Of String, String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim checkItemList As New Dictionary(Of String, String)

        With checkItemList
            '顧客
            Using dtCheckCustomer As New SC3070208DataSet.SC3070208InputCheckCustomerDataTable
                'ファーストネーム
                .Add(TGT_ITEM_ID_FIRST_NAME, dtCheckCustomer.FIRST_NAMEColumn.ColumnName)
                'ミドルネーム
                .Add(TGT_ITEM_ID_MIDDLE_NAME, dtCheckCustomer.MIDDLE_NAMEColumn.ColumnName)
                'ラストネーム
                .Add(TGT_ITEM_ID_LAST_NAME, dtCheckCustomer.LAST_NAMEColumn.ColumnName)
                '性別
                .Add(TGT_ITEM_ID_CST_GENDER, dtCheckCustomer.CST_GENDERColumn.ColumnName)
                '敬称
                .Add(TGT_ITEM_ID_NAMETITLE_CD, dtCheckCustomer.NAMETITLE_CDColumn.ColumnName)
                '個人/法人
                .Add(TGT_ITEM_ID_FLEET_FLG, dtCheckCustomer.FLEET_FLGColumn.ColumnName)
                '個人法人項目
                .Add(TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD, dtCheckCustomer.PRIVATE_FLEET_ITEM_CDColumn.ColumnName)
                '担当者氏名(法人)
                .Add(TGT_ITEM_ID_FLEET_PIC_NAME, dtCheckCustomer.FLEET_PIC_NAMEColumn.ColumnName)
                '担当者部署名(法人)
                .Add(TGT_ITEM_ID_FLEET_PIC_DEPT, dtCheckCustomer.FLEET_PIC_DEPTColumn.ColumnName)
                '役職(法人)
                .Add(TGT_ITEM_ID_FLEET_PIC_POSITION, dtCheckCustomer.FLEET_PIC_POSITIONColumn.ColumnName)
                '携帯番号
                .Add(TGT_ITEM_ID_CST_MOBILE, dtCheckCustomer.CST_MOBILEColumn.ColumnName)
                '自宅電話番号
                .Add(TGT_ITEM_ID_CST_PHONE, dtCheckCustomer.CST_PHONEColumn.ColumnName)
                '勤務先電話番号
                .Add(TGT_ITEM_ID_CST_BIZ_PHONE, dtCheckCustomer.CST_BIZ_PHONEColumn.ColumnName)
                '自宅FAX番号
                .Add(TGT_ITEM_ID_CST_FAX, dtCheckCustomer.CST_FAXColumn.ColumnName)
                '郵便番号
                .Add(TGT_ITEM_ID_CST_ZIPCD, dtCheckCustomer.CST_ZIPCDColumn.ColumnName)
                '住所1
                .Add(TGT_ITEM_ID_CST_ADDRESS_1, dtCheckCustomer.CST_ADDRESS_1Column.ColumnName)
                '住所2
                .Add(TGT_ITEM_ID_CST_ADDRESS_2, dtCheckCustomer.CST_ADDRESS_2Column.ColumnName)
                '住所3
                .Add(TGT_ITEM_ID_CST_ADDRESS_3, dtCheckCustomer.CST_ADDRESS_3Column.ColumnName)
                '住所(州)
                .Add(TGT_ITEM_ID_CST_ADDRESS_STATE, dtCheckCustomer.CST_ADDRESS_STATEColumn.ColumnName)
                '住所(地域)
                .Add(TGT_ITEM_ID_CST_ADDRESS_LOCATION, dtCheckCustomer.CST_ADDRESS_LOCATIONColumn.ColumnName)
                '住所(市)
                .Add(TGT_ITEM_ID_CST_ADDRESS_CITY, dtCheckCustomer.CST_ADDRESS_CITYColumn.ColumnName)
                '住所(地区)
                .Add(TGT_ITEM_ID_CST_ADDRESS_DISTRICT, dtCheckCustomer.CST_ADDRESS_DISTRICTColumn.ColumnName)
                '本籍
                .Add(TGT_ITEM_ID_CST_DOMICILE, dtCheckCustomer.CST_DOMICILEColumn.ColumnName)
                'e-Mail1
                .Add(TGT_ITEM_ID_CST_EMAIL_1, dtCheckCustomer.CST_EMAIL_1Column.ColumnName)
                'e-Mail2
                .Add(TGT_ITEM_ID_CST_EMAIL_2, dtCheckCustomer.CST_EMAIL_2Column.ColumnName)
                '国籍
                .Add(TGT_ITEM_ID_CST_COUNTRY, dtCheckCustomer.CST_COUNTRYColumn.ColumnName)
                '国民ID
                .Add(TGT_ITEM_ID_CST_SOCIALNUM, dtCheckCustomer.CST_SOCIALNUMColumn.ColumnName)
                '誕生日
                .Add(TGT_ITEM_ID_CST_BIRTH_DATE, dtCheckCustomer.CST_BIRTH_DATEColumn.ColumnName)
                '活動区分
                .Add(TGT_ITEM_ID_ACT_CAT_TYPE, dtCheckCustomer.ACT_CAT_TYPEColumn.ColumnName)
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                '顧客種別
                .Add(TGT_ITEM_ID_CST_TYPE, dtCheckCustomer.CST_TYPEColumn.ColumnName)
                '顧客組織コード
                .Add(TGT_ITEM_ID_CST_ORGNZ_CD, dtCheckCustomer.CST_ORGNZ_CDColumn.ColumnName)
                '顧客組織入力区分
                .Add(TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE, dtCheckCustomer.CST_ORGNZ_INPUT_TYPEColumn.ColumnName)
                '顧客組織名称
                .Add(TGT_ITEM_ID_CST_ORGNZ_NAME, dtCheckCustomer.CST_ORGNZ_NAMEColumn.ColumnName)
                'サブカテゴリ2コード
                .Add(TGT_ITEM_ID_CST_SUBCAT2_CD, dtCheckCustomer.CST_SUBCAT2_CDColumn.ColumnName)
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            End Using
            '2017/12/20 TCS 河原 TKM独自機能開発 START
            '希望車
            Using dtCheckSelectedCar As New SC3070208DataSet.SC3070208InputCheckSelectedCarDataTable
                '希望車種グレード
                .Add(TGT_ITEM_ID_GRADE_CD, dtCheckSelectedCar.GRADE_CDColumn.ColumnName)
                '希望車種サフィックス
                .Add(TGT_ITEM_ID_SUFFIX_CD, dtCheckSelectedCar.SUFFIX_CDColumn.ColumnName)
                '希望車種外装色
                .Add(TGT_ITEM_ID_BODYCLR_CD, dtCheckSelectedCar.BODYCLR_CDColumn.ColumnName)
                '希望車種内装色
                .Add(TGT_ITEM_ID_INTERIOR_BODYCLR_CD, dtCheckSelectedCar.INTERIORCLR_CDColumn.ColumnName)
            End Using
            '2017/12/20 TCS 河原 TKM独自機能開発 END
            '商談
            Using dtCheckSales As New SC3070208DataSet.SC3070208InputCheckSalesDataTable
                'ソース選択
                .Add(TGT_ITEM_ID_SOURCE_1_CD, dtCheckSales.SOURCE_1_CDColumn.ColumnName)
                '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)START
                'ソース選択2
                .Add(TGT_ITEM_ID_SOURCE_2_CD, dtCheckSales.SOURCE_2_CDColumn.ColumnName)
                '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)END
                '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
            End Using
            '商談条件
            Using dtCheckSalesCondition As New SC3070208DataSet.SC3070208InputCheckSalesConditionDataTable
                '商談条件
                .Add(TGT_ITEM_ID_SALESCONDITIONNO, dtCheckSalesCondition.SALESCONDITIONNOColumn.ColumnName)
            End Using
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        Return checkItemList
    End Function

    '2017/12/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 入力項目設定マスタによるチェックを実施
    ''' </summary>
    ''' <param name="parameter">パラメータDataRow</param>
    ''' <returns>対象項目ID(正常終了の場合ブランク)</returns>
    ''' <remarks></remarks>
    Private Function InputCheck(ByVal parameter As SC3070208DataSet.SC3070208ParameterRow) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        '入力項目設定マスタ取得
        Dim dtItemSetting As SC3070208DataSet.SC3070208InputItemSettingDataTable = _
                                SC3070208TableAdapter.GetInputItemSetting()

        '入力チェック用情報取得（顧客）
        Dim dtCheckCustomer As SC3070208DataSet.SC3070208InputCheckCustomerDataTable = _
                                SC3070208TableAdapter.GetInputCheckCustomer(parameter.DLR_CD, CType(Trim(parameter.CST_ID), Decimal))

        '入力チェック用情報取得（商談）
        Dim dtCheckSales As SC3070208DataSet.SC3070208InputCheckSalesDataTable = _
                                SC3070208TableAdapter.GetInputCheckSales(parameter.FLLWUPBOX_SEQNO)

        '入力チェック用情報取得（希望車）
        Dim dtCheckSelectedCar As SC3070208DataSet.SC3070208InputCheckSelectedCarDataTable = _
                                SC3070208TableAdapter.GetInputCheckSelectedCar(parameter.FLLWUPBOX_SEQNO)

        '入力チェック用情報取得（商談条件）
        Dim dtCheckSalesCondition As SC3070208DataSet.SC3070208InputCheckSalesConditionDataTable = _
                                SC3070208TableAdapter.GetInputCheckSalesCondition(parameter.FLLWUPBOX_SEQNO)

        '入力チェック実施
        Dim errorItem As String = String.Empty
        Dim checkItemList As Dictionary(Of String, String) = editCheckItemList()
        '入力項目設定マスタの件数分ループ
        For Each drItemSetting In dtItemSetting
            Select Case drItemSetting.TGT_ITEM_ID
                Case TGT_ITEM_ID_GRADE_CD, TGT_ITEM_ID_BODYCLR_CD, TGT_ITEM_ID_SUFFIX_CD, TGT_ITEM_ID_INTERIOR_BODYCLR_CD
                    '希望車種グレード、希望車種サフィックス、希望車種外装色、希望車種内装色
                    For Each drCheckSelectedCar In dtCheckSelectedCar
                        If String.IsNullOrWhiteSpace(drCheckSelectedCar.Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)).ToString()) Then
                            'エラー
                            errorItem = drItemSetting.TGT_ITEM_ID
                            Exit For
                        End If
                    Next
                    If String.IsNullOrEmpty(errorItem) = False Then
                        Exit For
                    End If
                    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
                Case TGT_ITEM_ID_SOURCE_1_CD
                    'ソース選択
                    '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END
                    If CType(dtCheckSales.Rows(0).Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)), Long) = 0 Then
                        'エラー
                        errorItem = drItemSetting.TGT_ITEM_ID
                        Exit For
                    End If
                Case TGT_ITEM_ID_SALESCONDITIONNO
                    '商談条件
                    '2015/03/20 TCS 藤井 セールスタブレット：0118 ADD START
                    '商談条件チェックは受注前、受注時に実施
                    Using dt As New ActivityInfoDataSet.ActivityInfoCountFromDataTable()
                        dt.AddActivityInfoCountFromRow(parameter.DLR_CD, parameter.BRN_CD, parameter.FLLWUPBOX_SEQNO)
                        Dim salesafterflg As String = ActivityInfoBusinessLogic.CountFllwupboxRslt(dt)

                        If Not salesafterflg.Equals(ActivityInfoBusinessLogic.SALESAFTER_YES) Then
                            '2015/03/20 TCS 藤井 セールスタブレット：0118 ADD END
                            If dtCheckSalesCondition.Select("SALESCONDITIONNO = " & drItemSetting.TGT_ITEM_DETAIL_ID).Count() = 0 Then
                                'エラー
                                errorItem = drItemSetting.TGT_ITEM_ID

                                'エラー対象の対象項目詳細IDで商談条件マスタを検索し、項目名を取得する
                                Dim dtSalesCondition As SC3070208DataSet.SC3070208SalesConditionDataTable = _
                                    SC3070208TableAdapter.GetSalesCondition(CType(drItemSetting.TGT_ITEM_DETAIL_ID, Long))
                                For Each drSalesCondition In dtSalesCondition
                                    Me._msg = drSalesCondition.TITLE
                                Next
                                Exit For
                            End If
                            '2015/03/20 TCS 藤井 セールスタブレット：0118 ADD START
                        End If
                    End Using
                    '2015/03/20 TCS 藤井 セールスタブレット：0118 ADD END
                Case TGT_ITEM_ID_CST_PHONE, TGT_ITEM_ID_CST_MOBILE
                    '電話番号・携帯電話番号
                    '後続処理で入力チェックする為、ここではなにもしない
                Case Else
                    '顧客関連
                    errorItem = InputCheckCustomer(drItemSetting, checkItemList, dtCheckCustomer)
                    If String.IsNullOrEmpty(errorItem) = False Then
                        'エラー
                        Exit For
                    End If
            End Select
        Next

        If String.IsNullOrEmpty(errorItem) Then
            '入力項目設定マスタの内容に限らずチェック

            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) START
            'ソース選択2
            If dtCheckSales.Rows(0).Item(checkItemList.Item(TGT_ITEM_ID_SOURCE_2_CD)) Is DBNull.Value OrElse
                CType(dtCheckSales.Rows(0).Item(checkItemList.Item(TGT_ITEM_ID_SOURCE_2_CD)), Long) = 0 Then
                'エラー
                errorItem = TGT_ITEM_ID_SOURCE_2_CD
                Return errorItem
            End If
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) END

            '携帯番号、自宅電話番号
            If String.IsNullOrWhiteSpace(dtCheckCustomer.Rows(0).Item(checkItemList.Item(TGT_ITEM_ID_CST_MOBILE)).ToString()) And _
                String.IsNullOrWhiteSpace(dtCheckCustomer.Rows(0).Item(checkItemList.Item(TGT_ITEM_ID_CST_PHONE)).ToString()) Then
                'エラー
                errorItem = TGT_ITEM_ID_CST_MOBILE & TGT_ITEM_ID_CST_PHONE
            End If

            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
            ' 未取引客の場合のみチェック
            Dim cstTypeValue = DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_CST_TYPE)), String)
            If IsNumeric(cstTypeValue) AndAlso CType(cstTypeValue, CstType) = CstType.NonTrading Then

                ' 個人法人区分（顧客カテゴリ）が未設定
                If String.IsNullOrWhiteSpace(DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_FLEET_FLG)), String)) Then
                    errorItem = TGT_ITEM_ID_FLEET_FLG
                    Return errorItem
                End If

                ' 個人法人項目コード（顧客サブカテゴリ１）が未設定
                If String.IsNullOrWhiteSpace(DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD)), String)) Then
                    errorItem = TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD
                    Return errorItem
                End If

                ' 顧客組織入力区分
                Dim cstOrgnzInputTypeValue = DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE)), String)
                If String.IsNullOrWhiteSpace(cstOrgnzInputTypeValue) Then
                    ' 未設定
                    errorItem = TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE
                    Return errorItem
                Else
                    Select Case CType(cstOrgnzInputTypeValue, CstOrgnzInputType)
                        Case CstOrgnzInputType.FromMaster
                            ' マスタから選択
                            If String.IsNullOrWhiteSpace(DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_CST_ORGNZ_CD)), String)) Then
                                ' 顧客組織コードが未設定
                                errorItem = TGT_ITEM_ID_CST_ORGNZ_CD
                                Return errorItem
                            End If
                        Case CstOrgnzInputType.Manual
                            ' 手入力
                            If String.IsNullOrWhiteSpace(DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_CST_ORGNZ_NAME)), String)) Then
                                ' 顧客組織名が未設定
                                errorItem = TGT_ITEM_ID_CST_ORGNZ_NAME
                                Return errorItem
                            End If
                        Case Else
                            ' 不正値
                            errorItem = TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE
                            Return errorItem
                    End Select
                End If

                ' 顧客サブカテゴリ２が未設定
                If String.IsNullOrWhiteSpace(DirectCast(dtCheckCustomer.Rows(0).Item(checkItemList(TGT_ITEM_ID_CST_SUBCAT2_CD)), String)) Then
                    errorItem = TGT_ITEM_ID_CST_SUBCAT2_CD
                    Return errorItem
                End If

            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
        End If

        Return errorItem
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Function
    '2017/12/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 入力チェック（顧客）
    ''' </summary>
    ''' <param name="drItemSetting">入力項目設定マスタDataRow</param>
    ''' <param name="checkItemList">入力チェック用リスト</param>
    ''' <param name="dtCheckCustomer">顧客DataTable</param>
    ''' <returns>対象項目ID(正常終了の場合ブランク)</returns>
    ''' <remarks></remarks>
    Private Function InputCheckCustomer(ByVal drItemSetting As SC3070208DataSet.SC3070208InputItemSettingRow, _
                                        ByVal checkItemList As Dictionary(Of String, String), _
                                        ByVal dtCheckCustomer As SC3070208DataSet.SC3070208InputCheckCustomerDataTable) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim errorItem As String = String.Empty

        Select Case drItemSetting.TGT_ITEM_ID
            Case TGT_ITEM_ID_FLEET_PIC_NAME, TGT_ITEM_ID_FLEET_PIC_DEPT, TGT_ITEM_ID_FLEET_PIC_POSITION
                '担当者氏名(法人)、担当者部署名(法人)、役職(法人)
                '法人フラグが法人の場合のみチェックを行う
                If FLEET_FLG_FLEET.Equals(dtCheckCustomer.Rows(0).Item(checkItemList.Item(TGT_ITEM_ID_FLEET_FLG)).ToString()) Then
                    If String.IsNullOrWhiteSpace(dtCheckCustomer.Rows(0).Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)).ToString()) Then
                        'エラー
                        errorItem = drItemSetting.TGT_ITEM_ID
                    End If
                End If
            Case TGT_ITEM_ID_CST_BIRTH_DATE
                '誕生日
                If CType(dtCheckCustomer.Rows(0).Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)), DateTime) = New DateTime(1900, 1, 1).Date Then
                    'エラー
                    errorItem = drItemSetting.TGT_ITEM_ID
                End If
            Case TGT_ITEM_ID_ACT_CAT_TYPE
                '活動区分
                For Each drCheckCustomer In dtCheckCustomer
                    '車両単位でチェック
                    If String.IsNullOrWhiteSpace(drCheckCustomer.Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)).ToString()) Then
                        'エラー
                        errorItem = drItemSetting.TGT_ITEM_ID
                        Exit For
                    End If
                Next
            Case Else
                'その他
                If String.IsNullOrWhiteSpace(dtCheckCustomer.Rows(0).Item(checkItemList.Item(drItemSetting.TGT_ITEM_ID)).ToString()) Then
                    'エラー
                    errorItem = drItemSetting.TGT_ITEM_ID
                End If
        End Select

        Return errorItem
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
    End Function

    ''' <summary>
    ''' エラー箇所に該当するエラー文言IDリストを取得する
    ''' </summary>
    ''' <returns>エラー文言IDリスト</returns>
    ''' <remarks></remarks>
    Private Function GetErrorMsgNo() As Dictionary(Of String, Integer)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim msgIdList As New Dictionary(Of String, Integer)

        With msgIdList
            .Add(TGT_ITEM_ID_FIRST_NAME, MsgId906) 'ファーストネーム
            .Add(TGT_ITEM_ID_MIDDLE_NAME, MsgId907) 'ミドルネーム
            .Add(TGT_ITEM_ID_LAST_NAME, MsgId908) 'ラストネーム
            .Add(TGT_ITEM_ID_CST_GENDER, MsgId909) '性別
            .Add(TGT_ITEM_ID_NAMETITLE_CD, MsgId910) '敬称
            .Add(TGT_ITEM_ID_FLEET_FLG, MsgId911) '個人/法人
            .Add(TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD, MsgId912) '個人法人項目
            .Add(TGT_ITEM_ID_FLEET_PIC_NAME, MsgId913) '担当者氏名(法人)
            .Add(TGT_ITEM_ID_FLEET_PIC_DEPT, MsgId914) '担当者部署名(法人)
            .Add(TGT_ITEM_ID_FLEET_PIC_POSITION, MsgId915) '役職(法人)
            .Add(TGT_ITEM_ID_CST_MOBILE, MsgId916) '携帯番号
            .Add(TGT_ITEM_ID_CST_PHONE, MsgId917) '自宅電話番号
            .Add(TGT_ITEM_ID_CST_BIZ_PHONE, MsgId918) '勤務先電話番号
            .Add(TGT_ITEM_ID_CST_FAX, MsgId919) '自宅FAX番号
            .Add(TGT_ITEM_ID_CST_ZIPCD, MsgId920) '郵便番号
            .Add(TGT_ITEM_ID_CST_ADDRESS_1, MsgId921) '住所1
            .Add(TGT_ITEM_ID_CST_ADDRESS_2, MsgId922) '住所2
            .Add(TGT_ITEM_ID_CST_ADDRESS_3, MsgId923) '住所3
            .Add(TGT_ITEM_ID_CST_ADDRESS_STATE, MsgId924) '住所(州)
            .Add(TGT_ITEM_ID_CST_ADDRESS_LOCATION, MsgId925) '住所(地域)
            .Add(TGT_ITEM_ID_CST_ADDRESS_CITY, MsgId926) '住所(市)
            .Add(TGT_ITEM_ID_CST_ADDRESS_DISTRICT, MsgId927) '住所(地区)
            .Add(TGT_ITEM_ID_CST_DOMICILE, MsgId928) '本籍
            .Add(TGT_ITEM_ID_CST_EMAIL_1, MsgId929) 'e-Mail1
            .Add(TGT_ITEM_ID_CST_EMAIL_2, MsgId930) 'e-Mail2
            .Add(TGT_ITEM_ID_CST_COUNTRY, MsgId931) '国籍
            .Add(TGT_ITEM_ID_CST_SOCIALNUM, MsgId932) '国民ID
            .Add(TGT_ITEM_ID_CST_BIRTH_DATE, MsgId933) '誕生日
            .Add(TGT_ITEM_ID_ACT_CAT_TYPE, MsgId934) '活動区分
            .Add(TGT_ITEM_ID_GRADE_CD, MsgId935) '希望車種グレード
            .Add(TGT_ITEM_ID_BODYCLR_CD, MsgId936) '希望車種カラー
            '2017/12/20 TCS 河原 TKM独自機能開発 START
            .Add(TGT_ITEM_ID_SUFFIX_CD, MsgId946) '希望車種サフィックス
            .Add(TGT_ITEM_ID_INTERIOR_BODYCLR_CD, MsgId947) '希望車種内装色
            '2017/12/20 TCS 河原 TKM独自機能開発 end
            .Add(TGT_ITEM_ID_SOURCE_1_CD, MsgId937) 'ソース選択
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061) DELETE
            .Add(TGT_ITEM_ID_SALESCONDITIONNO, MsgId939) '商談条件
            .Add(TGT_ITEM_ID_CST_MOBILE & TGT_ITEM_ID_CST_PHONE, MsgId940) '携帯番号&自宅電話番号
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
            .Add(TGT_ITEM_ID_CST_ORGNZ_CD, MsgId10901) '顧客組織
            .Add(TGT_ITEM_ID_CST_ORGNZ_NAME, MsgId10901) '顧客組織
            .Add(TGT_ITEM_ID_CST_SUBCAT2_CD, MsgId10902) 'サブカテゴリ2
            .Add(TGT_ITEM_ID_CST_ORGNZ_INPUT_TYPE, MsgId10901) '顧客組織
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)START
            .Add(TGT_ITEM_ID_SOURCE_2_CD, MsgId10903) 'ソース2選択
            '2020/01/21 TS 和田   TKM Change request development for Next Gen e-CRB (CR058,CR061)END
        End With

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)
        Return msgIdList
    End Function

    ''' <summary>
    ''' 通知登録IF呼び出し
    ''' </summary>
    ''' <param name="toAccount">送信先アカウント</param>
    ''' <param name="toAccountName">送信先アカウント名</param>
    ''' <param name="requestType">ステータス</param>
    ''' <param name="noticeReqId">依頼ID</param>
    ''' <param name="reqClassId">依頼種別ID</param>
    ''' <param name="parameter">パラメータDataRow</param>
    ''' <param name="requestClass">依頼種別</param>
    ''' <returns>XmlCommon</returns>
    ''' <remarks></remarks>
    Private Function NoticeRequest(ByVal toAccount As String, _
                                   ByVal toAccountName As String, _
                                   ByVal requestType As RequestTypeEnum, _
                                   ByVal noticeReqId As Long, _
                                   ByVal reqClassId As Decimal, _
                                   ByVal parameter As SC3070208DataSet.SC3070208ParameterRow, _
                                   Optional ByVal requestClass As String = "") As XmlCommon
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start", GetCurrentMethod.Name), True)

        Dim noticeData As XmlNoticeData = Nothing
        Dim account As XmlAccount = Nothing
        Dim requestNotice As XmlRequestNotice = Nothing
        Dim pushInfo As XmlPushInfo = Nothing
        Try
            noticeData = New XmlNoticeData
            'headにデータを格納
            noticeData.TransmissionDate = DateTimeFunc.Now(parameter.DLR_CD)

            '相談先情報をセット（セールスマネージャー）
            account = New XmlAccount
            account.ToAccount = toAccount
            account.ToAccountName = toAccountName

            '相談者情報（スタッフ情報）をセット
            requestNotice = New XmlRequestNotice
            requestNotice.DealerCode = parameter.DLR_CD
            requestNotice.StoreCode = parameter.BRN_CD

            '依頼種別
            If String.IsNullOrEmpty(requestClass) Then
                requestNotice.RequestClass = NOTICE_IF_ORDER_APPROVAL
            Else
                requestNotice.RequestClass = requestClass
            End If

            'ステータス、依頼ID
            If requestType = RequestTypeEnum.Request Then
                requestNotice.Status = NOTICE_IF_REQUEST
            Else
                requestNotice.Status = NOTICE_IF_CANCEL
                requestNotice.RequestId = noticeReqId
            End If

            requestNotice.RequestClassId = reqClassId
            requestNotice.FromAccount = parameter.ACCOUNT
            requestNotice.FromAccountName = parameter.ACCOUNTNAME

            'キャンセル時は下記内容を設定しない
            If requestType = RequestTypeEnum.Request Then
                requestNotice.CustomId = parameter.CST_ID
                requestNotice.CustomName = parameter.CST_NAME
                requestNotice.CustomerClass = parameter.CST_VCL_TYPE
                requestNotice.CustomerKind = parameter.CST_TYPE
                requestNotice.SalesStaffCode = parameter.SLS_PIC_STF_CD
                requestNotice.VehicleSequenceNumber = parameter.VehicleSequenceNumber
                requestNotice.FollowUpBoxStoreCode = parameter.FLLWUPBOXSTRCD
                If parameter.IsFLLWUPBOX_SEQNONull = False Then
                    requestNotice.FollowUpBoxNumber = parameter.FLLWUPBOX_SEQNO
                End If
            Else
                'キャンセル時でも、注文情報登録・変更時には、セットする
                If requestType = RequestTypeEnum.Cancel And _
                    NOTICE_IF_SALES_UPDATE.Equals(requestClass) Then
                    requestNotice.CustomId = parameter.CST_ID
                    requestNotice.CustomName = parameter.CST_NAME
                    requestNotice.CustomerClass = parameter.CST_VCL_TYPE
                    requestNotice.CustomerKind = parameter.CST_TYPE
                End If

            End If


            '通知方法をセット
            pushInfo = New XmlPushInfo
            pushInfo.PushCategory = NOTICE_IF_PUSHCATEGORY_POPUP
            pushInfo.PositionType = NOTICE_IF_POSITION_HEADER
            pushInfo.Time = NOTICE_IF_TIME
            pushInfo.DisplayType = NOTICE_IF_DISPLAY_TYPE_TEXT
            If requestType = RequestTypeEnum.Request Then
                pushInfo.DisplayContents = WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 901)
            Else
                pushInfo.DisplayContents = WebWordUtility.GetWord(SC3070208TableAdapter.ProgramId, 902)
            End If
            pushInfo.Color = NOTICE_IF_COLOR
            pushInfo.DisplayFunction = NOTICE_IF_DISPLAY_FUNCTION
            pushInfo.ActionFunction = NOTICE_IF_ACTION_FUNCTION


            '格納したデータを親クラスに格納
            noticeData.AccountList.Add(account)
            noticeData.RequestNotice = requestNotice
            noticeData.PushInfo = pushInfo

            'ロジックを呼ぶ
            Using noticeRequestIF As New IC3040801BusinessLogic

                'i-CROPへ送信
                Dim response As XmlCommon = noticeRequestIF.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)

                Return response
            End Using
        Finally
            If pushInfo IsNot Nothing Then
                pushInfo.Dispose()
            End If

            If requestNotice IsNot Nothing Then
                requestNotice.Dispose()
            End If

            If account IsNot Nothing Then
                account.Dispose()
            End If

            If noticeData IsNot Nothing Then
                noticeData.Dispose()
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End", GetCurrentMethod.Name), True)

        End Try
    End Function
#End Region

End Class
