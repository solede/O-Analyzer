<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_INTERNAL
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_INTERNAL))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CMB_SORT_ORDER3 = New System.Windows.Forms.ComboBox()
        Me.CMB_SORT_VAL3 = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CMB_SORT_ORDER2 = New System.Windows.Forms.ComboBox()
        Me.CMB_SORT_VAL2 = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CMB_SORT_ORDER1 = New System.Windows.Forms.ComboBox()
        Me.CMB_SORT_VAL1 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DGV_SHARED_POOL = New System.Windows.Forms.DataGridView()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.CHK_PERMANENT = New System.Windows.Forms.CheckBox()
        Me.CHK_R_FREEABLE = New System.Windows.Forms.CheckBox()
        Me.CHK_R_FREE = New System.Windows.Forms.CheckBox()
        Me.CHK_RECREATABLE = New System.Windows.Forms.CheckBox()
        Me.CHK_FREEABLE = New System.Windows.Forms.CheckBox()
        Me.CHK_FREE = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CMB_GROUPBY = New System.Windows.Forms.ComboBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TXT_USER_DEFINE = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DGV_HIDDEN_PARAM = New System.Windows.Forms.DataGridView()
        Me.LBL_INFO = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BTN_SHOW_LOG = New System.Windows.Forms.Button()
        Me.BTN_CANCEL = New System.Windows.Forms.Button()
        Me.BTN_EXEC = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DGV_SHARED_POOL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.DGV_HIDDEN_PARAM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(689, 530)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.DGV_SHARED_POOL)
        Me.TabPage1.Controls.Add(Me.GroupBox8)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(681, 504)
        Me.TabPage1.TabIndex = 8
        Me.TabPage1.Text = "共有プール断片化"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_ORDER3)
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_VAL3)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_ORDER2)
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_VAL2)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_ORDER1)
        Me.GroupBox2.Controls.Add(Me.CMB_SORT_VAL1)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 67)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(670, 56)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "整列条件"
        '
        'CMB_SORT_ORDER3
        '
        Me.CMB_SORT_ORDER3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_ORDER3.FormattingEnabled = True
        Me.CMB_SORT_ORDER3.Items.AddRange(New Object() {"ASC", "DESC"})
        Me.CMB_SORT_ORDER3.Location = New System.Drawing.Point(587, 18)
        Me.CMB_SORT_ORDER3.Name = "CMB_SORT_ORDER3"
        Me.CMB_SORT_ORDER3.Size = New System.Drawing.Size(49, 20)
        Me.CMB_SORT_ORDER3.TabIndex = 9
        '
        'CMB_SORT_VAL3
        '
        Me.CMB_SORT_VAL3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_VAL3.FormattingEnabled = True
        Me.CMB_SORT_VAL3.Items.AddRange(New Object() {"1 ヒープ", "2 属性", "3 コンポーネント"})
        Me.CMB_SORT_VAL3.Location = New System.Drawing.Point(491, 18)
        Me.CMB_SORT_VAL3.Name = "CMB_SORT_VAL3"
        Me.CMB_SORT_VAL3.Size = New System.Drawing.Size(90, 20)
        Me.CMB_SORT_VAL3.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(450, 21)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 12)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "順序3"
        '
        'CMB_SORT_ORDER2
        '
        Me.CMB_SORT_ORDER2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_ORDER2.FormattingEnabled = True
        Me.CMB_SORT_ORDER2.Items.AddRange(New Object() {"ASC", "DESC"})
        Me.CMB_SORT_ORDER2.Location = New System.Drawing.Point(358, 18)
        Me.CMB_SORT_ORDER2.Name = "CMB_SORT_ORDER2"
        Me.CMB_SORT_ORDER2.Size = New System.Drawing.Size(49, 20)
        Me.CMB_SORT_ORDER2.TabIndex = 6
        '
        'CMB_SORT_VAL2
        '
        Me.CMB_SORT_VAL2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_VAL2.FormattingEnabled = True
        Me.CMB_SORT_VAL2.Items.AddRange(New Object() {"0 なし", "1 ヒープ", "2 属性", "3 コンポーネント"})
        Me.CMB_SORT_VAL2.Location = New System.Drawing.Point(262, 18)
        Me.CMB_SORT_VAL2.Name = "CMB_SORT_VAL2"
        Me.CMB_SORT_VAL2.Size = New System.Drawing.Size(90, 20)
        Me.CMB_SORT_VAL2.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(221, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 12)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "順序2"
        '
        'CMB_SORT_ORDER1
        '
        Me.CMB_SORT_ORDER1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_ORDER1.FormattingEnabled = True
        Me.CMB_SORT_ORDER1.Items.AddRange(New Object() {"ASC", "DESC"})
        Me.CMB_SORT_ORDER1.Location = New System.Drawing.Point(146, 18)
        Me.CMB_SORT_ORDER1.Name = "CMB_SORT_ORDER1"
        Me.CMB_SORT_ORDER1.Size = New System.Drawing.Size(49, 20)
        Me.CMB_SORT_ORDER1.TabIndex = 3
        '
        'CMB_SORT_VAL1
        '
        Me.CMB_SORT_VAL1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_SORT_VAL1.FormattingEnabled = True
        Me.CMB_SORT_VAL1.Items.AddRange(New Object() {"1 ヒープ", "2 属性", "3 コンポーネント"})
        Me.CMB_SORT_VAL1.Location = New System.Drawing.Point(50, 18)
        Me.CMB_SORT_VAL1.Name = "CMB_SORT_VAL1"
        Me.CMB_SORT_VAL1.Size = New System.Drawing.Size(90, 20)
        Me.CMB_SORT_VAL1.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 12)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "順序1"
        '
        'DGV_SHARED_POOL
        '
        Me.DGV_SHARED_POOL.AllowUserToDeleteRows = False
        Me.DGV_SHARED_POOL.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_SHARED_POOL.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGV_SHARED_POOL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGV_SHARED_POOL.DefaultCellStyle = DataGridViewCellStyle2
        Me.DGV_SHARED_POOL.Location = New System.Drawing.Point(5, 137)
        Me.DGV_SHARED_POOL.Name = "DGV_SHARED_POOL"
        Me.DGV_SHARED_POOL.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_SHARED_POOL.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DGV_SHARED_POOL.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DGV_SHARED_POOL.RowTemplate.Height = 21
        Me.DGV_SHARED_POOL.Size = New System.Drawing.Size(661, 364)
        Me.DGV_SHARED_POOL.TabIndex = 3
        Me.DGV_SHARED_POOL.VirtualMode = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.CHK_PERMANENT)
        Me.GroupBox8.Controls.Add(Me.CHK_R_FREEABLE)
        Me.GroupBox8.Controls.Add(Me.CHK_R_FREE)
        Me.GroupBox8.Controls.Add(Me.CHK_RECREATABLE)
        Me.GroupBox8.Controls.Add(Me.CHK_FREEABLE)
        Me.GroupBox8.Controls.Add(Me.CHK_FREE)
        Me.GroupBox8.Controls.Add(Me.Label2)
        Me.GroupBox8.Controls.Add(Me.Label1)
        Me.GroupBox8.Controls.Add(Me.CMB_GROUPBY)
        Me.GroupBox8.Location = New System.Drawing.Point(6, 5)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(670, 56)
        Me.GroupBox8.TabIndex = 1
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "集約・抽出条件"
        '
        'CHK_PERMANENT
        '
        Me.CHK_PERMANENT.AutoSize = True
        Me.CHK_PERMANENT.Location = New System.Drawing.Point(501, 36)
        Me.CHK_PERMANENT.Name = "CHK_PERMANENT"
        Me.CHK_PERMANENT.Size = New System.Drawing.Size(78, 16)
        Me.CHK_PERMANENT.TabIndex = 9
        Me.CHK_PERMANENT.Text = "Permanent"
        Me.CHK_PERMANENT.UseVisualStyleBackColor = True
        '
        'CHK_R_FREEABLE
        '
        Me.CHK_R_FREEABLE.AutoSize = True
        Me.CHK_R_FREEABLE.Location = New System.Drawing.Point(501, 22)
        Me.CHK_R_FREEABLE.Name = "CHK_R_FREEABLE"
        Me.CHK_R_FREEABLE.Size = New System.Drawing.Size(82, 16)
        Me.CHK_R_FREEABLE.TabIndex = 8
        Me.CHK_R_FREEABLE.Text = "R-Freeable"
        Me.CHK_R_FREEABLE.UseVisualStyleBackColor = True
        '
        'CHK_R_FREE
        '
        Me.CHK_R_FREE.AutoSize = True
        Me.CHK_R_FREE.Location = New System.Drawing.Point(501, 8)
        Me.CHK_R_FREE.Name = "CHK_R_FREE"
        Me.CHK_R_FREE.Size = New System.Drawing.Size(61, 16)
        Me.CHK_R_FREE.TabIndex = 7
        Me.CHK_R_FREE.Text = "R-Free"
        Me.CHK_R_FREE.UseVisualStyleBackColor = True
        '
        'CHK_RECREATABLE
        '
        Me.CHK_RECREATABLE.AutoSize = True
        Me.CHK_RECREATABLE.Location = New System.Drawing.Point(413, 36)
        Me.CHK_RECREATABLE.Name = "CHK_RECREATABLE"
        Me.CHK_RECREATABLE.Size = New System.Drawing.Size(85, 16)
        Me.CHK_RECREATABLE.TabIndex = 6
        Me.CHK_RECREATABLE.Text = "Recreatable"
        Me.CHK_RECREATABLE.UseVisualStyleBackColor = True
        '
        'CHK_FREEABLE
        '
        Me.CHK_FREEABLE.AutoSize = True
        Me.CHK_FREEABLE.Location = New System.Drawing.Point(413, 22)
        Me.CHK_FREEABLE.Name = "CHK_FREEABLE"
        Me.CHK_FREEABLE.Size = New System.Drawing.Size(68, 16)
        Me.CHK_FREEABLE.TabIndex = 5
        Me.CHK_FREEABLE.Text = "Freeable"
        Me.CHK_FREEABLE.UseVisualStyleBackColor = True
        '
        'CHK_FREE
        '
        Me.CHK_FREE.AutoSize = True
        Me.CHK_FREE.Location = New System.Drawing.Point(413, 8)
        Me.CHK_FREE.Name = "CHK_FREE"
        Me.CHK_FREE.Size = New System.Drawing.Size(47, 16)
        Me.CHK_FREE.TabIndex = 4
        Me.CHK_FREE.Text = "Free"
        Me.CHK_FREE.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(354, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "抽出対象"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "集約条件"
        '
        'CMB_GROUPBY
        '
        Me.CMB_GROUPBY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_GROUPBY.FormattingEnabled = True
        Me.CMB_GROUPBY.Items.AddRange(New Object() {"0 集約なし", "1 ヒープ", "2 ヒープ、属性", "3 ヒープ、属性、コンポーネント"})
        Me.CMB_GROUPBY.Location = New System.Drawing.Point(68, 18)
        Me.CMB_GROUPBY.Name = "CMB_GROUPBY"
        Me.CMB_GROUPBY.Size = New System.Drawing.Size(179, 20)
        Me.CMB_GROUPBY.TabIndex = 2
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Controls.Add(Me.DGV_HIDDEN_PARAM)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(681, 504)
        Me.TabPage2.TabIndex = 9
        Me.TabPage2.Text = "隠しパラメータ"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TXT_USER_DEFINE)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(674, 47)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "集約・抽出条件"
        '
        'TXT_USER_DEFINE
        '
        Me.TXT_USER_DEFINE.Location = New System.Drawing.Point(71, 16)
        Me.TXT_USER_DEFINE.Name = "TXT_USER_DEFINE"
        Me.TXT_USER_DEFINE.Size = New System.Drawing.Size(597, 19)
        Me.TXT_USER_DEFINE.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 12)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "ユーザ定義"
        '
        'DGV_HIDDEN_PARAM
        '
        Me.DGV_HIDDEN_PARAM.AllowUserToDeleteRows = False
        Me.DGV_HIDDEN_PARAM.AllowUserToResizeRows = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_HIDDEN_PARAM.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DGV_HIDDEN_PARAM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGV_HIDDEN_PARAM.DefaultCellStyle = DataGridViewCellStyle5
        Me.DGV_HIDDEN_PARAM.Location = New System.Drawing.Point(5, 57)
        Me.DGV_HIDDEN_PARAM.Name = "DGV_HIDDEN_PARAM"
        Me.DGV_HIDDEN_PARAM.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGV_HIDDEN_PARAM.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.DGV_HIDDEN_PARAM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DGV_HIDDEN_PARAM.RowTemplate.Height = 21
        Me.DGV_HIDDEN_PARAM.Size = New System.Drawing.Size(675, 443)
        Me.DGV_HIDDEN_PARAM.TabIndex = 2
        Me.DGV_HIDDEN_PARAM.VirtualMode = True
        '
        'LBL_INFO
        '
        Me.LBL_INFO.AutoSize = True
        Me.LBL_INFO.Location = New System.Drawing.Point(8, 552)
        Me.LBL_INFO.Name = "LBL_INFO"
        Me.LBL_INFO.Size = New System.Drawing.Size(55, 12)
        Me.LBL_INFO.TabIndex = 3
        Me.LBL_INFO.Text = "LBL_INFO"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_SHOW_LOG)
        Me.GroupBox1.Controls.Add(Me.BTN_CANCEL)
        Me.GroupBox1.Controls.Add(Me.BTN_EXEC)
        Me.GroupBox1.Location = New System.Drawing.Point(701, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(80, 537)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "コマンド"
        '
        'BTN_SHOW_LOG
        '
        Me.BTN_SHOW_LOG.Location = New System.Drawing.Point(6, 79)
        Me.BTN_SHOW_LOG.Name = "BTN_SHOW_LOG"
        Me.BTN_SHOW_LOG.Size = New System.Drawing.Size(68, 25)
        Me.BTN_SHOW_LOG.TabIndex = 3
        Me.BTN_SHOW_LOG.TabStop = False
        Me.BTN_SHOW_LOG.Text = "ログ参照"
        Me.BTN_SHOW_LOG.UseVisualStyleBackColor = True
        '
        'BTN_CANCEL
        '
        Me.BTN_CANCEL.Location = New System.Drawing.Point(6, 48)
        Me.BTN_CANCEL.Name = "BTN_CANCEL"
        Me.BTN_CANCEL.Size = New System.Drawing.Size(68, 25)
        Me.BTN_CANCEL.TabIndex = 2
        Me.BTN_CANCEL.TabStop = False
        Me.BTN_CANCEL.Text = "キャンセル"
        Me.BTN_CANCEL.UseVisualStyleBackColor = True
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
        'FRM_INTERNAL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.LBL_INFO)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "FRM_INTERNAL"
        Me.Text = "非公開機能"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DGV_SHARED_POOL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.DGV_HIDDEN_PARAM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents DGV_SHARED_POOL As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents LBL_INFO As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents BTN_SHOW_LOG As System.Windows.Forms.Button
    Friend WithEvents BTN_CANCEL As System.Windows.Forms.Button
    Friend WithEvents BTN_EXEC As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CMB_GROUPBY As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents CMB_SORT_ORDER1 As System.Windows.Forms.ComboBox
    Friend WithEvents CMB_SORT_VAL1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CMB_SORT_ORDER3 As System.Windows.Forms.ComboBox
    Friend WithEvents CMB_SORT_VAL3 As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CMB_SORT_ORDER2 As System.Windows.Forms.ComboBox
    Friend WithEvents CMB_SORT_VAL2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CHK_PERMANENT As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_R_FREEABLE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_R_FREE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_RECREATABLE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_FREEABLE As System.Windows.Forms.CheckBox
    Friend WithEvents CHK_FREE As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents DGV_HIDDEN_PARAM As System.Windows.Forms.DataGridView
    Friend WithEvents TXT_USER_DEFINE As System.Windows.Forms.TextBox
End Class
