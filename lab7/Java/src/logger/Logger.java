package logger;
import enums.LogLevel;
import java.util.ArrayList;
import java.util.Random;

public class Logger implements Loggable {
    public class LoggerWrapper implements Loggable{
        int id;
        Loggable innerLogger;        
        LoggerWrapper(int id, Loggable innerLogger){
            this.id = id;
            this.innerLogger = innerLogger;
        }
        public int getId(){
            return id;
        }
        @Override
        public void Log(String text, LogLevel level){
            if(innerLogger != null){
                innerLogger.Log(text, level);
            }
        }
    }
    ArrayList<LoggerWrapper> wrappers = new ArrayList<>();

    @Override
    public void Log(String text, LogLevel level){
        for(var wrap : wrappers){
            wrap.Log(text, level);
        }
    } 

    public void RegisterLogger(Loggable logger){
        wrappers.add(new LoggerWrapper(new Random().nextInt(wrappers.size() > 0 ? wrappers.getLast().getId() : 0,9999), logger));
    }
    public void RemoveLogger(int id){
        wrappers.removeIf(wrapper -> wrapper.getId() == id);
    }
}