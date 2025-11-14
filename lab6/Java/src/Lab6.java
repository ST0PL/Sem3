
import enums.LogLevel;
import java.io.IOException;
import static java.lang.System.out;
import logger.*;
import unit.Unit;

class Lab6 {
    public static void main(String[] args) throws IOException {

        FileLogger logger = new FileLogger("logs.txt");
        logger.Log("Test", LogLevel.WARN, true);
        
    }

    public static void printTree(Unit parent, String tabs){
        
        out.println(tabs+parent.getName());

        
        for(var soldier : parent.getPersonnel()){
            out.println(tabs+'\t'+soldier.toString());
        }

        if(!parent.getChildren().isEmpty()){
            for(var child : parent.getChildren()){
                printTree(child, tabs+'\t');
            }
        }        
    }
}