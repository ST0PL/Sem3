package logger;
import enums.LogLevel;
import java.io.IOException;
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
        public void Log(String text, LogLevel level) throws IOException{
            if(innerLogger != null){
                innerLogger.Log(text, level);
            }
        }
    }
    ArrayList<LoggerWrapper> wrappers = new ArrayList<>();

    @Override
    public void Log(String text, LogLevel level) throws IOException{
        for(var wrap : wrappers){
            wrap.Log(text, level);
        }
    }

    public ArrayList<LoggerWrapper> GetWrappers(){
        return new ArrayList<>(wrappers);
    }

    public void RegisterLogger(Loggable logger){
        wrappers.add(new LoggerWrapper(new Random().nextInt(0,9999), logger));
    }
    public void RemoveLogger(int id){
        wrappers.removeIf(wrapper -> wrapper.getId() == id);
    }
}