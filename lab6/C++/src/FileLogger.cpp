#include <fstream>
#include "FileLogger.hpp"


void FileLogger::Log(const std::string& filename, const std::string& text, LogLevel level, bool print) const {
    if (print)
        Log(text, level);

    std::ofstream fileStream(filename,std::ios::app);

    if (fileStream.is_open()) {
        fileStream << "[" << GetNowTime("%H:%M:%S") << "]" << "[" << m_levels.at(level) << "] " << text << std::endl;
        fileStream.close();
    }
}