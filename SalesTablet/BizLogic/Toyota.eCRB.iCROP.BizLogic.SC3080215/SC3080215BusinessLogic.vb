'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080215BusinessLogic.vb
'─────────────────────────────────────
'機能： CSSurvey一覧・詳細
'補足： 
'作成： 2012/02/20 TCS 明瀬
'更新： 2013/06/30 TCS 坂井 2013/10対応版 既存流用
'─────────────────────────────────────

Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization

''' <summary>
''' CSSurvey一覧・詳細
''' ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3080215BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3080215"

    ''' <summary>
    ''' 自画面のプログラムファイル名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMFILE As String = "SC3080215.ascx "

    ''' <summary>
    ''' 顧客種別　自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CSTKIND_ORG As String = "1"

    ''' <summary>
    ''' アンケート用紙種別　顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAPAERTYPE_CST As String = "0"

    ''' <summary>
    ''' 回答タイプ　コンボボックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ANSWERTYPE_COMBO As String = "1"

    ''' <summary>
    ''' アンケート回答結果　チェックあり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHECK_ON As String = "1"

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
    ''' <summary>
    ''' テキスト回答結果 半角空白(1桁)の場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TXT_BLANK As String = " "
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' アンケート一覧の行インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private mListRowIndex As Integer = 0

    ''' <summary>
    ''' アンケート詳細の親テーブルID
    ''' </summary>
    ''' <remarks></remarks>
    Private mPurentDetailDtId As Integer = 0

    ''' <summary>
    ''' アンケート詳細の子テーブルID
    ''' </summary>
    ''' <remarks></remarks>
    Private mChildrenDetailDtId As Integer = 0

    ''' <summary>
    ''' アンケート詳細の質問項目インデックス名(Q1のQ)
    ''' </summary>
    ''' <remarks></remarks>
    Private mQuestionItemIndexName As String = WebWordUtility.GetWord(MY_PROGRAMID, 2)

    ''' <summary>
    ''' アンケート詳細の回答項目インデックス名(A1のA)
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnswerItemIndexName As String = WebWordUtility.GetWord(MY_PROGRAMID, 3)

    ''' <summary>
    ''' アンケート詳細の項目ID(比較用)
    ''' </summary>
    ''' <remarks></remarks>
    Private mQuestionItemId As Long = -1

    ''' <summary>
    ''' アンケート詳細の回答項目数カウント用
    ''' </summary>
    ''' <remarks></remarks>
    Private mQuestionCount As Long = 1

    ''' <summary>
    ''' コンボボックスフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private mComboBoxFlg As Boolean = False

    ''' <summary>
    ''' メッセージID：異常終了（その他エラー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpMessageIdSys As Long = 9999

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' メッセージID：異常終了（その他エラー）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>メッセージID：異常終了（その他エラー）</remarks>
    Public Shared ReadOnly Property MessageIdSys() As Long
        Get
            Return prpMessageIdSys
        End Get
    End Property
#End Region

#Region "Publicメソッド"
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
    ''' <summary>
    ''' CSアンケート回答結果の一覧件数を取得する
    ''' </summary>
    ''' <param name="orgCustId"></param>
    ''' <returns>CSアンケート回答結果の一覧件数</returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionListCount(ByVal orgCustId As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[orgCustId:{0}]", orgCustId))
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        Dim ta As New SC3080215TableAdapter

        '検索処理
        Dim countDt As SC3080215DataSet.SC3080215CSQuestionListCountDataTable

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
        countDt = ta.GetCSQuestionListCountDT(orgCustId)
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        Dim rtnCount As Integer = CInt(countDt.Item(0).COUNT)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_End[GetListCount:{0}]", rtnCount))

        Return rtnCount

    End Function

    ''' <summary>
    ''' CSアンケート回答結果の一覧を取得する
    ''' </summary>
    ''' <param name="sessionRow"></param>
    ''' <returns>検索結果を格納したDatatable</returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionList(ByVal sessionRow As SC3080215DataSet.SC3080215SessionRow) As SC3080215DataSet.SC3080215DisplayListDataTable

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[CUSTID:{0}][CSTKIND:{1}][CUSTOMERCLASS:{2}][DLRCD:{3}]", _
                                  sessionRow.ORGCUSTID, sessionRow.CSTKIND, sessionRow.CUSTOMERCLASS, sessionRow.DLRCD))
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        Dim ta As New SC3080215TableAdapter

        '取得用CSアンケート一覧データテーブル
        Dim listDt As SC3080215DataSet.SC3080215CSQuestionListDataTable

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
        listDt = ta.GetCSQuestionListDT(sessionRow.ORGCUSTID)
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

        'ここで一覧データが取得できていない場合、顧客詳細画面表示時に一覧件数が１件以上と判断した後、回答データが削除されているため異常と判断し、エラーを発生させる
        If listDt.Rows.Count = 0 Then
            Throw New ApplicationException(WebWordUtility.GetWord(MY_PROGRAMID, 4))
        End If

        '返却用データテーブル生成
        Using rtnDt As New SC3080215DataSet.SC3080215DisplayListDataTable

            'CSアンケート一覧データテーブルの取得件数分ループ
            For Each listRow In listDt
                Me.SetReturnListData(rtnDt, listRow, sessionRow.DLRCD)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_End[GetListCount:{0}]", rtnDt.Rows.Count))

            Return rtnDt

        End Using

    End Function

    ''' <summary>
    ''' CSアンケート回答結果の詳細を取得する
    ''' </summary>
    ''' <param name="answerId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCSQuestionDetail(ByVal answerId As Long) As SC3080215DataSet

        Logger.Info(String.Format(CultureInfo.InvariantCulture, MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_Start[answerId:{0}]", answerId))

        Dim ta As New SC3080215TableAdapter

        '検索処理
        Dim detailDt As SC3080215DataSet.SC3080215CSQuestionDetailDataTable = ta.GetCSQuestionDetailDT(answerId)

        'ここで詳細データが取得できていない場合、一覧表示後に回答データが削除されているため異常と判断し、エラーを発生させる
        If detailDt.Rows.Count = 0 Then
            Throw New ApplicationException(WebWordUtility.GetWord(MY_PROGRAMID, 4))
        End If

        '返却用データセット生成
        Using rtnDs As New SC3080215DataSet

            'CSアンケート詳細データテーブルの取得件数分ループ
            For Each detailRow In detailDt
                Me.SetReturnDetailData(rtnDs, detailRow)
            Next

            Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

            Return rtnDs

        End Using

    End Function
#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' アンケート一覧表示用データテーブルにデータを設定
    ''' </summary>
    ''' <param name="rtnDt"></param>
    ''' <param name="listRow"></param>
    ''' <param name="dlrCD"></param>
    ''' <remarks></remarks>
    Private Sub SetReturnListData(ByVal rtnDt As SC3080215DataSet.SC3080215DisplayListDataTable, _
                                  ByVal listRow As SC3080215DataSet.SC3080215CSQuestionListRow, _
                                  ByVal dlrCD As String)

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        '現在時刻とアンケートデータの更新時刻との差分に応じた表示用文言を取得する
        Dim dateWord As String = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, listRow.UPDATEDATE, dlrCD)

        If listRow.TARGETFLG.Equals(PAPAERTYPE_CST) Then
            '********************************************
            '* アンケート用紙種別：０(顧客)
            '********************************************

            '返却用データテーブルに行を追加
            With listRow
                rtnDt.AddSC3080215DisplayListRow(.ANSWERID, .PAPERNAME, String.Empty, String.Empty, _
                                                 dateWord, .ICON_IMGFILE, .USERNAME, .TARGETFLG, mListRowIndex)
            End With

            mListRowIndex += 1

        Else
            '********************************************
            '* アンケート用紙種別：１(車両)
            '********************************************

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 STRAT
            '返却用データテーブルに行を追加
            With listRow
                rtnDt.AddSC3080215DisplayListRow(.ANSWERID, .PAPERNAME, .SERIESNAME, .VCLREGNO, _
                                                 dateWord, .ICON_IMGFILE, .USERNAME, .TARGETFLG, mListRowIndex)
            End With
            mListRowIndex += 1
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
        End If

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

    ''' <summary>
    ''' アンケート詳細表示用データテーブルにデータを設定
    ''' </summary>
    ''' <param name="rtnDs"></param>
    ''' <param name="detailRow"></param>
    ''' <remarks></remarks>
    Private Sub SetReturnDetailData(ByVal rtnDs As SC3080215DataSet, _
                                    ByVal detailRow As SC3080215DataSet.SC3080215CSQuestionDetailRow)

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start")

        '項目IDが前のレコードから変更していた場合
        If Not mQuestionItemId = detailRow.QUESTIONITEMID Then

            '親データテーブルの主キーをインクリメント
            mPurentDetailDtId += 1

            '子データテーブルのID(親テーブルのIDフィールドの外部キー)をインクリメント
            mChildrenDetailDtId += 1

            '親データテーブルに行を追加する(ID,「Q～」,「A～」,質問内容,回答項目個数,回答タイプ(0:ラジオボタン, 1:コンボボックス, 2:チェックボックス, 3:テキストボックス))
            rtnDs.SC3080215DetailPurent.AddSC3080215DetailPurentRow(mPurentDetailDtId, _
                                                                    mQuestionItemIndexName & mPurentDetailDtId.ToString(CultureInfo.CurrentCulture), _
                                                                    mAnswerItemIndexName & mPurentDetailDtId.ToString(CultureInfo.CurrentCulture), _
                                                                    detailRow.QUESTIONCONTENT, _
                                                                    detailRow.ANSWERCOUNT, _
                                                                    detailRow.ANSWERTYPE)
            '回答項目数カウントを初期化
            mQuestionCount = 1
        End If

        Dim txtResult As String = String.Empty

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        'テキスト回答結果が半角空白(1桁)でない場合
        If Not C_TXT_BLANK.Equals(detailRow.TEXTRESULT) Then
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            txtResult = detailRow.TEXTRESULT
        Else
            '回答タイプがコンボボックス、かつ選択された値の場合
            If detailRow.ANSWERTYPE.Equals(ANSWERTYPE_COMBO) _
            AndAlso detailRow.CHECKVAL.Equals(CHECK_ON) Then

                txtResult = detailRow.ANSWERCONTENT
                'コンボボックスのレコードを追加するフラグを立てる
                mComboBoxFlg = True
            End If
        End If

        Dim answerContent As String = String.Empty
        Dim checkVal As String = String.Empty

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        'この2つの値は、回答タイプがテキストのときのみ半角スペースになっている
        If Not C_TXT_BLANK.Equals(detailRow.ANSWERCONTENT) AndAlso Not C_TXT_BLANK.Equals(detailRow.CHECKVAL) Then
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
            answerContent = detailRow.ANSWERCONTENT
            checkVal = detailRow.CHECKVAL
        End If

        If detailRow.ANSWERTYPE.Equals(ANSWERTYPE_COMBO) Then
            '回答タイプがコンボボックス
            If mComboBoxFlg Then
                '追加フラグが立っている(コンボボックスで選択されたもの)

                '子データテーブルに行を追加する(ID, 回答項目, チェック(0:なし, 1:あり), 回答タイプ, テキスト回答結果)
                rtnDs.SC3080215DetailChild.AddSC3080215DetailChildRow(mChildrenDetailDtId, _
                                                                      answerContent, _
                                                                      checkVal, _
                                                                      detailRow.ANSWERTYPE, _
                                                                      detailRow.ANSWERCOUNT, _
                                                                      txtResult)
                mComboBoxFlg = False
            End If
        Else
            '子データテーブルに行を追加する(ID, 回答項目, チェック(0:なし, 1:あり), 回答タイプ, テキスト回答結果)
            rtnDs.SC3080215DetailChild.AddSC3080215DetailChildRow(mChildrenDetailDtId, _
                                                                  answerContent, _
                                                                  checkVal, _
                                                                  detailRow.ANSWERTYPE, _
                                                                  detailRow.ANSWERCOUNT, _
                                                                  txtResult)
        End If

        '回答項目数カウントをインクリメント
        mQuestionCount += 1

        '比較用の質問項目IDを更新
        mQuestionItemId = detailRow.QUESTIONITEMID

        Logger.Info(MY_PROGRAMFILE & System.Reflection.MethodBase.GetCurrentMethod.Name & "_End")

    End Sub

#End Region

End Class
