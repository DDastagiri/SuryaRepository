'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080203.aspx.vb
'─────────────────────────────────────
'機能： 顧客詳細(活動登録)
'補足： 
'作成：            TCS 河原 【SALES_1A】
'更新： 2012/03/07 TCS 河原 【SALES_2】
'更新： 2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222)
'更新： 2012/04/26 TCS 河原 HTMLエンコード対応
'更新： 2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/02/07 TCS 河原 GL0873
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2012/12/10 TCS 坪根 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 庄   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス）
'更新： 2013/12/03 TCS 市川 Aカード情報相互連携開発
'更新： 2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) 
'更新： 2015/12/10 TCS 鈴木 受注後工程蓋閉め対応
'更新： 2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3
'─────────────────────────────────────

'Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationManager
Imports Toyota.eCRB.CommonUtility.BizLogic
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
Imports Toyota.eCRB.CommonUtility.DataAccess
'2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

Partial Class Pages_SC3080203
    Inherits System.Web.UI.UserControl
    Implements ISC3080203Control

    ' メッセージID
    Dim msgid As String = 0

#Region " セッションキー "
    'Follow-upBoxのシーケンスNo.
    Private Const CONST_FLLWUPBOX_SEQNO As String = "SearchKey.FOLLOW_UP_BOX"

    'Follow-upBoxの店舗コード
    Private Const CONST_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"

    '顧客区分　自社客：1　未取引客：2
    Private Const CONST_CSTKIND As String = "SearchKey.CSTKIND"

    '顧客ID　自社客：自社客連番　未取引客：未取引客ユーザID
    Private Const CONST_INSDID As String = "SearchKey.CRCUSTID"

    '車両ID 自社客：VIN　未取引客：未取引客車両SeqNo
    Private Const CONST_VCLINFO As String = "VCLINFO"

    '表示ページ
    Private Const SESSION_KEY_DISPPAGE As String = "SearchKey.DISPPAGE"

    '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    '顧客名 + 敬称
    Private Const SESSION_KEY_NAME As String = "SearchKey.NAME"
    '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    '2013/02/07 TCS 河原 GL0873 START
    ''' <summary>商談中Follow-upBox内連番</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX_SALES As String = "SearchKey.FOLLOW_UP_BOX_SALES"

    ''' <summary>商談中Follow-upBox店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD_SALES As String = "SearchKey.FLLWUPBOX_STRCD_SALES"
    '2013/02/07 TCS 河原 GL0873 END

    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' 活動ID
    Private Const SESSION_KEY_ACT_ID As String = "SearchKey.ACT_ID"

    ' 用件ID
    Private Const SESSION_KEY_REQ_ID As String = "SearchKey.REQ_ID"

    ' 誘致ID
    Private Const SESSION_KEY_ATT_ID As String = "SearchKey.ATT_ID"

    ' 活動回数
    Private Const SESSION_KEY_COUNT As String = "SearchKey.COUNT"

    ' 用件ロックバージョン
    Private Const SESSION_KEY_REQUEST_LOCK_VERSION As String = "SearchKey.REQUEST_LOCK_VERSION"

    ' 誘致ロックバージョン
    Private Const SESSION_KEY_ATTRACT_LOCK_VERSION As String = "SearchKey.ATTRACT_LOCK_VERSION"

    ' 商談ロックバージョン
    Private Const SESSION_KEY_SALES_LOCK_VERSION As String = "SearchKey.SALES_LOCK_VERSION"

    Private Const SESSION_KEY_PRICE As String = "SearchKey.PRICE"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

#End Region

#Region " 定数 "
    ''' <summary>
    ''' カタログ用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_CATALOG As Integer = 9

    ''' <summary>
    ''' 試乗用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_TESTDRIVE As Integer = 16

    ''' <summary>
    ''' 査定用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_ASSESSMENT As Integer = 18

    ''' <summary>
    ''' 見積り用SEQ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONTENT_SEQ_VALUATION As Integer = 10

    Private Const SUBMENU_TESTDRIVE As Integer = 201
    Private Const SUBMENU_VAL As Integer = 202
    Private Const SUBMENU_HELP As Integer = 203


    ''' <summary>
    ''' 画面で選択する活動結果
    ''' </summary>
    ''' <remarks></remarks>
    Public Const C_RSLT_WALKIN As String = "1"
    Public Const C_RSLT_PROSPECT As String = "2"
    Public Const C_RSLT_HOT As String = "3"
    Public Const C_RSLT_SUCCESS As String = "4"
    Public Const C_RSLT_GIVEUP As String = "5"

    ''' <summary>
    ''' 在席状態：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STAFF_STATUS_NEGOTIATION As String = "20"

    ''' <summary>
    ''' メッセージ出力時の日付フォーマット 年月日時分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_FORMAT_YMDHS_CONVID As Integer = 2

    ''' <summary>
    ''' メッセージＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_30932 As Integer = 30932
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    Private Const MSG_30933 As Integer = 30933
    Private Const MSG_30934 As Integer = 30934
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    Private Const MSG_40902 As Integer = 40902
    Private Const MSG_40907 As Integer = 40907
    Private Const MSG_CSTINFO_INPUTERR_START As Integer = 40949
    Private Const MSG_CSTINFO_INPUTERR_END As Integer = 40974
    Private Const MSG_SALESINFO_INPUTERR_START As Integer = 20901
    Private Const MSG_SALESINFO_INPUTERR_END As Integer = 20916

    '2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3 START
    Private Const MSG_4000901 As Integer = 4000901
    Private Const MSG_4000999 As Integer = 4000999
    '2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3 END

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
    Private Const MSG_20916 As Integer = 20916
    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

    ''' <summary>
    ''' ブランク/V4画面へ遷移後セッションの店舗コードがNullとなるため、セッションから取得できない場合はブランクを設定するように修正
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BLANK = " "

#End Region

    '2015/12/10 受注後工程蓋閉め対応 鈴木 START
#Region " プロパティ類 "
    '契約状況フラグ
    Private _contractStatusFlg As String
    Public Property ContractStatusFlg() As String
        Get
            Return _contractStatusFlg
        End Get
        Set(ByVal value As String)
            _contractStatusFlg = value
        End Set
    End Property
#End Region
    '2015/12/10 受注後工程蓋閉め対応 鈴木 END

    ''' <summary>
    ''' 初期表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--デバッグログ---------------------------------------------------
        Logger.Info("Page_Load Start")
        '-----------------------------------------------------------------

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        If Not Page.IsCallback Then
            If Me.Visible = False Then
                '--デバッグログ---------------------------------------------------
                Logger.Info("Page_Load End")
                '-----------------------------------------------------------------
                Return
            Else
                If Not Page.IsPostBack Then

                    'セッション情報取得
                    GetSessionValues()

                    '初期設定
                    InitDisplaySetting()

                End If

                '子画面の設定
                'InitSubDisplay()

            End If
        End If

        '--デバッグログ---------------------------------------------------
        Logger.Info("Page_Load End")
        '-----------------------------------------------------------------

    End Sub

    ''' <summary>
    ''' プレレンダー
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        If Not Page.IsCallback Then
            If Me.Visible Then
                '子画面の設定
                InitSubDisplay()
            End If
        End If
    End Sub

    ''' <summary>
    ''' セッション情報取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSessionValues()
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetSessionValues Start")
        '-----------------------------------------------------------------

        '顧客区分
        Me.Cstkind.Value = DirectCast(GetValue(ScreenPos.Current, CONST_CSTKIND, False), String)
        '活動先顧客コード
        Me.Insdid.Value = DirectCast(GetValue(ScreenPos.Current, CONST_INSDID, False), String)

        'フォローアップボックスSeqNo取得
        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
            Me.fllwSeq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
        Else
            Me.fllwSeq.Value = ""
        End If

        'フォローアップボックスの店舗コード
        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_STRCD) Then
            Me.fllwstrcd.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_STRCD, False), String)
        Else
            Me.fllwstrcd.Value = BLANK
        End If

        '1枚目で選んでいる車両
        If ContainsKey(ScreenPos.Current, CONST_VCLINFO) Then
            Me.Vclseq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_VCLINFO, False), String)
        Else
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.Vclseq.Value = "0"
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        End If

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetSessionValues End")
        '-----------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' 初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitDisplaySetting()
        '--デバッグログ---------------------------------------------------
        Logger.Info("InitDisplaySetting Start")
        '-----------------------------------------------------------------

        '2012/03/07 TCS 河原 【SALES_2】START

        '対応SC欄の設定
        'setStaffSelector()
        '2012/03/07 TCS 河原 【SALES_2】END

        'プロセス欄の車両情報を取得
        setSelectedCar()

        '2012/03/07 TCS 河原 【SALES_2】 START
        '活動方法取得
        'setActContact()
        '2012/03/07 TCS 河原 【SALES_2】 END

        'フォロー方法リスト作成
        GetFollowContact()

        '次回活動方法取得
        setNextActContact()

        'フォロー方法取得
        setFollowContact()

        '前回活動ステータス取得
        GetFollowCractstatus()

        '時間設定
        GetTime()

        '2012/03/07 TCS 河原 【SALES_2】 START
        'プロセスの文言取得
        'GetContentWord()
        '2012/03/07 TCS 河原 【SALES_2】 END

        'Give-up時の競合車種情報取得
        GetCompetition()

        'アラートデータ取得
        'GetAlert()

        '入力チェック用のエラー文言セット
        SetErrWord()

        '日付フォーマットセット
        GetDateFormat()

        ''アイコンパス取得
        'GetContentIconPath()

        '2012/03/07 TCS 河原 【SALES_2】 START
        'selectActAssesment.Value = ""
        GetNextActContactTitle()

        GetFollowContactTitle()

        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        '断念理由一覧の初期化
        InitGiveUpReasonPopup()

        '手動Success有効無効切り替え
        SetSuccessButton()
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

        '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
        SetResultButton()
        '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 END

        Sc3080218Page.SetInit()

        'Me.selectSelSeries.Value = ""
        Me.selectGiveupMaker.Value = ""
        Me.selectGiveupCar.Value = ""
        Me.selectGiveupReason.Value = ""
        Me.selectGiveupMakerWK.Value = ""
        Me.selectGiveupCarWK.Value = ""
        Me.selectGiveupCarName.Value = ""
        Me.selectGiveupMakerNameWK.Value = ""

        '2012/03/07 TCS 河原 【SALES_2】 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("InitDisplaySetting End")
        '-----------------------------------------------------------------
    End Sub

    '2012/03/07 TCS 河原 【SALES_2】START
    ' ''' <summary>
    ' ''' 対応SC欄のセット
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Protected Function setStaffSelector() As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("setStaffSelector Start")
    '    '-----------------------------------------------------------------
    '    Dim userary As String()
    '    Dim context As StaffContext = StaffContext.Current
    '    userary = Split(context.Account, "@")
    '    Me.selectStaff.Value = userary(0)
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("setStaffSelector End")
    '    '-----------------------------------------------------------------
    '    Return True
    'End Function
    '2012/03/07 TCS 河原 【SALES_2】END

    ''' <summary>
    ''' 選択車種を取得してプロセス用のリストを作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function setSelectedCar() As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("setSelectedCar Start")
        '-----------------------------------------------------------------

        '2012/03/07 TCS 河原 【SALES_2】START
        'Using Serchdt As New SC3080203DataSet.SC3080203DlrStrFollwDataTable
        Dim fllseq As Long
        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
            If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)) Then
                fllseq = 0
            Else
                fllseq = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
            End If
        Else
            fllseq = 0
        End If
        Dim carList As String = Nothing
        'Dim i As Integer = 0
        'Dim fllwseriesdt As SC3080203DataSet.SC3080203FllwSeriesDataTable
        'Dim fllwseriesrw As SC3080203DataSet.SC3080203FllwSeriesRow
        'fllwseriesdt = SC3080203BusinessLogic.GetFllwSeries(Me.fllwstrcd.Value, fllseq)
        'carList = ""
        'For i = 0 To fllwseriesdt.Count - 1
        '    fllwseriesrw = fllwseriesdt.Rows(i)
        '    carList = carList + CStr(fllwseriesrw.SEQNO)
        '    carList = carList + ",0;"
        'Next
        'Me.selectActCatalog.Value = carList
        'Me.selectActCatalogWK.Value = carList
        'Dim fllwmodeldt As SC3080203DataSet.SC3080203FllwModelDataTable
        'Dim fllwmodelrw As SC3080203DataSet.SC3080203FllwModelRow
        'fllwmodeldt = SC3080203BusinessLogic.GetFllwModel(Me.fllwstrcd.Value, fllseq)
        'carList = ""
        'For i = 0 To fllwmodeldt.Count - 1
        '    fllwmodelrw = fllwmodeldt.Rows(i)
        '    carList = carList + CStr(fllwmodelrw.SEQNO)
        '    carList = carList + ",0;"
        'Next
        'Me.selectActTestDrive.Value = carList
        'Me.selectActTestDriveWK.Value = carList


        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        'Dim fllwcolordt As SC3080203DataSet.SC3080203FllwColorDataTable
        'Dim fllwcolorrw As SC3080203DataSet.SC3080203FllwColorRow

        'fllwcolordt = SC3080203BusinessLogic.GetFllwColor(Me.fllwstrcd.Value, fllseq)

        Dim preferredCar As SC3080203DataSet.SC3080203PreferredCarDataTable
        preferredCar = SC3080203BusinessLogic.GetPreferredCar(Me.fllwstrcd.Value, fllseq)

        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END


        '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
        Dim dicSelectCar As Dictionary(Of String, String) = Nothing
        dicSelectCar = Me.GetSelectedCarDictionary(Me.selectSelSeries.Value)
        '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
        carList = ""

        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        'For i = 0 To fllwcolordt.Count - 1
        '    fllwcolorrw = fllwcolordt.Rows(i)
        '    carList = carList + CStr(fllwcolorrw.SEQNO)
        '    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
        '    'carList = carList + ",0;"
        '    If dicSelectCar.ContainsKey(CStr(fllwcolorrw.SEQNO)) Then
        '        carList = carList + ",1;"
        '    Else
        '        carList = carList + ",0;"
        '    End If
        '    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
        'Next

        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Dim estimateId As String
        Dim seqNo As String
        '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        For Each row In preferredCar
            '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
            If row.IS_EXISTS_ESTIMATE.Equals("1") Then
                estimateId = row.ESTIMATEID.ToString
            Else
                estimateId = "0"
            End If
            If row.IS_EXISTS_SELECTED_SERIES.Equals("1") Then
                seqNo = row.SEQNO.ToString
            Else
                seqNo = "0"
            End If
            '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            If dicSelectCar.ContainsKey(row.KEYVALUE.ToString) Then
                '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                'carList = carList & row.KEYVALUE.ToString & ",1," & row.IS_EXISTS_SELECTED_SERIES & "," & row.IS_EXISTS_ESTIMATE & ";"
                carList = carList & row.KEYVALUE.ToString & ",1," & row.IS_EXISTS_SELECTED_SERIES & "," & row.IS_EXISTS_ESTIMATE & "," & estimateId & "," & seqNo & ";"
                '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            Else
                '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
                'carList = carList & row.KEYVALUE.ToString & ",0," & row.IS_EXISTS_SELECTED_SERIES & "," & row.IS_EXISTS_ESTIMATE & ";"
                carList = carList & row.KEYVALUE.ToString & ",0," & row.IS_EXISTS_SELECTED_SERIES & "," & row.IS_EXISTS_ESTIMATE & "," & estimateId & "," & seqNo & ";"
                '2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
            End If
        Next

        '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

        'Me.selectActValuation.Value = carList
        'Me.selectActValuationWK.Value = carList
        '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
        If ContainsKey(ScreenPos.Current, SESSION_KEY_PRICE) Then

            If Not String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_PRICE, False), String)) Then
                Me.selectSelSeries.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_PRICE, False), String)
            Else
                Me.selectSelSeries.Value = carList
            End If
        Else
            Me.selectSelSeries.Value = carList
        End If
        '2013/06/30 TCS 趙 2013/10対応版 既存流用 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("setSelectedCar End")
        '-----------------------------------------------------------------

        Return True
        'End Using
        '2012/03/07 TCS 河原 【SALES_2】END
    End Function

    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START
    ''' <summary>
    ''' 選択されている車両のDictionaryを作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSelectedCarDictionary(ByVal carList As String) As Dictionary(Of String, String)
        Dim dic As New Dictionary(Of String, String)
        If String.IsNullOrEmpty(carList) Then
            Return dic
        End If

        Dim carArray() As String = Split(carList, ";")  '選択車両毎に分解

        For Each carInfo As String In carArray
            Dim car() As String = Split(carInfo, ",")   'SEQNOと選択状態に分解
            '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
            'If car.Length = 2 AndAlso String.IsNullOrEmpty(car(0)) = False Then
            If car.Length = 4 AndAlso String.IsNullOrEmpty(car(0)) = False Then
                '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
                If car(1) = "1" AndAlso dic.ContainsKey(car(0)) = False Then
                    '選択されている場合、Dictinaryに追加
                    dic.Add(car(0), car(1))
                End If
            End If
        Next
        Return dic
    End Function

    '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END
    '2012/03/07 TCS 河原 【SALES_2】START
    ' ''' <summary>
    ' ''' 今回活動方法取得
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Protected Function setActContact() As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("setActContact Start")
    '    '-----------------------------------------------------------------
    '    'スタッフ状況を取得
    '    Dim staffInfo As StaffContext = StaffContext.Current
    '    Dim staffStatus As String = staffInfo.PresenceCategory & staffInfo.PresenceDetail
    '    '活動方法の一覧を取得
    '    Dim actContactDt As SC3080203DataSet.SC3080203ActContactDataTable = Nothing
    '    actContactDt = SC3080203BusinessLogic.GetActContact()
    '    Dim actContactRw As SC3080203DataSet.SC3080203ActContactRow = actContactDt.Rows(0)
    '    '初期選択値を探す
    '    For Each dr As SC3080203DataSet.SC3080203ActContactRow In actContactDt.Rows
    '        If String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_WALKIN, "1") Then
    '            '商談中の場合、初期選択(商談)のレコードを探す
    '            actContactRw = dr
    '            Exit For
    '        ElseIf Not String.Equals(staffStatus, STAFF_STATUS_NEGOTIATION) AndAlso String.Equals(dr.FIRSTSELECT_NOTWALKIN, "1") Then
    '            '商談中以外の場合、初期選択(営業活動)のレコードを探す
    '            actContactRw = dr
    '            Exit For
    '        End If
    '    Next
    '    Me.selectActContact.Value = CStr(actContactRw.CONTACTNO)
    '    Me.ProcessFlg.Value = actContactRw.PROCESS
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("setActContact End")
    '    '-----------------------------------------------------------------
    '    Return True
    'End Function
    '2012/03/07 TCS 河原 【SALES_2】END

    ''' <summary>
    ''' 次回活動方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function setNextActContact() As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("setNextActContact Start")
        '-----------------------------------------------------------------

        Dim nextactcontactdt As SC3080203DataSet.SC3080203NextActContactDataTable
        nextactcontactdt = SC3080203BusinessLogic.GetNextActContact()
        Dim nextactcontactrw As SC3080203DataSet.SC3080203NextActContactRow
        nextactcontactrw = CType(nextactcontactdt.Rows(0), SC3080203DataSet.SC3080203NextActContactRow)
        Me.selectNextActContact.Value = nextactcontactrw.CONTACTNO.ToString()

        If String.Equals(nextactcontactrw.NEXTACTIVITY, "2") Then
            Me.FollowFlg.Value = "1"
            '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START
            Me.NextActivityFromToFlg.Value = "1"
            Me.NextActNextAct.Value = "2"
            '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START
        Else
            Me.FollowFlg.Value = "0"
            '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START
            Me.NextActivityFromToFlg.Value = "0"
            Me.NextActNextAct.Value = ""
            '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START
        End If

        '--デバッグログ---------------------------------------------------
        Logger.Info("setNextActContact End")
        '-----------------------------------------------------------------

        Return True
    End Function

    ''' <summary>
    ''' フォロー方法取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function setFollowContact() As Boolean

        '--デバッグログ---------------------------------------------------
        Logger.Info("setFollowContact Start")
        '-----------------------------------------------------------------

        'Dim followcontactdt As SC3080203DataSet.SC3080203FollowContactDataTable
        'followcontactdt = SC3080203BusinessLogic.GetFollowContact()
        'Dim followcontactrw As SC3080203DataSet.SC3080203FollowContactRow
        'followcontactrw = followcontactdt.Rows(0)
        Me.selectFollowContact.Value = "0"

        '--デバッグログ---------------------------------------------------
        Logger.Info("setFollowContact End")
        '-----------------------------------------------------------------

        Return True
    End Function

    ''' <summary>
    ''' Follow-upBoxの活動ステータス取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetFollowCractstatus() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowCractstatus Start")
        '-----------------------------------------------------------------
        Using Serchdt As New SC3080203DataSet.SC3080203DlrStrFollwDataTable
            Dim Serchrw As SC3080203DataSet.SC3080203DlrStrFollwRow
            Serchrw = Serchdt.NewSC3080203DlrStrFollwRow
            Dim context As StaffContext = StaffContext.Current
            Serchrw.DLRCD = context.DlrCD
            Serchrw.STRCD = context.BrnCD

            If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
                If String.IsNullOrEmpty(DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)) Then
                    Serchrw.FLLWUPBOX_SEQNO = 0
                Else
                    Serchrw.FLLWUPBOX_SEQNO = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
                End If
            Else
                Serchrw.FLLWUPBOX_SEQNO = 0
            End If

            Serchdt.Rows.Add(Serchrw)
            Dim followstatusdt As SC3080203DataSet.SC3080201FollowStatusDataTable
            followstatusdt = SC3080203BusinessLogic.GetFollowCractstatus(Serchdt)
            Dim followstatusrw As SC3080203DataSet.SC3080201FollowStatusRow
            If followstatusdt.Count > 0 Then
                followstatusrw = followstatusdt.Rows(0)
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                Me.fllwStatus.Value = followstatusrw.CRACTRESULT
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

                '活動結果の初期選択をHidden値へ反映
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                If String.Equals(Me.fllwStatus.Value, "4") Then
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                    Me.selectActRlst.Value = "1"
                ElseIf String.Equals(Me.fllwStatus.Value, "1") Then
                    Me.selectActRlst.Value = "3"
                ElseIf String.Equals(Me.fllwStatus.Value, "2") Then
                    Me.selectActRlst.Value = "2"
                End If

                '元々のフォローアップボックスの種別を取得する
                If String.Equals(followstatusrw.CRACTCATEGORY, "2") Then
                    'Repurchase
                    Me.FllwDvs.Value = "3"
                ElseIf String.Equals(followstatusrw.CRACTCATEGORY, "1") Or String.Equals(followstatusrw.CRACTCATEGORY, "4") Then
                    'Periodical
                    Me.FllwDvs.Value = "4"
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                ElseIf followstatusrw.PROMOTION_ID <> 0 Then
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                    'Promotion
                    Me.FllwDvs.Value = "5"
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                ElseIf String.Equals(followstatusrw.REQCATEGORY, "2") Or String.Equals(followstatusrw.REQCATEGORY, "3") Then
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                    'Request
                    Me.FllwDvs.Value = "5"
                ElseIf String.Equals(followstatusrw.REQCATEGORY, "1") Then
                    'Walk-in
                    Me.FllwDvs.Value = "6"
                ElseIf String.Equals(followstatusrw.CRACTRESULT, "1") Then
                    'Hot
                    Me.FllwDvs.Value = "1"
                ElseIf String.Equals(followstatusrw.CRACTRESULT, "2") Then
                    'Prospect
                    Me.FllwDvs.Value = "2"
                End If
            Else
                Me.fllwStatus.Value = ""
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                Me.selectActRlst.Value = ""
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            End If
        End Using
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowCractstatus End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' フォロー方法の取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>2012/04/26 TCS 河原 HTMLエンコード対応</history>
    Protected Function GetFollowContact() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowContact Start")
        '-----------------------------------------------------------------
        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Dim fllwcontdt As SC3080203DataSet.SC3080203FollowContactDataTable
        Dim fllwcontrw As SC3080203DataSet.SC3080203FollowContactRow
        fllwcontdt = SC3080203BusinessLogic.GetFollowContact()
        Dim title As String = ""

        title = HttpUtility.HtmlEncode(WebWordUtility.GetWord(30355))
        Dim html As String
        html = ""
        html = html & "<li title='" & title & "' id='FollowContactlist0' class='FollowContactlist ellipsis' value='0'>" & title & "<span value='0'></span></li>"
        For i = 0 To fllwcontdt.Count - 1
            fllwcontrw = fllwcontdt.Rows(i)
            html = html & "<li title='" & HttpUtility.HtmlEncode(fllwcontrw.CONTACT) & "' id='FollowContactlist" & fllwcontrw.CONTACTNO & "' class='FollowContactlist ellipsis' value='" & fllwcontrw.CONTACTNO & "'>" & HttpUtility.HtmlEncode(fllwcontrw.CONTACT) & "<span value='" & HttpUtility.HtmlEncode(fllwcontrw.FROMTO) & "'></span></li>"
        Next
        Me.followContactList.Text = html
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowContact End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' 次回活動日時・フォロー日時の設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetTime() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetTime Start")
        '-----------------------------------------------------------------
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dbNow As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        Dim dlrCd As String = staffInfo.DlrCD
        Dim strCd As String = Me.fllwstrcd.Value
        Dim fllwupboxSeqNo As Long
        Dim actDate As Date = dbNow
        Dim getTimeFlg As Boolean = False
        '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
        Dim actEndDate As Date = dbNow
        Dim getEndTimeFlg As Boolean = False
        '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
        Dim dtFllwupboxSales As SC3080203DataSet.SC3080203FllwupboxSalesDataTable = Nothing
        Dim drFllwupboxSales As SC3080203DataSet.SC3080203FllwupboxSalesRow = Nothing

        '商談(営業活動)開始時間取得
        Long.TryParse(Me.fllwSeq.Value, fllwupboxSeqNo)
        dtFllwupboxSales = SC3080203BusinessLogic.GetFllwupboxSales(dlrCd, strCd, fllwupboxSeqNo)

        If dtFllwupboxSales IsNot Nothing AndAlso dtFllwupboxSales.Rows.Count > 0 Then
            drFllwupboxSales = CType(dtFllwupboxSales(0), SC3080203DataSet.SC3080203FllwupboxSalesRow)
            If Not drFllwupboxSales.IsSTARTTIMENull Then
                '開始時間が設定されている
                actDate = drFllwupboxSales.STARTTIME
                getTimeFlg = True
            End If
            If Not drFllwupboxSales.IsWALKINNUMNull Then
                '来店人数が設定されている
                Me.walkinNum.Value = drFllwupboxSales.WALKINNUM
            End If
            '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
            If Not drFllwupboxSales.IsENDTIMENull Then
                '終了時間が設定されている
                actEndDate = drFllwupboxSales.ENDTIME
                getEndTimeFlg = True
            End If
            '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
        End If

        '今回活動時間
        Dim actFromHour As String = actDate.Hour.ToString("00")
        Dim actToHour As String = actFromHour

        If getTimeFlg Then
            '開始時間が取得できた
            actFromHour = actDate.ToString("HH:mm")

            '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 Start
            If getEndTimeFlg Then
                '終了時間が取得できた
                actToHour = actEndDate.ToString("HH:mm")
            Else
                '終了時間が取得できなかった
                If actDate.Hour = 23 Then
                    '開始時間が23時台の場合、日またぎしないように調整
                    actToHour = "23:59"
                Else
                    '開始時間＋1時間
                    actToHour = actDate.AddHours(1).ToString("HH:mm")
                End If
            End If
            '2012/03/02 Version1.01 Yasuda 【A.STEP2】代理商談入力機能開発 End
        Else
            '開始時間が取得できなかった
            If actFromHour.Equals("00") Then
                actFromHour = "23:00"
                actToHour = "23:59"
                actDate = actDate.AddDays(-1.0R)
            Else
                actFromHour = (CInt(actFromHour) - 1).ToString("00") & ":00"
                actToHour = actToHour & ":00"
            End If
        End If

        '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '活動時間Toが取得できた場合書き込み可否フラグを0に設定
        If getEndTimeFlg Then
            Me.SC3080203UpdateRWFlg.Value = "0"
        Else
            Me.SC3080203UpdateRWFlg.Value = "1"
        End If

        '2012/03/07 TCS 河原 【SALES_2】 START
        Sc3080218Page.ActTimeFrom = DateTime.ParseExact(actDate.ToString("yyyy/MM/dd") & " " & actFromHour, "yyyy/MM/dd HH:mm", Nothing)
        If Me.SC3080203UpdateRWFlg.Value = "0" Then
            Sc3080218Page.ActTimeTo = Date.ParseExact(actDate.ToString("yyyy/MM/dd") & " " & actToHour, "yyyy/MM/dd HH:mm", Nothing)
        End If
        '$2012/03/07 TCS 河原 【SALES_2】 END
        '2012/07/31 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '翌日の日付を取得
        Dim NowDate As Date
        NowDate = DateAdd(DateInterval.Day, 1, dbNow)

        '現在の時間を取得
        Dim NowHour As String = Hour(dbNow).ToString("00")
        '現在の分を取得
        Dim NowMinute As String = Minute(dbNow).ToString("00")

        Dim fromHour As String = NowHour
        Dim toHour As String = (CInt(NowHour) + 1).ToString("00")

        If String.Equals(NowHour, "22") Or String.Equals(NowHour, "23") Then
            '現在が23時台か22時台の場合
            fromHour = "23:00"
            toHour = "23:59"
        Else
            If CInt(NowMinute) <> 0 Then
                fromHour = (CInt(NowHour) + 1).ToString("00")
                toHour = (CInt(NowHour) + 2).ToString("00")
            End If
            fromHour = fromHour & ":00"
            toHour = toHour & ":00"
        End If

        '時間を切り捨て
        Dim truncNow As String = NowDate.ToString("yyyy/MM/dd")

        '--------------------
        '次回活動日設定
        '--------------------
        Dim initStartDate As Date = Date.ParseExact(truncNow & " " & fromHour, "yyyy/MM/dd HH:mm", Nothing)
        Dim initEndDate As Date = Date.ParseExact(truncNow & " " & toHour, "yyyy/MM/dd HH:mm", Nothing)

        '次回活動
        NextActStartDateTimeSelector.Value = initStartDate
        NextActEndDateTimeSelector.Value = initEndDate
        'TERA
        NextActStartDateSelector.Value = initStartDate
        NextActStartTimeSelector.Value = Nothing

        'フォロー
        FollowStartDateTimeSelector.Value = initStartDate
        FollowEndDateTimeSelector.Value = initEndDate
        'TERA
        FollowStartDateSelector.Value = initStartDate
        FollowStartTimeSelector.Value = Nothing
        '--------------------

        'ポップアップ読み込み前のWK
        NextActStartDateTimeSelectorWK.Value = initStartDate
        NextActEndDateTimeSelectorWK.Value = initEndDate
        NextActStartDateSelectorWK.Value = initStartDate
        NextActStartTimeSelectorWK.Value = Nothing
        FollowStartDateTimeSelectorWK.Value = initStartDate
        FollowEndDateTimeSelectorWK.Value = initEndDate
        FollowStartDateSelectorWK.Value = initStartDate
        FollowStartTimeSelectorWK.Value = Nothing

        'アラート初期化
        NextActivityAlertNoHidden.Value = "0"
        FollowAlertNoHidden.Value = "0"

        '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START
        ''2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） START
        'NextActivityFromToFlg.Value = "0"
        FollowFromToFlg.Value = "0"
        ''2013/10/04 TCS 安田 TCS 安田 次世代e-CRBタブレット iOS7.0 VersionUp対応（セールス） END
        '2013/11/15 TCS 三宅 2013/10対応版　既存流用 START

        '--デバッグログ---------------------------------------------------
        Logger.Info("GetTime End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ''' <summary>
    ''' 競合車種取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>2012/04/26 TCS 河原 HTMLエンコード対応</history>
    Protected Function GetCompetition() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetCompetition Start")
        '-----------------------------------------------------------------
        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Dim CompetitorMasterdt As SC3080203DataSet.SC3080203CompetitorMasterDataTable
        Dim CompetitorMasterrw As SC3080203DataSet.SC3080203CompetitorMasterRow
        CompetitorMasterdt = SC3080203BusinessLogic.GetCompetitorMaster()
        Dim html As New System.Text.StringBuilder
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim seqwk As String = " "
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        For i = 0 To CompetitorMasterdt.Count - 1
            CompetitorMasterrw = CompetitorMasterdt.Rows(i)
            '前回のメーカーと同じなら
            If seqwk = CompetitorMasterrw.COMPETITIONMAKERNO Then
                html.Append("<li class='GiveupCarList' id='GiveupCar")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORCD))
                html.Append("' title='")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITIONMAKER))
                html.Append(" / ")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORNM))
                html.Append("' value='")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORCD))
                html.Append("'>")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORNM))
                html.Append("</li>")
            Else
                seqwk = CompetitorMasterrw.COMPETITIONMAKERNO
                If i <> 0 Then
                    html.Append("</ul></div></div></div></div></div>")
                End If
                html.Append("<div id='GiveupCarlist")
                html.Append(CompetitorMasterrw.COMPETITIONMAKERNO)
                html.Append("' style='display:none'>")
                html.Append("<div class='scNscGiveupReasonListArea'>")
                html.Append("<div class='scNscGiveupReasonListBox'>")
                html.Append("<div class='scNscGiveupReasonListItemBox'>")
                html.Append("<div class='scNscGiveupReasonListItem'>")
                html.Append("<ul class='nscGiveupListBoxSetIn'>")
                html.Append("<li class='GiveupCarList' id='GiveupCar")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORCD))
                html.Append("' title='")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITIONMAKER))
                html.Append(" / ")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORNM))
                html.Append("' value='")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORCD))
                html.Append("'>")
                html.Append(HttpUtility.HtmlEncode(CompetitorMasterrw.COMPETITORNM))
                html.Append("</li>")
            End If
        Next
        html.Append("</ul></div></div></div></div></div>")

        Dim compdt As SC3080203DataSet.SC3080203CompetitionMakermasterDataTable
        Dim comprw As SC3080203DataSet.SC3080203CompetitionMakermasterRow
        compdt = SC3080203BusinessLogic.GetNoCompetitionMakermaster()

        If compdt.Count() > 0 Then
            For i = 0 To compdt.Count - 1
                comprw = compdt.Rows(i)
                html.Append("<div id='GiveupCarlist")
                html.Append(comprw.COMPETITIONMAKERNO)
                html.Append("' style='display:none'>")
                html.Append("<div class='scNscGiveupReasonListArea'>")
                html.Append("<div class='scNscGiveupReasonListBox'>")
                html.Append("<div class='scNscGiveupReasonListItemBox'>")
                html.Append("<div class='scNscGiveupReasonListItem'>")
                html.Append("<ul class='nscGiveupListBoxSetIn'>")
                html.Append("</ul></div></div></div></div></div>")
            Next
        End If

        Me.GiveupCarLabel.Text = html.ToString
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetCompetition End")
        '-----------------------------------------------------------------
        Return True
    End Function

    ' ''' <summary>
    ' ''' プロセスの文言設定
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Protected Function GetContentWord() As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("GetContentWord Start")
    '    '-----------------------------------------------------------------
    '    Using Serchdt As New SC3080203DataSet.SC3080203SeqDataTable
    '        Dim Serchrw As SC3080203DataSet.SC3080203SeqRow
    '        Serchrw = Serchdt.NewSC3080203SeqRow
    '        Serchdt.Rows.Clear()
    '        Serchrw.SEQNO = CONTENT_SEQ_CATALOG
    '        Serchdt.Rows.Add(Serchrw)
    '        Dim contworddt As SC3080203DataSet.SC3080203ContentWordDataTable
    '        contworddt = SC3080203BusinessLogic.GetContentWord(Serchdt)
    '        Dim contwordrw As SC3080203DataSet.SC3080203ContentWordRow
    '        contwordrw = contworddt.Rows(0)
    '        Me.CatalogWord.Text = contwordrw.ACTION
    '        Me.CatalogTitle.Text = contwordrw.ACTION
    '        Serchdt.Rows.Clear()
    '        Serchrw.SEQNO = CONTENT_SEQ_TESTDRIVE
    '        Serchdt.Rows.Add(Serchrw)
    '        contworddt = SC3080203BusinessLogic.GetContentWord(Serchdt)
    '        contwordrw = contworddt.Rows(0)
    '        Me.TestDriveWord.Text = contwordrw.ACTION
    '        Me.TestDriveTitle.Text = contwordrw.ACTION
    '        Serchdt.Rows.Clear()
    '        Serchrw.SEQNO = CONTENT_SEQ_ASSESSMENT
    '        Serchdt.Rows.Add(Serchrw)
    '        contworddt = SC3080203BusinessLogic.GetContentWord(Serchdt)
    '        contwordrw = contworddt.Rows(0)
    '        Me.AssesmentWord.Text = contwordrw.ACTION
    '        Serchdt.Rows.Clear()
    '        Serchrw.SEQNO = CONTENT_SEQ_VALUATION
    '        Serchdt.Rows.Add(Serchrw)
    '        contworddt = SC3080203BusinessLogic.GetContentWord(Serchdt)
    '        contwordrw = contworddt.Rows(0)
    '        Me.ValuationWord.Text = contwordrw.ACTION
    '        Me.ValuationTitle.Text = contwordrw.ACTION
    '    End Using
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("GetContentWord End")
    '    '-----------------------------------------------------------------
    '    Return True
    'End Function

    ''' <summary>
    ''' 日付フォーマットのセット
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetDateFormat() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetDateFormat Start")
        '-----------------------------------------------------------------
        Dim formatdt As SC3080203DataSet.SC3080203DateFormatDataTable
        Dim formatrw As SC3080203DataSet.SC3080203DateFormatRow
        formatdt = SC3080203BusinessLogic.GetDateFormat()
        formatrw = formatdt.Rows(0)
        Me.dateFormt.Value = formatrw.FORMAT
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetDateFormat End")
        '-----------------------------------------------------------------
        Return True
    End Function

    '2012/03/07 TCS 河原 【SALES_2】START
    ' ''' <summary>
    ' ''' アイコンパス取得
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Protected Function GetContentIconPath() As Boolean
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("GetContentIconPath Start")
    '    '-----------------------------------------------------------------
    '    Dim ipathdt As SC3080203DataSet.SC3080203ContentIconPathDataTable
    '    Dim ipathtw As SC3080203DataSet.SC3080203ContentIconPathRow
    '    'カタログ
    '    ipathdt = SC3080203BusinessLogic.GetContentIconPath(CONTENT_SEQ_CATALOG)
    '    ipathtw = ipathdt.Rows(0)
    '    Me.CatalogSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
    '    Me.CatalogNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED
    '    '試乗
    '    ipathdt = SC3080203BusinessLogic.GetContentIconPath(CONTENT_SEQ_TESTDRIVE)
    '    ipathtw = ipathdt.Rows(0)
    '    Me.TestDriveSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
    '    Me.TestDriveNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED
    '    '査定
    '    ipathdt = SC3080203BusinessLogic.GetContentIconPath(CONTENT_SEQ_ASSESSMENT)
    '    ipathtw = ipathdt.Rows(0)
    '    Me.AssesmentSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
    '    Me.AssesmentNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED
    '    '見積り
    '    ipathdt = SC3080203BusinessLogic.GetContentIconPath(CONTENT_SEQ_VALUATION)
    '    ipathtw = ipathdt.Rows(0)
    '    Me.ValuationSelPath.Value = ipathtw.ICONPATH_RESULT_SELECTED
    '    Me.ValuationNonSelPath.Value = ipathtw.ICONPATH_RESULT_NOTSELECTED
    '    '--デバッグログ---------------------------------------------------
    '    Logger.Info("GetContentIconPath End")
    '    '-----------------------------------------------------------------
    '    Return True
    'End Function
    '2012/03/07 TCS 河原 【SALES_2】END

    ''' <summary>
    ''' エラー文言設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SetErrWord() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetErrWord Start")
        '-----------------------------------------------------------------
        Me.ErrWord1.Value = WebWordUtility.GetWord(30909)
        Me.ErrWord2.Value = WebWordUtility.GetWord(30927)
        Me.ErrWord3.Value = WebWordUtility.GetWord(30929)
        Me.PopWord1.Value = WebWordUtility.GetWord(30331)
        Me.PopWord2.Value = WebWordUtility.GetWord(30388)
        '--デバッグログ---------------------------------------------------
        Logger.Info("SetErrWord End")
        '-----------------------------------------------------------------
        Return True
    End Function

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    '2015/12/21 TCS 鈴木 受注後工程蓋閉め対応 START
    ''' <summary>
    ''' 成約ボタン状態制御
    ''' </summary>
    ''' <remarks>成約ボタンの制御（有効・無効）を販売店設定に合わせる。</remarks>
    Private Sub SetSuccessButton()

        Dim contractflg As String = String.Empty

        '契約状況フラグ（0:未契約 1:契約済み 2:キャンセル）
        contractflg = Me.ContractStatusFlg

        If String.Equals(contractflg, "1") Then
            '契約済みの場合
            pnlSuccessButton.CssClass = "nscListIcnB4"
        Else
            '契約済み以外の場合
            If SC3080203BusinessLogic.GetSetting_AllowUISuccess() Then
                pnlSuccessButton.CssClass = "nscListIcnB4"
            Else
                'スタイルのみのクラス名にし、JavaScriptイベントが動作しないようにする。
                pnlSuccessButton.CssClass = "nscListIcnB4_Dummy"
            End If
        End If
    End Sub
    '2015/12/21 TCS 鈴木 受注後工程蓋閉め対応 END

    ''' <summary>
    ''' 断念理由選択ポップアップ初期設定
    ''' </summary>
    ''' <remarks>断念理由選択ポップアップのリストを作成する。</remarks>
    Private Sub InitGiveUpReasonPopup()
        Me.GiveUpReasonListRepeater.DataSource = SC3080203BusinessLogic.GetGiveUpReasonMaster()
        Me.GiveUpReasonListRepeater.DataBind()
    End Sub
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END


    ''' <summary>
    ''' 商談画面で希望車種が変更された場合に呼び出されるメソッド<br/>
    ''' 活動登録画面の希望車種情報を更新します。(商談画面で選択された希望車種にあわせる)
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateActivityResult() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.UpdateActivityResult

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityResult Start")
        '-----------------------------------------------------------------

        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) Then
            Me.fllwSeq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
        Else
            Me.fllwSeq.Value = ""
        End If

        If ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_STRCD) Then
            Me.fllwstrcd.Value = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_STRCD, False), String)
        Else
            Me.fllwstrcd.Value = BLANK
        End If

        If ContainsKey(ScreenPos.Current, CONST_VCLINFO) Then
            Me.Vclseq.Value = DirectCast(GetValue(ScreenPos.Current, CONST_VCLINFO, False), String)
        Else
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.Vclseq.Value = "0"
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        End If

        '来店人数を取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrCd As String = staffInfo.DlrCD
        Dim strCd As String = Me.fllwstrcd.Value
        Dim fllwupboxSeqNo As Long
        Dim dtFllwupboxSales As SC3080203DataSet.SC3080203FllwupboxSalesDataTable = Nothing
        Dim drFllwupboxSales As SC3080203DataSet.SC3080203FllwupboxSalesRow = Nothing
        Long.TryParse(Me.fllwSeq.Value, fllwupboxSeqNo)
        dtFllwupboxSales = SC3080203BusinessLogic.GetFllwupboxSales(dlrCd, strCd, fllwupboxSeqNo)
        If dtFllwupboxSales IsNot Nothing AndAlso dtFllwupboxSales.Rows.Count > 0 Then
            drFllwupboxSales = dtFllwupboxSales(0)
            If Not drFllwupboxSales.IsWALKINNUMNull Then
                '来店人数が設定されている
                Me.walkinNum.Value = drFllwupboxSales.WALKINNUM
            End If
        End If

        'プロセス欄の車両情報を取得
        '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) START 
        'コメント解除 成約車種のリスト(jsで使用)が更新されないため
        setSelectedCar()
        '2012/03/15 TCS 高橋 【SALES_2】(Sales1A ユーザテスト No.222) END

        ''カタログ欄更新
        'CatalogListRepeater.DataBind()
        'CatalogListUpdatePanel.Update()

        ''試乗欄更新
        'TestDriveListRepeater.DataBind()
        'TestDriveListUpdatePanel.Update()

        ''見積り欄更新
        'ValuationListRepeater.DataBind()
        'ValuationListUpdatePanel.Update()

        '成約車種欄更新
        SuccessSeriesRepeater.DataBind()
        SuccessSeriesUpdatePanel.Update()

        '更新情報のためのHidden項目の更新
        HiddenFieldUpdatePanelPage3.Update()


        Sc3080218Page.UpdateProcessList()


        'CType(Sc3080218Page, ISC3080218Control).UpdateActivityResult()

        '--デバッグログ---------------------------------------------------
        Logger.Info("UpdateActivityResult End")
        '-----------------------------------------------------------------

    End Sub

    '2012/03/07 TCS 河原 【SALES_2】START
    ''' <summary>
    ''' 共通部分の操作
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitSubDisplay()
        '受注後フラグ
        Sc3080218Page.BookedFlg = "0"
    End Sub

    ''' <summary>
    ''' 次回活動分類タイトル取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetNextActContactTitle()
        Dim contactNo As String
        contactNo = Me.selectNextActContact.Value
        Dim dt As SC3080203DataSet.SC3080203NextActContactDataTable
        dt = SC3080203BusinessLogic.GetNextActContactTitle(CInt(contactNo))
        Dim rw As SC3080203DataSet.SC3080203NextActContactRow
        rw = dt.Rows(0)
        Me.NextActContactTitle.Value = rw.CONTACT
        Me.NextActContactNextactivity.Value = rw.NEXTACTIVITY & "_" & rw.FROMTO
    End Sub

    ''' <summary>
    ''' 予約フォロー分類タイトル取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetFollowContactTitle() As Boolean
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowContactTitle Start")
        '-----------------------------------------------------------------
        Dim dt As SC3080203DataSet.SC3080203FollowContactDataTable = SC3080203BusinessLogic.GetFollowContact()
        Dim rw As SC3080203DataSet.SC3080203FollowContactRow
        Dim title As String = ""
        title = WebWordUtility.GetWord(30355)
        Dim contact As String = Me.selectFollowContact.Value
        For i = 0 To dt.Count - 1
            rw = dt.Rows(i)
            If contact = rw.CONTACTNO Then
                title = rw.CONTACT
            End If
        Next
        Me.FollowContactTitle.Value = title
        '--デバッグログ---------------------------------------------------
        Logger.Info("GetFollowContactTitle End")
        '-----------------------------------------------------------------
        Return True
    End Function
    '2012/03/07 TCS 河原 【SALES_2】END

    Public Event ContinueActivity(ByVal sender As Object, ByVal e As System.EventArgs) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.ContinueActivity
    Public Event SuccessActivity(ByVal sender As Object, ByVal e As System.EventArgs) Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.SuccessActivity

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function
#End Region

    ''' <summary>
    ''' 活動登録処理用のDataSetを作成する
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetRegistData(ByRef RegistDt As SC3080203DataSet.SC3080203RegistDataDataTable)
        Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow

        '行を作成
        RegistRw = RegistDt.NewSC3080203RegistDataRow

        '活動日
        RegistRw.ACTDAYFROM = String.Empty

        '2012/03/07 TCS 河原 【SALES_2】START
        'FROM
        If Not String.IsNullOrEmpty(Sc3080218Page.ActTimeFrom) Then
            '開始日時
            RegistRw.ACTDAYFROM = Sc3080218Page.ActTimeFrom.ToString("yyyy/MM/dd HH:mm")
        End If

        'TO
        If Not String.IsNullOrEmpty(Sc3080218Page.ActTimeTo) Then
            '終了日時
            RegistRw.ACTDAYTO = Sc3080218Page.ActTimeTo.ToString("HH:mm")
        End If
        '2012/03/07 TCS 河原 【SALES_2】END

        '前回までの活動終了時間で最新の時間
        Dim actTimeEnd As Date
        If GetLatestActTimeEnd(actTimeEnd) Then
            RegistRw.LATEST_TIME_END = actTimeEnd
        End If

        '2012/03/07 TCS 河原 【SALES_2】START
        'アカウント
        RegistRw.ACTACCOUNT = Sc3080218Page.SelectStaff

        'コンタクト方法
        RegistRw.ACTCONTACT = Sc3080218Page.SelectActContact

        'プロセス有無フラグ
        RegistRw.PROCESSFLG = Sc3080218Page.ProcessFlg

        '各プロセスの情報
        RegistRw.SELECTACTCATALOG = Sc3080218Page.selectActCatalog
        RegistRw.SELECTACTTESTDRIVE = Sc3080218Page.selectActTestDrive

        '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
        ''2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        ''RegistRw.SELECTACTASSESMENT = Sc3080218Page.selectActAssesment
        'Dim staffInfo As StaffContext = StaffContext.Current
        'Dim strcd As String = Me.fllwstrcd.Value
        'Dim fllwupboxSeqNo As String = 0
        'Long.TryParse(Me.fllwSeq.Value, fllwupboxSeqNo)

        'Dim checkResult As Boolean
        ''査定依頼機能使用可否判定
        'checkResult = SC3080203BusinessLogic.IsRegAsm()
        'If checkResult Then
        '    '使用中
        '    '査定実績判定
        '    checkResult = SC3080203BusinessLogic.IsActAsm(strcd, fllwupboxSeqNo)
        '    If checkResult Then
        '        '査定実績あり(実績登録する)
        '        RegistRw.SELECTACTASSESMENT = "1"
        '    Else
        '        '査定実績なし(実績登録しない)
        '        RegistRw.SELECTACTASSESMENT = ""
        '    End If
        'Else
        '    '未使用
        '    '画面で入力された結果を格納
        '    RegistRw.SELECTACTASSESMENT = Sc3080218Page.selectActAssesment
        'End If
        ''2013/01/10 TCS 上田 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

        '画面で入力された結果を格納
        RegistRw.SELECTACTASSESMENT = Sc3080218Page.selectActAssesment
        '2013/03/07 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END

        RegistRw.SELECTACTVALUATION = Sc3080218Page.selectActValuation
        '2012/03/07 TCS 河原 【SALES_2】END

        '活動結果
        RegistRw.ACTRESULT = Me.selectActRlst.Value

        '------------------------
        ' 次回活動
        '------------------------
        RegistRw.NEXTACTCONTACT = Me.selectNextActContact.Value

        'From-Toフラグ
        RegistRw.NEXTACTDAYTOFLG = NextActivityFromToFlg.Value

        '時間指定フラグ(デフォルトをFalseにする)
        RegistRw.NEXTACTTIMEFLG = False

        'FROM
        RegistRw.NEXTACTDAYFROM = String.Empty

        If RegistRw.NEXTACTDAYTOFLG.Equals("1") And NextActStartDateTimeSelector.Value.HasValue Then

            'From-To指定でFromに日付を入力している場合
            RegistRw.NEXTACTDAYFROM = NextActStartDateTimeSelector.Value.Value.ToString("yyyy/MM/dd HH:mm")

            '時間指定あり
            RegistRw.NEXTACTTIMEFLG = True

        ElseIf RegistRw.NEXTACTDAYTOFLG.Equals("0") And NextActStartDateSelector.Value.HasValue Then

            '納期のみで、日付を入力している場合
            RegistRw.NEXTACTDAYFROM = NextActStartDateSelector.Value.Value.ToString("yyyy/MM/dd")

            If NextActStartTimeSelector.Value.HasValue Then
                '時間指定あり
                RegistRw.NEXTACTDAYFROM &= " " & NextActStartTimeSelector.Value.Value.ToString("HH:mm")
                RegistRw.NEXTACTTIMEFLG = True
            Else
                '時間指定なし
                RegistRw.NEXTACTDAYFROM &= " 00:00"
            End If

        End If

        'TO
        RegistRw.NEXTACTDAYTO = String.Empty
        If NextActEndDateTimeSelector.Value.HasValue Then
            '開始日時又は納期
            RegistRw.NEXTACTDAYTO = NextActEndDateTimeSelector.Value.Value.ToString("HH:mm")
        End If

        'アラート
        If RegistRw.NEXTACTDAYTOFLG.Equals("1") Then
            RegistRw.NEXTACTALERT = NextActivityAlertNoHidden.Value
        Else
            RegistRw.NEXTACTALERT = "0"
        End If

        '------------------------
        ' フォロー
        '------------------------

        'フォロー有無しフラグ
        RegistRw.FOLLOWFLG = Me.FollowFlg.Value

        'コンタクト方法
        RegistRw.FOLLOWCONTACT = Me.selectFollowContact.Value

        'From-Toフラグ
        RegistRw.FOLLOWDAYTOFLG = Me.FollowFromToFlg.Value

        '時間指定フラグ(デフォルトをFalseにする)
        RegistRw.FOLLOWTIMEFLG = False

        'None選択の場合は、フォロー無しを設定
        If String.Equals(Me.selectFollowContact.Value, "0") Then
            RegistRw.FOLLOWFLG = "0"
        End If

        'FROM
        RegistRw.FOLLOWDAYFROM = String.Empty

        If RegistRw.FOLLOWDAYTOFLG.Equals("1") And FollowStartDateTimeSelector.Value.HasValue Then

            'From-To指定でFromに日付を入力している場合
            RegistRw.FOLLOWDAYFROM = FollowStartDateTimeSelector.Value.Value.ToString("yyyy/MM/dd HH:mm")

            '時間指定あり
            RegistRw.FOLLOWTIMEFLG = True

        ElseIf RegistRw.FOLLOWDAYTOFLG.Equals("0") And FollowStartDateSelector.Value.HasValue Then

            '納期のみで、日付を入力している場合
            RegistRw.FOLLOWDAYFROM = FollowStartDateSelector.Value.Value.ToString("yyyy/MM/dd")

            If FollowStartTimeSelector.Value.HasValue Then
                '時間指定あり
                RegistRw.FOLLOWDAYFROM &= " " & FollowStartTimeSelector.Value.Value.ToString("HH:mm")
                RegistRw.FOLLOWTIMEFLG = True
            Else
                '時間指定なし
                RegistRw.FOLLOWDAYFROM &= " 00:00"
            End If

        End If

        'TO
        RegistRw.FOLLOWDAYTO = String.Empty
        If FollowEndDateTimeSelector.Value.HasValue Then
            '開始日時又は納期
            RegistRw.FOLLOWDAYTO = FollowEndDateTimeSelector.Value.Value.ToString("HH:mm")
        End If

        'アラート
        If RegistRw.FOLLOWDAYTOFLG.Equals("1") Then
            RegistRw.FOLLOWALERT = Me.FollowAlertNoHidden.Value
        Else
            RegistRw.FOLLOWALERT = "0"
        End If

        '------------------------
        ' 他
        '------------------------
        RegistRw.SUCCESSSERIES = Me.selectSelSeries.Value
        RegistRw.GIVEUPMAKER = Me.selectGiveupMaker.Value

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim giveupmodel As New StringBuilder
        giveupmodel.Append(Me.selectGiveupCar.Value)
        giveupmodel.Replace("GiveupCar", "")
        RegistRw.GIVEUPMODEL = giveupmodel.ToString()
        ' 2013/06/30 TCS TCS 小幡 2013/10対応版　既存流用 END
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
        If Me.selectGiveupReasonID.Value.Length > 0 Then
            RegistRw.GIVEUP_REASON_ID = Decimal.Parse(Me.selectGiveupReasonID.Value)
        Else
            RegistRw.GIVEUP_REASON_ID = 0
        End If
        '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
        RegistRw.GIVEUPREASON = Me.selectGiveupReason.Value
        RegistRw.cstkind = Me.Cstkind.Value
        RegistRw.INSDID = Me.Insdid.Value

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        If String.Equals(Me.Vclseq.Value, "0") Or String.IsNullOrEmpty(Me.Vclseq.Value) Then
            RegistRw.VCLSEQ = 0
        Else
            RegistRw.VCLSEQ = Me.Vclseq.Value
        End If
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        RegistRw.FLLWSEQ = Me.fllwSeq.Value
        RegistRw.FLLWSTRCD = Me.fllwstrcd.Value
        If Not String.IsNullOrEmpty(Me.walkinNum.Value) Then    '来店人数
            RegistRw.WALKINNUM = Me.walkinNum.Value
        End If

        RegistRw.CUSTSEGMENT = Me.GetValue(ScreenPos.Current, CONST_CSTKIND, False).ToString()
        RegistRw.CRCUSTID = Me.GetValue(ScreenPos.Current, CONST_INSDID, False).ToString()

        '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
        RegistRw.CUSTNAME = Me.GetValue(ScreenPos.Current, SESSION_KEY_NAME, False).ToString()
        '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        ' 活動ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ACT_ID) Then
            RegistRw.ACTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ACT_ID, False), Decimal)
        Else
            RegistRw.ACTID = 0
        End If

        ' 用件ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_REQ_ID) Then
            RegistRw.REQID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_REQ_ID, False), Decimal)
        Else
            RegistRw.REQID = 0
        End If

        ' 誘致ID
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ATT_ID) Then
            RegistRw.ATTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ATT_ID, False), Decimal)
        Else
            RegistRw.ATTID = 0
        End If

        ' 活動回数
        If ContainsKey(ScreenPos.Current, SESSION_KEY_COUNT) Then
            RegistRw.COUNT = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_COUNT, False), Long)
        Else
            RegistRw.COUNT = 0
        End If

        ' 用件ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION) Then
            RegistRw.REQUESTLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_REQUEST_LOCK_VERSION, False), Long)
        Else
            RegistRw.REQUESTLOCKVERSION = 0
        End If

        ' 誘致ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION) Then
            RegistRw.ATTRACTLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ATTRACT_LOCK_VERSION, False), Long)
        Else
            RegistRw.ATTRACTLOCKVERSION = 0
        End If

        ' 商談ロックバージョン
        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION) Then
            RegistRw.SALESLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALES_LOCK_VERSION, False), Long)
        Else
            RegistRw.SALESLOCKVERSION = 0
        End If

        ' 断念競合車種連番
        RegistRw.GIVEUPVCLSEQ = 0
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        '登録対象のレコードを追加
        RegistDt.Rows.Add(RegistRw)
    End Sub

    ''' <summary>
    ''' 前回の活動時間取得
    ''' </summary>
    ''' <param name="actTimeEnd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLatestActTimeEnd(ByRef actTimeEnd As Date) As Boolean
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrCd As String = staffInfo.DlrCD
        Dim strCD As String = Me.fllwstrcd.Value
        Dim fllwupboxSeqNo As String = 0
        Dim dtLatestActTime As SC3080203DataSet.SC3080203LatestActTimeDataTable = Nothing
        Long.TryParse(Me.fllwSeq.Value, fllwupboxSeqNo)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        dtLatestActTime = SC3080203BusinessLogic.GetLatestActTimeEnd(fllwupboxSeqNo)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        If dtLatestActTime IsNot Nothing AndAlso dtLatestActTime.Rows.Count > 0 Then
            Dim drLatestActTime As SC3080203DataSet.SC3080203LatestActTimeRow = dtLatestActTime(0)
            If Not drLatestActTime.IsLATEST_TIME_ENDNull Then
                '前回までの活動終了時間で最新の時間が取得できた場合
                actTimeEnd = drLatestActTime.LATEST_TIME_END
                Return True
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' 活動登録処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RegistActivity() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.RegistActivity
        '--デバッグログ---------------------------------------------------
        Logger.Info("RegistActivity Start")
        '-----------------------------------------------------------------

        Using RegistDt As New SC3080203DataSet.SC3080203RegistDataDataTable
            '登録データ設定
            Me.SetRegistData(RegistDt)

            Dim RegistRw As SC3080203DataSet.SC3080203RegistDataRow = RegistDt(0)
            Dim checkRslt As Boolean
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
            Dim msgItem0 As String = String.Empty
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END

            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '未取引客情報取得
            Dim newCustDt As ActivityInfoDataSet.GetNewCustomerDataTable = ActivityInfoBusinessLogic.GetNewcustomer(Me.Insdid.Value)

            '入力チェック
            '2013/12/14 TCS 鈴木 受注後工程蓋閉め対応 MOD START
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
            checkRslt = SC3080203BusinessLogic.IsInputeCheck(RegistDt, newCustDt, Me.Cstkind.Value, msgid, msgItem0, Me.ContractStatusFlg)
            '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
            '2013/12/14 TCS 鈴木 受注後工程蓋閉め対応 MOD END

            If checkRslt = False Then

                SetValue(ScreenPos.Current, SESSION_KEY_DISPPAGE, "3")

                '2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3 START
                '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
                If MSG_30933 = msgid Or MSG_30934 = msgid _
                    OrElse msgid = MSG_40902 OrElse msgid = MSG_40907 _
                    OrElse (msgid >= MSG_CSTINFO_INPUTERR_START And msgid <= MSG_CSTINFO_INPUTERR_END) _
                    OrElse (msgid >= MSG_4000901 And msgid <= MSG_4000999) Then
                    '2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3 END
                    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END
                    ShowMessageBox(msgid)
                    'スクリプトの登録
                    JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "newCustomerDummyErrorActivity", "after")
                ElseIf MSG_30932 = msgid AndAlso RegistRw.IsLATEST_TIME_ENDNull = False Then
                    Dim param As String = DateTimeFunc.FormatDate(DATE_FORMAT_YMDHS_CONVID, RegistRw.LATEST_TIME_END)
                    ShowMessageBox(msgid, New String() {param})
                    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 START
                ElseIf MSG_20916 = msgid Then
                    ShowMessageBox(msgid, New String() {msgItem0})
                    '2013/12/24 TCS 市川 Aカード情報相互連携開発 追加要望 END
                Else
                    ShowMessageBox(msgid)
                End If
                '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
            Else
                '登録処理を実施
                Dim resistFlg As String

                If String.IsNullOrEmpty(Me.fllwStatus.Value) Then
                    If RegistRw.ACTRESULT = C_RSLT_WALKIN Or RegistRw.ACTRESULT = C_RSLT_PROSPECT Or RegistRw.ACTRESULT = C_RSLT_HOT Then
                        '新規活動登録
                        resistFlg = "1"
                    Else
                        '新規活動登録からの即Success、Give-up
                        resistFlg = "2"
                    End If
                Else
                    '既存の活動に対する活動結果
                    resistFlg = "3"
                End If


                Dim retValue As Boolean = False
                Dim bizLogic As New SC3080203BusinessLogic
                ' 2012/03/13 TCS 安田 【SALES_2】 START
                Dim staffStatus As String = bizLogic.GetStaffStatus
                ' 2012/03/13 TCS 安田 【SALES_2】 END

                ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD START
                retValue = bizLogic.InsertActivityResult(RegistDt, resistFlg, Me.fllwStatus.Value, Me.ContractStatusFlg)
                ' 2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 MOD END

                If retValue = False Then
                    '登録処理エラー
                    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
                    ShowMessageBox(901)
                    '2013/06/30 TCS 趙 2013/10対応版 既存流用 END
                    Return
                End If

                '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 START
                '活動結果が成約時の場合のみ、キャンセル通知を行う
                If RegistRw.ACTRESULT = C_RSLT_SUCCESS Then
                    Dim bizLogic3080201 As New SC3080201BusinessLogic
                    bizLogic3080201.UpdateNoticeRequest(RegistRw.FLLWSTRCD, _
                                                        RegistRw.FLLWSEQ, _
                                                        RegistRw.CUSTNAME)
                    bizLogic3080201 = Nothing
                End If
                '2012/12/10 TCS 坪根 【A.STEP2】ADD 次世代e-CRB  新車タブレット横展開に向けた機能開発 END

                ' 2012/03/13 TCS 安田 【SALES_2】 START
                '来店実績更新_商談終了時のPush送信
                bizLogic.PushUpdateVisitSalesEnd(staffStatus)
                ' 2012/03/13 TCS 安田 【SALES_2】 END

                If RegistRw.ACTRESULT = C_RSLT_WALKIN Or RegistRw.ACTRESULT = C_RSLT_PROSPECT Or RegistRw.ACTRESULT = C_RSLT_HOT Or retValue = False Then
                    RaiseEvent ContinueActivity(Me, EventArgs.Empty)
                    resistFlg = "1"
                Else
                    RaiseEvent SuccessActivity(Me, EventArgs.Empty)
                    resistFlg = "2"
                End If

                '画面表示を初期化する
                InitDisplaySetting()

            End If
        End Using

        '--デバッグログ---------------------------------------------------
        Logger.Info("RegistActivity End")
        '-----------------------------------------------------------------

    End Sub

    ''' <summary>
    ''' 商談画面で活動が変更された際に呼び出されるメソッド
    ''' </summary>
    ''' <remarks>選択された活動で、活動登録画面を初期化します。</remarks>
    Public Sub ChangeFollow() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080203Control.ChangeFollow
        '--デバッグログ---------------------------------------------------
        Logger.Info("ChangeFollow Start")
        '-----------------------------------------------------------------

        '2013/02/07 TCS 河原 GL0873 START
        If Me.ContainsKey(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO) AndAlso Me.ContainsKey(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES) Then
            Dim fllwSeq As String = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_SEQNO, False), String)
            Dim fllwStrcd As String = DirectCast(GetValue(ScreenPos.Current, CONST_FLLWUPBOX_STRCD, False), String)
            Dim salesfllwSeq As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX_SALES, False), String)
            Dim salesfllwStrcd As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD_SALES, False), String)
            '現在表示中の活動と、商談中の活動が一致する場合のみ、初期設定を行う
            'If (fllwSeq = salesfllwSeq) And (fllwStrcd = salesfllwStrcd) Then
            'セッション情報取得
            GetSessionValues()

            '初期設定
            InitDisplaySetting()
            'End If
        End If
        '2013/02/07 TCS 河原 GL0873 END

        '--デバッグログ---------------------------------------------------
        Logger.Info("ChangeFollow End")
        '-----------------------------------------------------------------
    End Sub

    '2015/12/11 TCS 鈴木 受注後工程蓋閉め対応 START
    ''' <summary>
    ''' 活動結果のボタン状態制御
    ''' </summary>
    ''' <remarks>受注後工程を利用しない場合、「Cold」「Warm」「Hot」「Give up」ボタンを非表示</remarks>
    Private Sub SetResultButton()

        Dim contractflg As String = String.Empty

        '契約状況フラグ（0:未契約 1:契約済み 2:キャンセル）
        contractflg = Me.ContractStatusFlg

        If String.Equals(contractflg, "1") Then
            '契約済みの場合

            '「Cold」、「Warm」、「Hot」、「Give up」ボタン非表示
            ButtonColdPanel.Visible = False
            ButtonWarmPanel.Visible = False
            ButtonHotPanel.Visible = False
            ButtonGiveUpPanel.Visible = False

            '希望車非表示
            Me.nscListBoxSet_HeightC_Panel.Visible = False

        End If
        ' 2015/12/09 TCS 鈴木 受注後工程蓋閉め対応 END 

    End Sub

#Region " ポップアップ後読み処理 "

    Protected Sub NextActContactButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextActContactButton.Click
        'パネルを表示
        Me.NextActContactPanel.Visible = True

        'データ取得
        NextActContactRepeater.DataBind()

        '取得完了フラグ
        Me.NextActPopupFlg.Value = "1"

        '反映させる為に更新
        Me.NextActContactUpdatePanel.Update()
        Me.PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setNextActContactPageOpenEnd", "startup")
    End Sub

    Protected Sub GiveupReasonButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GiveupReasonButton.Click
        'パネルを表示
        Me.GiveupReasonPanel.Visible = True

        'データ取得
        GiveupReasonRepeater.DataBind()

        '反映させる為に更新
        Me.GiveupReasonUpdatePanel.Update()
        Me.PopupFlgUpdatePanel.Update()

        '取得完了フラグ
        Me.GiveupReasonPopupFlg.Value = "1"

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setGiveupReasonPageOpenEnd", "startup")
    End Sub

    Protected Sub NextActTimeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextActTimeButton.Click
        'パネルを表示
        Me.NextActTimePanel.Visible = True

        'データ取得
        NextActivityAlertRepeater.DataBind()

        '取得完了フラグ
        Me.NextActTimePopupFlg.Value = "1"

        '反映させる為に更新
        Me.NextActTimeUpdatePanel.Update()
        Me.PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setNextActTimePageOpenEnd", "startup")
    End Sub

    Protected Sub FollowTimeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FollowTimeButton.Click
        'パネルを表示
        Me.NextActTimePanel.Visible = True

        'データ取得
        NextActivityAlertRepeater.DataBind()

        '取得完了フラグ
        Me.NextActTimePopupFlg.Value = "1"

        '反映させる為に更新
        Me.NextActTimeUpdatePanel.Update()
        Me.PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setFollowTimePageOpenEnd", "startup")
    End Sub

    Protected Sub FollowContactButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FollowContactButton.Click
        'パネルを表示
        Me.FollowContactPanel.Visible = True

        'データ取得
        GetFollowContact()

        '取得完了フラグ
        Me.FollowContactPopupFlg.Value = "1"

        '反映させる為に更新
        Me.FollowContactUpdatePanel.Update()
        Me.PopupFlgUpdatePanel.Update()

        '取得後JavaScript関数
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "setFollowContactPageOpenEnd", "startup")
    End Sub

#End Region

End Class


