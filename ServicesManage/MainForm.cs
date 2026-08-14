using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServicesManage
{

        public partial class MainForm : Form
        {
            private DataGridView dgvServices;
            private Button btnRefresh, btnStart, btnStop, btnUninstall, btnBrowse, btnInstall;
            private TextBox txtServiceName, txtFilePath;
            private Label lblServiceName, lblFilePath;

            public MainForm()
            {
                InitializeComponent();
            Init();
                LoadServices();
            }

            private void Init()
            {
                this.dgvServices = new DataGridView();
                this.btnRefresh = new Button();
                this.btnStart = new Button();
                this.btnStop = new Button();
                this.btnUninstall = new Button();
                this.btnBrowse = new Button();
                this.btnInstall = new Button();
                this.txtServiceName = new TextBox();
                this.txtFilePath = new TextBox();
                this.lblServiceName = new Label();
                this.lblFilePath = new Label();

                // dgvServices
                this.dgvServices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                this.dgvServices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this.dgvServices.Location = new System.Drawing.Point(12, 12);
                this.dgvServices.Name = "dgvServices";
                this.dgvServices.ReadOnly = true;
                this.dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.dgvServices.Size = new System.Drawing.Size(760, 300);
                this.dgvServices.TabIndex = 0;

                // btnRefresh
                this.btnRefresh.Location = new System.Drawing.Point(12, 330);
                this.btnRefresh.Name = "btnRefresh";
                this.btnRefresh.Size = new System.Drawing.Size(75, 30);
                this.btnRefresh.TabIndex = 1;
                this.btnRefresh.Text = "刷新";
                this.btnRefresh.UseVisualStyleBackColor = true;
                this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

                // btnStart
                this.btnStart.Location = new System.Drawing.Point(100, 330);
                this.btnStart.Name = "btnStart";
                this.btnStart.Size = new System.Drawing.Size(75, 30);
                this.btnStart.TabIndex = 2;
                this.btnStart.Text = "启动";
                this.btnStart.UseVisualStyleBackColor = true;
                this.btnStart.Click += new EventHandler(this.btnStart_Click);

                // btnStop
                this.btnStop.Location = new System.Drawing.Point(190, 330);
                this.btnStop.Name = "btnStop";
                this.btnStop.Size = new System.Drawing.Size(75, 30);
                this.btnStop.TabIndex = 3;
                this.btnStop.Text = "停止";
                this.btnStop.UseVisualStyleBackColor = true;
                this.btnStop.Click += new EventHandler(this.btnStop_Click);

                // btnUninstall
                this.btnUninstall.Location = new System.Drawing.Point(280, 330);
                this.btnUninstall.Name = "btnUninstall";
                this.btnUninstall.Size = new System.Drawing.Size(75, 30);
                this.btnUninstall.TabIndex = 4;
                this.btnUninstall.Text = "卸载";
                this.btnUninstall.UseVisualStyleBackColor = true;
                this.btnUninstall.Click += new EventHandler(this.btnUninstall_Click);

                // 安装区域
                this.lblServiceName.AutoSize = true;
                this.lblServiceName.Location = new System.Drawing.Point(12, 380);
                this.lblServiceName.Name = "lblServiceName";
                this.lblServiceName.Size = new System.Drawing.Size(56, 13);
                this.lblServiceName.TabIndex = 5;
                this.lblServiceName.Text = "服务名称:";

                this.txtServiceName.Location = new System.Drawing.Point(80, 377);
                this.txtServiceName.Name = "txtServiceName";
                this.txtServiceName.Size = new System.Drawing.Size(150, 20);
                this.txtServiceName.TabIndex = 6;

                this.lblFilePath.AutoSize = true;
                this.lblFilePath.Location = new System.Drawing.Point(250, 380);
                this.lblFilePath.Name = "lblFilePath";
                this.lblFilePath.Size = new System.Drawing.Size(64, 13);
                this.lblFilePath.TabIndex = 7;
                this.lblFilePath.Text = "程序路径:";

                this.txtFilePath.Location = new System.Drawing.Point(320, 377);
                this.txtFilePath.Name = "txtFilePath";
                this.txtFilePath.Size = new System.Drawing.Size(350, 20);
                this.txtFilePath.TabIndex = 8;

                this.btnBrowse.Location = new System.Drawing.Point(680, 375);
                this.btnBrowse.Name = "btnBrowse";
                this.btnBrowse.Size = new System.Drawing.Size(40, 23);
                this.btnBrowse.TabIndex = 9;
                this.btnBrowse.Text = "...";
                this.btnBrowse.UseVisualStyleBackColor = true;
                this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);

                this.btnInstall.Location = new System.Drawing.Point(730, 375);
                this.btnInstall.Name = "btnInstall";
                this.btnInstall.Size = new System.Drawing.Size(50, 23);
                this.btnInstall.TabIndex = 10;
                this.btnInstall.Text = "安装";
                this.btnInstall.UseVisualStyleBackColor = true;
                this.btnInstall.Click += new EventHandler(this.btnInstall_Click);

                // MainForm
                this.ClientSize = new System.Drawing.Size(784, 421);
                this.Controls.Add(this.btnInstall);
                this.Controls.Add(this.btnBrowse);
                this.Controls.Add(this.txtFilePath);
                this.Controls.Add(this.lblFilePath);
                this.Controls.Add(this.txtServiceName);
                this.Controls.Add(this.lblServiceName);
                this.Controls.Add(this.btnUninstall);
                this.Controls.Add(this.btnStop);
                this.Controls.Add(this.btnStart);
                this.Controls.Add(this.btnRefresh);
                this.Controls.Add(this.dgvServices);
                this.Name = "MainForm";
                this.Text = "Windows 服务管理器";
                this.Load += new EventHandler(this.MainForm_Load);
                ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();
            }

            // ---------- 加载服务列表 ----------
            private void MainForm_Load(object sender, EventArgs e)
            {
                LoadServices();
            }

            private void btnRefresh_Click(object sender, EventArgs e)
            {
                LoadServices();
            }

            private void LoadServices()
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ServiceName", typeof(string));
                    dt.Columns.Add("DisplayName", typeof(string));
                    dt.Columns.Add("Status", typeof(string));
                    dt.Columns.Add("Description", typeof(string));

                    // 使用 WMI 查询所有服务（包含描述）
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                        "SELECT Name, DisplayName, State, Description FROM Win32_Service"))
                    {
                        foreach (ManagementObject service in searcher.Get())
                        {
                            string name = service["Name"]?.ToString() ?? "";
                            string displayName = service["DisplayName"]?.ToString() ?? "";
                            string state = service["State"]?.ToString() ?? "";
                            string description = service["Description"]?.ToString() ?? "";

                            dt.Rows.Add(name, displayName, state, description);
                        }
                    }

                    dgvServices.DataSource = dt;
                    // 设置列宽
                    if (dgvServices.Columns.Count >= 4)
                    {
                        dgvServices.Columns["ServiceName"].Width = 150;
                        dgvServices.Columns["DisplayName"].Width = 180;
                        dgvServices.Columns["Status"].Width = 100;
                        dgvServices.Columns["Description"].Width = 250;
                    }

                    // 根据状态启用按钮
                    UpdateButtonStates();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载服务列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ---------- 更新按钮启用状态 ----------
            private void UpdateButtonStates()
            {
                if (dgvServices.SelectedRows.Count == 0)
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = false;
                    btnUninstall.Enabled = false;
                    return;
                }

                string status = dgvServices.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
                bool isRunning = status.Equals("Running", StringComparison.OrdinalIgnoreCase);
                bool isStopped = status.Equals("Stopped", StringComparison.OrdinalIgnoreCase);

                btnStart.Enabled = isStopped;
                btnStop.Enabled = isRunning;
                btnUninstall.Enabled = true;  // 卸载始终可用
            }

            private void dgvServices_SelectionChanged(object sender, EventArgs e)
            {
                UpdateButtonStates();
            }

            // ---------- 启动服务 ----------
            private void btnStart_Click(object sender, EventArgs e)
            {
                if (dgvServices.SelectedRows.Count == 0) return;
                string serviceName = dgvServices.SelectedRows[0].Cells["ServiceName"].Value.ToString();

                try
                {
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                        {
                            MessageBox.Show("服务未处于停止状态，无法启动。", "提示");
                            return;
                        }
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        MessageBox.Show($"服务 \"{serviceName}\" 已启动。");
                        LoadServices(); // 刷新列表
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"启动失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ---------- 停止服务 ----------
            private void btnStop_Click(object sender, EventArgs e)
            {
                if (dgvServices.SelectedRows.Count == 0) return;
                string serviceName = dgvServices.SelectedRows[0].Cells["ServiceName"].Value.ToString();

                try
                {
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
                        {
                            MessageBox.Show("服务未处于运行状态，无法停止。", "提示");
                            return;
                        }
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        MessageBox.Show($"服务 \"{serviceName}\" 已停止。");
                        LoadServices();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"停止失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ---------- 卸载服务 ----------
            private void btnUninstall_Click(object sender, EventArgs e)
            {
                if (dgvServices.SelectedRows.Count == 0) return;
                string serviceName = dgvServices.SelectedRows[0].Cells["ServiceName"].Value.ToString();

                if (MessageBox.Show($"确定要卸载服务 \"{serviceName}\" 吗？\n此操作将删除服务注册信息。",
                    "确认卸载", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                try
                {
                    // 先尝试停止服务
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        if (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.StartPending)
                        {
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        }
                    }

                    // 执行 sc delete
                    ProcessStartInfo psi = new ProcessStartInfo("sc", $"delete \"{serviceName}\"")
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process p = Process.Start(psi))
                    {
                        p.WaitForExit();
                        string output = p.StandardOutput.ReadToEnd();
                        string error = p.StandardError.ReadToEnd();

                        if (p.ExitCode == 0)
                        {
                            MessageBox.Show($"服务 \"{serviceName}\" 已成功卸载。");
                            LoadServices();
                        }
                        else
                        {
                            MessageBox.Show($"卸载失败，退出码：{p.ExitCode}\n错误信息：{error}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"卸载失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // ---------- 浏览可执行文件 ----------
            private void btnBrowse_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
                    ofd.Title = "选择服务程序";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtFilePath.Text = ofd.FileName;
                        // 自动生成服务名（取文件名）
                        if (string.IsNullOrWhiteSpace(txtServiceName.Text))
                        {
                            txtServiceName.Text = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                        }
                    }
                }
            }

            // ---------- 安装服务 ----------
            private void btnInstall_Click(object sender, EventArgs e)
            {
                string serviceName = txtServiceName.Text.Trim();
                string filePath = txtFilePath.Text.Trim();

                if (string.IsNullOrEmpty(serviceName))
                {
                    MessageBox.Show("请输入服务名称。", "提示");
                    return;
                }
                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("请选择有效的可执行文件路径。", "提示");
                    return;
                }

                // 检查服务是否已存在
                try
                {
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        // 如果能获取到，说明已存在
                        MessageBox.Show($"服务 \"{serviceName}\" 已存在，请更换名称。", "提示");
                        return;
                    }
                }
                catch (InvalidOperationException)
                {
                    // 服务不存在，可以继续安装
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"检查服务是否存在时出错：{ex.Message}", "错误");
                    return;
                }

                if (MessageBox.Show($"确定要安装服务 \"{serviceName}\" 吗？\n路径：{filePath}",
                    "确认安装", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                try
                {
                    // 使用 sc create
                    // 注意：binPath= 后面必须有一个空格
                    string arguments = $"create \"{serviceName}\" binPath= \"{filePath}\" start= auto";
                    ProcessStartInfo psi = new ProcessStartInfo("sc", arguments)
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (Process p = Process.Start(psi))
                    {
                        p.WaitForExit();
                        string output = p.StandardOutput.ReadToEnd();
                        string error = p.StandardError.ReadToEnd();

                        if (p.ExitCode == 0)
                        {
                            MessageBox.Show($"服务 \"{serviceName}\" 安装成功！");
                            LoadServices(); // 刷新列表
                            txtServiceName.Clear();
                            txtFilePath.Clear();
                        }
                        else
                        {
                            MessageBox.Show($"安装失败，退出码：{p.ExitCode}\n错误信息：{error}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"安装失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

