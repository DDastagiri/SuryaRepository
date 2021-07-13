'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3050704.aspx.vb
'─────────────────────────────────────
'機能： MOP/DOP設定設定
'補足： 
'作成： 2012/12/06 TMEJ 三和
'更新： 
'─────────────────────────────────────

Option Strict On

Imports System.Globalization
Imports Toyota.eCRB.Common
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class Pages_SC3050704
    Inherits BasePage

#Region " ページロード "

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'ヘッダー制御
        InitHeaderEvent()
        'フッター制御
        InitFooterEvent()

    End Sub

#End Region

#Region " フッター制御・ヘッダー制御 "

    'メニューのＩＤを定義
    Private Const MAIN_MENU As Integer = 100
    Private Const TCV_SETTING As Integer = 1300
    Private Const SUBMENU_CONTENTS_MENU As Integer = 1301
    Private Const SUBMENU_SALES_POINT As Integer = 1302

    ''' <summary>
    ''' マスタページ
    ''' </summary>
    ''' <remarks></remarks>
    Private localCommonMaster As CommonMasterPage

    ''' <summary>
    ''' フッター作成
    ''' </summary>
    ''' <param name="commonMaster"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        Me.localCommonMaster = commonMaster
        category = FooterMenuCategory.TCVSetting

        Return {SUBMENU_CONTENTS_MENU, SUBMENU_SALES_POINT}

    End Function

    ''' <summary>
    ''' コンテキストメニュー作成
    ''' </summary>
    ''' <param name="commonMaster">マスタページ</param>
    ''' <returns>表示内容</returns>
    ''' <remarks>コンテキストメニューの作成</remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()
        Return New Integer() {CommonMasterContextMenuBuiltinMenuID.LogoutItem}

    End Function

    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()
        '戻るボタンを活性()
        CType(Master, CommonMasterPage).IsRewindButtonEnabled = True

        'カスタマーサーチを非活性
        CType(Master, CommonMasterPage).SearchBox.Enabled = False

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'ボタン非表示
        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU).Visible = False
        End If

        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT).Visible = False
        End If

        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer).Visible = False
        End If

        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus).Visible = False
        End If

        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV).Visible = False
        End If

        'メニュー
        AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Click, _
            Sub()
                'メニューに遷移
                Me.RedirectNextScreen("SC3010203")
            End Sub

    End Sub

    ''' <summary>
    ''' フレンダリング前最終イベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        'TCV設定ボタンを表示
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting).Visible = True
        End If

    End Sub

#End Region

End Class
