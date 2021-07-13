Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3040601.BizLogic

    ''' <summary>
    ''' グローバル変数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GlobalValue

        '実行パス
        Private _strRootPath As String

        'root path　"/e-CRBInfoSync/DAV/CardDAV/IC3040601.aspx/"
        Private _CARDDAV_ROOT_URL As String

        ''' <summary>
        ''' Setter Getter
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StrRootPath As String
            Get
                Return _strRootPath
            End Get
            Set(ByVal value As String)
                _strRootPath = value
            End Set
        End Property

        Property CardDavRootUrl As String
            Get
                Return _CARDDAV_ROOT_URL
            End Get
            Set(ByVal value As String)
                _CARDDAV_ROOT_URL = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            _strRootPath = ""

            Try
            	Dim SystemEnv As New Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.SystemEnvSetting
	            _CARDDAV_ROOT_URL = SystemEnv.GetSystemEnvSetting("CARDDAV_ROOT_URL").Item("PARAMVALUE")
            Catch ex As ApplicationException
                Logger.Error("@@@ ERROR CARDDAV_ROOT_URLが取得できません")
                Logger.Error("TBL_SYSTEMENVSETTING の PARAMNAME に CARDDAV_ROOT_URLがありますか？")
            End Try

        End Sub


    End Class

End Namespace