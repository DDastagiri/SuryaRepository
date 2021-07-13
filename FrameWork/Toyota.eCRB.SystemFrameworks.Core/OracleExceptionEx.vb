Imports Oracle.DataAccess.Client
Imports System.Data.Common
Imports System.Runtime.Serialization

Namespace Toyota.eCRB.SystemFrameworks.Core

    <Serializable()> _
    Public Class OracleExceptionEx
        Inherits DbException

#Region "変数"
        ''' <summary>
        ''' SQLテキスト
        ''' </summary>
        ''' <remarks></remarks>
        Private _commandText As String = String.Empty
        ''' <summary>
        ''' パラメーター
        ''' </summary>
        ''' <remarks></remarks>
        Private _parameters As OracleParameterCollection = Nothing
        ''' <summary>
        ''' エラーコード
        ''' </summary>
        ''' <remarks></remarks>
        Private _number As Integer = 0
#End Region

#Region "プロパティ"
        ''' <summary>
        ''' SQLを返却します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CommandText As String
            Get
                Return _commandText
            End Get
        End Property

        ''' <summary>
        ''' SQLパラメーターを返却します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parameters As OracleParameterCollection
            Get
                Return _parameters
            End Get
        End Property

        ''' <summary>
        ''' Oracleエラーコードを返却します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Number As Integer
            Get
                Return _number
            End Get
        End Property
#End Region

#Region "New"

        ''' <summary>
        ''' 使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' 使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' 使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal message As String, ByVal ex As Exception)
            MyBase.New(message, ex)
        End Sub

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="ex">OracleException</param>
        ''' <param name="command">OracleCommand</param>
        ''' <remarks>OracleExceptionを拡張します。</remarks>
        Public Sub New(ByVal ex As OracleException, ByVal command As OracleCommand)

            MyBase.New(ex.Message, ex)

            _commandText = command.CommandText
            _parameters = command.Parameters
            _number = ex.Number

        End Sub

        ''' <summary>
        ''' 使用不可
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.new(info, context)
        End Sub

#End Region

        Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, _
                                      ByVal context As System.Runtime.Serialization.StreamingContext)

            MyBase.GetObjectData(info, context)

        End Sub

    End Class

End Namespace