#include "Logger.hpp"


Logger::LoggerWrap::LoggerWrap(int id, std::unique_ptr <ILogger> logger)
: m_id(id), m_innerLogger(std::move(logger)){}

void Logger::LoggerWrap::Log(const std::string& text, LogLevel level) const {
    if (m_innerLogger)
        m_innerLogger->Log(text, level);
}

int Logger::LoggerWrap::GetId() const {
    return m_id;
}

void Logger::Log(const std::string& text, LogLevel level) const {
    for (auto& logger : m_loggers)
        logger->Log(text, level);
}
void Logger::RegisterLogger(std::unique_ptr<ILogger> logger) {
    m_loggers.emplace_back(std::make_unique<LoggerWrap>(m_loggers.size(), std::move(logger)));
}

void Logger::RemoveLogger(int id) {
    m_loggers.erase(std::remove_if(m_loggers.begin(), m_loggers.end(),
        [&id](const std::unique_ptr<LoggerWrap>& wrap) { return wrap->GetId() == id; }), m_loggers.end());
}
const std::vector<std::unique_ptr<Logger::LoggerWrap>>& Logger::GetLoggers() const
{
    return m_loggers;
};