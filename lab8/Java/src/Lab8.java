import enums.LogLevel;
import java.io.IOException;
import logger.*;

public class Lab8 {
    public static void main(String[] args) throws IOException {
        Logger logger = new Logger();
        logger.RegisterLogger(new FileLogger("logs.txt"));
        logger.RegisterLogger(new ConsoleLogger());
        logger.Log("Демонстрация мульти-логгера", LogLevel.INFO);
        logger.RemoveLogger(logger.GetWrappers().get(1).getId());
        logger.Log("Сообщение после удаления консольного логгера", LogLevel.WARN);
    }
}
