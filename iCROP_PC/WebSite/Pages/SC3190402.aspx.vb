'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190402.aspx.vb
'─────────────────────────────────────
'機能： 部品庫モニター画面(PC端末アプリ)
'補足： 
'作成：            NEC 村瀬
'更新： 2014/xx/xx NEC 村瀬 改ページ機能追加
'更新： 2014/09/09 TMEJ Y.Gotoh 部品庫B／O管理に向けた評価用アプリ作成 $01
'更新： 2014/09/14 TMEJ M.Asano サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示) $02
'       2015/03/16 TMEJ M.Asano DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 $03
'更新： 2017/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $04
'更新： 2019/11/05 NSK M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 $05
'─────────────────────────────────────
Imports System.Globalization
Imports System.IO
Imports System.Xml

Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.Common.Login.BizLogic
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.PartsManagement.PSMonitor.BizLogic
Imports Toyota.eCRB.PartsManagement.PSMonitor.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports System.Web.Services
Imports System.Web.Script.Serialization

''' <summary>
''' 部品庫モニタークラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3190402
    Inherits BasePage

    ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
#Region "文言ID"

    ''' <summary>
    ''' 文言ID：中断中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdStop As String = "6"

#End Region
    ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

#Region "プロパティ"
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private ReadOnly Property DisplayID() As String
        Get
            Return "SC3190402"
        End Get
    End Property

    ''' <summary>
    ''' 現在日時(基盤関数にて取得)
    ''' </summary>
    Private _nowdate As String
    Public Property NowDate() As Date
        Get
            Return _nowdate
        End Get
        Set(ByVal value As Date)
            _nowdate = value
        End Set
    End Property

    ''' <summary>
    ''' 新着有無
    ''' </summary>
    ''' <remarks>見積もり待ちエリアまたは出庫待ちエリアの新着有無</remarks>
    Private _iswhatsnew As Integer
    Public Property IsWhatsNew() As SC3190402BusinessLogic.Enum_WhatsNew
        Get
            Return _iswhatsnew
        End Get
        Set(ByVal value As SC3190402BusinessLogic.Enum_WhatsNew)
            _iswhatsnew = value
        End Set
    End Property

    ''' <summary>
    ''' 追加作業見積もり待ちエリアの遅れ時間
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingdelayperiodminute As Integer
    Public Property SettingDelayPeriodMinute() As Integer
        Get
            Return _settingdelayperiodminute
        End Get
        Set(ByVal value As Integer)
            _settingdelayperiodminute = value
        End Set
    End Property

    ''' <summary>
    ''' 各エリアの明細最大表示数
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingdisplaychipsmaxcount As Integer
    Public Property SettingChipDispMaxCount() As Integer
        Get
            Return _settingdisplaychipsmaxcount
        End Get
        Set(ByVal value As Integer)
            _settingdisplaychipsmaxcount = value
        End Set
    End Property

    ''' <summary>
    ''' 追加作業見積り待ちエリア改ページ間隔(秒)
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingpagingintervaladdjob As Integer
    Public Property SettingPagingIntervalAddJob() As Integer
        Get
            Return _settingpagingintervaladdjob
        End Get
        Set(ByVal value As Integer)
            _settingpagingintervaladdjob = value
        End Set
    End Property

    ''' <summary>
    ''' 作業計画待ちエリア改ページ間隔(秒)
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingpagingintervaljobinstruct As Integer
    Public Property SettingPagingIntervalJobInstruct() As Integer
        Get
            Return _settingpagingintervaljobinstruct
        End Get
        Set(ByVal value As Integer)
            _settingpagingintervaljobinstruct = value
        End Set
    End Property

    ''' <summary>
    ''' 出庫待ちエリア改ページ間隔(秒)
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingpagingintervalshipment As Integer
    Public Property SettingPagingIntervalShipment() As Integer
        Get
            Return _settingpagingintervalshipment
        End Get
        Set(ByVal value As Integer)
            _settingpagingintervalshipment = value
        End Set
    End Property

    ''' <summary>
    ''' 引き取り待ちエリア改ページ間隔(秒)
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingpagingintervalpick As Integer
    Public Property SettingPagingIntervalPick() As Integer
        Get
            Return _settingpagingintervalpick
        End Get
        Set(ByVal value As Integer)
            _settingpagingintervalpick = value
        End Set
    End Property

    ''' <summary>
    ''' 各エリアの取得データ最大件数
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _settingchipacquisitionmaxcount As Integer
    Public Property SettingChipAcquisitionMaxCount() As Integer
        Get
            Return _settingchipacquisitionmaxcount
        End Get
        Set(ByVal value As Integer)
            _settingchipacquisitionmaxcount = value
        End Set
    End Property
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
    ''' <summary>
    ''' 更新処理の設定を取得(分)
    ''' </summary>
    ''' <remarks>販売店システム設定より取得しセットする</remarks>
    Private _psmonitorDelayUpdateInterval As Integer
    Public Property PsmonitorDelayUpdateInterval() As Integer
        Get
            Return _psmonitorDelayUpdateInterval
        End Get
        Set(ByVal value As Integer)
            _psmonitorDelayUpdateInterval = value
        End Set
    End Property
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05
#End Region

#Region "ページイベント"
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'ポストバック以外(初回起動のみ行う処理)
        If Not Me.IsPostBack Then
            '初回起動時のみ行う処理
            Me.InitializeControl()
            '初期値セット
            Me.RefreshControl()
            '全エリア更新
            Me.MainRefresh()
        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub
#End Region

#Region "全エリアの更新処理"
    ''' <summary>
    ''' 全エリアの更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub MainRefresh()
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))

        'ログイン情報
        Dim dealerCd As String = StaffContext.Current.DlrCD
        Dim branchCd As String = StaffContext.Current.BrnCD
        Dim account As String = StaffContext.Current.Account
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
        Dim branchWorkTimeDataTable As SC3190402DataSet.BranchWorkTimeDataTable

        ' 店舗の営業時間を取得する。
        Using businessLogic As SC3190402BusinessLogic = New SC3190402BusinessLogic
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'branchWorkTimeDataTable = businessLogic.GetBranchWorkTime(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)
            branchWorkTimeDataTable = businessLogic.GetBranchWorkTime(dealerCd, branchCd)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
        End Using
        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
        '追加作業見積もり待ちデータ表示
        Dim PageCntArea01 As Integer = Me.RefrashWaitingforPartsQuotationArea(branchWorkTimeDataTable)
        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        '作業計画待ちデータ表示
        Dim PageCntArea02 As Integer = Me.RefrashWaitingforJobPlanningArea()

        '$01 部品庫B／O管理に向けた評価用アプリ作成 START

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Using businessLogic As SC3190402BusinessLogic = New SC3190402BusinessLogic
        '    'かごの解放
        '    businessLogic.ReleaseCage(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD, Me.NowDate, StaffContext.Current.Account)
        'End Using

        '出庫待ち表示対象データセット
        Dim area04Data As SC3190402DataSet.AREA04ResDataTable
        '抽出件数（出庫待ち）
        Dim selectDataCountArea03 As Integer = 0
        '引き取り待ち表示対象データセット
        Dim area03Data As SC3190402DataSet.AREA03DataTable
        '抽出件数（引き取り待ち）
        Dim selectDataCountArea04 As Integer = 0


        Using businessLogic As SC3190402BusinessLogic = New SC3190402BusinessLogic

            '引き取り待ちデータ取得
            area04Data = businessLogic.GetWaitingforTechnicianPickupListData(Me.NowDate, Me.SettingChipAcquisitionMaxCount, selectDataCountArea04)

            '出庫待ちデータ取得
            area03Data = businessLogic.GetWaitingforPartsIssuingListData(Me.NowDate, Me.SettingChipAcquisitionMaxCount, selectDataCountArea03)

            'かご更新
            businessLogic.UpdateCageInfo(dealerCd, branchCd, Me.NowDate, account, area03Data, area04Data)

        End Using
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
        '引き取り待ちデータ表示
        'Dim PageCntArea04 As Integer = Me.RefrashWaitingforTechnicianPickupArea(branchWorkTimeDataTable)
        Dim PageCntArea04 As Integer = Me.RefrashWaitingforTechnicianPickupArea(branchWorkTimeDataTable, selectDataCountArea04, area04Data)

        '出庫待ちデータ表示
        'Dim PageCntArea03 As Integer = Me.RefrashWaitingforPartsIssuingArea(branchWorkTimeDataTable)
        Dim PageCntArea03 As Integer = Me.RefrashWaitingforPartsIssuingArea(branchWorkTimeDataTable, selectDataCountArea03, area03Data)
        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
        '$01 部品庫B／O管理に向けた評価用アプリ作成 END

        '2014/07/15 改ページ機能追加
        'JavaScriptの呼び出し／実行
        Dim sbBuff As StringBuilder = New StringBuilder

        If PageCntArea01 > 0 Then
            sbBuff.Append("SetIndicator('ulSubAreaBox01'")                      'チップ表示エリアID
            sbBuff.Append(",'Indi01_'")                                         'インジケーターID
            sbBuff.Append(",").Append("1")                                      '表示位置(初期なので「1」固定)
            sbBuff.Append(",").Append(PageCntArea01)                            'ページ数
            sbBuff.Append(",").Append(Me.SettingChipDispMaxCount)               '1ページあたり表示チップ数
            sbBuff.Append(",").Append(Me.SettingPagingIntervalAddJob * 1000)    '更新間隔(秒→ミリ秒変換)
            sbBuff.Append(");")

            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Area01 sbBuff.ToString=" & sbBuff.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key01", sbBuff.ToString, True)
        End If

        If PageCntArea02 > 0 Then
            sbBuff.Length = 0
            sbBuff.Append("SetIndicator('ulSubAreaBox02'")                        'チップ表示エリアID
            sbBuff.Append(",'Indi02_'")                                           'インジケーターID
            sbBuff.Append(",").Append("1")                                        '表示位置(初期なので「1」固定)
            sbBuff.Append(",").Append(PageCntArea02)                              'ページ数
            sbBuff.Append(",").Append(Me.SettingChipDispMaxCount)                 '1ページあたり表示チップ数
            sbBuff.Append(",").Append(Me.SettingPagingIntervalJobInstruct * 1000) '更新間隔(秒→ミリ秒変換)
            sbBuff.Append(");")

            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Area02 sbBuff.ToString=" & sbBuff.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key02", sbBuff.ToString, True)
        End If

        If PageCntArea03 > 0 Then
            sbBuff.Length = 0
            sbBuff.Append("SetIndicator('ulSubAreaBox03'")                      'チップ表示エリアID
            sbBuff.Append(",'Indi03_'")                                         'インジケーターID
            sbBuff.Append(",").Append("1")                                      '表示位置(初期なので「1」固定)
            sbBuff.Append(",").Append(PageCntArea03)                            'ページ数
            sbBuff.Append(",").Append(Me.SettingChipDispMaxCount)               '1ページあたり表示チップ数
            sbBuff.Append(",").Append(Me.SettingPagingIntervalShipment * 1000)  '更新間隔(秒→ミリ秒変換)
            sbBuff.Append(");")

            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Area03 sbBuff.ToString=" & sbBuff.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key03", sbBuff.ToString, True)
        End If

        If PageCntArea04 > 0 Then
            sbBuff.Length = 0
            sbBuff.Append("SetIndicator('ulSubAreaBox04'")                      'チップ表示エリアID
            sbBuff.Append(",'Indi04_'")                                         'インジケーターID
            sbBuff.Append(",").Append("1")                                      '表示位置(初期なので「1」固定)
            sbBuff.Append(",").Append(PageCntArea04)                            'ページ数
            sbBuff.Append(",").Append(Me.SettingChipDispMaxCount)               '1ページあたり表示チップ数
            sbBuff.Append(",").Append(Me.SettingPagingIntervalPick * 1000)      '更新間隔(秒→ミリ秒変換)
            sbBuff.Append(");")

            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info("DEBUG:Area04 sbBuff.ToString=" & sbBuff.ToString)
            '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key04", sbBuff.ToString, True)
        End If

        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
        sbBuff.Length = 0
        sbBuff.Append("SetUpdateIntervalTime(").Append(Me.NowDate.Year)             'サーバー時間(年)
        sbBuff.Append(",").Append(Me.NowDate.Month)                         'サーバー時間(月)
        sbBuff.Append(",").Append(Me.NowDate.Day)                           'サーバー時間(日)
        sbBuff.Append(",").Append(Me.NowDate.Hour)                          'サーバー時間(時間)
        sbBuff.Append(",").Append(Me.NowDate.Minute)                        'サーバー時間(分)
        sbBuff.Append(",").Append(Me.NowDate.Second)                        'サーバー時間(秒)

        sbBuff.Append(",").Append(Me.PsmonitorDelayUpdateInterval * 60 * 1000)      '更新間隔(分→ミリ秒変換)
        sbBuff.Append(");")

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "Key05", sbBuff.ToString, True)

        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05
        sbBuff = Nothing

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub
#End Region

#Region "追加作業見積もり待ちエリアの更新"
    ''' <summary>
    ''' 追加作業見積もり待ちエリアの更新
    ''' </summary>
    ''' <param name="branchWorkTime">店舗営業時間情報</param>
    ''' <returns>ページ数</returns>
    ''' <remarks>追加作業見積もり待ちエリアの更新処理を行う</remarks>
    Protected Function RefrashWaitingforPartsQuotationArea( _
        ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable) As Integer

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} branchWorkTime IsNull {3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart _
        '    , branchWorkTime Is Nothing))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数
        Dim PageCnt As Integer = 0

        ' データ取得用のビジネスロジック作成
        Using businessLogic As New SC3190402BusinessLogic
            Dim selectDataCount As Integer = 0
            'データの取得
            '2014/07/15 改ページ機能追加
            'Using resultDataTable As SC3190402DataSet.AREA01DataTable = _
            'businessLogic.GetWaitingforPartsQuotationListData(Me.NowDate, _
            '                                                  Me.SettingDelayPeriodMinute, _
            '                                                  Me.SettingChipDispMaxCount, _
            '                                                  selectDataCount)
            Using resultDataTable As SC3190402DataSet.AREA01DataTable = _
            businessLogic.GetWaitingforPartsQuotationListData(Me.NowDate, _
                                                              Me.SettingDelayPeriodMinute, _
                                                              Me.SettingChipAcquisitionMaxCount, _
                                                              selectDataCount)
                'エリアタイトルに件数をセット
                Me.lblAreaCount01.Text = selectDataCount.ToString

                'セッション関連
                Dim dic As New Dictionary(Of String, Integer)
                Dim dicKey As String = String.Empty
                Dim dicSave As New Dictionary(Of String, Integer)
                Dim dicSaveKey As String = String.Empty
                Dim refreshKbn As SC3190402BusinessLogic.Enum_RefreshKbn

                '初回起動チェック
                If MyBase.ContainsKey(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea01RoData) = False Then
                    'セッション情報が存在しないとき、初回起動とみなす
                    refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispInit
                Else
                    'セッション情報が存在したとき、画面更新とみなす(取得後に削除)
                    dicSave = DirectCast(MyBase.GetValue(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea01RoData, True), Dictionary(Of String, Integer))
                    refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispRefresh
                End If

                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info("DEBUG:resultDataTable.Rows.Count.ToString=" & resultDataTable.Rows.Count.ToString)
                '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                If resultDataTable.Rows.Count > 0 Then
                    'Masterページからの階層でコントロールを取得する
                    Dim ul As HtmlGenericControl = CType(CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("ulSubAreaBox01"), HtmlGenericControl)
                    Dim li As HtmlGenericControl
                    Dim divAreaBack As HtmlGenericControl
                    Dim divSubAreaDetail As HtmlGenericControl
                    Dim divRoNum As HtmlGenericControl
                    Dim divRegNum As HtmlGenericControl
                    Dim divModelName As HtmlGenericControl
                    Dim spnStaffName As HtmlGenericControl
                    Dim divStaffName As HtmlGenericControl
                    Dim divSmark As HtmlGenericControl
                    Dim divDispTime As HtmlGenericControl
                    Dim audio As HtmlGenericControl

                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    'Logger.Info("DEBUG:ul.TagName.ToString=" & ul.TagName.ToString)
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

                    'エリアの明細最大数あるいは取得データ数だけループ
                    For i As Integer = 0 To resultDataTable.Rows.Count - 1
                        '各項目の初期化
                        li = New HtmlGenericControl("li")
                        divAreaBack = New HtmlGenericControl("div")
                        divSubAreaDetail = New HtmlGenericControl("div")
                        divRoNum = New HtmlGenericControl("div")
                        divRegNum = New HtmlGenericControl("div")
                        divModelName = New HtmlGenericControl("div")
                        spnStaffName = New HtmlGenericControl("span")
                        divStaffName = New HtmlGenericControl("div")
                        divDispTime = New HtmlGenericControl("div")

                        '明細大枠以外のクラスをセットする
                        divSubAreaDetail.Attributes.Add("class", "divSubAreaDetail")
                        divRoNum.Attributes.Add("class", "RoNum")
                        divRegNum.Attributes.Add("class", "RegNum")
                        divModelName.Attributes.Add("class", "ModelName")
                        divStaffName.Attributes.Add("class", "StaffName")
                        divDispTime.Attributes.Add("class", "DispTime")

                        Dim row As SC3190402DataSet.AREA01Row = resultDataTable.Rows(i)
                        '赤明細のチェックを行い、明細大枠のクラスをセットする
                        If row.SORT_KEY = SC3190402BusinessLogic.Enum_BackColor.Red Then
                            divAreaBack.Attributes("class") = "divAreaBack BackColorRed"
                        Else
                            divAreaBack.Attributes("class") = "divAreaBack"
                        End If

                        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
                        Dim targetDate As DateTime = row.RO_CREATE_DATETIME.AddMinutes(Me.SettingDelayPeriodMinute)
                        Dim targetDateString As String = targetDate.Year & "," & targetDate.Month & "," & targetDate.Day & "," & targetDate.Hour & "," & targetDate.Minute
                        divSubAreaDetail.Attributes.Add("delaydatetime", targetDateString)
                        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

                        '新着チェック処理
                        If refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispRefresh Then
                            'セッション情報にRO情報が存在するかチェックする(画面更新のときのみ)
                            dicSaveKey = SC3190402BusinessLogic.MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                            'RO情報がセッション情報に存在しないときは新着とみなし、点滅クラスを追加する
                            If dicSave.ContainsKey(dicSaveKey) = False Then
                                divAreaBack.Attributes.Add("class", divAreaBack.Attributes("class") & " Blinking")
                                '新着有無プロパティに新着ありをセットする
                                If Me.IsWhatsNew = SC3190402BusinessLogic.Enum_WhatsNew.No Then
                                    '音声ファイルをセット
                                    audio = New HtmlGenericControl("audio")
                                    audio.Attributes("src") = SC3190402BusinessLogic.ConsWhatsNewMP3
                                    audio.Attributes("autoplay") = "autoplay"
                                    ul.Controls.Add(audio)
                                    Me.IsWhatsNew = SC3190402BusinessLogic.Enum_WhatsNew.Yes
                                End If
                            End If
                        End If

                        '明細の各項目を編集する
                        divRoNum.InnerText = SC3190402BusinessLogic.MakeRoNumAndSeq(row.RO_NUM.ToString.TrimEnd, _
                                                             row.RO_SEQ.ToString.TrimEnd)
                        divRegNum.InnerText = row.REG_NUM.ToString.TrimEnd
                        divModelName.InnerText = SC3190402BusinessLogic.MakeModelAndGradeName(row.MODEL_NAME.ToString.TrimEnd, _
                                                                       row.GRADE_NAME.ToString.TrimEnd)
                        spnStaffName.InnerText = row.USERNAME.ToString.TrimEnd

                        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                        divDispTime.InnerText = ChangeFormatDataTime(row.RO_CREATE_DATETIME, branchWorkTime)
                        ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                        divSubAreaDetail.Controls.Add(divRoNum)
                        divSubAreaDetail.Controls.Add(divRegNum)
                        divSubAreaDetail.Controls.Add(divModelName)
                        divStaffName.Controls.Add(spnStaffName)
                        divSubAreaDetail.Controls.Add(divStaffName)

                        'ROステータス=30のときだけ見積りアイコンを付加する
                        If row.RO_STATUS.ToString.TrimEnd.Equals(SC3190402BusinessLogic.ConsRoStatus_30) Then
                            divSmark = New HtmlGenericControl("div")
                            divSmark.Attributes("class") = "Smark"
                            divSubAreaDetail.Controls.Add(divSmark)
                        End If

                        divAreaBack.Controls.Add(divSubAreaDetail)
                        divAreaBack.Controls.Add(divDispTime)
                        li.Controls.Add(divAreaBack)
                        ul.Controls.Add(li)

                        '今回抽出したRO情報をDictionaryに追加する
                        dicKey = SC3190402BusinessLogic.MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                        dic.Add(dicKey, 0)
                    Next

                    '2014/07/15 改ページ機能追加
                    'ページ数の算出(取得データ件数/1ページあたりの表示最大件数)
                    PageCnt = Integer.Parse(Math.Ceiling(resultDataTable.Rows.Count / Me.SettingChipDispMaxCount))

                Else
                    '抽出件数が0件のときは無効データを1件保存する
                    dic.Add("0", 0)
                End If
                'セッション情報に表示データのRO情報を保存する
                MyBase.SetValue(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea01RoData, dic)
            End Using
        End Using

        '2014/07/15 改ページ機能追加
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} PAGE:CNT={3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd _
        '    , PageCnt))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数を返却する
        Return PageCnt

    End Function
#End Region

#Region "作業計画待ちエリアの更新"
    ''' <summary>
    ''' 作業計画待ちエリアの更新
    ''' </summary>
    ''' <returns>ページ数</returns>
    ''' <remarks>作業計画待ちエリアの更新処理を行う</remarks>
    Protected Function RefrashWaitingforJobPlanningArea() As Integer
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END


        '2014/07/15 改ページ機能追加
        'ページ数
        Dim PageCnt As Integer = 0

        ' データ取得用のビジネスロジック作成
        Using businessLogic As New SC3190402BusinessLogic
            Dim selectDataCount As Integer = 0
            'データの取得
            '2014/07/15 改ページ機能追加
            'Using resultDataTable As SC3190402DataSet.AREA02DataTable = _
            '    businessLogic.GetWaitingforJobPlanningListData(Me.NowDate, _
            '                                                   Me.SettingChipDispMaxCount, _
            '                                                   selectDataCount)
            Using resultDataTable As SC3190402DataSet.AREA02DataTable = _
                businessLogic.GetWaitingforJobPlanningListData(Me.NowDate, _
                                                               Me.SettingChipAcquisitionMaxCount, _
                                                               selectDataCount)

                'エリアタイトルに件数をセット
                Me.lblAreaCount02.Text = selectDataCount.ToString

                If resultDataTable.Rows.Count > 0 Then

                    'Masterページからの階層でコントロールを取得する
                    Dim ul As HtmlGenericControl = CType(CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("ulSubAreaBox02"), HtmlGenericControl)
                    Dim li As HtmlGenericControl
                    Dim divAreaBack As HtmlGenericControl
                    Dim divSubAreaDetail As HtmlGenericControl
                    Dim divRoNum As HtmlGenericControl
                    Dim divRegNum As HtmlGenericControl
                    Dim divModelName As HtmlGenericControl
                    Dim divWmark As HtmlGenericControl
                    ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                    Dim divBackOrder As HtmlGenericControl
                    ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                    For i As Integer = 0 To resultDataTable.Rows.Count - 1
                        '各項目の初期化
                        li = New HtmlGenericControl("li")
                        divAreaBack = New HtmlGenericControl("div")
                        divSubAreaDetail = New HtmlGenericControl("div")
                        divRoNum = New HtmlGenericControl("div")
                        divRegNum = New HtmlGenericControl("div")
                        divModelName = New HtmlGenericControl("div")
                        ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                        divBackOrder = New HtmlGenericControl("div")
                        ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                        '明細大枠以外のクラスをセットする
                        divSubAreaDetail.Attributes.Add("class", "divSubAreaDetail02")
                        divRoNum.Attributes.Add("class", "RoNum")
                        divRegNum.Attributes.Add("class", "RegNum02")
                        divModelName.Attributes.Add("class", "ModelName02")
                        ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                        divBackOrder.Attributes.Add("class", "BackOrder")
                        ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                        Dim row As SC3190402DataSet.AREA02Row = resultDataTable.Rows(i)
                        '赤明細のチェックを行い、明細大枠のクラスをセットする
                        If row.SORT_KEY = SC3190402BusinessLogic.Enum_BackColor.Red Then
                            divAreaBack.Attributes("class") = "divAreaBack BackColorRed"
                        Else
                            divAreaBack.Attributes("class") = "divAreaBack"
                        End If

                        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
                        Dim targetDate As DateTime = row.SCHE_DELI_DATETIME
                        Dim targetDateString As String = targetDate.Year & "," & targetDate.Month & "," & targetDate.Day & "," & targetDate.Hour & "," & targetDate.Minute
                        divSubAreaDetail.Attributes.Add("schedelidatetime", targetDateString)
                        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

                        '明細の各項目を編集する
                        divRoNum.InnerText = SC3190402BusinessLogic.MakeRoNumAndSeq(row.RO_NUM.ToString.TrimEnd, _
                                                             row.RO_SEQ.ToString.TrimEnd)
                        divRegNum.InnerText = row.REG_NUM.ToString.TrimEnd
                        divModelName.InnerText = row.MODEL_NAME.ToString.TrimEnd

                        divSubAreaDetail.Controls.Add(divRoNum)
                        divSubAreaDetail.Controls.Add(divRegNum)
                        divSubAreaDetail.Controls.Add(divModelName)

                        ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                        ' 部品ステータスが4:在庫無しの場合
                        If row.PARTS_ISSUE_STATUS.Equals(SC3190402BusinessLogic.ConsPartsIssueStatus_NoStock) Then
                            divBackOrder.InnerText = Server.HtmlEncode(WebWordUtility.GetWord(Me.DisplayID, WordIdStop))
                            divSubAreaDetail.Controls.Add(divBackOrder)
                        End If
                        ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                        '追加作業のときだけ追加作業アイコンを付加する
                        If Integer.Parse(row.RO_SEQ) >= SC3190402BusinessLogic.ConsAddRepair Then
                            divWmark = New HtmlGenericControl("div")
                            divWmark.Attributes("class") = "Wmark"
                            divSubAreaDetail.Controls.Add(divWmark)
                        End If
                        divAreaBack.Controls.Add(divSubAreaDetail)
                        li.Controls.Add(divAreaBack)
                        ul.Controls.Add(li)
                    Next

                    '2014/07/15 改ページ機能追加
                    'ページ数の算出(取得データ件数/1ページあたりの表示最大件数)
                    PageCnt = Integer.Parse(Math.Ceiling(resultDataTable.Rows.Count / Me.SettingChipDispMaxCount))

                End If
            End Using
        End Using

        '2014/07/15 改ページ機能追加
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} PAGE:CNT={3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd _
        '    , PageCnt))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数を返却する
        Return PageCnt

    End Function
#End Region

#Region "出庫待ちエリアの更新"
    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ' ''' <summary>
    ' ''' 出庫待ちエリアの更新
    ' ''' </summary>
    ' ''' <param name="branchWorkTime">店舗営業時間情報</param>
    ' ''' <returns>ページ数</returns>
    ' ''' <remarks>出庫待ちエリアの更新処理を行う</remarks>
    'Protected Function RefrashWaitingforPartsIssuingArea( _
    '    ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable) As Integer

    ''' <summary>
    ''' 出庫待ちエリアの更新
    ''' </summary>
    ''' <param name="branchWorkTime">店舗営業時間情報</param>
    ''' <param name="selectDataCount">抽出件数</param>
    ''' <param name="resultDataTable">表示対象データセット</param>
    ''' <returns>ページ数</returns>
    ''' <remarks>出庫待ちエリアの更新処理を行う</remarks>
    Protected Function RefrashWaitingforPartsIssuingArea( _
        ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable, _
        ByVal selectDataCount As Integer, _
        ByRef resultDataTable As SC3190402DataSet.AREA03DataTable) As Integer
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} branchWorkTime IsNull {3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart _
        '    , branchWorkTime Is Nothing))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数
        Dim PageCnt As Integer = 0

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        '' データ取得用のビジネスロジック作成
        'Using businessLogic As SC3190402BusinessLogic = New SC3190402BusinessLogic
        'Dim selectDataCount As Integer = 0
        ''データの取得
        ''2014/07/15 改ページ機能追加
        ''Using resultDataTable As SC3190402DataSet.AREA03DataTable = _
        ''    businessLogic.GetWaitingforPartsIssuingListData(Me.NowDate, _
        ''                                                    Me.SettingChipDispMaxCount, _
        ''                                                    selectDataCount)
        'Using resultDataTable As SC3190402DataSet.AREA03DataTable = _
        '    businessLogic.GetWaitingforPartsIssuingListData(Me.NowDate, _
        '                                                    Me.SettingChipAcquisitionMaxCount, _
        '                                                    selectDataCount)
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'エリアタイトルに件数をセット
        Me.lblAreaCount03.Text = selectDataCount.ToString

        'セッション関連
        Dim dic As New Dictionary(Of String, Integer)
        Dim dicKey As String = String.Empty
        Dim dicSave As New Dictionary(Of String, Integer)
        Dim dicSaveKey As String = String.Empty
        Dim refreshKbn As SC3190402BusinessLogic.Enum_RefreshKbn

        '初回起動チェック
        If MyBase.ContainsKey(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea03RoData) = False Then
            'セッション情報が存在しないとき、初回起動とみなす
            refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispInit
        Else
            'セッション情報が存在したとき、画面更新とみなす(取得後に削除)
            dicSave = DirectCast(MyBase.GetValue(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea03RoData, True), Dictionary(Of String, Integer))
            refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispRefresh
        End If

        If resultDataTable.Rows.Count > 0 Then
            'Masterページからの階層でコントロールを取得する
            Dim ul As HtmlGenericControl = CType(CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("ulSubAreaBox03"), HtmlGenericControl)
            Dim li As HtmlGenericControl
            Dim divAreaBack As HtmlGenericControl
            Dim divSubAreaDetail As HtmlGenericControl
            Dim divRoNum As HtmlGenericControl
            Dim divRegNum As HtmlGenericControl
            Dim divModelName As HtmlGenericControl
            Dim divStallName As HtmlGenericControl
            Dim divWmark As HtmlGenericControl
            Dim divDispTime As HtmlGenericControl
            Dim audio As HtmlGenericControl
            '$01 部品庫B／O管理に向けた評価用アプリ作成 START
            Dim divBoxmark As HtmlGenericControl
            Dim divBoxmarkWmark As HtmlGenericControl
            '$01 部品庫B／O管理に向けた評価用アプリ作成 END
            ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
            Dim divBackOrder As HtmlGenericControl
            ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

            For i As Integer = 0 To resultDataTable.Rows.Count - 1
                '各項目の初期化
                li = New HtmlGenericControl("li")
                divAreaBack = New HtmlGenericControl("div")
                divSubAreaDetail = New HtmlGenericControl("div")
                divRoNum = New HtmlGenericControl("div")
                divRegNum = New HtmlGenericControl("div")
                divModelName = New HtmlGenericControl("div")
                divStallName = New HtmlGenericControl("div")
                ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                divBackOrder = New HtmlGenericControl("div")
                ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                divSubAreaDetail.Attributes.Add("class", "divSubAreaDetail")
                divRoNum.Attributes.Add("class", "RoNum")
                divRegNum.Attributes.Add("class", "RegNum")
                divModelName.Attributes.Add("class", "ModelName")
                divStallName.Attributes.Add("class", "StallName")
                ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                divBackOrder.Attributes.Add("class", "BackOrder")
                ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                Dim row As SC3190402DataSet.AREA03Row = resultDataTable.Rows(i)

                '赤明細のチェックを行い、明細大枠のクラスをセットする
                If row.SORT_KEY = SC3190402BusinessLogic.Enum_BackColor.Red Then
                    divAreaBack.Attributes("class") = "divAreaBack BackColorRed"
                Else
                    divAreaBack.Attributes("class") = "divAreaBack"
                End If

                'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
                If Not row.IsSCHE_START_DATETIMENull Then
                    Dim targetDate As DateTime = row.SCHE_START_DATETIME
                    Dim targetDateString As String = targetDate.Year & "," & targetDate.Month & "," & targetDate.Day & "," & targetDate.Hour & "," & targetDate.Minute
                    divSubAreaDetail.Attributes.Add("schestartdatetime", targetDateString)
                    divSubAreaDetail.Attributes.Add("ro_num", row.RO_NUM)
                    divSubAreaDetail.Attributes.Add("ro_seq", row.RO_SEQ)
                    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05
                End If
                '新着チェック処理
                If refreshKbn = SC3190402BusinessLogic.Enum_RefreshKbn.DispRefresh Then
                    'セッション情報にRO情報が存在するかチェックする(画面更新のときのみ)
                    dicSaveKey = SC3190402BusinessLogic.MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                    'RO情報がセッション情報に存在しないときは新着とみなし、点滅クラスを追加する
                    If dicSave.ContainsKey(dicSaveKey) = False Then
                        divAreaBack.Attributes.Add("class", divAreaBack.Attributes("class") & " Blinking")
                        '新着有無プロパティに新着ありをセットする
                        '※すでに追加作業見積もり待ちで新着ありがセットされていたときは
                        '　出庫待ちで新着があってもセットしない
                        If Me.IsWhatsNew = SC3190402BusinessLogic.Enum_WhatsNew.No Then
                            '音声ファイルをセット
                            audio = New HtmlGenericControl("audio")
                            audio.Attributes("src") = SC3190402BusinessLogic.ConsWhatsNewMP3
                            audio.Attributes("autoplay") = "autoplay"
                            ul.Controls.Add(audio)
                            Me.IsWhatsNew = SC3190402BusinessLogic.Enum_WhatsNew.Yes
                        End If
                    End If
                End If

                '明細の各項目を編集する
                divRoNum.InnerText = SC3190402BusinessLogic.MakeRoNumAndSeq(row.RO_NUM.ToString.TrimEnd, _
                                                     row.RO_SEQ.ToString.TrimEnd)
                divRegNum.InnerText = row.REG_NUM.ToString.TrimEnd
                divModelName.InnerText = SC3190402BusinessLogic.MakeModelAndGradeName(row.MODEL_NAME.ToString.TrimEnd, _
                                                               row.GRADE_NAME.ToString.TrimEnd)
                divStallName.InnerText = row.STALL_NAME_SHORT.ToString

                divSubAreaDetail.Controls.Add(divRoNum)
                divSubAreaDetail.Controls.Add(divRegNum)
                divSubAreaDetail.Controls.Add(divModelName)
                divSubAreaDetail.Controls.Add(divStallName)

                ' $02 Start サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)
                ' 部品ステータスが4:在庫無しの場合
                If row.PARTS_ISSUE_STATUS.Equals(SC3190402BusinessLogic.ConsPartsIssueStatus_NoStock) Then
                    divBackOrder.InnerText = Server.HtmlEncode(WebWordUtility.GetWord(Me.DisplayID, WordIdStop))
                    divSubAreaDetail.Controls.Add(divBackOrder)
                End If
                ' $02 End   サービスタブレットDMS連携追加開発(部品庫モニター在庫無し表示)

                '$01 部品庫B／O管理に向けた評価用アプリ作成 START

                divBoxmarkWmark = New HtmlGenericControl("div")
                divBoxmarkWmark.Attributes("class") = "BoxmarkWmark" '変える

                'カゴNoが存在したときだけ表示
                If row.CAGE_NO.ToString.TrimEnd.Length > 0 Then
                    divBoxmark = New HtmlGenericControl("div")

                    If Integer.Parse(row.RO_SEQ) >= SC3190402BusinessLogic.ConsAddRepair Then
                        divBoxmark.Attributes("class") = "Boxmark MaxWidth70"
                    Else
                        divBoxmark.Attributes("class") = "Boxmark MaxWidth106"
                    End If

                    divBoxmark.InnerText = row.CAGE_NO.ToString
                    divBoxmarkWmark.Controls.Add(divBoxmark)
                End If

                '追加作業のときだけ追加作業アイコンを付加する(出庫待ちエリア専用クラス)
                If Integer.Parse(row.RO_SEQ) >= SC3190402BusinessLogic.ConsAddRepair Then
                    divWmark = New HtmlGenericControl("div")
                    divWmark.Attributes("class") = "Wmark03"
                    divBoxmarkWmark.Controls.Add(divWmark)
                End If

                divSubAreaDetail.Controls.Add(divBoxmarkWmark)
                '$01 部品庫B／O管理に向けた評価用アプリ作成 END

                divAreaBack.Controls.Add(divSubAreaDetail)

                '2014/07/31 部品のみ出庫対応によるNull値の考慮(予定開始日時)
                If Not row.IsSCHE_START_DATETIMENull Then
                    divDispTime = New HtmlGenericControl("div")
                    divDispTime.Attributes.Add("class", "DispTime")

                    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                    divDispTime.InnerText = ChangeFormatDataTime(row.SCHE_START_DATETIME, branchWorkTime)
                    ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END
                    divAreaBack.Controls.Add(divDispTime)
                Else
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                    'Logger.Info("DEBUG:部品のみ出庫／予定開始日時をブランク表示")
                    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                End If

                li.Controls.Add(divAreaBack)
                ul.Controls.Add(li)

                '今回抽出したRO情報をDictionaryに追加する
                dicKey = SC3190402BusinessLogic.MakeDictionaryKey(row.RO_NUM.ToString.TrimEnd, row.RO_SEQ.ToString.TrimEnd)
                dic.Add(dicKey, 0)
            Next

            '2014/07/15 改ページ機能追加
            'ページ数の算出(取得データ件数/1ページあたりの表示最大件数)
            PageCnt = Integer.Parse(Math.Ceiling(resultDataTable.Rows.Count / Me.SettingChipDispMaxCount))

        Else
            '抽出件数が0件のときは無効データを1件保存する
            dic.Add("0", 0)
        End If
        'セッション情報に表示データのRO情報を保存する
        MyBase.SetValue(ScreenPos.Current, SC3190402BusinessLogic.SessionKeyArea03RoData, dic)

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'End Using
        'End Using
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} PAGE:CNT={3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd _
        '    , PageCnt))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数を返却する
        Return PageCnt

    End Function
#End Region

#Region "引き取り待ちエリアの更新"
    '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ' ''' <summary>
    ' ''' 引き取り待ちエリアの更新
    ' ''' </summary>
    ' ''' <param name="branchWorkTime">店舗営業時間情報</param>
    ' ''' <returns>ページ数</returns>
    ' ''' <remarks>引き取り待ちエリアの更新処理を行う</remarks>
    'Protected Function RefrashWaitingforTechnicianPickupArea( _
    'ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable) As Integer

    ''' <summary>
    ''' 引き取り待ちエリアの更新
    ''' </summary>
    ''' <param name="branchWorkTime">店舗営業時間情報</param>
    ''' <param name="selectDataCount">抽出件数</param>
    ''' <param name="resultDataTable">表示対象データセット</param>
    ''' <returns>ページ数</returns>
    ''' <remarks>引き取り待ちエリアの更新処理を行う</remarks>
    Protected Function RefrashWaitingforTechnicianPickupArea( _
        ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable, _
        ByVal selectDataCount As Integer, _
        ByRef resultDataTable As SC3190402DataSet.AREA04ResDataTable) As Integer
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} branchWorkTime IsNull {3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart _
        '    , branchWorkTime Is Nothing))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数
        Dim PageCnt As Integer = 0

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        '' データ取得用のビジネスロジック作成
        'Using businessLogic As SC3190402BusinessLogic = New SC3190402BusinessLogic
        'Dim selectDataCount As Integer = 0
        ''データの取得
        ''2014/07/15 改ページ機能追加
        ''Using resultDataTable As SC3190402DataSet.AREA04ResDataTable = _
        ''    businessLogic.GetWaitingforTechnicianPickupListData(Me.NowDate, _
        ''                                                        Me.SettingChipDispMaxCount, _
        ''                                                        selectDataCount)
        'Using resultDataTable As SC3190402DataSet.AREA04ResDataTable = _
        '    businessLogic.GetWaitingforTechnicianPickupListData(Me.NowDate, _
        '                                                        Me.SettingChipAcquisitionMaxCount, _
        '                                                        selectDataCount)
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'エリアタイトルに件数をセット
        Me.lblAreaCount04.Text = selectDataCount.ToString

        If resultDataTable.Rows.Count > 0 Then
            'Masterページからの階層でコントロールを取得する
            Dim ul As HtmlGenericControl = CType(CType(Master.FindControl("content"), ContentPlaceHolder).FindControl("ulSubAreaBox04"), HtmlGenericControl)
            Dim li As HtmlGenericControl
            Dim divAreaBack As HtmlGenericControl
            Dim divSubAreaDetail As HtmlGenericControl
            Dim divRoNum As HtmlGenericControl
            Dim divRegNum As HtmlGenericControl
            Dim divBillNo As HtmlGenericControl
            Dim divModelName As HtmlGenericControl
            Dim divStallName As HtmlGenericControl
            Dim spnStaffName As HtmlGenericControl
            Dim divStaffName As HtmlGenericControl
            Dim divBoxmark As HtmlGenericControl
            Dim divDispTime As HtmlGenericControl

            For i As Integer = 0 To resultDataTable.Rows.Count - 1
                '各項目の初期化
                li = New HtmlGenericControl("li")
                divAreaBack = New HtmlGenericControl("div")
                divSubAreaDetail = New HtmlGenericControl("div")
                divRoNum = New HtmlGenericControl("div")
                divRegNum = New HtmlGenericControl("div")
                divBillNo = New HtmlGenericControl("div")
                divModelName = New HtmlGenericControl("div")
                divStallName = New HtmlGenericControl("div")
                spnStaffName = New HtmlGenericControl("span")
                divStaffName = New HtmlGenericControl("div")
                divDispTime = New HtmlGenericControl("div")

                '明細大枠以外のクラスをセットする
                divSubAreaDetail.Attributes.Add("class", "divSubAreaDetail")
                divRoNum.Attributes.Add("class", "RoNum")
                divRegNum.Attributes.Add("class", "RegNum04")
                divBillNo.Attributes.Add("class", "BillNo04")
                divModelName.Attributes.Add("class", "ModelName04")
                divStallName.Attributes.Add("class", "StallName")
                divDispTime.Attributes.Add("class", "DispTime")

                Dim row As SC3190402DataSet.AREA04ResRow = resultDataTable.Rows(i)
                '赤明細のチェックを行い、明細大枠のクラスをセットする
                If row.SORT_KEY = SC3190402BusinessLogic.Enum_BackColor.Red Then
                    divAreaBack.Attributes("class") = "divAreaBack BackColorRed"
                Else
                    divAreaBack.Attributes("class") = "divAreaBack"
                End If
                'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
                Dim targetDate As DateTime = row.SCHE_START_DATETIME
                Dim targetDateString As String = targetDate.Year & "," & targetDate.Month & "," & targetDate.Day & "," & targetDate.Hour & "," & targetDate.Minute
                divSubAreaDetail.Attributes.Add("schestartdatetime", targetDateString)
                targetDate = row.SCHE_DELI_DATETIME
                targetDateString = targetDate.Year & "," & targetDate.Month & "," & targetDate.Day & "," & targetDate.Hour & "," & targetDate.Minute
                divSubAreaDetail.Attributes.Add("schedelidatetime", targetDateString)
                divSubAreaDetail.Attributes.Add("ro_num", row.RO_NUM)
                divSubAreaDetail.Attributes.Add("ro_seq", row.RO_SEQ)
                'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05

                '明細の各項目を編集する
                divRoNum.InnerText = SC3190402BusinessLogic.MakeRoNumAndSeq(row.RO_NUM.ToString.TrimEnd, _
                                                     row.RO_SEQ.ToString.TrimEnd)
                divRegNum.InnerText = row.REG_NUM.ToString.TrimEnd
                divBillNo.InnerText = row.BILL_NO.ToString.TrimEnd
                divModelName.InnerText = SC3190402BusinessLogic.MakeModelAndGradeName(row.MODEL_NAME.ToString.TrimEnd, _
                                                               row.GRADE_NAME.ToString.TrimEnd)
                divStallName.InnerText = row.STALL_NAME_SHORT.ToString

                ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                divDispTime.InnerText = ChangeFormatDataTime(row.SCHE_START_DATETIME, branchWorkTime)
                ' $03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                '出庫担当者が存在したときだけ表示
                If row.PARTS_STAFF_NAME.ToString.TrimEnd.Length > 0 Then
                    divStaffName.Attributes.Add("class", "StaffName")
                    spnStaffName.InnerText = row.PARTS_STAFF_NAME.ToString
                    divStaffName.Controls.Add(spnStaffName)
                    divSubAreaDetail.Controls.Add(divStaffName)
                End If
                'カゴNoが存在したときだけ表示
                If row.CAGE_NO.ToString.TrimEnd.Length > 0 Then
                    divBoxmark = New HtmlGenericControl("div")
                    divBoxmark.Attributes("class") = "Boxmark"
                    divBoxmark.InnerText = row.CAGE_NO.ToString
                    divSubAreaDetail.Controls.Add(divBoxmark)
                End If
                divSubAreaDetail.Controls.Add(divRoNum)
                divSubAreaDetail.Controls.Add(divRegNum)
                divSubAreaDetail.Controls.Add(divBillNo)
                divSubAreaDetail.Controls.Add(divModelName)
                divSubAreaDetail.Controls.Add(divStallName)

                divAreaBack.Controls.Add(divSubAreaDetail)
                divAreaBack.Controls.Add(divDispTime)
                li.Controls.Add(divAreaBack)
                ul.Controls.Add(li)
            Next

            '2014/07/15 改ページ機能追加
            'ページ数の算出(取得データ件数/1ページあたりの表示最大件数)
            PageCnt = Integer.Parse(Math.Ceiling(resultDataTable.Rows.Count / Me.SettingChipDispMaxCount))

        End If

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'End Using
        'End Using
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2} PAGE:CNT={3}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd _
        '    , PageCnt))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '2014/07/15 改ページ機能追加
        'ページ数を返却する
        Return PageCnt

    End Function
#End Region

#Region "日付フォーマット変換"

    ''' <summary>
    ''' 日付をフォーマット変換する
    ''' </summary>
    ''' <param name="targetDate">変換対象日時</param>
    ''' <param name="branchWorkTime">店舗の営業時間</param>
    ''' <returns>フォーマット変換後の文字列</returns>
    ''' <remarks></remarks>
    Private Function ChangeFormatDataTime(ByVal targetDate As DateTime, _
                                          ByVal branchWorkTime As SC3190402DataSet.BranchWorkTimeDataTable) As String

        Dim afterConversionDate As String = String.Empty

        If branchWorkTime.Item(0).SVC_JOB_START_TIME <= targetDate AndAlso _
           targetDate <= branchWorkTime.Item(0).SVC_JOB_END_TIME Then
            ' 営業時間内に収まっている場合、時刻表記を行う。
            afterConversionDate = DateTimeFunc.FormatDate(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_14, targetDate)

        Else
            ' 営業時間外の場合、月日表記を行う。
            afterConversionDate = DateTimeFunc.FormatDate(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_11, targetDate)
        End If

        Return afterConversionDate

    End Function

#End Region



    ''' <summary>
    ''' 初回起動時のみ行う処理
    ''' </summary>
    ''' <remarks>作業計画待ちエリアの文言取得及び新着有無のフラグセットを行う</remarks>
    Protected Sub InitializeControl()
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Using biz As New SC3190402BusinessLogic
            '作業計画待ちエリアの文言取得
            Me.lblArea02Title.Text = WebWordUtility.GetWord(Me.DisplayID, SC3190402BusinessLogic.ConsArea02DisplayNo)
        End Using

        '新着有無を無しに設定
        Me.IsWhatsNew = SC3190402BusinessLogic.Enum_WhatsNew.No

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub

    ''' <summary>
    ''' コントロールセット
    ''' </summary>
    ''' <remarks>現在日時の取得及びシステム設定値の取得を行う</remarks>
    Protected Sub RefreshControl()
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '現在時刻を取得（この値を各エリア共通で使っていく）
        Using biz As New SC3190402BusinessLogic
            Me.NowDate = biz.GetDateTimeNow
        End Using
        'タイトルに現在日時をセット
        Me.divUpdateDate.Text = DateTimeFunc.FormatDate(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_03, Me.NowDate)
        Me.lblUpdateTime.Text = DateTimeFunc.FormatDate(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_14, Me.NowDate)

        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
        'hiddenに日付フォーマットを設定
        hidDateFormat.Value = DateTimeFunc.GetDateFormat(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_03)
        hidTimeFormat.Value = DateTimeFunc.GetDateFormat(SC3190402BusinessLogic.Enum_DateTimeForm.ConvID_14)
        'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05
        '販売店システム設定の値を取得
        Using biz As New IC3190402BusinessLogic
            Dim ret As String = String.Empty
            '明細最大表示数
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyChipDispMaxCount)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingChipDispMaxCount = SC3190402BusinessLogic.ConsValueChipDispMaxCount
            Else
                Me.SettingChipDispMaxCount = Integer.Parse(ret.TrimEnd)
            End If
            '追加作業見積もり待ち遅れ判定(分)
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyDelayPeriodMinute)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingDelayPeriodMinute = SC3190402BusinessLogic.ConsValueChipDispMaxCount
            Else
                Me.SettingDelayPeriodMinute = Integer.Parse(ret.TrimEnd)
            End If
            '追加作業見積り待ちエリア改ページ間隔(秒)
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyPagingIntervalAddJob)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingPagingIntervalAddJob = SC3190402BusinessLogic.ConsValuePagingInterval
            Else
                Me.SettingPagingIntervalAddJob = Integer.Parse(ret.TrimEnd)
            End If
            '作業計画待ちエリア改ページ間隔(秒)
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyPagingIntervalJobInstruct)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingPagingIntervalJobInstruct = SC3190402BusinessLogic.ConsValuePagingInterval
            Else
                Me.SettingPagingIntervalJobInstruct = Integer.Parse(ret.TrimEnd)
            End If
            '出庫待ちエリア改ページ間隔(秒)
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyPagingIntervalShipment)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingPagingIntervalShipment = SC3190402BusinessLogic.ConsValuePagingInterval
            Else
                Me.SettingPagingIntervalShipment = Integer.Parse(ret.TrimEnd)
            End If
            '引き取り待ちエリア改ページ間隔(秒)
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyPagingIntervalPick)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingPagingIntervalPick = SC3190402BusinessLogic.ConsValuePagingInterval
            Else
                Me.SettingPagingIntervalPick = Integer.Parse(ret.TrimEnd)
            End If
            '取得データ最大件数
            ret = biz.GetDlrSystemSettingValueBySettingName(SC3190402BusinessLogic.ConsKeyChipAcquisitionMaxCount)
            If ret.TrimEnd.Length = 0 Then
                Me.SettingChipAcquisitionMaxCount = SC3190402BusinessLogic.ConsValueChipAcquisitionMaxCount
            Else
                Me.SettingChipAcquisitionMaxCount = Integer.Parse(ret.TrimEnd)
            End If
            'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $05
            '更新時間の取得
            ret = biz.GetSystemSettingValueBySettingName(SC3190402BusinessLogic.ConstKeyPsmonitorDelayUpdateInterval)
            If ret.TrimEnd.Length = 0 Then
                Me.PsmonitorDelayUpdateInterval = SC3190402BusinessLogic.ConstValuePsmonitorDelayUpdateInterval
            Else
                Me.PsmonitorDelayUpdateInterval = UInteger.Parse(ret.TrimEnd)
            End If
            'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $05
        End Using

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info("DEBUG:SettingPagingIntervalAddJob=" & Me.SettingPagingIntervalAddJob)
        'Logger.Info("DEBUG:SettingPagingIntervalJobInstruct=" & Me.SettingPagingIntervalJobInstruct)
        'Logger.Info("DEBUG:SettingPagingIntervalShipment=" & Me.SettingPagingIntervalShipment)
        'Logger.Info("DEBUG:SettingPagingIntervalPick=" & Me.SettingPagingIntervalPick)
        'Logger.Info("DEBUG:SettingChipAcquisitionMaxCount=" & Me.SettingChipAcquisitionMaxCount)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub

    ''' <summary>
    ''' リフレッシュボタン(隠し)ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub hdnBtnRefreshPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRefreshPage.Click
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        '初期値セット
        Me.RefreshControl()
        '追加作業承認待ち／完成宣言承認待ちエリア表示処理
        Me.MainRefresh()

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub

    ''' <summary>
    ''' 画面遷移用(隠し)ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub hdnBtnMovePage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMovePage.Click
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogStart))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'BO管理ボードへ遷移
        Me.RedirectNextScreen("SC3190601")

        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} {2}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , SC3190402BusinessLogic.ConsLogEnd))
        '$04 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    End Sub

End Class
