Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Security.Principal
Imports System.Text
Imports System.Threading

Namespace Toyota.eCRB.SystemFrameworks.Core
    Public Class TraceLogListener
        Inherits System.Diagnostics.TraceListener

        Private lockObject As Object = New Object()
        Private _currentDate As DateTime
        Private _traceWriter As StreamWriter

        Public Property BaseFilename As String
            Get

                If _baseFilename Is Nothing Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("BaseFilename") Then
                            _baseFilename = Me.Attributes("BaseFilename")
                        Else
                            _baseFilename = "TraceLog"
                        End If
                    End SyncLock
                End If

                Return _baseFilename
            End Get
            Set(ByVal value As String)

                SyncLock lockObject
                    _baseFilename = value
                End SyncLock
            End Set
        End Property

        Private _baseFilename As String

        Public Property Delimiter As String
            Get

                If _delimiter Is Nothing Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("Delimiter") Then
                            _delimiter = Me.Attributes("Delimiter")
                        Else
                            _delimiter = ","
                        End If
                    End SyncLock
                End If

                Return _delimiter
            End Get
            Set(ByVal value As String)

                SyncLock lockObject
                    _delimiter = value
                End SyncLock
            End Set
        End Property

        Private _delimiter As String

        Public Property Encoding As Encoding
            Get

                If _encoding Is Nothing Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("Encoding") Then
                            _encoding = Encoding.GetEncoding(Me.Attributes("Encoding"))
                        Else
                            _encoding = Encoding.UTF8
                        End If
                    End SyncLock
                End If

                Return _encoding
            End Get
            Set(ByVal value As Encoding)

                SyncLock lockObject
                    _encoding = value
                End SyncLock
            End Set
        End Property

        Private _encoding As Encoding

        Public Property OutputHeaders As String()
            Get

                If _outputHeaders Is Nothing Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("OutputHeaders") Then
                            _outputHeaders = Me.Attributes("OutputHeaders").Split(","c)
                        Else
                            _outputHeaders = New String() {}
                        End If
                    End SyncLock
                End If

                Return _outputHeaders
            End Get
            Set(ByVal value As String())

                SyncLock lockObject
                    _outputHeaders = value
                End SyncLock
            End Set
        End Property

        Private _outputHeaders As String()

        Public Property LogDateTimeFormat As String
            Get

                If _logDateTimeFormat Is Nothing Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("LogDateTimeFormat") Then
                            _logDateTimeFormat = Me.Attributes("LogDateTimeFormat")
                        Else
                            _logDateTimeFormat = "d"
                        End If
                    End SyncLock
                End If

                Return _logDateTimeFormat
            End Get
            Set(ByVal value As String)

                SyncLock lockObject
                    _logDateTimeFormat = value
                End SyncLock
            End Set
        End Property

        Private _logDateTimeFormat As String

        Public Property MaxFileSize As Long
            Get

                If _maxFileSize = Long.MinValue Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("MaxFileSize") Then
                            _maxFileSize = Long.Parse(Me.Attributes("MaxFileSize"))
                        Else
                            _maxFileSize = 0
                        End If
                    End SyncLock
                End If

                Return _maxFileSize
            End Get
            Set(ByVal value As Long)

                SyncLock lockObject
                    _maxFileSize = value
                End SyncLock
            End Set
        End Property

        Private _maxFileSize As Long = Long.MinValue

        Public Property SingleProcess As Boolean
            Get

                If Not _singleProcess.HasValue Then

                    SyncLock lockObject

                        If Me.Attributes.ContainsKey("SingleProcess") Then
                            _singleProcess = Boolean.Parse(Me.Attributes("SingleProcess"))
                        Else
                            _singleProcess = True
                        End If
                    End SyncLock
                End If

                Return _singleProcess.Value
            End Get
            Set(ByVal value As Boolean)
                _singleProcess = value
            End Set
        End Property

        Private _singleProcess As Boolean?

        Public ReadOnly Property CurrentFilename As String
            Get
                CheckRollover()
                Return _currentFilename
            End Get
        End Property

        Private _currentFilename As String

        Public Sub New()
        End Sub

        Public Overrides Sub Write(ByVal message As String)
            CheckRollover()
            If _traceWriter IsNot Nothing Then _traceWriter.Write(message)
        End Sub

        Public Overrides Sub WriteLine(ByVal message As String)
            CheckRollover()
            If _traceWriter IsNot Nothing Then _traceWriter.WriteLine(message)
        End Sub

        Protected Overridable Function GenerateFilename() As String
            _currentDate = System.DateTime.Now.Date

            If MaxFileSize = 0 Then

                If SingleProcess Then
                    Return Path.Combine(Path.GetDirectoryName(BaseFilename), Path.GetFileNameWithoutExtension(BaseFilename) & "_" & _currentDate.ToString("yyyyMMdd") & Path.GetExtension(BaseFilename))
                Else
                    Return Path.Combine(Path.GetDirectoryName(BaseFilename), Path.GetFileNameWithoutExtension(BaseFilename) & "_" & _currentDate.ToString("yyyyMMdd") & "_" & Process.GetCurrentProcess().Id.ToString() & Path.GetExtension(BaseFilename))
                End If
            Else

                If SingleProcess Then
                    Return Path.Combine(Path.GetDirectoryName(BaseFilename), Path.GetFileNameWithoutExtension(BaseFilename) & "_" & _currentDate.ToString("yyyyMMdd") & "_" & DateTime.Now.ToString("HHmmss") & Path.GetExtension(BaseFilename))
                Else
                    Return Path.Combine(Path.GetDirectoryName(BaseFilename), Path.GetFileNameWithoutExtension(BaseFilename) & "_" & _currentDate.ToString("yyyyMMdd") & "_" & DateTime.Now.ToString("HHmmss") & "_" & Process.GetCurrentProcess().Id.ToString() & Path.GetExtension(BaseFilename))
                End If
            End If
        End Function

        Protected Overridable Function GenerateExtendedFileName(ByVal listener As TraceLogListener, ByVal inc As Integer) As String
            inc += 1
            Dim fileName = listener.GenerateFilename()
            If inc = 1 Then Return fileName
            Dim fileNameWithoutExtension = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName))
            Return String.Format("{0}_{1:0000}{2}", fileNameWithoutExtension, inc, Path.GetExtension(fileName))
        End Function

        Protected Overridable Sub CheckRollover()
            If _traceWriter Is Nothing OrElse (_currentDate.CompareTo(System.DateTime.Now.Date) <> 0) OrElse (Me.MaxFileSize > 0 AndAlso _traceWriter.BaseStream.Length > Me.MaxFileSize) Then
                Me.TryPrepareLog()
            End If
        End Sub

        Protected Sub TryPrepareLog(Optional ByVal retryCount As Integer = 10)
            If _traceWriter IsNot Nothing Then
                _traceWriter.Close()
                _traceWriter = Nothing
            End If

            _currentFilename = Nothing
            Dim inc As Integer = 0

            While True

                Try

                    If SingleProcess Then
                        Me._currentFilename = GenerateExtendedFileName(Me, Math.Min(System.Threading.Interlocked.Increment(inc), inc - 1))
                        Me._traceWriter = New StreamWriter(_currentFilename, True, Encoding)
                    Else

                        Do
                            Me._currentFilename = GenerateExtendedFileName(Me, Math.Min(System.Threading.Interlocked.Increment(inc), inc - 1))
                        Loop While File.Exists(Me._currentFilename)

                        Me._traceWriter = New StreamWriter(_currentFilename, True, Encoding)
                    End If

                    Me._traceWriter.AutoFlush = True
                    Return
                Catch __unusedIOException1__ As IOException

                    If System.Threading.Interlocked.Decrement(retryCount) < 0 Then
                        Me._currentFilename = Nothing
                        Me._traceWriter = Nothing
                        Throw
                    End If

                    Thread.Sleep(10)
                End Try
            End While
        End Sub

        Public Overrides Sub TraceEvent(ByVal eventCache As TraceEventCache, ByVal source As String, ByVal eventType As TraceEventType, ByVal id As Integer, ByVal message As String)
            If (Me.Filter Is Nothing) OrElse Me.Filter.ShouldTrace(eventCache, source, eventType, id, message, Nothing, Nothing, Nothing) Then
                Dim logHeader As List(Of String) = BuildHeaders(eventCache, source, eventType, id)
                Dim sb As StringBuilder = New StringBuilder(String.Join(Delimiter, logHeader.ToArray()))

                If sb.Length = 0 Then
                    WriteLine(message)
                Else
                    Write(sb.ToString() & Delimiter)
                    WriteLine(message)
                End If
            End If
        End Sub

        Public Overrides Sub TraceEvent(ByVal eventCache As TraceEventCache, ByVal source As String, ByVal eventType As TraceEventType, ByVal id As Integer, ByVal format As String, ParamArray args As Object())
            TraceEvent(eventCache, source, eventType, id, If(args IsNot Nothing, String.Format(format, args), format))
        End Sub

        Private Function BuildHeaders(ByVal eventCache As TraceEventCache, ByVal source As String, ByVal eventType As TraceEventType, ByVal id As Integer) As List(Of String)
            Dim logHeader As List(Of String) = New List(Of String)()

            For Each info As String In OutputHeaders

                If TraceLogger.AdditionalInfo.ContainsKey(info) Then
                    logHeader.Add(TraceLogger.AdditionalInfo(info))
                ElseIf info = "EventSource" Then
                    logHeader.Add(source)
                ElseIf info = "EventType" Then
                    logHeader.Add(eventType.ToString())
                ElseIf info = "EventId" Then
                    logHeader.Add(id.ToString())
                ElseIf info = "ThreadUserName" Then

                    If Thread.CurrentPrincipal.Identity.IsAuthenticated Then
                        logHeader.Add(Thread.CurrentPrincipal.Identity.Name)
                    Else
                        logHeader.Add("")
                    End If
                ElseIf info = "WindowsUserName" Then
                    logHeader.Add(WindowsIdentity.GetCurrent().Name)
                ElseIf info = "MachineName" Then
                    logHeader.Add(System.Environment.MachineName)
                ElseIf info = "ThreadId" Then
                    logHeader.Add(eventCache.ThreadId)
                ElseIf info = "ProcessId" Then
                    logHeader.Add(eventCache.ProcessId.ToString())
                ElseIf info = "DateTime" Then
                    logHeader.Add(DateTime.Now.ToString(LogDateTimeFormat))
                Else
                    logHeader.Add("")
                End If
            Next

            Return logHeader
        End Function

        Protected Overrides Function GetSupportedAttributes() As String()
            Return _supportedAttributes
        End Function

        Private _supportedAttributes As String() = New String() {"BaseFilename", "Encoding", "Delimiter", "OutputHeaders", "LogDateTimeFormat", "MaxFileSize", "SingleProcess"}

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then

                If _traceWriter IsNot Nothing Then
                    _traceWriter.Dispose()
                    _traceWriter = Nothing
                End If
            End If
        End Sub

        Public Overrides Sub Close()
            If _traceWriter IsNot Nothing Then
                _traceWriter.Close()
                _traceWriter = Nothing
            End If

            MyBase.Close()
        End Sub

        Public Overrides Sub Flush()
            If _traceWriter IsNot Nothing Then _traceWriter.Flush()
            MyBase.Flush()
        End Sub
    End Class
End Namespace
