package unit;

import enums.Rank;
import enums.Speciality;

public class Staff {
    int id;
    String fullName;
    Rank rank;
    Speciality speciality;
    int unitId;
    Unit unit;

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
        return switch(rank) {
            case PRIVATE -> "Рядовой";
            case CORPORAL -> "Ефрейтор";
            case JUNIOR_SERGEANT -> "Младший сержант";
            case SERGEANT -> "Сержант";
            case SENIOR_SERGEANT -> "Старший сержант";
            case SERGEANT_MAJOR -> "Старшина";
            case WARRANT_OFFICER -> "Прапорщик";
            case SENIOR_WARRANT_OFFICER -> "Старший прапорщик";
            case JUNIOR_LIEUTENANT -> "Младший лейтенант";
            case LIEUTENANT -> "Лейтенант";
            case SENIOR_LIEUTENANT -> "Старший лейтенант";
            case CAPTAIN -> "Капитан";
            case MAJOR -> "Майор";
            case LIEUTENANT_COLONEL -> "Подполковник";
            case COLONEL -> "Полковник";
            case MAJOR_GENERAL -> "Генерал-майор";
            case LIEUTENANT_GENERAL -> "Генерал-лейтенант";
            case COLONEL_GENERAL -> "Генерал-полковник";
            case ARMY_GENERAL -> "Генерал армии";
        };
    }

    static String specialityToString(Speciality speciality){
        return switch(speciality){
            case NONE -> "Отсутствует";
            case INFANTRY -> "Пехотинец";
            case MACHINE_GUNNER -> "Пулеметчик";
            case GRENADE_LAUNCHER -> "Гранатометчик";
            case RECONNAISSANCE -> "Разведчик";
            case SPECIAL_FORCES -> "Спецназ";
            case SNIPER -> "Снайпер";
            case TANKER -> "Танкист";
            case ANTI_AIRCRAFT -> "Зенитчик";
            case ENGINEER -> "Инженер";
            case MEDIC -> "Медик";
        };
    }

}
