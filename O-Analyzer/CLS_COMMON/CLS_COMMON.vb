Imports Microsoft.VisualBasic.FileSystem
Imports M_CONST = CLS_CONST.CLS_CONST
Imports System.IO

Public Class CLS_COMMON
    '#######################################################################
    '汎用クラス
    '#######################################################################

    Private Const abc As String = "nfdoasuobfejo;ger"

    Public Shared Sub ExecuteCmd(ByVal cmdFileName As String, Optional ByVal dirName As String = "", Optional ByVal args As String = vbNullString)
        '#######################################################################
        'ファイルを実行する
        'cmdFileName:実行するファイル名
        'dirName：実行ファイルのディレクトリ
        '#######################################################################
        Dim msgstr(0) As String

        If dirName = vbNullString Then
            dirName = Directory.GetCurrentDirectory()
        End If

        If My.Computer.FileSystem.FileExists(dirName & "\" & cmdFileName) = False Then
            msgstr(0) = dirName & "\" & cmdFileName
            MsgBox(GetMessage("E0002", msgstr))
            Exit Sub
        End If

        If args = vbNullString Then
            System.Diagnostics.Process.Start(dirName & "\" & cmdFileName)
        Else
            System.Diagnostics.Process.Start(dirName & "\" & cmdFileName, args)
        End If

    End Sub

    Public Shared Sub ExecuteCmd2(ByVal cmdFileName As String)
        '#######################################################################
        'ファイルを実行する
        'cmdFileName:実行するコマンド
        '#######################################################################
        Dim msgstr(0) As String
        Try
            System.Diagnostics.Process.Start(cmdFileName)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Shared Function GetDGVStr(ByVal isOutputHeader As Boolean, ByVal dgv As Object, _
                                           Optional ByVal delimiterStr As String = ControlChars.Tab, _
                                           Optional ByVal enclosureStr As String = ControlChars.Quote) As String()
        '#######################################################################
        'データグリッドビューのデータを文字列で戻す
        '引数
        '    isOutputHeader:ヘッダを出力するか否か
        '    dgv：コピー元データグリッドビュー
        '    delimiterStr：区切り文字(デフォルト：TAB)
        '    enclosureStr：囲い文字(デフォルト：ダブルクォーテーション)
        '戻り値
        '    全行の文字列
        '#######################################################################
        Dim isStart As Boolean
        Dim rowIndex As Integer
        Dim colIndex As Integer
        Dim rowCnt As Integer = dgv.Rows.Count
        Dim colCnt As Integer = dgv.Columns.Count
        Dim rowStr As String = vbNullString
        Dim allRows(rowCnt - 1) As String

        If isOutputHeader Then
            isStart = True
            'カラム出力
            For colIndex = 0 To colCnt - 1 Step 1
                If isStart = True Then
                    rowStr = enclosureStr & dgv.Columns.Item(colIndex).Name & enclosureStr
                    isStart = False
                Else
                    rowStr = rowStr & delimiterStr & enclosureStr & dgv.Columns.Item(colIndex).Name & enclosureStr
                End If
            Next colIndex

            allRows(0) = rowStr
            rowStr = vbNullString
        End If

        isStart = True

        'データ出力
        For rowIndex = 0 To rowCnt - 1 Step 1
            For colIndex = 0 To colCnt - 1 Step 1
                If isStart = True Then
                    If dgv.Item(colIndex, rowIndex).ValueType.Name = "Byte[]" Then
                        rowStr = enclosureStr & "Byte型には対応していません(unsupported)" & enclosureStr
                    Else
                        rowStr = enclosureStr & _
                                     dgv.Item(colIndex, rowIndex).Value.Replace(ControlChars.NullChar, " ") & _
                                     enclosureStr
                    End If
                    isStart = False
                Else
                    If dgv.Item(colIndex, rowIndex).ValueType.Name = "Byte[]" Then
                        rowStr = rowStr & delimiterStr & enclosureStr & "Byte型には対応していません(unsupported)" & enclosureStr
                    Else
                        rowStr = rowStr & delimiterStr & enclosureStr & dgv.Item(colIndex, rowIndex).Value & enclosureStr
                    End If
                End If
            Next colIndex
            allRows(rowIndex) = rowStr.Trim(ControlChars.NullChar, Chr(0))
            rowStr = vbNullString
            isStart = True
        Next rowIndex

        Return allRows
    End Function

    Public Shared Function DelCtrlChars(ByVal beforeStr As String) As String
        '#######################################################################
        '制御文字を取り除く
        '引数
        '    afterStr:元の文字列
        '戻り値
        '    制御文字除去後の文字列
        '#######################################################################
        Dim afterStr As String

        afterStr = beforeStr.Replace(ControlChars.CrLf, "").Replace(ControlChars.Tab, "")
        afterStr = afterStr.Replace(ControlChars.Lf, "").Replace(ControlChars.Cr, "")
        afterStr = afterStr.Replace(ControlChars.NullChar, "")

        Return afterStr
    End Function

    Public Shared Sub ShowLog()
        '#######################################################################
        'ログファイルを表示する
        '#######################################################################
        Dim filename As String
        Dim nowDateTime As DateTime = DateTime.Now

        filename = Format(nowDateTime, "yyyyMMdd") & ".txt"
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR) <> vbNullString Then
            ExecuteCmd(filename, M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR))
        Else
            ExecuteCmd(filename, System.IO.Directory.GetCurrentDirectory() & "\log")
        End If
    End Sub

    Public Shared Sub ShowLogDirectory()
        '#######################################################################
        'ログディレクトリを表示する
        '#######################################################################
        Dim filename As String
        Dim nowDateTime As DateTime = DateTime.Now

        filename = "explorer.exe"
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR) <> vbNullString Then
            ExecuteCmd(filename, "C:\WINDOWS", M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR))
        Else
            ExecuteCmd(filename, "C:\WINDOWS", System.IO.Directory.GetCurrentDirectory() & "\log")
        End If

    End Sub

    Public Shared Sub ShowHelp(ByVal formName As String)
        '#######################################################################
        'ヘルプをブラウザで表示する
        '#######################################################################
        Dim uri As String
        Dim nowDateTime As DateTime = DateTime.Now

        uri = "http://www.doppo1.net/O-Analyzer/" & formName & ".html"
        System.Diagnostics.Process.Start(uri)
    End Sub

    Public Shared Function GetMessage(ByVal messageID As String, Optional ByVal replaceStrings As String() = Nothing) As String
        '#######################################################################
        'メッセージの取得
        '#######################################################################
        Dim returnMessage As String = vbNullString
        Static lastMessage As String

        '処理時間短縮の為前回のIDと同一か確認
        If Left(lastMessage, 5) <> messageID Then
            'メッセージIDのメッセージ検索
            lastMessage = messageID
            lastMessage = lastMessage & ":" & M_CONST.C_MESSAGE_COLLECTION(messageID)
        End If

            'メッセージ変数の置換
            returnMessage = lastMessage
        If Not (replaceStrings Is Nothing) Then
            For i As Integer = 0 To replaceStrings.Length - 1
                If InStr(returnMessage, "%" & CStr(i + 1)) > 0 Then
                    returnMessage = returnMessage.Replace("%" & CStr(i + 1), replaceStrings(i))
                End If
            Next
        End If

        Return returnMessage
    End Function

    Public Function GetObjectId(ByVal name As String) As Short
        '#######################################################################
        'オブジェクトID取得
        '#######################################################################
        Dim cnt As Short
        Dim id As Short = -1

        For cnt = 0 To M_CONST.C_OBJECT.Length - 1
            If M_CONST.C_OBJECT(cnt).GetEName = name Or _
               M_CONST.C_OBJECT(cnt).GetJName = name Then
                id = M_CONST.C_OBJECT(cnt).GetID
                Exit For
            End If
        Next
        Return id
    End Function

    Public Function GetObjectEName(ByVal ID As String)
        '#######################################################################
        'オブジェクトの英語名取得
        '#######################################################################
        Dim cnt As Short
        Dim objEName As String = vbNullString

        For cnt = 0 To M_CONST.C_OBJECT.Length - 1
            If M_CONST.C_OBJECT(cnt).GetID = ID Then
                objEName = M_CONST.C_OBJECT(cnt).GetEName
                Exit For
            End If
        Next
        Return objEName
    End Function

    Public Function GetObjectJName(ByVal ID As String)
        '#######################################################################
        'オブジェクトの日本語名取得
        '#######################################################################
        Dim cnt As Short
        Dim objJName As String = vbNullString

        For cnt = 0 To M_CONST.C_OBJECT.Length - 1
            If M_CONST.C_OBJECT(cnt).GetID = ID Then
                objJName = M_CONST.C_OBJECT(cnt).GetJName
                Exit For
            End If
        Next
        Return objJName
    End Function

    Public Shared Sub SetRowheaderWidth(ByRef DGV As System.Windows.Forms.DataGridView)
        '#######################################################################
        '行ヘッダの幅調整
        '#######################################################################
        If DGV.Rows.Count >= 1000000 Then
            DGV.RowHeadersWidth = 61
        ElseIf DGV.Rows.Count >= 100000 Then
            DGV.RowHeadersWidth = 55
        ElseIf DGV.Rows.Count >= 10000 Then
            DGV.RowHeadersWidth = 49
        ElseIf DGV.Rows.Count >= 1000 Then
            DGV.RowHeadersWidth = 43
        ElseIf DGV.Rows.Count >= 100 Then
            DGV.RowHeadersWidth = 37
        ElseIf DGV.Rows.Count >= 10 Then
            DGV.RowHeadersWidth = 31
        ElseIf DGV.Rows.Count >= 1 Then
            DGV.RowHeadersWidth = 25
        ElseIf DGV.Rows.Count >= 0 Then
            Try
                DGV.RowHeadersWidth = 19
            Catch ex As Exception
                Exit Try
            End Try
        End If
    End Sub

    'Public Shared Function CheckSql(ByVal sql As String) As Integer
    '    '#######################################################################
    '    'SQL文のチェック
    '    '#######################################################################
    '    Dim sqlRows() As String = sql.Split(ControlChars.Lf)
    '    Dim tmpRow As String = ""
    '    Dim position As Integer = 0
    '    Dim isComment As Boolean = False
    '    Dim isNextRow As Boolean = False

    '    '行頭、行末の空白や制御文字の削除及び大文字への変換
    '    For i As Integer = 0 To sqlRows.Length - 1
    '        sqlRows(i) = sqlRows(i).Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
    '        sqlRows(i) = sqlRows(i).ToUpper
    '    Next

    '    For i As Integer = 0 To sqlRows.Length - 1
    '        isNextRow = False
    '        'コメントや空行ではない場合
    '        If InStr(sqlRows(i), "--") <> 1 And sqlRows(i).Length <> 0 Then
    '            'コメント中ではない場合
    '            If isComment = False Then
    '                'コメントの始まりではない場合
    '                If InStr(sqlRows(i), "/*") <> 1 Then
    '                    Return CheckSqlType(sqlRows(i))
    '                Else
    '                    'コメントの始まりの場合
    '                    If InStr(sqlRows(i), "*/") >= 3 Then
    '                        '同じ行でコメントが終了していた場合
    '                        position = InStr(sqlRows(i), "*/") + 2
    '                        tmpRow = sqlRows(i).Substring(position - 1)
    '                        tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
    '                        If InStr(tmpRow, "--") <> 1 And tmpRow.Length <> 0 Then
    '                            '「/* */」のコメントの後に「--」が続いていなかった場合
    '                            Return CheckSqlType(tmpRow)
    '                        End If
    '                    Else
    '                        isComment = True
    '                    End If
    '                End If
    '            Else
    '                If InStr(sqlRows(i), "*/") >= 1 Then
    '                    isComment = False
    '                    If isComment = 0 Then
    '                        '/* ～ */ の後にSQLが記述されていた場合
    '                        position = InStr(sqlRows(i), "*/") + 2
    '                        tmpRow = sqlRows(i).Substring(position - 1)
    '                        tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
    '                        If InStr(tmpRow, "--") <> 1 And _
    '                           tmpRow.Length <> 0 Then
    '                            Return CheckSqlType(tmpRow)
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Next
    'End Function

    Public Shared Function CheckSql(ByVal sql As String) As Integer
        '#######################################################################
        'SQL文のチェック
        '#######################################################################
        Dim sqlRows() As String = sql.Split(ControlChars.Lf)
        Dim tmpRow As String = ""
        Dim position As Integer = 0
        Dim isComment As Boolean = False
        Dim isCheckingRow As Boolean = True

        '行頭、行末の空白や制御文字の削除及び大文字への変換
        For i As Integer = 0 To sqlRows.Length - 1
            sqlRows(i) = sqlRows(i).Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
            sqlRows(i) = sqlRows(i).ToUpper
        Next

        For i As Integer = 0 To sqlRows.Length - 1
            isCheckingRow = True
            tmpRow = sqlRows(i)
            While isCheckingRow
                '空行ではない場合
                If tmpRow.Length <> 0 Then
                    'コメント中ではない場合
                    If isComment = False Then
                        'コメント中ではない場合
                        If InStr(tmpRow, "--") <> 1 Then
                            '「--」のコメントではない場合
                            If InStr(tmpRow, "/*") <> 1 Then
                                Return CheckSqlType(tmpRow)
                            Else
                                'コメントの始まりの場合
                                If InStr(tmpRow, "*/") >= 3 Then
                                    '同じ行でコメントが終了していた場合
                                    isComment = False
                                    position = InStr(tmpRow, "*/") + 2
                                    tmpRow = tmpRow.Substring(position - 1)
                                    tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
                                    '再度ループ
                                Else
                                    isComment = True
                                    isCheckingRow = False
                                    '次の行へ
                                End If
                            End If
                        Else
                            '「--」のコメントだった場合
                            isCheckingRow = False
                            '次の行へ
                        End If
                    Else
                        'コメント中の場合
                        If InStr(tmpRow, "*/") >= 1 Then
                            'コメントの終わりが見つかった場合
                            isComment = False
                            '/* ～ */ の後にSQLが記述されていた場合
                            position = InStr(tmpRow, "*/") + 2
                            tmpRow = tmpRow.Substring(position - 1)
                            tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
                            '再度ループ
                        Else
                            'コメントの終わりが行に見つからない場合
                            isCheckingRow = False
                            '次の行へ
                        End If
                    End If
                Else
                    '「--」のコメントや空行だった場合
                    isCheckingRow = False
                    '次の行へ
                End If
            End While
        Next

        MsgBox("予期せぬエラー")
        Return -1

    End Function

    Private Shared Function CheckSqlType(ByVal sqlRow As String) As Integer
        '#######################################################################
        'SQL文タイプの判定
        '#######################################################################
        If InStr(sqlRow, "SELECT") = 1 Then
            Return M_CONST.C_SQLTYPE.QUERY
        ElseIf InStr(sqlRow, "UPDATE") = 1 Or _
               InStr(sqlRow, "INSERT") = 1 Or _
               InStr(sqlRow, "DELETE") = 1 Or _
               InStr(sqlRow, "MERGE") = 1 Or _
               InStr(sqlRow, "SET TRANSACTION") = 1 Or _
               InStr(sqlRow, "SET ROLE") = 1 Then
            Return M_CONST.C_SQLTYPE.DML
        ElseIf InStr(sqlRow, "COMMIT") = 1 Or _
               InStr(sqlRow, "ROLLBACK") = 1 Or _
               InStr(sqlRow, "SAVEPOINT") = 1 Or _
               InStr(sqlRow, "ALTER") = 1 Or _
               InStr(sqlRow, "DROP") = 1 Or _
               InStr(sqlRow, "ANALYZE") = 1 Or _
               InStr(sqlRow, "PURGE") = 1 Or _
               InStr(sqlRow, "AUDIT") = 1 Or _
               InStr(sqlRow, "NOAUDIT") = 1 Or _
               InStr(sqlRow, "RENAME") = 1 Or _
               InStr(sqlRow, "COMMENT") = 1 Or _
               InStr(sqlRow, "CREATE") = 1 Or _
               InStr(sqlRow, "EXPLAIN PLAN") = 1 Or _
               InStr(sqlRow, "FLASHBACK") = 1 Or _
               InStr(sqlRow, "GRANT") = 1 Or _
               InStr(sqlRow, "REVOKE") = 1 Or _
               InStr(sqlRow, "TRUNCATE") = 1 Or _
               InStr(sqlRow, "LOCK TABLE") = 1 Or _
               InStr(sqlRow, "CALL") = 1 Or _
               InStr(sqlRow, "DECLARE") = 1 Or _
               InStr(sqlRow, "BEGIN") = 1 Or _
               InStr(sqlRow, "ASSOCIATE STATISTICS") = 1 Or _
               InStr(sqlRow, "DISASSOCIATE STATISTICS") = 1 Then
            Return M_CONST.C_SQLTYPE.DDL_DCL
        ElseIf 1 = 0 Then
            Return M_CONST.C_SQLTYPE.PROCEDURE
        Else
            Return M_CONST.C_SQLTYPE.OTHER
        End If

    End Function

    Public Shared Function AddHintToSQL(ByRef sql As String, ByVal hint As String) As Boolean
        '#######################################################################
        'SQL文へのヒント挿入
        '#######################################################################
        Dim sqlRows() As String = sql.Split(ControlChars.Lf)
        Dim tmpRow As String = ""
        Dim position As Integer = 0
        Dim isComment As Boolean = False
        Dim isCheckingRow As Boolean = True
        Dim insertRow As Integer = 0
        Dim insertPosition As Integer

        For i As Integer = 0 To sqlRows.Length - 1
            position = 0
            insertPosition = 0
            tmpRow = sqlRows(i)
            '行頭、行末の空白や制御文字の削除及び大文字への変換
            position = tmpRow.Length - tmpRow.TrimStart(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf).Length
            tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
            tmpRow = tmpRow.ToUpper
            isCheckingRow = True

            While isCheckingRow
                If tmpRow.Length <> 0 Then
                    '空行ではない場合
                    If isComment = False Then
                        'コメント中ではない場合
                        If InStr(tmpRow, "--") <> 1 Then
                            'コメント始まりではない場合
                            If InStr(tmpRow, "/*") <> 1 Then
                                isCheckingRow = False
                                insertRow = i
                                'コメントの挿入位置の確定
                                If InStr(tmpRow, "MERGE") = 1 Then
                                    position = position + 5
                                ElseIf InStr(tmpRow, "SELECT") = 1 Or _
                                       InStr(tmpRow, "UPDATE") = 1 Or _
                                       InStr(tmpRow, "INSERT") = 1 Or _
                                       InStr(tmpRow, "DELETE") = 1 Then
                                    position = position + 6
                                Else
                                    '入力されたSQLはDMLではない
                                    Return False
                                End If
                                sqlRows(insertRow) = sqlRows(insertRow).Substring(0, position) & hint & sqlRows(insertRow).Substring(position)
                                sql = vbNullString
                                For j As Integer = 0 To sqlRows.Length - 1
                                    sqlRows(j) = sqlRows(j).TrimEnd(ControlChars.Cr)
                                    If j = 0 Then
                                        sql = sqlRows(j)
                                    Else
                                        sql = sql & ControlChars.CrLf & sqlRows(j)
                                    End If
                                Next
                                Return True
                            Else
                                'コメントの始まりの場合
                                If InStr(tmpRow, "*/") >= 3 Then
                                    '同じ行でコメントが終了していた場合
                                    isComment = False
                                    'コメントが終わった次の文字から再度空白等をtrimした文字をチェックする。
                                    position = position + InStr(tmpRow, "*/") + 1
                                    tmpRow = tmpRow.Substring((InStr(tmpRow, "*/") + 1))
                                    position = position + tmpRow.Length - tmpRow.TrimStart(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf).Length
                                    tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
                                    '再度ループ
                                Else
                                    isComment = True
                                    isCheckingRow = False
                                    '次の行へ
                                End If
                            End If
                        Else
                            '「--」のコメントだった場合
                            isCheckingRow = False
                            '次の行へ
                        End If

                    Else
                        'コメント中の場合
                        If InStr(tmpRow, "*/") >= 1 Then
                            'コメントの終わりが見つかった場合
                            isComment = False
                            '/* ～ */ の後にSQLが記述されていた場合
                            position = position + InStr(tmpRow, "*/") + 1
                            tmpRow = tmpRow.Substring(position)
                            position = position + tmpRow.Length - tmpRow.TrimStart(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf).Length
                            tmpRow = tmpRow.Trim(" ", "　", ControlChars.Lf, ControlChars.Cr, ControlChars.CrLf)
                            '再度ループ
                        Else
                            'コメントの終わりが行に見つからない場合
                            isCheckingRow = False
                            '次の行へ
                        End If
                    End If
                Else
                    '空行だった場合
                    isCheckingRow = False
                    '次の行へ
                End If
            End While
        Next

        MsgBox("予期せぬエラー")
        Return False

    End Function


    Public Shared Function ValidateValue(ByVal value As String, ByVal type As Integer, Optional ByVal min As Integer = 0, _
                                         Optional ByVal max As Integer = 2147483647, Optional ByVal errchars As String() = Nothing) As String()
        '#######################################################################
        '値の検証
        '#######################################################################
        Dim i As Integer
        Dim errStr(1) As String

        If type = M_CONST.C_VALIDATE_TYPE.NUMBER Then
            'integer型チェック
            Try
                i = Integer.Parse(value)
                If i < min Then
                    errStr(0) = "E0012"
                    errStr(1) = min.ToString
                    Return errStr
                End If
                If i > max Then
                    errStr(0) = "E0013"
                    errStr(1) = max.ToString
                    Return errStr
                End If
            Catch ex As Exception
                errStr(0) = "E0016"
                Return errStr
            End Try
        ElseIf type = M_CONST.C_VALIDATE_TYPE.STRING Then
            If min <> 0 Then
                If value = vbNullString Then
                    errStr(0) = "E0014"
                    Return errStr
                End If
                If value.Length < min Then
                    errStr(0) = "E0014"
                    Return errStr
                End If
            End If

            If max <> 0 Then
                If value.Length > max Then
                    errStr(0) = "E0015"
                    Return errStr
                End If
            End If
            If Not errchars Is Nothing Then
                For i = 0 To errchars.Length - 1
                    If InStr(value, errchars(i)) <> 0 Then
                        errStr(0) = "E0017"
                        errStr(1) = errchars(i)
                        Return errStr
                    End If
                Next
            End If
        End If
        Return errStr
    End Function
    Public Shared Sub PaintDGVRownum(ByRef sender As System.Object, ByRef e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs)
        '#######################################################################
        'データグリッドビューの行ヘッダに行番号を表示する
        '#######################################################################
        Dim rect As New System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, _
                                                 sender.RowHeadersWidth - 1, sender.Rows(e.RowIndex).Height)

        System.Windows.Forms.TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), _
          sender.RowHeadersDefaultCellStyle.Font, rect, sender.RowHeadersDefaultCellStyle.ForeColor, _
          System.Windows.Forms.TextFormatFlags.VerticalCenter Or System.Windows.Forms.TextFormatFlags.Right)
    End Sub

    Public Shared Function GetAssemblyInfo() As String(,)
        '#######################################################################
        'ロード済みアセンブリ情報の取得
        '#######################################################################
        Dim assemblyInfoStr(My.Application.Info.LoadedAssemblies.Count - 1, 1) As String
        Dim i As Integer

        For i = 0 To My.Application.Info.LoadedAssemblies.Count - 1
            assemblyInfoStr(i, 0) = My.Application.Info.LoadedAssemblies(i).FullName
            assemblyInfoStr(i, 1) = My.Application.Info.LoadedAssemblies(i).Location
        Next

        Return assemblyInfoStr
    End Function

    Public Shared Function GetProcessModuleInfo() As String(,)
        '#######################################################################
        'プロセスモジュール情報の取得
        '#######################################################################
        Dim ModuleInfoStr(,) As String
        Dim currentProcess As New Process()
        Dim currentProcessModule As ProcessModule
        Dim currentProcessModuleCollection As System.Diagnostics.ProcessModuleCollection
        Dim i As Integer

        currentProcess = System.Diagnostics.Process.GetCurrentProcess
        currentProcessModuleCollection = currentProcess.Modules
        ReDim ModuleInfoStr(currentProcessModuleCollection.Count - 1, 2)

        For i = 0 To currentProcessModuleCollection.Count - 1
            currentProcessModule = currentProcessModuleCollection(i)
            ModuleInfoStr(i, 0) = currentProcessModule.FileName
            ModuleInfoStr(i, 1) = System.Diagnostics.FileVersionInfo.GetVersionInfo(currentProcessModule.FileName).FileVersion
            ModuleInfoStr(i, 2) = currentProcessModule.ModuleMemorySize
        Next i
        Return ModuleInfoStr
    End Function

    Public Shared Sub ValidateControls(ByVal controls As System.Windows.Forms.Control.ControlCollection, ByRef validationMessage As String)
        '#######################################################################
        'フォームコントロールコレクションの検証
        '#######################################################################
        Dim existingControl As System.Windows.Forms.Control

        For Each existingControl In controls
            ValidateControl(existingControl, validationMessage)
        Next
    End Sub

    Public Shared Sub ValidateControl(ByVal Control As System.Windows.Forms.Control, ByRef validationMessage As String)
        '#######################################################################
        'コントロール値の検証
        '#######################################################################
        Dim regexControl As RegExTextBox

        ' 現在のコントロールが RegExTextBox を継承する場合
        If TypeOf Control Is RegExTextBox Then
            If Control.Enabled Then
                ' コントロールのIsValid プロパティにアクセスできるようにするため existingControl 型から RegExTextBox 型にキャストする
                regexControl = CType(Control, RegExTextBox)

                ' 検証エラーの場合無効なコントロールリストにコントロールを追加する
                If Not regexControl.IsValid Then
                    validationMessage = validationMessage & regexControl.ErrorMessage & vbCrLf
                End If
            End If
        ElseIf Control.GetType Is GetType(System.Windows.Forms.TabControl) Or _
               Control.GetType Is GetType(System.Windows.Forms.TabPage) Or _
               Control.GetType Is GetType(System.Windows.Forms.SplitContainer) Or _
               Control.GetType Is GetType(System.Windows.Forms.SplitterPanel) Or _
               Control.GetType Is GetType(System.Windows.Forms.GroupBox) Or _
               Control.GetType Is GetType(System.Windows.Forms.ToolStripContainer) Or _
               Control.GetType Is GetType(System.Windows.Forms.ToolStripContentPanel) Then

            'コントロールをまとめるタイプのコントロールの場合は再帰処理
            For Each controlItem In Control.Controls
                'バージョンによって使用不能なコントロールがあるためenabled=trueのコントロールのみ処理
                ValidateControl(controlItem, validationMessage)
            Next
        End If
    End Sub

    Private Shared Sub GenerateKeyFromStr(ByVal password As String, ByVal keySize As Integer, ByRef key As Byte(), ByVal blockSize As Integer, ByRef iv As Byte())
        '#######################################################################
        '暗号化に必要な共有キーと初期化ベクタを作成する
        '#######################################################################
        Dim salt As Byte() = System.Text.Encoding.UTF8.GetBytes("NotAllowedDecompile!!")
        Dim deriveBytes As New System.Security.Cryptography.Rfc2898DeriveBytes(password, salt)

        '反復処理回数を指定する デフォルトで1000回
        deriveBytes.IterationCount = 1000

        '共有キーと初期化ベクタを生成する
        key = deriveBytes.GetBytes(keySize \ 8)
        iv = deriveBytes.GetBytes(blockSize \ 8)
    End Sub

    Public Shared Function EncryptString(ByVal sourceString As String) As String
        Dim rijndaelManaged As New System.Security.Cryptography.RijndaelManaged()
        Dim sharedKey() As Byte  '共有キー
        Dim iv() As Byte         '初期化ベクタ
        Dim sourceBytes As Byte()
        Dim encryptor As System.Security.Cryptography.ICryptoTransform
        Dim trasformedBytes As Byte()

        '警告(値が割り当てられる前に参照によって渡されています)を出さないためだけに初期化
        sharedKey = New Byte(0) {}
        iv = New Byte(0) {}

        GenerateKeyFromStr(abc, rijndaelManaged.KeySize, sharedKey, rijndaelManaged.BlockSize, iv)
        rijndaelManaged.Key = sharedKey
        rijndaelManaged.IV = iv

        '文字列をバイト型配列に変換する
        sourceBytes = System.Text.Encoding.UTF8.GetBytes(sourceString)
        encryptor = rijndaelManaged.CreateEncryptor()
        'バイト型配列を暗号化する
        trasformedBytes = encryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length)
        encryptor.Dispose()

        'バイト型配列を文字列に変換して返す
        Return System.Convert.ToBase64String(trasformedBytes)
    End Function

    Public Shared Function DecryptString(ByVal sourceString As String) As String
        Dim rijndaelManaged As New System.Security.Cryptography.RijndaelManaged()
        Dim sharedKey As Byte()
        Dim iv As Byte()
        Dim iCryptoTransform As System.Security.Cryptography.ICryptoTransform
        Dim encryptoBytes As Byte()
        Dim trasformedBytes As Byte()

        Try
            '警告(値が割り当てられる前に参照によって渡されています)を出さないためだけに初期化
            sharedKey = New Byte(0) {}
            iv = New Byte(0) {}

            'パスワードから共有キーと初期化ベクタを作成
            GenerateKeyFromStr(abc, rijndaelManaged.KeySize, sharedKey, rijndaelManaged.BlockSize, iv)
            rijndaelManaged.Key = sharedKey
            rijndaelManaged.IV = iv

            '文字列をバイト型配列に戻す
            encryptoBytes = System.Convert.FromBase64String(sourceString)

            '対称暗号化オブジェクトの作成
            iCryptoTransform = rijndaelManaged.CreateDecryptor()
            'バイト型配列を復号化する
            '復号化に失敗すると例外CryptographicExceptionが発生
            trasformedBytes = iCryptoTransform.TransformFinalBlock(encryptoBytes, 0, encryptoBytes.Length)
            iCryptoTransform.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
            trasformedBytes = Nothing
            Return vbNullString
        End Try

        'バイト型配列を文字列に戻して返す
        Return System.Text.Encoding.UTF8.GetString(trasformedBytes)
    End Function

    Public Shared Sub SetProcessEnvironment(ByVal envName As String, ByVal envValue As String, ByVal envTarget As EnvironmentVariableTarget)
        '環境変数の変更
        Try
            System.Environment.SetEnvironmentVariable(envName, envValue, envTarget)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Shared Function CheckInitParam(ByVal initParamRowStr As String) As String
        '初期化パラメータ値のチェック設定
        Dim i As Integer
        Dim rowStr(2) As String
        Dim errMsg As String = vbNullString
        Dim errStrs(1) As String
        Dim replaceStr(0) As String

        'パラメータの形式："paramname,description,paramvalue"
        '行数文の配列で定義
        rowStr = initParamRowStr.Split(",")
        If rowStr.Length = 3 Then
            For i = 0 To M_CONST.C_INITPARAM.Length - 1
                If M_CONST.C_INITPARAM(i).GET_NAME = rowStr(0) Then

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_APLOG Then
                        'INIT_APLOGのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 0, 1)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_ERRLOG Then
                        'INIT_ERRLOGのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 0, 1)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_FETCH Then
                        'INIT_FETCHのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 1)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_ENCODE Then
                        'INIT_ENCODEのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 0, 2)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_DELIMITER Then
                        'INIT_DELIMITERのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 0, 1)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_LOGDIR Then
                        'INIT_LOGDIRパラメータのチェック
                        If rowStr(2) = vbNullString Then
                            rowStr(2) = System.IO.Directory.GetCurrentDirectory() & "\log"
                            If My.Computer.FileSystem.DirectoryExists(rowStr(2)) = False Then
                                'デフォルトログディレクトリの作成
                                Try
                                    My.Computer.FileSystem.CreateDirectory(rowStr(2))
                                Catch ex As Exception
                                    replaceStr(0) = rowStr(2)
                                    errMsg = errMsg & GetMessage("E0018", replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                                End Try
                            End If
                        End If
                        If My.Computer.FileSystem.DirectoryExists(rowStr(2)) = False Then
                            replaceStr(0) = rowStr(2)
                            errMsg = errMsg & GetMessage("E0019", replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_FETCH_REFLESH Then
                        'INIT_FETCH_REFLESHのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 1)
                        If errStrs(0) <> vbNullString Then
                            errMsg = errMsg & GetMessage(errStrs(0)) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_ODP_FETCHSIZE Then
                        'INIT_ODP_FETCHSIZEのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 1024, )
                        If errStrs(0) <> vbNullString Then
                            errMsg = errMsg & GetMessage(errStrs(0)) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_ODP_LONGFETCHSIZE Then
                        'INIT_ODP_LONGFETCHSIZEのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 1024, )
                        If errStrs(0) <> vbNullString Then
                            errMsg = errMsg & GetMessage(errStrs(0)) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_NLS_LANG Then
                        'INIT_NLS_LANGのチェック
                        'チェックなし(誤った指定をした場合は接続できない)
                    End If

                    If M_CONST.C_INITPARAM(i).GET_NAME = M_CONST.C_INITPARAM_INIT_ONLINE Then
                        'INIT_ONLINEのチェック
                        errStrs = ValidateValue(rowStr(2), M_CONST.C_VALIDATE_TYPE.NUMBER, 0, 1)
                        If errStrs(0) <> vbNullString Then
                            replaceStr(0) = errStrs(1)
                            errMsg = errMsg & GetMessage(errStrs(0), replaceStr) & " パラメータ:" & rowStr(0) & " データ:" & rowStr(2) & vbCrLf
                        End If

                    End If

                    Exit For
                End If
            Next
        ElseIf rowStr.Length = 1 Then
            If rowStr(0).Replace(" ", vbNullString).Replace(vbLf, vbNullString).Replace("　", vbNullString).ToString <> vbNullString Then
                '全角半角の空白やlfの文字コードのみ以外の行でカンマ区切りのデータが足りない場合
                errMsg = errMsg & "不正な行が見つかりました。処理を終了します:" & vbCrLf & "データ:" & initParamRowStr
            End If
        Else
            'カンマ区切りのデータ数が足りない場合
            errMsg = errMsg & "不正な行が見つかりました。処理を終了します:" & vbCrLf & "データ:" & initParamRowStr
        End If
        Return errMsg

    End Function

    Public Shared Sub disposeControls(ByVal controls As System.Windows.Forms.Control.ControlCollection)
        For Each control As System.Windows.Forms.Control In controls
            If control.GetType Is GetType(System.Windows.Forms.TabControl) Or _
               control.GetType Is GetType(System.Windows.Forms.TabPage) Or _
               control.GetType Is GetType(System.Windows.Forms.SplitContainer) Or _
               control.GetType Is GetType(System.Windows.Forms.SplitterPanel) Or _
               control.GetType Is GetType(System.Windows.Forms.GroupBox) Or _
               control.GetType Is GetType(System.Windows.Forms.ToolStripContainer) Or _
               control.GetType Is GetType(System.Windows.Forms.ToolStripContentPanel) Then
                'コントロールをまとめるタイプのコントロールの場合は再帰処理
                For Each controlItem In control.Controls
                    'バージョンによって使用不能なコントロールがあるためenabled=trueのコントロールのみ処理
                    disposeControls(control.Controls)
                Next
            End If
            disposeControl(control)
        Next
    End Sub
    Public Shared Sub disposeControl(ByVal control As System.Windows.Forms.Control)
        control.Dispose()
        control = Nothing
    End Sub

End Class
