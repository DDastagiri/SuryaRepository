'===================================================================
' SC3100401.aspx.vb
'-------------------------------------------------------------------
' 機能：未振当て一覧画面 PL層
' 補足：               
' 作成：2013/03/01 TMEJ 河原 
' 更新：2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
' 更新：2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
' 更新：2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
' 更新：2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
' 更新：2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
' 更新：2019/06/14 NSK 近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される
'===================================================================

Option Explicit On
Option Strict On

Imports System.Text
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.iCROP.BizLogic.SC3100401
Imports Toyota.eCRB.iCROP.BizLogic.SC3100401.SC3100401BusinessLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3100401

'2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END


''' <summary>
''' SC3100401
''' 未振当て一覧画面
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3100401
    Inherits BasePage
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' アプリケーションID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationID As String = "SC3100401"

    ''' <summary>
    ''' 来店管理画面ID
    ''' </summary>
    Private Const WellComeBoardID As String = "SC3100303"

    ''' <summary>
    ''' 来店一覧の初期表示最大行数
    ''' </summary>
    Private Const ReceptionListMaxRow As Integer = 8

    ''' <summary>
    ''' 呼出ステータス「1：呼出中」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CallStatusCalling As String = "1"

    ''' <summary>
    ''' 振当てステータス「2：振当済」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssignStatusFin As String = "2"

    ''' <summary>
    ''' 予約情報表示欄スタイル「文字：青」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BlueStyle As String = "TimeBlue"

    ''' <summary>
    ''' 性別「0：男性」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Male As String = "0"

    ''' <summary>
    ''' システムフォーマットフラグ「1：フォーマット有り」
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SystemFormatFlag As String = "1"

    ''' <summary>
    ''' 車両登録No制限文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RegNoCurrentDigit As Integer = 16

    ''' <summary>
    ''' 車両登録Noチェックフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RegNoCheckFlag As String = "VCLREGNO_CHECK_FLG"

    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
    ' ''' <summary>
    ' ''' 車両登録Noチェックフォーマット
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const RegNoFormat As String = "^[a-zA-Z\d\u4E00-\u9FFF]+([-a-zA-Z\d\u4E00-\u9FFF]{0,10})?-+([-a-zA-Z\d\u4E00-\u9FFF]{0,10})?[a-zA-Z\d]{4}$"

    ''' <summary>
    ''' 車両登録Noチェックフォーマット
    ''' 1文字以上 + (- or 半角スペース1文字) + 1文字以上
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RegNoFormat As String = "((.){1,})+(-| )+((.){1,})"

    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' RO情報有無(1：情報あり)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderInfoExist As String = "1"

    ''' <summary>
    ''' 販売店設定テーブル(受付待ちモニターフラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DLRENV_WAIT_RECEPTION_TYPE As String = "WAIT_RECEPTION_TYPE"

    ''' <summary>
    ''' 受付待ちモニターフラグ(0：表示しない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WAIT_RECEPTION_TYPE_NONE As String = "0"

    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    Private Const WholeManagementID As String = "SC3220201"
    ''' <summary>
    ''' 他システム連携画面ID
    ''' </summary>
    Private Const OtherLinkageID As String = "SC3010501"

    ''' <summary>
    ''' セッションデータ(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataRepairOrder As Long = 14

    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(画面番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyDisplayNumber As String = "Session.DISP_NUM"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam1 As String = "Session.Param1"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam2 As String = "Session.Param2"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam3 As String = "Session.Param3"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター4)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam4 As String = "Session.Param4"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター5)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam5 As String = "Session.Param5"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター6)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam6 As String = "Session.Param6"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター7)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam7 As String = "Session.Param7"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam8 As String = "Session.Param8"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター9)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyParam9 As String = "Session.Param9"

    ''' <summary>
    ''' セッションキデータ(編集モード(0：編集))
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_DATA_VIEWMODE_EDIT As String = "0"

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
    ''' <summary>
    ''' 新規顧客ボタン押下時のID設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCUSTOMER_ID As Decimal = -1
    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

    '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' Pマークフラグ　(1：Pマーク表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PmarkFlg As String = "1"
    ''' <summary>
    ''' Lマークフラグ　(2：Lマーク表示)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LmarkFlg As String = "2"
    '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

#Region "Enum"

    ''' <summary>
    ''' PostBackID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum PostBackID

        ''' <summary>車両登録No登録ボタン</summary>
        RegisterRegNoButton = 1

        ''' <summary>SA振当登録ボタン</summary>
        SAAssignButton = 2

        ''' <summary>フッターボタン</summary>
        FooterButton = 3

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        ''' <summary>register</summary>
        RegisterVehicleButton = 4
        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

    End Enum

    ''' <summary>
    ''' イベントキーID
    ''' </summary>
    Private Enum EventKeyId

        ''' <summary>
        ''' 呼出ボタン
        ''' </summary>
        FooterCallButton = 100
        ''' <summary>
        ''' 呼出キャンセルボタン
        ''' </summary>
        FooterCancelButton = 200
        ''' <summary>
        ''' チップ削除ボタン
        ''' </summary>
        FooterDeleteButton = 300

        ''' <summary>
        ''' 発券番号テキスト
        ''' </summary>
        ReceiptNoText = 1100
        ''' <summary>
        ''' 車両登録Noテキスト
        ''' </summary>
        RegNoText = 1200
        ''' <summary>
        ''' 来店者テキスト
        ''' </summary>
        VisitorText = 1300
        ''' <summary>
        ''' 電話番号テキスト
        ''' </summary>
        TellNoText = 1400
        ''' <summary>
        ''' テーブルNoテキスト
        ''' </summary>
        TableNoText = 1500

        ''' <summary>
        ''' SA解除
        ''' </summary>
        SAUndo = 3200

    End Enum

    ''' <summary>
    ''' 文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordId

        ''' <summary>
        ''' 敬称(男)
        ''' </summary>
        ''' <remarks></remarks>
        Id017 = 17

        ''' <summary>
        ''' 敬称(女)
        ''' </summary>
        ''' <remarks></remarks>
        Id018 = 18

        ''' <summary>
        ''' 10キー入力画面の確定ボタン名
        ''' </summary>
        ''' <remarks></remarks>
        Id030 = 30

        ''' <summary>
        ''' 敬称(女)10キー入力画面のキャンセルボタン名
        ''' </summary>
        ''' <remarks></remarks>
        Id031 = 31

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        ''' <summary>
        ''' ハイフン
        ''' </summary>
        ''' <remarks></remarks>
        Id044 = 44

        ''' <summary>
        ''' 新規顧客(車両選択ポップアップのフッターボタン)
        ''' </summary>
        ''' <remarks></remarks>
        Id045 = 45
        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' P
        ''' </summary>
        ''' <remarks></remarks>
        Id10001 = 10001

        ''' <summary>
        ''' L
        ''' </summary>
        ''' <remarks></remarks>
        Id10002 = 10002
        '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        ''' <summary>
        ''' 担当SAを変更しますか？
        ''' </summary>
        Id905 = 905

        ''' <summary>
        ''' 選択された顧客のSA振当を解除しますか？
        ''' </summary>
        Id906 = 906

        ''' <summary>
        ''' No.を入力してください
        ''' </summary>
        Id907 = 907

        ''' <summary>
        ''' 車両登録Noを入力してください
        ''' </summary>
        Id909 = 909

        ''' <summary>
        ''' テーブルNoを入力してください
        ''' </summary>
        Id911 = 911

        ''' <summary>
        ''' 担当SAが選択されていません。担当SAを選択してください。
        ''' </summary>
        Id913 = 913

        ''' <summary>
        ''' 選択したNoのお客様をお呼出しますか？
        ''' </summary>
        Id914 = 914

        ''' <summary>
        ''' 選択したNoの呼出をキャンセルしますか？
        ''' </summary>
        Id915 = 915

        ''' <summary>
        ''' 選択した来店情報を削除しますか？
        ''' </summary>
        Id916 = 916

        ''' <summary>
        ''' 該当顧客は既に呼出中です。
        ''' </summary>
        Id917 = 917

        ''' <summary>
        ''' 該当顧客は呼出中ではありません。
        ''' </summary>
        Id919 = 919

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        Id922 = 922

        ''' <summary>
        ''' 担当SAが変更されていないため、担当SA変更処理を中断します。
        ''' </summary>
        Id923 = 923

        ''' <summary>
        ''' 該当チップはSA振当済または呼出中のため削除できません。
        ''' </summary>
        Id924 = 924

    End Enum

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

        ''' <summary>
        ''' 車両登録No禁止文字エラー
        ''' </summary>
        ErrRegNoValidation = 910

        ''' <summary>
        ''' テーブルNo禁止文字エラー
        ''' </summary>
        ErrTableNoValidation = 912

        ''' <summary>
        ''' 呼出処理エラー
        ''' </summary>
        ErrCall = 918

        ''' <summary>
        ''' 呼出キャンセル処理エラー
        ''' </summary>
        ErrCallCancel = 920

        ''' <summary>
        ''' チップ削除処理エラー
        ''' </summary>
        ErrTipDelete = 921

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ErrOutType = 922

        ''' <summary>
        ''' 来店者禁止文字エラー
        ''' </summary>
        ErrVisitorValidation = 925

        ''' <summary>
        ''' 電話番号禁止文字エラー
        ''' </summary>
        ErrTellNoValidation = 926

        ''' <summary>
        ''' 車両登録Noフォーマットエラー
        ''' </summary>
        ErrRegNoFormat = 927

        ''' <summary>
        ''' 車両登録No文字数オーバーエラー
        ''' </summary>
        ErrRegNoByteLength = 928

    End Enum

#End Region

#Region "クライアント文言用(メッセージ)クラス"

    ''' <summary>
    ''' クライアント文言(メッセージ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ClientMessageClass

        ''' <summary>
        ''' 10キー入力画面の確定ボタン名
        ''' </summary>
        ''' <remarks></remarks>
        Public Property id030 As String

        ''' <summary>
        ''' 敬称(女)10キー入力画面のキャンセルボタン名
        ''' </summary>
        ''' <remarks></remarks>
        Public Property id031 As String

        ''' <summary>
        ''' 担当SAを変更しますか？
        ''' </summary>
        Public Property id905 As String

        ''' <summary>
        ''' 選択された顧客のSA振当を解除しますか？
        ''' </summary>
        Public Property id906 As String

        ''' <summary>
        ''' No.を入力してください
        ''' </summary>
        Public Property id907 As String

        ''' <summary>
        ''' 車両登録Noを入力してください
        ''' </summary>
        Public Property id909 As String

        ''' <summary>
        ''' テーブルNoを入力してください
        ''' </summary>
        Public Property id911 As String

        ''' <summary>
        ''' 担当SAが選択されていません。担当SAを選択してください。
        ''' </summary>
        Public Property id913 As String

        ''' <summary>
        ''' 選択したNoのお客様をお呼出しますか？
        ''' </summary>
        Public Property id914 As String

        ''' <summary>
        ''' 選択したNoの呼出をキャンセルしますか？
        ''' </summary>
        Public Property id915 As String

        ''' <summary>
        ''' 選択した来店情報を削除しますか？
        ''' </summary>
        Public Property id916 As String

        ''' <summary>
        ''' 該当顧客は既に呼出中です。
        ''' </summary>
        Public Property id917 As String

        ''' <summary>
        ''' 該当顧客は呼出中ではありません。
        ''' </summary>
        Public Property id919 As String

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        Public Property id922 As String

        ''' <summary>
        ''' 担当SAが変更されていないため、担当SA変更処理を中断します。
        ''' </summary>
        Public Property id923 As String

        ''' <summary>
        ''' 該当チップはSA振当済または呼出中のため削除できません。
        ''' </summary>
        Public Property id924 As String

    End Class

#End Region

#Region "コールバック用内部クラス"

    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass

        ''' <summary>
        ''' 呼び出し元名(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Caller As String

        ''' <summary>
        ''' 呼び出し元のListクラス名(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ClassName As String

        ''' <summary>
        ''' 呼び出し元来店実績連番(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As String

        ''' <summary>
        ''' 呼び出し元変更前の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property BeforeValue As String

        ''' <summary>
        ''' 呼び出し元変更後の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property AfterValue As String

        ''' <summary>
        ''' 呼び出し元更新日時(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UpDateDate As String

    End Class

    ''' <summary>
    ''' コールバック処理用引数クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentManageClass

        ''' <summary>
        ''' 呼び出し元名(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Caller As String

        ''' <summary>
        ''' 呼び出し元のListクラス名(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ClassName As String

        ''' <summary>
        ''' 呼び出し元来店実績連番(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As Long

        ''' <summary>
        ''' 呼び出し元変更前の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property BeforeValue As String

        ''' <summary>
        ''' 呼び出し元変更後の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property AfterValue As String

        ''' <summary>
        ''' 呼び出し元更新日時(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UpDateDate As Date

        ''' <summary>
        ''' 返却コード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ReturnFlag As Boolean

    End Class

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Caller As String

        ''' <summary>
        ''' 呼び出し元のListクラス名(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ClassName As String

        ''' <summary>
        ''' 呼び出し元来店実績連番(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As Long

        ''' <summary>
        ''' 呼び出し元変更前の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property BeforeValue As String

        ''' <summary>
        ''' 呼び出し元変更後の値(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property AfterValue As String

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ResultCode As Integer

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Message As String

        ''' <summary>
        ''' 更新日時
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UpDateDate As String

    End Class

#End Region

#Region "ポストバック用引数の内部クラス"

    ''' <summary>
    ''' ポストバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PostBackArgumentClass

        ''' <summary>
        ''' 来店実績連番
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As Long

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        ''' <summary>
        ''' 予約ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property RezId As Decimal
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 車両登録No
        ''' </summary>
        ''' <remarks></remarks>
        Public Property RegNo As String

        ''' <summary>
        ''' イベントキーID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property EventKeyID As String

        ''' <summary>
        ''' 受付担当(予定)SAコード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property AfterAccount As String

        ''' <summary>
        ''' 更新日時
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UpDateDate As Date

        ''' <summary>
        ''' 返却コード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ReturnFlag As Boolean

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        ''' <summary>
        ''' 選択した顧客ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SelectCstId As Decimal

        ''' <summary>
        ''' 選択した車両ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SelectVclId As Decimal

        ''' <summary>
        ''' 選択した予約ID
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SelectRezId As Decimal
        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

    End Class

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callBackResult As String

#End Region

#Region "イベント"

    ''' <summary>
    ''' ページロードイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'コールバックスクリプト作成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3100401",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3100401.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3100401.packedArgument", _
                                                                      "gCallbackSC3100401.endCallback", "", False)
                          ),
            True
        )

        'ポストバック確認
        If Not IsPostBack And Not IsCallback Then
            '初期表示

            'フッターボタンの設定
            Me.InitFooterEvent()

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 初期表示ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Protected Sub MainLoadingButton_Click(sender As Object, e As System.EventArgs) Handles MainLoadingButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインユーザー
        Dim staffInfo As StaffContext = StaffContext.Current

        ' 画面初期化(文言関連)
        Me.InitDisp()

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ' 画面初期化(設定関連)
        Me.InitSetting(staffInfo)
        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


        '画面初期化(データ取得)
        Me.InitData(staffInfo)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 車両登録No登録ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RegisterRegNoButton_Click(sender As Object, e As System.EventArgs) Handles RegisterRegNoButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'クライアントから必要パラメータの取得
        Dim parameter As PostBackArgumentClass = Me.GetEventArgs(PostBackID.RegisterRegNoButton)

        Try

            'パラメータの取得確認
            If Not parameter.ReturnFlag Then
                '取得失敗

                'エラーメッセージの表示
                Me.ShowMessageBox(ResultCode.ErrOutType)

                '処理終了
                Exit Sub

            End If

            '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
            'フォーマットチェック
            Dim formatCheckResult As Integer = Me.RegNoFormatCheck(parameter.RegNo)

            If Not formatCheckResult = ResultCode.Success Then
                'エラーメッセージの表示
                Me.ShowMessageBox(formatCheckResult)

                '処理終了
                Exit Sub
            End If
            '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
            '車両登録No登録処理
            Me.RegistRegNoButton(parameter)

        Finally
            '最終処理
            parameter = Nothing
        End Try

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        Me.ContentUpdateButtonPanel.Update()
        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' SA振当ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RegisterAssignButton_Click(sender As Object, e As System.EventArgs) Handles RegisterAssignButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'クライアントから必要パラメータの取得
        Dim parameter As PostBackArgumentClass = Me.GetEventArgs(PostBackID.SAAssignButton)

        Try

            'パラメータの取得確認
            If Not parameter.ReturnFlag Then
                '取得失敗

                'エラーメッセージの表示
                Me.ShowMessageBox(ResultCode.ErrOutType)

                '処理終了
                Exit Sub

            End If

            'SA録処理
            Me.RegisterSAButton(parameter)

        Finally
            '最終処理
            parameter = Nothing
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' フッター「来店管理」ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub VisitManageButton_Click(sender As Object, e As System.EventArgs) Handles VisitManageButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '来店管理に画面遷移
        Me.RedirectNextScreen(WellComeBoardID)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' カスタムフッターボタン「呼出・呼出キャンセル・チップ削除」
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CustomFooterButton_Click(sender As Object, e As System.EventArgs) Handles CustomFooterButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'クライアントから必要パラメータの取得
        Dim parameter As PostBackArgumentClass = Me.GetEventArgs(PostBackID.FooterButton)

        Try

            'パラメータの取得確認
            If parameter.ReturnFlag Then
                '取得成功

                '登録処理
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'Me.CustomFooterRegister(parameter.VisitSeq, parameter.UpDateDate, parameter.EventKeyID)
                Me.CustomFooterRegister(parameter.VisitSeq, _
                                        parameter.RezId, _
                                        parameter.UpDateDate, _
                                        parameter.EventKeyID)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            Else
                '取得失敗

                'フッターボタンごと処理分岐
                Select Case parameter.EventKeyID
                    Case CType(EventKeyId.FooterCallButton, String)
                        '呼出

                        'エラーメッセージの表示
                        Me.ShowMessageBox(ResultCode.ErrCall)

                    Case CType(EventKeyId.FooterCancelButton, String)
                        '呼出キャンセル

                        'エラーメッセージの表示
                        Me.ShowMessageBox(ResultCode.ErrCallCancel)

                    Case CType(EventKeyId.FooterDeleteButton, String)
                        'チップ削除

                        'エラーメッセージの表示
                        Me.ShowMessageBox(ResultCode.ErrTipDelete)

                    Case Else
                        '上記以外

                        'エラーメッセージの表示
                        Me.ShowMessageBox(ResultCode.ErrOutType)

                End Select
            End If

        Finally
            '最終処理
            parameter = Nothing
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
    ''' <summary>
    ''' 車両選択ポップアップボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub PopupVehicleListEventButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopupVehicleListEventButton.Click

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'クライアントから必要パラメータの取得
        Dim parameter As PostBackArgumentClass = Me.GetEventArgs(PostBackID.RegisterVehicleButton)

        Try

            'パラメータの取得確認
            If Not parameter.ReturnFlag Then
                '取得失敗

                'エラーメッセージの表示
                Me.ShowMessageBox(ResultCode.ErrOutType)

                '処理終了
                Exit Sub

            End If

            '車両情報登録処理
            Me.UpdateVisitVehicle(parameter)

        Finally
            '最終処理
            parameter = Nothing
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

#End Region

#Region "コールバック処理"

#Region "Publicメソッド"

    ''' <summary>
    ''' コールバック開始時処理
    ''' </summary>
    ''' <param name="eventArgument"></param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'シリアライザー
        Dim serializer = New JavaScriptSerializer

        'コールバック引数用内部クラスのインスタンスを生成し、JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New CallBackArgumentClass
        argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

        'クライアント側より受取ったパラメータのチェック
        Dim argumentManage As CallBackArgumentManageClass = Me.CheckArgs(argument)

        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        Try

            'クライアント側より受取ったパラメータの確認
            If Not argumentManage.ReturnFlag Then
                'パラメータが異常の場合

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} AFTERVALUE IS ERR WORD TEXT" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                result.Caller = argument.Caller                                                                 '呼出元
                result.ClassName = argument.ClassName                                                           'Listクラス名
                result.ResultCode = ResultCode.ErrOutType                                                       '結果コード
                result.Message = WebWordUtility.GetWord(ApplicationID, ResultCode.ErrOutType)                   'エラーメッセージ

                '処理結果をコールバック返却用文字列に設定
                Me.callBackResult = serializer.Serialize(result)

                '処理終了
                Exit Sub

            End If

            'パラメータ正常

            'テキスト登録処理
            Me.CallBackRegister(argumentManage, result, serializer)

        Finally
            '最終処理
            serializer = Nothing
            argument = Nothing
            argumentManage = Nothing
            result = Nothing
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' コールバック終了時処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '返却文字列
        Return Me.callBackResult

    End Function

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' クライアント側より受取ったパラメータのチェック
    ''' </summary>
    ''' <param name="inArgument"></param>
    ''' <returns>パラメータ</returns>
    ''' <remarks></remarks>
    Private Function CheckArgs(ByVal inArgument As CallBackArgumentClass) As CallBackArgumentManageClass

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} START" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理用引数クラス
        Dim manageArgument As New CallBackArgumentManageClass

        '初期値
        manageArgument.ReturnFlag = False

        '引数チェック
        If inArgument IsNot Nothing Then
            '引数が存在する

            '呼出元の確認
            If String.IsNullOrEmpty(inArgument.Caller) Then
                '存在しない

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} CALLER IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー処理終了
                Return manageArgument

            End If

            '呼出元の設定
            manageArgument.Caller = inArgument.Caller

            'クラス名の確認
            If String.IsNullOrEmpty(inArgument.ClassName) Then
                '存在しない

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} CLASSNAME IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー処理終了
                Return manageArgument

            End If

            'クラス名の設定
            manageArgument.ClassName = inArgument.ClassName

            '変換処理結果格納変数
            Dim formatResult As Boolean = False

            '来店実績連番をLong型に変換
            formatResult = Long.TryParse(inArgument.VisitSeq.Trim, manageArgument.VisitSeq)

            '来店実績連番変換結果確認
            If Not formatResult Then
                '変換失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} VISITSEQ IS NULL OR NOTHING" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー処理終了
                Return manageArgument

            End If

            '更新日時をDate型に変換
            formatResult = Date.TryParse(inArgument.UpDateDate.Trim, manageArgument.UpDateDate)

            '更新日時変換結果確認
            If Not formatResult Then
                '変換失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} UPDATETIME IS NULL OR NOTHING" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'エラー処理終了
                Return manageArgument

            End If

            '変更前の値の設定
            manageArgument.BeforeValue = inArgument.BeforeValue

            '変更後の値の設定
            manageArgument.AfterValue = inArgument.AfterValue

        Else
            '存在しない

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} CALLBACKARGUMENT IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラー処理終了
            Return manageArgument

        End If

        '初期値
        manageArgument.ReturnFlag = True

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return manageArgument

    End Function

    ''' <summary>
    ''' コールバック登録処理
    ''' </summary>
    ''' <param name="inArgumentManage">コールバック引数</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub CallBackRegister(ByVal inArgumentManage As CallBackArgumentManageClass, _
                                 ByVal inResult As CallBackResultClass, _
                                 ByVal inSerializer As JavaScriptSerializer)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        'エラーメッセージ
        Dim errMessage As String = String.Empty

        'ユーザー情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在時間の取得
        Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        '呼出元の確認
        Select Case inArgumentManage.Caller
            Case CType(EventKeyId.ReceiptNoText, String),
                 CType(EventKeyId.VisitorText, String),
                 CType(EventKeyId.TellNoText, String),
                 CType(EventKeyId.TableNoText, String)
                'テキストエリア「受付番号・来店者・電話番号・テーブルNo」

                '禁止文字チェック
                Dim returnFlag As Boolean = Me.ErrWordCheck(inArgumentManage.AfterValue)

                '禁止文字の確認
                If returnFlag Then
                    '登録テキスト正常

                    'Bizの宣言
                    Using sc3100401Biz As New SC3100401BusinessLogic

                        'テキスト登録処理
                        returnCode = sc3100401Biz.RegisterTextArea(inArgumentManage.VisitSeq, _
                                                                   inArgumentManage.UpDateDate, _
                                                                   inArgumentManage.Caller, _
                                                                   inArgumentManage.AfterValue, _
                                                                   staffInfo, _
                                                                   presentTime)

                        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                        '処理結果とイベントの確認
                        If returnCode = ResultCode.Success AndAlso _
                           CType(EventKeyId.ReceiptNoText, String).Equals(inArgumentManage.Caller) Then
                            '処理成功且つ、受付番号変更時の場合

                            'PCへのPush処理
                            sc3100401Biz.SendPushServerPC(staffInfo.DlrCD, _
                                                          staffInfo.BrnCD, _
                                                          CType(EventKeyId.ReceiptNoText, String))

                        End If
                        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                    End Using

                Else
                    '禁止文字有り
                    'エラー

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} CALLER{2} AFTERVALUE IS ERR WORD TEXT" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                inArgumentManage.Caller))

                    '呼出元の確認
                    Select Case inArgumentManage.Caller
                        Case CType(EventKeyId.ReceiptNoText, String)
                            '発券番号テキスト

                            'エラーコードの設定
                            returnCode = ResultCode.ErrOutType

                        Case CType(EventKeyId.VisitorText, String)
                            '来店者テキスト

                            'エラーコードの設定
                            returnCode = ResultCode.ErrVisitorValidation

                        Case CType(EventKeyId.TellNoText, String)
                            '電話番号テキスト

                            'エラーコードの設定
                            returnCode = ResultCode.ErrTellNoValidation

                        Case CType(EventKeyId.TableNoText, String)
                            'テーブルNoテキスト

                            'エラーコードの設定
                            returnCode = ResultCode.ErrTableNoValidation

                    End Select
                End If

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                'Case CType(EventKeyId.RegNoText, String)
                '    '車両登録Noテキストエリア

                '    '車両登録Noのフォーマットチェック設定
                '    returnCode = Me.RegNoFormatCheck(inArgumentManage.AfterValue)

                '    '来店実績連番の設定
                '    inResult.VisitSeq = inArgumentManage.VisitSeq

                '    '車両登録Noの設定
                '    inResult.AfterValue = inArgumentManage.AfterValue

                '    '更新処理はしていないので、元の更新日時を設定
                '    presentTime = inArgumentManage.UpDateDate

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
            Case Else
                '想定外のテキストエリア

                'エラーコードの設定
                returnCode = ResultCode.ErrOutType

        End Select

        '結果確認
        If returnCode <> ResultCode.Success Then
            '失敗

            'エラーメッセージの設定
            errMessage = WebWordUtility.GetWord(ApplicationID, returnCode)

        End If

        '処理結果をクライアントに返却用のクラスに格納
        inResult.Caller = inArgumentManage.Caller             '呼出元
        inResult.ClassName = inArgumentManage.ClassName       'Listクラス名
        inResult.BeforeValue = inArgumentManage.BeforeValue   '変更前の値
        inResult.ResultCode = returnCode                      '結果コード
        inResult.Message = errMessage                         'エラーメッセージ
        inResult.UpDateDate = CType(presentTime, String)      '最新更新日時

        ' 2019/06/14 NSK 近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される START
        inResult.VisitSeq = inArgumentManage.VisitSeq         '来店実績連番
        ' 2019/06/14 NSK 近藤 TKM PUAT-4066 TableNoに値を入力後、SAを振当てるとアラートメッセージが表示される END

        '処理結果をコールバック返却用文字列に設定
        Me.callBackResult = inSerializer.Serialize(inResult)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} RESULTCODE = {2} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, _
                    returnCode))

    End Sub

    ''' <summary>
    ''' 禁止文字チェック
    ''' </summary>
    ''' <param name="inAfterValue">変更後の値</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function ErrWordCheck(ByVal inAfterValue As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} AFTERVALUE:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inAfterValue))

        '処理結果
        Dim returnFlag As Boolean = True

        '禁止文字チェック
        If Not Validation.IsValidString(inAfterValue) Then
            '禁止文字有り

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} AFTERVALUE IS ERR WORD TEXT" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラー
            returnFlag = False

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnFlag))

        Return returnFlag

    End Function

    ''' <summary>
    ''' 車両登録Noフォーマットチェック
    ''' </summary>
    ''' <param name="inRegNo">車両登録No</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function RegNoFormatCheck(ByVal inRegNo As String) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        '車両登録Noの文字数制限確認
        If RegNoCurrentDigit < Me.ByteLengthCheck(inRegNo) Then
            '文字数オーバー

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} REGNO IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラー
            returnCode = ResultCode.ErrRegNoByteLength

            Return returnCode

            '処理終了
            Exit Function

        End If

        '車両登録No禁止文字チェック
        If Not Validation.IsValidString(inRegNo) Then
            '禁止文字有り

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} REGNO IS ERR WORD TEXT" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラー
            returnCode = ResultCode.ErrRegNoValidation

            Return returnCode

            '処理終了
            Exit Function

        End If

        'システム設定よりフォーマットチェックを行うかを取得
        Dim row As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
            (New SystemEnvSetting).GetSystemEnvSetting(RegNoCheckFlag)

        '車両登録Noの独自フォーマットチェック
        If (row IsNot Nothing AndAlso SystemFormatFlag.Equals(row.PARAMVALUE)) _
            AndAlso Not (New System.Text.RegularExpressions.Regex(RegNoFormat)).IsMatch(inRegNo) Then
            'フォーマット異常

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} REGNO IS NO MACH FORMAT" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'エラー
            returnCode = ResultCode.ErrRegNoFormat

            Return returnCode

            '処理終了
            Exit Function
        End If

        '処理結果
        returnCode = ResultCode.Success

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END RETURNCODE = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , returnCode))

        Return returnCode

    End Function

    ''' <summary>
    ''' 車両登録Noバイトレングスチェック
    ''' </summary>
    ''' <param name="inRegNo">車両登録No</param>
    ''' <returns>車両登録Noのバイト数</returns>
    ''' <remarks></remarks>
    Private Function ByteLengthCheck(ByVal inRegNo As String) As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START REGNO:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRegNo))

        '全角文字検索
        Dim regex As New RegularExpressions.Regex("^[ -~｡-ﾟ]")

        'レングス
        Dim length As Integer = 0

        '全角文字は２バイトと計算する
        For i As Integer = 0 To inRegNo.Length - 1

            '全角文字かチェック
            If regex.IsMatch(inRegNo(i)) = True Then
                '半角文字

                '1バイトで計算
                length += 1
            Else
                '全角文字

                '2バイトで計算
                length += 2
            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , length))

        Return length

    End Function

#End Region

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 初期表示文言取得
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitDisp()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'クライアント文言(メッセージ)クラス
        Dim clientWord As New ClientMessageClass

        'WORDID:030 設定
        clientWord.id030 = WebWordUtility.GetWord(ApplicationID, WordId.Id030)

        'WORDID:031 設定
        clientWord.id031 = WebWordUtility.GetWord(ApplicationID, WordId.Id031)

        'WORDID:905 設定
        clientWord.id905 = WebWordUtility.GetWord(ApplicationID, WordId.Id905)

        'WORDID:906 設定
        clientWord.id906 = WebWordUtility.GetWord(ApplicationID, WordId.Id906)

        'WORDID:907 設定
        clientWord.id907 = WebWordUtility.GetWord(ApplicationID, WordId.Id907)

        'WORDID:909 設定
        clientWord.id909 = WebWordUtility.GetWord(ApplicationID, WordId.Id909)

        'WORDID:911 設定
        clientWord.id911 = WebWordUtility.GetWord(ApplicationID, WordId.Id911)

        'WORDID:913 設定
        clientWord.id913 = WebWordUtility.GetWord(ApplicationID, WordId.Id913)

        'WORDID:914 設定
        clientWord.id914 = WebWordUtility.GetWord(ApplicationID, WordId.Id914)

        'WORDID:915 設定
        clientWord.id915 = WebWordUtility.GetWord(ApplicationID, WordId.Id915)

        'WORDID:916 設定
        clientWord.id916 = WebWordUtility.GetWord(ApplicationID, WordId.Id916)

        'WORDID:917 設定
        clientWord.id917 = WebWordUtility.GetWord(ApplicationID, WordId.Id917)

        'WORDID:919 設定
        clientWord.id919 = WebWordUtility.GetWord(ApplicationID, WordId.Id919)

        'WORDID:922 設定
        clientWord.id922 = WebWordUtility.GetWord(ApplicationID, WordId.Id922)

        'WORDID:923 設定
        clientWord.id923 = WebWordUtility.GetWord(ApplicationID, WordId.Id923)

        'WORDID:924 設定
        clientWord.id924 = WebWordUtility.GetWord(ApplicationID, WordId.Id924)

        'シリアライザー
        Dim serializer = New JavaScriptSerializer

        'Json形式にしてクライアントに設定
        Me.HiddenClientMessage.Value = serializer.Serialize(clientWord)

        '後処理
        serializer = Nothing
        clientWord = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 初期表示設定処理
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub InitSetting(ByVal inStaffInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '販売店システム設定より受付モニター使用フラグ取得
        Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = _
            (New DealerEnvSetting).GetEnvSetting(inStaffInfo.DlrCD, _
                                                 DLRENV_WAIT_RECEPTION_TYPE)

        '受付モニター使用フラグ取得確認
        If row IsNot Nothing AndAlso Not String.Empty.Equals(row.PARAMVALUE) Then
            '取得に成功

            'Hiddenに格納
            Me.HiddenReceptFlag.Value = row.PARAMVALUE

        Else
            '取得に失敗

            'Hiddenに格納
            Me.HiddenReceptFlag.Value = WAIT_RECEPTION_TYPE_NONE

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


    ''' <summary>
    ''' 初期表示データ取得
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub InitData(ByVal staffInfo As StaffContext)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Sub InitData()
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Biz
        Using SC3100401Biz As New SC3100401BusinessLogic

            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''ログインユーザー
            'Dim staffInfo As StaffContext = StaffContext.Current
            '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

            '現在時間取得
            Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

            'データアクセス
            Using SC3100401DataSet As New SC3100401DataSetTableAdapters.SC3100401TableAdapter

                Try
                    '右側(SAリスト表示)
                    Me.SetSAInfo(SC3100401Biz, SC3100401DataSet, staffInfo, presentTime)


                    '左側(来店リスト表示)
                    Me.SetReceptionInfo(SC3100401Biz, SC3100401DataSet, staffInfo, presentTime)

                Catch ex As OracleExceptionEx When ex.Number = 1013

                    'DBタイムアウトエラー
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} ERR:DBTIMEOUT" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    'エラーメッセージの表示
                    Me.ShowMessageBox(ResultCode.ErrDBTimeout)

                End Try
            End Using
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 来店一覧の設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub SetReceptionInfo(ByVal inSC3100401Biz As SC3100401BusinessLogic, _
                                 ByVal inSC3100401DataSet As SC3100401DataSetTableAdapters.SC3100401TableAdapter, _
                                 ByVal inStaffInfo As StaffContext, _
                                 ByVal inPresentTime As Date)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '来店一覧表示用情報
        Dim dtReceptionList As SC3100401DataSet.ReceptionListDataTable = _
            inSC3100401Biz.GetReceptionInfo(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inPresentTime, inSC3100401DataSet)

        '表示件数確認
        If dtReceptionList Is Nothing _
            OrElse dtReceptionList.Count = 0 Then

            'リストを追加する行数を格納(最大8行)
            Me.HiddenReceptionListCount.Value = CType(ReceptionListMaxRow, String)

        Else
            '表示情報が存在する

            '初期表示の件数と最大件数の差を計算
            Dim listCount As Integer = ReceptionListMaxRow - dtReceptionList.Count

            'リストを追加する行数を格納               
            Me.HiddenReceptionListCount.Value = CType(listCount, String)

            '敬称の取得(男)
            Dim man As String = WebWordUtility.GetWord(ApplicationID, WordId.Id017)

            '敬称の取得(女)
            Dim woman As String = WebWordUtility.GetWord(ApplicationID, WordId.Id018)

            '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
            'Pマークの取得
            Dim pMark As String = WebWordUtility.GetWord(ApplicationID, WordId.Id10001)
            'Lマークの取得
            Dim lMark As String = WebWordUtility.GetWord(ApplicationID, WordId.Id10002)
            '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

            'Repeaterにバインド
            RepeaterReceptionList.DataSource = dtReceptionList
            RepeaterReceptionList.DataBind()

            'ループカウンター
            Dim i As Integer = 0

            'DateRow
            Dim rowReceptionList As SC3100401DataSet.ReceptionListRow

            'コントロール制御
            For Each ReceptionItem As Control In RepeaterReceptionList.Items

                '来店データを取得
                rowReceptionList = dtReceptionList(i)

                'タグに値の追加「行番号」
                DirectCast(ReceptionItem.FindControl("ReceptionList"), HtmlContainerControl).Attributes("CLASS") = rowReceptionList.ROWNO

                'タグに値の追加「来店実績連番」
                DirectCast(ReceptionItem.FindControl("ReceptionList"), HtmlContainerControl).Attributes("VISITSEQ") = CType(rowReceptionList.VISITSEQ, String)

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                'タグに値の追加「予約ID」
                DirectCast(ReceptionItem.FindControl("ReceptionList"), HtmlContainerControl).Attributes("REZID") = CType(rowReceptionList.REZID, String)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                'タグに値の追加「呼出ステータス」
                DirectCast(ReceptionItem.FindControl("ReceptionList"), HtmlContainerControl).Attributes("CALLSTATUS") = rowReceptionList.CALLSTATUS

                'タグに値の追加「更新日時」
                DirectCast(ReceptionItem.FindControl("ReceptionList"), HtmlContainerControl).Attributes("UPDATEDATE") = CType(rowReceptionList.UPDATEDATE, String)

                'タグに値の追加「SACODE」
                DirectCast(ReceptionItem.FindControl("SAButton"), HtmlContainerControl).Attributes("SACODE") = rowReceptionList.SACODE

                'タグに値の追加「振当ステータス」
                DirectCast(ReceptionItem.FindControl("SAButton"), HtmlContainerControl).Attributes("ASSIGNSTATUS") = rowReceptionList.ASSIGNSTATUS

                '所有者名の確認
                If Not String.IsNullOrEmpty(rowReceptionList.NAME) Then
                    '所有車名が存在する

                    '所有者名を作成する(所有車名+スペース+敬称)
                    Dim nameBuilder As New StringBuilder

                    '所有者名
                    nameBuilder.Append(rowReceptionList.NAME)

                    'スペース
                    nameBuilder.Append(Space(1))

                    '性別確認
                    If Male.Equals(rowReceptionList.SEX) Then
                        '男性

                        '敬称(男性)
                        nameBuilder.Append(man)

                    Else
                        '女性

                        '敬称(女性)
                        nameBuilder.Append(woman)

                    End If

                    '所有車名の設定
                    DirectCast(ReceptionItem.FindControl("LeftBoxListLabel10"), CustomLabel).Text = nameBuilder.ToString()

                End If

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '二重エンコード問題修正
                If Not rowReceptionList.IsVCLREGNONull Then
                    '車両登録番号
                    DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox01"), TextBox).Text = rowReceptionList.VCLREGNO
                End If
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                '来店者名の確認
                If rowReceptionList.IsVISITNAMENull Then
                    '来店者名が存在しない

                    '所有者を設定
                    DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox02"), TextBox).Text = rowReceptionList.NAME

                Else
                    '来店者名が存在する

                    '来店者名を設定
                    DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox02"), TextBox).Text = rowReceptionList.VISITNAME

                End If

                '来店者電話番号の確認
                If rowReceptionList.IsVISITTELNONull Then
                    '来店者電話番号が存在しない

                    '所有者の電話番号を設定
                    DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox03"), TextBox).Text = rowReceptionList.TELNO

                Else
                    '来店者電話番号が存在する

                    '来店者の電話番号を設定
                    DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox03"), TextBox).Text = rowReceptionList.VISITTELNO

                End If

                'テーブルNoを設定
                DirectCast(ReceptionItem.FindControl("LeftBoxListTextBox04"), TextBox).Text = rowReceptionList.CALLPLACE

                '振当済か判定
                If AssignStatusFin.Equals(rowReceptionList.ASSIGNSTATUS) Then
                    '振当済の場合

                    '車両登録Noテキストボックス非活性
                    DirectCast(ReceptionItem.FindControl("CarTypeDisabled"), HtmlContainerControl).Visible = True

                End If

                '呼出中か判定
                If CallStatusCalling.Equals(rowReceptionList.CALLSTATUS) Then
                    '呼出中の場合

                    '呼出中イメージ表示
                    DirectCast(ReceptionItem.FindControl("CallImage"), HtmlContainerControl).Visible = True

                    '呼出中テキストの表示
                    DirectCast(ReceptionItem.FindControl("CallText"), HtmlContainerControl).Visible = True

                    '受付番号テキストボックス非活性
                    DirectCast(ReceptionItem.FindControl("NoDisabled"), HtmlContainerControl).Visible = True

                    '車両登録Noテキストボックス非活性
                    DirectCast(ReceptionItem.FindControl("CarTypeDisabled"), HtmlContainerControl).Visible = True

                    'テーブルNoテキストボックス非活性
                    DirectCast(ReceptionItem.FindControl("TableNoDisabled"), HtmlContainerControl).Visible = True

                    'SAボタン非活性
                    DirectCast(ReceptionItem.FindControl("SAButtonDisabled"), HtmlContainerControl).Visible = True

                End If

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                ''整備受注Noが発行済か判定
                'If Not rowReceptionList.IsORDERNONull Then
                '    '発行済

                '    '予約表示欄の文字を青に変更
                '    DirectCast(ReceptionItem.FindControl("ReserveArea"), HtmlContainerControl).Attributes("CLASS") = BlueStyle

                'End If

                'RO情報が存在するか判定
                If RepairOrderInfoExist.Equals(rowReceptionList.RO_INFO_TYPE) Then
                    '存在する場合

                    '予約表示欄の文字を青に変更
                    DirectCast(ReceptionItem.FindControl("ReserveArea"), HtmlContainerControl).Attributes("CLASS") = BlueStyle

                End If
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                If (PmarkFlg.Equals(rowReceptionList.IMP_VCL_FLG)) Then
                    ReceptionItem.FindControl("RightIcnP").Visible = True
                    ReceptionItem.FindControl("RightIcnL").Visible = False
                ElseIf (LmarkFlg.Equals(rowReceptionList.IMP_VCL_FLG)) Then
                    ReceptionItem.FindControl("RightIcnL").Visible = True
                    ReceptionItem.FindControl("RightIcnP").Visible = False
                Else
                    ReceptionItem.FindControl("RightIcnL").Visible = False
                    ReceptionItem.FindControl("RightIcnP").Visible = False
                End If

                'アイコンの文言設定
                'Pマーク文言
                CType(ReceptionItem.FindControl("RightIcnP"), HtmlContainerControl).InnerText = pMark
                'Lマーク文言
                CType(ReceptionItem.FindControl("RightIcnL"), HtmlContainerControl).InnerText = lMark
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                '次の行
                i = i + 1
            Next

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' SA一覧の設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub SetSAInfo(ByVal inSC3100401Biz As SC3100401BusinessLogic, _
                          ByVal inSC3100401DataSet As SC3100401DataSetTableAdapters.SC3100401TableAdapter, _
                          ByVal inStaffInfo As StaffContext, _
                          ByVal inPresentTime As Date)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA一覧表示用情報
        Dim dtServiceAdviserList As SC3100401DataSet.ServiceAdviserListDataTable = _
            inSC3100401Biz.GetServiceAdvisorInfo(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inPresentTime, inSC3100401DataSet)

        '表示件数確認
        If dtServiceAdviserList Is Nothing _
            OrElse dtServiceAdviserList.Count = 0 Then
            '表示件数なし

            '処理無し

        Else
            '表示情報が存在する

            'ループカウンター
            Dim i As Integer = 0

            'Repeaterにバインド
            RepeaterSAList.DataSource = dtServiceAdviserList
            RepeaterSAList.DataBind()

            'コントロール制御
            For Each ReceptionItem As Control In RepeaterSAList.Items

                'SA一覧データを取得
                'ROWに変換
                Dim rowServiceAdviserList As SC3100401DataSet.ServiceAdviserListRow = dtServiceAdviserList(i)

                'タグに値の追加「ACCOUNT」
                DirectCast(ReceptionItem.FindControl("SAAccount"), HtmlContainerControl).Attributes("ACCOUNT") = rowServiceAdviserList.ACCOUNT

                '次の行
                i = i + 1
            Next
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' クライアント側よりパラメータの取得
    ''' </summary>
    ''' <param name="inPostBackID"></param>
    ''' <returns>車両登録Noのバイト数</returns>
    ''' <remarks></remarks>
    Private Function GetEventArgs(ByVal inPostBackID As PostBackID) As PostBackArgumentClass

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} POSTBACKID:{2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , inPostBackID))

        '引数用クラス
        Dim postBackArg As New PostBackArgumentClass

        '変換処理結果格納変数
        Dim formatResult As Boolean = False

        '来店実績連番をLong型に変換
        formatResult = Long.TryParse(HiddenVisitSeq.Value.Trim, postBackArg.VisitSeq)

        '来店実績連番変換結果確認
        If Not formatResult Then
            '変換失敗

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} VISITSEQ IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '処理終了
            postBackArg.ReturnFlag = False

            Return postBackArg

        End If

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        '予約IDをDecimal型に変換
        formatResult = Decimal.TryParse(HiddenReserveId.Value.Trim, postBackArg.RezId)

        '予約ID変換結果確認
        If Not formatResult Then
            postBackArg.RezId = -1

        End If
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '更新日時をDate型に変換
        formatResult = Date.TryParse(HiddenUpDateDate.Value.Trim, postBackArg.UpDateDate)

        '更新日時変換結果確認
        If Not formatResult Then
            '変換失敗

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} UPDATETIME IS NULL OR NOTHING" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '処理終了
            postBackArg.ReturnFlag = False

            Return postBackArg

        End If

        'IDごと取得項目の変更
        Select Case inPostBackID
            Case PostBackID.RegisterRegNoButton '車両登録No登録ボタン

                '車両登録Noの取得
                postBackArg.RegNo = HiddenRegNo.Value.Trim

                '車両登録NOの取得確認
                If String.IsNullOrEmpty(postBackArg.RegNo) Then
                    '取得失敗

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} REGNO IS NULL OR NOTHING" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '処理終了
                    postBackArg.ReturnFlag = False

                    Return postBackArg
                End If

            Case PostBackID.SAAssignButton 'SA振当ボタン

                'イベントID
                postBackArg.EventKeyID = HiddenEventKeyID.Value.Trim

                'イベントIDの取得確認
                If String.IsNullOrEmpty(postBackArg.EventKeyID) Then
                    '変換失敗

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} CUSTOMFOOTERID IS NULL OR NOTHING" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '処理終了
                    postBackArg.ReturnFlag = False

                    Return postBackArg

                End If

                'SAアカウントの取得
                postBackArg.AfterAccount = HiddenSAAccount.Value.Trim

                'SAアカウントの取得確認
                If String.IsNullOrEmpty(postBackArg.AfterAccount) _
                    AndAlso Not String.Equals(postBackArg.EventKeyID, CType(EventKeyId.SAUndo, String)) Then
                    '取得失敗(UNDO処理は含まない)

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} SAACCOUNT IS NULL OR NOTHING" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '処理終了
                    postBackArg.ReturnFlag = False

                    Return postBackArg
                End If

            Case PostBackID.FooterButton 'サブフッターボタン

                'イベントID
                postBackArg.EventKeyID = HiddenEventKeyID.Value.Trim

                'イベントIDの取得確認
                If String.IsNullOrEmpty(postBackArg.EventKeyID) Then
                    '変換失敗

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} CUSTOMFOOTERID IS NULL OR NOTHING" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '処理終了
                    postBackArg.ReturnFlag = False

                    Return postBackArg

                End If

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
            Case PostBackID.RegisterVehicleButton '顧客車両登録ボタン

                '車両登録Noの取得
                postBackArg.RegNo = HiddenRegNo.Value.Trim

                If Not Decimal.TryParse(HiddenSelectCstId.Value.Trim, postBackArg.SelectCstId) _
                    OrElse Not Decimal.TryParse(HiddenSelectVclId.Value.Trim, postBackArg.SelectVclId) Then

                    postBackArg.SelectCstId = NEWCUSTOMER_ID
                    postBackArg.SelectVclId = NEWCUSTOMER_ID

                End If

                If Not Decimal.TryParse(HiddenSelectRezId.Value.Trim, postBackArg.SelectRezId) Then
                    postBackArg.SelectRezId = NEWCUSTOMER_ID
                End If


                '車両登録NOの取得確認
                If String.IsNullOrEmpty(postBackArg.RegNo) Then
                    '取得失敗

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} REGNO IS NULL OR NOTHING" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '処理終了
                    postBackArg.ReturnFlag = False

                    Return postBackArg
                End If
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        End Select

        '引数取得完了
        postBackArg.ReturnFlag = True

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} END " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Return postBackArg

    End Function

    ''' <summary>
    ''' 車両登録No登録処理
    ''' </summary>
    ''' <param name="inParameter">パラメーター</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub RegistRegNoButton(ByVal inParameter As PostBackArgumentClass)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        ''処理結果
        'Dim returnCode As Integer = ResultCode.Success
        '車両情報取得
        Dim dt As SC3100401DataSet.VehicleInfoDataTable
        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        'ユーザー情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在時間の取得
        Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        'Bizの宣言
        Using sc3100401Biz As New SC3100401BusinessLogic

            '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
            ''車両登録No登録処理
            'returnCode = sc3100401Biz.RegisterRegNo(inParameter.VisitSeq, _
            '                                        inParameter.UpDateDate, _
            '                                        inParameter.RegNo, _
            '                                        staffInfo, _
            '                                        presentTime)

            '車両情報を取得
            dt = sc3100401Biz.GetVehicleInfo(inParameter.RegNo, _
                                            staffInfo, _
                                            presentTime)
            '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        End Using

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

        ''処理結果確認
        'If returnCode <> ResultCode.Success Then
        '    '処理失敗

        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                 , "{0}.{1} END RETURNCODE = {2}" _
        '                 , Me.GetType.ToString _
        '                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                 , returnCode))

        '    'エラーメッセージの表示
        '    Me.ShowMessageBox(returnCode)

        'End If

        '取得結果確認
        If IsNothing(dt) Then

            '顧客ID、車両ID、予約IDを初期化
            inParameter.SelectCstId = NEWCUSTOMER_ID
            inParameter.SelectVclId = NEWCUSTOMER_ID
            inParameter.SelectRezId = NEWCUSTOMER_ID

            '新規顧客として追加
            Me.UpdateVisitVehicle(inParameter)

        Else
            Me.HiddenVehicleListDisplayType.Value = "1"
            Me.SetVehicleList(dt)

        End If

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' SA登録ボタン処理
    ''' </summary>
    ''' <param name="inParameter">パラメーター</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub RegisterSAButton(ByVal inParameter As PostBackArgumentClass)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        'ユーザー情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在時間の取得
        Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        'Bizの宣言
        Using sc3100401Biz As New SC3100401BusinessLogic

            'SA登録処理
            returnCode = sc3100401Biz.RegisterSA(inParameter.VisitSeq, _
                                                 inParameter.UpDateDate, _
                                                 inParameter.EventKeyID, _
                                                 inParameter.AfterAccount, _
                                                 staffInfo, _
                                                 presentTime)

        End Using

        '処理結果確認
        If returnCode <> ResultCode.Success Then
            '処理失敗

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} END RETURNCODE = {2}" _
                         , Me.GetType.ToString _
                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                         , returnCode))

            'エラーメッセージの表示
            Me.ShowMessageBox(returnCode)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' カスタムフッターボタン処理
    ''' </summary>
    ''' <param name="invisitSeq">来店実績連番</param>
    ''' <param name="inReserveId">予約ID</param>
    ''' <param name="inupDateTime">更新日時</param>
    ''' <param name="incustomFooterID">イベントキーID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Sub CustomFooterRegister(ByVal inVisitSeq As Long, _
                                     ByVal inReserveId As Decimal, _
                                     ByVal inUpDateTime As Date, _
                                     ByVal inCustomFooterID As String)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Sub CustomFooterRegister(ByVal inVisitSeq As Long, _
        '                                 ByVal inUpDateTime As Date, _
        '                                 ByVal inCustomFooterID As String)
        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} VISITSEQ:{2} REZID:{3} UPDATETIME:{4} CUSTOMFOOTERID:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inReserveId, inUpDateTime, inCustomFooterID))
        '処理結果
        Dim returnCode As Integer = ResultCode.Success

        'ユーザー情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在時間の取得
        Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        'Bizの宣言
        Using sc3100401Biz As New SC3100401BusinessLogic

            'フッターボタンごと処理分岐
            Select Case inCustomFooterID
                Case CType(EventKeyId.FooterCallButton, String),
                     CType(EventKeyId.FooterCancelButton, String)
                    '呼出・呼出キャンセル処理

                    '呼出・呼出キャンセル登録処理
                    returnCode = sc3100401Biz.RegisterCallStatus(inVisitSeq, _
                                                                 inUpDateTime, _
                                                                 inCustomFooterID, _
                                                                 staffInfo.Account, _
                                                                 presentTime)


                    '処理結果確認
                    If returnCode = ResultCode.Success Then
                        '処理成功

                        'PCへのPush処理
                        sc3100401Biz.SendPushServerPC(staffInfo.DlrCD, staffInfo.BrnCD, inCustomFooterID)

                    End If

                Case CType(EventKeyId.FooterDeleteButton, String)
                    'チップ削除処理

                    'チップ削除登録処理
                    returnCode = sc3100401Biz.RegisterTipDelete(inVisitSeq, _
                                                                inUpDateTime, _
                                                                staffInfo.Account, _
                                                                presentTime)

                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    '処理結果確認
                    If returnCode = ResultCode.Success Then
                        '処理成功

                        'SAメインメニューへのPush処理
                        sc3100401Biz.RefreshSA(staffInfo, _
                                               Nothing)

                        '受付モニターへのPush処理
                        sc3100401Biz.SendPushServerPC(staffInfo.DlrCD, _
                                                      staffInfo.BrnCD, _
                                                      inCustomFooterID)

                        '予約IDチェック
                        If Not (IsNothing(inReserveId)) AndAlso 0 < inReserveId Then
                            '存在する場合
                            '来店管理画面へのPush処理
                            sc3100401Biz.RefreshSvr(staffInfo)

                        End If

                    End If
                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                Case Else
                    '上記以外

                    '予期しないエラー
                    returnCode = ResultCode.ErrOutType

                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} CUSTOMFOOTERID IS OUT OF RANGE" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End Select

            '処理結果確認
            If returnCode <> ResultCode.Success Then
                '処理失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} END RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

                'エラーメッセージの表示
                Me.ShowMessageBox(returnCode)

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
    ''' <summary>
    ''' 車両ポップアップ出力処理
    ''' </summary>
    ''' <param name="dtVehicleInfo">顧客車両情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
    ''' </history>
    Private Sub SetVehicleList(ByVal dtVehicleInfo As SC3100401DataSet.VehicleInfoDataTable)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.VehicleListRepeater.DataSource = dtVehicleInfo
        Me.VehicleListRepeater.DataBind()

        For i = 0 To Me.VehicleListRepeater.Items.Count - 1
            '画面定義取得
            Dim vehicleListRepeater As Control = Me.VehicleListRepeater.Items(i)

            'ROW取得
            Dim drVehicleInfo As SC3100401DataSet.VehicleInfoRow = dtVehicleInfo(i)

            '顧客名
            If Not (drVehicleInfo.IsCST_NAMENull) Then
                CType(vehicleListRepeater.FindControl("CustomerName"), CustomLabel).Text = drVehicleInfo.CST_NAME
            End If

            '電話番号
            If Not (drVehicleInfo.IsTELNONull) Then
                CType(vehicleListRepeater.FindControl("TelNumber"), CustomLabel).Text = drVehicleInfo.TELNO
            End If

            'モデル名
            If Not (drVehicleInfo.IsMODELNull) Then
                CType(vehicleListRepeater.FindControl("ModelName"), CustomLabel).Text = drVehicleInfo.MODEL
            End If

            'VIN
            If Not (drVehicleInfo.IsVINNull) Then
                CType(vehicleListRepeater.FindControl("VclVin"), CustomLabel).Text = drVehicleInfo.VIN
            End If

            '整備名称
            If Not (drVehicleInfo.IsMERCHANDISENAMENull) Then
                CType(vehicleListRepeater.FindControl("MerchandiseName"), CustomLabel).Text = drVehicleInfo.MERCHANDISENAME
            End If

            '開始日時 - 終了日時
            If Not (drVehicleInfo.IsPLANSTARTDATENull) AndAlso Not (drVehicleInfo.IsPLANENDDATENull) Then

                If String.Equals(DateTimeFunc.FormatDate(11, drVehicleInfo.PLANSTARTDATE), _
                                 DateTimeFunc.FormatDate(11, drVehicleInfo.PLANENDDATE)) Then
                    '日跨ぎ出ない場合は「MM/DD HH:MI - HH:MI」で表示する
                    DateTimeFunc.FormatDate(11, drVehicleInfo.PLANSTARTDATE)
                    DateTimeFunc.FormatDate(14, drVehicleInfo.PLANENDDATE)
                    CType(vehicleListRepeater.FindControl("PlanStartEndDate"), CustomLabel).Text = _
                        String.Concat(DateTimeFunc.FormatDate(11, drVehicleInfo.PLANSTARTDATE), _
                                      Space(1), _
                                      DateTimeFunc.FormatDate(14, drVehicleInfo.PLANSTARTDATE), _
                                      Space(1), _
                                      WebWordUtility.GetWord(ApplicationID, WordId.Id044), _
                                      Space(1), _
                                      DateTimeFunc.FormatDate(14, drVehicleInfo.PLANENDDATE))
                    'WebWordUtility.GetWord(ApplicationID, WordId.id020), _
                Else
                    '日跨ぎの場合は「MM/DD HH:MI - MM/DD HH:MI」で表示する
                    Dim reserveFromStart As String
                    Dim reserveFromEnd As String
                    Dim fromMD As String
                    Dim fromHM As String

                    fromMD = DateTimeFunc.FormatDate(11, drVehicleInfo.PLANSTARTDATE)      'MM/dd
                    fromHM = DateTimeFunc.FormatDate(14, drVehicleInfo.PLANSTARTDATE)      'hh:mm
                    reserveFromStart = fromMD & Space(1) & fromHM                         'MM/dd hh:mm

                    fromMD = DateTimeFunc.FormatDate(11, drVehicleInfo.PLANENDDATE)      'MM/dd
                    fromHM = DateTimeFunc.FormatDate(14, drVehicleInfo.PLANENDDATE)      'hh:mm
                    reserveFromEnd = fromMD & Space(1) & fromHM                         'MM/dd hh:mm

                    '開始終了日時を文字列結合
                    Dim strStartEndDate As String = String.Concat(reserveFromStart, _
                                      Space(1), _
                                      WebWordUtility.GetWord(ApplicationID, WordId.Id044), _
                                      Space(1), _
                                      reserveFromEnd)

                    CType(vehicleListRepeater.FindControl("PlanStartEndDate"), CustomLabel).Text = strStartEndDate

                End If
            End If


            '顧客ID
            If Not (drVehicleInfo.IsCST_IDNull) Then
                CType(vehicleListRepeater.FindControl("VehicleListItem"), HtmlContainerControl).Attributes("CSTID") = CStr(drVehicleInfo.CST_ID)
            End If

            '車両ID
            If Not (drVehicleInfo.IsVCL_IDNull) Then
                CType(vehicleListRepeater.FindControl("VehicleListItem"), HtmlContainerControl).Attributes("VCLID") = CStr(drVehicleInfo.VCL_ID)
            End If

            '予約ID
            If Not (drVehicleInfo.IsREZIDNull) Then
                CType(vehicleListRepeater.FindControl("VehicleListItem"), HtmlContainerControl).Attributes("REZID") = CStr(drVehicleInfo.REZID)
            End If

        Next

        '顧客車両ポップアップ一覧フッターボタン文言
        Me.PopUpVehicleListFooterButton.Text = WebWordUtility.GetWord(ApplicationID, WordId.Id045)

        '顧客車両ポップアップ一覧エリア更新
        Me.ContentUpdatePopupPanel.Update()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 来店車両更新処理
    ''' </summary>
    ''' <param name="inParameter">パラメーター</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
    ''' </history>
    Private Sub UpdateVisitVehicle(ByVal inParameter As PostBackArgumentClass)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果
        Dim returnCode As Integer = ResultCode.Success


        'ユーザー情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '現在時間の取得
        Dim presentTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        'Bizの宣言
        Using sc3100401Biz As New SC3100401BusinessLogic

            '来店車両情報更新処理
            returnCode = sc3100401Biz.UpdateVisitVehicle(inParameter.VisitSeq, _
                                                         inParameter.UpDateDate, _
                                                         inParameter.RegNo, _
                                                         staffInfo, _
                                                         presentTime, _
                                                         inParameter.SelectCstId, _
                                                         inParameter.SelectVclId, _
                                                         inParameter.SelectRezId)


            Me.HiddenVehicleListDisplayType.Value = "0"

            '処理結果確認
            If returnCode <> ResultCode.Success Then
                '処理失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                             , "{0}.{1} END RETURNCODE = {2}" _
                             , Me.GetType.ToString _
                             , System.Reflection.MethodBase.GetCurrentMethod.Name _
                             , returnCode))

                'エラーメッセージの表示
                Me.ShowMessageBox(returnCode)

            ElseIf inParameter.SelectRezId > 0 Then

                'WBへPush送信
                sc3100401Biz.SendPushForRefreshWelcomeBoard(staffInfo)

            End If
        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

#End Region

#Region "フッターボタンの制御"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '表示非表示に関わらず、使用するサブメニューボタンを宣言
        Return New Integer() {}

    End Function

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </History>
    Private Sub InitFooterEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'フッターボタンのオブジェクト作成

        'メインメニューボタンのイベント設定
        Dim mainMenuButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)

        'JavaScript関数「メインメニュー」の設定
        mainMenuButton.OnClientClick = "return FooterButtonControlMainMenu();"

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ''スケジュールボタンのイベント設定
        'Dim scheduleButton As CommonMasterFooterButton = _
        '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Schedule)

        ''JavaScript関数「スケジュール」の設定
        'scheduleButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        '予約管理ボタンのイベント設定
        Dim reserveButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)
        'JavaScript関数「予約管理ボタン」の設定
        reserveButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.ReserveManagement & ");"

        'RO一覧ボタンのイベント設定
        Dim repairOrderButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)
        'JavaScript関数「RO一覧ボタン」の設定
        repairOrderButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.RepairOrderList & ");"

        '全体管理ボタンのイベント設定
        Dim wholeManagementButton As CommonMasterFooterButton = _
        DirectCast(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.WholeManagement)
        'JavaScript関数「全体管理ボタン」の設定
        wholeManagementButton.OnClientClick = "return FooterButtonClick(" & FooterMenuCategory.WholeManagement & ");"

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '電話帳ボタンのイベント設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)

        'JavaScript関数「電話帳」の設定
        telDirectoryButton.OnClientClick = "return schedule.appExecute.executeCont();"

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub


    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 予約管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub ReserveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReserveManagementButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '来店管理画面に遷移する
        Me.RedirectNextScreen(WellComeBoardID)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' RO一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub RepairOrderButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepairOrderListButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '他システム連携画面に遷移する

        'スタッフ情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        Using biz As New ServiceCommonClassBusinessLogic

            Try
                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                    biz.GetIcropToDmsCode(staffInfo.DlrCD, _
                                          ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                          staffInfo.DlrCD, _
                                          staffInfo.BrnCD, _
                                          String.Empty, _
                                          staffInfo.Account)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SessionKeyDisplayNumber, SessionDataRepairOrder)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SessionKeyParam1, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SessionKeyParam2, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SessionKeyParam3, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SessionKeyParam4, String.Empty)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SessionKeyParam5, String.Empty)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SessionKeyParam6, String.Empty)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SessionKeyParam7, String.Empty)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SessionKeyParam8, String.Empty)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SessionKeyParam9, SESSION_DATA_VIEWMODE_EDIT)

                    '他システム連携画面に遷移する
                    Me.RedirectNextScreen(OtherLinkageID)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '予期せぬエラーのメッセージ表示
                    Me.ShowMessageBox(ResultCode.ErrOutType)

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウト処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} DB TIMEOUT:{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , ex.Message))

                'DBタイムアウトのメッセージ表示
                Me.ShowMessageBox(ResultCode.ErrDBTimeout)

            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 全体管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub WholeManagementButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles WholeManagementButton.Click
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '全体管理画面に遷移する
        Me.RedirectNextScreen(WholeManagementID)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

End Class
