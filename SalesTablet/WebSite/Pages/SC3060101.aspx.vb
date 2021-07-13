'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3060101.aspx.vb
'─────────────────────────────────────
'機能： 査定チェックシート
'補足： 
'作成： 2011/11/29 KN 清水
'更新： 2012/03/19 KN 清水  【SALES_1B】SALES_1B UT(課題No.0023) TCV遷移対応
'更新： 2012/05/16 KN 浅野　HTMLエンコード対応
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $02
'─────────────────────────────────────


Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

Imports Toyota.eCRB.Assessment.Assessment.BizLogic
Imports Toyota.eCRB.Assessment.Assessment.BizLogic.SC3060101BusinessLogic
Imports Toyota.eCRB.Assessment.Assessment.DataAccess
Imports Toyota.eCRB.Assessment.Assessment.DataAccess.SC3060101DataSet

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


Partial Class Pages_SC3060101
    Inherits BasePage
    Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl



#Region "定数"

    ''' <summary>車両画像情報・画像№</summary>
    Protected Const ColumnNamePhotoNo As String = "PhotoNo"
    ''' <summary>車両画像情報・サムネイル画像</summary>
    Protected Const ColumnNameSmallPhoto As String = "SmallPhoto"
    ''' <summary>車両画像情報・拡大画像</summary>
    Protected Const ColumnNameBigPhoto As String = "BigPhoto"

    ''' <summary>車両装備情報・装備番号</summary>
    Protected Const ColumnNameOptionNumber As String = "optionNo"
    ''' <summary>車両装備情報・装備文言</summary>
    Protected Const ColumnNameOptionWord As String = "optionWord"

    ''' <summary>外装損傷情報・外装コード</summary>
    Protected Const ColumnNameRegionCode As String = "PartsCode"
    ''' <summary>外装損傷情報・評価</summary>
    Protected Const ColumnNameRating As String = "Rating"

    ''' <summary>XML定義項目・環境情報</summary>
    Private Const ColumnNameParameter As String = "PARAM"
    ''' <summary>DataSetロケール</summary>
    Private Const DataSetCulture As String = "ja-JP"
    ''' <summary>画面ID</summary>
    Private Const APPLICATIONID As String = "SC3060101"
    ''' <summary>遷移パラメータ・依頼ID</summary>
    Private Const SESSION_KEY_REQUESTID As String = "SearchKey.REQUESTID"
    ''' <summary>遷移パラメータ・査定No</summary>
    Private Const SESSION_KEY_ASSESSMENTNO As String = "SearchKey.ASSESSMENTNO"
    ''' <summary>XML定義項目・タグ名・車両情報</summary>
    Private Const TableNameCarInfo As String = "CarInfo"
    ''' <summary>XML定義項目・タグ名・装備情報</summary>
    Private Const TableNameEquipment As String = "Equipment"
    ''' <summary>XML定義項目・タグ名・画像情報</summary>
    Private Const TableNameImageIngo As String = "CarPicture"
    ''' <summary>XML定義項目・タグ名・環境情報</summary>
    Private Const TableNameEnv As String = "ENV"
    ''' <summary>XML定義項目・タグ名・外装損傷情報</summary>
    Private Const TableNameOuter As String = "Outer"
    ''' <summary>XML定義項目・主要装備・ワンオーナー</summary>
    Private Const ColumnNameOneOwner As String = "OneOwner"
    ''' <summary>XML定義項目・主要装備・修復暦</summary>
    Private Const ColumnNameRepairHistory As String = "Repairhis"
    ''' <summary>XML定義項目・主要装備・整備手帳</summary>
    Private Const ColumnNameRegisterBook As String = "RegisterBook"
    ''' <summary>XML定義項目・主要装備・本革シート</summary>
    Private Const ColumnNameLeatherSeat As String = "SeatPurehide"
    ''' <summary>XML定義項目・主要装備・サンルーフ</summary>
    Private Const ColumnNameSunroof As String = "SunRoof"
    ''' <summary>XML定義項目・主要装備・ディスチャージランプ</summary>
    Private Const ColumnNameDischargeLamp As String = "DischargeLamp"
    ''' <summary>XML定義項目・主要装備・アルミホイール</summary>
    Private Const ColumnNameAluminumWheel As String = "AlumiWheel"
    ''' <summary>XML定義項目・主要装備・エアロ</summary>
    Private Const ColumnNameAppearance As String = "Appearance"
    ''' <summary>XML定義項目・主要装備・キーレス</summary>
    Private Const ColumnNameKeyless As String = "KeyLess"
    ''' <summary>XML定義項目・主要装備・カーナビ</summary>
    Private Const ColumnNameCarNavigation As String = "CarNavi"
    ''' <summary>XML定義項目・主要装備・車載テレビ</summary>
    Private Const ColumnNameTV As String = "Tv"
    ''' <summary>XML定義項目・主要装備・バックモニター</summary>
    Private Const ColumnNameBackMonitor As String = "BackMonitor"
    ''' <summary>XML定義項目・主要装備・DVDプレイヤー</summary>
    Private Const ColumnNameDvdPlayer As String = "DvdPlayer"
    ''' <summary>XML定義項目・主要装備・CDプレイヤー</summary>
    Private Const ColumnNameAudioCompactDisc As String = "AudioCd"
    ''' <summary>XML定義項目・主要装備・MDプレイヤー</summary>
    Private Const ColumnNameAudioMinidisc As String = "AudioMd"
    ''' <summary>XML定義項目・主要装備・カセットプレイヤー</summary>
    Private Const ColumnNameAudiotape As String = "AudioTape"
    ''' <summary>XML定義項目・主要装備・パワステ</summary>
    Private Const ColumnNamePowerSteering As String = "PowerSteering"
    ''' <summary>XML定義項目・主要装備・パワーシート</summary>
    Private Const ColumnNamePowerSeat As String = "PowerSeat"
    ''' <summary>XML定義項目・主要装備・パワーウィンドウ</summary>
    Private Const ColumnNamePowerWindow As String = "PowerWindow"
    ''' <summary>XML定義項目・主要装備・エアバッグ</summary>
    Private Const ColumnNameAirbag As String = "AirBag"
    ''' <summary>XML定義項目・主要装備・ABS</summary>
    Private Const ColumnNameAbs As String = "Abs"
    ''' <summary>XML定義項目・主要装備・ESC</summary>
    Private Const ColumnNameElectronicStabilityControl As String = "EstabilCtrl"
    ''' <summary>XML定義項目・主要装備・輸入</summary>
    Private Const ColumnNameSales As String = "Sales"
    ''' <summary>XML定義項目・主要装備・ESC</summary>
    Private Const ColumnNamePurpose As String = "Purpose"
    ''' <summary>XML定義項目・主要装備・メーター改竄</summary>
    Private Const ColumnNameMeterChange As String = "MeterChange"
    ''' <summary>XML定義項目・主要装備・保証書</summary>
    Private Const ColumnNameWarranty As String = "Warranty"
    ''' <summary>XML定義項目・主要装備・取扱説明書</summary>
    Private Const ColumnNameInstruction As String = "Instruction"
    ''' <summary>XML定義項目・車両情報・メーカー名</summary>
    Private Const ColumnNameMakerName As String = "MakerName"
    ''' <summary>XML定義項目・車両情報・車名</summary>
    Private Const ColumnNameVehicleName As String = "VehicleName"
    ''' <summary>XML定義項目・車両情報・グレード</summary>
    Private Const ColumnNameGrade As String = "Grade"
    ''' <summary>XML定義項目・車両情報・年式</summary>
    Private Const ColumnNameModelYear As String = "ModelYear"
    ''' <summary>XML定義項目・車両情報・走行距離</summary>
    Private Const ColumnNameMileage As String = "Mileage"
    ''' <summary>XML定義項目・車両情報・査定価格</summary>
    Private Const ColumnNamePrice As String = "apprisalPrice"
    ''' <summary>XML定義項目・車両情報・コメント</summary>
    Private Const ColumnNameContent As String = "content"
    ''' <summary>XML定義項目・車両情報・査定有効期限</summary>
    Private Const ColumnNameInspectLimit As String = "InspectLimit"
    ''' <summary>XML定義項目・車両情報・査定士名</summary>
    Private Const ColumnNameInspecter As String = "Inspecter"
    ''' <summary>XML定義項目・主要装備・査定士写真</summary>
    Private Const ColumnNameInspectorPhoto As String = "InspecterPhoto"
    ''' <summary> 自社客/未取引客フラグ (1：自社客)</summary>
    Private Const ORGCUSTFLG As String = "1"
    ''' <summary>自社客/未取引客フラグ (2：未取引客)</summary>
    Private Const NEWCUSTFLG As String = "2"

    ''' <summary>
    ''' メインメニュー画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_MAINMENU As String = "SC3010203"

    ''' <summary>
    ''' 顧客詳細画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_DISPID_CUST As String = "SC3080201"


    ''' <summary>
    ''' TRUE
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TRUE As String = "TRUE"


    ''' <summary>
    ''' 活動ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SALES_START As Integer = 1
    Private Const C_SALES_CANCEL As Integer = 2
    Private Const C_BUSINESS_START As Integer = 3
    Private Const C_BUSINESS_CANCEL As Integer = 4
    Private Const C_CORRESPOND_START As Integer = 5
    Private Const C_CORRESPOND_END As Integer = 6

    ''' <summary>
    ''' 開始ログ    
    ''' </summary>
    ''' <remarks>ログ出力用(開始)</remarks>
    Private Const STARTLOG As String = "START "

    ''' <summary>
    ''' 終了ログ
    ''' </summary>
    ''' <remarks>ログ出力用(終了)</remarks>
    Private Const ENDLOG As String = "END "

    ''' <summary>
    ''' 終了ログRETURN
    ''' </summary>
    ''' <remarks>ログ出力用(終了)</remarks>
    Private Const ENDLOGRETURN As String = "RETURN "


    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"
    ''' <summary>VIN</summary>
    Private Const SESSION_KEY_VCLID As String = "SearchKey.VCLID"
    ''' <summary>FBOX SEQNO</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>Follow-upBoxの店舗コード</summary>
    Private Const SESSION_KEY_FLLWUPBOX_STRCD As String = "SearchKey.FLLWUPBOX_STRCD"
    ''' <summary>顧客氏名</summary>
    Private Const SESSION_KEY_CUSTNAME As String = "SearchKey.NAME"

    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 START
    ''' <summary>契約状況</summary>
    Private Const CONTRACT As String = "1"
    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 END


#End Region


    ''' <summary>
    ''' ロード時の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Const METHODNAME As String = "Page_Load "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        'stubSessionValue()

        'セッション情報のログ出力
        SessionLog()

        If Not Page.IsPostBack Then
            '顧客氏名の取得
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTNAME, getCstName())
            '検索ボックス設定
            InitSearchBox()
        End If

        'ヘッダーボタンの制御
        InitHeaderEvent()

        'フッターボタンの制御
        InitFooterEvent()

        If Not Page.IsPostBack Then


            Dim crcustid As String = ""
            Dim followUpBox As String = ""
            Dim followUpStoreCd As String = ""

            If IsSession(SESSION_KEY_CRCUSTID) Then
                crcustid = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            End If

            If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                followUpBox = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False), String)
            End If

            If IsSession(SESSION_KEY_FLLWUPBOX_STRCD) Then
                followUpStoreCd = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False), String)
            End If

            SetActivityControl(crcustid, followUpBox, followUpStoreCd)


            SetDisplayWord()

            Dim requestId As String = ""
            Dim assessmentNo As String = ""

            '画面間パラメータを取得
            If IsSession(SESSION_KEY_REQUESTID) Then
                requestId = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_REQUESTID, False), String)
            End If
            If IsSession(SESSION_KEY_ASSESSMENTNO) Then
                assessmentNo = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_ASSESSMENTNO, False), String)
            End If

            ''ログインユーザー情報の取得
            Dim context As StaffContext = StaffContext.Current

            Dim BusinessLogic As SC3060101BusinessLogic = New SC3060101BusinessLogic
            Dim ds As System.Data.DataSet = BusinessLogic.GetAssessmentInfo(context.DlrCD, context.BrnCD, requestId, assessmentNo)

            'エラー判定
            If SC3060101BusinessLogic.ErrorCodeFinish = BusinessLogic.ResultId Then
                ' 画面への情報セット
                SetDispParts(ds)
            Else
                'エラ－ログ出力
                Dim endLogInfo2 As New StringBuilder
                endLogInfo2.Append(METHODNAME)
                endLogInfo2.Append(ENDLOG)
                endLogInfo2.Append(ENDLOGRETURN)
                endLogInfo2.Append(BusinessLogic.ResultId)
                Logger.Error(endLogInfo2.ToString)

                'IF呼び出しエラー時メッセージ表示
                If SC3060101BusinessLogic.ErrorCodeSend = BusinessLogic.ResultId Then
                    ShowMessageBox(41)
                    'パラメータ設定エラー時メッセージ表示
                Else
                    ShowMessageBox(40)
                End If
            End If
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub

    ''' <summary>
    ''' セッション情報のログ出力
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SessionLog()

        Const METHODNAME As String = "SessionLog "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        If IsSession(SESSION_KEY_CRCUSTID) Then
            Logger.Info("SESSION_KEY_CRCUSTID= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False))
        Else
            Logger.Info("SESSION_KEY_CRCUSTID= ")
        End If

        If IsSession(SESSION_KEY_CSTKIND) Then
            Logger.Info("SESSION_KEY_CSTKIND= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False))
        Else
            Logger.Info("SESSION_KEY_CSTKIND= ")
        End If

        If IsSession(SESSION_KEY_CUSTOMERCLASS) Then
            Logger.Info("SESSION_KEY_CUSTOMERCLASS= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False))
        Else
            Logger.Info("SESSION_KEY_CUSTOMERCLASS= ")
        End If

        If IsSession(SESSION_KEY_SALESSTAFFCD) Then
            Logger.Info("SESSION_KEY_SALESSTAFFCD= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False))
        Else
            Logger.Info("SESSION_KEY_SALESSTAFFCD= ")
        End If

        If IsSession(SESSION_KEY_VCLID) Then
            Logger.Info("SESSION_KEY_VCLID= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_VCLID, False))
        Else
            Logger.Info("SESSION_KEY_VCLID= ")
        End If

        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            Logger.Info("SESSION_KEY_FOLLOW_UP_BOX= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False))
        Else
            Logger.Info("SESSION_KEY_FOLLOW_UP_BOX= ")
        End If

        If IsSession(SESSION_KEY_FLLWUPBOX_STRCD) Then
            Logger.Info("SESSION_KEY_FLLWUPBOX_STRCD= " & Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False))
        Else
            Logger.Info("SESSION_KEY_FLLWUPBOX_STRCD= ")
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub

    ''' <summary>
    ''' 画面への情報セット
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    Private Sub SetDispParts(ByVal ds As System.Data.DataSet)

        Const MILEAGE_UNIT As String = "km"
        Const NUM_FORMAT As String = "#,##0"
        Const REPLACE_WORD_HAIHUN As String = "-"

        '主要装備テーブルセット
        SetOptions(ds)

        '車両情報セット
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
        'メーカー名
        MakerLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameMakerName), String), _
                                           7, _
                                           False))

        '車名
        VehicleLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameVehicleName), String), _
                                           8, _
                                           False))

        'グレード
        GradeLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameGrade), String), _
                                           10, _
                                           False))
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 END

        '年式
        Dim modelYear As String = DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameModelYear), String)
        Dim year As Integer
        Dim month As Integer
        If modelYear IsNot Nothing AndAlso modelYear.Length > 5 Then
            year = CInt(modelYear.Substring(0, 4))
            month = CInt(modelYear.Substring(4, 2))

            ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
            ModelYearLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(DateTimeFunc.FormatDate(10, New Date(year, month, 1)), _
                                           10, _
                                           False))
            ' 2012/05/16 KN 浅野　HTMLエンコード対応 END
        Else
            ModelYearLabel.Text = REPLACE_WORD_HAIHUN
        End If

        '走行距離
        Dim mileage As String = DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameMileage), String)
        If mileage IsNot Nothing AndAlso mileage.Length > 0 Then
            ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
            MileageLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(Format(CLng(mileage), NUM_FORMAT) & MILEAGE_UNIT, _
                                           12, _
                                           False))
            ' 2012/05/16 KN 浅野　HTMLエンコード対応 END
        Else
            MileageLabel.Text = REPLACE_WORD_HAIHUN
        End If

        '車両画像
        CarPhoto.ImageUrl = GetCarImage(ds.Tables(TableNameImageIngo), ds.Tables(TableNameEnv), 0, False).ToString
        If String.IsNullOrEmpty(CarPhoto.ImageUrl) Then
            CarPhoto.Visible = False
        Else
            CarPhoto.Visible = True
        End If

        '査定情報
        SetInspect(ds)
        '展開図
        SetOuterPoint(ds)
        '車両サムネイル画像
        SetCarImage(ds)

    End Sub
    ''' <summary>
    ''' サムネイル画像部分の情報セット
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    Private Sub SetCarImage(ByVal ds As System.Data.DataSet)
        Dim culture As System.Globalization.CultureInfo
        culture = New System.Globalization.CultureInfo(DataSetCulture)

        '車両サムネイル画像のセット
        Using imageDataSource As New System.Data.DataTable
            imageDataSource.Locale = culture
            imageDataSource.Columns.Add(ColumnNamePhotoNo)
            imageDataSource.Columns.Add(ColumnNameSmallPhoto)
            imageDataSource.Columns.Add(ColumnNameBigPhoto)

            If ds.Tables(TableNameImageIngo) IsNot Nothing Then

                For i As Integer = 0 To ds.Tables(TableNameImageIngo).Rows.Count - 1

                    Dim row As System.Data.DataRow

                    row = imageDataSource.NewRow
                    row.Item(ColumnNamePhotoNo) = i
                    row.Item(ColumnNameSmallPhoto) = GetCarImage(ds.Tables(TableNameImageIngo), ds.Tables(TableNameEnv), i, False)
                    row.Item(ColumnNameBigPhoto) = GetCarImage(ds.Tables(TableNameImageIngo), ds.Tables(TableNameEnv), i, True)

                    imageDataSource.Rows.Add(row)

                Next

                'サムネイル画像のDivの開始位置を算出 = 画面左端からのマージン + (サムネイル画像幅 *(最大画像枚数 - 表示画像枚数) / 2
                Dim mergin As Integer = 205 + (73 * (10 - imageDataSource.Rows.Count) / 2)

                SumDiv.Style.Value = "z-index:110;position:absolute; left:" + CStr(mergin) + "px"

                ImageRepeater.DataSource = imageDataSource
                ImageRepeater.DataBind()

            End If
        End Using

    End Sub
    ''' <summary>
    ''' 査定価格部分の情報セット
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    Private Sub SetInspect(ByVal ds As System.Data.DataSet)


        Dim year As Integer
        Dim month As Integer

        Const NUM_FORMAT As String = "#,##0"
        Const REPLACE_WORD_HAIHUN As String = "-"

        '査定価格セット
        Dim price As String = DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNamePrice), String)
        If price IsNot Nothing AndAlso price.Length > 0 Then
            ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
            PriceLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(Format(CLng(price), NUM_FORMAT), _
                                           13, _
                                           False))
            ' 2012/05/16 KN 浅野　HTMLエンコード対応 END
        Else
            PriceLabel.Text = REPLACE_WORD_HAIHUN
        End If

        'コメント
        MemoLabel.Text = EditDisplayWord(DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameContent), String), _
                                           47, _
                                           True)
        'コメントの改行文字をBRに置換
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
        Dim memoArray As String() = MemoLabel.Text.Split(vbCrLf)
        Dim memo As String = String.Empty
        For Each Val As String In memoArray
            memo = memo & HttpUtility.HtmlEncode(Val) & "<BR/>"
        Next

        ' 最後の<BR/>タグを消しつつ設定
        MemoLabel.Text = memo.TrimEnd("<BR/>")
        'MemoLabel.Text = MemoLabel.Text.Replace(vbCrLf, "<BR/>")
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 END

        '査定有効期限
        Dim inspectLimit As String = DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameInspectLimit), String)
        Dim day As Integer

        inspectLimit = inspectLimit.Replace("/", "")

        If inspectLimit IsNot Nothing AndAlso inspectLimit.Length > 7 Then
            year = CInt(inspectLimit.Substring(0, 4))
            month = CInt(inspectLimit.Substring(4, 2))
            day = CInt(inspectLimit.Substring(6, 2))

            InspectLimitLabel.Text = DateTimeFunc.FormatDate(21, New Date(year, month, day))
        Else
            InspectLimitLabel.Text = REPLACE_WORD_HAIHUN
        End If

        ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
        '査定士名
        InspectorLabel.Text = HttpUtility.HtmlEncode(EditDisplayWord(DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameInspecter), String), _
                                           5, _
                                           True))
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 END

        '査定士画像
        Dim inspecterImageUrl As String = GetInspectorImageFile(ds)

        If Not String.IsNullOrEmpty(inspecterImageUrl) Then
            InspectorImage.ImageUrl = inspecterImageUrl
            InspectorImage.Width = "39"
            InspectorImage.Height = "50"
        End If

    End Sub


    ''' <summary>
    ''' 車両画像URLの取得
    ''' </summary>
    ''' <param name="images">画像DataTable</param>
    ''' <param name="environmentTable">環境設定DataTable</param>
    ''' <param name="imageCount">画像の順番</param>
    ''' <param name="bigImage">True：拡大画像を返す。False：サムネイル画像を返す</param>
    ''' <returns>車両画像URL</returns>
    ''' <remarks></remarks>
    Protected Function GetCarImage(ByVal images As System.Data.DataTable, ByVal environmentTable As System.Data.DataTable, ByVal imageCount As Integer, ByVal bigImage As Boolean) As String


        Const METHODNAME As String = "GetCarImage "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim filename As String = ""

        '車両画像テーブルから、サムネイル画像ファイル名を取得
        If images IsNot Nothing AndAlso images.Rows.Count > imageCount Then
            If bigImage Then
                filename = DirectCast(images.Rows(imageCount).Item(ColumnNameBigPhoto), String)
            Else
                filename = DirectCast(images.Rows(imageCount).Item(ColumnNameSmallPhoto), String)
            End If
        End If

        Dim fileUrl As String = ""

        '環境情報テーブルからファイルURLを取得
        If environmentTable IsNot Nothing AndAlso environmentTable.Rows.Count > 1 Then
            fileUrl = DirectCast(environmentTable.Rows(1).Item(ColumnNameParameter), String)
        End If

        'ファイル名か、ファイルURLのどちらかが入っていなければ、ブランクを返す
        If String.IsNullOrEmpty(filename) OrElse String.IsNullOrEmpty(fileUrl) Then
            fileUrl = ""
        Else
            fileUrl = fileUrl & "/" & filename
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(fileUrl)
        Logger.Info(endLogInfo.ToString())


        Return fileUrl

    End Function



    ''' <summary>
    ''' 査定士画像URLの取得
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <returns>査定士画像URL</returns>
    ''' <remarks></remarks>
    Protected Function GetInspectorImageFile(ByVal ds As System.Data.DataSet) As String

        Const METHODNAME As String = "GetInspectorImageFile "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim fileUrl As String = ""

        If ds IsNot Nothing Then
            Dim filename As String = ""

            If ds.Tables(TableNameCarInfo) IsNot Nothing AndAlso ds.Tables(TableNameCarInfo).Rows.Count > 0 Then
                filename = DirectCast(ds.Tables(TableNameCarInfo).Rows(0).Item(ColumnNameInspectorPhoto), String)
            End If


            If ds.Tables(TableNameEnv) IsNot Nothing AndAlso ds.Tables(TableNameEnv).Rows.Count > 1 Then
                fileUrl = DirectCast(ds.Tables(TableNameEnv).Rows(0).Item(ColumnNameParameter), String)
            End If

            'ファイル名か、ファイルURLのどちらかが入っていなければ、ブランクを返す
            If String.IsNullOrEmpty(filename) OrElse String.IsNullOrEmpty(fileUrl) Then
                fileUrl = ""
            Else
                fileUrl = fileUrl & "/" & filename
            End If
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(fileUrl)
        Logger.Info(endLogInfo.ToString())


        Return fileUrl

    End Function

    ''' <summary>
    ''' 外傷損傷点のセット
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    Public Sub SetOuterPoint(ByVal ds As System.Data.DataSet)

        Const REGION_CD_SPARE_TIYA As String = "62"
        Const LESS_VALUE As String = "L"
        Const NONE_VALUE As String = "N"
        Const LESS_DISP As String = "Less"
        Const NONE_DISP As String = "None"

        Const REGIOC_CD_TIYA_YAMA As String = "58"

        Const RATING_P As String = "P"
        Const RATING_X As String = "X"
        Const RATING_B As String = "B"

        Const METHODNAME As String = "SetOuterPoint "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        If ds IsNot Nothing AndAlso ds.Tables(TableNameOuter) IsNot Nothing Then
            Dim dtIconP As System.Data.DataTable = ds.Tables(TableNameOuter).Clone()
            Dim dtIconB As System.Data.DataTable = ds.Tables(TableNameOuter).Clone()
            Dim dtIconX As System.Data.DataTable = ds.Tables(TableNameOuter).Clone()
            Dim dtTiya As System.Data.DataTable = ds.Tables(TableNameOuter).Clone()

            For i As Integer = 0 To ds.Tables(TableNameOuter).Rows.Count - 1

                Dim regionCd As String = DirectCast(ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRegionCode), String)
                Dim rating As String = DirectCast(ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating), String)

                'LはLessはにNはNoneに表記を変更する。
                If REGION_CD_SPARE_TIYA.Equals(regionCd) Then
                    If LESS_VALUE.Equals(rating.Trim) Then
                        ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating) = LESS_DISP
                    End If
                    If NONE_VALUE.Equals(rating.Trim) Then
                        ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating) = NONE_DISP
                    End If
                End If

                'タイヤ山、ライトはアイコンを表示しない。(外装コード「58」以上が対象)
                If REGIOC_CD_TIYA_YAMA <= (regionCd) Then
                    Dim rowTiya As System.Data.DataRow = dtTiya.NewRow()

                    rowTiya.Item(ColumnNameRating) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating)
                    rowTiya.Item(ColumnNameRegionCode) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRegionCode)

                    dtTiya.Rows.Add(rowTiya)
                Else

                    If rating IsNot Nothing AndAlso rating.Length > 1 Then

                        Dim iconMark As String = rating.Substring(0, 1)

                        If RATING_P.Equals(iconMark) Then
                            Dim rowIconP As System.Data.DataRow = dtIconP.NewRow()

                            rowIconP.Item(ColumnNameRating) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating)
                            rowIconP.Item(ColumnNameRegionCode) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRegionCode)

                            dtIconP.Rows.Add(rowIconP)
                        ElseIf RATING_B.Equals(iconMark) Then
                            Dim rowIconB As System.Data.DataRow = dtIconB.NewRow()

                            rowIconB.Item(ColumnNameRating) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating)
                            rowIconB.Item(ColumnNameRegionCode) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRegionCode)

                            dtIconB.Rows.Add(rowIconB)
                        ElseIf RATING_X.Equals(iconMark) Then
                            Dim rowIconX As System.Data.DataRow = dtIconX.NewRow()

                            rowIconX.Item(ColumnNameRating) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRating)
                            rowIconX.Item(ColumnNameRegionCode) = ds.Tables(TableNameOuter).Rows(i).Item(ColumnNameRegionCode)

                            dtIconX.Rows.Add(rowIconX)
                        End If

                    End If

                End If
            Next

            OuterRepeaterP.DataSource = dtIconP
            OuterRepeaterP.DataBind()

            OuterRepeaterB.DataSource = dtIconB
            OuterRepeaterB.DataBind()

            OuterRepeaterX.DataSource = dtIconX
            OuterRepeaterX.DataBind()

            OuterRepeater4.DataSource = dtTiya
            OuterRepeater4.DataBind()

        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


    End Sub

    ''' <summary>
    ''' 画面文言セット
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDisplayWord()

        Const METHODNAME As String = "SetDisplayWord "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        ' 2012/05/16 KN 浅野　HTMLエンコード対応 START
        '各タイトル文言
        CustomLabel2.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 2), 9, True))
        CustomLabel3.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 3), 9, True))
        CustomLabel31.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 31), 9, True))
        CustomLabel33.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 33), 9, True))

        '外装展開図のキズ表記
        CustomLabel34.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 34), 5, False))
        CustomLabel35.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 35), 5, False))
        CustomLabel36.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 36), 5, False))
        CustomLabel37.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 37), 4, False))
        CustomLabel38.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 38), 4, False))
        CustomLabel39.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 39), 4, False))

        '査定有効期限
        CustomLabel32.Text = HttpUtility.HtmlEncode(EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 32), 13, True))
        ' 2012/05/16 KN 浅野　HTMLエンコード対応 END

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


    End Sub

    ''' <summary>
    ''' 画面文言加工
    ''' <param name="terget">ターゲット</param>
    ''' <param name="maxLength">最大文字長</param>
    ''' <param name="cut">True:最大文字長さでカット。False:カットした上で「...」を付与</param>
    ''' </summary>
    ''' <remarks></remarks>
    Private Function EditDisplayWord(ByVal terget As String, ByVal maxLength As Integer, ByVal cut As Boolean) As String

        Const METHODNAME As String = "EditDisplayWord "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Const replaceWordContinue As String = "..."
        Const replaceWordHaihun As String = "-"

        If terget IsNot Nothing Then

            If terget.Length > maxLength Then
                terget = terget.Substring(0, maxLength)

                If Not cut Then
                    terget = terget + replaceWordContinue
                End If

            End If
        Else
            terget = ""
        End If

        If String.IsNullOrEmpty(terget) Then
            terget = replaceWordHaihun
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(terget)
        Logger.Info(endLogInfo.ToString())

        Return terget

    End Function


    ''' <summary>
    ''' 車両装備のセット
    ''' </summary>
    ''' <param name="ds">データセット</param>
    ''' <remarks></remarks>
    Private Sub SetOptions(ByVal ds As System.Data.DataSet)

        Const METHODNAME As String = "SetOptions "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Dim culture As System.Globalization.CultureInfo
        culture = New System.Globalization.CultureInfo(DataSetCulture)

        '車両サムネイル画像のセット
        Using optionDataSource As New System.Data.DataTable
            optionDataSource.Locale = culture
            optionDataSource.Columns.Add("optionNo")
            optionDataSource.Columns.Add("optionWord")

            Using optionDataSource2 As System.Data.DataTable = optionDataSource.Clone

                If ds IsNot Nothing AndAlso ds.Tables(TableNameEquipment) IsNot Nothing AndAlso ds.Tables(TableNameEquipment).Rows.Count > 0 Then

                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameOneOwner), 4, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 4), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameRepairHistory), 5, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 5), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameRegisterBook), 6, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 6), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameLeatherSeat), 7, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 7), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameSunroof), 8, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 8), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameDischargeLamp), 9, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 9), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAluminumWheel), 10, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 10), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAppearance), 11, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 11), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameKeyless), 12, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 12), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameCarNavigation), 13, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 13), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameTV), 14, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 14), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameBackMonitor), 15, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 15), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameDvdPlayer), 16, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 16), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAudioCompactDisc), 17, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 17), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAudioMinidisc), 18, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 18), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAudiotape), 19, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 19), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNamePowerSteering), 20, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 20), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNamePowerSeat), 21, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 21), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNamePowerWindow), 22, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 22), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAirbag), 23, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 23), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameAbs), 24, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 24), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameElectronicStabilityControl), 25, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 25), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameSales), 26, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 26), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNamePurpose), 27, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 27), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameMeterChange), 28, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 28), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameWarranty), 29, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 29), 7, False))
                    optionRowAdd(optionDataSource, optionDataSource2, ds.Tables(TableNameEquipment).Rows(0).Item(ColumnNameInstruction), 30, EditDisplayWord(WebWordUtility.GetWord(APPLICATIONID, 30), 7, False))

                End If

                OptionRepeater1.DataSource = optionDataSource
                OptionRepeater1.DataBind()
                OptionRepeater2.DataSource = optionDataSource2
                OptionRepeater2.DataBind()
            End Using
        End Using


        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


    End Sub

    ''' <summary>
    ''' 車両装備の行作成
    ''' </summary>
    ''' <param name="dt">車両装備データテーブル</param>
    ''' <param name="dt2">車両装備データテーブル</param>
    ''' <param name="value">装備の値</param>
    ''' <param name="number">装備の連番</param>
    ''' <param name="word">装備の文言</param>
    ''' <remarks></remarks>
    Private Sub optionRowAdd(ByVal dt As System.Data.DataTable, ByVal dt2 As System.Data.DataTable, ByVal value As String, ByVal number As Integer, ByVal word As String)

        Const METHODNAME As String = "optionRowAdd "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        If GetOption(value) Then

            Dim row As System.Data.DataRow

            If dt.Rows.Count < 10 Then

                row = dt.NewRow()
                row.Item(ColumnNameOptionNumber) = number
                row.Item(ColumnNameOptionWord) = word


                dt.Rows.Add(row)
            Else

                DivSlideImage1.Visible = True

                row = dt2.NewRow()
                row.Item(ColumnNameOptionNumber) = number
                row.Item(ColumnNameOptionWord) = word

                dt2.Rows.Add(row)
            End If

        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


    End Sub

    ''' <summary>
    ''' 装備済み判定
    ''' </summary>
    ''' <param name="optionsValue">装備の値</param>
    ''' <returns>True:装備している</returns>
    ''' <remarks></remarks>
    Private Function GetOption(ByVal optionsValue As Object) As Boolean

        Const METHODNAME As String = "GetOption "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Const OPTION_MAKER As String = "1"
        Const OPTION_OUT_MAKER As String = "2"

        Dim ret As Boolean = False

        If optionsValue IsNot Nothing Then

            Dim checkOptionsValue As String = DirectCast(optionsValue, String)

            '純正品か社外品であれば装備されていると判断する。
            If OPTION_MAKER.Equals(checkOptionsValue) OrElse OPTION_OUT_MAKER.Equals(checkOptionsValue) Then
                ret = True
            End If
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(ret)
        Logger.Info(endLogInfo.ToString())

        Return ret

    End Function



#Region " フッター制御・ヘッダー制御 "

    'メニューのＩＤを定義
    Private Const CUSTOMER_SEARCH As Integer = 200
    Private Const SUBMENU_TESTDRIVE As Integer = 201
    Private Const SUBMENU_VAL As Integer = 202
    Private Const SUBMENU_HELP As Integer = 203
    Private Const MAIN_MENU As Integer = 100
    Private Const SUBMENU_NEW_CAR_EXPLAIN As Integer = 1500


    ''' <summary>
    ''' フッター作成
    ''' </summary>
    ''' <param name="commonMaster"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        Const METHODNAME As String = "DeclareCommonMasterFooter "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        category = FooterMenuCategory.Customer

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        Return {SUBMENU_TESTDRIVE, SUBMENU_VAL, SUBMENU_HELP}
    End Function



    ''' <summary>
    ''' ヘッダーボタンの定義
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()


        Const METHODNAME As String = "DeclareCommonMasterContextMenu "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '商談中はログアウトすら表示されないので、コンテキストメニューを非活性にする。
        If IsSales() Then
            commonMaster.ContextMenu.Enabled = False
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


        '表示する可能性があるものを全て表示する
        Return New Integer() {CommonMasterContextMenuBuiltinMenuID.LogoutItem}
    End Function


    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        Const METHODNAME As String = "InitHeaderEvent "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        If IsSales() Then
            '戻るボタンを非活性
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = False
            '戻る・進む(商談中はログアウトが無いため)
            For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward}
                '活動破棄チェックのクライアントサイドスクリプトを埋め込む
                CType(Me.Master, CommonMasterPage).GetHeaderButton(buttonId).OnClientClick = "return cancellationCheck();"
            Next

        Else
            '戻るボタンを活性()
            CType(Master, CommonMasterPage).IsRewindButtonEnabled = True
            '戻る・進む・ログアウト
            For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward, HeaderButton.Logout}
                '活動破棄チェックのクライアントサイドスクリプトを埋め込む
                CType(Me.Master, CommonMasterPage).GetHeaderButton(buttonId).OnClientClick = "return cancellationCheck();"
            Next
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Const METHODNAME As String = "InitFooterEvent "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        ' ボタン非活性
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Enabled = False
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL).Enabled = False
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled = False

        ' ボタン非表示
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_NEW_CAR_EXPLAIN).Visible = False

        'ログイン権限がセールススタッフでない場合、フッターボタンを非表示
        Dim OpeCD As Integer = StaffContext.Current.OpeCD
        Dim SSF As Integer = Operation.SSF
        If OpeCD <> SSF Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Visible = False
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL).Visible = False
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Visible = False
            CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).Visible = False
        End If

        '押下時イベント
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).OnClientClick = "divSlideUp();closeBigImage();return false;"
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL).OnClientClick = "divSlideUp();closeBigImage();return showAssessmentPopup();"
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).OnClientClick = "divSlideUp();closeBigImage();return false;"

        CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).OnClientClick = "showLoadingIcon();return true;"

        '商談中・営業活動中・一時対応中の場合、メインメニューを非活性
        If IsSales() Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Enabled = False
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Enabled = False
        Else
            CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Enabled = True
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SPM).Enabled = True
        End If

        'Follow-upBox内連番がある場合、査定依頼・ヘルプ依頼を活性
        If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled = True
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_VAL).Enabled = True
        End If

        '試乗ボタンを活性は常に活性
        CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_TESTDRIVE).Enabled = True

        'ヘルプボタンが非活性の場合、ポップアップを非表示にする
        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_HELP).Enabled Then
            SC3080401.Visible = True
        Else
            SC3080401.Visible = False
        End If

        'メニュー
        AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Click, _
            Sub()

                '開始ログ出力
                Logger.Info("MAIN_MENU.Click START")

                'メニューに遷移
                Me.RedirectNextScreen(STR_DISPID_MAINMENU)

                Logger.Info("MAIN_MENU.Click END")

            End Sub

        CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).OnClientClick = "return cancellationCheck();"


        '顧客詳細
        AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(CUSTOMER_SEARCH).Click, _
            Sub()

                '開始ログ出力
                Logger.Info("CUSTOMER.Click START")

                '顧客詳細に遷移
                If IsSession(SESSION_KEY_CRCUSTID) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_CRCUSTID, Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False))
                End If

                If IsSession(SESSION_KEY_CSTKIND) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_CSTKIND, Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False))
                End If

                If IsSession(SESSION_KEY_CUSTOMERCLASS) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_CUSTOMERCLASS, Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False))
                End If

                If IsSession(SESSION_KEY_SALESSTAFFCD) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_SALESSTAFFCD, Me.GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False))
                End If

                If IsSession(SESSION_KEY_VCLID) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_VCLID, Me.GetValue(ScreenPos.Current, SESSION_KEY_VCLID, False))
                End If

                If IsSession(SESSION_KEY_FOLLOW_UP_BOX) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_FOLLOW_UP_BOX, Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False))
                End If

                If IsSession(SESSION_KEY_FLLWUPBOX_STRCD) Then
                    Me.SetValue(ScreenPos.Next, SESSION_KEY_FLLWUPBOX_STRCD, Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False))
                End If

                Me.RedirectNextScreen(STR_DISPID_CUST)

                Logger.Info("CUSTOMER.Click END")

            End Sub

        ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 START
        'TCVボタン
        Dim tcvButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV)
        AddHandler tcvButton.Click, AddressOf tcvButton_Click
        ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 END

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub


#End Region

    ''' <summary>
    ''' 商談(一時対応・営業活動)中判定
    ''' </summary>
    ''' <returns>True:商談中、False:スタンバイ(一時退席)</returns>
    ''' <remarks>ステータスを参照して商談中か判断する</remarks>
    Private Function IsSales() As Boolean

        Dim ret As Boolean = False

        Const METHODNAME As String = "IsSales "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "1")) Then

            ret = True
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(ret)
        Logger.Info(endLogInfo.ToString())

        Return ret

    End Function
    ''' <summary>
    ''' セッション存在判定
    ''' </summary>
    ''' <param name="SessionName">判定対象のセッション名</param>
    ''' <returns>True:あり False:なし</returns>
    ''' <remarks>セッション存在を判定</remarks>
    Private Function IsSession(ByVal SessionName As String) As Boolean

        Const METHODNAME As String = "IsSession "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim ret As Boolean = False


        If Me.ContainsKey(ScreenPos.Current, SessionName) Then
            If Not String.IsNullOrEmpty(Me.GetValue(ScreenPos.Current, SessionName, False)) Then

                ret = True
            End If
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(ret)
        Logger.Info(endLogInfo.ToString())

        Return ret

    End Function

    ''' <summary>
    ''' 活動登録画面の制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetActivityControl(ByVal crcustid As String, ByVal folloupseq As String, ByVal followUpStoreCd As String)

        Const METHODNAME As String = "SetActivityControl "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        If Not String.IsNullOrEmpty(crcustid) And Not String.IsNullOrEmpty(folloupseq) Then

            '活動状態取得
            Dim resultTable As SC3060101GetStatusToDataTable = GetFollowupboxStatus(followUpStoreCd, folloupseq)
            Dim resultTablePast As SC3060101GetStatusToDataTable = GetFollowupboxStatusPast(followUpStoreCd, folloupseq)

            Dim jsFlg As String = "false"

            If resultTable.Count + resultTablePast.Count = 0 Then
                jsFlg = "true"
            End If
            '新規活動中フラグ
            JavaScriptUtility.RegisterStartupScript(Me, "newActivityFlg = " _
                                        & jsFlg & ";" _
                                        & "redirectMessage = '" _
                                        & HttpUtility.JavaScriptStringEncode(WebWordUtility.GetWord(20908)) & "';", _
                                        "newActivityFlg", _
                                        True)
        Else

            '新規活動中フラグ
            JavaScriptUtility.RegisterStartupScript(Me, "newActivityFlg = false;", _
                                                    "newActivityFlg", _
                                                    True)

        End If


        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())


    End Sub



    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <returns>状態(true:生きている活動、false:完了)</returns>
    ''' <remarks></remarks>
    Private Function GetFollowupboxStatus(ByVal fllwupboxStrcd As String, ByVal folloupseq As String) As SC3060101GetStatusToDataTable

        Const METHODNAME As String = "GetFollowupboxStatus "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim context As StaffContext = StaffContext.Current()
        Dim returnFlg As Boolean = False

        Using param As New SC3060101GetStatusFromDataTable
            Dim dr As SC3060101GetStatusFromRow = param.NewSC3060101GetStatusFromRow()
            dr.DLRCD = context.DlrCD
            dr.STRCD = fllwupboxStrcd
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            dr.FLLWUPBOX_SEQNO = CDec(folloupseq)
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
            param.AddSC3060101GetStatusFromRow(dr)


            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return SC3060101BusinessLogic.GetFollowupboxStatus(param)
        End Using

    End Function

    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <returns>状態(true:生きている活動、false:完了)</returns>
    ''' <remarks></remarks>
    Private Function GetFollowupboxStatusPast(ByVal fllwupboxStrcd As String, ByVal folloupseq As String) As SC3060101GetStatusToDataTable

        Const METHODNAME As String = "GetFollowupboxStatusPast "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        Dim context As StaffContext = StaffContext.Current()
        Dim returnFlg As Boolean = False

        Using param As New SC3060101GetStatusFromDataTable
            Dim dr As SC3060101GetStatusFromRow = param.NewSC3060101GetStatusFromRow()
            dr.DLRCD = context.DlrCD
            dr.STRCD = fllwupboxStrcd
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            dr.FLLWUPBOX_SEQNO = CDec(folloupseq)
            '$02 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
            param.AddSC3060101GetStatusFromRow(dr)


            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return SC3060101BusinessLogic.GetFollowupboxStatusPast(param)
        End Using

    End Function

#Region "検索ボックスの制御"

    ''' <summary>
    ''' 検索ボックスの制御
    ''' </summary>
    ''' <remarks>商談中・営業・一時対応中は検索ボックスを非活性にする</remarks>
    Private Sub InitSearchBox()

        Const METHODNAME As String = "InitSearchBox "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        '商談中(営業活動・一時対応も)の場合、検索ボックスに名前を入れ非活性に
        If IsSales() Then
            '顧客氏名の取得
            Dim custName As String = getCstName()
            CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False
            'If Not String.IsNullOrEmpty(custName) Then
            '    CType(Me.Master, CommonMasterPage).SearchBox.SearchText = custName
            'Else
            '    CType(Me.Master, CommonMasterPage).SearchBox.SearchText = ""
            'End If


            If IsSession(SESSION_KEY_CUSTNAME) Then
                CType(Me.Master, CommonMasterPage).SearchBox.SearchText = Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTNAME, False).ToString()
            Else
                CType(Me.Master, CommonMasterPage).SearchBox.SearchText = ""
            End If




        ElseIf CType(Me.Master, CommonMasterPage).SearchBox.Enabled = False Then
            '検索ボックスの状態を元に戻す
            CType(Me.Master, CommonMasterPage).SearchBox.Enabled = True
            CType(Me.Master, CommonMasterPage).SearchBox.SearchText = ""
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub


    ''' <summary>
    ''' 顧客氏名取得
    ''' </summary>
    ''' <returns>敬称付き顧客氏名</returns>
    ''' <remarks></remarks>
    Private Function getCstName() As String

        Const METHODNAME As String = "getCstName "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Dim retName As String = ""

        '顧客種別(1：自社客 / 2：未取引客)
        Dim cstKind As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
            cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        End If

        Dim cstClass As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS) Then
            cstClass = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
        End If

        '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
        Dim crcustId As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
            crcustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        End If

        Dim vclId As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_VCLID) Then
            vclId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLID, False), String)
        End If


        '既存客モード、新規登録モードの判定
        If Not String.IsNullOrEmpty(Trim(crcustId)) Then

            Dim params As SC3060101DataSet.SC3060101ParameterDataTable = Me.setParameters(cstKind, cstClass, crcustId, vclId) '検索条件格納用

            '顧客氏名取得
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客取得
                Dim orgCustomerDataTbl As SC3060101DataSet.SC3060101OrgCustomerDataTable _
                    = SC3060101BusinessLogic.GetOrgCustomerData(params)

                If orgCustomerDataTbl.Rows.Count > 0 Then
                    Dim orgCustomerDataRow As SC3060101DataSet.SC3060101OrgCustomerRow
                    orgCustomerDataRow = CType(orgCustomerDataTbl.Rows(0), SC3060101DataSet.SC3060101OrgCustomerRow)
                    '敬称付き顧客氏名をセット
                    retName = Me.makeCustomerTitle(orgCustomerDataRow.NAME, orgCustomerDataRow.KEISYO_ZENGO, orgCustomerDataRow.NAMETITLE)
                End If
            ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
                '未取引客取得
                Dim newCustomerDataTbl As SC3060101DataSet.SC3060101NewCustomerDataTable _
                    = SC3060101BusinessLogic.GetNewCustomerData(params)

                If newCustomerDataTbl.Rows.Count > 0 Then

                    Dim newCustomerDataRow As SC3060101DataSet.SC3060101NewCustomerRow
                    newCustomerDataRow = CType(newCustomerDataTbl.Rows(0), SC3060101DataSet.SC3060101NewCustomerRow)
                    '敬称付き顧客氏名をセット
                    retName = Me.makeCustomerTitle(newCustomerDataRow.NAME, newCustomerDataRow.KEISYO_ZENGO, newCustomerDataRow.NAMETITLE)
                End If
            End If


        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(retName)

        Logger.Info(endLogInfo.ToString())


        Return retName

    End Function

    ''' <summary>
    ''' 敬称付名前作成
    ''' </summary>
    ''' <param name="name">名前</param>
    ''' <param name="pos">位置</param>
    ''' <param name="title">敬称</param>
    ''' <remarks></remarks>
    Private Function makeCustomerTitle(ByVal name As String, ByVal pos As String, ByVal title As String) As String

        Const METHODNAME As String = "makeCustomerTitle "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Dim sb As New StringBuilder
        If pos.Equals("1") Then
            If Not String.IsNullOrEmpty(title) Then
                sb.Append(title)
                sb.Append(" ")
            End If
        End If

        sb.Append(name)

        If pos.Equals("2") Then
            If Not String.IsNullOrEmpty(title) Then
                sb.Append(" ")
                sb.Append(title)
            End If
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        endLogInfo.Append(ENDLOGRETURN)
        endLogInfo.Append(name)

        Logger.Info(endLogInfo.ToString())

        Return name

    End Function





    ''' <summary>
    ''' パラメータセット
    ''' </summary>
    ''' <returns>パラメータ管理テーブル</returns>
    ''' <remarks></remarks>
    Private Function setParameters(ByVal cstKind As String, ByVal customerClass As String, ByVal crcustId As String, ByVal vclId As String) As SC3060101DataSet.SC3060101ParameterDataTable

        Const METHODNAME As String = "setParameters "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '販売店コード
        Dim dlrCd As String = StaffContext.Current.DlrCD

        '店舗コード
        Dim strCd As String = StaffContext.Current.BrnCD

        Using params As New SC3060101DataSet.SC3060101ParameterDataTable '検索条件格納用

            Dim paramsDr As SC3060101DataSet.SC3060101ParameterRow
            paramsDr = params.NewSC3060101ParameterRow

            '検索条件セット
            paramsDr.DLRCD = dlrCd
            paramsDr.STRCD = strCd
            paramsDr.CSTKIND = cstKind
            paramsDr.CUSTOMERCLASS = customerClass
            paramsDr.CRCUSTID = crcustId
            paramsDr.VCLID = vclId
            params.Rows.Add(paramsDr)

            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)

            Logger.Info(endLogInfo.ToString())


            Return params
        End Using
    End Function
#End Region
    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 START
#Region "TCV呼び出し"

    ''' <summary>
    ''' TCVとの連携ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>TCVとの連携ボタン</remarks>
    Private Sub tcvButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Const METHODNAME As String = "tcvButton_Click "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '商談フラグの設定(商談中・営業活動中・一時対応中の場合Trueを設定)
        Dim BusinessFlg As Boolean = False
        If IsSales() Then
            BusinessFlg = True
        End If

        '読み取り専用フラグ設定
        Dim ReadOnlyFlg As Boolean = True


        '商談中・一時対応中以外は読取専用にする
        Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
        Dim PresenceDetail As String = StaffContext.Current.PresenceDetail
        If (String.Equals(PresenceCategory, "1") And String.Equals(PresenceDetail, "1")) Or
            (String.Equals(PresenceCategory, "2") And String.Equals(PresenceDetail, "0")) Then
            ReadOnlyFlg = False
        End If

        Dim OpeCd As Integer = StaffContext.Current.OpeCD

        Dim EstimateId As String = ""
        '見積りID取得
        EstimateId = GetEstimatedId(StaffContext.Current.DlrCD,
                                    Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False).ToString(),
                                    CLng(Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString()))

        If EstimateId.Length <= 0 Then
            '見積りIDがない場合
            e.Parameters.Add("DataSource", "None")
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StrCd", Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False).ToString())
            e.Parameters.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            e.Parameters.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString())
            e.Parameters.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString())
            e.Parameters.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            'e.Parameters.Add("StartPageId", String.Empty)
            'e.Parameters.Add("SelectedEstimateIndex", String.Empty)
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("OperationCode", OpeCd)
            e.Parameters.Add("BusinessFlg", BusinessFlg)
            e.Parameters.Add("ReadOnlyFlg", ReadOnlyFlg)
        Else
            '見積りIDがある場合
            If ReadOnlyFlg = False Then
                ReadOnlyFlg = GetContractFlg(EstimateId)
            End If

            e.Parameters.Add("DataSource", "EstimateId")
            e.Parameters.Add("DlrCd", StaffContext.Current.DlrCD)
            e.Parameters.Add("StrCd", Me.GetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, False).ToString())
            e.Parameters.Add("FollowupBox_SeqNo", Me.GetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, False).ToString())
            e.Parameters.Add("CstKind", Me.GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False).ToString())
            e.Parameters.Add("CustomerClass", Me.GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False).ToString())
            e.Parameters.Add("CRCustId", Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False).ToString())
            e.Parameters.Add("StartPageId", "SC3050201")
            e.Parameters.Add("EstimateId", EstimateId)
            e.Parameters.Add("SelectedEstimateIndex", "0")
            e.Parameters.Add("Account", StaffContext.Current.Account)
            e.Parameters.Add("AccountStrCd", StaffContext.Current.BrnCD)
            e.Parameters.Add("MenuLockFlag", False)
            e.Parameters.Add("OperationCode", OpeCd)
            e.Parameters.Add("BusinessFlg", BusinessFlg)
            e.Parameters.Add("ReadOnlyFlg", ReadOnlyFlg)
        End If

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

    End Sub

     ''' <summary>
    ''' 見積りID取得
    ''' </summary>
    ''' <param name="fboxDlrCd">販売店コード</param>
    ''' <param name="fboxStrCd">店舗コード</param>
    ''' <param name="fboxSeqNo">Follow-up box 連番</param>
    ''' <returns>見積もりＩＤ（複数件存在する場合は、カンマ区切り）</returns>
    ''' <remarks>TCVに遷移する際に必要となる見積もりＩＤを取得します</remarks>
    Private Function GetEstimatedId(ByVal fboxDlrCd As String, ByVal fboxStrCd As String, ByVal fboxSeqNo As Long) As String

        'ログ出力 Start ***************************************************************************
        Logger.Debug("GetEstimatedId Start")
        'ログ出力 End *****************************************************************************

        '返却用の見積もりＩＤ変数
        Dim returnEstId As New StringBuilder

        Using param As New SC3060101GetEstimateidFromDataTable

            '検索条件となるレコードを作製
            Dim conditionRow As SC3060101GetEstimateidFromRow = param.NewSC3060101GetEstimateidFromRow()
            conditionRow.DLRCD = fboxDlrCd
            conditionRow.STRCD = fboxStrCd
            conditionRow.FLLWUPBOX_SEQNO = fboxSeqNo
            '検索条件を登録
            param.AddSC3060101GetEstimateidFromRow(conditionRow)

            '検索処理
            Dim result As SC3060101GetEstimateidToDataTable = SC3060101BusinessLogic.GetEstimatedId(param)
            'カンマ区切りに編集
            For Each dr As SC3060101GetEstimateidToRow In result.Rows
                'カンマ編集
                If returnEstId.Length > 0 Then
                    returnEstId.Append(",")
                End If
                '１件分の見積もりＩＤセット
                returnEstId.Append(dr.ESTIMATEID.ToString())
            Next

        End Using

        'ログ出力 Start ***************************************************************************
        Logger.Debug("GetEstimatedId End")
        'ログ出力 End *****************************************************************************

        '処理結果返却
        Return returnEstId.ToString()
    End Function

    ''' <summary>
    ''' 契約状況取得処理
    ''' </summary>
    ''' <param name="EstimateId">見積もりID</param>
    ''' <returns>True:契約済み False:契約済み以外</returns>
    ''' <remarks></remarks>
    Private Function GetContractFlg(ByVal EstimateId As String) As Boolean

        Const METHODNAME As String = "GetContractFlg "

        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())


        Dim result As SC3060101ContractDataTable = Nothing
        Dim rtnFlg As Boolean = True

        Using param As New SC3060101ESTIMATEINFODataTable
            Dim conditionRow As SC3060101ESTIMATEINFORow = param.NewSC3060101ESTIMATEINFORow
            conditionRow.ESTIMATEID = EstimateId

            '検索条件を登録
            param.AddSC3060101ESTIMATEINFORow(conditionRow)

            '検索処理
            result = SC3060101BusinessLogic.GetContractFlg(param)
        End Using

        'デバッグログ(終了)
        '終了ログ出力
        Dim endLogInfo As New StringBuilder
        endLogInfo.Append(METHODNAME)
        endLogInfo.Append(ENDLOG)
        Logger.Info(endLogInfo.ToString())

        '処理結果返却
        If result.Rows.Count > 0 Then
            Dim dr As SC3060101DataSet.SC3060101ContractRow = CType(result.Rows(0), SC3060101DataSet.SC3060101ContractRow)
            If Not CONTRACT.Equals(dr.CONTRACTFLG) Then
                rtnFlg = False
            End If
        End If

        Return rtnFlg
    End Function

#End Region
    ' 2012/03/19 KN 清水 【SALES_1B】TCV遷移対応 END

#Region " セッション取得・設定バイパス処理 "


    Public Sub SetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object) Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl.SetValueCommonBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Function GetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements Toyota.eCRB.iCROP.BizLogic.Common.ICommonSessionControl.GetValueCommonBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function


#End Region

    Private Sub stubSessionValue()


        Me.SetValue(ScreenPos.Current, SESSION_KEY_REQUESTID, "1234567890")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_ASSESSMENTNO, "2234567890")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, "1000000000000015143")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, "1")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, "3")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, "4")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_VCLID, "5")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_FOLLOW_UP_BOX, "6")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_FLLWUPBOX_STRCD, "01")


        Me.SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, "NCST0000000392")
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, "2")

    End Sub


End Class
