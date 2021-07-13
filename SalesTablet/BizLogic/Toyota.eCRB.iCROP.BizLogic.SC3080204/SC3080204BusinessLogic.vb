'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客メモ (ビジネスロジック)
'補足： 
'作成： 2011/11/24 TCS 安田
'更新： 2013/06/30 TCS 庄   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2014/11/20 TCS 河原  TMT B案
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展  
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic



''' <summary>
''' SC3080204(Customer Memo)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3080204BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' 顧客区分 (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OrgCustsegment As String = "1"

    ''' <summary>
    ''' 顧客区分 (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NewCustsegment As String = "2"

    ''' <summary>
    ''' 顧客分類 (1：所有者)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Customerclass1 As String = "1"

    ''' <summary>
    ''' 顧客分類 (2：使用者)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Customerclass2 As String = "2"

    ''' <summary>
    ''' 顧客分類 (3：その他)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Customerclass3 As String = "3"

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ V3データ表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_ICROP_OLD_SYSTEM_DISP_FLG As String = "ICROP_OLD_SYSTEM_DISP_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

    ''' <summary>
    ''' 初期データ取得
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>初期表示用のデータを取得する。</remarks>
    Public Shared Function GetCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As SC3080204DataSet.SC3080204CustMemoDataTable

        msgId = 0
        Dim retMemoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable
        Dim retMemoDataRow As SC3080204DataSet.SC3080204CustMemoRow
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)

        'ログ出力 Start ***************************************************************************
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo memoDataRow.CRCUSTID = " + Convert.ToString(memoDataRow.CRCUSTID))
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '2012/02/20 TCS 藤井 【SALES_2】Add Start
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo memoDataRow.NEWCUSTID = " + memoDataRow.NEWCUSTID)
        '2012/02/20 TCS 藤井 【SALES_2】Add End
        'ログ出力 End *****************************************************************************

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        retMemoDataTbl = SC3080204TableAdapter.GetCustomerMemo(memoDataRow.DLRCD, memoDataRow.CRCUSTID)

        '2014/11/20 TCS 河原  TMT B案 START

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '①販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '②販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim Setting_Val As String
        Dim systemBiz As New SystemSettingDlr
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(StaffContext.Current.DlrCD, ConstantBranchCD.AllBranchCD, C_ICROP_OLD_SYSTEM_DISP_FLG)
        If drSettingDlr Is Nothing Then
            'データがない場合は0にする
            Setting_Val = "0"
        Else
            Setting_Val = drSettingDlr.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        If String.Equals(Setting_Val, "1") Then
            Dim V3cst_IDDt As SC3080204DataSet.SC3080204Cst_CDDataTable
            V3cst_IDDt = SC3080204TableAdapter.GetV3CustomerCD(memoDataRow.CRCUSTID, StaffContext.Current.DlrCD)

            'V3の顧客IDを取得
            Dim V3cst_id As String = Nothing
            Dim V3NewCstCD As String = Nothing

            Dim V3cst_idRw As SC3080204DataSet.SC3080204Cst_CDRow
            V3cst_idRw = CType(V3cst_IDDt.Rows(0), SC3080204DataSet.SC3080204Cst_CDRow)
            V3cst_id = V3cst_idRw.CST_CD

            If Not String.IsNullOrEmpty(Trim(V3cst_id)) Then
                If String.Equals(V3cst_idRw.CST_TYPE, OrgCustsegment) Then
                    '自社客の場合、未取引客CDを取得
                    Dim V3NewCstCDDt As SC3080204DataSet.SC3080204Cst_CDDataTable
                    V3NewCstCDDt = SC3080204TableAdapter.GetV3NewCustomerCD(V3cst_id)
                    If V3NewCstCDDt.Count() > 0 Then
                        Dim V3NewCstCDRW As SC3080204DataSet.SC3080204Cst_CDRow
                        V3NewCstCDRW = CType(V3NewCstCDDt.Rows(0), SC3080204DataSet.SC3080204Cst_CDRow)
                        V3NewCstCD = V3NewCstCDRW.CST_CD
                    End If
                End If

                'V3の顧客メモ取得
                Dim retMemoV3DataTbl As SC3080204DataSet.SC3080204CustMemoDataTable
                retMemoV3DataTbl = SC3080204TableAdapter.GetV3CustomerMemo(memoDataRow.DLRCD, V3cst_id, V3NewCstCD)
                retMemoDataTbl.Merge(retMemoV3DataTbl)
            End If
        End If
        '2014/11/20 TCS 河原  TMT B案 END

        For Each retMemoDataRow In retMemoDataTbl

            '日付変換
            retMemoDataRow.UPDATEDATESTR = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                          retMemoDataRow.UPDATEDATE, _
                                                                          memoDataRow.DLRCD)

            '最初の行をセットする (Windows=CRLF , iPad=LF)
            retMemoDataRow.FIRSTMEMO = String.Empty
            Dim memos As String() = retMemoDataRow.MEMO.Split(New Char() {CChar(vbCrLf)})
            If (memos.Length = 1) Then
                memos = memos(0).Split(New Char() {CChar(vbLf)})
            End If
            Dim i As Integer
            For i = 0 To memos.Length - 1
                If (memos(i).Length > 0) Then
                    retMemoDataRow.FIRSTMEMO = memos(i)
                    Exit For
                End If
            Next

            retMemoDataRow.UPDATEDATEDAY = Format(retMemoDataRow.UPDATEDATE, "yyyy/MM/dd")
            retMemoDataRow.UPDATEDATETIME = Format(retMemoDataRow.UPDATEDATE, "HH:mm")
        Next

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerMemo_End")
        'ログ出力 End *****************************************************************************
        Return retMemoDataTbl
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' バリデーション判定
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>バリデーションを判定する。</remarks>
    Public Shared Function CheckValidation(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Boolean

        msgId = 0
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)

        'メモ未入力の場合	メモを入力してください。
        If (String.IsNullOrEmpty(memoDataRow.MEMO)) Then
            msgId = 70901
            Return False
        End If

        'メモが1024文字より多い	メモを1024文字以内で入力してください。
        If (Validation.IsCorrectDigit(memoDataRow.MEMO, 1024) = False) Then
            msgId = 70900
            Return False
        End If

        'メモに絵文字が入っている
        If (Validation.IsValidString(memoDataRow.MEMO) = False) Then
            msgId = 70902
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 顧客メモ新規登録処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を新規登録する。</remarks>
    <EnableCommit()>
    Public Function InsertCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)


        '顧客メモ連番采番
        Dim seqno As Long
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        seqno = SC3080204TableAdapter.GetCustmemoseq(memoDataRow.DLRCD, memoDataRow.CRCUSTID)
        memoDataRow.CUSTMEMOHIS_SEQNO = seqno

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq seqno = " + CType(seqno, String))
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq memoDataRow.MEMO = " + memoDataRow.MEMO)
        'ログ出力 End *****************************************************************************

        '顧客メモ追加
        ret = SC3080204TableAdapter.InsertCustomerMemo(memoDataRow.DLRCD, _
                                        memoDataRow.CRCUSTID, _
                                        memoDataRow.CUSTMEMOHIS_SEQNO, _
                                        memoDataRow.MEMO, _
                                        memoDataRow.ACCOUNT)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustmemoseq_End")
        'ログ出力 End *****************************************************************************
        Return ret
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 顧客メモ更新処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerMemo_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerMemo memoDataRow.CUSTMEMOHIS_SEQNO = " + CType(memoDataRow.CUSTMEMOHIS_SEQNO, String))
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomerMemo memoDataRow.MEMO = " + memoDataRow.MEMO)
        'ログ出力 End *****************************************************************************

        Try
            SC3080204TableAdapter.GetCustomerLock(memoDataRow.CRCUSTID)
        Catch ex As Exception
            Return 0
        End Try

        '顧客メモ更新
        ret = SC3080204TableAdapter.UpdateCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                    memoDataRow.MEMO, _
                                    memoDataRow.ACCOUNT, _
                                    memoDataRow.DLRCD, _
                                    memoDataRow.CRCUSTID, _
                                    memoDataRow.ROW_LOCK_VERSION)


        ''更新に失敗していたらロールバック
        If ret = 0 Then
            Me.Rollback = True
            Return -1
        End If

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerMemo_End")
        'ログ出力 End *****************************************************************************
        Return ret
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 顧客メモ削除処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を削除する。</remarks>
    <EnableCommit()>
    Public Function DeleteCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCustomerMemo_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCustomerMemo memoDataRow.CUSTMEMOHIS_SEQNO = " + CType(memoDataRow.CUSTMEMOHIS_SEQNO, String))
        'ログ出力 End *****************************************************************************

        Try
            SC3080204TableAdapter.GetCustomerLock(memoDataRow.CRCUSTID)
        Catch ex As Exception
            Return 0
        End Try
        '顧客メモ履歴移動
        ret = SC3080204TableAdapter.MoveCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                    memoDataRow.DLRCD, _
                                    memoDataRow.CRCUSTID)

        '顧客メモ削除
        ret = SC3080204TableAdapter.DeleteCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                    memoDataRow.DLRCD, _
                                    memoDataRow.CRCUSTID)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("DeleteCustomerMemo_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Return ret

    End Function

End Class
