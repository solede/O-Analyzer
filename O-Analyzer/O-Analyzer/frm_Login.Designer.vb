<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Login
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Login))
        Me.BTN_EXPLORER = New System.Windows.Forms.Button()
        Me.TXT_PASS = New System.Windows.Forms.TextBox()
        Me.LBL_PASS = New System.Windows.Forms.Label()
        Me.TXT_USERID = New System.Windows.Forms.TextBox()
        Me.LBL_USER = New System.Windows.Forms.Label()
        Me.LBL_SID = New System.Windows.Forms.Label()
        Me.TXT_CONN_STR = New System.Windows.Forms.TextBox()
        Me.BTN_EXECSQL = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.BTN_CONNFIG = New System.Windows.Forms.Button()
        Me.CHK_SYSDBA = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CMB_MIDDLE = New System.Windows.Forms.ComboBox()
        Me.CHK_SAVE_PASS = New System.Windows.Forms.CheckBox()
        Me.BTN_LOG = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TXT_OPTION = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BTN_REGIST = New System.Windows.Forms.Button()
        Me.LB_CONN_LIST = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TXT_CONN_LIST = New System.Windows.Forms.TextBox()
        Me.BTN_REMOVE = New System.Windows.Forms.Button()
        Me.BTN_UP = New System.Windows.Forms.Button()
        Me.BTN_DOWN = New System.Windows.Forms.Button()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BTN_EXPLORER
        '
        Me.BTN_EXPLORER.Location = New System.Drawing.Point(12, 231)
        Me.BTN_EXPLORER.Name = "BTN_EXPLORER"
        Me.BTN_EXPLORER.Size = New System.Drawing.Size(100, 25)
        Me.BTN_EXPLORER.TabIndex = 18
        Me.BTN_EXPLORER.TabStop = False
        Me.BTN_EXPLORER.Text = "エクスプローラ(&E)"
        Me.BTN_EXPLORER.UseVisualStyleBackColor = True
        '
        'TXT_PASS
        '
        Me.TXT_PASS.AcceptsReturn = True
        Me.TXT_PASS.Location = New System.Drawing.Point(88, 112)
        Me.TXT_PASS.Name = "TXT_PASS"
        Me.TXT_PASS.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TXT_PASS.Size = New System.Drawing.Size(150, 19)
        Me.TXT_PASS.TabIndex = 9
        Me.TXT_PASS.Text = "tiger"
        '
        'LBL_PASS
        '
        Me.LBL_PASS.AutoSize = True
        Me.LBL_PASS.Location = New System.Drawing.Point(12, 115)
        Me.LBL_PASS.Name = "LBL_PASS"
        Me.LBL_PASS.Size = New System.Drawing.Size(52, 12)
        Me.LBL_PASS.TabIndex = 8
        Me.LBL_PASS.Text = "パスワード"
        Me.LBL_PASS.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TXT_USERID
        '
        Me.TXT_USERID.AcceptsReturn = True
        Me.TXT_USERID.Location = New System.Drawing.Point(88, 89)
        Me.TXT_USERID.Name = "TXT_USERID"
        Me.TXT_USERID.Size = New System.Drawing.Size(150, 19)
        Me.TXT_USERID.TabIndex = 7
        Me.TXT_USERID.Text = "scott"
        '
        'LBL_USER
        '
        Me.LBL_USER.AutoSize = True
        Me.LBL_USER.Location = New System.Drawing.Point(12, 92)
        Me.LBL_USER.Name = "LBL_USER"
        Me.LBL_USER.Size = New System.Drawing.Size(47, 12)
        Me.LBL_USER.TabIndex = 6
        Me.LBL_USER.Text = "ユーザ名"
        Me.LBL_USER.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LBL_SID
        '
        Me.LBL_SID.AutoSize = True
        Me.LBL_SID.Location = New System.Drawing.Point(12, 138)
        Me.LBL_SID.Name = "LBL_SID"
        Me.LBL_SID.Size = New System.Drawing.Size(65, 12)
        Me.LBL_SID.TabIndex = 10
        Me.LBL_SID.Text = "接続識別子"
        Me.LBL_SID.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TXT_CONN_STR
        '
        Me.TXT_CONN_STR.AcceptsReturn = True
        Me.TXT_CONN_STR.Location = New System.Drawing.Point(88, 135)
        Me.TXT_CONN_STR.Name = "TXT_CONN_STR"
        Me.TXT_CONN_STR.Size = New System.Drawing.Size(150, 19)
        Me.TXT_CONN_STR.TabIndex = 11
        Me.TXT_CONN_STR.Text = "ORCL"
        '
        'BTN_EXECSQL
        '
        Me.BTN_EXECSQL.Location = New System.Drawing.Point(128, 231)
        Me.BTN_EXECSQL.Name = "BTN_EXECSQL"
        Me.BTN_EXECSQL.Size = New System.Drawing.Size(100, 25)
        Me.BTN_EXECSQL.TabIndex = 19
        Me.BTN_EXECSQL.TabStop = False
        Me.BTN_EXECSQL.Text = "SQL実行(&S)"
        Me.BTN_EXECSQL.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 310)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(244, 22)
        Me.StatusStrip1.TabIndex = 22
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'BTN_CONNFIG
        '
        Me.BTN_CONNFIG.Location = New System.Drawing.Point(12, 257)
        Me.BTN_CONNFIG.Name = "BTN_CONNFIG"
        Me.BTN_CONNFIG.Size = New System.Drawing.Size(100, 25)
        Me.BTN_CONNFIG.TabIndex = 20
        Me.BTN_CONNFIG.TabStop = False
        Me.BTN_CONNFIG.Text = "コンフィグ(&C)"
        Me.BTN_CONNFIG.UseVisualStyleBackColor = True
        '
        'CHK_SYSDBA
        '
        Me.CHK_SYSDBA.AutoSize = True
        Me.CHK_SYSDBA.Location = New System.Drawing.Point(14, 209)
        Me.CHK_SYSDBA.Name = "CHK_SYSDBA"
        Me.CHK_SYSDBA.Size = New System.Drawing.Size(93, 16)
        Me.CHK_SYSDBA.TabIndex = 16
        Me.CHK_SYSDBA.Text = "SYSDBA接続"
        Me.CHK_SYSDBA.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 162)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 12)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "ミドルウエア"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CMB_MIDDLE
        '
        Me.CMB_MIDDLE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CMB_MIDDLE.FormattingEnabled = True
        Me.CMB_MIDDLE.Location = New System.Drawing.Point(88, 159)
        Me.CMB_MIDDLE.Name = "CMB_MIDDLE"
        Me.CMB_MIDDLE.Size = New System.Drawing.Size(150, 20)
        Me.CMB_MIDDLE.TabIndex = 13
        '
        'CHK_SAVE_PASS
        '
        Me.CHK_SAVE_PASS.AutoSize = True
        Me.CHK_SAVE_PASS.Location = New System.Drawing.Point(128, 209)
        Me.CHK_SAVE_PASS.Name = "CHK_SAVE_PASS"
        Me.CHK_SAVE_PASS.Size = New System.Drawing.Size(104, 16)
        Me.CHK_SAVE_PASS.TabIndex = 17
        Me.CHK_SAVE_PASS.Text = "パスワードを保存"
        Me.CHK_SAVE_PASS.UseVisualStyleBackColor = True
        '
        'BTN_LOG
        '
        Me.BTN_LOG.Location = New System.Drawing.Point(128, 257)
        Me.BTN_LOG.Name = "BTN_LOG"
        Me.BTN_LOG.Size = New System.Drawing.Size(100, 25)
        Me.BTN_LOG.TabIndex = 21
        Me.BTN_LOG.TabStop = False
        Me.BTN_LOG.Text = "ログ(&L)"
        Me.BTN_LOG.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 187)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 12)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "オプション"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TXT_OPTION
        '
        Me.TXT_OPTION.AcceptsReturn = True
        Me.TXT_OPTION.Location = New System.Drawing.Point(88, 184)
        Me.TXT_OPTION.Name = "TXT_OPTION"
        Me.TXT_OPTION.Size = New System.Drawing.Size(150, 19)
        Me.TXT_OPTION.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "接続先"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'BTN_REGIST
        '
        Me.BTN_REGIST.Location = New System.Drawing.Point(200, 11)
        Me.BTN_REGIST.Name = "BTN_REGIST"
        Me.BTN_REGIST.Size = New System.Drawing.Size(38, 20)
        Me.BTN_REGIST.TabIndex = 2
        Me.BTN_REGIST.TabStop = False
        Me.BTN_REGIST.Text = "登録"
        Me.BTN_REGIST.UseVisualStyleBackColor = True
        '
        'LB_CONN_LIST
        '
        Me.LB_CONN_LIST.FormattingEnabled = True
        Me.LB_CONN_LIST.ItemHeight = 12
        Me.LB_CONN_LIST.Items.AddRange(New Object() {"aaa", "bbb", "ccc", "ddd"})
        Me.LB_CONN_LIST.Location = New System.Drawing.Point(88, 9)
        Me.LB_CONN_LIST.Name = "LB_CONN_LIST"
        Me.LB_CONN_LIST.Size = New System.Drawing.Size(109, 52)
        Me.LB_CONN_LIST.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "接続先名"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TXT_CONN_LIST
        '
        Me.TXT_CONN_LIST.AcceptsReturn = True
        Me.TXT_CONN_LIST.Location = New System.Drawing.Point(88, 66)
        Me.TXT_CONN_LIST.Name = "TXT_CONN_LIST"
        Me.TXT_CONN_LIST.Size = New System.Drawing.Size(150, 19)
        Me.TXT_CONN_LIST.TabIndex = 5
        Me.TXT_CONN_LIST.Text = "接続先１"
        '
        'BTN_REMOVE
        '
        Me.BTN_REMOVE.Location = New System.Drawing.Point(200, 37)
        Me.BTN_REMOVE.Name = "BTN_REMOVE"
        Me.BTN_REMOVE.Size = New System.Drawing.Size(38, 20)
        Me.BTN_REMOVE.TabIndex = 3
        Me.BTN_REMOVE.TabStop = False
        Me.BTN_REMOVE.Text = "削除"
        Me.BTN_REMOVE.UseVisualStyleBackColor = True
        '
        'BTN_UP
        '
        Me.BTN_UP.Location = New System.Drawing.Point(63, 11)
        Me.BTN_UP.Name = "BTN_UP"
        Me.BTN_UP.Size = New System.Drawing.Size(19, 20)
        Me.BTN_UP.TabIndex = 23
        Me.BTN_UP.TabStop = False
        Me.BTN_UP.Text = "↑"
        Me.BTN_UP.UseVisualStyleBackColor = True
        '
        'BTN_DOWN
        '
        Me.BTN_DOWN.Location = New System.Drawing.Point(63, 37)
        Me.BTN_DOWN.Name = "BTN_DOWN"
        Me.BTN_DOWN.Size = New System.Drawing.Size(19, 20)
        Me.BTN_DOWN.TabIndex = 24
        Me.BTN_DOWN.TabStop = False
        Me.BTN_DOWN.Text = "↓"
        Me.BTN_DOWN.UseVisualStyleBackColor = True
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(12, 283)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(100, 25)
        Me.BTN_HELP.TabIndex = 25
        Me.BTN_HELP.TabStop = False
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(128, 283)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 25)
        Me.Button1.TabIndex = 26
        Me.Button1.TabStop = False
        Me.Button1.Text = "案件の依頼"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frm_Login
        '
        Me.AcceptButton = Me.BTN_EXPLORER
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(244, 332)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BTN_HELP)
        Me.Controls.Add(Me.BTN_DOWN)
        Me.Controls.Add(Me.BTN_UP)
        Me.Controls.Add(Me.BTN_REMOVE)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TXT_CONN_LIST)
        Me.Controls.Add(Me.LB_CONN_LIST)
        Me.Controls.Add(Me.BTN_REGIST)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TXT_OPTION)
        Me.Controls.Add(Me.BTN_LOG)
        Me.Controls.Add(Me.CHK_SAVE_PASS)
        Me.Controls.Add(Me.CMB_MIDDLE)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.CHK_SYSDBA)
        Me.Controls.Add(Me.BTN_CONNFIG)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.BTN_EXECSQL)
        Me.Controls.Add(Me.LBL_SID)
        Me.Controls.Add(Me.TXT_CONN_STR)
        Me.Controls.Add(Me.LBL_USER)
        Me.Controls.Add(Me.TXT_USERID)
        Me.Controls.Add(Me.LBL_PASS)
        Me.Controls.Add(Me.TXT_PASS)
        Me.Controls.Add(Me.BTN_EXPLORER)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(260, 370)
        Me.MinimumSize = New System.Drawing.Size(260, 370)
        Me.Name = "frm_Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "O-Analyzer"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BTN_EXPLORER As System.Windows.Forms.Button
    Friend WithEvents TXT_PASS As System.Windows.Forms.TextBox
    Friend WithEvents LBL_PASS As System.Windows.Forms.Label
    Friend WithEvents TXT_USERID As System.Windows.Forms.TextBox
    Friend WithEvents LBL_USER As System.Windows.Forms.Label
    Friend WithEvents LBL_SID As System.Windows.Forms.Label
    Friend WithEvents TXT_CONN_STR As System.Windows.Forms.TextBox
    Friend WithEvents BTN_EXECSQL As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents BTN_CONNFIG As System.Windows.Forms.Button
    Friend WithEvents CHK_SYSDBA As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CMB_MIDDLE As System.Windows.Forms.ComboBox
    Friend WithEvents CHK_SAVE_PASS As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_LOG As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TXT_OPTION As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BTN_REGIST As System.Windows.Forms.Button
    Friend WithEvents LB_CONN_LIST As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TXT_CONN_LIST As System.Windows.Forms.TextBox
    Friend WithEvents BTN_REMOVE As System.Windows.Forms.Button
    Friend WithEvents BTN_UP As System.Windows.Forms.Button
    Friend WithEvents BTN_DOWN As System.Windows.Forms.Button
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
