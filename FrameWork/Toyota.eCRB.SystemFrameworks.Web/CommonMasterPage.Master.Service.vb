Imports System.Reflection
Imports System.Text
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet

Namespace Toyota.eCRB.SystemFrameworks.Web

#If BUILD_SERVICE = 1 Then ' Service, GK のビルド時有効
    ''' <summary>
    ''' メニューカテゴリ
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FooterMenuCategory
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
        ' ''' <summary>
        ' ''' 初期値
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'None = 0
        ' ''' <summary>
        ' ''' メインメニュー
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'MainMenu = 100
        ' ''' <summary>
        ' ''' Customer
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Customer = 200
        ' ''' <summary>
        ' ''' TCV
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'TCV = 300
        ' ''' <summary>
        ' ''' スケジューラ
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Schedule = 400
        ' ''' <summary>
        ' ''' 電話帳
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'TelDirectory = 500
        ' ''' <summary>
        ' ''' R/O作成
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'RO = 600
        ' ''' <summary>
        ' ''' 説明ツール
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Explanation = 700
        ' ''' <summary>
        ' ''' SMB
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'SMB = 800
        ' ''' <summary>
        ' ''' 部品準備
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Parts = 900
        ' ''' <summary>
        ' ''' 完成検査
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Examination = 1000
        ' ''' <summary>
        ' ''' 追加作業
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'AddOperation = 1100
        ' ''' <summary>
        ' ''' ショールームステータス
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'ShowRoomStatus = 1200
        ''' <summary>
        ''' 初期値
        ''' </summary>
        ''' <remarks></remarks>
        None = 0
        ''' <summary>
        ''' SAメイン
        ''' </summary>
        ''' <remarks></remarks>
        MainMenu = 100
        ''' <summary>
        ''' TCメイン
        ''' </summary>
        ''' <remarks></remarks>
        TechnicianMain = 200
        ''' <summary>
        ''' FMメイン
        ''' </summary>
        ''' <remarks></remarks>
        ForemanMain = 300
        ''' <summary>
        ''' 予約管理
        ''' </summary>
        ''' <remarks></remarks>
        ReserveManagement = 400
        ''' <summary>
        ''' R/O一覧
        ''' </summary>
        ''' <remarks></remarks>
        RepairOrderList = 500
        ''' <summary>
        ''' 連絡先
        ''' </summary>
        ''' <remarks></remarks>
        Contact = 600
        ''' <summary>
        ''' 顧客詳細
        ''' </summary>
        ''' <remarks></remarks>
        CustomerDetail = 700
        ''' <summary>
        ''' 商品訴求コンテンツ
        ''' </summary>
        ''' <remarks></remarks>
        GoodsSolicitationContents = 800
        ''' <summary>
        ''' ｷｬﾝﾍﾟｰﾝ
        ''' </summary>
        ''' <remarks></remarks>
        Campaign = 900
        ''' <summary>
        ''' 全体管理
        ''' </summary>
        ''' <remarks></remarks>
        WholeManagement = 1000
        ''' <summary>
        ''' SMB
        ''' </summary>
        ''' <remarks></remarks>
        SMB = 1100
        ''' <summary>
        ''' 追加作業一覧
        ''' </summary>
        ''' <remarks></remarks>
        AddWorkList = 1200
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END
    End Enum

    ''' <summary>
    ''' ヘッダーボタンの種類を表す列挙型
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HeaderButton
        ''' <summary>
        ''' 初期値
        ''' </summary>
        ''' <remarks></remarks>
        None = 0
        ''' <summary>
        ''' 戻るボタン
        ''' </summary>
        ''' <remarks></remarks>
        Rewind = 1
        ''' <summary>
        ''' 進むボタン
        ''' </summary>
        ''' <remarks></remarks>
        Forward = 2
        ''' <summary>
        ''' ログアウトボタン
        ''' </summary>
        ''' <remarks></remarks>
        Logout = 3
    End Enum

    ''' <summary>
    ''' 共通マスタページのページ処理クラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CommonMasterPage
        Inherits System.Web.UI.MasterPage
        Implements IPostBackEventHandler

#Region "IsRewindButtonEnabled"
        ''' <summary>
        ''' ヘッダの「戻る」ボタンの状態
        ''' </summary>
        ''' <value>有効の場合はTrue、無効の場合はFalse。</value>
        ''' <returns>有効の場合はTrue、無効の場合はFalse。</returns>
        Public Property IsRewindButtonEnabled As Boolean
            Get
                If (ViewState("IsRewindButtonEnabled") Is Nothing) Then
                    Return True
                Else
                    Return CBool(ViewState("IsRewindButtonEnabled"))
                End If
            End Get
            Set(value As Boolean)
                ViewState("IsRewindButtonEnabled") = value
            End Set
        End Property
#End Region


#Region "定数"
        Private Const C_MSTPG_DISPLAYID As String = "MASTERMAIN_SVR"                ''マスターページ文言取得ID
        Private Const C_MSTPG_FOTTER_DISPLAYID As String = "MASTERFOOTER_SVR"       ''マスターページフッター文言取得ID
        Private Const C_MSTPG_FOOTER_ID_PREFIX As String = "MstPG_FootItem_"        ''フッターボタンのIDの接頭辞
        Private Const C_MSTPG_FOOTER_ID_MAIN As String = "Main_"                    ''フッターメインタグ(MainMenu,Customer,TCV)
        Private Const C_MSTPG_FOOTER_ID_SUB As String = "Sub_"                      ''フッターサブタグ
        Private Const C_MSTPG_FOTTER_ICON_PATH = "~/Styles/Images/FooterButtons/"   ''フッターアイコンのパス
        Private Const C_MSTPG_FOOTER_ICON_ON As String = "_on"                      ''フッターアイコン(選択状態)のファイル名
        Private Const C_MSTPG_FOOTER_ICON_DISABLED As String = "_disabled"                ''フッターアイコン(非活性状態)のファイル名
        Private Const C_MSTPG_FOTTER_ICON_EXT = ".png"                              ''フッターアイコンイメージ

        Private Const C_MSTPG_MENUITEM_ID_PREFIX As String = "MstPG_MenuItem_"      ''コンテキストメニュー項目のIDの接頭辞

        '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
        Private Const C_SMB_CUSTOMER_SEARCHTYPE_CUSTOMER As String = "1"            ''SMB顧客検索条件(顧客)
        Private Const C_SMB_CUSTOMER_SEARCHTYPE_CHIP As String = "2"                ''SMB顧客検索条件(チップ)
        Private Const C_SMB_MAIN_MENU_DISPLAY_ID As String = "SC3240101"            ''工程管理画面の画面ID
        Private Const C_SMB_CHIP_SEARCH_DISPLAY_ID As String = "SC3240401"          ''チップ検索画面の画面ID
        Private Const C_SMB_DISPLAY_TYPE_PARAMID As String = "SMB_DISPLAY_TYPE"     ''SMB表示フラグ取得用のPARAMID
        Private Const C_SMB_DISPLAY_TYPE_ON As String = "0"                         ''SMB表示判定「0：表示する」
        Private Const C_SMB_DISPLAY_TYPE_OFF As String = "1"                        ''SMB表示判定「1：表示しない」
        Private Const SYSTEMENV_OTHER_LINKAGE_DOMAIN As String = "OTHER_LINKAGE_DOMAIN" ''ドメイン取得ID
        '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END

        Private Const C_CUSTOMER_SEARCHTYPE_REGNO = "1"                             ''顧客検索条件(登録番号)
        Private Const C_CUSTOMER_SEARCHTYPE_NAME = "2"                              ''顧客検索条件(名前)
        Private Const C_CUSTOMER_SEARCHTYPE_VIN = "3"                               ''顧客検索条件(VIN)
        Private Const C_CUSTOMER_SEARCHTYPE_TEL = "4"                               ''顧客検索条件(TEL)
        Private Const C_CUSTOMER_SEARCHTYPE_RO = "5"                                ''顧客検索条件(R/O No)

        Friend Const C_EVENT_TCVCALLBACK As String = "TCVCallBack"                  ''TCVがクローズされた時のイベント名
        Friend Const C_EVENT_CONTEXTMENU_OPEN As String = "ContextMenuOpen"         ''コンテキストメニューを開いた時のイベント名
        Friend Const C_EVENT_CONTEXTMENU_CLOSE As String = "ContextMenuClose"       ''コンテキストメニューを閉じた時のイベント名

        Private Const C_SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"
        Private Const C_SESSION_CUSTOMERSEARCH_TYPE As String = "Toyota.eCRB.SystemFrameworks.Web.Common.CustomerSearchType"
        Private Const C_SESSION_CUSTOMERSEARCH_VALUE As String = "Toyota.eCRB.SystemFrameworks.Web.Common.CustomerSearchValue"

        Private Const C_REFRESH_TIMER_TIME As String = "REFRESH_TIMER_TIME"                ''クルクル対応の待ち時間設定値
#End Region

#Region "変数"
        Private _footButtons As New Dictionary(Of Integer, CommonMasterFooterButton)            ''フッター管理配列
        Private _headerButtons As New Dictionary(Of HeaderButton, CommonMasterHeaderButton)     ''ヘッダー管理配列
        Private _footPanel As Panel = Nothing                                           ''フッター配置要素(全体)
        Private _left As New Panel                                                      ''フッター配置要素(左)
        Private _center As New Panel                                                    ''フッター配置要素(真中)
        Private _right As New Panel                                                     ''フッター配置要素(右)
        Private _addSpace As Integer = Space.Left                                       ''フッター登録位置1:左、2:真中、3：右
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
        Private _category As FooterMenuCategory                                               ''フッターカテゴリ
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END
        Private _contextMenu As CommonMasterContextMenu
        Private _searchBox As CommonMasterSearchBox
        '2012/07/06 KN 小澤 STEP2対応 START
        Private _arrowMarginLeft As Unit = 93                                                   ''矢印アイコン計算用  
        '2012/07/06 KN 小澤 STEP2対応 END
        '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
        Private _SMBDisplayType As String                                                       ''SMBボタン、チップ検索表示判定「0：表示する」「1：表示しない」
        '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
#End Region

#Region "Enum"
        ''' <summary>
        ''' 追加スペース
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum Space
            ''' <summary>
            ''' 左
            ''' </summary>
            ''' <remarks>サブを追加するまで</remarks>
            Left
            ''' <summary>
            ''' 真中
            ''' </summary>
            ''' <remarks>サブの追加</remarks>
            Center
            ''' <summary>
            ''' 真中
            ''' </summary>
            ''' <remarks>サブの追加後</remarks>
            Right
        End Enum
#End Region

#Region "イベント"
        ''' <summary>
        ''' ヘッダの「戻る」ボタンタップ時に呼び出されます。
        ''' </summary>
        ''' <remarks>イベントハンドラ内で、CancelEventArgsオブジェクトのCancelプロパティをTrueに設定すると、処理を中断できます。</remarks>
        Public Event Rewinding As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' ヘッダの「進む」ボタンタップ時、または顧客検索時に呼び出されます。
        ''' </summary>
        ''' <remarks>イベントハンドラ内で、CancelEventArgsオブジェクトのCancelプロパティをTrueに設定すると、処理を中断できます。</remarks>
        Public Event Forwarding As EventHandler(Of CancelEventArgs)

        ''' <summary>
        ''' ヘッダの「ログアウト」タップ時に呼び出されます。
        ''' </summary>
        ''' <remarks>イベントハンドラ内で、CancelEventArgsオブジェクトのCancelプロパティをTrueに設定すると、処理を中断できます。</remarks>
        Public Event Logout As EventHandler(Of CancelEventArgs)
#End Region

#Region "OnInit"
        ''' <summary>
        ''' 初期処理。
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' 初期処理でヘッダー・フッターのコントロールを作成。
        ''' </remarks>
        Protected Overrides Sub OnInit(e As System.EventArgs)

            MyBase.OnInit(e)

            Dim buttonIds As Integer() = CType(Me.Page, BasePage).DeclareCommonMasterFooter(Me, _category)
            Dim staff As StaffContext = StaffContext.Current

            _footPanel = CType(Me.FindControl("MstPG_FooterButton"), Panel)

            _left.ID = C_MSTPG_FOOTER_ID_PREFIX & "Space_Left"
            _center.ID = C_MSTPG_FOOTER_ID_PREFIX & "Space_Center"
            _right.ID = C_MSTPG_FOOTER_ID_PREFIX & "Space_Right"

            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
            ' ''メインメニューを追加。
            '_AddFooterButton(FooterMenuCategory.MainMenu)
            'If _category = FooterMenuCategory.MainMenu Then
            '    _AddFooterButton(buttonIds)
            'End If
            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
            'SMB表示フラグを取得する
            Dim daBranchEnvSetting As New BranchEnvSetting
            Dim drDealerEnvSetting As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
                daBranchEnvSetting.GetEnvSetting(staff.DlrCD, staff.BrnCD, C_SMB_DISPLAY_TYPE_PARAMID)
            If Not IsNothing(drDealerEnvSetting) Then _SMBDisplayType = drDealerEnvSetting.PARAMVALUE Else _SMBDisplayType = C_SMB_DISPLAY_TYPE_OFF

            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
            ' ''SAとSMとCTとFM権限はSMBボタンを追加する
            'If staff.OpeCD = Operation.SA OrElse staff.OpeCD = Operation.SM _
            '   OrElse staff.OpeCD = Operation.FM OrElse staff.OpeCD = Operation.CT Then
            '    _AddFooterButton(FooterMenuCategory.SMB)
            '    If _category = FooterMenuCategory.SMB Then
            '        _AddFooterButton(buttonIds)
            '    End If
            'End If
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END

            ''ショールームステータスを追加。
            ''2012/07/06 KN 小澤 STEP2対応 START
            ''If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
            'If VersionInformation.IsEqualOrLaterThan(1, 2, 0) AndAlso
            '   Not (_CheckSalesService()) Then
            '    '2012/07/06 KN 小澤 STEP2対応 END
            '    If (staff.OpeCD = Operation.BM OrElse staff.OpeCD = Operation.SSM OrElse staff.OpeCD = Operation.SLR) Then
            '        _AddFooterButton(FooterMenuCategory.ShowRoomStatus)
            '        If _category = FooterMenuCategory.ShowRoomStatus Then
            '            _AddFooterButton(buttonIds)
            '        End If
            '    End If
            'End If

            ''2013/03/07 TMEJ 成澤 IT7683_【A.STEP1】 START
            ' ''顧客情報を追加。
            'If (staff.OpeCD <> Operation.TEC AndAlso _
            '    staff.OpeCD <> Operation.SLR AndAlso _
            '    staff.OpeCD <> Operation.SVR) Then
            '    _AddFooterButton(FooterMenuCategory.Customer)
            '    If _category = FooterMenuCategory.Customer Then
            '        _AddFooterButton(buttonIds)
            '    End If
            'End If
            ''2013/03/07 TMEJ 成澤 IT7683_【A.STEP1】 END
            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

            _SetFooterButton()

            ''フッターの登録。
            _footPanel.Controls.Add(_left)
            _footPanel.Controls.Add(_center)
            _footPanel.Controls.Add(_right)

            '検索バー
            _searchBox = New CommonMasterSearchBox(CType(Me.FindControl("MstPG_CustomerSearchTextBoxPanel"), Panel), _
                                                   CType(Me.FindControl("MstPG_CustomerSearchTextBox"), CustomTextBox), _
                                                   CType(Me.FindControl("MstPG_CustomerSearchButton"), ImageButton))

            'ヘッダーのボタン、コンテキストメニュー登録
            _contextMenu = New CommonMasterContextMenu(CType(Me.FindControl("MstPG_PopOver1"), PopOver), _
                                                       CType(Me.FindControl("MstPG_IcropIcon"), ImageButton))

            Dim menuItemIds As Integer() = CType(Me.Page, BasePage).DeclareCommonMasterContextMenu(Me)
            _AddHeaderButton(menuItemIds)

            'ICustomerForm を実装している場合のみロック表示
            If (TypeOf Me.Page Is ICustomerForm) Then
                OperationLockedImage.Visible = True
                If (CType(Me.Page, ICustomerForm).DefaultOperationLocked) Then
                    OperationLocked.Value = "1"
                Else
                    OperationLocked.Value = "0"
                End If
            End If

            ' クルクル対応用のメッセージ・設定値を取得
            Dim sysEnv As New SystemEnvSetting
            CType(FindControl("MstPG_RefreshTimerTime"), HiddenField).Value = sysEnv.GetSystemEnvSetting(C_REFRESH_TIMER_TIME).PARAMVALUE
            CType(FindControl("MstPG_RefreshTimerMessage1"), HiddenField).Value = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 21)

            'ドメイン設定
            Me._SetDomain()

        End Sub


        Private Sub _SetFooterButton()

            Dim buttonIds As Integer() = CType(Me.Page, BasePage).DeclareCommonMasterFooter(Me, _category)
            Dim staff As StaffContext = StaffContext.Current


            If _CheckSalesService() Then

                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
                'Select Case staff.OpeCD
                '    '2012/07/06 KN 小澤 STEP2対応 START
                '    'Case Operation.SA
                '    '    '-----------------------------------------------サービスアドバイザ
                '    Case Operation.SA, Operation.SM
                '        '-----------------------------------------------サービスアドバイザ、サービスマネージャー
                '        '2012/07/06 KN 小澤 STEP2対応 END
                '        ''R/O作成を追加。
                '        _AddFooterButton(FooterMenuCategory.RO)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.RO Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''追加作業を追加。
                '        _AddFooterButton(FooterMenuCategory.AddOperation)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.AddOperation Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        '2012/07/06 KN 小澤 STEP2対応 START
                '        ' ''説明ツールを追加。
                '        '_AddFooterButton(FooterMenuCategory.Explanation)
                '        ' ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        'If _category = FooterMenuCategory.Explanation Then
                '        '    _AddFooterButton(buttonIds)
                '        'End If
                '        '2012/07/06 KN 小澤 STEP2対応 END


                '    Case Operation.PS
                '        '-----------------------------------------------パーツスタッフ
                '        ''部品準備を追加。
                '        _AddFooterButton(FooterMenuCategory.Parts)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.Parts Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''R/O作成を追加。
                '        _AddFooterButton(FooterMenuCategory.RO)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.RO Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''追加作業を追加。
                '        _AddFooterButton(FooterMenuCategory.AddOperation)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.AddOperation Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '    Case Operation.CT, Operation.FM
                '        '-----------------------------------------------コントローラ、フォアマン
                '        ''R/O作成を追加。
                '        _AddFooterButton(FooterMenuCategory.RO)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.RO Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''追加作業を追加。
                '        _AddFooterButton(FooterMenuCategory.AddOperation)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.AddOperation Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''完成検査を追加。
                '        _AddFooterButton(FooterMenuCategory.Examination)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.Examination Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '    Case Operation.TEC
                '        '-----------------------------------------------テクニシャン
                '        ''追加作業を追加。
                '        _AddFooterButton(FooterMenuCategory.AddOperation)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.AddOperation Then
                '            _AddFooterButton(buttonIds)
                '        End If

                '        ''完成検査を追加。
                '        _AddFooterButton(FooterMenuCategory.Examination)
                '        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                '        If _category = FooterMenuCategory.Examination Then
                '            _AddFooterButton(buttonIds)
                '        End If

                'End Select

                ' ''スケジューラを追加。
                '_AddFooterButton(FooterMenuCategory.Schedule)
                'If _category = FooterMenuCategory.Schedule Then
                '    _AddFooterButton(buttonIds)
                'End If

                ' ''電話帳を追加。
                '_AddFooterButton(FooterMenuCategory.TelDirectory)
                'If _category = FooterMenuCategory.TelDirectory Then
                '    _AddFooterButton(buttonIds)
                'End If
                Select Case staff.OpeCD
                    Case Operation.SA, Operation.SM
                        ''サービスアドバイザー「9」権限 or サービスマネージャ「10」権限

                        ''SAメイン
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''顧客詳細
                        _AddFooterButton(FooterMenuCategory.CustomerDetail)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.CustomerDetail Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''商品訴求コンテンツ
                        _AddFooterButton(FooterMenuCategory.GoodsSolicitationContents)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.GoodsSolicitationContents Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''キャンペーン
                        _AddFooterButton(FooterMenuCategory.Campaign)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Campaign Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''予約管理
                        _AddFooterButton(FooterMenuCategory.ReserveManagement)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.ReserveManagement Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''SMB
                        _AddFooterButton(FooterMenuCategory.SMB)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.SMB Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                    Case Operation.TEC
                        ''テクニシャン「14」権限

                        ''TCメイン
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''追加作業一覧
                        _AddFooterButton(FooterMenuCategory.AddWorkList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.AddWorkList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                    Case Operation.SVR
                        ''受付「52」権限

                        ''受付待ち一覧
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''予約管理
                        _AddFooterButton(FooterMenuCategory.ReserveManagement)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.ReserveManagement Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''全体管理
                        _AddFooterButton(FooterMenuCategory.WholeManagement)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.WholeManagement Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                    Case Operation.CT
                        ''コントローラー「55」権限

                        ''SMB
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''追加作業一覧
                        _AddFooterButton(FooterMenuCategory.AddWorkList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.AddWorkList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                    Case Operation.FM
                        ''フォアマン「58」権限

                        ''FMメイン
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''SMB
                        _AddFooterButton(FooterMenuCategory.SMB)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.SMB Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''追加作業一覧
                        _AddFooterButton(FooterMenuCategory.AddWorkList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.AddWorkList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                    Case Operation.CHT
                        ''チーフテクニシャン「60」権限

                        ''SMB
                        _AddFooterButton(FooterMenuCategory.MainMenu)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.MainMenu Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''TCメイン
                        _AddFooterButton(FooterMenuCategory.TechnicianMain)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.TechnicianMain Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''FMメイン
                        _AddFooterButton(FooterMenuCategory.ForemanMain)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.ForemanMain Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''R/O一覧
                        _AddFooterButton(FooterMenuCategory.RepairOrderList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.RepairOrderList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''追加作業一覧
                        _AddFooterButton(FooterMenuCategory.AddWorkList)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.AddWorkList Then
                            _AddFooterButton(buttonIds)
                        End If
                        ''連絡先
                        _AddFooterButton(FooterMenuCategory.Contact)
                        ''カテゴリーがTCVの時、TCVの下にアイテムを追加。
                        If _category = FooterMenuCategory.Contact Then
                            _AddFooterButton(buttonIds)
                        End If

                End Select
                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

            Else
                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
                ' ''TCVを追加。
                'If (staff.OpeCD <> Operation.SLR) Then
                '    _AddFooterButton(FooterMenuCategory.TCV)
                '    If _category = FooterMenuCategory.TCV Then
                '        _AddFooterButton(buttonIds)
                '    End If
                'End If
                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END
            End If


        End Sub

#End Region

        ''' <summary>
        ''' 通知アイコンを更新
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetNotice()
            Dim sb As New StringBuilder
            sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            sb.Append("    (function(window) {").Append(vbCrLf)
            sb.Append("         icropScript.ui.setNotice();})(window);").Append(vbCrLf)
            sb.Append("</script>" & vbCrLf)
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "icropScript.ui.setNotice", sb.ToString)
        End Sub

        ''' <summary>
        ''' 未対応来店客アイコンを更新
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetVisitor()
            Dim sb As New StringBuilder
            sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            sb.Append("    (function(window) {").Append(vbCrLf)
            sb.Append("         icropScript.ui.setVisitor();})(window);").Append(vbCrLf)
            sb.Append("</script>" & vbCrLf)
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "icropScript.ui.setVisitor", sb.ToString)
        End Sub

        ''' <summary>
        ''' ヘッダーボタンを管理配列に追加
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _AddHeaderButton(ByVal menuItemIds As Integer())
            '戻る
            _headerButtons.Add(HeaderButton.Rewind, New CommonMasterHeaderButton(CType(Me.FindControl("MstPG_BackLinkButton"), WebControl)))
            '進む
            _headerButtons.Add(HeaderButton.Forward, New CommonMasterHeaderButton(CType(Me.FindControl("MstPG_NextLinkButton"), WebControl)))
            'コンテキストメニュー項目
            For Each menuItemId As Integer In menuItemIds
                Using owner As New CustomHyperLink()
                    owner.ID = C_MSTPG_MENUITEM_ID_PREFIX & menuItemId
                    owner.CssClass = "MstPG_ContextMenuItem"
                    Dim menuItem As CommonMasterContextMenuItem = New CommonMasterContextMenuItem(owner, menuItemId)
                    _contextMenu.AddMenuItem(menuItem)
                    CType(Me.FindControl("MstPG_PopOver1"), WebControl).Controls.Add(owner)

                    If (menuItemId = CommonMasterContextMenuBuiltinMenuID.LogoutItem) Then
                        'ログアウト（API互換性維持のため）
                        _headerButtons.Add(HeaderButton.Logout, New CommonMasterHeaderButton(owner))

                        '組み込みメニュー項目（ログアウト）の初期化
                        menuItem.Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 12)
                        menuItem.PresenceCategory = "4"
                        menuItem.PresenceDetail = "0"
                        AddHandler menuItem.Click, AddressOf LogoutButton_Click
                    ElseIf (menuItemId = CommonMasterContextMenuBuiltinMenuID.StandByItem) Then
                        '組み込みメニュー項目（スタンバイ）の初期化
                        menuItem.Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 16)
                        menuItem.PresenceCategory = "1"
                        menuItem.PresenceDetail = "0"
                    ElseIf (menuItemId = CommonMasterContextMenuBuiltinMenuID.SuspendItem) Then
                        '組み込みメニュー項目（一時退席）の初期化
                        menuItem.Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 17)
                        menuItem.PresenceCategory = "3"
                        menuItem.PresenceDetail = "0"
                    End If
                End Using
            Next
        End Sub

        ''' <summary>
        ''' フッターにボタンを追加。
        ''' </summary>
        ''' <param name="buttonIds"></param>
        ''' <remarks></remarks>
        Private Sub _AddFooterButton(ByVal buttonIds As Integer())

            _addSpace = Space.Center

            ''サブメニューのため、位置を真中に変更して追加。
            For Each buttonId As Integer In buttonIds
                _AddFooterButton(buttonId)
            Next buttonId

            _addSpace = Space.Right
        End Sub

        ''' <summary>
        ''' フッターにボタンを追加。
        ''' </summary>
        ''' <param name="buttonId"></param>
        ''' <remarks></remarks>
        Private Sub _AddFooterButton(ByVal buttonId As Integer)

            Dim button As New CustomHyperLink

            ''種類によって、タグを変更。
            If buttonId Mod 100 = 0 Then
                ''表示しているカテゴリーの時非活性に。
                If _category = buttonId Then
                    button.CssClass = "mstpg-selected"
                Else
                    button.CssClass = ""
                End If
                button.ID = C_MSTPG_FOOTER_ID_PREFIX & C_MSTPG_FOOTER_ID_MAIN & buttonId
                button.Width = 78
                '2012/07/06 KN 小澤 STEP2対応 START
                button.ArrowMarginLeft = CInt(_arrowMarginLeft.Value)
                '2012/07/06 KN 小澤 STEP2対応 END
            Else
                button.ID = C_MSTPG_FOOTER_ID_PREFIX & C_MSTPG_FOOTER_ID_SUB & buttonId
                button.Width = 86
                '2012/07/06 KN 小澤 STEP2対応 START
                button.ArrowMarginLeft = CInt(_arrowMarginLeft.Value + 7)
                '2012/07/06 KN 小澤 STEP2対応 END
            End If

            button.IconUrl = C_MSTPG_FOTTER_ICON_PATH & buttonId & C_MSTPG_FOTTER_ICON_EXT
            button.Text = WebWordUtility.GetWord(C_MSTPG_FOTTER_DISPLAYID, buttonId)
            '2012/07/06 KN 小澤 STEP2対応 START
            button.ButtonId = CStr(buttonId)
            '2012/07/06 KN 小澤 STEP2対応 END

            'SMBボタンで表示フラグが1の場合は非表示を設定する
            If buttonId = FooterMenuCategory.SMB AndAlso C_SMB_DISPLAY_TYPE_OFF.Equals(_SMBDisplayType) Then
                button.Visible = False
            End If

            ''フッター管理配列に登録
            _footButtons.Add(buttonId, New CommonMasterFooterButton(button, buttonId))

            ''種類によって登録位置を変更
            If _addSpace = Space.Left Then
                _left.Controls.Add(button)
            ElseIf _addSpace = Space.Center Then
                _center.Controls.Add(button)
            ElseIf _addSpace = Space.Right Then
                _right.Controls.Add(button)
            End If
            '2012/07/06 KN 小澤 STEP2対応 START
            ''次のフッターボタン用に計算しておく
            Dim nextArrowMarginLeft As Integer = CInt(_arrowMarginLeft.Value + button.Width.Value + 5)
            _arrowMarginLeft = nextArrowMarginLeft
            '2012/07/06 KN 小澤 STEP2対応 END

        End Sub

#Region "FooterButtonClick_EventHandler"
        ''' <summary>
        ''' フッターボタンクリックイベントです。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub FooterButtonClick_EventHandler(ByVal sender As Object, e As EventArgs)

            ''押されたボタンのイベントを発生させます。
            Dim id As String = CType(sender, CustomHyperLink).ID
            id = id.Substring(id.LastIndexOf("_", StringComparison.OrdinalIgnoreCase) + 1)
            _footButtons(CInt(id)).OnClick()

            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
            ' ''TCVの場合はクライアント処理になるためスクリプトを作成。
            'If ((CInt(id) = FooterMenuCategory.TCV) OrElse Me.GetFooterButton(CInt(id)).EventArgs.TCVFunction) Then

            '    '画面タイトル
            '    CType(Me.FindControl("MstPG_TitleLabel"), Label).Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13))
            '    CType(Me.FindControl("MstPG_WindowTitle"), Literal).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 13)

            '    Dim sm As ClientScriptManager = Page.ClientScript
            '    Dim sb As New StringBuilder

            '    sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            '    sb.Append("(function(window) {").Append(vbCrLf)
            '    sb.Append(" icropScript.tcvCloseCallback = function (args) {").Append(vbCrLf)
            '    sb.Append("     $('#MstPG_TCV_Params')[0].value = $.toJSON(args);").Append(vbCrLf)
            '    sb.Append(sm.GetPostBackEventReference(Me, C_EVENT_TCVCALLBACK) & ";").Append(vbCrLf)
            '    sb.Append(" };" & vbCrLf)
            '    sb.Append(" icropScript.tcvStatusCallback = function (args) {").Append(vbCrLf)
            '    sb.Append("     $('#MstPG_OperationLocked').val(args.MenuLockFlag ? '1' : '0');").Append(vbCrLf)
            '    sb.Append(" };" & vbCrLf)

            '    Dim query As New StringBuilder
            '    query.Append("'" & CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("TCV")))

            '    Dim params As Dictionary(Of String, Object) = _footButtons(CInt(id)).EventArgs.Parameters
            '    params("CloseCallback") = "icropScript.tcvCloseCallback"
            '    params("StatusCallback") = "icropScript.tcvStatusCallback"

            '    Dim serializer As New JavaScriptSerializer
            '    'JSON形式に変換した文字列を戻り値として返却
            '    Dim result As String = serializer.Serialize(params)
            '    'query.Append("?jsonData=" & HttpUtility.UrlEncode(result))
            '    query.Append("?jsonData=' + encodeURIComponent('" & HttpUtility.JavaScriptStringEncode(result) & "')")

            '    Logger.Debug("TCV Call parameter:" & result)

            '    sb.Append(" window.addEventListener('load',function(){freezeHeaderOperation(); location.href = " & query.ToString & "; }); ").Append(vbCrLf)
            '    sb.Append("})(window);").Append(vbCrLf)
            '    sb.Append("g_MstPGshowLoding();").Append(vbCrLf)
            '    sb.Append("</script>" & vbCrLf)

            '    'スクリプト登録
            '    Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "tcv", sb.ToString)
            'End If
            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

        End Sub
#End Region

#Region "RaisePostBackEvent"
        ''' <summary>
        ''' ポストバックイベントを取得します。
        ''' </summary>
        ''' <param name="eventArgument"></param>
        ''' <remarks></remarks>
        Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent

            If eventArgument = C_EVENT_TCVCALLBACK Then
                ''TCVからコールバックされた時の処理。
                Dim params As String = "(no parameter)"
                Try
                    params = CType(Me.FindControl("MstPG_TCV_Params"), TextBox).Text
                    Logger.Debug("TCV CallBack parameter:" & params)

                    'JSON形式の文字列を変換
                    Dim serializer As New JavaScriptSerializer
                    Dim args As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(params)

                    ''BasePageから指定画面へ遷移させます。
                    Dim base As BasePage = CType(Me.Page, BasePage)
                    base.TCVCallBack(args)
                Catch ex As ThreadAbortException
                    '動作に影響しない例外の為、無視する（TCVからの画面遷移時に発生するケースがある）
                Catch ex As Exception
                    Logger.Error("TCV parameter parse error :" & ex.Message, ex)
                    Throw
                End Try

            ElseIf eventArgument = C_EVENT_CONTEXTMENU_OPEN Then
                ''コンテキストメニューを開く時の処理。
                _contextMenu.UseAutoOpening = True
                _contextMenu.OnOpen()

            ElseIf eventArgument = C_EVENT_CONTEXTMENU_CLOSE Then
                ''コンテキストメニューを閉じる時の処理。
                _contextMenu.UseAutoOpening = False
                _contextMenu.OnClose()
            End If

        End Sub
#End Region

#Region "Page_Load"
        ''' <summary>
        ''' マスターページのページロードを処理。
        ''' </summary>
        ''' <param name="sender">イベントの発生元。</param>
        ''' <param name="e">イベントに固有のデータ。</param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then

                ''マスターページの画面文言を設定します。
                Me._SetControlWord()

                ''画面初期表示時のフッターアニメーションを登録します。
                Me._RegisterStartUpFooterOpenScript()

                '顧客検索の設定
                Me._SetCustomerSearch()

            End If

            ''フッターのボタンを再割り当てします。
            For Each footButton In _footButtons
                AddHandler footButton.Value.Owner.Click, AddressOf FooterButtonClick_EventHandler
            Next

            ''画面で設定された状態に合わせフッターを設定します。
            _setFooterStatus()

            'タイトルの設定
            _SetTitile()

            '画面遷移履歴Listを取得
            Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList
            If siteMapNodeList.Count < 1 Then
                '先頭画面(メニュー)のため、戻るボタン非表示
                Me.FindControl("MstPG_BackLinkButton").Visible = False
            Else
                '先頭以外の画面のため、戻るボタンを表示
                Me.FindControl("MstPG_BackLinkButton").Visible = True
            End If

            '進む(Step1は範囲外)
            'Me.FindControl("MstPG_NextLinkButton").Visible = False

            'ロック状態の設定
            _SetLockState()

            '顧客検索の文言セット
            _SetControlWord()

            'スクリプト登録
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "MstPG_doPostBack", String.Format(CultureInfo.InvariantCulture, "function MstPG_doPostBack() {{ {0}; }}", Me.Page.ClientScript.GetPostBackEventReference(Me.Page, "")), True)

        End Sub
#End Region

#Region "LockCheckBox"
        ''' <summary>
        ''' 画面ロック用のチェックボックスインスタンスを取得。
        ''' </summary>
        ''' <remarks></remarks>
        Friend ReadOnly Property OperationLockedImage As Image
            Get
                Return CType(FindControl("MstPG_OperationLockedImage"), Image)
            End Get
        End Property

        Friend ReadOnly Property OperationLocked As HiddenField
            Get
                Return CType(FindControl("MstPG_OperationLocked"), HiddenField)
            End Get
        End Property
#End Region

#Region "_SetTitile"
        ''' <summary>
        ''' タイトルの設定
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetTitile()
            '画面タイトル
            CType(Me.FindControl("MstPG_TitleLabel"), Label).Text = HttpUtility.HtmlEncode(WebWordUtility.GetTitle)
            CType(Me.FindControl("MstPG_WindowTitle"), Literal).Text = HttpUtility.HtmlEncode(WebWordUtility.GetTitle)

            'セッションからログイン情報を設定する
            Dim staffContext As StaffContext = staffContext.Current

            Dim staffInfo As New StringBuilder
            staffInfo.Append(staffContext.DlrName)
            If Not String.IsNullOrEmpty(staffContext.BrnName) Then
                staffInfo.Append(" " & staffContext.BrnName)
            End If
            staffInfo.Append(" " & staffContext.OpeName)
            staffInfo.Append(" " & staffContext.UserName)

            CType(Me.FindControl("MstPG_StaffInfo"), Label).Text = HttpUtility.HtmlEncode(staffInfo.ToString)
            staffContext = Nothing
        End Sub
#End Region

#Region "_SetControlWord"
        ''' <summary>
        ''' 各コントロールへ文言を設定します。
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetControlWord()

            'セールスユーザかサービスユーザか判断
            Dim salesSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton"), SegmentedButton)
            Dim serviceSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton_Service"), SegmentedButton)
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
            Dim serviceSMBSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton_SMB"), SegmentedButton)
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
            Dim types As SegmentedButton = Nothing
            If Not _CheckSalesService() Then
                salesSearchTypes.Visible = True
                serviceSearchTypes.Visible = False
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
                serviceSMBSearchTypes.Visible = False
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
                types = salesSearchTypes
            Else
                salesSearchTypes.Visible = False
                serviceSearchTypes.Visible = True
                types = serviceSearchTypes

                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
                'チップ検索条件の設定
                'チップ検索エリアの文言設定
                For Each smbType As ListItem In serviceSMBSearchTypes.Items
                    Select Case smbType.Value
                        Case C_SMB_CUSTOMER_SEARCHTYPE_CUSTOMER
                            smbType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 22))
                        Case C_SMB_CUSTOMER_SEARCHTYPE_CHIP
                            smbType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 23))
                    End Select
                Next smbType

                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
                ''SMB表示フラグが0且つ、SA・SM・CT・FM権限の場合はチップ検索エリアを表示する
                'Dim staffContext As StaffContext = staffContext.Current
                'If C_SMB_DISPLAY_TYPE_ON.Equals(_SMBDisplayType) AndAlso _
                '    (staffContext.OpeCD = Operation.SA OrElse staffContext.OpeCD = Operation.SM OrElse _
                '     staffContext.OpeCD = Operation.CT OrElse staffContext.OpeCD = Operation.FM) Then
                '    serviceSMBSearchTypes.Visible = True
                'End If
                'SMB表示フラグが0且つ、SA・SM・CT・FM・CHT権限の場合はチップ検索エリアを表示する
                Dim staffContext As StaffContext = staffContext.Current
                If C_SMB_DISPLAY_TYPE_ON.Equals(_SMBDisplayType) AndAlso _
                    (staffContext.OpeCD = Operation.SA OrElse staffContext.OpeCD = Operation.SM OrElse _
                     staffContext.OpeCD = Operation.CT OrElse staffContext.OpeCD = Operation.FM OrElse _
                     staffContext.OpeCD = Operation.CHT) Then
                    serviceSMBSearchTypes.Visible = True
                End If
                '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

                '初期選択設定
                If Not (IsPostBack) Then
                    If Me.Context.Request.Path.IndexOf(C_SMB_MAIN_MENU_DISPLAY_ID, StringComparison.OrdinalIgnoreCase) > 0 OrElse _
                       Me.Context.Request.Path.IndexOf(C_SMB_CHIP_SEARCH_DISPLAY_ID, StringComparison.OrdinalIgnoreCase) > 0 Then
                        '工程管理画面、チップ検索画面の場合はチップタブを選択状態にする
                        serviceSMBSearchTypes.Items(1).Selected = True
                    Else
                        '上記以外の画面の場合は顧客タブを選択状態にする
                        serviceSMBSearchTypes.Items(0).Selected = True
                    End If
                End If
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
            End If

            '顧客検索条件の文言設定
            For Each type As ListItem In types.Items

                Select Case type.Value

                    Case C_CUSTOMER_SEARCHTYPE_REGNO
                        type.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 2))
                    Case C_CUSTOMER_SEARCHTYPE_NAME
                        type.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 3))
                    Case C_CUSTOMER_SEARCHTYPE_VIN
                        type.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 4))
                    Case C_CUSTOMER_SEARCHTYPE_TEL
                        type.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 5))
                    Case C_CUSTOMER_SEARCHTYPE_RO
                        type.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 14))
                End Select

            Next type

            ''顧客検索用文言
            CType(Me.FindControl("MstPG_CustomerSearchTypeWordRegNoTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 6)
            CType(Me.FindControl("MstPG_CustomerSearchTypeWordNameTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 7)
            CType(Me.FindControl("MstPG_CustomerSearchTypeWordVinTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 8)
            CType(Me.FindControl("MstPG_CustomerSearchTypeWordTelTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 9)
            CType(Me.FindControl("MstPG_CustomerSearchTypeWordROTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 15)

            ''入力チェック
            CType(Me.FindControl("MstPG_CustomerSearchWordNoDataTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 10)
            CType(Me.FindControl("MstPG_CustomerSearchWordNoSelectTextBox"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 11)

            '通知／来店ポップアップ用文言
            CType(Me.FindControl("MstPG_Title_Notice"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 18)
            CType(Me.FindControl("MstPG_Title_Visitor"), TextBox).Text = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, 19)
        End Sub
#End Region

#Region "_SetLockState"
        ''' <summary>
        ''' ロック状態の設定
        ''' </summary>
        Private Sub _SetLockState()
            If (OperationLockedImage.Visible) Then
                'ロックアイコンを設定
                If (OperationLocked.Value.Equals("1")) Then
                    OperationLockedImage.ImageUrl = "~/Styles/Images/MasterPage/FooterSetUnLock_on.png"
                Else
                    OperationLockedImage.ImageUrl = "~/Styles/Images/MasterPage/FooterSetUnLock.png"
                End If
            End If

            'ロック状態をicropScriptに設定
            If (OperationLocked.Value.Equals("1")) Then
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "OperationLocked", "icropScript.OperationLocked = true;", True)
            Else
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "OperationLocked", "icropScript.OperationLocked = false;", True)
            End If
        End Sub
#End Region

#Region "_SetCustomerSearch"
        Private Sub _SetCustomerSearch()

            Dim aplFile As String = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath
            Dim aplId As String() = aplFile.Substring(aplFile.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1).Split("."c)
            If Not aplId(0).Equals("SC3080101") AndAlso Not aplId(0).Equals("SC3080102") Then
                Return
            End If

            Dim current As Dictionary(Of String, Object) = Me._GetCurrentSessionInfo()

            If current.ContainsKey("searchString") Then
                CType(Me.FindControl("MstPG_CustomerSearchTextBox"), CustomTextBox).Text = CStr(current("searchString"))
            End If
            If current.ContainsKey("searchType") Then
                CType(Me.FindControl("MstPG_SearchTypeSegmenteButton"), SegmentedButton).SelectedValue = CStr(current("searchType"))
            End If

            If (_DispSearch()) Then
                _searchBox.Visible = True

                Dim sm As ClientScriptManager = Page.ClientScript
                Dim sb As New StringBuilder
                sb.Append("<script type='text/javascript'>").Append(vbCrLf)
                sb.Append(" $(function () { changeCustomerSearchSize(); }); ").Append(vbCrLf)
                sb.Append("</script>" & vbCrLf)

                sm.RegisterStartupScript(Me.GetType, "searchOpen", sb.ToString)
            Else
                _searchBox.Visible = False
            End If

        End Sub
#End Region

#Region "_SetDomain"

        Private Sub _SetDomain()
            'TBL_SYSTEMENVからドメイン名を取得
            Dim systemEnv As New SystemEnvSetting
            Dim systemEnvParam As String = String.Empty
            Dim drSystemEnvSetting As SYSTEMENVSETTINGRow = _
                systemEnv.GetSystemEnvSetting(SYSTEMENV_OTHER_LINKAGE_DOMAIN)

            '取得できた場合のみ設定する
            If Not (IsNothing(drSystemEnvSetting)) Then
                systemEnvParam = drSystemEnvSetting.PARAMVALUE

            End If

            'ドメインをHiddenに格納
            '「:」を抜いた文字列取得
            Dim domain = systemEnvParam.Split(CChar(":"))(0)
            'スーパードメイン名だけを取得する
            domain = domain.Substring(domain.IndexOf(".") + 1, _
                                      domain.Length - domain.IndexOf(".") - 1)
            CType(FindControl("MstPG_Domain"), HiddenField).Value = domain

        End Sub

#End Region

#Region ""
        Private Function _GetCurrentSessionInfo() As Dictionary(Of String, Object)

            Dim nodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList
            Dim node As SerializableSiteMapNode = nodeList(nodeList.Count - 1)

            '履歴のPageSessionInfoプロパティにて画面引渡しDictionary(Of String, Object)を取得
            Return node.PageSessionInfo

        End Function
#End Region

#Region "_RegisterStartUpFooterOpenScript"
        ''' <summary>
        ''' フッターアニメーションの登録をします。
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _RegisterStartUpFooterOpenScript()

            Dim sb As New StringBuilder

            sb.Append("<script type='text/javascript'>").Append(vbCrLf)
            sb.Append("    footerOpen();").Append(vbCrLf)
            sb.Append("</script>" & vbCrLf)

            'スクリプト登録
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "footerOpen", sb.ToString)

        End Sub
#End Region

#Region "BackButton_Click"
        ''' <summary>
        ''' 戻るボタンがクリックされた際の処理を行います。
        ''' </summary>
        ''' <param name="sender">イベント発生元</param>
        ''' <param name="e">イベントデータ</param>
        ''' <remarks></remarks>
        Protected Sub BackButton_Click(ByVal sender As Object, ByVal e As EventArgs)

            'Rewindingイベント呼び出し
            Dim eventArgs As New CancelEventArgs()
            RaiseEvent Rewinding(Me, eventArgs)
            If (eventArgs.Cancel) Then
                Return
            End If

            '画面遷移履歴Listを取得
            Dim siteMapNodeList As List(Of SerializableSiteMapNode) = _
                HistorySiteMapProvider.SiteMapNodeList

            Dim navigateUrl As String = Nothing
            If siteMapNodeList.Count <= 1 Then
                ''履歴が1つの時戻れるのはメインメニューのみ
                navigateUrl = ResolveUrl("~/Pages/" & CStr(Session(C_SESSION_TOPPAGE)) & ".aspx")
            Else
                '１つ前の位置の履歴を取得
                Dim backSiteNode As SerializableSiteMapNode = siteMapNodeList(siteMapNodeList.Count - 2)
                navigateUrl = backSiteNode.Url()
            End If

            '画面遷移履歴Listの最後尾の履歴を削除
            If (0 < siteMapNodeList.Count) Then
                siteMapNodeList.RemoveAt(siteMapNodeList.Count - 1)
            End If

            'Sessionより次画面引渡しHashTableを削除
            Session.Remove(HistorySiteMapProvider.SESSION_KEY_NEXT_PAGE_INFO)

            'リダイレクトする
            Logger.Debug(String.Format(CultureInfo.InvariantCulture, "CommonMasterPage.BackButton_Click: {0}", navigateUrl))
            Response.Redirect(navigateUrl)

        End Sub
#End Region

#Region "LogoutButton_Click"
        ''' <summary>
        ''' ログアウト処理を行います。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub LogoutButton_Click(ByVal sender As Object, ByVal e As EventArgs)

            'Logoutイベント呼び出し
            Dim eventArgs As New CancelEventArgs()
            RaiseEvent Logout(Me, eventArgs)
            If (eventArgs.Cancel) Then
                Return
            End If

            Session(C_SESSION_TOPPAGE) = Nothing
            FormsAuthentication.SignOut()
            Session.Abandon()
            Response.Redirect(ResolveUrl(EnvironmentSetting.LoginUrl))

        End Sub
#End Region

        ''' <summary>
        ''' 指定したヘッダーボタンの情報を取得します。
        ''' </summary>
        ''' <param name="headerButton">ボタンＩＤ</param>
        ''' <returns>ヘッダーボタンのインスタンス</returns>
        ''' <remarks></remarks>
        Public Function GetHeaderButton(ByVal headerButton As HeaderButton) As CommonMasterHeaderButton
            If (_headerButtons.ContainsKey(headerButton)) Then
                Return _headerButtons(headerButton)
            Else
                Return Nothing
            End If
        End Function

#Region "GetFooterButton"
        ''' <summary>
        ''' 指定したボタンIDのボタン情報を取得。
        ''' </summary>
        ''' <param name="footerButtonId">ボタンID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFooterButton(ByVal footerButtonId As Integer) As CommonMasterFooterButton
            If (_footButtons.ContainsKey(footerButtonId)) Then
                Return _footButtons(footerButtonId)
            Else
                Return Nothing
            End If
        End Function
#End Region

#Region "ContextMenu"

        Public ReadOnly Property ContextMenu As CommonMasterContextMenu
            Get
                Return _contextMenu
            End Get
        End Property

#End Region

#Region "SearchBox"

        Public ReadOnly Property SearchBox As CommonMasterSearchBox
            Get
                Return _searchBox
            End Get
        End Property

#End Region

#Region "_setFooterStatus"
        ''' <summary>
        ''' フッターのステータス反映
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _setFooterStatus()

            Dim target As CustomHyperLink = Nothing
            For Each footButton In _footButtons
                target = footButton.Value.Owner
                target.Visible = footButton.Value.Visible
                If (target.CssClass.Equals("mstpg-selected")) Then
                    target.IconUrl = C_MSTPG_FOTTER_ICON_PATH & footButton.Key & C_MSTPG_FOOTER_ICON_ON & C_MSTPG_FOTTER_ICON_EXT
                ElseIf (target.Enabled = False) Then
                    target.IconUrl = C_MSTPG_FOTTER_ICON_PATH & footButton.Key & C_MSTPG_FOOTER_ICON_DISABLED & C_MSTPG_FOTTER_ICON_EXT
                Else
                    target.IconUrl = C_MSTPG_FOTTER_ICON_PATH & footButton.Key & C_MSTPG_FOTTER_ICON_EXT
                End If
            Next

        End Sub
#End Region

#Region "_SetSalesStatus"

        ''' <summary>
        ''' 商談ステータスをHedden項目にセットする
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetSalesStatus()

            Dim staffInfo As StaffContext = StaffContext.Current

            CType(Me.FindControl("MstPG_PresenceCategory"), HiddenField).Value = staffInfo.PresenceCategory
            CType(Me.FindControl("MstPG_PresenceDetail"), HiddenField).Value = staffInfo.PresenceDetail

        End Sub

#End Region

#Region "MstPG_CustomerSearchButton_Click"
        ''' <summary>
        ''' 顧客検索ボタンのクリックイベントです。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub MstPG_CustomerSearchButton_Click(ByVal sender As Object, e As EventArgs)
            Dim searchString As String = CType(Me.FindControl("MstPG_CustomerSearchTextBox"), TextBox).Text.Trim()

            Dim salesSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton"), SegmentedButton)
            Dim serviceSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton_Service"), SegmentedButton)

            Dim searchType As Integer = 0
            If salesSearchTypes.Visible Then
                searchType = CInt(salesSearchTypes.SelectedValue)
            Else
                searchType = CInt(serviceSearchTypes.SelectedValue)
            End If
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
            Dim chipType As Integer = 0
            Dim serviceSMBSearchTypes As SegmentedButton = CType(Me.FindControl("MstPG_SearchTypeSegmenteButton_SMB"), SegmentedButton)
            If serviceSMBSearchTypes.Visible Then
                chipType = CInt(serviceSMBSearchTypes.SelectedValue)
            Else
                chipType = 1
            End If
            '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END

            If (Not String.IsNullOrEmpty(searchString)) Then
                'Forwardingイベント呼び出し
                Dim eventArgs As New CancelEventArgs()
                RaiseEvent Forwarding(Me, eventArgs)
                If (eventArgs.Cancel) Then
                    Return
                End If

                'HttpContext.Current.Session(C_SESSION_CUSTOMERSEARCH_TYPE) = searchType
                'HttpContext.Current.Session(C_SESSION_CUSTOMERSEARCH_VALUE) = searchString

                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） START
                'CType(Me.Page, BasePage).CustomerSearch_Click(searchString, searchType)
                CType(Me.Page, BasePage).CustomerSearch_Click(searchString, searchType, chipType)
                '2013/06/03 TMEJ 小澤 【A.STEP2】i-CROP業務機能要件定義（既存流用機能） END
            End If
        End Sub
#End Region

        Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
            _SetPrevButtonStatus()

            Dim staff As StaffContext = StaffContext.Current()

            If VersionInformation.IsEqualOrLaterThan(1, 2, 0) Then
                'i-CROPアイコンの状態を更新
                Dim icropIcon As ImageButton = CType(Me.FindControl("MstPG_IcropIcon"), ImageButton)
                icropIcon.ImageUrl = ResolveClientUrl(String.Format(CultureInfo.InvariantCulture, "~/Styles/Images/MasterPage/IcropIcon_{0}.png", staff.PresenceCategory))
            End If

            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
            ''TCVボタンの蓋閉め(1.0.0)
            'Dim version As New VersionInformation()
            'If (version.MajorVersion = 1 AndAlso version.MinorVersion = 0 AndAlso version.Revision = 0) Then
            '    _footButtons(FooterMenuCategory.TCV).Visible = False
            'End If

            ''ショールームステータスボタンの非活性化(1.2.X)
            'If (Not VersionInformation.IsEqualOrLaterThan(1, 3, 0)) AndAlso _footButtons.ContainsKey(FooterMenuCategory.ShowRoomStatus) Then
            '    _footButtons(FooterMenuCategory.ShowRoomStatus).Enabled = (staff.OpeCD = Operation.SLR)
            'End If
            '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

            '未対応来店客アイコン
            Dim visitorButtonPanel As Panel = CType(Me.FindControl("visitorButtonPanel"), Panel)
            If VersionInformation.IsEqualOrLaterThan(1, 2, 0) AndAlso Not _CheckSalesService() Then
                If ((staff.OpeCD = Operation.SLR) OrElse (staff.OpeCD = Operation.BM) OrElse (staff.OpeCD = Operation.SSM)) Then
                    '未対応来店客アイコン非表示
                    visitorButtonPanel.Visible = False
                Else
                    '未対応来店客アイコン表示
                    visitorButtonPanel.Visible = True
                    '未対応来店客アイコンを更新
                    _SetVisitor()
                End If
            Else
                '未対応来店客アイコン非表示
                visitorButtonPanel.Visible = False
            End If

            '通知アイコン
            Dim forumButtonPanel As Panel = CType(Me.FindControl("forumButtonPanel"), Panel)
            If VersionInformation.IsEqualOrLaterThan(1, 1, 0) Then
                If ((staff.OpeCD = Operation.BM) OrElse (staff.OpeCD = Operation.SSM)) Then
                    '通知アイコン非表示＆管理者用通知一覧表示
                    forumButtonPanel.Visible = False
                    Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "noticeListFrame", "<script type='text/javascript'>$(function(){{ $('body').append(""<div id='noticeListFrame' name='noticeListFrame'><iframe id='noticeListiFrame' name='noticeListiFrame' src='SC3040802.aspx'></iframe><div>""); }});</script>")
                ElseIf (staff.OpeCD = Operation.SLR) Then
                    '通知アイコン非表示
                    forumButtonPanel.Visible = False
                Else
                    '通知アイコン表示
                    forumButtonPanel.Visible = True
                    'ヘッダ部の通知アイコンを更新
                    _SetNotice()
                End If
            Else
                forumButtonPanel.Visible = False
            End If

            'コンテキストメニュー
            _SetContextMenu()

            '操作ロック制御
            If (Me.OperationLocked.Value.Equals("1")) Then
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "freezeHeaderOperation", "$(function() { freezeHeaderOperation(); });", True)
            End If

            'フッター
            _setFooterStatus()


            '商談ステータスをHedden項目にセットする
            _SetSalesStatus()

        End Sub

        ''' <summary>
        ''' コンテキストメニュー設定
        ''' </summary>
        ''' <remarks>コンテキストメニュー設定</remarks>
        Private Sub _SetContextMenu()

            Dim staff As StaffContext = StaffContext.Current()

            'コンテキストメニュー
            _contextMenu.Owner.OnClientOpen = ""
            _contextMenu.Owner.OnClientClose = ""
            If (_contextMenu.AutoPostBack) Then
                If (_contextMenu.UseAutoOpening) Then
                    _contextMenu.Owner.OnClientClose = String.Format(CultureInfo.InvariantCulture, "function () {{ {0}; }}", Me.Page.ClientScript.GetPostBackEventReference(Me, C_EVENT_CONTEXTMENU_CLOSE))
                Else
                    _contextMenu.Owner.OnClientOpen = String.Format(CultureInfo.InvariantCulture, "function () {{ {0}; }}", Me.Page.ClientScript.GetPostBackEventReference(Me, C_EVENT_CONTEXTMENU_OPEN))
                End If
            End If
            If (_contextMenu.UseAutoOpening) Then
                Me.Page.ClientScript.RegisterStartupScript(Me.GetType(), "OpenContextMenuOnStartup", "$(function() { setTimeout(function() { $('#" & _contextMenu.Owner.TriggerClientId & "').trigger('showPopover'); }, 500); });", True)
            End If

            'コンテキストメニュー項目
            Dim showSuspendItem As Boolean = False
            Dim showStandByItem As Boolean = False
            Dim showLogoutItem As Boolean = False
            '2012/07/06 KN 小澤 STEP2対応 START
            'If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
            '    If ((staff.OpeCD = Operation.BM) OrElse (staff.OpeCD = Operation.SSM) OrElse (staff.OpeCD = Operation.SLR) OrElse (_CheckSalesService())) Then
            '        'サービス、マネージャ、受付はステータスを使用しない
            '        showSuspendItem = False
            '        showStandByItem = False
            '        showLogoutItem = True
            '    ElseIf staff.PresenceCategory.Equals("1") AndAlso staff.PresenceDetail.Equals("0") Then
            '        showSuspendItem = True
            '        showStandByItem = False
            '        showLogoutItem = True
            '    ElseIf staff.PresenceCategory.Equals("3") AndAlso staff.PresenceDetail.Equals("0") Then
            '        showSuspendItem = False
            '        showStandByItem = True
            '        showLogoutItem = True
            '    End If
            'Else
            '    showSuspendItem = False
            '    showStandByItem = False
            '    showLogoutItem = True
            'End If
            '2013/03/07 TMEJ 成澤 IT7683_【A.STEP1】 START
            If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
                If ((staff.OpeCD = Operation.BM) OrElse (staff.OpeCD = Operation.SSM) OrElse
                    (staff.OpeCD = Operation.SLR) OrElse (staff.OpeCD = Operation.TEC) OrElse
                    (staff.OpeCD = Operation.PS) OrElse (staff.OpeCD = Operation.SM) OrElse
                    (staff.OpeCD = Operation.SVR)) Then
                    'マネージャ、受付、テクニシャン、パーツスタッフ、サービスマネージャーはステータスを使用しない
                    showSuspendItem = False
                    showStandByItem = False
                    showLogoutItem = True
                ElseIf staff.PresenceCategory.Equals("1") AndAlso staff.PresenceDetail.Equals("0") Then
                    showSuspendItem = True
                    showStandByItem = False
                    showLogoutItem = True
                ElseIf staff.PresenceCategory.Equals("3") AndAlso staff.PresenceDetail.Equals("0") Then
                    showSuspendItem = False
                    showStandByItem = True
                    showLogoutItem = True
                    'サービスユーザーで退席中の場合はコンテキストメニューを表示する
                    If _CheckSalesService() Then
                        ScriptManager.RegisterStartupScript(Me, _
                                                            Me.GetType, _
                                                            "openContextMenu", _
                                                            "setTimeout(function () { $('#MstPG_IcropIcon').click(); }, 1000);", _
                                                            True)
                    End If
                End If
            Else
                showSuspendItem = False
                showStandByItem = False
                showLogoutItem = True
            End If
            '2012/07/06 KN 小澤 STEP2対応 END
            '2013/03/07 TMEJ 成澤 IT7683_【A.STEP1】 END

            For Each item As CommonMasterContextMenuItem In _contextMenu.ContextMenuItems.Values
                'アイコン設定
                If (VersionInformation.IsEqualOrLaterThan(1, 2, 0)) Then
                    item.Owner.Style("background-image") = ResolveClientUrl(String.Format(CultureInfo.InvariantCulture, "~/Styles/Images/MasterPage/Presence_{0}.png", item.PresenceCategory))
                End If

                '組み込みコンテキストメニュー項目の表示／非表示制御
                Select Case item.ID
                    Case CommonMasterContextMenuBuiltinMenuID.StandByItem
                        item.Visible = showStandByItem
                    Case CommonMasterContextMenuBuiltinMenuID.SuspendItem
                        item.Visible = showSuspendItem
                    Case CommonMasterContextMenuBuiltinMenuID.LogoutItem
                        item.Visible = showLogoutItem
                End Select
            Next

        End Sub

        ''' <summary>
        ''' 戻るボタン処理
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub _SetPrevButtonStatus()
            '画面遷移履歴Listを取得
            Dim siteMapNodeList As List(Of SerializableSiteMapNode) = HistorySiteMapProvider.SiteMapNodeList
            If siteMapNodeList.Count < 1 Then
                '先頭画面(メニュー)のため、戻るボタン非表示
                Me.FindControl("MstPG_BackLinkButton").Visible = False
            Else
                '画面設定に合わせボタンを表示
                Me.FindControl("MstPG_BackLinkButton").Visible = IsRewindButtonEnabled
            End If
        End Sub

        ''' <summary>
        ''' セールスユーザかサービスユーザか判断
        ''' </summary>
        ''' <returns>サービス:true セールス:false</returns>
        ''' <remarks></remarks>
        Public Function _CheckSalesService() As Boolean
            If (Not VersionInformation.IsEqualOrLaterThan(1, 1, 0)) Then
                Return False
            End If

            Dim val As String = _GetConfigValue(SystemConfiguration.Current.Manager.StaffDivision)
            If (val.Equals("Service")) Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' 検索を表示するか判断
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function _DispSearch() As Boolean
            Dim val As String = _GetConfigValue(SystemConfiguration.Current.Manager.Individual)

            If Not (val.Equals("NonSerch")) Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' web.configの指定ノードの値を取得
        ''' </summary>
        ''' <param name="config">Configuration.ClassSection</param>
        ''' <returns>取得した値</returns>
        ''' <remarks></remarks>
        Private Function _GetConfigValue(ByVal config As Configuration.ClassSection) As String
            Dim rntVal As String = String.Empty
            Dim staff As StaffContext = StaffContext.Current

            If config IsNot Nothing Then
                Dim setting As Configuration.Setting = config.GetSetting(String.Empty)
                If (setting IsNot Nothing) Then
                    rntVal = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
                End If
            End If

            If rntVal Is Nothing Then
                rntVal = String.Empty
            End If

            Return rntVal
        End Function

        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 START
#Region "MstPG_RedirectNextScreenButton_Click"
        ''' <summary>
        ''' 次画面表示の隠しボタンイベントで、
        ''' 現地用イベントです。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub RedirectNextScreenButton_Click(ByVal sender As Object, e As EventArgs)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '画面遷移先のID取得
            Dim nextProgramId As String = CType(Me.FindControl("MstPG_RedirectProgramId"), HiddenField).Value.Trim()
            'Sessionキー取得
            Dim nextSessionKey As String = CType(Me.FindControl("MstPG_RedirectSessionKey"), HiddenField).Value.Trim()
            'Sessionデータ取得
            Dim nextSessionData As String = CType(Me.FindControl("MstPG_RedirectSessionData"), HiddenField).Value.Trim()

            '画面遷移先プログラムIDデータ確認
            If (Not String.IsNullOrEmpty(nextProgramId)) Then
                'データが存在する場合
                'Forwardingイベント呼び出し確認
                Dim eventArgs As New CancelEventArgs()
                RaiseEvent Forwarding(Me, eventArgs)
                If (eventArgs.Cancel) Then
                    Return
                End If

                '画面遷移処理
                CType(Me.Page, BasePage).RedirectNextScreenButton_Click(nextProgramId, nextSessionKey, nextSessionData)
            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End Sub
#End Region
        '2013/12/16 TMEJ 小澤 次世代サービス 工程管理機能開発 END

    End Class
#End If

End Namespace