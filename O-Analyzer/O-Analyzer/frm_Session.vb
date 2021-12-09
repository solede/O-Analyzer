Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Session
    '#######################################################################
    'セッション画面
    '#######################################################################

    '定数
    Private Const C_SESSION As Integer = 0
    Private Const C_SQL As Integer = 1
    Private Const C_SESSION_WAIT As Integer = 2
    Private Const C_SYSSTAT As Integer = 3
    Private Const C_PROCESS As Integer = 4
    Private Const C_LATCH As Integer = 5
    Private Const C_LOCK As Integer = 6
    Private Const C_CURSOR As Integer = 7
    Private Const C_WAIT_CLASS As Integer = 8
    Private Const C_TIME_MODEL As Integer = 9

    'モジュール変数
    Private M_SELECT_FLG As Boolean
    Private M_APP_SID As Integer
    Private M_LAST_CELL_ROW As Integer

    Private Sub DispSession()
        '#######################################################################
        'セッションデータ表示
        '#######################################################################
        Dim sql As String

        LBL_INFO.Text = vbNullString
        sql = CreateSQL(C_SESSION)
        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_SESSION, True)
            For i As Integer = 0 To DGV_SESSION.Rows.Count - 1
                'カレントセッションのみ色付け
                If DGV_SESSION.Rows(i).Cells(DGV_SESSION.Columns("SID").Index).Value = M_APP_SID Then
                    DGV_SESSION.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan
                Else
                    DGV_SESSION.Rows(i).DefaultCellStyle.BackColor = Color.White
                End If

                '長時間待機セッションの色付け
                '待機ベントがアイドル待機イベントかどうかを判断しないといけないので難しいかもしれない。
                'If Integer.Parse(DGV_SESSION.Rows(i).Cells(DGV_SESSION.Columns("SECONDS_IN_WAIT").Index).Value) >= 3 And _
                '    DGV_SESSION.Rows(i).Cells(DGV_SESSION.Columns("STATUS").Index).Value = "ACTIVE" Then
                '    'DGV_SESSION.Rows(i).Cells(DGV_SESSION.Columns("TYPE").Index).Value = "USER" Then

                '    'SQL実行中かつ特定のオペレーションで3秒以上待機している場合
                '    DGV_SESSION.Rows(i).DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow
                'End If

            Next
            '列幅調整
            DGV_SESSION.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        End If
    End Sub

    Private Sub DispSQLInfo()
        '#######################################################################
        '実行中SQL表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_SQL)
        If sql = vbNullString Then
            'SQL_IDやSQL_HASHがNULLだった場合
            Exit Sub
        End If
        Using DataSet As New DataSet
            'SQL実行
            If frm_Login.G_DB.GET_DATASET(sql, DataSet) Then
                Me.Refresh()
                '表示
                If DataSet.Tables(0).Rows.Count > 0 Then
                    For Each row As Object In DataSet.Tables(0).Rows
                        TXT_EXECUTING_SQL.Text = TXT_EXECUTING_SQL.Text & row.Item(0)
                    Next
                End If
            End If
        End Using
    End Sub

    Private Sub DispWaitEvent()
        '#######################################################################
        'セッション待機イベント表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_SESSION_WAIT)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_WAIT_EVENT, False)
        End If
    End Sub

    Private Sub DispSessStat()
        '#######################################################################
        'システム統計表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_SYSSTAT)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_SYSSTAT, False)
        End If
    End Sub

    Private Sub DispProcess()
        '#######################################################################
        'プロセス表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_PROCESS)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_PROCESS, False)
        End If
    End Sub

    Private Sub DispLatch()
        '#######################################################################
        '獲得ラッチ表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_LATCH)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_LATCH, False)
        End If
    End Sub

    Private Sub DispLock()
        '#######################################################################
        'ロック情報表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_LOCK)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_LOCK, False)
        End If
    End Sub

    Private Sub DispCursor()
        '#######################################################################
        'カーソル情報表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_CURSOR)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_CURSOR, False)
        End If
    End Sub

    Private Sub DispTimeModel()
        '#######################################################################
        '時刻統計表示
        '#######################################################################
        Dim sql As String

        sql = CreateSQL(C_TIME_MODEL)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_TIME_MODEL, False)
        End If
    End Sub

    Private Function CreateSQL(ByVal id As Integer) As String
        '#######################################################################
        'SQL文作成
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind() As M_CONST.S_BIND
        Dim isStart As Boolean = True

        If id = C_SESSION Then
            'セッション情報取得
            If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or (Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '10.2以降
                sql = "SELECT SID,SERIAL#,STATUS,LAST_CALL_ET,SCHEMANAME,USERNAME,PROGRAM,SERVER,OSUSER,MACHINE,LOGON_TIME, "
                sql = sql & "SQL_ID,PREV_SQL_ID,BLOCKING_SESSION_STATUS,BLOCKING_INSTANCE,BLOCKING_SESSION,EVENT, "
                sql = sql & "P1,P1TEXT,P2,P2TEXT,P3,P3TEXT,SEQ#,STATE,WAIT_TIME,SECONDS_IN_WAIT "
                sql = sql & "FROM V$SESSION A "
            ElseIf Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 1 Then
                '10.1
                sql = "SELECT SID,SERIAL#,STATUS,LAST_CALL_ET,SCHEMANAME,USERNAME,PROGRAM,SERVER,OSUSER,MACHINE,LOGON_TIME, "
                sql = sql & "SQL_ID,PREV_SQL_ID,BLOCKING_SESSION_STATUS,BLOCKING_SESSION,EVENT, "
                sql = sql & "P1,P1TEXT,P2,P2TEXT,P3,P3TEXT,SEQ#,STATE,WAIT_TIME,SECONDS_IN_WAIT "
                sql = sql & "FROM V$SESSION A "
            Else
                '9iまで
                sql = "SELECT A.SID,SERIAL#,STATUS,LAST_CALL_ET,SCHEMANAME,USERNAME,PROGRAM,SERVER,OSUSER,MACHINE, "
                sql = sql & "RAWTOHEX(SQL_ADDRESS) SQL_ADDRESS,SQL_HASH_VALUE,RAWTOHEX(PREV_SQL_ADDR) PREV_SQL_ADDR,PREV_HASH_VALUE,EVENT,P1,P1TEXT,P2,P2TEXT,P3,P3TEXT,SEQ#,STATE,WAIT_TIME,SECONDS_IN_WAIT "
                sql = sql & "FROM V$SESSION A,V$SESSION_WAIT B "
                sql = sql & "WHERE A.SID=B.SID "
                isStart = False
            End If

            '画面条件
            If TXT_USERNAME.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE USERNAME LIKE '" & TXT_USERNAME.Text & "' "
                    isStart = False
                Else
                    sql = sql & "AND USERNAME LIKE '" & TXT_USERNAME.Text & "' "
                End If
            End If
            If TXT_PGM.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE PROGRAM LIKE '" & TXT_PGM.Text & "' "
                    isStart = False
                Else
                    sql = sql & "AND PROGRAM LIKE '" & TXT_PGM.Text & "' "
                End If
            End If
            If TXT_OSUSER.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE OSUSER LIKE '" & TXT_OSUSER.Text & "' "
                    isStart = False
                Else
                    sql = sql & "AND OSUSER LIKE '" & TXT_OSUSER.Text & "' "
                End If
            End If
            If CHK_ONLY_USER.Checked Then
                If isStart Then
                    sql = sql & "WHERE TYPE='USER' "
                    isStart = False
                Else
                    sql = sql & "AND TYPE='USER' "
                End If
            End If
            If CHK_ONLY_ACTIVE.Checked Then
                If isStart Then
                    sql = sql & "WHERE STATUS='ACTIVE' "
                    isStart = False
                Else
                    sql = sql & "AND STATUS='ACTIVE' "
                End If
            End If
            If TXT_USER_DEFINE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE " & TXT_USER_DEFINE.Text & " "
                    isStart = False
                Else
                    sql = sql & "AND " & TXT_USER_DEFINE.Text & " "
                End If
            End If

        ElseIf id = C_SQL Then
            '実行中のSQL情報取得
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                '10g以降(SQLIDでSQL文を識別)
                Dim sqlId As String
                If Not IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_ID").Index).Value) Then
                    sqlId = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_ID").Index).Value
                ElseIf Not IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ID").Index).Value) Then
                    sqlId = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ID").Index).Value
                Else
                    'SQLID、PRIV_SQLID共に値なしの場合SQLは実行しない
                    Return vbNullString
                End If

                ReDim bind(0)
                bind(0).SET_BIND(":SQL_ID", sqlId, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
                frm_Login.G_DB.SET_BIND_VARIABLE(bind)
                sql = "SELECT SQL_TEXT FROM V$SQLTEXT WHERE SQL_ID = :SQL_ID ORDER BY PIECE"
            Else
                '9iまで(SQL_ADRESSとSQL_HASH_VALUEでSQL文を識別)
                Dim sqlAddress As String
                Dim sqlHash As String
                If Not (IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_ADDRESS").Index).Value) Or IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_HASH_VALUE").Index).Value)) Then
                    If DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_ADDRESS").Index).Value.ToString.Length = 8 Then
                        sqlAddress = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_ADDRESS").Index).Value
                        sqlHash = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SQL_HASH_VALUE").Index).Value
                    Else
                        '値はNULLでないが「00」などであった場合はPREV_SQL_ADDRESS等を使う
                        If Not (IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value) Or IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_HASH_VALUE").Index).Value)) Then
                            If DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value.ToString.Length = 8 Then
                                sqlAddress = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value
                                sqlHash = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_HASH_VALUE").Index).Value
                            Else
                                Return vbNullString
                            End If
                        Else
                            'SQLID、PRIV_SQLID共に値なしの場合SQLは実行しない
                            Return vbNullString
                        End If
                    End If
                ElseIf Not (IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value) Or IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_HASH_VALUE").Index).Value)) Then
                    If DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value.ToString.Length = 8 Then
                        sqlAddress = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_SQL_ADDR").Index).Value
                        sqlHash = DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("PREV_HASH_VALUE").Index).Value
                    Else
                        Return vbNullString
                    End If
                Else
                    'SQLID、PRIV_SQLID共に値なしの場合SQLは実行しない
                    Return vbNullString
                End If

                ReDim bind(1)
                bind(0).SET_BIND(":ADDRESS", sqlAddress, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
                bind(1).SET_BIND(":HASH_VALUE", sqlHash, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
                frm_Login.G_DB.SET_BIND_VARIABLE(bind)
                sql = "SELECT SQL_TEXT FROM V$SQLTEXT WHERE ADDRESS = :ADDRESS AND HASH_VALUE = :HASH_VALUE ORDER BY PIECE"
            End If
        ElseIf id = C_SYSSTAT Then
            'セッション統計取得
            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            sql = "SELECT CLASS || DECODE(BITAND(CLASS,128),128,' DEBUG') || DECODE(BITAND(CLASS,64),64,' SQL') || "
            sql = sql & "DECODE(BITAND(CLASS,32),32,' RAC') || DECODE(BITAND(CLASS,16),16,' OS') || DECODE(BITAND(CLASS,8),8,' CACHE') "
            sql = sql & "|| DECODE(BITAND(CLASS,4),4,' ENQUEUE') || DECODE(BITAND(CLASS,2),2,' REDO') "
            sql = sql & "|| DECODE(BITAND(CLASS,1),1,' USER') CLASS_NAME, "
            sql = sql & "A.STATISTIC# STATISTIC#,NAME,A.VALUE "
            sql = sql & "FROM V$SESSTAT A,V$STATNAME B "
            sql = sql & "WHERE A.STATISTIC#=B.STATISTIC# "
            sql = sql & "AND SID = :SID "
            If TXT_USER_DEFINE2.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE2.Text & " "
            End If
            sql = sql & "ORDER BY CLASS,STATISTIC#"
        ElseIf id = C_SESSION_WAIT Then
            'セッション待機イベント取得
            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
               (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '11g,10.2、以降
                sql = "SELECT EVENT,TRUNC(TIME_WAITED_MICRO/1000) ""TIME_WAITED(ms)"",TOTAL_WAITS,TOTAL_TIMEOUTS,AVERAGE_WAIT*10 ""AVERAGE_WAIT(ms)"",MAX_WAIT*10 ""MAX_WAIT(ms)"", WAIT_CLASS "
                sql = sql & "FROM V$SESSION_EVENT "
                sql = sql & "WHERE SID = :SID "
                If CHK_NOT_IDLE.Checked Then
                    sql = sql & "AND WAIT_CLASS# <> 6 "
                End If
                If TXT_USER_DEFINE3.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE3.Text & " "
                End If
                sql = sql & "ORDER BY TIME_WAITED_MICRO DESC"
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 Or _
                  (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 1) Then
                '9i,10.1
                sql = "SELECT EVENT,TRUNC(TIME_WAITED_MICRO/1000) ""TIME_WAITED(ms)"",TOTAL_WAITS,TOTAL_TIMEOUTS,AVERAGE_WAIT*10 ""AVERAGE_WAIT(ms)"",MAX_WAIT*10 ""MAX_WAIT(ms)"" "
                sql = sql & "FROM V$SESSION_EVENT "
                sql = sql & "WHERE SID = :SID "
                If TXT_USER_DEFINE3.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE3.Text & " "
                End If
                sql = sql & "ORDER BY TIME_WAITED_MICRO DESC"
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = "SELECT EVENT,TRUNC(TIME_WAITED*10) ""TIME_WAITED(ms)"",TOTAL_WAITS,TOTAL_TIMEOUTS,AVERAGE_WAIT*10 ""AVERAGE_WAIT(ms)"",MAX_WAIT*10 ""MAX_WAIT(ms)"" "
                sql = sql & "FROM V$SESSION_EVENT "
                sql = sql & "WHERE SID = :SID "
                If TXT_USER_DEFINE3.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE3.Text & " "
                End If
                sql = sql & "ORDER BY TIME_WAITED DESC"
            End If
        ElseIf id = C_PROCESS Then
            'プロセス情報取得
            ReDim bind(1)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":SERIAL", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SERIAL#").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Or (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '10g以降、9.2.0
                sql = "SELECT PID,SPID,PROGRAM,PGA_USED_MEM,PGA_ALLOC_MEM,PGA_FREEABLE_MEM,PGA_MAX_MEM "
                sql = sql & "FROM V$PROCESS A "
                sql = sql & "WHERE ADDR= (SELECT PADDR FROM V$SESSION B WHERE B.SID= :SID AND B.SERIAL# = :SERIAL)"
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 0 Then
                '9.0.1
                sql = "SELECT PID,SPID,PROGRAM,PGA_USED_MEM,PGA_ALLOC_MEM,PGA_MAX_MEM "
                sql = sql & "FROM V$PROCESS A "
                sql = sql & "WHERE ADDR= (SELECT PADDR FROM V$SESSION B WHERE B.SID = :SID AND B.SERIAL# = :SERIAL)"
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = "SELECT A.PID,A.SPID,A.PROGRAM,C.VALUE ""PGA MEMORY"",D.VALUE ""PGA MEMORY MAX"""
                sql = sql & "FROM V$PROCESS A,V$SESSION B,V$SESSTAT C,V$SESSTAT D "
                sql = sql & "WHERE ADDR= B.PADDR AND B.SID = :SID AND B.SERIAL# = :SERIAL AND B.SID=C.SID AND B.SID=D.SID AND C.STATISTIC#='20' AND D.STATISTIC#='21'"
            End If
            If TXT_USER_DEFINE4.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE4.Text & " "
            End If

        ElseIf id = C_LATCH Then
            'ラッチ情報取得

            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                '11g以降
                sql = "SELECT PID, LADDR, NAME, GETS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2 Then
                '10.2
                sql = "SELECT PID, LADDR, NAME, GETS "
            Else
                'それ以前
                sql = "SELECT PID, LADDR, NAME "
            End If
            sql = sql & "FROM V$LATCHHOLDER A "
            sql = sql & "WHERE SID = :SID "
            If TXT_USER_DEFINE5.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE5.Text & " "
            End If
        ElseIf id = C_LOCK Then
            'ロック情報取得
            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            sql = "SELECT TYPE,ID1,ID2,LMODE,REQUEST,CTIME,BLOCK "
            sql = sql & "FROM V$LOCK "
            sql = sql & "WHERE SID = :SID "
            If TXT_USER_DEFINE6.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE6.Text & " "
            End If
        ElseIf id = C_CURSOR Then
            'カーソル情報取得
            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            sql = "SELECT USER_NAME, "
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                sql = sql & "SQL_ID, "
            Else
                sql = sql & "ADDRESS,HASH_VALUE, "
            End If
            sql = sql & "SQL_TEXT "
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                sql = sql & ",LAST_SQL_ACTIVE_TIME "
            End If
            If (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 11 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Or _
                Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 12 Then
                sql = sql & ",CURSOR_TYPE "
            End If

            sql = sql & "FROM V$OPEN_CURSOR "
            sql = sql & "WHERE SID = :SID AND SADDR = (SELECT SADDR FROM V$SESSION WHERE SID = :SID) "
            If TXT_USER_DEFINE7.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE7.Text & " "
            End If

        ElseIf id = C_WAIT_CLASS Then
            'セッション待機イベントクラス取得
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
               (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '11g以降,10.2
                sql = "SELECT WAIT_CLASS,TIME_WAITED*10 "
                sql = sql & "FROM V$SESSION_WAIT_CLASS "
                sql = sql & "WHERE SID = :SID "
                sql = sql & "AND WAIT_CLASS#<>6 "
                sql = sql & "UNION ALL "
                sql = sql & "SELECT B.NAME,DECODE(B.NAME,'CPU used by this session',A.VALUE*10,'DB time',A.VALUE*10,A.VALUE) "
                sql = sql & "FROM V$SESSTAT A,V$STATNAME B "
                sql = sql & "WHERE A.SID = :SID AND A.STATISTIC#=B.STATISTIC# "
                sql = sql & "AND  B.NAME IN ('CPU used by this session','DB time','consistent gets from cache','consistent gets direct','db block gets from cache','db block gets direct','physical reads cache','physical reads direct') "
                sql = sql & "ORDER BY 2"
            End If
        ElseIf id = C_TIME_MODEL Then
            '時刻統計イベントクラス取得
            ReDim bind(0)
            bind(0).SET_BIND(":SID", DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10  Then
                '10g以降
                sql = "SELECT STAT_NAME,VALUE "
                sql = sql & "FROM V$SESS_TIME_MODEL "
                sql = sql & "WHERE SID = :SID "
                sql = sql & "ORDER BY VALUE DESC"
            End If
        End If
        Return sql
    End Function

    Private Function KillSession(ByVal sid As Integer, ByVal serial As Integer) As Boolean
        '#######################################################################
        '指定セッション切断
        '#######################################################################
        Dim sql As String = "BEGIN EXECUTE IMMEDIATE 'ALTER SYSTEM KILL SESSION ''" & sid & "," & serial & "'''; END;"
        Dim cnt As Long  'ダミー

        If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) Then
            MsgBox(M_COMMON.GetMessage("I0020"))
            Return True
        Else
            Return False
        End If

    End Function

    Private Function setSqlTrace(ByVal sid As Integer, ByVal serial As Integer, ByVal isSetTrace As Boolean) As Boolean
        '#######################################################################
        '指定セッショントレース設定変更
        '#######################################################################
        Dim sql As String
        Dim cnt As Long 'ダミー変数
        Dim msg As String

        If isSetTrace Then
            sql = "BEGIN SYS.DBMS_SYSTEM.SET_SQL_TRACE_IN_SESSION(" & sid & "," & serial & ",true); END;"
            msg = "I0021"
        Else
            sql = "BEGIN SYS.DBMS_SYSTEM.SET_SQL_TRACE_IN_SESSION(" & sid & "," & serial & ",false); END;"
            msg = "I0022"
        End If

        If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) Then
            MsgBox(M_COMMON.GetMessage(msg))
            Return True
        Else
            MsgBox(M_COMMON.GetMessage("E0009"))
            Return False
        End If
    End Function

    Private Function DispGraph(ByVal sid As Integer) As Boolean
        '#######################################################################
        '指定セッションの統計グラフ表示
        '#######################################################################
        Dim sql As String
        Dim dir As String = vbNullString
        Dim filename As String = vbNullString
        Dim datatime As DateTime = DateTime.Now
        Dim readBlocks As Long = 0
        Dim bind(0) As M_CONST.S_BIND
        Dim outputHTML As New System.Text.StringBuilder

        bind(0).SET_BIND(":SID", sid, M_CONST.C_COLUMN_TYPE.NUMBER, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR) = vbNullString Then
            dir = System.IO.Directory.GetCurrentDirectory() & "\log"
        Else
            dir = M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_LOGDIR)
        End If

        filename = datatime.ToString("yyyyMMddHHmmssfff") & ".html"
        sql = CreateSQL(C_WAIT_CLASS)

        Using DataSet As New DataSet
            'SQL実行
            If frm_Login.G_DB.GET_DATASET(sql, DataSet) Then
                If DataSet.Tables(0).Rows.Count = 0 Then
                    MsgBox(M_COMMON.GetMessage("E0021"))
                    Return False
                End If
                outputHTML.Append("<html><head><script type=""text/JavaScript"">")
                outputHTML.Append("function func(){")
                outputHTML.Append("obj=document.forms[0];")
                outputHTML.Append("obj.action=""http://" & M_CONST.C_SERVER_NAME & "/O-Analyzer/graph/Session.jsp"";")
                outputHTML.Append("obj.submit()};")
                outputHTML.Append("</script>")
                outputHTML.Append("</head><body onLoad=""func()""><form method=""post"">")
                outputHTML.Append("<input type=""hidden"" name=""SID"" value=""" & sid & """ />")
                For Each dr As DataRow In DataSet.Tables(0).Rows
                    outputHTML.Append("<input type=""hidden"" name=""" & dr.Item(0) & """ value=""" & dr.Item(1) & """ />")
                    If dr.Item(0) = "db block gets direct" Or dr.Item(0) = "consistent gets from cache" Or _
                       dr.Item(0) = "consistent gets direct" Or dr.Item(0) = "db block gets from cache" Then
                        readBlocks = readBlocks + CLng(dr.Item(1))
                    End If
                Next
                outputHTML.Append("<input type=""hidden"" name=""READ_BLOCKS"" value=""" & readBlocks & """ />")
                outputHTML.Append("</form></body></html>")
                M_FILE.WriteString(outputHTML.ToString, dir, filename)
                If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ONLINE) = 1 Then
                    M_COMMON.ExecuteCmd2("file://" & dir & "\" & filename)
                End If
                Return True
            Else
                Return False
            End If
        End Using

    End Function

    Private Sub FetchFromResultset(ByRef dgv As DataGridView, ByVal dispInfoLabel As Boolean, Optional ByVal fetchCnt As Long = 10000000000)
        '#######################################################################
        ''SELECT結果FETCH
        '#######################################################################
        Dim msgstr(0) As String
        Dim dataTable As New DataTable

        'FETCH+表示
        If frm_Login.G_DB.FETCH_TO_DATATABLE(dataTable, fetchCnt) = False Then
            Exit Sub
        End If

        'データソースをグリッドビューに設定した後だと他のSQLの実行結果が表示されてしまうため先に取得
        If dispInfoLabel Then
            msgstr(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgstr)
        End If
        dgv.DataSource = dataTable

    End Sub

    Private Sub frm_Session_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50

        tabControl1.Location = New Point(5, Me.Height / 2)
        tabControl1.Height = Me.Height - tabControl1.Location.Y - LBL_INFO.Height - 45
        DGV_SESSION.Width = GroupBox1.Location.X - 10
        DGV_SESSION.Height = Me.Height - LBL_INFO.Height - GroupBox3.Height - tabControl1.Location.Y - 5

        tabControl1.Width = DGV_SESSION.Width
        GroupBox3.Width = DGV_SESSION.Width

        GroupBox2.Width = tabControl1.Width - 20
        GroupBox4.Width = tabControl1.Width - 20
        GroupBox5.Width = tabControl1.Width - 20
        GroupBox6.Width = tabControl1.Width - 20
        GroupBox7.Width = tabControl1.Width - 20
        GroupBox8.Width = tabControl1.Width - 20
        GroupBox9.Width = tabControl1.Width - 20
        GroupBox10.Width = tabControl1.Width - 20

        GroupBox2.Height = tabControl1.Height - 40
        GroupBox4.Height = tabControl1.Height - 40
        GroupBox5.Height = tabControl1.Height - 40
        GroupBox6.Height = tabControl1.Height - 40
        GroupBox7.Height = tabControl1.Height - 40
        GroupBox8.Height = tabControl1.Height - 40
        GroupBox9.Height = tabControl1.Height - 40
        GroupBox10.Height = tabControl1.Height - 40
        TXT_EXECUTING_SQL.Height = GroupBox5.Height - 25

        DGV_WAIT_EVENT.Width = GroupBox4.Width - 10
        DGV_WAIT_EVENT.Height = GroupBox4.Height - 45
        DGV_SYSSTAT.Width = GroupBox2.Width - 10
        DGV_SYSSTAT.Height = GroupBox4.Height - 45
        DGV_PROCESS.Width = GroupBox6.Width - 10
        DGV_PROCESS.Height = GroupBox6.Height - 45
        DGV_LATCH.Width = GroupBox6.Width - 10
        DGV_LATCH.Height = GroupBox6.Height - 45
        DGV_LOCK.Width = GroupBox6.Width - 10
        DGV_LOCK.Height = GroupBox6.Height - 45
        DGV_CURSOR.Width = GroupBox6.Width - 10
        DGV_CURSOR.Height = GroupBox6.Height - 45
        DGV_TIME_MODEL.Width = GroupBox10.Width - 10
        DGV_TIME_MODEL.Height = GroupBox10.Height - 45

        TXT_EXECUTING_SQL.Width = GroupBox5.Width - 15
        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub ButtonCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CSV.Click
        Dim dir As String = vbNullString
        Dim msg(0) As String

        LBL_INFO.Text = M_COMMON.GetMessage("I0029")
        'ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        If DGV_SESSION.RowCount = 0 Then
            MsgBox(M_COMMON.GetMessage("E0005"))
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        If M_FILE.CreateCSV(DGV_SESSION, dir, Format(Now(), "yyyyMMddHHmmss") & "_data.csv") Then
            MsgBox(M_COMMON.GetMessage("I0012"))
        End If

        LBL_INFO.Text = vbNullString
        'ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

    End Sub

    Private Sub BtnReflesh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        DispSession()
    End Sub

    Private Sub BtnKill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_KILL.Click
        Dim sid As Integer = 0
        Dim serial As Integer = 0
        Dim retcd As Integer

        '選択セルを行選択化
        For Each a As DataGridViewCell In DGV_SESSION.SelectedCells
            DGV_SESSION.Rows(a.RowIndex).Selected = True
        Next

        If DGV_SESSION.SelectedRows.Count = 0 Then
            MsgBox(M_COMMON.GetMessage("E0007"))
            Exit Sub
        End If
        retcd = MsgBox(M_COMMON.GetMessage("W0004"), MsgBoxStyle.YesNo, "確認")

        If retcd = System.Windows.Forms.DialogResult.Yes Then
            For Each selectedRow As DataGridViewRow In DGV_SESSION.SelectedRows
                sid = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString)
                serial = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SERIAL#").Index).Value.ToString)
                If KillSession(sid, serial) = False Then
                    Exit For
                End If
            Next
            DispSession()
        End If
    End Sub

    Private Sub frm_Session_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LBL_INFO.Text = vbNullString

        '待機イベントのクラス分けがされている10.2以降のみ可能にする
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
            (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
            CHK_NOT_IDLE.Visible = True
            BTN_GRAPH.Enabled = True
        Else
            CHK_NOT_IDLE.Visible = False
            BTN_GRAPH.Enabled = False
        End If

        '10g以降のみ時刻統計を表示
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) < 10 Then
            TAB_TIME_MODEL.Dispose()
        End If

        M_FILE.SetControls(Me.Name, Me.Controls)
        M_APP_SID = frm_Login.G_DB.GET_SID
        Me.Text = "O-Analyzer - セッション情報(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & _
         ":" & Process.GetCurrentProcess().Id() & ")"
    End Sub

    Private Sub BTN_TRACE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_TRACE.Click
        Dim sid As Integer
        Dim serial As Integer
        Dim retcd As Integer

        '選択セルを行選択化
        For Each a As DataGridViewCell In DGV_SESSION.SelectedCells
            DGV_SESSION.Rows(a.RowIndex).Selected = True
        Next

        If DGV_SESSION.SelectedRows.Count = 0 Then
            MsgBox(M_COMMON.GetMessage("E0007"))
            Exit Sub
        End If

        retcd = MsgBox(M_COMMON.GetMessage("I0023"), MsgBoxStyle.YesNoCancel, "確認")

        For Each selectedRow As DataGridViewRow In DGV_SESSION.SelectedRows
            sid = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString)
            serial = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SERIAL#").Index).Value.ToString)

            If retcd = vbYes Then
                If setSqlTrace(sid, serial, True) = False Then
                    Exit For
                End If
            ElseIf retcd = vbNo Then
                If setSqlTrace(sid, serial, False) = False Then
                    Exit For
                End If
            ElseIf retcd = vbCancel Then
                Exit Sub
            End If
        Next
        DispSession()
    End Sub

    Private Sub DGV_1_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_SESSION.CurrentCellChanged
        TXT_EXECUTING_SQL.Text = vbNullString

        'データグリッドビューにデータがない場合は処理を抜ける
        If (DGV_SESSION.DataSource Is Nothing) Then
            Exit Sub
        End If
        If (DGV_SESSION.CurrentRow Is Nothing) Then
            Exit Sub
        End If

        '''''''''''''''''''''''スレッドテスト
        'Dim t1 As System.Threading.Thread
        'Dim t2 As System.Threading.Thread
        'Dim t3 As System.Threading.Thread
        'Dim t4 As System.Threading.Thread
        'Dim t5 As System.Threading.Thread
        'Dim t6 As System.Threading.Thread
        'Dim t7 As System.Threading.Thread
        'If CHK_CHECK_SQL.Checked Then
        '    '実行中SQL文表示
        '    If Integer.parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
        '        '10g～はSQLID
        '        If IsDBNull(DGV_SESSION.CurrentRow.Cells(11).Value) = False Then
        '            t1 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker1))
        '            t1.Start()
        '        End If
        '    Else
        '        '～9iはSQL_HASHとSQL_ADRRESS
        '        If IsDBNull(DGV_SESSION.CurrentRow.Cells(10).Value) = False And _
        '           IsDBNull(DGV_SESSION.CurrentRow.Cells(11).Value) = False Then
        '            t1 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker1))
        '            t1.Start()
        '        End If
        '    End If
        'End If

        ''SIDがNULLの場合処理は処理を抜ける
        'If IsDBNull(DGV_SESSION.CurrentRow.Cells(0).Value) Then
        '    Exit Sub
        'End If

        'If CHK_WAIT_EVENT.Checked Then
        '    '待機イベント表示
        '    t2 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker2))
        '    t2.Start()
        'End If

        'If CHK_SYSSTAT.Checked Then
        '    'システム統計表示
        '    t3 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker3))
        '    t3.Start()
        'End If
        'If CHK_PROCESS.Checked Then
        '    'プロセス情報表示
        '    t4 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker4))
        '    t4.Start()
        'End If
        'If CHK_LATCH.Checked Then
        '    'ラッチ情報表示
        '    t5 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker5))
        '    t5.Start()
        'End If
        'If CHK_LOCK.Checked Then
        '    'ロック情報表示
        '    t6 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker6))
        '    t6.Start()
        'End If
        'If CHK_CURSOR.Checked Then
        '    'カーソル情報表示
        '    t7 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker7))
        '    t7.Start()
        'End If

        '''''''''''''''''''''''スレッドテスト


        If CHK_CHECK_SQL.Checked Then
            '実行中SQL文表示
            DispSQLInfo()
        End If

        'SIDがNULLの場合処理は処理を抜ける
        If IsDBNull(DGV_SESSION.CurrentRow.Cells(DGV_SESSION.Columns("SID").Index).Value) Then
            Exit Sub
        End If

        If CHK_WAIT_EVENT.Checked Then
            '待機イベント表示
            DispWaitEvent()
        End If
        If CHK_SYSSTAT.Checked Then
            'システム統計表示
            DispSessStat()
        End If
        If CHK_PROCESS.Checked Then
            'プロセス情報表示
            DispProcess()
        End If
        If CHK_LATCH.Checked Then
            'ラッチ情報表示
            DispLatch()
        End If
        If CHK_LOCK.Checked Then
            'ロック情報表示
            DispLock()
        End If
        If CHK_CURSOR.Checked Then
            'カーソル情報表示
            DispCursor()
        End If
        If CHK_TIME_MODEL.Checked Then
            'カーソル情報表示
            DispTimeModel()
        End If

    End Sub

    ' '''''''''''''''''''''''スレッドテスト
    'Delegate Sub DispSQLDelegate()
    'Sub worker1()
    '    Invoke(New DispSQLDelegate(AddressOf t1))
    'End Sub
    'Private Sub t1()
    '    DispSQLInfo()
    'End Sub

    'Delegate Sub DispWaitDelegate()
    'Sub worker2()
    '    Invoke(New DispWaitDelegate(AddressOf t2))
    'End Sub
    'Private Sub t2()
    '    DispWaitEvent()
    'End Sub

    'Delegate Sub DispSessStatDelegate()
    'Sub worker3()
    '    Invoke(New DispSessStatDelegate(AddressOf t3))
    'End Sub
    'Private Sub t3()
    '    DispSessStat()
    'End Sub

    'Delegate Sub DispProcessDelegate()
    'Sub worker4()
    '    Invoke(New DispProcessDelegate(AddressOf t4))
    'End Sub
    'Private Sub t4()
    '    DispProcess()
    'End Sub

    'Delegate Sub DispLatchDelegate()
    'Sub worker5()
    '    Invoke(New DispLatchDelegate(AddressOf t5))
    'End Sub
    'Private Sub t5()
    '    DispLatch()
    'End Sub

    'Delegate Sub DispLockDelegate()
    'Sub worker6()
    '    Invoke(New DispLockDelegate(AddressOf t6))
    'End Sub
    'Private Sub t6()
    '    DispLock()
    'End Sub

    'Delegate Sub DispCursorDelegate()
    'Sub worker7()
    '    Invoke(New DispCursorDelegate(AddressOf t7))
    'End Sub
    'Private Sub t7()
    '    DispCursor()
    'End Sub
    ' '''''''''''''''''''''''スレッドテスト

    Private Sub frm_Session_FormClosed(ByVal sender As System.Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As DataGridViewRowPostPaintEventArgs) _
    Handles DGV_SESSION.RowPostPaint, DGV_WAIT_EVENT.RowPostPaint, DGV_SYSSTAT.RowPostPaint, _
            DGV_PROCESS.RowPostPaint, DGV_LATCH.RowPostPaint, DGV_CURSOR.RowPostPaint, DGV_TIME_MODEL.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    Private Sub DGV_SESSION_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_SESSION.CellLeave
        M_LAST_CELL_ROW = e.RowIndex
    End Sub

    Private Sub DGV_SESSION_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_SESSION.CellEnter
        DGV_SESSION.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue

        If DGV_SESSION.RowCount < M_LAST_CELL_ROW + 1 Then
            '前回表示していた行がなくなった場合
            Exit Sub
        End If
        If e.RowIndex <> M_LAST_CELL_ROW Then
            If DGV_SESSION.Rows(M_LAST_CELL_ROW).Cells(DGV_SESSION.Columns("SID").Index).Value = M_APP_SID Then
                DGV_SESSION.Rows(M_LAST_CELL_ROW).DefaultCellStyle.BackColor = Color.LightCyan
            Else
                DGV_SESSION.Rows(M_LAST_CELL_ROW).DefaultCellStyle.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub BTN_GRAPH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_GRAPH.Click
        '#######################################################################
        'セッショングラフ表示
        '#######################################################################
        Dim sid As Integer = 0
        Dim serial As Integer = 0

        '選択セルを行選択化
        For Each a As DataGridViewCell In DGV_SESSION.SelectedCells
            DGV_SESSION.Rows(a.RowIndex).Selected = True
        Next

        If DGV_SESSION.SelectedRows.Count = 0 Then
            MsgBox(M_COMMON.GetMessage("E0007"))
            Exit Sub
        ElseIf DGV_SESSION.SelectedRows.Count >= 3 Then
            MsgBox(M_COMMON.GetMessage("E0020"))
            Exit Sub
        End If

        For Each selectedRow As DataGridViewRow In DGV_SESSION.SelectedRows
            sid = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SID").Index).Value.ToString)
            serial = Integer.Parse(selectedRow.Cells(DGV_SESSION.Columns("SERIAL#").Index).Value.ToString)
            If DispGraph(sid) = False Then
                Exit For
            End If
        Next
        If M_CONST.GetInitParamValue(M_CONST.C_INITPARAM_INIT_ONLINE) = 0 Then
            'オフラインモードの場合
            MsgBox("HTMLファイルを生成しました")
        End If
    End Sub
End Class