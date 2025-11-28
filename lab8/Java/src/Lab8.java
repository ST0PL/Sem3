import java.io.IOException;

public class Lab8 {
    public static void main(String[] args) throws IOException {

    }
    public static <T extends Number> double CalculateRequirement(T count, T perOne) {
        return count.doubleValue()*perOne.doubleValue();
    }
}
