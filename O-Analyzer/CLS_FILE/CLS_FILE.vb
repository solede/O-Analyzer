Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.FileSystem
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_CONST = CLS_CONST.CLS_CONST

Public Class CLS_FILE
    '#######################################################################
    'ファイル操作クラス
    '#######################################################################

    Private Shared M_ENCODE As System.Text.Encoding = System.Text.Encoding.GetEncoding("SHIFT-JIS")
    Private Shared M_LOCK As New Object 'ファイルの書き込み用ロック
    '別スレッドで実行する負荷テスト時はすべて別ファイルに書き出すように分けたのでロックは不要になった
    'Private Shared streamWriter As StreamWriter = Nothing

    Public Shared Sub SetEncode()
        '#######################################################################
        'エンコード形式のセット
        '#######################################################################
        'エンコード設定
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCODE) = M_CONST.C_ENCODE.SJIS Then
            'SJIS
            M_ENCODE = System.Text.Encoding.GetEncoding("SHIFT-JIS")
        ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCODE) = M_CONST.C_ENCODE.EUC Then
            'EUC
            M_ENCODE = System.Text.Encoding.GetEncoding("EUC-JP")
        ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCODE) = M_CONST.C_ENCODE.UTF8 Then
            'UTF8
            M_ENCODE = System.Text.Encoding.GetEncoding("UTF-8")
        End If
    End Sub

    Public Shared Function CreateCSV(ByRef dgv As Windows.Forms.DataGridView, ByRef dir As String, ByVal fileName As String, _
                                 Optional ByVal encloseStr As String = vbNullString, _
                                 Optional ByVal delimiterStr As String = vbNullString) As Boolean
        '#######################################################################
        'CSVファイル作成
        '引数
        '    dgv：元データのデータグリッドビュー
        '    dir：ファイル作成ディレクトリ
        '    fileName：ファイル名称
        '    encloseStr：囲い文字(デフォルト「"」)
        '    delimiterStr：区切り文字(デフォルト「,」)
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        Dim rowIndex As Integer
        Dim colIindex As Integer
        Dim rowCnt As Integer
        Dim colCnt As Integer
        Dim writeStr As New System.Text.StringBuilder
        Dim fullFileName As String = vbNullString
        Dim ret As Boolean = False

        If dgv.RowCount = 0 Then
            MsgBox(M_COMMON.GetMessage("E0005"))
            Return ret
        End If

        WriteApLog("CSV出力開始")

        rowCnt = dgv.BindingContext(dgv.DataSource, dgv.DataMember).Count
        colCnt = dgv.Columns.Count

        '区切り文字
        If delimiterStr = vbNullString Then
            If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.COMMA Then
                delimiterStr = ","
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.TAB Then
                delimiterStr = ControlChars.Tab
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.SPACE Then
                delimiterStr = " "
            End If
        End If

        '囲い文字
        If encloseStr = vbNullString Then
            If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.NONE Then
                encloseStr = ""
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.DOUBLE_QUOTATION Then
                encloseStr = ControlChars.Quote
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.SINGLE_QUOTATION Then
                encloseStr = "'"
            End If
        End If

        '出力先ディレクトリ
        If dir = vbNullString Then
            Dim sfd As New SaveFileDialog()
            sfd.FileName = fileName
            If sfd.ShowDialog() = DialogResult.OK Then
                fullFileName = sfd.FileName
            Else
                Return ret
            End If

            dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
            If dir = vbNullString Then
                'デフォルトはアプリディレクトリ\log配下
                dir = Directory.GetCurrentDirectory() & "\log"
            End If
        Else
            fullFileName = dir + "\" & fileName
        End If

        Try
            SetEncode()
            streamWriter = New StreamWriter(fullFileName, False, M_ENCODE)

            'カラム出力
            For colIindex = 0 To colCnt - 1 Step 1
                If colIindex = 0 Then
                    writeStr.Append(encloseStr).Append(dgv.Columns.Item(colIindex).Name).Append(encloseStr)
                Else
                    writeStr.Append(delimiterStr).Append(encloseStr).Append(dgv.Columns.Item(colIindex).Name).Append(encloseStr)
                End If
            Next colIindex

            writeStr.AppendLine()
            streamWriter.Write(writeStr.ToString)
            writeStr = New System.Text.StringBuilder

            'データ出力
            For rowIndex = 0 To rowCnt - 1 Step 1
                For colIindex = 0 To colCnt - 1 Step 1
                    If colIindex = 0 Then
                        If dgv.Item(colIindex, rowIndex).ValueType.Name = "Byte[]" Then
                            writeStr.Append(encloseStr).Append("非対応").Append(encloseStr)
                        Else
                            writeStr.Append(encloseStr).Append(dgv.Item(colIindex, rowIndex).Value).Append(encloseStr)
                        End If
                    Else
                        If dgv.Item(colIindex, rowIndex).ValueType.Name = "Byte[]" Then
                            writeStr.Append(delimiterStr).Append(encloseStr).Append("非対応").Append(encloseStr)
                        Else
                            writeStr.Append(delimiterStr).Append(encloseStr).Append(dgv.Item(colIindex, rowIndex).Value).Append(encloseStr)
                        End If
                    End If
                Next colIindex
                writeStr.AppendLine()

                '1行辺りのサイズが小さいデータの場合は4KBを超えない程度でまとめて出力
                If M_ENCODE.GetByteCount(writeStr.ToString) > 3700 Then
                    streamWriter.Write(writeStr.ToString)
                    writeStr = New System.Text.StringBuilder
                End If
            Next rowIndex

            '出力されていない行の書き出し
            If writeStr.ToString <> vbNullString Then
                streamWriter.Write(writeStr.ToString)
            End If
            WriteApLog("CSV出力完了")
            ret = True
        Catch ex As Exception
            WriteErrLog(ex)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try

        Return ret
    End Function

    Public Shared Sub WriteString(ByVal str As String, ByVal dir As String, ByVal fileName As String)
        '#######################################################################
        'CSVデータを書き込む
        '引数
        '    str：書き込む文字列
        '    dir：ディレクトリ
        '    fileName:ファイル名
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing

        Try
            streamWriter = New StreamWriter(dir + "\" & fileName, True, M_ENCODE)
            streamWriter.Write(str)
        Catch ex As Exception
            WriteErrLog(ex)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try
    End Sub

    Public Shared Sub WriteApLog(ByVal log As String, Optional ByVal dir As String = "", _
                                   Optional ByVal fileName As String = "")
        '#######################################################################
        'アプリケーションログ出力
        '引数
        '    log：書き込みデータ
        '    dir：ディレクトリ
        '    filename:ファイル名
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        'ロギング無効の場合は処理を抜ける
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_APLOG) = "0" Then
            Exit Sub
        End If

        'Dim streamWriter As StreamWriter = Nothing
        'streamWriter = Nothing
        Dim errorStreamWriter As StreamWriter = Nothing
        Dim date_time As DateTime = DateTime.Now
        'Dim lock As New Object

        If dir = vbNullString Then
            dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
            If dir = vbNullString Then
                'デフォルトはカレントディレクトリ\log配下
                dir = Directory.GetCurrentDirectory() & "\log"
            End If
        End If

        If CreateDir(dir) = False Then
            Exit Sub
        End If

        If fileName = vbNullString Then
            fileName = Format(date_time, "yyyyMMdd") & ".txt"
        End If

        SyncLock (M_LOCK)
            Try
                streamWriter = New StreamWriter(dir + "\" & fileName, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
                streamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & log)
            Catch ex As System.IO.IOException
                'その他プロセスがファイルをつかんでいた場合等
                Try
                    errorStreamWriter = New StreamWriter(dir + "\IO_ERROR_" & Process.GetCurrentProcess().Id() & "_" & Threading.Thread.CurrentThread.ManagedThreadId & ".TXT", True, System.Text.Encoding.GetEncoding("Shift-JIS"))
                    errorStreamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & ex.Message)
                    errorStreamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & log)
                Catch ex3 As Exception
                    MsgBox(ex3.Message)
                Finally
                    If Not (errorStreamWriter Is Nothing) Then
                        errorStreamWriter.Close()
                        errorStreamWriter.Dispose()
                        errorStreamWriter = Nothing
                    End If
                End Try
            Catch ex4 As Threading.ThreadAbortException
                '何もしない
            Catch ex2 As Exception
                MsgBox(ex2.Message)
            Finally
                If Not (streamWriter Is Nothing) Then
                    streamWriter.Close()
                    streamWriter.Dispose()
                    streamWriter = Nothing
                End If
            End Try
        End SyncLock
    End Sub

    Public Shared Sub WriteErrLog(ByVal exception As Exception, Optional ByVal dir As String = "", _
                                    Optional ByVal fileName As String = "")
        '#######################################################################
        'エラー情報をログとして書き込む
        '引数
        '    exception：例外
        '    dir：ディレクトリ
        '    fileName: ファイル名()
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        'ロギング無効の場合
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ERRLOG) = "0" Then
            Exit Sub
        End If

        'Dim streamWriter As StreamWriter = Nothing
        Dim errorStreamWriter As StreamWriter = Nothing
        Dim date_time As DateTime = DateTime.Now
        Dim log As String
        'Dim lock As New Object

        log = "エラーメッセージ:" & exception.Message & vbCrLf
        log = log & "エラーソース:" & exception.Source & vbCrLf
        log = log & "スタックトレース:" & exception.StackTrace

        If dir = vbNullString Then
            dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
            If dir = vbNullString Then
                'デフォルトはカレントディレクトリ\log配下
                dir = Directory.GetCurrentDirectory() & "\log"
            End If
        End If

        If CreateDir(dir) = False Then
            Exit Sub
        End If

        If fileName = vbNullString Then
            fileName = Format(date_time, "yyyyMMdd") & ".txt"
        End If

        'SyncLock (M_LOCK)
        Try
            streamWriter = New StreamWriter(dir + "\" & fileName, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
            streamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & log)
        Catch ex As System.IO.IOException
            'その他プロセスがファイルをつかんでいた場合等
            Try
                errorStreamWriter = New StreamWriter(dir + "\IO_ERROR_" & Process.GetCurrentProcess().Id() & "_" & Threading.Thread.CurrentThread.ManagedThreadId & ".TXT", True, System.Text.Encoding.GetEncoding("Shift-JIS"))
                errorStreamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & ex.Message)
                errorStreamWriter.WriteLine(date_time.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & log)
            Catch ex3 As Exception
                MsgBox(ex3.Message)
            Finally
                If Not (errorStreamWriter Is Nothing) Then
                    errorStreamWriter.Close()
                    errorStreamWriter.Dispose()
                    errorStreamWriter = Nothing
                End If
            End Try
        Catch ex4 As Threading.ThreadAbortException
            '何もしない
        Catch ex2 As Exception
            MsgBox(ex2.Message)
        Finally
            If Not (streamWriter Is Nothing) Then
                streamWriter.Close()
                streamWriter.Dispose()
                streamWriter = Nothing
            End If
        End Try
        'End SyncLock
    End Sub

    Public Shared Function SetClipboardSelectedData(ByRef dgv As DataGridView, Optional ByVal endLineStr As String = ControlChars.CrLf) As Boolean
        '#######################################################################
        '選択されているデータをクリップボードにセットする
        '引数
        '    dgv：コピーするデータグリッドビュー
        '    endLineStr：改行コード(デフォルト：CRLF)
        '    delimiterStr：区切り文字(デフォルト：TAB)
        '    enclosureStr:囲い文字(デフォルト：ダブルクォーテーション)
        '#######################################################################
        Dim clipbortText As String = vbNullString
        Dim tmpClipboardCopyMode As DataGridViewClipboardCopyMode
        Dim ret As Boolean = False

        If dgv.RowCount = 0 Then
            MsgBox(M_COMMON.GetMessage("E0005"))
            Return ret
        End If

        WriteApLog("コピー処理開始")
        'Console.WriteLine(System.DateTime.Now)

        ''''Clipboard.SetDataObjectでクリップボードに貼り付けた場合区切り文字等を事前に設定できないためコメント化
        ''区切り文字
        'If delimiterStr = vbNullString Then
        '    If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.COMMA Then
        '        delimiterStr = ","
        '    ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.TAB Then
        '        delimiterStr = ControlChars.Tab
        '    End If
        'End If

        ''囲い文字
        'If enclosureStr = vbNullString Then
        '    If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.NONE Then
        '        enclosureStr = ""
        '    ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.DOUBLE_QUOTATION Then
        '        enclosureStr = ControlChars.Quote
        '    ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.SINGLE_QUOTATION Then
        '        enclosureStr = "'"
        '    End If
        'End If

        If dgv.GetCellCount(DataGridViewElementStates.Selected) > 0 Then
            'dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
            tmpClipboardCopyMode = dgv.ClipboardCopyMode
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText
            Try
                Clipboard.SetDataObject(dgv.GetClipboardContent(), False)
                clipbortText = Clipboard.GetText(TextDataFormat.Text)
                ''ROWヘッダが空である場合は先頭のtabを削除
                'If clipbortText.Substring(0, 1) = ControlChars.Tab Then
                '    clipbortText = Right(clipbortText, clipbortText.Length - 1)
                'End If
                'clipbortText = clipbortText.Replace(ControlChars.CrLf & ControlChars.Tab, ControlChars.CrLf)
                'Clipboard.Clear()

                'テキスト部分のみコピー
                Clipboard.SetText(clipbortText, TextDataFormat.Text)
                WriteApLog("コピー処理終了")
                ret = True
            Catch ex As System.Runtime.InteropServices.ExternalException
                MsgBox(ex.Message)
                WriteErrLog(ex)
            Finally
                dgv.ClipboardCopyMode = tmpClipboardCopyMode
            End Try
        End If

        Return ret
    End Function

    Public Shared Function SetClipboardAllData(ByRef dgv As DataGridView, Optional ByVal endLineStr As String = ControlChars.CrLf, _
                                Optional ByVal delimiterStr As String = ControlChars.Tab, _
                                Optional ByVal enclosureStr As String = ControlChars.Quote) As Boolean
        Dim rowIndex As Integer
        Dim colIindex As Integer
        Dim rowCnt As Integer
        Dim colCnt As Integer
        Dim writeStr As New System.Text.StringBuilder()
        Dim rowsize As Integer = 0

        If dgv.RowCount = 0 Then
            MsgBox(M_COMMON.GetMessage("E0005"))
            Return False
        End If

        WriteApLog("コピー処理開始")

        'Console.WriteLine("開始:" & System.DateTime.Now)
        'Debug.Print("開始:" & System.DateTime.Now)

        rowCnt = dgv.BindingContext(dgv.DataSource, dgv.DataMember).Count
        colCnt = dgv.Columns.Count

        '区切り文字
        If delimiterStr = vbNullString Then
            If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.COMMA Then
                delimiterStr = ","
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_DELIMITER) = M_CONST.C_DELIMITER.TAB Then
                delimiterStr = ControlChars.Tab
            End If
        End If

        '囲い文字
        If enclosureStr = vbNullString Then
            If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.NONE Then
                enclosureStr = ""
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.DOUBLE_QUOTATION Then
                enclosureStr = ControlChars.Quote
            ElseIf M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ENCLOSURE) = M_CONST.C_ENCLOSURE.SINGLE_QUOTATION Then
                enclosureStr = "'"
            End If
        End If

        Try
            'カラムヘッダ出力
            For colIindex = 0 To colCnt - 1 Step 1
                If colIindex = 0 Then
                    writeStr.Append(enclosureStr).Append(dgv.Columns.Item(colIindex).Name).Append(enclosureStr)
                Else
                    writeStr.Append(delimiterStr).Append(enclosureStr).Append(dgv.Columns.Item(colIindex).Name).Append(enclosureStr)
                End If
            Next colIindex

            writeStr.AppendLine()

            'データ出力
            For rowIndex = 0 To rowCnt - 1 Step 1
                If rowIndex = 0 Then
                    '行サイズを獲得する処理
                    rowsize = writeStr.ToString.Length
                End If

                For colIindex = 0 To colCnt - 1 Step 1
                    If colIindex = 0 Then
                        If dgv.Item(colIindex, rowIndex).ValueType.Name = "Byte[]" Then
                            writeStr.Append(enclosureStr).Append("非対応").Append(enclosureStr)
                        Else
                            writeStr.Append(enclosureStr).Append(dgv.Item(colIindex, rowIndex).Value).Append(enclosureStr)
                        End If
                    Else
                        If dgv.Item(colIindex, rowIndex).ValueType.Name = "Byte[]" Then
                            writeStr.Append(delimiterStr).Append(enclosureStr).Append("非対応").Append(enclosureStr)
                        Else
                            writeStr.Append(delimiterStr).Append(enclosureStr).Append(dgv.Item(colIindex, rowIndex).Value).Append(enclosureStr)
                        End If
                    End If
                Next colIindex
                writeStr.AppendLine()

                'If rowIndex Mod 1000 = 0 Then
                '    'Console.WriteLine(rowIndex)
                '    Debug.Print("開始:" & System.DateTime.Now)
                'End If
            Next rowIndex

            'クリップボードへコピー
            Clipboard.SetText(writeStr.ToString)
            'Console.WriteLine("完了:" & System.DateTime.Now)
            'Debug.Print("開始:" & System.DateTime.Now)
            WriteApLog("コピー処理終了")
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            WriteErrLog(ex)
            Return False
        End Try
    End Function

    Public Shared Sub SetClipboardFromSelectedGridViewCell(ByRef dgv As DataGridView, Optional ByVal endLineStr As String = ControlChars.CrLf, _
                                    Optional ByVal delimiterStr As String = ControlChars.Tab, _
                                    Optional ByVal enclosureStr As String = ControlChars.Quote)
        '#######################################################################
        'DataGridView選択cellのクリップボードへのコピー
        '#######################################################################
        Dim copydata As StringBuilder = New StringBuilder
        Dim isFirstCol As Boolean = True
        Dim isFirstRow As Boolean = True
        Dim colCnt As Integer
        Dim rowCnt As Integer
        Dim minRow As Integer = 2147483647
        Dim minCol As Integer = 2147483647
        Dim maxRow As Integer = 0
        Dim maxCol As Integer = 0
        Dim isminRow As Boolean = False
        Dim isminCol As Boolean = False
        Dim ismaxRow As Boolean = False
        Dim ismaxCol As Boolean = False
        Dim currentCol As Integer
        Dim currentRow As Integer

        If dgv.RowCount = 0 Then
            MsgBox(M_COMMON.GetMessage("E0005"))
            Exit Sub
        End If

        Console.WriteLine(System.DateTime.Now)

        rowCnt = dgv.Rows.Count
        colCnt = dgv.ColumnCount

        'コピー対象範囲の確認
        '全参照(遅い)
        For i As Integer = 0 To dgv.SelectedCells.Count - 1
            If isminCol = False Then
                If dgv.SelectedCells(i).ColumnIndex < minCol Then
                    minCol = dgv.SelectedCells(i).ColumnIndex
                    If minCol = 0 Then
                        isminCol = True
                    End If
                End If
            End If

            If ismaxCol = False Then
                If dgv.SelectedCells(i).ColumnIndex > maxCol Then
                    maxCol = dgv.SelectedCells(i).ColumnIndex
                    If maxCol = 0 Then
                        ismaxCol = True
                    End If
                End If
            End If

            If isminRow = False Then
                If dgv.SelectedCells(i).RowIndex < minRow Then
                    minRow = dgv.SelectedCells(i).RowIndex
                    If minRow = 0 Then
                        isminRow = True
                    End If
                End If
            End If

            If ismaxRow = False Then
                If dgv.SelectedCells(i).RowIndex > maxRow Then
                    maxRow = dgv.SelectedCells(i).RowIndex
                    If maxRow = 0 Then
                        ismaxRow = True
                    End If
                End If
            End If
            Console.WriteLine(i)
        Next

        Console.WriteLine(System.DateTime.Now)

        For currentRow = minRow To maxRow
            isFirstCol = True
            If isFirstRow Then
                isFirstRow = False
            Else
                copydata.Append(ControlChars.CrLf)
            End If

            For currentCol = minCol To maxCol
                If isFirstCol Then
                    If dgv.Rows(currentRow).Cells(currentCol).Selected Then
                        copydata.Append(dgv.Rows(currentRow).Cells(currentCol).Value)
                        isFirstCol = False
                    End If
                Else
                    If dgv.Rows(currentRow).Cells(currentCol).Selected Then
                        copydata.Append(delimiterStr & dgv.Rows(currentRow).Cells(currentCol).Value)
                    Else
                        copydata.Append(delimiterStr)
                    End If
                End If
            Next

        Next
        Clipboard.SetText(copydata.ToString)
        Console.WriteLine(System.DateTime.Now)
    End Sub

    Public Shared Function CreateDir(ByVal dir As String) As Boolean
        '#######################################################################
        '指定されたディレクトリを作成する
        '引数
        '   dir：ディレクトリ名
        '戻り値
        '   TRUE：ディレクトリ作成成功
        '   FALSE：ディレクトリ作成失敗
        '#######################################################################

        If dir = vbNullString Then
            dir = Directory.GetCurrentDirectory()
        End If

        If My.Computer.FileSystem.DirectoryExists(dir) = False Then
            Try
                My.Computer.FileSystem.CreateDirectory(dir)
                Return True
            Catch ex As Exception
                WriteApLog("ディレクトリ(" & dir & ")を作成出来ませんでした。理由：" & ex.Message)
                Return False
            End Try
        End If

        Return True

    End Function

    Public Shared Sub CreateConfigFile(ByRef initParams() As String, Optional ByVal fileName As String = "config.ini", _
                                         Optional ByVal dir As String = "")
        '#######################################################################
        'コンフィグファイルを作成する
        '引数
        '   initParams:パラメータの内容
        '   fileName：ファイル名称
        '   dir：ディレクトリ
        '#######################################################################
        Dim streamWriter As StreamWriter
        Dim dataTime As DateTime = DateTime.Now
        Dim fullPass As String

        If dir = vbNullString Then
            dir = Directory.GetCurrentDirectory()
        End If

        fullPass = dir + "\" & fileName

        If My.Computer.FileSystem.FileExists(dir & "\" & fileName) Then
            File.Delete(fullPass)
        End If

        streamWriter = New StreamWriter(fullPass, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
        '改行コード(CRLF)で分割
        For i As Integer = 0 To initParams.Length - 1
            streamWriter.WriteLine(initParams(i).Trim(ControlChars.CrLf))
        Next

        streamWriter.Close()
        streamWriter.Dispose()
        WriteApLog(fullPass & "を作成しました")
    End Sub

    Public Shared Function ReadConfigFile(ByVal fileName As String, Optional ByVal dir As String = "") As String()
        '#######################################################################
        'コンフィグファイルを読み込み文字列として返す
        '引数
        '   fileName：ファイル名称
        '   dir：ディレクトリ
        '戻り値
        '   コンフィグファイルのテキスト文字列
        '#######################################################################
        Dim rows() As String

        rows = (File.ReadAllLines(fileName, System.Text.Encoding.GetEncoding("Shift-JIS")))
        For i As Integer = 0 To rows.Length - 1
            rows(i) = M_COMMON.DelCtrlChars(rows(i))
        Next
        Return rows

    End Function

    Public Shared Sub SetControls(ByVal formName As String, ByRef controls As Control.ControlCollection)
        '#######################################################################
        '各画面用のiniファイルからコントロールの初期状態を設定する
        '引数
        '   formName：フォーム名称
        '   controls：画面内のコントロール
        '#######################################################################
        Dim dir As String = Directory.GetCurrentDirectory() & "\init"
        Dim fileName As String = formName & ".ini"
        Dim streamReader As StreamReader
        Dim tempStr As String
        Dim rows() As String
        Dim controlItem As Control

        If CreateDir(dir) = False Then
            Exit Sub
        End If

        If My.Computer.FileSystem.FileExists(dir & "\" & fileName) Then
            streamReader = New StreamReader(dir & "\" & fileName, _
                                              System.Text.Encoding.GetEncoding("Shift-JIS"))
            tempStr = streamReader.ReadToEnd
            rows = tempStr.Split(ControlChars.CrLf)
            streamReader.Close()
            streamReader.Dispose()

            For Each controlItem In controls
                'バージョンによって使用不能なコントロールがあるためenabled=trueのコントロールのみ処理
                If controlItem.Enabled Then
                    SetControlsValue(rows, controlItem)
                End If
            Next
        End If
        WriteApLog("コントロール(" & formName & ")の設定完了")
    End Sub

    Public Shared Sub SetControlsValue(ByVal rows As String(), ByRef control As Object)
        '#######################################################################
        'コントロールの値を設定する
        '引数
        '   rows     ：設定値
        '   control：設定するコントロール
        '#######################################################################
        Dim value(2) As String
        Dim tempRow As String = ""
        Dim textLength As Integer = 0
        Dim text As String = ""
        Dim offSet As Integer = 0
        Dim controlItem As Control

        Try
            '画面コントロールの値を設定
            If control.GetType Is GetType(TextBox) Or control.GetType Is GetType(RichTextBox) Or control.GetType Is GetType(CLS_COMMON.RegExTextBox) Then
                'テキストボックス
                If control.ReadOnly Then
                    '読み取り専用のTEXTBOXには何も設定しない
                    Exit Sub
                End If
                If control.multiline Then
                    'マルチライン可のテキストボックス
                    For i As Integer = 0 To rows.Length - 1
                        value = M_COMMON.DelCtrlChars(rows(i)).Split(":")
                        If control.NAME = value(0) Then
                            tempRow = rows(i)
                            textLength = value(1)
                            '2回目の「:」以降のデータがTEXTBOXのデータとなるので抽出
                            tempRow = tempRow.Substring(InStr(tempRow, ":"), tempRow.Length - InStr(tempRow, ":"))
                            tempRow = tempRow.Substring(InStr(tempRow, ":"), tempRow.Length - InStr(tempRow, ":"))
                            text = tempRow
                            offSet = offSet + tempRow.Length
                            For j As Integer = i + 1 To rows.Length - 1
                                rows(j) = rows(j).Replace(ControlChars.Lf, "")
                                If offSet >= textLength Then
                                    control.TEXT = text
                                    Exit For
                                End If
                                text = text & ControlChars.CrLf & rows(j)
                                offSet = offSet + rows(j).Length + ControlChars.CrLf.Length
                            Next
                            Exit For
                        End If
                    Next
                Else
                    'マルチライン不可のテキストボックス
                    For i As Integer = 0 To rows.Length - 1
                        value = rows(i).Trim().Trim(ControlChars.CrLf).Split(":")
                        If control.NAME = value(0) Then
                            tempRow = rows(i)
                            If value(1) = vbNullString Then
                                control.TEXT = vbNullString
                                Exit Sub
                            End If

                            textLength = Integer.Parse(value(1))


                            '1回目の「:」以降のデータがTEXTBOXのTEXTデータになる
                            tempRow = tempRow.Substring(InStr(tempRow, ":"), tempRow.Length - InStr(tempRow, ":"))
                            tempRow = tempRow.Substring(InStr(tempRow, ":"), tempRow.Length - InStr(tempRow, ":"))
                            text = tempRow
                            offSet = offSet + tempRow.Length
                            For j As Integer = i + 1 To rows.Length - 1
                                rows(j) = rows(j).Replace(ControlChars.Lf, "")
                                If offSet >= textLength Then
                                    If Not control.PasswordChar = Nothing Then
                                        'パスワード入力用かつセキュリティフラグがオフの場合、暗号化した文字列を記録
                                        control.TEXT = M_COMMON.DecryptString(text)
                                    Else
                                        control.TEXT = text
                                    End If
                                    Exit For
                                End If
                                text = text & ControlChars.CrLf & rows(j)
                                offSet = offSet + rows(j).Length + ControlChars.CrLf.Length
                            Next
                            Exit For
                        End If
                    Next
                End If
            ElseIf control.GetType Is GetType(ComboBox) Then
                'コンボボックス
                For i As Integer = 0 To rows.Length - 1
                    value = rows(i).Trim().Trim(ControlChars.CrLf).Split(":")
                    If control.NAME = value(0) Then
                        control.SelectedIndex = value(1)
                        Exit For
                    End If
                Next
            ElseIf control.GetType Is GetType(CheckBox) Then
                'チェックボックス
                For i As Integer = 0 To rows.Length - 1
                    value = rows(i).Trim().Trim(ControlChars.CrLf).Split(":")
                    If control.NAME = value(0) Then
                        control.checked = value(1)
                        Exit For
                    End If
                Next
                'ElseIf W_CONTROL_ITEM.GetType Is GetType(RadioButton) Then
                '    For W_CNT As Integer = 0 To W_ROW.Length - 1
                '        W_VALUE = W_ROW(W_CNT).Trim().Trim(ControlChars.CrLf).Split(":")
                '        If W_CONTROL_ITEM.NAME = W_VALUE(0) Then
                '            W_CONTROL_ITEM.checked = W_VALUE(1)
                '            Exit For
                '        End If
                '    Next
            ElseIf control.GetType Is GetType(TabControl) Or _
                   control.GetType Is GetType(TabPage) Or _
                   control.GetType Is GetType(SplitContainer) Or _
                   control.GetType Is GetType(SplitterPanel) Or _
                   control.GetType Is GetType(GroupBox) Or _
                   control.GetType Is GetType(ToolStripContainer) Or _
                   control.GetType Is GetType(ToolStripContentPanel) Then

                'コントロールをまとめるタイプのコントロールの場合は再帰処理
                For Each controlItem In control.Controls
                    'バージョンによって使用不能なコントロールがあるためenabled=trueのコントロールのみ処理
                    If controlItem.Enabled Then
                        SetControlsValue(rows, controlItem)
                    End If
                Next
            End If
        Catch ex As Exception
            WriteErrLog(ex)
            WriteApLog("コントロール(" & control.NAME & ")の設定できませんでした。")
        End Try
    End Sub

    Public Shared Sub CreateControlInitFile(ByVal formName As String, _
                                               ByRef control As System.Windows.Forms.Control.ControlCollection, _
                                               Optional ByVal isEnableSecurity As Boolean = True)
        '#######################################################################
        '各画面のiniファイルを作成する(設定値は現状の状態)
        '引数
        '   formName：フォーム名称
        '   control：画面内のコントロール
        '   isEnableSecurity:パスワード文字保存フラグ(TRUE:保存しない、FALSE:保存する)
        '#######################################################################
        Dim fileName As String = formName & ".ini"
        Dim dir As String = Directory.GetCurrentDirectory() & "\init"
        Dim streamWriter As StreamWriter = New StreamWriter(System.IO.Stream.Null)
        Dim controlItem As Control

        Try
            If CreateDir(dir) = False Then
                Exit Sub
            End If

            If My.Computer.FileSystem.FileExists(dir & "\" & fileName) Then
                File.Delete(dir & "\" & fileName)
            End If

            '画面コントロールの値を記録
            streamWriter = New StreamWriter(dir & "\" & fileName, True, _
                                              System.Text.Encoding.GetEncoding("Shift-JIS"))
            For Each controlItem In control
                WriteControlsValue(streamWriter, controlItem, isEnableSecurity)
            Next

            WriteApLog(fileName & "を作成しました")
        Catch ex As Exception
            WriteApLog(fileName & "を作成出来ませんでした。理由：" & ex.Message)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try
    End Sub

    Private Shared Sub WriteControlsValue(ByRef streamWriter As StreamWriter, ByRef control As Object, _
                                           Optional ByVal isEnableSecurity As Boolean = True)
        '#######################################################################
        'コントロールの値をファイルに書き出す
        '引数
        '   streamWriter：設定を書き込むストリームライター
        '   control     ：値を取得するコントロール
        '   isEnableSecurity：パスワード形式の値の保存有無フラグ
        '#######################################################################
        Dim controlItem As Control
        Dim encryptString As String

        If control.GetType Is GetType(TextBox) Or control.GetType Is GetType(CLS_COMMON.RegExTextBox) Then
            'テキストボックス
            If (Not control.PasswordChar = Nothing And isEnableSecurity) Or control.ReadOnly Then
                'パスワード入力用かつセキュリティフラグがオンの場合、または読み取り専用の場合値は記録しない
                streamWriter.WriteLine(control.Name & ":")
            ElseIf (Not control.PasswordChar = Nothing) And isEnableSecurity = False Then
                'パスワード入力用かつセキュリティフラグがオフの場合、暗号化した文字列を記録
                encryptString = M_COMMON.EncryptString(control.Text)
                streamWriter.WriteLine(control.Name & ":" & encryptString.Length & ":" & encryptString)
            Else
                streamWriter.WriteLine(control.Name & ":" & control.TextLength & ":" & control.Text)
            End If
        ElseIf control.GetType Is GetType(RichTextBox) Then
            'テキストボックス
            streamWriter.WriteLine(control.Name & ":" & control.TextLength & ":" & control.Text)
        ElseIf control.GetType Is GetType(ComboBox) Then
            'コンボボックス
            streamWriter.WriteLine(control.Name & ":" & control.SelectedIndex)
        ElseIf control.GetType Is GetType(CheckBox) Then
            'チェックボックス
            streamWriter.WriteLine(control.Name & ":" & control.checked)
            'ラジオボタンは今のところ非対応
            'ElseIf W_CONTROL_ITEM.GetType Is GetType(RadioButton) Then
            '    W_STREAMWRITER.WriteLine(W_CONTROL_ITEM.Name & ":" & W_CONTROL_ITEM.checked)
        ElseIf control.GetType Is GetType(TabControl) Or _
               control.GetType Is GetType(TabPage) Or _
               control.GetType Is GetType(SplitContainer) Or _
               control.GetType Is GetType(SplitterPanel) Or _
               control.GetType Is GetType(GroupBox) Or _
               control.GetType Is GetType(ToolStripContainer) Or _
               control.GetType Is GetType(ToolStripContentPanel) Then
            'コントロールをまとめるタイプのコントロールの場合は再帰処理
            For Each controlItem In control.Controls
                WriteControlsValue(streamWriter, controlItem, isEnableSecurity)
            Next
        End If
    End Sub

    Public Shared Sub CreateAssemblyInfoFile()
        '#######################################################################
        'アセンブリ情報の出力
        '#######################################################################
        Dim fileName As String = "assemblyInfo.tsv"
        Dim dir As String
        Dim streamWriter As StreamWriter = New StreamWriter(System.IO.Stream.Null)
        Dim assemblyInfoStr(,) As String
        Dim i As Integer

        dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
        If dir = vbNullString Then
            'デフォルトはカレントディレクトリ\log配下
            dir = Directory.GetCurrentDirectory() & "\log"
        End If

        Try
            If CreateDir(dir) = False Then
                Exit Sub
            End If

            If My.Computer.FileSystem.FileExists(dir & "\" & fileName) Then
                File.Delete(dir & "\" & fileName)
            End If

            'アセンブリ情報取得
            assemblyInfoStr = M_COMMON.GetAssemblyInfo()

            'ファイル書き出し
            streamWriter = New StreamWriter(dir + "\" & fileName, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
            streamWriter.WriteLine("AssemblyName" & ControlChars.Tab & "FullPath")
            For i = 0 To (assemblyInfoStr.Length / 2) - 1
                streamWriter.WriteLine(assemblyInfoStr(i, 0) & ControlChars.Tab & assemblyInfoStr(i, 1))
            Next
            WriteApLog(fileName & "を作成しました")
        Catch ex As Exception
            WriteApLog(fileName & "を作成出来ませんでした。理由：" & ex.Message)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try
    End Sub

    Public Shared Sub CreateModuleInfoFile()
        '#######################################################################
        'モジュール情報の出力
        '#######################################################################
        Dim fileName As String = "moduleInfo.tsv"
        Dim dir As String
        Dim streamWriter As StreamWriter = New StreamWriter(System.IO.Stream.Null)
        Dim moduleInfoStr(,) As String
        Dim i As Integer

        dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
        If dir = vbNullString Then
            'デフォルトはカレントディレクトリ\log配下
            dir = Directory.GetCurrentDirectory() & "\log"
        End If

        Try
            If CreateDir(dir) = False Then
                Exit Sub
            End If
            If My.Computer.FileSystem.FileExists(dir & "\" & fileName) Then
                File.Delete(dir & "\" & fileName)
            End If

            'モジュール情報取得
            moduleInfoStr = M_COMMON.GetProcessModuleInfo()

            'ファイル書き出し
            streamWriter = New StreamWriter(dir + "\" & fileName, True, System.Text.Encoding.GetEncoding("Shift-JIS"))
            streamWriter.WriteLine("FileName" & ControlChars.Tab & "Version" & ControlChars.Tab & "MemorySize")
            For i = 0 To (moduleInfoStr.Length / 3) - 1
                streamWriter.WriteLine(moduleInfoStr(i, 0) & ControlChars.Tab & moduleInfoStr(i, 1) & ControlChars.Tab & moduleInfoStr(i, 2))
            Next
            streamWriter.Close()
            WriteApLog(fileName & "を作成しました")
        Catch ex As Exception
            WriteApLog(fileName & "を作成出来ませんでした。理由：" & ex.Message)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try
    End Sub

    Public Shared Sub CreateConnList(ByVal index As Integer, ByVal value As String)
        '#######################################################################
        '接続リストファイルを作成する
        '引数
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        Dim dataTime As DateTime = DateTime.Now
        Dim connListFileName As String
        Dim rows() As String

        connListFileName = Directory.GetCurrentDirectory() & "\" & "connList.ini"

        Try
            If My.Computer.FileSystem.FileExists(connListFileName) Then
                rows = (File.ReadAllLines(connListFileName, System.Text.Encoding.GetEncoding("Shift-JIS")))
                If rows.Length > index Then
                    rows(index) = value
                Else
                    '新規追加
                    ReDim Preserve rows(rows.Length)
                    rows(index) = value
                End If

                streamWriter = My.Computer.FileSystem.OpenTextFileWriter(connListFileName, False, System.Text.Encoding.GetEncoding("Shift-JIS"))
                For Each row As String In rows
                    streamWriter.WriteLine(row)
                Next
            Else
                streamWriter = My.Computer.FileSystem.OpenTextFileWriter(connListFileName, False, System.Text.Encoding.GetEncoding("Shift-JIS"))
                streamWriter.WriteLine(value)
            End If

        Catch ex As Exception
            WriteErrLog(ex)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try

        WriteApLog(connListFileName & "を作成しました")
    End Sub

    Public Shared Sub RemoveConnlist(ByVal index As Integer)
        '#######################################################################
        '接続リストファイルを作成する
        '引数
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        Dim dataTime As DateTime = DateTime.Now
        Dim connListFileName As String
        Dim rows() As String

        connListFileName = Directory.GetCurrentDirectory() & "\" & "connList.ini"

        Try
            If My.Computer.FileSystem.FileExists(connListFileName) Then
                rows = (File.ReadAllLines(connListFileName, System.Text.Encoding.GetEncoding("Shift-JIS")))

                For i As Integer = index + 1 To rows.Length - 1
                    rows(i - 1) = rows(i)
                Next

                ReDim Preserve rows(rows.Length - 2)

                streamWriter = My.Computer.FileSystem.OpenTextFileWriter(connListFileName, False, System.Text.Encoding.GetEncoding("Shift-JIS"))
                For Each row As String In rows
                    streamWriter.WriteLine(row)
                Next
            Else
                Exit Sub
            End If
        Catch ex As Exception
            WriteErrLog(ex)
        Finally
            streamWriter.Close()
            streamWriter.Dispose()
        End Try
        WriteApLog(connListFileName & "を作成しました")
    End Sub

    Public Shared Function GetConnList() As String()
        '#######################################################################
        '接続リストファイルの内容を戻す
        '引数
        '#######################################################################
        Dim streamWriter As StreamWriter = Nothing
        Dim connListFileName As String
        Dim rows() As String = Nothing

        connListFileName = Directory.GetCurrentDirectory() & "\" & "connList.ini"

        Try
            If My.Computer.FileSystem.FileExists(connListFileName) Then
                rows = (File.ReadAllLines(connListFileName, System.Text.Encoding.GetEncoding("Shift-JIS")))
            End If
        Catch ex As Exception
            WriteErrLog(ex)
        End Try

        Return rows
    End Function

End Class
