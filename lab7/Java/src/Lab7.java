import enums.LogLevel;
import java.io.IOException;
import logger.*;

public class Lab7 {
    public static void main(String[] args) throws IOException {
        Logger logger = new Logger();
        logger.RegisterLogger(new FileLogger("logs.txt"));
        logger.RegisterLogger(new ConsoleLogger());
        logger.Log("Демонстраци мульти-логгера", LogLevel.INFO);
        logger.RemoveLogger(logger.GetWrappers().get(1).getId());
        logger.Log("Сообщение после удаления коснольного логгера", LogLevel.WARN);
    }
}
