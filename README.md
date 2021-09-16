# Development Tools for Godot Engine (3.3.3, C#)

Contains development tools for Godot Engine.

## How to install

Use `git` submodules: open a command prompt in your project folder and then:

```
git submodule add https://github.com/Srynetix/godot-plugin-debug addons/debug
```

## How to use

Add the `DebugMenu` scene as autoload, and enjoy the included tools:

- The node tracer panel (`F2`)
    - Contains properties exposed in your nodes containing a `NodeTracer` child node.
- The logging panel (F3)
    - Displays log messages with severity.

You also have the `Logging` facility to write logs to the console or to the logging panel.  
Usage:

```cs
var logger = Logging.GetLogger("MyLogger");
logger.InfoM("MyMethod", "Hello, I am a sample text in a method called MyMethod");
```

## Licenses

- Office Code Pro - SIL Open Font License 1.1 - https://github.com/nathco/Office-Code-Pro