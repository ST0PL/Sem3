package logger;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class BaseLogger{
    public void Log(String text){
        System.out.println(String.format("[LOGS][%s] %s", GetNowTime(), "LOGS", text));

    }
    public String GetNowTime(){
        return LocalDateTime.now().format(DateTimeFormatter.ofPattern("HH:mm:ss"));
    }
}