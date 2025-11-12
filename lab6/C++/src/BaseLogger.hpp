#pragma once
#include <string>

class BaseLogger {
public:
    BaseLogger(const BaseLogger&) = delete;
    void Log(const std::string&) const;
    virtual ~BaseLogger() = default;
protected:
    std::string GetNowTime(const std::string) const;
};