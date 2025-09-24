
**系统清理 / 优化工具功能清单**，不仅仅是“删除临时文件”，还包括注册表、卸载残留、系统优化项。
我先帮你把 **可做的清理和优化点** 分门别类列出来，像一个“功能池”，后面你可以从里头挑选做 MVP。

---

# 🗑️ 一、垃圾文件清理

1. **用户临时文件**

   * `%TEMP%` 目录
   * `%LocalAppData%\Temp`

2. **系统临时文件**

   * `C:\Windows\Temp`

3. **缓存类**

   * 浏览器缓存（Edge、Chrome、Firefox）
   * 缩略图缓存（`%LocalAppData%\Microsoft\Windows\Explorer\thumbcache_*`）
   * Windows 更新缓存（`C:\Windows\SoftwareDistribution\Download`）
   * 日志文件（.log）
   * Prefetch 文件（`C:\Windows\Prefetch`）

4. **回收站**

   * 清空所有驱动器回收站

---

# 🧹 二、注册表清理 / 优化

1. **无效的卸载项**

   * `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall`
   * `HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall`
   * 已卸载软件残留的条目

2. **文件关联残留**

   * 无效的 `.xxx` → 程序路径不存在

3. **服务/启动项残留**

   * 注册表里记录但程序已不存在

4. **孤立 COM/CLSID 项**

   * 指向的 DLL/EXE 文件已被删除

⚠️ 注意：注册表清理风险较高，容易误删 → 建议：

* 只扫描并提示用户“疑似无效项”
* 默认不自动删除
* 提供备份与恢复

---

# 🖥️ 三、系统优化项

1. **禁用/启用 Windows Search 索引**

   * 提高 SSD 用户性能，或在 HDD 上保留搜索功能

2. **关闭 Windows Defender 实时监控**（谨慎⚠️）

   * 可能提高性能，但带来安全风险 → 建议仅提供开关

3. **关闭不必要的启动项/服务**

   * 提供 UI 展示，用户勾选禁用

4. **禁用 Telemetry / 数据收集**

   * 关闭 `DiagTrack`、`CompatTelRunner` 等

5. **视觉效果优化**

   * 一键关闭动画、透明效果（类似“调整为最佳性能”）

6. **网络优化**

   * 调整 TCP/IP 参数（禁用 Windows Auto-Tuning）
   * 删 DNS 缓存

---

# 📦 四、软件卸载残留清理

1. **卸载目录残留**

   * 检测已卸载软件的安装目录是否仍存在（Program Files / AppData）
   * 类似 GeekUninstaller 的“强制移除”

2. **注册表卸载残留**

   * 卸载记录还在，但实际软件目录已删除

3. **服务/驱动残留**

   * 已卸载软件的驱动、服务未被删除

---

# 📊 五、系统维护项

1. **DISM / SFC 工具集成**

   * `dism /Online /Cleanup-Image /StartComponentCleanup`
   * `sfc /scannow`

2. **磁盘清理集成**

   * 调用 `cleanmgr.exe` 或 Storage Sense API

3. **磁盘碎片整理（传统 HDD）**

   * 提供入口

4. **计划任务清理**

   * 列出已不存在软件的计划任务，提示删除

---

# ✅ 总结

可以做的清理/优化项分为 5 大类：

1. **垃圾文件清理**（临时文件、缓存、回收站）
2. **注册表清理**（无效卸载项、文件关联、服务残留）
3. **系统优化**（禁用搜索、关闭 Defender、禁用启动项）
4. **软件卸载残留清理**（目录、注册表、服务/驱动）
5. **系统维护**（DISM、SFC、计划任务、碎片整理）

---

👉 我建议你先做 **1. 垃圾文件清理 + 5. 系统维护（DISM、SFC 集成）** 作为 MVP，风险低、容易见效。
等工具稳定了，再逐步增加 **注册表清理和优化功能**，避免一开始就让用户遇到“误删系统关键项”的风险。

---