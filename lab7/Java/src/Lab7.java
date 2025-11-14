import enums.*;
import java.io.IOException;
import java.util.ArrayList;
import logger.*;
import request.*;
import unit.*;
import warehouse.*;
import warehouse.resources.*;

public class Lab7 {
    public static void main(String[] args) throws CloneNotSupportedException, IOException {

        // Перегрузка базовых методов и переопределение виртуальных

        BaseLogger base = new BaseLogger();
        BaseLogger baseRef = new AdvancedLogger();
        base.Log("Вызов BaseLogger.Log через базовый тип");
        baseRef.Log("Вызов BaseLogger.Log через ссылку на AdvancedLogger (GetNowTime переопределён)");
        FileLogger fileLogger = new FileLogger("log.txt");
        fileLogger.Log("Вывод сообщения в консоль через перегрузку FileLogger (print = true)", LogLevel.INFO, true);

        // Клонирование

        Unit unit = new Unit(10, "1-й батальон", UnitType.BATTALION);

        Ammunition ammo = new Ammunition(1, "патроны 5.45мм", Caliber.C_545MM, 100);
        Ammunition ammoClone = ammo.clone();
        System.out.printf("\nОригинал: id=%d, name=%s, quantity=%.1f%n",
                ammo.getId(), ammo.getName(), ammo.getQuantity());
        System.out.printf("Клон:     id=%d, name=%s, quantity=%.1f%n",
                ammoClone.getId(), ammoClone.getName(), ammoClone.getQuantity());

        WarehouseEntry<Float> entry = ammo;
        SupplyRequestDetail checkDetail = new SupplyRequestDetail(1, SupplyType.AMMUNITION, 1.0f)
                .withCaliber(Caliber.C_545MM);
        System.out.println("entry.isMatches() => " + entry.isMatches(checkDetail));


        ArrayList<SupplyRequestDetail> details = new ArrayList<>();
        details.add(new SupplyRequestDetail(1, SupplyType.AMMUNITION, 30f).withCaliber(Caliber.C_545MM));

        // Демонстрация глубокого и поверхностного клонирования через список деталей запроса

        SupplyRequest request = unit.createRequest(details);

        System.out.printf("\nОригинал (ID: %d):", request.getId());
        for (SupplyRequestDetail d : request.getDetails()) {
            System.out.println("\n\t" + d.toString());
        }

        SupplyRequest shallow = request.clone(true);
        SupplyRequest deep = request.clone(false);

        request.getDetails().get(0).setCount(300);
        
        System.out.printf("\nОригинал (ID: %d) (после изменения детали)", request.getId());
        for (SupplyRequestDetail d : request.getDetails()) {
            System.out.println("\n\t" + d.toString());
        }        
        System.out.printf("\nПоверхностный клон (ID: %d):", shallow.getId());
        for (SupplyRequestDetail d : shallow.getDetails()) {
            System.out.println("\n\t" + d.toString());
        }
        System.out.printf("\nГлубокий клон (ID: %d):", deep.getId());
        for (SupplyRequestDetail d : deep.getDetails()) {
            System.out.println("\n\t" + d.toString());
        }
        
        // Использование интерфейса

        Warehouse wh = new Warehouse(1, "Тыловой склад", WarehouseType.REAR);
        wh.addResource(new Ammunition(100, "патроны 5.45мм", Caliber.C_545MM, 200));
        unit.assignWarehouse(wh);
        Suppliable suppliable = unit;
        SupplyResponse response = suppliable.makeSupplyRequest(request);
        System.out.println("\nСтатус запроса: " + SupplyResponse.statusToString(response.getStatus())
                + " | Комментарий: " + response.getComment());
    }
}
