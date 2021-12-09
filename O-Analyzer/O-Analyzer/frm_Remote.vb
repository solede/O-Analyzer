Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE
Imports System.IO

Public Class frm_Remote
    '#######################################################################
    'DBサーバリモート操作画面
    '#######################################################################

    Private bdumpDest As String
    Private alertlogName As String = vbNullString
    Private os As String = vbNullString
    Private M_DATA_TABLE As DataTable

    Private Sub CreateJavaProcedure()
        Dim sql As String = vbNullString
        Dim msgStr As String = vbNullString
        Dim isSuccess As Boolean = True

        '★この機能はjava stored procedure を使用するのでJVMがインストールされている必要があります！★
        'うまく動作しない場合はsqlplusでも確認します。

        'set serveroutput on size 1000000
        'call dbms_java.set_output(1000000);
        'declare
        '  command VARCHAR2(1000);
        '  returncd number;
        'begin
        '  command := 'C:\WINDOWS\system32\findstr.exe ORA- j:\program\oracle\diag\rdbms\orcl112\orcl112\trace\alert_orcl112.log';
        '  returncd := exec_os_command(command,'UTF-8');
        '  dbms_output.put_line('RETURN_CODE=' || returncd);
        'end;
        '/

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        'このソースだとJAVA上でExceptionがスローされた場合に子プロセスが終了しない。。えぃ
        sql = sql & "create or replace and compile java source named " & TXT_JAVA_PROC.Text & " as" & ControlChars.CrLf
        sql = sql & "import java.io.*;" & ControlChars.CrLf
        sql = sql & "public final class " & TXT_JAVA_PROC.Text & " {" & ControlChars.CrLf
        sql = sql & " public static int execCommand(String command, String encode) {" & ControlChars.CrLf
        sql = sql & "		int ret = 0;" & ControlChars.CrLf
        sql = sql & "		Runtime runtime = null;" & ControlChars.CrLf
        sql = sql & "		Process proc = null;" & ControlChars.CrLf
        sql = sql & "		java.io.InputStream outIs = null;" & ControlChars.CrLf
        sql = sql & "		InputStreamReader outIsr = null;" & ControlChars.CrLf
        sql = sql & "		BufferedReader  outBr = null;" & ControlChars.CrLf
        sql = sql & "		java.io.InputStream errIs = null;" & ControlChars.CrLf
        sql = sql & "		InputStreamReader errIsr = null;" & ControlChars.CrLf
        sql = sql & "		BufferedReader  errBr = null;" & ControlChars.CrLf
        sql = sql & "" & ControlChars.CrLf
        sql = sql & "		try {" & ControlChars.CrLf
        sql = sql & "			String line = new String();" & ControlChars.CrLf
        sql = sql & "			runtime = Runtime.getRuntime();" & ControlChars.CrLf
        sql = sql & "			proc = runtime.exec(command);" & ControlChars.CrLf
        sql = sql & "			//stdOut" & ControlChars.CrLf
        sql = sql & "			outIs = proc.getInputStream();" & ControlChars.CrLf
        sql = sql & "			outIsr = new InputStreamReader(outIs, encode);" & ControlChars.CrLf
        sql = sql & "			outBr = new BufferedReader(outIsr);" & ControlChars.CrLf
        sql = sql & "			while( (line = outBr.readLine()) != null ){" & ControlChars.CrLf
        sql = sql & "				System.out.println(line);" & ControlChars.CrLf
        sql = sql & "			};" & ControlChars.CrLf
        sql = sql & "			//stdErr" & ControlChars.CrLf
        sql = sql & "			errIs = proc.getErrorStream();" & ControlChars.CrLf
        sql = sql & "			errIsr = new InputStreamReader(errIs, encode);" & ControlChars.CrLf
        sql = sql & "			errBr = new BufferedReader(errIsr);" & ControlChars.CrLf
        sql = sql & "			while( (line = errBr.readLine()) != null ){" & ControlChars.CrLf
        sql = sql & "				if (ret == 0) {" & ControlChars.CrLf
        sql = sql & "					ret = 1;" & ControlChars.CrLf
        sql = sql & "					System.out.println(""stdError:"");" & ControlChars.CrLf
        sql = sql & "				}" & ControlChars.CrLf
        sql = sql & "				System.out.println(line);" & ControlChars.CrLf
        sql = sql & "			};" & ControlChars.CrLf
        sql = sql & "			proc.waitFor();" & ControlChars.CrLf
        sql = sql & "			proc.destroy();" & ControlChars.CrLf
        sql = sql & "			return ret;" & ControlChars.CrLf
        sql = sql & "		} catch(Exception e) {" & ControlChars.CrLf
        sql = sql & "			ret = -1;" & ControlChars.CrLf
        sql = sql & "			System.out.println(""!!!Exception Occured!!!"");" & ControlChars.CrLf
        sql = sql & "			System.out.println(""StackTrace:"");" & ControlChars.CrLf
        sql = sql & "			e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			System.out.println(""ErrorMessage:"");" & ControlChars.CrLf
        sql = sql & "			System.out.println(e.getMessage());" & ControlChars.CrLf
        sql = sql & "			return ret;" & ControlChars.CrLf
        sql = sql & "		}finally{" & ControlChars.CrLf
        sql = sql & "			//close stdout stream" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( outBr != null ){outBr.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( outIsr != null ){outIsr.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( outIs != null ){outIs.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			" & ControlChars.CrLf
        sql = sql & "			//close stderr stream" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( errBr != null ){errBr.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( errIsr != null ){errIsr.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			try" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				if( errIs != null ){errIs.close();}" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			catch( IOException e )" & ControlChars.CrLf
        sql = sql & "			{" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "			//process destroy" & ControlChars.CrLf
        sql = sql & "			try{" & ControlChars.CrLf
        sql = sql & "				if( proc != null ){proc.destroy();}" & ControlChars.CrLf
        sql = sql & "			}catch (Exception e ){" & ControlChars.CrLf
        sql = sql & "				e.printStackTrace();" & ControlChars.CrLf
        sql = sql & "			}" & ControlChars.CrLf
        sql = sql & "		}" & ControlChars.CrLf
        sql = sql & "	}" & ControlChars.CrLf
        sql = sql & "}" & ControlChars.CrLf

        If CreateProcedure(sql) Then
            'リターンコードは正常の場合でもコンパイルがエラーになっている可能性があるためエラーを確認
            sql = "SELECT TEXT "
            sql = sql & "FROM USER_ERRORS "
            sql = sql & "WHERE NAME = UPPER('" & TXT_JAVA_PROC.Text & "') AND TYPE = 'JAVA SOURCE' "
            sql = sql & "ORDER BY SEQUENCE"

            msgStr = getError(sql)
            If msgStr <> vbNullString Then
                MsgBox(msgStr)
                isSuccess = False
                LBL_INFO.Text = M_COMMON.GetMessage("E0001")
            End If
        Else
            isSuccess = False
            LBL_INFO.Text = M_COMMON.GetMessage("E0001")
        End If

        If isSuccess Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0030")
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub CreatePLSQLProcedure()
        Dim sql As String
        Dim msgStr As String = vbNullString
        Dim isSuccess As Boolean = True

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = "CREATE OR REPLACE FUNCTION " & TXT_PLSQL_PROC.Text & "(COMMAND VARCHAR2,ENCODE VARCHAR2) RETURN NUMBER " & ControlChars.CrLf
        sql = sql & "AS LANGUAGE JAVA NAME '" & TXT_JAVA_PROC.Text & ".execCommand(java.lang.String,java.lang.String) return int';"

        If CreateProcedure(sql) Then
            'リターンコードは正常の場合でもコンパイルがエラーになっている可能性があるためエラーを確認
            sql = "SELECT TEXT "
            sql = sql & "FROM USER_ERRORS "
            sql = sql & "WHERE NAME =UPPER('" & TXT_PLSQL_PROC.Text & "') AND TYPE = 'FUNCTION'"
            sql = sql & "ORDER BY SEQUENCE"

            msgStr = getError(sql)
            If msgStr <> vbNullString Then
                MsgBox(msgStr)
                isSuccess = False
                LBL_INFO.Text = M_COMMON.GetMessage("E0001")
            End If
        Else
            isSuccess = False
            LBL_INFO.Text = M_COMMON.GetMessage("E0001")
        End If

        If isSuccess Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0030")
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Function getError(ByVal sql As String) As String
        Dim datarow As DataRow
        Dim msgStr As String = vbNullString

        Using dataset As New DataSet
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then
                For Each datarow In dataset.Tables(0).Rows
                    If msgStr = vbNullString Then
                        msgStr = msgStr & datarow.Item(0).ToString
                    Else
                        msgStr = msgStr & vbCrLf & datarow.Item(0).ToString
                    End If
                Next
            Else
                msgStr = "予期せぬエラー"
            End If
        End Using

        Return msgStr

    End Function

    Private Sub DispJavaPermission()
        '#######################################################################
        'JAVAセキュリティポリシー表示
        '#######################################################################
        Dim sql As String

        sql = "SELECT * "
        sql = sql & "FROM DBA_JAVA_POLICY"

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_JAVA_PERMISSION, False)
            LBL_INFO.Text = M_COMMON.GetMessage("I0030")
        Else
            LBL_INFO.Text = vbNullString
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub FetchFromResultset(ByRef dgv As DataGridView, ByVal dispInfoLabel As Boolean, Optional ByVal fetchCnt As Long = 10000000000)
        '#######################################################################
        ''SELECT結果FETCH
        '#######################################################################
        Dim msgstr(0) As String

        M_DATA_TABLE = New DataTable

        'FETCH+表示
        If frm_Login.G_DB.FETCH_TO_DATATABLE(M_DATA_TABLE, fetchCnt) = False Then
            Exit Sub
        End If

        If frm_Login.G_DB.GET_SELECT_FLG = False Then
            dgv.AllowUserToAddRows = False
        End If
        dgv.DataSource = M_DATA_TABLE
        If dispInfoLabel Then
            msgstr(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgstr)
        End If

    End Sub

    Private Sub ExecOsCommand()
        Dim returnStr As String = vbNullString
        Dim cnt As Integer = 0
        Dim msg(0) As String
        Dim retCd As Integer

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        '最大メモリサイズの指定(メモリを食いすぎるのでコメント化)
        'If frm_Login.G_DB.GET_DATASET("SELECT NVL(setMaxMemorySize(33554432),0) FROM DUAL", dataset) = False Then
        '    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        '    LBL_INFO.Text = vbNullString
        '    Exit Sub
        'End If

        retCd = ExecJavaProcedure(TXT_EXEC_CMD.Text, returnStr)
        If retCd = 0 Then

        ElseIf retCd = 2 Then
            MsgBox("PL/SQLプロシージャの実行中にエラーが発生しました")
        ElseIf retCd = 3 Then
            MsgBox("バインドに失敗しました")
        ElseIf retCd = 4 Then
            MsgBox("標準出力、標準エラーの出力に失敗しました")
        ElseIf retCd = 5 Then
            MsgBox("予期せぬエラーが発生しました")
        End If

        If returnStr <> vbNullString Then
            msg(0) = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(returnStr)
            'NULL文字を空白に置換、lfをcrlfに置換
            returnStr = returnStr.Replace(ControlChars.CrLf, ControlChars.Lf).Replace(ControlChars.Lf, ControlChars.CrLf).Replace(ControlChars.NullChar, " ")
            TXT_RESULT_LOG.Text = returnStr
            'msg(0) = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(returnStr) & "(置換前：" & msg(0) & ")"
            LBL_INFO.Text = M_COMMON.GetMessage("I0032", msg)
        Else
            LBL_INFO.Text = M_COMMON.GetMessage("I0030", msg)
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Function CreateProcedure(ByVal DDL As String) As Boolean
        Dim retCnt As Long

        If frm_Login.G_DB.EXEC_NONQUERY(DDL, retCnt) = False Then
            Return False
        End If

        Return True
    End Function

    Private Function ExecJavaProcedure(ByVal command As String, ByRef retStr As String) As Integer
        Dim msgVal(0) As String
        Dim returnStr(0) As String
        Dim bindname As String
        Dim paramName As String
        Dim type_number As Short
        Dim inOut As ParameterDirection
        Dim size As Integer
        Dim sql As String = vbNullString
        Dim bindReturncd(0) As M_CONST.S_BIND
        Dim retCd As Integer = 0
        Dim stdOutStr(1) As String
        Dim retCnt As Long

        TXT_RESULT_LOG.Text = vbNullString
        Me.Refresh()

        'バインド
        bindname = ":returnCode"
        paramName = vbNullString
        type_number = M_CONST.C_COLUMN_TYPE.NUMBER
        inOut = ParameterDirection.Output
        size = 32767 'LONG型の戻り値は32Kまで
        bindReturncd(0).SET_BIND(bindname, paramName, type_number, inOut, size)
        If frm_Login.G_DB.SET_BIND_VARIABLE(bindReturncd) = False Then
            Return 3
        End If

        'プロシージャ実行
        sql = "begin "
        sql = sql & "DBMS_JAVA.SET_OUTPUT(10000000); "
        sql = sql & "DBMS_OUTPUT.ENABLE(NULL); "
        sql = sql & ":returnCode := (" & TXT_JAVA_PROC2.Text & "('" & command & "','" & CMB_ENCODE.SelectedItem.ToString & "')); "
        sql = sql & "end;"

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt, returnStr) Then
            retCd = Integer.Parse(returnStr(0))
            If retCd = -1 Then
                MsgBox("JAVAストアドプロシージャ実行中に例外が発生しました")
            ElseIf retCd = 1 Then
                MsgBox("OSコマンドプロセスから標準エラー出力を受け取りました")
            End If

            LBL_INFO.Text = M_COMMON.GetMessage("I0034")
        Else
            Return 2
        End If

        retCd = DispStdOutErr(retStr)

        Return retCd
    End Function

    Private Function DispStdOutErr(ByRef retStr As String) As Integer
        Dim retCnt As Long
        Dim returnStr(0) As String
        Dim returnStr2 As New System.Text.StringBuilder
        Dim bindname As String
        Dim paramValue As String
        Dim paramType As Short
        Dim inOut As ParameterDirection
        Dim size As Integer
        Dim sql As String = vbNullString
        Dim bindReturncd(0) As M_CONST.S_BIND
        Dim bindStdOut(1) As M_CONST.S_BIND
        Dim retCd As Integer = 0
        Dim stdOutStr(1) As String

        TXT_RESULT_LOG.Text = vbNullString
        Me.Refresh()

        Do While True
            If frm_Login.G_DB.GET_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                'ODP.netの場合はバインド配列を使用することができる
                'DBMS_OUTPUT.GET_LINESを使用して複数行取得する場合
                'バインド
                Dim arrayBindStdOut(0) As M_CONST.S_BIND_ARR
                Dim bindReturnRowCnt(0) As M_CONST.S_BIND
                Dim arrayCnt As Integer = 100
                Dim bindArraySize(arrayCnt - 1) As Integer
                Dim paramValues(arrayCnt - 1) As String
                Dim retRowCnt As Integer = 0

                For i As Integer = 0 To arrayCnt - 1
                    bindArraySize(i) = 32767
                Next

                '初期化
                stdOutStr = {"", ""}

                bindname = ":LINES"
                paramValue = vbNullString
                paramType = M_CONST.C_COLUMN_TYPE.STRING
                inOut = ParameterDirection.Output
                arrayBindStdOut(0).SET_BIND(bindname, arrayCnt, paramValues, paramType, inOut, bindArraySize)

                bindname = ":NUMLINES"
                paramValue = arrayCnt.ToString   '戻り値以上の桁数でバインドしないとデータが戻らないため
                paramType = M_CONST.C_COLUMN_TYPE.NUMBER
                inOut = ParameterDirection.InputOutput
                size = 32767
                bindReturnRowCnt(0).SET_BIND(bindname, paramValue, paramType, inOut, size)

                If frm_Login.G_DB.SET_BIND_ARRAYVARIABLE(arrayBindStdOut, bindReturnRowCnt) = False Then
                    'バインド失敗
                    Return 3
                End If

                sql = "BEGIN "
                sql = sql & "DBMS_OUTPUT.GET_LINES(:LINES,:NUMLINES); "
                sql = sql & "END;"
                If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt, stdOutStr) Then
                    'LBL_INFO.Text = M_COMMON.GetMessage("I0010", msgVal)
                    returnStr2.Append(stdOutStr(0))

                    If Integer.Parse(stdOutStr(1)) < arrayCnt Then
                        retStr = returnStr2.ToString
                        Return retCd
                    End If
                Else
                    Return 4
                End If
            Else
                'ODP.net以外の場合はバインド配列が使用できないため1行づつ取り出す
                'バインド
                bindname = ":LINE"
                paramValue = vbNullString
                paramType = M_CONST.C_COLUMN_TYPE.STRING
                inOut = ParameterDirection.InputOutput '本来LINEの定義はoutputだがinputOutputで定義しないと「ORA-06502: PL/SQL: 数値または値のエラー: 文字から数値への変換エラー」が発生するためinputOutputで定義
                size = 32764
                bindStdOut(0).SET_BIND(bindname, paramValue, paramType, inOut, size)

                bindname = ":STATUS"
                paramValue = vbNullString
                paramType = M_CONST.C_COLUMN_TYPE.NUMBER
                inOut = ParameterDirection.Output
                size = 3
                bindStdOut(1).SET_BIND(bindname, paramValue, paramType, inOut, size)
                If frm_Login.G_DB.SET_BIND_VARIABLE(bindStdOut) = False Then
                    'バインド失敗
                    Return 3
                End If

                sql = "BEGIN "
                sql = sql & "DBMS_OUTPUT.GET_LINE(:LINE,:STATUS); "
                sql = sql & "END;"
                If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt, stdOutStr) Then
                    'LBL_INFO.Text = M_COMMON.GetMessage("I0010", msgVal)
                    If stdOutStr(1) = "1" Then
                        'バッファにこれ以上行がない場合
                        returnStr2.Append(stdOutStr(0))
                        retStr = returnStr2.ToString
                        Return retCd
                    Else
                        '続きのバッファがある場合
                        returnStr2.Append(stdOutStr(0)).Append(ControlChars.CrLf)
                    End If
                Else
                    Return 4
                End If

            End If

        Loop

        Return retCd
    End Function

    Private Sub SetViewAlertlogCommand()
        If alertlogName = vbNullString Then
            '初回のみアラートログのパスを取得する
            GetAlertlogPath()
        End If

        If os = vbNullString Then
            '初回のみOSがWINDOWSであるかどうか判別する
            GetServerOs()
        End If

        If bdumpDest <> vbNullString And alertlogName <> vbNullString And os <> vbNullString Then
            If InStr(os, "WINDOWS") > 0 Then
                TXT_EXEC_CMD.Text = "C:\WINDOWS\system32\cmd.exe /c TYPE " & bdumpDest & "\" & alertlogName
            Else
                TXT_EXEC_CMD.Text = "/usr/bin/tail -100 " & bdumpDest & "/" & alertlogName
            End If
        End If
    End Sub

    Private Sub GetAlertlogPath()
        'アラートログのパスを取得する

        'Dim dataset As New DataSet()
        Dim sid As String

        Using DataSet As New DataSet
            If frm_Login.G_DB.GET_DATASET("SELECT VALUE FROM V$PARAMETER WHERE UPPER(NAME) = 'BACKGROUND_DUMP_DEST'", DataSet) = False Then
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                LBL_INFO.Text = vbNullString
                Exit Sub
            End If
            bdumpDest = DataSet.Tables(0).Rows(0).Item(0)
        End Using

        Using DataSet As New DataSet
            'SIDの取得
            If frm_Login.G_DB.GET_DATASET("SELECT INSTANCE_NAME FROM V$INSTANCE", DataSet) = False Then
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                LBL_INFO.Text = vbNullString
                Exit Sub
            End If
            sid = DataSet.Tables(0).Rows(0).Item(0)
        End Using

        If frm_Login.G_DB.G_ORA_VERSION(1) >= 9 Then
            alertlogName = "alert_" & sid & ".log"
        Else
            alertlogName = sid & "ALRT.LOG"
        End If

    End Sub

    Private Sub SetViewOraErrorCommand()
        'アラートログからORA-エラーをgrepした結果を表示する
        If alertlogName = vbNullString Then
            '初回のみアラートログのパスを取得する
            GetAlertlogPath()
        End If

        If os = vbNullString Then
            '初回のみOSがWINDOWSであるかどうか判別する
            GetServerOs()
        End If

        If InStr(os, "WINDOWS") > 0 Then
            TXT_EXEC_CMD.Text = "C:\WINDOWS\system32\findstr.exe ORA- " & bdumpDest & "\" & alertlogName
        Else
            TXT_EXEC_CMD.Text = "/bin/grep -2  ORA- " & bdumpDest & "/" & alertlogName
        End If

    End Sub

    Private Sub GetServerOs()
        'サーバーOSがWindowsであるか判別する

        Using dataset As New DataSet()
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                If frm_Login.G_DB.GET_DATASET("SELECT UPPER(PLATFORM_NAME) FROM V$DATABASE", dataset) = False Then
                    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                    LBL_INFO.Text = vbNullString
                    Exit Sub
                End If
                os = dataset.Tables(0).Rows(0).Item(0)
            ElseIf Int(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Or Int(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 Then
                If frm_Login.G_DB.GET_DATASET("SELECT SUBSTR(NAME,1,1) FROM V$DATAFILE WHERE ROWNUM=1", dataset) = False Then
                    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                    LBL_INFO.Text = vbNullString
                    Exit Sub
                End If
                If dataset.Tables(0).Rows(0).Item(0) <> "/" Then
                    os = "WINDOWS"
                Else
                    os = "OTHER"
                End If
            End If
        End Using

    End Sub

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXECUTE.Enabled = True

        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXECUTE.Enabled = False
            LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        End If
    End Sub

    Private Sub BTN_EXECUTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXECUTE.Click
        ExecOsCommand()
    End Sub

    Private Sub BTN_BIND_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frm_Bind.Show()
    End Sub

    Private Sub frm_Remote_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LBL_INFO.Text = vbNullString
        CMB_EXEC_PROCESS.SelectedIndex = 0
        CMB_ENCODE.SelectedIndex = 0
        TabControl1.SelectedTab = TAB_EXECUTE
        Me.Width = 800
        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
        Me.Text = "O-Analyzer - リモート操作(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & _
                  ":" & Process.GetCurrentProcess().Id() & ")"

    End Sub

    Private Sub frm_Remote_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If Not (M_DATA_TABLE Is Nothing) Then
            M_DATA_TABLE.Dispose()
            M_DATA_TABLE = Nothing
        End If

        '画面情報保存
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        Me.Dispose()
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub CMB_EXEC_PROCESS_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMB_EXEC_PROCESS.SelectedIndexChanged
        If CMB_EXEC_PROCESS.SelectedItem.ToString.Substring(0, 1) = "1" Then
            SetViewAlertlogCommand()
        ElseIf CMB_EXEC_PROCESS.SelectedItem.ToString.Substring(0, 1) = "2" Then
            SetViewOraErrorCommand()
        End If
    End Sub

    Private Sub frm_Remote_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50

        TabControl1.Width = GroupBox1.Location.X - 10
        TabControl1.Height = Me.Height - LBL_INFO.Height - 50

        TAB_PREPARE.Width = TabControl1.Width - 10
        TAB_EXECUTE.Width = TAB_PREPARE.Width
        TAB_PREPARE.Height = TabControl1.Height - 50
        TAB_EXECUTE.Height = TAB_PREPARE.Height

        '準備画面
        GroupBox2.Width = TAB_PREPARE.Width - 10
        GroupBox3.Width = TAB_PREPARE.Width - 10
        GroupBox4.Width = TAB_PREPARE.Width - 10
        GroupBox4.Height = TAB_PREPARE.Height - GroupBox4.Location.Y
        TXT_MEMO.Width = GroupBox4.Width - 10
        DGV_JAVA_PERMISSION.Width = GroupBox4.Width - 10
        DGV_JAVA_PERMISSION.Height = GroupBox4.Height - DGV_JAVA_PERMISSION.Location.Y - 5


        '結果画面
        TXT_RESULT_LOG.Width = TAB_EXECUTE.Width - 10
        TXT_RESULT_LOG.Height = TAB_EXECUTE.Height - TXT_RESULT_LOG.Location.Y - 5
        LBL_INFO.Location = New Point(5, Me.Height - 55)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        CreateJavaProcedure()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        CreatePLSQLProcedure()
    End Sub

    Private Sub BTN_PERMISSION_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_PERMISSION.Click
        DispJavaPermission()
    End Sub

    Private Sub DGV_JAVA_PERMISSION_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_JAVA_PERMISSION.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class