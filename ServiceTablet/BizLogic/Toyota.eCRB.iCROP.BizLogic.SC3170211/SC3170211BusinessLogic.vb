'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3170211BusinessLogic.vb
'─────────────────────────────────────
'機能： 商品紹介機能開発(RO)
'補足： 写真表示ポップアップ
'作成： 2014/02/18 SKFC 久代
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports Toyota.eCRB.iCROP.DataAccess.SC3170211
Imports Toyota.eCRB.SystemFrameworks.Core

Public Class SC3170211BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 設定セクション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_SETTING_SECTION As String = "ImagePopup"

    ''' <summary>
    ''' 設定キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const _PROGRAM_SETTING_KEY_IMGURL As String = "OpenDirectoryURL"
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' 画像へのパス生成
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetImagePath() As String
        Logger.Info("SC3170211BusinessLogic.GetImagePath Function Start.")

        '公開フォルダのURLはProgramSettingより取得
        Dim imageUrlPath As String = SC3170211TableAdapter.GetProgramSetting(_PROGRAM_SETTING_SECTION,
                                                                             _PROGRAM_SETTING_KEY_IMGURL)

        Logger.Info("SC3170211BusinessLogic.GetImagePath Function End.")
        Return imageUrlPath
    End Function
#End Region

End Class
