'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' SC3100303.aspx.vb
'─────────────────────────────────────
'機能: 来店管理画面 コードビハインド
'補足: 
'作成: 2013/03/06 TMEJ  張  初版作成
'更新: 2012/04/25 TMEJ  張  ITxxxx_TSL自主研緊急対応（サービス）
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新：2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Data
Imports System.Web.UI
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports Toyota.eCRB.iCROP.BizLogic.SC3100303
Imports Toyota.eCRB.iCROP.DataAccess.SC3100303
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports System.Reflection
'2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
'2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
Imports Toyota.eCRB.SMBLinkage.Customer.DataAccess.IC3810203DataSet
'2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

Partial Class Pages_SC3100303
    Inherits BasePage
    Implements ICallbackEventHandler

#Region "定数"
    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"
    ' 画面ID
    Private Const SA_MAINMENUID As String = "SC3140103"                     'SAメイン画面
    Private Const SM_MAINMENUID As String = "SC3220101"                     'SAステータスマネージメント画面
    Private Const APPLICATIONID_NOASSIGNMENTLIST As String = "SC3100401"    '未振当て一覧画面
    Private Const APPLICATIONID_GENERALMANAGER As String = "SC3220201"      '全体管理画面
    Private Const APPLICATIONID_ORDERLIST As String = "SC3160101"           'R/O一覧画面
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    'Private Const APPLICATIONID_ADD_LIST As String = "SC3170101"            '追加作業一覧画面
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
    Private Const APPLICATIONID_VSTMANAGER As String = "SC3100303"          '来店管理画面
    Private Const APPLICATIONID_CUSTOMERNEW As String = "SC3080207"         '新規顧客登録画面

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CONTROL_PAGE As String = "SC3240101"
    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' SessionKey(DearlerCode):ログインユーザーのDMS販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DEARLER_CODE As String = "Session.Param1"
    ''' <summary>
    ''' SessionKey(BranchCode):ログインユーザーのDMS店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BRANCH_CODE As String = "Session.Param2"
    ''' <summary>
    ''' SessionKey(LoginUserID):ログインユーザーのアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_LOGIN_USER_ID As String = "Session.Param3"
    ''' <summary>
    ''' SessionKey(SAChipID):来店管理番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SA_CHIP_ID As String = "Session.Param4"
    ''' <summary>
    ''' SessionKey(BASREZID):DMS予約ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BASREZID As String = "Session.Param5"
    ''' <summary>
    ''' SessionKey(R_O):RO番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_R_O As String = "Session.Param6"
    ''' <summary>
    ''' SessionKey(SEQ_NO):RO作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SEQ_NO As String = "Session.Param7"
    ''' <summary>
    ''' SessionKey(VIN_NO):車両登録No.のVIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIN_NO As String = "Session.Param8"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIEW_MODE As String = "Session.Param9"
    '2018/02/21 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    ''' <summary>
    ''' SessionKey(CustomerID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CUSTOMER_ID As String = "Session.Param10"
    ''' <summary>
    ''' SessionKey(ContactParson)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CONTACT_PARSON As String = "Session.Param11"
    ''' <summary>
    ''' SessionKey(ContactTEL)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_CONTACT_TELNO As String = "Session.Param12"
    ''' <summary>
    ''' SessionValue(DISP_NUM)：「1：R/O作成」固定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISP_NUM_ROCREATE As String = "1"
    '2018/02/21 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    ''' <summary>
    ''' SessionValue(ViewMode)：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_EDIT As String = "0"

    '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
    ''' <summary>
    ''' SessionValue(ViewMode)：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_READ As String = "1"
    '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

    ''' <summary>
    ''' セッション名("DealerCode")
    ''' </summary>
    Private Const SessionDealerCode As String = "DealerCode"

    ''' <summary>
    ''' セッション名("BranchCode")
    ''' </summary>
    Private Const SessionBranchCode As String = "BranchCode"

    ''' <summary>
    ''' セッション名("LoginUserID")
    ''' </summary>
    Private Const SessionLoginUserID As String = "LoginUserID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"

    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"

    ''' <summary>
    ''' セッション名("R_O")
    ''' </summary>
    Private Const SessionRO As String = "R_O"

    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSEQNO As String = "SEQ_NO"

    ''' <summary>
    ''' セッション名("VIN_NO")
    ''' </summary>
    Private Const SessionVINNO As String = "VIN_NO"

    ''' <summary>
    ''' セッション名("ViewMode")
    ''' </summary>
    Private Const SessionViewMode As String = "ViewMode"

    ''' <summary>
    ''' 商品訴求コンテンツ画面("SC3250101")
    ''' </summary>
    Private Const APPLICATIONID_PRODUCTSAPPEALCONTENT As String = "SC3250101"

    ''' <summary>
    ''' 編集モードフラグ("1"；リードオンリー) 
    ''' </summary>
    Private Const ReadMode As String = "1"

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' SessionValue(画面番号)：キャンペーン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SESSIONVALUE_CAMPAIGN As String = "3"
    ''' <summary>
    ''' SessionValue(画面番号)：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_CAMPAIGN As String = "15"
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
    ''' <summary>
    ''' SessionKey(DISP_NUM)：画面番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"
    ''' <summary>
    ''' 現地にシステム連携用画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_LOCAL_TACT As String = "SC3010501"
    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"
    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_RO_LIST As String = "14"

    ''' <summary>
    ''' 日付のフォーマット:yyyy/MM/dd
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMATDATE_YYYYMMDD As String = "yyyy/MM/dd"

    ''' <summary>
    ''' 日付のフォーマット:yyyy/MM/dd HH:mm:ss
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMATDATE_YYYYMMDDHHMMSS As String = "yyyy/MM/dd HH:mm:ss"
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

#End Region

#Region "メンバー変数"
    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext
    ''' <summary>
    ''' SC3500101BusinessLogic
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3100303BusinessLogic

    ''' <summary>
    ''' ストール開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_strStallStartTime As String
    ''' <summary>
    ''' ストール終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_strStallEndTime As String
    ''' <summary>
    ''' ストール開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_nStallStartTime As Double
    ''' <summary>
    ''' ストール終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_nStallEndTime As Double
    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callBackResult As String
#End Region

#Region "列挙体"
    ''' <summary>
    ''' 列挙体 コールバック結果コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        'DBTimeOut = 9

        ''' <summary>
        ''' 入力項目値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        CheckError = 1

        ''' <summary>
        ''' 失敗
        ''' </summary>
        ''' <remarks></remarks>
        Failure = 9999

    End Enum

    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        DBTimeOut = 901

        ''' <summary>
        ''' 排他エラー
        ''' </summary>
        OtherChanged = 902

    End Enum
    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    ''' <summary>
    ''' 文言ID管理(Client端必要な文言)
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
        ''' <summary>様（男性向け）</summary>
        id001 = 7
        ''' <summary>様（女性向け）</summary>
        id002 = 8
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
        ''' <summary>来店実績:{0}台</summary>
        id003 = 18
        ''' <summary>データベースへのアクセスにてタイムアウトが発生しました。再度実行して下さい。</summary>
        id004 = 901
        'id003 = 901
        ''' <summary>そのチップは、既に他のユーザーによって変更が加えられています。画面を再表示してから再度処理を行ってください。</summary>
        id005 = 902
        'id004 = 902
        ''' <summary>予期せぬエラーが発生しました。画面を再表示してから再度処理を行ってください。</summary>
        id006 = 903
        'id005 = 903
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END

        '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        ''' <summary>R/Oを作成しますか。</summary>
        id008 = 904
        ''' <summary>R/Oを作成できません。TOPSERVに顧客・車両を登録し、顧客・車両を検索して、R/Oを作成して下さい。</summary>
        id009 = 905
        '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    End Enum
#End Region

#Region "初期表示"
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load.S")
        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current
        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3100303",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3100303.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3100303.packedArgument", _
                                                                      "gCallbackSC3100303.endCallback", "", False)
                          ), True)

        If Not Me.IsPostBack AndAlso Not Me.IsCallback Then
            hidShowDate.Value = ""
            '店舗稼動時間情報の取得
            GetBranchOperationgHours()
            SetCalendarDay(0)
        End If

        'フッター初期化
        Me.InitFooterEvent()
        ConvertStallTimeToDouble()

        If Not Me.IsCallback AndAlso Not Me.Page.IsPostBackEventControlRegistered Then
            '来店管理ボタンを押すと、今日の日付で表示される
            SetCalendarToDay()

            'サーバ時間を取得し、設定する.
            SetServerCurrentTime()

            'hiddenコントロールにclient端用の文言を設定する
            SendWordToClient()

            '時間、ストール名、テクニク名がrepeaterコントロールとバインドする
            Me.DataBindWithControl()
        End If
        Logger.Info("Page_Load.E")
    End Sub
#End Region

#Region "文言処理"
    ''' <summary>
    ''' hiddenコントロールにclient端用の文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendWordToClient()
        Dim sbWord As StringBuilder = New StringBuilder
        sbWord.Append("{")
        '全てwordをループする
        For Each i As WordID In [Enum].GetValues(GetType(WordID))
            With sbWord
                .Append("""")
                .Append(Convert.ToInt32(i).ToString(CultureInfo.InvariantCulture))
                .Append(""":""")
                .Append(WebWordUtility.GetWord(APPLICATIONID_VSTMANAGER, Convert.ToInt32(i).ToString(CultureInfo.InvariantCulture)))
                .Append(""",")
            End With
        Next i

        '最後の","を削除する
        sbWord.Remove(sbWord.Length - 1, 1)
        sbWord.Append("}")
        Me.hidMsgData.Value = sbWord.ToString()
    End Sub
#End Region

#Region "日付処理"
    ''' <summary>
    ''' 現在のサーバ時間をHiddenFieldにセットする.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetServerCurrentTime()

        Logger.Info("SetServerCurrentTime.S")

        'サーバ時間を文字列として取得して、HiddenFieldに格納.（yyyy/MM/dd HH:mm:ss形式）
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        'Me.hidServerTime.Value = DateTimeFunc.FormatDate(1, DateTimeFunc.Now(objStaffContext.DlrCD))
        Me.hidServerTime.Value = DateTimeFunc.Now(objStaffContext.DlrCD).ToString(FORMATDATE_YYYYMMDDHHMMSS, CultureInfo.CurrentCulture)
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

        Logger.Info("SetServerCurrentTime.E SetTime:" + Me.hidServerTime.Value)
    End Sub

    ''' <summary>
    ''' 日付を画面に設定する
    ''' </summary>
    ''' <param name="nOffsetDays"></param>
    ''' <remarks></remarks>
    Protected Function SetCalendarDay(nOffsetDays As Integer) As Date
        Logger.Info("SetCalendarDay.S")

        Dim dtDay As Date
        If String.IsNullOrEmpty(hidShowDate.Value) Then
            dtDay = Now
        Else
            dtDay = hidShowDate.Value
            dtDay = DateAdd("d", nOffsetDays, dtDay)
        End If

        'yyyy/MM/ddの形式でhiddenコントロールに保存する
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        'hidShowDate.Value = DateTimeFunc.FormatDate(21, dtDay)
        hidShowDate.Value = dtDay.ToString(FORMATDATE_YYYYMMDD, CultureInfo.CurrentCulture)
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
        Logger.Info("SetCalendarDay.E")

        Return dtDay
    End Function

    ''' <summary>
    ''' 今日の日付を画面に設定する
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetCalendarToDay()
        Logger.Info("SetCalendarToDay.S")
        'yyyy/MM/ddの形式でhiddenコントロールに保存する
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        'hidShowDate.Value = DateTimeFunc.FormatDate(21, DateTimeFunc.Now(objStaffContext.DlrCD))
        hidShowDate.Value = DateTimeFunc.Now(objStaffContext.DlrCD).ToString(FORMATDATE_YYYYMMDD, CultureInfo.CurrentCulture)
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
        Logger.Info("SetCalendarToDay.E")
    End Sub
#End Region

#Region "営業時間処理"
    ''' <summary>
    ''' 営業時間を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetBranchOperationgHours()

        Dim dt = businessLogic.GetBranchOperatingHours()
        If dt.Rows.Count = 1 Then
            'ストール開始時間
            m_strStallStartTime = dt.Rows(0)(0).ToString().Trim()
            'ストール終了時間
            m_strStallEndTime = dt.Rows(0)(1).ToString().Trim()

            '遅刻時間(秒)
            hidDelayTime.Value = dt.Rows(0)(2).ToString().Trim()
            'リフレッシュ時間(秒)
            hidRefreshTime.Value = dt.Rows(0)(3).ToString().Trim()
        Else
            '取得してない場合、ディフォルト値を設定する
            m_strStallStartTime = "09:00"
            m_strStallEndTime = "20:00"

            hidDelayTime.Value = "900"
            hidRefreshTime.Value = "180"
        End If

        '営業終了時間が営業開始時間より、早い場合、"00:00"に設定する
        If m_strStallStartTime > m_strStallEndTime Then
            m_strStallEndTime = "00:00"
        End If

        'hiddenコントロールに開始時間と終了時間を設定する
        hidStallStartTime.Value = m_strStallStartTime
        hidStallEndTime.Value = m_strStallEndTime

        hidOpeCD.Value = objStaffContext.OpeCD
    End Sub

    ''' <summary>
    ''' ストール開始時間と終了時間がstring→doubleに変える
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConvertStallTimeToDouble()
        'hiddenコントロールに開始時間と終了時間を設定する
        If hidStallStartTime.Value <> "" Then
            m_strStallStartTime = hidStallStartTime.Value
        End If
        If hidStallEndTime.Value <> "" Then
            m_strStallEndTime = hidStallEndTime.Value
        End If

        m_nStallStartTime = Left(m_strStallStartTime, 2)
        m_nStallEndTime = Left(m_strStallEndTime, 2)
        If Right(m_strStallEndTime, 2) <> "00" Then
            m_nStallEndTime = m_nStallEndTime + 1
        End If
    End Sub
#End Region

#Region "Repeaterコントロールのbind"
    ''' <summary>
    ''' Repeaterコントロールのbind
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DataBindWithControl()

        Logger.Info("DataBindWithControl.S")
        Using dt As New SC3100303DataSet.SC3100303TimeRepeaterDataTable
            Dim dr As DataRow
            Dim culture As CultureInfo = New CultureInfo("en")

            '列数を計算する
            Dim nColNum = m_nStallEndTime - m_nStallStartTime
            If nColNum <= 0 Then
                nColNum = nColNum + 24
            End If

            Dim dtDate As DateTime = New DateTime(1, 1, 1, m_nStallStartTime, 0, 0)
            '全部コラムをループ
            For nLoop As Integer = 0 To nColNum - 1
                dr = dt.NewRow
                Dim sbData As StringBuilder = New StringBuilder
                'No列の設定：01、02…10、11…
                If nLoop + 1 < 10 Then
                    sbData.Append("0")
                End If
                sbData.Append((nLoop + 1).ToString(culture))

                dr(0) = sbData.ToString()
                '時間の設定：09:00、10:00…23:00、00:00、01:00…
                dr(1) = DateTimeFunc.FormatDate(14, dtDate.AddHours(nLoop))
                '1行datarowをdatatableに追加する
                dt.Rows.Add(dr)
            Next

            '時間をrepeaterコントロールとbindする
            Me.stallTimeRepeater.DataSource = dt
            Me.stallTimeRepeater.DataBind()

        End Using
        Logger.Info("DataBindWithControl.E")
    End Sub

#End Region

#Region "チップ情報の取得処理"
    ''' <summary>
    ''' チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="dtDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChipDataFromServer(ByVal dtDate As Date) As String

        Logger.Info("GetChipDataFromServer.S")
        'チップ情報の最新を取得し、作業対象チップを設定する
        Dim dtStallStartTime As Date = DateAdd("h", m_nStallStartTime, dtDate)

        Dim dtStallEndTime As Date
        If m_nStallEndTime - m_nStallStartTime <= 0 Then
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime + 24, dtStallStartTime)
        Else
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime, dtStallStartTime)
        End If

        Dim dtChipList = businessLogic.GetVisitChips(dtStallStartTime, dtStallEndTime)
        Dim chipDataJson As String
        chipDataJson = businessLogic.DataTableToJson(dtChipList)
        Logger.Debug("GetChipDataFromServer ChipData:" + chipDataJson)
        Logger.Info("GetChipDataFromServer.E")
        Return chipDataJson
    End Function

    ''' <summary>
    ''' チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="dtDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSwitchChipId(ByVal dtDate As Date) As String

        Logger.Info("GetSwitchChipId.S")
        'チップ情報の最新を取得し、作業対象チップを設定する
        Dim dtStallStartTime As Date = DateAdd("h", m_nStallStartTime, dtDate)

        Dim dtStallEndTime As Date
        If m_nStallEndTime - m_nStallStartTime <= 0 Then
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime + 24, dtStallStartTime)
        Else
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime, dtStallStartTime)
        End If

        Dim dtChipList = businessLogic.GetSwitchChipId(dtStallStartTime, dtStallEndTime)
        Dim chipDataJson As String
        chipDataJson = businessLogic.DataTableToJson(dtChipList)
        Logger.Debug("GetSwitchChipId ChipId:" + chipDataJson)
        Logger.Info("GetSwitchChipId.E")
        Return chipDataJson
    End Function

    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
    ''' <summary>
    ''' 来店実績台数を取得
    ''' </summary>
    ''' <param name="dtDate">当ページの日付</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetVstCarCnt(ByVal dtDate As Date) As Long

        Logger.Info("GetVstCarCnt.S")

        '将来の場合、来店実績は0
        If Date.Compare(Now.Date, dtDate) < 0 Then
            Return 0
        End If

        'チップ情報の最新を取得し、作業対象チップを設定する
        Dim dtStallStartTime As Date = DateAdd("h", m_nStallStartTime, dtDate)

        Dim dtStallEndTime As Date
        If m_nStallEndTime - m_nStallStartTime <= 0 Then
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime + 24, dtStallStartTime)
        Else
            dtStallEndTime = DateAdd("h", m_nStallEndTime - m_nStallStartTime, dtStallStartTime)
        End If

        Dim nVstCarCnt As Long = businessLogic.GetVstCarCnt(dtStallStartTime, dtStallEndTime)

        Logger.Debug("GetVstCarCnt Visit Car Count:" + nVstCarCnt.ToString(CultureInfo.CurrentCulture))
        Logger.Info("GetVstCarCnt.E")

        Return nVstCarCnt
    End Function
    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
#End Region

#Region "フッター制御"
    ''' <summary>
    ''' メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAIN_MENU As Integer = 100
    ''' <summary>
    ''' 顧客情報
    ''' </summary>
    Private Const CUSTOMER_INFORMATION As Integer = 200
    ''' <summary>
    ''' R/O作成
    ''' </summary>
    Private Const SUBMENU_RO_MAKE As Integer = 600
    ''' <summary>
    ''' スケジューラ
    ''' </summary>
    Private Const SUBMENU_SCHEDULER As Integer = 400
    ''' <summary>
    ''' 電話帳
    ''' </summary>
    Private Const SUBMENU_TELEPHONE_BOOK As Integer = 500
    ''' <summary>
    ''' 追加作業一覧
    ''' </summary>
    Private Const SUBMENU_ADD_LIST As Integer = 1100

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_SMB As Integer = 800
    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, _
                        ByRef category As FooterMenuCategory) As Integer()
        ' TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        ''（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        'Return New Integer() {}

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        category = FooterMenuCategory.ReserveManagement

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}
        ' TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

    End Function

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()
        'メインメニュー
        Dim mainMenuButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        mainMenuButton.OnClientClick = "return FooterButtonControl();"

        'スケジューラ
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        'Dim schedulerButton As CommonMasterFooterButton = _
        'CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SCHEDULER)
        'schedulerButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        ''電話帳
        'Dim telephoneBookButton As CommonMasterFooterButton = _
        'CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TELEPHONE_BOOK)
        '連絡先
        Dim telephoneBookButton As CommonMasterFooterButton = _
        CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Or objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA Then
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            ''R/O作成
            'Dim roMakeButton As CommonMasterFooterButton = _
            'CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_RO_MAKE)
            'AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
            'roMakeButton.OnClientClick = "return FooterButtonControl();"
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'SMBボタンの設定
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            'Dim smbButton As CommonMasterFooterButton = _
            'CType(Me.Master, CommonMasterPage).GetFooterButton(FOOTER_SMB)
            Dim smbButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
            AddHandler smbButton.Click, AddressOf SMBButton_Click
            smbButton.OnClientClick = "return FooterButtonControl();"

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            ''追加作業一覧
            'Dim addListButton As CommonMasterFooterButton = _
            'CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_ADD_LIST)
            'AddHandler addListButton.Click, AddressOf addListButton_Click
            'addListButton.OnClientClick = "return FooterButtonControl();"
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            '商品訴求
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            AddHandler goodsSolicitationContentsButton.Click, AddressOf goodsSolicitationContentsButtonButton_Click
            goodsSolicitationContentsButton.OnClientClick = "return FooterButtonControl();"

            'キャンペーン
            Dim campaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)
            AddHandler campaignButton.Click, AddressOf campaignButton_Click
            campaignButton.OnClientClick = "return FooterButtonControl();"
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

            '顧客詳細ボタンの設定
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            'Dim customerButton As CommonMasterFooterButton = _
            '    CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_INFORMATION)
            'customerButton.OnClientClick = _
            '    String.Format(CultureInfo.CurrentCulture, _
            '                  FOOTER_REPLACE_EVENT, _
            '                  CUSTOMER_INFORMATION.ToString(CultureInfo.CurrentCulture))
            Dim customerButton As CommonMasterFooterButton = _
                CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            customerButton.OnClientClick = "return false ;"
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
        End If

        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Or objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA Or objStaffContext.OpeCD = iCROP.BizLogic.Operation.SVR Then

            'R/O作成
            Dim roMakeButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
            AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
            roMakeButton.OnClientClick = "return FooterButtonControl();"

            '来店管理
            Dim reserveManagementButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
            AddHandler reserveManagementButton.Click, AddressOf reserveManagementButton_Click
            reserveManagementButton.OnClientClick = "return FooterButtonControl();"

        End If

        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SVR Then

            '全体管理
            Dim wholeManagementButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.WholeManagement)
            AddHandler wholeManagementButton.Click, AddressOf wholeManagementButton_Click
            wholeManagementButton.OnClientClick = "return FooterButtonControl();"

        End If

        ''SM以外の場合全体管理ボタン非表示にする
        'If objStaffContext.OpeCD <> iCROP.BizLogic.Operation.SM Then
        '    Me.FooterButton100.Style.Value = "display:none"
        'End If

        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

    End Sub

    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub mainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Logger.Info("MainMenuButton_Click Start")
        Dim strMainMenuId As String

        '権限により、別々の画面へ遷移する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
            'strMainMenuId = SM_MAINMENUID
            strMainMenuId = APPLICATIONID_GENERALMANAGER
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END
        ElseIf objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA Then
            strMainMenuId = SA_MAINMENUID
        Else
            strMainMenuId = APPLICATIONID_NOASSIGNMENTLIST
        End If

        ' メイン画面に遷移する
        Me.RedirectNextScreen(strMainMenuId)

        Logger.Info("MainMenuButton_Click End")

    End Sub

    ''' <summary>
    ''' フッター「顧客詳細ボタン」クリック時の処理。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' 新規顧客登録画面に遷移します。
    ''' </remarks>
    Private Sub CustomerButton_Click(sender As Object, e As System.EventArgs) Handles CustomerButton.Click

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        'SA権限の場合、
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA Then
            ' 新規顧客登録画面に遷移
            Me.RedirectNextScreen(APPLICATIONID_CUSTOMERNEW)
        End If

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    ''' <summary>
    ''' フッター「R/Oボタン」クリック時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' 　R/O一覧画面に遷移します。
    ''' </remarks>
    Private Sub roMakeButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        'R/O一覧画面に遷移
        Me.RedirectOrderList()

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    ''' <summary>
    ''' R/O一覧画面に遷移
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectOrderList()
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        Dim logOrderList As StringBuilder = New StringBuilder(String.Empty)
        With logOrderList
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ORDERLIST))
        End With
        Logger.Info(logOrderList.ToString())

        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        If StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SM _
            OrElse StaffContext.Current.OpeCD = iCROP.BizLogic.Operation.SVR Then

            'ログインスタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            Using biz As New SC3100303BusinessLogic

                '基幹コードへ変換処理
                Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

                '基幹販売店コードチェック
                If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordID.id006)

                    '処理終了
                    Exit Sub

                End If

                '基幹店舗コードチェック
                If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordID.id006)

                    '処理終了
                    Exit Sub

                End If

                '基幹アカウントチェック
                If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                    '値無し

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordID.id006)

                    '処理終了
                    Exit Sub

                End If

                'セション値の設定
                'DMS用販売店コード
                Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

                'DMS用店舗コード
                Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

                'ログインユーザアカウント
                Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

                'RO番号
                Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

                '車両登録NOのVIN
                Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

                'RO作成フラグ
                Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

                '画面番号(RO一覧)
                Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_RO_LIST)

            End Using
            '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

            '決定した遷移先に遷移
            Me.RedirectNextScreen(PGMID_LOCAL_TACT)

        End If

        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
        ' R/O一覧画面に遷移
        'Me.RedirectNextScreen(PGMID_LOCAL_TACT)
        '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub


    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' フッター「追加作業ボタン」クリック時の処理
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベントデータ</param>
    ' ''' <remarks></remarks>
    'Private Sub addListButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

    '    '開始ログ出力
    '    Dim logStart As New StringBuilder
    '    With logStart
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" Start")
    '    End With
    '    Logger.Info(logStart.ToString)

    '    '追加作業一覧画面遷移
    '    Me.RedirectAddRepairList()

    '    '終了ログ出力
    '    Dim logEnd As New StringBuilder
    '    With logEnd
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" End")
    '    End With
    '    Logger.Info(logEnd.ToString)

    'End Sub

    ' ''' <summary>
    ' ''' 追加作業一覧画面遷移
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub RedirectAddRepairList()
    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                             , "{0}.{1} {2}" _
    '                             , Me.GetType.ToString _
    '                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                             , LOG_START))

    '    Dim logAddList As StringBuilder = New StringBuilder(String.Empty)

    '    With logAddList
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ADD_LIST))
    '    End With
    '    Logger.Info(logAddList.ToString())

    '    ' 追加作業一覧画面に遷移
    '    Me.RedirectNextScreen(APPLICATIONID_ADD_LIST)

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} {2}" _
    '               , Me.GetType.ToString _
    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '               , LOG_END))
    'End Sub
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 全体管理に遷移する為のダミーボタンクリック
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベント引数</param>
    ' ''' <remarks></remarks>
    'Protected Sub GeneralMngButton_Click(sender As Object, e As System.EventArgs) Handles GeneralMngButton.Click

    '    Dim logOrderList As StringBuilder = New StringBuilder(String.Empty)
    '    With logOrderList
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", APPLICATIONID_ORDERLIST))
    '    End With
    '    Logger.Info(logOrderList.ToString())

    '    ' 全体管理に遷移
    '    Me.RedirectNextScreen(APPLICATIONID_GENERALMANAGER)

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} {2}" _
    '               , Me.GetType.ToString _
    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '               , LOG_END))

    'End Sub
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START))

        '工程管理画面に遷移する
        Me.RedirectNextScreen(PROCESS_CONTROL_PAGE)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))
    End Sub
    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 商品訴求ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub goodsSolicitationContentsButtonButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '次画面遷移パラメータ設定

        '販売店コード
        Me.SetValue(ScreenPos.Next, SessionDealerCode, Space(1))
        '店舗コード
        Me.SetValue(ScreenPos.Next, SessionBranchCode, Space(1))
        'アカウント
        Me.SetValue(ScreenPos.Next, SessionLoginUserID, Space(1))
        '来店者実績連番
        Me.SetValue(ScreenPos.Next, SessionSAChipID, String.Empty)
        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionBASREZID, String.Empty)
        'RO
        Me.SetValue(ScreenPos.Next, SessionRO, String.Empty)
        'RO_JOB_SEQ           
        Me.SetValue(ScreenPos.Next, SessionSEQNO, String.Empty)
        'VIN
        Me.SetValue(ScreenPos.Next, SessionVINNO, String.Empty)
        'ViewMode
        Me.SetValue(ScreenPos.Next, SessionViewMode, ReadMode)


        '商品訴求コンテンツ画面遷移
        Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub campaignButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインスタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New SC3100303BusinessLogic

            '基幹コードへ変換処理
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = biz.ChangeDmsCode(staffInfo)

            '基幹販売店コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE1) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE1=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id006)

                '処理終了
                Exit Sub

            End If

            '基幹店舗コードチェック
            If String.Empty.Equals(rowDmsCodeMap.CODE2) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.CODE2=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id006)

                '処理終了
                Exit Sub

            End If

            '基幹アカウントチェック
            If String.Empty.Equals(rowDmsCodeMap.ACCOUNT) Then
                '値無し

                'エラーログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} Err:rowDmsCodeMap.ACCOUNT=NOTHING" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラーメッセージ表示
                Me.ShowMessageBox(WordID.id006)

                '処理終了
                Exit Sub

            End If

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
            'Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_READ)
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_CAMPAIGN)

        End Using

        '決定した遷移先に遷移
        Me.RedirectNextScreen(PGMID_LOCAL_TACT)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' 来店管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub reserveManagementButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '決定した遷移先に遷移
        Me.RedirectNextScreen(APPLICATIONID_VSTMANAGER)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 全体管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub wholeManagementButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '決定した遷移先に遷移
        Me.RedirectNextScreen(APPLICATIONID_GENERALMANAGER)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    ''' <summary>
    ''' R/O作成確認ダイアログ「OK」ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ROCreateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ROCreateButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try
            'ログインスタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current
            '基幹コードへ変換用
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow
            '来店情報登録及びRO情報登録・RO連携処理結果用
            Dim resultRegist As IC3810203ReservationInfoRow
            '来店情報取得結果用
            Dim resultVisitInfo As SC3100303DataSet.SC3100303ContactInfoDataTable
            'セッション格納情報取得結果用
            Dim resultSessionInfo As SC3100303DataSet.SC3100303SessionInfoDataTable
            '基幹作業内容ID取得結果用
            Dim resultDmsJobDtlId As SC3100303DataSet.SC3100303DmsJobDtlIdDataTable

            Using biz As New SC3100303BusinessLogic
                Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
                '選択チップのサービス入庫ID取得
                Dim svcInId As Decimal = Decimal.Parse(Me.hidSelectedRezId.Value)

                '基幹顧客コードへの変換処理
                rowDmsCodeMap = biz.ChangeDmsCode(staffInfo)
                '基幹コードチェック
                If (String.IsNullOrWhiteSpace(rowDmsCodeMap.CODE1) OrElse _
                    String.IsNullOrWhiteSpace(rowDmsCodeMap.CODE2) OrElse _
                    String.IsNullOrWhiteSpace(rowDmsCodeMap.ACCOUNT)) Then

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} Err:DmsCodeMap Nothing" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージ表示
                    Me.ShowMessageBox(WordID.id006)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    '処理終了
                    Exit Sub

                End If

                'セッション格納用情報取得
                resultSessionInfo = biz.GetSessionInfo(svcInId)

                'セッション格納用情報取得チェック
                If ((resultSessionInfo.Count = 0) OrElse _
                    String.IsNullOrWhiteSpace(resultSessionInfo(0).VCL_VIN) OrElse _
                    String.IsNullOrWhiteSpace(resultSessionInfo(0).DMS_CST_CD) OrElse _
                    String.IsNullOrWhiteSpace(resultSessionInfo(0).DMS_CST_CD_DISP)) Then

                    'エラーログ
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:SessionInfo Nothing" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'セッション格納用情報が取得できなかった場合はエラーメッセージを表示する
                    Me.ShowMessageBox(WordID.id006)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    Exit Sub
                End If


                '来店情報登録及びRO情報登録・RO連携処理
                Try
                    resultRegist = biz.VisitRegistProccess(svcInId, nowDate)
                Catch timeOutEx As OracleExceptionEx When timeOutEx.Number = 30006
                    '排他エラー
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:RECORD LOCK TIMEOUT" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージを表示する
                    Me.ShowMessageBox(WordID.id005)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    Exit Sub

                End Try
                If IsNothing(resultRegist) Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR OUT:ReturnCode = Nothing" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '取得できなかった場合はエラーメッセージを表示する
                    Me.ShowMessageBox(WordID.id006)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    Exit Sub
                ElseIf (resultRegist._RETURN = ReturnCode.OtherChanged) Then
                    '排他エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:DATA CHANGED BY OTHER" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージを表示する
                    Me.ShowMessageBox(WordID.id005)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    Exit Sub
                ElseIf (resultRegist._RETURN = ReturnCode.DBTimeOut) Then
                    'DBタイムアウトエラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:DBTIMEOUT" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージを表示する
                    Me.ShowMessageBox(WordID.id004)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                        "setTimeout(function () { RefreshVstChips(); }, 100);", True)
                    Exit Sub
                End If

                '来店情報取得
                resultVisitInfo = biz.GetVisitInfo(resultRegist.VISITSEQ)
                '基幹作業内容ID取得
                resultDmsJobDtlId = biz.GetDmsJobDtlId(svcInId, _
                                                       staffInfo.DlrCD, _
                                                       staffInfo.BrnCD)
            End Using

            'RO作成画面に遷移
            Me.RedirectOrderCreatePage(rowDmsCodeMap, _
                                       resultRegist, _
                                       resultSessionInfo, _
                                       resultVisitInfo, _
                                       resultDmsJobDtlId)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Catch timeOutEx As OracleExceptionEx When timeOutEx.Number = 1013
            'DBタイムアウトエラー
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBTIMEOUT" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラーメッセージを表示する
            Me.ShowMessageBox(WordID.id004)
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RefreshVstChips", _
                                                "setTimeout(function () { RefreshVstChips(); }, 100);", True)
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' RO作成画面に遷移する
    ''' </summary>
    ''' <param name="rowDmsCodeMap">基幹コード変換結果</param>
    ''' <param name="resultRegist">来店情報登録及びRO情報登録・RO連携処理結果</param>
    ''' <param name="resultSessionInfo">セッション格納用情報</param>
    ''' <param name="resultVisitInfo">来店情報</param>
    ''' <param name="resultDmsJobDtlId">基幹作業内容ID</param>
    ''' <remarks></remarks>
    Private Sub RedirectOrderCreatePage(ByVal rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow, _
                                        ByVal resultRegist As IC3810203ReservationInfoRow, _
                                        ByVal resultSessionInfo As SC3100303DataSet.SC3100303SessionInfoDataTable, _
                                        ByVal resultVisitInfo As SC3100303DataSet.SC3100303ContactInfoDataTable, _
                                        ByVal resultDmsJobDtlId As SC3100303DataSet.SC3100303DmsJobDtlIdDataTable)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'セション値の設定
        'DMS用販売店コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, rowDmsCodeMap.CODE1)

        'DMS用店舗コード
        Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, rowDmsCodeMap.CODE2)

        'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, rowDmsCodeMap.ACCOUNT)

        '来店実績連番 
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, resultRegist.VISITSEQ)

        'DMS予約ID
        If ((resultDmsJobDtlId.Count = 0) OrElse _
             resultDmsJobDtlId(0).IsDMS_JOB_DTL_IDNull OrElse _
             String.IsNullOrWhiteSpace(resultDmsJobDtlId(0).DMS_JOB_DTL_ID)) Then
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, String.Empty)
        Else
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, resultDmsJobDtlId(0).DMS_JOB_DTL_ID)
        End If

        'RO番号
        Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, String.Empty)

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, String.Empty)

        'VIN　(値取得チェック済み)
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, resultSessionInfo(0).VCL_VIN)

        '「0：編集」固定
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_EDIT)

        'DMSID　(値取得チェック済み)
        Me.SetValue(ScreenPos.Next, SESSIONKEY_CUSTOMER_ID, resultSessionInfo(0).DMS_CST_CD_DISP)

        'コンタクトパーソン
        '値チェック
        If ((resultVisitInfo.Count = 0) OrElse _
             resultVisitInfo(0).IsVISITNAMENull OrElse _
             String.IsNullOrWhiteSpace(resultVisitInfo(0).VISITNAME)) Then
            '値無し
            Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_PARSON, String.Empty)
        Else
            '値有り
            Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_PARSON, HttpUtility.UrlEncode(resultVisitInfo(0).VISITNAME))
        End If

        'コンタクト電話番号
        '値チェック
        If ((resultVisitInfo.Count = 0) OrElse _
            resultVisitInfo(0).IsVISITTELNONull OrElse _
             String.IsNullOrWhiteSpace(resultVisitInfo(0).VISITTELNO)) Then
            '値無し
            Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_TELNO, String.Empty)
        Else
            '値有り
            Me.SetValue(ScreenPos.Next, SESSIONKEY_CONTACT_TELNO, HttpUtility.UrlEncode(resultVisitInfo(0).VISITTELNO))
        End If

        'RO作成フラグ
        Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISP_NUM_ROCREATE)

        ' RO作成画面に遷移
        Me.RedirectNextScreen(PGMID_LOCAL_TACT)


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
    '2018/02/20 NSK 山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

#End Region

    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 START
#Region "DMS販売店コード、店舗コードの取得する"

    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <returns>中断情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetDmsBlnCd(ByVal dealerCode As String, _
                                 ByVal branchCode As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            '基幹販売店コード、店舗コードを取得
            dmsDlrBrnTable = smbCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                            dealerCode, _
                                                            branchCode, _
                                                            String.Empty)
            If dmsDlrBrnTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode: Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable.Item(0)

    End Function

#End Region
    '2014/01/17 TMEJ  陳　TMEJ次世代サービス 工程管理機能開発 END

#Region "Call Back"
#Region "コールバック用受信クラス"
    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass

        ''' <summary>
        ''' 関数名前
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MEDTHOD As String

        ''' <summary>
        ''' 予約ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property REZID As Long

        ''' <summary>
        ''' 画面に表示される時間
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SHOWDATE As String

        ''' <summary>
        ''' 更新Count
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UPDATECNT As Integer
    End Class
#End Region
#Region "コールバック用送信クラス"

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        ''' <summary>
        ''' 関数名前
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MEDTHOD As String


        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CALLER As String

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RESULTCODE As Short

        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MESSAGEID As Long

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MESSAGE As String

        ''' <summary>
        ''' HTMLコンテンツ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CONTENTS As String
        ''' <summary>
        ''' 表示される日付
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SHOWDATE As String
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
        ''' <summary>
        ''' 来店実績台数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VSTCARCNT As Long
        '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
    End Class
#End Region
#Region "Call Back 関数"
    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements ICallbackEventHandler.RaiseCallbackEvent
        Logger.Info("RaiseCallbackEvent.S")

        Dim serializer = New JavaScriptSerializer
        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        'コールバック引数用内部クラスのインスタンスを生成し、JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New CallBackArgumentClass
        Dim rtValue = 0
        Dim dtStartDate As Date
        Dim dtEndDate As Date
        'clientからのパラを取得
        argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

        Dim sbStallStartTime As New StringBuilder
        sbStallStartTime.Append(argument.SHOWDATE)
        sbStallStartTime.Append(" ")
        sbStallStartTime.Append(m_strStallStartTime)

        Dim sbStallEndTime As New StringBuilder
        sbStallEndTime.Append(argument.SHOWDATE)
        sbStallEndTime.Append(" ")
        sbStallEndTime.Append(m_strStallEndTime)

        '移動、リサイズ、開始、終了の場合、画面の日付のパラがある
        dtStartDate = Date.Parse(sbStallStartTime.ToString(), CultureInfo.InvariantCulture)
        dtEndDate = Date.Parse(sbStallEndTime.ToString(), CultureInfo.InvariantCulture)
        If m_strStallEndTime = "00:00" Then
            dtEndDate = DateAdd("d", 1, dtEndDate)
        End If

        Try
            '呼ばられる関数
            Select Case argument.MEDTHOD
                Case "GetVstChips"
                    'UpdateDateSRがjsから当画面の日付
                    Dim dtDate As Date = Date.Parse(argument.SHOWDATE, CultureInfo.InvariantCulture)
                    '左上の日付をMM/dd(Day)の形式で表示される
                    Dim sbDate As StringBuilder = New StringBuilder
                    'MM/ddを取得
                    sbDate.Append(DateTimeFunc.FormatDate(11, dtDate))
                    '曜日を取得
                    sbDate.Append(GetDay(dtDate.DayOfWeek))
                    result.SHOWDATE = sbDate.ToString()
                    '当画面の日付のチップ情報を取得する
                    result.CONTENTS = GetChipDataFromServer(dtDate)
                    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
                    '当画面の来店実績台数を取得する
                    result.VSTCARCNT = GetVstCarCnt(dtDate)
                    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
                    rtValue = 0
                Case "GetSwitchChipId"
                    'UpdateDateSRがjsから当画面の日付
                    Dim dtDate As Date = Date.Parse(argument.SHOWDATE, CultureInfo.InvariantCulture)
                    '当画面の日付のチップ情報を取得する
                    result.CONTENTS = GetSwitchChipId(dtDate)
                    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） START
                    '当画面の来店実績台数を取得する
                    result.VSTCARCNT = GetVstCarCnt(dtDate)
                    '2013/04/25 TMEJ 張 ITxxxx_TSL自主研緊急対応（サービス） END
                    rtValue = 0
                Case "ClickBtnFollow"
                    rtValue = businessLogic.UpdateFollowFlg(argument.REZID, "1", objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, argument.UPDATECNT)
                    '当画面の日付のチップ情報を取得する
                    Dim dtDate As Date = Date.Parse(argument.SHOWDATE, CultureInfo.InvariantCulture)
                    result.CONTENTS = GetChipDataFromServer(dtDate)
                Case "ClickBtnClearFollow"
                    rtValue = businessLogic.UpdateFollowFlg(argument.REZID, "0", objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account, argument.UPDATECNT)
                    '当画面の日付のチップ情報を取得する
                    Dim dtDate As Date = Date.Parse(argument.SHOWDATE, CultureInfo.InvariantCulture)
                    result.CONTENTS = GetChipDataFromServer(dtDate)
            End Select

            If rtValue = 0 Then
                result.MEDTHOD = argument.MEDTHOD
                result.RESULTCODE = ResultCode.Success
            Else
                result.MEDTHOD = argument.MEDTHOD
                result.RESULTCODE = ResultCode.Failure
                result.MESSAGEID = rtValue
            End If

            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)
        Catch timeOutEx As OracleExceptionEx When timeOutEx.Number = 1013
            'DBタイムアウトエラー
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} ERR:DBTIMEOUT" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'Timeout
            result.RESULTCODE = ResultCode.Failure
            result.MESSAGEID = 901
            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)
        Finally
            serializer = Nothing
            argument = Nothing
            result = Nothing
            Logger.Info("RaiseCallbackEvent.E")
        End Try
    End Sub

    ''' <summary>
    ''' 曜日を取得
    ''' </summary>
    ''' <param name="nDay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDay(nDay As Integer) As String
        Dim rtDay As String
        Select Case nDay
            Case 0
                rtDay = WebWordUtility.GetWord(APPLICATIONID_VSTMANAGER, 17)
            Case Else
                rtDay = WebWordUtility.GetWord(APPLICATIONID_VSTMANAGER, 10 + nDay)
        End Select
        Return rtDay
    End Function
    ''' <summary>
    ''' コールバック用文字列を返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return Me.callBackResult
    End Function
#End Region
#End Region

End Class
