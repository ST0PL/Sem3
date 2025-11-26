#include <iostream>
#include <memory>
#include "Logger.hpp"
#include "FileLogger.hpp"
#include "ConsoleLogger.hpp"

using namespace std;

int main()
{
    setlocale(LC_ALL, "Rus");
    auto logger = make_unique<Logger>();
    logger->RegisterLogger(make_unique<FileLogger>("logs.txt"));
    logger->RegisterLogger(make_unique<ConsoleLogger>());
    logger->Log("Демонстрация мульти-логгера", LogLevel::eInfo);
    logger->RemoveLogger(logger->GetLoggers()[1]->GetId());
    logger->Log("Сообщение после удаления консольного логгера", LogLevel::eWarn);
}
