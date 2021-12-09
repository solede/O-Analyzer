<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Execsql
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Execsql))
        Me.BTN_EXECUTE = New System.Windows.Forms.Button()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.BTN_CSV = New System.Windows.Forms.Button()
        Me.BTN_COPY = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_ALL_COPY = New System.Windows.Forms.Button()
        Me.BTN_HIST_DOWN = New System.Windows.Forms.Button()
        Me.BTN_HIST_UP = New System.Windows.Forms.Button()
        Me.BTN_BIND = New System.Windows.Forms.Button()
        Me.BTN_RESTART = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TXT_SQL = New System.Windows.Forms.RichTextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.RB_SELECT2 = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TXT_EXEC_CNT = New CLS_COMMON.RegExTextBox()
        Me.TXT_TIMER_INTERVAL = New CLS_COMMON.RegExTextBox()
        Me.CHK_TIMER = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RB_SELECT1 = New System.Windows.Forms.RadioButton()
        Me.TXT_MAX_DISP_CNT = New CLS_COMMON.RegExTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TXT_MIN_DISP_CNT = New CLS_COMMON.RegExTextBox()
        Me.CMB_DISPLAY = New System.Windows.Forms.ComboBox()
        Me.CHK_COLSIZE_REGULATE = New System.Windows.Forms.CheckBox()
        Me.TXT_TIMEOUT_VALUE = New CLS_COMMON.RegExTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LBL_TIMEOUT_VALUE = New System.Windows.Forms.Label()
        Me.DGV_DATA = New System.Windows.Forms.DataGridView()
        Me.TXT_WATCH_INTERVAL = New CLS_COMMON.RegExTextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CHK_REFRESH = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DGV_DATA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BTN_EXECUTE
        '
        Me.BTN_EXECUTE.Location = New System.Drawing.Point(6, 18)
        Me.BTN_EXECUTE.Name = "BTN_EXECUTE"
        Me.BTN_EXECUTE.Size = New System.Drawing.Size(68, 25)
        Me.BTN_EXECUTE.TabIndex = 1
        Me.BTN_EXECUTE.TabStop = False
        Me.BTN_EXECUTE.Text = "実行(&E)"
        Me.BTN_EXECUTE.UseVisualStyleBackColor = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(3, 545)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(40, 12)
        Me.LBL_INFO.TabIndex = 15
        Me.LBL_INFO.Tag = "label"
        Me.LBL_INFO.Text = "LABEL"
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
        'BTN_COPY
        '
        Me.BTN_COPY.Location = New System.Drawing.Point(6, 111)
        Me.BTN_COPY.Name = "BTN_COPY"
        Me.BTN_COPY.Size = New System.Drawing.Size(68, 25)
        Me.BTN_COPY.TabIndex = 3
        Me.BTN_COPY.TabStop = False
        Me.BTN_COPY.Text = "選択コピー"
        Me.BTN_COPY.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(6, 49)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 2
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 204)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 6
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ(&L)"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_HELP)
        Me.GroupBox1.Controls.Add(Me.BTN_ALL_COPY)
        Me.GroupBox1.Controls.Add(Me.BTN_HIST_DOWN)
        Me.GroupBox1.Controls.Add(Me.BTN_HIST_UP)
        Me.GroupBox1.Controls.Add(Me.BTN_BIND)
        Me.GroupBox1.Controls.Add(Me.BTN_EXECUTE)
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_COPY)
        Me.GroupBox1.Controls.Add(Me.BTN_CSV)
        Me.GroupBox1.Controls.Add(Me.BTN_RESTART)
        Me.GroupBox1.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox1.Location = New System.Drawing.Point(700, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 537)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(6, 297)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HELP.TabIndex = 23
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_ALL_COPY
        '
        Me.BTN_ALL_COPY.Location = New System.Drawing.Point(6, 80)
        Me.BTN_ALL_COPY.Name = "BTN_ALL_COPY"
        Me.BTN_ALL_COPY.Size = New System.Drawing.Size(68, 25)
        Me.BTN_ALL_COPY.TabIndex = 16
        Me.BTN_ALL_COPY.TabStop = False
        Me.BTN_ALL_COPY.Text = "全コピー(&C)"
        Me.BTN_ALL_COPY.UseVisualStyleBackColor = True
        '
        'BTN_HIST_DOWN
        '
        Me.BTN_HIST_DOWN.Location = New System.Drawing.Point(6, 266)
        Me.BTN_HIST_DOWN.Name = "BTN_HIST_DOWN"
        Me.BTN_HIST_DOWN.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HIST_DOWN.TabIndex = 22
        Me.BTN_HIST_DOWN.TabStop = False
        Me.BTN_HIST_DOWN.Text = "履歴↓(&D)"
        Me.BTN_HIST_DOWN.UseVisualStyleBackColor = True
        '
        'BTN_HIST_UP
        '
        Me.BTN_HIST_UP.Location = New System.Drawing.Point(6, 235)
        Me.BTN_HIST_UP.Name = "BTN_HIST_UP"
        Me.BTN_HIST_UP.Size = New System.Drawing.Size(68, 25)
        Me.BTN_HIST_UP.TabIndex = 21
        Me.BTN_HIST_UP.TabStop = False
        Me.BTN_HIST_UP.Text = "履歴↑(&U)"
        Me.BTN_HIST_UP.UseVisualStyleBackColor = True
        '
        'BTN_BIND
        '
        Me.BTN_BIND.Location = New System.Drawing.Point(6, 173)
        Me.BTN_BIND.Name = "BTN_BIND"
        Me.BTN_BIND.Size = New System.Drawing.Size(68, 25)
        Me.BTN_BIND.TabIndex = 5
        Me.BTN_BIND.TabStop = False
        Me.BTN_BIND.Text = "バインド(&B)"
        Me.BTN_BIND.UseVisualStyleBackColor = True
        '
        'BTN_RESTART
        '
        Me.BTN_RESTART.Location = New System.Drawing.Point(6, 49)
        Me.BTN_RESTART.Name = "BTN_RESTART"
        Me.BTN_RESTART.Size = New System.Drawing.Size(68, 25)
        Me.BTN_RESTART.TabIndex = 18
        Me.BTN_RESTART.TabStop = False
        Me.BTN_RESTART.Text = "再開(&R)"
        Me.BTN_RESTART.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(5, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DGV_DATA)
        Me.SplitContainer1.Size = New System.Drawing.Size(682, 537)
        Me.SplitContainer1.SplitterDistance = 111
        Me.SplitContainer1.TabIndex = 15
        Me.SplitContainer1.TabStop = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TXT_SQL)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.GroupBox3)
        Me.SplitContainer2.Size = New System.Drawing.Size(682, 111)
        Me.SplitContainer2.SplitterDistance = 369
        Me.SplitContainer2.TabIndex = 0
        Me.SplitContainer2.TabStop = False
        '
        'TXT_SQL
        '
        Me.TXT_SQL.DetectUrls = False
        Me.TXT_SQL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TXT_SQL.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TXT_SQL.Location = New System.Drawing.Point(0, 0)
        Me.TXT_SQL.Name = "TXT_SQL"
        Me.TXT_SQL.Size = New System.Drawing.Size(369, 111)
        Me.TXT_SQL.TabIndex = 2
        Me.TXT_SQL.Text = ""
        Me.TXT_SQL.WordWrap = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CHK_REFRESH)
        Me.GroupBox3.Controls.Add(Me.TXT_WATCH_INTERVAL)
        Me.GroupBox3.Controls.Add(Me.RB_SELECT2)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.GroupBox2)
        Me.GroupBox3.Controls.Add(Me.RB_SELECT1)
        Me.GroupBox3.Controls.Add(Me.TXT_MAX_DISP_CNT)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.TXT_MIN_DISP_CNT)
        Me.GroupBox3.Controls.Add(Me.CMB_DISPLAY)
        Me.GroupBox3.Controls.Add(Me.CHK_COLSIZE_REGULATE)
        Me.GroupBox3.Controls.Add(Me.TXT_TIMEOUT_VALUE)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.LBL_TIMEOUT_VALUE)
        Me.GroupBox3.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(307, 102)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "オプション"
        '
        'RB_SELECT2
        '
        Me.RB_SELECT2.AutoSize = True
        Me.RB_SELECT2.Location = New System.Drawing.Point(122, 40)
        Me.RB_SELECT2.Name = "RB_SELECT2"
        Me.RB_SELECT2.Size = New System.Drawing.Size(95, 16)
        Me.RB_SELECT2.TabIndex = 18
        Me.RB_SELECT2.TabStop = True
        Me.RB_SELECT2.Text = "非接続型処理"
        Me.RB_SELECT2.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TXT_EXEC_CNT)
        Me.GroupBox2.Controls.Add(Me.TXT_TIMER_INTERVAL)
        Me.GroupBox2.Controls.Add(Me.CHK_TIMER)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 56)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(240, 40)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "　　　　"
        '
        'TXT_EXEC_CNT
        '
        Me.TXT_EXEC_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_EXEC_CNT.ErrorMessage = "実行回数は1以上を設定する必要がありますである必要があります"
        Me.TXT_EXEC_CNT.Location = New System.Drawing.Point(173, 16)
        Me.TXT_EXEC_CNT.MaxLength = 8
        Me.TXT_EXEC_CNT.Name = "TXT_EXEC_CNT"
        Me.TXT_EXEC_CNT.Size = New System.Drawing.Size(57, 19)
        Me.TXT_EXEC_CNT.TabIndex = 4
        Me.TXT_EXEC_CNT.TabStop = False
        Me.TXT_EXEC_CNT.Text = "1"
        Me.TXT_EXEC_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_EXEC_CNT.ValidationExpression = "^[1-9][0-9]*$"
        '
        'TXT_TIMER_INTERVAL
        '
        Me.TXT_TIMER_INTERVAL.ErrorColor = System.Drawing.Color.Red
        Me.TXT_TIMER_INTERVAL.ErrorMessage = "間隔は1以上を設定する必要があります"
        Me.TXT_TIMER_INTERVAL.Location = New System.Drawing.Point(51, 16)
        Me.TXT_TIMER_INTERVAL.MaxLength = 8
        Me.TXT_TIMER_INTERVAL.Name = "TXT_TIMER_INTERVAL"
        Me.TXT_TIMER_INTERVAL.Size = New System.Drawing.Size(57, 19)
        Me.TXT_TIMER_INTERVAL.TabIndex = 2
        Me.TXT_TIMER_INTERVAL.TabStop = False
        Me.TXT_TIMER_INTERVAL.Text = "1"
        Me.TXT_TIMER_INTERVAL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_TIMER_INTERVAL.ValidationExpression = "^[1-9][0-9]*$"
        '
        'CHK_TIMER
        '
        Me.CHK_TIMER.AutoSize = True
        Me.CHK_TIMER.Location = New System.Drawing.Point(1, -1)
        Me.CHK_TIMER.Name = "CHK_TIMER"
        Me.CHK_TIMER.Size = New System.Drawing.Size(89, 16)
        Me.CHK_TIMER.TabIndex = 0
        Me.CHK_TIMER.TabStop = False
        Me.CHK_TIMER.Text = "繰り返し実行"
        Me.CHK_TIMER.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(114, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 3
        Me.Label4.Tag = "label"
        Me.Label4.Text = "実行回数"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 12)
        Me.Label3.TabIndex = 1
        Me.Label3.Tag = "label"
        Me.Label3.Text = "間隔(S)"
        '
        'RB_SELECT1
        '
        Me.RB_SELECT1.AutoSize = True
        Me.RB_SELECT1.Checked = True
        Me.RB_SELECT1.Location = New System.Drawing.Point(122, 18)
        Me.RB_SELECT1.Name = "RB_SELECT1"
        Me.RB_SELECT1.Size = New System.Drawing.Size(83, 16)
        Me.RB_SELECT1.TabIndex = 17
        Me.RB_SELECT1.TabStop = True
        Me.RB_SELECT1.Text = "接続型処理"
        Me.RB_SELECT1.UseVisualStyleBackColor = True
        '
        'TXT_MAX_DISP_CNT
        '
        Me.TXT_MAX_DISP_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_MAX_DISP_CNT.ErrorMessage = "表示終了行は0以上を設定する必要があります"
        Me.TXT_MAX_DISP_CNT.Location = New System.Drawing.Point(123, 32)
        Me.TXT_MAX_DISP_CNT.MaxLength = 8
        Me.TXT_MAX_DISP_CNT.Name = "TXT_MAX_DISP_CNT"
        Me.TXT_MAX_DISP_CNT.Size = New System.Drawing.Size(57, 19)
        Me.TXT_MAX_DISP_CNT.TabIndex = 8
        Me.TXT_MAX_DISP_CNT.TabStop = False
        Me.TXT_MAX_DISP_CNT.Text = "99999999"
        Me.TXT_MAX_DISP_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_MAX_DISP_CNT.ValidationExpression = "^[0-9]*$"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 12)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "表示行"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(105, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(17, 12)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "～"
        '
        'TXT_MIN_DISP_CNT
        '
        Me.TXT_MIN_DISP_CNT.ErrorColor = System.Drawing.Color.Red
        Me.TXT_MIN_DISP_CNT.ErrorMessage = "表示開始行は0以上を設定する必要があります"
        Me.TXT_MIN_DISP_CNT.Location = New System.Drawing.Point(47, 32)
        Me.TXT_MIN_DISP_CNT.MaxLength = 8
        Me.TXT_MIN_DISP_CNT.Name = "TXT_MIN_DISP_CNT"
        Me.TXT_MIN_DISP_CNT.Size = New System.Drawing.Size(57, 19)
        Me.TXT_MIN_DISP_CNT.TabIndex = 6
        Me.TXT_MIN_DISP_CNT.TabStop = False
        Me.TXT_MIN_DISP_CNT.Text = "0"
        Me.TXT_MIN_DISP_CNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_MIN_DISP_CNT.ValidationExpression = "^[0-9]*$"
        '
        'CMB_DISPLAY
        '
        Me.CMB_DISPLAY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_DISPLAY.FormattingEnabled = True
        Me.CMB_DISPLAY.Items.AddRange(New Object() {"1 通常実行", "2 実行計画表示", "3 待機イベント監視"})
        Me.CMB_DISPLAY.Location = New System.Drawing.Point(61, 9)
        Me.CMB_DISPLAY.Name = "CMB_DISPLAY"
        Me.CMB_DISPLAY.Size = New System.Drawing.Size(120, 20)
        Me.CMB_DISPLAY.TabIndex = 4
        Me.CMB_DISPLAY.TabStop = False
        '
        'CHK_COLSIZE_REGULATE
        '
        Me.CHK_COLSIZE_REGULATE.AutoSize = True
        Me.CHK_COLSIZE_REGULATE.Location = New System.Drawing.Point(194, 14)
        Me.CHK_COLSIZE_REGULATE.Name = "CHK_COLSIZE_REGULATE"
        Me.CHK_COLSIZE_REGULATE.Size = New System.Drawing.Size(106, 16)
        Me.CHK_COLSIZE_REGULATE.TabIndex = 9
        Me.CHK_COLSIZE_REGULATE.TabStop = False
        Me.CHK_COLSIZE_REGULATE.Text = "列幅の自動調整"
        Me.CHK_COLSIZE_REGULATE.UseVisualStyleBackColor = True
        '
        'TXT_TIMEOUT_VALUE
        '
        Me.TXT_TIMEOUT_VALUE.Enabled = False
        Me.TXT_TIMEOUT_VALUE.ErrorColor = System.Drawing.Color.Red
        Me.TXT_TIMEOUT_VALUE.ErrorMessage = "タイムアウト値は0以上を設定する必要があります"
        Me.TXT_TIMEOUT_VALUE.Location = New System.Drawing.Point(255, 33)
        Me.TXT_TIMEOUT_VALUE.MaxLength = 5
        Me.TXT_TIMEOUT_VALUE.Name = "TXT_TIMEOUT_VALUE"
        Me.TXT_TIMEOUT_VALUE.Size = New System.Drawing.Size(40, 19)
        Me.TXT_TIMEOUT_VALUE.TabIndex = 11
        Me.TXT_TIMEOUT_VALUE.TabStop = False
        Me.TXT_TIMEOUT_VALUE.Text = "60"
        Me.TXT_TIMEOUT_VALUE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_TIMEOUT_VALUE.ValidationExpression = "^[0-9]*$"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 12)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "実行形式"
        '
        'LBL_TIMEOUT_VALUE
        '
        Me.LBL_TIMEOUT_VALUE.AutoSize = True
        Me.LBL_TIMEOUT_VALUE.Location = New System.Drawing.Point(183, 37)
        Me.LBL_TIMEOUT_VALUE.Name = "LBL_TIMEOUT_VALUE"
        Me.LBL_TIMEOUT_VALUE.Size = New System.Drawing.Size(73, 12)
        Me.LBL_TIMEOUT_VALUE.TabIndex = 10
        Me.LBL_TIMEOUT_VALUE.Text = "タイムアウト(S)"
        '
        'DGV_DATA
        '
        Me.DGV_DATA.AllowUserToAddRows = False
        Me.DGV_DATA.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_DATA.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGV_DATA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGV_DATA.DefaultCellStyle = DataGridViewCellStyle2
        Me.DGV_DATA.Location = New System.Drawing.Point(0, 3)
        Me.DGV_DATA.Name = "DGV_DATA"
        Me.DGV_DATA.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_DATA.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DGV_DATA.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DGV_DATA.RowTemplate.Height = 21
        Me.DGV_DATA.Size = New System.Drawing.Size(679, 309)
        Me.DGV_DATA.StandardTab = True
        Me.DGV_DATA.TabIndex = 15
        '
        'TXT_WATCH_INTERVAL
        '
        Me.TXT_WATCH_INTERVAL.ErrorColor = System.Drawing.Color.Red
        Me.TXT_WATCH_INTERVAL.ErrorMessage = "間隔は1以上を設定する必要があります"
        Me.TXT_WATCH_INTERVAL.Location = New System.Drawing.Point(89, 32)
        Me.TXT_WATCH_INTERVAL.MaxLength = 8
        Me.TXT_WATCH_INTERVAL.Name = "TXT_WATCH_INTERVAL"
        Me.TXT_WATCH_INTERVAL.Size = New System.Drawing.Size(57, 19)
        Me.TXT_WATCH_INTERVAL.TabIndex = 17
        Me.TXT_WATCH_INTERVAL.TabStop = False
        Me.TXT_WATCH_INTERVAL.Text = "100"
        Me.TXT_WATCH_INTERVAL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TXT_WATCH_INTERVAL.ValidationExpression = "^[0-9]*$"
        Me.TXT_WATCH_INTERVAL.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 12)
        Me.Label6.TabIndex = 16
        Me.Label6.Tag = "label"
        Me.Label6.Text = "監視間隔(ms)"
        Me.Label6.Visible = False
        '
        'CHK_REFRESH
        '
        Me.CHK_REFRESH.AutoSize = True
        Me.CHK_REFRESH.Location = New System.Drawing.Point(7, 55)
        Me.CHK_REFRESH.Name = "CHK_REFRESH"
        Me.CHK_REFRESH.Size = New System.Drawing.Size(109, 16)
        Me.CHK_REFRESH.TabIndex = 16
        Me.CHK_REFRESH.TabStop = False
        Me.CHK_REFRESH.Text = "監視毎リフレッシュ"
        Me.CHK_REFRESH.UseVisualStyleBackColor = True
        '
        'frm_Execsql
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 562)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LBL_INFO)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Execsql"
        Me.Text = "SQL実行"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DGV_DATA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BTN_EXECUTE As System.Windows.Forms.Button
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents BTN_CSV As System.Windows.Forms.Button
    Friend WithEvents BTN_COPY As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_BIND As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents BTN_RESTART As System.Windows.Forms.Button
    Friend WithEvents BTN_HIST_DOWN As System.Windows.Forms.Button
    Friend WithEvents BTN_HIST_UP As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents RB_SELECT2 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TXT_EXEC_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents TXT_TIMER_INTERVAL As CLS_COMMON.RegExTextBox
    Friend WithEvents CHK_TIMER As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents RB_SELECT1 As System.Windows.Forms.RadioButton
    Friend WithEvents TXT_MAX_DISP_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_MIN_DISP_CNT As CLS_COMMON.RegExTextBox
    Friend WithEvents CMB_DISPLAY As System.Windows.Forms.ComboBox
    Friend WithEvents CHK_COLSIZE_REGULATE As System.Windows.Forms.CheckBox
    Friend WithEvents TXT_TIMEOUT_VALUE As CLS_COMMON.RegExTextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LBL_TIMEOUT_VALUE As System.Windows.Forms.Label
    Friend WithEvents DGV_DATA As System.Windows.Forms.DataGridView
    Friend WithEvents BTN_ALL_COPY As System.Windows.Forms.Button
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents TXT_SQL As System.Windows.Forms.RichTextBox
    Friend WithEvents TXT_WATCH_INTERVAL As CLS_COMMON.RegExTextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CHK_REFRESH As System.Windows.Forms.CheckBox
End Class
