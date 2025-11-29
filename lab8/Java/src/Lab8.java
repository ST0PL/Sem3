import enums.*;
import java.io.IOException;
import static java.lang.System.out;
import java.util.Comparator;
import java.util.List;
import warehouse.equipments.Weapon;
import warehouse.resources.Ammunition;
import warehouse.resources.Fuel;

public class Lab8 {
    public static void main(String[] args) throws IOException {
        WarehouseRepository<Ammunition> ammoRepo = new WarehouseRepository<>();

        ammoRepo.add(new Ammunition(1, "5.45mm", Caliber.C_545MM, 1000));
        ammoRepo.add(new Ammunition(2, "122mm", Caliber.C_122MM, 50));
        ammoRepo.add(new Ammunition(3, "5.45mm", Caliber.C_545MM, 500));

        out.println("До сортировки\n");

        ammoRepo.getItems().forEach(a->
        {
            out.println(String.format("ID: %d", a.getId()));
            out.println(String.format("Название: %s", a.getName()));
            out.println(String.format("Калибр: %s", a.getCaliber()));  
            out.println(String.format("Количество: %f\n", a.getQuantity()));          
        });

        out.println("После сортировки по количеству\n");

        var sorted = ammoRepo.orderBy(Comparator.comparingDouble(a->a.getQuantity())).toList().reversed();

        sorted.forEach(a->
        {
            out.println(String.format("ID: %d", a.getId()));
            out.println(String.format("Название: %s", a.getName()));
            out.println(String.format("Калибр: %s", a.getCaliber()));  
            out.println(String.format("Количество: %f\n", a.getQuantity()));          
        });

        out.println(String.format("Размер репозитория боеприпасов: %d", ammoRepo.getSize()));
        out.println(String.format("Репозиторий пуст?: %s", ammoRepo.isEmpty() ? "да" : "нет" ));

        // Поиск по ID
        var found = ammoRepo.findById(1);
        if (found.isPresent()) {
            out.println(String.format("Элемент с идентификатором 1: %s", found.get().getName()));
        }

        // Поиск через предикат
        var result1 = ammoRepo.find(a->a.getQuantity() > 100);
        out.println(String.format("Боеприпасы в количестве более 100: %d", result1.count()));

        // Поиск по типу МТО
        var result2 = ammoRepo.findByMaterialType(MaterialType.AMMUNITION);
        out.println(String.format("Количество через поиск по типу: %d", result2.count()));

        // Подсчет количества записей МТО определенного типа
        var count = ammoRepo.countByMaterialType(MaterialType.AMMUNITION);
        out.println(String.format("Количество записей боеприпасов: %d", count, '\n'));

        // Удаление по идентификатору
        boolean removed = ammoRepo.remove(2);
        out.println(String.format("Удаление элемента с ID 2: %s", removed ? "удален" : "не удален"));
        out.println(String.format("Размер после удаления: %d", ammoRepo.getSize()));

        out.println(String.format("Содержит элемент с ID 1?: %s", ammoRepo.containsId(1) ? "да" : "нет"));
        out.println(String.format("Содержит элемент с ID 2? %s", ammoRepo.containsId(2) ? "да" : "нет"));


        WarehouseRepository<Fuel> fuelRepo = new WarehouseRepository<>();
        fuelRepo.add(new Fuel(4, "Дизель", FuelType.DIESEL, 2000));
        fuelRepo.add(new Fuel(5, "Бензин", FuelType.GASOLINE, 1500));

        var allFuel = fuelRepo.findByMaterialType(MaterialType.FUEL).toList();

        out.println(String.format("Количество записей с топливом: %d", allFuel.size()));
        
        allFuel.forEach(f->
        {
            out.println(String.format("Название: %s", f.getName()));
            out.println(String.format("Литры: %f", f.getQuantity()));
            out.println(String.format("Проверка на пустоту: %s", f.isEmpty() ? "пустое" : "не пустое"));
        });

        // Поиск по наполняемости

        WarehouseRepository<Weapon> weaponRepo = new WarehouseRepository<>();
        weaponRepo.add(new Weapon(6, "AK-74", Caliber.C_545MM, 10));
        weaponRepo.add(new Weapon(7, "РСЗО Град", Caliber.C_122MM, 5));

        var nonEmpty = weaponRepo.findNonEmpty();
        out.println(String.format("Не пустое вооружения: %d записей", nonEmpty.count()));

        var empty = weaponRepo.findEmpty();
        out.println(String.format("Пустое вооружения: %d записей", empty.count()));

        // Массовые операции

        WarehouseRepository<Ammunition> repo = new WarehouseRepository<>();

        List<Ammunition> ammos =
            List.of(new Ammunition(8, "5.45mm", Caliber.C_545MM, 100), new Ammunition(9, "122mm", Caliber.C_122MM, 20));

        repo.addRange(ammos);
        out.println(String.format("После массового добавления: %d", repo.getSize()));

        List<Integer> idsToRemove = List.of(8,9);
        removed = repo.removeRange(idsToRemove);
        out.println(String.format("Удален хотя бы один элемент?: %s", removed ? "да" : "нет"));
        out.println(String.format("Размер после массового удаления: %d", repo.getSize()));

        // Шаблонная функция

        double distanceKm = 45.3;
        double fuelPerKm = 0.08;
        double total_fuel = CalculateRequirement(distanceKm, fuelPerKm);
        out.println(String.format("Общий расход топлива: %f", total_fuel));        
    }

    public static <T extends Number> double CalculateRequirement(T count, T perOne) {
        return count.doubleValue()*perOne.doubleValue();
    }
}
