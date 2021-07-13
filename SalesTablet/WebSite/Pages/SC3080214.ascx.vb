'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080214.ascx.vb
'─────────────────────────────────────
'機能： 顧客詳細コントロール
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Data
Imports System.Globalization
Imports System.Reflection

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess

'更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する START
Imports Toyota.eCRB.iCROP.BizLogic.IC3080204
Imports Toyota.eCRB.iCROP.DataAccess.IC3080204
'更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する END

Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class PagesSC3080214
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler, ISC3080201Control


#Region "顧客編集、車両編集"

#Region " メソット 顧客情報編集"

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CustomerEditInitialize(ByVal mode As Integer)

        ''引数をログに出力
        Dim argsIN As New List(Of String)
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "mode = {0}", mode))
          ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            'コールバックスプリクト登録
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
                                               "Callback", _
                                               String.Format(CultureInfo.InvariantCulture, _
                                                             "callback.beginCallback = function () {{ {0}; }};", _
                                                             Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                         "callback.packedArgument", _
                                                                                                         "callback.endCallback", _
                                                                                                         "", _
                                                                                                         True)), _
                                                             True)

            '前画面からセッション情報を取得
            'モード (０：新規登録モード、１：編集モード)

            Dim msgID As Integer = 0
            'Dim bizClass As New SC3080205BusinessLogic
            Using custDataTbl As New SC3080205DataSet.SC3080205CustDataTable
                Dim custDataRow As SC3080205DataSet.SC3080205CustRow

                custDataRow = custDataTbl.NewSC3080205CustRow
                custDataTbl.Rows.Add(custDataRow)       '追加する

                'セッション内の値をセットする
                SetSessionValue(custDataRow, mode)

                '初期表示用フラグ情報取得
                SC3080205BusinessLogic.GetInitializeFlg(custDataTbl, msgID)

                If (mode = SC3080205BusinessLogic.ModeEdit) Then
                    '１：編集モード
                    '初期表示情報取得
                    'custDataTbl = SC3080205BusinessLogic.GetInitialize(custDataTbl, msgID)
                    SC3080205BusinessLogic.GetInitialize(custDataTbl, msgID)
                End If

                '顧客メモの情報をセッションに保持する
                Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CUSTSEGMENT, CType(custDataRow.CUSTFLG, String))    '顧客区分 (1：自社客 / 2：未取引客)
                Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CUSTOMERCLASS, "1")                                 '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
                If (mode = SC3080205BusinessLogic.ModeEdit) Then
                    '活動先顧客コード
                    If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
                        '０：自社客
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.ORIGINALID)
                    Else
                        '１：未顧客
                        Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.CSTID)
                    End If

                    Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTNAME, custDataRow.NAME)                '活動先顧客名
                End If
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' セッションの値をDataRowにセットする。
    ''' </summary>
    ''' <param name="customerDataRow">顧客情報DataRow</param>
    ''' <remarks></remarks>
    Protected Sub SetSessionValue(ByVal customerDataRow As SC3080205DataSet.SC3080205CustRow, ByVal mode As Integer)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        For Each column As DataColumn In customerDataRow.Table.Columns
            If customerDataRow.IsNull(column.ColumnName) = True Then
                argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, customerDataRow(column.ColumnName)))
            End If
        Next
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "mode = {0}", mode))

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try

            If (customerDataRow Is Nothing) Then
                Return
            End If

            'モード (０：新規登録モード、１：編集モード)
            '自社客/未取引客フラグ (０：自社客、１：未取引客)
            Dim custflg As Short = CType(SC3080205BusinessLogic.NewCustFlg, Short)
            If (mode = SC3080205BusinessLogic.ModeEdit) Then
                '編集モード時のみ、未取引客となる場合がある
                custflg = CType(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), Short)
            End If
            '自社客連番/未取引客ユーザID
            Dim originalid As String = String.Empty
            If (mode = SC3080205BusinessLogic.ModeEdit) Then
                originalid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            End If

            'ログインユーザー情報取得用
            Dim context As StaffContext = StaffContext.Current

            Dim dlrcd As String = context.DlrCD         '自身の販売店コード
            Dim strcd As String = context.BrnCD         '自身の店舗コード
            Dim account As String = context.Account     '自身のアカウント
            Dim strcdstaff As String = context.BrnCD    '自身の店舗コード (スタッフ店舗コード)
            Dim staffcd As String = context.Account     '自身のアカウント (スタッフコード)

            'セッション情報のセット
            customerDataRow.CUSTFLG = custflg
            If (customerDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
                '０：自社客
                customerDataRow.ORIGINALID = originalid
            Else
                '１：未取引客
                customerDataRow.CSTID = originalid
            End If
            '販売店コード
            customerDataRow.DLRCD = dlrcd
            '店舗コード
            customerDataRow.STRCD = strcd
            'スタッフ店舗コード
            customerDataRow.STRCDSTAFF = strcdstaff
            'スタッフコード
            customerDataRow.STAFFCD = staffcd
            'AC変更アカウント
            customerDataRow.AC_MODFACCOUNT = account
            '更新アカウント
            customerDataRow.UPDATEACCOUNT = account
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))
        End Try
    End Sub

#End Region

#Region "コールバック処理"
    Private Const OKText As String = "1"

    Private Const ErrorText As String = "9"

    Private _callbackResult As String

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return _callbackResult

    End Function

    ''' <summary>
    ''' コールバックイベントハンドリング
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        'Dim tokens As String() = eventArgument.Split(New Char() {","c})
        'Dim method As String = tokens(0)
        'Dim argument As String = tokens(1)
        'Dim resultString As String = String.Empty

    End Sub

#End Region

#End Region

#Region "顧客詳細(顧客情報)"
#Region " 定数 "

    '顧客詳細セッションキー
    '''' <summary>販売店コード</summary>
    'Private Const SESSION_KEY_DLRCD As String = "SearchKey.DLRCD"
    '''' <summary>店舗コード</summary>
    'Private Const SESSION_KEY_STRCD As String = "SearchKey.STRCD"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"                   '1：自社客 / 2：未取引客
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"       '1：所有者 / 2：使用者 / 3：その他
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"                 '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
    ''' <summary>車両ID</summary>
    Private Const SESSION_KEY_VCLID As String = "SearchKey.VCLID"                       '車両ID(VIN：自社客 / 車両シーケンスNo.：未取引客)
    '''' <summary>FOLLOW_UP_BOX</summary>
    'Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    '''' <summary>顧客メモクリア用</summary>
    'Private Const SESSION_KEY_MEMO_INIT As String = "SearchKey.MEMOINIT"               '9:顧客メモ読み込み完了

    '更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する START
    Private Const SESSION_KEY_DMSID As String = "SearchKey.DMSID"                       '基幹顧客ID
    '更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する END

    '顧客メモセッションキー
    Private Const SESSION_KEY_MEMO_CUSTSEGMENT As String = "SearchKey.CUSTSEGMENT"      '顧客区分 (1：自社客 / 2：未取引客)
    Private Const SESSION_KEY_MEMO_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"  '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
    Private Const SESSION_KEY_MEMO_CRCUSTID As String = "SearchKey.CRCUSTID"            '活動先顧客コード
    Private Const SESSION_KEY_MEMO_CRCUSTNAME As String = "SearchKey.CRCUSTNAME"        '活動先顧客名

    '活動結果登録連携
    Private Const CONST_VCLINFO As String = "VCLINFO"

    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    Private Const ImageFilePath As String = "~/styles/images/SC3080214/"
    Private Const ImageFileExt As String = ".png"
    Private Const ClassString As String = "class"
    Private Const SelectedButtonString As String = "selectedButton"

    Private Const ContactFlgOn As String = "1"
    Private Const ContactFlgOff As String = "0"

    Private Const ORGCUSTFLG As String = "1" ' 自社客/未取引客フラグ (1：自社客)
    Private Const NEWCUSTFLG As String = "2" ' 自社客/未取引客フラグ (2：未取引客)

    Private Const FIRSTLOAD As String = "1" '車両選択ポップアップ初期表示
    Private Const NEXTLOAD As String = "2" '車両選択ポップアップ2回目以降表示

    Private Const NOTEXT As String = "-"
#Region " 顧客編集ポップアップ自動起動フラグ "
    Private Const CUSTOMER_POPUP_AUTOFLG_OFF As String = "0"
    Private Const CUSTOMER_POPUP_AUTOFLG_ON As String = "1"
#End Region

#Region " 希望連絡方法、時間帯"

    ''' <summary>
    ''' 時間帯クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TIMEZONECLASS_1 As Integer = 1
    Private Const TIMEZONECLASS_2 As Integer = 2

    Private Const CONTACTIMAGE_MOBILE As String = "icon_mobile.png"
    Private Const CONTACTIMAGE_HOME As String = "icon_home.png"
    Private Const CONTACTIMAGE_SMS As String = "icon_SMSmail.png"
    Private Const CONTACTIMAGE_EMAIL As String = "icon_Email.png"
    Private Const CONTACTIMAGE_DM As String = "icon_DM.png"

#End Region

#Region " コンタクト履歴 "
    ''' <summary>活動種類アイコン セールス</summary>
    Private Const ACTUALKIND_IMAGE_SALSE As String = "~/Styles/Images/SC3080214/scNscCurriculumListCarIcon1.png"

    ''' <summary>活動種類 セールス</summary>
    Private Const ACTUALKIND_SALSE As String = "1"

    ''' <summary>カウント表示</summary>
    Private Const COUNTVIEW_NO As String = "0" '表示無し
    Private Const COUNTVIEW_YES As String = "1" '表示あり

    ''' <summary>ステータス 1: Walk-in、2: Prospect、3: Hot、4: Success、5: Give-up</summary>
    Private Const CRACTSTATUS_W As String = "1"
    Private Const CRACTSTATUS_P As String = "2"
    Private Const CRACTSTATUS_H As String = "3"
    Private Const CRACTSTATUS_S As String = "4"
    Private Const CRACTSTATUS_G As String = "5"

    Private Const CRACTSTATUS_ICON_PATH As String = "~/Styles/Images/SC3080214/"
    Private Const CRACTSTATUS_W_ICON As String = "scNscCurriculumListStarIcon1.png"
    Private Const CRACTSTATUS_P_ICON As String = "scNscCurriculumListStarIcon2.png"
    Private Const CRACTSTATUS_H_ICON As String = "scNscCurriculumListStarIcon3.png"
    Private Const CRACTSTATUS_S_ICON As String = "scNscCurriculumListStarIcon4.png"
    Private Const CRACTSTATUS_G_ICON As String = "scNscCurriculumListStarIcon5.png"

    ''' <summary>権限</summary>
    Private Const OPERATIONCODE_CCM As String = "1" 'Call Centre Manager
    Private Const OPERATIONCODE_CCO As String = "2" 'Call Centre Operator
    Private Const OPERATIONCODE_AHO As String = "3" 'Assistant (H/O)
    Private Const OPERATIONCODE_AB As String = "4" 'Assistant (Branch)
    Private Const OPERATIONCODE_SGM As String = "5" 'Sales General Manager
    Private Const OPERATIONCODE_BM As String = "6" 'Branch Manager
    Private Const OPERATIONCODE_SSM As String = "7" 'Sales Manager
    Private Const OPERATIONCODE_SS As String = "8" 'Sales Staff 
    Private Const OPERATIONCODE_SA As String = "9" 'Service Adviser
    Private Const OPERATIONCODE_SM As String = "10" 'Service Manager

    Private Const OPERATIONCODE_IMAGE_PATH As String = "~/Styles/Images/Authority/"
    Private Const OPERATIONCODE_IMAGE_CCO As String = "CCO.png"
    Private Const OPERATIONCODE_IMAGE_MANAGER As String = "Manager.png"
    Private Const OPERATIONCODE_IMAGE_RECEPTIONIST As String = "Receptionist.png"
    Private Const OPERATIONCODE_IMAGE_SA As String = "SA.png"
    Private Const OPERATIONCODE_IMAGE_SC As String = "SC.png"
#End Region

#Region "顔写真"
    '顔写真
    Private Const IMAGEFILE_L As String = "_L"
    Private Const IMAGEFILE_M As String = "_M"
    Private Const IMAGEFILE_S As String = "_S"

#End Region

#Region "エラーメッセージ"
    Private Const ERRMSGID_10909 As Integer = 10909
    Private Const ERRMSGID_10910 As Integer = 10910
    Private Const ERRMSGID_10911 As Integer = 10911
#End Region

#End Region

#Region "プロパティ"
    Private Property FamilyNumber As Integer

#End Region

#Region "Page_Load"

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If Not Page.IsPostBack Then
                '初期表示
                _PageOpen()

            Else

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' PreRender処理を実施します。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If Page.IsPostBack Then
                If Not ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then

                    '一部ポップアップを再表示する

                    '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
                    Dim crcustId As String = String.Empty
                    If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                        crcustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                    End If

                    '既存客モードの場合のみ実行
                    If Not String.IsNullOrEmpty(Trim(crcustId)) = True Then
                        'If Not String.Equals(Trim(crcustId), String.Empty) Then
                        'Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
                        ''顧客種別(1：自社客 / 2：未取引客)
                        'Dim cstKind As String = String.Empty
                        'If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
                        '    cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                        'End If

                        '家族ポップアップ再表示
                        Me.CustomerRelatedFamilyLoad()

                        '顧客情報編集の初期処理
                        'Call CustomerEditInitialize()
                        'コールバックスプリクト登録
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
                                                            "Callback", _
                                                            String.Format(CultureInfo.InvariantCulture, _
                                                                          "callback.beginCallback = function () {{ {0}; }};", _
                                                                          Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                                      "callback.packedArgument", _
                                                                                                                      "callback.endCallback", _
                                                                                                                      "", _
                                                                                                                      True)), _
                                                                          True)
                        '車両編集の初期処理
                        'Call VehicleInitialize()

                    End If
                End If
            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "初期処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _PageOpen()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try

            '顧客種別(1：自社客 / 2：未取引客)
            'Dim cstKind As String = String.Empty
            'If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
            '    cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            'End If

            '更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する START
            '基幹顧客ID(DMSID)から顧客コードを取得するため、一旦、顧客コードをクリア
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                '顧客コードのクリア
                RemoveValueBypass(ScreenPos.Current, SESSION_KEY_CRCUSTID)
            End If
            If ContainsKey(ScreenPos.Current, SESSION_KEY_DMSID) Then
                '基幹顧客ID(DMSID)を取得した場合、基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得
                Dim basicCustomerId As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_DMSID, False), String)
                Using da As New IC3080204BusinessLogic()
                    Using dt As IC3080204DataSet.IC3080204CustomerDataTable = da.GetMyCustomerInfo(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, basicCustomerId)
                        If dt.Rows.Count > 0 Then
                            'セッションに顧客コードを設定
                            Dim row As IC3080204DataSet.IC3080204CustomerRow = DirectCast(dt.Rows(0), IC3080204DataSet.IC3080204CustomerRow)
                            SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, row.ORIGINALID)
                        End If
                    End Using
                End Using
            End If
            '更新： 2012/02/16 KN 瀧 【SERVICE_1】基幹顧客ID(DMSID)から自社客連番(ORIGINALID)を取得する END

            '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
            Dim crcustId As String = String.Empty
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                crcustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            End If

            '既存客モード、新規登録モードの判定
            'If String.Equals(Trim(crcustId), String.Empty) Then
            If String.IsNullOrEmpty(Trim(crcustId)) = True Then
                '新規登録モード

                '各表示欄を新規登録の状態に変更
                '///////////////////////////////////////////
                '職業
                CustomerRelatedOccupationSelectedPanel.Visible = False
                CustomerRelatedOccupationNewPanel.Visible = True
                '家族
                CustomerRelatedFamilySelectedEditPanel.Visible = False
                CustomerRelatedFamilySelectedNewPanel.Visible = True
                '趣味
                CustomerRelatedHobbySelectedEditPanel.Visible = False
                CustomerRelatedHobbySelectedNewPanel.Visible = True
                '連絡
                CustomerRelatedContactSelectedEditPanel.Visible = False
                CustomerRelatedContactSelectedNewPanel.Visible = True
                'メモ
                EditCustomerMemoPanel.Visible = False
                NewCustomerMemoPanel.Visible = True
                '///////////////////////////////////////////

                '顧客編集ポップアップ以外のポップアップを実行不可にする
                '///////////////////////////////////////////
                Me.CustomerRelatedOccupationArea.Attributes.Item("onclick") = String.Empty
                Me.CustomerRelatedFamilyArea.Attributes.Item("onclick") = String.Empty
                Me.CustomerRelatedHobbyArea.Attributes.Item("onclick") = String.Empty
                Me.CustomerRelatedContactArea.Attributes.Item("onclick") = String.Empty
                Me.CustomerMemo_Click.Attributes.Item("onclick") = String.Empty
                '///////////////////////////////////////////

                '顧客情報編集の初期処理
                Call CustomerEditInitialize(SC3080205BusinessLogic.ModeCreate)

            Else
                '既存客モード

                '全ポップアップを実行可にする
                '///////////////////////////////////////////
                '処理不要
                '///////////////////////////////////////////

                Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用

                '顧客職業取得
                Dim occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable _
                    = SC3080201BusinessLogic.GetOccupationData(params)
                occupationDataTbl = SC3080201BusinessLogic.EditOccupatonData(occupationDataTbl)
                Me._SetCustomerRelatedOccupationPopUp(occupationDataTbl)
                Me._SetCustomerRelatedOccupationArea(occupationDataTbl)

                '家族構成マスタ取得
                Dim custFamilyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable _
                    = SC3080201BusinessLogic.GetCustFamilyMstData(params)

                Me._SetCustomerRelatedFamilyPopUpRelationshipList(custFamilyMstDataTbl)

                '顧客家族構成取得
                Dim custFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable _
                    = SC3080201BusinessLogic.GetCustFamilyData(params)
                '不明行追加前に件数保持
                Dim familyCount As Integer = custFamilyDataTbl.Rows.Count
                '本人行編集、不明行追加
                custFamilyDataTbl = SC3080201BusinessLogic.EditCustFamilyData(custFamilyDataTbl, custFamilyMstDataTbl)
                'Me._SetCustomerRelatedFamilyArea(custFamilyDataTbl, familyCount)
                Me._SetCustomerRelatedFamilyArea(familyCount)
                Me._SetCustomerRelatedFamilyPopUpFamilyList(custFamilyDataTbl, familyCount)

                '顧客趣味取得
                Dim hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable _
                    = SC3080201BusinessLogic.GetHobbyData(params)

                Me._SetCustomerRelatedHobbyArea(hobbyDataTbl)

                '希望コンタクト方法取得
                Dim contactFlg As SC3080201DataSet.SC3080201ContactFlgDataTable _
                    = SC3080201BusinessLogic.GetContactFlg(params)
                Me._SetCustomerRelatedContactPopupContactTool(contactFlg)

                'TIMEZONECLASS分ループ
                For i = TIMEZONECLASS_1 To TIMEZONECLASS_2
                    params.Rows(0).Item(params.TIMEZONECLASSColumn.ColumnName) = i

                    '希望連絡時間帯取得
                    Dim timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable _
                        = SC3080201BusinessLogic.GetTimeZoneData(params)

                    '希望連絡曜日取得
                    Dim weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable _
                        = SC3080201BusinessLogic.GetWeekOfDayData(params)
                    'ポップアップ画面設定
                    Me._SetCustomerRelatedContactPopup(timeZoneDataTbl, weekOfDayDataTbl, i)
                Next

                '最新顧客メモ取得、画面設定
                Me._ShowLastCustMemo(params)

                'コンタクト履歴取得、画面設定
                'Me._ShowContactHistory(params)

                '顧客情報編集の初期処理
                Call CustomerEditInitialize(SC3080205BusinessLogic.ModeEdit)

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "各種データ取得、画面設定"

    ''' <summary>
    ''' 顧客メモの取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _ShowLastCustMemo(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In params Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            '最新顧客メモ取得
            Dim lastCustMemoDataTbl As SC3080201DataSet.SC3080201LastCustomerMemoDataTable _
                = SC3080201BusinessLogic.GetLastCustMemoData(params)
            '画面設定
            ControltimeLastCustMemo(lastCustMemoDataTbl)
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "パラメータセット"

    ''' <summary>
    ''' パラメータセット
    ''' </summary>
    ''' <returns>パラメータ管理テーブル</returns>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Function _SetParameters() As SC3080201DataSet.SC3080201ParameterDataTable
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '販売店コード
            Dim dlrCd As String = StaffContext.Current.DlrCD

            '店舗コード
            Dim strCd As String = StaffContext.Current.BrnCD

            '顧客種別(1：自社客 / 2：未取引客)
            Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

            '顧客分類(1：所有者 / 2：使用者 / 3：その他)
            Dim customerClass As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)

            '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
            Dim crcustId As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

            '車両ID(VIN：自社客 / 車両シーケンスNo.：未取引客)
            Dim vclId As String = String.Empty
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLID) = True) Then
                vclId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLID, False), String)
            End If

            Using params As New SC3080201DataSet.SC3080201ParameterDataTable '検索条件格納用

                Dim paramsDr As SC3080201DataSet.SC3080201ParameterRow
                paramsDr = params.NewSC3080201ParameterRow

                '検索条件セット
                paramsDr.DLRCD = dlrCd
                paramsDr.STRCD = strCd
                paramsDr.CSTKIND = cstKind
                paramsDr.CUSTOMERCLASS = customerClass
                paramsDr.CRCUSTID = crcustId
                paramsDr.VCLID = vclId
                params.Rows.Add(paramsDr)

                Return params
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Function
#End Region

#Region "職業ポップアップ関連"

#Region "職業ポップアップ表示エリアの作成"

    ''' <summary>
    ''' 職業ポップアップ表示エリアの設定
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedOccupationArea(ByVal dt As SC3080201DataSet.SC3080201CustomerOccupationDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In dt Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))

        Try
            Dim drs As SC3080201DataSet.SC3080201CustomerOccupationRow() _
                = CType(dt.Select("SELECTION = 1"), SC3080201DataSet.SC3080201CustomerOccupationRow())

            If drs.Count = 0 Then
                Me.CustomerRelatedOccupationNewPanel.Visible = True
                Me.CustomerRelatedOccupationSelectedPanel.Visible = False
            Else
                Me.CustomerRelatedOccupationNewPanel.Visible = False
                Me.CustomerRelatedOccupationSelectedPanel.Visible = True

                Dim dr As SC3080201DataSet.SC3080201CustomerOccupationRow = drs(0)
                Me.CustomerRelatedOccupationSelectedImage.BackImageUrl = ResolveClientUrl(dr.ICONPATH_VIEWONLY)
                Me.CustomerRelatedOccupationSelectedLabel.Text = HttpUtility.HtmlEncode(dr.OCCUPATION)
            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業選択ポップアップ作成"

    ''' <summary>
    ''' 職業選択ポップアップ作成
    ''' </summary>
    ''' <param name="occupationDataTbl"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedOccupationPopUp(ByVal occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In occupationDataTbl Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            Me.OccupationPopupTitlePage1.Value = WebWordUtility.GetWord(10122)
            Me.OccupationPopupTitlePage2.Value = WebWordUtility.GetWord(10123)
            Me.OccupationOtherErrMsg.Value = WebWordUtility.GetWord(10902)
            Me.CustomerRelatedOccupationRegisterButton.Text = WebWordUtility.GetWord(10126)
            Me.CustomerRelatedOccupationOtherCustomTextBox.Text = String.Empty

            'ポップアップ動的アイコン設定
            Me.CustomerRelatedOccupationButtonRepeater.DataSource = occupationDataTbl
            Me.CustomerRelatedOccupationButtonRepeater.DataBind()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 職業選択ポップアップ作成
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub OccupationButtonAria_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles CustomerRelatedOccupationButtonRepeater.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim dr As SC3080201DataSet.SC3080201CustomerOccupationRow = _
                    CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerOccupationRow)

                Dim occupationPanel As Panel = CType(e.Item.FindControl("CustomerRelatedOccupationPanel"), Panel)
                If dr.SELECTION.Equals("1") Then
                    occupationPanel.BackImageUrl = ResolveClientUrl(dr.ICONPATH_SELECTED)
                Else
                    occupationPanel.BackImageUrl = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)
                End If

                Dim occupationLabel As Label = CType(e.Item.FindControl("CustomerRelatedOccupationText"), Label)
                occupationLabel.Text = HttpUtility.HtmlEncode(dr.OCCUPATION)
                If dr.SELECTION.Equals("1") Then
                    occupationLabel.CssClass = occupationLabel.CssClass & " selectedFont"
                    If dr.OTHER.Equals("1") Then
                        Me.CustomerRelatedOccupationOtherCustomTextBox.Text = dr.OCCUPATION
                    End If

                End If

                '初期選択状態を保持
                Dim occupationSelectedField As HiddenField = CType(e.Item.FindControl("CustomerRelatedOccupationSelectedHiddenField"), HiddenField)
                occupationSelectedField.Value = dr.SELECTION

                If dr.OTHER.Equals("1") And dr.SELECTION.Equals("0") Then

                    Dim occupationLink As LinkButton = CType(e.Item.FindControl("CustomerRelatedOccupationHyperLink"), LinkButton)
                    occupationLink.OnClientClick = "setPopupOccupationPage('page2'," & dr.OCCUPATIONNO & ");return false;"

                End If

                Dim occupationFiled As HiddenField = CType(e.Item.FindControl("CustomerRelatedOccupationIdHiddenField"), HiddenField)
                occupationFiled.Value = CStr(dr.OCCUPATIONNO)

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業選択時イベント"

    ''' <summary>
    ''' 職業選択時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub CustomerRelatedOccupationButtonRepeater_ItemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles CustomerRelatedOccupationButtonRepeater.ItemCommand
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '職業登録処理
            Me._RegistCustomerRelatedOccupation(CType(e.Item.FindControl("CustomerRelatedOccupationIdHiddenField"), HiddenField).Value, _
                                                String.Empty, _
                                                CType(e.Item.FindControl("CustomerRelatedOccupationSelectedHiddenField"), HiddenField).Value)
            '職業再表示
            Me._SetCustomerRelatedOccupation()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業(その他)入力完了イベント"

    ''' <summary>
    ''' 職業(その他)入力完了イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedOccupationRegisterButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedOccupationRegisterButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '禁則文字チェック
            If Not Validation.IsValidString(Me.CustomerRelatedOccupationOtherCustomTextBox.Text) Then
                ShowMessageBox(ERRMSGID_10909)
            Else
                '職業登録処理(その他)
                Me._RegistCustomerRelatedOccupation(Me.CustomerRelatedOccupationOtherIdHiddenField.Value, _
                                                    Me.CustomerRelatedOccupationOtherCustomTextBox.Text, _
                                                    String.Empty)
            End If

            '職業再表示
            Me._SetCustomerRelatedOccupation()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業再表示"

    ''' <summary>
    ''' 職業再表示
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedOccupation()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '顧客職業取得
            Dim occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable _
                = SC3080201BusinessLogic.GetOccupationData(_SetParameters())
            occupationDataTbl = SC3080201BusinessLogic.EditOccupatonData(occupationDataTbl)
            Me._SetCustomerRelatedOccupationPopUp(occupationDataTbl)
            Me._SetCustomerRelatedOccupationArea(occupationDataTbl)

            'CustomTextBox
            JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#CustomerRelatedOccupationOtherCustomTextBox').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業登録処理"

    ''' <summary>
    ''' 職業登録処理
    ''' </summary>
    ''' <param name="OccupationNo"></param>
    ''' <param name="otherOccupation"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _RegistCustomerRelatedOccupation(ByVal OccupationNo As String, ByVal otherOccupation As String, ByVal selectedFlg As String)
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:OccupationNo = {2}, otherOccupation = {3}, selectedFlg = {4}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , OccupationNo _
            , otherOccupation _
            , selectedFlg))
        Try
            Using params As New SC3080201DataSet.SC3080201InsertCstOccupationDataTable '検索条件格納用

                Dim paramsDr As SC3080201DataSet.SC3080201InsertCstOccupationRow
                paramsDr = params.NewSC3080201InsertCstOccupationRow

                '登録値設定
                paramsDr.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                paramsDr.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                paramsDr.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                If Not String.Equals(selectedFlg, "1") Then
                    ' ビルドを通す為一時的に修正 START
                    paramsDr.OCCUPATIONNO = OccupationNo
                    ' ビルドを通す為一時的に修正 END
                    paramsDr.OTHEROCCUPATION = otherOccupation
                End If

                params.Rows.Add(paramsDr)

                Dim bizClass As New SC3080201BusinessLogic
                If bizClass.InsertCstOccupation(params) Then
                    Exit Sub
                End If
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "職業キャンセル押下処理"

    ''' <summary>
    ''' 職業キャンセル押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedOccupationCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedOccupationCancelButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '職業再表示
            Me._SetCustomerRelatedOccupation()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#End Region

#Region "家族ポップアップ関連"

#Region "画面設定 家族編集画面ポップアップボタン"

    ''' <summary>
    ''' 画面設定 家族編集画面ポップアップボタン
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedFamilyArea(ByVal familyCount As Integer)
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            'Private Sub _SetCustomerRelatedFamilyArea(ByVal FamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable, _
            '                                          ByVal familyCount As Integer)
            Me.FamilyCountLabel.Text = HttpUtility.HtmlEncode(familyCount)
            Me.FamilyCount.Value = CStr(familyCount)

            If 1 = familyCount Then
                Me.CustomerRelatedFamilySelectedImage.BackImageUrl = "~/Styles/Images/SC3080214/scNscCustomerInfoIcon2.png"
            Else
                Me.CustomerRelatedFamilySelectedImage.BackImageUrl = "~/Styles/Images/SC3080214/scNscCustomerInfoIcon2.png"
            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "画面設定 家族ポップアップ続柄一覧作成"

    ''' <summary>
    ''' 画面設定 家族続柄マスタ
    ''' </summary>
    ''' <param name="FamilyMstDataTbl"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedFamilyPopUpRelationshipList(ByVal familyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In familyMstDataTbl Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            Me.FamilyPopupTitlePage1.Value = WebWordUtility.GetWord(10147)
            Me.FamilyPopupTitlePage2.Value = WebWordUtility.GetWord(10153)
            Me.FamilyPopupTitlePage3.Value = WebWordUtility.GetWord(10158)

            Me.CustomerRelatedFamilyRegisterButton.Text = WebWordUtility.GetWord(10126)
            'Me.CustomerRelatedFamilyPopUpCancelButton.Text = WebWordUtility.GetWord(10125)

            Me.RelationOtherErrMsgHidden.Value = WebWordUtility.GetWord(10905)

            Me.FamilyOtherRelationshipTextBox.Text = String.Empty

            FamilyRelationshipRepeater.DataSource = familyMstDataTbl
            FamilyRelationshipRepeater.DataBind()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 画面設定 家族続柄マスタ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub FamilyRelationshipRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles FamilyRelationshipRepeater.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim data As SC3080201DataSet.SC3080201CustomerFamilyMstRow _
                    = CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerFamilyMstRow)


                With CType(e.Item.FindControl("familyRelationshipLabel_No"), CustomLabel)
                    .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                    .Text = HttpUtility.HtmlEncode(data.FAMILYRELATIONSHIP)
                End With

                With CType(e.Item.FindControl("familyRelationshipNoHidden_No"), HiddenField)
                    .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                    .Value = CStr(data.FAMILYRELATIONSHIPNO)
                End With

                With CType(e.Item.FindControl("familyRelationshipList_No"), HtmlGenericControl)

                    .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                    If data.OTHERUNKNOWN.Equals("1") Then
                        .Attributes.Add("onclick", "setPopupFamilyPage('page3','page2'," & data.FAMILYRELATIONSHIPNO & ")")
                        Me.RelationOtherWordHidden.Value = data.FAMILYRELATIONSHIP
                        Me.RelationOtherNoHidden.Value = CStr(data.FAMILYRELATIONSHIPNO)
                    Else
                        .Attributes.Add("onclick", "selectFamilyRelationship('" & data.FAMILYRELATIONSHIPNO & "')")
                    End If

                End With

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "画面設定 家族ポップアップ家族構成一覧作成"

    ''' <summary>
    ''' 画面設定 顧客家族構成
    ''' </summary>
    ''' <param name="FamilyDataTbl"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedFamilyPopUpFamilyList(ByVal FamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable, _
                                                         ByVal familyCount As Integer)

        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In FamilyDataTbl Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}, familyCount = {3}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray()) _
            , familyCount))
        Try
            '人数ボタン選択解除
            For i = 1 To 10
                DirectCast(Me.FindControl("FamilyCount" & i), System.Web.UI.HtmlControls.HtmlAnchor).Attributes.Remove("class")
            Next i

            '人数ボタン選択
            Select Case familyCount
                Case 1
                    Me.FamilyCount1.Attributes.Add("class", "selectedButton")
                Case 2
                    Me.FamilyCount2.Attributes.Add("class", "selectedButton")
                Case 3
                    Me.FamilyCount3.Attributes.Add("class", "selectedButton")
                Case 4
                    Me.FamilyCount4.Attributes.Add("class", "selectedButton")
                Case 5
                    Me.FamilyCount5.Attributes.Add("class", "selectedButton")
                Case 6
                    Me.FamilyCount6.Attributes.Add("class", "selectedButton")
                Case 7
                    Me.FamilyCount7.Attributes.Add("class", "selectedButton")
                Case 8
                    Me.FamilyCount8.Attributes.Add("class", "selectedButton")
                Case 9
                    Me.FamilyCount9.Attributes.Add("class", "selectedButton")
                Case 10
                    Me.FamilyCount10.Attributes.Add("class", "selectedButton")
            End Select

            Me.FamilyNumber = familyCount


            CustomerRelatedFamilySelectedEditPanel.Visible = True
            CustomerRelatedFamilySelectedNewPanel.Visible = False

            FamilyBirthdayList.DataSource = FamilyDataTbl
            FamilyBirthdayList.DataBind()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 画面設定 顧客家族構成
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub FamilyBirthdayList_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles FamilyBirthdayList.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim data As SC3080201DataSet.SC3080201CustomerFamilyRow _
                    = CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerFamilyRow)

                With CType(e.Item.FindControl("FamilyBirthdayList_Row"), HtmlGenericControl)
                    If FamilyNumber = (e.Item.ItemIndex + 1) Then
                        .Attributes.Add("class", "FamilyBirthdayListAreaNoBorder")
                    ElseIf FamilyNumber < (e.Item.ItemIndex + 1) Then
                        .Attributes.Add("class", "displaynone")
                    End If
                End With

                With CType(e.Item.FindControl("FamilyBirthdayListRelationLabel_Row"), CustomLabel)
                    .Text = HttpUtility.HtmlEncode(data.FAMILYRELATIONSHIP)
                    If Not e.Item.ItemIndex = 0 Then
                        .Attributes.Add("onclick", "setPopupFamilyPage('page2','page1'," & e.Item.ItemIndex & ")")
                    End If
                End With

                CType(e.Item.FindControl("FamilyBirthdayListRelationNoHidden_Row"), HiddenField).Value = CStr(data.FAMILYRELATIONSHIPNO)
                CType(e.Item.FindControl("FamilyBirthdayListFamilyNoHidden_Row"), HiddenField).Value = CStr(data.FAMILYNO)
                CType(e.Item.FindControl("FamilyBirthdayListRelationOtherHidden_Row"), HiddenField).Value = CStr(data.OTHERFAMILYRELATIONSHIP)

                With CType(e.Item.FindControl("FamilyBirthdayListBirthdayDate_Row"), DateTimeSelector)
                    If Not data.IsBIRTHDAYNull Then
                        .Value = data.BIRTHDAY
                        CType(e.Item.FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value = CStr(data.BIRTHDAY)
                    End If

                    If e.Item.ItemIndex = 0 Then
                        .Enabled = False
                    End If
                End With

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "家族登録押下イベント"

    ''' <summary>
    ''' 家族登録押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedFamilyRegisterButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedFamilyRegisterButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '登録処理
            Me.RegistCustomerRelatedFamily()
            'ポップアップ再表示
            Me.CustomerRelatedFamilyLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 家族ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub CustomerRelatedFamilyLoad()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

            '家族構成マスタ取得
            Dim custFamilyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable _
                = SC3080201BusinessLogic.GetCustFamilyMstData(params)

            Me._SetCustomerRelatedFamilyPopUpRelationshipList(custFamilyMstDataTbl)

            '顧客家族構成取得
            Dim custFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable _
                = SC3080201BusinessLogic.GetCustFamilyData(params)
            '不明行追加前に件数保持
            Dim familyCount As Integer = custFamilyDataTbl.Rows.Count
            '本人行編集、不明行追加
            custFamilyDataTbl = SC3080201BusinessLogic.EditCustFamilyData(custFamilyDataTbl, custFamilyMstDataTbl)
            'Me._SetCustomerRelatedFamilyArea(custFamilyDataTbl, familyCount)
            Me._SetCustomerRelatedFamilyArea(familyCount)
            Me._SetCustomerRelatedFamilyPopUpFamilyList(custFamilyDataTbl, familyCount)

            CustomerRelatedFamilySelectedEditPanel.Visible = True
            CustomerRelatedFamilySelectedNewPanel.Visible = False

            JavaScriptUtility.RegisterStartupFunctionCallScript(Page, "bindFingerScroll", "startup")
            'CustomTextBox
            JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#FamilyOtherRelationshipTextBox').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "家族登録処理"

    ''' <summary>
    ''' 家族登録処理
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub RegistCustomerRelatedFamily()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Using dt As New SC3080201DataSet.SC3080201InsertCstFamilyDataTable
                ''1番目は顧客家族構成削除、家族人数登録用に使用
                For i As Integer = 0 To CInt(Me.FamilyCount.Value) - 1
                    Dim dr As SC3080201DataSet.SC3080201InsertCstFamilyRow = dt.NewSC3080201InsertCstFamilyRow

                    dr.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                    dr.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                    dr.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                    dr.FAMILYNO = i
                    dr.FAMILYRELATIONSHIPNO = CInt(CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayListRelationNoHidden_Row"), HiddenField).Value)
                    '禁則文字チェック
                    If Not i = 0 Then
                        Dim otherFamilyRelationShip As String = CStr(CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayListRelationOtherHidden_Row"), HiddenField).Value)
                        If Not String.IsNullOrEmpty(otherFamilyRelationShip) AndAlso Not Validation.IsValidString(otherFamilyRelationShip) Then
                            ShowMessageBox(ERRMSGID_10910)
                            Return
                        End If
                    End If
                    dr.OTHERFAMILYRELATIONSHIP = CStr(CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayListRelationOtherHidden_Row"), HiddenField).Value)
                    'If Not CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayListBirthdayDate_Row"), DateTimeSelector).Value Is Nothing Then
                    '    dr.BIRTHDAY = CDate(CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayListBirthdayDate_Row"), DateTimeSelector).Value)
                    'End If
                    'If Not CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value Is Nothing Then
                    '    dr.BIRTHDAY = CDate((CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value))
                    'End If
                    If Not String.IsNullOrEmpty(CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value) Then
                        dr.BIRTHDAY = CType((CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value), Date)
                        'dr.BIRTHDAY = CDate((CType(FamilyBirthdayList.Items(i).FindControl("FamilyBirthdayHidden_Row"), HiddenField).Value))
                    End If

                    ' ビルドを通す為一時的に修正 START
                    dr.NUMBEROFFAMILY = Me.FamilyCount.Value
                    ' ビルドを通す為一時的に修正 END

                    dt.Rows.Add(dr)

                Next i

                Dim bizClass As New SC3080201BusinessLogic
                bizClass.InsertCstFamily(dt)
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "家族キャンセル押下処理"

    ''' <summary>
    ''' 家族キャンセル押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedFamilyCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedFamilyCancelButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            'ポップアップ再表示
            Me.CustomerRelatedFamilyLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#End Region

#Region "趣味ポップアップ関連"

#Region "画面設定 趣味ポップアップ"

    ''' <summary>
    ''' 画面設定 趣味ポップアップ
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedHobbyArea(ByVal dt As SC3080201DataSet.SC3080201CustomerHobbyDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In dt Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            Me.RegisterCustomerRelatedHobbyButton.Text = WebWordUtility.GetWord(10126)
            Me.CustomerRelatedHobbyPopupTitlePage1.Value = WebWordUtility.GetWord(10127)
            Me.CustomerRelatedHobbyPopupTitlePage2.Value = WebWordUtility.GetWord(10128)
            Me.HobbyOtherErrorMessage.Value = WebWordUtility.GetWord(10907)


            Dim selDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
                = CType(dt.Select("SELECTION = '1' AND SORTNO_1ST = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())

            If selDrs.Count = 0 Then
                Me.CustomerRelatedHobbySelectedNewPanel.Visible = True
                Me.CustomerRelatedHobbySelectedEditPanel.Visible = False
            Else
                Me.CustomerRelatedHobbySelectedNewPanel.Visible = False
                Me.CustomerRelatedHobbySelectedEditPanel.Visible = True

                Me.HobbyCountLabel.Text = HttpUtility.HtmlEncode(selDrs.Count)
                Me.CustomerRelatedHobbySelectedImage.BackImageUrl = ResolveClientUrl(selDrs(0).ICONPATH_VIEWONLY)
                Me.CustomerRelatedHobbySelectedLabel.Text = HttpUtility.HtmlEncode(selDrs(0).HOBBY)

            End If

            Dim selOtherDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
                = CType(dt.Select("SELECTION = '1' AND SORTNO_1ST = '2'"), SC3080201DataSet.SC3080201CustomerHobbyRow())
            If selDrs.Count = 1 And selOtherDrs.Count = 1 Then
                'その他のみ選択の場合
                Me.CustomerRelatedHobbySelectedLabel.Text = HttpUtility.HtmlEncode(selOtherDrs(0).HOBBY)
            End If

            Dim otherDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
                = CType(dt.Select("SORTNO_1ST = '1' AND OTHER = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())

            If otherDrs.Count() = 0 Then
                Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = ""
                Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = ""
            Else
                Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = CStr(otherDrs(0).HOBBYNO)
                Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = otherDrs(0).HOBBY
            End If


            '取得件数保持
            Dim selCountDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
                = CType(dt.Select("SORTNO_1ST = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())
            CustomerRelatedHobbyPopupRowCount.Value = CStr(selCountDrs.Count())

            '取得したデータの編集
            dt = SC3080201BusinessLogic.EditHobbyData(dt)
            'その他テキストボックス初期化
            Me.CustomerRelatedHobbyPopupOtherText.Text = String.Empty

            Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataSource = dt
            Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataBind()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 画面設定 趣味ポップアップ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub CustomerRelatedHobbyPopupSelectButtonRepeater_ItemDataBound1(sender As Object, e As RepeaterItemEventArgs) _
        Handles CustomerRelatedHobbyPopupSelectButtonRepeater.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim dr As SC3080201DataSet.SC3080201CustomerHobbyRow = _
                    CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerHobbyRow)

                With CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonPanel_Row"), Panel)

                    If dr.SELECTION.Equals("1") Then
                        .BackImageUrl = ResolveClientUrl(dr.ICONPATH_SELECTED)
                    Else
                        .BackImageUrl = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)
                    End If

                    If dr.OTHER.Equals("1") Then
                        .Attributes.Add("onclick", "setCustomerRelatedHobbyPopupPage('page2','" & e.Item.ItemIndex & "');")
                    Else
                        .Attributes.Add("onclick", "selectCustomerRelatedHobbyPopupButton('" & e.Item.ItemIndex & "');")
                    End If

                End With

                With CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row"), Label)
                    .Text = HttpUtility.HtmlEncode(dr.HOBBY)
                    If dr.SELECTION.Equals("1") Then
                        .CssClass = "selectedButton ellipsis"
                    End If
                End With
                If dr.OTHER.Equals("1") Then
                    CustomerRelatedHobbyPopupOtherHiddenField.Value = dr.HOBBY
                End If

                CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonOther_Row"), HiddenField).Value = dr.OTHER
                CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row"), HiddenField).Value = CStr(dr.HOBBYNO)
                CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonCheck_Row"), HiddenField).Value = dr.SELECTION
                CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectedButtonPath_Row"), HiddenField).Value = ResolveClientUrl(dr.ICONPATH_SELECTED)
                CType(e.Item.FindControl("CustomerRelatedHobbyPopupNotSelectedButtonPath_Row"), HiddenField).Value = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "趣味登録押下イベント"

    ''' <summary>
    ''' 趣味登録押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub RegisterCustomerRelatedHobbyButton_Click(sender As Object, e As System.EventArgs) Handles RegisterCustomerRelatedHobbyButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '禁則文字チェック
            If Not Validation.IsValidString(Me.CustomerRelatedHobbyPopupOtherHiddenField.Value) Then
                ShowMessageBox(ERRMSGID_10911)
            Else
                '登録処理
                Me.RegistCustomerRelatedHobby()
            End If

            'ポップアップ再表示
            Me.CustomerRelatedHobbyLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 趣味ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub CustomerRelatedHobbyLoad()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

            '顧客趣味取得
            Dim hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable _
                = SC3080201BusinessLogic.GetHobbyData(params)

            Me._SetCustomerRelatedHobbyArea(hobbyDataTbl)

            'CustomTextBox
            JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#CustomerRelatedHobbyPopupOtherText').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "趣味登録処理"

    ''' <summary>
    ''' 趣味登録処理
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub RegistCustomerRelatedHobby()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Using dt As New SC3080201DataSet.SC3080201InsertCstHobbyDataTable
                For i As Integer = 0 To CInt(CustomerRelatedHobbyPopupRowCount.Value) - 1
                    If String.Equals(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonCheck_Row"), HiddenField).Value, "1") Then
                        '選択されているもののみ登録
                        Dim drInsert As SC3080201DataSet.SC3080201InsertCstHobbyRow = dt.NewSC3080201InsertCstHobbyRow

                        drInsert.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                        drInsert.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                        drInsert.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                        drInsert.HOBBYNO = CInt(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row"), HiddenField).Value)

                        If String.Equals(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonOther_Row"), HiddenField).Value, "1") Then
                            'その他の場合
                            drInsert.OTHERHOBBY = CustomerRelatedHobbyPopupOtherHiddenField.Value
                        Else
                            drInsert.OTHERHOBBY = String.Empty
                        End If

                        dt.Rows.Add(drInsert)

                    End If
                Next i

                Dim bizClass As New SC3080201BusinessLogic
                If dt.Rows.Count = 0 Then
                    '削除のみ行う
                    Dim drDelete As SC3080201DataSet.SC3080201InsertCstHobbyRow = dt.NewSC3080201InsertCstHobbyRow
                    drDelete.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                    drDelete.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                    drDelete.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                    dt.Rows.Add(drDelete)

                    bizClass.InsertCstHobby(dt)
                Else
                    bizClass.InsertCstHobby(dt)

                End If
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "趣味キャンセル押下処理"

    ''' <summary>
    ''' 趣味キャンセル押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedHobbyPopupCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedHobbyPopupCancelButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            'ポップアップ再表示
            Me.CustomerRelatedHobbyLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#End Region

#Region "希望連絡方法ポップアップ作成"

#Region "希望連絡方法設定"

    ''' <summary>
    ''' 希望連絡方法設定
    ''' </summary>
    ''' <param name="contactFlgTbl"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedContactPopupContactTool(ByVal contactFlgTbl As SC3080201DataSet.SC3080201ContactFlgDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In contactFlgTbl Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            Dim drContactFlg As SC3080201DataSet.SC3080201ContactFlgRow = CType(contactFlgTbl.Rows(0), SC3080201DataSet.SC3080201ContactFlgRow)

            Dim mobileFlg As String = drContactFlg.CONTACTMOBILEFLG
            Dim homeFlg As String = drContactFlg.CONTACTHOMEFLG
            Dim smsFlg As String = drContactFlg.CONTACTSMSFLG
            Dim emailFlg As String = drContactFlg.CONTACTEMAILFLG
            Dim dmFlg As String = drContactFlg.CONTACTDMFLG

            If mobileFlg.Equals(ContactFlgOn) OrElse _
                homeFlg.Equals(ContactFlgOn) OrElse _
                smsFlg.Equals(ContactFlgOn) OrElse _
                emailFlg.Equals(ContactFlgOn) OrElse _
                dmFlg.Equals(ContactFlgOn) Then
                '希望連絡方法が一つでも選択済み

                CustomerRelatedContactMobileLabel.Visible = False
                CustomerRelatedContactHomeLabel.Visible = False
                CustomerRelatedContactShortMessageServiceLabel.Visible = False
                CustomerRelatedContactEmailLabel.Visible = False
                CustomerRelatedContactDMLabel.Visible = False
                CustomerRelatedContactTelImage.Visible = False
                CustomerRelatedContactMailImage.Visible = False

                If mobileFlg.Equals(ContactFlgOn) OrElse homeFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactTelImage.Visible = True

                    If mobileFlg.Equals(ContactFlgOn) Then
                        CustomerRelatedContactTelImage.ImageUrl = ImageFilePath & CONTACTIMAGE_MOBILE
                        CustomerRelatedContactMobileLabel.Visible = True
                    ElseIf homeFlg.Equals(ContactFlgOn) Then
                        CustomerRelatedContactTelImage.ImageUrl = ImageFilePath & CONTACTIMAGE_HOME
                        CustomerRelatedContactHomeLabel.Visible = True
                    End If
                End If

                If smsFlg.Equals(ContactFlgOn) OrElse emailFlg.Equals(ContactFlgOn) OrElse dmFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactMailImage.Visible = True

                    If smsFlg.Equals(ContactFlgOn) Then
                        CustomerRelatedContactMailImage.ImageUrl = ImageFilePath & CONTACTIMAGE_SMS
                        CustomerRelatedContactShortMessageServiceLabel.Visible = True
                    ElseIf emailFlg.Equals(ContactFlgOn) Then
                        CustomerRelatedContactMailImage.ImageUrl = ImageFilePath & CONTACTIMAGE_EMAIL
                        CustomerRelatedContactEmailLabel.Visible = True
                    ElseIf dmFlg.Equals(ContactFlgOn) Then
                        CustomerRelatedContactMailImage.ImageUrl = ImageFilePath & CONTACTIMAGE_DM
                        CustomerRelatedContactDMLabel.Visible = True
                    End If
                End If

                Me.CustomerRelatedContactSelectedNewPanel.Visible = False
                Me.CustomerRelatedContactSelectedEditPanel.Visible = True

            Else
                '希望連絡方法が選択されていない
                Me.CustomerRelatedContactSelectedNewPanel.Visible = True
                Me.CustomerRelatedContactSelectedEditPanel.Visible = False
            End If

            Dim selectedCssClass As String = "scNscPopUpContactSelectBtnMiddleOn"
            Dim selectedString As String = "selected"

            'Dim mobileIconFileName As String = "scNscContactSelectIconMobile"
            If mobileFlg.Equals(ContactFlgOn) Then
                Me.ContactToolMobileLI.Attributes.Add(ClassString, selectedCssClass)
                Me.ContactToolMobileHidden.Value = ContactFlgOn
                Me.ContactToolMobileImage.CssClass = selectedString & " ContactToolIcon"
            Else
                Me.ContactToolMobileLI.Attributes.Remove(ClassString)
                Me.ContactToolMobileHidden.Value = ContactFlgOff
                Me.ContactToolMobileImage.CssClass = "ContactToolIcon"
            End If

            'Dim homeIconFileName As String = "scNscContactSelectIconTel"
            If homeFlg.Equals(ContactFlgOn) Then
                Me.ContactToolTelLI.Attributes.Add(ClassString, selectedCssClass)
                Me.ContactToolTelHidden.Value = ContactFlgOn
                Me.ContactToolTelImage.CssClass = selectedString & " ContactToolIcon"
            Else
                Me.ContactToolTelLI.Attributes.Remove(ClassString)
                Me.ContactToolTelHidden.Value = ContactFlgOff
                Me.ContactToolTelImage.CssClass = "ContactToolIcon"
            End If

            'Dim smsIconFileName As String = "scNscContactSelectIconSMS"
            If smsFlg.Equals(ContactFlgOn) Then
                Me.ContactToolShortMessageServiceLI.Attributes.Add(ClassString, selectedCssClass)
                Me.ContactToolShortMessageServiceHidden.Value = ContactFlgOn
                Me.ContactToolShortMessageServiceImage.CssClass = selectedString & " ContactToolIcon"
            Else
                Me.ContactToolShortMessageServiceLI.Attributes.Remove(ClassString)
                Me.ContactToolShortMessageServiceHidden.Value = ContactFlgOff
                Me.ContactToolShortMessageServiceImage.CssClass = "ContactToolIcon"
            End If

            'Dim emailIconFileName As String = "scNscContactSelectIconEmail"
            If emailFlg.Equals(ContactFlgOn) Then
                Me.ContactToolEmailLI.Attributes.Add(ClassString, selectedCssClass)
                Me.ContactToolEmailHidden.Value = ContactFlgOn
                Me.ContactToolEmailImage.CssClass = selectedString & " ContactToolIcon"
            Else
                Me.ContactToolEmailLI.Attributes.Remove(ClassString)
                Me.ContactToolEmailHidden.Value = ContactFlgOff
                Me.ContactToolEmailImage.CssClass = "ContactToolIcon"
            End If

            'Dim dmIconFileName As String = "scNscContactSelectIconDM"
            If dmFlg.Equals(ContactFlgOn) Then
                Me.ContactToolDirectMailLI.Attributes.Add(ClassString, selectedCssClass)
                Me.ContactToolDirectMailHidden.Value = ContactFlgOn
                Me.ContactToolDirectMailImage.CssClass = selectedString & " ContactToolIcon"
            Else
                Me.ContactToolDirectMailLI.Attributes.Remove(ClassString)
                Me.ContactToolDirectMailHidden.Value = ContactFlgOff
                Me.ContactToolDirectMailImage.CssClass = "ContactToolIcon"
            End If

        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "希望連絡曜日設定"

    ''' <summary>
    ''' 希望連絡曜日設定
    ''' </summary>
    ''' <param name="monFlg"></param>
    ''' <param name="tueFlg"></param>
    ''' <param name="wedFlg"></param>
    ''' <param name="turFlg"></param>
    ''' <param name="friFlg"></param>
    ''' <param name="satFlg"></param>
    ''' <param name="sunFlg"></param>
    ''' <param name="timeZoneClass"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedContactPopupWeekOfDay(ByVal monFlg As String, _
                                                        ByVal tueFlg As String, _
                                                        ByVal wedFlg As String, _
                                                        ByVal turFlg As String, _
                                                        ByVal friFlg As String, _
                                                        ByVal satFlg As String, _
                                                        ByVal sunFlg As String, _
                                                        ByVal timeZoneClass As Integer)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "monFlg = {0}", monFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "tueFlg = {0}", tueFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "wedFlg = {0}", wedFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "turFlg = {0}", turFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "friFlg = {0}", friFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "satFlg = {0}", satFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "sunFlg = {0}", sunFlg))
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "timeZoneClass = {0}", timeZoneClass))
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try

            Dim selectedCssClass As String = "scNscPopUpDaySelectBtnSmallOn"
            'Dim selectedString As String = "selected"
            Dim timeZone As String = CStr(timeZoneClass)

            If monFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "MonLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "MonHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "MonLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "MonHidden"), HiddenField).Value = ContactFlgOff
            End If

            If tueFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "TueLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "TueHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "TueLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "TueHidden"), HiddenField).Value = ContactFlgOff
            End If

            If wedFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "WedLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "WedHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "WedLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "WedHidden"), HiddenField).Value = ContactFlgOff
            End If

            If turFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "ThuLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "ThuHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "ThuLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "ThuHidden"), HiddenField).Value = ContactFlgOff
            End If

            If friFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "FriLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "FriHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "FriLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "FriHidden"), HiddenField).Value = ContactFlgOff
            End If

            If satFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "SatLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "SatHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "SatLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "SatHidden"), HiddenField).Value = ContactFlgOff
            End If

            If sunFlg.Equals(ContactFlgOn) Then
                CType(Me.FindControl("ContactWeek" & timeZone & "SunLI"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
                CType(Me.FindControl("ContactWeek" & timeZone & "SunHidden"), HiddenField).Value = ContactFlgOn
            Else

                CType(Me.FindControl("ContactWeek" & timeZone & "SunLI"), HtmlGenericControl).Attributes.Remove(ClassString)
                CType(Me.FindControl("ContactWeek" & timeZone & "SunHidden"), HiddenField).Value = ContactFlgOff
            End If

        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "希望連絡曜日、時間設定"

    ''' <summary>
    ''' 希望連絡曜日、時間設定
    ''' </summary>
    ''' <param name="timeZoneDataTbl"></param>
    ''' <param name="weekOfDayDataTbl"></param>
    ''' <param name="timeZoneClass"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub _SetCustomerRelatedContactPopup(ByVal timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable, _
                                                ByVal weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable, _
                                                ByVal timeZoneClass As Integer)


        ''引数をログに出力
        Dim argsIN As New List(Of String)
        For i As Integer = 0 To timeZoneDataTbl.Rows.Count - 1
            Dim rowIN As DataRow = timeZoneDataTbl.Rows(i)
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "timeZoneDataTbl.ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        For i As Integer = 0 To weekOfDayDataTbl.Rows.Count - 1
            Dim rowIN As DataRow = weekOfDayDataTbl.Rows(i)
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "weekOfDayDataTbl.ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        argsIN.Add(String.Format(CultureInfo.CurrentCulture, "timeZoneClass = {0}", timeZoneClass))
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try

            If weekOfDayDataTbl.Rows.Count = 1 Then
                Dim drWeekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayRow = CType(weekOfDayDataTbl.Rows(0), SC3080201DataSet.SC3080201ContactWeekOfDayRow)
                Me._SetCustomerRelatedContactPopupWeekOfDay(drWeekOfDayDataTbl.MONDAY, _
                                                            drWeekOfDayDataTbl.TUESWDAY, _
                                                            drWeekOfDayDataTbl.WEDNESDAY, _
                                                            drWeekOfDayDataTbl.THURSDAY, _
                                                            drWeekOfDayDataTbl.FRIDAY, _
                                                            drWeekOfDayDataTbl.SATURDAY, _
                                                            drWeekOfDayDataTbl.SUNDAY, _
                                                            timeZoneClass)
            Else
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "MonHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "TueHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "WedHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "ThuHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "FriHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "SatHidden"), HiddenField).Value = ContactFlgOff
                CType(Me.FindControl("ContactWeek" & timeZoneClass & "SunHidden"), HiddenField).Value = ContactFlgOff
            End If

            Me.ContactHeaderRegisterLinkButton.Text = WebWordUtility.GetWord(10126)
            Me.ContactErrMsg.Value = WebWordUtility.GetWord(10908)

            '希望連絡曜日設定
            CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Count"), HiddenField).Value = CStr(timeZoneDataTbl.Rows.Count)
            CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Repeater"), Repeater).DataSource = timeZoneDataTbl
            CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Repeater"), Repeater).DataBind()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 希望連絡曜日、時間設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub ContactTime1Repeater_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles ContactTime1Repeater.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim dr As SC3080201DataSet.SC3080201ContactTimeZoneRow = _
                    CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201ContactTimeZoneRow)

                CType(e.Item.FindControl("ContactTime1Label_Row"), CustomLabel).Text = HttpUtility.HtmlEncode(dr.CONTACTTIMEZONETITLE)

                With CType(e.Item.FindControl("ContactTime1Li_Row"), HtmlGenericControl)
                    .Attributes.Item("onclick") = "selectContactTime(1," & e.Item.ItemIndex & ");"
                    CType(e.Item.FindControl("ContactTimeZoneNo1Hidden_Row"), HiddenField).Value = CStr(dr.CONTACTTIMEZONENO)
                    If dr.CONTACTTIMEZONESELECT.Equals("1") Then
                        .Attributes.Item("class") = "scNscPopUpContactSelectBtnMiddleOn"
                        CType(e.Item.FindControl("ContactTime1Hidden_Row"), HiddenField).Value = ContactFlgOn
                    Else
                        CType(e.Item.FindControl("ContactTime1Hidden_Row"), HiddenField).Value = ContactFlgOff
                    End If
                End With

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 希望連絡曜日、時間設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub ContactTime2Repeater_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles ContactTime2Repeater.ItemDataBound
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            If e Is Nothing Then
                Return
            End If

            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim dr As SC3080201DataSet.SC3080201ContactTimeZoneRow = _
                    CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201ContactTimeZoneRow)

                CType(e.Item.FindControl("ContactTime2Label_Row"), CustomLabel).Text = HttpUtility.HtmlEncode(dr.CONTACTTIMEZONETITLE)

                With CType(e.Item.FindControl("ContactTime2Li_Row"), HtmlGenericControl)
                    .Attributes.Item("onclick") = "selectContactTime(2," & e.Item.ItemIndex & ");"
                    CType(e.Item.FindControl("ContactTimeZoneNo2Hidden_Row"), HiddenField).Value = CStr(dr.CONTACTTIMEZONENO)
                    If dr.CONTACTTIMEZONESELECT.Equals("1") Then
                        .Attributes.Item("class") = "scNscPopUpContactSelectBtnMiddleOn"
                        CType(e.Item.FindControl("ContactTime2Hidden_Row"), HiddenField).Value = ContactFlgOn
                    Else
                        CType(e.Item.FindControl("ContactTime2Hidden_Row"), HiddenField).Value = ContactFlgOff
                    End If
                End With

            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "希望連絡方法登録押下イベント"

    ''' <summary>
    ''' 希望連絡方法登録押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Protected Sub ContactHeaderRegisterLinkButton_Click(sender As Object, e As System.EventArgs) Handles ContactHeaderRegisterLinkButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '登録処理
            Me.RegistCustomerRelatedContact()
            'ポップアップ再表示
            Me.CustomerRelatedContacLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 希望コンタクト方法ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    '''
    ''' <history>
    ''' </history>
    Private Sub CustomerRelatedContacLoad()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

            '希望コンタクト方法取得
            Dim contactFlg As SC3080201DataSet.SC3080201ContactFlgDataTable _
                = SC3080201BusinessLogic.GetContactFlg(params)
            Me._SetCustomerRelatedContactPopupContactTool(contactFlg)

            'TIMEZONECLASS分ループ
            For i = TIMEZONECLASS_1 To TIMEZONECLASS_2
                params.Rows(0).Item(params.TIMEZONECLASSColumn.ColumnName) = i

                '希望連絡時間帯取得
                Dim timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable _
                    = SC3080201BusinessLogic.GetTimeZoneData(params)

                '希望連絡曜日取得
                Dim weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable _
                    = SC3080201BusinessLogic.GetWeekOfDayData(params)
                'ポップアップ画面設定
                Me._SetCustomerRelatedContactPopup(timeZoneDataTbl, weekOfDayDataTbl, i)
            Next
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "希望連絡方法登録処理"

    ''' <summary>
    ''' 希望連絡方法登録処理
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Sub RegistCustomerRelatedContact()
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            Using dtInfo As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable
                Dim drInfo As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtInfo.NewSC3080201InsertCstContactInfoRow

                '基本情報
                drInfo.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                drInfo.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                '連絡方法
                drInfo.CONTACTMOBILEFLG = ContactToolMobileHidden.Value
                drInfo.CONTACTHOMEFLG = ContactToolTelHidden.Value
                drInfo.CONTACTSMSFLG = ContactToolShortMessageServiceHidden.Value
                drInfo.CONTACTEMAILFLG = ContactToolEmailHidden.Value
                drInfo.CONTACTDMFLG = ContactToolDirectMailHidden.Value

                dtInfo.Rows.Add(drInfo)
                '希望コンタクト方法登録処理

                Dim bizClass As New SC3080201BusinessLogic
                bizClass.InsertCstContactInfo(dtInfo)

                Using dtWeekOfDay As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable
                    Using dtTime As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable

                        '時間帯クラスの数だけループ
                        For i As Integer = TIMEZONECLASS_1 To TIMEZONECLASS_2

                            Dim drWeekOfDay As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtWeekOfDay.NewSC3080201InsertCstContactInfoRow
                            '基本情報
                            drWeekOfDay.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                            drWeekOfDay.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                            drWeekOfDay.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                            '時間帯クラス
                            drWeekOfDay.TIMEZONECLASS = i
                            '連絡曜日
                            drWeekOfDay.MONDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "MonHidden"), HiddenField).Value
                            drWeekOfDay.TUESWDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "TueHidden"), HiddenField).Value
                            drWeekOfDay.WEDNESDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "WedHidden"), HiddenField).Value
                            drWeekOfDay.THURSDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "ThuHidden"), HiddenField).Value
                            drWeekOfDay.FRIDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "FriHidden"), HiddenField).Value
                            drWeekOfDay.SATURDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "SatHidden"), HiddenField).Value
                            drWeekOfDay.SUNDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "SunHidden"), HiddenField).Value

                            dtWeekOfDay.Rows.Add(drWeekOfDay)

                            '希望連絡時間件数だけループ
                            Dim count As Integer = CInt(CType(Me.FindControl("ContactTime" & CStr(i) & "Count"), HiddenField).Value)
                            For j As Integer = 0 To count - 1

                                Dim drTimeInsert As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtTime.NewSC3080201InsertCstContactInfoRow
                                '時間帯クラス
                                drTimeInsert.TIMEZONECLASS = i
                                '基本情報
                                drTimeInsert.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                                drTimeInsert.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                                drTimeInsert.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

                                '連絡時間帯
                                Select Case i
                                    Case TIMEZONECLASS_1
                                        If String.Equals(CType(ContactTime1Repeater.Items(j).FindControl("ContactTime1Hidden_Row"), HiddenField).Value, ContactFlgOn) Then
                                            ' ビルドを通す為一時的に修正 START
                                            drTimeInsert.CONTACTTIMEZONENO = CType(ContactTime1Repeater.Items(j).FindControl("ContactTimeZoneNo1Hidden_Row"), HiddenField).Value
                                            ' ビルドを通す為一時的に修正 END
                                            dtTime.Rows.Add(drTimeInsert)
                                        End If
                                    Case TIMEZONECLASS_2
                                        If String.Equals(CType(ContactTime2Repeater.Items(j).FindControl("ContactTime2Hidden_Row"), HiddenField).Value, ContactFlgOn) Then
                                            ' ビルドを通す為一時的に修正 START
                                            drTimeInsert.CONTACTTIMEZONENO = CType(ContactTime2Repeater.Items(j).FindControl("ContactTimeZoneNo2Hidden_Row"), HiddenField).Value
                                            ' ビルドを通す為一時的に修正 END
                                            dtTime.Rows.Add(drTimeInsert)
                                        End If
                                End Select
                            Next j
                        Next i
                        '希望連絡曜日登録処理
                        bizClass.InsertCstContactWeekOfDay(dtWeekOfDay)
                        '希望連絡時間登録処理
                        If dtTime.Rows.Count = 0 Then
                            '削除のみ行う
                            Dim drTimeDelete As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtTime.NewSC3080201InsertCstContactInfoRow
                            '基本情報
                            drTimeDelete.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                            drTimeDelete.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                            drTimeDelete.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                            dtTime.Rows.Add(drTimeDelete)
                            bizClass.InsertCstContactTime(dtTime)
                        Else
                            bizClass.InsertCstContactTime(dtTime)
                        End If
                    End Using
                End Using
            End Using
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#Region "希望連絡方法キャンセル押下処理"
    ''' <summary>
    ''' 希望連絡方法キャンセル押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Protected Sub CustomerRelatedContactPopupCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedContactPopupCancelButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            'ポップアップ再表示
            Me.CustomerRelatedContacLoad()
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#End Region

#Region "画面設定 メモ"
    ''' <summary>
    ''' 画面設定 メモ
    ''' </summary>
    ''' <param name="lastCustMemoTbl"></param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Sub ControltimeLastCustMemo(ByVal lastCustMemoTbl As SC3080201DataSet.SC3080201LastCustomerMemoDataTable)
        ''引数をログに出力
        Dim argsIN As New List(Of String)
        Dim i As Integer = 0
        For Each rowIN As DataRow In From r As DataRow In lastCustMemoTbl Where r.RowState <> DataRowState.Deleted
            i += 1
            argsIN.Add(String.Format(CultureInfo.CurrentCulture, "ROW = {0}", i))
            For Each column As DataColumn In rowIN.Table.Columns
                If rowIN.IsNull(column.ColumnName) = True Then
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    argsIN.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, rowIN(column.ColumnName)))
                End If
            Next
        Next
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN:{2}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , String.Join(", ", argsIN.ToArray())))
        Try
            If lastCustMemoTbl.Rows.Count >= 1 Then
                Dim lastCustMemoDataRow As SC3080201DataSet.SC3080201LastCustomerMemoRow
                lastCustMemoDataRow = CType(lastCustMemoTbl.Rows(0), SC3080201DataSet.SC3080201LastCustomerMemoRow)

                '更新日
                CustomerMemoDayLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                           lastCustMemoDataRow.UPDATEDATE, _
                                                                           StaffContext.Current.DlrCD))
                'メモ
                CustomerMemoLabel.Text = lastCustMemoDataRow.MEMO
            Else
                '更新日
                CustomerMemoDayLabel.Text = HttpUtility.HtmlEncode("　")
                'メモ
                CustomerMemoLabel.Text = String.Empty
            End If
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

#End Region

#Region "その他ポップアップ関連"

#Region "顧客メモ遷移処理"
    ''' <summary>
    ''' 顧客メモオープン時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Protected Sub CustomerMemoEditOpenButton_Click(sender As Object, e As System.EventArgs) Handles CustomerMemoEditOpenButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            'スクリプトで顧客メモポップアップを起動
            JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Parent.Parent.Parent.Parent.Parent, BasePage), "commitCompleteOpenCustomerMemoEdit", "after")
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub

    ''' <summary>
    ''' 顧客メモクローズ時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Protected Sub CustomerMemoEditCloseButton_Click(sender As Object, e As System.EventArgs) Handles CustomerMemoEditCloseButton.Click
        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
        Try
            '最新顧客メモ取得、画面設定
            Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
            Me._ShowLastCustMemo(params)
        Finally
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End Try
    End Sub
#End Region

#End Region

#End Region

#Region " ページクラス処理のバイパス処理 "
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Sub SetValue(pos As ScreenPos, key As String, value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Function GetValue(pos As ScreenPos, key As String, removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Sub ShowMessageBox(wordNo As Integer, ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Function ContainsKey(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Sub RemoveValueBypass(pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, key As String)
        GetPageInterface().RemoveValueBypass(pos, key)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' </history>
    Public Sub RegistActivityAfter() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080201Control.RegistActivityAfter
        'コンタクト履歴取得、画面設定
        'Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        '_ShowContactHistory(params)
    End Sub
#End Region

End Class
