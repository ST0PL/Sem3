package logger;
import enums.LogLevel;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Map;

public class ConsoleLogger implements Loggable{


    protected Map<LogLevel, String> levels = Map.of(
        LogLevel.INFO, "Информация",
        LogLevel.WARN, "Предупреждение",
        LogLevel.DEBUG, "Отладка",
        LogLevel.ERROR, "Ошибка");

    public void Log(String text, LogLevel level){
        System.out.println(String.format("[%s][%s] %s", GetNowTime(), levels.get(level), text));
    }
    
    public String GetNowTime(){
        return LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd.MM.yyyy HH:mm:ss"));
    }
}