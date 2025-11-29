package unit;

import enums.Rank;
import enums.Speciality;
import java.util.Map;
import static java.util.Map.entry;

public class Staff {
    int id;
    String fullName;
    Rank rank;
    Speciality speciality;
    int unitId;
    Unit unit;

    static final Map<Rank, String> ranks = Map.ofEntries(
        entry(Rank.PRIVATE, "Рядовой"),
        entry(Rank.CORPORAL, "Ефрейтор"),
        entry(Rank.JUNIOR_SERGEANT, "Младший сержант"),
        entry(Rank.SERGEANT, "Сержант"),
        entry(Rank.SENIOR_SERGEANT, "Старший сержант"),
        entry(Rank.SERGEANT_MAJOR, "Старшина"),
        entry(Rank.WARRANT_OFFICER, "Прапорщик"),
        entry(Rank.SENIOR_WARRANT_OFFICER, "Старший прапорщик"),
        entry(Rank.JUNIOR_LIEUTENANT, "Младший лейтенант"),
        entry(Rank.LIEUTENANT, "Лейтенант"),
        entry(Rank.SENIOR_LIEUTENANT, "Старший лейтенант"),
        entry(Rank.CAPTAIN, "Капитан"),
        entry(Rank.MAJOR, "Майор"),
        entry(Rank.LIEUTENANT_COLONEL, "Подполковник"),
        entry(Rank.COLONEL, "Полковник"),
        entry(Rank.MAJOR_GENERAL, "Генерал-майор"),
        entry(Rank.LIEUTENANT_GENERAL, "Генерал-лейтенант"),
        entry(Rank.COLONEL_GENERAL, "Генерал-полковник"),
        entry(Rank.ARMY_GENERAL, "Генерал армии"));

    static final Map<Speciality, String> specialities = Map.ofEntries(
        entry(Speciality.NONE, "Отсутствует"),
        entry(Speciality.INFANTRY, "Пехотинец"),
        entry(Speciality.MACHINE_GUNNER, "Пулеметчик"),
        entry(Speciality.GRENADE_LAUNCHER, "Гранатометчик"),
        entry(Speciality.RECONNAISSANCE, "Разведчик"),
        entry(Speciality.SPECIAL_FORCES, "Спецназ"),
        entry(Speciality.SNIPER, "Снайпер"),
        entry(Speciality.TANKER, "Танкист"),
        entry(Speciality.ANTI_AIRCRAFT, "Зенитчик"),
        entry(Speciality.ENGINEER, "Инженер"),
        entry(Speciality.MEDIC, "Медик"));

    public Staff(int id, String fullName, Rank rank, Speciality speciality){
        this.id = id;
        this.fullName = fullName;
        this.rank = rank;
        this.speciality = speciality;
    }

    public int getId(){
        return id;
    }
    
    public String getFullName(){
        return fullName;
    }

    public Rank getRank(){
        return rank;
    }

    public Speciality getSpeciality(){
        return speciality;
    }

    public void setFullName(String fullName){
        this.fullName = fullName;
    }

    public void setRank(Rank rank){
        this.rank = rank;
    }

    public void setSpeciality(Speciality speciality){
        this.speciality = speciality;
    }

    public void setUnit(Unit unit){
        if(unit != null){
            this.unitId = unit.getId();
            this.unit = unit;
        }
    }

    public Unit getUnit(){
        return unit;
    }

    @Override
    public String toString(){
        return String.format("%s [Звание: %s, Специальность: %s]",
             fullName, rankToString(rank), specialityToString(speciality));
    }

    static String rankToString(Rank rank) {
        return ranks.getOrDefault(rank, "Неизвестно");
    }

    static String specialityToString(Speciality speciality){
        return specialities.getOrDefault(speciality, "Неизвестно");
    }

}
