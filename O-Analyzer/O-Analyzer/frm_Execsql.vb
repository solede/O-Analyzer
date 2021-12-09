Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE
Imports System.IO
Imports System.Text
Imports System.Threading

Public Class frm_Execsql
    '#######################################################################
    'SQL実行画面
    '#######################################################################

    '待機監視機能関連変数
    Private M_BG_THREAD As System.Threading.Thread 'バックグラウンドスレッド
    Private M_MIDDLEWARE As Integer = 0
    Private M_USER As String = vbNullString
    Private M_PASSWORD As String = vbNullString
    Private M_CONN_STR As String = vbNullString
    Private M_IS_SYSDBA As Boolean = False
    Private M_SQL As String = vbNullString
    Private M_ERR_MSG As String = vbNullString
    Private M_INFO_MSG As String = vbNullString
    Private M_CLS_DB As CLS_DB.CLS_DB
    Private M_SID As Integer = -1
    Private M_SERIAL As Integer = -1
    Private M_TIMEOUT As Integer = 0

    'モジュール変数
    Private M_CANCELFLG As Boolean = False
    Private M_EXEC_CNT As Integer = 0
    Private M_SQL_HISTORY(100) As String
    Private M_HISTORY_INDEX As Integer = 0
    Private M_DATATABLE As DataTable
    Private M_SET_READER_CNT As Integer

    Private Function ExecQuery(Optional ByVal isGetStatsMode As Boolean = False) As Boolean
        '#######################################################################
        'SELECT実行
        '#######################################################################
        Dim sql As String = vbNullString
        Dim timeoutValue As Integer = 0

        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)

        DGV_DATA.DataSource = Nothing

        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = TXT_SQL.Text
        If isGetStatsMode Then
            'M_COMMON.AddHintToSQL(sql, " /*+ gather_plan_statistics */ ")
            sql = "/* O-Analyzer " & DateTime.Now.ToString & " */ " & sql
        End If

        If RB_SELECT2.Checked Then
            '非接続型(OO4O以外)
            If frm_Login.G_DB.GET_DATA_ADAPTER(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                FetchFromDataset(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            End If
        Else
            '接続型
            DGV_DATA.RowCount = 1
            DGV_DATA.ColumnCount = 0

            If Not frm_Bind Is Nothing Then
                frm_Bind.SetBind(False)
            End If

            'SQL実行
            If frm_Login.G_DB.SET_RESULTSET(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                'DataGridView初期化
                frm_Login.G_DB.INIT_DATA_TABLE()
                'フェッチ&表示
                FetchFromResultset(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            Else
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
                Timer1.Enabled = False
                Timer1.Stop()
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                Return False
            End If

            ' '''''''''''''''''''''''スレッド実装バージョンテスト中
            ''SQL実行
            'Dim t2 As System.Threading.Thread

            't2 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker2))
            't2.Start()
            ''t2.Join()
            ' ''''''''''''''''''''''''スレッド実装バージョンテスト中
        End If

        '列幅調整
        If CHK_COLSIZE_REGULATE.Checked Then
            If Not DGV_DATA Is Nothing Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        End If
        Return True
    End Function

    Private Sub ExecQueryUsingLocalDataTable()
        '#######################################################################
        'ローカル定義のデータテーブルを使用してSELECT実行する(試作版)
        '#######################################################################

        Dim sql As String
        Dim timeoutValue As Integer = 0

        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)

        DGV_DATA.DataSource = Nothing

        'SELECT文発行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = TXT_SQL.Text

        If RB_SELECT2.Checked Then
            '非接続型(OO4O以外)
            If frm_Login.G_DB.GET_DATA_ADAPTER(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                FetchFromDataset(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            End If
        Else
            '接続型
            DGV_DATA.RowCount = 1
            DGV_DATA.ColumnCount = 0

            'SQL実行
            If frm_Login.G_DB.SET_RESULTSET(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                'DataGridView初期化
                ''''''''''''
                M_DATATABLE = New DataTable
                frm_Login.G_DB.INIT_LOCAL_DATA_TABLE(M_DATATABLE)
                'フェッチ&表示
                FetchFromResultsetToLocalDataTable(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
                ''''''''''''
            Else
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
                Timer1.Enabled = False
                Timer1.Stop()
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            End If

            ' '''''''''''''''''''''''スレッド実装バージョンテスト中
            ''SQL実行
            'Dim t2 As System.Threading.Thread

            't2 = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf worker2))
            't2.Start()
            ''t2.Join()
            ' ''''''''''''''''''''''''スレッド実装バージョンテスト中
        End If

        '列幅調整
        If CHK_COLSIZE_REGULATE.Checked Then
            If Not DGV_DATA Is Nothing Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        End If
    End Sub

    '''''''''''''''''''''''''スレッド実装バージョンテスト中
    'Private Function aaa() As Boolean
    '    Dim sql As String = TXT_SQL.Text
    '    Dim timeoutValue As Integer

    '    If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
    '        timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
    '    End If

    '    If frm_Login.G_DB.SET_RESULTSET(sql, timeoutValue) Then
    '        LBL_INFO.Text = M_COMMON.GetMessage("I0006")
    '        Me.Refresh()
    '        'DataGridView初期化
    '        frm_Login.G_DB.INIT_DATA_TABLE()
    '        'フェッチ&表示
    '        FetchFromResultset(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
    '    Else
    '        frm_Login.G_DB.SET_SELECT_FLG(False)
    '        LBL_INFO.Text = vbNullString
    '        Timer1.Enabled = False
    '        Timer1.Stop()
    '        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    '    End If

    '    '列幅調整
    '    If CHK_COLSIZE_REGULATE.Checked Then
    '        DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
    '    End If

    '    Return True
    'End Function
    'Delegate Sub DispWaitDelegate()
    'Sub worker2()
    '    Invoke(New DispWaitDelegate(AddressOf t2))
    'End Sub
    'Private Sub t2()
    '    aaa()
    'End Sub
    '''''''''''''''''''''''''スレッド実装バージョンテスト中


    Private Sub ExecExplain()
        '#######################################################################
        '実行計画取得
        '#######################################################################

        Dim sql As String
        Dim timeoutValue As Integer = 0
        Dim retCnt As Long
        Dim msgVal(0) As String
        Dim date_time As DateTime = DateTime.Now

        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        End If

        'EXPLAIN文実行
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = "EXPLAIN PLAN SET STATEMENT_ID = '" & date_time.ToString("yyyyMMddHHmmssfff") & "' FOR " & TXT_SQL.Text

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt) Then
            msgVal(0) = retCnt.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0010", msgVal)
        Else
            LBL_INFO.Text = ""
            Exit Sub
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)

        DGV_DATA.DataSource = Nothing

        '列幅自動調整
        If CHK_COLSIZE_REGULATE.Checked Then
            DGV_DATA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        Else
            DGV_DATA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If

        'SQL文の組み立て
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            sql = "SELECT ID,LPAD(' ',2*(DEPTH-1)) || OPERATION || ' ' || OPTIONS OPERATION, OBJECT_NAME,OBJECT_TYPE,CARDINALITY,BYTES,COST,CPU_COST,IO_COST,TIME,QBLOCK_NAME,"
            sql = sql & "OPTIMIZER,SEARCH_COLUMNS,POSITION,OTHER_TAG,PARTITION_START,PARTITION_STOP,PARTITION_ID,OTHER,DISTRIBUTION,TEMP_SPACE,ACCESS_PREDICATES,FILTER_PREDICATES"
            sql = sql & " FROM PLAN_TABLE WHERE STATEMENT_ID = '" & date_time.ToString("yyyyMMddHHmmssfff") & "' ORDER BY ID"
        Else
            sql = "SELECT ID,LPAD(' ',2*(LEVEL-1)) || OPERATION || ' ' || OPTIONS OPERATION, OBJECT_NAME,OBJECT_TYPE,CARDINALITY,BYTES,COST,CPU_COST,IO_COST,"
            sql = sql & "OPTIMIZER, SEARCH_COLUMNS, POSITION, OTHER_TAG, PARTITION_START, PARTITION_STOP, PARTITION_ID, OTHER, DISTRIBUTION, TEMP_SPACE, ACCESS_PREDICATES, FILTER_PREDICATES "
            sql = sql & "FROM(PLAN_TABLE) "
            sql = sql & "START WITH id = 0 AND statement_id = '" & date_time.ToString("yyyyMMddHHmmssfff") & "'"
            sql = sql & "CONNECT BY PRIOR id = parent_id AND STATEMENT_ID = '" & date_time.ToString("yyyyMMddHHmmssfff") & "' ORDER BY ID"
        End If

        If RB_SELECT2.Checked Then
            '非接続型(OO4O以外)
            Me.Refresh()

            If frm_Login.G_DB.GET_DATA_ADAPTER(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                FetchFromDataset(Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            End If
        Else
            '接続型
            DGV_DATA.RowCount = 1
            DGV_DATA.ColumnCount = 0

            'SQL実行
            If frm_Login.G_DB.SET_RESULTSET(sql, timeoutValue) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                'DataGridView初期化
                frm_Login.G_DB.INIT_DATA_TABLE()
                'フェッチ&表示
                FetchFromResultset(Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))

                Dim isPartitionQuery As Boolean = False
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                    '16,17,18列目がパーティション情報
                    For Each DatagridviewRow As DataGridViewRow In DGV_DATA.Rows
                        If IsDBNull(DatagridviewRow.Cells(15).Value) = False Or IsDBNull(DatagridviewRow.Cells(16).Value) = False Or IsDBNull(DatagridviewRow.Cells(17).Value) = False Then
                            isPartitionQuery = True
                            Exit For
                        End If
                    Next
                    If isPartitionQuery = False Then
                        DGV_DATA.Columns(15).Visible = False
                        DGV_DATA.Columns(16).Visible = False
                        DGV_DATA.Columns(17).Visible = False
                    End If
                Else
                    '14,15,16列目がパーティション情報
                    For Each DatagridviewRow As DataGridViewRow In DGV_DATA.Rows
                        If IsDBNull(DatagridviewRow.Cells(13).Value) = False Or IsDBNull(DatagridviewRow.Cells(14).Value) = False Or IsDBNull(DatagridviewRow.Cells(15).Value) = False Then
                            isPartitionQuery = True
                            Exit For
                        End If
                    Next
                    If isPartitionQuery = False Then
                        DGV_DATA.Columns(13).Visible = False
                        DGV_DATA.Columns(14).Visible = False
                        DGV_DATA.Columns(15).Visible = False
                    End If
                End If

            Else
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
                Timer1.Enabled = False
                Timer1.Stop()
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            End If
        End If

        'PLAN_TABLE削除文実行
        Me.Refresh()
        sql = "DELETE PLAN_TABLE WHERE STATEMENT_ID = '" & date_time.ToString("yyyyMMddHHmmssfff") & "'"

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt) = False Then
            frm_Login.G_DB.SET_SELECT_FLG(False)
            LBL_INFO.Text = vbNullString
            Timer1.Enabled = False
            Timer1.Stop()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If
    End Sub

    Private Sub ExecExplainStat(ByVal sqlType As Integer)
        '#######################################################################
        '実行計画＋統計取得
        '#######################################################################
        Dim sql As String
        Dim timeoutValue As Integer = 0
        Dim retCnt As Long
        Dim msgVal(0) As String
        Dim date_time As DateTime = DateTime.Now
        Dim partitionStartIndex As Integer
        Dim partitionStopIndex As Integer
        Dim partitionIdIndex As Integer
        Dim isPartitionQuery As Boolean = False
        Dim sqlId As String
        Dim childNumber As Integer

        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        'STATISTICS_LEVEL=ALLを設定し実行計画と実行統計を取得する
        'ALLはパフォーマンス劣化が激しいので注意
        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        sql = "ALTER SESSION SET STATISTICS_LEVEL=ALL"

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt) Then
            msgVal(0) = retCnt.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0010", msgVal)
        Else
            LBL_INFO.Text = ""
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        M_CANCELFLG = False
        frm_Login.G_DB.SET_SELECT_FLG(False)
        DGV_DATA.DataSource = Nothing

        'SQL実行
        If sqlType = M_CONST.C_SQLTYPE.QUERY Or sqlType = M_CONST.C_SQLTYPE.OTHER Then
            If ExecQuery(True) = False Then
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
                Timer1.Enabled = False
                Timer1.Stop()
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                Exit Sub
            End If
        ElseIf sqlType = M_CONST.C_SQLTYPE.DML Or _
               sqlType = M_CONST.C_SQLTYPE.DDL_DCL Then
            If ExecNonquery(sqlType, True) = False Then
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
                Timer1.Enabled = False
                Timer1.Stop()
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                Exit Sub
            End If
        End If

        '前回実行時のSQL識別情報を取得
        sql = "SELECT PREV_SQL_ID,PREV_CHILD_NUMBER FROM V$SESSION WHERE SID=" & frm_Login.G_DB.GET_SID
        frm_Login.G_DB.SET_SELECT_FLG(False)

        'SELECT文発行
        Using dataset As New DataSet
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then
                sqlId = dataset.Tables(0).Rows(0).Item(0)
                childNumber = dataset.Tables(0).Rows(0).Item(1)
                M_FILE.WriteApLog("SQLID:" & sqlId & " CHILD_NUMBER:" & childNumber)
            Else
                Exit Sub
            End If
        End Using


        'STATISTICS_LEVEL=TYPICALに戻す
        sql = "ALTER SESSION SET STATISTICS_LEVEL=TYPICAL"

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt) = False Then
            LBL_INFO.Text = ""
            Exit Sub
        End If

        DGV_DATA.DataSource = Nothing

        'V$SQL_PLAN_STATISTICS_ALLから統計取得
        'SQL文の組み立て
        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            sql = "SELECT ID,DEPTH,LPAD(' ',2*(DEPTH-1)) || A.OPERATION || NVL2(OPTIMIZER,'(' || OPTIMIZER || ')',NULL) || NVL2(A.OPTIONS,' ' || A.OPTIONS ,NULL) OPERATION, "
            sql = sql & "QBLOCK_NAME,OBJECT_NAME,PARTITION_START,PARTITION_STOP,PARTITION_ID,CARDINALITY,LAST_OUTPUT_ROWS,BYTES,COST,CPU_COST,IO_COST,TIME*1000 ""TIME(MS)"",TRUNC(LAST_ELAPSED_TIME/1000) ""LAST_ELAPSED_TIME(ms)"","
            sql = sql & "LAST_STARTS,LAST_CR_BUFFER_GETS,LAST_CU_BUFFER_GETS,LAST_DISK_READS,LAST_DISK_WRITES,"
            sql = sql & "ACTIVE_TIME,ESTIMATED_OPTIMAL_SIZE,ESTIMATED_ONEPASS_SIZE,LAST_MEMORY_USED,TEMP_SPACE,LAST_TEMPSEG_SIZE,LAST_EXECUTION,LAST_DEGREE, "
            sql = sql & "ACCESS_PREDICATES,FILTER_PREDICATES "
            sql = sql & "FROM V$SQL_PLAN_STATISTICS_ALL A "
            'sql = sql & "WHERE EXISTS (SELECT 1 FROM V$SESSION B WHERE (A.ADDRESS = B.PREV_SQL_ADDR AND A.HASH_VALUE = B.PREV_HASH_VALUE AND A.CHILD_NUMBER = B.PREV_CHILD_NUMBER) AND B.SID=" & frm_Login.G_DB.GET_SID & ") "
            sql = sql & "WHERE SQL_ID = '" & sqlId & "' AND CHILD_NUMBER = " & childNumber & " "
            sql = sql & "ORDER BY ID"
            'ElseIf Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) = 9 And Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(2)) = 2 Then
            ''9.2のバージョンでもv$sql_plan_statisticsはあるが直前に実行したSQLのSQL_ADDRESSを確実に持ってくる方法がなさそうなので凍結。(V$SESSIONのPREV_SQL_ADDRはSQL_ADDRESSと同じものが入ってる)
            'sql = "SELECT ADDRESS,HASH_VALUE,CHILD_NUMBER,ID,LPAD(' ',2*(DEPTH-1)) || OPERATION || ' ' || OPTIONS OPERATION,OBJECT_NAME,CARDINALITY,BYTES,COST,CPU_COST,IO_COST,LAST_OUTPUT_ROWS,LAST_CR_BUFFER_GETS,LAST_CU_BUFFER_GETS,LAST_DISK_READS,LAST_DISK_WRITES,LAST_ELAPSED_TIME,ACTIVE_TIME,"
            'sql = sql & "ESTIMATED_OPTIMAL_SIZE,ESTIMATED_ONEPASS_SIZE,LAST_MEMORY_USED,LAST_EXECUTION,LAST_DEGREE, "
            'sql = sql & "OPTIMIZER,SEARCH_COLUMNS,POSITION,OTHER_TAG,PARTITION_START,PARTITION_STOP,PARTITION_ID,OTHER,DISTRIBUTION,TEMP_SPACE,ACCESS_PREDICATES,FILTER_PREDICATES "
            'sql = sql & "FROM V$SQL_PLAN_STATISTICS_ALL A "
            'sql = sql & "WHERE EXISTS (SELECT 1 FROM V$SQL B WHERE (A.ADDRESS = B.ADDRESS AND A.HASH_VALUE = B.HASH_VALUE) AND '" & TXT_SQL.Text & "' LIKE SUBSTR(B.SQL_TEXT,1,1000) || '%') "
            'sql = sql & "ORDER BY ADDRESS,HASH_VALUE,CHILD_NUMBER,ID"
        End If

        '接続型
        DGV_DATA.RowCount = 1
        DGV_DATA.ColumnCount = 0

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql, timeoutValue) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            'フェッチ&表示(全行表示)
            FetchFromResultset(Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")), 1, 100000000)

            'パーティション情報はパーティションにアクセスした場合のみ表示
            partitionStartIndex = DGV_DATA.Columns("PARTITION_START").Index
            partitionStopIndex = DGV_DATA.Columns("PARTITION_STOP").Index
            partitionIdIndex = DGV_DATA.Columns("PARTITION_ID").Index

            For Each DatagridviewRow As DataGridViewRow In DGV_DATA.Rows
                If IsDBNull(DatagridviewRow.Cells(partitionStartIndex).Value) = False Or IsDBNull(DatagridviewRow.Cells(partitionStopIndex).Value) = False Or IsDBNull(DatagridviewRow.Cells(partitionIdIndex).Value) = False Then
                    isPartitionQuery = True
                    Exit For
                End If
            Next
            If isPartitionQuery = False Then
                DGV_DATA.Columns(partitionStartIndex).Visible = False
                DGV_DATA.Columns(partitionStopIndex).Visible = False
                DGV_DATA.Columns(partitionIdIndex).Visible = False
            End If

            '統計列に色づけ
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_OUTPUT_ROWS").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_ELAPSED_TIME(ms)").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_STARTS").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_CR_BUFFER_GETS").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_CU_BUFFER_GETS").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_DISK_READS").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_DISK_WRITES").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("ACTIVE_TIME").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("ESTIMATED_OPTIMAL_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("ESTIMATED_ONEPASS_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_MEMORY_USED").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_TEMPSEG_SIZE").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_EXECUTION").Index).DefaultCellStyle.BackColor = Color.LightCyan
            DGV_DATA.Columns(DGV_DATA.Columns("LAST_DEGREE").Index).DefaultCellStyle.BackColor = Color.LightCyan

        Else
            frm_Login.G_DB.SET_SELECT_FLG(False)
            LBL_INFO.Text = vbNullString
            Timer1.Enabled = False
            Timer1.Stop()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If

        '列幅調整
        If CHK_COLSIZE_REGULATE.Checked Then
            If Not DGV_DATA Is Nothing Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub FetchFromResultset(Optional ByVal fetchRowCnt As Long = 1000000000, Optional ByVal minDispCnt As Long = -1, Optional ByVal maxDispCnt As Long = -1)
        '#######################################################################
        'SELECT結果FETCH
        '#######################################################################
        Dim readedRowCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim isInterrupt As Boolean = True

        TXT_SQL.Focus()

        If minDispCnt = -1 Then
            If TXT_MIN_DISP_CNT.Text <> vbNullString Then
                minDispCnt = CLng(TXT_MIN_DISP_CNT.Text)
            Else
                minDispCnt = 0
            End If
        End If

        If maxDispCnt = -1 Then
            If TXT_MAX_DISP_CNT.Text <> vbNullString Then
                maxDispCnt = CLng(TXT_MAX_DISP_CNT.Text)
            Else
                maxDispCnt = 10000000
            End If
        End If


        'DGVのDataSourceに設定してあるDataTableを切り離さないとDatatableへの行追加が遅くなるため初期化する
        DGV_DATA.DataSource = Nothing

        M_DATATABLE = New DataTable
        Do While readedRowCnt < fetchRowCnt
            If frm_Login.G_DB.FETCH_TO_DATATABLE(M_DATATABLE, refleshCnt, minDispCnt, maxDispCnt) = False Then
                isInterrupt = False
                Exit Do
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                '最終行までフェッチが終わっている場合
                isInterrupt = False
                DGV_DATA.DataSource = M_DATATABLE
                Exit Do
            End If

            Try
                'セルドラッグ時の場合のエラー対応
                System.Windows.Forms.Application.DoEvents()
            Catch ex As Exception
                'NULL
            End Try

            readedRowCnt = readedRowCnt + refleshCnt

            'キャンセル指示があった場合
            If M_CANCELFLG Then
                Timer1.Enabled = False
                Timer1.Stop()
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
        Loop

        If M_DATATABLE Is Nothing Then
            '画面が閉じられる等のケース
            Exit Sub
        End If
        DGV_DATA.DataSource = M_DATATABLE

        M_COMMON.SetRowheaderWidth(DGV_DATA)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)

        If isInterrupt Then
            '未フェッチの行が残っている場合

            '次回の再開指示までに他の画面から結果セットを破棄したうえで中断中になっている可能性があるため現在の中断回数を保存
            M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.INTERRUPT)
            M_FILE.WriteApLog("フェッチを中断しました(" & msg(0) & ")")
        Else
            If Timer1.Enabled = False Then
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            End If
            M_FILE.WriteApLog("フェッチが終了しました(" & msg(0) & ")")
        End If
    End Sub

    Private Sub FetchFromResultsetToLocalDataTable(Optional ByVal fetchRowCnt As Long = 1000000000)
        '#######################################################################
        'SELECT結果FETCH
        '#######################################################################
        Dim readedRowCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim minDispCnt As Long = 0
        Dim maxDispCnt As Long = 100000000
        Dim isInterrupt As Boolean = True

        TXT_SQL.Focus()

        If TXT_MIN_DISP_CNT.Text <> vbNullString Then
            minDispCnt = CLng(TXT_MIN_DISP_CNT.Text)
        End If

        If TXT_MAX_DISP_CNT.Text <> vbNullString Then
            maxDispCnt = CLng(TXT_MAX_DISP_CNT.Text)
        End If

        DGV_DATA.DataSource = Nothing

        Do While readedRowCnt < fetchRowCnt
            If frm_Login.G_DB.FETCH_TO_LOCAL_DATATABLE(M_DATATABLE, refleshCnt, minDispCnt, maxDispCnt) = False Then
                isInterrupt = False
                Exit Do
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                '最終行までフェッチが終わっている場合
                isInterrupt = False
                DGV_DATA.DataSource = M_DATATABLE
                Exit Do
            End If

            Try
                'セルドラッグ時の場合のエラー対応
                System.Windows.Forms.Application.DoEvents()
            Catch ex As Exception
                'NULL
            End Try

            readedRowCnt = readedRowCnt + refleshCnt

            'キャンセル指示があった場合
            If M_CANCELFLG Then
                Timer1.Enabled = False
                Timer1.Stop()
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
        Loop

        If M_DATATABLE Is Nothing Then
            '画面が閉じられる等のケース
            Exit Sub
        End If
        DGV_DATA.DataSource = M_DATATABLE

        M_COMMON.SetRowheaderWidth(DGV_DATA)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)

        If isInterrupt Then
            '未フェッチの行が残っている場合

            '次回の再開指示までに他の画面から結果セットを破棄したうえで中断中になっている可能性があるため現在の中断回数を保存
            M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.INTERRUPT)
            M_FILE.WriteApLog("フェッチを中断しました(" & msg(0) & ")")
        Else
            If Timer1.Enabled = False Then
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            End If
            M_FILE.WriteApLog("フェッチが終了しました(" & msg(0) & ")")
        End If
    End Sub

    Private Sub FetchFromDataset(Optional ByVal readCnt As Long = 10000000000)
        '#######################################################################
        '非接続型クエリのSELECT結果FETCH
        '#######################################################################
        Dim tmpCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim isInterrupt As Boolean = True

        TXT_SQL.Focus()
        'Try
        Do While tmpCnt < readCnt
            If frm_Login.G_DB.FETCH_DATA_DS(refleshCnt) = False Then
                LBL_INFO.Text = vbNullString
                Exit Sub
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            tmpCnt = tmpCnt + refleshCnt
            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                '最終行までフェッチが終わっている場合
                isInterrupt = False
                Exit Do
            End If
            System.Windows.Forms.Application.DoEvents()
            'キャンセル指示があった場合
            If M_CANCELFLG Then
                Timer1.Enabled = False
                Timer1.Stop()
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
        Loop

        If frm_Login.G_DB.SET_BINDING_SOURCE(DGV_DATA) = False Then
            LBL_INFO.Text = vbNullString
            Exit Sub
        End If

        M_COMMON.SetRowheaderWidth(DGV_DATA)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
        LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

        If isInterrupt Then
            '未フェッチの行が残っている場合
            ChangeFormStatus(M_CONST.C_FORM_STATUS.INTERRUPT)
        Else
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If

        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    LBL_INFO.Text = ""
        '    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        'End Try

    End Sub

    Private Function ExecNonquery(ByVal sqlType As Integer, Optional ByVal isGetStatsMode As Boolean = False) As Boolean
        '#######################################################################
        '非クエリ(DMLやプロシージャの実行)
        '#######################################################################
        Dim retCnt As Long
        Dim msgVal(0) As String
        Dim retStr() As String 'バインド変数の戻り値
        Dim sql As String = vbNullString
        Dim timeoutValue As Integer = 0

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0029")
        Me.Refresh()

        ReDim retStr(frm_Bind.DGV_BIND.RowCount - 2)  '最後の1行は新規行のため-2

        'トランザクションがすでにスタートしている場合は呼ばないようにするか、CLS_DB側でトランザクションの有無を判断するようにしないといけない。今後実装。
        ''トランザクションのスタート
        'If frm_Login.G_DB.SET_TRANSACTION() = False Then
        '    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        '    LBL_INFO.Text = vbNullString
        '    Exit Sub
        'End If

        sql = TXT_SQL.Text

        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            timeoutValue = Integer.Parse(TXT_TIMEOUT_VALUE.Text)
        End If


        If isGetStatsMode Then
            'M_COMMON.AddHintToSQL(sql, " /*+ gather_plan_statistics */ ")
            sql = "/* O-Analyzer " & DateTime.Now.ToString & " */ " & sql
        End If

        If Not frm_Bind Is Nothing Then
            frm_Bind.SetBind(False)
        End If

        If frm_Login.G_DB.EXEC_NONQUERY(sql, retCnt, retStr, True, timeoutValue) Then
            msgVal(0) = retCnt.ToString
            If sqlType = M_CONST.C_SQLTYPE.DML Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0010", msgVal)
            Else
                LBL_INFO.Text = M_COMMON.GetMessage("I0030")
            End If

            '戻り値をバインド画面に反映
            If Not (retStr Is Nothing) Then
                For i As Integer = 0 To retStr.Length - 1
                    frm_Bind.DGV_BIND.Rows(i).Cells(3).Value = retStr(i)
                Next
            End If
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        Else
            LBL_INFO.Text = ""
            Timer1.Enabled = False
            Timer1.Stop()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Return False
        End If
        Return True
    End Function

    Private Sub ExecProcedure()
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0029")
        Me.Refresh()
        If frm_Login.G_DB.EXEC_PROCEDURE(TXT_SQL.Text) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0011")
        Else
            LBL_INFO.Text = ""
            Timer1.Enabled = False
            Timer1.Stop()
        End If
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub ExecEventWatch()
        Dim sqlType As Integer
        Dim sql As String
        Dim errmsg As String = vbNullString
        Dim retCnt As Integer = 0
        Dim watchInterval As Integer = 0

        '入力値チェック
        M_COMMON.ValidateControls(Me.Controls, errmsg)
        If errmsg <> vbNullString Then
            MsgBox(errmsg)
            Exit Sub
        End If

        If TXT_SQL.Text = vbNullString Then
            MsgBox(M_COMMON.GetMessage("E0008"))
            Exit Sub
        End If

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        sqlType = M_COMMON.CheckSql(TXT_SQL.Text)

        '接続情報
        M_MIDDLEWARE = frm_Login.CMB_MIDDLE.SelectedIndex
        M_USER = frm_Login.G_DB.GET_LOGIN_USER
        M_PASSWORD = frm_Login.TXT_PASS.Text
        M_CONN_STR = frm_Login.G_DB.GET_CONNECT_STRING
        M_SQL = TXT_SQL.Text
        M_CANCELFLG = False
        If frm_Login.CHK_SYSDBA.Checked Then
            M_IS_SYSDBA = True
        Else
            M_IS_SYSDBA = False
        End If

        M_SID = -1
        M_ERR_MSG = vbNullString
        M_INFO_MSG = vbNullString
        If TXT_WATCH_INTERVAL.Text <> vbNullString Then
            watchInterval = TXT_WATCH_INTERVAL.Text
        End If
        If TXT_TIMEOUT_VALUE.Text <> vbNullString Then
            M_TIMEOUT = TXT_TIMEOUT_VALUE.Text
        End If
        DGV_DATA.DataSource = Nothing
        DGV_DATA.RowCount = 0
        M_BG_THREAD = New System.Threading.Thread(AddressOf StartThread)
        M_BG_THREAD.Start()

        While M_SID = -1
            If M_BG_THREAD.IsAlive = False Then
                LBL_INFO.Text = vbNullString
                MsgBox(M_ERR_MSG)
                ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
                Exit Sub
            End If
            LBL_INFO.Text = M_INFO_MSG
            System.Windows.Forms.Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 9 Then
            sql = "SELECT TO_CHAR(SYSTIMESTAMP,'HH24:MI:SS.FF') time,A.* FROM V$SESSION_WAIT A WHERE SID=" & M_SID
        Else
            sql = "SELECT TO_CHAR(SYSDATE,'HH24:MI:SS') time,A.* FROM V$SESSION_WAIT A WHERE SID=" & M_SID
        End If

        While M_BG_THREAD.IsAlive
            If M_CANCELFLG Then
                LBL_INFO.Text = "処理を中断しています"
                System.Windows.Forms.Application.DoEvents()
                If frm_Login.G_DB.EXEC_NONQUERY("ALTER SYSTEM KILL SESSION '" & M_SID & "," & M_SERIAL & "'", retCnt, , True, ) Then
                    LBL_INFO.Text = "中断に失敗しました"
                End If
                M_INFO_MSG = "処理を中断しました"
                Exit While
            End If
            Using dataset As New DataSet
                'SESSIONイベントの追加
                If frm_Login.G_DB.SET_RESULTSET(sql, , False) Then
                    frm_Login.G_DB.INIT_DATA_TABLE()

                    M_DATATABLE = New DataTable
                    If frm_Login.G_DB.FETCH_TO_DATATABLE(M_DATATABLE, 1, 0, 0, False) = False Then
                        Exit Sub
                    End If

                    If DGV_DATA.Columns.Count = 0 Then
                        For Each a As DataColumn In M_DATATABLE.Columns
                            DGV_DATA.Columns.Add(a.ColumnName, a.ColumnName)
                        Next
                    End If
                    If M_DATATABLE.Rows.Count > 0 Then
                        DGV_DATA.Rows.Add(M_DATATABLE.Rows(0).ItemArray)
                    End If

                Else
                    MsgBox("EVENT取得エラー")
                End If
            End Using
            LBL_INFO.Text = M_INFO_MSG
            If CHK_REFRESH.Checked Then
                System.Windows.Forms.Application.DoEvents()
            End If
            System.Threading.Thread.Sleep(watchInterval)
        End While

        LBL_INFO.Text = M_INFO_MSG
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)

        If M_ERR_MSG <> vbNullString Then
            LBL_INFO.Text = vbNullString
            MsgBox(M_ERR_MSG)
            Exit Sub
        End If

        '列幅調整
        If CHK_COLSIZE_REGULATE.Checked Then
            If Not DGV_DATA Is Nothing Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        End If
    End Sub

    Private Sub StartThread()
        '#######################################################################
        'スレッド実行
        '#######################################################################
        Dim sqlType As Integer
        Dim sql As String
        Dim msgVal(0) As String
        Dim retcnt As Long = 0
        Dim date_time As DateTime = DateTime.Now

        M_CLS_DB = New CLS_DB.CLS_DB

        Try
            M_CLS_DB.SET_MIDDLEWARE(M_MIDDLEWARE)
            If M_CLS_DB.DB_CONNECT(M_USER, M_PASSWORD, M_CONN_STR, frm_Login.TXT_OPTION.Text, M_IS_SYSDBA, False, Format(date_time, "yyyyMMdd") & ".txt") = False Then
                '接続失敗
                M_ERR_MSG = "接続が失敗しました"
                Exit Sub
            End If
            M_INFO_MSG = "接続しました"
        Catch e As ThreadAbortException
            M_ERR_MSG = "スレッドが強制停止されました"
            Exit Sub
        Catch ex As Exception
            M_ERR_MSG = "接続が失敗しました " & ex.Message & " " & ex.StackTrace
            Exit Sub
        End Try

        Try
            'SIDの取得
            sql = "SELECT SID,SERIAL# FROM V$SESSION WHERE SID=(SELECT SID FROM V$MYSTAT WHERE ROWNUM=1)"
            Using dataset As New DataSet
                If M_CLS_DB.GET_DATASET(sql, dataset, M_TIMEOUT, False) Then
                    M_SID = dataset.Tables(0).Rows(0).Item(0)
                    M_SERIAL = dataset.Tables(0).Rows(0).Item(1)
                Else
                    M_ERR_MSG = "SID取得エラー"
                    Exit Sub
                End If
            End Using

            '主処理
            sqlType = M_COMMON.CheckSql(M_SQL)
            If sqlType = M_CONST.C_SQLTYPE.QUERY Or _
               sqlType = M_CONST.C_SQLTYPE.OTHER Then
                ExecQueryBackGround()
            ElseIf sqlType = M_CONST.C_SQLTYPE.DML Or _
                   sqlType = M_CONST.C_SQLTYPE.DDL_DCL Then
                If M_CLS_DB.EXEC_NONQUERY(M_SQL, retcnt, , False, M_TIMEOUT) Then
                    msgVal(0) = retcnt.ToString
                    If sqlType = M_CONST.C_SQLTYPE.DML Then
                        M_INFO_MSG = M_COMMON.GetMessage("I0010", msgVal)
                    Else
                        M_INFO_MSG = M_COMMON.GetMessage("I0030")
                    End If
                End If
            End If
        Catch threadAbortException As ThreadAbortException
            M_ERR_MSG = "スレッドが強制停止されました"
            'cls_db.DB_CLOSE()
        Catch e As Exception
            M_ERR_MSG = "異常終了しました " & e.Message & " " & e.StackTrace
            'cls_db.DB_CLOSE()
        Finally
            M_CLS_DB.DB_CLOSE()
            M_CLS_DB = Nothing
        End Try
    End Sub

    Private Function ExecQueryBackGround() As Boolean
        '#######################################################################
        'スレッドでSELECT実行
        '#######################################################################
        M_CANCELFLG = False
        M_CLS_DB.SET_SELECT_FLG(False)

        M_INFO_MSG = M_COMMON.GetMessage("I0005")

        'SQL実行
        If M_CLS_DB.SET_RESULTSET(M_SQL, M_TIMEOUT, False) Then
            M_INFO_MSG = M_COMMON.GetMessage("I0006")
            'DataGridView初期化
            M_CLS_DB.INIT_DATA_TABLE()
            'フェッチ&表示
            FetchFromResultsetBackGround(Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
        Else
            M_CLS_DB.SET_SELECT_FLG(False)
            M_ERR_MSG = M_COMMON.GetMessage("E0001")
            M_INFO_MSG = vbNullString
            Return False
        End If
        Return True
    End Function

    Private Sub FetchFromResultsetBackGround(Optional ByVal fetchRowCnt As Long = 1000000000)
        '#######################################################################
        'SELECT結果FETCH
        '#######################################################################
        Dim readedRowCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim isInterrupt As Boolean = True
        Dim datatable As New DataTable

        Do While readedRowCnt < fetchRowCnt
            If M_CLS_DB.FETCH_TO_DATATABLE(datatable, refleshCnt, 0, 10000000, False) = False Then
                Exit Do
            End If
            msg(0) = M_CLS_DB.GET_ROW_COUNT.ToString
            M_INFO_MSG = M_COMMON.GetMessage("I0007", msg)

            If M_CLS_DB.GET_SELECT_FLG = False Then
                'フェッチ終了
                Exit Do
            End If

            readedRowCnt = readedRowCnt + refleshCnt
        Loop

        msg(0) = M_CLS_DB.GET_ROW_COUNT.ToString
        M_INFO_MSG = M_COMMON.GetMessage("I0009", msg)
    End Sub

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXECUTE.Enabled = True
            BTN_COPY.Enabled = True
            BTN_CSV.Enabled = True
            BTN_CANCEL.Enabled = False
            BTN_BIND.Enabled = True
            GroupBox3.Enabled = True
            BTN_RESTART.Enabled = False
            BTN_RESTART.Visible = False
            BTN_ALL_COPY.Enabled = True
        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXECUTE.Enabled = False
            BTN_COPY.Enabled = False
            BTN_CSV.Enabled = False
            BTN_CANCEL.Enabled = True
            BTN_BIND.Enabled = False
            BTN_RESTART.Enabled = False
            BTN_RESTART.Visible = False
            GroupBox3.Enabled = False
            BTN_ALL_COPY.Enabled = False
        ElseIf statusID = M_CONST.C_FORM_STATUS.INTERRUPT Then
            '処理が中断して未フェッチのデータが残っている場合
            BTN_EXECUTE.Enabled = True
            BTN_COPY.Enabled = True
            BTN_CSV.Enabled = True
            BTN_CANCEL.Enabled = False
            BTN_BIND.Enabled = True
            GroupBox3.Enabled = True
            BTN_RESTART.Enabled = True
            BTN_RESTART.Visible = True
            BTN_ALL_COPY.Enabled = True
        End If
    End Sub

    Private Sub tickTimer()
        Dim sqlType As Integer

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        Me.Refresh()

        If frm_Login.G_DB.GET_SELECT_FLG Then
            '前回の処理がまだ実行中であった場合処理せず抜ける
            Exit Sub
        End If

        If M_EXEC_CNT + 1 = Integer.Parse(TXT_EXEC_CNT.Text) Then
            Timer1.Stop()
            Timer1.Enabled = False
        End If
        If M_EXEC_CNT >= Integer.Parse(TXT_EXEC_CNT.Text) Then
            Timer1.Enabled = False
            Timer1.Stop()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        If M_CANCELFLG Then
            Timer1.Enabled = False
            M_CANCELFLG = False
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        If TXT_SQL.Text = vbNullString Then
            MsgBox(M_COMMON.GetMessage("E0008"))
            Timer1.Enabled = False
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

        'If M_EXEC_CNT > TXT_EXEC_CNT.Text Then
        '    Timer1.Enabled = False
        '    ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        '    Exit Sub
        'End If

        M_EXEC_CNT = M_EXEC_CNT + 1
        sqlType = M_COMMON.CheckSql(TXT_SQL.Text)

        If sqlType = M_CONST.C_SQLTYPE.QUERY Or sqlType = M_CONST.C_SQLTYPE.OTHER Then
            ExecQuery()
        ElseIf sqlType = M_CONST.C_SQLTYPE.DML Or sqlType = M_CONST.C_SQLTYPE.DDL_DCL Then
            ExecNonquery(sqlType)
        ElseIf sqlType = M_CONST.C_SQLTYPE.PROCEDURE Then
            ExecProcedure()
        End If

        If Timer1.Enabled Then
            ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        End If

        If M_CANCELFLG Then
            M_CANCELFLG = False
            Timer1.Enabled = False
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            Exit Sub
        End If

    End Sub
    Private Sub frm_Execsql_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)
        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        SplitContainer1.Width = GroupBox1.Location.X - 10
        SplitContainer1.Height = Me.Height - LBL_INFO.Height - 50
        DGV_DATA.Width = SplitContainer1.Width
        DGV_DATA.Height = SplitContainer1.Panel2.Height - 5
        TXT_SQL.Width = SplitContainer2.Panel1.Width - 5
        TXT_SQL.Height = SplitContainer1.Panel1.Height - 5
        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub BTN_CSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CSV.Click
        Dim msg(0) As String

        LBL_INFO.Text = M_COMMON.GetMessage("I0029")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        Me.Refresh()
        If M_FILE.CreateCSV(DGV_DATA, msg(0), Format(Now(), "yyyyMMddHHmmss") & ".csv") Then
            MsgBox(M_COMMON.GetMessage("I0012"))
        End If

        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub BTN_COPY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_COPY.Click
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        LBL_INFO.Text = M_COMMON.GetMessage("I0015")
        Me.Refresh()

        If M_FILE.SetClipboardSelectedData(DGV_DATA) Then
            MsgBox(M_COMMON.GetMessage("I0016"))
        End If

        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub frm_Execsql_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        LBL_INFO.Text = vbNullString
        CHK_TIMER.Checked = True
        CHK_TIMER.Checked = False

        If frm_Login.G_DB.GET_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            'ODP.net以外はタイムアウトプロパティをサポートしていない
            TXT_TIMEOUT_VALUE.Enabled = True
        Else
            TXT_TIMEOUT_VALUE.Enabled = False
            TXT_TIMEOUT_VALUE.Text = "0"
        End If

        If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            '10g以降
            CMB_DISPLAY.Items.Add("4 実行統計表示")
        End If

        CMB_DISPLAY.SelectedIndex = 0

        TXT_SQL.LanguageOption = RichTextBoxLanguageOptions.UIFonts

        Me.Text = "O-Analyzer - SQL実行(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"

        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
        RB_SELECT1.Visible = False
        RB_SELECT2.Visible = False
        Me.Width = 800
    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        M_CANCELFLG = True
    End Sub

    Private Sub BTN_EXECUTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXECUTE.Click
        Dim sqlType As Integer
        Dim errmsg As String = vbNullString

        '入力値チェック
        M_COMMON.ValidateControls(Me.Controls, errmsg)
        If errmsg <> vbNullString Then
            MsgBox(errmsg)
            Exit Sub
        End If

        If TXT_SQL.Text = vbNullString Then
            MsgBox(M_COMMON.GetMessage("E0008"))
            Exit Sub
        End If

        If CHK_TIMER.Checked Then
            Timer1.Interval = Integer.Parse(TXT_TIMER_INTERVAL.Text) * 1000
            M_EXEC_CNT = 0
            frm_Login.G_DB.SET_SELECT_FLG(False)
            Timer1.Enabled = True
            Timer1.Start()
            tickTimer()
        Else
            sqlType = M_COMMON.CheckSql(TXT_SQL.Text)
            If M_SQL_HISTORY(0) <> TXT_SQL.Text Then
                For i As Integer = 98 To 0 Step -1 '99番目の値は上書きされるので98から0まで
                    M_SQL_HISTORY(i + 1) = M_SQL_HISTORY(i)
                Next
                M_SQL_HISTORY(0) = TXT_SQL.Text
            End If
            M_HISTORY_INDEX = 0

            If CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "1" Then
                '通常実行
                If sqlType = M_CONST.C_SQLTYPE.QUERY Or _
                   sqlType = M_CONST.C_SQLTYPE.OTHER Then
                    ExecQuery()
                ElseIf sqlType = M_CONST.C_SQLTYPE.DML Or _
                       sqlType = M_CONST.C_SQLTYPE.DDL_DCL Then
                    ExecNonquery(sqlType)
                ElseIf sqlType = M_CONST.C_SQLTYPE.PROCEDURE Then
                    ExecProcedure()
                End If
            ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "2" Then
                '実行計画表示
                ExecExplain()
            ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "3" Then
                '待機イベント監視
                ExecEventWatch()
            ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "4" Then
                '統計取得
                ExecExplainStat(sqlType)
            End If
        End If
    End Sub

    Private Sub EXCUTING_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TXT_SQL.KeyDown
        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        Else
            If (e.KeyData And Keys.Control) = Keys.Control AndAlso (e.KeyData And Keys.KeyCode) = Keys.V Then
                'CTRL+Vを押した場合
                If Not Clipboard.GetText() = Nothing Then
                    Clipboard.SetText(Clipboard.GetText())
                End If
            ElseIf (e.KeyData And Keys.Control) = Keys.Control AndAlso (e.KeyData And Keys.KeyCode) = Keys.E Then
                'CTRL+Eを押した場合
                '書式が中央よせになってしまうため無効化
                e.Handled = True
                'BTN_EXECUTE_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_DATA.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
    End Sub

    Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) Handles DGV_DATA.DataError
        If Not (e.Exception Is Nothing) Then
            e.Cancel = False
        End If
    End Sub

    Private Sub frm_Execsql_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        '主にメモリを使用するデータテーブルとグリッドビューを明示的に解放する
        frm_Login.G_DB.DISPOSE_DATA_TABLE()

        If Not (M_DATATABLE Is Nothing) Then
            M_DATATABLE.Dispose()
            M_DATATABLE = Nothing
        End If

        If Not (DGV_DATA Is Nothing) Then
            DGV_DATA.Dispose()
            DGV_DATA = Nothing
        End If

        'GCを実行してメモリを解放する。なぜか2回実行しないと解放されない
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
        GC.WaitForPendingFinalizers()

        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        If frm_Explorer.Visible = False Then
            frm_Login.G_DB.DB_CLOSE()
            frm_Login.Visible = True
        End If
        Me.Dispose()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub DGV_DATA_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles DGV_DATA.PreviewKeyDown
        '列幅を自動調整している場合、Endキーで最終列に移動した場合カレントセルが表示されない現象の対応
        If e.KeyCode = Keys.End Then
            Dim cellWidth As Integer = Me.DGV_DATA.ClientSize.Width - DGV_DATA.Rows(0).HeaderCell.Size.Width

            For i As Integer = DGV_DATA.Columns.Count - 1 To 0 Step -1
                cellWidth -= Me.DGV_DATA.Columns(i).Width
                If cellWidth <= 0 Then
                    DGV_DATA.FirstDisplayedScrollingColumnIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub BTN_BIND_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_BIND.Click
        frm_Bind.Show()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        tickTimer()
    End Sub

    Private Sub BTN_RESTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_RESTART.Click
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        If M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT Then
            FetchFromResultset(Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            '列幅調整
            If CHK_COLSIZE_REGULATE.Checked Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        Else
            MsgBox(M_COMMON.GetMessage("I0035"))
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If
    End Sub

    Private Sub CHK_TIMER_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CHK_TIMER.CheckedChanged
        If CHK_TIMER.Checked Then
            TXT_TIMER_INTERVAL.Enabled = True
            TXT_EXEC_CNT.Enabled = True
        Else
            TXT_TIMER_INTERVAL.Enabled = False
            TXT_EXEC_CNT.Enabled = False
        End If
    End Sub

    Private Sub BTN_HIST_UP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HIST_UP.Click
        If M_HISTORY_INDEX < 99 Then
            M_HISTORY_INDEX += 1
            TXT_SQL.Text = M_SQL_HISTORY(M_HISTORY_INDEX)
        End If
    End Sub

    Private Sub BTN_HIST_DOWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HIST_DOWN.Click
        If M_HISTORY_INDEX > 0 Then
            M_HISTORY_INDEX -= 1
            TXT_SQL.Text = M_SQL_HISTORY(M_HISTORY_INDEX)
        End If
    End Sub

    Private Sub SplitContainer3_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel1.Resize
        TXT_SQL.Width = SplitContainer2.Panel1.Width - 5
    End Sub

    Private Sub SplitContainer1_Panel2_Resize_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel2.Resize
        DGV_DATA.Height = SplitContainer1.Panel2.Height - 5
    End Sub

    Private Sub SplitContainer1_Panel1_Resize_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize
        TXT_SQL.Height = SplitContainer1.Panel1.Height - 5
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub BTN_ALL_COPY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_ALL_COPY.Click
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        LBL_INFO.Text = M_COMMON.GetMessage("I0015")
        Me.Refresh()
        'M_FILE.SetClipboard(DGV_DATA)
        If M_FILE.SetClipboardAllData(DGV_DATA) Then
            MsgBox(M_COMMON.GetMessage("I0016"))
        End If
        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    'Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    ExecQueryUsingLocalDataTable()
    'End Sub

    Private Sub CMB_DISPLAY_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMB_DISPLAY.SelectedIndexChanged
        If CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "1" Then
            CHK_TIMER.Enabled = True
            Label6.Visible = False
            TXT_WATCH_INTERVAL.Visible = False
            CHK_TIMER.Visible = True
            GroupBox2.Visible = True
            Label2.Visible = True
            TXT_MIN_DISP_CNT.Visible = True
            Label1.Visible = True
            TXT_MAX_DISP_CNT.Visible = True
            CHK_REFRESH.Visible = False
        ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "2" Then
            CHK_TIMER.Enabled = False
            CHK_TIMER.Checked = False
            Label6.Visible = False
            TXT_WATCH_INTERVAL.Visible = False
            CHK_TIMER.Visible = False
            GroupBox2.Visible = False
            Label2.Visible = False
            TXT_MIN_DISP_CNT.Visible = False
            Label1.Visible = False
            TXT_MAX_DISP_CNT.Visible = False
            CHK_REFRESH.Visible = False
        ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "3" Then
            CHK_TIMER.Enabled = False
            CHK_TIMER.Checked = False
            Label6.Visible = True
            TXT_WATCH_INTERVAL.Visible = True
            CHK_TIMER.Visible = False
            GroupBox2.Visible = False
            Label2.Visible = False
            TXT_MIN_DISP_CNT.Visible = False
            Label1.Visible = False
            TXT_MAX_DISP_CNT.Visible = False
            CHK_REFRESH.Visible = True
        ElseIf CMB_DISPLAY.SelectedItem.ToString.Substring(0, 1) = "4" Then
            CHK_TIMER.Enabled = True
            Label6.Visible = False
            TXT_WATCH_INTERVAL.Visible = False
            CHK_TIMER.Visible = False
            GroupBox2.Visible = False
            Label2.Visible = True
            TXT_MIN_DISP_CNT.Visible = True
            Label1.Visible = True
            TXT_MAX_DISP_CNT.Visible = True
            CHK_REFRESH.Visible = False
        End If
    End Sub

    Private Sub DGV_DATA_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DGV_DATA.KeyUp
        If (e.KeyData And Keys.Control) = Keys.Control AndAlso (e.KeyData And Keys.KeyCode) = Keys.C Then
            'CTRL+Cを押してコピーした場合クリップボードデータをテキスト形式に変換する
            If Not Clipboard.GetText() = Nothing Then
                Clipboard.SetText(Clipboard.GetText())
            End If
            'ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            'LBL_INFO.Text = "コピーしました"
        End If
    End Sub

    'コピー中だということをユーザが分かるようにしたほうがよさそうだが今のところよいソース思いつかず。
    'Private Sub DGV_DATA_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DGV_DATA.KeyDown
    '    If (e.KeyData And Keys.Control) = Keys.Control AndAlso (e.KeyData And Keys.KeyCode) = Keys.C Then
    '        'CTRL+Cを押してコピーした場合処理終了まで実行中モードにする
    '        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
    '        LBL_INFO.Text = "コピー中"
    '    End If
    'End Sub

End Class