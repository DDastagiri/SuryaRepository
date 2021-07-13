'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290104BusinessLogic.vb
'─────────────────────────────────────
'機能： フォロー設定データアクセス
'補足： 
'作成： 2014/06/11 TMEJ t.mizumoto
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web
Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess

''' <summary>
''' SC3290104 フォロー設定画面 ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3290104BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3290104BusinessLogic

#Region "定数"

    ''' <summary>
    ''' 正常
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNormal As Integer = 0

#End Region


#Region "公開メソッド"

    ''' <summary>
    ''' 異常項目フォローの取得
    ''' </summary>
    ''' <param name="irregFllwId">異常フォローID</param>
    ''' <param name="irregClassCode">異常分類コード</param>
    ''' <param name="irregItemCode">異常項目コード</param>
    ''' <param name="stfCode">スタッフコード</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>引数で指定した値に一致した異常項目フォローの行を返却。データが存在しない場合はNothingを返却。</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularFollowInfo(ByVal irregFllwId As String, ByVal irregClassCode As String, ByVal irregItemCode As String, _
                                           ByVal stfCode As String, ByVal nowDate As Date) As SC3290104DataSet.SC3290104IrregFllwRow

        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularFollowInfo_Start ")
            .Append("irregFllwId[" & irregFllwId & "]")
            .Append("irregClassCd[" & irregClassCode & "]")
            .Append(",irregItemCd[" & irregItemCode & "]")
            .Append(",stfCd[" & stfCode & "]")
            .Append(",nowDate[" & nowDate & "]")
        End With
        Logger.Info(startLog.ToString)

        Dim row As SC3290104DataSet.SC3290104IrregFllwRow

        Using adapter As New SC3290104DataSetTableAdapters.SC3290104TableAdapter

            ' 異常項目フォローの取得
            row = adapter.GetIrregularFollowInfo(irregFllwId, irregClassCode, irregItemCode, stfCode, nowDate)

        End Using

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetIrregularFollowInfo_End Ret:[")
            .Append(IsNothing(row))
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return row
    End Function

    ''' <summary>
    ''' 異常項目フォローの設定
    ''' </summary>
    ''' <param name="row">異常項目フォローの行</param>
    ''' <param name="account">更新するアカウント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()> _
    Public Function SetIrregularFollowInfo(ByVal row As SC3290104DataSet.SC3290104IrregFllwRow, ByVal account As String, ByVal nowDate As Date) As Integer _
    Implements ISC3290104BusinessLogic.SetIrregularFollowInfo

        Dim startLog As New StringBuilder
        With startLog
            .Append("SetIrregularFollowInfo_Start ")
            .Append("row[")
            .Append(IsNothing(row))
            .Append("]")
            .Append(",account[" & account & "]")
            .Append(",nowDate[" & nowDate & "]")
        End With
        Logger.Info(startLog.ToString)

        Try

            Using adapter As New SC3290104DataSetTableAdapters.SC3290104TableAdapter

                ' 異常項目フォローの設定
                adapter.SetIrregularFollowInfo(row, account, nowDate)

            End Using

        Catch ex As OracleExceptionEx

            ' データベースの操作中に例外が発生した場合
            Logger.Error("SetIrregularFollowInfo_001 " & "Catch OracleExceptionEx")
            Logger.Error("SetIrregularFollowInfo_End Ret[Throw OracleExceptionEx]")
            Logger.Error("ErrorID:" & CStr(ex.Number) & "Exception:" & ex.Message)
            Throw

        End Try

        Logger.Info("SetIrregularFollowInfo_End Ret[ " + MessageIdNormal.ToString(CultureInfo.InvariantCulture()) + " ]")
        Return MessageIdNormal
    End Function

#End Region

End Class
