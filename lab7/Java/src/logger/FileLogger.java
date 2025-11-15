package logger;
import enums.LogLevel;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
public class FileLogger extends ConsoleLogger{

    private Path path;

    public Path getPath(){
        return path;
    }

    public FileLogger(String path){
        this.path = Paths.get(path);
    }

    public void Log(String text, LogLevel level, Boolean print) throws IOException{
        String formatted = String.format("[%s][%s] %s", GetNowTime(), levels.get(level), text);
        Files.writeString(path, (Files.exists(path) && Files.size(path) > 0 ? System.lineSeparator() : "") + formatted, StandardOpenOption.CREATE, StandardOpenOption.APPEND);
    }
}