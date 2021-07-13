'-------------------------------------------------------------------------
'SC3040801DateSet.vb
'-------------------------------------------------------------------------
'機能：通知履歴
'補足：
'作成：2012/02/3 KN 河原 【servive_1】
'更新：2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加
'─────────────────────────────────────


Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3040801DataSetTableAdapters

    ''' <summary>
    ''' 通知履歴のデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3040801TableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub

#Region "定数"

        ''' <summary>
        ''' ROWカウント
        ''' </summary>
        Private Const RowNextRowsCount As Integer = 6

        ''' <summary>
        ''' 既読フラグ
        ''' </summary>
        Private Const ReadList As String = "1"

        ''' <summary>
        ''' ステータスキャンセル
        ''' </summary>
        Private Const Status As String = "2"

        ''' <summary>
        ''' 査定
        ''' </summary>
        Private Const Assessment As String = "01"

        ''' <summary>
        ''' 価格相談
        ''' </summary>
        Private Const Consultation As String = "02"

        ''' <summary>
        ''' ヘルプ
        ''' </summary>
        Private Const Help As String = "03"

        ''' <summary>
        ''' 来店者実績（商談中)
        ''' </summary>
        Private Const Negotiations As String = "07"

        ''' <summary>
        ''' 写真URLkey1
        ''' </summary>
        Private Const FilePath As String = "FILE_PATH_STAFFPHOTO"

        ''' <summary>
        ''' 写真URLkey2
        ''' </summary>
        Private Const FileUrl As String = "URI_STAFFPHOTO"

        ''' <summary>
        ''' 表示日付設定key
        ''' </summary>
        Private Const DisplayDayKey As String = "NOTICE_DISP_DAYS"

#End Region

#Region "メソッド"

        ''' <summary>
        ''' セールス通知履歴を取得
        ''' </summary>
        ''' <param name="userAccount">ログインユーザーアカウント</param>
        ''' <param name="beginRowIndex">リピータのスタート行</param>
        ''' <param name="displayDays">表示日数</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>GetSalesNoticeDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetSalesNotice(ByVal userAccount As String, _
                                              ByVal beginRowIndex As Integer, _
                                              ByVal displayDays As Date, _
                                              ByVal dealerCode As String, _
                                              ByVal storeCode As String) _
                                          As SC3040801DataSet.GetSalesNoticeDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_userAccount=" & _
                        userAccount & _
                        "_beginRowIndex=" & _
                        CStr(beginRowIndex) & _
                        "_DispDays=" & _
                        CStr(displayDays))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetSalesNoticeDataTable)("SC3040801_001")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("   SELECT  /* SC3040801_001 */")
                    .Append("		     A.NOTICEREQID ")
                    .Append("		    ,A.NOTICEREQCTG ")
                    .Append("		    ,A.CRCUSTID ")
                    .Append("		    ,NVL(A.CUSTOMNAME,'NONAME') AS CUSTOMNAME")
                    .Append("		    ,A.FLLWUPBOX ")
                    .Append("		    ,F.NOTICEID ")
                    .Append("           ,F.FROMACCOUNT ")
                    .Append("           ,F.FROMACCOUNTNAME ")
                    .Append("           ,F.TOACCOUNTNAME ")
                    .Append("		    ,F.READFLG ")
                    .Append("		    ,TO_CHAR(F.SENDDATE,'yyyy/mm/dd hh24:mi') AS SENDDATE ")
                    .Append("		    ,F.STATUS ")
                    .Append("           ,C.FLLWUPBOX_SEQNO ")
                    '.Append("		    ,NVL(D.SYS_IMGFILE,NVL(D.ORG_IMGFILE,'NOPHOTO')) AS SYS_IMGFILE")
                    .Append("		    ,D.ORG_IMGFILE ")
                    .Append("		    ,NVL(E.NOTICEMSG_DLR,NVL(E.NOTICEMSG_ENG,'NOMESSAGE')) AS NOTICEMSG_DLR ")
                    .Append("    FROM ")
                    .Append("			     TBL_NOTICEREQUEST A ")
                    .Append("			     ,( SELECT ")
                    .Append("			                MAX(Z.NOTICEID) AS NOTICEID ")
                    .Append("			          FROM      TBL_NOTICEINFO Z")
                    .Append("			         WHERE          Z.TOACCOUNT = :TOACCOUNT ")
                    .Append("			      GROUP BY              Z.NOTICEREQID ")
                    .Append("			                           ,Z.STATUS ")
                    .Append("			                           ,Z.FROMACCOUNT ")
                    .Append("			                           ,Z.TOACCOUNT ) B ")
                    .Append("			    ,TBL_NOTICEINFO F")
                    .Append("			    ,TBL_FLLWUPBOX C ")
                    .Append("			    ,TBL_USERS D")
                    .Append("			    ,TBL_NOTICECTGMST E ")
                    .Append("   WHERE 		    B.NOTICEID = F.NOTICEID ")
                    .Append("     AND   		A.NOTICEREQID = F.NOTICEREQID ")
                    .Append("     AND 		    A.STATUS = F.STATUS ")
                    .Append("     AND 		    A.FLLWUPBOX = C.FLLWUPBOX_SEQNO(+) ")
                    .Append("     AND 		    A.NOTICEREQCTG = E.NOTICEREQCTG ")
                    .Append("     AND 		    F.STATUS = E.NOTICESTATUSID ")
                    .Append("     AND 		    F.FROMACCOUNT = D.ACCOUNT(+) ")
                    .Append("     AND           A.NOTICEREQCTG in(:ASSESSMENT,:CONSULTATION,:HELP) ")
                    .Append("     AND           F.TOACCOUNT = :TOACCOUNT ")
                    .Append("     AND           C.DLRCD(+) = :DLRCD ")
                    .Append("     AND           C.STRCD(+) = :STRCD ")
                    .Append("     AND           F.SENDDATE >= TRUNC(:SENDDATE) ")

                    'スタート行が１行目以外は既読のみ読み取る
                    If beginRowIndex <> RowNextRowsCount Then
                        Logger.Info("WHERE.APPEND=(F.READFLG = :READFLG)")
                        .Append(" AND       F.READFLG = :READFLG ")
                    End If

                    .Append("ORDER BY           F.SENDDATE DESC ")
                    .Append("                  ,A.NOTICEREQID ASC ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("ASSESSMENT", OracleDbType.Char, Assessment)
                query.AddParameterWithTypeValue("CONSULTATION", OracleDbType.Char, Consultation)
                query.AddParameterWithTypeValue("HELP", OracleDbType.Char, Help)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)
                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, displayDays)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)

                'スタート行が１行目以外は既読のみ読み取る
                If beginRowIndex <> RowNextRowsCount Then
                    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, ReadList)
                End If

                Dim dt As SC3040801DataSet.GetSalesNoticeDataTable = query.GetData()

                Logger.Info("return=GetSalesNoticeDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' サービス通知履歴を取得
        ''' </summary>
        ''' <param name="userAccount">ログインユーザーアカウント</param>
        ''' <param name="beginRowIndex">リピータのスタート行</param>
        ''' <param name="displayDays">表示日数</param>
        ''' <returns>GetServiceNoticeDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetServiceNotice(ByVal userAccount As String, _
                                                ByVal beginRowIndex As Integer, _
                                                ByVal displayDays As Date) _
                                            As SC3040801DataSet.GetServiceNoticeDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_userAccount=" & _
                        userAccount & _
                        "_beginRowIndex=" & _
                        CStr(beginRowIndex) & _
                        "_displayDays=" & _
                        CStr(displayDays))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetServiceNoticeDataTable)("SC3040801_002")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("  SELECT /* SC3040801_002 */ ")
                    .Append("	 	    A.NOTICEREQID ")
                    .Append("	 	   ,B.NOTICEID ")
                    .Append("		   ,B.READFLG ")
                    .Append("		   ,B.SESSIONVALUE ")
                    '.Append("		   ,NVL(B.SESSIONVALUE,',') AS SESSIONVALUE")
                    .Append("		   ,TO_CHAR(B.SENDDATE,'yyyy/mm/dd hh24:mi') AS SENDDATE ")
                    .Append("		   ,NVL(B.MESSAGE,'NOMESSAGE') AS MESSAGE ")
                    '.Append("		   ,NVL(C.SYS_IMGFILE,NVL(C.ORG_IMGFILE,'NOPHOTO')) AS SYS_IMGFILE")
                    .Append("		   ,C.ORG_IMGFILE ")
                    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
                    .Append("		   ,NVL(B.SUPPORTSTATUS, '0') AS SUPPORTSTATUS")
                    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End
                    .Append("    FROM 		 TBL_NOTICEREQUEST A ")
                    .Append("			    ,TBL_NOTICEINFO B ")
                    .Append("			    ,TBL_USERS C ")
                    .Append("   WHERE 			A.NOTICEREQID = B.NOTICEREQID ")
                    .Append("     AND			B.FROMACCOUNT = C.ACCOUNT(+) ")
                    .Append("     AND           B.TOACCOUNT = :TOACCOUNT ")
                    .Append("     AND           B.SENDDATE >= TRUNC(:SENDDATE) ")

                    'スタート行が１行目以外は既読のみ読み取る
                    If beginRowIndex <> RowNextRowsCount Then
                        Logger.Info("WHERE.APPEND=(B.READFLG = :READFLG)")
                        .Append(" AND           B.READFLG = :READFLG ")
                    End If
                    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 Start
                    .Append("ORDER BY            B.SUPPORTSTATUS ASC ")
                    .Append("                   ,B.SENDDATE DESC ")
                    '.Append("ORDER BY            B.SENDDATE DESC ")
                    '2012/05/25 KN 長谷 【SERVICE_1】対応ステータスの追加 End
                    .Append("                   ,A.NOTICEREQID ASC ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)
                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, displayDays)

                'スタート行が１行目以外は既読のみ読み取る
                If beginRowIndex <> RowNextRowsCount Then
                    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, ReadList)
                End If

                Dim dt As SC3040801DataSet.GetServiceNoticeDataTable = query.GetData()

                Logger.Info("return=GetServiceNoticeDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 写真のURLの取得
        ''' </summary>
        ''' <param name="storeCode">販売店コード</param>
        ''' <param name="dealerCode">店舗コード</param>
        ''' <returns>写真のURL</returns>
        ''' <remarks></remarks>
        Public Function GetPhotoPath(ByVal dealerCode As String, _
                                            ByVal storeCode As String) _
                                        As SC3040801DataSet.GetPhotoPathDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_dealerCode=" & _
                        dealerCode & _
                        "_storeCode=" & _
                        storeCode)

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetPhotoPathDataTable)("SC3040801_003")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /* SC3040801_003 */ ")
                    .Append("       NVL(A.PARAMVALUE,'NOPHOTO') AS IPPATH ")
                    .Append("      ,NVL(B.PARAMVALUE,'NOPHOTO') AS IMGPATH ")
                    .Append("  FROM 	 TBL_DLRENVSETTING A ")
                    .Append("           ,TBL_DLRENVSETTING B ")
                    .Append(" WHERE 		A.PARAMNAME = :URL1 ")
                    .Append("   AND 		A.STRCD = :STRCD ")
                    .Append("   AND 		A.DLRCD = :DLRCD ")
                    .Append("   AND 		B.PARAMNAME = :URL2 ")
                    .Append("   AND 		B.STRCD = :STRCD ")
                    .Append("   AND		    B.DLRCD = :DLRCD ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("URL1", OracleDbType.Varchar2, FilePath)
                query.AddParameterWithTypeValue("URL2", OracleDbType.Varchar2, FileUrl)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)

                Dim dt As SC3040801DataSet.GetPhotoPathDataTable = query.GetData()

                Logger.Info("return=GetPhotoPathDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")

                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 最終ステータスの確認
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <returns>最終ステータス</returns>
        ''' <remarks></remarks>
        Public Function GetLastStatus(ByVal noticeRequestId As Long) _
                   As SC3040801DataSet.GetLastStatusDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetLastStatusDataTable)("SC3040801_004")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /* SC3040801_004 */ ")
                    .Append("       STATUS ")
                    .Append("  FROM     TBL_NOTICEREQUEST ")
                    .Append(" WHERE          NOTICEREQID = :NOTICEREQID ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)

                Dim dt As SC3040801DataSet.GetLastStatusDataTable = query.GetData()

                Logger.Info("return=GetLastStatusDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' session情報
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <returns>GetTransitionParameterDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetTransitionParameter(ByVal noticeRequestId As Long) _
                   As SC3040801DataSet.GetTransitionParameterDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetTransitionParameterDataTable)("SC3040801_005")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("	SELECT /* SC3040801_005 */ ")
                    .Append("				  REQCLASSID ")
                    .Append("				 ,CRCUSTID ")
                    .Append("				 ,CSTKIND ")
                    .Append("				 ,CUSTOMERCLASS ")
                    .Append("				 ,SALESSTAFFCD ")
                    .Append("				 ,VCLID ")
                    .Append("				 ,FLLWUPBOX ")
                    .Append("				 ,FLLWUPBOXSTRCD ")
                    .Append("	 FROM           TBL_NOTICEREQUEST ")
                    .Append("   WHERE 				NOTICEREQID = :NOTICEREQID ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)

                Dim dt As SC3040801DataSet.GetTransitionParameterDataTable = query.GetData()

                Logger.Info("return=GetTransitionParameterDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' CANCEL処理を行うパラメーターの取得
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <param name="userAccount">ユーザーアカウント</param>
        ''' <returns>GetCancelParameterDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetCancelParameter(ByVal noticeRequestId As Long, _
                                           ByVal userAccount As String) _
                                           As SC3040801DataSet.GetCancelParameterDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetCancelParameterDataTable)("SC3040801_006")

                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3040801_006 */ ")
                    .Append("           A.NOTICEREQID ")
                    .Append("          ,A.NOTICEREQCTG ")
                    .Append("          ,A.REQCLASSID ")
                    .Append("          ,A.CUSTOMNAME ")
                    .Append("          ,B.FROMCLIENTID ")
                    .Append("          ,B.TOACCOUNT ")
                    .Append("          ,B.TOCLIENTID ")
                    .Append("          ,B.TOACCOUNTNAME ")
                    .Append("   FROM     TBL_NOTICEREQUEST A ")
                    .Append("           ,TBL_NOTICEINFO B ")
                    .Append("  WHERE        A.NOTICEREQID = B.NOTICEREQID ")
                    .Append("    AND        A.STATUS = B.STATUS ")
                    .Append("    AND        A.NOTICEREQID = :NOTICEREQID ")
                    .Append("    AND        A.LASTNOTICEID <= B.NOTICEID ")
                    .Append("    AND       (B.TOACCOUNT <> :TOACCOUNT ")
                    .Append("     OR        B.TOACCOUNT IS NULL) ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)

                Dim dt As SC3040801DataSet.GetCancelParameterDataTable = query.GetData()

                Logger.Info("return=GetCancelParameterDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

#End Region

    End Class

End Namespace

Partial Class SC3040801DataSet
    Partial Class SalesNoticeHistoryDataTable

        Private Sub SalesNoticeHistoryDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.SESSIONVALUEColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class