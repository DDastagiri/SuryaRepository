'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060103BusinessLogic.vb
'─────────────────────────────────────
'機能： 査定価格登録IF
'補足： 
'作成：  
'更新： 2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 坂井 2013/10対応版 既存流用
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Assessment.Assessment.DataAccess

''' <summary>
''' 中古車査定情報登録ビジネスクラス
''' </summary>
''' <remarks>中古車査定情報の登録を行います。</remarks>
Public Class IC3060103BizLogic
    Inherits BaseBusinessComponent

#Region "定数"
    Private Const assessmentStatusCanceld As String = "2"

    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 査定実績フラグ(実績なし)
    ''' </summary>
    Private Const C_ASMACTFLG_OFF As String = "0"

    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    ''' <summary>
    ''' モデル名なし(半角ブランク)
    ''' </summary>
    Private Const C_NO_NAME As String = " "

    ''' <summary>
    ''' 自社客
    ''' </summary>
    Private Const C_ORG_CUST As String = "1"
    '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
    '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 中古車査定データテーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private ucaAsesssmentDt As IC3060103DataSet.IC3060103SetUcarAssessmentDataTable

    ''' <summary>
    ''' 中古車査定情報登録対象項目リスト
    ''' </summary>
    ''' <remarks>更新対象項目名をリストに設定してください。</remarks>
    Private ucaAsesssmentUpColumnList As List(Of String)

#End Region

#Region "査定価格登録メイン処理"
    ''' <summary>
    ''' 査定価格登録メイン処理
    ''' </summary>
    ''' <returns>処理結果データテーブル</returns>
    ''' <remarks>査定情報を取得します。</remarks>
    <EnableCommit()>
    Public Function SetAssessmentPrice() As IC3060103ResultCode

        Logger.Info("SetAssessmentPrice Start")

        Dim adapter As New IC3060103TableAdapter(ucaAsesssmentDt, ucaAsesssmentUpColumnList)

        Try
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            'ロック取得
            adapter.GetUcarAssessmentLock()
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
            Dim asmActFlg As String
            '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

            '中古車査定情報取得
            Dim resDatatable As IC3060103DataSet.IC3060103GetUcarAssessmentDataTable = Nothing      '実行用
            Using resDatatableTemp As New IC3060103DataSet.IC3060103GetUcarAssessmentDataTable      'オブジェクト破棄用

                '実行用データテーブルへインスタンスコピー
                resDatatable = resDatatableTemp

                '中古車査定情報取得実行
                resDatatable = adapter.GetUcarAssessment

                '中古車査定情報存在確認
                If resDatatable.Rows.Count = 0 Then
                    '査定情報存在エラー
                    Return IC3060103ResultCode.ErrAssessmentNoData
                End If

                '査定情報ステータス確認
                If resDatatable(0).STATUS = assessmentStatusCanceld Then
                    '査定依頼キャンセル済みエラー
                    Return IC3060103ResultCode.ErrAssessmentCanceld
                End If

                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
                asmActFlg = resDatatable(0).ASM_ACT_FLG
                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END
            End Using

            '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD START
            ''中古車査定情報更新実行
            'If adapter.SetUcarAssessment() <> 1 Then

            '中古車査定情報更新実行
            If adapter.SetUcarAssessment(asmActFlg) <> 1 Then
                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 MOD END
                '査定情報更新エラー
                Return IC3060103ResultCode.ErrAssessmentNotUpdate
            End If

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            Dim resDatatableRow As IC3060103DataSet.IC3060103GetUcarAssessmentRow
            resDatatableRow = resDatatable.Item(0)
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
            '査定依頼の回答前に活動が登録された場合
            If asmActFlg.Equals(C_ASMACTFLG_OFF) Then

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                '商談活動登録
                Dim resModelNameDatatable As IC3060103DataSet.IC3060103GetModelNameDataTable
                Dim resModelNameDatatableRow As IC3060103DataSet.IC3060103GetModelNameRow

                If (C_ORG_CUST.Equals(resDatatableRow.CSTKIND)) Then
                    resModelNameDatatable = adapter.GetModelName()
                Else
                    resModelNameDatatable = adapter.GetModelNameNewCust()
                End If

                If Not resModelNameDatatable.Count = 0 Then
                    resModelNameDatatableRow = resModelNameDatatable.Item(0)
                    adapter.SetCRHis(resModelNameDatatableRow.MODEL_NAME)
                Else
                    adapter.SetCRHis(C_NO_NAME)
                End If
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START DEL
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                '査定依頼中活動履歴削除
                adapter.DelFllwupBoxCRHisAsm()
            End If
                '2013/03/08 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

                '正常終了
                Return IC3060103ResultCode.Succsess

        Catch ex As Exception
            Me.Rollback = True
            Throw
        Finally
            adapter = Nothing
        End Try

        Logger.Info("SetAssessmentPrice Was Normal End")

    End Function

#End Region


#Region "コンストラクタ"
    ''' <summary>
    ''' イニシャライズ
    ''' </summary>
    ''' <param name="dt">中古車査定情報DataTable</param>
    ''' <param name="lt">中古車査定情報登録対象項目リスト</param>
    ''' <remarks>データアクセスクラスの中古車査定情報DataTableと、中古車査定情報登録対象項目リストをセットします。</remarks>
    Public Sub New(ByVal dt As IC3060103DataSet.IC3060103SetUcarAssessmentDataTable, ByVal lt As List(Of String))
        ucaAsesssmentDt = dt
        ucaAsesssmentUpColumnList = lt
    End Sub

#End Region

End Class
#Region "Businessクラス終了コード列挙体"

''' <summary>
''' Businessクラス終了コード
''' </summary>
''' <remarks>当クラスの公開メソッドの戻り値の列挙体です。</remarks>
Public Enum IC3060103ResultCode As Integer

    ''' <summary>
    ''' 処理正常終了
    ''' </summary>
    ''' <remarks>正常終了した場合</remarks>
    Succsess = 0

    ''' <summary>
    ''' 査定依頼キャンセル済み
    ''' </summary>
    ''' <remarks>該当査定依頼が既にキャンセルされている場合</remarks>
    ErrAssessmentCanceld = 1

    ''' <summary>
    ''' 査定情報存在エラー
    ''' </summary>
    ''' <remarks>中古車査定テーブルに該当レコードが存在しなかった場合</remarks>
    ErrAssessmentNoData = 21

    ''' <summary>
    ''' 査定情報更新エラー
    ''' </summary>
    ''' <remarks>中古車査定テーブル更新に失敗した場合</remarks>
    ErrAssessmentNotUpdate = 22

End Enum
#End Region

