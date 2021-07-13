
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170209.aspx.vb
'─────────────────────────────────────
'機能： 追加作業サムネイル
'補足： 
'作成： 2013/12/03 SKFC 橋本 
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.SC3170209
Imports Toyota.eCRB.iCROP.DataAccess.SC3170209
Imports System.Globalization

Partial Class Pages_SC3170209
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    Private Const C_FUNCTION_ID As String = "SC3170209"

    ''' <summary>
    ''' 元画面引数のキー名：販売店コード
    ''' </summary>
    Private Const C_REQ_STRING_DLR_CD As String = "DealerCode"

    ''' <summary>
    ''' 元画面引数のキー名：店舗コード
    ''' </summary>
    Private Const C_REQ_STRING_BRN_CD As String = "BranchCode"

    ''' <summary>
    ''' 元画面引数のキー名：来店実績連番
    ''' </summary>
    Private Const C_REQ_STRING_VISIT_SEQ As String = "SAChipID"

    ''' <summary>
    ''' 元画面引数のキー名：基幹予約ID
    ''' </summary>
    Private Const C_REQ_STRING_BASREZ_ID As String = "BASREZID"

    ''' <summary>
    ''' 元画面引数のキー名：R/ONO
    ''' </summary>
    Private Const C_REQ_STRING_RO_NUM As String = "R_O"

    ''' <summary>
    ''' 元画面引数のキー名：R/O枝番
    ''' </summary>
    Private Const C_REQ_STRING_RO_SEQ_NUM As String = "SEQ_NO"

    ''' <summary>
    ''' 元画面引数のキー名：VIN
    ''' </summary>
    Private Const C_REQ_STRING_VIN_NUM As String = "VIN_NO"

    ''' <summary>
    ''' 元画面引数のキー名：写真区分
    ''' </summary>
    Private Const C_REQ_STRING_PICTURE_GROUP As String = "PictMode"

    ''' <summary>
    ''' 元画面引数のキー名：撮影区分
    ''' </summary>
    Private Const C_REQ_STRING_CAPTURE_GROUP As String = "CaptMode"

    ''' <summary>
    ''' 元画面引数のキー名：ログインユーザーID
    ''' </summary>
    Private Const C_REQ_STRING_LOGIN_USER_ID As String = "LoginUserID"

    ''' <summary>
    ''' 元画面引数のキー名：呼出元システム区分
    ''' </summary>
    Private Const C_REQ_STRING_LINK_SYS_TYPE As String = "LinkSysType"

    ''' <summary>
    ''' 呼出元システム区分：基幹（規定値）
    ''' </summary>
    Private Const C_LINK_SYS_TYPE_DMS As String = "0"

    ''' <summary>
    ''' 呼出元システム区分：e-CRB
    ''' </summary>
    Private Const C_LINK_SYS_TYPE_ECRB As String = "1"

    ''' <summary>
    ''' 写真区分：追加作業
    ''' </summary>
    Private Const C_PICTURE_GROUP_ADD As String = "1"

    ''' <summary>
    ''' 写真区分：外観チェック
    ''' </summary>
    Private Const C_PICTURE_GROUP_EXTE As String = "2"

    ''' <summary>
    ''' 撮影区分：追加作業（規定値）
    ''' </summary>
    Private Const C_CAPTURE_GROUP_ADD As String = "1"

    ''' <summary>
    ''' TB_M_PROGRAM_SETTINGのキー名：スーパードメイン
    ''' </summary>
    Private Const C_SETTINGKEY_SUPERDOMAIN As String = "SuperDomain"

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

        Dim dlrCd As String = String.Empty
        Dim brnCd As String = String.Empty
        Dim visitSeq As Long = -1
        Dim basrezId As String = String.Empty
        Dim roNo As String = String.Empty
        Dim roSeqNo As Long = 0
        Dim vinNo As String = String.Empty
        Dim pictureGroup As String = String.Empty
        Dim captureGroup As String = String.Empty
        Dim loginUserId As String = String.Empty
        Dim linkSysType As String = String.Empty
        Dim pictureFormat As String = String.Empty

        '画像アップロードフラグ初期化
        If String.IsNullOrEmpty(Me.Hidden_UploadFlag.Value) Then
            Me.Hidden_UploadFlag.Value = "0"
        End If

        '初期表示
        If (Not IsPostBack AndAlso Not IsCallback) Then
            Logger.Info("Request URL=" & Request.RawUrl)

            'メッセージ取得
            Me.Hidden_MessageSaveImageFailure.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(C_FUNCTION_ID, 901))

            '呼び出し元から引数を取得
            Me.GetRequestValue(dlrCd, brnCd, visitSeq, basrezId, roNo, roSeqNo, vinNo, pictureGroup, captureGroup, loginUserId, linkSysType)

            'クライアント側の初期化処理
            Me.InitializeClient()

        Else
            dlrCd = Me.Hidden_DlrCd.Value
            brnCd = Me.Hidden_BrnCd.Value
            visitSeq = CLng(Me.Hidden_VisitSeq.Value)
            basrezId = Me.Hidden_BasrezId.Value
            roNo = Me.Hidden_RoNo.Value
            roSeqNo = CLng(Me.Hidden_RoSeqNo.Value)
            vinNo = Me.Hidden_VinNo.Value
            pictureGroup = Me.Hidden_PictureGroup.Value
            captureGroup = Me.Hidden_CaptureGroup.Value
            linkSysType = Me.Hidden_LinkSysType.Value

        End If

        'ビジネスロジッククラス
        Dim clsBizLogic As New SC3170209BusinessLogic

        'RO外装ダメージマスタ情報取得
        Dim dtThumbnail As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable
        dtThumbnail = clsBizLogic.GetRoThumbnailImgInfo(dlrCd, brnCd, visitSeq, basrezId, roNo, roSeqNo, vinNo, pictureGroup, linkSysType)

        'ファイルの拡張子を取得
        pictureFormat = clsBizLogic.GetSystemSettingValue()
        Me.Hidden_PictureFormat.Value = pictureFormat.Replace("{0}", "")

        If Me.Hidden_UploadFlag.Value.Equals("0") Then
            '非画像アップロード時はクルクルを非表示にする
            Me.LoadingScreen.Style.Remove("display")
            Me.LoadingScreen.Style.Add("display", "none")
        Else
            '画像アップロード中はクルクルを表示する
            Me.LoadingScreen.Style.Remove("display")
        End If

        Me.SetDisplayView(dtThumbnail)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


#Region "初期表示"

    ''' <summary>
    ''' 呼び出し元からリクエストを取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetRequestValue(ByRef dlrCd As String,
                                ByRef brnCd As String,
                                ByRef visitSeq As Long,
                                ByRef basrezId As String,
                                ByRef roNo As String,
                                ByRef roSeqNo As Long,
                                ByRef vinNo As String,
                                ByRef pictureGroup As String,
                                ByRef captureGroup As String,
                                ByRef loginUserId As String,
                                ByRef linkSysType As String
                                )

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '販売店コード（必須）
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_DLR_CD)) Then
            dlrCd = Request(C_REQ_STRING_DLR_CD).Trim()
            Me.Hidden_DlrCd.Value = dlrCd
        Else
            Throw New ApplicationException
        End If

        '店舗コード（必須）
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_BRN_CD)) Then
            brnCd = Request(C_REQ_STRING_BRN_CD).Trim()
            Me.Hidden_BrnCd.Value = brnCd
        Else
            Throw New ApplicationException
        End If

        '来店実績連番（来店実績連番とR/ONOのいずれもNULLの場合エラー）
        Dim isNotNullParam As Boolean = False
        Dim strVisitSeq As String = String.Empty
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_VISIT_SEQ)) Then
            isNotNullParam = Long.TryParse(Request(C_REQ_STRING_VISIT_SEQ).Trim(), visitSeq)
        End If
        Me.Hidden_VisitSeq.Value = visitSeq

        '基幹予約ID
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_BASREZ_ID)) Then
            basrezId = Request(C_REQ_STRING_BASREZ_ID).Trim()
            Me.Hidden_BasrezId.Value = basrezId
        End If

        'R/ONO
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_RO_NUM)) Then
            roNo = Request(C_REQ_STRING_RO_NUM).Trim()
            Me.Hidden_RoNo.Value = roNo

            isNotNullParam = True
        End If

        'R/O枝番
        Dim strRoSeqNo As String = Request(C_REQ_STRING_RO_SEQ_NUM)
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_RO_SEQ_NUM)) Then
            If Not Long.TryParse(Request(C_REQ_STRING_RO_SEQ_NUM).Trim(), roSeqNo) Then
                Throw New ApplicationException("SEQ_NO is not number.")
            End If
        End If
        Me.Hidden_RoSeqNo.Value = roSeqNo

        'VIN
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_VIN_NUM)) Then
            vinNo = Request(C_REQ_STRING_VIN_NUM).Trim()
            Me.Hidden_VinNo.Value = vinNo
        End If

        '写真区分
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_PICTURE_GROUP)) Then
            pictureGroup = Request(C_REQ_STRING_PICTURE_GROUP).Trim()
            Me.Hidden_PictureGroup.Value = pictureGroup
        End If

        '撮影区分
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_CAPTURE_GROUP)) Then
            captureGroup = Request(C_REQ_STRING_CAPTURE_GROUP).Trim()
        Else
            'リクエスト未指定の場合は、写真区分を利用する
            captureGroup = pictureGroup
        End If
        If String.IsNullOrEmpty(captureGroup) Then
            '値が指定されていない場合は、規定値を使用する
            captureGroup = C_CAPTURE_GROUP_ADD
        End If
        Me.Hidden_CaptureGroup.Value = captureGroup

        'ログインユーザーID
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_LOGIN_USER_ID)) Then
            loginUserId = Request(C_REQ_STRING_LOGIN_USER_ID).Trim()
            Me.Hidden_LoginUserId.Value = loginUserId
        End If

        If Not isNotNullParam Then
            Throw New ApplicationException
        End If

        '呼出元システム区分
        If Not String.IsNullOrEmpty(Request(C_REQ_STRING_LINK_SYS_TYPE)) Then
            linkSysType = Request(C_REQ_STRING_LINK_SYS_TYPE).Trim()
        Else
            linkSysType = C_LINK_SYS_TYPE_DMS
        End If
        Me.Hidden_LinkSysType.Value = linkSysType

        '基幹連携の場合
        If linkSysType = C_LINK_SYS_TYPE_DMS Then
            'ビジネスロジッククラス
            Dim clsBizLogic As New SC3170209BusinessLogic

            '基幹のコードに置き換え
            clsBizLogic.ChangeDmsCode(dlrCd, brnCd)
            Me.Hidden_DlrCd.Value = dlrCd
            Me.Hidden_BrnCd.Value = brnCd

            'LinkSysTypeをeCRBに変更する
            linkSysType = C_LINK_SYS_TYPE_ECRB
            Me.Hidden_LinkSysType.Value = linkSysType

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [dlrCd:{1}][brnCd:{2}][visitSeq:{3}][basrezId:{4}][roNo:{5}]" & _
                                  "[roSeqNo:{6}][vinNo:{7}][pictureGroup:{8}][captureGroup:{9}][LoginUserID:{10}][LinkSysType:{11}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  dlrCd, brnCd, strVisitSeq, basrezId, roNo, strRoSeqNo, vinNo, pictureGroup, captureGroup, loginUserId, linkSysType))

    End Sub

#End Region

#Region "表示設定"

    ''' <summary>
    ''' 表示設定
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub SetDisplayView(ByVal dt As SC3170209DataSet.TB_T_RO_THUMBNAILDataTable)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} Start [dtThumbnailRowsCount:{1}]",
                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                          dt.Rows.Count))

        Dim clsBizLogic As New SC3170209BusinessLogic
        Dim aryFilePath As New ArrayList

        '取得した画像パス
        For Each dr As SC3170209DataSet.TB_T_RO_THUMBNAILRow In dt.Rows
            aryFilePath.Add(dr.THUMBNAIL_IMG_PATH)
        Next
        'パスが存在する場合、相対パスを取得
        If aryFilePath.Count > 0 Then
            clsBizLogic.GetImageFileRelativePath(aryFilePath)
        End If

        Dim count As Integer = 0
        Dim img1FilePath As String = String.Empty
        Dim img2FilePath As String = String.Empty
        Dim dtImgPath As New SC3170209DataSet.ImageSorceDataTable

        '取得したパス分
        For Each filePath As String In aryFilePath
            count += 1

            '偶数の場合は行追加
            If count Mod 2 = 0 Then
                dtImgPath.AddImageSorceRow(img1FilePath, filePath)

            Else
                '奇数
                img1FilePath = filePath

                '奇数で最後の行
                If count = aryFilePath.Count Then
                    dtImgPath.AddImageSorceRow(img1FilePath, String.Empty)

                End If
            End If
        Next

        Me.ListView_ThumbnailImgList.DataSource = dtImgPath
        Me.ListView_ThumbnailImgList.DataBind()

        '表示枚数を設定
        Me.Label_PhotoCount.Text = aryFilePath.Count

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "{0} End",
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "カメラ機能"

    ''' <summary>
    ''' カメラボタン押下時（カメラ機能後）
    ''' </summary>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Function Botton_Camera_Click() As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '新ファイルパスを作成
        Dim clsBizLogic As New SC3170209BusinessLogic

        'ROサムネイルID取得
        Dim roThumbnailId As Decimal

        '新ファイルパスを取得
        Dim filePath As String = clsBizLogic.CleateNewFilePath(roThumbnailId)

        'カメラのみ「.png」を消す
        filePath = filePath.Remove(filePath.LastIndexOf("."))

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [newFilePath:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  filePath))

        Return filePath

    End Function


    ''' <summary>
    ''' カメラ起動後に新ファイルパスを返却
    ''' </summary>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod()>
    Public Shared Sub TakePhotoAfter(ByVal roThumbnailId As Decimal,
                                     ByVal dlrCd As String,
                                     ByVal brnCd As String,
                                     ByVal visitSeq As Long,
                                     ByVal basrezId As String,
                                     ByVal roNum As String,
                                     ByVal roSeqNum As Long,
                                     ByVal vinNum As String,
                                     ByVal pictureGroup As String,
                                     ByVal loginUserId As String,
                                     ByVal thumbnailImgPath As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} {1}Start [roThumbnailId:{2}][dlrCd:{3}][brnCd:{4}][visitSeq:{5}][basrezId:{6}][roNum:{7}]" & _
                                  "[roSeqNum:{8}][vinNum:{9}][pictureGroup:{10}][loginUserId:{11}]",
                                  C_FUNCTION_ID, System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  roThumbnailId, dlrCd, brnCd, visitSeq, basrezId, roNum, roSeqNum, vinNum, pictureGroup, loginUserId))

        Dim isSuccess As Boolean
        Dim clsBizLogic As New SC3170209BusinessLogic

        'ROサムネイル画像登録
        isSuccess = clsBizLogic.SetRoThumbnailImgInfo(roThumbnailId,
                                                      dlrCd,
                                                      brnCd,
                                                      visitSeq,
                                                      basrezId,
                                                      roNum,
                                                      roSeqNum,
                                                      vinNum,
                                                      pictureGroup,
                                                      loginUserId,
                                                      thumbnailImgPath)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End [IsSuccess:{1}]",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
                                  isSuccess))

    End Sub

#End Region

#Region "クライアント処理"

    ''' <summary>
    ''' クライアント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeClient()

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim clsTblAdapter As New SC3170209TableAdapter

        'スーパードメイン
        Dim superDomain As String = clsTblAdapter.GetProgramSettingValue(C_FUNCTION_ID, , C_SETTINGKEY_SUPERDOMAIN)

        Dim initilizeScript As New StringBuilder
        With initilizeScript
            'JS生成
            .Append("<script type='text/javascript'>")

            'スーパードメイン設定
            If Not String.IsNullOrEmpty(superDomain) Then .Append(" document.domain = """ & superDomain & """;")

            .Append("</script>")
        End With

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "init", initilizeScript.ToString)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

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

        'Javascript関数実行
        Dim scriptLine As String = "<script type='text/javascript'>window.onload=" & method & ";</script>"

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "run", scriptLine)

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End",
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

End Class

