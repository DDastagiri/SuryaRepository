'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080215.ascx.vb
'─────────────────────────────────────
'機能： CSSurvey一覧・詳細
'補足： 
'作成： 2012/02/20 TCS 明瀬
'更新： 2012/04/13 TCS 明瀬 HTMLエンコード対応
'更新： 2013/06/30 TCS 坂井 2013/10対応版 既存流用
'更新： 2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移)
'更新： 2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応)
'更新： 2016/09/14 TCS 河原 TMTタブレット性能改善
'─────────────────────────────────────

Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Data

''' <summary>
''' CSSurvey一覧・詳細
''' プレゼンテーションクラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3080215
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3080215"

    ''' <summary>
    ''' 自画面のプログラムファイル名（ログ用）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMFILE As String = "SC3080215.ascx "

    ''' <summary>
    ''' Style　「background」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STYLE_BACKGROUND As String = "background"

    ''' <summary>
    ''' 画像ベースURL　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BASEURL_IMGAUTH As String = "url(../Styles/Images/Authority/"

    ''' <summary>
    ''' アンケート詳細データテーブルの親子関係名称　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DETAIL_RELATIONNAME As String = "DetailRelation"

    ''' <summary>
    ''' アンケート種別 - 顧客アンケート
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CSSURVEYTYPE_CST As String = "0"

    ''' <summary>
    ''' アンケート種別 - 車両アンケート
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CSSURVEYTYPE_VCL As String = "1"

    ''' <summary>
    ''' 回答タイプ - ラジオボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERTYPE_RADIO As String = "0"

    ''' <summary>
    ''' 回答タイプ - コンボボックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERTYPE_COMBO As String = "1"

    ''' <summary>
    ''' 回答タイプ - チェックボックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERTYPE_CHECK As String = "2"

    ''' <summary>
    ''' 回答タイプ - テキストボックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERTYPE_TEXT As String = "3"

    ''' <summary>
    ''' 回答結果 - 未選択
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERRESULT_NOSELECT As String = "0"

    ''' <summary>
    ''' 回答結果 - 選択
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERRESULT_SELECT As String = "1"

    ''' <summary>
    ''' クライアントから要求したメソッド名 - アンケート一覧画面作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Const METHOD_CREATELIST As String = "CreateCSSurveyList"

    ''' <summary>
    ''' クライアントから要求したメソッド名 - アンケート詳細画面作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Const METHOD_CREATEDETAIL As String = "CreateCSSurveyDetail"

    ''' <summary>
    ''' HTMLタグ - 改行タグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HTMLTAG_BR As String = "<br>"

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 顧客コード(未取引客の時：未取引客ID, 自社客の時：自社客連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private _crcustId As String

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 DEL START
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 DEL END

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private _cstKind As String

    ''' <summary>
    ''' 顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private _customerClass As String

    ''' <summary>
    ''' 回答ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _answerID As String

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _dlrCD As String

    ''' <summary>
    ''' 表示データ取得件数
    ''' </summary>
    ''' <remarks></remarks>
    Private _csSurveyCount As Integer

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private CallBackResult As String

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 顧客コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CrcustId() As String
        Get
            Return _crcustId
        End Get
        Set(value As String)
            _crcustId = value
        End Set
    End Property

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 DEL START
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 DEL END

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstKind() As String
        Get
            Return _cstKind
        End Get
        Set(value As String)
            _cstKind = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客分類
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerClass() As String
        Get
            Return _customerClass
        End Get
        Set(value As String)
            _customerClass = value
        End Set
    End Property

    ''' <summary>
    ''' 回答ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AnswerId() As String
        Get
            Return _answerID
        End Get
        Set(value As String)
            _answerID = value
        End Set
    End Property

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DlrCD() As String
        Get
            Return _dlrCD
        End Get
        Set(value As String)
            _dlrCD = value
        End Set
    End Property

    ''' <summary>
    ''' 表示データ取得件数
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CSSurveyCount() As Integer
        Get
            Return _csSurveyCount
        End Get
        Set(value As Integer)
            _csSurveyCount = value
        End Set
    End Property

    ''' <summary>
    ''' ポップアップ表示のトリガーボタンID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TriggerClientId() As String
        Get
            Return Me.CSSurveyPopOverForm.Attributes("data-TriggerClientID")
        End Get
        Set(ByVal value As String)
            Me.CSSurveyPopOverForm.Attributes("data-TriggerClientID") = value
        End Set
    End Property

#End Region

#Region "列挙体"
    ''' <summary>
    ''' 列挙体　CSSurveyページ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum CSSurveyPage As Integer

        ''' <summary>アンケート一覧</summary>
        List = 1
        ''' <summary>アンケート詳細</summary>
        Detail = 2
        ''' <summary>アンケート一覧・詳細</summary>
        All = 3

    End Enum
#End Region

#Region "イベント処理メソッド"
    ''' <summary>
    ''' ロードの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2016/09/14 TCS 河原 TMTタブレット性能改善 START
        If Me.Page.IsCallback AndAlso Me.Page.IsPostBack Then
            Return
        End If
        '2016/09/14 TCS 河原 TMTタブレット性能改善 END

        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
        If ContainsKey(ScreenPos.Current, "StartPageId") Then
            If DirectCast(GetValue(ScreenPos.Current, "StartPageId", False), String).Equals("SC3070201") Then
                Exit Sub
            End If
        End If
        '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

        '2012/04/05 TCS 明瀬 JavaScriptエラー対応 Start
        If Me.Visible Then
            '2012/04/05 TCS 明瀬 JavaScriptエラー対応 End

            'コールバックスクリプトの生成
            ScriptManager.RegisterStartupScript(
                Me,
                Me.GetType(),
                "CallbackSC3080215",
                String.Format(CultureInfo.InvariantCulture,
                              "CallbackSC3080215.beginCallback = function () {{ {0}; }};",
                              Page.ClientScript.GetCallbackEventReference(Me, "CallbackSC3080215.packedArgument", "CallbackSC3080215.endCallback", "", False)
                              ),
                True
            )

            If Not Page.IsPostBack Then
                '処理なし
            End If

            Dim bizLogic As New SC3080215BusinessLogic

            '本画面のアンケート一覧件数プロパティに取得件数を設定(０件の場合、顧客詳細画面のCSSurveyボタンが非活性になる)
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            Me.CSSurveyCount = bizLogic.GetCSQuestionListCount(Me.CrcustId)
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            'ポップオーバーフォームの紐付けをページロード後に行う
            Dim script As New StringBuilder
            script.Append("popOverFormSC3080215();")

            '回答IDが渡されていた場合は、ページロード後にトリガーのボタンをクリック
            If Not String.IsNullOrEmpty(Me.AnswerId) AndAlso String.IsNullOrEmpty(Me.answerIdHidden.Value) Then
                Me.answerIdHidden.Value = Me.AnswerId
                script.Append("$('#")
                script.Append(Me.TriggerClientId)
                script.Append("').click();")
            End If

            'スクリプトの登録
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DirectDetail", script.ToString(), True)

            '2012/04/05 TCS 明瀬 JavaScriptエラー対応 Start
        End If
        '2012/04/05 TCS 明瀬 JavaScriptエラー対応 End

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' アンケート一覧リピーターの行バインド時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CSSurveyListRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles CSSurveyListRepeater.ItemDataBound

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        If e.Item.ItemType = ListItemType.Item _
        OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            'リピータコントロールの取得
            Dim surveyNameBox As HtmlGenericControl = DirectCast(e.Item.FindControl("surveyNameBox"), HtmlGenericControl)
            Dim vehicleBox As HtmlGenericControl = DirectCast(e.Item.FindControl("vehicleBox"), HtmlGenericControl)
            Dim updateAccountName As HtmlGenericControl = DirectCast(e.Item.FindControl("updateAccountName"), HtmlGenericControl)

            'データバインドされたデータテーブルの行情報を取得する
            Dim row As SC3080215DataSet.SC3080215DisplayListRow = DirectCast(e.Item.DataItem.row, SC3080215DataSet.SC3080215DisplayListRow)

            If row.PAPERTYPE.Equals(CSSURVEYTYPE_CST) Then
                '***************************************************
                '* アンケート種別：０(顧客)
                '***************************************************
                surveyNameBox.Style.Add(HtmlTextWriterStyle.Width, "285px")
                vehicleBox.Visible = False
            Else
                '***************************************************
                '* アンケート種別：１(車両)
                '***************************************************
                surveyNameBox.Style.Add(HtmlTextWriterStyle.Width, "130px")
            End If

            'スタッフのアイコン設定
            updateAccountName.Style.Add(STYLE_BACKGROUND, BASEURL_IMGAUTH & row.ICONFILENAME & ") left top no-repeat")
        End If

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' アンケート詳細親リピーターの行バインド時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CSSurveyDetailRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles CSSurveyDetailRepeater.ItemDataBound

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        If e.Item.ItemType = ListItemType.Item _
        OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            'リピータコントロールの取得
            Dim questionTable As HtmlTable = DirectCast(e.Item.FindControl("questionTable"), HtmlTable)
            Dim answerTable As HtmlTable = DirectCast(e.Item.FindControl("answerTable"), HtmlTable)
            '2012/04/13 TCS 明瀬 HTMLエンコード対応 Start
            Dim questionText As HtmlGenericControl = DirectCast(e.Item.FindControl("questionText"), HtmlGenericControl)
            '2012/04/13 TCS 明瀬 HTMLエンコード対応 End

            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            'テーブルの開始行スタイルを設定
            If e.Item.ItemIndex = 0 Then
                questionTable.Style.Add("border-top-left-radius", "5px")
                questionTable.Style.Add("border-top-right-radius", "5px")
            End If

            'テーブルの最終行スタイルを設定
            If view.Count - 1 = e.Item.ItemIndex Then
                answerTable.Style.Add("border-bottom", "1px solid #BBB")
                answerTable.Style.Add("border-bottom-left-radius", "5px")
                answerTable.Style.Add("border-bottom-right-radius", "5px")
            End If

            '2012/04/13 TCS 明瀬 HTMLエンコード対応 Start
            questionText.InnerHtml = questionText.InnerHtml.Replace(vbCrLf, HTMLTAG_BR)
            '2012/04/13 TCS 明瀬 HTMLエンコード対応 End

        End If

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' アンケート詳細子リピーターの行バインド時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>このイベントメソッドは、子リピーターのOnItemDataBound属性に記述したメソッドです。
    ''' リピーターに入れ子になったリピーターは、通常イベントが取得できないため、この方法でイベントを取得します。
    ''' </remarks>
    Protected Sub CSSurveyDetailAnswerRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        If e.Item.ItemType = ListItemType.Item _
        OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            'リピータコントロールの取得
            Dim answerText As HtmlGenericControl = DirectCast(e.Item.FindControl("answerText"), HtmlGenericControl)
            Dim answerContentBox As HtmlGenericControl = DirectCast(e.Item.FindControl("answerContentBox"), HtmlGenericControl)

            'データバインドされた子データテーブルの行情報を取得する
            Dim row As SC3080215DataSet.SC3080215DetailChildRow = DirectCast(e.Item.DataItem.row, SC3080215DataSet.SC3080215DetailChildRow)

            If row.ANSWERTYPE.Equals(ANSWERTYPE_RADIO) OrElse row.ANSWERTYPE.Equals(ANSWERTYPE_CHECK) Then
                '***************************************************
                '* 回答タイプ：０(ラジオボタン)、２(チェックボックス)
                '***************************************************
                answerText.Visible = False
                answerContentBox.Visible = True

                If row.CHECKRESULT.Equals(ANSWERRESULT_SELECT) Then
                    Me.AddCssClass(answerContentBox, "On")
                End If

            ElseIf row.ANSWERTYPE.Equals(ANSWERTYPE_COMBO) Then
                '***************************************************
                '* 回答タイプ：１(コンボボックス)
                '***************************************************
                answerText.Visible = True
                answerContentBox.Visible = False
                Me.AddCssClass(answerText, "CSSurveyEllipsis")
            Else
                '***************************************************
                '* 回答タイプ：３(テキスト)
                '***************************************************
                answerText.Visible = True
                answerContentBox.Visible = False

                If row.ANSWERCOUNT = 1 Then
                    '単一行の回答入力
                    Me.AddCssClass(answerText, "CSSurveyEllipsis")
                Else
                    '複数行の回答入力
                    Me.AddCssClass(answerText, "CSSurveyWrap")
                    answerText.InnerHtml = answerText.InnerHtml.Replace(vbCrLf, HTMLTAG_BR)
                End If

            End If

        End If

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub
#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' コールバック用文字列を返却します。
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name)

        Return Me.CallBackResult

    End Function

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument"></param>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/13 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Try
            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                        "_Start[eventArgument:{0}]", eventArgument))

            '2012/04/13 TCS 明瀬 HTMLエンコード対応 Start
            'Hiddenに文言格納「CS Survey」
            Me.SC3080215Word0001Hidden.Value = WebWordUtility.GetWord(MY_PROGRAMID, 1)
            '2012/04/13 TCS 明瀬 HTMLエンコード対応 End

            'コールバック呼び出し元に返却する文字列
            Dim resultString As String = String.Empty

            'イベントパラメータを配列に格納
            Dim tokens As String() = eventArgument.Split(New Char() {","c})

            '呼び出しメソッド名の取得
            Dim method As String = tokens(0)

            '作成するページの種類
            Dim pageType As Pages_SC3080215.CSSurveyPage = 0

            '呼び出しメソッドの判定
            If method.Equals(METHOD_CREATELIST) Then

                'アンケート一覧画面
                pageType = CSSurveyPage.List

            ElseIf method.Equals(METHOD_CREATEDETAIL) Then

                Me.answerIdHidden.Value = HttpUtility.UrlDecode(tokens(1))      '回答ID
                Me.paperNameHidden.Value = HttpUtility.UrlDecode(tokens(2))     'アンケート用紙名
                Me.iconFileNameHidden.Value = HttpUtility.UrlDecode(tokens(3))  'スタッフアイコンファイル名
                Me.staffNameHidden.Value = HttpUtility.UrlDecode(tokens(4))     'スタッフ名称
                Me.seriesNameHidden.Value = HttpUtility.UrlDecode(tokens(5))    'シリーズ名称
                Me.vclRegNoHidden.Value = HttpUtility.UrlDecode(tokens(6))      '車両登録No.
                Me.dateWordHidden.Value = HttpUtility.UrlDecode(tokens(7))      '日付文言

                'アンケート詳細画面
                pageType = CSSurveyPage.Detail
            Else
                'アンケート一覧・詳細画面
                pageType = CSSurveyPage.All
            End If

            '画面の作成
            resultString = Me.GetMyDisplayCreateData(pageType)

            ' 処理結果をコールバック返却用文字列に設定
            Me.CallBackResult = HttpUtility.HtmlEncode(resultString)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_End[CallBackResult:{0}]", Me.CallBackResult))

        Catch ex As Exception
            ' エラーメッセージの設定
            Me.CallBackResult = String.Format(CultureInfo.InvariantCulture, "{0}|{1}",
                                              SC3080215BusinessLogic.MessageIdSys, ex.Message)

            Logger.Error(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_End[MessageID:{0}]", SC3080215BusinessLogic.MessageIdSys), ex)

        End Try

    End Sub

    ''' <summary>
    ''' 親子関係にあるDataviewの子を取得する
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="relName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChildView(item As Object, relName As String) As DataView

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[item:{0}][relName:{1}]", item, relName))

        Return CType(item, DataRowView).CreateChildView(relName)

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' セッション情報（親画面からセットされたプロパティ）を本画面のデータテーブル行にセットする
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValToDtRow() As SC3080215DataSet.SC3080215SessionRow

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        Using tbl As New SC3080215DataSet.SC3080215SessionDataTable

            Dim tblRow As SC3080215DataSet.SC3080215SessionRow = tbl.NewSC3080215SessionRow

            '顧客コード
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            tblRow.ORGCUSTID = Me.CrcustId
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            '顧客種別
            If Not String.IsNullOrEmpty(Me.CstKind) Then
                tblRow.CSTKIND = Me.CstKind
            End If

            '顧客分類
            If Not String.IsNullOrEmpty(Me.CustomerClass) Then
                tblRow.CUSTOMERCLASS = Me.CustomerClass
            End If

            '販売店コード
            If Not String.IsNullOrEmpty(Me.DlrCD) Then
                tblRow.DLRCD = Me.DlrCD
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                      "[ORGCUSTID:{0}][CSTKIND:{1}][CUSTOMERCLASS:{2}][DLRCD:{3}]", _
                                      tblRow.ORGCUSTID, tblRow.CSTKIND, tblRow.CUSTOMERCLASS, tblRow.DLRCD))

            Return tblRow

        End Using

    End Function

    ''' <summary>
    ''' 本画面のコントロールに文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/04/13 TCS 明瀬 HTMLエンコード対応
    ''' </History>
    Private Sub SetMyControlWord()

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        '2012/04/13 TCS 明瀬 HTMLエンコード対応 Start
        '第２ヘッダー　スタッフ名
        Me.updStaffNameLabel.Text = HttpUtility.HtmlEncode(Me.staffNameHidden.Value)
        '第２ヘッダー　シリーズ名
        Me.vehicleLabel.Text = HttpUtility.HtmlEncode(Me.seriesNameHidden.Value)
        '第２ヘッダー　車両登録No.
        Me.regNoLabel.Text = HttpUtility.HtmlEncode(Me.vclRegNoHidden.Value)
        '第２ヘッダー　日付文言
        Me.dateWordLabel.Text = HttpUtility.HtmlEncode(Me.dateWordHidden.Value)
        '2012/04/13 TCS 明瀬 HTMLエンコード対応 End

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' 画面を作成するために必要な情報を取得する
    ''' </summary>
    ''' <param name="pageType"></param>
    ''' <remarks></remarks>
    Private Function GetMyDisplayCreateData(ByVal pageType As CSSurveyPage) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                  "_Start[pageType:{0}]", pageType))

        'セッション情報（親画面に設定されたプロパティ値）をデータテーブル行に設定（回答IDは除く）
        Dim sessionRow As SC3080215DataSet.SC3080215SessionRow = Me.SetSessionValToDtRow()

        Dim bizLogic As New SC3080215BusinessLogic

        If pageType = CSSurveyPage.List OrElse pageType = CSSurveyPage.All Then
            '********************************************************
            '* ページ種別：１(アンケート一覧)、３(アンケート一覧・詳細)
            '********************************************************

            'CSアンケート一覧情報を取得する（必ず１件以上存在する）
            Dim displayListDt As SC3080215DataSet.SC3080215DisplayListDataTable = bizLogic.GetCSQuestionList(sessionRow)

            '取得件数をプロパティに設定
            Me.CSSurveyCount = displayListDt.Rows.Count

            'アンケート一覧リピータのデータ連結
            Me.CSSurveyListRepeater.DataSource = displayListDt
            Me.CSSurveyListRepeater.DataBind()

            If pageType = CSSurveyPage.All Then
                Dim row As SC3080215DataSet.SC3080215DisplayListRow = displayListDt.Rows.Find(Me.answerIdHidden.Value)

                'ここでデータ行が取得できていない場合、回答が消されているためエラーを発生させる
                If row Is Nothing Then
                    Throw New ApplicationException(WebWordUtility.GetWord(MY_PROGRAMID, 4))
                End If

                Me.answerIdHidden.Value = row.ANSWERID              '回答ID
                Me.paperNameHidden.Value = row.PAPERNAME            'アンケート用紙名
                Me.iconFileNameHidden.Value = row.ICONFILENAME      'スタッフアイコンファイル名
                Me.staffNameHidden.Value = row.STAFFNAME            'スタッフ名称
                Me.seriesNameHidden.Value = row.SERIESNAME          'シリーズ名称
                Me.vclRegNoHidden.Value = row.VCLREGNO              '車両登録No.
                Me.dateWordHidden.Value = row.DATEWORD              '日付文言
            End If
        End If

        If pageType = CSSurveyPage.Detail OrElse pageType = CSSurveyPage.All Then
            '********************************************************
            '* ページ種別：２(アンケート詳細)、３(アンケート一覧・詳細)
            '********************************************************

            'アンケート詳細のスタッフアイコンを設定
            Me.updStaffNameLabel.Style.Clear()
            Me.updStaffNameLabel.Style.Add(STYLE_BACKGROUND, BASEURL_IMGAUTH & Me.iconFileNameHidden.Value & ") left center no-repeat")

            '文言の設定
            SetMyControlWord()

            Dim answerId As Long = CLng(Me.answerIdHidden.Value)

            'CSアンケート詳細情報を取得する
            Dim displayDetailDs As SC3080215DataSet = bizLogic.GetCSQuestionDetail(answerId)

            Dim purentDt As SC3080215DataSet.SC3080215DetailPurentDataTable = displayDetailDs.SC3080215DetailPurent
            Dim childrenDt As SC3080215DataSet.SC3080215DetailChildDataTable = displayDetailDs.SC3080215DetailChild

            'リピーターの入れ子処理のためにリレーションを設定する
            purentDt.ChildRelations.Add(DETAIL_RELATIONNAME, purentDt.CONTENTIDColumn, childrenDt.CONTENTIDColumn)

            'アンケート詳細リピータのデータ連結(親テーブルで連結)
            Me.CSSurveyDetailRepeater.DataSource = purentDt
            Me.CSSurveyDetailRepeater.DataBind()

        End If

        ' 上記で作成した画面のHTMLを返却する
        Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

            Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
            Me.RenderControl(writer)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_End[GetStringBuilder:{0}]", sw.GetStringBuilder().ToString))

            Return sw.GetStringBuilder().ToString
        End Using

    End Function

    ''' <summary>
    ''' コントロールに指定したCSSクラスを追加する
    ''' </summary>
    ''' <param name="element">コントロールオブジェクト</param>
    ''' <param name="cssClass">CSSクラス名</param>
    ''' <remarks></remarks>
    Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                  "_Start[element:{0}][cssClass:{1}]", element, cssClass))

        If String.IsNullOrEmpty(element.Attributes("Class").Trim) Then
            element.Attributes("Class") = cssClass
        Else
            element.Attributes("Class") = element.Attributes("Class") & Space(1) & cssClass
        End If

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

#End Region

    '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) START
#Region " ページクラス処理のバイパス処理 "
    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function
#End Region
    '2014/05/13 TCS 山田 性能改善(TCVから見積作成画面遷移) END

End Class
