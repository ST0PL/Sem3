#pragma once
#include "AdvancedLogger.hpp"

class FileLogger : public AdvancedLogger {
    using AdvancedLogger::Log;
public:
    FileLogger(const std::string&);
    FileLogger(const FileLogger&) = delete;
    void Log(const std::string&, LogLevel, int, bool = false) const;
    ~FileLogger();
private:
    std::string m_fileName;
};