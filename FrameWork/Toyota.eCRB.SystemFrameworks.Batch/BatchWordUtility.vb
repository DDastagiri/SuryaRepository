Option Strict On
Option Explicit On

Imports System.IO
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Batch

    ''' <summary>
    ''' 文言メッセージを提供するクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class BatchWordUtility


        ''' <summary>
        ''' コードテーブル取得時の排他用に使用するオブジェクト
        ''' </summary>
        Private Shared _lockGetCodeTables As Object = New Object()

        ''' <summary>
        ''' 文言管理
        ''' KEY:画面ID VALUE(KEY:販売店CD VALUE:(KEY:文言NO VALUE:文言)))の連想配列
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _wordTable As New Dictionary(Of String, Dictionary(Of Decimal, String))


        Private Sub New()

        End Sub

        ''' <summary> 
        ''' 文言メッセージを取得します。
        ''' </summary>
        ''' <param name="displayID">機能ID</param>
        ''' <param name="wordNo">文言No</param>
        ''' <returns>文言メッセージ</returns>
        ''' <remarks></remarks>
        Public Shared Function GetWord(ByVal displayId As String, ByVal wordNo As Decimal) As String

            Return GetWordData(displayId, wordNo)

        End Function

        ''' <summary>
        ''' 文言メッセージを取得します。
        ''' </summary>
        ''' <param name="wordNo">文言No</param>
        ''' <returns>文言メッセージ</returns>
        ''' <remarks></remarks>
        Public Shared Function GetWord(ByVal wordNo As Decimal) As String

            Return GetWordData(GetCurrntDisplayID(), wordNo)

        End Function

        ''' <summary>
        ''' 現在の要求に対する機能IDを取得します。
        ''' </summary>
        ''' <returns>機能ID</returns>
        ''' <remarks></remarks>
        Private Shared Function GetCurrntDisplayID() As String

            'Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim fullPath As String = System.Reflection.Assembly.GetEntryAssembly().Location
            Dim displayId As String
            Dim index As Integer = fullPath.IndexOf("MC", StringComparison.OrdinalIgnoreCase)
            displayId = fullPath.Substring(index, 9)

            Return displayId

        End Function

        ''' <summary>
        ''' 文言メッセージを取得します。
        ''' </summary>
        ''' <param name="wordNo">機能ID</param>
        ''' <returns>
        ''' 該当する文言が存在した場合、その文言。
        ''' 存在しなかった場合はString.Emptyを返却します。
        ''' </returns>
        ''' <remarks></remarks>
        Friend Shared Function GetWordData(ByVal displayID As String, ByVal wordNo As Decimal) As String

            Dim wordDict As Dictionary(Of Decimal, String) = Nothing
            If Not _wordTable.ContainsKey(displayID) Then
                Dim path As String = GetWordFilePath(displayID)
                wordDict = New Dictionary(Of Decimal, String)

                If System.IO.File.Exists(path) Then
                    LoadWordFile(path, wordDict)
                Else
                    CheckLoad(displayID, wordDict)
                End If
                _wordTable.Add(displayID, wordDict)
            Else
                wordDict = _wordTable(displayID)
            End If

            If Not wordDict.ContainsKey(wordNo) Then
                Return String.Empty
            End If

            Return wordDict(wordNo)

        End Function

        Private Shared Sub LoadWordFile(ByVal path As String, ByVal wordDict As Dictionary(Of Decimal, String))

            Using wordFile = New System.IO.StreamReader(path, System.Text.Encoding.GetEncoding(0))

                'ワードデータ1行分
                Dim wordLine As String
                'ワードデータ　カンマで区切り
                Dim wordVlue As String()

                While wordFile.Peek() > -1
                    wordLine = wordFile.ReadLine()
                    wordVlue = Nothing
                    wordVlue = wordLine.Split(New Char() {","c}, 2)
                    If wordVlue.Length = 2 Then
                        wordDict.Add(CDec(wordVlue(0).Trim()), wordVlue(1).Trim())
                    Else
                        Throw New FormatException
                    End If
                End While

            End Using

        End Sub


        ''' <summary>
        ''' 引数で指定された画面ＩＤの文言がメモリ上に読み込まれているかチェックし、
        ''' 読み込まれていない場合、メモリー上にロードします。
        ''' </summary>
        ''' <param name="displayID"></param>
        ''' <remarks></remarks>
        Private Shared Sub CheckLoad(ByVal displayID As String, ByVal wordDict As Dictionary(Of Decimal, String))

            '排他処理を行う
            SyncLock _lockGetCodeTables

                '文言読み込み
                LoadWordOneDisplay(displayID, wordDict)

            End SyncLock

        End Sub

        ''' <summary>
        ''' １画面文の文言情報をＤＢから取得します。
        ''' </summary>
        ''' <param name="displayID">画面ID</param>
        Private Shared Sub LoadWordOneDisplay(ByVal displayID As String, ByVal wordDict As Dictionary(Of Decimal, String))
            '引数画面IDに該当する文言情報をＤＢから取得
            Dim dtWord As IcropWordDataSet.IcropWordTableDataTable = IcropWordTableAdapter.GetIcropWordTable(displayID)

            '文言Ｎｏをキー、文言を値として配列に登録
            For Each dr As IcropWordDataSet.IcropWordTableRow In dtWord.Rows
                wordDict.Add(CType(dr.DISPLAYNO, Decimal), dr.WORD)
            Next

            '開放
            dtWord.Dispose()
        End Sub

        Private Shared Function GetWordFilePath(ByVal displayId As String) As String

            Dim base As String = System.AppDomain.CurrentDomain.BaseDirectory
            Dim wordFilePath As New System.Text.StringBuilder
            wordFilePath.Append(base)
            wordFilePath.Append(displayId)
            wordFilePath.Append(".word")

            Return wordFilePath.ToString

        End Function

    End Class

End Namespace
