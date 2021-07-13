'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290101.ascx.vb
'─────────────────────────────────────
'機能： 異常リスト
'補足： 
'作成： 2014/06/13 TMEJ y.gotoh
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports Toyota.eCRB.SalesManager.IrregularControl.BizLogic


''' <summary>
''' 異常リスト
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3290101_Control
    Inherits System.Web.UI.UserControl

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayId As String = "SC3290101"

    ''' <summary>
    ''' 文言：異常リスト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoIrregularityList As String = "1"

    ''' <summary>
    ''' 文言：異常項目
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoIrregularityItem As String = "2"

    ''' <summary>
    ''' 文言：スタッフ数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoStaffs As String = "3"

    ''' <summary>
    ''' 文言：異常数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoIrregularities As String = "4"

    ''' <summary>
    ''' 文言：※
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoMarker As String = "5"

    ''' <summary>
    ''' 文言：更新日時：
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoLastUpdate As String = "6"

    ''' <summary>
    ''' 文言：異常項目はありません
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DisplayNoThereIsNoIrregularity As String = "7"

#End Region

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("SC3290101_Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then
            Logger.Info("SC3290101_Page_Load_End PostBack")
            Return
        End If

        ' 文言設定
        Me.SetWord()

        Logger.Info("SC3290101_Page_Load_End")
    End Sub

#Region "非公開メソッド"

    ''' <summary>
    ''' 文言をセットする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SC3290101_SetWord_Start")

        Me.SC3290101_Title.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoIrregularityList))
        Me.SC3290101_IrregularityItemTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoIrregularityItem))
        Me.SC3290101_NoOfStaffsTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoStaffs))
        Me.SC3290101_NoOfIrregularitiesTitle.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoIrregularities))

        Dim lastUpdateText As New StringBuilder
        With lastUpdateText
            .Append(WebWordUtility.GetWord(DisplayId, DisplayNoMarker))
            .Append(" ")
            .Append(WebWordUtility.GetWord(DisplayId, DisplayNoLastUpdate))
        End With
        Me.SC3290101_LastUpdateText.Text = Server.HtmlEncode(lastUpdateText.ToString)
        Me.SC3290101_ItemNotingLabel.Text = Server.HtmlEncode(WebWordUtility.GetWord(DisplayId, DisplayNoThereIsNoIrregularity))

        Logger.Info("SC3290101_SetWord_End")
    End Sub
#End Region

End Class
