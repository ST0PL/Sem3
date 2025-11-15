#pragma once
#include "AdvancedLogger.hpp"

class FileLogger : public ConsoleLogger {
public:
    FileLogger(const std::string&);
    FileLogger(const FileLogger&) = delete;
    void Log(const std::string&, LogLevel, int) const;
    void Log(const std::string&, LogLevel) const override;
private:
    std::string m_fileName;
};