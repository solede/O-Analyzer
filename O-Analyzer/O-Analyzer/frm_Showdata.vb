Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE

Public Class frm_showdata
    '#######################################################################
    'テーブルデータ表示画面
    '#######################################################################

    '定数
    Private Const C_DATA As Short = 0
    Private Const C_COL_INFO As Short = 1
    Private Const C_HISOGRAM_STAT As Short = 2
    Private Const C_SOURCE = 3

    'モジュール変数
    Private M_SCHEMA_NAME As String
    Private M_OBJECT_TYPE As String
    Private M_OBJECT_NAME As String
    Private M_PARTITION_NAME As String
    Private M_SUBPARTITION_NAME As String
    Private M_CANCELFLG As Boolean = False
    Private M_DATATABLE_DATA As DataTable
    Private M_DATATABLE_COL_STAT As DataTable
    Private M_DATATABLE_HISTOGRAM As DataTable
    Private M_SET_READER_CNT As Integer
    Private M_TYPE_NAME As String

    Public Sub SetObjectInfo(ByVal schemaName As String, ByVal objectType As String, ByVal objectName As String, Optional ByVal partitionName As String = vbNullString, Optional ByVal subPartitionName As String = vbNullString)
        M_SCHEMA_NAME = schemaName
        M_OBJECT_TYPE = objectType
        M_OBJECT_NAME = objectName
        M_PARTITION_NAME = partitionName
        M_SUBPARTITION_NAME = subPartitionName

        If M_SCHEMA_NAME <> vbNullString Then
            Me.Text = M_SCHEMA_NAME & "." & M_OBJECT_NAME
        Else
            Me.Text = M_OBJECT_NAME
        End If

        If M_PARTITION_NAME <> vbNullString Then
            Me.Text = Me.Text & " PARTITION:" & M_PARTITION_NAME
        End If

        If M_SUBPARTITION_NAME <> vbNullString Then
            Me.Text = Me.Text & " SUBPARTITION:" & M_SUBPARTITION_NAME
        End If

        If objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE).GetEName Or _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE_BODY).GetEName Or _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.FUNCTION).GetEName Or _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PROCEDURE).GetEName Or _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TYPE).GetEName Or _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.JAVA_SOURCE).GetEName Then
            Me.tabControl1.TabPages.RemoveByKey("TAB_DATA")
            Me.tabControl1.TabPages.RemoveByKey("TAB_COL_STAT")
            Me.tabControl1.TabPages.RemoveByKey("TAB_SOURCE")
            Me.tabControl1.TabPages.Add(TAB_SOURCE)
            BTN_CSV.Enabled = False
        Else
            Me.tabControl1.TabPages.RemoveByKey("TAB_DATA")
            Me.tabControl1.TabPages.RemoveByKey("TAB_COL_STAT")
            Me.tabControl1.TabPages.RemoveByKey("TAB_SOURCE")
            Me.tabControl1.TabPages.Add(TAB_DATA)
            Me.tabControl1.TabPages.Add(TAB_COL_STAT)
            BTN_CSV.Enabled = True
        End If

        Me.Text = Me.Text & "(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
    End Sub

    Public Sub DispData(ByVal id As Short)
        '#######################################################################
        'データ表示
        '#######################################################################
        Dim sql As String

        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        DGV_DATA.DataSource = Nothing
        frm_Login.G_DB.SET_SELECT_FLG(False)
        DGV_DATA.RowCount = 1
        DGV_DATA.ColumnCount = 0
        M_CANCELFLG = False

        sql = CreateSQL(C_DATA)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            frm_Login.G_DB.INIT_DATA_TABLE()
            DGV_DATA.Focus()
            'フェッチ&表示
            FetchFromResultset(M_DATATABLE_DATA, DGV_DATA, Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
        Else
            frm_Login.G_DB.SET_SELECT_FLG(False)
            LBL_INFO.Text = vbNullString
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If

        '列幅調整
        If CHK_COLSIZE_REGULATE.Checked Then
            DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        End If
    End Sub

    Public Sub DispSource()
        '#######################################################################
        'データ表示
        '#######################################################################
        Dim sql As String
        Dim source As New System.Text.StringBuilder

        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        'DGV_DATA.DataSource = Nothing
        frm_Login.G_DB.SET_SELECT_FLG(False)
        'DGV_DATA.RowCount = 1
        'DGV_DATA.ColumnCount = 0
        M_CANCELFLG = False

        sql = CreateSQL(C_SOURCE)

        Using dataSet As New DataSet
            'SQL実行
            If frm_Login.G_DB.GET_DATASET(sql, dataSet) Then
                LBL_INFO.Text = M_COMMON.GetMessage("I0006")
                Me.Refresh()
                For Each a As DataRow In dataSet.Tables(0).Rows
                    source.Append(a.Item(0))
                Next

                TXT_SOURCE.Text = source.ToString
                LBL_INFO.Text = M_COMMON.GetMessage("I0001")
            Else
                frm_Login.G_DB.SET_SELECT_FLG(False)
                LBL_INFO.Text = vbNullString
            End If
        End Using
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Public Sub SelectColInfo()
        '#######################################################################
        '列統計取得SQL実行
        '#######################################################################
        Dim sql As String
        Dim bindingSource1 As New BindingSource

        LBL_INFO.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()

        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)

        DGV_COL_STAT.DataSource = Nothing
        DGV_HISTOGRAM.DataSource = Nothing
        frm_Login.G_DB.SET_SELECT_FLG(False)
        DGV_COL_STAT.RowCount = 1
        DGV_COL_STAT.ColumnCount = 0
        DGV_HISTOGRAM.RowCount = 1
        DGV_HISTOGRAM.ColumnCount = 0

        M_CANCELFLG = False

        sql = CreateSQL(C_COL_INFO)
        'Try
        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            LBL_INFO.Text = M_COMMON.GetMessage("I0006")
            Me.Refresh()
            'DataGridView初期化
            M_DATATABLE_COL_STAT = New DataTable
            frm_Login.G_DB.INIT_LOCAL_DATA_TABLE(M_DATATABLE_COL_STAT)
            DGV_COL_STAT.Focus()
            'フェッチ&表示
            FetchFromResultsetToLocalDataTable(M_DATATABLE_COL_STAT, DGV_COL_STAT, Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            Me.Refresh()
            DGV_COL_STAT.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        Else
            frm_Login.G_DB.SET_SELECT_FLG(False)
            LBL_INFO.Text = vbNullString
        End If

        Me.Refresh()
        '列幅調整
        DGV_COL_STAT.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)  '列統計取得時はキャンセルしても再開ボタンは表示させない
    End Sub

    Public Sub SelectHistogramStat()
        '#######################################################################
        'ヒストグラム統計取得SQL実行
        '#######################################################################
        Dim sql As String

        If M_OBJECT_TYPE = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Or _
           M_OBJECT_TYPE = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
            'ディクショナリに統計情報はないため処理なし
            Exit Sub
        End If

        If DGV_COL_STAT.CurrentRow.Cells(DGV_COL_STAT.Columns("HISTOGRAM").Index).Value = "NONE" Then
            DGV_HISTOGRAM.DataSource = Nothing
            Exit Sub
        End If

        sql = CreateSQL(C_HISOGRAM_STAT)

        'SQL実行
        If frm_Login.G_DB.SET_RESULTSET(sql) Then
            Me.Refresh()
            'DataGridView初期化
            M_DATATABLE_HISTOGRAM = New DataTable
            frm_Login.G_DB.INIT_LOCAL_DATA_TABLE(M_DATATABLE_HISTOGRAM)
            'フェッチ&表示
            FetchFromResultsetToLocalDataTable(M_DATATABLE_HISTOGRAM, DGV_HISTOGRAM, Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
        End If

        '列幅調整
        DGV_HISTOGRAM.AutoResizeColumns(DataGridViewAutoSizeColumnMode.DisplayedCells)
        LBL_INFO.Text = M_COMMON.GetMessage("I0001")
    End Sub

    Private Sub FetchFromResultset(ByRef datatable As DataTable, ByRef DGV As DataGridView, Optional ByVal fetchRowCnt As Long = 1000000000)
        '#######################################################################
        '非接続型クエリのSELECT結果FETCH
        '#######################################################################

        Dim readedRowCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim minDispCnt As Long = 0
        Dim maxDispCnt As Long = 100000000
        Dim isInterrupt As Boolean = True

        TXT_KOMOKU.Focus()

        If TXT_MIN_DISP_CNT.Text <> vbNullString Then
            minDispCnt = CLng(TXT_MIN_DISP_CNT.Text)
        End If

        If TXT_MAX_DISP_CNT.Text <> vbNullString Then
            maxDispCnt = CLng(TXT_MAX_DISP_CNT.Text)
        End If

        'DGVのDataSourceに設定してあるDataTableを切り離さないとDatatableへの行追加が遅くなるため初期化する
        DGV.DataSource = Nothing

        Do While readedRowCnt < fetchRowCnt
            If frm_Login.G_DB.FETCH_TO_DATATABLE(datatable, refleshCnt, minDispCnt, maxDispCnt) = False Then
                isInterrupt = False
                Exit Do
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                '最終行までフェッチが終わっている場合
                isInterrupt = False
                'DGV.DataSource = datatable1
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
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
        Loop
        If datatable Is Nothing Then
            '画面が閉じられる等のケース
            Exit Sub
        End If
        DGV.DataSource = datatable

        M_COMMON.SetRowheaderWidth(DGV)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)

        If isInterrupt Then
            '未フェッチの行が残っている場合
            M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.INTERRUPT)
            M_FILE.WriteApLog("フェッチを中断しました(" & msg(0) & ")")
        Else
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            M_FILE.WriteApLog("フェッチが終了しました(" & msg(0) & ")")
        End If
    End Sub

    Private Sub FetchFromResultsetToLocalDataTable(ByRef datatable As DataTable, ByRef DGV As DataGridView, Optional ByVal fetchRowCnt As Long = 1000000000)
        '#######################################################################
        '非接続型クエリのSELECT結果FETCH
        '#######################################################################
        Dim readedRowCnt As Long
        Dim refleshCnt As Integer = Integer.Parse(M_CONST.GetInitParamValue("INIT_FETCH_REFLESH"))
        Dim msg(0) As String
        Dim minDispCnt As Long = 0
        Dim maxDispCnt As Long = 100000000
        Dim isInterrupt As Boolean = True

        TXT_KOMOKU.Focus()

        If TXT_MIN_DISP_CNT.Text <> vbNullString Then
            minDispCnt = CLng(TXT_MIN_DISP_CNT.Text)
        End If

        If TXT_MAX_DISP_CNT.Text <> vbNullString Then
            maxDispCnt = CLng(TXT_MAX_DISP_CNT.Text)
        End If

        'DGVのDataSourceに設定してあるDataTableを切り離さないとDatatableへの行追加が遅くなるため初期化する
        DGV.DataSource = Nothing

        Do While readedRowCnt < fetchRowCnt
            If frm_Login.G_DB.FETCH_TO_LOCAL_DATATABLE(datatable, refleshCnt, minDispCnt, maxDispCnt) = False Then
                isInterrupt = False
                Exit Do
            End If
            msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
            LBL_INFO.Text = M_COMMON.GetMessage("I0007", msg)

            If frm_Login.G_DB.GET_SELECT_FLG = False Then
                '最終行までフェッチが終わっている場合
                isInterrupt = False
                'DGV.DataSource = datatable1
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
                M_CANCELFLG = False
                MsgBox(M_COMMON.GetMessage("I0008"))
                Exit Do
            End If
        Loop
        If datatable Is Nothing Then
            '画面が閉じられる等のケース
            Exit Sub
        End If
        DGV.DataSource = datatable

        M_COMMON.SetRowheaderWidth(DGV)
        msg(0) = frm_Login.G_DB.GET_ROW_COUNT.ToString
        LBL_INFO.Text = M_COMMON.GetMessage("I0009", msg)

        If isInterrupt Then
            '未フェッチの行が残っている場合
            M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT()
            ChangeFormStatus(M_CONST.C_FORM_STATUS.INTERRUPT)
            M_FILE.WriteApLog("フェッチを中断しました(" & msg(0) & ")")
        Else
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
            M_FILE.WriteApLog("フェッチが終了しました(" & msg(0) & ")")
        End If
    End Sub

    Sub ChangeFormStatus(ByVal statusID As Short)
        If statusID = M_CONST.C_FORM_STATUS.NORMAL Then
            BTN_EXECUTE.Enabled = True
            BTN_COPY.Enabled = True
            BTN_CSV.Enabled = True
            BTN_CANCEL.Enabled = False
            BTN_RESTART.Enabled = False
            BTN_RESTART.Visible = False
        ElseIf statusID = M_CONST.C_FORM_STATUS.EXECUTING Then
            BTN_EXECUTE.Enabled = False
            BTN_COPY.Enabled = False
            BTN_CSV.Enabled = False
            BTN_CANCEL.Enabled = True
            BTN_RESTART.Enabled = False
            BTN_RESTART.Visible = False
        ElseIf statusID = M_CONST.C_FORM_STATUS.INTERRUPT Then
            BTN_EXECUTE.Enabled = True
            BTN_COPY.Enabled = True
            BTN_CSV.Enabled = True
            BTN_CANCEL.Enabled = False
            BTN_RESTART.Enabled = True
            BTN_RESTART.Visible = True
        End If
    End Sub

    Private Function CreateSQL(ByVal id As Short) As String
        Dim sql As String = vbNullString

        If id = C_DATA Then
            'データ表示
            sql = "SELECT "
            If TXT_KOMOKU.Text = vbNullString Then
                sql = sql & "*"
            Else
                sql = sql & TXT_KOMOKU.Text
            End If

            If M_SCHEMA_NAME <> vbNullString Then
                sql = sql & " FROM " & ControlChars.Quote & M_SCHEMA_NAME & ControlChars.Quote & "." & ControlChars.Quote & M_OBJECT_NAME & ControlChars.Quote
            Else
                sql = sql & " FROM " & ControlChars.Quote & M_OBJECT_NAME & ControlChars.Quote
            End If

            'パーティション表の場合
            If M_SUBPARTITION_NAME <> vbNullString Then
                sql = sql & " SUBPARTITION(" & M_SUBPARTITION_NAME & ") "
            ElseIf M_PARTITION_NAME <> vbNullString Then
                sql = sql & " PARTITION(" & M_PARTITION_NAME & ") "
            End If

            If TXT_JOKEN.Text <> vbNullString Then
                sql = sql & " WHERE " & TXT_JOKEN.Text
            End If

        ElseIf id = C_COL_INFO Then
            '列情報
            If M_OBJECT_TYPE = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Or M_OBJECT_TYPE = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
                'ディクショナリの場合
                sql = "SELECT COLUMN_NAME,COMMENTS "
                sql = sql & "FROM DICT_COLUMNS "
                sql = sql & "WHERE TABLE_NAME = '" & M_OBJECT_NAME & "'"
            Else
                If Integer.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
                    sql = "SELECT A.COLUMN_NAME, DATA_TYPE,DECODE(DATA_TYPE,'NUMBER',DECODE(DATA_PRECISION,NULL,NULL,DATA_PRECISION || ',' || DATA_SCALE),DATA_LENGTH) DATA_LENGTH,NULLABLE,DEFAULT_LENGTH,DATA_DEFAULT,NUM_DISTINCT,  "
                    sql = sql & "  DECODE(DATA_TYPE,'NUMBER',TO_CHAR(UTL_RAW.CAST_TO_NUMBER(LOW_VALUE)),'VARCHAR2',UTL_RAW.CAST_TO_VARCHAR2(LOW_VALUE),'VARCHAR',UTL_RAW.CAST_TO_VARCHAR2(LOW_VALUE),'CHAR',UTL_RAW.CAST_TO_VARCHAR2(LOW_VALUE),RAWTOHEX(LOW_VALUE)) LOW_VALUE,"
                    sql = sql & "  DECODE(DATA_TYPE,'NUMBER',TO_CHAR(UTL_RAW.CAST_TO_NUMBER(HIGH_VALUE)),'VARCHAR2',UTL_RAW.CAST_TO_VARCHAR2(HIGH_VALUE),'VARCHAR',UTL_RAW.CAST_TO_VARCHAR2(HIGH_VALUE),'CHAR',UTL_RAW.CAST_TO_VARCHAR2(HIGH_VALUE),RAWTOHEX(HIGH_VALUE)) HIGH_VALUE, "
                    'sql = sql & "  LOW_VALUE, HIGH_VALUE, "
                    sql = sql & "  DENSITY, NUM_NULLS, NUM_BUCKETS, LAST_ANALYZED, SAMPLE_SIZE, GLOBAL_STATS, AVG_COL_LEN, HISTOGRAM, HIDDEN_COLUMN, VIRTUAL_COLUMN, B.COMMENTS "
                    sql = sql & "FROM ALL_TAB_COLS A,ALL_COL_COMMENTS B "
                    sql = sql & "WHERE A.OWNER = '" & M_SCHEMA_NAME & "' AND "
                    sql = sql & "A.TABLE_NAME = '" & M_OBJECT_NAME & "' AND "
                    sql = sql & "B.OWNER = '" & M_SCHEMA_NAME & "' AND "
                    sql = sql & "B.TABLE_NAME = '" & M_OBJECT_NAME & "' AND "
                    sql = sql & "A.OWNER = B.OWNER AND "
                    sql = sql & "A.TABLE_NAME = B.TABLE_NAME AND "
                    sql = sql & "A.COLUMN_NAME = B.COLUMN_NAME "
                Else
                    sql = "SELECT COLUMN_NAME, DATA_TYPE,DECODE(DATA_TYPE,'NUMBER',DECODE(DATA_PRECISION,NULL,NULL,DATA_PRECISION || ',' || DATA_SCALE),DATA_LENGTH) DATA_LENGTH,NULLABLE,DEFAULT_LENGTH,DATA_DEFAULT,NUM_DISTINCT, LOW_VALUE, HIGH_VALUE, DENSITY, NUM_NULLS, NUM_BUCKETS, LAST_ANALYZED, SAMPLE_SIZE, GLOBAL_STATS, AVG_COL_LEN,B.COMMENTS "
                    'sql = "  UTL_RAW.CAST_TO_NUMBER(LOW_VALUE) LOW_VALUE, UTL_RAW.CAST_TO_NUMBER(HIGH_VALUE) HIGH_VALUE, "
                    sql = sql & "  LOW_VALUE, HIGH_VALUE, "
                    sql = sql & "  DENSITY, NUM_NULLS, NUM_BUCKETS, LAST_ANALYZED, SAMPLE_SIZE, GLOBAL_STATS, AVG_COL_LEN,B.COMMENTS "
                    sql = sql & "FROM ALL_TAB_COLUMNS,ALL_COL_COMMENTS B "
                    sql = sql & "WHERE OWNER = '" & M_SCHEMA_NAME & "' AND "
                    sql = sql & "TABLE_NAME = '" & M_OBJECT_NAME & "' AND "
                    sql = sql & "B.OWNER = '" & M_SCHEMA_NAME & "' AND "
                    sql = sql & "B.TABLE_NAME = '" & M_OBJECT_NAME & "' AND "
                    sql = sql & "A.OWNER = B.OWNER AND "
                    sql = sql & "A.TABLE_NAME = B.TABLE_NAME AND "
                    sql = sql & "A.COLUMN_NAME = B.COLUMN_NAME "
                End If
            End If

        ElseIf id = C_HISOGRAM_STAT Then
            '列のヒストグラム統計
            sql = "SELECT COLUMN_NAME, ENDPOINT_NUMBER, ENDPOINT_VALUE, ENDPOINT_ACTUAL_VALUE "
            sql = sql & "FROM ALL_TAB_HISTOGRAMS "
            sql = sql & "WHERE OWNER = '" & M_SCHEMA_NAME & "' AND "
            sql = sql & "TABLE_NAME = '" & M_OBJECT_NAME & "' AND "
            sql = sql & "COLUMN_NAME = '" & DGV_COL_STAT.CurrentRow.Cells(0).Value.ToString & "'"

        ElseIf id = C_SOURCE Then
            sql = "SELECT TEXT "
            sql = sql & "FROM ALL_SOURCE "
            sql = sql & "WHERE OWNER = '" & M_SCHEMA_NAME & "' AND "
            sql = sql & "NAME = '" & M_OBJECT_NAME & "' AND "
            sql = sql & "TYPE = '" & M_OBJECT_TYPE & "' "
            sql = sql & "ORDER BY LINE"
        End If

        CreateSQL = sql

    End Function

    Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs)
        Dim col As Integer
        Dim row As Integer

        If Not (e.Exception Is Nothing) Then
            e.Cancel = False
            row = e.RowIndex
            col = e.ColumnIndex
        End If
    End Sub

    Private Sub frm_showdata_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'If BTN_CANCEL.Enabled = True Then
        '    '処理実行中はESCキー以外の入力を無視
        '    If e.KeyCode = Keys.Escape Then
        '        M_CANCELFLG = True
        '    Else
        '        e.Handled = True
        '    End If
        'End If

        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub BTN_CANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CANCEL.Click
        M_CANCELFLG = True
    End Sub

    Private Sub BTN_SHOW_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SHOW_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub frm_Showdata_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If M_SCHEMA_NAME <> vbNullString Then
            Me.Text = M_SCHEMA_NAME & "." & M_OBJECT_NAME
        Else
            Me.Text = M_OBJECT_NAME
        End If

        If M_PARTITION_NAME <> vbNullString Then
            Me.Text = Me.Text & " PARTITION:" & M_PARTITION_NAME
        End If

        If M_SUBPARTITION_NAME <> vbNullString Then
            Me.Text = Me.Text & " SUBPARTITION:" & M_SUBPARTITION_NAME
        End If
        Me.Text = "O-Analyzer - " & Me.Text & "(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"
        LBL_INFO.Text = ""
        Me.Width = 800

        TXT_SOURCE.BackColor = Color.White
        TXT_SOURCE.LanguageOption = RichTextBoxLanguageOptions.UIFonts

        M_FILE.SetControls(Me.Name, Me.Controls)

        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub submit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_EXECUTE.Click
        If tabControl1.SelectedTab.Name = "TAB_DATA" Then
            DispData(C_DATA)
        ElseIf tabControl1.SelectedTab.Name = "TAB_COL_STAT" Then
            SelectColInfo()
        ElseIf tabControl1.SelectedTab.Name = "TAB_SOURCE" Then
            DispSource()
        End If
    End Sub

    Private Sub SplitContainer1_Panel1_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel1.Resize
        DGV_COL_STAT.Height = (SplitContainer1.Panel1.Height) - 15
    End Sub
    Private Sub SplitContainer1_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer1.Panel2.Resize
        DGV_HISTOGRAM.Height = (SplitContainer1.Panel2.Height) - 15
    End Sub

    Private Sub frm_showdata_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        GroupBox1.Location = New Point(Me.Width - GroupBox1.Width - 20, 5)

        tabControl1.Width = Me.Width - GroupBox1.Width - 30
        tabControl1.Height = Me.Height - LBL_INFO.Height - 50
        TAB_DATA.Width = tabControl1.Width - 10
        TAB_DATA.Height = tabControl1.Height - 10
        TAB_COL_STAT.Width = tabControl1.Width - 10
        TAB_COL_STAT.Height = tabControl1.Height - 10
        SplitContainer1.Width = TAB_COL_STAT.Width - 10
        SplitContainer1.Height = TAB_COL_STAT.Height - 10

        DGV_DATA.Width = TAB_DATA.Width - 10
        DGV_DATA.Height = TAB_DATA.Height - TXT_JOKEN.Height - TXT_KOMOKU.Height - 25
        DGV_COL_STAT.Width = SplitContainer1.Width
        DGV_COL_STAT.Height = SplitContainer1.Panel1.Height - 15
        DGV_HISTOGRAM.Width = SplitContainer1.Width
        DGV_HISTOGRAM.Height = SplitContainer1.Panel2.Height - 15

        GroupBox1.Height = Me.Height - LBL_INFO.Height - 50
        LBL_INFO.Location = New Point(5, Me.Height - 55)
    End Sub

    Private Sub DGV_COL_STAT_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGV_COL_STAT.CurrentCellChanged
        'データグリッドビューにデータがない場合は処理を抜ける
        If (DGV_COL_STAT.DataSource Is Nothing) Or (DGV_COL_STAT.CurrentRow Is Nothing) Then
            Exit Sub
        End If

        SelectHistogramStat()
    End Sub

    Private Sub DGV_RowPostPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles DGV_DATA.RowPostPaint, DGV_COL_STAT.RowPostPaint, DGV_HISTOGRAM.RowPostPaint
        '行ヘッダに行番号を表示する
        M_COMMON.PaintDGVRownum(sender, e)
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

    Private Sub EXCUTING_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TXT_KOMOKU.KeyDown
        '処理実行中はESCキー以外の入力を無視
        If BTN_CANCEL.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                BTN_CANCEL_Click(Me, AcceptButton)
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub BTN_RESTART_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_RESTART.Click
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        If M_SET_READER_CNT = frm_Login.G_DB.GET_SET_READER_CNT Then
            FetchFromResultset(M_DATATABLE_DATA, DGV_DATA, Long.Parse(M_CONST.GetInitParamValue("INIT_FETCH")))
            '列幅調整
            If CHK_COLSIZE_REGULATE.Checked Then
                DGV_DATA.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            End If
        Else
            MsgBox(M_COMMON.GetMessage("I0035"))
            ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
        End If
    End Sub

    Private Sub BTN_CSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CSV.Click
        Dim msg(0) As String
        Dim dgv As DataGridView

        If tabControl1.SelectedTab.Name = "TAB_DATA" Then
            dgv = DGV_DATA
        ElseIf tabControl1.SelectedTab.Name = "TAB_COL_STAT" Then
            dgv = DGV_COL_STAT
        Else
            dgv = DGV_DATA
        End If

        LBL_INFO.Text = M_COMMON.GetMessage("I0029")
        ChangeFormStatus(M_CONST.C_FORM_STATUS.EXECUTING)
        Me.Refresh()

        If M_FILE.CreateCSV(DGV_DATA, msg(0), Format(Now(), "yyyyMMddHHmmss") & ".csv") Then
            MsgBox(M_COMMON.GetMessage("I0012"))
        End If

        LBL_INFO.Text = vbNullString
        ChangeFormStatus(M_CONST.C_FORM_STATUS.NORMAL)
    End Sub

    Private Sub frm_showdata_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        frm_Login.G_DB.DISPOSE_DATA_TABLE()

        If Not (M_DATATABLE_DATA Is Nothing) Then
            M_DATATABLE_DATA.Dispose()
            M_DATATABLE_DATA = Nothing
        End If

        If Not (DGV_DATA Is Nothing) Then
            DGV_DATA.Dispose()
            DGV_DATA = Nothing
        End If

        If Not (DGV_COL_STAT Is Nothing) Then
            DGV_COL_STAT.Dispose()
            DGV_COL_STAT = Nothing
        End If

        If Not (DGV_HISTOGRAM Is Nothing) Then
            DGV_HISTOGRAM.Dispose()
            DGV_HISTOGRAM = Nothing
        End If

        'GCを実行してメモリを解放する。なぜか2回実行しないと解放されない
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
        GC.WaitForPendingFinalizers()

        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)
        Me.Dispose()
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
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
End Class
