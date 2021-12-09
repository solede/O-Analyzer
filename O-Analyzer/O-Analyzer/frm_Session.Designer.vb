<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Session
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Session))
        Me.DGV_SESSION = New System.Windows.Forms.DataGridView()
        Me.BTN_KILL = New System.Windows.Forms.Button()
        Me.BTN_EXEC = New System.Windows.Forms.Button()
        Me.BTN_CSV = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_GRAPH = New System.Windows.Forms.Button()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_TRACE = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.TXT_EXECUTING_SQL = New System.Windows.Forms.TextBox()
        Me.DGV_SYSSTAT = New System.Windows.Forms.DataGridView()
        Me.CHK_SYSSTAT = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CHK_ONLY_ACTIVE = New System.Windows.Forms.CheckBox()
        Me.CHK_ONLY_USER = New System.Windows.Forms.CheckBox()
        Me.TXT_USER_DEFINE = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TXT_PGM = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TXT_USERNAME = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TXT_OSUSER = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.DGV_WAIT_EVENT = New System.Windows.Forms.DataGridView()
        Me.CHK_WAIT_EVENT = New System.Windows.Forms.CheckBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.CHK_CHECK_SQL = New System.Windows.Forms.CheckBox()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.TAB_SQL = New System.Windows.Forms.TabPage()
        Me.TAB_SESSTAT = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TAB_SESWAIT = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CHK_NOT_IDLE = New System.Windows.Forms.CheckBox()
        Me.TXT_USER_DEFINE3 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TAB_PROC = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE4 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DGV_PROCESS = New System.Windows.Forms.DataGridView()
        Me.CHK_PROCESS = New System.Windows.Forms.CheckBox()
        Me.TAB_LATCH = New System.Windows.Forms.TabPage()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE5 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.DGV_LATCH = New System.Windows.Forms.DataGridView()
        Me.CHK_LATCH = New System.Windows.Forms.CheckBox()
        Me.TAB_LOCK = New System.Windows.Forms.TabPage()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE6 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.DGV_LOCK = New System.Windows.Forms.DataGridView()
        Me.CHK_LOCK = New System.Windows.Forms.CheckBox()
        Me.TAB_CURSOR = New System.Windows.Forms.TabPage()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE7 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.DGV_CURSOR = New System.Windows.Forms.DataGridView()
        Me.CHK_CURSOR = New System.Windows.Forms.CheckBox()
        Me.TAB_TIME_MODEL = New System.Windows.Forms.TabPage()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.DGV_TIME_MODEL = New System.Windows.Forms.DataGridView()
        Me.CHK_TIME_MODEL = New System.Windows.Forms.CheckBox()
        CType(Me.DGV_SESSION, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DGV_SYSSTAT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DGV_WAIT_EVENT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.tabControl1.SuspendLayout()
        Me.TAB_SQL.SuspendLayout()
        Me.TAB_SESSTAT.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TAB_SESWAIT.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TAB_PROC.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.DGV_PROCESS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_LATCH.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        CType(Me.DGV_LATCH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_LOCK.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        CType(Me.DGV_LOCK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_CURSOR.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        CType(Me.DGV_CURSOR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_TIME_MODEL.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        CType(Me.DGV_TIME_MODEL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGV_SESSION
        '
        Me.DGV_SESSION.AllowUserToAddRows = False
        Me.DGV_SESSION.AllowUserToDeleteRows = False
        Me.DGV_SESSION.AllowUserToResizeRows = False
        Me.DGV_SESSION.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_SESSION.Location = New System.Drawing.Point(5, 70)
        Me.DGV_SESSION.Name = "DGV_SESSION"
        Me.DGV_SESSION.ReadOnly = True
        Me.DGV_SESSION.RowTemplate.Height = 21
        Me.DGV_SESSION.Size = New System.Drawing.Size(687, 227)
        Me.DGV_SESSION.StandardTab = True
        Me.DGV_SESSION.TabIndex = 1
        Me.DGV_SESSION.TabStop = False
        '
        'BTN_KILL
        '
        Me.BTN_KILL.Location = New System.Drawing.Point(6, 49)
        Me.BTN_KILL.Name = "BTN_KILL"
        Me.BTN_KILL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_KILL.TabIndex = 2
        Me.BTN_KILL.TabStop = False
        Me.BTN_KILL.Text = "切断(&K)"
        Me.BTN_KILL.UseVisualStyleBackColor = True
        '
        'BTN_EXEC
        '
        Me.BTN_EXEC.Location = New System.Drawing.Point(6, 18)
        Me.BTN_EXEC.Name = "BTN_EXEC"
        Me.BTN_EXEC.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXEC.TabIndex = 1
        Me.BTN_EXEC.TabStop = False
        Me.BTN_EXEC.Text = "実行(&E)"
        Me.BTN_EXEC.UseVisualStyleBackColor = True
        '
        'BTN_CSV
        '
        Me.BTN_CSV.Location = New System.Drawing.Point(6, 142)
        Me.BTN_CSV.Name = "BTN_CSV"
        Me.BTN_CSV.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CSV.TabIndex = 4
        Me.BTN_CSV.TabStop = False
        Me.BTN_CSV.Text = "CSV(&O)"
        Me.BTN_CSV.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_GRAPH)
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_TRACE)
        Me.GroupBox1.Controls.Add(Me.BTN_EXEC)
        Me.GroupBox1.Controls.Add(Me.BTN_CSV)
        Me.GroupBox1.Controls.Add(Me.BTN_KILL)
        Me.GroupBox1.Location = New System.Drawing.Point(698, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 530)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_GRAPH
        '
        Me.BTN_GRAPH.Location = New System.Drawing.Point(6, 111)
        Me.BTN_GRAPH.Name = "BTN_GRAPH"
        Me.BTN_GRAPH.Size = New System.Drawing.Size(68, 25)
        Me.BTN_GRAPH.TabIndex = 25
        Me.BTN_GRAPH.TabStop = False
        Me.BTN_GRAPH.Text = "統計グラフ(&G)"
        Me.BTN_GRAPH.UseVisualStyleBackColor = True
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 204)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 24
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 173)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 5
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_TRACE
        '
        Me.BTN_TRACE.Location = New System.Drawing.Point(6, 80)
        Me.BTN_TRACE.Name = "BTN_TRACE"
        Me.BTN_TRACE.Size = New System.Drawing.Size(68, 25)
        Me.BTN_TRACE.TabIndex = 3
        Me.BTN_TRACE.TabStop = False
        Me.BTN_TRACE.Text = "トレース(&T)"
        Me.BTN_TRACE.UseVisualStyleBackColor = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(11, 541)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(38, 12)
        Me.LBL_INFO.TabIndex = 4
        Me.LBL_INFO.Text = "Label1"
        '
        'TXT_EXECUTING_SQL
        '
        Me.TXT_EXECUTING_SQL.Location = New System.Drawing.Point(8, 18)
        Me.TXT_EXECUTING_SQL.Multiline = True
        Me.TXT_EXECUTING_SQL.Name = "TXT_EXECUTING_SQL"
        Me.TXT_EXECUTING_SQL.ReadOnly = True
        Me.TXT_EXECUTING_SQL.Size = New System.Drawing.Size(654, 174)
        Me.TXT_EXECUTING_SQL.TabIndex = 9
        '
        'DGV_SYSSTAT
        '
        Me.DGV_SYSSTAT.AllowUserToAddRows = False
        Me.DGV_SYSSTAT.AllowUserToDeleteRows = False
        Me.DGV_SYSSTAT.AllowUserToResizeRows = False
        Me.DGV_SYSSTAT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_SYSSTAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_SYSSTAT.Location = New System.Drawing.Point(6, 40)
        Me.DGV_SYSSTAT.Name = "DGV_SYSSTAT"
        Me.DGV_SYSSTAT.ReadOnly = True
        Me.DGV_SYSSTAT.RowTemplate.Height = 21
        Me.DGV_SYSSTAT.Size = New System.Drawing.Size(659, 148)
        Me.DGV_SYSSTAT.StandardTab = True
        Me.DGV_SYSSTAT.TabIndex = 11
        Me.DGV_SYSSTAT.TabStop = False
        '
        'CHK_SYSSTAT
        '
        Me.CHK_SYSSTAT.AutoSize = True
        Me.CHK_SYSSTAT.Location = New System.Drawing.Point(6, 0)
        Me.CHK_SYSSTAT.Name = "CHK_SYSSTAT"
        Me.CHK_SYSSTAT.Size = New System.Drawing.Size(142, 16)
        Me.CHK_SYSSTAT.TabIndex = 10
        Me.CHK_SYSSTAT.Text = "セッション統計を確認する"
        Me.CHK_SYSSTAT.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CHK_ONLY_ACTIVE)
        Me.GroupBox3.Controls.Add(Me.CHK_ONLY_USER)
        Me.GroupBox3.Controls.Add(Me.TXT_USER_DEFINE)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.TXT_PGM)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.TXT_USERNAME)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.TXT_OSUSER)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(687, 60)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "抽出条件"
        '
        'CHK_ONLY_ACTIVE
        '
        Me.CHK_ONLY_ACTIVE.AutoSize = True
        Me.CHK_ONLY_ACTIVE.Location = New System.Drawing.Point(595, 14)
        Me.CHK_ONLY_ACTIVE.Name = "CHK_ONLY_ACTIVE"
        Me.CHK_ONLY_ACTIVE.Size = New System.Drawing.Size(81, 16)
        Me.CHK_ONLY_ACTIVE.TabIndex = 7
        Me.CHK_ONLY_ACTIVE.Text = "処理中のみ"
        Me.CHK_ONLY_ACTIVE.UseVisualStyleBackColor = True
        '
        'CHK_ONLY_USER
        '
        Me.CHK_ONLY_USER.AutoSize = True
        Me.CHK_ONLY_USER.Location = New System.Drawing.Point(472, 14)
        Me.CHK_ONLY_USER.Name = "CHK_ONLY_USER"
        Me.CHK_ONLY_USER.Size = New System.Drawing.Size(117, 16)
        Me.CHK_ONLY_USER.TabIndex = 6
        Me.CHK_ONLY_USER.Text = "ユーザセッションのみ"
        Me.CHK_ONLY_USER.UseVisualStyleBackColor = True
        '
        'TXT_USER_DEFINE
        '
        Me.TXT_USER_DEFINE.Location = New System.Drawing.Point(95, 35)
        Me.TXT_USER_DEFINE.Name = "TXT_USER_DEFINE"
        Me.TXT_USER_DEFINE.Size = New System.Drawing.Size(586, 19)
        Me.TXT_USER_DEFINE.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 12)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "ユーザ定義条件"
        '
        'TXT_PGM
        '
        Me.TXT_PGM.Location = New System.Drawing.Point(205, 12)
        Me.TXT_PGM.Name = "TXT_PGM"
        Me.TXT_PGM.Size = New System.Drawing.Size(87, 19)
        Me.TXT_PGM.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(149, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "プログラム"
        '
        'TXT_USERNAME
        '
        Me.TXT_USERNAME.Location = New System.Drawing.Point(47, 12)
        Me.TXT_USERNAME.Name = "TXT_USERNAME"
        Me.TXT_USERNAME.Size = New System.Drawing.Size(87, 19)
        Me.TXT_USERNAME.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 15)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 12)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "ユーザ"
        '
        'TXT_OSUSER
        '
        Me.TXT_OSUSER.Location = New System.Drawing.Point(363, 12)
        Me.TXT_OSUSER.Name = "TXT_OSUSER"
        Me.TXT_OSUSER.Size = New System.Drawing.Size(87, 19)
        Me.TXT_OSUSER.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(307, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 12)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "OSユーザ"
        '
        'DGV_WAIT_EVENT
        '
        Me.DGV_WAIT_EVENT.AllowUserToAddRows = False
        Me.DGV_WAIT_EVENT.AllowUserToDeleteRows = False
        Me.DGV_WAIT_EVENT.AllowUserToResizeRows = False
        Me.DGV_WAIT_EVENT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_WAIT_EVENT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_WAIT_EVENT.Location = New System.Drawing.Point(6, 40)
        Me.DGV_WAIT_EVENT.Name = "DGV_WAIT_EVENT"
        Me.DGV_WAIT_EVENT.ReadOnly = True
        Me.DGV_WAIT_EVENT.RowTemplate.Height = 21
        Me.DGV_WAIT_EVENT.Size = New System.Drawing.Size(656, 148)
        Me.DGV_WAIT_EVENT.StandardTab = True
        Me.DGV_WAIT_EVENT.TabIndex = 13
        Me.DGV_WAIT_EVENT.TabStop = False
        '
        'CHK_WAIT_EVENT
        '
        Me.CHK_WAIT_EVENT.AutoSize = True
        Me.CHK_WAIT_EVENT.Location = New System.Drawing.Point(6, 0)
        Me.CHK_WAIT_EVENT.Name = "CHK_WAIT_EVENT"
        Me.CHK_WAIT_EVENT.Size = New System.Drawing.Size(136, 16)
        Me.CHK_WAIT_EVENT.TabIndex = 12
        Me.CHK_WAIT_EVENT.Text = "待機イベントを確認する"
        Me.CHK_WAIT_EVENT.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.CHK_CHECK_SQL)
        Me.GroupBox5.Controls.Add(Me.TXT_EXECUTING_SQL)
        Me.GroupBox5.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(668, 198)
        Me.GroupBox5.TabIndex = 25
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "             　　    　　                 "
        '
        'CHK_CHECK_SQL
        '
        Me.CHK_CHECK_SQL.AutoSize = True
        Me.CHK_CHECK_SQL.Location = New System.Drawing.Point(6, 0)
        Me.CHK_CHECK_SQL.Name = "CHK_CHECK_SQL"
        Me.CHK_CHECK_SQL.Size = New System.Drawing.Size(172, 16)
        Me.CHK_CHECK_SQL.TabIndex = 8
        Me.CHK_CHECK_SQL.Text = "最後に実行したSQLを確認する"
        Me.CHK_CHECK_SQL.UseVisualStyleBackColor = True
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.TAB_SQL)
        Me.tabControl1.Controls.Add(Me.TAB_SESSTAT)
        Me.tabControl1.Controls.Add(Me.TAB_SESWAIT)
        Me.tabControl1.Controls.Add(Me.TAB_PROC)
        Me.tabControl1.Controls.Add(Me.TAB_LATCH)
        Me.tabControl1.Controls.Add(Me.TAB_LOCK)
        Me.tabControl1.Controls.Add(Me.TAB_CURSOR)
        Me.tabControl1.Controls.Add(Me.TAB_TIME_MODEL)
        Me.tabControl1.Location = New System.Drawing.Point(5, 303)
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(687, 232)
        Me.tabControl1.TabIndex = 2
        Me.tabControl1.TabStop = False
        '
        'TAB_SQL
        '
        Me.TAB_SQL.Controls.Add(Me.GroupBox5)
        Me.TAB_SQL.Location = New System.Drawing.Point(4, 22)
        Me.TAB_SQL.Name = "TAB_SQL"
        Me.TAB_SQL.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_SQL.Size = New System.Drawing.Size(679, 206)
        Me.TAB_SQL.TabIndex = 0
        Me.TAB_SQL.Text = "実行SQL"
        Me.TAB_SQL.UseVisualStyleBackColor = True
        '
        'TAB_SESSTAT
        '
        Me.TAB_SESSTAT.Controls.Add(Me.GroupBox4)
        Me.TAB_SESSTAT.Location = New System.Drawing.Point(4, 22)
        Me.TAB_SESSTAT.Name = "TAB_SESSTAT"
        Me.TAB_SESSTAT.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_SESSTAT.Size = New System.Drawing.Size(679, 206)
        Me.TAB_SESSTAT.TabIndex = 1
        Me.TAB_SESSTAT.Text = "セッション統計"
        Me.TAB_SESSTAT.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TXT_USER_DEFINE2)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.DGV_SYSSTAT)
        Me.GroupBox4.Controls.Add(Me.CHK_SYSSTAT)
        Me.GroupBox4.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(671, 195)
        Me.GroupBox4.TabIndex = 24
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "                                  "
        '
        'TXT_USER_DEFINE2
        '
        Me.TXT_USER_DEFINE2.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE2.Name = "TXT_USER_DEFINE2"
        Me.TXT_USER_DEFINE2.Size = New System.Drawing.Size(570, 19)
        Me.TXT_USER_DEFINE2.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 12)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "ユーザ定義条件"
        '
        'TAB_SESWAIT
        '
        Me.TAB_SESWAIT.Controls.Add(Me.GroupBox2)
        Me.TAB_SESWAIT.Location = New System.Drawing.Point(4, 22)
        Me.TAB_SESWAIT.Name = "TAB_SESWAIT"
        Me.TAB_SESWAIT.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_SESWAIT.Size = New System.Drawing.Size(679, 206)
        Me.TAB_SESWAIT.TabIndex = 2
        Me.TAB_SESWAIT.Text = "待機イベント"
        Me.TAB_SESWAIT.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CHK_NOT_IDLE)
        Me.GroupBox2.Controls.Add(Me.TXT_USER_DEFINE3)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.DGV_WAIT_EVENT)
        Me.GroupBox2.Controls.Add(Me.CHK_WAIT_EVENT)
        Me.GroupBox2.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox2.TabIndex = 22
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "                                "
        '
        'CHK_NOT_IDLE
        '
        Me.CHK_NOT_IDLE.AutoSize = True
        Me.CHK_NOT_IDLE.Checked = True
        Me.CHK_NOT_IDLE.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CHK_NOT_IDLE.Location = New System.Drawing.Point(523, 19)
        Me.CHK_NOT_IDLE.Name = "CHK_NOT_IDLE"
        Me.CHK_NOT_IDLE.Size = New System.Drawing.Size(145, 16)
        Me.CHK_NOT_IDLE.TabIndex = 16
        Me.CHK_NOT_IDLE.Text = "アイドル待機イベント除外"
        Me.CHK_NOT_IDLE.UseVisualStyleBackColor = True
        '
        'TXT_USER_DEFINE3
        '
        Me.TXT_USER_DEFINE3.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE3.Name = "TXT_USER_DEFINE3"
        Me.TXT_USER_DEFINE3.Size = New System.Drawing.Size(420, 19)
        Me.TXT_USER_DEFINE3.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 12)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "ユーザ定義条件"
        '
        'TAB_PROC
        '
        Me.TAB_PROC.Controls.Add(Me.GroupBox6)
        Me.TAB_PROC.Location = New System.Drawing.Point(4, 22)
        Me.TAB_PROC.Name = "TAB_PROC"
        Me.TAB_PROC.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_PROC.Size = New System.Drawing.Size(679, 206)
        Me.TAB_PROC.TabIndex = 3
        Me.TAB_PROC.Text = "プロセス"
        Me.TAB_PROC.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.TXT_USER_DEFINE4)
        Me.GroupBox6.Controls.Add(Me.Label5)
        Me.GroupBox6.Controls.Add(Me.DGV_PROCESS)
        Me.GroupBox6.Controls.Add(Me.CHK_PROCESS)
        Me.GroupBox6.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox6.TabIndex = 23
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "                                 "
        '
        'TXT_USER_DEFINE4
        '
        Me.TXT_USER_DEFINE4.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE4.Name = "TXT_USER_DEFINE4"
        Me.TXT_USER_DEFINE4.Size = New System.Drawing.Size(570, 19)
        Me.TXT_USER_DEFINE4.TabIndex = 17
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 12)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "ユーザ定義条件"
        '
        'DGV_PROCESS
        '
        Me.DGV_PROCESS.AllowUserToAddRows = False
        Me.DGV_PROCESS.AllowUserToDeleteRows = False
        Me.DGV_PROCESS.AllowUserToResizeRows = False
        Me.DGV_PROCESS.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_PROCESS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_PROCESS.Location = New System.Drawing.Point(6, 40)
        Me.DGV_PROCESS.Name = "DGV_PROCESS"
        Me.DGV_PROCESS.ReadOnly = True
        Me.DGV_PROCESS.RowTemplate.Height = 21
        Me.DGV_PROCESS.Size = New System.Drawing.Size(656, 148)
        Me.DGV_PROCESS.StandardTab = True
        Me.DGV_PROCESS.TabIndex = 15
        Me.DGV_PROCESS.TabStop = False
        '
        'CHK_PROCESS
        '
        Me.CHK_PROCESS.AutoSize = True
        Me.CHK_PROCESS.Location = New System.Drawing.Point(6, 0)
        Me.CHK_PROCESS.Name = "CHK_PROCESS"
        Me.CHK_PROCESS.Size = New System.Drawing.Size(137, 16)
        Me.CHK_PROCESS.TabIndex = 14
        Me.CHK_PROCESS.Text = "プロセス情報を確認する"
        Me.CHK_PROCESS.UseVisualStyleBackColor = True
        '
        'TAB_LATCH
        '
        Me.TAB_LATCH.Controls.Add(Me.GroupBox7)
        Me.TAB_LATCH.Location = New System.Drawing.Point(4, 22)
        Me.TAB_LATCH.Name = "TAB_LATCH"
        Me.TAB_LATCH.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_LATCH.Size = New System.Drawing.Size(679, 206)
        Me.TAB_LATCH.TabIndex = 4
        Me.TAB_LATCH.Text = "ラッチ"
        Me.TAB_LATCH.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.TXT_USER_DEFINE5)
        Me.GroupBox7.Controls.Add(Me.Label6)
        Me.GroupBox7.Controls.Add(Me.DGV_LATCH)
        Me.GroupBox7.Controls.Add(Me.CHK_LATCH)
        Me.GroupBox7.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox7.TabIndex = 24
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "                        "
        '
        'TXT_USER_DEFINE5
        '
        Me.TXT_USER_DEFINE5.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE5.Name = "TXT_USER_DEFINE5"
        Me.TXT_USER_DEFINE5.Size = New System.Drawing.Size(570, 19)
        Me.TXT_USER_DEFINE5.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 19)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 12)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "ユーザ定義条件"
        '
        'DGV_LATCH
        '
        Me.DGV_LATCH.AllowUserToAddRows = False
        Me.DGV_LATCH.AllowUserToDeleteRows = False
        Me.DGV_LATCH.AllowUserToResizeRows = False
        Me.DGV_LATCH.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_LATCH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_LATCH.Location = New System.Drawing.Point(6, 40)
        Me.DGV_LATCH.Name = "DGV_LATCH"
        Me.DGV_LATCH.ReadOnly = True
        Me.DGV_LATCH.RowTemplate.Height = 21
        Me.DGV_LATCH.Size = New System.Drawing.Size(656, 148)
        Me.DGV_LATCH.StandardTab = True
        Me.DGV_LATCH.TabIndex = 2
        Me.DGV_LATCH.TabStop = False
        '
        'CHK_LATCH
        '
        Me.CHK_LATCH.AutoSize = True
        Me.CHK_LATCH.Location = New System.Drawing.Point(6, 0)
        Me.CHK_LATCH.Name = "CHK_LATCH"
        Me.CHK_LATCH.Size = New System.Drawing.Size(100, 16)
        Me.CHK_LATCH.TabIndex = 1
        Me.CHK_LATCH.Text = "ラッチを確認する"
        Me.CHK_LATCH.UseVisualStyleBackColor = True
        '
        'TAB_LOCK
        '
        Me.TAB_LOCK.Controls.Add(Me.GroupBox8)
        Me.TAB_LOCK.Location = New System.Drawing.Point(4, 22)
        Me.TAB_LOCK.Name = "TAB_LOCK"
        Me.TAB_LOCK.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_LOCK.Size = New System.Drawing.Size(679, 206)
        Me.TAB_LOCK.TabIndex = 5
        Me.TAB_LOCK.Text = "ロック"
        Me.TAB_LOCK.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.TXT_USER_DEFINE6)
        Me.GroupBox8.Controls.Add(Me.Label8)
        Me.GroupBox8.Controls.Add(Me.DGV_LOCK)
        Me.GroupBox8.Controls.Add(Me.CHK_LOCK)
        Me.GroupBox8.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox8.TabIndex = 25
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "                       "
        '
        'TXT_USER_DEFINE6
        '
        Me.TXT_USER_DEFINE6.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE6.Name = "TXT_USER_DEFINE6"
        Me.TXT_USER_DEFINE6.Size = New System.Drawing.Size(570, 19)
        Me.TXT_USER_DEFINE6.TabIndex = 15
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 12)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "ユーザ定義条件"
        '
        'DGV_LOCK
        '
        Me.DGV_LOCK.AllowUserToAddRows = False
        Me.DGV_LOCK.AllowUserToDeleteRows = False
        Me.DGV_LOCK.AllowUserToResizeRows = False
        Me.DGV_LOCK.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_LOCK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_LOCK.Location = New System.Drawing.Point(6, 40)
        Me.DGV_LOCK.Name = "DGV_LOCK"
        Me.DGV_LOCK.ReadOnly = True
        Me.DGV_LOCK.RowTemplate.Height = 21
        Me.DGV_LOCK.Size = New System.Drawing.Size(656, 148)
        Me.DGV_LOCK.StandardTab = True
        Me.DGV_LOCK.TabIndex = 2
        Me.DGV_LOCK.TabStop = False
        '
        'CHK_LOCK
        '
        Me.CHK_LOCK.AutoSize = True
        Me.CHK_LOCK.Location = New System.Drawing.Point(6, 0)
        Me.CHK_LOCK.Name = "CHK_LOCK"
        Me.CHK_LOCK.Size = New System.Drawing.Size(100, 16)
        Me.CHK_LOCK.TabIndex = 1
        Me.CHK_LOCK.Text = "ロックを確認する"
        Me.CHK_LOCK.UseVisualStyleBackColor = True
        '
        'TAB_CURSOR
        '
        Me.TAB_CURSOR.Controls.Add(Me.GroupBox9)
        Me.TAB_CURSOR.Location = New System.Drawing.Point(4, 22)
        Me.TAB_CURSOR.Name = "TAB_CURSOR"
        Me.TAB_CURSOR.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_CURSOR.Size = New System.Drawing.Size(679, 206)
        Me.TAB_CURSOR.TabIndex = 6
        Me.TAB_CURSOR.Text = "カーソル"
        Me.TAB_CURSOR.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.TXT_USER_DEFINE7)
        Me.GroupBox9.Controls.Add(Me.Label9)
        Me.GroupBox9.Controls.Add(Me.DGV_CURSOR)
        Me.GroupBox9.Controls.Add(Me.CHK_CURSOR)
        Me.GroupBox9.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox9.TabIndex = 23
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "                           "
        '
        'TXT_USER_DEFINE7
        '
        Me.TXT_USER_DEFINE7.Location = New System.Drawing.Point(95, 16)
        Me.TXT_USER_DEFINE7.Name = "TXT_USER_DEFINE7"
        Me.TXT_USER_DEFINE7.Size = New System.Drawing.Size(570, 19)
        Me.TXT_USER_DEFINE7.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 12)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "ユーザ定義条件"
        '
        'DGV_CURSOR
        '
        Me.DGV_CURSOR.AllowUserToAddRows = False
        Me.DGV_CURSOR.AllowUserToDeleteRows = False
        Me.DGV_CURSOR.AllowUserToResizeRows = False
        Me.DGV_CURSOR.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_CURSOR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_CURSOR.Location = New System.Drawing.Point(6, 40)
        Me.DGV_CURSOR.Name = "DGV_CURSOR"
        Me.DGV_CURSOR.ReadOnly = True
        Me.DGV_CURSOR.RowTemplate.Height = 21
        Me.DGV_CURSOR.Size = New System.Drawing.Size(656, 148)
        Me.DGV_CURSOR.StandardTab = True
        Me.DGV_CURSOR.TabIndex = 13
        Me.DGV_CURSOR.TabStop = False
        '
        'CHK_CURSOR
        '
        Me.CHK_CURSOR.AutoSize = True
        Me.CHK_CURSOR.Location = New System.Drawing.Point(6, 0)
        Me.CHK_CURSOR.Name = "CHK_CURSOR"
        Me.CHK_CURSOR.Size = New System.Drawing.Size(114, 16)
        Me.CHK_CURSOR.TabIndex = 12
        Me.CHK_CURSOR.Text = "カーソルを確認する"
        Me.CHK_CURSOR.UseVisualStyleBackColor = True
        '
        'TAB_TIME_MODEL
        '
        Me.TAB_TIME_MODEL.Controls.Add(Me.GroupBox10)
        Me.TAB_TIME_MODEL.Location = New System.Drawing.Point(4, 22)
        Me.TAB_TIME_MODEL.Name = "TAB_TIME_MODEL"
        Me.TAB_TIME_MODEL.Padding = New System.Windows.Forms.Padding(3)
        Me.TAB_TIME_MODEL.Size = New System.Drawing.Size(679, 206)
        Me.TAB_TIME_MODEL.TabIndex = 7
        Me.TAB_TIME_MODEL.Text = "時刻統計"
        Me.TAB_TIME_MODEL.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.TextBox1)
        Me.GroupBox10.Controls.Add(Me.Label11)
        Me.GroupBox10.Controls.Add(Me.DGV_TIME_MODEL)
        Me.GroupBox10.Controls.Add(Me.CHK_TIME_MODEL)
        Me.GroupBox10.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(668, 195)
        Me.GroupBox10.TabIndex = 24
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "                           "
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(95, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(570, 19)
        Me.TextBox1.TabIndex = 15
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(83, 12)
        Me.Label11.TabIndex = 14
        Me.Label11.Text = "ユーザ定義条件"
        '
        'DGV_TIME_MODEL
        '
        Me.DGV_TIME_MODEL.AllowUserToAddRows = False
        Me.DGV_TIME_MODEL.AllowUserToDeleteRows = False
        Me.DGV_TIME_MODEL.AllowUserToResizeRows = False
        Me.DGV_TIME_MODEL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DGV_TIME_MODEL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGV_TIME_MODEL.Location = New System.Drawing.Point(6, 40)
        Me.DGV_TIME_MODEL.Name = "DGV_TIME_MODEL"
        Me.DGV_TIME_MODEL.ReadOnly = True
        Me.DGV_TIME_MODEL.RowTemplate.Height = 21
        Me.DGV_TIME_MODEL.Size = New System.Drawing.Size(656, 148)
        Me.DGV_TIME_MODEL.StandardTab = True
        Me.DGV_TIME_MODEL.TabIndex = 13
        Me.DGV_TIME_MODEL.TabStop = False
        '
        'CHK_TIME_MODEL
        '
        Me.CHK_TIME_MODEL.AutoSize = True
        Me.CHK_TIME_MODEL.Location = New System.Drawing.Point(6, 0)
        Me.CHK_TIME_MODEL.Name = "CHK_TIME_MODEL"
        Me.CHK_TIME_MODEL.Size = New System.Drawing.Size(124, 16)
        Me.CHK_TIME_MODEL.TabIndex = 12
        Me.CHK_TIME_MODEL.Text = "時刻統計を確認する"
        Me.CHK_TIME_MODEL.UseVisualStyleBackColor = True
        '
        'frm_Session
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.tabControl1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Controls.Add(Me.DGV_SESSION)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Session"
        Me.Text = "セッション情報"
        CType(Me.DGV_SESSION, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DGV_SYSSTAT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.DGV_WAIT_EVENT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.tabControl1.ResumeLayout(False)
        Me.TAB_SQL.ResumeLayout(False)
        Me.TAB_SESSTAT.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.TAB_SESWAIT.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TAB_PROC.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.DGV_PROCESS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_LATCH.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        CType(Me.DGV_LATCH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_LOCK.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        CType(Me.DGV_LOCK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_CURSOR.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        CType(Me.DGV_CURSOR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_TIME_MODEL.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        CType(Me.DGV_TIME_MODEL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DGV_SESSION As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_KILL As System.Windows.Forms.Button
    Friend WithEvents BTN_EXEC As System.Windows.Forms.Button
    Friend WithEvents BTN_CSV As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents BTN_TRACE As System.Windows.Forms.Button
    Friend WithEvents TXT_EXECUTING_SQL As System.Windows.Forms.TextBox
    Friend WithEvents CHK_SYSSTAT As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TXT_USERNAME As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TXT_OSUSER As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TXT_PGM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TXT_USER_DEFINE As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CHK_WAIT_EVENT As System.Windows.Forms.CheckBox
    Friend WithEvents DGV_WAIT_EVENT As System.Windows.Forms.DataGridView
    Friend WithEvents DGV_SYSSTAT As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents CHK_CHECK_SQL As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_ONLY_USER As System.Windows.Forms.CheckBox
    Friend WithEvents tabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TAB_SQL As System.Windows.Forms.TabPage
    Friend WithEvents TAB_SESSTAT As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TAB_SESWAIT As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TAB_PROC As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents DGV_PROCESS As System.Windows.Forms.DataGridView
    Friend WithEvents CHK_PROCESS As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents CHK_ONLY_ACTIVE As System.Windows.Forms.CheckBox
    Friend WithEvents TAB_LATCH As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents DGV_LATCH As System.Windows.Forms.DataGridView
    Friend WithEvents CHK_LATCH As System.Windows.Forms.CheckBox
    Friend WithEvents TXT_USER_DEFINE2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_USER_DEFINE3 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TXT_USER_DEFINE5 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CHK_NOT_IDLE As System.Windows.Forms.CheckBox
    Friend WithEvents TXT_USER_DEFINE4 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TAB_LOCK As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents TXT_USER_DEFINE6 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents DGV_LOCK As System.Windows.Forms.DataGridView
    Friend WithEvents CHK_LOCK As System.Windows.Forms.CheckBox
    Friend WithEvents TAB_CURSOR As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents TXT_USER_DEFINE7 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DGV_CURSOR As System.Windows.Forms.DataGridView
    Friend WithEvents CHK_CURSOR As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents BTN_GRAPH As System.Windows.Forms.Button
    Friend WithEvents TAB_TIME_MODEL As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents DGV_TIME_MODEL As System.Windows.Forms.DataGridView
    Friend WithEvents CHK_TIME_MODEL As System.Windows.Forms.CheckBox
End Class
