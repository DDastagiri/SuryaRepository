'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080227.aspx.vb
'─────────────────────────────────────
'機能： 顧客イメージ画像取得のコードビハインド
'補足： 
'作成： 2017/01/05 NSK 荒川
'更新： 2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更
'──────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.Common.Api.BizLogic
Imports Toyota.eCRB.iCROP.BizLogic.SC3080227
Imports Toyota.eCRB.iCROP.DataAccess.SC3080227.SC3080227DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class Pages_SC3080227
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 顧客イメージ画像が見つからなかった際のシルエット画像パス(S)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoImagePhotoPathSmall As String = "~/Styles/Images/SC3080227/no_photo_S.png"

    ''' <summary>
    ''' 顧客イメージ画像が見つからなかった際のシルエット画像パス(M)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoImagePhotoPathMedium As String = "~/Styles/Images/SC3080227/no_photo_M.png"

    ''' <summary>
    ''' 顧客イメージ画像が見つからなかった際のシルエット画像パス(L)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoImagePhotoPathLarge As String = "~/Styles/Images/SC3080227/no_photo_L.png"
    
    '''<summary>
    ''' jpg形式の画像ファイルの拡張子
    ''' </summary>
    Private Const ExtensionJpg As String = "jpg"

#End Region

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dealerCode As String = Request("DealerCode") '基幹販売店コード
        Dim customerID As String = Request("CustomerID") '基幹顧客コード(表示用)
        Dim size As String = Request("Size") '画像サイズ

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} Start dealerCode:[{1}] customerID:[{2}] size:[{3}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  dealerCode, customerID, size))

        '画像サイズがS, M, Lのいずれでもない場合、サイズをデフォルトのSにする
        If Not String.Equals(SC3080227TableAdapter.ImageFileSmall, size) AndAlso _
            Not String.Equals(SC3080227TableAdapter.ImageFileMedium, size) AndAlso _
            Not String.Equals(SC3080227TableAdapter.ImageFileLarge, size) Then
            size = SC3080227TableAdapter.ImageFileSmall
        End If

        Dim bizLogic As New SC3080227BusinessLogic
        Dim custImageFilePath As String = Nothing

        Try
            '顧客イメージ画像パス取得
            custImageFilePath = bizLogic.GetCustImageFilePath(dealerCode, customerID, size)
        Catch ex As Exception
            '例外発生時は、エラーログを出力して処理を続行する
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                      , "{0} Error:{1}" _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , ex.Message), ex)
        End Try

        If String.IsNullOrEmpty(custImageFilePath) OrElse Not File.Exists(Server.MapPath(custImageFilePath)) Then
            'ファイルのパスが取得できなかった、またはファイルが存在しない場合
            If String.Equals(SC3080227TableAdapter.ImageFileSmall, size) Then
                '画像サイズがSの場合
                custImageFilePath = NoImagePhotoPathSmall
            ElseIf String.Equals(SC3080227TableAdapter.ImageFileMedium, size) Then
                '画像サイズがMの場合
                custImageFilePath = NoImagePhotoPathMedium
            ElseIf String.Equals(SC3080227TableAdapter.ImageFileLarge, size) Then
                '画像サイズがLの場合
                custImageFilePath = NoImagePhotoPathLarge
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0} End : Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, custImageFilePath))
        
        '2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 START
        'Response.ContentType = "image/png"

        'ファイル名から拡張子を取得する
        Dim extension As String = Path.GetExtension(custImageFilePath)
        extension = extension.Remove(0,1)
        extension = extension.ToLower
        'ContentTypeの判定
        If extension.Equals(ExtensionJpg) Then
            Response.ContentType = "image/jpeg"
        Else
            Response.ContentType = "image/" + extension
        End If
        '2019/05/22 NSK 坂本 (トライ店システム評価)画像アップロード機能における、応答性向上の為の仕様変更 END
        Response.Flush()
        Response.WriteFile(Server.MapPath(custImageFilePath))
        Response.End()
    End Sub


End Class
