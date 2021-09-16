using Godot;
using Godot.Collections;
using System;
using System.Linq;

public enum LogLevel {
    Trace,
    Debug,
    Info,
    Warning,
    Error,
    Critical
}

public class LogMessage: Godot.Object {
    public LogLevel Level;
    public string LoggerName;
    public string Message;
    public int PeerId;
}

public class Logging: Godot.Object {
    private static LogLevel DEFAULT_LOG_LEVEL = LogLevel.Debug;
    private static Dictionary<string, LogLevel> _Levels = new Dictionary<string, LogLevel>();
    private static Dictionary<string, Logging> _Loggers = new Dictionary<string, Logging>();
    public static Array<LogMessage> Messages = new Array<LogMessage>();
    public static bool ShowInConsole = true;

    public string Name;

    public Logging(): this("root") {}
    public Logging(string name) {
        Name = name;
    }

    private static LogLevel ParseLogLevel(string name) {
        Enum.TryParse(name.Capitalize(), out LogLevel logLevel);
        return logLevel;
    }

    public static Logging GetLogger(string name) {
        if (_Loggers.ContainsKey(name)) {
            return _Loggers[name];
        } else {
            var logger = new Logging(name);
            _Loggers[name] = logger;
            return logger;
        }
    }

    private static LogLevel GetLogLevelForName(string name) {
        if (_Levels.ContainsKey(name)) {
            return _Levels[name];
        } else if (_Levels.ContainsKey("")) {
            return _Levels[""];
        } else {
            return DEFAULT_LOG_LEVEL;
        }
    }

    /// Configure log level for each loggers using a configuration string.
    /// Example: info,KVStore=debug
    public static void ConfigureLogLevels(string logLevelConf) {
        var allSplit = logLevelConf.Split(',');
        foreach (var logConf in allSplit) {
            var split = logConf.Split("=");
            if (split.Length == 0) {
                continue;
            } else if (split.Length == 1) {
                // Root logger configuration
                _Levels[""] = ParseLogLevel(split[0]);
            } else if (split.Length == 2) {
                var loggerName = split[0];
                _Levels[loggerName] = ParseLogLevel(split[1]);
            }
        }
    }

    public static void SetMaxLogLevel(string name, LogLevel logLevel) {
        _Levels[name] = logLevel;
    }

    public void SetMaxLogLevel(LogLevel logLevel) {
        SetMaxLogLevel(Name, logLevel);
    }

    private bool _IsLevelShown(LogLevel level) {
        var thisLevel = GetLogLevelForName(Name);
        return level >= thisLevel;
    }

    private string _FormatArgs(params object[] values) {
        return string.Join(" ", values.Select(p => p.ToString()).ToArray());
    }

    private void _ShowLogLine(LogLevel level, string logLine) {
        if (!ShowInConsole) {
            return;
        }

        if (level < LogLevel.Warning) {
            GD.Print(logLine);
        } else {
            GD.PrintErr(logLine);
        }
    }

    #region General log

    private string _FormatLog(LogLevel level, params object[] values) {
        var log_level = level.ToString().ToUpper();
        return $"[{log_level}] [{Name}] {_FormatArgs(values)}";
    }

    private void _Log(LogLevel level, params object[] values) {
        if (!_IsLevelShown(level)) {
            return;
        }

        Messages.Add(new LogMessage() {
            Level = level,
            LoggerName = Name,
            Message = _FormatArgs(values)
        });
        _ShowLogLine(level, _FormatLog(level, values));
    }

    public void Trace(params object[] values) {
        _Log(LogLevel.Trace, values);
    }

    public void Debug(params object[] values) {
        _Log(LogLevel.Debug, values);
    }

    public void Info(params object[] values) {
        _Log(LogLevel.Info, values);
    }

    public void Warn(params object[] values) {
        _Log(LogLevel.Warning, values);
    }

    public void Error(params object[] values) {
        _Log(LogLevel.Error, values);
    }

    public void Critical(params object[] values) {
        _Log(LogLevel.Critical, values);
    }

    #endregion

    #region Method log

    private string _FormatLogMethod(LogLevel level, string method, params object[] values) {
        var log_level = level.ToString().ToUpper();
        return $"[{log_level}] [{Name}::{method}] {_FormatArgs(values)}";
    }

    private void _LogMethod(LogLevel level, string method, params object[] values) {
        if (!_IsLevelShown(level)) {
            return;
        }

        Messages.Add(new LogMessage() {
            Level = level,
            LoggerName = $"{Name}::{method}",
            Message = _FormatArgs(values)
        });
        _ShowLogLine(level, _FormatLogMethod(level, method, values));
    }

    public void TraceM(string method, params object[] values) {
        _LogMethod(LogLevel.Trace, method, values);
    }

    public void DebugM(string method, params object[] values) {
        _LogMethod(LogLevel.Debug, method, values);
    }

    public void InfoM(string method, params object[] values) {
        _LogMethod(LogLevel.Info, method, values);
    }

    public void WarnM(string method, params object[] values) {
        _LogMethod(LogLevel.Warning, method, values);
    }

    public void ErrorM(string method, params object[] values) {
        _LogMethod(LogLevel.Error, method, values);
    }

    public void CriticalM(string method, params object[] values) {
        _LogMethod(LogLevel.Critical, method, values);
    }

    #endregion

    #region Network log

    private string _FormatLogMethodNetwork(LogLevel level, int peerId, string method, params object[] values) {
        var log_level = level.ToString().ToUpper();
        return $"[{log_level}] [{Name}::{method}] *{peerId}* {_FormatArgs(values)}";
    }

    private void _LogMethodNetwork(LogLevel level, int peerId, string method, params object[] values) {
        if (!_IsLevelShown(level)) {
            return;
        }

        Messages.Add(new LogMessage() {
            Level = level,
            LoggerName = $"{Name}::{method}",
            PeerId = peerId,
            Message = _FormatArgs(values)
        });
        _ShowLogLine(level, _FormatLogMethodNetwork(level, peerId, method, values));
    }

    public void TraceMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Trace, peerId, method, values);
    }

    public void DebugMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Debug, peerId, method, values);
    }

    public void InfoMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Info, peerId, method, values);
    }

    public void WarnMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Warning, peerId, method, values);
    }

    public void ErrorMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Error, peerId, method, values);
    }

    public void CriticalMN(int peerId, string method, params object[] values) {
        _LogMethodNetwork(LogLevel.Critical, peerId, method, values);
    }

    #endregion

    #region Node utilities

    public void DumpTree(SceneTree tree, bool showSceneTrees = false, string prefix = "", LogLevel level = LogLevel.Info) {
        DumpNode(tree.Root, recursive: true, showSceneTrees: showSceneTrees, prefix: prefix, level: level);
    }

    public void DumpNode(Node node, bool recursive = false, bool showSceneTrees = false, string prefix = "", LogLevel level = LogLevel.Info) {
        var name = node.Name;
        var id = node.GetInstanceId();
        var typ = node.GetType();

        if (name == "root") {
            _Log(level, $"{prefix}/");
        } else {
            _Log(level, $"{prefix}|- {name} ({typ}) (#{id})");
        }

        if (name != "root") {
            prefix = $"{prefix}  ";
        }

        if (recursive) {
            if (showSceneTrees && node is ListenServerPeer listenServerPeer) {
                DumpTree(listenServerPeer.GetServerTree(), showSceneTrees, prefix, level);
            } else {
                foreach (Node child in node.GetChildren()) {
                    DumpNode(child, recursive, showSceneTrees, prefix, level);
                }
            }
        }
    }

    #endregion
}