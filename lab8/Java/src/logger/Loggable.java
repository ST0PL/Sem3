package logger;
import enums.LogLevel;
import java.io.IOException;

public interface Loggable{
    public void Log(String text, LogLevel level) throws IOException;
}