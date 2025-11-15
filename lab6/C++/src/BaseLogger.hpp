#pragma once
#include <string>

class BaseLogger {
public:
    BaseLogger() = default;
    BaseLogger(const BaseLogger&) = delete;
    void Log(const std::string&) const;
    virtual ~BaseLogger();
public:
    std::string GetNowTime() const;
    virtual std::string GetNowTimeVirtual() const;
};