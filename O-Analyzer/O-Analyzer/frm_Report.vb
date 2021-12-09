Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_Report
    '#######################################################################
    'レポート解析画面
    '#######################################################################

    ''定数
    'SQL条件
    Private Const C_SQL_SNAP As Integer = 0
    Private Const C_SQL_SYSTEM_WAIT As Integer = 1
    Private Const C_SQL_SYSSTAT As Integer = 2
    Private Const C_SQL_SQL_SUMMARY As Integer = 3
    Private Const C_SQL_FULLTEXT As Integer = 4
    Private Const C_SQL_PLANS As Integer = 5
    Private Const C_SQL_EXECUTION_PLAN As Integer = 6
    Private Const C_SQL_TIME_MODEL As Integer = 7
    Private Const C_SQL_VERSION As Integer = 8

    'V$BHの集約条件
    Private Const C_BH_NONE = 0        '集約なし
    Private Const C_BH_TABLESPACE = 1  '表領域
    Private Const C_BH_OBJECT = 2      'オブジェクト
    Private Const C_BH_STATUS = 3      'ステータス
    Private Const C_BH_BLOCKCLASS = 4  'ブロッククラス

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
    Private M_BEGIN_ID As String = vbNullString
    Private M_END_ID As String = vbNullString
    Private M_DBID As String = vbNullString
    Private M_INST_NUM As String = vbNullString
    Private M_VERSION() As String = {0, 0, 0, 0, 0}

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
        If Integer.Parse(M_VERSION(0)) < 10 Then
            If TXT_SQL_HASH.Text = vbNullString Then
                Return False
            End If
        Else
            If TXT_SQLID2.Text = vbNullString Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub DispSnap()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim rowstr(7) As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_SNAP.DataSource = Nothing
        DGV_SNAP.RowCount = 0

        '処理対象スナップ情報初期化
        M_BEGIN_ID = vbNullString
        M_END_ID = vbNullString
        M_DBID = vbNullString
        M_INST_NUM = vbNullString
        Label32.Text = vbNullString

        'スナップ一覧の取得
        sql = CreateSQL(C_SQL_SNAP)
        If frm_Login.G_DB.SET_RESULTSET(sql) = False Then
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        While frm_Login.G_DB.FETCH_ROW_TO_STRING(rowstr)
            DGV_SNAP.Rows.Add(False, rowstr(0), rowstr(1), rowstr(2), rowstr(3), rowstr(4), rowstr(5))
            cnt = cnt + 1
        End While

        DGV_SNAP.AutoResizeColumns()
        msgVal(0) = cnt
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSysStat()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_SYSSTAT.DataSource = Nothing
        bindingSource.DataSource = Nothing

        'システム統計の取得
        sql = CreateSQL(C_SQL_SYSSTAT)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_SYSSTAT.DataSource = bindingSource
        msgVal(0) = dataset.Tables(0).Rows.Count.ToString

        DGV_SYSSTAT.AutoResizeColumns()
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSystemWait()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_WAIT_EVENT.DataSource = Nothing
        bindingSource.DataSource = Nothing

        'システム統計の取得
        sql = CreateSQL(C_SQL_SYSTEM_WAIT)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_WAIT_EVENT.DataSource = bindingSource
        msgVal(0) = dataset.Tables(0).Rows.Count.ToString

        DGV_WAIT_EVENT.AutoResizeColumns()
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispSqlSummary()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_SQL.DataSource = Nothing
        bindingSource.DataSource = Nothing

        'SQLサマリの取得
        sql = CreateSQL(C_SQL_SQL_SUMMARY)
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultset(DGV_SQL, Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
        Else
            frm_Login.G_DB.SET_SELECT_FLG(False)
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If

        DGV_SQL.AutoResizeColumns()
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub DispTimeModel()
        Dim sql As String
        Dim cnt As Long = 0
        Dim msgVal(0) As String
        Dim dataset As New DataSet()
        Dim bindingSource As New BindingSource

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")

        Me.Refresh()
        DGV_TIME_MODEL.DataSource = Nothing
        bindingSource.DataSource = Nothing

        'システム時間統計の取得
        sql = CreateSQL(C_SQL_TIME_MODEL)
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        bindingSource.DataSource = dataset.Tables(0)
        DGV_TIME_MODEL.DataSource = bindingSource
        msgVal(0) = dataset.Tables(0).Rows.Count.ToString

        DGV_TIME_MODEL.AutoResizeColumns()
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msgVal)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub GetVersion()
        Dim sql As String

        sql = CreateSQL(C_SQL_VERSION)

        Using dataset As New DataSet
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then

                'SQL全文表示
                For Each row As Object In dataset.Tables(0).Rows
                    M_VERSION = row.Item(0).ToString.Split(".")
                Next

            Else
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
            End If
        End Using


    End Sub

    Private Function ExecQuery(ByVal sql As String, ByRef DGV As DataGridView) As Boolean
        '#######################################################################
        'SELECT実行
        '#######################################################################

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

        If id = C_SQL_SNAP Then
            'スナップ一覧取得
            sql = sql & "SELECT SNAP_ID,DBID,INSTANCE_NUMBER,SNAP_TIME,STARTUP_TIME,SNAP_LEVEL "
            If TXT_PERFSTAT_SCHEMA.Text <> vbNullString Then
                sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SNAPSHOT "
            Else
                sql = sql & "FROM STATS$SNAPSHOT "
            End If

            sql = sql & "WHERE 1=1 "
            If TXT_BEGIN_ID.Text <> vbNullString Then
                sql = sql & "AND SNAP_ID>=" & TXT_BEGIN_ID.Text & " "
            End If
            If TXT_END_ID.Text <> vbNullString Then
                sql = sql & "AND SNAP_ID<=" & TXT_END_ID.Text & " "
            End If
            If TXT_DBID.Text <> vbNullString Then
                sql = sql & "AND DBID=" & TXT_DBID.Text & " "
            End If
            If TXT_INST_NUM.Text <> vbNullString Then
                sql = sql & "AND INSTANCE_NUMBER=" & TXT_INST_NUM.Text & " "
            End If
            sql = sql & "ORDER BY DBID ASC,INSTANCE_NUMBER ASC,SNAP_ID DESC"
        ElseIf id = C_SQL_SYSTEM_WAIT Then
            '待機イベント取得
            If Integer.Parse(M_VERSION(0)) >= 9 Then
                sql = "SELECT E.EVENT,"
                If CHK_BG_WAIT.Checked Then
                    'sql = sql & "B.TOTAL_WAITS_FG BEGIN_TOTAL_WAITS_FG,E.TOTAL_WAITS_FG END_TOTAL_WAITS_FG,"
                    sql = sql & "NVL(E.TOTAL_WAITS_FG,0)-NVL(B.TOTAL_WAITS_FG,0) DELTA_TOTAL_WAITS,"
                    'sql = sql & "B.TOTAL_TIMEOUTS_FG BEGIN_TOTAL_TIMEOUTS_FG,E.TOTAL_TIMEOUTS_FG END_TOTAL_TIMEOUTS_FG,"
                    sql = sql & "NVL(E.TOTAL_TIMEOUTS_FG,0)-NVL(B.TOTAL_TIMEOUTS_FG,0) DELTA_TOTAL_TIMEOUTS,"
                    'sql = sql & "B.TIME_WAITED_MICRO_FG BEGIN_TIME_WAITED_MICRO_FG,E.TIME_WAITED_MICRO_FG END_TIME_WAITED_MICRO_FG,"
                    sql = sql & "NVL(E.TIME_WAITED_MICRO_FG,0)-NVL(B.TIME_WAITED_MICRO_FG,0) DELTA_TIME_WAITED_MICRO "
                Else
                    'sql = sql & "B.TOTAL_WAITS BEGIN_TOTAL_WAITS,E.TOTAL_WAITS END_TOTAL_WAITS,"
                    sql = sql & "NVL(E.TOTAL_WAITS,0)-NVL(B.TOTAL_WAITS,0) DELTA_TOTAL_WAITS,"
                    'sql = sql & "B.TOTAL_TIMEOUTS BEGIN_TOTAL_TIMEOUTS,E.TOTAL_TIMEOUTS END_TOTAL_TIMEOUTS,"
                    sql = sql & "NVL(E.TOTAL_TIMEOUTS,0)-NVL(B.TOTAL_TIMEOUTS,0) DELTA_TOTAL_TIMEOUTS,"
                    'sql = sql & "B.TIME_WAITED_MICRO BEGIN_TIME_WAITED_MICRO,E.TIME_WAITED_MICRO END_TIME_WAITED_MICRO,"
                    sql = sql & "NVL(E.TIME_WAITED_MICRO,0)-NVL(B.TIME_WAITED_MICRO,0) DELTA_TIME_WAITED_MICRO "
                End If
                sql = sql & "FROM "
                sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSTEM_EVENT "
                sql = sql & " WHERE SNAP_ID=" & M_BEGIN_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") B,"
                sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSTEM_EVENT "
                sql = sql & " WHERE SNAP_ID=" & M_END_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") E "
                sql = sql & "WHERE "

                If Integer.Parse(M_VERSION(0)) >= 10 Then
                    sql = sql & "E.EVENT_ID=B.EVENT_ID(+) "
                Else
                    sql = sql & "E.EVENT=B.EVENT(+) "
                End If

                If TXT_USER_DEFINE2.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE2.Text & " "
                End If

                If CHK_IDLE_WAIT.Checked Then
                    sql = sql & "AND E.EVENT NOT IN (SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$IDLE_EVENT) "
                End If

                sql = sql & "ORDER BY DELTA_TIME_WAITED_MICRO DESC"
            ElseIf Integer.Parse(M_VERSION(0)) = 8 Then
                sql = "SELECT E.EVENT,"
                sql = sql & "B.TOTAL_WAITS BEGIN_TOTAL_WAITS,E.TOTAL_WAITS END_TOTAL_WAITS,NVL(E.TOTAL_WAITS,0)-NVL(B.TOTAL_WAITS,0) DELTA_TOTAL_WAITS,"
                sql = sql & "B.TOTAL_TIMEOUTS BEGIN_TOTAL_TIMEOUTS,E.TOTAL_TIMEOUTS END_TOTAL_TIMEOUTS,NVL(E.TOTAL_TIMEOUTS,0)-NVL(B.TOTAL_TIMEOUTS,0) DELTA_TOTAL_TIMEOUTS,"
                sql = sql & "B.TIME_WAITED BEGIN_TIME_WAITED,E.TIME_WAITED END_TIME_WAITED,NVL(E.TIME_WAITED,0)-NVL(B.TIME_WAITED,0) DELTA_TIME_WAITED "
                sql = sql & "FROM "
                sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSTEM_EVENT "
                sql = sql & " WHERE SNAP_ID=" & M_BEGIN_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") B,"
                sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSTEM_EVENT "
                sql = sql & " WHERE SNAP_ID=" & M_END_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") E "
                sql = sql & "WHERE "
                sql = sql & "E.EVENT=B.EVENT(+) "

                If TXT_USER_DEFINE2.Text <> vbNullString Then
                    sql = sql & "AND " & TXT_USER_DEFINE2.Text & " "
                End If
                sql = sql & "ORDER BY DELTA_TIME_WAITED DESC"
            End If

        ElseIf id = C_SQL_SYSSTAT Then
            'システム統計取得
            sql = "SELECT E.NAME,"
            sql = sql & "B.VALUE BEGIN_VALUE,E.VALUE END_VALUE,NVL(E.VALUE,0) - NVL(B.VALUE,0) DELTA_VALUE "
            sql = sql & "FROM "
            sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSSTAT "
            sql = sql & " WHERE SNAP_ID=" & M_BEGIN_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") B,"
            sql = sql & "(SELECT * FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYSSTAT "
            sql = sql & " WHERE SNAP_ID=" & M_END_ID & " AND DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM & ") E "
            sql = sql & "WHERE "
            sql = sql & "E.NAME = B.NAME(+) "

            If TXT_USER_DEFINE3.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE3.Text & " "
            End If
            sql = sql & "ORDER BY E.NAME"
        ElseIf id = C_SQL_SQL_SUMMARY Then
            'SQLサマリ
            sql = "SELECT * FROM ("
            '11g
            If Integer.Parse(M_VERSION(0)) = 11 Then
                'BEGIN～ENDまでのSQLサマリの増加分を表示するSQL
                '①BEGINスナップで取得されているデータは合算しない
                '②ageout等により前回スナップと比較して値が小さくなっている場合は前回スナップの値を合算しない
                '③ADDRESSと前回スナップのADDRESSが異なる場合は前回スナップの値を合算しない
                sql = sql & "SELECT "
                sql = sql & " SQL_ID,"
                sql = sql & " OLD_HASH_VALUE,"
                sql = sql & " TEXT_SUBSET,"
                sql = sql & " MODULE,"
                sql = sql & " DELTA_EXECUTIONS,"
                sql = sql & " DELTA_ELAPSED_TIME,"
                sql = sql & " TRUNC(DELTA_ELAPSED_TIME/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS),0) AVG_ELAPSED_TIME,"
                sql = sql & " DELTA_CPU_TIME,"
                sql = sql & " TRUNC(DELTA_CPU_TIME/DECODE(DELTA_ELAPSED_TIME,0,1,DELTA_ELAPSED_TIME)*100,1) ""CPU_TIME(%)"","
                sql = sql & " DELTA_APPLICATION_WAIT_TIME,"
                sql = sql & " DELTA_CONCURRENCY_WAIT_TIME,"
                sql = sql & " DELTA_USER_IO_WAIT_TIME,"
                sql = sql & " DELTA_CLUSTER_WAIT_TIME,"
                sql = sql & " DELTA_ROWS_PROCESSED,"
                sql = sql & " DELTA_BUFFER_GETS,"
                sql = sql & " TRUNC(DELTA_BUFFER_GETS/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS),1) AVG_BUFFER_GETS,"
                sql = sql & " DELTA_DISK_READS,"
                sql = sql & " TRUNC((DELTA_BUFFER_GETS-DELTA_DISK_READS)/DECODE(DELTA_BUFFER_GETS,0,1,DELTA_BUFFER_GETS)*100,1) ""CACHE_HIT(%)"","
                sql = sql & " DELTA_DIRECT_WRITES,"
                sql = sql & " DELTA_PARSE_CALLS,"
                sql = sql & " MAX_SHARABLE_MEM,"
                sql = sql & " LAST_SHARABLE_MEM,"
                sql = sql & " DELTA_VERSION_COUNT,"
                sql = sql & " MAX_VERSION_COUNT,"
                sql = sql & " LAST_VERSION_COUNT "
                sql = sql & "FROM "
                sql = sql & "(SELECT "
                sql = sql & "  OLD_HASH_VALUE,"
                sql = sql & "  TEXT_SUBSET,"
                sql = sql & "  SQL_ID,"
                sql = sql & "  MODULE,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE "
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (BUFFER_GETS < PREV_BUFFER_GETS) THEN BUFFER_GETS"
                sql = sql & "       ELSE BUFFER_GETS - PREV_BUFFER_GETS"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_BUFFER_GETS,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (EXECUTIONS < PREV_EXECUTIONS) THEN EXECUTIONS"
                sql = sql & "       ELSE EXECUTIONS - PREV_EXECUTIONS"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_EXECUTIONS,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (CPU_TIME < PREV_CPU_TIME)"
                sql = sql & "       THEN CPU_TIME"
                sql = sql & "       ELSE CPU_TIME - PREV_CPU_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_CPU_TIME,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (ELAPSED_TIME < PREV_ELAPSED_TIME)"
                sql = sql & "       THEN ELAPSED_TIME"
                sql = sql & "       ELSE ELAPSED_TIME - PREV_ELAPSED_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_ELAPSED_TIME,"
                sql = sql & "  AVG(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE AVG_HARD_PARSE_TIME"
                sql = sql & "      END) AVG_HARD_PARSE_TIME,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (DISK_READS < PREV_DISK_READS)"
                sql = sql & "       THEN DISK_READS"
                sql = sql & "       ELSE DISK_READS - PREV_DISK_READS"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_DISK_READS,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (PARSE_CALLS < PREV_PARSE_CALLS)"
                sql = sql & "       THEN PARSE_CALLS"
                sql = sql & "       ELSE PARSE_CALLS - PREV_PARSE_CALLS"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_PARSE_CALLS,"
                sql = sql & "  MAX(SHARABLE_MEM) MAX_SHARABLE_MEM,"
                sql = sql & "  SUM(CASE WHEN SNAP_ID = " & M_END_ID & " THEN LAST_SHARABLE_MEM"
                sql = sql & "      ELSE 0"
                sql = sql & "      END) LAST_SHARABLE_MEM,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (VERSION_COUNT < PREV_VERSION_COUNT)"
                sql = sql & "       THEN VERSION_COUNT"
                sql = sql & "       ELSE VERSION_COUNT - PREV_VERSION_COUNT"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_VERSION_COUNT,"
                sql = sql & "  MAX(VERSION_COUNT) MAX_VERSION_COUNT,"
                sql = sql & "  SUM(CASE WHEN SNAP_ID = " & M_END_ID & " THEN LAST_VERSION_COUNT"
                sql = sql & "      ELSE 0"
                sql = sql & "      END) LAST_VERSION_COUNT,"

                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (DIRECT_WRITES < PREV_DIRECT_WRITES)"
                sql = sql & "       THEN DIRECT_WRITES"
                sql = sql & "       ELSE DIRECT_WRITES - PREV_DIRECT_WRITES"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_DIRECT_WRITES,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (APPLICATION_WAIT_TIME < PREV_APPLICATION_WAIT_TIME)"
                sql = sql & "       THEN APPLICATION_WAIT_TIME"
                sql = sql & "       ELSE APPLICATION_WAIT_TIME - PREV_APPLICATION_WAIT_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_APPLICATION_WAIT_TIME,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (CONCURRENCY_WAIT_TIME < PREV_CONCURRENCY_WAIT_TIME)"
                sql = sql & "       THEN CONCURRENCY_WAIT_TIME"
                sql = sql & "       ELSE CONCURRENCY_WAIT_TIME - PREV_CONCURRENCY_WAIT_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_CONCURRENCY_WAIT_TIME,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (USER_IO_WAIT_TIME < PREV_USER_IO_WAIT_TIME)"
                sql = sql & "       THEN USER_IO_WAIT_TIME"
                sql = sql & "       ELSE USER_IO_WAIT_TIME - PREV_USER_IO_WAIT_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_USER_IO_WAIT_TIME,"

                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (CLUSTER_WAIT_TIME < PREV_CLUSTER_WAIT_TIME)"
                sql = sql & "       THEN CLUSTER_WAIT_TIME"
                sql = sql & "       ELSE CLUSTER_WAIT_TIME - PREV_CLUSTER_WAIT_TIME"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_CLUSTER_WAIT_TIME,"
                sql = sql & "  SUM(CASE"
                sql = sql & "      WHEN SNAP_ID = " & M_BEGIN_ID & " AND PREV_SNAP_ID = -1 THEN 0"
                sql = sql & "      ELSE"
                sql = sql & "       CASE WHEN (ADDRESS != PREV_ADDRESS) OR (ROWS_PROCESSED < PREV_ROWS_PROCESSED)"
                sql = sql & "       THEN ROWS_PROCESSED"
                sql = sql & "       ELSE ROWS_PROCESSED - PREV_ROWS_PROCESSED"
                sql = sql & "       END"
                sql = sql & "      END) DELTA_ROWS_PROCESSED"
                sql = sql & " FROM "
                sql = sql & "  (SELECT /*+ FIRST_ROWS */"
                sql = sql & "    SNAP_ID,"
                sql = sql & "    OLD_HASH_VALUE,"
                sql = sql & "    TEXT_SUBSET,"
                sql = sql & "    SQL_ID,"
                sql = sql & "    MODULE,"
                sql = sql & "    (LAG(SNAP_ID, 1, -1) "
                sql = sql & "     OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_SNAP_ID,"
                sql = sql & "    (LEAD(SNAP_ID, 1, -1)"
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) NEXT_SNAP_ID,"
                sql = sql & "    ADDRESS,"
                sql = sql & "    (LAG(ADDRESS, 1, HEXTORAW(0)) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_ADDRESS,"
                sql = sql & "    BUFFER_GETS,"
                sql = sql & "    (LAG(BUFFER_GETS, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_BUFFER_GETS,"
                sql = sql & "    CPU_TIME,"
                sql = sql & "    (LAG(CPU_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_CPU_TIME,"
                sql = sql & "    EXECUTIONS,"
                sql = sql & "    (LAG(EXECUTIONS, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_EXECUTIONS,"
                sql = sql & "    ELAPSED_TIME,"
                sql = sql & "    (LAG(ELAPSED_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_ELAPSED_TIME,"
                sql = sql & "    AVG_HARD_PARSE_TIME,"
                sql = sql & "    DISK_READS,"
                sql = sql & "    (LAG(DISK_READS, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_DISK_READS,"
                sql = sql & "    PARSE_CALLS,"
                sql = sql & "    (LAG(PARSE_CALLS, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_PARSE_CALLS,"
                sql = sql & "     SHARABLE_MEM,"
                sql = sql & "    (LAST_VALUE(SHARABLE_MEM) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) LAST_SHARABLE_MEM,"
                sql = sql & "    (LAG(SHARABLE_MEM, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_SHARABLE_MEM,"
                sql = sql & "     VERSION_COUNT,"
                sql = sql & "    (LAG(VERSION_COUNT, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_VERSION_COUNT,"
                sql = sql & "    (LAST_VALUE(VERSION_COUNT) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) LAST_VERSION_COUNT,"

                'オリジナルから追加
                sql = sql & "    DIRECT_WRITES,"
                sql = sql & "    (LAG(DIRECT_WRITES, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_DIRECT_WRITES,"
                sql = sql & "    APPLICATION_WAIT_TIME,"
                sql = sql & "    (LAG(APPLICATION_WAIT_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_APPLICATION_WAIT_TIME,"
                sql = sql & "    CONCURRENCY_WAIT_TIME,"
                sql = sql & "    (LAG(CONCURRENCY_WAIT_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_CONCURRENCY_WAIT_TIME,"
                sql = sql & "    USER_IO_WAIT_TIME,"
                sql = sql & "    (LAG(USER_IO_WAIT_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_USER_IO_WAIT_TIME,"
                sql = sql & "    CLUSTER_WAIT_TIME,"
                sql = sql & "    (LAG(CLUSTER_WAIT_TIME, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_CLUSTER_WAIT_TIME,"
                sql = sql & "    ROWS_PROCESSED,"
                sql = sql & "    (LAG(ROWS_PROCESSED, 1, 0) "
                sql = sql & "      OVER (PARTITION BY OLD_HASH_VALUE, DBID, INSTANCE_NUMBER ORDER BY SNAP_ID)"
                sql = sql & "    ) PREV_ROWS_PROCESSED"
                sql = sql & "   FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY S"
                sql = sql & "   WHERE S.SNAP_ID BETWEEN " & M_BEGIN_ID & " AND " & M_END_ID
                sql = sql & "    AND S.DBID = " & M_DBID
                sql = sql & "    AND S.INSTANCE_NUMBER = " & M_INST_NUM
                sql = sql & "   )"
                sql = sql & " GROUP BY "
                sql = sql & "  OLD_HASH_VALUE, TEXT_SUBSET,SQL_ID, MODULE"
                sql = sql & " )"
                sql = sql & "WHERE "
                sql = sql & " DELTA_BUFFER_GETS       > 0 OR"
                sql = sql & " DELTA_EXECUTIONS        > 0 OR"
                sql = sql & " DELTA_CPU_TIME          > 0 OR"
                sql = sql & " DELTA_DISK_READS        > 0 OR"
                sql = sql & " DELTA_PARSE_CALLS       > 0 OR"
                sql = sql & " MAX_SHARABLE_MEM        > 0 OR"
                sql = sql & " MAX_VERSION_COUNT       > 0 OR"
                sql = sql & " DELTA_CLUSTER_WAIT_TIME > 0"

            ElseIf Integer.Parse(M_VERSION(0)) = 10 Then
                sql = sql & "SELECT "
                sql = sql & " SQL_ID,"
                sql = sql & " OLD_HASH_VALUE,"
                sql = sql & " TEXT_SUBSET,"
                sql = sql & " MODULE,"
                sql = sql & " DELTA_EXECUTIONS,"
                sql = sql & " DELTA_ELAPSED_TIME,"
                sql = sql & " TRUNC(DELTA_ELAPSED_TIME/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS),0) ""AVG_ELAPSED_TIME"","
                sql = sql & " DELTA_CPU_TIME,"
                sql = sql & " TRUNC(DELTA_CPU_TIME/DECODE(DELTA_ELAPSED_TIME,0,1,DELTA_ELAPSED_TIME)*100,1) ""CPU_TIME(%)"","
                sql = sql & " DELTA_APPLICATION_WAIT_TIME,"
                sql = sql & " DELTA_CONCURRENCY_WAIT_TIME,"
                sql = sql & " DELTA_USER_IO_WAIT_TIME,"
                sql = sql & " DELTA_CLUSTER_WAIT_TIME,"
                sql = sql & " DELTA_ROWS_PROCESSED,"
                sql = sql & " DELTA_BUFFER_GETS,"
                sql = sql & " TRUNC(DELTA_BUFFER_GETS/DECODE(DELTA_DISK_READS,0,1,DELTA_DISK_READS),1) AVG_BUFFER_GETS,"
                sql = sql & " DELTA_DISK_READS,"
                sql = sql & " TRUNC((DELTA_BUFFER_GETS-DELTA_DISK_READS)/DECODE(DELTA_BUFFER_GETS,0,1,DELTA_BUFFER_GETS)*100,1) ""CACHE_HIT(%)"","
                sql = sql & " DELTA_DIRECT_WRITES,"
                sql = sql & " DELTA_SORTS,"
                sql = sql & " DELTA_FETCHES,"
                sql = sql & " DELTA_DELTA_PARSE_CALLS,"
                sql = sql & " SHARABLE_MEM,"
                sql = sql & " LOADED_VERSIONS,"
                sql = sql & " LOADS,"
                sql = sql & " DELTA_INVALIDATIONS,"
                sql = sql & " DELTA_VERSION_COUNT "
                sql = sql & "FROM ("
                sql = sql & "SELECT "
                sql = sql & " E.SQL_ID,"
                sql = sql & " E.OLD_HASH_VALUE,"
                sql = sql & " E.TEXT_SUBSET,"
                sql = sql & " E.MODULE,"
                sql = sql & " E.EXECUTIONS - NVL(B.EXECUTIONS,0) DELTA_EXECUTIONS,"
                sql = sql & " E.ELAPSED_TIME - NVL(B.ELAPSED_TIME,0) DELTA_ELAPSED_TIME,"
                sql = sql & " E.CPU_TIME - NVL(B.CPU_TIME,0) DELTA_CPU_TIME,"
                sql = sql & " E.APPLICATION_WAIT_TIME - NVL(B.APPLICATION_WAIT_TIME,0) DELTA_APPLICATION_WAIT_TIME,"
                sql = sql & " E.CONCURRENCY_WAIT_TIME - NVL(B.CONCURRENCY_WAIT_TIME,0) DELTA_CONCURRENCY_WAIT_TIME,"
                sql = sql & " E.USER_IO_WAIT_TIME - NVL(B.USER_IO_WAIT_TIME,0) DELTA_USER_IO_WAIT_TIME,"
                sql = sql & " E.CLUSTER_WAIT_TIME - NVL(B.CLUSTER_WAIT_TIME,0) DELTA_CLUSTER_WAIT_TIME,"
                sql = sql & " E.ROWS_PROCESSED - NVL(B.ROWS_PROCESSED,0) DELTA_ROWS_PROCESSED,"
                sql = sql & " E.BUFFER_GETS - NVL(B.BUFFER_GETS,0) DELTA_BUFFER_GETS,"
                sql = sql & " E.DISK_READS - NVL(B.DISK_READS,0) DELTA_DISK_READS,"
                sql = sql & " E.DIRECT_WRITES - NVL(B.DIRECT_WRITES,0) DELTA_DIRECT_WRITES,"
                sql = sql & " E.SORTS - NVL(B.SORTS,0) DELTA_SORTS,"
                sql = sql & " E.FETCHES - NVL(B.FETCHES,0) DELTA_FETCHES,"
                sql = sql & " E.PARSE_CALLS - NVL(B.PARSE_CALLS,0) DELTA_DELTA_PARSE_CALLS,"
                sql = sql & " E.SHARABLE_MEM,"
                sql = sql & " E.LOADED_VERSIONS,"
                sql = sql & " E.LOADS,"
                sql = sql & " E.INVALIDATIONS-NVL(B.INVALIDATIONS,0) DELTA_INVALIDATIONS,"
                sql = sql & " E.VERSION_COUNT-NVL(B.VERSION_COUNT,0) DELTA_VERSION_COUNT "
                sql = sql & "FROM "
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY E,"
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY B "
                sql = sql & "WHERE B.SNAP_ID(+)        = " & M_BEGIN_ID
                sql = sql & " AND B.DBID(+)            = " & M_DBID
                sql = sql & " AND B.INSTANCE_NUMBER(+) = " & M_INST_NUM
                sql = sql & " AND B.DBID(+)            = E.DBID"
                sql = sql & " AND B.INSTANCE_NUMBER(+) = E.INSTANCE_NUMBER"
                sql = sql & " AND B.OLD_HASH_VALUE(+)  = E.OLD_HASH_VALUE"
                sql = sql & " AND B.ADDRESS(+)         = E.ADDRESS"
                sql = sql & " AND B.TEXT_SUBSET(+)     = E.TEXT_SUBSET"
                sql = sql & " AND E.SNAP_ID            = " & M_END_ID
                sql = sql & " AND E.DBID               = " & M_DBID
                sql = sql & " AND E.INSTANCE_NUMBER    = " & M_INST_NUM
                sql = sql & " AND E.EXECUTIONS         > NVL(B.EXECUTIONS(+),0) "
                sql = sql & ") "
            ElseIf Integer.Parse(M_VERSION(0)) = 9 Then
                sql = sql & "SELECT "
                sql = sql & " E.HASH_VALUE,"
                sql = sql & " E.TEXT_SUBSET,"
                sql = sql & " E.MODULE,"
                sql = sql & " E.EXECUTIONS - NVL(B.EXECUTIONS,0) DELTA_EXECUTIONS,"
                sql = sql & " E.CPU_TIME - NVL(B.CPU_TIME,0) DELTA_CPU_TIME,"
                sql = sql & " E.ELAPSED_TIME - NVL(B.ELAPSED_TIME,0) DELTA_ELAPSED_TIME,"
                sql = sql & " E.BUFFER_GETS - NVL(B.BUFFER_GETS,0) DELTA_BUFFER_GETS, "
                sql = sql & " E.DISK_READS - NVL(B.DISK_READS,0) DELTA_DISK_READS, "
                sql = sql & " E.SORTS - NVL(B.SORTS,0) DELTA_SORTS, "
                sql = sql & " E.FETCHES - NVL(B.FETCHES,0) DELTA_FETCHES, "
                sql = sql & " E.PARSE_CALLS - NVL(B.PARSE_CALLS,0) DELTA_DELTA_PARSE_CALLS, "
                sql = sql & " E.ROWS_PROCESSED - NVL(B.ROWS_PROCESSED,0) DELTA_ROWS_PROCESSED "

                sql = sql & "FROM "
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY E, "
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY B "
                sql = sql & "WHERE B.SNAP_ID(+)        = " & M_BEGIN_ID
                sql = sql & " AND B.DBID(+)            = " & M_DBID
                sql = sql & " AND B.INSTANCE_NUMBER(+) = " & M_INST_NUM
                sql = sql & " AND B.DBID(+)            = E.DBID"
                sql = sql & " AND B.INSTANCE_NUMBER(+) = E.INSTANCE_NUMBER"
                sql = sql & " AND B.ADDRESS(+)         = E.ADDRESS"
                sql = sql & " AND B.HASH_VALUE(+)      = E.HASH_VALUE"
                sql = sql & " AND B.TEXT_SUBSET(+)     = E.TEXT_SUBSET"
                sql = sql & " AND E.SNAP_ID            = " & M_END_ID
                sql = sql & " AND E.DBID               = " & M_DBID
                sql = sql & " AND E.INSTANCE_NUMBER    = " & M_INST_NUM
                sql = sql & " AND E.EXECUTIONS         > NVL(B.EXECUTIONS(+),0)"
            ElseIf Integer.Parse(M_VERSION(0)) < 9 Then
                sql = sql & "SELECT "
                sql = sql & " E.HASH_VALUE,"
                sql = sql & " E.TEXT_SUBSET,"
                sql = sql & " E.MODULE,"
                sql = sql & " E.EXECUTIONS - NVL(B.EXECUTIONS,0) DELTA_EXECUTIONS,"
                sql = sql & " E.BUFFER_GETS - NVL(B.BUFFER_GETS,0) DELTA_BUFFER_GETS, "
                sql = sql & " E.DISK_READS - NVL(B.DISK_READS,0) DELTA_DISK_READS, "
                sql = sql & " E.SORTS - NVL(B.SORTS,0) DELTA_SORTS, "
                sql = sql & " E.PARSE_CALLS - NVL(B.PARSE_CALLS,0) DELTA_DELTA_PARSE_CALLS, "
                sql = sql & " E.ROWS_PROCESSED - NVL(B.ROWS_PROCESSED,0) DELTA_ROWS_PROCESSED "

                sql = sql & "FROM "
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY E, "
                sql = sql & " " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY B "
                sql = sql & "WHERE B.SNAP_ID(+)        = " & M_BEGIN_ID
                sql = sql & " AND B.DBID(+)            = " & M_DBID
                sql = sql & " AND B.INSTANCE_NUMBER(+) = " & M_INST_NUM
                sql = sql & " AND B.DBID(+)            = E.DBID"
                sql = sql & " AND B.INSTANCE_NUMBER(+) = E.INSTANCE_NUMBER"
                sql = sql & " AND B.ADDRESS(+)         = E.ADDRESS"
                sql = sql & " AND B.HASH_VALUE(+)      = E.HASH_VALUE"
                sql = sql & " AND B.TEXT_SUBSET(+)     = E.TEXT_SUBSET"
                sql = sql & " AND E.SNAP_ID            = " & M_END_ID
                sql = sql & " AND E.DBID               = " & M_DBID
                sql = sql & " AND E.INSTANCE_NUMBER    = " & M_INST_NUM
                sql = sql & " AND E.EXECUTIONS         > NVL(B.EXECUTIONS(+),0)"
            End If
            sql = sql & ") WHERE 1=1 "

            If TXT_SQLID1.Text <> vbNullString Then
                sql = sql & " AND SQL_ID = '" & TXT_SQLID1.Text & "' "
            End If

            If TXT_MODULE.Text <> vbNullString Then
                sql = sql & " AND UPPER(MODULE) LIKE UPPER('" & TXT_MODULE.Text & "') "
            End If

            If TXT_AVG_GE.Text <> vbNullString Then
                sql = sql & " AND DECODE(DELTA_EXECUTIONS,0,DELTA_ELAPSED_TIME/1,DELTA_ELAPSED_TIME/DELTA_EXECUTIONS) >= " & TXT_AVG_GE.Text & " "
            End If
            If TXT_AVG_LE.Text <> vbNullString Then
                sql = sql & " AND DECODE(DELTA_EXECUTIONS,0,DELTA_ELAPSED_TIME/1,DELTA_ELAPSED_TIME/DELTA_EXECUTIONS) <= " & TXT_AVG_LE.Text & " "
            End If

            If TXT_SUM_GE.Text <> vbNullString Then
                sql = sql & " AND DELTA_ELAPSED_TIME >= " & TXT_SUM_GE.Text & " "
            End If
            If TXT_SUM_LE.Text <> vbNullString Then
                sql = sql & " AND DELTA_ELAPSED_TIME <= " & TXT_SUM_LE.Text & " "
            End If

            If TXT_USER_DEFINE.Text <> vbNullString Then
                sql = sql & "AND " & TXT_USER_DEFINE.Text
            End If

            If Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) <> C_CURSOR_SORT_NONE Then
                sql = sql & "ORDER BY "
                If Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 2)) = C_CURSOR_SORT_EXECUTIONS Then
                    '実行回数
                    sql = sql & "DELTA_EXECUTIONS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_READ_BUFF Then
                    '合計バッファ読込ブロック数
                    sql = sql & "DELTA_BUFFER_GETS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_READ_BUFF Then
                    '平均バッファ読込ブロック数
                    sql = sql & "DELTA_BUFFER_GETS/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_READ_DISK Then
                    '合計DISK読込ブロック数
                    sql = sql & "DELTA_DISK_READS "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_READ_DISK Then
                    '平均DISK読込ブロック数
                    sql = sql & "DELTA_DISK_READS/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_ELAPSED_TIME Then
                    '合計処理時間
                    sql = sql & "DELTA_ELAPSED_TIME "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_ELAPSED_TIME Then
                    '平均処理時間
                    sql = sql & "DELTA_ELAPSED_TIME/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS) "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_SUM_WRITE_DISK Then
                    '合計DISK書込ブロック数
                    sql = sql & "DELTA_DIRECT_WRITES "
                ElseIf Integer.Parse(CMB_SORT_VALUE.SelectedItem.ToString.Substring(0, 1)) = C_CURSOR_SORT_AVG_WRITE_DISK Then
                    '平均DISK書込ブロック数
                    sql = sql & "DELTA_DIRECT_WRITES/DECODE(DELTA_EXECUTIONS,0,1,DELTA_EXECUTIONS) "
                End If
                sql = sql & CMB_SORT_ORDER.SelectedItem.ToString & " "
            End If

        ElseIf id = C_SQL_FULLTEXT Then
            'SQL全文取得
            If Integer.Parse(M_VERSION(0)) >= 10 Then
                sql = sql & "SELECT A.SQL_TEXT "
                sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQLTEXT A, "
                sql = sql & "(SELECT OLD_HASH_VALUE,TEXT_SUBSET "
                sql = sql & " FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY B "
                sql = sql & " WHERE SNAP_ID BETWEEN " & M_BEGIN_ID & " AND " & M_END_ID & " "
                sql = sql & "  AND B.DBID=" & M_DBID & " "
                sql = sql & "  AND B.INSTANCE_NUMBER=" & M_INST_NUM & " "
                sql = sql & "  AND B.OLD_HASH_VALUE=" & TXT_SQL_HASH.Text & " "
                sql = sql & " GROUP BY OLD_HASH_VALUE,TEXT_SUBSET) C "
                sql = sql & "WHERE "
                sql = sql & " C.TEXT_SUBSET=A.TEXT_SUBSET "
                sql = sql & " AND C.OLD_HASH_VALUE=A.OLD_HASH_VALUE "
                sql = sql & "ORDER BY PIECE "
            Else
                sql = sql & "SELECT A.SQL_TEXT "
                sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQLTEXT A, " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_SUMMARY B "
                sql = sql & "WHERE "
                sql = sql & " B.SNAP_ID=" & M_END_ID & " "
                sql = sql & " AND B.DBID=" & M_DBID & " "
                sql = sql & " AND B.INSTANCE_NUMBER=" & M_INST_NUM & " "
                sql = sql & " AND B.HASH_VALUE=" & TXT_SQL_HASH.Text & " "
                sql = sql & " AND B.TEXT_SUBSET=A.TEXT_SUBSET "
                sql = sql & " AND B.HASH_VALUE=A.HASH_VALUE "
                sql = sql & "ORDER BY PIECE "
            End If
        ElseIf id = C_SQL_PLANS Then
            'プラン一覧
            sql = "SELECT PLAN_HASH_VALUE,COST "
            If Integer.Parse(M_VERSION(0)) >= 11 Or _
              (Integer.Parse(M_VERSION(0)) = 10 And Integer.Parse(M_VERSION(1)) = 2) Then
                sql = sql & ",MAX(LAST_ACTIVE_TIME) LAST_ACTIVE_TIME "
            End If
            sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_PLAN_USAGE "
            If Integer.Parse(M_VERSION(0)) >= 10 Then
                sql = sql & "WHERE SNAP_ID BETWEEN " & M_BEGIN_ID & " AND " & M_END_ID
                sql = sql & " AND DBID(+)            = " & M_DBID & " "
                sql = sql & " AND INSTANCE_NUMBER(+) = " & M_INST_NUM & " "
                If RBN_OLD_HASH_VALUE.Checked = True Then
                    sql = sql & " AND OLD_HASH_VALUE=" & TXT_SQL_HASH.Text & " "
                Else
                    sql = sql & " AND SQL_ID='" & TXT_SQLID2.Text & "' "
                End If

                sql = sql & "GROUP BY PLAN_HASH_VALUE,COST "
            ElseIf Integer.Parse(M_VERSION(0)) < 10 Then
                sql = sql & "WHERE SNAP_ID BETWEEN " & M_BEGIN_ID & " AND " & M_END_ID
                sql = sql & " AND DBID(+)            = " & M_DBID & " "
                sql = sql & " AND INSTANCE_NUMBER(+) = " & M_INST_NUM & " "
                sql = sql & " AND HASH_VALUE=" & TXT_SQL_HASH.Text & " "
                sql = sql & "GROUP BY PLAN_HASH_VALUE,COST "
            End If
            sql = sql & "ORDER BY 1"
        ElseIf id = C_SQL_EXECUTION_PLAN Then
            '実行計画表示
            If Integer.Parse(M_VERSION(0)) >= 10 Then
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || NVL2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || NVL2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "A.OBJECT_NAME,A.CARDINALITY,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,QBLOCK_NAME,A.TIME*1000 ""TIME(ms)"","
                sql = sql & "A.TEMP_SPACE,A.PROJECTION "
            ElseIf Integer.Parse(M_VERSION(0)) = 9 And Integer.Parse(M_VERSION(1)) = 2 Then
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || NVL2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || NVL2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "A.OBJECT_NAME,A.CARDINALITY,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,A.TEMP_SPACE,NULL PROJECTION "
            ElseIf (Integer.Parse(M_VERSION(0)) = 9 And Integer.Parse(M_VERSION(1)) = 0) Or Integer.Parse(M_VERSION(0)) <= 8 Then
                sql = "SELECT A.ID,DEPTH, "
                sql = sql & "LPAD(' ',2*(DEPTH-1)) || A.OPERATION || nvl2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || nvl2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION , "
                sql = sql & "A.OBJECT_NAME,NULL CARDINALITY,A.BYTES,A.COST,A.CPU_COST,A.IO_COST,A.TEMP_SPACE,NULL PROJECTION "
            End If
            sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SQL_PLAN A "
            sql = sql & "WHERE PLAN_HASH_VALUE = " & DGV_PLANS.CurrentRow.Cells(DGV_PLANS.Columns("PLAN_HASH_VALUE").Index).Value.ToString & " "
            sql = sql & "ORDER BY ID"
        ElseIf id = C_SQL_TIME_MODEL Then
            '時間統計表示
            If Integer.Parse(M_VERSION(0)) >= 10 Then
                sql = "SELECT /*+ ORDERED USE_NL(B N) */ "
                sql = sql & " N.STAT_NAME,(E.VALUE-NVL(B.VALUE,0)) DELTA_VALUE "
                sql = sql & "FROM "
                sql = sql & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYS_TIME_MODEL E, "
                sql = sql & TXT_PERFSTAT_SCHEMA.Text & ".STATS$SYS_TIME_MODEL B, "
                sql = sql & TXT_PERFSTAT_SCHEMA.Text & ".STATS$TIME_MODEL_STATNAME N "
                sql = sql & "WHERE B.SNAP_ID        = " & M_BEGIN_ID & " "
                sql = sql & " AND E.SNAP_ID         = " & M_END_ID & " "
                sql = sql & " AND E.DBID            = " & M_DBID & " "
                sql = sql & " AND B.DBID            = " & M_DBID & " "
                sql = sql & " AND E.DBID            = B.DBID "
                sql = sql & " AND E.INSTANCE_NUMBER = " & M_INST_NUM & " "
                sql = sql & " AND B.INSTANCE_NUMBER = " & M_INST_NUM & " "
                sql = sql & " AND E.INSTANCE_NUMBER = B.INSTANCE_NUMBER "
                sql = sql & " AND E.STAT_ID         = N.STAT_ID "
                sql = sql & " AND B.STAT_ID         = N.STAT_ID "
                sql = sql & "ORDER BY DELTA_VALUE DESC"
            End If
        ElseIf id = C_SQL_VERSION Then
            sql = "SELECT DISTINCT VERSION "
            sql = sql & "FROM " & TXT_PERFSTAT_SCHEMA.Text & ".STATS$DATABASE_INSTANCE "
            sql = sql & "WHERE DBID=" & M_DBID & " AND INSTANCE_NUMBER=" & M_INST_NUM
        End If

        Return sql
    End Function

    Private Sub DispSqlAndPlans()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        DGV_EXECUTION_PLAN.DataSource = Nothing
        Me.Refresh()

        sql = CreateSQL(C_SQL_FULLTEXT)

        Using dataset As New DataSet
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then
                TXT_FULL_SQL.Text = vbNullString
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
        sql = CreateSQL(C_SQL_PLANS)

        If ExecQuery(sql, DGV_PLANS) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If
        DGV_PLANS.AutoResizeColumns()

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
    End Sub

    Private Sub DispExecutionPlan()
        Dim sql As String

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        sql = CreateSQL(C_SQL_EXECUTION_PLAN)

        If ExecQuery(sql, DGV_EXECUTION_PLAN) = False Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        'プロジェクションは画面下部で表示させるため非表示
        DGV_EXECUTION_PLAN.Columns.Item(DGV_EXECUTION_PLAN.Columns("PROJECTION").Index).Visible = False

        DGV_EXECUTION_PLAN.AutoResizeColumns()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString

    End Sub

    Private Sub frm_SGA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LBL_BEGIN_TIME.Text = vbNullString
        LBL_END_TIME.Text = vbNullString
        Label32.Text = vbNullString
        RBN_OLD_HASH_VALUE.Checked = True

        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_NONE & " なし")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_EXECUTIONS & " 実行回数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_READ_BUFF & " 合計バッファ読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_READ_BUFF & " 平均バッファ読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_READ_DISK & " 合計DISK読込ブロック数")
        CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_READ_DISK & " 平均DISK読込ブロック数")

        '今後追加するかもしれない
        TAB_LATCH.Dispose()
        TAB_SEGMENT_STAT.Dispose()
        TAB_SGA_COMPONENT.Dispose()

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            '10g～の設定
            TXT_SQLID1.Visible = True
            TXT_SQLID1.Enabled = True
            Label1.Visible = True
            RBN_SQL_ID.Visible = True
            TXT_SQLID2.Visible = True
            TXT_SQLID2.Enabled = True
            RBN_OLD_HASH_VALUE.Visible = True
            TXT_SQL_HASH.Visible = True
            TXT_SQL_HASH.Enabled = True
            TXT_SQLID2.Enabled = True
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_WRITE_DISK & " 合計DIRECT WRITEブロック数")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_WRITE_DISK & " 平均DIRECT WRITEブロック数")
            If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 10 Then
                CHK_BG_WAIT.Checked = False
                CHK_BG_WAIT.Visible = False
                CHK_BG_WAIT.Enabled = False
            End If
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 Then
            '9iの設定
            TXT_SQLID1.Visible = False
            TXT_SQLID1.Enabled = False
            Label1.Visible = False
            RBN_SQL_ID.Visible = False
            RBN_SQL_ID.Enabled = False
            TXT_SQLID2.Visible = False
            TXT_SQLID2.Enabled = False
            RBN_OLD_HASH_VALUE.Visible = True
            RBN_OLD_HASH_VALUE.Text = "HASH_VALUE"
            TXT_SQL_HASH.Visible = True
            TXT_SQL_HASH.Enabled = True
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
            CHK_BG_WAIT.Checked = False
            CHK_BG_WAIT.Visible = False
            CHK_BG_WAIT.Enabled = False

            '時間統計は10g以降
            TAB_TIME_MODEL.Dispose()
        ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 8 Then
            '8の設定
            TXT_SQLID1.Visible = False
            TXT_SQLID1.Enabled = False
            Label1.Visible = False
            RBN_SQL_ID.Visible = False
            RBN_SQL_ID.Enabled = False
            TXT_SQLID2.Visible = False
            TXT_SQLID2.Enabled = False
            RBN_OLD_HASH_VALUE.Visible = True
            RBN_OLD_HASH_VALUE.Text = "HASH_VALUE"
            TXT_SQL_HASH.Visible = True
            TXT_SQL_HASH.Enabled = True

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

            CHK_IDLE_WAIT.Checked = False
            CHK_IDLE_WAIT.Visible = False
            CHK_IDLE_WAIT.Enabled = False
            CHK_BG_WAIT.Checked = False
            CHK_BG_WAIT.Visible = False
            CHK_BG_WAIT.Enabled = False

            '8iまではSTATS$SQL_PLANがないため実行計画確認機能は使用できない
            TAB_EXECUTE_PLAN.Dispose()
            '時間統計は10g以降
            TAB_TIME_MODEL.Dispose()
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
        CMB_STAT_GROUP1.SelectedIndex = 0
        CMB_STAT_GROUP2.SelectedIndex = 0
        CMB_STAT_GROUP3.SelectedIndex = 0
        CMB_STAT_GROUP4.SelectedIndex = 0
        CMB_STAT_GROUP5.SelectedIndex = 0
        CMB_STAT_GROUP6.SelectedIndex = 0

        TXT_PERFSTAT_SCHEMA.Text = frm_Login.G_DB.GET_LOGIN_USER

        Me.Text = "O-Analyzer - レポート解析(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
        Me.Width = 800

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
    End Sub

    'Private Sub TabControl1_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles tabControl1.Selecting
    '    If tabControl1.SelectedIndex <> 0 Then
    '        If Integer.Parse(M_VERSION(0)) >= 10 Then
    '            '10g～の設定
    '            TXT_SQLID1.Visible = True
    '            TXT_SQLID1.Enabled = True
    '            Label1.Visible = True
    '            RBN_SQL_ID.Visible = True
    '            TXT_SQLID2.Visible = True
    '            TXT_SQLID2.Enabled = True
    '            RBN_OLD_HASH_VALUE.Visible = True
    '            TXT_SQL_HASH.Visible = True
    '            TXT_SQL_HASH.Enabled = True
    '            TXT_SQLID2.Enabled = True
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_WRITE_DISK & " 合計DIRECT WRITEブロック数")
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_WRITE_DISK & " 平均DIRECT WRITEブロック数")
    '            If Integer.Parse(M_VERSION(0)) = 10 Then
    '                CHK_BG_WAIT.Checked = False
    '                CHK_BG_WAIT.Visible = False
    '                CHK_BG_WAIT.Enabled = False
    '            End If
    '        ElseIf Integer.Parse(M_VERSION(0)) = 9 Then
    '            '9iの設定
    '            TXT_SQLID1.Visible = False
    '            TXT_SQLID1.Enabled = False
    '            Label1.Visible = False
    '            RBN_SQL_ID.Visible = False
    '            RBN_SQL_ID.Enabled = False
    '            TXT_SQLID2.Visible = False
    '            TXT_SQLID2.Enabled = False
    '            RBN_OLD_HASH_VALUE.Visible = True
    '            RBN_OLD_HASH_VALUE.Text = "HASH_VALUE"
    '            TXT_SQL_HASH.Visible = True
    '            TXT_SQL_HASH.Enabled = True
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_SUM_ELAPSED_TIME & " 合計処理時間")
    '            CMB_SORT_VALUE.Items.Add(C_CURSOR_SORT_AVG_ELAPSED_TIME & " 平均処理時間")
    '            CHK_BG_WAIT.Checked = False
    '            CHK_BG_WAIT.Visible = False
    '            CHK_BG_WAIT.Enabled = False
    '        ElseIf Integer.Parse(M_VERSION(0)) = 8 Then
    '            '8の設定
    '            TXT_SQLID1.Visible = False
    '            TXT_SQLID1.Enabled = False
    '            Label1.Visible = False
    '            RBN_SQL_ID.Visible = False
    '            RBN_SQL_ID.Enabled = False
    '            TXT_SQLID2.Visible = False
    '            TXT_SQLID2.Enabled = False
    '            RBN_OLD_HASH_VALUE.Visible = True
    '            RBN_OLD_HASH_VALUE.Text = "HASH_VALUE"
    '            TXT_SQL_HASH.Visible = True
    '            TXT_SQL_HASH.Enabled = True

    '            Label3.Visible = False
    '            Label4.Visible = False
    '            Label5.Visible = False
    '            Label6.Visible = False
    '            TXT_AVG_GE.Text = vbNullString
    '            TXT_AVG_GE.Visible = False
    '            TXT_AVG_GE.Enabled = False
    '            TXT_AVG_LE.Text = vbNullString
    '            TXT_AVG_LE.Visible = False
    '            TXT_AVG_LE.Enabled = False
    '            TXT_SUM_GE.Text = vbNullString
    '            TXT_SUM_GE.Visible = False
    '            TXT_SUM_GE.Enabled = False
    '            TXT_SUM_LE.Text = vbNullString
    '            TXT_SUM_LE.Visible = False
    '            TXT_SUM_LE.Enabled = False

    '            CHK_IDLE_WAIT.Checked = False
    '            CHK_IDLE_WAIT.Visible = False
    '            CHK_IDLE_WAIT.Enabled = False
    '            CHK_BG_WAIT.Checked = False
    '            CHK_BG_WAIT.Visible = False
    '            CHK_BG_WAIT.Enabled = False
    '        End If


    '    End If
    'End Sub

    Private Sub BTN_EXEC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXEC.Click
        If tabControl1.SelectedTab.Name <> "TAB_SNAP" Then
            If M_BEGIN_ID = vbNullString Or M_END_ID = vbNullString Or _
                M_DBID = vbNullString Or M_INST_NUM = vbNullString Then
                MsgBox("スナップショットを選択してください")
                Exit Sub
            End If

            If M_VERSION(0) = 0 Then
                GetVersion()
            End If
        End If

        If tabControl1.SelectedTab.Name = "TAB_SNAP" Then
            DispSnap()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SQL_CURSOR" Then
            DispSqlSummary()
        ElseIf tabControl1.SelectedTab.Name = "TAB_EXECUTE_PLAN" Then
            If CheckInputValue() Then
                DispSqlAndPlans()
            Else
                MsgBox(M_COMMON.GetMessage("E0010"))
            End If

        ElseIf tabControl1.SelectedTab.Name = "TAB_WAIT_EVENT" Then
            DispSystemWait()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SYSTEM_STAT" Then
            DispSysStat()
        ElseIf tabControl1.SelectedTab.Name = "TAB_INIT_PARAM" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_SGA_COMPONENT" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_HIT_RATIO" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_LATCH" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_LOCK" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_SEGMENT_STAT" Then
        ElseIf tabControl1.SelectedTab.Name = "TAB_TIME_MODEL" Then
            DispTimeModel()
        Else
        End If
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        'M_CANCELFLG = True
    End Sub

    Private Sub DGV_SQL_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_SQL.CurrentCellChanged
        If Not (DGV_SQL.DataSource Is Nothing) And Not (DGV_SQL.CurrentRow Is Nothing) Then
            If DGV_SQL.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            If Integer.Parse(M_VERSION(0)) < 10 Then
                '～9i
                If IsDBNull(DGV_SQL.CurrentRow.Cells(0).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(1).Value) Then
                    Exit Sub
                End If
                TXT_SQL_HASH.Text = DGV_SQL.CurrentRow.Cells(0).Value
                TXT_FULL_SQL.Text = DGV_SQL.CurrentRow.Cells(1).Value
            Else
                '10g～
                If IsDBNull(DGV_SQL.CurrentRow.Cells(0).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(1).Value) Or _
                   IsDBNull(DGV_SQL.CurrentRow.Cells(2).Value) Then
                    Exit Sub
                End If

                TXT_SQLID2.Text = DGV_SQL.CurrentRow.Cells(0).Value
                TXT_SQL_HASH.Text = DGV_SQL.CurrentRow.Cells(1).Value
                TXT_FULL_SQL.Text = DGV_SQL.CurrentRow.Cells(2).Value
            End If
        End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
    Handles TXT_SUM_LE.KeyPress, TXT_SUM_GE.KeyPress, TXT_AVG_LE.KeyPress, TXT_AVG_GE.KeyPress
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
    Handles DGV_SQL.KeyDown, BTN_EXEC.KeyDown, tabControl1.KeyDown
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

    Private Sub frm_Report_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) _
    Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        'M_COMMON.disposeControls(Me.Controls)
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) _
        Handles DGV_LATCH.RowPostPaint, DGV_SGA_CMP_SIZE.RowPostPaint, DGV_SYSSTAT.RowPostPaint, DGV_WAIT_EVENT.RowPostPaint, DGV_EXECUTION_PLAN.RowPostPaint, DGV_SQL.RowPostPaint, DGV_SNAP.RowPostPaint, DGV_SEGMENT_STAT.RowPostPaint, DGV_TIME_MODEL.RowPostPaint
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

        If Not IsDBNull(DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.Columns("PROJECTION").Index).Value) Then
            TXT_ACCESS_PREDICATE.Text = DGV_EXECUTION_PLAN.CurrentRow.Cells(DGV_EXECUTION_PLAN.ColumnCount - 1).Value
        Else
            TXT_ACCESS_PREDICATE.Text = vbNullString
        End If

    End Sub

    Private Sub frm_info_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        tabControl1.Width = GroupBox1.Location.X - 10
        tabControl1.Height = Me.Height - LBL_INFO.Height - 50

        TAB_SNAP.Width = tabControl1.Width - 10
        TAB_SNAP.Height = tabControl1.Height - 10
        TAB_SQL_CURSOR.Width = tabControl1.Width - 10
        TAB_SQL_CURSOR.Height = tabControl1.Height - 10
        TAB_EXECUTE_PLAN.Width = tabControl1.Width - 10
        TAB_EXECUTE_PLAN.Height = tabControl1.Height - 10
        TAB_SEGMENT_STAT.Width = tabControl1.Width - 10
        TAB_SEGMENT_STAT.Height = tabControl1.Height - 10
        TAB_TIME_MODEL.Width = tabControl1.Width - 10
        TAB_TIME_MODEL.Height = tabControl1.Height - 10

        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50

        '実行計画画面
        SplitContainer1.Width = TAB_EXECUTE_PLAN.Width - 10
        SplitContainer1.Height = (TAB_EXECUTE_PLAN.Height - GroupBox7.Height - 15)
        TXT_FULL_SQL.Width = SplitContainer4.Panel1.Width - 10
        TXT_FULL_SQL.Height = SplitContainer1.Panel1.Height - 20
        DGV_PLANS.Height = SplitContainer1.Panel1.Height - 20

        DGV_EXECUTION_PLAN.Width = SplitContainer1.Panel1.Width - 2
        DGV_EXECUTION_PLAN.Height = SplitContainer2.Panel1.Height - 20
        TXT_ACCESS_PREDICATE.Width = SplitContainer2.Panel2.Width - 2
        TXT_ACCESS_PREDICATE.Height = SplitContainer2.Panel2.Height - 20

        DGV_SQL.Width = tabControl1.Width - 20
        DGV_SQL.Height = tabControl1.Height - GroupBox3.Height - 45
        DGV_SNAP.Width = tabControl1.Width - 20
        DGV_SNAP.Height = tabControl1.Height - 115
        DGV_WAIT_EVENT.Width = tabControl1.Width - 20
        DGV_WAIT_EVENT.Height = tabControl1.Height - 85
        DGV_SYSSTAT.Width = tabControl1.Width - 20
        DGV_SYSSTAT.Height = tabControl1.Height - 85
        DGV_LATCH.Width = tabControl1.Width - 20
        DGV_LATCH.Height = tabControl1.Height - 85
        DGV_SGA_CMP_SIZE.Width = tabControl1.Width - 20
        DGV_SGA_CMP_SIZE.Height = tabControl1.Height - 85

        'セグメント統計画面
        DGV_SEGMENT_STAT.Width = tabControl1.Width - 20
        DGV_SEGMENT_STAT.Height = tabControl1.Height - 155

        '時間統計画面
        DGV_TIME_MODEL.Width = tabControl1.Width - 20
        DGV_TIME_MODEL.Height = tabControl1.Height - 85

        LBL_INFO.Location = New Point(5, Me.Height - 55)

    End Sub

    Private Sub SplitContainer_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize, SplitContainer2.Panel1.Resize
        TXT_FULL_SQL.Height = SplitContainer1.Panel1.Height - 20
        DGV_PLANS.Height = SplitContainer1.Panel1.Height - 22
        TXT_ACCESS_PREDICATE.Width = SplitContainer2.Panel2.Width - 2
        TXT_ACCESS_PREDICATE.Height = SplitContainer2.Panel2.Height - 20
    End Sub

    Private Sub SplitContainer2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel1.Resize
        DGV_EXECUTION_PLAN.Height = SplitContainer2.Panel1.Height - 20
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    Private Sub DGV_SNAP_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV_SNAP.CellValueChanged
        Dim checkedCnt As Integer = 0
        Dim startupTime As String = vbNullString

        For Each row As DataGridViewRow In DGV_SNAP.Rows
            If row.Cells(0).Value = True Then
                checkedCnt = checkedCnt + 1
                If checkedCnt = 1 Then
                    LBL_END_TIME.Text = row.Cells(4).Value
                    M_BEGIN_ID = row.Cells(1).Value
                    M_END_ID = row.Cells(1).Value
                    M_DBID = row.Cells(2).Value
                    M_INST_NUM = row.Cells(3).Value
                    startupTime = row.Cells(5).Value
                    Label32.Text = vbNullString
                ElseIf checkedCnt = 2 Then
                    If M_DBID <> row.Cells(2).Value Then
                        MsgBox("異なるDBIDのスナップは選択できません")
                        row.Cells(0).Value = False
                        Exit For
                    End If

                    If M_INST_NUM <> row.Cells(3).Value Then
                        MsgBox("異なるインスタンス番号のスナップは選択できません")
                        row.Cells(0).Value = False
                        Exit For
                    End If

                    If startupTime <> row.Cells(5).Value Then
                        Label32.Text = "警告：選択期間中にインスタンスが再起動されています"
                    End If

                    LBL_BEGIN_TIME.Text = row.Cells(4).Value
                    M_BEGIN_ID = row.Cells(1).Value
                Else
                    row.Cells(0).Value = False
                End If
            End If
        Next

        If checkedCnt = 0 Then
            LBL_END_TIME.Text = vbNullString
            LBL_BEGIN_TIME.Text = vbNullString
            Label32.Text = vbNullString
            M_BEGIN_ID = vbNullString
            M_END_ID = vbNullString
            M_DBID = vbNullString
            M_INST_NUM = vbNullString
            Me.Text = "O-Analyzer - レポート解析(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        ElseIf checkedCnt = 1 Then
            LBL_BEGIN_TIME.Text = LBL_END_TIME.Text
            Label32.Text = vbNullString
            M_BEGIN_ID = M_END_ID
        End If
        Me.Text = "O-Analyzer - レポート解析(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ") " & LBL_BEGIN_TIME.Text & "～" & LBL_END_TIME.Text
    End Sub

    Private Sub SplitContainer4_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer4.Panel1.Resize, SplitContainer4.Panel2.Resize
        TXT_FULL_SQL.Width = SplitContainer4.Panel1.Width - 10
        DGV_PLANS.Width = SplitContainer4.Panel2.Width - 5
    End Sub

    Private Sub DGV_PLANS_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_PLANS.CurrentCellChanged
        'データグリッドビューにデータがない場合は処理を抜ける
        If (DGV_PLANS.DataSource Is Nothing) Then
            Exit Sub
        End If
        If (DGV_PLANS.CurrentRow Is Nothing) Then
            Exit Sub
        End If

        DispExecutionPlan()


    End Sub

End Class