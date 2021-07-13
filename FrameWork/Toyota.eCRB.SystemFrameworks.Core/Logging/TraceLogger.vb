Imports System
Imports System.Linq
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Threading


Namespace Toyota.eCRB.SystemFrameworks.Core

    Public Class TraceLogger
        Inherits TraceSource

        Friend Class LogRuleItem
            Private Shared separator As Char = GetAppConfigValue(Of Char)("LogRuleItem.Separator", "#"c)

            Public Sub New(ByVal ele As LogRuleConfigElement)
                Key = ele.Key.Split(separator)(0)
                Value = ele.Value
                LevelValue = ele.Level
            End Sub

            Public Property Key As String
            Public Property Value As String
            Public Property LevelValue As String

            Public Function CheckItem(ByVal table As Dictionary(Of String, String)) As Boolean
                If String.Equals(Key, "EventId", StringComparison.InvariantCultureIgnoreCase) Then
                    Return table.Any(Function(x) x.Key = Me.Key AndAlso x.Value = Me.Value)
                Else
                    Return table.Any(Function(x) x.Key = Me.Key AndAlso x.Value.StartsWith(Me.Value))
                End If
            End Function
        End Class

        Friend Const ApplicationNameAddTag As String = "ApplicationName"
        Friend Const ClientIPAddressAddTag As String = "ClientIPAddress"
        Friend Const ClientPCNameAddTag As String = "ClientPCName"
        Friend Const ServiceNameAddTag As String = "ServiceName"
        Private _logRules As List(Of LogRuleItem)

        Friend ReadOnly Property LogRules As List(Of LogRuleItem)
            Get
                Return Me._logRules
            End Get
        End Property

        Public Class IgnoreAsCallerAttribute
            Inherits Attribute
        End Class

        Private Property ConfigSwitch As SourceSwitch


        Private Shared Function GetAppConfigValue(ByVal key As String) As String
            Return ConfigurationManager.AppSettings.[Get](key)
        End Function

        Private Shared Function GetAppConfigValue(ByVal key As String, ByVal defValue As String) As String
            Dim val As String = ConfigurationManager.AppSettings.[Get](key)
            Return If(val, defValue)
        End Function

        Private Shared Function GetAppConfigValue(Of T)(ByVal key As String, ByVal defValue As T) As T
            Dim val As String = ConfigurationManager.AppSettings.[Get](key)
            If String.IsNullOrEmpty(val) Then Return defValue
            Return CType(ComponentModel.TypeDescriptor.GetConverter(GetType(T)).ConvertFrom(val), T)
        End Function

        Public Sub New()
            Me.New(GetAppConfigValue("TraceLogger.DefaultSourceName", "ApplicationTrace"), SourceLevels.[Error])
            Dim logRulesSection As TraceLoggerLogRuleSection = TryCast(ConfigurationManager.GetSection("TraceLogger"), TraceLoggerLogRuleSection)

            If logRulesSection IsNot Nothing Then

                If logRulesSection.Rules IsNot Nothing Then
                    Me._logRules.AddRange(CType(logRulesSection.Rules, IEnumerable(Of TraceLogger.LogRuleItem)))
                End If

                Me.ConfigSwitch = New SourceSwitch(Me.Name, SourceLevels.[Error].ToString())
                Me.Switch = New SourceSwitch(Nothing, SourceLevels.All.ToString())
            End If
        End Sub

        Public Sub New(ByVal name As String)
            Me.New(name, SourceLevels.[Error])
        End Sub

        Public Sub New(ByVal name As String, ByVal defaultLevel As SourceLevels)
            MyBase.New(name, defaultLevel)
            SetupMaxObjectInfoLine(name)
            SetupMaxObjectInfoLength(name)
        End Sub

        Private Sub SetupMaxObjectInfoLine(ByVal name As String)
            MaxObjectInfoLine = 10
            Dim val As String = ConfigurationManager.AppSettings.[Get](name & ".MaxObjectInfoLine")

            If val IsNot Nothing Then
                MaxObjectInfoLine = Integer.Parse(val)
            Else
                val = ConfigurationManager.AppSettings.[Get]("TraceLogger.MaxObjectInfoLine")

                If val IsNot Nothing Then
                    MaxObjectInfoLine = Integer.Parse(val)
                End If
            End If
        End Sub

        Private Sub SetupMaxObjectInfoLength(ByVal name As String)
            MaxObjectInfoLength = 2000
            Dim val As String = ConfigurationManager.AppSettings.[Get](name & ".MaxObjectInfoLength")

            If val IsNot Nothing Then
                MaxObjectInfoLength = Integer.Parse(val)
            Else
                val = ConfigurationManager.AppSettings.[Get]("TraceLogger.MaxObjectInfoLength")

                If val IsNot Nothing Then
                    MaxObjectInfoLength = Integer.Parse(val)
                End If
            End If
        End Sub

        Public Shared ReadOnly Property AdditionalInfo As Dictionary(Of String, String)
            Get
                If _additionalInfo Is Nothing Then _additionalInfo = New Dictionary(Of String, String)()
                Return _additionalInfo
            End Get
        End Property

        <ThreadStatic()>
        Private Shared _additionalInfo As Dictionary(Of String, String)

        Public Property MaxObjectInfoLine As Integer
            Get
                Return _maxObjectInfoLine
            End Get
            Set(ByVal value As Integer)
                _maxObjectInfoLine = value
            End Set
        End Property

        Private _maxObjectInfoLine As Integer

        Public Property MaxObjectInfoLength As Integer
            Get
                Return _maxObjectInfoLength
            End Get
            Set(ByVal value As Integer)
                _maxObjectInfoLength = value
            End Set
        End Property

        Private _maxObjectInfoLength As Integer

        Public Function CanLog(ByVal level As TraceEventType, Optional ByVal eventId As TraceCategory = TraceCategory.None) As Boolean
            Return CanLog(level, CInt(eventId))
        End Function

        Public Function CanLog(ByVal level As TraceEventType, ByVal id As Integer) As Boolean
            If LogRules IsNot Nothing Then

                If LogRules.Count > 0 Then
                    Dim headerTable = BuildHeaders(id)
                    Dim levelCondition = From p In LogRules Where p.CheckItem(headerTable) Select p.LevelValue
                    Dim values As List(Of String) = levelCondition.ToList()

                    If values.Contains("All") Then
                        Return True
                    ElseIf values.Contains("Information") Then
                        Return level <> TraceEventType.Verbose
                    ElseIf values.Contains("Warning") Then
                        Return ((level = TraceEventType.Critical) OrElse (level = TraceEventType.[Error]) OrElse (level = TraceEventType.Warning))
                    ElseIf values.Contains("Error") Then
                        Return ((level = TraceEventType.Critical) OrElse (level = TraceEventType.[Error]))
                    End If
                End If
            End If

            If ConfigSwitch Is Nothing Then
                If Switch.Level = SourceLevels.All Then Return True
                Return (CUInt(Switch.Level) And CUInt(level)) <> 0
            Else
                If ConfigSwitch.Level = SourceLevels.All Then Return True
                Return (CUInt(ConfigSwitch.Level) And CUInt(level)) <> 0
            End If
        End Function

        Private Function BuildHeaders(ByVal eventId As Integer) As Dictionary(Of String, String)
            Dim logHeader As Dictionary(Of String, String) = New Dictionary(Of String, String)()

            For Each item In TraceLogger.AdditionalInfo
                logHeader.Add(item.Key, item.Value)
            Next

            If Thread.CurrentPrincipal.Identity.IsAuthenticated Then
                logHeader.Add("ThreadUserName", Thread.CurrentPrincipal.Identity.Name)
            Else
                logHeader.Add("ThreadUserName", "")
            End If

            logHeader.Add("MachineName", System.Environment.MachineName)
            logHeader.Add("ThreadId", Thread.CurrentThread.ManagedThreadId.ToString())
            logHeader.Add("EventId", eventId.ToString())
            Return logHeader
        End Function

        Public Overloads Sub TraceEvent(ByVal eventType As TraceEventType, ByVal id As TraceCategory, ByVal message As String)
            If Not CanLog(eventType, id) Then Return
            MyBase.TraceEvent(eventType, CInt(id), message)
        End Sub

        Public Overloads Sub TraceEvent(ByVal eventType As TraceEventType, ByVal id As TraceCategory, ByVal format As String, ParamArray args As Object())
            If Not CanLog(eventType, id) Then Return
            MyBase.TraceEvent(eventType, CInt(id), format, args)
        End Sub

        Public Overloads Sub TraceEvent(ByVal eventType As TraceEventType, ByVal id As Integer, ByVal format As String, ParamArray args As Object())
            If Not CanLog(eventType, id) Then Return
            MyBase.TraceEvent(eventType, CInt(id), format, args)
        End Sub

        Public Overloads Sub TraceEvent(ByVal eventType As TraceEventType, ByVal id As Integer, ByVal message As String)
            If Not CanLog(eventType, id) Then Return
            MyBase.TraceEvent(eventType, CInt(id), message)
        End Sub

        Public Overloads Sub TraceVerbose(ByVal message As String, ByVal info As Object)
            If Not CanLog(TraceEventType.Verbose, TraceCategory.AppDebug) Then Return

            If info IsNot Nothing Then

                Dim od As ObjectDumper = New ObjectDumper(Sub(line As String)
                                                              line = message & line
                                                              MyBase.TraceEvent(TraceEventType.Verbose, CInt(TraceCategory.AppDebug), line)
                                                          End Sub, MaxObjectInfoLine, MaxObjectInfoLength)
                od.OutputObjectString(info)
            Else
                MyBase.TraceEvent(TraceEventType.Verbose, CInt(TraceCategory.AppDebug), message)
            End If

            Me.Flush()
        End Sub

        Public Overloads Sub TraceInformation(ByVal message As String)
            If Not CanLog(TraceEventType.Information, TraceCategory.AppInformation) Then Return
            MyBase.TraceEvent(TraceEventType.Information, CInt(TraceCategory.AppInformation), message)
        End Sub

        Public Overloads Sub TraceInformation(ByVal format As String, ParamArray args As Object())
            If Not CanLog(TraceEventType.Information, TraceCategory.AppInformation) Then Return
            MyBase.TraceEvent(TraceEventType.Information, CInt(TraceCategory.AppInformation), format, args)
        End Sub
    End Class
End Namespace
