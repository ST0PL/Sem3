#include <fstream>
#include <iostream>
#include "FileLogger.hpp"

FileLogger::FileLogger(const std::string& fileName) : m_fileName(fileName) {}

void FileLogger::Log(const std::string& text, LogLevel level, int mode) const {
    std::ofstream fileStream(m_fileName,mode);
    if (fileStream.is_open()) {
        fileStream << "[" << GetNowTime() << "]" << "[" << m_levels.at(level) << "] " << text << std::endl;
        fileStream.close();
    }
}

void FileLogger::Log(const std::string& text, LogLevel level) const {
    Log(text, level, std::ios::app);
}