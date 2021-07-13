'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080204BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客メモ (ビジネスロジック)
'補足： 
'作成： 2011/12/?? ?????
'更新： 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3080204
Imports Toyota.eCRB.iCROP.DataAccess.SC3080204.SC3080204DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

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

    '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START
    ''' <summary>
    ''' 設定名：ICROP_OLD_SYSTEM_DISP_FLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICROP_OLD_SYSTEM_DISP_FLG As String = "ICROP_OLD_SYSTEM_DISP_FLG"

    ''' <summary>
    ''' システム設定フラグON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_SYSTEM_SETTING_ON As String = "1"

    ''' <summary>
    ''' システム設定フラグOFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_SYSTEM_SETTING_OFF As String = "0"
    '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START

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

        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomerMemo memoDataRow.CRCUSTID = " + CStr(memoDataRow.CRCUSTID))

        retMemoDataTbl = SC3080204DataTableTableAdapter.GetCustomerMemo(memoDataRow.DLRCD, memoDataRow.CRCUSTID)

        '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 START

        Dim v3ReferenceFlg As String = ENV_SYSTEM_SETTING_OFF
        Dim drDealerEnvSetting As String = String.Empty

        '販売店のシステム設定値を取得
        Using dealerEnvBiz As New ServiceCommonClassBusinessLogic

            drDealerEnvSetting = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(ICROP_OLD_SYSTEM_DISP_FLG)

        End Using

        '取得できた場合のみ設定する
        If Not (String.IsNullOrEmpty(drDealerEnvSetting)) Then

            v3ReferenceFlg = drDealerEnvSetting

        End If

        '販売店システム設定値がONの場合、V3情報を取得する
        If v3ReferenceFlg.Equals(ENV_SYSTEM_SETTING_ON) Then

            '変数宣言
            Dim V3cst_id As String = Nothing
            Dim V3NewCstCD As String = Nothing

            'V3の顧客IDを取得
            Dim V3cst_IDDt As SC3080204DataSet.SC3080204CustomerCodeDataTable
            V3cst_IDDt = SC3080204DataTableTableAdapter.GetV3CustomerCD(memoDataRow.CRCUSTID, StaffContext.Current.DlrCD)

            '値を格納
            Dim V3cst_idRw As SC3080204DataSet.SC3080204CustomerCodeRow
            V3cst_idRw = CType(V3cst_IDDt.Rows(0), SC3080204DataSet.SC3080204CustomerCodeRow)
            ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
            'V3cst_id = V3cst_idRw.CST_CD

            If Not String.IsNullOrWhiteSpace(V3cst_idRw.NEWCST_CD) Then
                '未取引客コード が存在する場合
                V3cst_id = V3cst_idRw.NEWCST_CD

            ElseIf Not String.IsNullOrWhiteSpace(V3cst_idRw.ORGCST_CD) AndAlso "0".Equals(V3cst_idRw.ORGCST_CD.Substring(0, 1)) Then
                '自社客コードが存在し、自社客コードの1桁目が0（V3で作成された自社客）の場合
                '※V4で作成された自社客の場合、シーケンスより採番されるため1桁目は0以外の値となるため
                V3cst_id = V3cst_idRw.ORGCST_CD

            End If
            ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END

            '自社客の場合
            If Not String.IsNullOrEmpty(Trim(V3cst_id)) Then
                If String.Equals(V3cst_idRw.CST_TYPE, OrgCustsegment) Then

                    '自社客の場合、未取引客CDを取得
                    Dim V3NewCstCDDt As SC3080204DataSet.SC3080204CustomerCodeDataTable
                    V3NewCstCDDt = SC3080204DataTableTableAdapter.GetV3NewCustomerCD(V3cst_id)

                    '取得した値を格納
                    If V3NewCstCDDt.Count() > 0 Then
                        Dim V3NewCstCDRW As SC3080204DataSet.SC3080204CustomerCodeRow
                        V3NewCstCDRW = CType(V3NewCstCDDt.Rows(0), SC3080204DataSet.SC3080204CustomerCodeRow)
                        ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い START
                        'V3NewCstCD = V3NewCstCDRW.CST_CD
                        V3NewCstCD = V3NewCstCDRW.NEWCST_CD
                        ' 2016/06/14 NSK 皆川 TR-SVT-TMT-20160524-001 メモ機能の参照・登録の反応速度が遅い END
                    End If

                End If

                'V3の顧客メモ取得
                Dim retMemoV3DataTbl As SC3080204DataSet.SC3080204CustMemoDataTable
                retMemoV3DataTbl = SC3080204DataTableTableAdapter.GetV3CustomerMemo(memoDataRow.DLRCD, V3cst_id, V3NewCstCD)
                retMemoDataTbl.Merge(retMemoV3DataTbl)

            End If

        End If
        '2014/12/03 TMEJ 成澤 IT9842_新プラットフォーム版e-CRB 移行方式見直しに伴う仕様変更対応 END

        For Each retMemoDataRow In retMemoDataTbl

            '日付変換
            retMemoDataRow.UPDATEDATESTR = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                          retMemoDataRow.UPDATEDATE, _
                                                                          memoDataRow.DLRCD)

            'TODO:後から消す
            'メモを26バイト以降を...に変換
            'Dim bEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
            'Dim btByte As Byte() = bEncoding.GetBytes(retMemoDataRow.MEMO)

            'If (btByte.Length <= 26) Then
            '    retMemoDataRow.MEMODISP = retMemoDataRow.MEMO
            'Else
            '    retMemoDataRow.MEMODISP = bEncoding.GetString(btByte, 0, 24) & "..."
            'End If

            '最初の行をセットする (Windows=CRLF , iPad=LF)
            retMemoDataRow.FIRSTMEMO = String.Empty
            Dim memos As String() = retMemoDataRow.MEMO.Split(New Char() {CChar(vbCrLf)})
            If (memos.Length = 1) Then
                memos = memos(0).Split(New Char() {CChar(vbCrLf)})
            End If
            Dim i As Integer
            For i = 0 To memos.Length - 1
                If (memos(i).Length > 0) Then
                    retMemoDataRow.FIRSTMEMO = memos(i)
                    Exit For
                End If
            Next

            'TODO:あとから修正
            'retMemoDataRow.UPDATEDATEDAY = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
            '                                                              retMemoDataRow.UPDATEDATE, _
            '                                                              Now, _
            '                                                              memoDataRow.DLRCD, _
            '                                                              False)

            retMemoDataRow.UPDATEDATEDAY = Format(retMemoDataRow.UPDATEDATE, "yyyy/MM/dd")
            retMemoDataRow.UPDATEDATETIME = Format(retMemoDataRow.UPDATEDATE, "HH:mm")
        Next

        Return retMemoDataTbl

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
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    <EnableCommit()>
    Public Function InsertCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)


        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        Try
            '行ロック
            SC3080204DataTableTableAdapter.GetCustomerLock(memoDataRow.CRCUSTID)

        Catch ex As OracleExceptionEx When ex.Number = 30006
            '行ロック失敗(WAIT時間超え)
            Return 0
        End Try

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '顧客メモ連番采番
        Dim seqno As Long
        seqno = SC3080204DataTableTableAdapter.GetCustmemoseq(memoDataRow.DLRCD, memoDataRow.CRCUSTID)
        memoDataRow.CUSTMEMOHIS_SEQNO = seqno

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '顧客メモ追加
        'ret = SC3080204DataTableTableAdapter.InsertCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
        '                                memoDataRow.DLRCD, _
        '                                memoDataRow.STRCD, _
        '                                memoDataRow.CUSTSEGMENT, _
        '                                memoDataRow.CUSTOMERCLASS, _
        '                                memoDataRow.CRCUSTID, _
        '                                memoDataRow.CRCUSTNAME, _
        '                                memoDataRow.MEMO, _
        '                                memoDataRow.ACCOUNT)

        ret = SC3080204DataTableTableAdapter.InsertCustomerMemo(memoDataRow.DLRCD, _
                                                                memoDataRow.CRCUSTID, _
                                                                memoDataRow.CUSTMEMOHIS_SEQNO, _
                                                                memoDataRow.MEMO, _
                                                                memoDataRow.ACCOUNT)

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        ''更新に失敗していたらロールバック
        'If ret = 0 Then
        '    Rollback = True
        '    Return 0
        'End If

        Return ret

    End Function

    ''' <summary>
    ''' 顧客メモ更新処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を更新する。</remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    <EnableCommit()>
    Public Function UpdateCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)


        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '顧客メモ更新
        'ret = SC3080204DataTableTableAdapter.UpdateCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
        '                            memoDataRow.MEMO, _
        '                            memoDataRow.ACCOUNT)

        Try
            '行ロック
            SC3080204DataTableTableAdapter.GetCustomerLock(memoDataRow.CRCUSTID)

        Catch ex As OracleExceptionEx When ex.Number = 30006
            '行ロック失敗(WAIT時間超え)
            Return 0
        End Try
        
        '顧客メモ更新
        ret = SC3080204DataTableTableAdapter.UpdateCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                                                memoDataRow.MEMO, _
                                                                memoDataRow.ACCOUNT, _
                                                                memoDataRow.DLRCD, _
                                                                memoDataRow.CRCUSTID, _
                                                                memoDataRow.ROW_LOCK_VERSION)

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''更新に失敗していたらロールバック
        If ret = 0 Then
            Me.Rollback = True
            Return -1
        End If

        Return ret

    End Function

    ''' <summary>
    ''' 顧客メモ削除処理
    ''' </summary>
    ''' <param name="memoDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を削除する。</remarks>
    ''' <history>
    ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    <EnableCommit()>
    Public Function DeleteCustomerMemo(ByVal memoDataTbl As SC3080204DataSet.SC3080204CustMemoDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim memoDataRow As SC3080204DataSet.SC3080204CustMemoRow

        memoDataRow = memoDataTbl.Item(0)

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '顧客メモ削除
        'ret = SC3080204DataTableTableAdapter.DeleteCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
        '                            memoDataRow.ACCOUNT)

        Try
            '行ロック
            SC3080204DataTableTableAdapter.GetCustomerLock(memoDataRow.CRCUSTID)

        Catch ex As OracleExceptionEx When ex.Number = 30006
            '行ロック失敗(WAIT時間超え)
            Return 0
        End Try

        '顧客メモ履歴移動
        ret = SC3080204DataTableTableAdapter.MoveCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                                              memoDataRow.DLRCD, _
                                                              memoDataRow.CRCUSTID)

        '顧客メモ削除
        ret = SC3080204DataTableTableAdapter.DeleteCustomerMemo(memoDataRow.CUSTMEMOHIS_SEQNO, _
                                                                memoDataRow.DLRCD, _
                                                                memoDataRow.CRCUSTID)

        '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''更新に失敗していたらロールバック
        'If ret = 0 Then
        '    Me.Rollback = True
        '    Return 0
        'End If

        Return ret

    End Function


End Class
