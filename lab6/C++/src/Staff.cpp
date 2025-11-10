#include "Staff.hpp"

Staff::Staff(const Staff& other) :
    m_id(rand()), m_fullName(other.m_fullName), m_rank(other.m_rank),
    m_speciality(other.m_speciality), m_unitId(other.m_unitId), m_unit(other.m_unit){ }

Staff::Staff(int id, const std::string& fullName, Rank rank, Speciality speciality) :
    m_id(id), m_fullName(fullName), m_rank(rank), m_speciality(speciality) {
}

int Staff::GetId() const {
    return m_id;
}

const std::string& Staff::GetFullName() const {
    return m_fullName;
}

Rank Staff::GetRank() const {
    return m_rank;
}

Speciality Staff::GetSpeciality() const {
    return m_speciality;
}

int Staff::GetUnitId() const {
    return m_unitId;
}

std::weak_ptr<Unit> Staff::GetUnit() const {
    return m_unit;
}

void Staff::SetId(int id) {
    m_id = id;
}

void Staff::SetFullName(const std::string& fullName) {
    m_fullName = fullName;
}

void Staff::SetRank(Rank rank) {
    m_rank = rank;
}

void Staff::SetSpeciality(Speciality speciality) {
    m_speciality = speciality;
}

void Staff::SetUnit(std::weak_ptr<Unit> unit) {
    if (auto u = unit.lock()) {
        m_unitId = u->GetId();
        m_unit = unit;
    }
}

bool Staff::operator < (const Staff& other) const {
    return m_rank < other.m_rank;
}

bool Staff::operator > (const Staff& other) const {
    return !(*this < other);
}

bool Staff::operator == (const Staff& other) const {
    return m_id == other.m_id;
}

bool Staff::operator != (const Staff& other) const {
    return !(*this == other);
}

bool Staff::operator <= (const Staff& other) const {
    return *this < other || *this == other;
}

bool Staff::operator >= (const Staff& other) const {
    return *this > other || *this == other;
}

std::string Staff::RankToString(Rank rank) {
    switch (rank) {
    case Rank::ePrivate: 
        return "Рядовой";
    case Rank::eCorporal: 
        return "Ефрейтор";
    case Rank::eJuniorSergeant: 
        return "Младший сержант";
    case Rank::eSergeant: 
        return "Сержант";
    case Rank::eSeniorSergeant: 
        return "Старший сержант";
    case Rank::eSergeantMajor:
        return "Старшина";
    case Rank::eWarrantOfficer: 
        return "Прапорщик";
    case Rank::eSeniorWarrantOfficer: 
        return "Старший прапорщик";
    case Rank::eJuniorLieutenant: 
        return "Младший лейтенант";
    case Rank::eLieutenant:
        return "Лейтенант";
    case Rank::eSeniorLieutenant:
        return "Старший лейтенант";
    case Rank::eCaptain:
        return "Капитан";
    case Rank::eMajor: 
        return "Майор";
    case Rank::eLieutenantColonel: return "Подполковник";
    case Rank::eColonel: 
        return "Полковник";
    case Rank::eMajorGeneral:
        return "Генерал-майор";
    case Rank::eLieutenantGeneral:
        return "Генерал-лейтенант";
    case Rank::eColonelGeneral:
        return "Генерал-полковник";
    case Rank::eArmyGeneral:
        return "Генерал армии";
    default:
        return "Неизвестно";
    }
}

std::string Staff::SpecialityToString(Speciality speciality) {
    switch (speciality) {
    case Speciality::eNone:
        return "Отсутствует";
    case Speciality::eInfantry:
        return "Пехотинец";
    case Speciality::eMachineGunner:
        return "Пулеметчик";
    case Speciality::eGrenadeLauncher:
        return "Гранатометчик";
    case Speciality::eReconnaissance:
        return "Разведчик";
    case Speciality::eSpecialForces:
        return "Спецназ";
    case Speciality::eSniper:
        return "Снайпер";
    case Speciality::eTanker:
        return "Танкист";
    case Speciality::eAntiAircraft:
        return "Зенитчик";
    case Speciality::eEngineer:
        return "Инженер";
    case Speciality::eMedic:
        return "Медик";
    default:
        return "Неизвестно";
    }
}

std::string Staff::ToString() const {
    std::string result;
    result.append(m_fullName).append(" [")
        .append("Звание: ").append(RankToString(m_rank))
        .append(", Специальность: ").append(SpecialityToString(m_speciality))
        .append("]");
    return result;
}