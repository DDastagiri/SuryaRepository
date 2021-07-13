
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160219.aspx.vb
'─────────────────────────────────────
'機能： RO損傷登録画面
'補足： 
'作成： 2013/11/19 SKFC 橋本
'更新： 2018/03/29 SKFC 横田　REQ-SVT-TMT-20170809-001　損傷写真複数対応
'    ： 2019/09/20 SKFC 二村 TR-V4-TKM-20190813-003横展
'    ： 2020/03/24 SKFC 橋詰 損傷写真5枚目削除時に消えない不具合対応
'─────────────────────────────────────

'型宣言を強制
Option Explicit On

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3160219
Imports Toyota.eCRB.iCROP.DataAccess.SC3160219

Partial Class Pages_SC3160219
    Inherits BasePage


#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3160219"

    ''' <summary>
    ''' 元画面からの引数のキー名：画面モード
    ''' </summary>
    Private Const C_REQ_STRING_DISP_MODE As String = "ViewMode"

    ''' <summary>
    '''  元画面からの引数のキー名：RO外装ID
    ''' </summary>
    Private Const C_REQ_STRING_RO_EXTERIOR_ID As String = "ExteriorId"

    ''' <summary>
    '''  元画面からの引数のキー名：部品種別
    ''' </summary>
    Private Const C_REQ_STRING_PARTS_TYPE As String = "PartsNo"

    ''' <summary>
    '''  元画面からの引数のキー名：ログインユーザーID
    ''' </summary>
    Private Const C_REQ_STRING_LOGIN_USER_ID As String = "LoginUserId"

    ''' <summary>
    ''' 画面モード：登録モード
    ''' </summary>
    Private Const C_DISPMODE_EDIT As String = "0"

    ''' <summary>
    ''' 画面モード：参照モード
    ''' </summary>
    Private Const C_DISPMODE_VIEW As String = "1"

    ''' <summary>
    ''' ダメージ種別のボタン数
    ''' </summary>
    Private Const P_DAMAGE_TYPE_BOTTOM_NUM As Integer = 5

    ''' <summary>
    ''' ダメージ種別配列の列：種別コード
    ''' </summary>
    Private Const P_ARRAYROWNO_DAMAGETYPE As Integer = 0

    ''' <summary>
    ''' ダメージ種別配列の列：ダメージ有無
    ''' </summary>
    Private Const P_ARRAYROWNO_EXISTS As Integer = 1

    ''' <summary>
    ''' ダメージ有無：ノーダメージ
    ''' </summary>
    Private Const C_DBVAL_DAMAGEEXISTS_NODAMAGE As String = "-"

#End Region

#Region "プロパティ"

    Public ReadOnly Property GetSessionAryDamageExists() As String(,)
        Get
            Return CType(Me.Session("ARY_DAMAGE_TYPE_EXISTS"), String(,))
        End Get
    End Property
    'ビジネスロジッククラス
    Dim clsBizLogic As New SC3160219BusinessLogic


#End Region

    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ビジネスロジッククラス
        Dim clsBizLogic As New SC3160219BusinessLogic

        '画像アップロードフラグ初期化
        If String.IsNullOrEmpty(Me.Hidden_UploadFlag.Value) Then
            Me.Hidden_UploadFlag.Value = "0"
        End If

        '初期表示
        If (Not IsPostBack AndAlso Not IsCallback) Then

            Dim dispMode As String = String.Empty       '画面モード
            Dim roExteriorId As Decimal = 0             'RO外装ID
            Dim partsType As String = String.Empty      '部位種別
            Dim loginUserId As String = String.Empty    'ログインユーザーID

            '登録ボタン初期化
            Me.Hidden_DoneClickFlg.Value = "0"

            'メッセージ取得
            Me.Hidden_MessageSaveImageFailure.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, 901))

            '呼び出し元から引数を取得
            Me.GetRequestValue(dispMode, roExteriorId, partsType, loginUserId)

            'RO外装ダメージマスタ情報取得
            Dim dtMaster As SC3160219DataSet.RoExteriorDamageMasterDataTable
            dtMaster = clsBizLogic.GetRoExteriorDamageMaster(partsType)

            '該当するダメージ種別の件数を取得（ノーダメージを除く）
            Dim masterCount As Integer = 0
            For Each dr As SC3160219DataSet.RoExteriorDamageMasterRow In dtMaster
                If Not dr.DAMAGE_TYPE.Equals(C_DBVAL_DAMAGEEXISTS_NODAMAGE) Then
                    masterCount += 1
                End If
            Next
            Me.Hidden_DamageTypeCount.Value = masterCount

            'ダメージ種別情報用の配列
            Dim aryDamageExists(,) As String
            aryDamageExists = New String(masterCount - 1, 1) {}

            '画面表示モードとマスターによる表示設定
            Me.SetDisplayView(dispMode, dtMaster, aryDamageExists)

            'RO外装ダメージ情報取得
            Dim dtInfo As SC3160219DataSet.RoExteriorDamageInfoDataTable
            dtInfo = clsBizLogic.GetRoExteriorDamageInfo(roExteriorId, partsType)

            'ダメージ情報の表示設定
            If Not IsDBNull(dtInfo) AndAlso dtInfo.Rows.Count > 0 Then
                Me.SetDamageInfo(dtInfo, masterCount, aryDamageExists)
            End If

            'ダメージ種別情報をセッションに追加
            Me.Session.Add("ARY_DAMAGE_TYPE_EXISTS", aryDamageExists)

            '写真画像ファイルパス
            Dim imagePath As String = String.Empty
            Dim imagePath1 As String = String.Empty
            Dim imagePath2 As String = String.Empty
            Dim imagePath3 As String = String.Empty
            Dim imagePath4 As String = String.Empty

            '削除対象画像ファイルパスの配列
            'Dim alDeleteFilePath As New ArrayList

            Me.PhotoBox.Visible = False
            Me.PhotoBox1.Visible = False
            Me.PhotoBox2.Visible = False
            Me.PhotoBox3.Visible = False
            Me.PhotoBox4.Visible = False

            If Not IsDBNull(dtInfo) AndAlso dtInfo.Rows.Count > 0 _
            AndAlso Not String.IsNullOrEmpty(dtInfo.Rows(0).Item("THUMBNAIL_IMG_PATH").ToString.Trim) Then

                '写真画像ファイルパス（一部）を取得
                imagePath = dtInfo.Rows(0).Item("THUMBNAIL_IMG_PATH").ToString

                '現ファイルパスを保持
                Me.Hidden_RoThumbnailImgOrg1.Value = imagePath

                'サムネイルIDを保持
                'Me.Hidden_RoThumbnailIdOrg1.Value = dtInfo.Rows(0).Item("RO_THUMBNAIL_ID").ToString

                '現ファイルパスを削除対象に
                'alDeleteFilePath.Add(imagePath)

            Else
                '現ファイルパスを空で保持
                Me.Hidden_RoThumbnailImgOrg1.Value = String.Empty
                Me.Hidden_RoThumbnailImgOrg2.Value = String.Empty
                Me.Hidden_RoThumbnailImgOrg3.Value = String.Empty
                Me.Hidden_RoThumbnailImgOrg4.Value = String.Empty
                Me.Hidden_RoThumbnailImgOrg5.Value = String.Empty

                '連番を0で保持
                Me.Hidden_RoThumbnailImgSeq1.Value = "0"
                Me.Hidden_RoThumbnailImgSeq2.Value = "0"
                Me.Hidden_RoThumbnailImgSeq3.Value = "0"
                Me.Hidden_RoThumbnailImgSeq4.Value = "0"
                Me.Hidden_RoThumbnailImgSeq5.Value = "0"

                'サムネイルIDを-1で保持
                Me.Hidden_RoThumbnailIdOrg1.Value = "-1"
                Me.Hidden_RoThumbnailIdOrg2.Value = "-1"
                Me.Hidden_RoThumbnailIdOrg3.Value = "-1"
                Me.Hidden_RoThumbnailIdOrg4.Value = "-1"
                Me.Hidden_RoThumbnailIdOrg5.Value = "-1"

            End If

            '削除対象画像ファイルパスの配列をセッションに追加
            'Me.Session.Add("AL_DELETE_FILE_PATH", alDeleteFilePath)

            Dim dtInfo1 As SC3160219DataSet.RoExteriorDamageInfoDataTable
            dtInfo1 = clsBizLogic.GetRothumbnailInfo(roExteriorId, partsType)

            If Not IsDBNull(dtInfo1) AndAlso dtInfo1.Rows.Count > 0 _
            AndAlso Not String.IsNullOrEmpty(dtInfo1.Rows(0).Item("THUMBNAIL_IMG_PATH").ToString.Trim) Then

                Dim i As Integer = 1

                For Each dt As SC3160219DataSet.RoExteriorDamageInfoRow In dtInfo1.Rows
                    imagePath = dt.Item("THUMBNAIL_IMG_PATH").ToString.Trim
                    Dim thumNailID As String = dt.Item("RO_THUMBNAIL_ID").ToString.Trim

                    If (i.Equals(1)) Then

                        Me.Hidden_RoThumbnailImgOrg1.Value = imagePath
                        Me.Hidden_RoThumbnailIdOrg1.Value = thumNailID
                        Me.Hidden_RoThumbnailImgPath1.Value = imagePath

                        Me.Hidden_RoThumbnailImgSeq1.Value = Mid(imagePath,
                                                                imagePath.LastIndexOf("_") + 2,
                                                                imagePath.LastIndexOf(".") - imagePath.LastIndexOf("_") - 1)

                    End If
                    If (i.Equals(2)) Then

                        Me.Hidden_RoThumbnailImgOrg2.Value = imagePath
                        Me.Hidden_RoThumbnailIdOrg2.Value = thumNailID
                        Me.Hidden_RoThumbnailImgPath2.Value = imagePath

                        Me.Hidden_RoThumbnailImgSeq2.Value = Mid(imagePath,
                                                                imagePath.LastIndexOf("_") + 2,
                                                                imagePath.LastIndexOf(".") - imagePath.LastIndexOf("_") - 1)

                    End If
                    If (i.Equals(3)) Then
                        Me.Hidden_RoThumbnailImgOrg3.Value = imagePath
                        Me.Hidden_RoThumbnailIdOrg3.Value = thumNailID
                        Me.Hidden_RoThumbnailImgPath3.Value = imagePath

                        Me.Hidden_RoThumbnailImgSeq3.Value = Mid(imagePath,
                                                                imagePath.LastIndexOf("_") + 2,
                                                                imagePath.LastIndexOf(".") - imagePath.LastIndexOf("_") - 1)

                    End If
                    If (i.Equals(4)) Then
                        Me.Hidden_RoThumbnailImgOrg4.Value = imagePath
                        Me.Hidden_RoThumbnailIdOrg4.Value = thumNailID
                        Me.Hidden_RoThumbnailImgPath4.Value = imagePath

                        Me.Hidden_RoThumbnailImgSeq4.Value = Mid(imagePath,
                                                                imagePath.LastIndexOf("_") + 2,
                                                                imagePath.LastIndexOf(".") - imagePath.LastIndexOf("_") - 1)

                    End If
                    If (i.Equals(5)) Then
                        Me.Hidden_RoThumbnailImgOrg5.Value = imagePath
                        Me.Hidden_RoThumbnailIdOrg5.Value = thumNailID
                        Me.Hidden_RoThumbnailImgPath5.Value = imagePath

                        Me.Hidden_RoThumbnailImgSeq5.Value = Mid(imagePath,
                                                                imagePath.LastIndexOf("_") + 2,
                                                                imagePath.LastIndexOf(".") - imagePath.LastIndexOf("_") - 1)

                    End If

                    '写真領域の表示設定
                    Me.SetPhotoArea(imagePath, i)
                    i = i + 1
                Next

            Else

                Me.SetPhotoArea(String.Empty, 0)

            End If

        ElseIf Not ScriptManager.IsInAsyncPostBack And IsPostBack Then
            ' 非同期ポストバックでない場合のみ行う処理
            '写真領域の表示設定

            If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath1.Value) Then
                Me.SetPhotoArea(Me.Hidden_RoThumbnailImgPath1.Value, 1)
            End If

            If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath2.Value) Then
                Me.SetPhotoArea(Me.Hidden_RoThumbnailImgPath2.Value, 2)
            End If

            If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath3.Value) Then
                Me.SetPhotoArea(Me.Hidden_RoThumbnailImgPath3.Value, 3)
            End If

            If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath4.Value) Then
                Me.SetPhotoArea(Me.Hidden_RoThumbnailImgPath4.Value, 4)
            End If

            If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath5.Value) Then

                Me.SetPhotoArea(Me.Hidden_RoThumbnailImgPath5.Value, 5)
            End If

        End If

        If Me.Hidden_UploadFlag.Value.Equals("0") Then
            '非画像アップロード時はクルクルを非表示にする
            Me.LoadingScreen.Style.Remove("display")
            Me.LoadingScreen.Style.Add("display", "none")
        Else
            '画像アップロード中はクルクルを表示する
            Me.LoadingScreen.Style.Remove("display")
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


#Region "初期表示"

    ''' <summary>
    ''' 呼び出し元からリクエストを取得
    ''' </summary>
    ''' <param name="dispMode">画面モード</param>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <param name="loginUserId">ログインユーザーID</param>
    ''' <remarks></remarks>
    Private Sub GetRequestValue(ByRef dispMode As String, ByRef roExteriorId As Decimal, ByRef partsType As String, ByRef loginUserId As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '画面モード
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_DISP_MODE)) Then
            dispMode = Request(C_REQ_STRING_DISP_MODE).Trim()
            Me.Hidden_DispMode.Value = dispMode
        End If

        'RO外装ID
        Dim strRoExteriorId As String = String.Empty
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_RO_EXTERIOR_ID)) Then
            strRoExteriorId = Request(C_REQ_STRING_RO_EXTERIOR_ID).Trim()
            '整数判定
            If Long.TryParse(strRoExteriorId, roExteriorId) Then
                Me.Hidden_RoExteriorId.Value = strRoExteriorId
            Else
                Me.Hidden_RoExteriorId.Value = "-1"
            End If
        End If

        '部位種別
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_PARTS_TYPE)) Then
            partsType = Request(C_REQ_STRING_PARTS_TYPE).Trim()
            Me.Hidden_PartsType.Value = partsType
        End If

        'ログインユーザーID
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_LOGIN_USER_ID)) Then
            loginUserId = Request(C_REQ_STRING_LOGIN_USER_ID).Trim()
            Me.Hidden_LoginUserId.Value = loginUserId
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [DispMode:{1}][RoExteriorId:{2}][PartsType:{3}][LoginUserId:{4}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dispMode, strRoExteriorId, partsType, loginUserId))

    End Sub


    ''' <summary>
    ''' 画面表示モードとマスターによる画面表示設定
    ''' </summary>
    ''' <param name="dispMode">画面表示モード</param>
    ''' <param name="dtMaster">部位種別のマスターデータ</param>
    ''' <param name="aryDamageExists">ダメージ種別情報用の配列</param>
    ''' <remarks></remarks>
    Private Sub SetDisplayView(ByVal dispMode As String, ByVal dtMaster As SC3160219DataSet.RoExteriorDamageMasterDataTable,
                               ByRef aryDamageExists(,) As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [dispMode:{1}][dtMasterRowsCount:{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dispMode, dtMaster.Rows.Count))

        'ダメージ種別ボタンの設定
        For i As Integer = 0 To P_DAMAGE_TYPE_BOTTOM_NUM - 1

            '該当ダメージ種別の件数内、且つ"-"ではないの場合
            If i <= dtMaster.Rows.Count - 1 AndAlso Not dtMaster.Rows(i).Item("DAMAGE_TYPE").ToString.Equals(C_DBVAL_DAMAGEEXISTS_NODAMAGE) Then

                'ボタンタイトルの設定
                CType(Me.FindControl("Anchor_DamageTypeButton_" & (i + 1).ToString), HtmlAnchor).InnerText = _
                    HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, CDec(dtMaster.Rows(i).Item("DAMAGE_WORD_NUM"))))

                'ダメージ種別マークの設定
                CType(Me.FindControl("Span_DamageTypeSymbol_" & (i + 1).ToString), HtmlGenericControl).InnerText = _
                    HttpUtility.HtmlEncode(CStr(dtMaster.Rows(i).Item("DAMAGE_TYPE")))
                CType(Me.FindControl("Span_DamageTypeSymbol_" & (i + 1).ToString), HtmlGenericControl).Style.Item("background") = _
                    "-webkit-gradient(linear, left top, left bottom, " & _
                    "from(" & HttpUtility.HtmlEncode(CStr(dtMaster.Rows(i).Item("GRADATION_FROM"))) & "), " & _
                    "to(" & HttpUtility.HtmlEncode(CStr(dtMaster.Rows(i).Item("GRADATION_TO"))) & "))"

                'ダメージ種別の保持
                aryDamageExists(i, P_ARRAYROWNO_DAMAGETYPE) = CStr(dtMaster.Rows(i).Item("DAMAGE_TYPE"))    'ダメージの種別
                aryDamageExists(i, P_ARRAYROWNO_EXISTS) = CStr(False)                                       'ダメージの有無（初期値＝ダメージ無し）

            Else
                '非表示
                CType(Me.FindControl("Div_DamageTypeDammyButton_" & (i + 1).ToString), HtmlGenericControl).Visible = False

            End If
        Next

        '参照モードの場合のコントロール設定
        If dispMode.Equals(C_DISPMODE_VIEW) Then

            Me.Input_DeletePhotoButton1.Visible = False
            Me.Input_DeletePhotoButton2.Visible = False
            Me.Input_DeletePhotoButton3.Visible = False
            Me.Input_DeletePhotoButton4.Visible = False
            Me.Input_DeletePhotoButton5.Visible = False
            Me.Div_CameraArea.Visible = False
            Me.TextBox_Memo.Enabled = False

            'タイトルバーの設定
            Me.Hidden_CancelTitle.Value = String.Empty
            Me.Hidden_DoneTitle.Value = String.Empty

        Else
            'カメラボタンの文言
            'Me.A_CameraButtom_Retake.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, 13))

            'タイトルバーの設定
            Me.Hidden_CancelTitle.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, 10))
            Me.Hidden_DoneTitle.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, 11))
        End If

        'タイトルバーの設定
        Me.Hidden_Title.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, CDec(dtMaster.Rows(0).Item("PARTS_WORD_NUM"))))

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' ダメージ情報の表示設定
    ''' </summary>
    ''' <param name="dtInfo">RO外装ダメージ情報</param>
    ''' <param name="damageTypeCount">ダメージ種別の件数</param>
    ''' <param name="aryDamageExists">ダメージ種別情報用の配列</param>
    ''' <remarks></remarks>
    Private Sub SetDamageInfo(ByVal dtInfo As SC3160219DataSet.RoExteriorDamageInfoDataTable, ByVal damageTypeCount As Integer,
                              ByRef aryDamageExists(,) As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [dtInfoRowsCount:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dtInfo.Rows.Count))

        'ダメージ有無文字列を取得
        Dim damageExistsLine As String = dtInfo.Rows(0).Item("DAMAGE_TYPE_EXISTS").ToString

        'ダメージ有無の設定
        For i As Integer = 0 To damageTypeCount - 1

            '該当ダメージ種別区分がダメージ有無文字列にある場合
            If damageExistsLine.Contains(aryDamageExists(i, 0).ToString) Then
                'ダメージ有り
                aryDamageExists(i, P_ARRAYROWNO_EXISTS) = CStr(True)

            Else
                'ダメージ無し
                aryDamageExists(i, P_ARRAYROWNO_EXISTS) = CStr(False)

            End If

            'ダメージ種別ボタン色設定
            Me.SetButtonColor(i + 1, CBool(aryDamageExists(i, P_ARRAYROWNO_EXISTS)))

        Next

        'メモを表示
        If Not String.IsNullOrEmpty(dtInfo.Rows(0).Item("DAMAGE_MEMO").ToString.Trim) Then
            Me.TextBox_Memo.Text = dtInfo.Rows(0).Item("DAMAGE_MEMO").ToString
        End If

        'ROサムネイルIDとサムネイル画像ファイルパスを保持
        'Me.Hidden_RoThumbnailIdOrg1.Value = dtInfo.Rows(0).Item("RO_THUMBNAIL_ID").ToString
        'Me.Hidden_RoThumbnailImgPath1.Value = dtInfo.Rows(0).Item("THUMBNAIL_IMG_PATH").ToString

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "ダメージ有無変更時"

    ''' <summary>
    ''' ダメージ種別ボタン押下時
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub DamageTypeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles Anchor_DamageTypeButton_1.ServerClick, Anchor_DamageTypeButton_2.ServerClick, Anchor_DamageTypeButton_3.ServerClick, Anchor_DamageTypeButton_4.ServerClick, Anchor_DamageTypeButton_5.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        Dim buttonName As String = DirectCast(sender, HtmlAnchor).ID

        'コントロール名の最後の数字を取得
        Dim buttonNo As Integer = CInt(Mid(buttonName, buttonName.LastIndexOf("_") + 2, 1))

        'ダメージ有無変更
        Me.ChangeDamageExistence(buttonNo)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' ダメージ有無変更
    ''' </summary>
    ''' <param name="damageTypeBottonNo">ダメージ種別ボタンを識別する番号</param>
    ''' <remarks></remarks>
    Private Sub ChangeDamageExistence(ByVal damageTypeBottonNo As Integer)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [damageTypeBottonNo:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  damageTypeBottonNo))

        '参照モード時は何もしない
        If Me.Hidden_DispMode.Value.Equals(C_DISPMODE_VIEW) Then
            Return
        End If

        'ダメージ種別マスターの件数を取得
        Dim masterCount As Integer = CInt(Me.Hidden_DamageTypeCount.Value)

        '念の為（ありえない予定）
        If damageTypeBottonNo > masterCount Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0} End [damageTypeBottonNo:{1}][masterCount:{2}]",
                                      System.Reflection.MethodBase.GetCurrentMethod.Name,
                                      damageTypeBottonNo, masterCount))
            Return

        End If

        'ダメージ種別情報用の配列
        Dim aryDamageExists(,) As String
        aryDamageExists = New String(masterCount - 1, 1) {}

        'セッションからダメージ種別情報を取得
        aryDamageExists = CType(Me.Session("ARY_DAMAGE_TYPE_EXISTS"), String(,))

        'ダメージ有り
        If CBool(aryDamageExists(damageTypeBottonNo - 1, P_ARRAYROWNO_EXISTS)) Then
            'ダメージ無しに変更
            aryDamageExists(damageTypeBottonNo - 1, P_ARRAYROWNO_EXISTS) = CStr(False)
            'ダメージ種別ボタン色設定
            Me.SetButtonColor(damageTypeBottonNo, False)

        Else
            'ダメージ有りに変更
            aryDamageExists(damageTypeBottonNo - 1, P_ARRAYROWNO_EXISTS) = CStr(True)
            'ダメージ種別ボタン色設定
            Me.SetButtonColor(damageTypeBottonNo, True)

        End If

        'ダメージ種別情報を画面に格納
        Me.Session("ARY_DAMAGE_TYPE_EXISTS") = aryDamageExists

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} End",
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' ダメージ種別ボタン色設定
    ''' </summary>
    ''' <param name="damageTypeBottonNo">ダメージ種別ボタンを識別する番号</param>
    ''' <param name="isDamage">ダメージ有り</param>
    ''' <remarks></remarks>
    Private Sub SetButtonColor(ByVal damageTypeBottonNo As Integer, ByVal isDamage As Boolean)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [damageTypeBottonNo:{1}][isDamage:{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  damageTypeBottonNo, CStr(isDamage)))

        'ダメージ有り
        If isDamage Then
            'ボタンカラーを青に設定
            CType(Me.FindControl("Div_DamageTypeDammyButton_" & damageTypeBottonNo.ToString), HtmlGenericControl).Style.Item("background") = _
                "-webkit-gradient(linear, left top, left bottom, from(#7b94e4), color-stop(0.500, #4e6fdc), color-stop(0.501, #3f61d9), to(#4063d9))"
            'ボタンタイトルを白に設定
            CType(Me.FindControl("Anchor_DamageTypeButton_" & damageTypeBottonNo.ToString), HtmlAnchor).Style.Item("color") = "#ffffff"

        Else
            'ボタンカラーを規定値に設定
            CType(Me.FindControl("Div_DamageTypeDammyButton_" & damageTypeBottonNo.ToString), HtmlGenericControl).Style.Item("background") = _
                "-webkit-gradient(linear, left top, left bottom, from(#ffffff), color-stop(0.500, #f7f8f9), color-stop(0.501, #edeff3), to(#edeff3))"
            'ボタンタイトルをグレーに設定
            CType(Me.FindControl("Anchor_DamageTypeButton_" & damageTypeBottonNo.ToString), HtmlAnchor).Style.Item("color") = "#666666"

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "写真表示削除"

    ''' <summary>
    ''' 写真削除ボタン押下時
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>表示上消えるのみで、ファイルはまだ削除しない</remarks>
    Private Sub DeletePhotoBotton_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Input_DeletePhotoButton1.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        '保持用ファイルパスをクリア
        Me.Hidden_RoThumbnailImgPath1.Value = String.Empty

        '写真領域の表示設定
        Me.SetPhotoArea(String.Empty, 1)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    Private Sub DeletePhotoBotton_Click2(ByVal sender As Object, ByVal e As System.EventArgs) Handles Input_DeletePhotoButton2.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        '保持用ファイルパスをクリア
        Me.Hidden_RoThumbnailImgPath2.Value = String.Empty

        '写真領域の表示設定
        Me.SetPhotoArea(String.Empty, 2)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    Private Sub DeletePhotoBotton_Click3(ByVal sender As Object, ByVal e As System.EventArgs) Handles Input_DeletePhotoButton3.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        '保持用ファイルパスをクリア
        Me.Hidden_RoThumbnailImgPath3.Value = String.Empty

        '写真領域の表示設定
        Me.SetPhotoArea(String.Empty, 3)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    Private Sub DeletePhotoBotton_Click4(ByVal sender As Object, ByVal e As System.EventArgs) Handles Input_DeletePhotoButton4.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        '保持用ファイルパスをクリア
        Me.Hidden_RoThumbnailImgPath4.Value = String.Empty

        '写真領域の表示設定
        Me.SetPhotoArea(String.Empty, 4)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    Private Sub DeletePhotoBotton_Click5(ByVal sender As Object, ByVal e As System.EventArgs) Handles Input_DeletePhotoButton5.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [sender:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  sender.ToString))

        '保持用ファイルパスをクリア
        Me.Hidden_RoThumbnailImgPath5.Value = String.Empty

        '写真領域の表示設定
        Me.SetPhotoArea(String.Empty, 5)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "写真領域の表示設定"

    ''' <summary>
    ''' 写真領域の表示設定
    ''' </summary>
    ''' <param name="imageFilePath">画像ファイルパスの一部（ファイル名）</param>
    ''' <remarks></remarks>
    Private Sub SetPhotoArea(ByVal imageFilePath As String, ByVal editFlag As Integer)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start [imageFilePath:{1},{2}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  imageFilePath, editFlag))

        'ビジネスロジッククラス
        Dim clsBizLogic As New SC3160219BusinessLogic

        '変数宣言
        Dim retRelativePath As String

        Dim allRoThumbnailImgPath(P_DAMAGE_TYPE_BOTTOM_NUM - 1) As Object
        Dim allOriginalImageFilePath(P_DAMAGE_TYPE_BOTTOM_NUM - 1) As Object
        Dim allImgDamagePhoto(P_DAMAGE_TYPE_BOTTOM_NUM - 1) As Object
        Dim allPhotoBox(P_DAMAGE_TYPE_BOTTOM_NUM - 1) As Object

        'ImgPathとFilePathを格納した配列
        For i As Integer = 0 To P_DAMAGE_TYPE_BOTTOM_NUM - 1
            allRoThumbnailImgPath(i) = Me.FindControl("Hidden_RoThumbnailImgPath" & (i + 1).ToString)
            allOriginalImageFilePath(i) = Me.FindControl("Hidden_OriginalImageFilePath" & (i + 1).ToString)
            allImgDamagePhoto(i) = Me.FindControl("Img_DmagePhoto" & (i + 1).ToString)

            'PhotoBoxのみ、IDが1～5ではなく、無しと1～4で振り分けられているため、以下の処理
            If i = 0 Then
                allPhotoBox(i) = Me.FindControl("PhotoBox")
            Else
                allPhotoBox(i) = Me.FindControl("PhotoBox" & (i).ToString)
            End If
        Next

        If String.IsNullOrEmpty(imageFilePath.Trim) Then

            If editFlag <> 0 Then
                'editFlag(削除ボタンを押された損傷写真の値)から写真の数まで処理を繰り返す
                For i As Integer = editFlag To allRoThumbnailImgPath.Length
                    If i <> allRoThumbnailImgPath.Length Then
                        allRoThumbnailImgPath(i - 1).Value = allRoThumbnailImgPath(i).Value
                        allOriginalImageFilePath(i - 1).Value = allOriginalImageFilePath(i).Value
                    Else
                        allRoThumbnailImgPath(i - 1).Value = String.Empty
                        allOriginalImageFilePath(i - 1).Value = String.Empty
                    End If

                    '写真相対パスの取得
                    If Not String.IsNullOrEmpty(allRoThumbnailImgPath(i - 1).Value) And i <> allRoThumbnailImgPath.Length Then
                        retRelativePath = clsBizLogic.GetImageFileRelativePath(allRoThumbnailImgPath(i - 1).Value, allOriginalImageFilePath(i - 1).Value)
                        allImgDamagePhoto(i - 1).src = retRelativePath
                    Else
                        allImgDamagePhoto(i - 1).src = String.Empty
                    End If
                Next

            End If

            Me.DisplayPhoto()

        Else

            Dim OriginalImageFilePath As String = String.Empty

            '写真相対パスの取得
            retRelativePath = clsBizLogic.GetImageFileRelativePath(imageFilePath, OriginalImageFilePath)

            allImgDamagePhoto(editFlag - 1).Src = retRelativePath
            allOriginalImageFilePath(editFlag - 1).Value = OriginalImageFilePath
            allPhotoBox(editFlag - 1).Visible = True

        End If

        'パスがある場合
        If Not String.IsNullOrEmpty(allRoThumbnailImgPath(0).Value) Then

            '写真領域の表示
            Me.Div_PhotoArea.Visible = True

            'カメラボタンの設定
            Me.Div_CameraDammyButtom_Img.Visible = True
            Me.Div_CameraDammyButtom_Retake.Visible = False

            If Not String.IsNullOrEmpty(allRoThumbnailImgPath(P_DAMAGE_TYPE_BOTTOM_NUM - 1).Value) Then

                'カメラボタンの設定
                Me.Div_CameraDammyButtom_Img.Visible = False
                Me.Div_CameraDammyButtom_Retake.Visible = True

            End If

        Else

            '写真領域非表示
            'Me.Img_DmagePhoto.Src = String.Empty
            Me.Div_PhotoArea.Visible = False

            'カメラボタンの設定
            Me.Div_CameraDammyButtom_Img.Visible = True
            Me.Div_CameraDammyButtom_Retake.Visible = False


        End If

        'ポップアップのリサイズ
        Me.RunJavaScript("resizePopUp()")

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "カメラ機能"

    ''' <summary>
    ''' カメラボタン押下時（カメラ機能後）
    ''' </summary>TakePhotoAfter
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub Botton_Camera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles A_CameraButtom.ServerClick ', A_CameraButtom_Retake.ServerClick

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim clsBizLogic As New SC3160219BusinessLogic

        '画面の情報を取得
        Dim roExteriorId As String = Me.Hidden_RoExteriorId.Value
        Dim partsType As String = Me.Hidden_PartsType.Value
        'Dim imageSeq As String = Me.Hidden_RoThumbnailImgSeq1.Value

        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgSeq1.Value) Then
            Me.Hidden_RoThumbnailImgSeq1.Value = "0"
        End If
        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgSeq2.Value) Then
            Me.Hidden_RoThumbnailImgSeq2.Value = "0"
        End If
        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgSeq3.Value) Then
            Me.Hidden_RoThumbnailImgSeq3.Value = "0"
        End If
        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgSeq4.Value) Then
            Me.Hidden_RoThumbnailImgSeq4.Value = "0"
        End If
        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgSeq5.Value) Then
            Me.Hidden_RoThumbnailImgSeq5.Value = "0"
        End If




        ' 宣言と初期値代入
        Dim ImgSeqArray() As Integer = {CInt(Me.Hidden_RoThumbnailImgSeq1.Value),
                                   CInt(Me.Hidden_RoThumbnailImgSeq2.Value),
                                   CInt(Me.Hidden_RoThumbnailImgSeq3.Value),
                                   CInt(Me.Hidden_RoThumbnailImgSeq4.Value),
                                   CInt(Me.Hidden_RoThumbnailImgSeq5.Value)}

        Dim maxKakaku As Integer = 0

        ' 最大値を求める
        Dim imageSeq As String = ImgSeqArray.Max()




        '新ファイルパスを作成
        Dim filePath As String = clsBizLogic.CleateNewFilePath(roExteriorId, partsType, imageSeq)

        Me.Hidden_RoThumbnailImgSeq1.Value = Mid(filePath,
                                        filePath.LastIndexOf("_") + 2,
                                        filePath.LastIndexOf(".") - filePath.LastIndexOf("_") - 1)


        Me.Hidden_RoThumbnailImgSeq2.Value = Me.Hidden_RoThumbnailImgSeq1.Value
        Me.Hidden_RoThumbnailImgSeq3.Value = Me.Hidden_RoThumbnailImgSeq1.Value
        Me.Hidden_RoThumbnailImgSeq4.Value = Me.Hidden_RoThumbnailImgSeq1.Value
        Me.Hidden_RoThumbnailImgSeq5.Value = Me.Hidden_RoThumbnailImgSeq1.Value

        'カメラのみ「.png」を消す
        filePath = filePath.Remove(filePath.LastIndexOf("."))

        Dim freeNo As Integer = 0

        If String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath1.Value) Then
            freeNo = 1
        ElseIf String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath2.Value) Then
            freeNo = 2
        ElseIf String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath3.Value) Then
            freeNo = 3
        ElseIf String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath4.Value) Then
            freeNo = 4
        ElseIf String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath5.Value) Then
            freeNo = 5
        End If

        '画像アップロードフラグを画像アップロード中に設定
        Me.Hidden_UploadFlag.Value = "1"
        'クルクルを表示
        Me.LoadingScreen.Style.Remove("display")

        Me.RunJavaScript("onCamera('" & filePath & "', '" & freeNo & "')")

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    ''' <summary>
    ''' カメラ起動後に新ファイルパスを返却
    ''' </summary>
    ''' <param name="roExteriorId">RO外装ID</param>
    ''' <param name="partsType">部位種別</param>
    ''' <param name="imageSeq">画像連番</param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function TakePhotoAfter(ByVal roExteriorId As String, ByVal partsType As String, ByVal imageSeq As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} {1} Start [roExteriorId:{2}][partsType:{3}][imageSeq:{4}]",
                                  C_FUNCTION_ID, System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roExteriorId, partsType, imageSeq))

        Dim clsBizLogic As New SC3160219BusinessLogic
        Dim filePath As String = String.Empty

        Dim nextImageSeq As Integer
        If String.IsNullOrEmpty(imageSeq) Then
            nextImageSeq = 1
        Else
            nextImageSeq = CInt(imageSeq) - 1
        End If
        imageSeq = nextImageSeq.ToString

        '新ファイルパスを取得
        filePath = clsBizLogic.CleateNewFilePath(roExteriorId, partsType, imageSeq)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} {1} End [filePath:{2}]",
                                  C_FUNCTION_ID, System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath))
        Return filePath

    End Function

#End Region

#Region "登録"

    ''' <summary>
    ''' 追加ボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Sub Botton_Done_Click(ByVal roExteriorId As Decimal,
                                        ByVal partsType As String,
                                        ByVal memo As String,
                                        ByVal roThumbnailId As String,
                                        ByVal roThumbnailImgPath As String,
                                        ByVal roThumbnailImgPathOrg As String,
                                        ByVal account As String,
                                        ByVal masterCount As Integer)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} {1} Start [roExteriorId:{2}][partsType:{3}][memo:{4}][roThumbnailId:{5}]" & _
                                  "[roThumbnailImgPath:{6}][roThumbnailImgPathOrg:{7}][account:{8}][masterCount:{9}]",
                                  C_FUNCTION_ID, System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roExteriorId, partsType, memo, roThumbnailId,
                                  roThumbnailImgPath,
                                  roThumbnailImgPathOrg,
                                  account, masterCount))

        'ビジネスロジッククラス
        Dim clsBizLogic As New SC3160219BusinessLogic
        Dim clsPage As New Pages_SC3160219

        'ダメージ種別情報用の配列
        Dim aryDamageExists(,) As String
        aryDamageExists = New String(masterCount - 1, 1) {}

        'セッションからダメージ種別情報を取得
        aryDamageExists = clsPage.GetSessionAryDamageExists()

        'ダメージ有無の文字列を作成
        Dim damageTypeExists As String = String.Empty
        For i As Integer = 0 To masterCount - 1

            'ダメージ有りの場合
            If Not IsNothing(aryDamageExists(i, P_ARRAYROWNO_EXISTS)) AndAlso CBool(aryDamageExists(i, P_ARRAYROWNO_EXISTS)) Then
                damageTypeExists += aryDamageExists(i, P_ARRAYROWNO_DAMAGETYPE)
            End If

        Next

        Dim isSuccess As Boolean

        'RO外装ダメージ情報登録
        isSuccess = clsBizLogic.SetROExteriorDamageInfo(roExteriorId,
                                                        partsType,
                                                        damageTypeExists,
                                                        memo,
                                                        roThumbnailId,
                                                        roThumbnailImgPath,
                                                        roThumbnailImgPathOrg,
                                                        account)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} {1} End [IsSuccess:{2}]",
                                  C_FUNCTION_ID, System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  isSuccess))

    End Sub

#End Region

    ''' <summary>
    ''' Javascript関数実行
    ''' </summary>
    ''' <param name="method">関数名（引数）</param>
    ''' <remarks>※WebMethodからは呼べない</remarks>
    Private Sub RunJavaScript(ByVal method As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} Start [method:{1}]",
                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                          method))

        Dim clsPage As New Pages_SC3160219

        'Javascript関数実行
        Dim scriptLine As String = "<script type='text/javascript'>window.onload=" & method & ";</script>"

        Me.Literal_JavaScript.Text = scriptLine

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' Javascript関数実行
    ''' </summary>
    ''' <remarks>※WebMethodからは呼べない</remarks>
    Private Sub DisplayPhoto()

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} Start",
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim i As Integer = 0

        '写真相対パスの取得
        If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath1.Value) Then
            i = i + 1
        End If
        If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath2.Value) Then
            i = i + 1
        End If
        If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath3.Value) Then
            i = i + 1
        End If
        If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath4.Value) Then
            i = i + 1
        End If
        If Not String.IsNullOrEmpty(Me.Hidden_RoThumbnailImgPath5.Value) Then
            i = i + 1
        End If

        Select Case i
            Case 1
                Me.PhotoBox1.Visible = False
                Me.PhotoBox2.Visible = False
                Me.PhotoBox3.Visible = False
                Me.PhotoBox4.Visible = False
            Case 2
                Me.PhotoBox2.Visible = False
                Me.PhotoBox3.Visible = False
                Me.PhotoBox4.Visible = False
            Case 3
                Me.PhotoBox3.Visible = False
                Me.PhotoBox4.Visible = False
            Case 4
                Me.PhotoBox4.Visible = False

        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} End",
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
End Class

