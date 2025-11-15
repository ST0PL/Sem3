#pragma once
#include <vector>
#include <memory>
#include "ILogger.hpp"
class Logger : public ILogger {
public:
    class LoggerWrap {
    public:
        LoggerWrap(int id, std::unique_ptr<ILogger>);
        void Log(const std::string&, LogLevel level) const;
        int GetId() const;
    private:
        int m_id;
        std::unique_ptr<ILogger> m_innerLogger;
    };
    void Log(const std::string&, LogLevel level) const override;
    void RegisterLogger(std::unique_ptr<ILogger>);
    void RemoveLogger(int id);
private:
    std::vector<std::unique_ptr<LoggerWrap>> m_loggers;
};