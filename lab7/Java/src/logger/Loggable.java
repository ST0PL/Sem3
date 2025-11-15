package logger;
import enums.LogLevel;

public interface Loggable{
    public void Log(String text, LogLevel level);
}