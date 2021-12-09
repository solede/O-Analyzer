Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_SGA
    '#######################################################################
    'SGA情報画面
    '#######################################################################

    ''定数
    'SQL条件
    Private Const C_SQL_CURSOR As Integer = 0
    Private Const C_EXECUTION_PLAN_WITH_STAT As Integer = 2
    Private Const C_SQL_FULLTEXT As Integer = 3
    Private Const C_CREATE_TEMP_TAB As Integer = 5
    Private Const C_INSERT_BH As Integer = 6
    Private Const C_SELECT_BH As Integer = 7
    Private Const C_WAIT_EVENT As Integer = 8
    Private Const C_SYSSTAT As Integer = 9
    Private Const C_INIT_PARAMETER As Integer = 10
    Private Const C_SGASTAT As Integer = 11
    Private Const C_BUFFER_STAT As Integer = 12
    Private Const C_LIBCACHE_STAT As Integer = 13
    Private Const C_ROWCACHE_STAT As Integer = 14
    Private Const C_LATCH As Integer = 15
    Private Const C_DELETE_BH As Integer = 16
    Private Const C_GET_VSQL_COLUMNS As Integer = 17
    Private Const C_LOCK As Integer = 18
    Private Const C_LOCK_DEPEND As Integer = 19
    Private Const C_SEGMENT_STAT As Integer = 20

    'V$BHの集約条件
    Private Const C_BH_NONE = 0        '集約なし
    Private Const C_BH_TABLESPACE = 1  '表領域
    Private Const C_BH_OBJECT = 2      'オブジェクト
    Private Const C_BH_STATUS = 3      'ステータス
    Private Const C_BH_BLOCKCLASS = 4  'ブロッククラス

    'BUFFER_POOL
    Private Const C_BP_ALL = 0
    Private Const C_BP_DEFAULT = 1
    Private Const C_BP_RECYCLE = 2
    Private Const C_BP_KEEP = 3
    Private Const C_BP_2K = 4
    Private Const C_BP_4K = 5
    Private Const C_BP_8K = 6
    Private Const C_BP_16K = 7
    Private Const C_BP_32K = 8

    'セグメント統計の集約条件
    Private Const C_SEGSTAT_NONE = 0          '集約なし
    Private Const C_SEGSTAT_STATNAME = 1      '統計名
    Private Const C_SEGSTAT_OWNER = 2         'スキーマ
    Private Const C_SEGSTAT_OBJECT = 3        'オブジェクト
    Private Const C_SEGSTAT_SUBOBJECTNONE = 4 'サブオブジェクト
    Private Const C_SEGSTAT_TABLESPACE = 5    '表領域
    Private Const C_SEGSTAT_OBJECT_TYPE = 6   'オブジェクトタイプ

    'SQLカーソルのソート条件
    Private Const C_CURSOR_SORT_NONE = 0
    Private Const C_CURSOR_SORT_EXECUTIONS = 1
    Private Const C_CURSOR_SORT_SUM_READ_BUFF = 2
    Private Const C_CURSOR_SORT_AVG_READ_BUFF = 3
    Private Const C_CURSOR_SORT_SUM_READ_DISK = 4
    Private Const C_CURSOR_SORT_AVG_READ_DISK = 5
    Private Const C_CURSOR_SORT_SUM_ELAPSED_TIME = 6
    Private Const C_CURSOR_SORT_AVG_ELAPSED_TIME = 7
    Private Const C_CURSOR_SORT_SUM_WRITE_DISK = 8
    Private Const C_CURSOR_SORT_AVG_WRITE_DISK = 9
    Private Const C_CURSOR_SORT_LAST_EXECUTION_TIME = 10

    'ロック画面の表示タイプ
    Private Const C_LOCK_MODE_LOCKER_WAITER As Integer = 0
    Private Const C_LOCK_MODE_WAITER_LOCKER As Integer = 1
    Private Const C_LOCK_MODE_OBJ_TX As Integer = 2

    'ORACLEのロックタイプ
    Private Const C_ORA_LOCK_TYPE_NULL As Integer = 1
    Private Const C_ORA_LOCK_TYPE_SS As Integer = 2
    Private Const C_ORA_LOCK_TYPE_SX As Integer = 3
    Private Const C_ORA_LOCK_TYPE_S As Integer = 4
    Private Const C_ORA_LOCK_TYPE_SSX As Integer = 5
    Private Const C_ORA_LOCK_TYPE_X As Integer = 6

    'モジュール変数
    Private M_CANCELFLG As Boolean = False
    Private M_LOCK_MODE As Integer = 0

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXEC.Enabled = True
            BTN_CANCEL.Enabled = False
        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXEC.Enabled = False
            BTN_CANCEL.Enabled = True
        End If
    End Sub

    Private Function CheckInputValue() As Boolean
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) < 10 Then
            If TXT_SQL_ADDRESS.Text = vbNullString Or TXT_SQL_HASH.Text = vbNullString Then
                Return False
            End If
        Else
            If TXT_SQLID2.Text = vbNullString Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub CreateTempTable()
        Dim sql As String
        Dim msgVal(0) As String
        Dim cnt As Long = 0
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        sql = CreateSQL(C_CREATE_TEMP_TAB)
        If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) Then
            msgVal(0) = TXT_TABLE_NAME.Text
            MsgBox(M_COMMON.GetMessage("I0028", msgVal))
        Else
            MsgBox(M_COMMON.GetMessage("E0003"))
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub DispBufferCache()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_BH.DataSource = Nothing
        bindingSource.DataSource = Nothing

        '一時表へバッファキャッシュの情報をコピー
        sql = CreateSQL(C_INSERT_BH)
        If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) = False Then
            MsgBox(M_COMMON.GetMessage("E0004"))
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        '一時表からデータ抽出
        sql = CreateSQL(C_SELECT_BH)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_BH.DataSource = bindingSource

        '一時表の削除
        sql = CreateSQL(C_DELETE_BH)
        If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        msgVal(0) = dataset.Tables(0).Rows.Count.ToString

        DGV_BH.AutoResizeColumns()
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSQL()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_SQL_CURSOR)
        ExecQuery(sql, DGV_SQL)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispExecutionPlan()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        TXT_FULL_SQL.Text = vbNullString
        Me.Refresh()

        sql = CreateSQL(C_SQL_FULLTEXT)

        Using dataset As New DataSet
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then
                'SQL全文表示
                If dataset.Tables(0).Rows.Count > 0 Then
                    For Each row As Object In dataset.Tables(0).Rows
                        TXT_FULL_SQL.Text = TXT_FULL_SQL.Text & row.Item(0).ToString
                    Next
                End If
            Else
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                Exit Sub
            End If
        End Using

        Me.Refresh()
        sql = CreateSQL(C_EXECUTION_PLAN_WITH_STAT)

        If ExecQuery(sql, DGV_EXECUTION_PLAN) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        '実行統計に色づけ
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            DGV_EXECUTION_PLAN.Columns(DGV_EXECUTION_PLAN.Columns("ACTIVE_TIME(ms)").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_EXECUTION_PLAN.Columns(DGV_EXECUTION_PLAN.Columns("ESTIMATED_OPTIMAL_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_EXECUTION_PLAN.Columns(DGV_EXECUTION_PLAN.Columns("ESTIMATED_ONEPASS_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_EXECUTION_PLAN.Columns(DGV_EXECUTION_PLAN.Columns("LAST_MEMORY_USED").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_EXECUTION_PLAN.Columns(DGV_EXECUTION_PLAN.Columns("LAST_TEMPSEG_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
        End If

        'アクセス述語とフィルター述語は画面下部で表示させるため非表示
        DGV_EXECUTION_PLAN.Columns.Item(DGV_EXECUTION_PLAN.Columns.Count - 1).Visible = False
        DGV_EXECUTION_PLAN.Columns.Item(DGV_EXECUTION_PLAN.Columns.Count - 2).Visible = False

        DGV_EXECUTION_PLAN.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispWaitEvent()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_WAIT_EVENT)
        ExecQuery(sql, DGV_WAIT_EVENT)
        DGV_WAIT_EVENT.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSystemStat()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_SYSSTAT)
        ExecQuery(sql, DGV_SYSSTAT)
        DGV_SYSSTAT.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispParameter()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_INIT_PARAMETER)
        ExecQuery(sql, DGV_PARAM)
        DGV_PARAM.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSharedPool()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_SGASTAT)
        ExecQuery(sql, DGV_SGA_CMP_SIZE)
        DGV_SGA_CMP_SIZE.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispStatistics()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_BUFFER_STAT)
        If ExecQuery(sql, DGV_BUFFER) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        sql = CreateSQL(C_LIBCACHE_STAT)
        If ExecQuery(sql, DGV_LIBRARY) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        sql = CreateSQL(C_ROWCACHE_STAT)
        If ExecQuery(sql, DGV_DICTIONARY) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        DGV_BUFFER.AutoResizeColumns()
        DGV_LIBRARY.AutoResizeColumns()
        DGV_DICTIONARY.AutoResizeColumns()

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispLatch()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_LATCH)
        ExecQuery(sql, DGV_LATCH)
        DGV_LATCH.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispLock()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        M_LOCK_MODE = CMB_LOCK.SelectedItem.ToString.Substring(0, 1)
        sql = CreateSQL(C_LOCK)
        If ExecQuery(sql, DGV_LOCK) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        If DGV_DEPEND.CurrentCellAddress.X > 0 Then
            DGV_DEPEND.CurrentCell = DGV_DEPEND(0, 0)
        End If
        For Each obj As Object In DGV_DEPEND.Columns
            obj.Width = 1
        Next

        DGV_DEPEND.DataSource = Nothing

        DGV_LOCK.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSegmentStat()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_SEGMENT_STAT)
        If ExecQuery(sql, DGV_SEGMENT_STAT) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        DGV_SEGMENT_STAT.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSession()
        Dim sql As String

        If BTN_CANCEL.Enabled = True Then
            Exit Sub
        End If
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_LOCK_DEPEND)
        ExecQuery(sql, DGV_DEPEND)
        DGV_DEPEND.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Function ExecQuery(ByVal sql As String, ByRef DGV As DataGridView) As Boolean
        '#######################################################################
        'SELECT実行
        '#######################################################################

        'DGVの行の新規追加可能
        'DGV.AllowUserToAddRows = True

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)

        'カレントセルの列位置が大きい、列幅が大きい場合などに
        '「DGV_1.DataSource = Nothing」の処理がエラーになるため移動
        If DGV.CurrentCellAddress.X > 0 Then
            DGV.CurrentCell = DGV(0, 0)
        End If
        For Each obj As Object In DGV.Columns
            obj.Width = 1
        Next

        DGV.DataSource = Nothing

        '列幅自動調整無効
        'DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        '接続型
        DGV.RowCount = 1
        DGV.ColumnCount = 0

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            If FetchFromResultset(DGV) = False Then
                ExecQuery = False
            End If
            ExecQuery = True
        Else
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            ExecQuery = False
        End If
    End Function

    Private Function FetchFromResultset(ByRef DGV As DataGridView, Optional ByVal fetchRowCnt As Long = 10000000000) As Boolean
        '#######################################################################
        'SELECT実行
        '#######################################################################
        Dim dataTable As New DataTable
        Dim ReadedRowCnt As Long
        Dim RefleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String

        TXT_SQLID1.Focus()

        Try
            Do While ReadedRowCnt < fetchRowCnt
                If frm_Login.G_DB.FETCH_TO_DATATABLE(dataTable, RefleshCnt) = False Then
                    LBL_INFO.Text = ""
                    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                    FetchFromResultset = False
                End If
                msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
                LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

                If frm_Login.G_DB.GET_SELECT_FLG = False Then
                    DGV.DataSource = dataTable
                    DGV.AllowUserToAddRows = False
                    Exit Do
                End If

                Try
                    'セルドラッグ時の場合のエラー対応
                    System.Windows.Forms.Application.DoEvents()
                Catch ex As Exception
                    'NULL
                End Try

                'キャンセル指示があった場合
                If M_CANCELFLG Then
                    DGV.DataSource = dataTable
                    M_CANCELFLG = False
                    MsgBox(M_COMMON.GetMessage("I0008"))
                    Exit Do
                End If
                ReadedRowCnt = ReadedRowCnt + RefleshCnt
            Loop
            DGV.DataSource = dataTable

            M_COMMON.SetRowheaderWidth(DGV)
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)
            FetchFromResultset = True
        Catch ex As Exception
            MsgBox(ex.Message)
            LBL_INFO.Text = ""
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            FetchFromResultset = False
        End Try

    End Function

    Private Function CreateSQL(ByVal id As Short) As String
        '#######################################################################
        'SQL作成
        'id:SQL文の条件
        '#######################################################################
        Dim sql As String = vbNullString
        Dim isStart As Boolean = True
        Dim lockColumn As Integer = 0
        Dim groupBy(3) As Integer

        If id = C_SQL_CURSOR Then
            'SQLカーソル情報取得
            'SUBSTR(SQL_TEXT)は1000byte目がマルチバイト文字の途中だと「ORA-29275: 不完全なマルチバイト文字です」が発生するため
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
              (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                sql = "SELECT SQL_ID,SUBSTR(SQL_TEXT,1,1000) SQL_TEXT,CHILD_NUMBER,PLAN_HASH_VALUE,EXECUTIONS,ROWS_PROCESSED,ELAPSED_TIME,TRUNC(ELAPSED_TIME/DECODE(EXECUTIONS,0,1,EXECUTIONS)) AVG_ELAPSED_TIME,CPU_TIME,SERVICE,PARSING_SCHEMA_NAME,MODULE,SORTS,FETCHES, "
                sql = sql & "END_OF_FETCH_COUNT,PX_SERVERS_EXECUTIONS,PARSE_CALLS,BUFFER_GETS,DISK_READS,DIRECT_WRITES,SHARABLE_MEM, "
                sql = sql & "PERSISTENT_MEM,RUNTIME_MEM,OPTIMIZER_COST,USER_IO_WAIT_TIME,PLSQL_EXEC_TIME,JAVA_EXEC_TIME,FIRST_LOAD_TIME,LAST_LOAD_TIME,LAST_ACTIVE_TIME,OBJECT_STATUS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 1 Then
                sql = "SELECT SQL_ID,SUBSTR(SQL_TEXT,1,1000) SQL_TEXT,CHILD_NUMBER,PLAN_HASH_VALUE,TRUNC(ELAPSED_TIME/DECODE(EXECUTIONS,0,1,EXECUTIONS)) AVG_ELAPSED_TIME,ELAPSED_TIME,CPU_TIME,MODULE,EXECUTIONS,SORTS,FETCHES, "
                sql = sql & "DISK_READS,DIRECT_WRITES,BUFFER_GETS,SHARABLE_MEM,PERSISTENT_MEM,RUNTIME_MEM,OPTIMIZER_COST, "
                sql = sql & "END_OF_FETCH_COUNT,PARSE_CALLS,PLSQL_EXEC_TIME,JAVA_EXEC_TIME,FIRST_LOAD_TIME,LAST_LOAD_TIME,OBJECT_STATUS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2 Then
                sql = "SELECT RAWTOHEX(ADDRESS) ADDRESS,HASH_VALUE,SUBSTR(SQL_TEXT,1,1000) SQL_TEXT,CHILD_NUMBER,PLAN_HASH_VALUE,TRUNC(ELAPSED_TIME/DECODE(EXECUTIONS,0,1,EXECUTIONS)) AVG_ELAPSED_TIME,ELAPSED_TIME,CPU_TIME,MODULE,SORTS,FETCHES, "
                sql = sql & "EXECUTIONS,PARSE_CALLS,DISK_READS,BUFFER_GETS,SHARABLE_MEM,PERSISTENT_MEM,RUNTIME_MEM,OPTIMIZER_COST,FIRST_LOAD_TIME,LAST_LOAD_TIME,OBJECT_STATUS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 0 Then
                sql = "SELECT RAWTOHEX(ADDRESS) ADDRESS,HASH_VALUE,SUBSTR(SQL_TEXT,1,1000) SQL_TEXT,CHILD_NUMBER,PLAN_HASH_VALUE,TRUNC(ELAPSED_TIME/DECODE(EXECUTIONS,0,1,EXECUTIONS)) AVG_ELAPSED_TIME,ELAPSED_TIME,CPU_TIME,MODULE,SORTS, "
                sql = sql & "EXECUTIONS,PARSE_CALLS,DISK_READS,BUFFER_GETS,SHARABLE_MEM,PERSISTENT_MEM,RUNTIME_MEM,OPTIMIZER_COST,FIRST_LOAD_TIME,LAST_LOAD_TIME,OBJECT_STATUS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = "SELECT RAWTOHEX(ADDRESS) ADDRESS,HASH_VALUE,SUBSTR(SQL_TEXT,1,1000) SQL_TEXT,CHILD_NUMBER,MODULE,SORTS,EXECUTIONS,FIRST_LOAD_TIME,PARSE_CALLS, "
                sql = sql & "DISK_READS,BUFFER_GETS,SHARABLE_MEM,PERSISTENT_MEM,RUNTIME_MEM,OPTIMIZER_COST "
            End If
            '追加のユーザ定義表示列
            If TXT_USER_COLUMN.Text <> vbNullString Then
                sql = sql & TXT_USER_COLUMN.Text & " "
            End If

            sql = sql & "FROM V$SQL "

            If TXT_SQLID1.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "SQL_ID = '" & TXT_SQLID1.Text & "' "
            End If

            If TXT_SQL.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) <= 9 Then
                    sql = sql & "UPPER(SQL_TEXT) LIKE UPPER('" & TXT_SQL.Text & "') "
                Else
                    sql = sql & "UPPER(SQL_FULLTEXT) LIKE UPPER('" & TXT_SQL.Text & "') "
                End If

            End If

            If TXT_SCHEMA.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If

                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
                   (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                    sql = sql & "PARSING_SCHEMA_NAME LIKE UPPER('" & TXT_SCHEMA.Text & "') "
                Else
                    sql = sql & "PARSING_SCHEMA_ID = (SELECT USER_ID FROM ALL_USERS WHERE USERNAME LIKE UPPER('" & TXT_SCHEMA.Text & "')) "
                End If
            End If

            If TXT_MODULE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "UPPER(MODULE) LIKE UPPER('" & TXT_MODULE.Text & "') "
            End If

            If TXT_AVG_GE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "DECODE(EXECUTIONS,0,ELAPSED_TIME/1,ELAPSED_TIME/EXECUTIONS) >= " & TXT_AVG_GE.Text & " "
            End If
            If TXT_AVG_LE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "DECODE(EXECUTIONS,0,ELAPSED_TIME/1,ELAPSED_TIME/EXECUTIONS) <= " & TXT_AVG_LE.Text & " "
            End If

            If TXT_SUM_GE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "ELAPSED_TIME >= " & TXT_SUM_GE.Text & " "
            End If
            If TXT_SUM_LE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE "
                    isStart = False
                Else
                    sql = sql & "AND "
                End If
                sql = sql & "ELAPSED_TIME <= " & TXT_SUM_LE.Text & " "
            End If

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
            (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                If CHK_NOT_EXITST_BG.Checked Then
                    If isStart Then
                        sql = sql & "WHERE "
                        isStart = False
                    Else
                        sql = sql & "AND "
                    End If
                    sql = sql & "SERVICE <> 'SYS$BACKGROUND' "
                End If
                If CHK_NOT_EXITST_USERS.Checked Then
                    If isStart Then
                        sql = sql & "WHERE "
                        isStart = False
                    Else
                        sql = sql & "AND "
                    End If
                    sql = sql & "SERVICE <> 'SYS$USERS' "
                End If
            End If

            If TXT_USER_DEFINE.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE " & TXT_USER_DEFINE.Text
                    isStart = False
                Else
                    sql = sql & "AND " & TXT_USER_DEFINE.Text
                End If
            End If


            If Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) <> C_CURSOR_SORT_NONE Then
                sql = sql & " ORDER BY "
                If Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 2)) = C_CURSOR_SORT_EXECUTIONS Then
                    '実行回数
                    sql = sql & "EXECUTIONS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_READ_BUFF Then
                    '合計バッファ読込ブロック数
                    sql = sql & "BUFFER_GETS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_READ_BUFF Then
                    '平均バッファ読込ブロック数
                    sql = sql & "BUFFER_GETS/DECODE(EXECUTIONS,0,1,EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_READ_DISK Then
                    '合計DISK読込ブロック数
                    sql = sql & "DISK_READS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_READ_DISK Then
                    '平均DISK読込ブロック数
                    sql = sql & "DISK_READS/DECODE(EXECUTIONS,0,1,EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_ELAPSED_TIME Then
                    '合計処理時間
                    sql = sql & "ELAPSED_TIME "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_ELAPSED_TIME Then
                    '平均処理時間
                    sql = sql & "ELAPSED_TIME/DECODE(EXECUTIONS,0,1,EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_WRITE_DISK Then
                    '合計DISK書込ブロック数
                    sql = sql & "DIRECT_WRITES "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_WRITE_DISK Then
                    '平均DISK書込ブロック数
                    sql = sql & "DIRECT_WRITES/DECODE(EXECUTIONS,0,1,EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 2)) = C_CURSOR_SORT_LAST_EXECUTION_TIME Then
                    '最終実行時間
                    sql = sql & "NVL(LAST_ACTIVE_TIME,TO_DATE('19000101000000','YYYYMMDDHH24MISS')) "
                End If
                sql = sql & CMB_SORT_ORDER.SelectedItem.ToString & " "
            End If
        ElseIf id = C_GET_VSQL_COLUMNS Then
            'V$SQLのカラム取得
            sql = "SELECT COLUMN_NAME "
            sql = sql & "FROM DBA_TAB_COLUMNS "
            sql = sql & "WHERE TABLE_NAME = 'V_$SQL'"

        ElseIf id = C_EXECUTION_PLAN_WITH_STAT Then
            '統計とあわせた実行計画取得
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || NVL2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || NVL2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "QBLOCK_NAME,A.OBJECT_NAME,A.CARDINALITY,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,A.TIME*1000 ""TIME(ms)"",TRUNC(B.ACTIVE_TIME/1000) ""ACTIVE_TIME(ms)"","
                sql = sql & "B.ESTIMATED_OPTIMAL_SIZE,B.ESTIMATED_ONEPASS_SIZE,B.LAST_MEMORY_USED,A.TEMP_SPACE,B.LAST_TEMPSEG_SIZE,A.ACCESS_PREDICATES,A.FILTER_PREDICATES "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2 Then
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || NVL2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || NVL2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "A.OBJECT_NAME,A.CARDINALITY,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,TRUNC(B.ACTIVE_TIME/1000) ""ACTIVE_TIME(ms)"",A.TEMP_SPACE,A.ACCESS_PREDICATES,A.FILTER_PREDICATES "
            ElseIf (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 0) Then
                ''''8にはv$sql_planがないので動かない
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || nvl2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || nvl2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "A.OBJECT_NAME,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,A.TEMP_SPACE "
            End If
            sql = sql & "FROM V$SQL_PLAN A, V$SQL_WORKAREA B "
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                sql = sql & "WHERE A.SQL_ID='" & TXT_SQLID2.Text & "' AND A.CHILD_NUMBER= " & TXT_CHILD_NUM.Text & " "
                sql = sql & "AND A.SQL_ID=B.SQL_ID(+) AND A.CHILD_NUMBER = B.CHILD_NUMBER(+) AND A.ID = B.OPERATION_ID(+) "
            Else
                sql = sql & "WHERE A.ADDRESS='" & TXT_SQL_ADDRESS.Text & "' AND A.HASH_VALUE='" & TXT_SQL_HASH.Text & "' AND A.CHILD_NUMBER=" & TXT_CHILD_NUM.Text & " "
                sql = sql & "AND A.ADDRESS=B.ADDRESS(+) AND A.HASH_VALUE=B.HASH_VALUE(+) AND A.CHILD_NUMBER=B.CHILD_NUMBER(+) AND A.ID = B.OPERATION_ID(+) "
            End If
            sql = sql & "ORDER BY ID"
        ElseIf id = C_SQL_FULLTEXT Then
            'SQL全文取得
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                sql = "SELECT SQL_TEXT FROM V$SQLTEXT WHERE SQL_ID = '" & TXT_SQLID2.Text & "' ORDER BY PIECE"
            Else
                sql = "SELECT SQL_TEXT FROM V$SQLTEXT WHERE ADDRESS = '" & TXT_SQL_ADDRESS.Text & "' AND HASH_VALUE = '" & TXT_SQL_HASH.Text & "' ORDER BY PIECE"
            End If
        ElseIf id = C_CREATE_TEMP_TAB Then
            'V$BHの一時表の作成
            sql = "CREATE GLOBAL TEMPORARY TABLE "
            If TXT_CREATE_SCHEMA.Text <> vbNullString Then
                sql = sql & TXT_CREATE_SCHEMA.Text & "."
            End If
            sql = sql & TXT_TABLE_NAME.Text
            sql = sql & " (TS# NUMBER, OBJD NUMBER,STATUS VARCHAR2(7),CLASS# NUMBER,COUNT NUMBER) ON COMMIT DELETE ROWS"
        ElseIf id = C_INSERT_BH Then
            '一時表にバッファヘッダ情報を格納
            sql = "INSERT INTO "
            If TXT_CREATE_SCHEMA.Text <> vbNullString Then
                sql = sql & TXT_CREATE_SCHEMA.Text & "."
            End If
            sql = sql & TXT_TABLE_NAME.Text
            sql = sql & " (SELECT TS#,OBJD,STATUS,CLASS#,COUNT(*) COUNT "
            sql = sql & "  FROM V$BH "

            'ブロックステータス
            If CHK_CR.Checked Then
                sql = sql & "WHERE (STATUS IN('cr' "
                isStart = False
            End If

            If CHK_XCUR.Checked Then
                If isStart Then
                    sql = sql & "WHERE (STATUS IN('xcur' "
                Else
                    sql = sql & ", 'xcur' "
                End If
                isStart = False
            End If

            If CHK_FREE.Checked Then
                If isStart Then
                    sql = sql & "WHERE (STATUS IN('free' "
                Else
                    sql = sql & ",'free' "
                End If
                isStart = False
            End If
            If isStart = False Then
                sql = sql & ") "
            End If

            If CHK_OHER.Checked Then
                If isStart Then
                    sql = sql & "WHERE (STATUS NOT IN('cr','xcur','free') "
                Else
                    sql = sql & "OR STATUS NOT IN ('cr','xcur','free') "
                End If
                isStart = False
            End If

            '一つもステータスを選択していない場合
            If isStart Then
                sql = sql & "WHERE 1=0 "
                isStart = False
            Else
                sql = sql & ") "
            End If

            'ブロッククラス
            If TXT_CLASS.Text <> vbNullString Then
                sql = sql & "AND CLASS#= '" & TXT_CLASS.Text & "' "
            End If

            If TXT_DATA_OBJ_ID.Text <> vbNullString Then
                sql = sql & "AND OBJD= '" & TXT_DATA_OBJ_ID.Text & "' "
            End If

            sql = sql & "GROUP BY TS#,OBJD,STATUS,CLASS#"
            sql = sql & ")"
        ElseIf id = C_SELECT_BH Then
            '一時表のバッファヘッダ情報とセグメント情報を結合した結果を取得
            sql = sql & "SELECT"
            'If Integer.Parse(CMB_GROUPBY.SelectedItem.ToString.Substring(0, 1)) = C_BH_TS Then
            '    '表領域毎
            '    sql = sql & "  C.NAME TABLESPACE, "
            '    sql = sql & "  SUM(B.COUNT) BLOCK_COUNT "
            'ElseIf Integer.Parse(CMB_GROUPBY.SelectedItem.ToString.Substring(0, 1)) = C_BH_TS_OBJ Then
            '    '表領域毎、オブジェクト毎
            '    sql = sql & "  C.NAME TABLESPACE, "
            '    sql = sql & "  B.OBJD DATA_OBJECT_ID,"
            '    sql = sql & "  A.OWNER OWNER,"
            '    sql = sql & "  A.OBJECT_NAME OBJECT_NAME,"
            '    sql = sql & "  A.SUBOBJECT_NAME SUBOBJECT_NAME,"
            '    sql = sql & "  A.OBJECT_TYPE OBJECT_TYPE,"
            '    sql = sql & "  SUM(B.COUNT) BLOCK_COUNT "
            'ElseIf Integer.Parse(CMB_GROUPBY.SelectedItem.ToString.Substring(0, 1)) = C_BH_TS_OBJ_ST Then
            '    '表領域毎、オブジェクト、ステータス毎
            '    sql = sql & "  C.NAME TABLESPACE, "
            '    sql = sql & "  B.OBJD DATA_OBJECT_ID,"
            '    sql = sql & "  A.OWNER OWNER,"
            '    sql = sql & "  A.OBJECT_NAME OBJECT_NAME,"
            '    sql = sql & "  A.SUBOBJECT_NAME SUBOBJECT_NAME,"
            '    sql = sql & "  A.OBJECT_TYPE OBJECT_TYPE,"
            '    sql = sql & "  B.STATUS,"
            '    sql = sql & "  SUM(B.COUNT) BLOCK_COUNT "
            'ElseIf Integer.Parse(CMB_GROUPBY.SelectedItem.ToString.Substring(0, 1)) = C_BH_TS_OBJ_ST_BC Then
            '    '表領域毎、オブジェクト、ステータス、CLASS#毎
            '    sql = sql & "  C.NAME TABLESPACE, "
            '    sql = sql & "  B.OBJD DATA_OBJECT_ID,"
            '    sql = sql & "  A.OWNER OWNER,"
            '    sql = sql & "  A.OBJECT_NAME OBJECT_NAME,"
            '    sql = sql & "  A.SUBOBJECT_NAME SUBOBJECT_NAME,"
            '    sql = sql & "  A.OBJECT_TYPE OBJECT_TYPE,"
            '    sql = sql & "  B.STATUS,"
            '    sql = sql & "  B.CLASS#,"
            '    sql = sql & "  SUM(B.COUNT) BLOCK_COUNT "
            'End If
            groupBy(0) = Integer.Parse(CMB_GROUPBY1.SelectedItem.ToString.Substring(0, 1))
            groupBy(1) = Integer.Parse(CMB_GROUPBY2.SelectedItem.ToString.Substring(0, 1))
            groupBy(2) = Integer.Parse(CMB_GROUPBY3.SelectedItem.ToString.Substring(0, 1))
            groupBy(3) = Integer.Parse(CMB_GROUPBY4.SelectedItem.ToString.Substring(0, 1))

            If groupBy(0) = C_BH_NONE And groupBy(1) = C_BH_NONE And groupBy(2) = C_BH_NONE And groupBy(3) = C_BH_NONE Then
                sql = sql & "  C.NAME TABLESPACE, "
                sql = sql & "  B.OBJD DATA_OBJECT_ID,"
                sql = sql & "  A.OWNER OWNER,"
                sql = sql & "  A.OBJECT_NAME OBJECT_NAME,"
                sql = sql & "  A.SUBOBJECT_NAME SUBOBJECT_NAME,"
                sql = sql & "  A.OBJECT_TYPE OBJECT_TYPE,"
                sql = sql & "  B.STATUS,"
                sql = sql & "  B.CLASS# "
            Else
                For i As Integer = 0 To groupBy.Length - 1
                    If groupBy(i) = C_BH_TABLESPACE Then
                        sql = sql & "  C.NAME TABLESPACE, "
                    ElseIf groupBy(i) = C_BH_OBJECT Then
                        sql = sql & "  B.OBJD DATA_OBJECT_ID,"
                        sql = sql & "  A.OWNER OWNER,"
                        sql = sql & "  A.OBJECT_NAME OBJECT_NAME,"
                        sql = sql & "  A.SUBOBJECT_NAME SUBOBJECT_NAME,"
                        sql = sql & "  A.OBJECT_TYPE OBJECT_TYPE,"
                    ElseIf groupBy(i) = C_BH_STATUS Then
                        sql = sql & "  B.STATUS,"
                    ElseIf groupBy(i) = C_BH_BLOCKCLASS Then
                        sql = sql & "  B.CLASS#,"
                    End If
                Next
                sql = sql & "  SUM(B.COUNT) BLOCK_COUNT "
            End If

            sql = sql & "FROM "

            'V$BHの一時表
            If TXT_CREATE_SCHEMA.Text <> vbNullString Then
                sql = sql & "    " & TXT_CREATE_SCHEMA.Text & "."
            End If

            'CLUSTERオブジェクトは構成する表やインデックスがすべて同じDATA_OBJECT_IDになるため該当のDATA_OBJECT_IDはCLUSTERのみ抽出する
            sql = sql & TXT_TABLE_NAME.Text & " B,"
            sql = sql & "  (SELECT "
            sql = sql & "    DATA_OBJECT_ID, "
            sql = sql & "    TRIM(LEADING ' ' FROM MIN(OWNER)) OWNER, "
            sql = sql & "    TRIM(LEADING ' ' FROM MIN(OBJECT_NAME)) OBJECT_NAME, "
            sql = sql & "    TRIM(LEADING ' ' FROM MIN(SUBOBJECT_NAME)) SUBOBJECT_NAME, "
            sql = sql & "    TRIM(LEADING ' ' FROM MIN(OBJECT_TYPE)) OBJECT_TYPE "
            sql = sql & "   FROM "
            sql = sql & "    (SELECT DATA_OBJECT_ID, DECODE(OBJECT_TYPE,'CLUSTER', ' ' || OWNER,OWNER) OWNER, "
            sql = sql & "     DECODE(OBJECT_TYPE,'CLUSTER', ' ' || OBJECT_NAME,OBJECT_NAME) OBJECT_NAME, "
            sql = sql & "     DECODE(OBJECT_TYPE,'CLUSTER', ' ' || SUBOBJECT_NAME,SUBOBJECT_NAME) SUBOBJECT_NAME, "
            sql = sql & "     DECODE(OBJECT_TYPE,'CLUSTER', ' ' || OBJECT_TYPE,OBJECT_TYPE) OBJECT_TYPE  "
            sql = sql & "     FROM DBA_OBJECTS) DBA_OBJECTS "
            sql = sql & "   GROUP BY DATA_OBJECT_ID ) A, "
            sql = sql & "  V$TABLESPACE C "
            sql = sql & "WHERE"
            sql = sql & "  B.OBJD=A.DATA_OBJECT_ID(+) AND "
            sql = sql & "  B.TS#=C.TS# "

            If TXT_SCHEMA2.Text <> vbNullString Then
                sql = sql & "  AND A.OWNER LIKE UPPER('" & TXT_SCHEMA2.Text & "') "
            End If
            If TXT_OBJECT.Text <> vbNullString Then
                sql = sql & "  AND A.OBJECT_NAME LIKE UPPER('" & TXT_OBJECT.Text & "') "
            End If
            If TXT_TABLESPACE.Text <> vbNullString Then
                sql = sql & "  AND C.NAME LIKE UPPER('" & TXT_TABLESPACE.Text & "') "
            End If

            If Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) <> C_BP_ALL Then
                If Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_DEFAULT Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE BUFFER_POOL = 'DEFAULT') "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_RECYCLE Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE BUFFER_POOL = 'RECYCLE') "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_KEEP Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE BUFFER_POOL = 'KEEP') "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_2K Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE TABLESPACE_NAME IN "
                    sql = sql & " (SELECT TABLESPACE_NAME FROM DBA_TABLESPACES WHERE BLOCK_SIZE = 2048)) "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_4K Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE TABLESPACE_NAME IN "
                    sql = sql & " (SELECT TABLESPACE_NAME FROM DBA_TABLESPACES WHERE BLOCK_SIZE = 4096)) "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_8K Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE TABLESPACE_NAME IN "
                    sql = sql & " (SELECT TABLESPACE_NAME FROM DBA_TABLESPACES WHERE BLOCK_SIZE = 8192)) "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_16K Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE TABLESPACE_NAME IN "
                    sql = sql & " (SELECT TABLESPACE_NAME FROM DBA_TABLESPACES WHERE BLOCK_SIZE = 16384)) "
                ElseIf Integer.Parse(CMB_BUFFER_POOL.SelectedIndex.ToString.Substring(0, 1)) = C_BP_32K Then
                    sql = sql & " AND NVL(SUBOBJECT_NAME,OBJECT_NAME) IN (SELECT SEGMENT_NAME FROM DBA_SEGMENTS WHERE TABLESPACE_NAME IN "
                    sql = sql & " (SELECT TABLESPACE_NAME FROM DBA_TABLESPACES WHERE BLOCK_SIZE = 32768)) "
                End If
            End If

            If groupBy(0) = C_BH_NONE And groupBy(1) = C_BH_NONE And groupBy(2) = C_BH_NONE And groupBy(3) = C_BH_NONE Then

            Else
                '集約条件がある場合
                sql = sql & "GROUP BY "
                For i As Integer = 0 To groupBy.Length - 1
                    If groupBy(i) = C_BH_TABLESPACE Then
                        If isStart = False Then
                            sql = sql & ", "
                        End If
                        sql = sql & "  C.NAME "
                        isStart = False
                    ElseIf groupBy(i) = C_BH_OBJECT Then
                        If isStart = False Then
                            sql = sql & ", "
                        End If
                        sql = sql & "  B.OBJD,A.OWNER,A.OBJECT_NAME,A.SUBOBJECT_NAME,A.OBJECT_TYPE "
                        isStart = False
                    ElseIf groupBy(i) = C_BH_STATUS Then
                        If isStart = False Then
                            sql = sql & ", "
                        End If
                        sql = sql & "  B.STATUS "
                        isStart = False
                    ElseIf groupBy(i) = C_BH_BLOCKCLASS Then
                        If isStart = False Then
                            sql = sql & ", "
                        End If
                        sql = sql & "  B.CLASS# "
                        isStart = False
                    End If
                Next
                sql = sql & "ORDER BY"
                sql = sql & "  BLOCK_COUNT DESC"
            End If

        ElseIf id = C_DELETE_BH Then
            sql = "DELETE FROM "
            'V$BHの一時表
            If TXT_CREATE_SCHEMA.Text <> vbNullString Then
                sql = sql & TXT_CREATE_SCHEMA.Text & "."
            End If
            sql = sql & TXT_TABLE_NAME.Text

        ElseIf id = C_WAIT_EVENT Then
            '待機イベントの取得
            sql = "SELECT EVENT,"
            If CHK_BG_WAIT.Checked Then
                'フォアグラウンド
                sql = sql & "TIME_WAITED_FG*10 ""TIME_WAITED(ms)"",TOTAL_WAITS_FG,TOTAL_TIMEOUTS_FG,TRUNC(AVERAGE_WAIT_FG*10) ""AVERAGE_WAIT(ms)"" "
            Else
                'フォアグラウンド＋バックグラウンド
                sql = sql & "TIME_WAITED*10 ""TIME_WAITED(ms)"",TOTAL_WAITS,TOTAL_TIMEOUTS,TRUNC(AVERAGE_WAIT*10) ""AVERAGE_WAIT(ms)"" "
            End If

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
                (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '10.2以降の場合は待機クラス追加
                sql = sql & ",WAIT_CLASS "
            End If

            sql = sql & "FROM V$SYSTEM_EVENT "
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
            (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                sql = sql & "WHERE WAIT_CLASS# <> 6 "
            Else
                sql = sql & "WHERE 1=1 "
            End If

            If CHK_NOT_IDLE.Checked Then
                sql = sql & "AND WAIT_CLASS# <> 6 "
            End If

            If CHK_BG_WAIT.Checked Then

            End If

            If TXT_USER_DIFINE2.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DIFINE2.Text & " "
            End If
            If CHK_BG_WAIT.Checked Then
                sql = sql & "ORDER BY TIME_WAITED_FG DESC"
            Else
                sql = sql & "ORDER BY TIME_WAITED DESC"
            End If

        ElseIf id = C_SYSSTAT Then
            'システム統計の取得
            sql = "SELECT CLASS || DECODE(BITAND(CLASS,128),128,' DEBUG') || DECODE(BITAND(CLASS,64),64,' SQL') "
            sql = sql & "|| DECODE(BITAND(CLASS,32),32,' RAC') || DECODE(BITAND(CLASS,16),16,' OS') || DECODE(BITAND(CLASS,8),8,' CACHE') "
            sql = sql & "|| DECODE(BITAND(CLASS,4),4,' ENQUEUE') || DECODE(BITAND(CLASS,2),2,' REDO') "
            sql = sql & "|| DECODE(BITAND(CLASS,1),1,' USER') CLASS_NAME, STATISTIC#,NAME,VALUE "
            sql = sql & "FROM V$SYSSTAT "
            If TXT_USER_DEFINE3.Text <> vbNullString Then
                sql = sql & "WHERE " & TXT_USER_DEFINE3.Text
            End If
            sql = sql & "ORDER BY CLASS "
        ElseIf id = C_INIT_PARAMETER Then
            'パラメータの取得
            sql = "SELECT * FROM V$PARAMETER "
            If CHK_NOT_DEFAULT.Checked Then
                If isStart Then
                    sql = sql & "WHERE ISDEFAULT='FALSE' "
                    isStart = False
                Else
                    sql = sql & "AND ISDEFAULT='FALSE' "
                End If
            End If
            If TXT_USER_DEFINE4.Text <> vbNullString Then
                If isStart Then
                    sql = sql & "WHERE " & TXT_USER_DEFINE4.Text
                    isStart = False
                Else
                    sql = sql & "AND " & TXT_USER_DEFINE4.Text
                End If
            End If
        ElseIf id = C_SGASTAT Then
            'SGA統計の取得
            sql = "SELECT * FROM V$SGASTAT "
            If TXT_USER_DEFINE5.Text <> vbNullString Then
                sql = sql & "WHERE " & TXT_USER_DEFINE5.Text
                isStart = False
            End If
        ElseIf id = C_BUFFER_STAT Then
            'バッファプール統計の取得
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = "SELECT NAME, BLOCK_SIZE, FREE_BUFFER_WAIT, BUFFER_BUSY_WAIT, DB_BLOCK_GETS, CONSISTENT_GETS, PHYSICAL_READS, "
                sql = sql & " TRUNC((1-(PHYSICAL_READS/DECODE(DB_BLOCK_GETS+CONSISTENT_GETS,0,1,DB_BLOCK_GETS+CONSISTENT_GETS)))*100,2) ""HIT(%)"", PHYSICAL_WRITES "
                sql = sql & " FROM V$BUFFER_POOL_STATISTICS "
            ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
                sql = "SELECT NAME, FREE_BUFFER_WAIT, BUFFER_BUSY_WAIT, DB_BLOCK_GETS, CONSISTENT_GETS, PHYSICAL_READS, "
                sql = sql & "TRUNC((1-(PHYSICAL_READS/DECODE((DB_BLOCK_GETS+CONSISTENT_GETS),0,1,(DB_BLOCK_GETS+CONSISTENT_GETS))))*100,2) ""HIT(%)"", PHYSICAL_WRITES "
                sql = sql & "FROM V$BUFFER_POOL_STATISTICS "
            End If

        ElseIf id = C_LIBCACHE_STAT Then
            'ライブラリキャッシュ統計の取得
            sql = "SELECT NAMESPACE,GETS,GETHITS,TRUNC(GETHITRATIO*100,2) ""GET(%)"",PINS,PINHITS,TRUNC(PINHITRATIO *100,2) ""PIN(%)"" FROM V$LIBRARYCACHE "
            sql = sql & "ORDER BY GETS DESC"
        ElseIf id = C_ROWCACHE_STAT Then
            'ディクショナリキャッシュ統計の取得
            sql = "SELECT PARAMETER,GETS,GETMISSES, TRUNC((GETS - GETMISSES)/DECODE(GETS,0,1,GETS)*100,2) ""GETS(%)"" FROM V$ROWCACHE "
            sql = sql & "ORDER BY GETS DESC"
        ElseIf id = C_LATCH Then
            'ラッチ情報の取得
            sql = "SELECT NAME,GETS,MISSES,SPIN_GETS,SLEEPS,IMMEDIATE_GETS,IMMEDIATE_MISSES "
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
                sql = sql & ", TRUNC(WAIT_TIME/1000) ""WAIT_TIME(ms)"" "
            End If
            sql = sql & "FROM V$LATCH "
            If TXT_USER_DEFINE6.Text <> vbNullString Then
                sql = sql & "WHERE " & TXT_USER_DEFINE6.Text
            End If
        ElseIf id = C_LOCK Then
            'ロック情報の取得
            If Integer.Parse(CMB_LOCK.SelectedItem.ToString.Substring(0, 1)) = C_LOCK_MODE_LOCKER_WAITER Then
                'ロッカー - ウェイター
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                    sql = "SELECT "
                    sql = sql & "SID, "
                    sql = sql & "A.TYPE,DECODE(SUBSTR(A.TYPE,0,1),'L','ライブラリ・キャッシュ・ロック・インスタンス・ロック','N','ライブラリ・キャッシュ確保インスタンス','Q','行キャッシュ・インスタンス', DECODE(A.TYPE,'AT','ALTER TABLE文用に保持されているロック','BL','バッファ・ハッシュ表インスタンス','CF','制御ファイル・スキーマ・グローバル・エンキュー','CI','インスタンス間ファンクション起動インスタンス','CU','カーソル・バインド','DF','データ・ファイル・インスタンス','DL','ダイレクト・ローダー・パラレル索引作成','DM','マウント/起動dbプライマリ/セカンダリ・インスタンス','DR','分散リカバリ・プロセス','DX','分散トランザクション・エントリ','FS','ファイル・セット','HW','特定のセグメントの領域管理操作','IX','インスタンス番号','IR','インスタンス・リカバリ直列化グローバル・エンキュー','IS','インスタンス状態','IV','ライブラリ・キャッシュ無効化インスタンス','JQ','ジョブ・キュー','KK','スレッド・キック','MM','マウント定義グローバル・エンキュー','MR','メディア・リカバリ','PF','パスワード・ファイル','PI','パラレル操作','PS','パラレル操作','PR','プロセス起動','RT','REDOスレッド・グローバル・エンキュー','SC','システム変更番号インスタンス','SM','SMON','SN','順序番号インスタンス','SQ','順序番号エンキュー','SS','ソート・セグメント','ST','領域トランザクション・エンキュー','SV','順序番号値','TA','総称エンキュー','TM','DMLエンキュー','TS',DECODE(ID2,0,'一時セグメント・エンキュー（ID2=0)',1,'新規ブロック割当てエンキュー'),'TT','一時表エンキュー','TX','トランザクション・エンキュー','UL','ユーザー定義','UN','ユーザー名','US','UNDOセグメントDDL','WL','書込み中REDOログ・インスタンス',B.NAME)) TYPE_NAME, "
                    sql = sql & "ID1, B.ID1_TAG, ID2, B.ID2_TAG, LMODE, DECODE(LMODE,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                    sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                    sql = sql & "FROM V$LOCK A,V$LOCK_TYPE B "
                    sql = sql & "WHERE A.TYPE = B.TYPE(+) AND "
                    sql = sql & "REQUEST = 0 "
                    If TXT_USER_DEFINE2.Text <> vbNullString Then
                        sql = sql & "AND " & TXT_USER_DEFINE2.Text
                    End If
                Else
                    sql = "SELECT "
                    sql = sql & "SID, "
                    sql = sql & "A.TYPE,DECODE(SUBSTR(A.TYPE,0,1),'L','ライブラリ・キャッシュ・ロック・インスタンス・ロック','N','ライブラリ・キャッシュ確保インスタンス','Q','行キャッシュ・インスタンス', DECODE(A.TYPE,'AT','ALTER TABLE文用に保持されているロック','BL','バッファ・ハッシュ表インスタンス','CF','制御ファイル・スキーマ・グローバル・エンキュー','CI','インスタンス間ファンクション起動インスタンス','CU','カーソル・バインド','DF','データ・ファイル・インスタンス','DL','ダイレクト・ローダー・パラレル索引作成','DM','マウント/起動dbプライマリ/セカンダリ・インスタンス','DR','分散リカバリ・プロセス','DX','分散トランザクション・エントリ','FS','ファイル・セット','HW','特定のセグメントの領域管理操作','IX','インスタンス番号','IR','インスタンス・リカバリ直列化グローバル・エンキュー','IS','インスタンス状態','IV','ライブラリ・キャッシュ無効化インスタンス','JQ','ジョブ・キュー','KK','スレッド・キック','MM','マウント定義グローバル・エンキュー','MR','メディア・リカバリ','PF','パスワード・ファイル','PI','パラレル操作','PS','パラレル操作','PR','プロセス起動','RT','REDOスレッド・グローバル・エンキュー','SC','システム変更番号インスタンス','SM','SMON','SN','順序番号インスタンス','SQ','順序番号エンキュー','SS','ソート・セグメント','ST','領域トランザクション・エンキュー','SV','順序番号値','TA','総称エンキュー','TM','DMLエンキュー','TS',DECODE(ID2,0,'一時セグメント・エンキュー（ID2=0)',1,'新規ブロック割当てエンキュー'),'TT','一時表エンキュー','TX','トランザクション・エンキュー','UL','ユーザー定義','UN','ユーザー名','US','UNDOセグメントDDL','WL','書込み中REDOログ・インスタンス')) TYPE_NAME, "
                    sql = sql & "ID1, ID2, LMODE, DECODE(LMODE,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                    sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                    sql = sql & "FROM V$LOCK A "
                    sql = sql & "WHERE REQUEST = 0 "
                    If TXT_USER_DEFINE2.Text <> vbNullString Then
                        sql = sql & "AND " & TXT_USER_DEFINE2.Text
                    End If
                End If

            ElseIf Integer.Parse(CMB_LOCK.SelectedItem.ToString.Substring(0, 1)) = C_LOCK_MODE_WAITER_LOCKER Then
                'ウェイター - ロッカー
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                    sql = "SELECT "
                    sql = sql & "SID, "
                    sql = sql & "A.TYPE,DECODE(SUBSTR(A.TYPE,0,1),'L','ライブラリ・キャッシュ・ロック・インスタンス・ロック','N','ライブラリ・キャッシュ確保インスタンス','Q','行キャッシュ・インスタンス', DECODE(A.TYPE,'AT','ALTER TABLE文用に保持されているロック','BL','バッファ・ハッシュ表インスタンス','CF','制御ファイル・スキーマ・グローバル・エンキュー','CI','インスタンス間ファンクション起動インスタンス','CU','カーソル・バインド','DF','データ・ファイル・インスタンス','DL','ダイレクト・ローダー・パラレル索引作成','DM','マウント/起動dbプライマリ/セカンダリ・インスタンス','DR','分散リカバリ・プロセス','DX','分散トランザクション・エントリ','FS','ファイル・セット','HW','特定のセグメントの領域管理操作','IX','インスタンス番号','IR','インスタンス・リカバリ直列化グローバル・エンキュー','IS','インスタンス状態','IV','ライブラリ・キャッシュ無効化インスタンス','JQ','ジョブ・キュー','KK','スレッド・キック','MM','マウント定義グローバル・エンキュー','MR','メディア・リカバリ','PF','パスワード・ファイル','PI','パラレル操作','PS','パラレル操作','PR','プロセス起動','RT','REDOスレッド・グローバル・エンキュー','SC','システム変更番号インスタンス','SM','SMON','SN','順序番号インスタンス','SQ','順序番号エンキュー','SS','ソート・セグメント','ST','領域トランザクション・エンキュー','SV','順序番号値','TA','総称エンキュー','TM','DMLエンキュー','TS',DECODE(ID2,0,'一時セグメント・エンキュー（ID2=0)',1,'新規ブロック割当てエンキュー'),'TT','一時表エンキュー','TX','トランザクション・エンキュー','UL','ユーザー定義','UN','ユーザー名','US','UNDOセグメントDDL','WL','書込み中REDOログ・インスタンス',B.NAME)) TYPE_NAME, "
                    sql = sql & "ID1, B.ID1_TAG, ID2, B.ID2_TAG, LMODE, DECODE(LMODE,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                    sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                    sql = sql & "FROM V$LOCK A,V$LOCK_TYPE B "
                    sql = sql & "WHERE A.TYPE = B.TYPE(+) AND "
                    sql = sql & "LMODE = 0 "
                    If TXT_USER_DEFINE2.Text <> vbNullString Then
                        sql = sql & "AND " & TXT_USER_DEFINE2.Text
                    End If
                Else
                    sql = "SELECT "
                    sql = sql & "SID, "
                    sql = sql & "A.TYPE,DECODE(SUBSTR(A.TYPE,0,1),'L','ライブラリ・キャッシュ・ロック・インスタンス・ロック','N','ライブラリ・キャッシュ確保インスタンス','Q','行キャッシュ・インスタンス', DECODE(A.TYPE,'AT','ALTER TABLE文用に保持されているロック','BL','バッファ・ハッシュ表インスタンス','CF','制御ファイル・スキーマ・グローバル・エンキュー','CI','インスタンス間ファンクション起動インスタンス','CU','カーソル・バインド','DF','データ・ファイル・インスタンス','DL','ダイレクト・ローダー・パラレル索引作成','DM','マウント/起動dbプライマリ/セカンダリ・インスタンス','DR','分散リカバリ・プロセス','DX','分散トランザクション・エントリ','FS','ファイル・セット','HW','特定のセグメントの領域管理操作','IX','インスタンス番号','IR','インスタンス・リカバリ直列化グローバル・エンキュー','IS','インスタンス状態','IV','ライブラリ・キャッシュ無効化インスタンス','JQ','ジョブ・キュー','KK','スレッド・キック','MM','マウント定義グローバル・エンキュー','MR','メディア・リカバリ','PF','パスワード・ファイル','PI','パラレル操作','PS','パラレル操作','PR','プロセス起動','RT','REDOスレッド・グローバル・エンキュー','SC','システム変更番号インスタンス','SM','SMON','SN','順序番号インスタンス','SQ','順序番号エンキュー','SS','ソート・セグメント','ST','領域トランザクション・エンキュー','SV','順序番号値','TA','総称エンキュー','TM','DMLエンキュー','TS',DECODE(ID2,0,'一時セグメント・エンキュー（ID2=0)',1,'新規ブロック割当てエンキュー'),'TT','一時表エンキュー','TX','トランザクション・エンキュー','UL','ユーザー定義','UN','ユーザー名','US','UNDOセグメントDDL','WL','書込み中REDOログ・インスタンス')) TYPE_NAME, "
                    sql = sql & "ID1, ID2, LMODE, DECODE(LMODE,0,'なし',1,'1 NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                    sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                    sql = sql & "FROM V$LOCK A "
                    sql = sql & "WHERE LMODE = 0 "
                    If TXT_USER_DEFINE2.Text <> vbNullString Then
                        sql = sql & "AND " & TXT_USER_DEFINE2.Text
                    End If
                End If
            ElseIf Integer.Parse(CMB_LOCK.SelectedItem.ToString.Substring(0, 1)) = C_LOCK_MODE_OBJ_TX Then
                'ロックオブジェクト - トランザクション
                sql = "SELECT A.XIDUSN,XIDSLOT,XIDSQN,A.OBJECT_ID, B.OWNER || '.' ||B.OBJECT_NAME || NVL2(SUBOBJECT_NAME,':' || SUBOBJECT_NAME,NULL) OBJECT_NAME, SESSION_ID,ORACLE_USERNAME,OS_USER_NAME,PROCESS,DECODE(LOCKED_MODE,0,'0 なし',1,'1 NULL',2,'2 SS(行共有)',3,'3 SX(行排他)',4,'4 S(共有)',5,'5 SSX(共有/行排他)',6,'6 X(排他)') LOCKED_MODE "
                If frm_Login.G_DB.G_SELECT_DICTIONARY_FLG Then
                    sql = sql & "FROM V$LOCKED_OBJECT A,DBA_OBJECTS B "
                Else
                    sql = sql & "FROM V$LOCKED_OBJECT A,ALL_OBJECTS B "
                End If
                sql = sql & "WHERE A.OBJECT_ID = B.OBJECT_ID(+) "
                If TXT_USER_DEFINE2.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE2.Text
                End If
            End If
        ElseIf id = C_LOCK_DEPEND Then
            'ロック表のカレントデータに紐づくデータの表示
            If M_LOCK_MODE = C_LOCK_MODE_LOCKER_WAITER Then
                'ロッカー - ウェイター
                sql = "SELECT "
                sql = sql & "SID, TYPE, ID1, ID2, LMODE, DECODE(LMODE,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                sql = sql & "FROM V$LOCK "
                sql = sql & "WHERE "

                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                    lockColumn = 7
                Else
                    lockColumn = 5
                End If

                If DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_NULL Then
                    sql = sql & "REQUEST >= 1 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SS Then
                    sql = sql & "REQUEST = 6 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SX Then
                    sql = sql & "REQUEST >= 4 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_S Then
                    sql = sql & "(REQUEST >= 5 OR REQUEST = 3) AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SSX Then
                    sql = sql & "REQUEST >= 3 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_X Then
                    sql = sql & "REQUEST >= 2 AND "
                End If
                sql = sql & "ID1 = " & DGV_LOCK.CurrentRow.Cells(3).Value.ToString & " AND "

                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
                    lockColumn = 5
                Else
                    lockColumn = 4
                End If
                sql = sql & "ID2 = " & DGV_LOCK.CurrentRow.Cells(lockColumn).Value.ToString

            ElseIf M_LOCK_MODE = C_LOCK_MODE_WAITER_LOCKER Then
                'ウェイター - ロッカー
                sql = "SELECT "
                sql = sql & "SID, TYPE, ID1, ID2, LMODE, DECODE(LMODE,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') LMODE_NAME, "
                sql = sql & "REQUEST, DECODE(REQUEST,0,'なし',1,'NULL',2,'SS(行共有)',3,'SX(行排他)',4,'S(共有)',5,'SSX(共有/行排他)',6,'X(排他)') REQUEST_NAME, CTIME, BLOCK "
                sql = sql & "FROM V$LOCK "
                sql = sql & "WHERE "

                If frm_Login.G_DB.G_ORA_VERSION(1) >= 11 Then
                    lockColumn = 9
                Else
                    lockColumn = 7
                End If

                If DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_NULL Then
                    sql = sql & "LMODE = 6 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SS Then
                    sql = sql & "LMODE = 6 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SX Then
                    sql = sql & "LMODE >= 4 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_S Then
                    sql = sql & "(LMODE >= 5 OR LMODE = 3) AND"
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_SSX Then
                    sql = sql & "LMODE >= 3 AND "
                ElseIf DGV_LOCK.CurrentRow.Cells(lockColumn).Value = C_ORA_LOCK_TYPE_X Then
                    sql = sql & "LMODE >= 2 AND "
                End If

                sql = sql & "ID1 = " & DGV_LOCK.CurrentRow.Cells(3).Value & " AND "
                If frm_Login.G_DB.G_ORA_VERSION(1) >= 11 Then
                    sql = sql & "ID2 = " & DGV_LOCK.CurrentRow.Cells(5).Value
                Else
                    sql = sql & "ID2 = " & DGV_LOCK.CurrentRow.Cells(4).Value
                End If

            ElseIf M_LOCK_MODE = C_LOCK_MODE_OBJ_TX Then
                'ロックオブジェクト - トランザクション
                sql = "SELECT * "
                sql = sql & "FROM V$TRANSACTION "
                sql = sql & "WHERE XIDUSN = " & DGV_LOCK.CurrentRow.Cells(0).Value & " AND "
                sql = sql & " XIDSLOT = " & DGV_LOCK.CurrentRow.Cells(1).Value & " AND "
                sql = sql & " XIDSQN = " & DGV_LOCK.CurrentRow.Cells(2).Value
            End If

            'ロックチェーンを表示する機能を今後付け足す。以下はTXエンキューの排他ロックのみのSQL。

            'select
            'addr,kaddr,
            'SID,
            'locker_sid,
            'level,
            'SYS_CONNECT_BY_PATH(sid, '>') LOCK_CHAIN,
            'type,id1,id2,lmode,request,ctime
            'from (
            'select 
            'w.ADDR,w.KADDR,w.SID sid,NVL(l.sid,'-1') locker_sid,w.TYPE,l.type locker_type,w.ID1,l.ID1 locker_id1,w.ID2,l.id2 locker_id2,w.LMODE,w.REQUEST,w.CTIME,w.BLOCK
            'from v$lock w, (select * from v$lock l1 where exists (select 0 from v$lock l2 where l1.id2 = l2.id2 and l1.id2 = l2.id2 and l2.block = 1 ) and l1.lmode > 0) l
            'where
            ' w.type = 'TX' and
            ' w.id1 = l.id1(+) and
            ' w.id2 = l.id2(+) and
            ' w.type = l.type(+) and
            ' w.sid <> l.sid(+)
            ' and (l.sid is not null or w.sid not in (select sid from V$lock where request > 0 ))
            ' order by w.sid,w.type
            ')
            'start with LOCKER_SID = '-1'
            'connect by prior SID  =  LOCKER_SID and type = locker_type
            'order by level,locker_sid,sid
        ElseIf id = C_SEGMENT_STAT Then
            Dim tmpStr As String
            ReDim groupBy(5)
            Dim selectCnt As Integer = 0

            groupBy(0) = Integer.Parse(CMB_STAT_GROUP1.SelectedItem.ToString.Substring(0, 1))
            groupBy(1) = Integer.Parse(CMB_STAT_GROUP2.SelectedItem.ToString.Substring(0, 1))
            groupBy(2) = Integer.Parse(CMB_STAT_GROUP3.SelectedItem.ToString.Substring(0, 1))
            groupBy(3) = Integer.Parse(CMB_STAT_GROUP4.SelectedItem.ToString.Substring(0, 1))
            groupBy(4) = Integer.Parse(CMB_STAT_GROUP5.SelectedItem.ToString.Substring(0, 1))
            groupBy(5) = Integer.Parse(CMB_STAT_GROUP6.SelectedItem.ToString.Substring(0, 1))

            sql = "SELECT "

            If groupBy(0) = C_SEGSTAT_NONE And groupBy(1) = C_SEGSTAT_NONE And groupBy(2) = C_SEGSTAT_NONE And _
                groupBy(3) = C_SEGSTAT_NONE And groupBy(4) = C_SEGSTAT_NONE And groupBy(5) = C_SEGSTAT_NONE Then
                sql = sql & "OBJ#,DATAOBJ#,OWNER,NVL2(SUBOBJECT_NAME,OBJECT_NAME || ':' || SUBOBJECT_NAME,OBJECT_NAME) OBJECT_NAME, "
                sql = sql & "TABLESPACE_NAME,OBJECT_TYPE,STATISTIC_NAME,STATISTIC#,VALUE "
            Else
                For i As Integer = 0 To groupBy.Length - 1
                    If groupBy(i) = C_SEGSTAT_OBJECT Then
                        selectCnt = selectCnt + 1
                        sql = sql & "OBJECT_NAME, "
                    ElseIf groupBy(i) = C_SEGSTAT_SUBOBJECTNONE Then
                        selectCnt = selectCnt + 1
                        sql = sql & "SUBOBJECT_NAME, "
                    ElseIf groupBy(i) = C_SEGSTAT_OBJECT_TYPE Then
                        selectCnt = selectCnt + 1
                        sql = sql & "OBJECT_TYPE, "
                    ElseIf groupBy(i) = C_SEGSTAT_OWNER Then
                        selectCnt = selectCnt + 1
                        sql = sql & "OWNER, "
                    ElseIf groupBy(i) = C_SEGSTAT_STATNAME Then
                        selectCnt = selectCnt + 1
                        sql = sql & "STATISTIC_NAME, "
                    ElseIf groupBy(i) = C_SEGSTAT_TABLESPACE Then
                        selectCnt = selectCnt + 1
                        sql = sql & "TABLESPACE_NAME, "
                    End If
                Next
                sql = sql & "  SUM(VALUE) VALUE "
            End If

            sql = sql & "FROM V$SEGMENT_STATISTICS "
            sql = sql & "WHERE 1=1 "

            '表領域
            If TXT_TS.Text <> vbNullString Then
                sql = sql & "AND TABLESPACE_NAME LIKE '" & TXT_TS.Text & "' "
            End If

            'スキーマ
            If TXT_SCHEMA3.Text <> vbNullString Then
                sql = sql & "AND OWNER LIKE '" & TXT_SCHEMA3.Text & "' "
            End If

            'オブジェクト名
            If TXT_OBJECT2.Text <> vbNullString Then
                sql = sql & "AND OBJECT_NAME LIKE '" & TXT_OBJECT2.Text & "' "
            End If

            '統計名
            If CLB_STAT_NAME.CheckedItems.Count <> 0 Then
                isStart = True
                sql = sql & "AND STATISTIC_NAME IN ("
                For i As Integer = 0 To CLB_STAT_NAME.CheckedItems.Count - 1
                    If isStart Then
                        sql = sql & "'" & CLB_STAT_NAME.CheckedItems.Item(i).ToString & "'"
                        isStart = False
                    Else
                        sql = sql & ", '" & CLB_STAT_NAME.CheckedItems.Item(i).ToString & "'"
                    End If
                Next
                sql = sql & ") "
            End If

            'ユーザ定義条件
            If TXT_USER_DEFINE7.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE7.Text & " "
            End If

            If groupBy(0) = C_SEGSTAT_NONE And groupBy(1) = C_SEGSTAT_NONE And groupBy(2) = C_SEGSTAT_NONE And _
               groupBy(3) = C_SEGSTAT_NONE And groupBy(4) = C_SEGSTAT_NONE And groupBy(5) = C_SEGSTAT_NONE Then
            Else
                isStart = True
                sql = sql & "GROUP BY "
                tmpStr = vbNullString
                For i As Integer = 0 To groupBy.Length - 1
                    If groupBy(i) = C_SEGSTAT_OBJECT Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "OBJECT_NAME "
                        isStart = False
                    End If
                    If groupBy(i) = C_SEGSTAT_SUBOBJECTNONE Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "SUBOBJECT_NAME "
                        isStart = False
                    End If
                    If groupBy(i) = C_SEGSTAT_OBJECT_TYPE Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "OBJECT_TYPE "
                        isStart = False
                    End If
                    If groupBy(i) = C_SEGSTAT_OWNER Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "OWNER "
                        isStart = False
                    End If
                    If groupBy(i) = C_SEGSTAT_STATNAME Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "STATISTIC_NAME "
                        isStart = False
                    End If
                    If groupBy(i) = C_SEGSTAT_TABLESPACE Then
                        If isStart = False Then
                            tmpStr = tmpStr & ", "
                        End If
                        tmpStr = tmpStr & "TABLESPACE_NAME "
                        isStart = False
                    End If
                Next
                sql = sql & tmpStr
                sql = sql & "ORDER BY "
                For i As Integer = 0 To selectCnt - 1
                    If i = 0 Then
                        sql = sql & i + 1
                    Else
                        sql = sql & "," & i + 1
                    End If
                Next
                'sql = sql & tmpStr
            End If
        End If

        Return sql
    End Function

    Private Sub frm_SGA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_NONE & " なし")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_EXECUTIONS & " 実行回数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_READ_BUFF & " 合計バッファ読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_READ_BUFF & " 平均バッファ読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_READ_DISK & " 合計DISK読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_READ_DISK & " 平均DISK読込ブロック数")

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
        Else
            CHK_BG_WAIT.Checked = False
            CHK_BG_WAIT.Enabled = False
        End If

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
            CMB_BUFFER_POOL.Items.Add(C_BP_2K & " 2K")
            CMB_BUFFER_POOL.Items.Add(C_BP_4K & " 4K")
            CMB_BUFFER_POOL.Items.Add(C_BP_8K & " 8K")
            CMB_BUFFER_POOL.Items.Add(C_BP_16K & " 16K")
            CMB_BUFFER_POOL.Items.Add(C_BP_32K & " 32K")
        End If

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            '10g～の設定
            TXT_SQLID1.Visible = True
            TXT_SQLID1.Enabled = True
            Label1.Visible = True
            Label9.Visible = True
            TXT_SQLID2.Visible = True
            TXT_SQLID2.Enabled = True
            Label16.Visible = False
            TXT_SQL_ADDRESS.Visible = False
            TXT_SQL_ADDRESS.Enabled = False
            Label17.Visible = False
            TXT_SQL_HASH.Visible = False
            TXT_SQL_HASH.Enabled = False
            Label14.Visible = True
            TXT_SCHEMA.Visible = True
            TXT_SQLID2.Enabled = True
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_WRITE_DISK & " 合計DIRECT WRITEブロック数")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_WRITE_DISK & " 平均DIRECT WRITEブロック数")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_LAST_EXECUTION_TIME & " 最終実行時間")
            Label12.Location = New Point(165, 19)
            TXT_CHILD_NUM.Location = New Point(265, 16)

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Or _
              (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2) Then
                '10.2～
                CHK_NOT_EXITST_BG.Enabled = True
                CHK_NOT_EXITST_BG.Visible = True
                CHK_NOT_EXITST_USERS.Enabled = True
                CHK_NOT_EXITST_USERS.Visible = True
            End If
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 Then
            '9の設定
            TXT_SQLID1.Visible = False
            TXT_SQLID1.Enabled = False
            Label1.Visible = False
            Label9.Visible = False
            TXT_SQLID2.Visible = False
            TXT_SQLID2.Enabled = False
            Label16.Visible = True
            TXT_SQL_ADDRESS.Visible = True
            TXT_SQL_ADDRESS.Enabled = True
            Label17.Visible = True
            TXT_SQL_HASH.Visible = True
            TXT_SQL_HASH.Enabled = True
            Label14.Visible = False
            TXT_SCHEMA.Visible = False
            TXT_SCHEMA.Enabled = False
            CHK_NOT_EXITST_BG.Visible = False
            CHK_NOT_EXITST_USERS.Visible = False
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
            CHK_NOT_IDLE.Checked = False
            CHK_NOT_IDLE.Enabled = False
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
            '8の設定
            TXT_SQLID1.Visible = False
            TXT_SQLID1.Enabled = False
            Label1.Visible = False
            Label9.Visible = False
            TXT_SQLID2.Visible = False
            TXT_SQLID2.Enabled = False
            Label16.Visible = True
            TXT_SQL_ADDRESS.Visible = True
            TXT_SQL_ADDRESS.Enabled = True
            Label17.Visible = True
            TXT_SQL_HASH.Visible = True
            TXT_SQL_HASH.Enabled = True
            Label14.Visible = False
            TXT_SCHEMA.Visible = False
            TXT_SCHEMA.Enabled = False

            Label3.Visible = False
            Label4.Visible = False
            Label5.Visible = False
            Label6.Visible = False
            TXT_AVG_GE.Text = vbNullString
            TXT_AVG_GE.Visible = False
            TXT_AVG_GE.Enabled = False
            TXT_AVG_LE.Text = vbNullString
            TXT_AVG_LE.Visible = False
            TXT_AVG_LE.Enabled = False
            TXT_SUM_GE.Text = vbNullString
            TXT_SUM_GE.Visible = False
            TXT_SUM_GE.Enabled = False
            TXT_SUM_LE.Text = vbNullString
            TXT_SUM_LE.Visible = False
            TXT_SUM_LE.Enabled = False
            CHK_NOT_IDLE.Checked = False
            CHK_NOT_IDLE.Enabled = False

            '8iまではV$SQL_PLANがないためSQLカーソルの実行計画確認機能は使用できない
            TAB_EXECUTE_PLAN.Dispose()
        End If

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 11 Then
            CLB_STAT_NAME.Items.Add("logical reads", True)
            CLB_STAT_NAME.Items.Add("db block changes", True)
            CLB_STAT_NAME.Items.Add("physical reads", True)
            CLB_STAT_NAME.Items.Add("physical reads direct", True)
            CLB_STAT_NAME.Items.Add("physical read requests", True)
            CLB_STAT_NAME.Items.Add("physical writes", True)
            CLB_STAT_NAME.Items.Add("physical writes direct", True)
            CLB_STAT_NAME.Items.Add("physical write requests", True)
            CLB_STAT_NAME.Items.Add("optimized physical reads", True)
            CLB_STAT_NAME.Items.Add("space used", True)
            CLB_STAT_NAME.Items.Add("segment scans", True)
            CLB_STAT_NAME.Items.Add("row lock waits", True)
            CLB_STAT_NAME.Items.Add("buffer busy waits", True)
            CLB_STAT_NAME.Items.Add("space allocated", True)
            CLB_STAT_NAME.Items.Add("ITL waits", True)
            CLB_STAT_NAME.Items.Add("gc buffer busy", True)
            CLB_STAT_NAME.Items.Add("gc cr blocks received", True)
            CLB_STAT_NAME.Items.Add("gc current blocks received", True)
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 Then
            CLB_STAT_NAME.Items.Add("logical reads", True)
            CLB_STAT_NAME.Items.Add("db block changes", True)
            CLB_STAT_NAME.Items.Add("physical reads", True)
            CLB_STAT_NAME.Items.Add("physical reads direct", True)
            CLB_STAT_NAME.Items.Add("physical writes", True)
            CLB_STAT_NAME.Items.Add("physical writes direct", True)
            CLB_STAT_NAME.Items.Add("space used", True)
            CLB_STAT_NAME.Items.Add("segment scans", True)
            CLB_STAT_NAME.Items.Add("row lock waits", True)
            CLB_STAT_NAME.Items.Add("buffer busy waits", True)
            CLB_STAT_NAME.Items.Add("space allocated", True)
            CLB_STAT_NAME.Items.Add("ITL waits", True)
            CLB_STAT_NAME.Items.Add("gc buffer busy", True)
            CLB_STAT_NAME.Items.Add("gc cr blocks received", True)
            CLB_STAT_NAME.Items.Add("gc current blocks received", True)
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2 Then
            CLB_STAT_NAME.Items.Add("logical reads", True)
            CLB_STAT_NAME.Items.Add("db block changes", True)
            CLB_STAT_NAME.Items.Add("physical reads", True)
            CLB_STAT_NAME.Items.Add("physical reads direct", True)
            CLB_STAT_NAME.Items.Add("physical writes", True)
            CLB_STAT_NAME.Items.Add("physical writes direct", True)
            CLB_STAT_NAME.Items.Add("row lock waits", True)
            CLB_STAT_NAME.Items.Add("buffer busy waits", True)
            CLB_STAT_NAME.Items.Add("ITL waits", True)
            CLB_STAT_NAME.Items.Add("global cache cr blocks served", True)
            CLB_STAT_NAME.Items.Add("global cache current blocks served", True)
        End If

        CMB_SORT_ORDER.SelectedIndex = 0
        CMB_SORT_VALUE.SelectedIndex = 0
        CMB_GROUPBY1.SelectedIndex = 1
        CMB_GROUPBY2.SelectedIndex = 2
        CMB_GROUPBY3.SelectedIndex = 0
        CMB_GROUPBY4.SelectedIndex = 0
        CMB_STAT_GROUP1.SelectedIndex = 0
        CMB_STAT_GROUP2.SelectedIndex = 0
        CMB_STAT_GROUP3.SelectedIndex = 0
        CMB_STAT_GROUP4.SelectedIndex = 0
        CMB_STAT_GROUP5.SelectedIndex = 0
        CMB_STAT_GROUP6.SelectedIndex = 0
        CMB_BUFFER_POOL.SelectedIndex = 0
        CMB_LOCK.SelectedIndex = 0

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) <= 8 Or (Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 0) Then
            '～9.0の場合
            tabControl1.TabPages("TAB_SEGMENT_STAT").Dispose()
        End If

        TXT_CREATE_SCHEMA.Text = frm_Login.G_DB.GET_LOGIN_USER

        Me.Text = "O-Analyzer - SGA情報(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
        Me.Width = 800

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub TabControl1_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles tabControl1.Selecting
        If tabControl1.SelectedIndex = 0 Then

        ElseIf tabControl1.SelectedIndex = 1 Then

        End If
    End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If tabControl1.SelectedTab.Name = "TAB_BUFFER" Then
            DispBufferCache()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SQL_CURSOR" Then
            DispSQL()
        ElseIf tabControl1.SelectedTab.Name = "TAB_EXECUTE_PLAN" Then
            If CheckInputValue() Then
                DispExecutionPlan()
            Else
                MsgBox(M_COMMON.GetMessage("E0010"))
            End If
        ElseIf tabControl1.SelectedTab.Name = "TAB_WAIT_EVENT" Then
            DispWaitEvent()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SYSTEM_STAT" Then
            DispSystemStat()
        ElseIf tabControl1.SelectedTab.Name = "TAB_INIT_PARAM" Then
            DispParameter()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SGA_COMPONENT" Then
            DispSharedPool()
        ElseIf tabControl1.SelectedTab.Name = "TAB_HIT_RATIO" Then
            DispStatistics()
        ElseIf tabControl1.SelectedTab.Name = "TAB_LATCH" Then
            DispLatch()
        ElseIf tabControl1.SelectedTab.Name = "TAB_LOCK" Then
            DispLock()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SEGMENT_STAT" Then
            DispSegmentStat()
        Else
        End If
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        'M_CANCELFLG = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CREATE_TABLE.Click
        CreateTempTable()
    End Sub

    Private Sub DGV_SQL_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_SQL.CurrentCellChanged
        If Not (DGV_SQL.DataSource Is Nothing) And Not (DGV_SQL.CurrentRow Is Nothing) Then
            If DGV_SQL.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) < 10 Then
                '10g～
                If IsDBNull(DGV_SQL.CurrentRow.Cells(0).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(1).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(2).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(3).Value) Then
                    Exit Sub
                End If
                TXT_SQL_ADDRESS.Text = DGV_SQL.CurrentRow.Cells(0).Value
                TXT_SQL_HASH.Text = DGV_SQL.CurrentRow.Cells(1).Value
                TXT_CHILD_NUM.Text = DGV_SQL.CurrentRow.Cells(3).Value
                TXT_FULL_SQL.Text = DGV_SQL.CurrentRow.Cells(2).Value
            Else
                '～9i
                If IsDBNull(DGV_SQL.CurrentRow.Cells(0).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(1).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(2).Value) Then
                    Exit Sub
                End If

                TXT_SQLID2.Text = DGV_SQL.CurrentRow.Cells(0).Value
                TXT_CHILD_NUM.Text = DGV_SQL.CurrentRow.Cells(2).Value
                TXT_FULL_SQL.Text = DGV_SQL.CurrentRow.Cells(1).Value
            End If
        End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
    Handles TXT_SUM_LE.KeyPress, TXT_SUM_GE.KeyPress, TXT_AVG_LE.KeyPress, TXT_AVG_GE.KeyPress, TXT_CHILD_NUM.KeyPress
        '数字以外の入力を無効化
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) _
         And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub DGV_1_NewRowNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) _
    Handles DGV_SQL.NewRowNeeded
        If frm_Login.G_DB.GET_SELECT_FLG Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
            FetchFromResultset(DGV_SQL, Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If
    End Sub

    Private Sub EXCUTING_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
    Handles DGV_SQL.KeyDown, TXT_SQL.KeyDown, BTN_EXEC.KeyDown, tabControl1.KeyDown
        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub frm_SGA_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) _
    Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        'M_COMMON.disposeControls(Me.Controls)
    End Sub

    Private Sub DGV_DICTIONARY_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) _
        Handles DGV_DICTIONARY.RowPostPaint, DGV_LATCH.RowPostPaint, DGV_LIBRARY.RowPostPaint, DGV_BUFFER.RowPostPaint, DGV_SGA_CMP_SIZE.RowPostPaint, DGV_PARAM.RowPostPaint, DGV_SYSSTAT.RowPostPaint, DGV_WAIT_EVENT.RowPostPaint, DGV_EXECUTION_PLAN.RowPostPaint, DGV_SQL.RowPostPaint, DGV_BH.RowPostPaint, DGV_LOCK.RowPostPaint, DGV_DEPEND.RowPostPaint, DGV_SEGMENT_STAT.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub DGV_EXECUTING_PLAN_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_EXECUTION_PLAN.CurrentCellChanged
        'データグリッドビューにデータがない場合は処理を抜ける
        If (DGV_EXECUTION_PLAN.DataSource Is Nothing) Or (DGV_EXECUTION_PLAN.CurrentRow Is Nothing) Then
            Exit Sub
        End If
        If DGV_EXECUTION_PLAN.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        If Not IsDBNull(DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.ColumnCount - 2).Value) Then
            TXT_ACCESS_PREDICATE.Text = DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.ColumnCount - 2).Value
        Else
            TXT_ACCESS_PREDICATE.Text = vbNullString
        End If
        If Not IsDBNull(DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.ColumnCount - 1).Value) Then
            TXT_FILTER_PREDICATE.Text = DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.ColumnCount - 1).Value
        Else
            TXT_FILTER_PREDICATE.Text = vbNullString
        End If

    End Sub

    Private Sub frm_info_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        tabControl1.Width = GroupBox1.Location.X - 10
        tabControl1.Height = Me.Height - LBL_INFO.Height - 50

        TAB_BUFFER.Width = tabControl1.Width - 10
        TAB_BUFFER.Height = tabControl1.Height - 10
        TAB_SQL_CURSOR.Width = tabControl1.Width - 10
        TAB_SQL_CURSOR.Height = tabControl1.Height - 10
        TAB_EXECUTE_PLAN.Width = tabControl1.Width - 10
        TAB_EXECUTE_PLAN.Height = tabControl1.Height - 10
        TAB_LOCK.Width = tabControl1.Width - 10
        TAB_LOCK.Height = tabControl1.Height - 10
        TAB_SEGMENT_STAT.Width = tabControl1.Width - 10
        TAB_SEGMENT_STAT.Height = tabControl1.Height - 10

        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        GroupBox13.Width = tabControl1.Width - 20
        GroupBox14.Width = tabControl1.Width - 20
        GroupBox14.Height = tabControl1.Height - GroupBox13.Height - 45

        '実行計画画面
        SplitContainer1.Width = TAB_EXECUTE_PLAN.Width - 10
        SplitContainer1.Height = (TAB_EXECUTE_PLAN.Height - GroupBox7.Height - 15)
        TXT_FULL_SQL.Width = SplitContainer1.Panel1.Width - 2
        TXT_FULL_SQL.Height = SplitContainer1.Panel1.Height - 20
        DGV_EXECUTION_PLAN.Width = SplitContainer1.Panel1.Width - 2
        DGV_EXECUTION_PLAN.Height = SplitContainer2.Panel1.Height - 20
        TXT_ACCESS_PREDICATE.Width = SplitContainer3.Panel1.Width - 2
        TXT_ACCESS_PREDICATE.Height = SplitContainer3.Panel1.Height - 20
        TXT_FILTER_PREDICATE.Width = SplitContainer3.Panel2.Width - 2
        TXT_FILTER_PREDICATE.Height = SplitContainer3.Panel2.Height - 20

        DGV_SQL.Width = tabControl1.Width - 20
        DGV_SQL.Height = tabControl1.Height - GroupBox6.Height - GroupBox3.Height - 45
        DGV_BH.Width = tabControl1.Width - 20
        DGV_BH.Height = tabControl1.Height - 155
        DGV_WAIT_EVENT.Width = tabControl1.Width - 20
        DGV_WAIT_EVENT.Height = tabControl1.Height - 85
        DGV_SYSSTAT.Width = tabControl1.Width - 20
        DGV_SYSSTAT.Height = tabControl1.Height - 85
        DGV_PARAM.Width = tabControl1.Width - 20
        DGV_PARAM.Height = tabControl1.Height - 85
        DGV_LATCH.Width = tabControl1.Width - 20
        DGV_LATCH.Height = tabControl1.Height - 85
        DGV_SGA_CMP_SIZE.Width = tabControl1.Width - 20
        DGV_SGA_CMP_SIZE.Height = tabControl1.Height - 85
        DGV_BUFFER.Width = tabControl1.Width - 35
        Label25.Location = New Point(5, (GroupBox14.Height / 2))
        DGV_DICTIONARY.Location = New Point(5, (GroupBox14.Height / 2) + Label25.Height + 5)
        DGV_LIBRARY.Width = tabControl1.Width - 35
        DGV_LIBRARY.Height = (GroupBox14.Height / 2) - Label25.Height - 30
        DGV_DICTIONARY.Width = tabControl1.Width - 35
        DGV_DICTIONARY.Height = GroupBox14.Height / 2 - Label25.Height - 10

        'ロック画面
        SplitContainer4.Width = TAB_LOCK.Width - 10
        SplitContainer4.Height = (TAB_LOCK.Height - GroupBox17.Height - 10)
        DGV_LOCK.Width = SplitContainer4.Panel1.Width - 2
        DGV_LOCK.Height = SplitContainer4.Panel1.Height - 4
        DGV_DEPEND.Width = SplitContainer4.Panel2.Width - 2
        DGV_DEPEND.Height = SplitContainer4.Panel2.Height - 4

        'セグメント統計画面
        DGV_SEGMENT_STAT.Width = tabControl1.Width - 20
        DGV_SEGMENT_STAT.Height = tabControl1.Height - 155

        LBL_INFO.Location = New Point(5, Me.Height - 55)

    End Sub

    Private Sub SplitContainer_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize, SplitContainer2.Panel1.Resize, SplitContainer3.Panel1.Resize
        TXT_FULL_SQL.Height = SplitContainer1.Panel1.Height - 20
        TXT_ACCESS_PREDICATE.Width = SplitContainer3.Panel1.Width - 2
        TXT_ACCESS_PREDICATE.Height = SplitContainer3.Panel1.Height - 20
    End Sub

    Private Sub SplitContainer2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel1.Resize
        DGV_EXECUTION_PLAN.Height = SplitContainer2.Panel1.Height - 20
    End Sub

    Private Sub SplitContainer3_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer3.Panel2.Resize
        TXT_FILTER_PREDICATE.Width = SplitContainer3.Panel2.Width - 2
        TXT_FILTER_PREDICATE.Height = SplitContainer3.Panel2.Height - 20
    End Sub

    Private Sub SplitContainer4_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer4.Panel1.Resize
        DGV_LOCK.Height = SplitContainer4.Panel1.Height - 4
    End Sub

    Private Sub SplitContainer4_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer4.Panel2.Resize
        DGV_DEPEND.Height = SplitContainer4.Panel2.Height - 4
    End Sub

    Private Sub DGV_LOCK_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_LOCK.CurrentCellChanged
        'データグリッドビューにデータがない場合は処理を抜ける
        If (DGV_LOCK.DataSource Is Nothing) Or (DGV_LOCK.CurrentRow Is Nothing) Then
            Exit Sub
        End If

        'ロック情報表示
        DispSession()
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub
End Class